using UnityEngine;

namespace Lua {
  public class CellData {
    public Actor.Actor Actor { get; }
    public Vector2Int Cell { get; }

    public CellData(Actor.Actor actor, Vector2Int cell) {
      Actor = actor;
      Cell = cell;
    }
  }
}