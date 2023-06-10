using Unity.Collections;
using UnityEngine;

namespace Battle.View {
  [RequireComponent(typeof(Menu), 
                    typeof(AbilitySelect))]
  public class StateManager : MonoBehaviour {
    [SerializeField, ReadOnly] private PanelState panelState;
    [SerializeField, ReadOnly] private FocusState focusState;

    private Menu menu;
    private AbilitySelect abilitySelect;

    public PanelState PanelState => panelState;
    public FocusState FocusState => focusState;

    protected void Awake() {
      panelState = PanelState.Grid;
      focusState = FocusState.Free;

      menu = GetComponent<Menu>();
      abilitySelect = GetComponent<AbilitySelect>();
    }

    protected void Update() {
      if (panelState == PanelState.Menu) {
        if (focusState == FocusState.Free) {
          menu.HandleInput(this);
        }
        else {
          abilitySelect.HandleInput(this);
        }
      }
      else { // panelState == PanelState.Grid
        if (Input.GetButtonDown("Toggle")) {
          Transition(PanelState.Menu);
        }
      }
    }

    private bool CanTransitionTo(PanelState state) {
      return state switch {
              PanelState.Grid => panelState == PanelState.Menu,
              PanelState.Menu => panelState == PanelState.Grid,
              _               => false
      };
    }

    public void Transition(PanelState state) {
      if (!CanTransitionTo(state)) {
        Debug.LogWarningFormat("Cannot transition panel state from {0} to {1}!", panelState, state);
        return;
      }

      panelState = state;
      focusState = FocusState.Free;

      if (panelState == PanelState.Grid) {
        abilitySelect.Reset();
      }
      else { // panelState == PanelState.Menu
        menu.LoadAbility();
      }
    }

    private bool CanTransitionTo(FocusState state) {
      if (panelState != PanelState.Menu) {
        return false;
      }

      return state switch {
              FocusState.Free   => focusState != FocusState.Free,
              FocusState.Focus  => focusState == FocusState.Free,
              FocusState.Select => focusState == FocusState.Focus,
              _                 => false
      };
    }

    public void Transition(FocusState state) {
      if (!CanTransitionTo(state)) {
        Debug.LogWarningFormat("Cannot transition focus state from {0} to {1}!", focusState, state);
        return;
      }

      focusState = state;
      abilitySelect.FocusState = focusState;
    }
  }
}