using System.Collections.Generic;
using System.Linq;
using Battle;
using Battle.Animation;
using Data;
using UnityEngine;

namespace Actor {
  [RequireComponent(typeof(SpriteRenderer))]
  [RequireComponent(typeof(ActorAnimation))]
  public class Actor : MonoBehaviour {
    // TODO: Move to single file
    // TODO: Separate to ActorModel and ActorView
    
    [SerializeField] private ActorData data;
    [SerializeField] private List<Ability> abilities;
    [SerializeField] private uint health;

    private Team team;

    public Team Team => team;
    public string Name => data.Name;
    public uint MaxHealth => data.MaxHealth;
  
    public ActorAlignment Alignment => team.Alignment;
    public IEnumerable<Ability> Abilities => abilities.ToArray();
    public uint Health => health;
    public bool IsAlive => health > 0;

    // TODO: Change type to ActorView
    public GameObject View { get; private set; }

    public static void Load(ref Actor actor, ResourcesCache resources, string actorID) {
      { // Set sprite
        var renderer = actor.GetComponent<SpriteRenderer>();
        var sprite = resources.GetSprite(actorID);

        if (sprite == null) {
          Debug.LogError($"Sprite with actor ID \"{actorID}\" could not be found!");
          actor = null;
          return;
        }
      
        renderer.sprite = sprite;
      }

      { // Set actor data
        var actorData = resources.GetActorData(actorID);

        if (actorData == null) {
          Debug.LogError($"Actor data with actor ID \"{actorID}\" could not be found!");
          actor = null;
          return;
        }
      
        actor.data = actorData;
        actor.health = actorData.MaxHealth;
      
        { // Set abilities
          var jobData = resources.GetJobData(actorData.Job);

          if (jobData == null) {
            Debug.LogError($"Job data with job ID \"{actorData.Job}\" could not be found!");
            actor = null;
            return;
          }
        
          var abilities = jobData.BaseAbilities.Select(resources.GetAbility);
          actor.abilities = abilities.ToList();
        }
      }
    }

    public GameObject CreateView(Transform viewRoot) {
      View = Instantiate(gameObject, viewRoot);
      // TODO: Add ActorView behavior to instanced view
      View.SetActive(true);

      return View;
    }

    public void SetTeam(Team team) {
      this.team = team;
    } 
  
    public void TakeHealth(uint damage) {
      Effect.ShowDamagePopup(View.transform.position, damage);
      
      if (damage >= health) {
        health = 0;
        return;
      }
  
      health -= damage;
    }
  
    // public void GiveHealth(uint heal) {
    //   Effect.ShowHealPopup(View.transform.position, heal);
    //   
    //   health += heal;
    //   health = health > data.maxHealth ? data.maxHealth : health;
    // }


    public void Attack(AttackContext ctx) {
      { // TODO: Cache ActorAnimation
        var anim = View.GetComponent<ActorAnimation>();
        anim.StartAttack(ctx);
      }
      
      { // TODO: Make hurt animation public
        var anim = ctx.Target.View.GetComponent<ActorAnimation>();
        anim.StartHurt(ctx);
      }

      var target = ctx.Target;
      target.TakeHealth(1);

      if (!target.IsAlive) {
        target.Die(ctx);
      }
    }

    private void Die(AttackContext ctx) {
      var grid = ctx.Grid;
      var target = ctx.Target;
    
      bool removed = grid.TryRemoveActor(target);
      Debug.Assert(removed);
    
      // Deactivate view
      View.SetActive(false);
      gameObject.SetActive(false);
    }
  }
}
