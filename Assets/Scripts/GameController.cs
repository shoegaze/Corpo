using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ResourcesCache), typeof(Team))]
public class GameController : MonoBehaviour {
  private enum GameMode {
    World,
    Battle
  }
  
  [SerializeField] private GameMode mode = GameMode.World;
  // TODO: Move to Team definition?
  [SerializeField] private int money;
  [SerializeField] private uint year;
  [SerializeField, Range(0, 3)] private uint quarter;

  private Team team;
  private BattleController battle;

  protected void Awake() {
    team = GetComponent<Team>();
  }

  protected void Start() {
    // DEBUG
    team.Add("dimpp");
    
    // DEBUG
    StartCoroutine(LoadBattleScene());
  }

  // ReSharper disable Unity.PerformanceAnalysis
  private IEnumerator LoadBattleScene() {
    var load = SceneManager.LoadSceneAsync("Scenes/BattleScene", LoadSceneMode.Additive);
    
    while (!load.isDone) {
      yield return null;
    }

    var battleScene = SceneManager.GetSceneByName("BattleScene");
    SceneManager.SetActiveScene(battleScene);
    
    var battleRoot = battleScene.GetRootGameObjects().First();
    battle = battleRoot.GetComponent<BattleController>();
    
    StartBattle();
  }

  private IEnumerator UnloadBattleScene() {
    var load = SceneManager.UnloadSceneAsync("BattleScene");

    while (!load.isDone) {
      yield return null;
    }

    var worldScene = SceneManager.GetSceneByName("WorldScene");
    SceneManager.SetActiveScene(worldScene);

    battle = null;
  }
  
  private void StartBattle() {
    mode = GameMode.Battle;
    
    { // Generate random battle
      var enemies = new List<Actor>();
      
      uint n = 4;
      for (var i = 0; i < n; i++) {
        var enemy = battle.RandomEnemy();
        enemies.Add(enemy);  
      }
      
      battle.SetUp(team.Actors, enemies);
    }
  }

  public void EndBattle() {
    mode = GameMode.World;
    
    // TODO
    StartCoroutine(UnloadBattleScene());
  }
}
