using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Data;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Lua {
  [RequireComponent(typeof(ScriptRunner))]
  public class AbilityScriptRunner : MonoBehaviour {
    private AbilityData data;
    
    // TODO: Lock script execution when animation loop is running
    // private bool locked;
    private BattleController battle;
    
    private readonly Script script = new(CoreModules.Preset_SoftSandbox);
    // TODO: Replace object types with Func<...>
    private object scriptFnStart;
    private object scriptFnGetCandidateCells;
    private object scriptFnBeforeAnimation;
    private object scriptFnAfterAnimation;
    private object scriptFnAnimate;

    // HACK:
    public string ScriptName => data.Name + ".lua";

    protected void Start() {
      var go = GameObject.FindWithTag("BattleController");
      battle = go.GetComponent<BattleController>();
    }

    // TODO: Return bool based on validation
    public void Load(AbilityScript abilityScript) {
      data = abilityScript.Data;
      
      // TODO: Validate script and just load string
      //  script.LoadString(abilityScript.RawScript);

      script.DoString(abilityScript.RawScript);

      script.Globals["vector2"] = typeof(Vector2);
      script.Globals["vector2i"] = typeof(Vector2Int);
      script.Globals["vector3"] = typeof(Vector3);

      script.Globals["proc"] = new Procedural();

      scriptFnStart = script.Globals["start"];
      scriptFnGetCandidateCells = script.Globals["get_candidate_cells"];
      scriptFnBeforeAnimation = script.Globals["before_animation"];
      scriptFnAfterAnimation = script.Globals["after_animation"];
      scriptFnAnimate = script.Globals["animate"];
    }

    private void UpdateScriptGlobals(GameController game) {
      // Convenient logging functions
      script.Globals["log"] = (Action<string>)(
              s => Debug.LogFormat("[{0}]: {1}", ScriptName, s));
      script.Globals["warn"] = (Action<string>)(
              s => Debug.LogWarningFormat("[{0}]: {1}", ScriptName, s));
      script.Globals["err"] = (Action<string>)(
              s => Debug.LogErrorFormat("[{0}]: {1}", ScriptName, s));

      // TODO
      // script.Globals["duration"] = ...;
      // script.Globals["grid"] = ...;
    }

    // TODO: Add error handling for Execute_ methods
    public void ExecuteStart(GameController game) {
      Debug.Assert(script != null);
      
      UpdateScriptGlobals(game);
      bool success = script.Call(scriptFnStart).Boolean;
      
      // DEBUG:
      Debug.Log($"[{ScriptName}].start() => {success}");
    }
     
    public IEnumerable<Vector2Int> ExecuteGetCandidateCells(GameController game) {
      Debug.Assert(script != null);
       
      UpdateScriptGlobals(game);
       
      var actor = battle.ActiveActor;
      Debug.Assert(actor != null);
       
      var cell = battle.Grid.GetPosition(actor);
      Debug.Assert(cell != null);

      var cellData = new CellData(actor, cell.Value); 
      var candidates = script.Call(scriptFnGetCandidateCells, cellData);

      return candidates.Table.Values.Select(c => {
        // TODO: Support coord chains (i.e. IEnumerable<(x,y)>)
        int x = (int)c.Table.Get("x").Number;
        int y = (int)c.Table.Get("y").Number;
         
        return new Vector2Int(x, y);
      });
    }

    // TODO
    private IEnumerator DoAnimationLoop(CellData source, CellData target) {
      // TODO: Lock input until animation loop is done
      // locked = true;
      
      // DEBUG:
      const float duration = 1f;
       
      float startTime = Time.time;
      var t = 0f;
       
      Debug.Log("Calling script.before_animation");
      script.Call(scriptFnBeforeAnimation, source, target);

      while (t <= 1f) {
        Debug.Log("Calling script.animate");
        script.Call(scriptFnAnimate, source, target, t);
       
        yield return new WaitForEndOfFrame();

        t = (Time.time - startTime) / duration;
      }
       
      Debug.Log("Calling script.after_animation");
      script.Call(scriptFnAfterAnimation, source, target);

      // locked = false;
    }

    public void ExecuteAnimate(GameController game, CellData source, CellData target) {
      Debug.Assert(script != null);
       
      UpdateScriptGlobals(game);
      StartCoroutine(DoAnimationLoop(source, target));
    }
  }
}