using KCV.Scene.Port;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicFloorFurnitureSandBeach : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_TheWorks;

		[SerializeField]
		private Texture mTexture2d_TypeA;

		[SerializeField]
		private Texture mTexture2d_TypeB;

		protected override void OnInitialize(UIFurnitureModel uiFurnitureModel)
		{
			DateTime dateTime = mFurnitureModel.GetDateTime();
			InitializeTheWorks(dateTime);
		}

		private void InitializeTheWorks(DateTime dateTime)
		{
			if (6 <= dateTime.Hour && dateTime.Hour < 18)
			{
				mTexture_TheWorks.mainTexture = mTexture2d_TypeA;
				mTexture_TheWorks.SetDimensions(147, 77);
			}
			else
			{
				mTexture_TheWorks.mainTexture = mTexture2d_TypeB;
				mTexture_TheWorks.SetDimensions(171, 103);
			}
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_TheWorks);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_TypeA);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_TypeB);
		}
	}
}
