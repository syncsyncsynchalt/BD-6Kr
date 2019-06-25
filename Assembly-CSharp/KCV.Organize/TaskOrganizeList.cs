using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	public class TaskOrganizeList : SceneTaskMono
	{
		[SerializeField]
		protected GameObject _shipListPanel;

		[SerializeField]
		protected UITexture _maskList;

		[SerializeField]
		protected Camera Camera;

		protected bool isControl;

		protected bool IsCreate;

		protected bool IsShip;

		protected int shipNumber;

		protected bool IsCreated;

		protected bool isEnd;

		protected string changeState;

		protected GameObject _mainObj;

		protected ShipModel[] AllShips;

		protected ShipModel ship;

		public bool isEnabled;

		public static KeyControl KeyController;

		public static OrganizeScrollListParent ListScroll;

		[NonSerialized]
		public bool isInit;

		protected bool isShowFrame;

		protected override bool Init()
		{
			if (!ListScroll.isOpen)
			{
				ListScroll.MovePosition(205, isOpen: true);
				setList(isHeadFocus: true);
			}
			else
			{
				setList(isHeadFocus: false);
			}
			isInit = true;
			return true;
		}

		public void FirstInit()
		{
			if (!IsCreated)
			{
				IsCreate = false;
				IsShip = false;
				AllShips = OrganizeTaskManager.Instance.GetLogicManager().GetShipList();
				KeyController = new KeyControl();
				KeyController.useDoubleIndex(0, 3);
				_mainObj = OrganizeTaskManager.GetMainObject().transform.FindChild("OrganizeScrollListParent").gameObject;
				_shipListPanel = _mainObj.transform.FindChild("List/ListFrame/ShipListScroll").gameObject;
				_maskList = ((Component)_mainObj.transform.FindChild("Panel/ListBackMask")).GetComponent<UITexture>();
				UIButtonMessage component = _maskList.GetComponent<UIButtonMessage>();
				component.target = base.gameObject;
				component.functionName = "BackListEL";
				component.trigger = UIButtonMessage.Trigger.OnClick;
				ListScroll = ((Component)_mainObj.transform.FindChild("List")).GetComponent<OrganizeScrollListParent>();
				ListScroll.Initialize(OrganizeTaskManager.Instance.GetLogicManager(), Camera);
				ListScroll.HeadFocus();
				ListScroll.SetOnSelect(ListSelectEL);
				ListScroll.SetOnCancel(ListCancelEL);
				IsCreated = true;
			}
		}

		protected override bool UnInit()
		{
			isInit = false;
			return true;
		}

		private void OnDestroy()
		{
			_shipListPanel = null;
			_maskList = null;
			_mainObj = null;
			AllShips = null;
			ship = null;
			KeyController = null;
			ListScroll = null;
		}

		protected override bool Run()
		{
			isShowFrame = false;
			if (isEnd)
			{
				if (changeState == "listDetail")
				{
					OrganizeTaskManager.Instance.GetTopTask()._state2 = TaskOrganizeTop.OrganizeState.DetailList;
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.ListDetail);
				}
				else if (changeState == "detail")
				{
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Detail);
				}
				else if (changeState == "top")
				{
					OrganizeTaskManager.Instance.GetTopTask()._state2 = TaskOrganizeTop.OrganizeState.Top;
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Phase_ST);
				}
				isEnd = false;
				return false;
			}
			if (Input.touchCount == 0)
			{
				KeyController.Update();
			}
			if (KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			return true;
		}

		public void setShipNumber(ShipModel ship)
		{
			IsShip = true;
			this.ship = ship;
		}

		public virtual void setList(bool isHeadFocus)
		{
			if (!IsCreate)
			{
				KeyController.firstUpdate = true;
				KeyController.ClearKeyAll();
				updateList(isHeadFocus: true);
				IsCreate = true;
			}
			else
			{
				updateList(isHeadFocus);
			}
			ListScroll.SetKeyController(KeyController);
			ListScroll.StartControl();
		}

		public void ListSelectEL(ShipModel model)
		{
			if (!OrganizeTaskManager.Instance.GetListDetailTask().isEnabled)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
				OrganizeTaskManager.Instance.GetListTask().setChangePhase("listDetail");
				OrganizeTaskManager.Instance.GetListDetailTask().Show(model);
			}
		}

		public void ListCancelEL()
		{
			BackListEL(null);
		}

		protected virtual void updateList(bool isHeadFocus)
		{
			KeyController.KeyInputInterval = 0.05f;
		}

		public void Show(bool isShip)
		{
			if (!isEnabled)
			{
				Debug.Log("Show");
				KeyController = new KeyControl();
				ListScroll.SetKeyController(KeyController);
				_maskList.transform.localPosition = Vector3.zero;
				_maskList.SafeGetTweenAlpha(0f, 0.5f, 0.2f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, string.Empty);
				OrganizeTaskManager.Instance.GetTopTask()._state2 = TaskOrganizeTop.OrganizeState.List;
				isEnabled = true;
				isEnd = false;
				isShowFrame = true;
			}
		}

		protected void OnAnimationComp()
		{
		}

		public void BackListEL(GameObject obj)
		{
			BackListEL(obj, isForce: false);
		}

		public void BackListEL(GameObject obj, bool isForce)
		{
			if (isEnabled && !isShowFrame && (isInit || isForce))
			{
				Debug.Log("BackListEL");
				isEnabled = false;
				OrganizeTaskManager.Instance.GetTopTask().isControl = false;
				SoundUtils.PlaySE(SEFIleInfos.CommonRemove);
				ListScroll.MovePosition(1060, isOpen: false, compBackList);
				_maskList.SafeGetTweenAlpha(0.5f, 0f, 0.2f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, string.Empty);
				setChangePhase("top");
				KeyController.ClearKeyAll();
			}
		}

		private void compBackList()
		{
			OrganizeTaskManager.Instance.GetTopTask().isControl = true;
			ListScroll.transform.localPositionX(1060f);
			_maskList.alpha = 0f;
		}

		public void setChangePhase(string state)
		{
			changeState = state;
			isEnd = true;
		}

		internal void UpdateList()
		{
			ListScroll.RefreshViews();
		}
	}
}
