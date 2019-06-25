using KCV.Scene.Port;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Item
{
	public class UIItemStoreManager : MonoBehaviour
	{
		private enum State
		{
			NONE,
			ItemStoreList,
			ItemStoreBuyConfirm
		}

		[SerializeField]
		private Camera mCamera_SwipeEvent;

		[SerializeField]
		private UIButton mButton_ChangeItemList;

		[SerializeField]
		private UIItemStoreBuyConfirm mUIItemStoreBuyConfirm;

		[SerializeField]
		private UIItemStoreChildScrollView mUIItemStoreChildren;

		private ItemStoreManager mItemStoreManager;

		private KeyControl mKeyController;

		private Stack<State> mStateStack = new Stack<State>();

		private Action mOnSwitchItemListener;

		private Action mOnItemStoreBackListener;

		private State CurrentState
		{
			get
			{
				if (0 < mStateStack.Count)
				{
					return mStateStack.Peek();
				}
				return State.NONE;
			}
		}

		private IEnumerator Start()
		{
			mUIItemStoreBuyConfirm.SetOnBuyStartCallBack(OnBuyStart);
			mUIItemStoreBuyConfirm.SetOnBuyCancelCallBack(OnBuyCancel);
			mUIItemStoreChildren.SetOnSelectListener(OnSelectItemStoreChild);
			yield return new WaitForEndOfFrame();
		}

		private void OnSelectItemStoreChild(UIItemStoreChild view)
		{
			if (mItemStoreManager.IsValidBuy(view.GetModel().MstId, 1))
			{
				mUIItemStoreChildren.SetKeyController(null);
				mUIItemStoreChildren.LockControl();
				mUIItemStoreBuyConfirm.Initialize(view.GetModel(), mItemStoreManager);
				mUIItemStoreBuyConfirm.Show(null);
				mKeyController.ClearKeyAll();
				mKeyController.firstUpdate = true;
				mUIItemStoreBuyConfirm.SetKeyController(mKeyController);
				ChangeState(State.ItemStoreBuyConfirm, popStack: false);
			}
			else if (mItemStoreManager.UserInfo.SPoint < view.GetModel().Price)
			{
				CommonPopupDialog.Instance.StartPopup("戦略ポイントが不足しています");
			}
			else
			{
				CommonPopupDialog.Instance.StartPopup("保有上限を超えています");
			}
		}

		private void Update()
		{
			if (mKeyController != null)
			{
				if (mKeyController.IsRDown() && SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable)
				{
					OnBack();
				}
				else if (mKeyController.IsRSLeftDown())
				{
					SwitchToItemList();
				}
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_ChangeItemList);
			mCamera_SwipeEvent = null;
			mUIItemStoreBuyConfirm = null;
			mUIItemStoreChildren = null;
			mItemStoreManager = null;
			mKeyController = null;
		}

		private void OnBuyStart(ItemStoreModel itemStoreModel, int count)
		{
			if (CurrentState != State.ItemStoreBuyConfirm)
			{
				return;
			}
			if (mItemStoreManager.IsValidBuy(itemStoreModel.MstId, count))
			{
				int fuel = mItemStoreManager.Material.Fuel;
				int ammo = mItemStoreManager.Material.Ammo;
				int baux = mItemStoreManager.Material.Baux;
				int steel = mItemStoreManager.Material.Steel;
				if (mItemStoreManager.BuyItem(itemStoreModel.MstId, count))
				{
					int fuel2 = mItemStoreManager.Material.Fuel;
					int ammo2 = mItemStoreManager.Material.Ammo;
					int baux2 = mItemStoreManager.Material.Baux;
					int steel2 = mItemStoreManager.Material.Steel;
					if (fuel != fuel2 || ammo != ammo2 || baux != baux2 || steel != steel2)
					{
						TrophyUtil.Unlock_Material();
					}
					TrophyUtil.Unlock_AlbumSlotNum();
				}
				mUIItemStoreChildren.Refresh(mItemStoreManager.Items.ToArray());
				if (SingletonMonoBehaviour<UIPortFrame>.exist())
				{
					SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(mItemStoreManager);
				}
			}
			else if (mItemStoreManager.UserInfo.SPoint >= itemStoreModel.Price)
			{
				CommonPopupDialog.Instance.StartPopup("保有上限に達しています");
			}
			mUIItemStoreBuyConfirm.Close(null);
			mUIItemStoreBuyConfirm.SetKeyController(null);
			PopState();
		}

		private void OnBuyCancel()
		{
			if (CurrentState == State.ItemStoreBuyConfirm)
			{
				mUIItemStoreBuyConfirm.Close(null);
				mUIItemStoreBuyConfirm.SetKeyController(null);
				PopState();
			}
		}

		public void SwitchToItemList()
		{
			if (CurrentState == State.ItemStoreList)
			{
				OnSwitchToItemList();
			}
		}

		private void OnSwitchToItemList()
		{
			if (mOnSwitchItemListener != null)
			{
				mOnSwitchItemListener();
			}
		}

		public void Initialize(ItemStoreManager manager)
		{
			mItemStoreManager = manager;
			ItemStoreModel[] itemStoreModels = mItemStoreManager.Items.ToArray();
			mUIItemStoreChildren.Initialize(manager, itemStoreModels, mCamera_SwipeEvent);
		}

		public void StartState()
		{
			mStateStack.Clear();
			ChangeState(State.ItemStoreList, popStack: false);
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
			mUIItemStoreChildren.SetKeyController(mKeyController);
		}

		public void LockControl()
		{
			mUIItemStoreChildren.LockControl();
		}

		public void SetOnSwitchItemListListener(Action onSwitchItemListListener)
		{
			mOnSwitchItemListener = onSwitchItemListListener;
		}

		public void SetOnBackListener(Action onItemStoreBackListener)
		{
			mOnItemStoreBackListener = onItemStoreBackListener;
		}

		private void OnBack()
		{
			if (mOnItemStoreBackListener != null)
			{
				mOnItemStoreBackListener();
			}
		}

		private void ChangeState(State state, bool popStack)
		{
			if (popStack && 0 < mStateStack.Count)
			{
				PopState();
			}
			mStateStack.Push(state);
			OnPushState(mStateStack.Peek());
		}

		private void PopState()
		{
			if (0 < mStateStack.Count)
			{
				State state = mStateStack.Pop();
				OnPopState(state);
				if (0 < mStateStack.Count)
				{
					OnResumeState(mStateStack.Peek());
				}
			}
		}

		private void OnPushState(State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			switch (state)
			{
			case State.ItemStoreList:
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				ItemStoreModel[] itemStoreModels = mItemStoreManager.Items.ToArray();
				mUIItemStoreChildren.Initialize(mItemStoreManager, itemStoreModels, mCamera_SwipeEvent);
				mUIItemStoreChildren.SetKeyController(mKeyController);
				mUIItemStoreChildren.StartState();
				break;
			}
			case State.ItemStoreBuyConfirm:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				break;
			}
		}

		private void OnPopState(State state)
		{
			if (state == State.ItemStoreList)
			{
				OnPopStateItemStoreList();
			}
		}

		private void OnPopStateItemStoreList()
		{
			mUIItemStoreChildren.SetKeyController(mKeyController);
		}

		private void OnResumeState(State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			switch (state)
			{
			case State.ItemStoreList:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				OnResumeStateItemStoreList();
				break;
			case State.ItemStoreBuyConfirm:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				break;
			}
		}

		private void OnResumeStateItemStoreList()
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mUIItemStoreChildren.SetKeyController(mKeyController);
			mUIItemStoreChildren.ResumeState();
		}

		public string StateToString()
		{
			mStateStack.ToArray();
			string text = string.Empty;
			foreach (State item in mStateStack)
			{
				text = item + " > " + text;
			}
			return text;
		}

		public void Release()
		{
			mKeyController = null;
			mItemStoreManager = null;
			mUIItemStoreBuyConfirm.Release();
			mStateStack.Clear();
		}
	}
}
