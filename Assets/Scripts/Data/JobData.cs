using System.Linq;
using UnityEngine;

namespace Data {
  [System.Serializable]
  public class JobData {
    [SerializeField] private string name;
    [SerializeField] private string[] baseAbilities;

    public string Name => name;
    public string[] BaseAbilities => baseAbilities;
  
    public override string ToString() {
      return $"({name}, [{string.Join(", ", baseAbilities)}])";
    }
  }
}
