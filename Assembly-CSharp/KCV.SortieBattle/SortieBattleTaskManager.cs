using Common.Enum;
using KCV.Battle;
using KCV.BattleCut;
using KCV.SortieMap;
using KCV.Strategy;
using local.managers;
using local.utils;
using Server_Models;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.SortieBattle
{
	public class SortieBattleTaskManager : MonoBehaviour
	{
		private static SortieBattleTaskManager instance;

		[SerializeField]
		private SortieMapTaskManager _clsSortieMapTaskManager;

		[SerializeField]
		private Camera _camTransitionCamera;

		[SerializeField]
		[Header("[SortieBattle Prefab Files]")]
		private SortieBattlePrefabFile _clsSortieBattlePrefabFile;

		private static KeyControl _clsInput;

		private static ShipRecoveryType _iRecoveryType;

		private static MapManager _clsMapManager;

		private static SortieBattleMode _iMode;

		private static SortieBattleMode _iModeReq;

		private static SceneTasksMono _clsTasks;

		private BattleTaskManager _clsBattleTaskManager;

		private BattleCutManager _clsBattleCutManager;

		private static SortieBattleTaskManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = (Object.FindObjectOfType(typeof(SortieBattleTaskManager)) as SortieBattleTaskManager);
					if (instance == null)
					{
						return null;
					}
				}
				return instance;
			}
		}

		private void OnLevelWasLoaded(int nLevel)
		{
			UIDrawCall.ReleaseInactive();
			SoundFile.ClearAllSE();
		}

		private void Awake()
		{
			_clsInput = new KeyControl();
			_iRecoveryType = ShipRecoveryType.None;
			_clsTasks = this.SafeGetComponent<SceneTasksMono>();
			_clsTasks.Init();
			_iMode = (_iModeReq = SortieBattleMode.SortieBattleMode_BEF);
			_camTransitionCamera.enabled = false;
		}

		private void Start()
		{
			Observable.FromCoroutine((IObserver<bool> observer) => Startup(observer)).Subscribe(delegate
			{
				_iMode = (_iModeReq = SortieBattleMode.SortieBattleMode_ST);
			}).AddTo(base.gameObject);
		}

		private void OnDestroy()
		{
			TrophyUtil.Unlock_Material();
			Mem.DelIDisposableSafe(ref _clsSortieBattlePrefabFile);
			Mem.Del(ref _camTransitionCamera);
			Mem.Del(ref _clsInput);
			Mem.Del(ref _iRecoveryType);
			if (_clsMapManager != null)
			{
				_clsMapManager.MapEnd();
			}
			Mem.Del(ref _clsMapManager);
			if (_clsTasks != null)
			{
				_clsTasks.UnInit();
			}
			Mem.Del(ref _clsTasks);
			Mem.Del(ref _iMode);
			Mem.Del(ref _iModeReq);
			if (_clsSortieMapTaskManager != null)
			{
				_clsSortieMapTaskManager.Terminate();
			}
			Mem.Del(ref _clsSortieMapTaskManager);
			Mem.Del(ref _clsBattleTaskManager);
			Mem.Del(ref _clsBattleCutManager);
			Mst_DataManager.Instance.PurgeUIBattleMaster();
			Mem.Del(ref instance);
		}

		private void Update()
		{
			if (Input.touchCount == 0 && !Input.GetMouseButton(0) && (_iMode != SortieBattleMode.Battle || _iMode != SortieBattleMode.BattleCut) && _clsInput != null)
			{
				_clsInput.Update();
			}
			_clsTasks.Run();
			UpdateMode();
		}

		private IEnumerator Startup(IObserver<bool> observer)
		{
			if (!App.isMasterInit)
			{
				yield return Observable.NextFrame(FrameCountType.EndOfFrame).StartAsCoroutine();
			}
			MapManager mm = null;
			if (RetentionData.GetData() != null)
			{
				mm = ((!RetentionData.GetData().ContainsKey("sortieMapManager")) ? ((MapManager)RetentionData.GetData()["rebellionMapManager"]) : ((MapManager)RetentionData.GetData()["sortieMapManager"]));
			}
			_clsSortieMapTaskManager.Startup((!(SingletonMonoBehaviour<MapTransitionCutManager>.Instance != null)) ? null : SingletonMonoBehaviour<MapTransitionCutManager>.Instance.GetPrefabAreaMap(), mm, OnSetMapManager);
			RetentionData.Release();
			Mst_DataManager.Instance.MakeUIBattleMaster(_clsMapManager.Map.MstId);
			yield return Observable.NextFrame(FrameCountType.EndOfFrame).StartAsCoroutine();
			if (SingletonMonoBehaviour<MapTransitionCutManager>.Instance != null)
			{
				bool isWait = true;
				SingletonMonoBehaviour<MapTransitionCutManager>.Instance.Discard(delegate
				{
					isWait = false;
				});
				while (isWait)
				{
					yield return new WaitForEndOfFrame();
				}
			}
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		public static SortieBattleMode GetMode()
		{
			return _iModeReq;
		}

		public static void ReqMode(SortieBattleMode iMode)
		{
			_iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (_iModeReq == SortieBattleMode.SortieBattleMode_BEF)
			{
				return;
			}
			switch (_iModeReq)
			{
			case SortieBattleMode.SortieBattleMode_ST:
				if (_clsTasks.Open(_clsSortieMapTaskManager) < 0)
				{
					return;
				}
				break;
			case SortieBattleMode.Battle:
				if (_clsBattleTaskManager == null)
				{
					_clsBattleTaskManager = BattleTaskManager.Instantiate(((Component)_clsSortieBattlePrefabFile.prefabBattleTaskManager).GetComponent<BattleTaskManager>(), delegate(ShipRecoveryType x)
					{
						_iRecoveryType = x;
						ReqMode(SortieBattleMode.SortieBattleMode_ST);
						Mem.DelComponentSafe(ref _clsBattleTaskManager);
					});
					CheckDiscardSortieMapTaskManager();
				}
				break;
			case SortieBattleMode.BattleCut:
				if (_clsBattleCutManager == null)
				{
					_clsBattleCutManager = BattleCutManager.Instantiate(((Component)GetSortieBattlePrefabFile().prefabBattleCutManager).GetComponent<BattleCutManager>(), Vector3.left * 20f);
					_clsBattleCutManager.StartBattleCut(GetMapManager(), delegate
					{
						_clsSortieMapTaskManager.GetCamera().isCulling = true;
					}, delegate(ShipRecoveryType x)
					{
						_iRecoveryType = x;
						ReqMode(SortieBattleMode.SortieBattleMode_ST);
						Mem.DelComponentSafe(ref _clsBattleCutManager);
					});
					CheckDiscardSortieMapTaskManager();
				}
				break;
			}
			_iMode = _iModeReq;
			_iModeReq = SortieBattleMode.SortieBattleMode_BEF;
		}

		public static bool IsRebellionSortieBattle()
		{
			if (_clsMapManager == null)
			{
				return false;
			}
			return (_clsMapManager is RebellionMapManager) ? true : false;
		}

		private void CheckDiscardSortieMapTaskManager()
		{
			if (_clsMapManager.IsNextFinal())
			{
				Observable.NextFrame(FrameCountType.EndOfFrame).Subscribe(delegate
				{
					if (_clsSortieMapTaskManager != null)
					{
						_clsSortieMapTaskManager.Terminate();
					}
					Mem.DelComponentSafe(ref _clsSortieMapTaskManager);
				});
			}
		}

		public static ShipRecoveryType GetBattleShipRecoveryType()
		{
			return _iRecoveryType;
		}

		public static SortieBattlePrefabFile GetSortieBattlePrefabFile()
		{
			if (Instance == null)
			{
				return null;
			}
			return Instance._clsSortieBattlePrefabFile;
		}

		public static KeyControl GetKeyControl()
		{
			return _clsInput;
		}

		public static MapManager GetMapManager()
		{
			return (_clsMapManager == null) ? BattleTaskManager.GetMapManager() : _clsMapManager;
		}

		public static Camera GetTransitionCamera()
		{
			if (Instance == null)
			{
				return null;
			}
			return Instance._camTransitionCamera;
		}

		private void OnSetMapManager(MapManager mapManager)
		{
			_clsMapManager = mapManager;
		}
	}
}
