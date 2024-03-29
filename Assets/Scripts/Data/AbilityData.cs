﻿using UnityEngine;

namespace Data {
  [System.Serializable]
  public class AbilityData {
    [SerializeField] private string name;
    [SerializeField] private string type; // :: "passive" | "active" 
    [SerializeField] private int cost;

    // public string getTargets; // lua: (origin) => [coord] 
    // public string onHit;  // lua: (Actor) => ()
    // public string onMiss; // lua: (Actor) => ()

    public string Name => name;
    public string Type => type;
    public int Cost => cost;
  
    public override string ToString() {
      return $"({name}, {type}, {cost})";
    }
  }
}
