using Common.Enum;
using local.managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIDeckPracticeManager : MonoBehaviour
	{
		public enum State
		{
			NONE,
			DeckPracticeTypeSelect,
			Production
		}

		[SerializeField]
		private UIPracticeDeckTypeSelect mDeckPracticeTypeSelect;

		private StateManager<State> mStateManager;

		private PracticeManager mPracticeManager;

		private KeyControl mKeyController;

		private Action mOnBackListener;

		private DeckPracticeContext mDeckPracticeContext;

		private Action<State> mOnChangedStateListener;

		private Action<DeckPracticeContext> mOnCommitDeckPracticeListener;

		private void Start()
		{
			mDeckPracticeTypeSelect.SetActive(isActive: false);
			mDeckPracticeTypeSelect.SetOnBackCallBack(OnCancelDeckPracticeTypeSelect);
			mDeckPracticeTypeSelect.SetOnSelectedDeckPracticeTypeCallBack(OnSelectedDeckPracticeType);
		}

		public void Release()
		{
			mPracticeManager = null;
			mDeckPracticeContext = null;
			mStateManager = null;
		}

		public void Initialize(PracticeManager practiceManager)
		{
			mStateManager = new StateManager<State>(State.NONE);
			mStateManager.OnPush = OnPushState;
			mStateManager.OnPop = OnPopState;
			mStateManager.OnResume = OnResumeState;
			mStateManager.OnSwitch = OnChangedState;
			mPracticeManager = practiceManager;
			mDeckPracticeContext = new DeckPracticeContext();
			mDeckPracticeContext.SetFriendDeck(mPracticeManager.CurrentDeck);
		}

		public void StartState()
		{
			mStateManager.PushState(State.DeckPracticeTypeSelect);
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public void SetOnBackListener(Action onBackListener)
		{
			mOnBackListener = onBackListener;
		}

		private void Back()
		{
			mDeckPracticeTypeSelect.Hide(OnBack);
		}

		private void OnBack()
		{
			if (mOnBackListener != null)
			{
				mOnBackListener();
			}
		}

		public void SetOnChangedStateListener(Action<State> onChangedStateListener)
		{
			mOnChangedStateListener = onChangedStateListener;
		}

		private void OnChangedState(State changedState)
		{
			if (mOnChangedStateListener != null)
			{
				mOnChangedStateListener(changedState);
			}
		}

		private void OnPushStateDeckPracticeTypeSelect()
		{
			Dictionary<DeckPracticeType, bool> deckPracticeTypeDic = mPracticeManager.DeckPracticeTypeDic;
			mDeckPracticeTypeSelect.SetActive(isActive: true);
			mDeckPracticeTypeSelect.Initialize(deckPracticeTypeDic);
			mDeckPracticeTypeSelect.Show(null);
			mDeckPracticeTypeSelect.SetKeyController(mKeyController);
		}

		private void OnPopStateDeckPracticeTypeSelect()
		{
			mDeckPracticeTypeSelect.SetKeyController(null);
			Back();
		}

		private void OnPushState(State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			switch (state)
			{
			case State.DeckPracticeTypeSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				OnPushStateDeckPracticeTypeSelect();
				break;
			case State.Production:
				OnPushStateProduction();
				break;
			}
		}

		private void OnPushStateProduction()
		{
			mDeckPracticeTypeSelect.DisableButtonAll();
		}

		private void OnPopState(State state)
		{
			if (state == State.DeckPracticeTypeSelect)
			{
				OnPopStateDeckPracticeTypeSelect();
			}
		}

		private void OnCancelDeckPracticeTypeSelect()
		{
			if (mStateManager.CurrentState == State.DeckPracticeTypeSelect)
			{
				mStateManager.PopState();
			}
		}

		public DeckPracticeContext GetContext()
		{
			return mDeckPracticeContext;
		}

		private void OnSelectedDeckPracticeType(DeckPracticeType selectedType)
		{
			if (mStateManager.CurrentState == State.DeckPracticeTypeSelect)
			{
				if (mPracticeManager.DeckPracticeTypeDic[selectedType])
				{
					mDeckPracticeContext.SetPracticeType(selectedType);
					mDeckPracticeTypeSelect.SetKeyController(null);
					OnCommitDeckPractice(mDeckPracticeContext);
					mStateManager.PushState(State.Production);
				}
				else
				{
					string mes = UserInterfacePracticeManager.Util.DeckPracticeTypeToString(selectedType) + "に参加できる編成ではありません";
					CommonPopupDialog.Instance.StartPopup(mes, 0, CommonPopupDialogMessage.PlayType.Long);
				}
			}
		}

		public void SetOnCommitDeckPracticeListener(Action<DeckPracticeContext> onCommitDeckPracticeListener)
		{
			mOnCommitDeckPracticeListener = onCommitDeckPracticeListener;
		}

		private void OnCommitDeckPractice(DeckPracticeContext context)
		{
			if (mOnCommitDeckPracticeListener != null)
			{
				mOnCommitDeckPracticeListener(context);
			}
		}

		public override string ToString()
		{
			return (mStateManager == null) ? string.Empty : mStateManager.ToString();
		}

		private void OnResumeState(State state)
		{
		}

		private void OnDestroy()
		{
			mDeckPracticeTypeSelect = null;
			mStateManager = null;
			mPracticeManager = null;
			mKeyController = null;
			mOnBackListener = null;
			mDeckPracticeContext = null;
		}
	}
}
