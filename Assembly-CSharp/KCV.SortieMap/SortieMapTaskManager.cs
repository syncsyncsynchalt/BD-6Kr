using Common.Enum;
using KCV.SortieBattle;
using KCV.Utils;
using Librarys.State;
using local.managers;
using System;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	public class SortieMapTaskManager : SceneTaskMono
	{
		private static SortieMapTaskManager instance;

		[SerializeField]
		private UIRoot _uiRoot;

		[SerializeField]
		private UISortieMapName _uiSortieMapName;

		[SerializeField]
		private UIAreaMapFrame _uiAreaMapFrame;

		[SerializeField]
		private UISortieShipCharacter _uiShipCharacter;

		[SerializeField]
		private Transform _sharedPlace;

		[SerializeField]
		private UIShortCutSwitch _shortCutSwitch;

		[SerializeField]
		private BaseCamera _cam;

		[Header("[SortieMap Prefab Files]")]
		[SerializeField]
		private SortiePrefabFile _clsSortiePrefabFile;

		private bool _isFirstInit;

		private Action<MapManager> _actOnSetMapManager;

		private CtrlActiveBranching _ctrlActiveBranching;

		private static UIMapManager _uiMapManager;

		private static SortieMapTaskManagerMode _iMode;

		private static SortieMapTaskManagerMode _iModeReq;

		private static Tasks _clsTasks;

		private static TaskSortieMoveShip _clsTaskMoveShip;

		private static TaskSortieEvent _clsTaskEvent;

		private static TaskSortieFormation _clsTaskFormation;

		private static TaskSortieResult _clsTaskResult;

		[Header("[Debug Properties]")]
		[SerializeField]
		private SortieDebugProperties _strSortieDebugProperties = default(SortieDebugProperties);

		private static SortieMapTaskManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = UnityEngine.Object.FindObjectOfType<SortieMapTaskManager>();
					if (instance == null)
					{
						throw new Exception();
					}
				}
				return instance;
			}
		}

		public void Startup(Transform prefabAreaMap, MapManager mapManager, Action<MapManager> onSetMapManager)
		{
			_clsTasks = new Tasks();
			_clsTaskMoveShip = new TaskSortieMoveShip();
			_clsTaskEvent = new TaskSortieEvent(GoNext);
			_clsTaskFormation = new TaskSortieFormation();
			_clsTaskResult = new TaskSortieResult();
			_actOnSetMapManager = onSetMapManager;
			_isFirstInit = true;
			this.GetComponentThis(ref _uiRoot);
			Util.SetRootContentSize(_uiRoot, App.SCREEN_RESOLUTION);
			InitSortieMapData(prefabAreaMap, mapManager);
			DrawDefaultShip();
		}

		public void Terminate()
		{
			Mem.Del(ref _uiRoot);
			Mem.Del(ref _uiSortieMapName);
			Mem.Del(ref _uiAreaMapFrame);
			Mem.Del(ref _uiShipCharacter);
			Mem.Del(ref _sharedPlace);
			Mem.Del(ref _shortCutSwitch);
			Mem.Del(ref _cam);
			Mem.DelIDisposableSafe(ref _clsSortiePrefabFile);
			Mem.Del(ref _isFirstInit);
			Mem.Del(ref _actOnSetMapManager);
			Mem.DelIDisposableSafe(ref _ctrlActiveBranching);
			Mem.Del(ref _uiMapManager);
			Mem.Del(ref _iModeReq);
			Mem.Del(ref _iMode);
			if (_clsTasks != null)
			{
				_clsTasks.UnInit();
			}
			Mem.Del(ref _clsTasks);
			Mem.DelIDisposableSafe(ref _clsTaskMoveShip);
			Mem.DelIDisposableSafe(ref _clsTaskEvent);
			Mem.DelIDisposableSafe(ref _clsTaskFormation);
			Mem.DelIDisposableSafe(ref _clsTaskResult);
			Mem.Del(ref instance);
		}

		private void InitSortieMapData(Transform prefabAreaMap, MapManager mapManager)
		{
			if (mapManager != null)
			{
				Dlg.Call(ref _actOnSetMapManager, mapManager);
				StartupUIMapManager(prefabAreaMap);
				GetGoNextData();
			}
		}

		private void StartupUIMapManager(Transform prefabAreaMap)
		{
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			_uiMapManager = UIMapManager.Instantiate(mapManager, (!(prefabAreaMap != null)) ? null : ((Component)prefabAreaMap).GetComponent<UIMapManager>(), _uiRoot.transform.FindChild("MapGenerator"), ((Component)_clsSortiePrefabFile.prefabUISortieShip).GetComponent<UISortieShip>());
			_uiSortieMapName.SetMapInformation(mapManager);
		}

		private void GetGoNextData()
		{
			ShipRecoveryType recovery = ShipRecoveryType.None;
			if (RetentionData.GetData().ContainsKey("shipRecoveryType"))
			{
				recovery = (ShipRecoveryType)(int)RetentionData.GetData()["shipRecoveryType"];
			}
			if ((int)RetentionData.GetData()["rootType"] == 0)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(isActive: false);
				UpdateUIMapManager();
				return;
			}
			UpdateUIMapManager();
			_uiMapManager.InitAfterBattle();
			if (SingletonMonoBehaviour<FadeCamera>.Instance.isFadeOut)
			{
				Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate
				{
					SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, delegate
					{
						GoNext(recovery);
					});
				});
			}
		}

		private void GoNext(ShipRecoveryType iRecoveryType)
		{
			MapManager mm = SortieBattleTaskManager.GetMapManager();
			if (mm.GetNextCellCandidate().Count != 0)
			{
				Instance._ctrlActiveBranching = new CtrlActiveBranching(mm.GetNextCellCandidate(), delegate(int x)
				{
					mm.GoNext(iRecoveryType, x);
					Instance.UpdateUIMapManager();
					ReqMode(SortieMapTaskManagerMode.SortieMapTaskManagerMode_ST);
					Mem.DelIDisposableSafe(ref Instance._ctrlActiveBranching);
				});
				return;
			}
			mm.GoNext(iRecoveryType);
			Instance.UpdateUIMapManager();
			ReqMode(SortieMapTaskManagerMode.SortieMapTaskManagerMode_ST);
		}

		private void UpdateUIMapManager()
		{
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			_uiMapManager.UpdatePassedRoutesStates(mapManager);
			_uiMapManager.UpdateNowNNextCell(mapManager.NowCell, mapManager.NextCell);
			if (_uiMapManager.nowCell != null)
			{
				_uiMapManager.nowCell.isPassedCell = true;
			}
			_uiMapManager.SetShipPosition();
		}

		private void DrawDefaultShip()
		{
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			_uiShipCharacter.SetShipData(mapManager.Deck.GetFlagShip());
			_uiShipCharacter.DrawDefault();
		}

		protected override bool Init()
		{
			_iMode = (_iModeReq = SortieMapTaskManagerMode.SortieMapTaskManagerMode_BEF);
			_clsTasks.Init();
			SoundUtils.SwitchBGM((BGMFileInfos)SortieBattleTaskManager.GetMapManager().BgmId);
			if (!_isFirstInit)
			{
				if (SingletonMonoBehaviour<FadeCamera>.Instance.isFadeOut)
				{
					Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate
					{
						SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, delegate
						{
							GoNext(SortieBattleTaskManager.GetBattleShipRecoveryType());
						});
					});
				}
				else
				{
					Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate
					{
						GoNext(SortieBattleTaskManager.GetBattleShipRecoveryType());
					});
				}
			}
			else
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(isActive: false);
				_iMode = (_iModeReq = SortieMapTaskManagerMode.SortieMapTaskManagerMode_ST);
			}
			_uiAreaMapFrame.Show();
			_isFirstInit = false;
			_cam.isCulling = true;
			return true;
		}

		protected override bool UnInit()
		{
			_clsTasks.UnInit();
			_uiMapManager.wobblingIcons.DestroyDrawWobblingIcons();
			_uiAreaMapFrame.ClearMessage();
			_cam.isCulling = false;
			App.TimeScale(1f);
			return true;
		}

		protected override bool Run()
		{
			KeyControl keyControl = SortieBattleTaskManager.GetKeyControl();
			if (_iMode != SortieMapTaskManagerMode.Formation)
			{
				if (keyControl.GetDown(KeyControl.KeyName.L))
				{
					_shortCutSwitch.Switch();
				}
			}
			else if (_shortCutSwitch.isShortCut)
			{
				_shortCutSwitch.Hide();
			}
			if (_ctrlActiveBranching != null)
			{
				_ctrlActiveBranching.Update();
			}
			_clsTasks.Update();
			UpdateMode();
			if (SortieBattleTaskManager.GetMode() != SortieBattleMode.SortieBattleMode_BEF)
			{
				return (SortieBattleTaskManager.GetMode() == SortieBattleMode.SortieBattleMode_ST) ? true : false;
			}
			return true;
		}

		public static SortieMapTaskManagerMode GetMode()
		{
			return _iModeReq;
		}

		public static void ReqMode(SortieMapTaskManagerMode iMode)
		{
			_iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (_iModeReq == SortieMapTaskManagerMode.SortieMapTaskManagerMode_BEF)
			{
				return;
			}
			switch (_iModeReq)
			{
			case SortieMapTaskManagerMode.SortieMapTaskManagerMode_ST:
				if (_clsTasks.Open(_clsTaskMoveShip) < 0)
				{
					return;
				}
				break;
			case SortieMapTaskManagerMode.Event:
				if (_clsTasks.Open(_clsTaskEvent) < 0)
				{
					return;
				}
				break;
			case SortieMapTaskManagerMode.Formation:
				if (_clsTasks.Open(_clsTaskFormation) < 0)
				{
					return;
				}
				break;
			case SortieMapTaskManagerMode.Result:
				if (_clsTasks.Open(_clsTaskResult) < 0)
				{
					return;
				}
				break;
			}
			_iMode = _iModeReq;
			_iModeReq = SortieMapTaskManagerMode.SortieMapTaskManagerMode_BEF;
		}

		public static Transform GetSharedPlace()
		{
			return Instance._sharedPlace;
		}

		public static UIShortCutSwitch GetShortCutSwitch()
		{
			return Instance._shortCutSwitch;
		}

		public static SortiePrefabFile GetPrefabFile()
		{
			return Instance._clsSortiePrefabFile;
		}

		public BaseCamera GetCamera()
		{
			return _cam;
		}

		public static TaskSortieMoveShip GetTaskMoveShip()
		{
			return _clsTaskMoveShip;
		}

		public static TaskSortieEvent GetTaskEvent()
		{
			return _clsTaskEvent;
		}

		public static UIMapManager GetUIMapManager()
		{
			return _uiMapManager;
		}

		public static UIAreaMapFrame GetUIAreaMapFrame()
		{
			return Instance._uiAreaMapFrame;
		}

		public static UISortieShipCharacter GetUIShipCharacter()
		{
			return Instance._uiShipCharacter;
		}
	}
}
