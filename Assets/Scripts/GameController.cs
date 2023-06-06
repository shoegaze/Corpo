using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Battle;
using Zenject;

public class GameController : MonoBehaviour {
  [SerializeField] private GameMode gameMode = GameMode.World;
  [SerializeField] private uint year;
  [SerializeField, Range(0, 3)] private uint quarter;

  [Inject] private ResourcesCache resources;

  // DEBUG:
  public Team Allies { get; } = new(ActorAlignment.Ally, 1000);

  // ReSharper disable once EventNeverSubscribedTo.Global
  public event Action<GameMode> OnModeChanged;

  protected void Start() {
    // DEBUG:
    var dimpp = resources.GetActor("dimpp");
    Allies.Add(dimpp);
    
    // DEBUG: Move this to World scene
    StartCoroutine(LoadBattleScene());
  }

  // ReSharper disable Unity.PerformanceAnalysis
  private IEnumerator LoadBattleScene() {
    Debug.Log("Loading Battle scene ...");
  
    var load = SceneManager.LoadSceneAsync("Scenes/Battle", LoadSceneMode.Additive);
    
    while (!load.isDone) {
      yield return null;
    }
    
    Debug.Log("Done loading Battle scene!");

    var battleScene = SceneManager.GetSceneByName("Battle");
    SceneManager.SetActiveScene(battleScene);
    
    StartBattle();
  }

  private IEnumerator UnloadBattleScene() {
    Debug.Log("Unloading Battle scene ...");
    
    var load = SceneManager.UnloadSceneAsync("Battle");

    while (!load.isDone) {
      yield return null;
    }
    
    Debug.Log("Done unloading Battle scene!");

    // TODO: Set WorldScene as active scene 
    var baseScene = SceneManager.GetSceneByName("Base");
    SceneManager.SetActiveScene(baseScene);
  }
  
  private void StartBattle() {
    gameMode = GameMode.Battle;
    OnModeChanged?.Invoke(gameMode);

    // HACK: GameController should be decoupled from BattleController
    var go = GameObject.FindWithTag("BattleController");
    var battle = go.GetComponent<BattleController>();
    
    var enemies = new Team(ActorAlignment.Enemy, 1000);
    { // Generate random battle
      const uint n = 1;
      for (var i = 0; i < n; i++) {
        var enemy = battle.GetRandomEnemy();
        enemies.Add(enemy);
      }
    }
    
    battle.StartBattle(Allies, enemies);
  }

  public void EndBattle() {
    gameMode = GameMode.World;
    OnModeChanged?.Invoke(gameMode);
    
    StartCoroutine(UnloadBattleScene());
  }
}
