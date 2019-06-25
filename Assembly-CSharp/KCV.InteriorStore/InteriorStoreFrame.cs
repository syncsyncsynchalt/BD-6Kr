using local.managers;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class InteriorStoreFrame : MonoBehaviour
	{
		[SerializeField]
		private UILabel FCoin;

		[SerializeField]
		private UILabel Worker;

		[SerializeField]
		private UIButton GoHomeButton;

		public void updateUserInfo(FurnitureStoreManager manager)
		{
			FCoin.textInt = manager.UserInfo.FCoin;
			Worker.textInt = manager.GetWorkerCount();
		}

		public void setGoHomeButtonEnable(bool isEnable)
		{
			GoHomeButton.isEnabled = isEnable;
		}
	}
}
