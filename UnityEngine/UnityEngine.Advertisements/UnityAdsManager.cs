using System;

using System.Runtime.CompilerServices;

namespace UnityEngine.Advertisements
{
	internal class UnityAdsManager
	{
		public static bool enabled
		{
			get;
			set;
		}

		public static bool initializeOnStartup
		{
			get;
			set;
		}

		public static bool testMode
		{
			get;
			set;
		}

		public static bool IsPlatformEnabled(RuntimePlatform platform) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetPlatformEnabled(RuntimePlatform platform, bool value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static string GetGameId(RuntimePlatform platform) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetGameId(RuntimePlatform platform, string gameId) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
