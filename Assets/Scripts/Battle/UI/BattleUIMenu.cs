using System;
using System.Linq;
using UnityEngine;

namespace Battle.UI {
  [RequireComponent(typeof(BattleUI))]
  public class BattleUIMenu : MonoBehaviour {
    private BattleUI ui;
    
    public int AbilityIndex { get; private set; }

    protected void Awake() {
      ui = GetComponent<BattleUI>();
    }

    protected void Update() {
      if (ui.Mode != BattleUIMode.Menu) {
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