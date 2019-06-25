using Common.Enum;
using KCV;
using KCV.Organize;
using KCV.Utils;
using KCV.View.Scroll;
using local.models;
using UnityEngine;

public class OrganizeShipListScroll : UIScrollListParent<ShipModel, OrganizeShipListChild>
{
	[SerializeField]
	private UIShipSortButton mUIShipSortButton;

	private void Start()
	{
		mUIShipSortButton.SetSortKey(SortKey.UNORGANIZED);
		mUIShipSortButton.SetOnSortedShipsListener(OnSorted);
	}

	public void Init(ShipModel[] models)
	{
		mUIShipSortButton.InitializeForOrganize(models);
		mUIShipSortButton.SetClickable(clickable: true);
		mUIShipSortButton.ReSort();
	}

	public void SwitchFocusShipLockState()
	{
		ViewFocus.SwitchShipLockState();
	}

	protected override void OnAction(ActionType actionType, UIScrollListParent<ShipModel, OrganizeShipListChild> calledObject, OrganizeShipListChild actionChild)
	{
		base.OnAction(actionType, calledObject, actionChild);
		switch (actionType)
		{
		case ActionType.OnChangeFocus:
			if (0 < GetModelSize())
			{
				CommonPopupDialog.Instance.StartPopup($"{actionChild.SortIndex + 1}/{GetModelSize()}", 0, CommonPopupDialogMessage.PlayType.Short);
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			break;
		}
	}

	private void OnSorted(ShipModel[] sortedShipModels)
	{
		base.Initialize(sortedShipModels);
	}

	protected override void OnKeyPressTriangle()
	{
		base.OnKeyPressTriangle();
		mUIShipSortButton.OnClickSortButton();
	}

	private void OnDestroy()
	{
		mUIShipSortButton = null;
	}
}
