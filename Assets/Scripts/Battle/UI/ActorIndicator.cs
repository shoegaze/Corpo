using TMPro;
using UnityEngine;

namespace Battle.UI {
  public class ActorIndicator : MonoBehaviour {
    [SerializeField] private BattleController battle;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI healthLabel;

    protected void OnGUI() {
      var actor = battle.ActiveActor;

      if (actor != null) {
        nameLabel.text = $"* {actor.Name}";
        healthLabel.text = $"{actor.Health}/{actor.MaxHealth}";
      }
      else {
        nameLabel.text = "* error";
        healthLabel.text = "0/0";
      }
    }
  }
}