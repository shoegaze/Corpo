using System.Linq;
using Actor;
using Battle.View;
using Lua;
using UnityEngine;
using Zenject;

namespace Battle.Model {
  public class MenuModel : IMenuModel {
    [Inject] private GameManager game;
    [Inject] private BattleManager battle;
    [Inject] private BattleView view;
    
    // TODO: Get rid of this injection:
    [Inject] private AbilityScriptRunner abilityScriptRunner;
    // TODO: Get rid of this injection:
    [Inject] private AbilitySelect abilitySelect;

    public StateManager StateManager { get; private set; }
    public int AbilityIndex { get; private set; }

    public void Initialize() {
      StateManager = new StateManager(this);
    }
    
    // Select ability:
    //  1. Enter candidate cell selection mode
    //   * On select candidate:
    //    1. Execute script with target context
    //    2. Enter animation coroutine
    //    3. Update animation context
    //    4. Clean up animation coroutine
    //    5. Increment turn
    //   * On cancel:
    //    1. Go back to cycling mode
    public void SelectAbility() {
      StateManager.Transition(SelectState.Focus);
    }

    public void CycleAbility(int dy) {
      int index = AbilityIndex;
      
      AbilityIndex -= dy;

      var actor = battle.ActiveActor;
      int max = actor.Abilities.Count() - 1;
      AbilityIndex = Mathf.Clamp(AbilityIndex, 0, max);

      if (index != AbilityIndex) {
        // TODO: Refactor into LoadAbility(game, actor)
        var ability = GetCurrentAbility(actor);
        abilityScriptRunner.Load(ability.Script);
        abilityScriptRunner.ExecuteStart(game);
        
        // Precondition: Script should be loaded
        var candidates = abilityScriptRunner.ExecuteGetCandidateCells(game);
        abilitySelect.Queue(candidates);
      }
    }
   
    public void LoadAbility() {
      var actor = battle.ActiveActor;
      if (actor == null || actor.Alignment != ActorAlignment.Ally) {
        return;
      }
      
      // Load ability at current index on Mode.MenuModel
      // TODO: Avoid by keeping track of loaded ability
      var ability = GetCurrentAbility(actor);
      abilityScriptRunner.Load(ability.Script);
      abilityScriptRunner.ExecuteStart(game);

      // Precondition: Script should be loaded
      var candidates = abilityScriptRunner.ExecuteGetCandidateCells(game);
      abilitySelect.Queue(candidates);
    }
    
    private Ability GetCurrentAbility(Actor.Actor actor) {
      return actor.Abilities
          .Skip(AbilityIndex)
          .First();
    }
  }
}