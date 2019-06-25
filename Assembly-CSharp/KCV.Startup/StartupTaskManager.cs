using Common.Enum;
using local.models;
using System.Collections;
using UnityEngine;

namespace KCV.Startup
{
	public class StartupTaskManager : MonoBehaviour
	{
		public enum StartupTaskManagerMode
		{
			StartupTaskManagerMode_ST = 0,
			StartupTaskManagerMode_BEF = -1,
			StartupTaskManagerMode_NONE = -1,
			AdmiralInfo = 0,
			FirstShipSelect = 1,
			PictureStoryShow = 2,
			StartupTaskManagerMode_AFT = 3,
			StartupTaskManagerMode_NUM = 3,
			StartupTaskManagerMode_ED = 2
		}

		private static StartupTaskManager instance;

		[SerializeField]
		private UIStartupHeader _uiStartupHeader;

		[SerializeField]
		private UIStartupNavigation _uiStartupNavigation;

		[SerializeField]
		private Transform _sharedPlace;

		[SerializeField]
		private PSVitaMovie _clsPSVitaMovie;

		[SerializeField]
		private FirstMeetingManager _clsFirstMeetingManager;

		[SerializeField]
		private StartupPrefabFile _clsStartupPrefabFile;

		private static Defines _clsDefines;

		private static StartupData _clsData;

		private static KeyControl _clsInput;

		private static SceneTasksMono _clsTasks;

		private static StartupTaskManagerMode _iMode;

		private static StartupTaskManagerMode _iModeReq;

		private static TaskStartupAdmiralInfo _clsTaskAdmiralInfo;

		private static TaskStartupFirstShipSelect _clsTaskFirstShipSelect;

		private static TaskStartupPictureStoryShow _clsTaskPictureStoryShow;

		private static StartupTaskManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Object.FindObjectOfType<StartupTaskManager>();
				}
				return instance;
			}
		}

		private void Awake()
		{
			_clsDefines = new Defines();
			_clsInput = new KeyControl();
			_clsData = new StartupData();
			SetStartupData();
			_clsTasks = base.gameObject.SafeGetComponent<SceneTasksMono>();
			Transform transform = GameObject.Find("Tasks").transform;
			_clsTaskAdmiralInfo = transform.GetComponentInChildren<TaskStartupAdmiralInfo>();
			_clsTaskFirstShipSelect = transform.GetComponentInChildren<TaskStartupFirstShipSelect>();
			_clsTaskPictureStoryShow = transform.GetComponentInChildren<TaskStartupPictureStoryShow>();
			_clsTaskAdmiralInfo.Setup();
			_uiStartupNavigation.Startup(_clsData.isInherit, new SettingModel());
		}

		private void Start()
		{
			_iMode = (_iModeReq = StartupTaskManagerMode.StartupTaskManagerMode_ST);
			_clsTasks.Init();
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiStartupHeader);
			Mem.Del(ref _uiStartupNavigation);
			Mem.Del(ref _sharedPlace);
			Mem.DelIDisposableSafe(ref _clsStartupPrefabFile);
			Mem.Del(ref _clsPSVitaMovie);
			Mem.Del(ref _clsFirstMeetingManager);
			Mem.DelIDisposableSafe(ref _clsDefines);
			Mem.Del(ref _clsInput);
			_clsTasks.UnInit();
			Mem.Del(ref _clsTasks);
			Mem.Del(ref _iMode);
			Mem.Del(ref _iModeReq);
			Mem.Del(ref _clsTaskAdmiralInfo);
			Mem.Del(ref _clsTaskFirstShipSelect);
			Mem.Del(ref _clsTaskPictureStoryShow);
			Mem.Del(ref instance);
			UIDrawCall.ReleaseInactive();
		}

		private void Update()
		{
			if (Input.touchCount == 0 && !Input.GetMouseButton(0))
			{
				_clsInput.Update();
			}
			_clsTasks.Run();
			UpdateMode();
		}

		public static StartupTaskManagerMode GetMode()
		{
			return _iModeReq;
		}

		public static void ReqMode(StartupTaskManagerMode iMode)
		{
			_iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (_iModeReq == StartupTaskManagerMode.StartupTaskManagerMode_BEF)
			{
				return;
			}
			switch (_iModeReq)
			{
			case StartupTaskManagerMode.StartupTaskManagerMode_ST:
				if (_clsTasks.Open(_clsTaskAdmiralInfo) < 0)
				{
					return;
				}
				break;
			case StartupTaskManagerMode.FirstShipSelect:
				if (_clsTasks.Open(_clsTaskFirstShipSelect) < 0)
				{
					return;
				}
				break;
			case StartupTaskManagerMode.PictureStoryShow:
				if (_clsTasks.Open(_clsTaskPictureStoryShow) < 0)
				{
					return;
				}
				break;
			}
			_iMode = _iModeReq;
			_iModeReq = StartupTaskManagerMode.StartupTaskManagerMode_BEF;
		}

		private void SetStartupData()
		{
			if (RetentionData.GetData() != null)
			{
				Hashtable data = RetentionData.GetData();
				_clsData.Difficlty = (DifficultKind)(int)data["difficulty"];
				_clsData.isInherit = (data.ContainsKey("isInherit") ? true : false);
				_clsData.AdmiralName = ((!data.ContainsKey("isInherit")) ? string.Empty : App.GetTitleManager().UserName);
			}
			RetentionData.Release();
		}

		public static UIStartupHeader GetStartupHeader()
		{
			return Instance._uiStartupHeader;
		}

		public static UIStartupNavigation GetNavigation()
		{
			return Instance._uiStartupNavigation;
		}

		public static Transform GetSharedPlace()
		{
			return Instance._sharedPlace;
		}

		public static StartupData GetData()
		{
			return _clsData;
		}

		public static KeyControl GetKeyControl()
		{
			return _clsInput;
		}

		public static StartupPrefabFile GetPrefabFile()
		{
			return Instance._clsStartupPrefabFile;
		}

		public static CtrlPartnerSelect GetPartnerSelect()
		{
			return _clsTaskFirstShipSelect.ctrlPartnerSelect;
		}

		public static PSVitaMovie GetPSVitaMovie()
		{
			return Instance._clsPSVitaMovie;
		}

		public static FirstMeetingManager GetFirstMeetingManager()
		{
			return Instance._clsFirstMeetingManager;
		}

		public static bool IsInheritStartup()
		{
			return _clsData.isInherit;
		}
	}
}
