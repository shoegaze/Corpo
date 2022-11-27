using System.Collections.Generic;
using Data;
using UnityEngine;

public class ResourcesCache : MonoBehaviour {
  [SerializeField] private GameObject prototypeActor;
  [SerializeField] private GameObject prototypeAbility;
  
  // TODO: Separate into actorSprites & effectSprites
  private readonly Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
  private readonly Dictionary<string, ActorData> actorsData = new Dictionary<string, ActorData>();
  private readonly Dictionary<string, JobData> jobsData = new Dictionary<string, JobData>();
  private readonly Dictionary<string, AbilityData> abilitiesData = new Dictionary<string, AbilityData>();

  private readonly Dictionary<string, Actor> actors = new Dictionary<string, Actor>();
  private readonly Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();

  private Transform actorsRoot;
  private Transform effectsRoot;
  
  public Sprite GetSprite(string spriteID) {
    sprites.TryGetValue(spriteID, out var sprite);
    return sprite;
  }

  public ActorData GetActorData(string actorID) {
    actorsData.TryGetValue(actorID, out var actorData);
    return actorData;
  }

  public JobData GetJobData(string jobID) {
    jobsData.TryGetValue(jobID, out var jobData);
    return jobData;
  }

  public AbilityData GetAbilityData(string abilityID) {
    abilitiesData.TryGetValue(abilityID, out var abilityData);
    return abilityData;
  }
  
  public Actor GetActor(string actorID, ActorAlignment team) {
    if (!actors.ContainsKey(actorID)) {
      var go = Instantiate(prototypeActor, actorsRoot);
      go.gameObject.SetActive(false);
      
      var actor = go.GetComponent<Actor>();
      actor.name = $"{actorID}";
      
      Actor.Load(ref actor, this, actorID, team);

      if (actor == null) {
        Debug.LogError($"Could not create actor \"{actorID}\"!");
        return null;
      }
      
      actors[actorID] = actor;
    }

    return actors[actorID];
  }

  public Ability GetAbility(string abilityID) {
    if (!abilities.ContainsKey(abilityID)) {
      var go = Instantiate(prototypeAbility, effectsRoot);
      go.gameObject.SetActive(false);

      var ability = go.GetComponent<Ability>();
      ability.name = $"{abilityID}";
      
      Ability.Load(ref ability, this, abilityID);

      if (ability == null) {
        Debug.LogError($"Could not create ability \"{abilityID}\"!");
        return null;
      }

      abilities[abilityID] = ability;
    }

    return abilities[abilityID];
  }
  
  private void Awake() {
    actorsRoot = transform.Find("Cache/Actors");
    effectsRoot = transform.Find("Cache/Effects");
    
    Debug.Assert(actorsRoot != null);
    Debug.Assert(effectsRoot != null);
    
    LoadAll();
  }

  private void LoadAll() {
    LoadSprites();
    LoadActorsData();
    LoadJobsData();
    LoadAbilitiesData();
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

  private void LoadActorsData() {
    var resources = Resources.LoadAll<TextAsset>("Actors");

    foreach (var res in resources) {
      var actorData = JsonUtility.FromJson<ActorData>(res.text);
      var id = $"{actorData.Name.ToLower()}";

      if (actorsData.ContainsKey(id)) {
        Debug.LogError(
                $" ! Duplicate actor ID \"{id}\"!\n" + 
                "   * Ignoring..."
        );
        continue;
      }

      actorsData[id] = actorData;
    }
  }

  private void LoadJobsData() {
    var resources = Resources.LoadAll<TextAsset>("Jobs");
    
    foreach (var res in resources) {
      var jobData = JsonUtility.FromJson<JobData>(res.text);
      var id = $"{jobData.Name.ToLower()}";

      if (jobsData.ContainsKey(id)) {
        Debug.LogError(
                $" ! Duplicate actor ID \"{id}\"!" + 
                "   * Ignoring..."
        );
        continue;
      }

      jobsData[id] = jobData;
    }
  }

  private void LoadAbilitiesData() {
    var resources = Resources.LoadAll<TextAsset>("Abilities");
    
    foreach (var res in resources) {
      var abilityData = JsonUtility.FromJson<AbilityData>(res.text);
      var id = $"{abilityData.Name.ToLower()}";
      
      if (abilitiesData.ContainsKey(id)) {
        Debug.LogError(
                $" ! Duplicate ability ID \"{id}\"!" + 
                "   * Ignoring..."
        );
        continue;
      }
      
      abilitiesData[id] = abilityData;
    }
  }
}
