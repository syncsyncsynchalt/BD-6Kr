using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public class UIScrollListChildTest : MonoBehaviour, UIScrollListItem<ShipModel, UIScrollListChildTest>
{
	[SerializeField]
	private UILabel mLabel_ShipName;

	[SerializeField]
	private UITexture mTexture_ShipBanner;

	[SerializeField]
	private UITexture mTexture_Background;

	private int mRealIndex;

	private ShipModel mShipModel;

	private UIWidget mWidgetThis;

	private Transform mTransform;

	private Action<UIScrollListChildTest> mOnTouchListener;

	private void Awake()
	{
		mWidgetThis = GetComponent<UIWidget>();
		mWidgetThis.alpha = 1E-09f;
	}

	public void Initialize(int realIndex, ShipModel model)
	{
		mRealIndex = realIndex;
		mShipModel = model;
		mTexture_ShipBanner.mainTexture = UIScrollListTest.sShipBannerManager.GetShipBanner(mShipModel);
		mLabel_ShipName.text = mShipModel.Name;
		NGUITools.ImmediatelyCreateDrawCalls(base.gameObject);
		mWidgetThis.alpha = 1f;
		base.name = "[" + realIndex + "]【" + model.Name + "】";
	}

	public void InitializeDefault(int realIndex)
	{
		mRealIndex = realIndex;
		mTexture_ShipBanner.mainTexture = null;
		mLabel_ShipName.text = string.Empty;
		mShipModel = null;
		mWidgetThis.alpha = 1E-09f;
		base.name = "[" + realIndex + "]";
	}

	public Transform GetTransform()
	{
		if (mTransform == null)
		{
			mTransform = base.transform;
		}
		return mTransform;
	}

	public ShipModel GetModel()
	{
		return mShipModel;
	}

	public int GetHeight()
	{
		return 75;
	}

	private void OnClick()
	{
		if (mOnTouchListener != null)
		{
			mOnTouchListener(this);
		}
	}

	public void SetOnTouchListener(Action<UIScrollListChildTest> onTouchListener)
	{
		mOnTouchListener = onTouchListener;
	}

	public void Hover()
	{
		mTexture_Background.color = Color.cyan;
	}

	public void RemoveHover()
	{
		mTexture_Background.color = Color.white;
	}

	public float GetBottomPositionY()
	{
		Vector3 localPosition = GetTransform().localPosition;
		return localPosition.y + (float)GetHeight();
	}

	public float GetHeadPositionY()
	{
		Vector3 localPosition = GetTransform().localPosition;
		return localPosition.y;
	}

	public int GetRealIndex()
	{
		return mRealIndex;
	}
}
