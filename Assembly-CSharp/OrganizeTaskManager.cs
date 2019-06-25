using KCV;
using KCV.Organize;
using KCV.Utils;
using local.managers;
using local.models;
using System.Collections;
using UnityEngine;

public class OrganizeTaskManager : MonoBehaviour
{
	public enum OrganizePhase
	{
		Phase_ST = 0,
		Phase_BEF = -1,
		Phase_NONE = -1,
		Top = 0,
		Detail = 1,
		List = 2,
		ListDetail = 3,
		Phase_AFT = 4,
		Phase_NUM = 4,
		Phase_ED = 3
	}

	protected static GameObject _uiCommon;

	protected static KeyControl _clsInputKey;

	protected static SceneTasksMono _clsTasks;

	protected static OrganizePhase _iPhase;

	protected static OrganizePhase _iPhaseReq;

	public static TaskOrganizeTop _clsTop;

	public static TaskOrganizeDetail _clsDetail;

	public static TaskOrganizeList _clsList;

	public static TaskOrganizeListDetail _clsListDetail;

	public static OrganizeManager logicManager;

	protected static BaseDialogPopup dialogPopUp;

	protected static DeckModel[] _deck;

	protected static ShipModel[] _ship;

	protected static ShipModel[] _allShip;

	protected static GameObject _mainObj;

	public static OrganizeTaskManager Instance;

	protected bool isRun;

	public virtual TaskOrganizeTop GetTopTask()
	{
		return _clsTop;
	}

	public virtual TaskOrganizeDetail GetDetailTask()
	{
		return _clsDetail;
	}

	public virtual TaskOrganizeList GetListTask()
	{
		return _clsList;
	}

	public virtual TaskOrganizeListDetail GetListDetailTask()
	{
		return _clsListDetail;
	}

	public virtual IOrganizeManager GetLogicManager()
	{
		return logicManager;
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

	public static GameObject GetMainObject()
	{
		return _mainObj;
	}

	protected virtual void Awake()
	{
		_clsInputKey = new KeyControl();
		_clsInputKey.useDoubleIndex(0, 3);
		dialogPopUp = new BaseDialogPopup();
		Instance = this;
	}

	private IEnumerator Start()
	{
		_clsTasks = base.gameObject.SafeGetComponent<SceneTasksMono>();
		_mainObj = GameObject.Find("OrganizeRoot").gameObject;
		GameObject task = _mainObj.transform.FindChild("Task").gameObject;
		_clsTop = ((Component)task.transform.FindChild("TaskTop")).GetComponent<TaskOrganizeTop>();
		_clsDetail = ((Component)task.transform.FindChild("TaskDetail")).GetComponent<TaskOrganizeDetail>();
		_clsList = ((Component)task.transform.FindChild("TaskList")).GetComponent<TaskOrganizeList>();
		_clsListDetail = ((Component)task.transform.FindChild("TaskListDetail")).GetComponent<TaskOrganizeListDetail>();
		_clsTasks.Init();
		_iPhase = (_iPhaseReq = OrganizePhase.Phase_ST);
		logicManager = new OrganizeManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID);
		_clsDetail.FirstInit();
		_clsList.FirstInit();
		_clsListDetail.FirstInit();
		_clsTop.FirstInit();
		isRun = false;
		yield return StartCoroutine(Util.WaitEndOfFrames(3));
		SingletonMonoBehaviour<UIPortFrame>.Instance.CircleUpdateInfo(logicManager);
		BGMFileInfos sceneBGM = (BGMFileInfos)logicManager.UserInfo.GetPortBGMId(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
		SoundUtils.SwitchBGM(sceneBGM);
		SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(null, isLockTouchOff: false);
		yield return new WaitForSeconds(0.2f);
		_clsTop.ShowBanner();
		yield return new WaitForSeconds(0.4f);
		if (!SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(logicManager.UserInfo.Tutorial, TutorialGuideManager.TutorialID.Organize, null, delegate
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.CloseMenu();
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: false);
			_clsTop.isControl = true;
		}))
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: false);
		}
		isRun = true;
		yield return null;
	}

	private void Update()
	{
		if (isRun)
		{
			_clsInputKey.Update();
			_clsTasks.Run();
			UpdateMode();
		}
	}

	public static void ReqPhase(OrganizePhase iPhase)
	{
		_iPhaseReq = iPhase;
	}

	public static OrganizePhase GetPhase()
	{
		return _iPhase;
	}

	protected void UpdateMode()
	{
		if (_iPhaseReq == OrganizePhase.Phase_BEF)
		{
			return;
		}
		switch (_iPhaseReq)
		{
		case OrganizePhase.Phase_ST:
			if (_clsTasks.Open(_clsTop) < 0)
			{
				return;
			}
			break;
		case OrganizePhase.Detail:
			if (_clsTasks.Open(_clsDetail) < 0)
			{
				return;
			}
			break;
		case OrganizePhase.List:
			if (_clsTasks.Open(_clsList) < 0)
			{
				return;
			}
			break;
		case OrganizePhase.ListDetail:
			if (_clsTasks.Open(_clsListDetail) < 0)
			{
				return;
			}
			break;
		}
		_iPhase = _iPhaseReq;
		_iPhaseReq = OrganizePhase.Phase_BEF;
		_clsTop.UpdateByModeChanging();
	}

	public static KeyControl GetKeyControl()
	{
		return _clsInputKey;
	}
}
