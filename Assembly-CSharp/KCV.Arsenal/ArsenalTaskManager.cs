using KCV.Scene.Port;
using local.managers;
using local.models;
using UnityEngine;

namespace KCV.Arsenal
{
	public class ArsenalTaskManager : MonoBehaviour
	{
		public enum ArsenalPhase
		{
			BattlePhase_ST = 0,
			BattlePhase_BEF = -1,
			BattlePhase_NONE = -1,
			MainArsenal = 0,
			NormalConstruct = 1,
			Development = 2,
			List = 3,
			BattlePhase_AFT = 4,
			BattlePhase_NUM = 4,
			BattlePhase_ED = 3
		}

		private static GameObject _uiCommon;

		private static KeyControl _clsInputKey;

		private static SceneTasksMono _clsTasks;

		private static ArsenalPhase _iPhase;

		private static ArsenalPhase _iPhaseReq;

		public static TaskMainArsenalManager _clsArsenal;

		public static TaskConstructManager _clsConstruct;

		public static TaskArsenalListManager _clsList;

		private static ArsenalManager logicManager;

		private static SoundManager soundManager;

		private static BaseDialogPopup dialogPopUp;

		private static AsyncObjects asyncObj;

		private static CommonPopupDialog commonPopup;

		private static UIPanel _uiBgPanel;

		private static DeckModel[] _deck;

		private static ShipModel[] _ship;

		private static ShipModel[] _allShip;

		private static BuildDockModel[] dock;

		public static bool IsArsenalCreate;

		public static bool IsConstructCreate;

		private static int _dockIndex;

		[SerializeField]
		private Texture[] mTextures_Preload;

		[SerializeField]
		private ParticleSystem[] mParticleSystems_Managed;

		public static ArsenalManager GetLogicManager()
		{
			return logicManager;
		}

		public static SoundManager GetSoundManager()
		{
			return soundManager;
		}

		public static BaseDialogPopup GetDialogPopUp()
		{
			return dialogPopUp;
		}

		public static DeckModel[] GetDeck()
		{
			return _deck;
		}

		public static ShipModel[] GetShip()
		{
			return _ship;
		}

		public static ShipModel[] GetAllShip()
		{
			return _allShip;
		}

		public static int GetDockIndex()
		{
			return _dockIndex;
		}

		private void Awake()
		{
			_clsInputKey = new KeyControl();
			_clsInputKey.useDoubleIndex(0, 5);
			_clsTasks = base.gameObject.SafeGetComponent<SceneTasksMono>();
			GameObject gameObject = base.transform.FindChild("TaskArsenalMain").gameObject;
			GameObject gameObject2 = gameObject.transform.FindChild("Task").gameObject;
			_clsArsenal = ((Component)gameObject2.transform.FindChild("Arsenal")).GetComponent<TaskMainArsenalManager>();
			_clsConstruct = ((Component)gameObject2.transform.FindChild("Construct")).GetComponent<TaskConstructManager>();
			_clsList = ((Component)gameObject2.transform.FindChild("TaskArsenalListManager")).GetComponent<TaskArsenalListManager>();
			logicManager = new ArsenalManager();
			dialogPopUp = new BaseDialogPopup();
		}

		private void Start()
		{
			_clsTasks.Init();
			_iPhase = (_iPhaseReq = ArsenalPhase.BattlePhase_ST);
			SingletonMonoBehaviour<UIPortFrame>.Instance.CircleUpdateInfo(logicManager);
			_clsConstruct.firstInit();
			_clsList.firstInit();
		}

		private void Update()
		{
			_clsInputKey.Update();
			_clsTasks.Run();
			UpdateMode();
		}

		public static void ReqPhase(ArsenalPhase iPhase)
		{
			_iPhaseReq = iPhase;
		}

		protected void UpdateMode()
		{
			if (_iPhaseReq == ArsenalPhase.BattlePhase_BEF)
			{
				return;
			}
			switch (_iPhaseReq)
			{
			case ArsenalPhase.BattlePhase_ST:
				if (_clsTasks.Open(_clsArsenal) < 0)
				{
					return;
				}
				break;
			case ArsenalPhase.NormalConstruct:
				if (_clsTasks.Open(_clsConstruct) < 0)
				{
					return;
				}
				break;
			case ArsenalPhase.List:
				if (_clsTasks.Open(_clsList) < 0)
				{
					return;
				}
				break;
			}
			_iPhase = _iPhaseReq;
			_iPhaseReq = ArsenalPhase.BattlePhase_BEF;
		}

		public static KeyControl GetKeyControl()
		{
			return _clsInputKey;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref mTextures_Preload);
			UserInterfacePortManager.ReleaseUtils.Releases(ref mParticleSystems_Managed);
			_uiCommon = null;
			_clsInputKey = null;
			_clsTasks = null;
			_clsArsenal = null;
			_clsConstruct = null;
			_clsList = null;
			logicManager = null;
			soundManager = null;
			dialogPopUp = null;
			asyncObj = null;
			commonPopup = null;
			_uiBgPanel = null;
			Mem.DelAry(ref _deck);
			Mem.DelAry(ref _ship);
			Mem.DelAry(ref _allShip);
			Mem.DelAry(ref dock);
			UIDrawCall.ReleaseInactive();
		}
	}
}
