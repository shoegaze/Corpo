using System.Linq;
using UnityEngine;

namespace Battle.UI {
  [RequireComponent(typeof(BattleUI))]
  public class Menu : MonoBehaviour {
    private BattleUI ui;
    
    public Mode Mode { get; private set; }
    public int AbilityIndex { get; private set; }

    protected void Awake() {
      ui = GetComponent<BattleUI>();
    }

    // 1. Handle input mode toggle
    // if mode is menu:
    //  2. Handle input ability select
    //  3. Handle input ability cycling
    protected void Update() {
      if (Input.GetButtonDown("Toggle")) {
        Mode = Mode.Cycle();
      }

      if (ui.Mode != Mode.Menu) {
        return;
      }

      var actor = ui.ActiveActor;
      if (actor == null) {
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

    private void SelectAbility(Actor.Actor actor) {
      var ability = actor.Abilities
                                 .Skip(AbilityIndex)
                                 .First();
              
      // TODO: Select ability
      Debug.Log($"Selecting ability: {ability.Name}");

      // TODO: Cache on Start
      // var go = GameObject.FindWithTag("BattleController");
      // var abilityScriptRunner = go.GetComponent<AbilityScriptRunner>();
      // 
      // abilityScriptRunner.Load(ability);
      // abilityScriptRunner.Execute_(ability, ...);
    }

    private void CycleAbility(Actor.Actor actor, int dy) {
      AbilityIndex -= dy;

      var max = actor.Abilities.Count() - 1;
      AbilityIndex = Mathf.Clamp(AbilityIndex, 0, max);
    }
  }
}