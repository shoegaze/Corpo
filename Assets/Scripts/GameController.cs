using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Lua;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ResourcesCache), typeof(Team))]
public class GameController : MonoBehaviour {
  [SerializeField] private GameMode mode = GameMode.World;
  // TODO: Move to Team definition?
  [SerializeField] private int money;
  [SerializeField] private uint year;
  [SerializeField, Range(0, 3)] private uint quarter;

  private Team team;
  private readonly LuaRunner luaRunner = new LuaRunner();

  public GameMode Mode => mode;
  public BattleController Battle { get; private set; }

  public event Action<GameMode> OnModeChanged;

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
    var load = SceneManager.LoadSceneAsync("Scenes/Battle", LoadSceneMode.Additive);
    
    while (!load.isDone) {
      yield return null;
    }

    var battleScene = SceneManager.GetSceneByName("Battle");
    SceneManager.SetActiveScene(battleScene);
    
    var battleRoot = battleScene.GetRootGameObjects().First();
    Battle = battleRoot.GetComponent<BattleController>();
    
    StartBattle();
  }

  private IEnumerator UnloadBattleScene() {
    Battle = null;
    
    var load = SceneManager.UnloadSceneAsync("Battle");

    while (!load.isDone) {
      yield return null;
    }

    // TODO: Set WorldScene as active scene 
    var baseScene = SceneManager.GetSceneByName("Base");
    SceneManager.SetActiveScene(baseScene);
  }
  
  private void StartBattle() {
    mode = GameMode.Battle;
    OnModeChanged?.Invoke(mode);
    
    var enemies = new List<Actor.Actor>();
    { // Generate random battle
      const uint n = 1;
      for (var i = 0; i < n; i++) {
        var enemy = Battle.GetRandomEnemy();
        enemies.Add(enemy);  
      }
    }
    
    Battle.StartBattle(team.Actors, enemies);
  }

  public void EndBattle() {
    mode = GameMode.World;
    OnModeChanged?.Invoke(mode);
    
    Debug.Log("Unloading battle scene");
    
    // TODO
    StartCoroutine(UnloadBattleScene());
  }
}
