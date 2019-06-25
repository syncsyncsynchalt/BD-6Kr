using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Arsenal
{
	public class ArsenalScrollItemListNew : UIScrollList<ArsenalScrollSlotItemListChoiceModel, ArsenalScrollItemListChildNew>
	{
		[SerializeField]
		private Transform MessageSlot;

		private ArsenalManager mArsenalManager;

		private bool _isFirst = true;

		private int _before_focus;

		private Action mOnBackListener;

		private KeyControl mKeyController;

		private Action<ArsenalScrollItemListChildNew> mOnItemSelectedListener;

		protected override void OnUpdate()
		{
			if (base.mState == ListState.Waiting && mKeyController != null)
			{
				if (mKeyController.IsDownDown())
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
				else if (mKeyController.IsBatuDown())
				{
					Back();
				}
			}
		}

		protected override void OnCallDestroy()
		{
			base.OnCallDestroy();
			MessageSlot = null;
			mArsenalManager = null;
			mOnBackListener = null;
			mKeyController = null;
			mOnItemSelectedListener = null;
		}

		private void SetOnBackListener(Action onBackListener)
		{
			mOnBackListener = onBackListener;
		}

		private void Back()
		{
			if (mOnBackListener != null)
			{
				mOnBackListener();
			}
		}

		protected override void OnChangedFocusView(ArsenalScrollItemListChildNew focusToView)
		{
			if (base.mState == ListState.Waiting && mModels != null && mModels.Length > 0)
			{
				if (!_isFirst && _before_focus != focusToView.GetRealIndex())
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				_isFirst = false;
				_before_focus = focusToView.GetRealIndex();
				CommonPopupDialog.Instance.StartPopup(focusToView.GetRealIndex() + 1 + "/" + mModels.Length, 0, CommonPopupDialogMessage.PlayType.Short);
			}
		}

		public void Message(SlotitemModel[] slotItemModels)
		{
			if (slotItemModels.Length == 0)
			{
				MessageSlot.localScale = Vector3.one;
			}
			else
			{
				MessageSlot.localScale = Vector3.zero;
			}
		}

		internal new void StartControl()
		{
			base.StartControl();
			HeadFocus();
		}

		internal void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		internal new void LockControl()
		{
			base.LockControl();
		}

		public void SetOnSelectedListener(Action<ArsenalScrollItemListChildNew> onItemSelectedListener)
		{
			mOnItemSelectedListener = onItemSelectedListener;
		}

		public SlotitemModel[] GetSlotItemModels()
		{
			List<SlotitemModel> list = new List<SlotitemModel>();
			ArsenalScrollSlotItemListChoiceModel[] mModels = base.mModels;
			foreach (ArsenalScrollSlotItemListChoiceModel arsenalScrollSlotItemListChoiceModel in mModels)
			{
				list.Add(arsenalScrollSlotItemListChoiceModel.GetSlotItemModel());
			}
			return list.ToArray();
		}

		internal void Initialize(ArsenalManager arsenalManager, SlotitemModel[] items, Camera camera)
		{
			_isFirst = true;
			_before_focus = 0;
			mArsenalManager = arsenalManager;
			ArsenalScrollSlotItemListChoiceModel[] models = GenerateModels(arsenalManager, items);
			base.ChangeImmediateContentPosition(ContentDirection.Hell);
			Initialize(models);
			SetSwipeEventCamera(camera);
			Message(items);
		}

		internal void Refresh(SlotitemModel[] items)
		{
			mModels = GenerateModels(mArsenalManager, items);
			RefreshViews();
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
			Message(items);
		}

		protected override void OnSelect(ArsenalScrollItemListChildNew view)
		{
			if (!(view == null) && view.GetModel() != null && mOnItemSelectedListener != null)
			{
				mOnItemSelectedListener(view);
			}
		}

		internal void ResumeControl()
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			base.StartControl();
		}

		private ArsenalScrollSlotItemListChoiceModel[] GenerateModels(ArsenalManager manager, SlotitemModel[] slotItemModels)
		{
			List<ArsenalScrollSlotItemListChoiceModel> list = new List<ArsenalScrollSlotItemListChoiceModel>();
			foreach (SlotitemModel slotitemModel in slotItemModels)
			{
				bool selected = manager.IsSelected(slotitemModel.MemId);
				ArsenalScrollSlotItemListChoiceModel item = new ArsenalScrollSlotItemListChoiceModel(slotitemModel, selected);
				list.Add(item);
			}
			return list.ToArray();
		}

		public void UpdateChoiceModelAndView(int realIndex, SlotitemModel slotItemModel)
		{
			mModels[realIndex] = new ArsenalScrollSlotItemListChoiceModel(slotItemModel, mArsenalManager.IsSelected(slotItemModel.MemId));
			RefreshViews();
		}

		internal void ClearChecked()
		{
			for (int i = 0; i < mModels.Length; i++)
			{
				mModels[i] = new ArsenalScrollSlotItemListChoiceModel(mModels[i].GetSlotItemModel(), selected: false);
			}
			RefreshViews();
		}
	}
}
