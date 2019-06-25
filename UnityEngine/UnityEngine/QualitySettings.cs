using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class QualitySettings : Object
	{
		public static string[] names
		{
			get;
		}

		[Obsolete("Use GetQualityLevel and SetQualityLevel")]
		public static QualityLevel currentLevel
		{
			get;
			set;
		}

		public static int pixelLightCount
		{
			get;
			set;
		}

		public static ShadowProjection shadowProjection
		{
			get;
			set;
		}

		public static int shadowCascades
		{
			get;
			set;
		}

		public static float shadowDistance
		{
			get;
			set;
		}

		public static float shadowNearPlaneOffset
		{
			get;
			set;
		}

		public static float shadowCascade2Split
		{
			get;
			set;
		}

		public static Vector3 shadowCascade4Split
		{
			get
			{
				INTERNAL_get_shadowCascade4Split(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_shadowCascade4Split(ref value);
			}
		}

		public static int masterTextureLimit
		{
			get;
			set;
		}

		public static AnisotropicFiltering anisotropicFiltering
		{
			get;
			set;
		}

		public static float lodBias
		{
			get;
			set;
		}

		public static int maximumLODLevel
		{
			get;
			set;
		}

		public static int particleRaycastBudget
		{
			get;
			set;
		}

		public static bool softVegetation
		{
			get;
			set;
		}

		public static bool realtimeReflectionProbes
		{
			get;
			set;
		}

		public static bool billboardsFaceCameraPosition
		{
			get;
			set;
		}

		public static int maxQueuedFrames
		{
			get;
			set;
		}

		public static int vSyncCount
		{
			get;
			set;
		}

		public static int antiAliasing
		{
			get;
			set;
		}

		public static ColorSpace desiredColorSpace
		{
			get;
		}

		public static ColorSpace activeColorSpace
		{
			get;
		}

		public static BlendWeights blendWeights
		{
			get;
			set;
		}

		public static int GetQualityLevel() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetQualityLevel(int index, [DefaultValue("true")] bool applyExpensiveChanges) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void SetQualityLevel(int index)
		{
			bool applyExpensiveChanges = true;
			SetQualityLevel(index, applyExpensiveChanges);
		}

		public static void IncreaseLevel([DefaultValue("false")] bool applyExpensiveChanges) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void IncreaseLevel()
		{
			bool applyExpensiveChanges = false;
			IncreaseLevel(applyExpensiveChanges);
		}

		public static void DecreaseLevel([DefaultValue("false")] bool applyExpensiveChanges) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void DecreaseLevel()
		{
			bool applyExpensiveChanges = false;
			DecreaseLevel(applyExpensiveChanges);
		}

		private static void INTERNAL_get_shadowCascade4Split(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_set_shadowCascade4Split(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
