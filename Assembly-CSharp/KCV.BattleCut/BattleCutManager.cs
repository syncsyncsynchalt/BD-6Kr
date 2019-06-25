using Common.Enum;
using KCV.SortieBattle;
using local.managers;
using local.models;
using LT.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.BattleCut
{
	public class BattleCutManager : MonoBehaviour
	{
		private static BattleCutManager instance;

		[SerializeField]
		private BattleCutTitle titleText;

		[SerializeField]
		private UIPanel bgPanel;

		[SerializeField]
		private BtlCut_Live2D _btlCutLive2D;

		[SerializeField]
		private Transform _sharedPlace;

		[SerializeField]
		private BaseCamera _camera;

		[SerializeField]
		private UIBattleCutNavigation _uiNavigation;

		[SerializeField]
		private BattleCutPrefabFile _clsBattleCutPrefabFile;

		private KeyControl _clsInput;

		private StatementMachine _clsState;

		private static MapManager _clsMapManger;

		private static BattleManager _clsBattleManager;

		private static BattleCutPhase _iNowPhase;

		private static BattleData _clsBattleData;

		private static List<BaseBattleCutState> _listBattleCutState;

		private static Action _actOnFinished;

		private static Action _actOnStartFadeOut;

		private static Action<ShipRecoveryType> _actOnFinishedRecoveryType;

		private LTDescr test;

		private static BattleCutManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = UnityEngine.Object.FindObjectOfType<BattleCutManager>();
				}
				return instance;
			}
			set
			{
				instance = value;
			}
		}

		public static BattleCutManager Instantiate(BattleCutManager prefab, Vector3 worldPosition)
		{
			BattleCutManager battleCutManager = UnityEngine.Object.Instantiate(prefab);
			battleCutManager.transform.localScaleOne();
			battleCutManager.transform.position = worldPosition;
			return battleCutManager;
		}

		private void Awake()
		{
			App.TimeScale(1f);
			_clsInput = ((SortieBattleTaskManager.GetKeyControl() == null) ? new KeyControl() : SortieBattleTaskManager.GetKeyControl());
			_clsState = new StatementMachine();
			Util.SetRootContentSize(GetComponent<UIRoot>(), App.SCREEN_RESOLUTION);
			bgPanel.widgetsAreStatic = true;
			_btlCutLive2D.panel.alpha = 0f;
			_listBattleCutState = new List<BaseBattleCutState>();
			_listBattleCutState.Add(new StateBattleCutFormationSelect());
			_listBattleCutState.Add(new StateBattleCutCommand());
			_listBattleCutState.Add(new StateBattleCutBattle());
			_listBattleCutState.Add(new StateBattleCutWithdrawalDecision());
			_listBattleCutState.Add(new StateBattleCutJudge());
			_listBattleCutState.Add(new StateBattleCutResult());
			_listBattleCutState.Add(new StateBattleCutAdvancingWithdrawal());
			_listBattleCutState.Add(new StateBattleCutAdvancingWithdrawalDC());
			_listBattleCutState.Add(new StateBattleCutClearReward());
			_listBattleCutState.Add(new StateBattleCutMapOpen());
			_listBattleCutState.Add(new StateBattleCutFlagshipWreck());
			_listBattleCutState.Add(new StateBattleCutEscortShipEvacuation());
			_uiNavigation.Startup(new SettingModel());
			_uiNavigation.panel.depth = 100;
		}

		private void OnDestroy()
		{
			Mem.Del(ref titleText);
			Mem.Del(ref bgPanel);
			Mem.Del(ref _btlCutLive2D);
			Mem.Del(ref _sharedPlace);
			Mem.Del(ref _camera);
			_clsBattleCutPrefabFile.Dispose();
			_clsBattleCutPrefabFile.UnInit();
			Mem.Del(ref _clsBattleCutPrefabFile);
			Mem.Del(ref _clsInput);
			if (_clsState != null)
			{
				_clsState.Clear();
			}
			Mem.Del(ref _clsState);
			Mem.Del(ref _clsMapManger);
			Mem.Del(ref _clsBattleManager);
			if (_clsBattleData != null)
			{
				_clsBattleData.UnInit();
			}
			Mem.Del(ref _clsBattleData);
			if (_listBattleCutState != null)
			{
				_listBattleCutState.Clear();
			}
			Mem.DelListSafe(ref _listBattleCutState);
			Mem.Del(ref _actOnFinished);
			Mem.Del(ref _actOnStartFadeOut);
			Mem.Del(ref _actOnFinishedRecoveryType);
			Mem.Del(ref instance);
		}

		private void Update()
		{
			if (SortieBattleTaskManager.GetKeyControl() == null)
			{
				_clsInput.Update();
			}
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
		}

		public void StartBattleCut(MapManager manager, Action onStartFadeOut, Action<ShipRecoveryType> onFinished)
		{
			_actOnStartFadeOut = onStartFadeOut;
			_actOnFinishedRecoveryType = onFinished;
			_clsMapManger = manager;
			ReqPhase(BattleCutPhase.BattleCutPhase_ST);
			_btlCutLive2D.shipCharacter.ChangeCharacter(manager.Deck.GetFlagShip());
			Transform parent = _btlCutLive2D.shipCharacter.transform.parent;
			Vector3 enterPosition = _btlCutLive2D.shipCharacter.getEnterPosition2();
			parent.localPositionX(enterPosition.x);
			_btlCutLive2D.shipCharacter.transform.parent.localScale = Vector3.one * 1.1f;
		}

		public BattleCutManager StartBattleCut(PracticeManager manager, int enemyDeckID, BattleFormationKinds1 iKind, Action onFinished)
		{
			_actOnFinished = onFinished;
			_clsBattleManager = manager.StartBattlePractice(enemyDeckID, iKind);
			StartBattle(iKind);
			_btlCutLive2D.shipCharacter.ChangeCharacter(manager.CurrentDeck.GetFlagShip());
			return this;
		}

		public static void StartBattle(BattleFormationKinds1 formationKind)
		{
			if (!(_clsBattleManager is PracticeBattleManager))
			{
				if (_clsMapManger is SortieMapManager)
				{
					_clsBattleManager = ((SortieMapManager)_clsMapManger).BattleStart(formationKind);
				}
				else
				{
					_clsBattleManager = ((RebellionMapManager)_clsMapManger).BattleStart(formationKind);
				}
			}
			switch (_clsBattleManager.WarType)
			{
			case enumMapWarType.None:
				Dlg.Call(ref _actOnFinished);
				break;
			case enumMapWarType.Normal:
				ReqPhase(BattleCutPhase.Command);
				break;
			case enumMapWarType.Midnight:
				ReqPhase(BattleCutPhase.NightBattle);
				break;
			case enumMapWarType.Night_To_Day:
				ReqPhase(BattleCutPhase.NightBattle);
				break;
			case enumMapWarType.AirBattle:
				ReqPhase(BattleCutPhase.Command);
				break;
			}
		}

		public static void EndBattleCut()
		{
			List<UIPanel> panels = new List<UIPanel>(from x in Instance.GetComponentsInChildren<UIPanel>()
				where x.alpha != 0f
				select x);
			Instance.transform.LTValue(1f, 0f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panels.ForEach(delegate(UIPanel y)
				{
					y.alpha = x;
				});
			})
				.setOnComplete((Action)delegate
				{
					Instance.SetActive(isActive: false);
					Dlg.Call(ref _actOnFinished);
				});
		}

		public static void EndBattleCut(ShipRecoveryType iType)
		{
			Dlg.Call(ref _actOnStartFadeOut);
			List<UIPanel> panels = new List<UIPanel>(from x in Instance.GetComponentsInChildren<UIPanel>()
				where x.alpha != 0f
				select x);
			Instance.transform.LTValue(1f, 0f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panels.ForEach(delegate(UIPanel y)
				{
					y.alpha = x;
				});
			})
				.setOnComplete((Action)delegate
				{
					Instance.SetActive(isActive: false);
					Dlg.Call(ref _actOnFinishedRecoveryType, iType);
				});
		}

		public static void ReqPhase(BattleCutPhase NextPhase)
		{
			CheckNextBattleState(NextPhase);
			_iNowPhase = ((NextPhase != BattleCutPhase.NightBattle) ? NextPhase : BattleCutPhase.DayBattle);
			StatementMachine clsState = Instance._clsState;
			BaseBattleCutState baseBattleCutState = _listBattleCutState[(int)_iNowPhase];
			StatementMachine.StatementMachineInitialize init = baseBattleCutState.Init;
			BaseBattleCutState baseBattleCutState2 = _listBattleCutState[(int)_iNowPhase];
			StatementMachine.StatementMachineUpdate update = baseBattleCutState2.Run;
			BaseBattleCutState baseBattleCutState3 = _listBattleCutState[(int)_iNowPhase];
			clsState.AddState(init, update, baseBattleCutState3.Terminate);
			SetTitleText(NextPhase);
		}

		private static void CheckNextBattleState(BattleCutPhase iPhase)
		{
			switch (iPhase)
			{
			case BattleCutPhase.DayBattle:
			{
				StateBattleCutBattle stateBattleCutBattle2 = _listBattleCutState[2] as StateBattleCutBattle;
				stateBattleCutBattle2.isNightCombat = false;
				break;
			}
			case BattleCutPhase.NightBattle:
			{
				StateBattleCutBattle stateBattleCutBattle = _listBattleCutState[2] as StateBattleCutBattle;
				stateBattleCutBattle.isNightCombat = true;
				break;
			}
			}
		}

		public static KeyControl GetKeyControl()
		{
			return Instance._clsInput;
		}

		public static StateBattleCutBattle GetStateBattle()
		{
			return (StateBattleCutBattle)_listBattleCutState[2];
		}

		public static BattleCutPhase GetNowPhase()
		{
			return _iNowPhase;
		}

		public static Generics.BattleRootType GetBattleType()
		{
			if (_clsBattleManager is SortieBattleManager)
			{
				return Generics.BattleRootType.SortieMap;
			}
			if (_clsBattleManager is RebellionBattleManager)
			{
				return Generics.BattleRootType.Rebellion;
			}
			return Generics.BattleRootType.Practice;
		}

		public static BaseCamera GetCamera()
		{
			return Instance._camera;
		}

		public static MapManager GetMapManager()
		{
			return _clsMapManger;
		}

		public static BattleManager GetBattleManager()
		{
			return _clsBattleManager;
		}

		public static BattleCutPrefabFile GetPrefabFile()
		{
			return Instance._clsBattleCutPrefabFile;
		}

		public static BattleData GetBattleData()
		{
			if (_clsBattleData == null)
			{
				_clsBattleData = new BattleData();
			}
			return _clsBattleData;
		}

		public static Transform GetSharedPlase()
		{
			return Instance._sharedPlace;
		}

		public static BtlCut_Live2D GetLive2D()
		{
			return Instance._btlCutLive2D;
		}

		public static UIBattleCutNavigation GetNavigation()
		{
			return Instance._uiNavigation;
		}

		public static void SetTitleText(BattleCutPhase phase)
		{
			Instance.titleText.SetPhaseText(phase);
		}

		public static void Discard()
		{
			UnityEngine.Object.Destroy(Instance.gameObject);
		}
	}
}
