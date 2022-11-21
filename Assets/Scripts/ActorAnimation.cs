using System.Collections;
using Battle;
using UnityEngine;

public class ActorAnimation : MonoBehaviour {
  [SerializeField, Min(0f)] private float attackDuration;
  
  public void StartAttack(AttackContext ctx) {
    StartCoroutine(DoAttack(ctx));
  }

  // private float BoomerangLerp(float a, float b, float t) {
  //   // f(t) := saturate(1.0 - 2 * abs(t - 0.5))
  //   return a + (b - a) * Mathf.Clamp01(1f - 2f  * Mathf.Abs(t - 0.5f));
  // }

  private float BoomerangCubic(float a, float b, float t) {
    // f(t) := saturate(1.0 - (2*t - 1)**2)
    float s = 2f*t - 1f;
    s *= s;
    
    return a + (b - a) * Mathf.Clamp01(1f - s);
  }
  
  private IEnumerator DoAttack(AttackContext ctx) {
    var source = new Vector3(ctx.From.x, ctx.From.y);
    var dir = ctx.To - ctx.From;
    var destination = source + new Vector3(dir.x, dir.y).normalized / 2f;
    
    var startTime = Time.time;
    // Interpolate to target, then back to source
    while (Time.time < startTime + attackDuration) {
      var t = (Time.time - startTime) / attackDuration;

      transform.localPosition = new Vector3(
        BoomerangCubic(source.x, destination.x, t),
        BoomerangCubic(source.y, destination.y, t)
      );
      
      yield return null;
    }
   
    transform.localPosition = source;
  }
}
