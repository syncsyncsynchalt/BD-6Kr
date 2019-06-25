using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class ProceduralMaterial : Material
	{
		public static bool isSupported
		{
			get;
		}

		internal ProceduralMaterial()
			: base((Material)null)
		{
		}

		public static void StopRebuilds() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
