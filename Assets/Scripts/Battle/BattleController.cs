using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle.UI;
using UnityEngine;

namespace Battle {
  [RequireComponent(typeof(BattleScreen))]
  public class BattleController : MonoBehaviour {
    // TODO: Make configurable from GameController
    [SerializeField] private uint width;
    [SerializeField] private uint height;

    [SerializeField] private List<Actor.Actor> order = new List<Actor.Actor>();
    [SerializeField, Min(0)] private int turn;

    private BattleUI UI { get; set; }
    private BattleGrid Grid { get; set; }
    private BattleScreen screen;
    private GameController game;
    private ResourcesCache cache;

    public int Turn => turn;
    // order.Count == 0 until SetUp is called
    public Actor.Actor ActiveActor => order.Count > 0 ? order[turn % order.Count] : null;
    public bool DoAlliesWin => AreAlliesAlive && !AreEnemiesAlive;
    public bool DoEnemiesWin => !AreAlliesAlive;
    
    private bool AreAlliesAlive => order.Where(a => a.Alignment == ActorAlignment.Ally)
                                        .Any(a => a.IsAlive);

    private bool AreEnemiesAlive => order.Where(a => a.Alignment == ActorAlignment.Enemy)
                                         .Any(e => e.IsAlive);

    protected void Awake() {
      Grid = new BattleGrid(width, height);
      screen = GetComponent<BattleScreen>();
      UI = GetComponent<BattleUI>();
    }

    protected void Start() {
      var go = GameObject.FindWithTag("GameController");
      Debug.Assert(go != null);
      
      game = go.GetComponent<GameController>();
      cache = go.GetComponent<ResourcesCache>();
    }

    // DEBUG:
    public Actor.Actor GetRandomEnemy() {
      // Battlefield/Instances
      var instanceRoot = transform.Find("Instances");
      Debug.Assert(instanceRoot != null);

      var actorIDs = new[] {"oeur", "floton", "kabey"};
      var actorID = actorIDs[(int)(actorIDs.Length * Random.value)];
      
      var actor = cache.GetActor(actorID, ActorAlignment.Enemy);
      return Instantiate(actor, instanceRoot);
    }

    private void IncrementTurn() {
      turn++;
    }

    private void SetUp(IEnumerable<Actor.Actor> allies, IEnumerable<Actor.Actor> enemies) {
      // TODO: Shuffle order?
      order = allies.Concat(enemies).ToList();
      turn = 0;
      
      // TODO: Read level from presets?
      Grid.GenerateRandomWalls(10, 0.125f);
      
      { // DEBUG: Place Actors
        Grid.RandomlyPlaceActors(order);
      }
      
      screen.BuildViews(Grid, cache);
    }

    public void StartBattle(IEnumerable<Actor.Actor> allies, IEnumerable<Actor.Actor> enemies) {
      SetUp(allies, enemies);
      StartCoroutine(DoBattle());
    }

    private IEnumerator DoBattle() {
      while (!DoAlliesWin && !DoEnemiesWin) {
        while (true) { // Do turn
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
        
        { // Wait for actor animations to end
          var anims = Grid.GridActors
                          .Select(v => v.actor)
                          .Where(a => a.IsAlive)
                          .Select(a => a.View.GetComponent<ActorAnimation>());
          
          while (anims.Any(a => a.IsPlaying)) {
            yield return null;
          }
        }
        
        IncrementTurn();

        yield return null;
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
    
    private bool DoPlayerTurn() {
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
        BattleScreen.UpdateActorView(actor, Grid);
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

      BattleScreen.UpdateActorView(actor, Grid);
      
      return true;
    }
  }
}
