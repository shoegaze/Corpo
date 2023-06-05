using System;
using System.Collections.Generic;
using UnityEngine;

// TODO: Make ally/enemy agnostic
// TODO: Attach to GameController then copy to BattleController
public class Team {
  private const uint MaxMembers = 4;
  
  private readonly List<Actor.Actor> actors = new();

  public IEnumerable<Actor.Actor> Actors => actors;
  public ActorAlignment Alignment { get; }
  public int Money { get; private set; }

  public Team(ActorAlignment alignment) {
    Alignment = alignment;
  }

  public Team(ActorAlignment alignment, IEnumerable<Actor.Actor> actors) {
    Alignment = alignment;
    
    foreach (var actor in actors) {
      this.actors.Add(actor);
    }
  }
  
  public bool CanAdd(/*Actor.Actor actor*/) {
    // TODO: Ensure unique actors i.e. !actors.Any(a => a.id == actor.id)
    return (actors.Count + 1) < MaxMembers;
  }

  public void Add(Actor.Actor actor) {
    Debug.Assert(CanAdd());

    actor.SetTeam(this);
    actors.Add(actor);
  }
  
  //public bool CanRemove() {}
  //public void Remove() {}

  public void TakeMoney(int cost) {
    if (cost > Money) {
      Debug.LogErrorFormat(
          "Team {0} doesn't have enough money! {1}/{2}", 
          Alignment,
          cost,
          Money);
      
      return;
    }
    
    Money -= cost;
  }
}
