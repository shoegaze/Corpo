using UnityEngine;

namespace Data {
  [System.Serializable]
  public class AbilityScript {
    [SerializeField] private string script;
    
    // TODO: Expose script string

    public AbilityScript(string script) {
      // TODO: Validate script with LuaRunner.loadString
      this.script = script;
    }
  }
}