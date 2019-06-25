using KCV.Scene.Port;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicChestFurnitureDaruma : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_Daruma;

		private Vector3 mVector3_DefaultPosition;

		protected override void OnAwake()
		{
			mVector3_DefaultPosition = mTexture_Daruma.transform.localPosition;
		}

		protected override void OnCalledActionEvent()
		{
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Daruma);
		}
	}
}
