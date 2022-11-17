using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
  public enum GameMode {
    World,
    Battle
  }
  
  [SerializeField] private GameMode mode = GameMode.World;
  [SerializeField] private List<Actor> team;
  [SerializeField] private int money;
  [SerializeField] private uint year;
  [SerializeField, Range(0, 4)] private uint quarter;

  private BattleController battle;

  protected void Start() {
    // DEBUG
    var cache = GetComponent<ResourcesCache>();
    var ally = cache.GetActor("dimpp", Actor.ActorTeam.Ally);
    team.Add(ally);
    
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
      
      battle.SetUp(team, enemies);
    }
  }

  // private void EndBattle() {
  //   mode = GameMode.World;
  //   
  //   // TODO
  //   StartCoroutine(UnloadBattleScene());
  // }
}
