using DG.Tweening;
using KCV;
using KCV.Remodel;
using KCV.Scene.Port;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public class UIRemodelOtherShipPickerChildNew : MonoBehaviour, UIScrollListItem<ShipModel, UIRemodelOtherShipPickerChildNew>
{
	private enum AnimationType
	{
		Show
	}

	private UIWidget mWidgetThis;

	[SerializeField]
	private UILabel shipName;

	[SerializeField]
	private UILabel shipLevel;

	[SerializeField]
	private UiStarManager stars;

	[SerializeField]
	private CommonShipBanner CommonShipBanner;

	[SerializeField]
	private Transform mTransform_Background;

	private int mRealIndex;

	private ShipModel mShipModel;

	private Transform mTransformCache;

	private Action<UIRemodelOtherShipPickerChildNew> mOnTouchListener;

	private void Awake()
	{
		mWidgetThis = GetComponent<UIWidget>();
		mWidgetThis.alpha = 1E-07f;
	}

	public CommonShipBanner GetBanner()
	{
		return CommonShipBanner;
	}

	public void Initialize(int realIndex, ShipModel model)
	{
		UITexture uITexture = CommonShipBanner.GetUITexture();
		Texture releaseRequestTexture = uITexture.mainTexture;
		uITexture.mainTexture = null;
		UserInterfaceRemodelManager.instance.ReleaseRequestBanner(ref releaseRequestTexture);
		mRealIndex = realIndex;
		mShipModel = model;
		CommonShipBanner.SetShipData(mShipModel);
		CommonShipBanner.StopParticle();
		uITexture.alpha = 1f;
		shipName.text = mShipModel.Name;
		shipLevel.text = mShipModel.Level.ToString();
		stars.init(mShipModel.Srate);
		mWidgetThis.alpha = 1f;
	}

	public void InitializeDefault(int realIndex)
	{
		UITexture uITexture = CommonShipBanner.GetUITexture();
		Texture releaseRequestTexture = uITexture.mainTexture;
		uITexture.mainTexture = null;
		UserInterfaceRemodelManager.instance.ReleaseRequestBanner(ref releaseRequestTexture);
		mRealIndex = realIndex;
		mShipModel = null;
		mWidgetThis.alpha = 1E-08f;
	}

	public int GetRealIndex()
	{
		return mRealIndex;
	}

	public ShipModel GetModel()
	{
		return mShipModel;
	}

	public int GetHeight()
	{
		return 58;
	}

	private void OnClick()
	{
		if (mOnTouchListener != null)
		{
			mOnTouchListener(this);
		}
	}

	public void SetOnTouchListener(Action<UIRemodelOtherShipPickerChildNew> onTouchListener)
	{
		mOnTouchListener = onTouchListener;
	}

	public Transform GetTransform()
	{
		if (mTransformCache == null)
		{
			mTransformCache = base.transform;
		}
		return mTransformCache;
	}

	public void Hover()
	{
		UISelectedObject.SelectedOneObjectBlink(mTransform_Background.gameObject, value: true);
	}

	public void RemoveHover()
	{
		UISelectedObject.SelectedOneObjectBlink(mTransform_Background.gameObject, value: false);
	}

	private void OnDestroy()
	{
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this);
		}
		UserInterfacePortManager.ReleaseUtils.Release(ref mWidgetThis);
		UserInterfacePortManager.ReleaseUtils.Release(ref shipName);
		UserInterfacePortManager.ReleaseUtils.Release(ref shipLevel);
		UserInterfacePortManager.ReleaseUtils.Release(ref mWidgetThis);
		UserInterfacePortManager.ReleaseUtils.Release(ref mWidgetThis);
		UserInterfacePortManager.ReleaseUtils.Release(ref mWidgetThis);
		UserInterfacePortManager.ReleaseUtils.Release(ref mWidgetThis);
		UserInterfacePortManager.ReleaseUtils.Release(ref CommonShipBanner, unloadAsset: true);
		stars = null;
		CommonShipBanner = null;
		mTransform_Background = null;
		mShipModel = null;
		mTransformCache = null;
		mOnTouchListener = null;
	}
}
