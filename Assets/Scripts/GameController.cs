using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Battle;

[RequireComponent(typeof(ResourcesCache))]
public class GameController : MonoBehaviour {
  [SerializeField] private GameMode gameMode = GameMode.World;
  [SerializeField] private uint year;
  [SerializeField, Range(0, 3)] private uint quarter;

  private Team allies = new(ActorAlignment.Ally, 1000);
  public Team Allies => allies; 
  
  private ResourcesCache resources;

  // ReSharper disable once EventNeverSubscribedTo.Global
  public event Action<GameMode> OnModeChanged;

  protected void Awake() {
    // TODO: Get this via DI?
    resources = GetComponent<ResourcesCache>();
  }

  protected void Start() {
    // DEBUG
    var dimpp = resources.GetActor("dimpp");
    allies.Add(dimpp);
    
    // DEBUG
    StartCoroutine(LoadBattleScene());
  }

  // ReSharper disable Unity.PerformanceAnalysis
  private IEnumerator LoadBattleScene() {
    var load = SceneManager.LoadSceneAsync("Scenes/Battle", LoadSceneMode.Additive);
    
    while (!load.isDone) {
      yield return null;
    }

    var battleScene = SceneManager.GetSceneByName("Battle");
    SceneManager.SetActiveScene(battleScene);
    
    StartBattle();
  }

  private IEnumerator UnloadBattleScene() {
    var load = SceneManager.UnloadSceneAsync("Battle");

    while (!load.isDone) {
      yield return null;
    }

    // TODO: Set WorldScene as active scene 
    var baseScene = SceneManager.GetSceneByName("Base");
    SceneManager.SetActiveScene(baseScene);
  }
  
  private void StartBattle() {
    gameMode = GameMode.Battle;
    OnModeChanged?.Invoke(gameMode);

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
    
    Debug.Log("Unloading battle scene");
    
    // TODO
    StartCoroutine(UnloadBattleScene());
  }
}
