using DG.Tweening;
using KCV.Scene.Port;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicWindowFurniture : UIDynamicFurniture
	{
		[SerializeField]
		protected UITexture mTexture_WindowMain;

		[SerializeField]
		protected UITexture mTexture_WindowBackground;

		private int mOutPlaceTimeType = -1;

		private int[] WINDOW_OUTPLACE_GRAPYC_TYPE_MASTER = new int[39]
		{
			1,
			1,
			1,
			4,
			4,
			1,
			4,
			4,
			4,
			2,
			4,
			3,
			1,
			1,
			4,
			1,
			1,
			3,
			3,
			1,
			1,
			4,
			1,
			1,
			3,
			1,
			4,
			3,
			3,
			4,
			4,
			4,
			4,
			4,
			4,
			4,
			4,
			4,
			4
		};

		protected override void OnUpdate()
		{
			UpdateWindow();
		}

		protected void UpdateWindow()
		{
			OnUpdateWindow();
		}

		protected virtual void OnUpdateWindow()
		{
			if (mFurnitureModel != null)
			{
				int outPlaceTimeType = GetOutPlaceTimeType(mFurnitureModel.GetDateTime().Hour);
				if (mOutPlaceTimeType != outPlaceTimeType)
				{
					mOutPlaceTimeType = outPlaceTimeType;
					StartCoroutine(UpdateWindowCoroutine());
				}
			}
		}

		private IEnumerator UpdateWindowCoroutine()
		{
			UITexture prevTexture = UnityEngine.Object.Instantiate(mTexture_WindowBackground);
			prevTexture.transform.parent = mTexture_WindowBackground.transform.parent;
			prevTexture.transform.localScale = mTexture_WindowBackground.transform.localScale;
			prevTexture.transform.localPosition = mTexture_WindowBackground.transform.localPosition;
			prevTexture.depth++;
			int graphicType = GetOutPlaceGraphicType(mFurnitureModel.GetFurnitureModel());
			mTexture_WindowBackground.mainTexture = RequestOutPlaceTexture(graphicType, mOutPlaceTimeType);
			if (DOTween.IsTweening(mTexture_WindowBackground))
			{
				DOTween.Kill(mTexture_WindowBackground, complete: true);
			}
			yield return new WaitForEndOfFrame();
			DOVirtual.Float(prevTexture.alpha, 0f, 10f, delegate(float alpha)
			{
                prevTexture.alpha = alpha;
			}).OnComplete(delegate
			{
				UserInterfacePortManager.ReleaseUtils.Release(ref prevTexture);
				UnityEngine.Object.Destroy(prevTexture.gameObject);
			}).SetId(mTexture_WindowBackground);
		}

		protected override void OnInitialize(UIFurnitureModel uiFurnitureModel)
		{
			FurnitureModel furnitureModel = uiFurnitureModel.GetFurnitureModel();
			ChangeOffset(furnitureModel);
			DateTime dateTime = uiFurnitureModel.GetDateTime();
			int outPlaceGraphicType = GetOutPlaceGraphicType(furnitureModel);
			mOutPlaceTimeType = GetOutPlaceTimeType(dateTime.Hour);
			Texture mainTexture = UIFurniture.LoadTexture(furnitureModel);
			Texture mainTexture2 = RequestOutPlaceTexture(outPlaceGraphicType, mOutPlaceTimeType);
			mTexture_WindowMain.mainTexture = mainTexture;
			mTexture_WindowBackground.mainTexture = mainTexture2;
		}

		private void ChangeOffset(FurnitureModel furnitureModel)
		{
			switch (furnitureModel.NoInType + 1)
			{
			case 1:
			case 2:
			case 3:
			case 4:
			case 6:
				base.transform.localPosition = new Vector3(25f, 12f, 0f);
				break;
			case 10:
				base.transform.localPosition = new Vector3(0f, 18f, 0f);
				break;
			case 12:
				base.transform.localPosition = new Vector3(0f, 20f, 0f);
				break;
			case 13:
				base.transform.localPosition = new Vector3(24f, 30f, 0f);
				break;
			case 16:
				base.transform.localPosition = new Vector3(38f, 35.5f, 0f);
				break;
			case 18:
				base.transform.localPosition = new Vector3(25f, 14f, 0f);
				break;
			case 25:
				base.transform.localPosition = new Vector3(0f, 14f, 0f);
				break;
			case 28:
				base.transform.localPosition = new Vector3(10f, 15f, 0f);
				break;
			case 30:
				base.transform.localPosition = new Vector3(0f, 0f, 0f);
				mTexture_WindowBackground.transform.localPosition = new Vector3(0f, 35f, 0f);
				break;
			case 33:
				base.transform.localPosition = new Vector3(18f, 15f, 0f);
				break;
			case 34:
				base.transform.localPosition = new Vector3(28f, 22f, 0f);
				break;
			case 37:
				base.transform.localPosition = new Vector3(12f, 8f, 0f);
				break;
			default:
				base.transform.localPosition = new Vector3(0f, 0f, 0f);
				break;
			}
		}

		protected int GetOutPlaceGraphicType(FurnitureModel windowFurnitureModel)
		{
			return WINDOW_OUTPLACE_GRAPYC_TYPE_MASTER[windowFurnitureModel.NoInType];
		}

		protected Texture RequestOutPlaceTexture(int graphicType, int timeType)
		{
			string text = "window_bg_" + graphicType + "-" + timeType;
			string path = "Textures/Furnitures/Capiz/" + graphicType + "/" + text;
			return Resources.Load(path) as Texture;
		}

		protected int GetOutPlaceTimeType(int hour24)
		{
			if (4 <= hour24 && hour24 < 8)
			{
				return 5;
			}
			if (8 <= hour24 && hour24 < 16)
			{
				return 1;
			}
			if (16 <= hour24 && hour24 < 18)
			{
				return 2;
			}
			if (18 <= hour24 && hour24 < 20)
			{
				return 3;
			}
			if (20 <= hour24 || hour24 < 4)
			{
				return 4;
			}
			return -1;
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			if (DOTween.IsTweening(mTexture_WindowBackground))
			{
				DOTween.Kill(mTexture_WindowBackground);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_WindowMain);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_WindowBackground);
		}
	}
}
