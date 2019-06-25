using KCV.Scene.Port;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicHangingsFurnitureMusashiMemorial : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_MusashiRoll;

		[SerializeField]
		private Texture mTexture2d_TypeA;

		[SerializeField]
		private Texture mTexture2d_TypeB;

		[SerializeField]
		private ParticleSystem mParticleSystem_Petal;

		protected override void OnAwake()
		{
			mParticleSystem_Petal.Stop();
		}

		protected override void OnInitialize(UIFurnitureModel uiFurnitureModel)
		{
			InitializeMusashiRoll(uiFurnitureModel.GetDateTime());
		}

		private void InitializeMusashiRoll(DateTime dateTime)
		{
			if (dateTime.Hour == 0 || dateTime.Hour % 2 == 0)
			{
				mTexture_MusashiRoll.mainTexture = mTexture2d_TypeA;
			}
			else
			{
				mTexture_MusashiRoll.mainTexture = mTexture2d_TypeB;
			}
			mParticleSystem_Petal.Play();
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_MusashiRoll);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_TypeA);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_TypeB);
			UserInterfacePortManager.ReleaseUtils.Release(ref mParticleSystem_Petal);
		}
	}
}
