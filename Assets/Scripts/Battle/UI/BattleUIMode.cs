using System;

namespace Battle.UI {
  public enum BattleUIMode {
    Grid,
    Menu
  }

  public static class BattleUIModeExtensions {
    public static BattleUIMode Next(this BattleUIMode mode) {
      switch (mode) {
        case BattleUIMode.Grid:
          return BattleUIMode.Menu;
        
        case BattleUIMode.Menu:
          return BattleUIMode.Grid;
        
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}