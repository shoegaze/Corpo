using Battle;
using Lua.Proxy;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Lua {
  [RequireComponent(typeof(AbilityScriptRunner))]
  public class ScriptRunner : MonoBehaviour {
    static ScriptRunner() {
      Debug.Log("Setting up the scripting engine...");
      
      Script.DefaultOptions.DebugPrint = Debug.Log;
      
      // TODO: Should probably avoid registering structs...
      UserData.RegisterType<Vector2>();
      UserData.RegisterType<Vector2Int>();
      UserData.RegisterType<Vector3>();
      
      Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(
		      DataType.Table,
		      typeof(Vector2),
		      v => {
			      var t = v.Table;
			      float x = (float)t.Get("x").Number;
			      float y = (float)t.Get("y").Number;

			      return new Vector2(x, y);
		      });

      Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(
		      DataType.Table,
		      typeof(Vector2Int),
		      v => {
			      var t = v.Table;
			      int x = (int)t.Get("x").Number;
			      int y = (int)t.Get("y").Number;

			      return new Vector2Int(x, y);
		      });
     
      Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(
		      DataType.Table,
		      typeof(Vector3),
		      v => {
			      var t = v.Table;
			      float x = (float)t.Get("x").Number;
			      float y = (float)t.Get("y").Number;
			      float z = (float)t.Get("z").Number;

			      return new Vector3(x, y, z);
		      });

      
      UserData.RegisterProxyType<ActorProxy, Actor.Actor>(
              a => new ActorProxy(a));
    
      UserData.RegisterProxyType<CellDataProxy, CellData>(
              cd => new CellDataProxy(cd));
      
      UserData.RegisterProxyType<BattleGridProxy, BattleGrid>(
              grid => new BattleGridProxy(grid));
      
      UserData.RegisterAssembly();
    }
  }
}