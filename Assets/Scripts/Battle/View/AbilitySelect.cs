using System.Collections.Generic;
using System.Linq;
using Lua;
using Shapes;
using UnityEngine;
using Zenject;

namespace Battle.View {
  [RequireComponent(typeof(StateManager))]
  public class AbilitySelect : ImmediateModeShapeDrawer {
    [SerializeField] private Transform origin;
    [SerializeField, Range(0f, 1f)] private float sideLength = 0.5f;
    [SerializeField, Range(0f, 1f)] private float borderSize = 0.1f;
    // [SerializeField, Range(0f, 1f)] private float unselectedAlpha = 0.25f;
    // [SerializeField, Range(0f, 1f)] private float selectedAlpha = 0.5f;
    
    [Inject] private BattleController battle;
    
    private readonly List<Vector2Int> candidates = new();
    private int cursor = -1;

    public FocusState FocusState { get; set; }

    public override void DrawShapes(Camera cam) {
      if (FocusState == FocusState.Select) {
        return;
      }
      
      using (Draw.Command(cam)) {
        for (var i = 0; i < candidates.Count; i++) {
          var cell = candidates[i];
          var position = cell - 0.5f * sideLength * Vector2.one;
          var size = sideLength * Vector2.one;
          var rect = new Rect(position, size);
          
          Draw.SetMatrix(
                  origin.position,
                  Quaternion.identity,
                  origin.lossyScale);
          
          // DEBUG: Set alpha in inspector
          float alpha = FocusState == FocusState.Focus && i == cursor ? 0.5f : 0.25f;
          Draw.Color = new Color(1f, 1f, 1f, alpha);
          Draw.DashStyle = DashStyle.defaultDashStyleRing;
          
          Draw.RectangleBorder(Vector3.zero, rect, borderSize);
          Draw.ResetAllDrawStates();
        }
      }
    }

    private void TryExit(StateManager stateManager) {
      if (!Input.GetButtonDown("Cancel")) {
        return;
      }
      
      stateManager.Transition(PanelState.Grid);
    }

    private void TrySubmit(StateManager stateManager) {
      if (cursor < 0) {
        return;
      }

      if (!Input.GetButtonDown("Submit")) {
        return;
      }

      if (stateManager.FocusState == FocusState.Free) {
        stateManager.Transition(FocusState.Focus);
      }
      else if (stateManager.FocusState == FocusState.Focus) {
        stateManager.Transition(FocusState.Select);
        
        var sourceActor = battle.ActiveActor;
        var sourceCell = battle.Grid.GetPosition(sourceActor).Value;
        var source = new CellData(sourceActor, sourceCell);
        
        var targetCell = candidates[cursor];
        // Nullable:
        var targetActor = battle.Grid.GetActor(targetCell);
        var target = new CellData(targetActor, targetCell);
        
        // TODO: Move below to
        battle.TryDoAbility(source, target);
      }
    }

    public void HandleInput(StateManager stateManager) {
      TryExit(stateManager);
      
      if (candidates.Count == 0) {
        return;
      }

      Debug.Assert(cursor >= 0);
      
      TrySubmit(stateManager);
      TryMoveCursor(stateManager);
    }

    public void Queue(IEnumerable<Vector2Int> cells) {
      Reset();
      
      foreach (var candidate in cells) {
        // Cull OOB
        if (!battle.Grid.IsValidCell(candidate)) {
          continue;
        }
        
        // TODO: Cull wall collisions
        // if (...) { continue; }
        
        candidates.Add(candidate);
      }

      if (this.candidates.Count > 0) {
        cursor = 0;
      }
    }

    public void Reset() {
      candidates.RemoveAll(_ => true);
      cursor = -1;
    }

    private Vector2Int? ClosestCell(Vector2Int position) {
      if (candidates.Count == 0) {
        return null;
      }
      
      return candidates.Aggregate(
          candidates.First(),
          (minCoord, coord) => { 
            int distance = (position - coord).sqrMagnitude;
            int minDistance = (position - minCoord).sqrMagnitude; 
                
            if (distance < minDistance) { 
              return coord;
            }
                
            return minCoord;
          }); 
    }

    // TODO: What to do in a tie?
    //  => Prioritize different cell
    //   -> Calculate AABB for cells
    //   -> position = mod(position, AABB)
    //   -> distance = (p - c).sqrMagnitude
    private Vector2Int? ChooseCell(Vector2Int position, Vector2Int direction) {
      if (candidates.Count == 0) {
        return null;
      }

      var aabb = new Rect();
      foreach (var cell in candidates) {
        aabb.min = Vector2.Min(aabb.min, cell - 0.5f * Vector2.one);
        aabb.max = Vector2.Max(aabb.max, cell + 0.5f * Vector2.one);
      }

      { // 1. March towards direction
        // TODO: Normalize direction?
        var p = position + direction;
        while (aabb.Contains(p)) {
          int i = candidates.IndexOf(p);
          
          if (i >= 0) {
            return p;
          }

          p += direction;
        } 
      }
      
      { // 2. Go to closest cell     
        var p = position + direction;
        var closest = ClosestCell(p + direction);

        if (closest != null && closest.Value != position) {
          return closest;
        }
      }

      return null;
    }

    private void MoveCursor(Vector2Int direction) {
      if (candidates.Count == 0) {
        return;
      }
      
      Debug.Assert(cursor >= 0);
      
      var source = candidates[cursor];
      var target = ChooseCell(source, direction);
      
      if (target != null) {
        cursor = candidates.IndexOf(target.Value);
      }
    }

    private void TryMoveCursor(StateManager stateManager) {
      if (stateManager.FocusState != FocusState.Focus) {
        return;
      }
      
      var h = Input.GetButtonDown("Horizontal");
      var v = Input.GetButtonDown("Vertical");

      if (!h && !v) {
        return;
      }
     
      if (h && v) {
        return;
      }
     
      var direction = new Vector2Int(
              Mathf.RoundToInt(Input.GetAxis("Horizontal")),
              Mathf.RoundToInt(Input.GetAxis("Vertical")));
     
      MoveCursor(direction);
    }
  }
}