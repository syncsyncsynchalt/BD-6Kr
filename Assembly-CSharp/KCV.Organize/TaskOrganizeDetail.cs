using KCV.Utils;
using local.models;
using UnityEngine;

namespace KCV.Organize
{
	public class TaskOrganizeDetail : SceneTaskMono
	{
		[SerializeField]
		protected OrganizeDetail_Manager detailManager;

		protected bool isControl;

		protected bool isCreate;

		protected bool isEnd;

		protected string changeState;

		protected ShipModel ship;

		public int bannerIndex;

		public static KeyControl KeyController;

		public UiStarManager StarManager;

		public bool isEnabled => detailManager.isShow;

		protected override bool Init()
		{
			detailManager.Init();
			detailManager.Open();
			isEnd = false;
			return true;
		}

		public void FirstInit()
		{
			if (!isCreate)
			{
				KeyController = OrganizeTaskManager.GetKeyControl();
				isCreate = true;
			}
		}

		protected override bool UnInit()
		{
			return true;
		}

		private void OnDestroy()
		{
			StarManager = null;
			ship = null;
			detailManager = null;
			KeyController = null;
		}

		protected override bool Run()
		{
			if (isEnd)
			{
				if (changeState == "top")
				{
					OrganizeTaskManager.Instance.GetTopTask()._state2 = TaskOrganizeTop.OrganizeState.Top;
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Phase_ST);
				}
				else if (changeState == "list")
				{
					OrganizeTaskManager.Instance.GetTopTask()._state2 = TaskOrganizeTop.OrganizeState.List;
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.List);
				}
				return false;
			}
			if (KeyController.IsLeftDown())
			{
				detailManager.buttons.UpdateButton(isLeft: true);
			}
			else if (KeyController.IsRightDown())
			{
				detailManager.buttons.UpdateButton(isLeft: false);
			}
			else if (KeyController.IsMaruDown())
			{
				detailManager.buttons.Decide();
			}
			else if (KeyController.IsBatuDown())
			{
				BackMaskEL(null);
			}
			if (KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			return true;
		}

		public bool CheckBtnEnabled()
		{
			return (!isEnabled && !OrganizeTaskManager.Instance.GetTopTask().isTenderAnimation()) ? true : false;
		}

		public void Show(ShipModel ship)
		{
			if (!isEnabled)
			{
				this.ship = ship;
				changeState = string.Empty;
				detailManager.SetDetailPanel(this.ship, isFirstDitail: true, OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), OrganizeTaskManager.Instance.GetLogicManager(), TaskOrganizeTop.BannerIndex - 1, null);
				detailManager.Open();
				isEnd = false;
				isControl = true;
			}
		}

		protected void BackMaskEL(GameObject obj)
		{
			if (!isEnd)
			{
				isControl = false;
				detailManager.Close();
				SoundUtils.PlaySE(SEFIleInfos.CommonRemove);
				changeState = "top";
				isEnd = true;
			}
		}

		public void SetBtnEL(GameObject obj)
		{
			if (string.IsNullOrEmpty(changeState) && !isEnd && isControl)
			{
				isControl = false;
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
				changeState = "list";
				isEnd = true;
				OrganizeTaskManager.Instance.GetListTask().setShipNumber(ship);
				OrganizeTaskManager.Instance.GetListTask().Show(isShip: true);
				detailManager.Close();
			}
		}

		public void ResetBtnEL(GameObject obj)
		{
			if (string.IsNullOrEmpty(changeState) && !isEnd && isControl)
			{
				TutorialModel tutorial = OrganizeTaskManager.logicManager.UserInfo.Tutorial;
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID != 1 && SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckFirstTutorial(tutorial, TutorialGuideManager.TutorialID.Bring))
				{
					SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(tutorial, TutorialGuideManager.TutorialID.Bring, null);
					return;
				}
				isEnd = true;
				isControl = false;
				detailManager.Close();
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
				changeState = "top";
				OrganizeTaskManager.Instance.GetLogicManager().UnsetOrganize(OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), TaskOrganizeTop.BannerIndex - 1);
				OrganizeTaskManager.Instance.GetTopTask().UpdateAllBannerByRemoveShip(allReset: false);
				OrganizeTaskManager.Instance.GetTopTask().UpdateAllSelectBanner();
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.setDisable();
				OrganizeTaskManager.Instance.GetListTask().UpdateList();
			}
		}
	}
}
