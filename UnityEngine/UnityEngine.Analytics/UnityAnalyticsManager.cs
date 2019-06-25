using System.Runtime.CompilerServices;

namespace UnityEngine.Analytics
{
	internal class UnityAnalyticsManager
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

		public static string testEventUrl
		{
			get;
			set;
		}

		public static string testConfigUrl
		{
			get;
			set;
		}

		public static string unityAdsId
		{
			get;
		}

		public static bool unityAdsTrackingEnabled
		{
			get;
		}

		public static string deviceUniqueIdentifier
		{
			get;
		}
	}
}
