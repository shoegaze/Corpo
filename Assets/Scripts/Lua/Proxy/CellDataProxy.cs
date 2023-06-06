using MoonSharp.Interpreter;
using UnityEngine;
// ReSharper disable UnusedMember.Global

namespace Lua.Proxy {
  public class CellDataProxy {
    private readonly CellData cd;

    public Actor.Actor Actor => cd.Actor;
    public Vector2Int Cell => cd.Cell;
    public bool HasActor => cd.HasActor;
    public Vector3 Position => cd.Position;
    
    [MoonSharpHidden]
    public CellDataProxy(CellData cd) {
      this.cd = cd;
    }
  }
}