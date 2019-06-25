using Common.Enum;
using KCV.Utils;
using KCV.View.ScrollView;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodelOtherShipPickerParentNew : UIScrollList<ShipModel, UIRemodelOtherShipPickerChildNew>, UIRemodelView, IBannerResourceManage
	{
		[SerializeField]
		private Collider2D TouchBackArea;

		[SerializeField]
		private UIShipSortButton mUIShipSortButton;

		private KeyControl mKeyController;

		private Vector3 showPos = new Vector3(233f, 250f);

		private Vector3 hidePos = new Vector3(800f, 250f);

		private bool mCallFirstInitialized;

		private bool isShown;

		protected override void OnAwake()
		{
			base.transform.localPosition = hidePos;
		}

		public void Initialize(KeyControl keyController)
		{
			UIPanel component = ((Component)mTransform_ContentPosition.parent).GetComponent<UIPanel>();
			if (component != null)
			{
				component.clipping = UIDrawCall.Clipping.None;
			}
			if (!mCallFirstInitialized)
			{
				mUIShipSortButton.SetSortKey(SortKey.LEVEL);
				mCallFirstInitialized = true;
			}
			mUIShipSortButton.SetOnSortedShipsListener(OnSortedShipsListener);
			SetKeyController(keyController);
			ShipModel[] otherShipList = UserInterfaceRemodelManager.instance.mRemodelManager.GetOtherShipList();
			mUIShipSortButton.SetClickable(clickable: true);
			if (mModels == null)
			{
				Initialize(otherShipList);
				mUIShipSortButton.Initialize(otherShipList);
			}
			else
			{
				mUIShipSortButton.Initialize(otherShipList);
			}
			mUIShipSortButton.ReSort();
			ChangeFocusToUserViewHead();
		}

		protected override void OnUpdate()
		{
			if (base.mState == ListState.Waiting && isShown && mKeyController != null)
			{
				if (mKeyController.IsSankakuDown())
				{
					mUIShipSortButton.OnClickSortButton();
				}
				else if (mKeyController.IsDownDown())
				{
					NextFocus();
				}
				else if (mKeyController.IsUpDown())
				{
					PrevFocus();
				}
				else if (mKeyController.IsLeftDown())
				{
					PrevPageOrHeadFocus();
				}
				else if (mKeyController.IsRightDown())
				{
					NextPageOrTailFocus();
				}
				else if (mKeyController.IsMaruDown())
				{
					Select();
				}
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		private void OnSortedShipsListener(ShipModel[] shipModels)
		{
			base.ChangeImmediateContentPosition(ContentDirection.Hell);
			Refresh(shipModels, firstPage: true);
		}

		public void Refresh(ShipModel ship)
		{
			ShipModel[] otherShipList = UserInterfaceRemodelManager.instance.mRemodelManager.GetOtherShipList();
			mUIShipSortButton.RefreshModels(otherShipList);
			mUIShipSortButton.ReSort();
			ChangePageFromModel(ship);
		}

		public new void SetSwipeEventCamera(Camera camera)
		{
			base.SetSwipeEventCamera(camera);
		}

		private void Start()
		{
			Hide(animation: false);
		}

		public void Show()
		{
			base.RefreshViews();
			base.gameObject.SetActive(false);
			base.gameObject.SetActive(true);
			base.enabled = true;
			StartStaticWidgetChildren();
			RemodelUtils.MoveWithManual(base.gameObject, showPos, 0.2f, delegate
			{
				isShown = true;
				StopStaticWidgetChildren();
			});
			TouchBackArea.enabled = true;
			StartControl();
			ChangeShip(mCurrentFocusView.GetModel());
		}

		public void Hide()
		{
			Hide(animation: true);
			LockControl();
		}

		public void Hide(bool animation)
		{
			isShown = false;
			StartStaticWidgetChildren();
			base.enabled = false;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.gameObject, hidePos, 0.2f, delegate
				{
					StopStaticWidgetChildren();
				}).PlayForward();
			}
			else
			{
				base.transform.localPosition = hidePos;
			}
			TouchBackArea.enabled = false;
		}

		private void ChangeShip(ShipModel model)
		{
			if (model != null && base.enabled)
			{
				UserInterfaceRemodelManager.instance.ChangeFocusShip(model);
			}
		}

		private void Forward()
		{
			Hide();
			UserInterfaceRemodelManager.instance.Forward2ModeSelect();
		}

		protected override void OnSelect(UIRemodelOtherShipPickerChildNew view)
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			Forward();
		}

		protected override void OnChangedFocusView(UIRemodelOtherShipPickerChildNew focusToView)
		{
			if (base.mState == ListState.Waiting && base.gameObject.activeSelf)
			{
				int num = Array.IndexOf(mModels, focusToView.GetModel());
				int num2 = mModels.Length;
				CommonPopupDialog.Instance.StartPopup(num + 1 + "/" + num2);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				ChangeShip(focusToView.GetModel());
			}
		}

		protected override int GetModelIndex(ShipModel model)
		{
			if (model == null)
			{
				return -1;
			}
			int num = 0;
			ShipModel[] mModels = base.mModels;
			foreach (ShipModel shipModel in mModels)
			{
				if (shipModel != null && shipModel.MemId == model.MemId)
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		protected override bool EqualsModel(ShipModel targetA, ShipModel targetB)
		{
			if (targetA == null)
			{
				return false;
			}
			if (targetB == null)
			{
				return false;
			}
			return targetA.MemId == targetB.MemId;
		}

		protected override void OnCallDestroy()
		{
			TouchBackArea = null;
			mUIShipSortButton = null;
		}

		internal new void RefreshViews()
		{
			base.RefreshViews();
		}

		public CommonShipBanner[] GetBanner()
		{
			List<CommonShipBanner> list = new List<CommonShipBanner>();
			UIRemodelOtherShipPickerChildNew[] mViews = base.mViews;
			foreach (UIRemodelOtherShipPickerChildNew uIRemodelOtherShipPickerChildNew in mViews)
			{
				CommonShipBanner banner = uIRemodelOtherShipPickerChildNew.GetBanner();
				list.Add(banner);
			}
			return list.ToArray();
		}
	}
}
