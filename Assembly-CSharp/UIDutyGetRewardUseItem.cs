using KCV.Scene.Port;
using local.models;
using UnityEngine;

public class UIDutyGetRewardUseItem : MonoBehaviour
{
	[SerializeField]
	private UISprite mSprite_Icon;

	[SerializeField]
	private UILabel mLabel_Message;

	public void Initialize(Reward_Useitem reward)
	{
		mSprite_Icon.spriteName = $"item_{reward.Id}";
		if (reward.Id == 63)
		{
			mLabel_Message.text = string.Empty;
			mLabel_Message.fontSize = 24;
			mLabel_Message.text = string.Format("任務受託数がupしました！", reward.Name);
		}
		else if (reward.Count == 1)
		{
			mLabel_Message.text = string.Empty;
			mLabel_Message.fontSize = 32;
			mLabel_Message.text = $"{reward.Name}を\n入手しました";
		}
		else
		{
			mLabel_Message.text = string.Empty;
			mLabel_Message.fontSize = 32;
			mLabel_Message.text = $"{reward.Name}を\n{reward.Count}個 入手しました";
		}
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Icon);
		UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Message);
	}
}
