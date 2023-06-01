using Battle;
using Lua.Proxy;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Lua {
  [RequireComponent(typeof(AbilityScriptRunner))]
  public class ScriptRunner : MonoBehaviour {
    static ScriptRunner() {
      Debug.Log("Configuring scripting engine...");
      
      Script.DefaultOptions.DebugPrint = Debug.Log;
      UserData.RegisterAssembly();
      
      UserData.RegisterType<Vector2>();
      UserData.RegisterType<Vector2Int>();
      
      UserData.RegisterProxyType<TransformProxy, Transform>(
              t => new TransformProxy(t));
      
      UserData.RegisterProxyType<ActorProxy, Actor.Actor>(
              a => new ActorProxy(a));
    
      UserData.RegisterProxyType<CellDataProxy, CellData>(
              cd => new CellDataProxy(cd));
      
      UserData.RegisterProxyType<BattleGridProxy, BattleGrid>(
              grid => new BattleGridProxy(grid));
    }
  }
}