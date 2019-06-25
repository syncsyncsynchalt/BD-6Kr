using Common.Enum;
using KCV.EscortOrganize;
using KCV.Utils;
using local.models;
using UnityEngine;

namespace KCV.Organize
{
	public class TaskOrganizeListDetail : SceneTaskMono
	{
		[SerializeField]
		protected OrganizeDetail_Manager detailManager;

		protected bool isCreate;

		protected bool isControl;

		protected bool isEnd;

		protected string changeState;

		protected ShipModel[] ships;

		public int index;

		public ShipModel ship;

		public static KeyControl KeyController;

		public bool isEnabled => detailManager.isShow;

		protected override bool Init()
		{
			isEnd = false;
			detailManager.buttons.LockSwitch.setChangeListViewIcon(TaskOrganizeList.ListScroll.ChangeLockBtnState);
			return true;
		}

		public virtual void FirstInit()
		{
			if (!isCreate)
			{
				KeyController = OrganizeTaskManager.GetKeyControl();
				detailManager.Init();
				index = 0;
				isControl = false;
				isEnd = false;
				isCreate = true;
			}
		}

		protected override bool Run()
		{
			if (isEnd)
			{
				if (changeState == "list")
				{
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.List);
				}
				else if (changeState == "top")
				{
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Phase_ST);
				}
				return false;
			}
			if (KeyController.IsLeftDown())
			{
				if (!ship.IsLocked())
				{
					detailManager.buttons.LockSwitch.MoveLockBtn();
				}
			}
			else if (KeyController.IsRightDown())
			{
				if (ship.IsLocked())
				{
					detailManager.buttons.LockSwitch.MoveLockBtn();
				}
			}
			else if (KeyController.IsShikakuDown())
			{
				detailManager.buttons.LockSwitch.MoveLockBtn();
			}
			else if (KeyController.IsMaruDown())
			{
				if (!isEnd)
				{
					ChangeButtonEL(null);
				}
			}
			else if (KeyController.IsBatuDown())
			{
				BackDataEL(null);
			}
			if (KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			return true;
		}

		public virtual void Show(ShipModel ship)
		{
			if (!isEnabled)
			{
				this.ship = ship;
				index = 0;
				isControl = true;
				detailManager.SetDetailPanel(this.ship, isFirstDitail: false, OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), OrganizeTaskManager.Instance.GetLogicManager(), TaskOrganizeTop.BannerIndex - 1, null);
				detailManager.Open();
				OrganizeTaskManager.Instance.GetTopTask()._state2 = TaskOrganizeTop.OrganizeState.DetailList;
			}
		}

		public void ChangeButtonEL(GameObject obj)
		{
			if (isEnd || !isControl)
			{
				return;
			}
			int bannerIndex = TaskOrganizeTop.BannerIndex;
			int memId = ship.MemId;
			bool flag = OrganizeTaskManager.Instance.GetLogicManager().ChangeOrganize(OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), bannerIndex - 1, memId);
			OrganizeTaskManager.Instance.GetListTask().UpdateList();
			if (!flag)
			{
				Debug.Log("EROOR: ChangeOrganize");
				return;
			}
			DifficultKind difficulty = OrganizeTaskManager.logicManager.UserInfo.Difficulty;
			isControl = false;
			OrganizeTaskManager.Instance.GetTopTask().isControl = false;
			detailManager.Close();
			SoundUtils.PlaySE(SEFIleInfos.SE_003);
			OrganizeTaskManager.Instance.GetListTask().BackListEL(null, isForce: true);
			OrganizeTaskManager.Instance.GetTopTask().UpdateAllBannerByChangeShip();
			ShipUtils.PlayShipVoice(ship, 13);
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.setDisable();
			setChangePhase("top");
			if (OrganizeTaskManager.Instance.GetType() != typeof(EscortOrganizeTaskManager))
			{
				TutorialModel tutorial = OrganizeTaskManager.logicManager.UserInfo.Tutorial;
				if (tutorial.GetStep() == 3 && !tutorial.GetStepTutorialFlg(4))
				{
					tutorial.SetStepTutorialFlg(4);
					CommonPopupDialog.Instance.StartPopup("「艦隊を編成！」 達成");
					SoundUtils.PlaySE(SEFIleInfos.SE_012);
				}
			}
		}

		public void BackDataEL(GameObject obj)
		{
			if (!isEnd && isControl)
			{
				isEnd = true;
				detailManager.Close();
				setChangePhase("list");
				OrganizeTaskManager.Instance.GetTopTask()._state2 = TaskOrganizeTop.OrganizeState.List;
				TaskOrganizeList.KeyController = new KeyControl();
				TaskOrganizeList.KeyController.useDoubleIndex(0, 3);
				SoundUtils.PlaySE(SEFIleInfos.CommonRemove);
			}
		}

		public void setChangePhase(string state)
		{
			changeState = state;
			isEnd = true;
		}

		protected override bool UnInit()
		{
			return true;
		}

		private void OnDestroy()
		{
			detailManager = null;
			ships = null;
			ship = null;
			KeyController = null;
		}
	}
}
