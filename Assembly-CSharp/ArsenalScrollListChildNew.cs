using KCV;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public class ArsenalScrollListChildNew : MonoBehaviour, UIScrollListItem<ShipModel, ArsenalScrollListChildNew>
{
	public delegate bool CheckSelectedDelegate(ShipModel shipModel);

	[SerializeField]
	private UILabel _labelName;

	[SerializeField]
	private Transform mBackground;

	[SerializeField]
	private UILabel _labelLv;

	[SerializeField]
	private UISprite _check;

	[SerializeField]
	private UISprite _ShipType;

	[SerializeField]
	private UISprite _LockType;

	private UIWidget mWidgetThis;

	private ShipModel mShipModel;

	private int mRealIndex;

	private Action<ArsenalScrollListChildNew> mOnTouchListener;

	private CheckSelectedDelegate mCheckSelectedDelegate;

	private Transform mCachedTransform;

	private void Awake()
	{
		mWidgetThis = GetComponent<UIWidget>();
		mWidgetThis.alpha = 1E-08f;
	}

	public void refresh()
	{
		_check.alpha = 0f;
	}

	private void UpdateCheckState(bool enabled)
	{
		if (enabled)
		{
			_check.alpha = 1f;
		}
		else
		{
			_check.alpha = 0f;
		}
	}

	public void Initialize(int realIndex, ShipModel model)
	{
		mRealIndex = realIndex;
		mShipModel = model;
		refresh();
		_ShipType.spriteName = "ship" + mShipModel.ShipType;
		_labelName.text = mShipModel.Name;
		_labelLv.textInt = mShipModel.Level;
		if (mShipModel.IsLocked())
		{
			_LockType.spriteName = "lock_ship";
		}
		else if (mShipModel.HasLocked())
		{
			_LockType.spriteName = "lock_on";
		}
		else
		{
			_LockType.spriteName = null;
		}
		if (CheckSelected(mShipModel))
		{
			UpdateCheckState(enabled: true);
		}
		else
		{
			UpdateCheckState(enabled: false);
		}
		mWidgetThis.alpha = 1f;
	}

	public void InitializeDefault(int realIndex)
	{
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
		return 61;
	}

	public void SetOnTouchListener(Action<ArsenalScrollListChildNew> onTouchListener)
	{
		mOnTouchListener = onTouchListener;
	}

	public void SetCheckSelectedDelegate(CheckSelectedDelegate checkSelectedDelegate)
	{
		mCheckSelectedDelegate = checkSelectedDelegate;
	}

	private bool CheckSelected(ShipModel shipModel)
	{
		if (mCheckSelectedDelegate != null)
		{
			return mCheckSelectedDelegate(shipModel);
		}
		return false;
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void Touch()
	{
		if (mOnTouchListener != null)
		{
			mOnTouchListener(this);
		}
	}

	public void Hover()
	{
		UISelectedObject.SelectedOneObjectBlink(mBackground.gameObject, value: true);
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
		UISelectedObject.SelectedOneObjectBlink(mBackground.gameObject, value: false);
	}

	private void OnDestroy()
	{
		_labelName = null;
		mBackground = null;
		_labelLv = null;
		_check = null;
		_ShipType = null;
		_LockType = null;
		mWidgetThis = null;
		mShipModel = null;
	}
}
