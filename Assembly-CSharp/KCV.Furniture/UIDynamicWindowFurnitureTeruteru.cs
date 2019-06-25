using DG.Tweening;
using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicWindowFurnitureTeruteru : UIDynamicWindowFurniture
	{
		[SerializeField]
		private Texture[] mTexture2ds_Teruteru;

		[SerializeField]
		private UITexture mTexture_Teruteru;

		protected override void OnUpdate()
		{
			UpdateWindow();
		}

		protected override void OnInitialize(UIFurnitureModel uiFurnitureModel)
		{
			base.OnInitialize(uiFurnitureModel);
			FurnitureModel furnitureModel = uiFurnitureModel.GetFurnitureModel();
			DateTime dateTime = uiFurnitureModel.GetDateTime();
			int outPlaceGraphicType = GetOutPlaceGraphicType(furnitureModel);
			int outPlaceTimeType = GetOutPlaceTimeType(dateTime.Hour);
			Texture mainTexture = RequestOutPlaceTexture(outPlaceGraphicType, outPlaceTimeType);
			mTexture_WindowBackground.mainTexture = mainTexture;
			mTexture_Teruteru.mainTexture = mTexture2ds_Teruteru[2];
		}

		protected override void OnCalledActionEvent()
		{
			Animation();
		}

		private void Animation()
		{
			if (!DOTween.IsTweening(this))
			{
				int[] array = new int[13]
				{
					2,
					1,
					0,
					1,
					2,
					3,
					4,
					3,
					2,
					1,
					2,
					3,
					2
				};
				Sequence sequence = DOTween.Sequence();
				sequence.SetId(this);
				int[] array2 = array;
				foreach (int num in array2)
				{
					int index = num;
					TweenCallback callback = delegate
					{
						mTexture_Teruteru.mainTexture = mTexture2ds_Teruteru[index];
					};
					sequence.AppendCallback(callback);
					sequence.AppendInterval(0.2f);
				}
			}
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Releases(ref mTexture2ds_Teruteru);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Teruteru);
		}
	}
}
