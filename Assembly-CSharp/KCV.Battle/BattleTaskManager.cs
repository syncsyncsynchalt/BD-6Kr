using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.SortieBattle;
using KCV.SortieMap;
using KCV.Utils;
using Librarys.State;
using local.managers;
using local.models;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class BattleTaskManager : SceneTaskMono
	{
		private static BattleTaskManager instance;

		[SerializeField]
		private Light _lWorldLight;

		[SerializeField]
		private UIPanel _uiSeparatorLine;

		private static Transform _traStage;

		private static BattleField _clsBattleField;

		private static BattleShips _clsBattleShips;

		private static BattleCameras _clsBattleCameras;

		private static BattleHPGauges _clsBattleHPGauges;

		private static KeyControl _clsInputKey;

		private static Generics.BattleRootType _iRootType;

		private static KCV.Battle.Utils.TimeZone _iTimeZone;

		private static SkyType _iSkyType;

		private static BattlePhase _iPhase;

		private static BattlePhase _iPhaseReq;

		private static Action<ShipRecoveryType> _actOnFinished;

		private static ObserverActionQueue _clsAnimationObserver;

		private static SettingModel _clsSettingModel;

		private static MapManager _clsMapManager;

		private static BattleManager _clsBattleManager;

		private static Tasks _clsTasks;

		private static TaskBattleFleetAdvent _clsTaskFleetAdvent;

		private static TaskBattleBossInsert _clsTaskBossInsert;

		private static TaskBattleDetection _clsTaskDetection;

		private static TaskBattleCommand _clsTaskCommand;

		private static TaskBattleAerialCombat _clsTaskAerialCombat;

		private static TaskBattleAerialCombatSecond _clsTaskAerialCombatSecond;

		private static TaskBattleSupportingFire _clsTaskSupportingFire;

		private static TaskBattleOpeningTorpedoSalvo _clsTaskOpeningTorpedoSalvo;

		private static TaskBattleShelling _clsTaskShelling;

		private static TaskBattleTorpedoSalvo _clsTaskTorpedoSalvo;

		private static TaskBattleWithdrawalDecision _clsTaskWithdrawalDecision;

		private static TaskBattleNightCombat _clsTaskNightCombat;

		private static TaskBattleResult _clsTaskResult;

		private static TaskBattleFlagshipWreck _clsTaskFlagshipWreck;

		private static TaskBattleEscortShipEvacuation _clsTaskEscortShipEvacuation;

		private static TaskBattleAdvancingWithdrawal _clsTaskAdvancingWithdrawal;

		private static TaskBattleAdvancingWithdrawalDC _clsTaskAdvancingWithdrawalDC;

		private static TaskBattleClearReward _clsTaskClearReward;

		private static TaskBattleMapOpen _clsTaskMapOpen;

		[SerializeField]
		private BattleParticleFile _clsBattleParticleFile;

		[SerializeField]
		private BattlePefabFile _clsBattlePrefabFile;

		private static TorpedoHpGauges _clsTorpedoHpGauges;

		private static BattleTaskManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = (UnityEngine.Object.FindObjectOfType(typeof(BattleTaskManager)) as BattleTaskManager);
					if (instance == null)
					{
						return null;
					}
				}
				return instance;
			}
		}

		public static BattleTaskManager Instantiate(BattleTaskManager prefab, Action<ShipRecoveryType> onFinished)
		{
			BattleTaskManager battleTaskManager = UnityEngine.Object.Instantiate(prefab);
			battleTaskManager.transform.localScaleOne();
			battleTaskManager.transform.localPositionZero();
			return battleTaskManager.VirtualCtor(onFinished);
		}

		private BattleTaskManager VirtualCtor(Action<ShipRecoveryType> onFinished)
		{
			_actOnFinished = onFinished;
			return this;
		}

		private new void Awake()
		{
			_clsInputKey = new KeyControl();
			_clsTasks = new Tasks();
			_clsTasks.Init();
			base.transform.GetComponentsInChildren<UIRoot>().ForEach(delegate(UIRoot x)
			{
				Util.SetRootContentSize(x, App.SCREEN_RESOLUTION);
			});
			_clsAnimationObserver = new ObserverActionQueue();
			_clsTaskBossInsert = new TaskBattleBossInsert();
			_clsTaskDetection = new TaskBattleDetection();
			_clsTaskCommand = new TaskBattleCommand();
			_clsTaskAerialCombat = new TaskBattleAerialCombat();
			_clsTaskAerialCombatSecond = new TaskBattleAerialCombatSecond();
			_clsTaskSupportingFire = new TaskBattleSupportingFire();
			_clsTaskOpeningTorpedoSalvo = new TaskBattleOpeningTorpedoSalvo();
			_clsTaskShelling = new TaskBattleShelling();
			_clsTaskTorpedoSalvo = new TaskBattleTorpedoSalvo();
			_clsTaskFlagshipWreck = new TaskBattleFlagshipWreck();
			_clsTaskEscortShipEvacuation = new TaskBattleEscortShipEvacuation();
			_clsTaskClearReward = new TaskBattleClearReward();
			_clsTaskMapOpen = new TaskBattleMapOpen();
			_clsTaskAdvancingWithdrawal = new TaskBattleAdvancingWithdrawal(GotoSortieMap);
			_clsTaskAdvancingWithdrawalDC = new TaskBattleAdvancingWithdrawalDC(GotoSortieMap);
			_clsTaskFleetAdvent = new TaskBattleFleetAdvent();
			_clsTaskWithdrawalDecision = new TaskBattleWithdrawalDecision();
			_clsTaskNightCombat = new TaskBattleNightCombat();
			_clsTaskResult = new TaskBattleResult();
			if (SingletonMonoBehaviour<DontDestroyObject>.Instance != null)
			{
				SingletonMonoBehaviour<DontDestroyObject>.Instance.transform.position = Vector3.up * 9999f;
			}
		}

		private new void Start()
		{
			InitBattleData();
			_clsSettingModel = new SettingModel();
			_iPhase = (_iPhaseReq = BattlePhase.BattlePhase_BEF);
			_traStage = base.transform.FindChild("Stage");
			_clsBattleShips = new BattleShips();
			_clsBattleCameras = new BattleCameras();
			_clsBattleHPGauges = new BattleHPGauges();
			_clsBattleField = base.transform.GetComponentInChildren<BattleField>();
			UICircleHPGauge circleHPGauge = _clsBattlePrefabFile.circleHPGauge;
			UIBattleNavigation battleNavigation = _clsBattlePrefabFile.battleNavigation;
			battleNavigation.panel.depth = 100;
			_clsTorpedoHpGauges = new TorpedoHpGauges();
			_clsBattleShips.Init(GetBattleManager());
			_clsBattleField.ReqTimeZone(GetStartTimeZone(GetBattleManager().WarType), GetSkyType());
			KCV.Utils.SoundUtils.SwitchBGM((BGMFileInfos)GetBattleManager().GetBgmId());
			ProdSortieTransitionToBattle psttb = (SortieBattleTaskManager.GetSortieBattlePrefabFile() != null) ? SortieBattleTaskManager.GetSortieBattlePrefabFile().prodSortieTransitionToBattle : ProdSortieTransitionToBattle.Instantiate(Resources.Load<ProdSortieTransitionToBattle>("Prefabs/SortieMap/SortieTransitionToBattle/ProdSortieTransitionToBattle"), _clsBattleCameras.cutInCamera.transform).QuickFadeInInit();
			Observable.FromCoroutine((UniRx.IObserver<float> observer) => InitBattle(observer)).Subscribe(delegate(float x)
			{
				if (x == 1f)
				{
					_iPhase = (_iPhaseReq = BattlePhase.FleetAdvent);
					Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate
					{
						_clsBattleField.AlterWaveDirection(FleetType.Friend);
						psttb.Play(ProdSortieTransitionToBattle.AnimationName.ProdSortieTransitionToBattleFadeIn, delegate
						{
							if (SortieBattleTaskManager.GetSortieBattlePrefabFile() != null)
							{
								SortieBattleTaskManager.GetSortieBattlePrefabFile().DisposeProdSortieTransitionToBattle();
							}
							else
							{
								UnityEngine.Object.Destroy(psttb.gameObject);
							}
							if (SortieBattleTaskManager.GetTransitionCamera() != null)
							{
								SortieBattleTaskManager.GetTransitionCamera().enabled = false;
							}
							Mem.Del(ref psttb);
						});
					});
				}
			}).AddTo(base.gameObject);
		}

		private void OnDestroy()
		{
			Mem.Del(ref _lWorldLight);
			Mem.Del(ref _clsBattleField);
			Mem.DelIDisposableSafe(ref _clsBattleParticleFile);
			Mem.DelIDisposableSafe(ref _clsBattlePrefabFile);
			Mem.Del(ref _clsTorpedoHpGauges);
			Mem.DelIDisposableSafe(ref _clsBattleShips);
			Mem.DelIDisposableSafe(ref _clsBattleCameras);
			Mem.DelIDisposableSafe(ref _clsBattleHPGauges);
			Mem.Del(ref _clsInputKey);
			Mem.Del(ref _actOnFinished);
			Mem.DelIDisposableSafe(ref _clsAnimationObserver);
			Mem.Del(ref _clsSettingModel);
			Mem.Del(ref _clsMapManager);
			Mem.Del(ref _clsBattleManager);
			_clsTasks.UnInit();
			Mem.DelIDisposableSafe(ref _clsTaskFleetAdvent);
			Mem.DelIDisposableSafe(ref _clsTaskBossInsert);
			Mem.DelIDisposableSafe(ref _clsTaskDetection);
			Mem.DelIDisposableSafe(ref _clsTaskCommand);
			Mem.DelIDisposableSafe(ref _clsTaskAerialCombat);
			Mem.DelIDisposableSafe(ref _clsTaskAerialCombatSecond);
			Mem.DelIDisposableSafe(ref _clsTaskSupportingFire);
			Mem.DelIDisposableSafe(ref _clsTaskOpeningTorpedoSalvo);
			Mem.DelIDisposableSafe(ref _clsTaskShelling);
			Mem.DelIDisposableSafe(ref _clsTaskTorpedoSalvo);
			Mem.DelIDisposableSafe(ref _clsTaskWithdrawalDecision);
			Mem.DelIDisposableSafe(ref _clsTaskNightCombat);
			Mem.DelIDisposableSafe(ref _clsTaskResult);
			Mem.DelIDisposableSafe(ref _clsTaskFlagshipWreck);
			Mem.DelIDisposableSafe(ref _clsTaskEscortShipEvacuation);
			Mem.DelIDisposableSafe(ref _clsTaskAdvancingWithdrawal);
			Mem.DelIDisposableSafe(ref _clsTaskAdvancingWithdrawalDC);
			Mem.DelIDisposableSafe(ref _clsTaskClearReward);
			Mem.DelIDisposableSafe(ref _clsTaskMapOpen);
			Mem.Del(ref _clsTasks);
			KCV.Battle.Utils.ShipUtils.UnInit();
			UIDrawCall.ReleaseInactive();
			App.TimeScale(1f);
			App.SetFramerate(60);
			Mem.Del(ref instance);
		}

		private void Update()
		{
			if (Input.touchCount == 0 && !Input.GetMouseButton(0) && _clsInputKey != null)
			{
				_clsInputKey.Update();
			}
			_clsBattleShips.Update();
			_clsTasks.Update();
			UpdateMode();
		}

		private IEnumerator InitBattle(UniRx.IObserver<float> observer)
		{
			observer.OnNext(0f);
			while (!_clsBattleShips.isInitialize)
			{
				yield return null;
			}
			observer.OnNext(1f);
			observer.OnCompleted();
		}

		private void InitBattleData()
		{
			if (RetentionData.GetData() != null)
			{
				_iRootType = (Generics.BattleRootType)(int)RetentionData.GetData()["rootType"];
				switch (_iRootType)
				{
				case Generics.BattleRootType.SortieMap:
					InitSortieBattle();
					break;
				case Generics.BattleRootType.Practice:
					InitPracticeBattle();
					break;
				case Generics.BattleRootType.Rebellion:
					InitRebellionBattle();
					break;
				}
				RetentionData.Release();
			}
			SetSkyType(_clsBattleManager);
		}

		private static bool InitSortieBattle()
		{
			BattleFormationKinds1 formationId = (BattleFormationKinds1)(int)RetentionData.GetData()["formation"];
			_clsBattleManager = ((SortieMapManager)SortieBattleTaskManager.GetMapManager()).BattleStart(formationId);
			return true;
		}

		private static bool InitPracticeBattle()
		{
			int enemy_deck_id = (int)RetentionData.GetData()["deckID"];
			BattleFormationKinds1 formation_id = (BattleFormationKinds1)(int)RetentionData.GetData()["formation"];
			PracticeManager practiceManager = RetentionData.GetData()["practiceManager"] as PracticeManager;
			_clsBattleManager = practiceManager.StartBattlePractice(enemy_deck_id, formation_id);
			return true;
		}

		private static bool InitRebellionBattle()
		{
			BattleFormationKinds1 formation_id = (BattleFormationKinds1)(int)RetentionData.GetData()["formation"];
			_clsBattleManager = ((RebellionMapManager)SortieBattleTaskManager.GetMapManager()).BattleStart(formation_id);
			return true;
		}

		private void SetSkyType(BattleManager manager)
		{
			if (manager is PracticeBattleManager)
			{
				_iSkyType = SkyType.Normal;
			}
			else if (manager.Map.AreaId == 17)
			{
				switch (manager.Map.No)
				{
				case 1:
					_iSkyType = SkyType.FinalArea171;
					break;
				case 2:
					_iSkyType = SkyType.FinalArea172;
					break;
				case 3:
					_iSkyType = SkyType.FinalArea173;
					break;
				case 4:
					_iSkyType = SkyType.FinalArea174;
					break;
				default:
					_iSkyType = SkyType.Normal;
					break;
				}
			}
			else
			{
				_iSkyType = SkyType.Normal;
			}
		}

		public static void DestroyUnneccessaryObject2Result()
		{
			Mem.DelComponentSafe(ref _traStage);
			Mem.DelComponentSafe(ref Instance._lWorldLight);
			Mem.DelComponentSafe(ref Instance._uiSeparatorLine);
			Mem.Del(ref _clsTaskBossInsert);
			Mem.Del(ref _clsTaskDetection);
			Mem.Del(ref _clsTaskCommand);
			Mem.Del(ref _clsTaskAerialCombat);
			Mem.Del(ref _clsTaskAerialCombatSecond);
			Mem.Del(ref _clsTaskSupportingFire);
			Mem.Del(ref _clsTaskOpeningTorpedoSalvo);
			Mem.Del(ref _clsTaskShelling);
			Mem.Del(ref _clsTaskTorpedoSalvo);
			Mem.Del(ref _clsTaskFleetAdvent);
			Mem.Del(ref _clsTaskWithdrawalDecision);
			Mem.Del(ref _clsTaskNightCombat);
			Instance._clsBattlePrefabFile.DisposeUnneccessaryObject2Result();
		}

		public static BattlePhase GetPhase()
		{
			return _iPhaseReq;
		}

		public static void ReqPhase(BattlePhase iPhase)
		{
			_iPhaseReq = iPhase;
		}

		protected void UpdateMode()
		{
			if (_iPhaseReq == BattlePhase.BattlePhase_BEF)
			{
				return;
			}
			switch (_iPhaseReq)
			{
			case BattlePhase.BattlePhase_ST:
				if (_clsTasks.Open(_clsTaskBossInsert) < 0)
				{
					return;
				}
				break;
			case BattlePhase.FleetAdvent:
				if (_clsTasks.Open(_clsTaskFleetAdvent) < 0)
				{
					return;
				}
				break;
			case BattlePhase.Detection:
				if (_clsTasks.Open(_clsTaskDetection) < 0)
				{
					return;
				}
				break;
			case BattlePhase.Command:
				if (_clsTasks.Open(_clsTaskCommand) < 0)
				{
					return;
				}
				break;
			case BattlePhase.AerialCombat:
				if (_clsTasks.Open(_clsTaskAerialCombat) < 0)
				{
					return;
				}
				break;
			case BattlePhase.AerialCombatSecond:
				if (_clsTasks.Open(_clsTaskAerialCombatSecond) < 0)
				{
					return;
				}
				break;
			case BattlePhase.SupportingFire:
				if (_clsTasks.Open(_clsTaskSupportingFire) < 0)
				{
					return;
				}
				break;
			case BattlePhase.OpeningTorpedoSalvo:
				if (_clsTasks.Open(_clsTaskOpeningTorpedoSalvo) < 0)
				{
					return;
				}
				break;
			case BattlePhase.Shelling:
				if (_clsTasks.Open(_clsTaskShelling) < 0)
				{
					return;
				}
				break;
			case BattlePhase.TorpedoSalvo:
				if (_clsTasks.Open(_clsTaskTorpedoSalvo) < 0)
				{
					return;
				}
				break;
			case BattlePhase.WithdrawalDecision:
				if (_clsTasks.Open(_clsTaskWithdrawalDecision) < 0)
				{
					return;
				}
				break;
			case BattlePhase.Result:
				if (_clsTasks.Open(_clsTaskResult) < 0)
				{
					return;
				}
				break;
			case BattlePhase.NightCombat:
				if (_clsTasks.Open(_clsTaskNightCombat) < 0)
				{
					return;
				}
				break;
			case BattlePhase.FlagshipWreck:
				if (_clsTasks.Open(_clsTaskFlagshipWreck) < 0)
				{
					return;
				}
				break;
			case BattlePhase.EscortShipEvacuation:
				if (_clsTasks.Open(_clsTaskEscortShipEvacuation) < 0)
				{
					return;
				}
				break;
			case BattlePhase.AdvancingWithdrawal:
				if (_clsTasks.Open(_clsTaskAdvancingWithdrawal) < 0)
				{
					return;
				}
				break;
			case BattlePhase.AdvancingWithdrawalDC:
				if (_clsTasks.Open(_clsTaskAdvancingWithdrawalDC) < 0)
				{
					return;
				}
				break;
			case BattlePhase.ClearReward:
				if (_clsTasks.Open(_clsTaskClearReward) < 0)
				{
					return;
				}
				break;
			case BattlePhase.MapOpen:
				if (_clsTasks.Open(_clsTaskMapOpen) < 0)
				{
					return;
				}
				break;
			}
			_iPhase = _iPhaseReq;
			_iPhaseReq = BattlePhase.BattlePhase_BEF;
		}

		public static MapManager GetMapManager()
		{
			return _clsMapManager;
		}

		public static BattleManager GetBattleManager()
		{
			return _clsBattleManager;
		}

		public static Transform GetStage()
		{
			return _traStage;
		}

		public static BattleShips GetBattleShips()
		{
			return _clsBattleShips;
		}

		public static BattleCameras GetBattleCameras()
		{
			return _clsBattleCameras;
		}

		public static BattleHPGauges GetBattleHPGauges()
		{
			return _clsBattleHPGauges;
		}

		public static BattleField GetBattleField()
		{
			return _clsBattleField;
		}

		public static KeyControl GetKeyControl()
		{
			return _clsInputKey;
		}

		public static Generics.BattleRootType GetRootType()
		{
			return _iRootType;
		}

		public static ObserverActionQueue GetObserverAction()
		{
			return _clsAnimationObserver;
		}

		public static SettingModel GetSettingModel()
		{
			return _clsSettingModel;
		}

		public static SkyType GetSkyType()
		{
			return _iSkyType;
		}

		public static TaskBattleCommand GetTaskCommand()
		{
			return _clsTaskCommand;
		}

		public static TaskBattleShelling GetTaskShelling()
		{
			return _clsTaskShelling;
		}

		public static TaskBattleTorpedoSalvo GetTaskTorpedoSalvo()
		{
			return _clsTaskTorpedoSalvo;
		}

		public static TaskBattleNightCombat GetTaskNightCombat()
		{
			return _clsTaskNightCombat;
		}

		public static BattleParticleFile GetParticleFile()
		{
			return Instance._clsBattleParticleFile;
		}

		public static BattlePefabFile GetPrefabFile()
		{
			return Instance._clsBattlePrefabFile;
		}

		public static TorpedoHpGauges GetTorpedoHpGauges()
		{
			return _clsTorpedoHpGauges;
		}

		private void GotoSortieMap(ShipRecoveryType iType)
		{
			Dlg.Call(ref _actOnFinished, iType);
		}

		public static bool IsSortieBattle()
		{
			return (_actOnFinished != null) ? true : false;
		}

		public static bool GetIsSameBGM()
		{
			BattleManager battleManager = GetBattleManager();
			bool bossBattle = battleManager.BossBattle;
			bool master_loaded = false;
			int bgmId = battleManager.GetBgmId(is_day: true, bossBattle, out master_loaded);
			int bgmId2 = battleManager.GetBgmId(is_day: false, bossBattle, out master_loaded);
			return (bgmId == bgmId2) ? true : false;
		}

		public static bool GetIsFinalAreaBattle()
		{
			return _clsBattleManager.Map != null && _clsBattleManager.Map.AreaId == 17;
		}

		private KCV.Battle.Utils.TimeZone GetStartTimeZone(enumMapWarType iType)
		{
			switch (iType)
			{
			case enumMapWarType.Normal:
			case enumMapWarType.AirBattle:
				return KCV.Battle.Utils.TimeZone.DayTime;
			case enumMapWarType.Midnight:
			case enumMapWarType.Night_To_Day:
				return KCV.Battle.Utils.TimeZone.Night;
			default:
				return KCV.Battle.Utils.TimeZone.None;
			}
		}

		public static KCV.Battle.Utils.TimeZone GetTimeZone()
		{
			return (_iPhase == BattlePhase.NightCombat) ? KCV.Battle.Utils.TimeZone.Night : KCV.Battle.Utils.TimeZone.DayTime;
		}
	}
}
