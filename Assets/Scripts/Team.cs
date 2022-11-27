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
    // TODO: actorID not in actors.Select(a => a.name)
    return (actors.Count + 1) < maxCount;
  }
  
  public void Add(string actorID) {
    if (!CanAdd()) {
      Debug.LogWarning($"Cannot add actor {actorID} to team...");
      return;
    }
    
    // GameController/Instances
    var instanceRoot = transform.Find("Instances");
    Debug.Assert(instanceRoot != null);
    
    var actor = cache.GetActor(actorID, ActorAlignment.Ally);
    var instance = Instantiate(actor, instanceRoot);
    actors.Add(instance);
  }
  
  //public bool CanRemove() {}
  //public void Remove() {}
}
