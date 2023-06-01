using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Lua.Proxy;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Lua {
  [RequireComponent(typeof(ScriptRunner))]
  public class AbilityScriptRunner : MonoBehaviour {
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
      Debug.Assert(enabled && script != null);
      
      UpdateScriptGlobals(game);
      bool success = script.Call(scriptFnStart).Boolean;
      
      // DEBUG:
      Debug.Log($"[{ScriptName}].start() => {success}");
    }
     
    public IEnumerable<Vector2Int> ExecuteGetCandidateCells(GameController game) {
      Debug.Assert(enabled && script != null);
       
      UpdateScriptGlobals(game);
       
      var actor = game.Battle.ActiveActor;
      Debug.Assert(actor != null);
       
      var cell = game.Battle.Grid.GetPosition(actor);
      Debug.Assert(cell != null);
      
      var cellData = new CellData(actor, cell.Value); 
      var candidates = script.Call(scriptFnGetCandidateCells, cellData);

      return candidates.Table.Values.Select(c => {
        // TODO: Support coord chains (i.e. IEnumerable<(x,y)>)
        var coord = c.Table.Values.ToArray();
        
        var x = (int)(coord[0].Number);
        var y = (int)(coord[1].Number);
         
        return new Vector2Int(x, y);
      });
    }

    // TODO
    private IEnumerator DoAnimationLoop() {
      // TODO: Lock input until animation loop is done
      
      // DEBUG:
      const float duration = 1f;
       
      float startTime = Time.time;
      var t = 0f;
       
      // script.Call(scriptFnBeforeAnimation, { actor: 4, cell: (5, 6) });

      while (t <= 1f) {
        // script.Call(
        //         scriptFnAnimate,
        //         { actor: 10, cell: (11, 12) },
        //         { actor: 13, cell: (14, 15) },
        //         t
        // );
       
        yield return new WaitForEndOfFrame();

        t = (Time.time - startTime) / duration;
      }
       
      // script.Call(scriptFnAfterAnimation, { actor: 7, cell: (8, 9) });
    }

    public void ExecuteAnimate(GameController game, float t) {
      Debug.Assert(enabled && script != null);
       
      UpdateScriptGlobals(game);
      StartCoroutine(DoAnimationLoop());
    }
  }
}