using KCV.Organize;
using UnityEngine;

namespace KCV.EscortOrganize
{
	public class TaskEscortOrganizeList : TaskOrganizeList
	{
		protected override void Start()
		{
			IsCreate = false;
			IsShip = false;
			TaskOrganizeList.KeyController = new KeyControl();
			TaskOrganizeList.KeyController.useDoubleIndex(0, 3);
			_mainObj = OrganizeTaskManager.GetMainObject().transform.FindChild("OrganizeScrollListParent").gameObject;
			_shipListPanel = _mainObj.transform.FindChild("List/ListFrame/ShipListScroll").gameObject;
			_maskList = ((Component)_mainObj.transform.FindChild("Panel/ListBackMask")).GetComponent<UITexture>();
			UIButtonMessage component = _maskList.GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = "BackListEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
		}

		protected override bool Init()
		{
			if (!TaskOrganizeList.ListScroll.isOpen)
			{
				TaskOrganizeList.ListScroll.MovePosition(205, isOpen: true);
				setList(isHeadFocus: true);
			}
			else
			{
				setList(isHeadFocus: false);
			}
			isInit = true;
			return true;
		}

		protected override bool UnInit()
		{
			return true;
		}

		protected override bool Run()
		{
			isShowFrame = false;
			if (isEnd)
			{
				if (changeState == "listDetail")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.ListDetail);
				}
				else if (changeState == "detail")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Detail);
				}
				else if (changeState == "top")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Phase_ST);
				}
				isEnd = false;
				Debug.Log("deff");
				return false;
			}
			TaskOrganizeList.KeyController.Update();
			if (!TaskOrganizeList.KeyController.IsShikakuDown())
			{
				if (TaskOrganizeList.KeyController.IsBatuDown())
				{
					changeState = "top";
					isEnd = true;
					BackListEL(null);
				}
				else if (TaskOrganizeList.KeyController.IsRDown())
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
				}
			}
			return true;
		}

		public bool isListOpen()
		{
			return TaskOrganizeList.ListScroll.isOpen;
		}
	}
}
