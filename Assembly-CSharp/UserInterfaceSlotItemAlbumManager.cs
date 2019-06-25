using DG.Tweening;
using KCV;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceSlotItemAlbumManager : MonoBehaviour
{
	public enum State
	{
		None,
		SlotItemList,
		SlotItemDetail
	}

	private class Context
	{
		private IAlbumModel mAlbumModel;

		public IAlbumModel GetAlbumModel()
		{
			return mAlbumModel;
		}

		public void SetAlbumModel(IAlbumModel albumModel)
		{
			mAlbumModel = albumModel;
		}
	}

	private class StateManager<State>
	{
		private Stack<State> mStateStack;

		private State mEmptyState;

		public Action<State> OnPush
		{
			private get;
			set;
		}

		public Action<State> OnPop
		{
			private get;
			set;
		}

		public Action<State> OnResume
		{
			private get;
			set;
		}

		public Action<State> OnSwitch
		{
			private get;
			set;
		}

		public State CurrentState
		{
			get
			{
				if (0 < mStateStack.Count)
				{
					return mStateStack.Peek();
				}
				return mEmptyState;
			}
		}

		public StateManager(State emptyState)
		{
			mEmptyState = emptyState;
			mStateStack = new Stack<State>();
		}

		public void PushState(State state)
		{
			mStateStack.Push(state);
			Notify(OnPush, mStateStack.Peek());
			Notify(OnSwitch, mStateStack.Peek());
		}

		public void ReplaceState(State state)
		{
			if (0 < mStateStack.Count)
			{
				PopState();
			}
			mStateStack.Push(state);
			Notify(OnPush, mStateStack.Peek());
			Notify(OnSwitch, mStateStack.Peek());
		}

		public void PopState()
		{
			if (0 < mStateStack.Count)
			{
				State state = mStateStack.Pop();
				Notify(OnPop, state);
			}
		}

		public void ResumeState()
		{
			if (0 < mStateStack.Count)
			{
				Notify(OnResume, mStateStack.Peek());
				Notify(OnSwitch, mStateStack.Peek());
			}
		}

		public override string ToString()
		{
			mStateStack.ToArray();
			string text = string.Empty;
			foreach (State item in mStateStack)
			{
				text = item + " > " + text;
			}
			return text;
		}

		private void Notify(Action<State> target, State state)
		{
			target?.Invoke(state);
		}
	}

	[SerializeField]
	private UISlotItemAlbumList mUISlotItemAlbumList;

	[SerializeField]
	private UISlotItemAlbumDetail mUISlotItemAlbumDetail;

	private BGMFileInfos DefaultBGM = BGMFileInfos.Port;

	private KeyControl mKeyController;

	private IAlbumModel[] mAlbumModels;

	private StateManager<State> mStateManager;

	private Context mContext;

	private Action mOnBackListener;

	private Action<State> mOnChangeStateUserInterfaceSlotItemAlbumManager;

	public bool Initialized
	{
		get;
		private set;
	}

	public void Initialize(IAlbumModel[] albumModels)
	{
		mAlbumModels = albumModels;
		mUISlotItemAlbumList.SetOnSelectedListItemListener(OnSelectedListItemListener);
		mUISlotItemAlbumList.SetOnBackListener(OnBackListListener);
		mUISlotItemAlbumDetail.SetActive(isActive: false);
		mUISlotItemAlbumDetail.SetOnBackListener(OnBackAlbumDetailListener);
		mStateManager = new StateManager<State>(State.None);
		mStateManager.OnPush = OnPushState;
		mStateManager.OnPop = OnPopState;
		mStateManager.OnResume = OnResumeState;
		mStateManager.OnSwitch = OnChangeState;
		Initialized = true;
	}

	private void OnBackAlbumDetailListener(Tween closeTween)
	{
		if (mStateManager.CurrentState == State.SlotItemDetail)
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mUISlotItemAlbumDetail.SetKeyController(null);
			closeTween.OnComplete(delegate
			{
				mStateManager.PopState();
				mStateManager.ResumeState();
				mUISlotItemAlbumDetail.SetActive(isActive: false);
			});
		}
	}

	private void OnBackListListener()
	{
		if (mStateManager.CurrentState == State.SlotItemList)
		{
			mStateManager.PopState();
			OnBack();
		}
	}

	public void SetOnBackListener(Action onBack)
	{
		mOnBackListener = onBack;
	}

	private void OnBack()
	{
		if (mOnBackListener != null)
		{
			mOnBackListener();
		}
	}

	private void OnSelectedListItemListener(IAlbumModel albumModel)
	{
		if (mStateManager.CurrentState == State.SlotItemList && albumModel is AlbumSlotModel)
		{
			AlbumSlotModel albumModel2 = (AlbumSlotModel)albumModel;
			mContext.SetAlbumModel(albumModel2);
			mUISlotItemAlbumList.SetKeyController(null);
			mStateManager.PushState(State.SlotItemDetail);
		}
	}

	public void SetKeyController(KeyControl keyController)
	{
		mKeyController = keyController;
	}

	public void StartState()
	{
		mContext = new Context();
		mStateManager.PushState(State.SlotItemList);
	}

	private void OnPushState(State state)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		switch (state)
		{
		case State.SlotItemList:
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			mUISlotItemAlbumList.SetActive(isActive: true);
			mUISlotItemAlbumList.Initialize(mAlbumModels);
			mUISlotItemAlbumList.SetKeyController(mKeyController);
			mUISlotItemAlbumList.StartState();
			break;
		case State.SlotItemDetail:
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			OnPushSlotItemDetailState();
			break;
		}
	}

	private void OnPushSlotItemDetailState()
	{
		IEnumerator routine = OnPushSlotItemDetailStateCoroutine();
		StartCoroutine(routine);
	}

	private IEnumerator OnPushSlotItemDetailStateCoroutine()
	{
		mUISlotItemAlbumDetail.SetActive(isActive: true);
		yield return new WaitForEndOfFrame();
		mUISlotItemAlbumDetail.Initialize((AlbumSlotModel)mContext.GetAlbumModel());
		yield return new WaitForEndOfFrame();
		mUISlotItemAlbumDetail.SetKeyController(mKeyController);
		mUISlotItemAlbumDetail.Show();
		mUISlotItemAlbumDetail.StartState();
	}

	private void OnPopState(State state)
	{
	}

	private void OnResumeState(State state)
	{
		if (state == State.SlotItemList)
		{
			mUISlotItemAlbumList.SetKeyController(mKeyController);
			mUISlotItemAlbumList.ResumeState();
		}
	}

	private void OnDestroy()
	{
		mUISlotItemAlbumList = null;
		mUISlotItemAlbumDetail = null;
		mKeyController = null;
		mAlbumModels = null;
		mStateManager = null;
		mContext = null;
		mOnChangeStateUserInterfaceSlotItemAlbumManager = null;
	}

	internal void SetOnChangeStateListener(Action<State> onChangeStateUserInterfaceSlotItemAlbumManager)
	{
		mOnChangeStateUserInterfaceSlotItemAlbumManager = onChangeStateUserInterfaceSlotItemAlbumManager;
	}

	private void OnChangeState(State state)
	{
		if (mOnChangeStateUserInterfaceSlotItemAlbumManager != null)
		{
			mOnChangeStateUserInterfaceSlotItemAlbumManager(state);
		}
	}
}
