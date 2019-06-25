using Common.Enum;
using KCV.Utils;
using KCV.View.Scroll;
using local.models;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodeModernizationShipTargetListParent : UIScrollListParent<RemodeModernizationShipTargetListChild, UIRemodeModernizationShipTargetListChild>, UIRemodelView
	{
		[SerializeField]
		private UIButton mButton_TouchBack;

		[SerializeField]
		private Transform mMessage;

		[SerializeField]
		private UIShipSortButton mUIShipSortButton;

		private Vector3 showPos = new Vector3(270f, 0f, 0f);

		private Vector3 hidePos = new Vector3(700f, 0f, 0f);

		private bool mIsFirstInitialized;

		private KeyControl originKeyController;

		private ShipModel mTargetExchangeShipModel;

		private bool mCallFirstInitialized;

		private void Awake()
		{
			base.transform.localPosition = hidePos;
		}

		public void Initialize(KeyControl keyController, ShipModel targetExchangeShipModel, List<ShipModel> exceptShipModels)
		{
			mTargetExchangeShipModel = targetExchangeShipModel;
			originKeyController = keyController;
			UserInterfaceRemodelManager.instance.mRemodelManager.PowupTargetShip = UserInterfaceRemodelManager.instance.focusedShipModel;
			ShipModel[] candidateShips = UserInterfaceRemodelManager.instance.mRemodelManager.GetCandidateShips(exceptShipModels);
			if (!mCallFirstInitialized)
			{
				mUIShipSortButton.SetSortKey(SortKey.LEVEL);
				mCallFirstInitialized = true;
			}
			mUIShipSortButton.Initialize(candidateShips);
			mUIShipSortButton.SetClickable(clickable: true);
			mUIShipSortButton.SetOnSortedShipsListener(onSortedShipsListener);
			List<RemodeModernizationShipTargetListChild> list = new List<RemodeModernizationShipTargetListChild>();
			if (targetExchangeShipModel != null)
			{
				list.Add(new RemodeModernizationShipTargetListChild(RemodeModernizationShipTargetListChild.ListItemOption.UnSet, null));
			}
			if (targetExchangeShipModel == null && candidateShips.Length == 0)
			{
				mMessage.localScale = Vector3.one;
			}
			else
			{
				mMessage.localScale = Vector3.zero;
			}
			ShipModel[] array = candidateShips;
			foreach (ShipModel model in array)
			{
				list.Add(new RemodeModernizationShipTargetListChild(RemodeModernizationShipTargetListChild.ListItemOption.Model, model));
			}
			if (!mIsFirstInitialized)
			{
				base.Initialize(list.ToArray());
				mIsFirstInitialized = true;
			}
			else
			{
				RefreshAndFirstFocus(list.ToArray());
			}
			SetOnUIScrollListParentAction(delegate(ActionType actionType, UIScrollListParent<RemodeModernizationShipTargetListChild, UIRemodeModernizationShipTargetListChild> calledObject, UIScrollListChild<RemodeModernizationShipTargetListChild> actionChild)
			{
				OnScrollAction(actionType, (UIRemodeModernizationShipTargetListChild)actionChild);
			});
		}

		protected override void OnKeyPressTriangle()
		{
			mUIShipSortButton.OnClickSortButton();
		}

		private void onSortedShipsListener(ShipModel[] shipModels)
		{
			List<RemodeModernizationShipTargetListChild> list = new List<RemodeModernizationShipTargetListChild>();
			if (mTargetExchangeShipModel != null)
			{
				list.Add(new RemodeModernizationShipTargetListChild(RemodeModernizationShipTargetListChild.ListItemOption.UnSet, null));
			}
			foreach (ShipModel model in shipModels)
			{
				list.Add(new RemodeModernizationShipTargetListChild(RemodeModernizationShipTargetListChild.ListItemOption.Model, model));
			}
			base.Initialize(list.ToArray());
		}

		public void Back()
		{
			Hide();
			UserInterfaceRemodelManager.instance.Back2KindaikaKaishu();
		}

		public void OnTouchHide()
		{
			Back();
		}

		public void Show()
		{
			SetKeyController(originKeyController);
			mButton_TouchBack.SetActive(isActive: true);
			RemodelUtils.MoveWithManual(base.gameObject, showPos, 0.2f);
		}

		public void Hide()
		{
			Hide(animation: true);
		}

		public void Hide(bool animation)
		{
			SetKeyController(null);
			mButton_TouchBack.SetActive(isActive: false);
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.gameObject, hidePos, 0.2f);
			}
			else
			{
				base.transform.localPosition = hidePos;
			}
		}

		public override void SetKeyController(KeyControl keyController)
		{
			if (keyController != null)
			{
				keyController.firstUpdate = true;
				keyController.ClearKeyAll();
			}
			base.SetKeyController(keyController);
		}

		private void OnScrollAction(ActionType actionType, UIRemodeModernizationShipTargetListChild actionChild)
		{
			switch (actionType)
			{
			case ActionType.OnChangeFirstFocus:
				break;
			case ActionType.OnButtonSelect:
			case ActionType.OnTouch:
			{
				ShipModel selectedShipModel = null;
				if (actionChild != null && actionChild.Model.mOption == RemodeModernizationShipTargetListChild.ListItemOption.Model)
				{
					selectedShipModel = actionChild.Model.mShipModel;
				}
				Hide();
				UserInterfaceRemodelManager.instance.SelectKindaikaKaishuSozai(selectedShipModel);
				break;
			}
			case ActionType.OnBack:
				Back();
				break;
			case ActionType.OnChangeFocus:
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				break;
			}
		}

		private void OnDestroy()
		{
			mButton_TouchBack = null;
		}
	}
}
