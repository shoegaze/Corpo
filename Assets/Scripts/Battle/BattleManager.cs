using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Battle.View;
using Lua;
using UnityEngine;
using Zenject;

namespace Battle {
  [RequireComponent(typeof(AbilityScriptRunner))]
  public class BattleManager : MonoBehaviour {
    // TODO: Make configurable from GameManager
    [SerializeField] private uint width;
    [SerializeField] private uint height;

    [SerializeField] private List<Actor.Actor> order = new();
    [SerializeField, Min(0)] private int turn;

    [Inject] private ResourcesCache resources;
    [Inject] private GameManager game;
    // TODO: Get rid of this injection:
    [Inject] private AbilityScriptRunner abilityScriptRunner;
    [Inject] private BattleView view;
    // TODO: Get rid of this injection:
    [Inject] private BattleGridView gridView;
    
    private int turnLock;
    private readonly List<Team> teams = new();
    
    public Team Allies => teams.First(t => t.Alignment == ActorAlignment.Ally);
    public Team Enemies => teams.First(t => t.Alignment == ActorAlignment.Enemy);

    public BattleGrid Grid { get; private set; }
    
    public int Turn => turn;
    // GOTCHA: order.Count == 0 until SetUp is called
    public Actor.Actor ActiveActor => order.Count > 0 ? order[turn % order.Count] : null;
    public bool DoAlliesWin => AreAlliesAlive && !AreEnemiesAlive;
    public bool DoEnemiesWin => !AreAlliesAlive;
    
    private bool AreAlliesAlive => order.Where(a => a.Alignment == ActorAlignment.Ally)
                                        .Any(a => a.IsAlive);

    private bool AreEnemiesAlive => order.Where(a => a.Alignment == ActorAlignment.Enemy)
                                         .Any(e => e.IsAlive);

    protected void Awake() {
      Grid = new BattleGrid(width, height);
    }

    // DEBUG:
    public Actor.Actor GetRandomEnemy() {
      // Battlefield/Instances
      var instanceRoot = transform.Find("Instances");
      Debug.Assert(instanceRoot != null);

      var actorIDs = new[] {"oeur", "floton", "kabey"};
      var actorID = actorIDs[(int)(actorIDs.Length * Random.value)];
      
      var actor = resources.GetActor(actorID);
      return Instantiate(actor, instanceRoot);
    }

    public void LockTurn() {
      turnLock++;
    }
    
    public void UnlockTurn() {
      turnLock--;
      turnLock = Mathf.Max(turnLock, 0);
    }

    public bool IncrementTurn() {
      if (turnLock > 0) {
        return false;
      }
      
      // TODO:
      if (view.StateManager.FocusState != FocusState.Grid) { 
        view.StateManager.Transition(FocusState.Grid);
      }
      
      if (view.StateManager.SelectState != SelectState.Free) {
        view.StateManager.Transition(SelectState.Free);
      }

      turn++;

      return true;
    }

    private void SetUp(Team allies, Team enemies) {
      teams.Add(allies);
      teams.Add(enemies);
      
      // TODO: Shuffle order?
      order = allies.Actors.Concat(enemies.Actors).ToList();
      turn = 0;
      
      // TODO: Read level from presets?
      Grid.GenerateRandomWalls(10, 0.125f);
      
      { // DEBUG: Place Actors
        Grid.RandomlyPlaceActors(order);
      }
      
      gridView.BuildViews(Grid, resources);
    }

    public void StartBattle(Team allies, Team enemies) {
      SetUp(allies, enemies);
      StartCoroutine(DoBattle());
    }

    private IEnumerator DoBattle() {
      while (!DoAlliesWin && !DoEnemiesWin) {
        while (true) { // Do turn
          if (turnLock > 0) {
            yield return null;
            continue;
          }
          
          bool decided;

          if (ActiveActor.Alignment == ActorAlignment.Ally) {
            decided = DoPlayerTurn();
          }
          else { // Enemy CPU
            // DEBUG:
            yield return new WaitForSeconds(0.5f);
            
            decided = DoComputerTurn();
          }
          
          if (decided) {
            break;
          }

          yield return null;
        }

        while (!IncrementTurn()) {
          yield return null;
        }
      }
      
      // Wait for t seconds before input
      yield return new WaitForSeconds(0.5f);
      
      // Wait for player input before unloading scene
      while (!Input.GetButtonDown("Submit")) {
        yield return null;
      }

      EndBattle();
    }
   
