using UnityEngine;

namespace Data {
  [System.Serializable]
  public class ActorData {
    [SerializeField] private string name;
    [SerializeField] private string job;
    [SerializeField] private uint maxHealth;
    [SerializeField] private int payRate;

    // public s†ring scriptGetIdea; // lua: (ctx) => "move" | "attack" | "ability" ...
    
    public string Name => name;
    public string Job => job;
    public uint MaxHealth => maxHealth;
    public int PayRate => payRate;
  
    public override string ToString() {
      return $"({name}, {job}, {maxHealth}, {payRate})";
    }
  }
}
