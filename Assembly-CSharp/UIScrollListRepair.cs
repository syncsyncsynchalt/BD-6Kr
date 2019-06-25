using Common.Enum;
using KCV;
using KCV.Port.Repair;
using KCV.Utils;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

public class UIScrollListRepair : UIScrollList<ShipModel, UIScrollListRepairChild>
{
	private KeyControl mKeyController;

	[SerializeField]
	private UIShipSortButton SortButton;

	[SerializeField]
	private Transform noShips;

	private int _NowShipLength;

	private string _before_s;

	private repair _rep;

	private bool mCallFirstInitialze;

	private Action mOnBackListener;

	private Action<UIScrollListRepairChild> mOnUIScrollListRepairSelectedListener;

	public KeyControl keyController => mKeyController;

	protected override void OnUpdate()
	{
		if (mKeyController == null || base.mState != ListState.Waiting || _rep.now_mode() != 2)
		{
			return;
		}
		if (mKeyController.keyState[12].down)
		{
			NextFocus();
		}
		else if (mKeyController.keyState[8].down)
		{
			PrevFocus();
		}
		else if (mKeyController.keyState[14].down)
		{
			PrevPageOrHeadFocus();
		}
		else if (mKeyController.keyState[10].down)
		{
			NextPageOrTailFocus();
		}
		else if (mKeyController.keyState[1].down)
		{
			if (_NowShipLength != 0)
			{
				Select();
			}
		}
		else if (mKeyController.keyState[0].down)
		{
			mKeyController = null;
		}
		else if (mKeyController.keyState[3].down)
		{
			SortButton.OnClickSortButton();
		}
	}

	protected override void OnChangedFocusView(UIScrollListRepairChild focusToView)
	{
		if (!(focusToView == null) && _rep.now_mode() == 2 && base.mState == ListState.Waiting)
		{
			string text = (focusToView.GetRealIndex() + 1).ToString() + "/" + mModels.Length.ToString();
			if (_before_s != text)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			_before_s = text;
			if (CommonPopupDialog.Instance != null)
			{
				CommonPopupDialog.Instance.StartPopup(text);
			}
		}
	}

	protected override void OnSelect(UIScrollListRepairChild view)
	{
		if (!(view == null) && mOnUIScrollListRepairSelectedListener != null)
		{
			mOnUIScrollListRepairSelectedListener(view);
		}
	}

	internal void SetCamera(Camera camera)
	{
		SetSwipeEventCamera(camera);
	}

	internal new void Initialize(ShipModel[] ships)
	{
		_NowShipLength = ships.Length;
		noShips.transform.localScale = ((_NowShipLength != 0) ? Vector3.zero : Vector3.one);
		_before_s = string.Empty;
		_rep = ((Component)base.gameObject.transform.parent.parent.parent).GetComponent<repair>();
		mKeyController.ClearKeyAll();
		if (SortButton.isActiveAndEnabled)
		{
			SortButton.Initialize(ships);
			SortButton.SetClickable(clickable: true);
			if (!mCallFirstInitialze)
			{
				SortButton.SetSortKey(SortKey.DAMAGE);
				mCallFirstInitialze = true;
			}
			SortButton.SetCheckClicableDelegate(CheckSortButtonClickable);
			SortButton.SetOnSortedShipsListener(OnSorted);
			SortButton.ReSort();
		}
	}

	private void OnSorted(ShipModel[] ships)
	{
		if (ships.Length != 0)
		{
			Refresh(ships, firstPage: true);
			base.ChangeImmediateContentPosition(ContentDirection.Hell);
		}
	}

	private bool CheckSortButtonClickable()
	{
		return base.mState == ListState.Waiting;
	}

	internal void SetOnBackListener(Action OnUIScrollListRepairBackListener)
	{
		mOnBackListener = OnUIScrollListRepairBackListener;
	}

	internal void SetOnSelectedListener(Action<UIScrollListRepairChild> OnUIScrollListRepairSelectedListener)
	{
		mOnUIScrollListRepairSelectedListener = OnUIScrollListRepairSelectedListener;
	}

	internal void SetKeyController(KeyControl dockSelectController)
	{
		mKeyController = dockSelectController;
	}

	public new void StartControl()
	{
		base.StartControl();
	}

	public new void LockControl()
	{
		base.LockControl();
	}

	public void ResumeControl()
	{
		base.StartControl();
	}
}
