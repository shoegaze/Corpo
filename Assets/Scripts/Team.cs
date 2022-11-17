using System.Collections.Generic;
using UnityEngine;

// TODO: Make ally/enemy agnostic
// TODO: Attach to GameController then copy to BattleController
public class Team : MonoBehaviour {
  // [SerializeField] private int money;
  [SerializeField] private uint maxCount = 4;
  [SerializeField] private List<Actor> actors;
  
  private ResourcesCache cache;

  public IEnumerable<Actor> Actors => actors;
  
  protected void Start() {
    var game = GameObject.FindGameObjectWithTag("GameController");
    cache = game.GetComponent<ResourcesCache>();
    
    Debug.Assert(cache != null);
  }
  
  public bool CanAdd(/*string actorID*/) {
    return (actors.Count + 1) < maxCount;
  }
  
  public void Add(string actorID) {
    if (!CanAdd()) {
      Debug.LogWarning($"Cannot add actor {actorID} to team...");
      return;
    }
    
    var ally = cache.GetActor(actorID, Actor.ActorAlignment.Ally);
    actors.Add(ally);
  }
  
  //public bool CanRemove() {}
  //public void Remove() {}
}
