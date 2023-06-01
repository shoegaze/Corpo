using System;

namespace Battle.UI {
  public enum PanelState {
    Grid,
    Menu
  }

  public static class ModeExtensions {
    public static PanelState Cycle(this PanelState panelState) {
      return panelState switch {
              PanelState.Grid => PanelState.Menu,
              PanelState.Menu => PanelState.Grid,
              _         => throw new ArgumentOutOfRangeException()
      };
    }
  }
}