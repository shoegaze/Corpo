using MoonSharp.Interpreter;
using UnityEngine;
using CellData = System.Tuple<Actor.Actor, UnityEngine.Vector2Int>;

namespace Lua.Proxy {
  public class CellDataProxy {
    static CellDataProxy() {
      UserData.RegisterProxyType<CellDataProxy, CellData>(
              cd => new CellDataProxy(cd)
      );
    }

    private readonly CellData cd;
    
    [MoonSharpHidden]
    public CellDataProxy(CellData cd) {
      this.cd = cd;
    }
    
    public Actor.Actor GetActor() {
      return cd.Item1;
    }

    public Vector2Int GetCell() {
      return cd.Item2;
    }
  }
}