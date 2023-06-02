using System.Collections.Generic;
using System.Linq;
using Lua;
using Shapes;
using UnityEngine;

namespace Battle.UI {
  [RequireComponent(typeof(StateManager))]
  public class CandidateCells : ImmediateModeShapeDrawer {
    [SerializeField] private BattleController battle;
    [SerializeField] private Transform origin;
    [SerializeField, Range(0f, 1f)] private float sideLength = 0.5f;
    [SerializeField, Range(0f, 1f)] private float borderSize = 0.1f;
    // [SerializeField, Range(0f, 1f)] private float unselectedAlpha = 0.25f;
    // [SerializeField, Range(0f, 1f)] private float selectedAlpha = 0.5f;
    
    private readonly List<Vector2Int> cells = new();
    private int cursor = -1;

    private AbilityScriptRunner abilityScriptRunner;

    public FocusState FocusState { get; set; }
    
    protected void Start() {
      abilityScriptRunner = battle.AbilityScriptRunner;
    }

    public override void DrawShapes(Camera cam) {
      if (FocusState == FocusState.Select) {
        return;
      }
      
      using (Draw.Command(cam)) {
        for (var i = 0; i < cells.Count; i++) {
          var cell = cells[i];
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
        
        var targetCell = cells[cursor];
        // Nullable:
        var targetActor = battle.Grid.GetActor(targetCell);
        var target = new CellData(targetActor, targetCell);
        
        Debug.LogFormat("TODO: Activating ability at {0}", target);
        
        abilityScriptRunner.ExecuteAnimate(battle.Game, source, target);
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

    public void HandleInput(StateManager stateManager) {
      TryExit(stateManager);
      
      if (cells.Count == 0) {
        return;
      }

      Debug.Assert(cursor >= 0);
      
      TrySubmit(stateManager);
      TryMoveCursor(stateManager);
    }

    public void Queue(IEnumerable<Vector2Int> candidates) {
      Reset();
      
      foreach (var candidate in candidates) {
        // Cull OOB
        if (!battle.Grid.IsValidCell(candidate)) {
          continue;
        }
        
        // TODO: Cull wall collisions
        // if (...) { continue; }
        
        cells.Add(candidate);
      }

      if (cells.Count > 0) {
        cursor = 0;
      }
    }

    public void Reset() {
      cells.RemoveAll(_ => true);
      cursor = -1;
    }

    private Vector2Int? ClosestCellTo(Vector2Int position) {
      if (cells.Count == 0) {
        return null;
      }
      
      return cells.Aggregate(
              cells.First(),
              (minCoord, coord) => { 
                // TODO: What to do in a tie?
                //  => Prioritize different cell
                //   -> Calculate AABB for cells
                //   -> position = mod(position, AABB)
                //   -> distance = (p - c).sqrMagnitude
                int distance = (position - coord).sqrMagnitude;
                int minDistance = (position - minCoord).sqrMagnitude; 
                
                if (distance < minDistance) { 
                  return coord;
                }
                
                return minCoord;
      });
    }

    private void MoveCursor(Vector2Int direction) {
      if (cells.Count == 0) {
        return;
      }
      
      Debug.Assert(cursor >= 0);
      
      var source = cells[cursor];
      var target = ClosestCellTo(source + direction);
      Debug.Assert(target != null);

      cursor = cells.IndexOf(target.Value);
    }
  }
}