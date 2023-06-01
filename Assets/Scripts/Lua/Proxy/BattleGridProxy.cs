using MoonSharp.Interpreter;
using UnityEngine;
using Battle;

namespace Lua.Proxy {
  public class BattleGridProxy {
    private readonly BattleGrid grid;

    [MoonSharpHidden]
    public BattleGridProxy(BattleGrid grid) {
      this.grid = grid;
    }
    
    public Actor.Actor GetActor(Vector2Int position) {
      return grid.GetActor(position);
    }

    public Vector2Int? GetPosition(Actor.Actor actor) {
      return grid.GetPosition(actor);
    }

    public bool HasActor(Vector2Int position) {
      return grid.HasActor(position);
    }

    public bool CanMove(Vector2Int from, Vector2Int to) {
      return grid.CanMove(from, to);
    }
  }
}