using System.Collections.Generic;
using Data;
using UnityEngine;

public class ResourcesCache : MonoBehaviour {
  private readonly Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
  private readonly Dictionary<string, ActorData> actors = new Dictionary<string, ActorData>();
  private readonly Dictionary<string, JobData> jobs = new Dictionary<string, JobData>();
  private readonly Dictionary<string, AbilityData> abilities = new Dictionary<string, AbilityData>();

  public Sprite GetSprite(string spriteID) {
    sprites.TryGetValue(spriteID, out var sprite);
    return sprite;
  }

  public ActorData GetActorData(string actorID) {
    actors.TryGetValue(actorID, out var actorData);
    return actorData;
  }

  public JobData GetJobData(string jobID) {
    jobs.TryGetValue(jobID, out var jobData);
    return jobData;
  }

  public AbilityData GetAbilityData(string abilityID) {
    abilities.TryGetValue(abilityID, out var abilityData);
    return abilityData;
  }
  
  private void Awake() {
    LoadAll();
  }

  private void LoadAll() {
    LoadSprites();
    LoadActors();
    LoadJobs();
    LoadAbilities();
    
    foreach (var id in sprites.Keys) {
      Debug.Log($"Sprite: {id} => {GetSprite(id)})");
    }
    
    foreach (var id in actors.Keys) {
      Debug.Log($"Actor: {id} => {GetActorData(id)}");
    }
    
    foreach (var id in jobs.Keys) {
      Debug.Log($"Job: {id} => {GetJobData(id)}");
    }
    
    foreach (var id in abilities.Keys) {
      Debug.Log($"Ability: {id} => {GetAbilityData(id)}");
    }
  }

  private void LoadSprites() {
    var resources = Resources.LoadAll<Sprite>("Sprites");

    foreach (var res in resources) {
      var id = $"{res.name.ToLower()}";

      if (sprites.ContainsKey(id)) {
        Debug.Log($" ! Duplicate sprite ID \"{id}\"!");
        Debug.Log( "   * Ignoring...");
        continue;
      }

      sprites[id] = res;
    }
  }

  private void LoadActors() {
    var resources = Resources.LoadAll<TextAsset>("Actors");

    foreach (var res in resources) {
      var actorData = JsonUtility.FromJson<ActorData>(res.text);
      var id = $"{actorData.Name.ToLower()}";

      if (actors.ContainsKey(id)) {
        Debug.Log($" ! Duplicate actor ID \"{id}\"!");
        Debug.Log( "   * Ignoring...");
        continue;
      }

      actors[id] = actorData;
    }
  }

  private void LoadJobs() {
    var resources = Resources.LoadAll<TextAsset>("Jobs");
    
    foreach (var res in resources) {
      var jobData = JsonUtility.FromJson<JobData>(res.text);
      var id = $"{jobData.Name.ToLower()}";

      if (jobs.ContainsKey(id)) {
        Debug.Log($" ! Duplicate actor ID \"{id}\"!");
        Debug.Log( "   * Ignoring...");
        continue;
      }

      jobs[id] = jobData;
    }
  }

  private void LoadAbilities() {
    var resources = Resources.LoadAll<TextAsset>("Abilities");
    
    foreach (var res in resources) {
      var abilityData = JsonUtility.FromJson<AbilityData>(res.text);
      var id = $"{abilityData.Name.ToLower()}";
      
      if (abilities.ContainsKey(id)) {
        Debug.Log($" ! Duplicate ability ID \"{id}\"!");
        Debug.Log( "   * Ignoring...");
        continue;
      }
      
      abilities[id] = abilityData;
    }
  }
}
