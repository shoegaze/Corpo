using MoonSharp.Interpreter;
using UnityEngine;

namespace Lua.Proxy {
  public class CellDataProxy {
    private readonly CellData cd;

    public Actor.Actor Actor => cd.Actor;
    public Vector2Int Cell => cd.Cell;
    public bool HasActor => cd.HasActor;
    
    [MoonSharpHidden]
    public CellDataProxy(CellData cd) {
      this.cd = cd;
    }
  }
}