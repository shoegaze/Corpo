using UnityEngine;

namespace Data {
  [System.Serializable]
  public class AbilityScript {
    [SerializeField] private string rawScript;
    
    // TODO: Store AbilityData to get animation parameters
    // private AbilityData data;
    
    public string RawScript => rawScript;

    public AbilityScript(string rawScript) {
      this.rawScript = rawScript;
    }

    // public bool IsValid() { ... }
  }
}