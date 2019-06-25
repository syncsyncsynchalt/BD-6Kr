using KCV.Scene.Port;
using local.models;
using UnityEngine;

namespace KCV.Scene.Duty.Reward
{
	public class UIDutyGetRewardSlotItem : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTexture_SlotItem;

		[SerializeField]
		private UILabel mLabel_Name;

		public void Initialize(Reward_Slotitem reward_Slotitem)
		{
			mTexture_SlotItem.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(reward_Slotitem.Id, 1);
			mLabel_Name.text = reward_Slotitem.Name;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_SlotItem);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Name);
		}
	}
}
