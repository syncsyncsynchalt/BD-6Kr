using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
	public sealed class LightProbes : Object
	{
		public Vector3[] positions
		{
			get;
		}

		public SphericalHarmonicsL2[] bakedProbes
		{
			get;
			set;
		}

		public int count
		{
			get;
		}

		public int cellCount
		{
			get;
		}

		[Obsolete("coefficients property has been deprecated. Please use bakedProbes instead.", true)]
		public float[] coefficients
		{
			get
			{
				return new float[0];
			}
			set
			{
			}
		}

		public static void GetInterpolatedProbe(Vector3 position, Renderer renderer, out SphericalHarmonicsL2 probe)
		{
			INTERNAL_CALL_GetInterpolatedProbe(ref position, renderer, out probe);
		}

		private static void INTERNAL_CALL_GetInterpolatedProbe(ref Vector3 position, Renderer renderer, out SphericalHarmonicsL2 probe) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("GetInterpolatedLightProbe has been deprecated. Please use the static GetInterpolatedProbe instead.", true)]
		public void GetInterpolatedLightProbe(Vector3 position, Renderer renderer, float[] coefficients)
		{
		}
	}
}
