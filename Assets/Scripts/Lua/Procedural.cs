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
		// public static void Hurt(Actor.Actor actor, float t) {
		// 	
		// }
		
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
