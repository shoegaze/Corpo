using MoonSharp.Interpreter;
using UnityEngine;

namespace Lua.Proxy {
  public class TransformProxy {
    static TransformProxy() {
      UserData.RegisterProxyType<TransformProxy, Transform>(
              t => new TransformProxy(t)
      );
    }

    private readonly Transform t;
    
    [MoonSharpHidden]
    public TransformProxy(Transform t) {
      this.t = t;
    }

    public Vector3 GetPosition() {
      return t.position;
    }

    public void SetPosition(Vector3 position) {
      t.position = position;
    }
  }
}