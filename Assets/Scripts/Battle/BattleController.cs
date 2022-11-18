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
    [SerializeField] private bool waitingForPlayer;

    private BattleGrid Grid { get; set; }
    private BattleScreen screen;
    private GameController game;
    private ResourcesCache cache;

    protected void Awake() {
      Grid = new BattleGrid(width, height);
      screen = GetComponent<BattleScreen>();
    }

    protected void Start() {
      var go = GameObject.FindWithTag("GameController");
      game = go.GetComponent<GameController>();
      cache = go.GetComponent<ResourcesCache>();
      
      StartCoroutine(StartBattle());
    }

    protected void Update() {
      if (waitingForPlayer) {
        // TODO: Configure input to be hard-edged in settings
        var h = Input.GetButtonDown("Horizontal");
        var v = Input.GetButtonDown("Vertical");

        // Ignore idling
        if (!h && !v) {
          return;
        }

        // Prevent diagonal movement
        if (h && v) {
          return;
        }

        var actor = order[turn];
        var from = Grid.GetPosition(actor);

        if (from == null) {
          return;
        }

        var x = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        var y = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        var to = from.Value + new Vector2Int(x, y);
        
        // Try attacking
        if (Grid.HasActor(to) /*&& Grid.AreConnected(from.Value, to.Value)*/) {
          var target = Grid.GetActor(to);
          
          Debug.Assert(target != null);

          // Do attack if different alignments
          if (actor.Alignment != target.Alignment) {
            // TODO: Attack animation
            target.TakeHealth(1);

            if (!target.IsAlive) {
              // TODO: Actor.Die(Grid)
              var removed = Grid.TryRemoveActor(target);
              Debug.Assert(removed);
              
              target.View.SetActive(false);
              target.gameObject.SetActive(false);
            }

            waitingForPlayer = false;
            IncrementTurn();
          }

          return;
        }
        
        // Try moving
        if (Grid.TryMoveActor(actor, to)) {
          BattleScreen.UpdateActorView(actor, Grid);
          
          waitingForPlayer = false;
          IncrementTurn();
        }
      }
    }

    public void SetUp(IEnumerable<Actor> allies, IEnumerable<Actor> enemies) {
      // TODO: Shuffle turnOrder?
      order = allies.Concat(enemies).ToList();
      
      // TODO: Read level from presets?
      Grid.GenerateRandomWalls(10, 0.125f);

      { // DEBUG: Place Actors
        Grid.RandomlyPlaceActors(order);
      }

      screen.BuildViews(Grid, cache);
    }

    public Actor GetRandomEnemy() {
      // DEBUG:
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
    
    private IEnumerator StartBattle() {
      // Wait for SetUp to be called
      while (order.Count == 0) {
        yield return null;
      }
      
      var alliesAreAlive = true;
      var enemiesAreAlive = true;
      
      while (alliesAreAlive && enemiesAreAlive) {
        DoTurn();
      
        alliesAreAlive = order.Exists(a => a.IsAlive);
        enemiesAreAlive = order.Exists(a => a.IsAlive);

        yield return null;
      }
      
      // TODO:
      if (enemiesAreAlive) {
        // TODO: Win procedure
        Debug.Log("YOU WIN!");
      }
      else { // enemiesAreDead
        // TODO: Lose procedure
        Debug.Log("YOU LOSE!");
      }

      game.EndBattle();
    }
    
    private void DoTurn() {
      // TODO
      var actor = order[turn];

      if (actor.Alignment == Actor.ActorAlignment.Ally) {
        WaitForPlayerDecision();
      }
      else { // actor.Alignment == Actor.ActorAlignment.Enemy
        WaitForComputerDecision();
      }
    }

    private void WaitForPlayerDecision() {
      waitingForPlayer = true;
      
      // TODO
    }

    private void WaitForComputerDecision() {
      waitingForPlayer = false;

      var actor = order[turn];

      // Dead
      if (!actor.IsAlive) {
        IncrementTurn();
        return;
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
      IncrementTurn();
    }
  }
}
