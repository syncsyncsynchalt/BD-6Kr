using KCV.Scene.Others;
using KCV.Utils;
using local.managers;
using local.models;
using local.utils;
using System;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIInteriorStoreManager : MonoBehaviour
	{
		public enum State
		{
			NONE,
			CategoryTabSelect,
			ListFurnitureSelect,
			PreviewFurniture,
			FurnitureDetailDialog
		}

		public enum Mode
		{
			TabSelect,
			ListSelect,
			StoreDialog,
			Preview
		}

		private Context mContext;

		[SerializeField]
		private Transform mUIInteriorStoreManagerContents;

		[SerializeField]
		private InteriorStoreTabManager tabManager;

		[SerializeField]
		private UIFurnitureStoreTabList mUIFurnitureStoreTabList;

		[SerializeField]
		private UIFurniturePurchaseDialog storeDialog;

		[SerializeField]
		private InteriorStoreFrame storeFrame;

		[SerializeField]
		private UIInteriorFurniturePreviewWaiter mUIInteriorFurniturePreviewWaiter;

		[SerializeField]
		private UserInterfacePortInteriorManager mUserInterfacePortInteriorManager;

		private KeyControl mKeyController;

		private StateManager<State> mStateManager;

		private InteriorManager mInteriorManager;

		private FurnitureStoreManager mFurnitureStoreManager;

		private Action mOnRequestMoveToInteriorListener;

		private void OnPushState(State state)
		{
			switch (state)
			{
			case State.CategoryTabSelect:
				OnPushStateCategoryTabSelect();
				break;
			case State.ListFurnitureSelect:
				OnPushStateListFurnitureSelect();
				break;
			case State.PreviewFurniture:
				OnPushStatePreviewFurniture();
				break;
			case State.FurnitureDetailDialog:
				OnPushStateDetailDialog();
				break;
			}
		}

		private void OnPushStatePreviewFurniture()
		{
			mUIInteriorStoreManagerContents.SetActive(isActive: false);
			mUserInterfacePortInteriorManager.SetActive(isActive: true);
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			FurnitureModel selectedFurniture = mContext.SelectedFurniture;
			mUserInterfacePortInteriorManager.UpdateFurniture(mInteriorManager.Deck, selectedFurniture.Type, selectedFurniture);
			mUIInteriorFurniturePreviewWaiter.SetKeyController(mKeyController);
			mUIInteriorFurniturePreviewWaiter.StartWait();
		}

		private void OnPushStateDetailDialog()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			FurnitureModel selectedFurniture = mContext.SelectedFurniture;
			bool isValidBuy = mFurnitureStoreManager.IsValidExchange(selectedFurniture);
			storeDialog.Initialize(selectedFurniture, isValidBuy);
			storeDialog.SetKeyController(mKeyController);
			storeDialog.Show();
		}

		private void OnPushStateListFurnitureSelect()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mUIFurnitureStoreTabList.SetKeyController(mKeyController);
			mUIFurnitureStoreTabList.ResumeControl();
		}

		private void OnPushStateCategoryTabSelect()
		{
			if (mStateManager.CurrentState == State.CategoryTabSelect)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				tabManager.StartState();
			}
		}

		private void OnResumeState(State state)
		{
			switch (state)
			{
			case State.CategoryTabSelect:
				OnResumeStateCategoryTabSelect();
				break;
			case State.ListFurnitureSelect:
				OnResumeStateListFurnitureSelect();
				break;
			case State.PreviewFurniture:
				OnResumeStatePreviewFurniture();
				break;
			case State.FurnitureDetailDialog:
				OnResumeFurnitureDetailDialog();
				break;
			}
		}

		private void OnResumeStateCategoryTabSelect()
		{
			tabManager.ResumeState();
		}

		private void OnResumeStatePreviewFurniture()
		{
			mUserInterfacePortInteriorManager.UpdateFurniture(mInteriorManager.Deck, mContext.SelectedCategory, mInteriorManager.GetRoomInfo()[mContext.SelectedCategory]);
		}

		private void OnResumeFurnitureDetailDialog()
		{
			mUIInteriorStoreManagerContents.SetActive(isActive: true);
			mUserInterfacePortInteriorManager.SetActive(isActive: false);
			storeDialog.SetKeyController(mKeyController);
			storeDialog.ResumeFocus();
		}

		private void OnResumeStateListFurnitureSelect()
		{
			mUIFurnitureStoreTabList.ResumeControl();
		}

		private void OnPopState(State state)
		{
			switch (state)
			{
			case State.FurnitureDetailDialog:
				break;
			case State.CategoryTabSelect:
				tabManager.PopState();
				break;
			case State.ListFurnitureSelect:
				mUIFurnitureStoreTabList.LockControl();
				break;
			case State.PreviewFurniture:
				OnResumeStatePreviewFurniture();
				break;
			}
		}

		private void Update()
		{
			if (mKeyController != null && mStateManager != null && mStateManager.CurrentState == State.CategoryTabSelect)
			{
				if (mKeyController.IsRightDown())
				{
					tabManager.NextTab();
				}
				else if (mKeyController.IsLeftDown())
				{
					tabManager.PrevTab();
				}
				else if (mKeyController.IsMaruDown())
				{
					OnDesideTabListener();
				}
				else if (mKeyController.IsBatuDown())
				{
					RequestMoveToPort();
				}
				else if (mKeyController.IsRSLeftDown())
				{
					RequestMoveToInterior();
				}
			}
		}

		public void Initialize(InteriorManager interiorManager, FurnitureStoreManager furnitureStoreManager, UserInterfacePortInteriorManager uiPortInteriorManager)
		{
			mInteriorManager = interiorManager;
			mUserInterfacePortInteriorManager = uiPortInteriorManager;
			mFurnitureStoreManager = furnitureStoreManager;
			mUIFurnitureStoreTabList.Initialize(mFurnitureStoreManager);
			tabManager.InitTab();
			tabManager.Init(OnChangedTabListener, OnDesideTabListener);
			storeFrame.updateUserInfo(mFurnitureStoreManager);
			mUserInterfacePortInteriorManager.InitializeFurnituresForConfirmation(interiorManager.Deck, interiorManager.GetRoomInfo());
		}

		private void OnSelectedFurnitureListener(UIFurnitureStoreTabListChild selectedView)
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mUIFurnitureStoreTabList.LockControl();
			FurnitureModel model = selectedView.GetModel();
			mContext.SetSelectedFurniture(model);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			mStateManager.PushState(State.FurnitureDetailDialog);
		}

		private void OnBackListListener()
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mUIFurnitureStoreTabList.LockControl();
			mStateManager.PopState();
			mStateManager.ResumeState();
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public void StartState()
		{
			mContext = new Context();
			mStateManager = new StateManager<State>(State.NONE);
			mStateManager.OnPush = OnPushState;
			mStateManager.OnPop = OnPopState;
			mStateManager.OnResume = OnResumeState;
			storeDialog.SetOnSelectNegativeListener(OnSelectNegative);
			storeDialog.SetOnSelectPositiveListener(OnSelectPositive);
			storeDialog.SetOnSelectPreviewListener(OnSelectPreview);
			mUIFurnitureStoreTabList.SetOnBackListener(OnBackListListener);
			mUIFurnitureStoreTabList.SetOnSelectedFurnitureListener(OnSelectedFurnitureListener);
			mUIInteriorFurniturePreviewWaiter.SetOnBackListener(OnBackFromPreview);
			mStateManager.PushState(State.CategoryTabSelect);
		}

		public void SetOnRequestMoveToInteriorListener(Action onRequestMoveToInteriorListener)
		{
			mOnRequestMoveToInteriorListener = onRequestMoveToInteriorListener;
		}

		private void RequestMoveToInterior()
		{
			if (mOnRequestMoveToInteriorListener != null)
			{
				mOnRequestMoveToInteriorListener();
			}
		}

		private void RequestMoveToPort()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
		}

		public void OnChangedTabListener()
		{
			if (mStateManager.CurrentState == State.ListFurnitureSelect)
			{
				mKeyController.ClearKeyAll();
				mKeyController.firstUpdate = true;
				tabManager.changeNowCategory();
				mContext.SetSelectedCategory(tabManager.GetCurrentCategory());
				mUIFurnitureStoreTabList.ChangeCategory(mContext.SelectedCategory);
				mUIFurnitureStoreTabList.StopFocusBlink();
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				mStateManager.PopState();
				mStateManager.ResumeState();
			}
			else if (mStateManager.CurrentState == State.CategoryTabSelect)
			{
				mKeyController.ClearKeyAll();
				mKeyController.firstUpdate = true;
				tabManager.changeNowCategory();
				mContext.SetSelectedCategory(tabManager.GetCurrentCategory());
				mUIFurnitureStoreTabList.ChangeCategory(mContext.SelectedCategory);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		public void SetSwipeEventCamera(Camera camera)
		{
			mUIFurnitureStoreTabList.SetSwipeEventCamera(camera);
		}

		public void OnDesideTabListener()
		{
			if (mStateManager.CurrentState != State.ListFurnitureSelect)
			{
				mKeyController.ClearKeyAll();
				mKeyController.firstUpdate = true;
				tabManager.hideUnselectTabs();
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				mStateManager.PushState(State.ListFurnitureSelect);
			}
		}

		private void OnRequestChangeListMode()
		{
			OnDesideTabListener();
		}

		[Obsolete("Inspector上で設定して使用します。")]
		public void OnTouchSwitchToFurniture()
		{
			RequestMoveToInterior();
		}

		private void OnSelectPositive()
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			FurnitureModel selectedFurniture = mContext.SelectedFurniture;
			if (mFurnitureStoreManager.IsValidExchange(selectedFurniture) && mFurnitureStoreManager.Exchange(selectedFurniture))
			{
				TrophyUtil.Unlock_At_BuyFurniture();
				SoundUtils.PlaySE(SEFIleInfos.SE_004);
				storeFrame.updateUserInfo(mFurnitureStoreManager);
				storeDialog.Hide();
				storeDialog.SetKeyController(null);
				mUIFurnitureStoreTabList.Refresh();
				mStateManager.PopState();
				mStateManager.ResumeState();
			}
		}

		private void OnSelectNegative()
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			mContext.SetSelectedFurniture(null);
			storeDialog.SetKeyController(null);
			storeDialog.Hide();
			mStateManager.PopState();
			mStateManager.ResumeState();
		}

		private void OnSelectPreview()
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			FurnitureModel selectedFurniture = mContext.SelectedFurniture;
			mStateManager.PushState(State.PreviewFurniture);
			storeDialog.SetKeyController(null);
		}

		private void OnBackFromPreview()
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mStateManager.PopState();
			mStateManager.ResumeState();
		}

		private void OnDestroy()
		{
			mFurnitureStoreManager = null;
		}
	}
}
