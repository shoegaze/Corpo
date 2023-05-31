using UnityEngine;

namespace Data {
  [System.Serializable]
  public class AbilityScript {
    [SerializeField] private AbilityData data;
    [SerializeField] private string rawScript;

    public AbilityData Data => data;
    public string RawScript => rawScript;

    public AbilityScript(AbilityData data, string rawScript) {
      this.data = data;
      this.rawScript = rawScript;
    }

    // public bool IsValid() { ... }
  }
}