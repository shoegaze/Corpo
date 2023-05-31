using System.Linq;
using UnityEngine;
using Lua;

namespace Battle.UI {
  [RequireComponent(typeof(BattleUI))]
  [RequireComponent(typeof(CandidateCells))]
  public class Menu : MonoBehaviour {
    [SerializeField] private AbilityScriptRunner abilityScriptRunner;
    
    public Mode Mode { get; private set; }
    public int AbilityIndex { get; private set; }

    private GameController game;
    private BattleUI ui;
    // private CandidateCells candidateCells;
    
    protected void Awake() {
      ui = GetComponent<BattleUI>();
      // candidateCells = GetComponent<CandidateCells>();
    }

    protected void Start() {
      var go = GameObject.FindWithTag("GameController");
      game = go.GetComponent<GameController>();
    }

    // 1. Handle input mode toggle
    // if mode is menu:
    //  2. Handle input ability select
    //  3. Handle input ability cycling
    protected void Update() {
      var actor = ui.ActiveActor;
      if (actor == null || actor.Alignment != ActorAlignment.Ally) {
        return;
      }
      
      if (Input.GetButtonDown("Toggle")) {
        Mode = Mode.Cycle();
        
        // Load ability at current index on Mode.Menu
        //  TODO: Avoid by keeping track of loaded ability
        if (Mode == Mode.Menu) { 
          // TODO: Refactor into LoadAbility(game, actor)
          var ability = GetCurrentAbility(actor);
          abilityScriptRunner.Load(ability.Script);
          abilityScriptRunner.ExecuteStart(game);
        }
      }

      if (ui.Mode != Mode.Menu) {
        return;
      }
      
      bool select = Input.GetButtonDown("Submit");
      if (select) {
        SelectAbility(actor);
        return;
      }
      
      bool v = Input.GetButtonDown("Vertical");
      if (v) {
        int dy = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        CycleAbility(actor, dy);
      }
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
    private void SelectAbility(Actor.Actor actor) {
      var ability = GetCurrentAbility(actor);
              
      // TODO: Select ability
      Debug.Log($"Selecting ability: {ability.Name}");
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
      }
      
      // Precondition: Script should be loaded
      // var candidates = abilityScriptRunner.ExecuteGetCandidateCells(game);
      // candidateCells.Queue(candidates);
    }
  }
}