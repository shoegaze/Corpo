using System;

namespace Battle.UI {
  public enum Mode {
    Grid,
    Menu
  }

  public static class ModeExtensions {
    public static Mode Cycle(this Mode mode) {
      return mode switch {
              Mode.Grid => Mode.Menu,
              Mode.Menu => Mode.Grid,
              _         => throw new ArgumentOutOfRangeException()
      };
    }
  }
}