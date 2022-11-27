using TMPro;
using UnityEngine;

namespace Battle.UI {
[RequireComponent(typeof(TextMeshProUGUI))]
  public class TurnValueLabel : MonoBehaviour {
    [SerializeField] private BattleController battle;

    private TextMeshProUGUI label;

    protected void Awake() {
      label = GetComponent<TextMeshProUGUI>();
    }

    protected void OnGUI() {
      label.text = $"{battle.Turn:0000}";
    }
  }
}