    private void EndBattle() {
      game.EndBattle();
    }

    public void TryDoAbility(CellData source, CellData target) {
      var team = source.Actor.Team;
      
      // HACK: Strip .lua extension
      string name = abilityScriptRunner.ScriptName;
      name = name[..^4];
      
      var data = resources.GetAbilityData(name);
      Debug.Assert(data != null);
      
      var cost = data.Cost;
      
      if (cost > team.Money) {
        return;
      }

      team.TakeMoney(cost);
      
      Debug.LogFormat(
              "Executing ability [{0}]: source => {1}, target => {2}",
              abilityScriptRunner.ScriptName,
              source,
              target);

      abilityScriptRunner.ExecuteAnimate(game, source, target);
    }
    
    // @return bool decided 
    private bool DoPlayerTurn() {
      if (view.FocusState == FocusState.Menu) {
        return false;
      }
      
      var h = Input.GetButtonDown("Horizontal");
      var v = Input.GetButtonDown("Vertical");

      // Ignore idling
      if (!h && !v) {
        return false;
      }

      // Prevent diagonal movement
      if (h && v) {
        return false;
      }

      var actor = ActiveActor;
      var from = Grid.GetPosition(actor);

      if (from == null) {
        return false;
      }

      var x = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
      var y = Mathf.RoundToInt(Input.GetAxis("Vertical"));
      var to = from.Value + new Vector2Int(x, y);
      
      // Try attacking
      // TODO: Make sure the target is not behind a wall
      if (Grid.HasActor(to) && Grid.AreConnected(from.Value, to)) {
        var target = Grid.GetActor(to);
        Debug.Assert(target != null);

        // Do attack if different alignments
        if (actor.Alignment != target.Alignment) {
          var ctx = new AttackContext(
                  actor, 
                  target, 
                  Grid, 
                  from.Value, 
                  to
          );
          
          actor.Attack(ctx);
          
          return true;
        }
      }
      
      // Try moving
      if (Grid.TryMoveActor(actor, to)) {
        BattleGridView.UpdateActorView(actor, Grid);
        return true;
      }

      return false;
    }

    private bool DoComputerTurn() {
      var actor = ActiveActor;

      // Ignore dead actors
      if (!actor.IsAlive) {
        return true;
      }
      
      var from = Grid.GetPosition(actor);
      Debug.Assert(from != null);
      
      var candidates = new[] {
              from.Value + new Vector2Int(-1,  0),
              from.Value + new Vector2Int(+1,  0),
              from.Value + new Vector2Int( 0, -1),
              from.Value + new Vector2Int( 0, +1)
      };

      var neighbors = candidates.Select(to => Grid.GetActor(to))
                                .Where(a => a != null);

      var allies = neighbors.Where(a => a.Alignment != actor.Alignment);
      
      if (allies.Any()) { // DEBUG: Attack if possible
        var target = allies.First();
        var to = Grid.GetPosition(target);
        Debug.Assert(to != null);
        
        var ctx = new AttackContext(actor, target, Grid, from.Value, to.Value);
        actor.Attack(ctx);
      }
      else { // DEBUG: Random walk
        var squares = candidates.Where(to => Grid.CanMove(from.Value, to))
                                .ToArray();
        
        // If we can't do anything, just idle
        if (squares.Length > 0) {
          var i = (int)(squares.Length * Random.value);
          var to = squares[i];

          var moved = Grid.TryMoveActor(actor, to);
          Debug.Assert(moved);
        }
      }

      BattleGridView.UpdateActorView(actor, Grid);
      
      return true;
    }
  }
}
