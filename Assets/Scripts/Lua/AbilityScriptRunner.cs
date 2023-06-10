using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Battle.Animation;
using Data;
using MoonSharp.Interpreter;
using UnityEngine;
using Zenject;

namespace Lua {
  // Register this binding for injection? 
  [RequireComponent(typeof(ScriptRunner))]
  public class AbilityScriptRunner : MonoBehaviour {
    [Inject] private BattleController battle;
    
    private AbilityData data;
    
    private readonly Script script = new(CoreModules.Preset_SoftSandbox);
    // TODO: Replace object types with Func<...>
    private object scriptFnStart;
    private object scriptFnGetCandidateCells;
    private object scriptFnBeforeAnimation;
    private object scriptFnAfterAnimation;
    private object scriptFnAnimate;

    // HACK:
    public string ScriptName => data.Name + ".lua";

    // TODO: Return bool based on validation
    public void Load(AbilityScript abilityScript) {
      data = abilityScript.Data;
      
      // TODO: Validate script and just load string
      //  script.LoadString(abilityScript.RawScript);

      script.DoString(abilityScript.RawScript);

      script.Globals["vector2"] = typeof(Vector2);
      script.Globals["vector2i"] = typeof(Vector2Int);
      script.Globals["vector3"] = typeof(Vector3);

		  script.Globals["math"] = typeof(Math);
      script.Globals["proc"] = typeof(Procedural);

      scriptFnStart = script.Globals["start"];
      scriptFnGetCandidateCells = script.Globals["get_candidate_cells"];
      scriptFnBeforeAnimation = script.Globals["before_animation"];
      scriptFnAfterAnimation = script.Globals["after_animation"];
      scriptFnAnimate = script.Globals["animate"];
    }

    private void UpdateScriptGlobals(GameManager game) {
      // Convenient logging functions
      script.Globals["log"] = (Action<string>)(
              s => Debug.LogFormat("[{0}]: {1}", ScriptName, s));
      script.Globals["warn"] = (Action<string>)(
              s => Debug.LogWarningFormat("[{0}]: {1}", ScriptName, s));
      script.Globals["err"] = (Action<string>)(
              s => Debug.LogErrorFormat("[{0}]: {1}", ScriptName, s));

      // TODO
      // script.Globals["grid"] = battle.Grid;
    }

    public void ExecuteStart(GameManager game) {
      Debug.Assert(script != null);

      bool success = false;

      try {
        UpdateScriptGlobals(game);
        success = script.Call(scriptFnStart).Boolean;
      }
      catch (ScriptRuntimeException e) {
        Debug.LogError(e);
      }
      
      // DEBUG:
      Debug.Log($"[{ScriptName}].start() => {success}");
    }

    public IEnumerable<Vector2Int> ExecuteGetCandidateCells(GameManager game) {
      Debug.Assert(script != null);

      var actor = battle.ActiveActor;
      Debug.Assert(actor != null);

      var cell = battle.Grid.GetPosition(actor);
      Debug.Assert(cell != null);

      IEnumerable<DynValue> candidates = Array.Empty<DynValue>();

      try {
        UpdateScriptGlobals(game);

        var cellData = new CellData(actor, cell.Value);
        candidates = script.Call(scriptFnGetCandidateCells, cellData).Table.Values;
      }
      catch (ScriptRuntimeException e) {
        Debug.LogError(e);
      }
      
      return candidates.Select(c => {
        // TODO: Support coord chains (i.e. IEnumerable<(x,y)>)
        int x = (int)c.Table.Get("x").Number;
        int y = (int)c.Table.Get("y").Number;
         
        return new Vector2Int(x, y);
      });
    }

    private IEnumerator DoAnimationLoop(GameManager game, CellData source, CellData target) {
      battle.LockTurn();
      
      // TODO: Refactor try-catches into separate methods?
      try {
        UpdateScriptGlobals(game);

        Debug.Log("Calling script.before_animation");
        script.Call(scriptFnBeforeAnimation, source, target);
      }
      catch (ScriptRuntimeException e) {
        Debug.LogError(e);
      }

      float startTime = Time.time;
      bool keep = true;
      
      while (keep) {
        Debug.Log("Calling script.animate");

        float seconds = Time.time - startTime;
        
        try {
          keep = script.Call(scriptFnAnimate, source, target, seconds).Boolean;
        }
        catch (ScriptRuntimeException e) {
          Debug.LogError(e);
        }

        yield return null;
      }

      try {
        Debug.Log("Calling script.after_animation");
        script.Call(scriptFnAfterAnimation, source, target);
      }
      catch (ScriptRuntimeException e) {
        Debug.LogError(e);
      }

      battle.UnlockTurn();

      while (!battle.IncrementTurn()) {
        yield return null;
      }
    }

    public void ExecuteAnimate(GameManager game, CellData source, CellData target) {
      Debug.Assert(script != null);

      StartCoroutine(DoAnimationLoop(game, source, target));
    }
  }
}