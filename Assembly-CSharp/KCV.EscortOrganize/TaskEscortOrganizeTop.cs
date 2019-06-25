using KCV.Organize;
using KCV.Strategy;
using KCV.Utils;
using local.managers;
using local.models;
using Sony.Vita.Dialog;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.EscortOrganize
{
	public class TaskEscortOrganizeTop : TaskOrganizeTop
	{
		private const BGMFileInfos SCENE_BGM = BGMFileInfos.Strategy;

		private IOrganizeManager logicManager;

		public override int GetDeckID()
		{
			return StrategyAreaManager.FocusAreaID;
		}

		private new void Start()
		{
			StateControllerDic = new Dictionary<string, StateController>();
			StateControllerDic.Add("banner", StateKeyControl_Banner);
			StateControllerDic.Add("system", StateKeyControl_System);
			StateControllerDic.Add("tender", base.StateKeyControl_Tender);
			base.currentDeck = EscortOrganizeTaskManager.GetEscortManager().EditDeck;
			StartRefGet();
			_fleetNameLabel.text = ((EscortDeckModel)base.currentDeck).Name;
			_fleetNameLabel.supportEncoding = false;
			mEditName = ((EscortDeckModel)base.currentDeck).Name;
			Ime.OnGotIMEDialogResult += base.OnGotIMEDialogResult;
			Main.Initialise();
		}

		private void StartRefGet()
		{
			GameObject gameObject = GameObject.Find("DeployOrganizePanel").gameObject;
			Util.FindParentToChild(ref _bgPanel, gameObject.transform, "DeployOrganizeButtons");
			Util.FindParentToChild(ref _bannerPanel, gameObject.transform, "Banner");
			GameObject.Find("DeckManager");
			UIPanel component = ((Component)_bgPanel.transform.FindChild("MiscContainer")).GetComponent<UIPanel>();
			Util.FindParentToChild(ref _fleetNameLabel, component.transform, "DeckNameLabel");
			Transform parent = _bgPanel.transform.FindChild("SideButtons");
			Util.FindParentToChild(ref _allUnsetBtn, parent, "AllUnsetBtn");
			Util.FindParentToChild(ref _tenderBtn, parent, "TenderBtn");
			Util.FindParentToChild(ref _fleetNameBtn, parent, "DeckNameBtn");
		}

		public new bool FirstInit()
		{
			if (!IsCreate)
			{
				TaskOrganizeTop.KeyController = OrganizeTaskManager.GetKeyControl();
				TaskOrganizeTop.KeyController.IsRun = false;
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: true);
				logicManager = OrganizeTaskManager.Instance.GetLogicManager();
				TaskOrganizeTop.decks = new EscortDeckModel[1]
				{
					OrganizeTaskManager.Instance.GetLogicManager().MapArea.GetEscortDeck()
				};
				ShipModel[] ships = TaskOrganizeTop.decks[0].GetShips();
				TaskOrganizeTop.allShip = logicManager.GetShipList();
				TaskOrganizeTop.BannerIndex = 1;
				TaskOrganizeTop.SystemIndex = 0;
				base.isControl = true;
				EscortOrganizeTaskManager._clsTop.setControlState();
				_bannerManager = new OrganizeBannerManager[6];
				for (int i = 0; i < 6; i++)
				{
					Util.FindParentToChild(ref _bannerManager[i], _bannerPanel.transform, "ShipSlot" + (i + 1));
					_bannerManager[i].init(i + 1, OnCheckDragDropTarget, OnDragDropStart, OnDragDropRelease, OnDragDropEnd, isInitPos: false);
					if (ships.Length > i)
					{
						_bannerManager[i].setBanner(ships[i], openAnimation: true, null, isShutterHide: true);
					}
					_bannerManager[i].UpdateBanner(enabled: false);
				}
				_bannerManager[0].UpdateBanner(enabled: true);
				this.DelayAction(0.3f, delegate
				{
					TaskOrganizeTop.KeyController.IsRun = true;
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: false);
				});
				Transform parent = _bgPanel.transform.FindChild("SideButtons");
				Util.FindParentToChild(ref _allUnsetBtn, parent, "AllUnsetBtn");
				Util.FindParentToChild(ref _tenderBtn, parent, "TenderBtn");
				Util.FindParentToChild(ref _fleetNameBtn, parent, "DeckNameBtn");
				base.currentDeck = EscortOrganizeTaskManager.GetEscortManager().EditDeck;
				UpdateSystemButtons();
				base.isControl = false;
				TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
				if (!SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(tutorial, TutorialGuideManager.TutorialID.EscortOrganize, null, delegate
				{
					IsCreate = true;
					base.isControl = true;
				}))
				{
					base.isControl = true;
					IsCreate = true;
				}
			}
			return true;
		}

		protected override bool Init()
		{
			if (TaskOrganizeList.ListScroll.isOpen)
			{
				TaskOrganizeList.ListScroll.OnCancel();
			}
			return true;
		}

		protected override bool UnInit()
		{
			return true;
		}

		protected override bool Run()
		{
			if (!isInit)
			{
				Init();
				isInit = true;
			}
			Main.Update();
			if (isEnd)
			{
				if (TaskOrganizeTop.changeState == "detail")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Detail);
				}
				else if (TaskOrganizeTop.changeState == "list")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.List);
				}
				isEnd = false;
				return false;
			}
			if (!base.isControl)
			{
				return true;
			}
			if (TaskOrganizeTop.controlState != null && StateControllerDic.ContainsKey(TaskOrganizeTop.controlState))
			{
				return StateControllerDic[TaskOrganizeTop.controlState]();
			}
			return true;
		}

		private new bool StateKeyControl_Banner()
		{
			if (TaskOrganizeTop.KeyController.IsMaruDown())
			{
				_bannerManager[TaskOrganizeTop.BannerIndex - 1].DetailEL(null);
				return true;
			}
			if (isTenderAnimation())
			{
				return true;
			}
			if (TaskOrganizeTop.KeyController.IsBatuDown())
			{
				((EscortOrganizeTaskManager)OrganizeTaskManager.Instance).backToDeployTop();
				return false;
			}
			if (TaskOrganizeTop.KeyController.IsShikakuDown())
			{
				SoundUtils.PlaySE(SEFIleInfos.MainMenuOnClick);
				AllUnsetBtnEL();
			}
			else if (TaskOrganizeTop.KeyController.IsDownDown())
			{
				TaskOrganizeTop.BannerIndex += 2;
				if (TaskOrganizeTop.BannerIndex >= 7)
				{
					TaskOrganizeTop.BannerIndex -= 6;
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				UpdateChangeBanner();
			}
			else if (TaskOrganizeTop.KeyController.IsUpDown())
			{
				TaskOrganizeTop.BannerIndex -= 2;
				if (TaskOrganizeTop.BannerIndex <= 0)
				{
					TaskOrganizeTop.BannerIndex += 6;
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				UpdateChangeBanner();
			}
			else if (TaskOrganizeTop.KeyController.IsLeftDown())
			{
				TaskOrganizeTop.BannerIndex--;
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				if (TaskOrganizeTop.BannerIndex == 0)
				{
					if (IsTenderBtn() || IsAllUnsetBtn())
					{
						bool flag = false;
						if (IsTenderBtn())
						{
							TaskOrganizeTop.SystemIndex = 0;
							flag = true;
						}
						if (!flag)
						{
							TaskOrganizeTop.SystemIndex = 1;
						}
					}
					else
					{
						TaskOrganizeTop.SystemIndex = 2;
					}
					UpdateSystemButtons();
					OrganizeTaskManager.Instance.GetTopTask().setControlState();
				}
				UpdateChangeBanner();
			}
			else if (TaskOrganizeTop.KeyController.IsRightDown())
			{
				if (TaskOrganizeTop.BannerIndex >= 6)
				{
					return true;
				}
				TaskOrganizeTop.BannerIndex++;
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				for (int i = 0; i < 6; i++)
				{
					if (TaskOrganizeTop.BannerIndex - 1 == i)
					{
						_bannerManager[i].UpdateBanner(enabled: true);
					}
					else
					{
						_bannerManager[i].UpdateBanner(enabled: false);
					}
				}
			}
			else if (TaskOrganizeTop.KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			return true;
		}

		private new bool StateKeyControl_System()
		{
			if (TaskOrganizeTop.KeyController.IsMaruDown() && TaskOrganizeTop.controlState == "system")
			{
				if (TaskOrganizeTop.SystemIndex == 0)
				{
					TenderManager.ShowSelectTender();
					OrganizeTaskManager.Instance.GetTopTask().setControlState();
				}
				else if (TaskOrganizeTop.SystemIndex == 1)
				{
					AllUnsetBtnEL();
					TaskOrganizeTop.SystemIndex = 0;
					TaskOrganizeTop.BannerIndex = 1;
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					UpdateChangeBanner();
					OrganizeTaskManager.Instance.GetTopTask().setControlState();
				}
				else
				{
					Debug.Log("openDeckNameInput");
					openDeckNameInput();
				}
				return true;
			}
			if (isTenderAnimation())
			{
				return true;
			}
			if (TaskOrganizeTop.KeyController.IsBatuDown())
			{
				((EscortOrganizeTaskManager)OrganizeTaskManager.Instance).backToDeployTop();
				return false;
			}
			if (TaskOrganizeTop.KeyController.IsDownDown())
			{
				if (TaskOrganizeTop.SystemIndex >= 2)
				{
					TaskOrganizeTop.SystemIndex = 2;
					return true;
				}
				int systemIndex = TaskOrganizeTop.SystemIndex;
				TaskOrganizeTop.SystemIndex++;
				if (TaskOrganizeTop.SystemIndex == 1 && !IsAllUnsetBtn())
				{
					TaskOrganizeTop.SystemIndex = 2;
				}
				UpdateSystemButtons();
				if (systemIndex != TaskOrganizeTop.SystemIndex)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
			else if (TaskOrganizeTop.KeyController.IsUpDown())
			{
				if (TaskOrganizeTop.SystemIndex <= 0)
				{
					Debug.Log("KEY:" + TaskOrganizeTop.KeyController.Index);
					TaskOrganizeTop.SystemIndex = 0;
					return true;
				}
				int systemIndex2 = TaskOrganizeTop.SystemIndex;
				TaskOrganizeTop.SystemIndex--;
				if (TaskOrganizeTop.SystemIndex == 1 && !IsAllUnsetBtn())
				{
					if (!IsTenderBtn())
					{
						TaskOrganizeTop.SystemIndex = 2;
					}
					else
					{
						TaskOrganizeTop.SystemIndex = 0;
					}
				}
				if (TaskOrganizeTop.SystemIndex == 0 && !IsTenderBtn())
				{
					if (!IsAllUnsetBtn())
					{
						TaskOrganizeTop.SystemIndex = 2;
					}
					else
					{
						TaskOrganizeTop.SystemIndex = 1;
					}
				}
				Debug.Log("KEY:" + TaskOrganizeTop.KeyController.Index + " KEE:" + TaskOrganizeTop.BannerIndex);
				UpdateSystemButtons();
				if (systemIndex2 != TaskOrganizeTop.SystemIndex)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
			else if (TaskOrganizeTop.KeyController.IsRightDown())
			{
				TaskOrganizeTop.BannerIndex++;
				UpdateSystemButtons();
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				UpdateChangeBanner();
				OrganizeTaskManager.Instance.GetTopTask().setControlState();
			}
			else if (TaskOrganizeTop.KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			return true;
		}

		public void UnVisibleEmptyFrame()
		{
			for (int i = 0; i < 6; i++)
			{
				if (!_bannerManager[i].IsSet)
				{
					_bannerManager[i].SetShipFrameActive(active: false);
				}
			}
		}

		public override void UpdateDeckSwitchManager()
		{
		}

		private bool OnCheckDragDropTarget(OrganizeBannerManager target)
		{
			return false;
		}

		private void OnDragDropStart(OrganizeBannerManager target)
		{
		}

		private bool OnDragDropRelease(OrganizeBannerManager target)
		{
			OrganizeManager organizeManager = OrganizeTaskManager.logicManager;
			DeckModel currentDeck = deckSwitchManager.currentDeck;
			ShipModel ship = target.ship;
			ShipModel ship2 = _uiDragDropItem.ship;
			if (organizeManager.ChangeOrganize(currentDeck.Id, target.number - 1, ship2.MemId))
			{
				target.setBanner(ship2, openAnimation: true, null);
				_uiDragDropItem.setBanner(ship, openAnimation: true, delegate
				{
					OnDragDropEnd();
				});
			}
			return true;
		}

		private void OnDragDropEnd()
		{
		}

		private void OnDestroy()
		{
			_bgPanel = null;
			_bannerPanel = null;
			_allUnsetBtn = null;
			_tenderBtn = null;
			_fleetNameBtn = null;
			_fleetNameLabel = null;
			mTransform_TurnEndStamp = null;
			deckIcon = null;
			Mem.DelDictionarySafe(ref StateControllerDic);
			_bannerManager = null;
			TaskOrganizeTop.SystemIndex = 0;
			TaskOrganizeTop.prevControlState = string.Empty;
			TaskOrganizeTop.changeState = string.Empty;
			Mem.Del(ref TaskOrganizeTop.BannerIndex);
			Mem.Del(ref TaskOrganizeTop.controlState);
			uiCamera = null;
			TenderManager = null;
			base.currentDeck = null;
			deckSwitchManager = null;
			TaskOrganizeTop.decks = null;
			TaskOrganizeTop.allShip = null;
			TaskOrganizeTop.KeyController = null;
		}
	}
}
