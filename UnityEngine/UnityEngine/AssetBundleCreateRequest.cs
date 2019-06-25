using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AssetBundleCreateRequest : AsyncOperation
	{
		public AssetBundle assetBundle
		{
			get;
		}

		internal void DisableCompatibilityChecks() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
