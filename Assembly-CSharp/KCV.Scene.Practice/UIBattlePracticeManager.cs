using Common.Enum;
using KCV.Battle.Formation;
using KCV.Scene.Port;
using KCV.Strategy;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIWidget))]
	public class UIBattlePracticeManager : MonoBehaviour
	{
		public enum State
		{
			NONE,
			BattlePracticeTargetSelect,
			FormationSelect,
			BattlePracticeProd,
			BattlePracticeTargetConfirm,
			BattlePracticeTargetAlertConfirm
		}

		private StateManager<State> mStateManager;

		private UIWidget mWidgetThis;

		[SerializeField]
		private UIPracticeBattleList mPracticeBattleTargetSelect;

		[SerializeField]
		private UIPracticeBattleStartProduction mUIPracticeBattleStartProduction;

		[SerializeField]
		private UIPracticeBattleConfirm mPracticeBattleConfirm;

		[SerializeField]
		private CommonDialog mCommonDialog_Dialog;

		[SerializeField]
		private UIBattleFormationKindSelectManager mPrefab_UIBattleFormationKindSelectManager;

		[SerializeField]
		private UIPracticeMenu mPracticeMenu;

		private Camera mCamera_CatchTouchEvent;

		private PracticeManager mPracticeManager;

		private KeyControl mKeyController;

		private Action mOnBackListener;

		private BattlePracticeContext mBattlePracticeContext;

		private Action<State> mOnChangedStateListener;

		private Action<BattlePracticeContext> mOnCommitBattleListener;

		private void OnSwitchState(State changedState)
		{
			if (mOnChangedStateListener != null)
			{
				mOnChangedStateListener(changedState);
			}
		}

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
		}

		private void Start()
		{
			try
			{
				mCamera_CatchTouchEvent = StrategyTaskManager.GetOverViewCamera();
			}
			catch (Exception)
			{
				UnityEngine.Debug.LogError("Strategy's StaticMethod Called Exception ::[" + StackTraceUtility.ExtractStackTrace() + "]");
			}
			mPracticeBattleTargetSelect.SetOnBackCallBack(OnCancelBattleTargetSelect);
			mPracticeBattleTargetSelect.SetOnSelectedDeckListener(OnSelectedBattleTargetDeck);
			mPracticeBattleTargetSelect.SetActive(isActive: false);
			mUIPracticeBattleStartProduction.SetOnAnimationFinishedListener(OnPracticeBattleStartProductionOnFinished);
			mUIPracticeBattleStartProduction.SetActive(isActive: false);
			mPracticeBattleConfirm.SetOnCancelListener(OnCancelSelectedPracticeBattleConfirm);
			mPracticeBattleConfirm.SetOnStartListener(OnStartSelectedPracticeBattleConfirm);
			mPracticeBattleConfirm.SetActive(isActive: false);
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public void SetOnBackListener(Action onBackListener)
		{
			mOnBackListener = onBackListener;
		}

		public void Initialize(PracticeManager practiceManager)
		{
			mStateManager = new StateManager<State>(State.NONE);
			base.transform.localScale = Vector3.one;
			mStateManager.OnPop = OnPopState;
			mStateManager.OnPush = OnPushState;
			mStateManager.OnResume = OnResumeState;
			mStateManager.OnSwitch = OnSwitchState;
			mPracticeManager = practiceManager;
			mBattlePracticeContext = new BattlePracticeContext();
		}

		public void SetOnChangedStateListener(Action<State> onChangedStateListener)
		{
			mOnChangedStateListener = onChangedStateListener;
		}

		public void StartState()
		{
			mStateManager.PushState(State.BattlePracticeTargetSelect);
			mPracticeBattleTargetSelect.Show(delegate
			{
				mKeyController.ClearKeyAll();
				mKeyController.firstUpdate = true;
				mPracticeBattleTargetSelect.SetKeyController(mKeyController);
			});
		}

		private void OnResumeStateBattlePracticeTargetSelect()
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mPracticeBattleTargetSelect.SetKeyController(mKeyController);
		}

		private void OnPushStateBattlePracticeTargetConfirm()
		{
			StartCoroutine(OnPushStateBattlePracticeTargetConfirmCoroutine());
		}

		private IEnumerator OnPushStateBattlePracticeTargetConfirmCoroutine()
		{
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Reset();
			stopWatch.Start();
			mPracticeBattleConfirm.Initialize(mBattlePracticeContext.FriendDeck, mBattlePracticeContext.TargetDeck, matchValid: true);
			yield return new WaitForEndOfFrame();
			mPracticeBattleConfirm.SetActive(isActive: true);
			for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
			{
				yield return new WaitForEndOfFrame();
			}
			mPracticeBattleConfirm.Show(delegate
			{
				this.mPracticeBattleConfirm.SetKeyController(this.mKeyController);
			});
		}

		private void OnPushStateBattlePracticeTargetSelect()
		{
			mPracticeBattleTargetSelect.transform.SetActive(isActive: true);
			mBattlePracticeContext.SetFriendDeck(mPracticeManager.CurrentDeck);
			List<DeckModel> rivalDecks = mPracticeManager.RivalDecks;
			mPracticeBattleTargetSelect.Initialize(rivalDecks, mPracticeManager);
		}

		private void OnPushStateBattlePracticeTargetAlertConfirm()
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mCommonDialog_Dialog.SetActive(isActive: true);
			mCommonDialog_Dialog.setCloseAction(OnClosePracticeBattleAlert);
			mCommonDialog_Dialog.OpenDialog(0);
		}

		private void OnPushStateBattlePracticeProd()
		{
			if (SingletonMonoBehaviour<Live2DModel>.exist())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			}
			mUIPracticeBattleStartProduction.SetActive(isActive: true);
			mUIPracticeBattleStartProduction.Initialize(mBattlePracticeContext.FriendDeck, mBattlePracticeContext.TargetDeck);
			mUIPracticeBattleStartProduction.SetKeyController(mKeyController);
			mUIPracticeBattleStartProduction.Play();
		}

		private void OnPushStateFormationSelect()
		{
			HashSet<BattleFormationKinds1> selectableFormations = DeckUtil.GetSelectableFormations(mBattlePracticeContext.FriendDeck);
			BattleFormationKinds1[] array = (from w in selectableFormations
				where true
				select w).ToArray();
			mPracticeBattleTargetSelect.SetActive(isActive: false);
			mPracticeMenu.SetActive(isActive: false);
			if (1 < array.Count())
			{
				GameObject gameObject = GameObject.Find("Live2DRender");
				if (gameObject != null)
				{
					UIPanel component = gameObject.GetComponent<UIPanel>();
					if (component != null)
					{
						component.depth = 6;
					}
				}
				UIBattleFormationKindSelectManager battleFormationKindSelectManager = Util.Instantiate(mPrefab_UIBattleFormationKindSelectManager.gameObject, base.transform.gameObject).GetComponent<UIBattleFormationKindSelectManager>();
				battleFormationKindSelectManager.Initialize(mCamera_CatchTouchEvent, array, manualUpdate: true);
				battleFormationKindSelectManager.SetOnUIBattleFormationKindSelectManagerAction(delegate(UIBattleFormationKindSelectManager.ActionType actionType, UIBattleFormationKindSelectManager calledObject, UIBattleFormationKind centerView)
				{
					int width2 = StrategyTopTaskManager.Instance.UIModel.Character.GetWidth();
					int num2 = -960 - Mathf.Abs(width2) / 2;
					StrategyTopTaskManager.Instance.UIModel.Character.moveAddCharacterX(num2, 1f, null);
					BattleFormationKinds1 category = centerView.Category;
					mBattlePracticeContext.SetFormationType(category);
					mStateManager.PopState();
					mStateManager.PushState(State.BattlePracticeProd);
					UnityEngine.Object.Destroy(battleFormationKindSelectManager.gameObject);
				});
				mKeyController.ClearKeyAll();
				mKeyController.firstUpdate = true;
				battleFormationKindSelectManager.SetKeyController(mKeyController);
			}
			else
			{
				int width = StrategyTopTaskManager.Instance.UIModel.Character.GetWidth();
				int num = -960 - Mathf.Abs(width) / 2;
				StrategyTopTaskManager.Instance.UIModel.Character.moveAddCharacterX(num, 1f, null);
				mBattlePracticeContext.SetFormationType(BattleFormationKinds1.TanJuu);
				mStateManager.PopState();
				mStateManager.PushState(State.BattlePracticeProd);
			}
		}

		private void OnPopStateBattlePracticeTargetConfirm()
		{
			mPracticeBattleConfirm.SetKeyController(null);
			mKeyController.IsRun = false;
			mPracticeBattleConfirm.Hide(delegate
			{
				mKeyController.IsRun = true;
				mPracticeBattleConfirm.SetActive(isActive: false);
			});
		}

		private void OnPopStateBattlePracticeTargetAlertConfirm()
		{
			mCommonDialog_Dialog.SetActive(isActive: false);
		}

		private void OnPopStateBattlePracticeTargetSelect()
		{
			mPracticeBattleTargetSelect.SetKeyController(null);
			Back();
		}

		private void OnResumeState(State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			switch (state)
			{
			case State.BattlePracticeTargetAlertConfirm:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				break;
			case State.BattlePracticeTargetSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				OnResumeStateBattlePracticeTargetSelect();
				break;
			}
		}

		private void OnBack()
		{
			if (mOnBackListener != null)
			{
				mOnBackListener();
			}
		}

		private void OnCancelBattleTargetSelect()
		{
			if (mStateManager.CurrentState == State.BattlePracticeTargetSelect)
			{
				mStateManager.PopState();
				mStateManager.ResumeState();
			}
		}

		private void OnPopState(State state)
		{
			switch (state)
			{
			case State.FormationSelect:
			case State.BattlePracticeProd:
				break;
			case State.BattlePracticeTargetSelect:
				OnPopStateBattlePracticeTargetSelect();
				break;
			case State.BattlePracticeTargetConfirm:
				OnPopStateBattlePracticeTargetConfirm();
				break;
			case State.BattlePracticeTargetAlertConfirm:
				OnPopStateBattlePracticeTargetAlertConfirm();
				break;
			}
		}

		private void OnCancelSelectedPracticeBattleConfirm()
		{
			if (mStateManager.CurrentState == State.BattlePracticeTargetConfirm)
			{
				mStateManager.PopState();
				mStateManager.ResumeState();
			}
		}

		private void OnStartSelectedPracticeBattleConfirm()
		{
			if (mStateManager.CurrentState == State.BattlePracticeTargetConfirm)
			{
				mPracticeBattleConfirm.SetKeyController(null);
				mStateManager.PopState();
				mStateManager.PushState(State.FormationSelect);
			}
		}

		private void OnPushState(State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			switch (state)
			{
			case State.FormationSelect:
				OnPushStateFormationSelect();
				break;
			case State.BattlePracticeProd:
				OnPushStateBattlePracticeProd();
				break;
			case State.BattlePracticeTargetSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				OnPushStateBattlePracticeTargetSelect();
				break;
			case State.BattlePracticeTargetConfirm:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				OnPushStateBattlePracticeTargetConfirm();
				break;
			case State.BattlePracticeTargetAlertConfirm:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				OnPushStateBattlePracticeTargetAlertConfirm();
				break;
			}
		}

		private void OnClosePracticeBattleAlert()
		{
			if (mStateManager.CurrentState == State.BattlePracticeTargetAlertConfirm)
			{
				mStateManager.PopState();
				mStateManager.ResumeState();
			}
		}

		private void OnPracticeBattleStartProductionOnFinished(bool isShortCutBattleStart)
		{
			mUIPracticeBattleStartProduction.SetKeyController(null);
			if (isShortCutBattleStart)
			{
				mUIPracticeBattleStartProduction.ShowCover();
				mBattlePracticeContext.SetBattleType(BattlePracticeContext.PlayType.ShortCutBattle);
			}
			else
			{
				mBattlePracticeContext.SetBattleType(BattlePracticeContext.PlayType.Battle);
			}
			OnCommitBattleStart(mBattlePracticeContext);
		}

		private void OnCommitBattleStart(BattlePracticeContext context)
		{
			if (mOnCommitBattleListener != null)
			{
				mOnCommitBattleListener(context);
			}
		}

		private void OnSelectedBattleTargetDeck(DeckModel selectedDeck, List<IsGoCondition> conditions)
		{
			if (mStateManager.CurrentState == State.BattlePracticeTargetSelect)
			{
				if (0 == conditions.Count)
				{
					mPracticeBattleTargetSelect.SetKeyController(null);
					mBattlePracticeContext.SetTargetDeck(selectedDeck);
					mBattlePracticeContext.SetConditions(conditions);
					mStateManager.PushState(State.BattlePracticeTargetConfirm);
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup(UserInterfacePracticeManager.Util.IsGoConditionToString(conditions[0]), 0, CommonPopupDialogMessage.PlayType.Long);
				}
			}
		}

		private void Back()
		{
			mPracticeBattleTargetSelect.Hide(OnBack);
		}

		public void SetOnCommitBattleListener(Action<BattlePracticeContext> onCommitBattleStart)
		{
			mOnCommitBattleListener = onCommitBattleStart;
		}

		private void OnDestroy()
		{
			mStateManager = null;
			UserInterfacePortManager.ReleaseUtils.Release(ref mWidgetThis);
			mPracticeBattleTargetSelect = null;
			mUIPracticeBattleStartProduction = null;
			mPracticeBattleConfirm = null;
			mCommonDialog_Dialog = null;
			mPrefab_UIBattleFormationKindSelectManager = null;
			mPracticeMenu = null;
			mCamera_CatchTouchEvent = null;
			mPracticeManager = null;
			mKeyController = null;
			mOnBackListener = null;
			mBattlePracticeContext = null;
			mOnChangedStateListener = null;
			mOnCommitBattleListener = null;
		}

		public override string ToString()
		{
			return (mStateManager == null) ? string.Empty : mStateManager.ToString();
		}
	}
}
