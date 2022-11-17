using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Actor : MonoBehaviour {
  public enum ActorTeam {
    Ally,
    Enemy
  }
  
  [SerializeField] private ActorData data;
  [SerializeField] private ActorTeam team;
  [SerializeField] private List<Ability> abilities;
  [SerializeField] private uint health;

  public string Name => data.Name;
  public ActorTeam Team => team;

  public static void Load(ref Actor actor, ResourcesCache cache, string actorID, ActorTeam team) {
    // ?? Enforce actorID lowercase
    actorID = actorID.ToLower();
            
    { // Set sprite
      var renderer = actor.GetComponent<SpriteRenderer>();
      var sprite = cache.GetSprite(actorID);

      if (sprite == null) {
        Debug.LogError($"Sprite with actor ID \"{actorID}\" could not be found!");
        actor = null;
        return;
      }
      
      renderer.sprite = sprite;
    }

    { // Set actor data
      var actorData = cache.GetActorData(actorID);

      if (actorData == null) {
        Debug.LogError($"Actor data with actor ID \"{actorID}\" could not be found!");
        actor = null;
        return;
      }
      
      actor.data = actorData;
      actor.health = actorData.MaxHealth;
      actor.team = team;
      
      { // Set abilities
        var jobData = cache.GetJobData(actorData.Job);

        if (jobData == null) {
          Debug.LogError($"Job data with job ID \"{actorData.Job}\" could not be found!");
          actor = null;
          return;
        }
        
        var abilities = jobData.BaseAbilities.Select(cache.GetAbility);
        actor.abilities = abilities.ToList();
      }
    }
  }

  public GameObject GetView(Transform viewRoot) {
    var view = Instantiate(gameObject, viewRoot);
    view.SetActive(true);

    return view;
  }
  
  // TODO:
  
  // public bool IsAlive => health > 0;

  // public void TakeHealth(uint damage) {
  //   if (damage >= health) {
  //     health = 0;
  //     return;
  //   }
  //
  //   health -= damage;
  // }
  
  // public void GiveHealth(uint heal) {
  //   health += heal;
  //   health = health > data.maxHealth ? data.maxHealth : health;
  // }
}
