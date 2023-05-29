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
      
      bool select = Input.GetButtonDown("Submit");
      if (select) {
        // TODO: Select ability
        Debug.Log("Selecting ability");
        
        return;
      }
      
      bool v = Input.GetButtonDown("Vertical");
      if (v) {
        AbilityIndex -= Mathf.RoundToInt(Input.GetAxis("Vertical"));

        var max = 0;
        if (ui.ActiveActor) {
          max = ui.ActiveActor.Abilities.Count() - 1;
        } 
        
        AbilityIndex = Mathf.Clamp(AbilityIndex, 0, max);
      }
    }
  }
}