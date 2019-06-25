using Common.Enum;
using KCV.CommandMenu;
using KCV.Display;
using KCV.Scene.Practice;
using KCV.Utils;
using local.managers;
using local.models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Strategy
{
	public class TaskStrategyCommandMenu : SceneTaskMono
	{
		private enum Scene
		{
			NONE,
			EXPEDISION,
			PRACTICE,
			ORGANIZE
		}

		private enum MENU_NAME
		{
			SALLY,
			MOVE,
			DEPLOY,
			ENSEI,
			ENSYU,
			INFO,
			TURNEND,
			PORT,
			_NUM
		}

		private delegate bool CommandMenuMethod();

		public GameObject ENSEI;

		[SerializeField]
		public GameObject EscortOrganize;

		[SerializeField]
		private GameObject ENSEI_Cancel;

		[SerializeField]
		private UserInterfacePracticeManager mPrefab_UserInterfacePracticeManager;

		private UserInterfacePracticeManager mUserInterfacePracticeManager;

		private StrategyTopTaskManager sttm;

		private TaskStrategySailSelect sailSelect;

		private int areaID;

		private int IndexChange;

		private bool swipeWait;

		public RotateMenu_Strategy2 CommandMenu;

		private StrategyMapManager LogicMng;

		public UILabel ShipNumberLabel;

		private MapAreaModel areaModel;

		public GameObject warningPanel;

		private bool sceneChange;

		public GameObject InfoRoot;

		public GameObject MapRoot;

		public GameObject OverView;

		public GameObject OverSceneObject;

		private StopExpedition StopExpeditionPanel;

		public KeyControl keyController;

		private UIDisplaySwipeEventRegion SwipeEvent;

		private int Debug_MstID = 1;

		private Scene OverScene;

		private List<CommandMenuMethod> pushCommandMenuList;

		private bool isKeyControlDisable;

		private MENU_NAME currentMenu;

		private bool isChangeingDeck;

		private bool isInfoOpenEnable;

		private float prevMoveX;

		private float prevMoveY;

		private new void Awake()
		{
			pushCommandMenuList = new List<CommandMenuMethod>();
			pushCommandMenuList.Add(pushSally);
			pushCommandMenuList.Add(pushMove);
			pushCommandMenuList.Add(pushDeploy);
			pushCommandMenuList.Add(pushExpedition);
			pushCommandMenuList.Add(pushPractice);
			pushCommandMenuList.Add(pushInfo);
			pushCommandMenuList.Add(pushTurnEnd);
			pushCommandMenuList.Add(pushPort);
		}

		protected override void Start()
		{
			OverScene = Scene.NONE;
			keyController = new KeyControl();
			keyController.setChangeValue(-1f, 0f, 1f, 0f);
			keyController.KeyInputInterval = 0.2f;
			CommandMenu.Init(keyController);
			currentMenu = MENU_NAME.SALLY;
			sailSelect = StrategyTopTaskManager.GetSailSelect();
			isInfoOpenEnable = true;
		}

		protected override bool Init()
		{
			if (!isKeyControlDisable)
			{
				keyController.IsRun = true;
			}
			IndexChange = 0;
			swipeWait = false;
			SwipeEvent = ((Component)CommandMenu.transform.FindChild("AreaInfoBG")).GetComponent<UIDisplaySwipeEventRegion>();
			SwipeEvent.SetOnSwipeActionJudgeCallBack(CheckSwipe);
			LogicMng = StrategyTopTaskManager.GetLogicManager();
			CommandMenu.SetActive(isActive: true);
			Util.FindParentToChild(ref CommandMenu.Menus, CommandMenu.transform, "Menus");
			sttm = StrategyTaskManager.GetStrategyTop();
			areaID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
			areaModel = StrategyTopTaskManager.GetLogicManager().Area[areaID];
			sceneChange = true;
			DeckEnableCheck();
			CommandMenu.MenuEnter((int)currentMenu);
			keyController.Index = (int)currentMenu;
			KeyControlManager.Instance.KeyController = keyController;
			isInfoOpenEnable = true;
			if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
			}
			return true;
		}

		protected override bool Run()
		{
			if (OverScene != 0)
			{
				return true;
			}
			if (StopExpeditionPanel != null)
			{
				return true;
			}
			keyController.Update();
			if (IndexChange != 0 && !keyController.IsChangeIndex)
			{
				keyController.Index += IndexChange;
				IndexChange = 0;
			}
			if (keyController.IsRightDown())
			{
				StrategyTopTaskManager.GetSailSelect().DeckSelectController.Index++;
				StrategyTopTaskManager.GetSailSelect().SearchAndChangeDeck(isNext: true, isSeachLocalArea: true);
				if (StrategyTopTaskManager.GetSailSelect().PrevDeckID != SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID)
				{
					StrategyTopTaskManager.GetSailSelect().changeDeck(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
					StrategyTopTaskManager.Instance.UIModel.Character.PlayVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
					DeckEnableCheck();
					CommandMenu.setFocus();
				}
			}
			else if (keyController.IsLeftDown())
			{
				StrategyTopTaskManager.GetSailSelect().DeckSelectController.Index--;
				StrategyTopTaskManager.GetSailSelect().SearchAndChangeDeck(isNext: false, isSeachLocalArea: true);
				if (StrategyTopTaskManager.GetSailSelect().PrevDeckID != SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID)
				{
					StrategyTopTaskManager.GetSailSelect().changeDeck(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
					StrategyTopTaskManager.Instance.UIModel.Character.PlayVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
					DeckEnableCheck();
					CommandMenu.setFocus();
				}
			}
			if (keyController.IsChangeIndex && !SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsFocus)
			{
				CommandMenu.moveCursol();
			}
			if (keyController.keyState[14].press || keyController.keyState[10].press)
			{
				isChangeingDeck = true;
			}
			else
			{
				isChangeingDeck = false;
			}
			if (keyController.keyState[1].down)
			{
				pushMenuButton();
			}
			else if (keyController.keyState[0].down)
			{
				currentMenu = MENU_NAME.SALLY;
				ExitCommandMenu();
			}
			if (keyController.keyState[5].down)
			{
				pushPort();
			}
			return sceneChange;
		}

		public void pushMenuButton()
		{
			if (isChangeingDeck || StrategyTopTaskManager.Instance.UIModel.MapCamera.isMoving)
			{
				return;
			}
			if (!StrategyTopTaskManager.GetCommandMenu().isRun)
			{
				Debug.Log("Not CommandMenuMode return");
				return;
			}
			if (!CommandMenu.isOpen)
			{
				Debug.Log("Not CommandMenu Open return");
				return;
			}
			int num = areaModel.GetDecks().Length;
			bool flag;
			if (isMenuActive((MENU_NAME)keyController.Index))
			{
				Debug.Log("PUSH");
				flag = pushCommandMenuList[keyController.Index]();
				CommandMenu.menuParts[keyController.Index].setColider(isEnable: false);
			}
			else
			{
				flag = false;
			}
			if (flag)
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
			}
			else
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonCancel2);
			}
		}

		private bool isMenuActive(MENU_NAME MenuName)
		{
			return CommandMenu.menuParts[(int)MenuName].menuState != CommandMenuParts.MenuState.Disable;
		}

		public void ExitCommandMenu()
		{
			Debug.Log("ExitCommandMenu");
			if (!isChangeingDeck)
			{
				CommandMenu.MenuExit();
				StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_ST);
				StrategyTopTaskManager.Instance.GetInfoMng().EnterInfoPanel(0.4f);
				sceneChange = false;
				SoundUtils.PlayOneShotSE(SEFIleInfos.SE_037);
			}
		}

		public void DeckEnableCheck()
		{
			for (int i = 0; i < CommandMenu.menuParts.Length; i++)
			{
				CommandMenu.menuParts[i].SetMenuState(CommandMenuParts.MenuState.Forcus);
				CommandMenu.menuParts[i].SetMenuState(CommandMenuParts.MenuState.NonForcus);
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.IsActionEnd() || SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState != 0 || SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.HasBling())
			{
				CommandMenu.menuParts[0].SetMenuState(CommandMenuParts.MenuState.Disable);
				CommandMenu.menuParts[1].SetMenuState(CommandMenuParts.MenuState.Disable);
				CommandMenu.menuParts[4].SetMenuState(CommandMenuParts.MenuState.Disable);
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.IsActionEnd() || SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.HasBling())
			{
				CommandMenu.menuParts[3].SetMenuState(CommandMenuParts.MenuState.Disable);
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState == MissionStates.STOP)
			{
				CommandMenu.menuParts[3].SetMenuState(CommandMenuParts.MenuState.Disable);
			}
			if (StrategyAreaManager.FocusAreaID != SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId || SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Count <= 0)
			{
				if (currentMenu != MENU_NAME.INFO)
				{
					currentMenu = MENU_NAME.DEPLOY;
				}
				CommandMenu.menuParts[0].SetMenuState(CommandMenuParts.MenuState.Disable);
				CommandMenu.menuParts[1].SetMenuState(CommandMenuParts.MenuState.Disable);
				CommandMenu.menuParts[4].SetMenuState(CommandMenuParts.MenuState.Disable);
				CommandMenu.menuParts[3].SetMenuState(CommandMenuParts.MenuState.Disable);
			}
			MissionStates missionState = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState;
			if (missionState != 0)
			{
				if (currentMenu != MENU_NAME.INFO)
				{
					currentMenu = MENU_NAME.DEPLOY;
				}
				CommandMenu.menuParts[7].SetMenuState(CommandMenuParts.MenuState.Disable);
			}
			CommandMenu.SetMissionState(missionState);
			if (!CheckActiveDeckExist())
			{
				currentMenu = MENU_NAME.TURNEND;
			}
			else if (currentMenu == MENU_NAME.TURNEND || currentMenu == MENU_NAME.MOVE)
			{
				currentMenu = MENU_NAME.SALLY;
			}
		}

		private bool CheckActiveDeckExist()
		{
			return StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecks().Any((DeckModel x) => !x.IsActionEnd());
		}

		private bool pushSally()
		{
			if (!validCheck(MENU_NAME.SALLY))
			{
				return false;
			}
			if (StrategyTopTaskManager.Instance.UIModel.MapCamera.GetComponent<iTween>() != null)
			{
				return true;
			}
			CommandMenu.MenuExit();
			StrategyTopTaskManager.Instance.ShipIconManager.SetVisible(isVisible: false);
			StrategyTopTaskManager.Instance.GetAreaMng().tileRouteManager.HideRoute();
			StrategyTopTaskManager.Instance.TileManager.SetVisible(isVisible: false);
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.MapSelect);
			StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(isEnter: false, null);
			sceneChange = false;
			currentMenu = MENU_NAME.SALLY;
			if (StrategyTopTaskManager.Instance.TutorialGuide6_2 != null)
			{
				StrategyTopTaskManager.Instance.TutorialGuide6_2.Hide();
			}
			return true;
		}

		private bool pushMove()
		{
			if (!validCheck(MENU_NAME.MOVE))
			{
				return false;
			}
			CommandMenu.MenuExit();
			StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(isEnter: false, null);
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.ShipMove);
			ShipUtils.PlayShipVoice(SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel, 14);
			currentMenu = MENU_NAME.MOVE;
			sceneChange = false;
			return true;
		}

		private bool pushDeploy()
		{
			if (!validCheck(MENU_NAME.DEPLOY))
			{
				return false;
			}
			keyController.IsRun = false;
			CommandMenu.MenuExit();
			currentMenu = MENU_NAME.DEPLOY;
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.Deploy);
			StrategyTopTaskManager.Instance.UIModel.OverView.FindChild("Deploy");
			StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenOut(null, isCharacterExit: false);
			StrategyTaskManager.setCallBack(delegate
			{
				StrategyTopTaskManager.Instance.UIModel.Character.transform.localPosition = StrategyTopTaskManager.Instance.UIModel.Character.getExitPosition();
				StrategyTopTaskManager.Instance.UIModel.Character.isEnter = false;
				StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenIn(null, isCharacterEnter: true, isSidePanelEnter: false);
				keyController.IsRun = true;
			});
			if (StrategyTopTaskManager.Instance.TutorialGuide9_1 != null)
			{
				if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().HideAndDestroy();
				}
				StrategyTopTaskManager.Instance.TutorialGuide9_1.HideAndDestroy();
			}
			sceneChange = false;
			return true;
		}

		private bool pushInfo()
		{
			if (!isInfoOpenEnable)
			{
				return true;
			}
			CommandMenu.MenuExit();
			currentMenu = MENU_NAME.INFO;
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.Info);
			keyController.IsRun = false;
			sceneChange = false;
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip() != null && SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState == MissionStates.NONE)
			{
				ShipUtils.PlayShipVoice(SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel, 8);
			}
			StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenOut(null, isCharacterExit: false);
			StrategyTopTaskManager.GetAreaInfoTask().setExitAction(delegate
			{
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState == MissionStates.NONE)
				{
					isInfoOpenEnable = false;
					StrategyTopTaskManager.Instance.UIModel.Character.moveCharacterX(StrategyTopTaskManager.Instance.UIModel.Character.getModelDefaultPosX(), 0.4f, delegate
					{
						isInfoOpenEnable = true;
					});
					StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenIn(null, isCharacterEnter: false, isSidePanelEnter: false);
				}
				else
				{
					StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenIn(delegate
					{
						isInfoOpenEnable = true;
					}, isCharacterEnter: false, isSidePanelEnter: false);
				}
			});
			return true;
		}

		private bool pushPractice()
		{
			if (!validCheck(MENU_NAME.ENSYU))
			{
				return false;
			}
			CommandMenu.MenuExit();
			currentMenu = MENU_NAME.ENSYU;
			StrategyTaskManager.setCallBack(delegate
			{
				if (mUserInterfacePracticeManager != null)
				{
					Object.Destroy(mUserInterfacePracticeManager.gameObject);
					mUserInterfacePracticeManager = null;
				}
				StrategyTopTaskManager.Instance.GetInfoMng().updateFooterInfo(isUpdateMaterial: false);
				InfoRoot.SetActive(true);
				MapRoot.SetActive(true);
				OverView.SetActive(true);
				OverScene = Scene.NONE;
				KeyControlManager.Instance.KeyController = keyController;
				StrategyTopTaskManager.Instance.UIModel.MapCamera.setBlurEnable(enable: false);
				CommandMenu.MenuEnter(4);
				keyController.Index = 4;
				StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenIn(null, isCharacterEnter: false);
				StrategyTopTaskManager.Instance.UIModel.Character.moveCharacterX(StrategyTopTaskManager.Instance.UIModel.Character.getModelDefaultPosX(), 0.4f, delegate
				{
					keyController.IsRun = true;
				});
				StrategyTopTaskManager.Instance.UIModel.MapCamera.setBlurEnable(enable: false);
				StrategyTopTaskManager.Instance.TileManager.setActivePositionAnimations(isActive: true);
				keyController.IsRun = false;
				StrategyTopTaskManager.Instance.setActiveStrategy(isActive: true);
				StrategyTopTaskManager.Instance.UIModel.Character.SetCollisionEnable(isEnable: true);
			});
			StrategyTopTaskManager.Instance.UIModel.MapCamera.setBlurEnable(enable: true);
			this.DelayActionFrame(1, delegate
			{
				StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenOut(null, isCharacterExit: false);
				StrategyTopTaskManager.Instance.UIModel.Character.moveCharacterX(StrategyTopTaskManager.Instance.UIModel.Character.getModelDefaultPosX() - 600f, 0.4f, delegate
				{
					this.DelayAction(0.1f, delegate
					{
						mUserInterfacePracticeManager = Util.Instantiate(mPrefab_UserInterfacePracticeManager.gameObject, OverView).GetComponent<UserInterfacePracticeManager>();
						OverSceneObject = GameObject.Find("UIRoot");
					});
				});
				StrategyTopTaskManager.Instance.UIModel.Character.SetCollisionEnable(isEnable: false);
			});
			OverScene = Scene.PRACTICE;
			StrategyTopTaskManager.Instance.setActiveStrategy(isActive: false);
			return true;
		}

		private bool pushExpedition()
		{
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState == MissionStates.NONE)
			{
				if (!validCheck(MENU_NAME.ENSEI))
				{
					return false;
				}
				CommandMenu.MenuExit();
				currentMenu = MENU_NAME.ENSEI;
				StrategyTaskManager.setCallBack(delegate
				{
					StrategyTopTaskManager.Instance.GetInfoMng().updateFooterInfo(isUpdateMaterial: false);
					StrategyTopTaskManager.Instance.GetInfoMng().updateUpperInfo();
					InfoRoot.SetActive(true);
					MapRoot.SetActive(true);
					OverView.SetActive(true);
					OverScene = Scene.NONE;
					KeyControlManager.Instance.KeyController = keyController;
					CommandMenu.MenuEnter(3);
					StrategyTopTaskManager.Instance.UIModel.UIMapManager.ShipIconManager.setShipIconsState();
					StrategyTopTaskManager.Instance.UIModel.Character.setState(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
					StrategyTopTaskManager.Instance.setActiveStrategy(isActive: true);
					SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
				});
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					InfoRoot.SetActive(false);
					OverView.SetActive(false);
					GameObject gameObject = Object.Instantiate(ENSEI);
					gameObject.transform.positionX(999f);
				});
				OverSceneObject = GameObject.Find("UIRoot");
				OverScene = Scene.EXPEDISION;
				StrategyTopTaskManager.Instance.setActiveStrategy(isActive: false);
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			}
			else
			{
				StopExpeditionPanel = Util.Instantiate(ENSEI_Cancel, OverView).GetComponent<StopExpedition>();
				MissionManager missionMng = new MissionManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID);
				StopExpeditionPanel.StartPanel(missionMng);
			}
			return true;
		}

		private bool pushTurnEnd()
		{
			Debug.Log("タ\u30fcンエンド");
			StrategyTopTaskManager.GetTurnEnd().TurnEnd();
			CommandMenu.MenuExit();
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.TurnEnd);
			sceneChange = false;
			if (StrategyTopTaskManager.Instance.TutorialGuide8_1 != null)
			{
				if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().HideAndDestroy();
				}
				StrategyTopTaskManager.Instance.TutorialGuide8_1.HideAndDestroy();
			}
			return true;
		}

		private bool pushPort()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			return true;
		}

		private bool validCheck(MENU_NAME menuName)
		{
			DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			List<IsGoCondition> list = new List<IsGoCondition>();
			switch (menuName)
			{
			case MENU_NAME.SALLY:
				list = currentDeck.IsValidSortie();
				break;
			case MENU_NAME.MOVE:
				list = currentDeck.IsValidMove();
				break;
			case MENU_NAME.ENSEI:
				list = currentDeck.IsValidMission();
				break;
			case MENU_NAME.ENSYU:
				list = currentDeck.IsValidPractice();
				break;
			case MENU_NAME.DEPLOY:
			{
				int num = StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID;
				if (num == 15 || num == 16 || num == 17)
				{
					CommonPopupDialog.Instance.StartPopup("この海域には配備出来ません");
					return false;
				}
				break;
			}
			}
			bool flag = list.Count == 0;
			if (!flag)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getCancelReason(list[0]));
			}
			else
			{
				bool flag2 = MENU_NAME.ENSEI == menuName;
				if (!StrategyTopTaskManager.GetLogicManager().GetMissionAreaId().Contains(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID) && flag2)
				{
					CommonPopupDialog.Instance.StartPopup("この海域の遠征任務は解放されていません");
					return false;
				}
			}
			return flag;
		}

		private void CheckSwipe(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp || SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsFocus)
			{
				swipeWait = false;
				prevMoveY = 0f;
				keyController.ClearKeyAll();
			}
			else if (!swipeWait)
			{
				if (movedPercentageY - prevMoveY > 0.15f && !SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsFocus)
				{
					IndexChange = -1;
					prevMoveY = movedPercentageY;
				}
				else if (movedPercentageY - prevMoveY < -0.15f && !SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsFocus)
				{
					IndexChange = 1;
					prevMoveY = movedPercentageY;
				}
			}
		}

		private void OnDestroy()
		{
			ENSEI = null;
			EscortOrganize = null;
			mPrefab_UserInterfacePracticeManager = null;
			mUserInterfacePracticeManager = null;
			sttm = null;
			sailSelect = null;
			CommandMenu = null;
			LogicMng = null;
			ShipNumberLabel = null;
			areaModel = null;
			warningPanel = null;
			InfoRoot = null;
			MapRoot = null;
			OverView = null;
			OverSceneObject = null;
			StopExpeditionPanel = null;
			keyController = null;
			SwipeEvent = null;
			StopExpeditionPanel = null;
		}
	}
}
