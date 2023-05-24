using MoonSharp.Interpreter;
using UnityEngine;

namespace Battle.Lua {
  public class LuaRunner : MonoBehaviour {
    protected void Awake() {
      const string lua = @"
        function factorial(n)
          if n == 0 then
            return 1
          end

          return n * factorial(n - 1)
        end
      ";

      var script = new Script();
      script.Globals["my_number"] = 7;

      script.DoString(lua);

      const int n = 7;
      var result = script.Call(script.Globals["factorial"], n);
      Debug.Log($"lua: factorial({n}) = {result.Number}");
    }
    
    // TODO
  }
}