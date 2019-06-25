using DG.Tweening;
using KCV.Scene.Port;
using KCV.View.ScrollView;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	[RequireComponent(typeof(UIWidget))]
	public class UIRemodeModernizationShipTargetListChildNew : MonoBehaviour, UIScrollListItem<RemodeModernizationShipTargetListChildNew, UIRemodeModernizationShipTargetListChildNew>
	{
		[SerializeField]
		private CommonShipBanner mCommonShipBanner;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UISprite mSprite_Karyoku;

		[SerializeField]
		private UISprite mSprite_Raisou;

		[SerializeField]
		private UISprite mSprite_Soukou;

		[SerializeField]
		private UISprite mSprite_Taikuu;

		[SerializeField]
		private UISprite mSprite_Luck;

		[SerializeField]
		private Transform mLEVEL;

		[SerializeField]
		private Transform mUNSET;

		[SerializeField]
		private UISprite mListBar;

		private UIWidget mWidgetThis;

		private int mRealIndex;

		private RemodeModernizationShipTargetListChildNew mModel;

		private Action<UIRemodeModernizationShipTargetListChildNew> mOnTouchListener;

		private Transform mCachedTransform;

		private Action<CommonShipBanner> mReleaseRequestShipTexture;

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
		}

		public void Initialize(int realIndex, RemodeModernizationShipTargetListChildNew model)
		{
			UITexture uITexture = mCommonShipBanner.GetUITexture();
			Texture releaseRequestTexture = uITexture.mainTexture;
			uITexture.mainTexture = null;
			UserInterfaceRemodelManager.instance.ReleaseRequestBanner(ref releaseRequestTexture);
			mWidgetThis.alpha = 1f;
			if (model.mOption == RemodeModernizationShipTargetListChildNew.ListItemOption.UnSet)
			{
				mRealIndex = realIndex;
				mModel = null;
				mCommonShipBanner.SetActive(isActive: false);
				mSprite_Karyoku.alpha = 0f;
				mSprite_Raisou.alpha = 0f;
				mSprite_Soukou.alpha = 0f;
				mSprite_Taikuu.alpha = 0f;
				mSprite_Luck.alpha = 0f;
				mLabel_Level.alpha = 0f;
				mLabel_Level.text = "はずす";
				mLEVEL.localScale = Vector3.zero;
				mUNSET.localScale = Vector3.one;
				return;
			}
			mRealIndex = realIndex;
			mModel = model;
			mCommonShipBanner.SetActive(isActive: true);
			mSprite_Karyoku.alpha = 1f;
			mSprite_Raisou.alpha = 1f;
			mSprite_Soukou.alpha = 1f;
			mSprite_Taikuu.alpha = 1f;
			mSprite_Luck.alpha = 1f;
			mLabel_Level.alpha = 1f;
			mLEVEL.localScale = Vector3.one;
			mUNSET.localScale = Vector3.zero;
			if (0 < model.mShipModel.PowUpKaryoku)
			{
				mSprite_Karyoku.spriteName = "icon_1_on";
			}
			else
			{
				mSprite_Karyoku.spriteName = "icon_1";
			}
			if (0 < model.mShipModel.PowUpRaisou)
			{
				mSprite_Raisou.spriteName = "icon_2_on";
			}
			else
			{
				mSprite_Raisou.spriteName = "icon_2";
			}
			if (0 < model.mShipModel.PowUpSoukou)
			{
				mSprite_Soukou.spriteName = "icon_3_on";
			}
			else
			{
				mSprite_Soukou.spriteName = "icon_3";
			}
			if (0 < model.mShipModel.PowUpTaikuu)
			{
				mSprite_Taikuu.spriteName = "icon_4_on";
			}
			else
			{
				mSprite_Taikuu.spriteName = "icon_4";
			}
			if (0 < model.mShipModel.PowUpLucky)
			{
				mSprite_Luck.spriteName = "icon_5_on";
			}
			else
			{
				mSprite_Luck.spriteName = "icon_5";
			}
			mCommonShipBanner.SetShipData(model.mShipModel);
			mCommonShipBanner.GetUITexture().alpha = 1f;
			mCommonShipBanner.StopParticle();
			mLabel_Level.text = model.mShipModel.Level.ToString();
		}

		public void InitializeDefault(int realIndex)
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			UITexture uITexture = mCommonShipBanner.GetUITexture();
			Texture releaseRequestTexture = uITexture.mainTexture;
			uITexture.mainTexture = null;
			UserInterfaceRemodelManager.instance.ReleaseRequestBanner(ref releaseRequestTexture);
			mModel = null;
			mRealIndex = realIndex;
			base.name = "[" + realIndex + "]";
			mWidgetThis.alpha = 1E-09f;
		}

		public int GetRealIndex()
		{
			return mRealIndex;
		}

		public RemodeModernizationShipTargetListChildNew GetModel()
		{
			return mModel;
		}

		public int GetHeight()
		{
			return 58;
		}

		public void SetOnTouchListener(Action<UIRemodeModernizationShipTargetListChildNew> onTouchListener)
		{
			mOnTouchListener = onTouchListener;
		}

		private void OnTouchEvent()
		{
			if (mOnTouchListener != null)
			{
				mOnTouchListener(this);
			}
		}

		public void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(mListBar.gameObject, value: true);
		}

		public Transform GetTransform()
		{
			if (mCachedTransform == null)
			{
				mCachedTransform = base.transform;
			}
			return mCachedTransform;
		}

		public void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(mListBar.gameObject, value: false);
		}

		public void OnClick()
		{
			OnTouchEvent();
		}

		public CommonShipBanner GetShipBanner()
		{
			return mCommonShipBanner;
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Level);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Karyoku);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Raisou);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Soukou);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Taikuu);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Luck);
			UserInterfacePortManager.ReleaseUtils.Release(ref mListBar);
			UserInterfacePortManager.ReleaseUtils.Release(ref mWidgetThis);
			UserInterfacePortManager.ReleaseUtils.Release(ref mCommonShipBanner, unloadAsset: true);
			mLEVEL = null;
			mUNSET = null;
			mModel = null;
			mOnTouchListener = null;
			mCachedTransform = null;
			mReleaseRequestShipTexture = null;
		}
	}
}
