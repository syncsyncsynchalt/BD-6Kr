using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Random
	{
		public static int seed
		{
			get;
			set;
		}

		public static float value
		{
			get;
		}

		public static Vector3 insideUnitSphere
		{
			get
			{
				INTERNAL_get_insideUnitSphere(out Vector3 value);
				return value;
			}
		}

		public static Vector2 insideUnitCircle
		{
			get
			{
				GetRandomUnitCircle(out Vector2 output);
				return output;
			}
		}

		public static Vector3 onUnitSphere
		{
			get
			{
				INTERNAL_get_onUnitSphere(out Vector3 value);
				return value;
			}
		}

		public static Quaternion rotation
		{
			get
			{
				INTERNAL_get_rotation(out Quaternion value);
				return value;
			}
		}

		public static Quaternion rotationUniform
		{
			get
			{
				INTERNAL_get_rotationUniform(out Quaternion value);
				return value;
			}
		}

		public static float Range(float min, float max) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int Range(int min, int max)
		{
			return RandomRangeInt(min, max);
		}

		private static int RandomRangeInt(int min, int max) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_get_insideUnitSphere(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void GetRandomUnitCircle(out Vector2 output) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_get_onUnitSphere(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_get_rotation(out Quaternion value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_get_rotationUniform(out Quaternion value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Use Random.Range instead")]
		public static float RandomRange(float min, float max)
		{
			return Range(min, max);
		}

		[Obsolete("Use Random.Range instead")]
		public static int RandomRange(int min, int max)
		{
			return Range(min, max);
		}
	}
}
