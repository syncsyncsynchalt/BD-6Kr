using Common.Enum;
using KCV;
using KCV.Supply;
using KCV.Utils;
using KCV.View.ScrollView;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

public class OtherShipListScrollNew : UIScrollList<ShipModel, OtherShipListChildNew>
{
	[SerializeField]
	private UIShipSortButton mUIShipSortButton;

	[SerializeField]
	private Camera touchEventCamera;

	private Vector3 SHOW_LOCAL_POSITION = new Vector3(-163f, 115f);

	private Vector3 HIDE_LOCAL_POSITION = new Vector3(-1000f, 115f);

	private Dictionary<ShipModel, bool> selectedModels;

	private KeyControl mKeyController;

	private bool mCallFirstInitialized;

	private ShipModel nextFocusShipModel;

	public int GetShipCount()
	{
		return mModels.Length;
	}

	public void StartState()
	{
		mUIShipSortButton.SetCheckClicableDelegate(CheckSortButtonClickable);
		mUIShipSortButton.SetClickable(clickable: true);
		KillScrollAnimation();
		if (nextFocusShipModel == null)
		{
			HeadFocus();
		}
		else
		{
			ChangePageFromModel(nextFocusShipModel);
		}
		base.StartControl();
	}

	protected override void OnUpdate()
	{
		if (mKeyController != null && base.mState == ListState.Waiting)
		{
			if (mKeyController.keyState[12].down)
			{
				NextFocus();
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			else if (mKeyController.keyState[8].down)
			{
				PrevFocus();
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
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
				Select();
			}
			else if (mKeyController.keyState[3].down)
			{
				mUIShipSortButton.OnClickSortButton();
			}
			else if (mKeyController.keyState[0].down)
			{
				mKeyController = null;
			}
		}
	}

	protected override void OnChangedFocusView(OtherShipListChildNew focusToView)
	{
		if (mModels != null && mModels.Length > 0 && base.mState == ListState.Waiting)
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_014);
			if (mModels != null && mModels.Length > 0)
			{
				CommonPopupDialog.Instance.StartPopup(mCurrentFocusView.GetRealIndex() + 1 + "/" + mModels.Length, 0, CommonPopupDialogMessage.PlayType.Short);
			}
		}
	}

	protected override void OnSelect(OtherShipListChildNew view)
	{
		if (!(view == null) && view.shipBanner.IsSelectable() && SupplyMainManager.Instance.IsShipSelectableStatus())
		{
			SupplyMainManager.Instance.change_2_SHIP_SELECT(defaultFocus: true);
			SwitchSelected(view);
		}
	}

	public void Initialize(KeyControl keyController, ShipModel[] otherShipModels)
	{
		selectedModels = new Dictionary<ShipModel, bool>();
		SetSwipeEventCamera(touchEventCamera);
		if (!mCallFirstInitialized)
		{
			mUIShipSortButton.SetSortKey(SortKey.LEVEL);
			mCallFirstInitialized = true;
		}
		mUIShipSortButton.Initialize(otherShipModels);
		mUIShipSortButton.SetOnSortedShipsListener(OnSortedShipsListener);
		mUIShipSortButton.ReSort();
		mKeyController = keyController;
		List<ShipModel> list = new List<ShipModel>();
		SupplyMainManager.Instance.SupplyManager.Ships.ForEach(delegate(ShipModel e)
		{
			if (e != null)
			{
				list.Add(e);
			}
		});
		list.ToArray();
		mUIShipSortButton.SetClickable(clickable: true);
		base.gameObject.SetActive(false);
		base.gameObject.SetActive(true);
	}

	private void OnSortedShipsListener(ShipModel[] refreshOtherShipModels)
	{
		Refresh(refreshOtherShipModels, firstPage: true);
	}

	public void SwitchSelected(OtherShipListChildNew view)
	{
		ShipModel ship = view.shipBanner.Ship;
		if (view.shipBanner.SwitchSelected())
		{
			selectedModels[ship] = true;
		}
		else
		{
			selectedModels.Remove(ship);
		}
		SupplyMainManager.Instance.UpdateRightPain();
	}

	public List<ShipModel> getSeletedModelList()
	{
		List<ShipModel> list = new List<ShipModel>();
		for (int i = 0; i < mModels.Length; i++)
		{
			ShipModel shipModel = mModels[i];
			if (selectedModels.ContainsKey(shipModel))
			{
				list.Add(shipModel);
			}
		}
		return list;
	}

	public bool isSelected(ShipModel model)
	{
		return model != null && selectedModels.ContainsKey(model);
	}

	public void Show(bool animation = true)
	{
		base.transform.localPosition = SHOW_LOCAL_POSITION;
	}

	public void Hide(bool animation = true)
	{
		KillScrollAnimation();
		LockControl();
		base.transform.localPosition = HIDE_LOCAL_POSITION;
		mKeyController = null;
	}

	public void ProcessRecoveryAnimation()
	{
		Debug.Log("回復アニメ\u30fcション");
		for (int i = 0; i < mViews.Length; i++)
		{
			if (isSelected(mViews[i].GetModel()))
			{
				mViews[i].shipBanner.ProcessRecoveryAnimation();
			}
		}
	}

	internal new void LockControl()
	{
		mUIShipSortButton.SetClickable(clickable: false);
		KillScrollAnimation();
		base.LockControl();
	}

	public new void StartControl()
	{
		mUIShipSortButton.SetClickable(clickable: true);
		mKeyController.ClearKeyAll();
		mKeyController.firstUpdate = true;
		base.StartControl();
	}

	private bool CheckSortButtonClickable()
	{
		return base.mState == ListState.Waiting;
	}

	internal void MemoryNextFocus()
	{
		nextFocusShipModel = null;
		if (!(mCurrentFocusView == null) && mCurrentFocusView.GetModel() != null)
		{
			int num = Array.IndexOf(mModels, mCurrentFocusView.GetModel());
			if (0 < num)
			{
				nextFocusShipModel = mModels[num - 1];
			}
			else
			{
				nextFocusShipModel = null;
			}
		}
	}
}
