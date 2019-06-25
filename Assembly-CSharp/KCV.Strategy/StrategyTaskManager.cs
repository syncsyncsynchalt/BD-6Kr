using KCV.Strategy.Rebellion;
using local.managers;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyTaskManager : SceneTaskMono
	{
		public enum StrategyTaskManagerMode
		{
			StrategyTaskManagerMode_ST = 0,
			StrategyTaskManagerMode_BEF = -1,
			StrategyTaskManagerMode_NONE = -1,
			StrategyTop = 0,
			MapSelect = 1,
			ExercisesPartnerSelection = 2,
			Expedition = 3,
			TransportShipDeployment = 4,
			EscortFleetOrganization = 5,
			Rebellion = 6,
			StrategyTaskManagerMode_AFT = 7,
			StrategyTaskManagerMode_NUM = 7,
			StrategyTaskManagerMode_ED = 6
		}

		private static Transform _traOverView;

		private static Camera _traOverViewCamera;

		private static Transform _traMapRoot;

		private SceneTasksMono _clsTasks;

		private static StrategyTaskManagerMode _iMode;

		private static StrategyTaskManagerMode _iModeReq;

		private static StrategyMapManager _clsStrategyMapManager;

		private static StrategyTopTaskManager _clsTopTask;

		private static StrategyRebellionTaskManager _clsRebellionTask;

		private static Action callBack;

		[SerializeField]
		private GameObject KeyManager;

		public static Transform GetOverView()
		{
			return _traOverView;
		}

		public static Camera GetOverViewCamera()
		{
			return _traOverViewCamera;
		}

		public static Transform GetMapRoot()
		{
			return _traMapRoot;
		}

		public static StrategyMapManager GetStrategyMapManager()
		{
			if (_clsStrategyMapManager == null)
			{
				if (StrategyTopTaskManager.GetLogicManager() != null)
				{
					_clsStrategyMapManager = StrategyTopTaskManager.GetLogicManager();
				}
				else
				{
					_clsStrategyMapManager = new StrategyMapManager();
				}
			}
			return _clsStrategyMapManager;
		}

		public static StrategyTopTaskManager GetStrategyTop()
		{
			return _clsTopTask;
		}

		public static StrategyRebellionTaskManager GetStrategyRebellion()
		{
			return _clsRebellionTask;
		}

		protected override void Awake()
		{
			_clsTasks = this.SafeGetComponent<SceneTasksMono>();
			_clsTasks.Init();
			GameObject gameObject = base.transform.FindChild("Task").gameObject;
			_clsTopTask = ((Component)gameObject.transform.FindChild("StrategyTop")).GetComponent<StrategyTopTaskManager>();
			_clsRebellionTask = ((Component)gameObject.transform.FindChild("Rebellion")).GetComponent<StrategyRebellionTaskManager>();
			if (SingletonMonoBehaviour<FadeCamera>.Instance != null)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(isActive: true);
			}
			if (!KeyControlManager.exist())
			{
				Util.Instantiate(KeyManager);
			}
			_traOverView = base.transform.FindChild("OverView");
			_traOverViewCamera = ((Component)_traOverView.FindChild("OverViewCamera")).GetComponent<Camera>();
			_traMapRoot = base.transform.FindChild("Map Root");
		}

		private void OnDestroy()
		{
			_clsTasks.UnInit();
			_clsTopTask = null;
			_clsRebellionTask = null;
			_traOverView = null;
			_traOverViewCamera = null;
			_traMapRoot = null;
			_traScenePrefab = null;
			_clsStrategyMapManager = null;
			callBack = null;
			KeyManager = null;
		}

		protected override void Start()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.SceneObject = base.gameObject;
			SingletonMonoBehaviour<PortObjectManager>.Instance.EnterStrategy();
			_iMode = (_iModeReq = StrategyTaskManagerMode.StrategyTaskManagerMode_ST);
		}

		private void Update()
		{
			_clsTasks.Run();
			UpdateMode();
		}

		public static StrategyTaskManagerMode GetMode()
		{
			return _iModeReq;
		}

		public static void ReqMode(StrategyTaskManagerMode iMode)
		{
			_iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (_iModeReq == StrategyTaskManagerMode.StrategyTaskManagerMode_BEF)
			{
				return;
			}
			switch (_iModeReq)
			{
			case StrategyTaskManagerMode.StrategyTaskManagerMode_ST:
				if (_clsTasks.Open(_clsTopTask) < 0)
				{
					return;
				}
				break;
			case StrategyTaskManagerMode.Rebellion:
				if (_clsTasks.Open(_clsRebellionTask) < 0)
				{
					return;
				}
				break;
			}
			_iMode = _iModeReq;
			_iModeReq = StrategyTaskManagerMode.StrategyTaskManagerMode_BEF;
		}

		public static void setCallBack(Action act)
		{
			callBack = act;
		}

		public static void SceneCallBack()
		{
			StrategyTopTaskManager.GetCommandMenu().DeckEnableCheck();
			if (callBack != null)
			{
				callBack();
				callBack = null;
			}
		}
	}
}
