using System;

namespace Battle.View {
  public enum FocusState {
    Grid,
    Menu
  }

  public static class ModeExtensions {
    public static FocusState Cycle(this FocusState focusState) {
      return focusState switch {
              FocusState.Grid => FocusState.Menu,
              FocusState.Menu => FocusState.Grid,
              _         => throw new ArgumentOutOfRangeException()
      };
    }
  }
}