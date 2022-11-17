using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle {
  public class BattleController : MonoBehaviour {
    // TODO: Make configurable from GameController
    [SerializeField] private uint width;
    [SerializeField] private uint height;

    private ResourcesCache cache;

    public BattleGrid Grid { get; private set; }
    private BattleScreen screen;

    protected void Awake() {
      Grid = new BattleGrid(width, height);
      screen = GetComponent<BattleScreen>();
    }

    protected void Start() {
      var game = GameObject.FindWithTag("GameController");
      cache = game.GetComponent<ResourcesCache>();
    }

    // TODO
    public void SetUp(IEnumerable<Actor> allies, IEnumerable<Actor> enemies) {
      // TODO: Read level from presets?
      Grid.GenerateRandomWalls(10, 0.125f);

      {
        // Place Actors
        var actors = allies.Concat(enemies);
        Grid.RandomlyPlaceActors(actors);
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
