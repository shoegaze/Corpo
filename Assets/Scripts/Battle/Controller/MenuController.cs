using Actor;
using Battle.Model;
using Battle.View;
using UnityEngine;
using Zenject;

namespace Battle.Controller {
	public class MenuController : MonoBehaviour, IController<IMenuModel> {
		[Inject] private BattleManager battle;
	
		// protected void Update() {
		//   if (focusState == FocusState.Menu) {
		//     if (selectState == SelectState.Free) {
		//       model.HandleInput(this);
		//     }
		//     else {
		//       abilitySelect.HandleInput(this);
		//     }
		//   }
		//   else { // focusState == FocusState.Grid
		//     if (Input.GetButtonDown("Toggle")) {
		//       Transition(FocusState.Menu);
		//     }
		//   }
		// }
	
    // 1. Handle input mode toggle
    // if mode is menu:
    //  2. Handle input ability select
    //  3. Handle input ability cycling
		public void HandleInput(IMenuModel model) {
			var stateManager = model.StateManager;
			
			TryExit(stateManager);
      
			var actor = battle.ActiveActor;
			if (actor == null || actor.Alignment != ActorAlignment.Ally) {
				return;
			}
      
			TryInputModeToggle(stateManager);
      
			bool select = Input.GetButtonDown("Submit");
			if (select) {
				model.SelectAbility();
				return;
			}
      
			bool v = Input.GetButtonDown("Vertical");
			if (v) {
				int dy = Mathf.RoundToInt(Input.GetAxis("Vertical"));
				model.CycleAbility(dy);
			}
		}
		
		private void TryInputModeToggle(StateManager stateManager) {
			if (Input.GetButtonDown("Toggle")) {
				stateManager.Transition(FocusState.Grid);
			}
		}

		private void TryExit(StateManager stateManager) {
			if (!Input.GetButtonDown("Cancel")) {
				return;
			}

			stateManager.Transition(FocusState.Grid);
		}

	}
}
