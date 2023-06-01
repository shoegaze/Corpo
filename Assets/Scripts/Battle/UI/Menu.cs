using System.Linq;
using UnityEngine;
using Lua;

namespace Battle.UI {
  [RequireComponent(typeof(BattleUI))]
  [RequireComponent(typeof(CandidateCells))]
  public class Menu : MonoBehaviour {
    [SerializeField] private AbilityScriptRunner abilityScriptRunner;
    
    public int AbilityIndex { get; private set; }

    private GameController game;
    private BattleUI ui;
    private CandidateCells candidateCells;

    protected void Awake() {
      ui = GetComponent<BattleUI>();
      candidateCells = GetComponent<CandidateCells>();
    }

    protected void Start() {
      var go = GameObject.FindWithTag("GameController");
      game = go.GetComponent<GameController>();
    }

    private Ability GetCurrentAbility(Actor.Actor actor) {
      return actor.Abilities
                  .Skip(AbilityIndex)
                  .First();
    }

    // Select ability:
    //  1. Enter candidate cell selection mode
    //   * On select candidate:
    //    1. Execute script with target context
    //    2. Enter animation coroutine
    //    3. Update animation context
    //    4. Clean up animation coroutine
    //    5. Increment turn
    //   * On cancel:
    //    1. Go back to cycling mode
    private void SelectAbility(StateManager stateManager) {
      stateManager.Transition(FocusState.Focus);
    }

    private void CycleAbility(Actor.Actor actor, int dy) {
      int index = AbilityIndex;
      
      AbilityIndex -= dy;

      int max = actor.Abilities.Count() - 1;
      AbilityIndex = Mathf.Clamp(AbilityIndex, 0, max);

      if (index != AbilityIndex) {
        // TODO: Refactor into LoadAbility(game, actor)
        var ability = GetCurrentAbility(actor);
        abilityScriptRunner.Load(ability.Script);
        abilityScriptRunner.ExecuteStart(game);
        
        // Precondition: Script should be loaded
        var candidates = abilityScriptRunner.ExecuteGetCandidateCells(game);
        candidateCells.Queue(candidates);
      }
    }
    
    public void LoadAbility() {
      var actor = ui.ActiveActor;
      if (actor == null || actor.Alignment != ActorAlignment.Ally) {
        return;
      }
      
      // Load ability at current index on Mode.Menu
      // TODO: Avoid by keeping track of loaded ability
      // TODO: Refactor into LoadAbility(game, actor)
      var ability = GetCurrentAbility(actor);
      abilityScriptRunner.Load(ability.Script);
      abilityScriptRunner.ExecuteStart(game);

      // Precondition: Script should be loaded
      var candidates = abilityScriptRunner.ExecuteGetCandidateCells(game);
      candidateCells.Queue(candidates);
    }
    
    private void TryInputModeToggle(StateManager stateManager, Actor.Actor actor) {
      if (Input.GetButtonDown("Toggle")) {
        stateManager.Transition(PanelState.Grid);
      }
    }

    private void TryExit(StateManager stateManager) {
      if (!Input.GetButtonDown("Cancel")) {
        return;
      }

      stateManager.Transition(PanelState.Grid);
    }
    
    // 1. Handle input mode toggle
    // if mode is menu:
    //  2. Handle input ability select
    //  3. Handle input ability cycling
    public void HandleInput(StateManager stateManager) {
      TryExit(stateManager);
      
      var actor = ui.ActiveActor;
      if (actor == null || actor.Alignment != ActorAlignment.Ally) {
        return;
      }
      
      TryInputModeToggle(stateManager, actor);
      
      bool select = Input.GetButtonDown("Submit");
      if (select) {
        SelectAbility(stateManager);
        return;
      }
      
      bool v = Input.GetButtonDown("Vertical");
      if (v) {
        int dy = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        CycleAbility(actor, dy);
      }
    }
  }
}