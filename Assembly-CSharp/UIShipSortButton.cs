using Common.Enum;
using KCV;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using local.utils;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UISprite))]
[RequireComponent(typeof(OnClickEventSender))]
[RequireComponent(typeof(UIWidget))]
public class UIShipSortButton : MonoBehaviour
{
	public delegate bool CheckClickable();

	private OnClickEventSender mOnClickEventSender;

	private UISprite mSpriteStatus;

	private UIWidget mWidgetThis;

	private Action<ShipModel[]> mOnSortedListener;

	private CheckClickable mCheckClicable;

	private SortKey[] mSortKeys;

	private ShipModel[] mShipModels;

	public SortKey CurrentSortKey
	{
		get;
		private set;
	}

	public bool IsClickable
	{
		get;
		private set;
	}

	private void Awake()
	{
		CurrentSortKey = SortKey.LEVEL;
		mOnClickEventSender = GetComponent<OnClickEventSender>();
		mSpriteStatus = GetComponent<UISprite>();
		mWidgetThis = GetComponent<UIWidget>();
	}

	public void SetSortKey(SortKey sortKey)
	{
		CurrentSortKey = sortKey;
		InitializeSortIcon(sortKey);
	}

	public void SetClickable(bool clickable)
	{
		IsClickable = clickable;
		if (IsClickable)
		{
			mOnClickEventSender.SetClickable(clickable: true);
		}
		else
		{
			mOnClickEventSender.SetClickable(clickable: false);
		}
	}

	public void Initialize(ShipModel[] shipModels)
	{
		mShipModels = shipModels;
		mSortKeys = new SortKey[4]
		{
			SortKey.SHIPTYPE,
			SortKey.NEW,
			SortKey.DAMAGE,
			SortKey.LEVEL
		};
	}

	public void InitializeForOrganize(ShipModel[] shipModels)
	{
		mShipModels = shipModels;
		mSortKeys = new SortKey[5]
		{
			SortKey.UNORGANIZED,
			SortKey.SHIPTYPE,
			SortKey.NEW,
			SortKey.DAMAGE,
			SortKey.LEVEL
		};
	}

	public void SetOnSortedShipsListener(Action<ShipModel[]> onSortedListener)
	{
		mOnSortedListener = onSortedListener;
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchSortButton()
	{
		OnClickSortButton();
	}

	public void SetCheckClicableDelegate(CheckClickable checkClickable)
	{
		mCheckClicable = checkClickable;
	}

	public void OnClickSortButton()
	{
		if (IsClickable && CallCheckClickable())
		{
			SortNext();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}
	}

	private bool CallCheckClickable()
	{
		if (mCheckClicable != null)
		{
			return mCheckClicable();
		}
		return true;
	}

	private void SortNext()
	{
		SortKey nextSortKey = GetNextSortKey(CurrentSortKey);
		ChangeSortKey(nextSortKey);
	}

	private void OnSortedShips(ShipModel[] sortedModels)
	{
		if (mOnSortedListener != null)
		{
			mOnSortedListener(sortedModels);
		}
	}

	private SortKey GetNextSortKey(SortKey currentSortKey)
	{
		int num = Array.IndexOf(mSortKeys, CurrentSortKey);
		num++;
		if (num >= mSortKeys.Length)
		{
			num = 0;
		}
		return mSortKeys[num];
	}

	private void InitializeSortIcon(SortKey sortKey)
	{
		switch (sortKey)
		{
		case SortKey.LEVEL_LOCK:
		case SortKey.LOCK_LEVEL:
			break;
		case SortKey.DAMAGE:
			mSpriteStatus.spriteName = "sort_crane";
			break;
		case SortKey.LEVEL:
			mSpriteStatus.spriteName = "sort_lv";
			break;
		case SortKey.NEW:
			mSpriteStatus.spriteName = "sort_new";
			break;
		case SortKey.SHIPTYPE:
			mSpriteStatus.spriteName = "sort_ship";
			break;
		case SortKey.UNORGANIZED:
			mSpriteStatus.spriteName = "sort_flag";
			break;
		}
	}

	private void ChangeSortKey(SortKey sortKey)
	{
		CurrentSortKey = sortKey;
		InitializeSortIcon(sortKey);
		ShipModel[] sortedModels = DeckUtil.GetSortedList(new List<ShipModel>(mShipModels), sortKey).ToArray();
		OnSortedShips(sortedModels);
	}

	public void RefreshModels(ShipModel[] models)
	{
		mShipModels = models;
	}

	public void ReSort()
	{
		ChangeSortKey(CurrentSortKey);
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Release(ref mSpriteStatus);
		UserInterfacePortManager.ReleaseUtils.Release(ref mWidgetThis);
		mOnClickEventSender = null;
		mOnSortedListener = null;
		mCheckClicable = null;
		mShipModels = null;
	}

	internal ShipModel[] SortModels(SortKey sortKey)
	{
		return DeckUtil.GetSortedList(new List<ShipModel>(mShipModels), sortKey).ToArray();
	}
}
