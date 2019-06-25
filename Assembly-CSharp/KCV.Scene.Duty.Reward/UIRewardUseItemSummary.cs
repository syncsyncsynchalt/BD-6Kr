using KCV.Scene.Port;
using KCV.View;
using local.models;
using UnityEngine;

namespace KCV.Scene.Duty.Reward
{
	public class UIRewardUseItemSummary : BaseUISummary<Reward_Useitem>
	{
		[SerializeField]
		private UILabel mLabelName;

		[SerializeField]
		private UILabel mLabelValue;

		[SerializeField]
		private UISprite mSpriteIcon;

		public override void Initialize(int index, Reward_Useitem model)
		{
			base.Initialize(index, model);
			mLabelName.text = model.Name;
			mLabelValue.text = model.Count.ToString();
			mSpriteIcon.spriteName = $"item_{model.Id}";
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelName);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSpriteIcon);
		}
	}
}
