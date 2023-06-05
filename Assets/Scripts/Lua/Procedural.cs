using MoonSharp.Interpreter;
using UnityEngine;

namespace Lua {
	[MoonSharpUserData]
	public class Procedural {
		public static float BoomerangCubic(float a, float b, float t) {
			// f(t) := saturate(1.0 - (2*t - 1)**2)
			float s = 2f*t - 1f;
			s *= s;
    
			return a + (b - a) * Mathf.Clamp01(1f - s);
		}

		// TODO:
		public static void Hurt(Actor.Actor actor, Vector3 originalPosition, float t) {
			// DEBUG:
			const float a1 = 0.07f;
			const float f1 = 5f;
			const float a2 = 0.05f;
			const float f2 = 7f;

			actor.View.transform.localPosition = new Vector3(
					originalPosition.x + a1 * Mathf.Sin(2f * Mathf.PI * f1 * t), 
					originalPosition.y + a2 * Mathf.Sin(2f * Mathf.PI * f2 * t));
		}
		
		// TODO:
		// public static void Heal(Actor.Actor actor, float t) {
		// 	
		// }
		
		// TODO:
		// public static void Death(Actor.Actor actor, float t) {
		// 	
		// }
	}
}
