using Common.Enum;
using KCV.Utils;
using KCV.View.ScrollView;
using local.models;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRemodeModernizationShipTargetListParentNew : UIScrollList<RemodeModernizationShipTargetListChildNew, UIRemodeModernizationShipTargetListChildNew>, UIRemodelView, IBannerResourceManage
	{
		private UIPanel mPanelThis;

		[SerializeField]
		private Transform mTransform_TouchBack;

		[SerializeField]
		private Transform mMessage;

		[SerializeField]
		private UIShipSortButton mUIShipSortButton;

		private Vector3 showPos = new Vector3(270f, 250f, 0f);

		private Vector3 hidePos = new Vector3(730f, 250f, 0f);

		private bool mIsFirstInitialized;

		private bool mCallFirstInitialize;

		private KeyControl mKeyController;

		private ShipModel mTargetExchangeShipModel;

		private bool isShown;

		private float ANIMATION_DURATION = 0.2f;

		protected override void OnAwake()
		{
			mPanelThis = GetComponent<UIPanel>();
			base.transform.localPosition = hidePos;
		}

		public void SetCamera(Camera swipeEventCamera)
		{
			SetSwipeEventCamera(swipeEventCamera);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (mKeyController != null && base.mState == ListState.Waiting && isShown)
			{
				if (mKeyController.keyState[8].down)
				{
					PrevFocus();
				}
				else if (mKeyController.keyState[12].down)
				{
					NextFocus();
				}
				else if (mKeyController.keyState[3].down)
				{
					mUIShipSortButton.OnClickSortButton();
				}
				else if (mKeyController.keyState[1].down)
				{
					Select();
				}
				else if (mKeyController.keyState[0].down)
				{
					Back();
				}
				else if (mKeyController.keyState[14].down)
				{
					PrevPageOrHeadFocus();
				}
				else if (mKeyController.keyState[10].down)
				{
					NextPageOrTailFocus();
				}
			}
		}

		protected override void OnChangedFocusView(UIRemodeModernizationShipTargetListChildNew focusToView)
		{
			if (base.mState == ListState.Waiting)
			{
				base.OnChangedFocusView(focusToView);
				if (isShown)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
		}

		public void Initialize(KeyControl keyController, ShipModel targetExchangeShipModel, List<ShipModel> exceptShipModels)
		{
			UIPanel component = ((Component)mTransform_ContentPosition.parent).GetComponent<UIPanel>();
			if (component != null)
			{
				component.clipping = UIDrawCall.Clipping.None;
			}
			base.ChangeImmediateContentPosition(ContentDirection.Hell);
			mTargetExchangeShipModel = targetExchangeShipModel;
			mKeyController = keyController;
			UserInterfaceRemodelManager.instance.mRemodelManager.PowupTargetShip = UserInterfaceRemodelManager.instance.focusedShipModel;
			ShipModel[] candidateShips = UserInterfaceRemodelManager.instance.mRemodelManager.GetCandidateShips(exceptShipModels);
			List<RemodeModernizationShipTargetListChildNew> list = new List<RemodeModernizationShipTargetListChildNew>();
			if (targetExchangeShipModel != null)
			{
				list.Add(new RemodeModernizationShipTargetListChildNew(RemodeModernizationShipTargetListChildNew.ListItemOption.UnSet, null));
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
				list.Add(new RemodeModernizationShipTargetListChildNew(RemodeModernizationShipTargetListChildNew.ListItemOption.Model, model));
			}
			if (!mCallFirstInitialize)
			{
				mUIShipSortButton.SetSortKey(SortKey.LEVEL);
				mCallFirstInitialize = true;
			}
			mUIShipSortButton.Initialize(candidateShips);
			mUIShipSortButton.SetClickable(clickable: true);
			mUIShipSortButton.SetOnSortedShipsListener(onSortedShipsListener);
			mUIShipSortButton.ReSort();
			HeadFocus();
			StartControl();
		}

		private void onSortedShipsListener(ShipModel[] shipModels)
		{
			List<RemodeModernizationShipTargetListChildNew> list = new List<RemodeModernizationShipTargetListChildNew>();
			if (mTargetExchangeShipModel != null)
			{
				list.Add(new RemodeModernizationShipTargetListChildNew(RemodeModernizationShipTargetListChildNew.ListItemOption.UnSet, null));
			}
			foreach (ShipModel model in shipModels)
			{
				list.Add(new RemodeModernizationShipTargetListChildNew(RemodeModernizationShipTargetListChildNew.ListItemOption.Model, model));
			}
			if (!mIsFirstInitialized)
			{
				Initialize(list.ToArray());
				if (shipModels.Length == 0)
				{
					mMessage.SetActive(isActive: true);
				}
				else
				{
					mMessage.SetActive(isActive: false);
				}
				mIsFirstInitialized = true;
			}
			else
			{
				Refresh(list.ToArray(), firstPage: true);
				if (shipModels.Length == 0)
				{
					mMessage.SetActive(isActive: true);
				}
				else
				{
					mMessage.SetActive(isActive: false);
				}
				base.gameObject.SetActive(true);
			}
			base.ChangeImmediateContentPosition(ContentDirection.Hell);
			HeadFocus();
			StartControl();
		}

		public void Back()
		{
			if (isShown)
			{
				Hide();
				UserInterfaceRemodelManager.instance.Back2KindaikaKaishu();
			}
		}

		public void OnTouchHide()
		{
			Back();
		}

		public void Show()
		{
			base.transform.SetActive(isActive: false);
			base.transform.SetActive(isActive: true);
			mTransform_TouchBack.SetActive(isActive: true);
			mPanelThis.widgetsAreStatic = true;
			StartStaticWidgetChildren();
			RemodelUtils.MoveWithManual(base.gameObject, showPos, ANIMATION_DURATION, delegate
			{
				isShown = true;
				mPanelThis.widgetsAreStatic = false;
				StopStaticWidgetChildren();
			});
		}

		public void Hide()
		{
			RemoveFocus();
			Hide(animation: true);
		}

		public void Hide(bool animation)
		{
			StartStaticWidgetChildren();
			SetKeyController(null);
			mTransform_TouchBack.SetActive(isActive: false);
			isShown = false;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.gameObject, hidePos, ANIMATION_DURATION, delegate
				{
					StopStaticWidgetChildren();
				});
				return;
			}
			base.transform.localPosition = hidePos;
			StopStaticWidgetChildren();
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		protected override void OnSelect(UIRemodeModernizationShipTargetListChildNew view)
		{
			if (!isShown || base.mState != ListState.Waiting)
			{
				return;
			}
			RemoveFocus();
			ShipModel selectedShipModel = null;
			if (view != null && view.GetModel() != null)
			{
				switch (view.GetModel().mOption)
				{
				case RemodeModernizationShipTargetListChildNew.ListItemOption.Model:
					selectedShipModel = view.GetModel().mShipModel;
					break;
				case RemodeModernizationShipTargetListChildNew.ListItemOption.UnSet:
					selectedShipModel = null;
					break;
				}
			}
			Hide();
			UserInterfaceRemodelManager.instance.SelectKindaikaKaishuSozai(selectedShipModel);
		}

		protected override void OnCallDestroy()
		{
			mTransform_TouchBack = null;
			mMessage = null;
			mUIShipSortButton = null;
			mKeyController = null;
			mTargetExchangeShipModel = null;
		}

		public CommonShipBanner[] GetBanner()
		{
			List<CommonShipBanner> list = new List<CommonShipBanner>();
			UIRemodeModernizationShipTargetListChildNew[] mViews = base.mViews;
			foreach (UIRemodeModernizationShipTargetListChildNew uIRemodeModernizationShipTargetListChildNew in mViews)
			{
				CommonShipBanner shipBanner = uIRemodeModernizationShipTargetListChildNew.GetShipBanner();
				list.Add(shipBanner);
			}
			return list.ToArray();
		}
	}
}
