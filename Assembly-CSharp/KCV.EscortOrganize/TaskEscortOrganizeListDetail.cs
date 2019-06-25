using KCV.Organize;
using KCV.Utils;
using local.models;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.EscortOrganize
{
	public class TaskEscortOrganizeListDetail : TaskOrganizeListDetail
	{
		protected override void Start()
		{
			TaskOrganizeListDetail.KeyController = OrganizeTaskManager.GetKeyControl();
		}

		protected override bool Init()
		{
			isEnd = false;
			detailManager.buttons.LockSwitch.setChangeListViewIcon(TaskOrganizeList.ListScroll.ChangeLockBtnState);
			return true;
		}

		protected override bool Run()
		{
			if (isEnd)
			{
				if (changeState == "list")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.List);
				}
				else if (changeState == "top")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Phase_ST);
				}
				return false;
			}
			if (TaskOrganizeListDetail.KeyController.IsLeftDown())
			{
				if (!ship.IsLocked())
				{
					detailManager.buttons.LockSwitch.MoveLockBtn();
				}
			}
			else if (TaskOrganizeListDetail.KeyController.IsRightDown())
			{
				if (ship.IsLocked())
				{
					detailManager.buttons.LockSwitch.MoveLockBtn();
				}
			}
			else if (TaskOrganizeListDetail.KeyController.IsShikakuDown())
			{
				detailManager.buttons.LockSwitch.MoveLockBtn();
			}
			else if (TaskOrganizeListDetail.KeyController.IsMaruDown())
			{
				if (!isEnd)
				{
					ChangeButtonEL(null);
				}
			}
			else if (TaskOrganizeListDetail.KeyController.IsBatuDown())
			{
				BackDataEL(null);
			}
			else if (TaskOrganizeListDetail.KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			return true;
		}

		public override void Show(ShipModel ship)
		{
			base.ship = ship;
			index = 0;
			isControl = true;
			detailManager.SetDetailPanel(ship, isFirstDitail: false, OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), EscortOrganizeTaskManager.GetEscortManager(), TaskOrganizeTop.BannerIndex - 1, null);
			detailManager.Open();
		}

		public new void ChangeButtonEL(GameObject obj)
		{
			if (!isEnd)
			{
				int bannerIndex = TaskOrganizeTop.BannerIndex;
				int memId = ship.MemId;
				List<int> list = new List<int>();
				list.Add(memId);
				List<int> list2 = list;
				ShipModel[] ships = OrganizeTaskManager.Instance.GetTopTask().currentDeck.GetShips();
				if (bannerIndex - 1 < ships.Length)
				{
					list2.Add(ships[bannerIndex - 1].MemId);
				}
				if (!EscortOrganizeTaskManager.GetEscortManager().ChangeOrganize(OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), bannerIndex - 1, memId))
				{
					Debug.Log("EROOR: ChangeOrganize");
					return;
				}
				detailManager.Close();
				SoundUtils.PlaySE(SEFIleInfos.SE_003);
				EscortOrganizeTaskManager._clsList.BackListEL(null, isForce: true);
				EscortOrganizeTaskManager._clsTop.UpdateAllBannerByChangeShip();
				OrganizeTaskManager.Instance.GetListTask().UpdateList();
				ShipUtils.PlayShipVoice(ship, 13);
				setChangePhase("top");
			}
		}

		public new void BackDataEL(GameObject obj)
		{
			if (!isEnd)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonRemove);
				detailManager.Close();
				setChangePhase("list");
				TaskOrganizeList.KeyController = new KeyControl();
			}
		}

		public new void setChangePhase(string state)
		{
			changeState = state;
			isEnd = true;
		}

		protected override bool UnInit()
		{
			return true;
		}
	}
}
