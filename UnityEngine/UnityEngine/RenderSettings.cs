using System;

using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
	public sealed class RenderSettings : Object
	{
		public static bool fog
		{
			get;
			set;
		}

		public static FogMode fogMode
		{
			get;
			set;
		}

		public static Color fogColor
		{
			get
			{
				INTERNAL_get_fogColor(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_fogColor(ref value);
			}
		}

		public static float fogDensity
		{
			get;
			set;
		}

		public static float fogStartDistance
		{
			get;
			set;
		}

		public static float fogEndDistance
		{
			get;
			set;
		}

		public static AmbientMode ambientMode
		{
			get;
			set;
		}

		public static Color ambientSkyColor
		{
			get
			{
				INTERNAL_get_ambientSkyColor(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_ambientSkyColor(ref value);
			}
		}

		public static Color ambientEquatorColor
		{
			get
			{
				INTERNAL_get_ambientEquatorColor(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_ambientEquatorColor(ref value);
			}
		}

		public static Color ambientGroundColor
		{
			get
			{
				INTERNAL_get_ambientGroundColor(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_ambientGroundColor(ref value);
			}
		}

		public static Color ambientLight
		{
			get
			{
				INTERNAL_get_ambientLight(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_ambientLight(ref value);
			}
		}

		public static float ambientIntensity
		{
			get;
			set;
		}

		public static SphericalHarmonicsL2 ambientProbe
		{
			get
			{
				INTERNAL_get_ambientProbe(out SphericalHarmonicsL2 value);
				return value;
			}
			set
			{
				INTERNAL_set_ambientProbe(ref value);
			}
		}

		public static float reflectionIntensity
		{
			get;
			set;
		}

		public static int reflectionBounces
		{
			get;
			set;
		}

		public static float haloStrength
		{
			get;
			set;
		}

		public static float flareStrength
		{
			get;
			set;
		}

		public static float flareFadeSpeed
		{
			get;
			set;
		}

		public static Material skybox
		{
			get;
			set;
		}

		public static DefaultReflectionMode defaultReflectionMode
		{
			get;
			set;
		}

		public static int defaultReflectionResolution
		{
			get;
			set;
		}

		public static Cubemap customReflection
		{
			get;
			set;
		}

		private static void INTERNAL_get_fogColor(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_set_fogColor(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_get_ambientSkyColor(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_set_ambientSkyColor(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_get_ambientEquatorColor(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_set_ambientEquatorColor(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_get_ambientGroundColor(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_set_ambientGroundColor(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_get_ambientLight(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_set_ambientLight(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_get_ambientProbe(out SphericalHarmonicsL2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_set_ambientProbe(ref SphericalHarmonicsL2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static void Reset() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static Object GetRenderSettings() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
