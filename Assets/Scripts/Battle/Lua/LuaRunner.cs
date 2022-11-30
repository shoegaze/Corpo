using MoonSharp.Interpreter;
using UnityEngine;

namespace Battle.Lua {
  public class LuaExecutor : MonoBehaviour {
    protected void Awake() {
      var lua = @"
        function factorial(n) do
          if (n == 0) then
            return 1
          end

          return n * factorial(n - 1)
        end

        return factorial(my_number)
      ";

      var script = new Script();
      script.Globals["my_number"] = 7;

      var foo = script.Globals["my_number"];
      
      var result = script.DoString(lua);
      Debug.Log($"lua factorial({7}) = {result.Number}");
    }
  }
}