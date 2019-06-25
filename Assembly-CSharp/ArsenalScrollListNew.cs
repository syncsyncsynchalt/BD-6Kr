using Common.Enum;
using KCV;
using KCV.Utils;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

public class ArsenalScrollListNew : UIScrollList<ShipModel, ArsenalScrollListChildNew>
{
	[SerializeField]
	private UIShipSortButton mUIShipSortButton;

	[SerializeField]
	private Transform MessageShip;

	private KeyControl mKeyController;

	private bool _isFirst = true;

	private int _before_focus;

	private Action<ArsenalScrollListChildNew> mOnSelectedListener;

	private bool mCallFirstInitialized;

	public ShipModel SelectedShip
	{
		get;
		private set;
	}

	protected override void OnUpdate()
	{
		if (base.mState != ListState.Waiting || mKeyController == null)
		{
			return;
		}
		if (mKeyController.keyState[3].down)
		{
			mUIShipSortButton.OnClickSortButton();
		}
		else if (mKeyController.keyState[1].down)
		{
			if (mModels != null && mModels.Length > 0)
			{
				Select();
			}
		}
		else if (mKeyController.keyState[14].down)
		{
			PrevPageOrHeadFocus();
		}
		else if (mKeyController.keyState[10].down)
		{
			NextPageOrTailFocus();
		}
		else if (mKeyController.keyState[12].down)
		{
			NextFocus();
		}
		else if (mKeyController.keyState[8].down)
		{
			PrevFocus();
		}
	}

	public void SetOnSelectedListener(Action<ArsenalScrollListChildNew> onSelectedListener)
	{
		mOnSelectedListener = onSelectedListener;
	}

	protected override void OnSelect(ArsenalScrollListChildNew view)
	{
		SelectedShip = view.GetModel();
		base.RefreshViews();
		if (mOnSelectedListener != null)
		{
			mOnSelectedListener(view);
		}
	}

	protected override void OnAwake()
	{
		mUIShipSortButton.SetOnSortedShipsListener(OnSorted);
		ArsenalScrollListChildNew[] mViews = base.mViews;
		foreach (ArsenalScrollListChildNew arsenalScrollListChildNew in mViews)
		{
			arsenalScrollListChildNew.SetCheckSelectedDelegate(OnCheckSelected);
		}
	}

	private bool OnCheckSelected(ShipModel shipModel)
	{
		if (SelectedShip == null)
		{
			return false;
		}
		if (shipModel == null)
		{
			return false;
		}
		if (SelectedShip.MemId == shipModel.MemId)
		{
			return true;
		}
		return false;
	}

	public new void Initialize(ShipModel[] ships)
	{
		_before_focus = 0;
		_isFirst = true;
		SelectedShip = null;
		Message(ships);
		if (!mCallFirstInitialized)
		{
			mUIShipSortButton.SetSortKey(SortKey.LEVEL);
			mCallFirstInitialized = true;
		}
		mUIShipSortButton.Initialize(ships);
		ShipModel[] models = mUIShipSortButton.SortModels(mUIShipSortButton.CurrentSortKey);
		base.ChangeImmediateContentPosition(ContentDirection.Hell);
		base.Initialize(models);
		HeadFocus();
	}

	public void SetCamera(Camera camera)
	{
		SetSwipeEventCamera(camera);
	}

	public void Refresh(ShipModel[] models)
	{
		Message(models);
		SelectedShip = null;
		mUIShipSortButton.RefreshModels(models);
		mModels = mUIShipSortButton.SortModels(mUIShipSortButton.CurrentSortKey);
		base.RefreshViews();
		if (mCurrentFocusView.GetModel() == null && mModels.Length != 0)
		{
			TailFocus();
		}
		else if (mCurrentFocusView.GetRealIndex() == mModels.Length - 1)
		{
			TailFocus();
		}
		else if (mViews_WorkSpace[mUserViewCount - 1].GetModel() == null && mModels.Length != 0)
		{
			int num = Array.IndexOf(mViews_WorkSpace, mCurrentFocusView);
			TailFocusPage();
			ChangeFocusView(mViews_WorkSpace[num]);
		}
	}

	public void Message(ShipModel[] models)
	{
		if (models.Length == 0)
		{
			MessageShip.localScale = Vector3.one;
		}
		else
		{
			MessageShip.localScale = Vector3.zero;
		}
	}

	protected override void OnChangedFocusView(ArsenalScrollListChildNew focusToView)
	{
		if (mModels != null && mModels.Length > 0 && base.mState == ListState.Waiting)
		{
			if (!_isFirst && _before_focus != focusToView.GetRealIndex())
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			_isFirst = false;
			if (mModels != null && mModels.Length > 0)
			{
				_before_focus = focusToView.GetRealIndex();
				CommonPopupDialog.Instance.StartPopup(mCurrentFocusView.GetRealIndex() + 1 + "/" + mModels.Length, 0, CommonPopupDialogMessage.PlayType.Short);
			}
		}
	}

	public ListState GetCurrentState()
	{
		return base.mState;
	}

	private void OnSorted(ShipModel[] sortedShipModels)
	{
		Refresh(sortedShipModels, firstPage: true);
	}

	public new void RefreshViews()
	{
		base.RefreshViews();
	}

	public new void LockControl()
	{
		base.LockControl();
	}

	public void SetKeyController(KeyControl keyController)
	{
		mKeyController = keyController;
	}

	public new void StartControl()
	{
		base.StartControl();
	}

	public void ResumeControl()
	{
		mKeyController.ClearKeyAll();
		mKeyController.firstUpdate = true;
		base.StartControl();
	}

	protected override void OnCallDestroy()
	{
		mUIShipSortButton = null;
		MessageShip = null;
		mKeyController = null;
	}

	internal bool HasSelectedShip()
	{
		return SelectedShip != null;
	}

	internal void RemoveSelectedShip()
	{
		SelectedShip = null;
	}

	internal int GetFocusModelIndex()
	{
		return Array.IndexOf(mModels, mCurrentFocusView.GetModel());
	}

	internal void ClearSelected()
	{
		RemoveSelectedShip();
		base.RefreshViews();
	}
}
