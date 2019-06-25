using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Advertisements
{
	internal sealed class UnityAdsInternal
	{
		public static event UnityAdsDelegate onCampaignsAvailable;

		public static event UnityAdsDelegate onCampaignsFetchFailed;

		public static event UnityAdsDelegate onShow;

		public static event UnityAdsDelegate onHide;

		public static event UnityAdsDelegate<string, bool> onVideoCompleted;

		public static event UnityAdsDelegate onVideoStarted;

		public static void RegisterNative() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void Init(string gameId, bool testModeEnabled, bool debugModeEnabled, string unityVersion) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool Show(string zoneId, string rewardItemKey, string options) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool CanShowAds(string zoneId) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetLogLevel(int logLevel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetCampaignDataURL(string url) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void RemoveAllEventHandlers()
		{
			UnityAdsInternal.onCampaignsAvailable = null;
			UnityAdsInternal.onCampaignsFetchFailed = null;
			UnityAdsInternal.onShow = null;
			UnityAdsInternal.onHide = null;
			UnityAdsInternal.onVideoCompleted = null;
			UnityAdsInternal.onVideoStarted = null;
		}

		public static void CallUnityAdsCampaignsAvailable()
		{
			UnityAdsInternal.onCampaignsAvailable?.Invoke();
		}

		public static void CallUnityAdsCampaignsFetchFailed()
		{
			UnityAdsInternal.onCampaignsFetchFailed?.Invoke();
		}

		public static void CallUnityAdsShow()
		{
			UnityAdsInternal.onShow?.Invoke();
		}

		public static void CallUnityAdsHide()
		{
			UnityAdsInternal.onHide?.Invoke();
		}

		public static void CallUnityAdsVideoCompleted(string rewardItemKey, bool skipped)
		{
			UnityAdsInternal.onVideoCompleted?.Invoke(rewardItemKey, skipped);
		}

		public static void CallUnityAdsVideoStarted()
		{
			UnityAdsInternal.onVideoStarted?.Invoke();
		}
	}
}
