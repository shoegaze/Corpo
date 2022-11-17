using Data;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Ability : MonoBehaviour {
  [SerializeField] private AbilityData data;

  public static void Load(ref Ability ability, ResourcesCache cache, string abilityID) {
    // ?? Enforce abilityID lowercase
    abilityID = abilityID.ToLower();

    { // Set sprite
      // TODO
      var renderer = ability.GetComponent<SpriteRenderer>();
      var sprite = cache.GetSprite(abilityID);

      if (sprite == null) {
        Debug.LogError($"Sprite with ability ID \"{abilityID}\" could not be found!");
        ability = null;
        return;
      }

      renderer.sprite = sprite;
    }

    { // Set ability data
      var abilityData = cache.GetAbilityData(abilityID);

      if (abilityData == null) {
        Debug.LogError($"Ability data with ability ID \"{abilityID}\" could not be found!");
        ability = null;
        return;
      }

      ability.data = abilityData;
    }
  }
  
  // TODO:
  
  // public void ExecuteGetTargets() {}
  // public void ExecuteOnHit() {}
  // public void ExecuteOnMiss() {}
}

