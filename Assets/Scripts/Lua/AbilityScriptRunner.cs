using Data;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Lua {
  public class AbilityScriptRunner {
     private readonly Script script = new Script(CoreModules.Preset_SoftSandbox);
     private bool enabled = false;
     // TODO: Replace object types with Func<...>
     private object scriptFnStart;
     private object scriptFnGetCandidateCells;
     private object scriptFnBeforeAnimation;
     private object scriptFnAfterAnimation;
     private object scriptFnAnimate;
     
     // TODO: Return bool based on validation
     public void Load(AbilityScript abilityScript) {
       // TODO: Validate script and just load string
       script.DoString(abilityScript.RawScript);

       scriptFnStart = script.Globals["start"];
       scriptFnGetCandidateCells = script.Globals["get_candidate_cells"];
       scriptFnBeforeAnimation = script.Globals["before_animation"];
       scriptFnAfterAnimation = script.Globals["after_animation"];
       scriptFnAnimate = script.Globals["animate"];

       enabled = true;
     }

     public void Disable() {
       enabled = false;
     }

     private void UpdateScriptGlobals(GameController game) {
       // TODO
       // script.Globals["duration"] = ...;
       // script.Globals["grid"] = ...;
     }

     // TODO: Add error handling for Execute_ methods
     public void ExecuteStart(GameController game) {
       Debug.Assert(enabled && script != null);
       
       UpdateScriptGlobals(game);
       script.Call(scriptFnStart);
     }
     
     public void ExecuteGetCandidateCells(GameController game) {
       Debug.Assert(enabled && script != null);
       
       UpdateScriptGlobals(game);
       // var candidates = script.Call(scriptFnGetCandidateCells, { actor: 1, cell: (2, 3) });
     }

     public void ExecuteBeforeAnimation(GameController game) {
       Debug.Assert(enabled && script != null);
       
       UpdateScriptGlobals(game);
       // script.Call(scriptFnBeforeAnimation, { actor: 4, cell: (5, 6) });
     }

     public void ExecuteAfterAnimation(GameController game) {
       Debug.Assert(enabled && script != null);
       
       UpdateScriptGlobals(game);
       // script.Call(scriptFnAfterAnimation, { actor: 7, cell: (8, 9) });
     }

     public void ExecuteAnimate(GameController game, float t) {
       Debug.Assert(enabled && script != null);
       
       UpdateScriptGlobals(game);
       // script.Call(
       //         scriptFnAnimate,
       //         t,
       //         { actor: 10, cell: (11, 12) },
       //         { actor: 13, cell: (14, 15) }
       // );
     }
   }
}