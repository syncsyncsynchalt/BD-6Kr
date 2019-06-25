using Common.Enum;
using KCV.Scene.Port;
using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using local.utils;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodelEquipListParentNew : UIScrollList<SlotitemModel, UIRemodelEquipListChildNew>, UIRemodelView
	{
		[SerializeField]
		private Transform mTransform_TouchBack;

		[SerializeField]
		private UILabel titleLabel;

		[SerializeField]
		private Transform mMessage;

		private KeyControl mKeyController;

		private Vector3 showPos = new Vector3(270f, 250f);

		private Vector3 hidePos = new Vector3(775f, 250f);

		private SlotitemModel mCurrentEquipSlotitemModel;

		private int mSelectedSlotIndex;

		private ShipModel mTargetShipModel;

		private SlotitemCategory slotitemCategory;

		private RemodelManager mRemodelManager;

		private UIRemodelShipStatus mUIRemodelShipStatus;

		private bool isShown;

		protected override void OnAwake()
		{
			base.transform.localPosition = hidePos;
		}

		public void SetSwipeEventCatchCamera(Camera swipeEventCatchCamera)
		{
			SetSwipeEventCamera(swipeEventCatchCamera);
		}

		public void Initialize(KeyControl keyController, UIRemodelShipStatus uiRemodelShipStatus, UIRemodelEquipSlotItems uiRemodelEquipSlotItems, ShipModel targetShipModel, SlotitemCategory slotitemCategory)
		{
			mUIRemodelShipStatus = uiRemodelShipStatus;
			int currentSlotIndex = uiRemodelEquipSlotItems.GetCurrentSlotIndex();
			bool isExSlot = uiRemodelEquipSlotItems.currentFocusItem.isExSlot;
			mTargetShipModel = targetShipModel;
			mSelectedSlotIndex = currentSlotIndex;
			this.slotitemCategory = slotitemCategory;
			mRemodelManager = UserInterfaceRemodelManager.instance.mRemodelManager;
			SetKeyController(keyController);
			SetTitle(slotitemCategory);
			SlotitemModel[] models = CreateModelArray(isExSlot);
			Initialize(models);
			base.gameObject.SetActive(false);
			base.gameObject.SetActive(true);
			ChangeFocusToUserViewHead();
			base.ChangeImmediateContentPosition(ContentDirection.Hell);
			StartControl();
		}

		private void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		private SlotitemModel[] CreateModelArray(bool isExSlot)
		{
			SlotitemModel[] array = isExSlot ? mRemodelManager.GetSlotitemExList(mTargetShipModel.MemId) : mRemodelManager.GetSlotitemList(mTargetShipModel.MemId, slotitemCategory);
			List<SlotitemModel> list = new List<SlotitemModel>(array);
			SlotitemUtil.Sort(list, SlotitemUtil.SlotitemSortKey.Type3);
			if (array.Length == 0)
			{
				mMessage.localScale = Vector3.one;
			}
			else
			{
				mMessage.localScale = Vector3.zero;
			}
			return list.ToArray();
		}

		private void SetTitle(SlotitemCategory category)
		{
			string str = string.Empty;
			switch (category)
			{
			case SlotitemCategory.Syuhou:
				str = "主砲";
				break;
			case SlotitemCategory.Fukuhou:
				str = "副砲";
				break;
			case SlotitemCategory.Gyorai:
				str = "魚雷";
				break;
			case SlotitemCategory.Kiju:
				str = "機銃";
				break;
			case SlotitemCategory.Kanjouki:
				str = "艦上機";
				break;
			case SlotitemCategory.Suijouki:
				str = "水上機";
				break;
			case SlotitemCategory.Dentan:
				str = "電探";
				break;
			case SlotitemCategory.Other:
				str = "その他";
				break;
			}
			titleLabel.text = "装備選択\u3000- " + str + " -";
		}

		protected override void OnUpdate()
		{
			if (mKeyController != null && base.mState == ListState.Waiting && isShown)
			{
				if (mKeyController.IsShikakuDown())
				{
					SwitchLockItem();
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
				else if (mKeyController.IsBatuDown())
				{
					Back();
				}
				else if (mKeyController.IsMaruDown())
				{
					Select();
				}
			}
		}

		public void Show()
		{
			mTransform_TouchBack.SetActive(isActive: true);
			StartStaticWidgetChildren();
			base.gameObject.SetActive(false);
			base.gameObject.SetActive(true);
			RemodelUtils.MoveWithManual(base.gameObject, showPos, 0.2f, delegate
			{
				isShown = true;
				StopStaticWidgetChildren();
			});
			if (mKeyController != null)
			{
				mKeyController.ClearKeyAll();
				mKeyController.firstUpdate = true;
				StartControl();
			}
		}

		public void Hide()
		{
			Hide(animation: true);
		}

		public void Hide(bool animation)
		{
			mTransform_TouchBack.SetActive(isActive: false);
			LockControl();
			StartStaticWidgetChildren();
			isShown = false;
			if (animation)
			{
				StartStaticWidgetChildren();
				RemodelUtils.MoveWithManual(base.gameObject, hidePos, 0.2f, delegate
				{
					StopStaticWidgetChildren();
					base.gameObject.SetActive(false);
				});
			}
			else
			{
				StopStaticWidgetChildren();
				base.transform.localPosition = hidePos;
				base.gameObject.SetActive(false);
			}
		}

		public void OnTouchHide()
		{
			if (isShown)
			{
				Back();
			}
		}

		protected override void OnSelect(UIRemodelEquipListChildNew child)
		{
			if (isShown && base.mState == ListState.Waiting && !(child == null) && child.GetModel() != null)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				UserInterfaceRemodelManager.instance.Forward2SoubiHenkouPreview(mTargetShipModel, mSelectedSlotIndex, child);
				Hide();
			}
		}

		private void Back()
		{
			if (isShown)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				Hide();
				UserInterfaceRemodelManager.instance.Back2SoubiHenkouTypeSelect();
			}
		}

		protected override void OnChangedFocusView(UIRemodelEquipListChildNew focusToView)
		{
			if (base.mState == ListState.Waiting)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		private void SwitchLockItem()
		{
			UserInterfaceRemodelManager.instance.mRemodelManager.SlotLock(mCurrentFocusView.GetModel().MemId);
			mCurrentFocusView.SwitchLockedIcon(Change: false);
			RefreshViews();
		}

		public void OnCancel()
		{
			Back();
		}

		internal void Release()
		{
			mTransform_TouchBack = null;
			titleLabel = null;
			mUIRemodelShipStatus = null;
			mCurrentEquipSlotitemModel = null;
			mTargetShipModel = null;
			mRemodelManager = null;
		}

		protected override void OnCallDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref titleLabel);
			mTransform_TouchBack = null;
			mMessage = null;
			mKeyController = null;
			mCurrentEquipSlotitemModel = null;
			mTargetShipModel = null;
			mRemodelManager = null;
			mUIRemodelShipStatus = null;
		}
	}
}
