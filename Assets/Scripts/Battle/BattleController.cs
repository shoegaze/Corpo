using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle {
  public class BattleController : MonoBehaviour {
    // TODO: Make configurable from GameController
    [SerializeField] private uint width;
    [SerializeField] private uint height;

    [SerializeField] private List<Actor> order = new List<Actor>();
    [SerializeField] private int turn;

    private BattleGrid Grid { get; set; }
    private BattleScreen screen;
    private GameController game;
    private ResourcesCache cache;

    private Actor ActiveActor => order[turn];

    private bool AlliesAlive => order.Where(a => a.Alignment == Actor.ActorAlignment.Ally)
                                     .Any(a => a.IsAlive);

    private bool EnemiesAlive => order.Where(a => a.Alignment == Actor.ActorAlignment.Enemy)
                                      .Any(e => e.IsAlive);

    protected void Awake() {
      Grid = new BattleGrid(width, height);
      screen = GetComponent<BattleScreen>();
    }

    protected void Start() {
      var go = GameObject.FindWithTag("GameController");
      game = go.GetComponent<GameController>();
      cache = go.GetComponent<ResourcesCache>();
    }

    // DEBUG:
    public Actor GetRandomEnemy() {
      // Battlefield/Instances
      var instanceRoot = transform.Find("Instances");
      
      Debug.Assert(instanceRoot != null);
      
      var actor = cache.GetActor("oeur", Actor.ActorAlignment.Enemy);
      var instance = Instantiate(actor, instanceRoot);
      return instance;
    }

    private void IncrementTurn() {
      turn = (turn + 1) % order.Count;
    }

    private void SetUp(IEnumerable<Actor> allies, IEnumerable<Actor> enemies) {
      // TODO: Shuffle turnOrder?
      order = allies.Concat(enemies).ToList();
      turn = 0;
      
      // TODO: Read level from presets?
      Grid.GenerateRandomWalls(10, 0.125f);

      { // DEBUG: Place Actors
        Grid.RandomlyPlaceActors(order);
      }

      screen.BuildViews(Grid, cache);
    }

    public void StartBattle(IEnumerable<Actor> allies, IEnumerable<Actor> enemies) {
      SetUp(allies, enemies);

      StartCoroutine(DoBattle());
    }

    private void EndBattle() {
      // TODO
      if (AlliesAlive) {
        Debug.Log("YOU WIN!");
      }
      else {
        Debug.Log("YOU LOSE!");
      }

      game.EndBattle();
    }

    private IEnumerator DoBattle() {
      while (AlliesAlive && EnemiesAlive) {
        while (true) { // Do turn
          bool decided;

          if (ActiveActor.Alignment == Actor.ActorAlignment.Ally) {
            decided = DoPlayerTurn();
          }
          else { // Enemy CPU
            decided = DoComputerTurn();
          }
          
          if (decided) {
            break;
          }

          yield return null;
        }
        
        IncrementTurn();

        yield return null;
      }

      EndBattle();
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
      if (Grid.HasActor(to) /*&& Grid.AreConnected(from.Value, to)*/) {
        var target = Grid.GetActor(to);
        Debug.Assert(target != null);

        // Do attack if different alignments
        if (actor.Alignment != target.Alignment) {
          var context = new AttackContext(target, Grid, from.Value, to);
          actor.Attack(context);
          
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

      // Dead
      if (!actor.IsAlive) {
        return true;
      }
      
      var from = Grid.GetPosition(actor);
      Debug.Assert(from != null);

      { // DEBUG: Go to random neighbor
        var candidates = new[] {
                from.Value + new Vector2Int(-1,  0),
                from.Value + new Vector2Int(+1,  0),
                from.Value + new Vector2Int( 0, -1),
                from.Value + new Vector2Int( 0, +1)
        };

        // TODO: Implement attack
        candidates = candidates.Where(to => Grid.CanMove(from.Value, to))
                               .ToArray();

        // If we can't do anything, just idle
        if (candidates.Length > 0) {
          var i = (int)(candidates.Length * Random.value);
          var to = candidates[i];

          var moved = Grid.TryMoveActor(actor, to);
          Debug.Assert(moved);
        }
      }

      BattleScreen.UpdateActorView(actor, Grid);
      
      return true;
    }
  }
}
