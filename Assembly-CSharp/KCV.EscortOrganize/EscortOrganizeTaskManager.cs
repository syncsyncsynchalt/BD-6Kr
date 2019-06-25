using KCV.Organize;
using KCV.Strategy;
using KCV.Strategy.Deploy;
using KCV.Utils;
using local.managers;
using System.Collections;
using UnityEngine;

namespace KCV.EscortOrganize
{
	public class EscortOrganizeTaskManager : OrganizeTaskManager
	{
		public static int CurrentAreaID;

		private static EscortDeckManager EscortLogicManager;

		public new static TaskEscortOrganizeTop _clsTop;

		public new static TaskEscortOrganizeDetail _clsDetail;

		public new static TaskEscortOrganizeList _clsList;

		public new static TaskEscortOrganizeListDetail _clsListDetail;

		public TaskDeployTop DeployTop;

		public override IOrganizeManager GetLogicManager()
		{
			return EscortLogicManager;
		}

		public static EscortDeckManager GetEscortManager()
		{
			return EscortLogicManager;
		}

		public override TaskOrganizeTop GetTopTask()
		{
			return _clsTop;
		}

		public override TaskOrganizeDetail GetDetailTask()
		{
			return _clsDetail;
		}

		public override TaskOrganizeList GetListTask()
		{
			return _clsList;
		}

		public override TaskOrganizeListDetail GetListDetailTask()
		{
			return _clsListDetail;
		}

		protected override void Awake()
		{
			base.Awake();
		}

		private IEnumerator Start()
		{
			OrganizeTaskManager._clsTasks = base.gameObject.SafeGetComponent<SceneTasksMono>();
			OrganizeTaskManager._mainObj = GameObject.Find("DeployOrganizePanel").gameObject;
			GameObject task = base.transform.FindChild("Task").gameObject;
			_clsTop = ((Component)task.transform.FindChild("TaskTop")).GetComponent<TaskEscortOrganizeTop>();
			_clsDetail = ((Component)task.transform.FindChild("TaskDetail")).GetComponent<TaskEscortOrganizeDetail>();
			_clsList = ((Component)task.transform.FindChild("TaskList")).GetComponent<TaskEscortOrganizeList>();
			_clsListDetail = ((Component)task.transform.FindChild("TaskListDetail")).GetComponent<TaskEscortOrganizeListDetail>();
			OrganizeTaskManager._clsTasks.Init();
			OrganizeTaskManager._iPhase = (OrganizeTaskManager._iPhaseReq = OrganizePhase.Phase_ST);
			if (EscortLogicManager == null)
			{
				EscortLogicManager = new EscortDeckManager(StrategyAreaManager.FocusAreaID);
			}
			EscortLogicManager.InitEscortOrganizer();
			_clsDetail.FirstInit();
			_clsList.FirstInit();
			_clsListDetail.FirstInit();
			isRun = false;
			yield return new WaitForEndOfFrame();
			_clsTop.FirstInit();
			_clsTop.ShowBanner();
			yield return new WaitForSeconds(0.4f);
			isRun = true;
			yield return null;
		}

		public static void CreateLogicManager()
		{
			EscortLogicManager = new EscortDeckManager(StrategyAreaManager.FocusAreaID);
		}

		public static void Init()
		{
			if (OrganizeTaskManager._clsTasks != null)
			{
				OrganizeTaskManager._iPhase = (OrganizeTaskManager._iPhaseReq = OrganizePhase.Phase_ST);
			}
			StrategyTopTaskManager.Instance.UIModel.OverCamera.SetActive(isActive: false);
		}

		private void Update()
		{
			if (isRun)
			{
				OrganizeTaskManager._clsInputKey.Update();
				OrganizeTaskManager._clsTasks.Run();
				UpdateMode();
			}
		}

		public new static void ReqPhase(OrganizePhase iPhase)
		{
			OrganizeTaskManager._iPhaseReq = iPhase;
		}

		public new static OrganizePhase GetPhase()
		{
			return OrganizeTaskManager._iPhase;
		}

		protected new void UpdateMode()
		{
			if (OrganizeTaskManager._iPhaseReq == OrganizePhase.Phase_BEF)
			{
				return;
			}
			Debug.Log("リクエストされたフェ\u30fcズ:" + OrganizeTaskManager._iPhaseReq);
			switch (OrganizeTaskManager._iPhaseReq)
			{
			case OrganizePhase.Phase_ST:
				if (OrganizeTaskManager._clsTasks.Open(_clsTop) < 0)
				{
					return;
				}
				break;
			case OrganizePhase.Detail:
				if (OrganizeTaskManager._clsTasks.Open(_clsDetail) < 0)
				{
					return;
				}
				break;
			case OrganizePhase.List:
				if (OrganizeTaskManager._clsTasks.Open(_clsList) < 0)
				{
					return;
				}
				break;
			case OrganizePhase.ListDetail:
				if (OrganizeTaskManager._clsTasks.Open(_clsListDetail) < 0)
				{
					return;
				}
				break;
			}
			OrganizeTaskManager._iPhase = OrganizeTaskManager._iPhaseReq;
			OrganizeTaskManager._iPhaseReq = OrganizePhase.Phase_BEF;
			_clsTop.UpdateByModeChanging();
		}

		public void backToDeployTop()
		{
			TweenAlpha.Begin(base.transform.parent.gameObject, 0.2f, 0f);
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			_clsTop.UnVisibleEmptyFrame();
			this.DelayAction(0.2f, delegate
			{
				StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.Deploy);
			});
		}

		public void BGTouch()
		{
			if (OrganizeTaskManager._clsTasks.ChkRun(_clsTop) != -1 && !_clsList.isListOpen())
			{
				backToDeployTop();
				OrganizeTaskManager._clsTasks.CloseAll();
			}
		}
	}
}
