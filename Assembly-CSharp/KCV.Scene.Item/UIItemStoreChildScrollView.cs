using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Item
{
	public class UIItemStoreChildScrollView : UIScrollList<ItemStoreModel, UIItemStoreChild>
	{
		[SerializeField]
		private Transform mTransform_ArrowUp;

		[SerializeField]
		private Transform mTransform_ArrowDown;

		private ItemStoreManager mItemStoreManager;

		private Action<UIItemStoreChild> mOnSelectListener;

		private KeyControl mKeyController;

		protected override void OnAwake()
		{
			base.OnAwake();
		}

		public void Initialize(ItemStoreManager itemStoreManager, ItemStoreModel[] itemStoreModels, Camera touchEventCatchCamera)
		{
			mTransform_ArrowDown.SetActive(isActive: false);
			mTransform_ArrowUp.SetActive(isActive: false);
			base.ChangeImmediateContentPosition(ContentDirection.Hell);
			mItemStoreManager = itemStoreManager;
			Initialize(itemStoreModels);
			SetSwipeEventCamera(touchEventCatchCamera);
		}

		protected override void OnUpdate()
		{
			if (mKeyController != null && base.mState == ListState.Waiting)
			{
				if (mKeyController.keyState[8].down)
				{
					PrevFocus();
				}
				else if (mKeyController.keyState[12].down)
				{
					NextFocus();
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
				else if (mKeyController.keyState[0].down)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
				}
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchPrev()
		{
			if (base.mState == ListState.Waiting)
			{
				PrevFocus();
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchNext()
		{
			if (base.mState == ListState.Waiting)
			{
				NextFocus();
			}
		}

		public void StartState()
		{
			StartControl();
			HeadFocus();
		}

		public void ResumeState()
		{
			StartControl();
		}

		protected override void OnSelect(UIItemStoreChild view)
		{
			if (mOnSelectListener != null)
			{
				mOnSelectListener(view);
			}
		}

		public void SetOnSelectListener(Action<UIItemStoreChild> onSelectListener)
		{
			mOnSelectListener = onSelectListener;
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public new void LockControl()
		{
			base.LockControl();
		}

		public void Refresh(ItemStoreModel[] itemStoreModels)
		{
			mModels = itemStoreModels;
			RefreshViews();
		}

		protected override bool OnSelectable(UIItemStoreChild view)
		{
			bool flag = view.GetModel().Count == 0;
			if (flag)
			{
				CommonPopupDialog.Instance.StartPopup("売り切れです");
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			}
			return !flag;
		}

		protected override void OnChangedFocusView(UIItemStoreChild focusToView)
		{
			CommonPopupDialog.Instance.StartPopup(focusToView.GetRealIndex() + 1 + "/" + mModels.Length, 0, CommonPopupDialogMessage.PlayType.Short);
			if (mCurrentFocusView.GetRealIndex() == 0)
			{
				mTransform_ArrowUp.SetActive(isActive: false);
			}
			else
			{
				mTransform_ArrowUp.SetActive(isActive: true);
			}
			if (mCurrentFocusView.GetRealIndex() == mModels.Length - 1)
			{
				mTransform_ArrowDown.SetActive(isActive: false);
			}
			else
			{
				mTransform_ArrowDown.SetActive(isActive: true);
			}
			SoundUtils.PlaySE(SEFIleInfos.SE_014);
		}

		protected override void OnCallDestroy()
		{
			base.OnCallDestroy();
			mItemStoreManager = null;
			mKeyController = null;
			mOnSelectListener = null;
			mTransform_ArrowDown = null;
			mTransform_ArrowUp = null;
		}
	}
}
