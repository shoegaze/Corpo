using MoonSharp.Interpreter;
using UnityEngine;

namespace Lua.Proxy {
  public class TransformProxy {
    static TransformProxy() {
      UserData.RegisterProxyType<TransformProxy, Transform>(
              t => new TransformProxy(t));
    }

    private readonly Transform t;

    public Vector3 Position {
      get => t.position;
      set => t.position = value;
    }
    
    [MoonSharpHidden]
    public TransformProxy(Transform t) {
      this.t = t;
    }
  }
}