using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle.UI {
  public class AbilityEntry : MonoBehaviour {
    [SerializeField] private Sprite errorIcon;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI costLabel;
    
    public void SetLabels(Ability ability) {
      var game = GameObject.FindWithTag("GameController");
      var cache = game.GetComponent<ResourcesCache>();
      var icon = cache.GetSprite(ability.Name);

      iconImage.sprite = icon ?? errorIcon;
      nameLabel.text = ability.Name;
      costLabel.text = $"{ability.Cost}";
    }
  }
}