using Data;

namespace Lua {
  public class LuaRunner {
    public AbilityScriptRunner AbilityRunner { get; }

    static LuaRunner() {
      // TODO: Register proxy types
      // UserData.RegisterType<MyProxy, MyType>(r => new MyProxy(r));
    }
    
    public LuaRunner() {
      AbilityRunner = new AbilityScriptRunner();
    }
  }
}