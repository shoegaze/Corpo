using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle {
  public class BattleController : MonoBehaviour {
    // TODO: Make configurable from GameController
    [SerializeField] private uint width;
    [SerializeField] private uint height;

    private List<Actor> order = new List<Actor>();
    private int turn;
    private bool waitingForPlayer;

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
        var h = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        var v = Mathf.RoundToInt(Input.GetAxis("Vertical"));

        // Ignore idling
        if (h == 0 && v == 0) {
          return;
        }

        // Prevent diagonal movement
        if (h != 0 && v != 0) {
          return;
        }

        var actor = order[turn];
        var dp = new Vector2Int(h, v);
        var moved = Grid.TryMoveActor(actor, dp);

        if (moved) {
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

    public Actor RandomEnemy() {
      // DEBUG:
      return cache.GetActor("oeur", Actor.ActorTeam.Enemy);
    }

    // private void DoTurn() {
    //   // TODO
    // }

    // private void WaitForPlayerDecision() {
    //   // TODO
    // }
  }
}
