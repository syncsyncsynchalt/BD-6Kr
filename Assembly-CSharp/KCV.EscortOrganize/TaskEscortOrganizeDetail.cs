using KCV.Organize;
using KCV.Utils;
using local.models;
using UnityEngine;

namespace KCV.EscortOrganize
{
	public class TaskEscortOrganizeDetail : TaskOrganizeDetail
	{
		protected override bool Init()
		{
			TaskOrganizeDetail.KeyController = OrganizeTaskManager.GetKeyControl();
			detailManager.Open();
			isEnd = false;
			return true;
		}

		protected override bool Run()
		{
			if (isEnd)
			{
				if (changeState == "top")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Phase_ST);
				}
				else if (changeState == "list")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.List);
				}
				return false;
			}
			if (TaskOrganizeDetail.KeyController.IsLeftDown())
			{
				detailManager.buttons.UpdateButton(isLeft: true);
			}
			else if (TaskOrganizeDetail.KeyController.IsRightDown())
			{
				detailManager.buttons.UpdateButton(isLeft: false);
			}
			else if (TaskOrganizeDetail.KeyController.IsMaruDown())
			{
				detailManager.buttons.Decide();
			}
			else if (TaskOrganizeDetail.KeyController.IsBatuDown())
			{
				BackMaskEL(null);
			}
			else if (TaskOrganizeDetail.KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			return true;
		}

		public new bool CheckBtnEnabled()
		{
			bool result = true;
			if (base.isEnabled || EscortOrganizeTaskManager._clsTop.isTenderAnimation())
			{
				result = false;
			}
			return result;
		}

		public new void Show(ShipModel ship)
		{
			base.ship = ship;
			changeState = string.Empty;
			detailManager.SetDetailPanel(base.ship, isFirstDitail: true, OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), EscortOrganizeTaskManager.GetEscortManager(), TaskOrganizeTop.BannerIndex - 1, null);
			detailManager.Open();
			isEnd = false;
		}

		private new void BackMaskEL(GameObject obj)
		{
			if (!isEnd)
			{
				detailManager.Close();
				SoundUtils.PlaySE(SEFIleInfos.CommonRemove);
				changeState = "top";
				isEnd = true;
			}
		}

		public new void SetBtnEL(GameObject obj)
		{
			if (string.IsNullOrEmpty(changeState))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
				changeState = "list";
				isEnd = true;
				EscortOrganizeTaskManager._clsList.setShipNumber(ship);
				EscortOrganizeTaskManager._clsList.Show(isShip: true);
				detailManager.Close();
			}
		}

		public new void ResetBtnEL(GameObject obj)
		{
			if (string.IsNullOrEmpty(changeState) && !isEnd)
			{
				detailManager.Close();
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
				changeState = "top";
				isEnd = true;
				OrganizeTaskManager.Instance.GetLogicManager().UnsetOrganize(OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), TaskOrganizeTop.BannerIndex - 1);
				EscortOrganizeTaskManager._clsTop.UpdateAllBannerByRemoveShip(allReset: false);
				EscortOrganizeTaskManager._clsTop.UpdateAllSelectBanner();
				OrganizeTaskManager.Instance.GetListTask().UpdateList();
			}
		}

		protected override bool UnInit()
		{
			return true;
		}
	}
}
