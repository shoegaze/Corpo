using Battle;
using UnityEngine;

namespace Lua {
  // TODO: Move up namespaces
  // TODO: Provide method to convert from Cell => Position
  public class CellData {
    public Actor.Actor Actor { get; }
    public Vector2Int Cell { get; }

    public bool HasActor => Actor != null;
    public Vector3 Position => new(Cell.x, Cell.y, 0f);
    
    public CellData(Actor.Actor actor, Vector2Int cell) {
      Actor = actor;
      Cell = cell;
    }

    public override string ToString() {
      return $"CellData({Actor}, {Cell})";
    }
  }
}