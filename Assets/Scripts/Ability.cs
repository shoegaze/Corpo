using Data;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Ability : MonoBehaviour {
  [SerializeField] private AbilityData data;
  [SerializeField] private AbilityScript script;

  // ReSharper disable once MemberCanBePrivate.Global
  // ReSharper disable once UnusedAutoPropertyAccessor.Global
  public Sprite Icon { get; private set; }
  public string Name => data.Name;
  // public string Type => data.Type;
  public int Cost => data.Cost;
  public AbilityScript Script => script;

  public static void Load(ref Ability ability, ResourcesCache resources, string abilityID) {
    { // Set sprite
      // TODO
      var renderer = ability.GetComponent<SpriteRenderer>();
      var sprite = resources.GetSprite(abilityID);

      if (sprite == null) {
        Debug.LogError($"Sprite with ability ID \"{abilityID}\" could not be found!");
        ability = null;
        return;
      }

      ability.Icon = sprite;
      renderer.sprite = sprite;
    }

    { // Set ability data
      var abilityData = resources.GetAbilityData(abilityID);

      if (abilityData == null) {
        Debug.LogError($"Ability data with ability ID \"{abilityID}\" could not be found!");
        ability = null;
        return;
      }

      ability.data = abilityData;
    }

    { // Set ability script
      var abilityScript = resources.GetAbilityScript(abilityID);

      if (abilityScript == null) {
        Debug.LogError($"Ability script with ability ID \"{abilityID}\" could not be found!");
        ability = null;
        return;
      }
              
      ability.script = abilityScript;
    }
  }
}

