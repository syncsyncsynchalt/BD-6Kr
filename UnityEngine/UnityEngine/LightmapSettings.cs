using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class LightmapSettings : Object
	{
		public static LightmapData[] lightmaps
		{
			get;
			set;
		}

		[Obsolete("Use lightmapsMode property")]
		public static LightmapsModeLegacy lightmapsModeLegacy
		{
			get;
			set;
		}

		public static LightmapsMode lightmapsMode
		{
			get;
			set;
		}

		[Obsolete("bakedColorSpace is no longer valid. Use QualitySettings.desiredColorSpace.", false)]
		public static ColorSpace bakedColorSpace
		{
			get
			{
				return QualitySettings.desiredColorSpace;
			}
			set
			{
			}
		}

		public static LightProbes lightProbes
		{
			get;
			set;
		}

		internal static void Reset() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
