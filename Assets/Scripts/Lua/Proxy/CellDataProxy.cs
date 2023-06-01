using MoonSharp.Interpreter;
using UnityEngine;

namespace Lua.Proxy {
  public class CellDataProxy {
    private readonly CellData cd;

    public ActorProxy Actor => new ActorProxy(cd.Actor);
    public Vector2Int Cell => cd.Cell;
    
    [MoonSharpHidden]
    public CellDataProxy(CellData cd) {
      this.cd = cd;
    }
  }
}