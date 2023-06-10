using System.Collections;
using Battle;
using UnityEngine;

namespace Actor {
  public class ActorAnimation : MonoBehaviour {
    [SerializeField, Min(0f)] private float attackDuration;
    [SerializeField, Min(0f)] private float hurtDuration;

    private bool IsPlaying { get; set; }
  
    // private static float BoomerangLerp(float a, float b, float t) {
    //   // f(t) := saturate(1.0 - 2 * abs(t - 0.5))
    //   return a + (b - a) * Mathf.Clamp01(1f - 2f  * Mathf.Abs(t - 0.5f));
    // }

    public void StartAttack(AttackContext ctx) {
      StartCoroutine(DoAttack(ctx));
    }

    public void StartHurt(AttackContext ctx) {
      StartCoroutine(DoHurt(ctx));
    }
  
    private IEnumerator DoAttack(AttackContext ctx) {
      IsPlaying = true;
    
      var source = new Vector3(ctx.From.x, ctx.From.y);
      var dir = ctx.To - ctx.From;
      var destination = source + new Vector3(dir.x, dir.y).normalized / 2f;
      float startTime = Time.time;
    
      // Interpolate to target, then back to source
      while (Time.time < startTime + attackDuration) {
        float t = (Time.time - startTime) / attackDuration;

        transform.localPosition = new Vector3(
                Battle.Animation.Procedural.BoomerangCubic(source.x, destination.x, t),
                Battle.Animation.Procedural.BoomerangCubic(source.y, destination.y, t)
            );
      
        yield return null;
      }
   
      transform.localPosition = source;

      IsPlaying = false;
    }

    private IEnumerator DoHurt(AttackContext ctx) {
      IsPlaying = true;

      var source = new Vector3(ctx.To.x, ctx.To.y);
      var startTime = Time.time;
    
      while (Time.time < startTime + hurtDuration) {
        var t = (Time.time - startTime) / hurtDuration;
      
        // DEBUG:
        const float a1 = 0.07f;
        const float f1 = 5f;
        const float a2 = 0.05f;
        const float f2 = 7f;
      
        transform.localPosition = new Vector3(
                source.x + a1 * Mathf.Sin(2f * Mathf.PI * f1 * t),
                source.y + a2 * Mathf.Sin(2f * Mathf.PI * f2 * t)
            );

        yield return null;
      }

      transform.localPosition = source;

      IsPlaying = false;
    }
  }
}
