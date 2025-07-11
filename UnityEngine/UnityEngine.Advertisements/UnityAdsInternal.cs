using System.Runtime.CompilerServices;

namespace UnityEngine.Advertisements;

internal sealed class UnityAdsInternal
{
	public static event UnityAdsDelegate onCampaignsAvailable;

	public static event UnityAdsDelegate onCampaignsFetchFailed;

	public static event UnityAdsDelegate onShow;

	public static event UnityAdsDelegate onHide;

	public static event UnityAdsDelegate<string, bool> onVideoCompleted;

	public static event UnityAdsDelegate onVideoStarted;

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void RegisterNative();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void Init(string gameId, bool testModeEnabled, bool debugModeEnabled, string unityVersion);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool Show(string zoneId, string rewardItemKey, string options);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool CanShowAds(string zoneId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetLogLevel(int logLevel);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetCampaignDataURL(string url);

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
