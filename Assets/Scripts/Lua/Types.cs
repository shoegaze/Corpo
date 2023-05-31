using MoonSharp.Interpreter;
using UnityEngine;

namespace Lua {
  // ReSharper disable once UnusedType.Global
  public static class Types {
    static Types() {
      UserData.RegisterType<Vector2>();
      UserData.RegisterType<Vector2Int>();
    }
  }
}