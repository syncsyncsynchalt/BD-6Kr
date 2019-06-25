using KCV.Scene.Port;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicDeskFurnitureInflatablePool : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_Pool;

		[SerializeField]
		private Texture mTexture2d_Pool_On;

		[SerializeField]
		private Texture mTexture2d_Pool_Off;

		protected override void OnInitialize(UIFurnitureModel uiFurnitureModel)
		{
			InitializePool(uiFurnitureModel.GetDateTime());
		}

		private void InitializePool(DateTime dateTime)
		{
			if (10 <= dateTime.Hour && dateTime.Hour < 20)
			{
				mTexture_Pool.mainTexture = mTexture2d_Pool_On;
			}
			else
			{
				mTexture_Pool.mainTexture = mTexture2d_Pool_Off;
			}
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Pool);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Pool_On);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Pool_Off);
		}
	}
}
