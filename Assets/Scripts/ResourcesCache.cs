using System.Collections.Generic;
using System.Linq;
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
  private readonly Dictionary<string, AbilityScript> abilitiesScripts = new Dictionary<string, AbilityScript>();

  private readonly Dictionary<string, Actor.Actor> actors = new Dictionary<string, Actor.Actor>();
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

  public AbilityScript GetAbilityScript(string abilityID) {
    // HACK: Append .lua extension because Resource.Load* won't natively load lua files
    abilitiesScripts.TryGetValue(abilityID + ".lua", out var abilityScript);
    return abilityScript;
  }

  public Actor.Actor GetActor(string actorID, ActorAlignment team) {
    if (!actors.ContainsKey(actorID)) {
      var go = Instantiate(prototypeActor, actorsRoot);
      go.gameObject.SetActive(false);
      
      var actor = go.GetComponent<Actor.Actor>();
      actor.name = $"{actorID}";
      
      Actor.Actor.Load(ref actor, this, actorID, team);

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
    LoadAbilitiesScripts();
  }

  private void LoadSprites() {
    var resources = Resources.LoadAll<Sprite>("Sprites");

    Debug.Log("Loading sprites...");

    foreach (var res in resources) {
      var id = $"{res.name.ToLower()}";

      if (sprites.ContainsKey(id)) {
        Debug.LogError(
                $" ! Duplicate sprite ID \"{id}\"!\n" + 
                "   * Ignoring..."
        );
        continue;
      }

      sprites[id] = res;
    }
  }

  private void LoadActorsData() {
    var resources = Resources.LoadAll<TextAsset>("Actors");

    Debug.Log("Loading actors data...");

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

    Debug.Log("Loading jobs data...");
    
    foreach (var res in resources) {
      var jobData = JsonUtility.FromJson<JobData>(res.text);
      var id = $"{jobData.Name.ToLower()}";

      if (jobsData.ContainsKey(id)) {
        Debug.LogError(
                $" ! Duplicate actor ID \"{id}\"!\n" + 
                "   * Ignoring..."
        );
        continue;
      }

      jobsData[id] = jobData;
    }
  }

  private void LoadAbilitiesData() {
    var resources = Resources.LoadAll<TextAsset>("Abilities/Data");

    Debug.Log("Loading abilities data...");
    
    foreach (var res in resources) {
      var abilityData = JsonUtility.FromJson<AbilityData>(res.text);
      var id = $"{abilityData.Name.ToLower()}";
      
      if (abilitiesData.ContainsKey(id)) {
        Debug.LogError(
                $" ! Duplicate ability ID \"{id}\"!\n" + 
                "   * Ignoring..."
        );
        continue;
      }
      
      abilitiesData[id] = abilityData;
    }
  }

  private void LoadAbilitiesScripts() {
    var resources = Resources.LoadAll<TextAsset>("Abilities/Scripts");

    Debug.Log("Loading abilities scripts...");
    
    foreach (var res in resources) {
      var scriptID = $"{res.name.ToLower()}";
      
      // HACK:
      var id = scriptID.Substring(0, scriptID.Length - 4);
      var data = GetAbilityData(id);
      Debug.Assert(data != null);
      
      string abilityRawScript = res.text;

      Debug.Log($" > Loading ability script \"{scriptID}\"");
      
      if (abilitiesScripts.ContainsKey(scriptID)) {
        Debug.LogError(
                $" ! Duplicate ability ID \"{scriptID}\"!\n" + 
                "   * Ignoring..."
        );
        continue;
      }
      
      abilitiesScripts[scriptID] = new AbilityScript(data, abilityRawScript);
    }
  }
}
