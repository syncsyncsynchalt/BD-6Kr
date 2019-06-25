using KCV.Scene.Port;
using local.models;
using UnityEngine;

public class UIDutyGetTransportCraftRewardGet : MonoBehaviour
{
	[SerializeField]
	private UILabel mLabel_Message;

	public void Initialize(Reward_TransportCraft reward)
	{
		mLabel_Message.text = $"輸送船 x {reward.Num}";
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Message);
	}
}
