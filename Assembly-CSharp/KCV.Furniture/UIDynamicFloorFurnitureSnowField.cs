using KCV.Scene.Port;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicFloorFurnitureSnowField : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_TheWorks;

		[SerializeField]
		private Texture mTexture2d_TypeA;

		[SerializeField]
		private Texture mTexture2d_TypeB;

		[SerializeField]
		private Texture mTexture2d_TypeC;

		protected override void OnInitialize(UIFurnitureModel uiFurnitureModel)
		{
			base.OnInitialize(uiFurnitureModel);
			InitializeTheWorks(uiFurnitureModel.GetDateTime());
		}

		private void InitializeTheWorks(DateTime dateTime)
		{
			if (4 <= dateTime.Hour && dateTime.Hour < 10)
			{
				mTexture_TheWorks.mainTexture = mTexture2d_TypeA;
				mTexture_TheWorks.SetDimensions(135, 90);
			}
			else if (10 <= dateTime.Hour && dateTime.Hour < 16)
			{
				mTexture_TheWorks.mainTexture = mTexture2d_TypeB;
				mTexture_TheWorks.SetDimensions(158, 125);
			}
			else if (16 <= dateTime.Hour && dateTime.Hour < 22)
			{
				mTexture_TheWorks.mainTexture = null;
				mTexture_TheWorks.SetDimensions(0, 0);
			}
			else if (22 <= dateTime.Hour || dateTime.Hour < 4)
			{
				mTexture_TheWorks.mainTexture = mTexture2d_TypeC;
				mTexture_TheWorks.SetDimensions(132, 89);
			}
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_TheWorks);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_TypeA);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_TypeB);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_TypeC);
		}
	}
}
