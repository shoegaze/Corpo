using MoonSharp.Interpreter;
using UnityEngine;

namespace Lua.Proxy {
  public class CellDataProxy {
    static CellDataProxy() {
      UserData.RegisterProxyType<CellDataProxy, CellData>(
              cd => new CellDataProxy(cd));
    }
    
    private readonly CellData cd;

    public Actor.Actor Actor => cd.Actor;
    public Vector2Int Cell => cd.Cell;
    
    [MoonSharpHidden]
    public CellDataProxy(CellData cd) {
      this.cd = cd;
    }
  }
}