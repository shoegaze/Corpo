using UnityEngine;

namespace Battle.UI {
  public class AbilitiesList : MonoBehaviour {
    [SerializeField] private BattleController battle;
    [SerializeField] private Transform entriesRoot;
    [SerializeField] private GameObject entryPrototype;

    private Actor.Actor previousActor;

    protected void OnGUI() {
      var actor = battle.ActiveActor;

      if (actor != previousActor) {
        GenerateEntries();
      }

      previousActor = actor;
    }

    private void DestroyEntries() {
      for (var i = entriesRoot.childCount-1; i >= 0; i--) {
        var child = entriesRoot.GetChild(i);
        Destroy(child.gameObject);
      }
    }

    private void GenerateEntries() {
      DestroyEntries();
      
      var actor = battle.ActiveActor;
      
      if (actor == null) {
        return;
      }

      var abilities = actor.Abilities;

      foreach (var ability in abilities) {
        var view = Instantiate(entryPrototype, entriesRoot);
        var entry = view.GetComponent<AbilityEntry>();
        
        entry.SetLabels(ability);
      }
    }
  }
}