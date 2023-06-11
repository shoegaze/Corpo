using Battle.View;
using Unity.Collections;
using UnityEngine;

namespace Battle.Model {
  public class StateManager {
    private MenuModel model;
    
    private FocusState focusState;
    private SelectState selectState;
    
    // TODO: Initialize this
    // private AbilitySelect abilitySelect;

    public FocusState FocusState => focusState;
    public SelectState SelectState => selectState;

    public StateManager(MenuModel model) {
      this.model = model;
      
      focusState = FocusState.Grid;
      selectState = SelectState.Free;

      // TODO: Initialize
      // abilitySelect = GetComponent<AbilitySelect>();
    }

    private bool CanTransitionTo(FocusState state) {
      return state switch {
              FocusState.Grid => focusState == FocusState.Menu,
              FocusState.Menu => focusState == FocusState.Grid,
              _               => false
      };
    }

    public void Transition(FocusState state) {
      if (!CanTransitionTo(state)) {
        Debug.LogWarningFormat("Cannot transition panel state from {0} to {1}!", focusState, state);
        return;
      }

      focusState = state;
      selectState = SelectState.Free;

      if (focusState == FocusState.Grid) {
        model.AbilitySelect.Reset();
      }
      else { // focusState == FocusState.MenuModel
        model.LoadAbility();
      }
    }

    private bool CanTransitionTo(SelectState state) {
      if (focusState != FocusState.Menu) {
        return false;
      }

      return state switch {
              SelectState.Free   => selectState != SelectState.Free,
              SelectState.Focus  => selectState == SelectState.Free,
              SelectState.Select => selectState == SelectState.Focus,
              _                 => false
      };
    }

    public void Transition(SelectState state) {
      if (!CanTransitionTo(state)) {
        Debug.LogWarningFormat("Cannot transition focus state from {0} to {1}!", selectState, state);
        return;
      }

      selectState = state;
      model.AbilitySelect.FocusState = selectState;
    }
  }
}