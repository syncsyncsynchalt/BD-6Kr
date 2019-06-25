using KCV.Supply;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

public class OtherShipListChildNew : MonoBehaviour, UIScrollListItem<ShipModel, OtherShipListChildNew>
{
	private int mRealIndex;

	private ShipModel mShipModel;

	private UIWidget mWidgetThis;

	[SerializeField]
	public UISupplyOtherShipBanner shipBanner;

	private Transform mTransform;

	private Action<OtherShipListChildNew> mOnTouchListener;

	private void Awake()
	{
		mWidgetThis = GetComponent<UIWidget>();
	}

	public void Initialize(int realIndex, ShipModel shipModel)
	{
		mRealIndex = realIndex;
		mShipModel = shipModel;
		base.enabled = true;
		shipBanner.Init();
		shipBanner.SetBanner(shipModel, mRealIndex);
		shipBanner.Select(SupplyMainManager.Instance._otherListParent.isSelected(mShipModel));
		mWidgetThis.alpha = 1f;
	}

	public void InitializeDefault(int realIndex)
	{
		mRealIndex = realIndex;
		mShipModel = null;
		mWidgetThis.alpha = 1E-09f;
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
		return 56;
	}

	private void OnClick()
	{
		if (mOnTouchListener != null)
		{
			mOnTouchListener(this);
		}
	}

	public void SetOnTouchListener(Action<OtherShipListChildNew> onTouchListener)
	{
		mOnTouchListener = onTouchListener;
	}

	public void Hover()
	{
		shipBanner.Hover(enabled: true);
	}

	public void RemoveHover()
	{
		shipBanner.Hover(enabled: false);
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
