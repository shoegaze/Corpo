using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using UnityEngine;
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Lua {
	[MoonSharpUserData]
	public class Math {
		public static Vector3 ToVector3(Vector2Int v2I) {
			return new Vector3(v2I.x, v2I.y, 0f);
		}
		
		public static float Min(float a, float b) {
			return Mathf.Min(a, b);
		}

		public static float Max(float a, float b) {
			return Mathf.Max(a, b);
		}
		
		public static float Clamp(float v, float min, float max) {
			return Mathf.Clamp(v, min, max);
		}

		public static float Saturate(float v) {
			return Clamp(v, 0f, 1f);
		}

		// https://stackoverflow.com/a/3451607
		public static float Map(float v, float minFrom, float maxFrom, float minTo, float maxTo) {
			return minTo + (v - minFrom) * (maxTo - minTo) / (minFrom - maxFrom);
		}

		public static float Lerp(float min, float max, float t) {
			return (1f - t) * min + t * max;
		}

		public static float LerpClamped(float min, float max, float t) {
			return Lerp(min, max, Saturate(t));
		}
	}
}
