using Common.Enum;
using DG.Tweening;
using KCV.BattleCut;
using KCV.Scene.Port;
using KCV.SortieMap;
using KCV.Strategy;
using KCV.Utils;
using local.managers;
using local.models;
using Server_Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIPanel))]
	public class UserInterfacePracticeManager : MonoBehaviour
	{
		public static class DebugUtils
		{
			public static void Debug_OpenAllDeckPractice()
			{
				Debug_Mod.DeckPracticeMenu_StateChange(DeckPracticeType.Hou, state: true);
				Debug_Mod.DeckPracticeMenu_StateChange(DeckPracticeType.Kouku, state: true);
				Debug_Mod.DeckPracticeMenu_StateChange(DeckPracticeType.Normal, state: true);
				Debug_Mod.DeckPracticeMenu_StateChange(DeckPracticeType.Rai, state: true);
				Debug_Mod.DeckPracticeMenu_StateChange(DeckPracticeType.Sougou, state: true);
				Debug_Mod.DeckPracticeMenu_StateChange(DeckPracticeType.Taisen, state: true);
			}
		}

		public static class Util
		{
			public static string IsGoConditionToString(IsGoCondition condition)
			{
				switch (condition)
				{
				case IsGoCondition.ConditionRed:
					return "疲労しています";
				case IsGoCondition.NeedSupply:
					return "補給が必要な艦娘がいます";
				case IsGoCondition.ActionEndDeck:
					return "行動終了しています";
				case IsGoCondition.FlagShipTaiha:
					return "旗艦が大破しています";
				case IsGoCondition.HasRepair:
					return "修復中の艦娘がいます";
				case IsGoCondition.Mission:
					return "遠征中です";
				case IsGoCondition.Invalid:
					return "戦う相手がいません";
				default:
					return string.Empty;
				}
			}

			public static string DeckPracticeTypeToString(DeckPracticeType deckPracticeType)
			{
				switch (deckPracticeType)
				{
				case DeckPracticeType.Normal:
					return "艦隊行動";
				case DeckPracticeType.Hou:
					return "砲戦";
				case DeckPracticeType.Kouku:
					return "航空戦";
				case DeckPracticeType.Rai:
					return "雷撃戦";
				case DeckPracticeType.Sougou:
					return "総合";
				case DeckPracticeType.Taisen:
					return "対潜戦";
				default:
					return string.Empty;
				}
			}
		}

		public enum State
		{
			NONE,
			PracticeTypeSelect,
			BattlePractice,
			DeckPractice,
			DeckPracticeProd,
			BattlePracticeProd
		}

		private UIPanel mPanelThis;

		[SerializeField]
		private Texture[] mTextures_Preload;

		[SerializeField]
		private UIPracticeMenu mPracticeMenu;

		[SerializeField]
		private UIPracticeHeader mPracticeHeader;

		[SerializeField]
		private Transform mTransform_PracticeDeckPlayer;

		[SerializeField]
		private BattleCutManager mPrefab_BattleCutManager;

		[SerializeField]
		private UIBattlePracticeManager mUIBattlePracticeManager;

		[SerializeField]
		private UIDeckPracticeManager mUIDeckPracticeManager;

		[SerializeField]
		private UIDeckPracticeProductionManager mUIDeckPracticeProductionManager;

		private KeyControl mKeyController;

		private PracticeManager mPracticeManager;

		private Action mOnBackCallBack;

		private StateManager<State> mStateManager;

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
		}

		private void OnPushState(State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			switch (state)
			{
			case State.PracticeTypeSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				OnPushStatePracticeTypeSelect();
				break;
			case State.BattlePractice:
				OnPushStateBattlePractice();
				break;
			case State.DeckPractice:
				OnPushStateDeckPractice();
				break;
			}
		}

		private void OnPushStateDeckPractice()
		{
			mUIDeckPracticeManager.SetActive(isActive: true);
			mUIDeckPracticeManager.Release();
			mUIDeckPracticeManager.Initialize(mPracticeManager);
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mUIDeckPracticeManager.SetKeyController(mKeyController);
			mUIDeckPracticeManager.StartState();
			mPracticeMenu.MoveToButtonCenterFocus(delegate
			{
			});
		}

		private void OnPushStateBattlePractice()
		{
			mUIBattlePracticeManager.SetActive(isActive: true);
			mUIBattlePracticeManager.Initialize(mPracticeManager);
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mUIBattlePracticeManager.SetKeyController(mKeyController);
			mUIBattlePracticeManager.StartState();
			mPracticeMenu.MoveToButtonCenterFocus(delegate
			{
			});
		}

		private void OnPopState(State state)
		{
			if (state == State.PracticeTypeSelect)
			{
				OnPopStatePracticeTypeSelect();
			}
		}

		private void OnResumeState(State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			if (state == State.PracticeTypeSelect)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				OnResumeStatePracticeTypeSelect();
			}
		}

		public void SetOnBackCallBack(Action action)
		{
			mOnBackCallBack = action;
		}

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			mStateManager = new StateManager<State>(State.NONE);
			mStateManager.OnPop = OnPopState;
			mStateManager.OnPush = OnPushState;
			mStateManager.OnResume = OnResumeState;
			mPracticeManager = new PracticeManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id);
			mKeyController = new KeyControl();
			mPracticeMenu.SetOnSelectedCallBack(OnSelectedPracticeMenu);
			mUIBattlePracticeManager.SetOnChangedStateListener(OnBattlePracticeManagerOnChangedStateListener);
			mUIBattlePracticeManager.SetOnBackListener(BackFromBattlePracticeSetting);
			mUIBattlePracticeManager.SetOnCommitBattleListener(OnCommitBattleStart);
			mUIDeckPracticeManager.SetOnChangedStateListener(OnDeckPracticeManagerOnChangedStateListener);
			mUIDeckPracticeManager.SetOnBackListener(BackFromDeckPracticeSetting);
			mUIDeckPracticeManager.SetOnCommitDeckPracticeListener(OnCommitDeckPracticeStart);
			mUIDeckPracticeProductionManager.SetOnFinishedProduction(OnFinishedDeckPracticeStartProduction);
			Vector3 localPosition = mPracticeHeader.transform.localPosition;
			float headerDefaultLocalPositionY = localPosition.y;
			mPracticeHeader.transform.localPositionY(320f);
			yield return new WaitForEndOfFrame();
			mPracticeHeader.transform.DOLocalMoveY(headerDefaultLocalPositionY, 0.5f);
			mStateManager.PushState(State.PracticeTypeSelect);
		}

		private void Update()
		{
			if (mKeyController != null)
			{
				mKeyController.Update();
				if (SingletonMonoBehaviour<UIShortCutMenu>.Instance != null && SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable && mKeyController.IsRDown())
				{
					mKeyController.ClearKeyAll();
					mKeyController.firstUpdate = true;
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
					mKeyController = null;
				}
			}
		}

		private void BackFromBattlePracticeSetting()
		{
			mStateManager.PopState();
			mStateManager.ResumeState();
		}

		private void BackFromDeckPracticeSetting()
		{
			mStateManager.PopState();
			mStateManager.ResumeState();
		}

		private void OnDeckPracticeManagerOnChangedStateListener(UIDeckPracticeManager.State state)
		{
			if (state == UIDeckPracticeManager.State.DeckPracticeTypeSelect)
			{
				mPracticeHeader.UpdateHeaderText("どの演習を行いますか？");
			}
		}

		private void OnBattlePracticeManagerOnChangedStateListener(UIBattlePracticeManager.State state)
		{
			switch (state)
			{
			case UIBattlePracticeManager.State.NONE:
				break;
			case UIBattlePracticeManager.State.BattlePracticeProd:
				mPracticeHeader.transform.DOKill();
				mPracticeHeader.transform.DOLocalMoveY(320f, 0.3f);
				break;
			case UIBattlePracticeManager.State.BattlePracticeTargetAlertConfirm:
				mPracticeHeader.UpdateHeaderText(string.Empty);
				break;
			case UIBattlePracticeManager.State.BattlePracticeTargetConfirm:
				mPracticeHeader.UpdateHeaderText(string.Empty);
				break;
			case UIBattlePracticeManager.State.BattlePracticeTargetSelect:
				mPracticeHeader.UpdateHeaderText("演習相手を選んでください");
				break;
			case UIBattlePracticeManager.State.FormationSelect:
				mPracticeHeader.UpdateHeaderText("陣形を選んでください");
				break;
			}
		}

		private void OnSelectedPracticeMenu(UIPracticeMenu.SelectType selectType)
		{
			if (mStateManager.CurrentState != State.PracticeTypeSelect)
			{
				return;
			}
			switch (selectType)
			{
			case UIPracticeMenu.SelectType.BattlePractice:
			{
				if (mPracticeManager.IsValidBattlePractice())
				{
					mPracticeMenu.SetKeyController(null);
					mStateManager.PushState(State.BattlePractice);
					break;
				}
				DeckModel currentDeck2 = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
				List<IsGoCondition> list2 = mPracticeManager.IsValidPractice(currentDeck2.Id);
				if (0 < list2.Count())
				{
					string mes2 = Util.IsGoConditionToString(list2[0]);
					CommonPopupDialog.Instance.StartPopup(mes2);
				}
				else
				{
					string mes3 = "演習可能な艦隊がありません";
					CommonPopupDialog.Instance.StartPopup(mes3);
				}
				break;
			}
			case UIPracticeMenu.SelectType.DeckPractice:
			{
				if (mPracticeManager.IsValidDeckPractice())
				{
					mPracticeMenu.SetKeyController(null);
					mStateManager.PushState(State.DeckPractice);
					break;
				}
				DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
				List<IsGoCondition> list = mPracticeManager.IsValidPractice(currentDeck.Id);
				string mes = Util.IsGoConditionToString(list[0]);
				CommonPopupDialog.Instance.StartPopup(mes);
				break;
			}
			case UIPracticeMenu.SelectType.Back:
				mStateManager.PopState();
				break;
			}
		}

		private void OnResumeStatePracticeTypeSelect()
		{
			mPracticeHeader.UpdateHeaderText("演習選択");
			mPracticeMenu.MoveToButtonDefaultFocus(delegate
			{
				mKeyController.ClearKeyAll();
				mKeyController.firstUpdate = true;
				mPracticeMenu.SetKeyController(mKeyController);
			});
		}

		private void OnPushStatePracticeTypeSelect()
		{
			StartCoroutine(OnPushStatePracticeTypeSelectCoroutine());
		}

		private IEnumerator OnPushStatePracticeTypeSelectCoroutine()
		{
			DeckModel friendDeckModel = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			mPracticeHeader.UpdateHeaderText("演習選択");
			mPracticeMenu.SetActive(isActive: true);
			mPracticeMenu.Initialize(friendDeckModel);
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mPracticeMenu.SetKeyController(mKeyController);
			yield return new WaitForEndOfFrame();
		}

		private void OnPopStatePracticeTypeSelect()
		{
			mPracticeMenu.SetKeyController(null);
			TweenAlpha.Begin(base.gameObject, 0.2f, 0f);
			StrategyTaskManager.SceneCallBack();
		}

		private void OnCommitBattleStart(BattlePracticeContext context)
		{
			StartCoroutine(OnCommitBattleStartCoroutine(context));
		}

		private IEnumerator OnCommitBattleStartCoroutine(BattlePracticeContext context)
		{
			yield return new WaitForEndOfFrame();
			if (context.BattleStartType == BattlePracticeContext.PlayType.Battle)
			{
				mPracticeHeader.UpdateHeaderText(string.Empty);
				RetentionData.SetData(new Hashtable
				{
					{
						"rootType",
						Generics.BattleRootType.Practice
					},
					{
						"areaId",
						SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID
					},
					{
						"practiceManager",
						mPracticeManager
					},
					{
						"formation",
						context.FormationType
					},
					{
						"deckID",
						context.TargetDeck.Id
					}
				});
				yield return new WaitForEndOfFrame();
				GameObject prefab = Resources.Load("Prefabs/SortieMap/SortieTransitionToBattle/ProdSortieTransitionToBattle") as GameObject;
				ProdSortieTransitionToBattle smokeProduction = ProdSortieTransitionToBattle.Instantiate(prefab.GetComponent<ProdSortieTransitionToBattle>(), base.transform);
				yield return new WaitForEndOfFrame();
				bool animationFinished = false;
				smokeProduction.Play(delegate
				{
                    animationFinished = true;
				});
				while (!animationFinished)
				{
					yield return new WaitForEndOfFrame();
				}
				if (SingletonMonoBehaviour<PortObjectManager>.exist())
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.ManualRelease();
					UnityEngine.Object.Destroy(SingletonMonoBehaviour<PortObjectManager>.Instance.gameObject);
					SingletonMonoBehaviour<PortObjectManager>.Instance = null;
				}
				yield return new WaitForEndOfFrame();
				SingletonMonoBehaviour<AppInformation>.Instance.NextLoadType = AppInformation.LoadType.White;
				SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Battle;
				Application.LoadLevel("LoadingScene");
			}
			else
			{
				mPracticeHeader.UpdateHeaderText(string.Empty);
				PracticeManager practiceManager = new PracticeManager(context.FriendDeck.Id);
				BattleCutManager bcm = BattleCutManager.Instantiate(mPrefab_BattleCutManager, new Vector3(20f, 0f));
				yield return new WaitForEndOfFrame();
				bcm.StartBattleCut(practiceManager, context.TargetDeck.Id, context.FormationType, delegate
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.Strategy);
				});
			}
		}

		private void OnDeckPracticeProductionStateChangeListener(UIDeckPracticeProductionManager.State state)
		{
			if (state == UIDeckPracticeProductionManager.State.EndOfPractice)
			{
				StrategyTopTaskManager.Instance.UIModel.Character.GetWidth();
				int num = 960;
				StrategyTopTaskManager.Instance.UIModel.Character.transform.DOLocalMoveX(num, 0.5f).SetEase(Ease.InCubic);
				DeckPracticeContext context = mUIDeckPracticeManager.GetContext();
				string empty = string.Empty;
				string text = Util.DeckPracticeTypeToString(context.PracticeType) + "演習-結果";
				mPracticeHeader.UpdateHeaderText(text);
			}
		}

		private void OnCommitDeckPracticeStart(DeckPracticeContext context)
		{
			IEnumerator routine = OnCommitDeckPracticeStartCoroutine(context);
			StartCoroutine(routine);
		}

		private IEnumerator OnCommitDeckPracticeStartCoroutine(DeckPracticeContext context)
		{
			DeckPracticeType type = context.PracticeType;
			string practiceTypeToString = Util.DeckPracticeTypeToString(type);
			mPracticeHeader.UpdateHeaderText(practiceTypeToString + "演習\u3000参加中");
			yield return new WaitForEndOfFrame();
			DeckPracticeResultModel result = mPracticeManager.StartDeckPractice(context.PracticeType);
			yield return new WaitForEndOfFrame();
			mUIDeckPracticeProductionManager.SetKeyController(mKeyController);
			IEnumerator initializeCoroutine = mUIDeckPracticeProductionManager.InitializeCoroutine(mPracticeManager.CurrentDeck, result);
			yield return StartCoroutine(initializeCoroutine);
			mUIDeckPracticeProductionManager.SetOnChangeStateListener(OnDeckPracticeProductionStateChangeListener);
			IEnumerator playVoiceAndLive2DMotionCoroutine = GeneratePlayVoiceAndLive2DMotionCoroutine(mPracticeManager.CurrentDeck.GetFlagShip(), 14, OnFinishedPlayVoiceAndLive2DMotion);
			yield return StartCoroutine(playVoiceAndLive2DMotionCoroutine);
		}

		private void OnFinishedPlayVoiceAndLive2DMotion()
		{
			StartCoroutine(OnFinishedPlayVoiceAndLive2DMotionCoroutine());
		}

		private IEnumerator OnFinishedPlayVoiceAndLive2DMotionCoroutine()
		{
			mUIDeckPracticeProductionManager.PlayProduction();
			yield return new WaitForEndOfFrame();
			mUIDeckPracticeProductionManager.PlayShipBannerIn();
			yield return new WaitForEndOfFrame();
			Sequence sequence = DOTween.Sequence();
			Tween tweenMoveMenu = mPracticeMenu.transform.DOLocalMoveX(960f, 0.5f).SetEase(Ease.InCubic);
			Tween tweenMoveManager = mUIDeckPracticeManager.transform.DOLocalMoveX(960f, 0.5f).SetEase(Ease.InCubic);
			Vector3 enterPosition = StrategyTopTaskManager.Instance.UIModel.Character.getEnterPosition();
			Tween tweenMoveFlagShip = ShortcutExtensions.DOLocalMoveX(endValue: (int)enterPosition.x, target: StrategyTopTaskManager.Instance.UIModel.Character.transform, duration: 0.5f).SetEase(Ease.InCubic);
			sequence.Append(tweenMoveMenu);
			sequence.Join(tweenMoveManager);
			sequence.Join(tweenMoveFlagShip);
			yield return sequence.WaitForCompletion();
			if (SingletonMonoBehaviour<Live2DModel>.exist())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Play();
			}
		}

		private IEnumerator GeneratePlayVoiceAndLive2DMotionCoroutine(ShipModelMst shipModelMst, int voiceId, Action onFinished)
		{
			SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Battle);
			ShipUtils.PlayShipVoice(shipModelMst, voiceId);
			yield return new WaitForEndOfFrame();
			SingletonMonoBehaviour<Live2DModel>.Instance.PlayOnce();
			bool live = true;
			while (live)
			{
				live = ((!SingletonMonoBehaviour<Live2DModel>.Instance.isLive2DModel) ? SingletonMonoBehaviour<SoundManager>.Instance.isAnyVoicePlaying : (SingletonMonoBehaviour<SoundManager>.Instance.isAnyVoicePlaying || !SingletonMonoBehaviour<Live2DModel>.Instance.IsStop));
				yield return null;
			}
			SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Loop);
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			onFinished();
		}

		private void OnFinishedDeckPracticeStartProduction()
		{
			Application.LoadLevel(Generics.Scene.Strategy.ToString());
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref mTextures_Preload);
			UserInterfacePortManager.ReleaseUtils.Release(ref mPanelThis);
			mPracticeMenu = null;
			mPracticeHeader = null;
			mTransform_PracticeDeckPlayer = null;
			mPrefab_BattleCutManager = null;
			mUIBattlePracticeManager = null;
			mUIDeckPracticeManager = null;
			mUIDeckPracticeProductionManager = null;
			mKeyController = null;
			mPracticeManager = null;
			mOnBackCallBack = null;
		}
	}
}
