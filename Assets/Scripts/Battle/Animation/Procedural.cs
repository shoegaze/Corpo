using MoonSharp.Interpreter;
using UnityEngine;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace Battle.Animation {
	[MoonSharpUserData]
	public class Procedural {
		public static float BoomerangCubic(float a, float b, float t) {
			// f(t) := saturate(1.0 - (2*t - 1)**2)
			float s = 2f*t - 1f;
			s *= s;
    
			return a + (b - a) * Mathf.Clamp01(1f - s);
		}

		public static void Attack(Actor.Actor actor, Vector3 from, Vector3 to, float t) {
			actor.View.transform.localPosition = new Vector3(
							BoomerangCubic(from.x, to.x, t),
							BoomerangCubic(from.y, to.y, t));
		}

		public static void Hurt(Actor.Actor actor, Vector3 from, float t) {
			// DEBUG:
			const float a1 = 0.07f;
			const float f1 = 5f;
			const float a2 = 0.05f;
			const float f2 = 7f;

			actor.View.transform.localPosition = new Vector3(
					from.x + a1 * Mathf.Sin(2f * Mathf.PI * f1 * t), 
					from.y + a2 * Mathf.Sin(2f * Mathf.PI * f2 * t));
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
