using System.Collections.Generic;
using System.Linq;
using Battle;
using Data;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Actor : MonoBehaviour {
  // TODO: Move to single file
  public enum ActorAlignment {
    Ally,
    Enemy
  }

  // [SerializeField] private uint instanceID;
  [SerializeField] private ActorData data;
  [SerializeField] private ActorAlignment alignment;
  [SerializeField] private List<Ability> abilities;
  [SerializeField] private uint health;

  private GameObject view;
  
  public string Name => data.Name;
  public ActorAlignment Alignment => alignment;
  public uint Health => health;
  public bool IsAlive => health > 0;
  public GameObject View => view;

  public static void Load(ref Actor actor, ResourcesCache cache, string actorID, ActorAlignment team) {
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
      actor.alignment = team;
      
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

  public GameObject CreateView(Transform viewRoot) {
    view = Instantiate(gameObject, viewRoot);
    view.SetActive(true);

    return view;
  }
  
  public void TakeHealth(uint damage) {
    if (damage >= health) {
      health = 0;
      return;
    }
  
    health -= damage;
  }
  
  // public void GiveHealth(uint heal) {
  //   health += heal;
  //   health = health > data.maxHealth ? data.maxHealth : health;
  // }


  public void Attack(AttackContext ctx) {
    var anim = view.GetComponent<ActorAnimation>();
    anim.StartAttack(ctx);

    var target = ctx.Target;
    target.TakeHealth(1);

    if (!target.IsAlive) {
      target.Die(ctx);
    }
  }

  private void Die(AttackContext ctx) {
    var grid = ctx.Grid;
    var target = ctx.Target;
    
    var removed = grid.TryRemoveActor(target);
    Debug.Assert(removed);
    
    target.View.SetActive(false);
    target.gameObject.SetActive(false);
  }
}
