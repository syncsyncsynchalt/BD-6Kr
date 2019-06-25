using Common.Enum;
using KCV.PopupString;
using KCV.Utils;
using local.managers;
using local.models;
using System.Linq;
using UnityEngine;

namespace KCV.Arsenal
{
	public class TaskMainArsenalManager : SceneTaskMono
	{
		public enum State
		{
			NONE,
			KENZOU,
			KENZOU_BIG,
			KAIHATSU,
			KAITAI,
			HAIKI,
			YUSOUSEN
		}

		public enum Mode
		{
			MENU_FOCUS,
			DOCK_FOCUS,
			KENZOU_DIALOG,
			KAIHATSU_DIALOG,
			KAITAI_HAIKI_DIALOG,
			HIGHSPEED_DIALOG,
			DOCKOPEN_DIALOG,
			TANKER_DIALOG
		}

		private enum DialogType
		{
			Tanker
		}

		private const BGMFileInfos SCENE_BGM = BGMFileInfos.PortTools;

		public static State StateType;

		public static ArsenalManager arsenalManager;

		public static UiArsenalDock[] dockMamager;

		public static GameObject _MainObj;

		[SerializeField]
		public UiArsenalDockOpenDialog _dockOpenDialogManager;

		[SerializeField]
		public ArsenalTankerDialog _tankerDialog;

		public int DockIndex;

		public bool _isEnd;

		[SerializeField]
		private CommonDialog commonDialog;

		[SerializeField]
		private ArsenalHexMenu _hexMenu;

		[SerializeField]
		private UiArsenalSpeedDialog _speedDialogManager;

		[SerializeField]
		private GameObject _bgObj;

		[SerializeField]
		private GameObject _ConstructObj;

		[SerializeField]
		private GameObject _DismantleObj;

		[SerializeField]
		private UILabel mLabel_ListHeaderCategory;

		private GameObject Tutorial;

		private int dockSelectIndex;

		private bool isDockSelect;

		private bool _isCreate;

		private BuildDockModel[] _dock;

		private KeyControl KeyController;

		private static UICamera uiCamera;

		private static bool isControl;

		public static Mode NowMode;

		private bool mNeedRefreshShipKaitaiList = true;

		private bool mNeedRefreshSlotItemKaitaiList = true;

		private Transform ArrowAchor;

		public static bool isTouchEnable
		{
			set
			{
				uiCamera.enabled = value;
			}
		}

		public static bool IsControl
		{
			get
			{
				return isControl;
			}
			set
			{
				isControl = value;
				SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = value;
			}
		}

		public Mode CurrentMode
		{
			get;
			private set;
		}

		protected override bool Init()
		{
			if (!_isCreate)
			{
				IsControl = true;
				_isEnd = false;
				isDockSelect = false;
				CurrentMode = Mode.MENU_FOCUS;
				KeyController = ArsenalTaskManager.GetKeyControl();
				arsenalManager = ArsenalTaskManager.GetLogicManager();
				dockMamager = new UiArsenalDock[4];
				_dock = arsenalManager.GetDocks();
				_MainObj = base.scenePrefab.gameObject;
				_bgObj = base.transform.parent.parent.transform.FindChild("BackGroundPanel").gameObject;
				if (_hexMenu == null)
				{
					_hexMenu = ((Component)_bgObj.transform.FindChild("HexMenu")).GetComponent<ArsenalHexMenu>();
				}
				if (_speedDialogManager == null)
				{
					_speedDialogManager = GameObject.Find("TaskArsenalMain/HighSpeedPanel").GetComponent<UiArsenalSpeedDialog>();
				}
				if (_dockOpenDialogManager == null)
				{
					_dockOpenDialogManager = GameObject.Find("TaskArsenalMain/DockOpenDialog").GetComponent<UiArsenalDockOpenDialog>();
				}
				if (_tankerDialog == null)
				{
					_tankerDialog = commonDialog.dialogMessages[0].GetComponent<ArsenalTankerDialog>();
				}
				uiCamera = GameObject.Find("Arsenal Root/Camera").GetComponent<UICamera>();
				_hexMenu.Init();
				_speedDialogManager.init();
				_dockOpenDialogManager.init();
				int numOfKeyPossession = arsenalManager.NumOfKeyPossessions;
				for (int i = 0; i < 4; i++)
				{
					dockMamager[i] = _bgObj.transform.FindChild("Dock" + (i + 1)).SafeGetComponent<UiArsenalDock>();
					dockMamager[i].init(this, i);
					dockMamager[i].EnableParticles();
					if (_dock.Length > i)
					{
						dockMamager[i]._setShow();
						dockMamager[i].HideKeyLock();
					}
				}
				for (int j = 0; j < 4; j++)
				{
					if (!dockMamager[j].SelectDockMode())
					{
						dockMamager[j].ShowKeyLock();
						break;
					}
				}
				Animation component = _bgObj.GetComponent<Animation>();
				component.Stop();
				component.Play();
				if (SingletonMonoBehaviour<PortObjectManager>.exist())
				{
					SoundUtils.SwitchBGM(BGMFileInfos.PortTools);
					SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(delegate
					{
						isCreated();
					});
				}
				else
				{
					isCreated();
				}
			}
			else if (!isDockSelect)
			{
				_hexMenu.UpdateButtonForcus();
			}
			if (_dock.Any((BuildDockModel x) => x.Ship != null && x.CompleteTurn != 0))
			{
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(ArsenalTaskManager.GetLogicManager().UserInfo.Tutorial, TutorialGuideManager.TutorialID.SpeedBuild, null);
			}
			TutorialModel tutorial = arsenalManager.UserInfo.Tutorial;
			if (tutorial.GetStep() == 2 && !tutorial.GetStepTutorialFlg(3) && Tutorial == null)
			{
				Tutorial = Util.InstantiatePrefab("TutorialGuide/TutorialGuide3_3", base.gameObject);
				this.DelayActionFrame(2, delegate
				{
					setTutorialCat();
				});
			}
			setTutorialVisible(isVisible: true);
			IsControl = true;
			return true;
		}

		private void OnDestroy()
		{
			arsenalManager = null;
			for (int i = 0; i < dockMamager.Length; i++)
			{
				dockMamager[i] = null;
			}
			Mem.DelAry(ref _dock);
			commonDialog = null;
			Mem.DelAry(ref dockMamager);
			_hexMenu = null;
			_speedDialogManager = null;
			_dockOpenDialogManager = null;
			_tankerDialog = null;
			_MainObj = null;
			_bgObj = null;
			_ConstructObj = null;
			_DismantleObj = null;
			mLabel_ListHeaderCategory = null;
		}

		public void setTutorialVisible(bool isVisible)
		{
			if (Tutorial != null)
			{
				if (isVisible)
				{
					TweenAlpha.Begin(Tutorial, 0.5f, 1f);
				}
				else
				{
					TweenAlpha.Begin(Tutorial, 0.2f, 0f);
				}
			}
		}

		public void DestroyTutorial()
		{
			if (Tutorial != null)
			{
				Object.Destroy(Tutorial);
				Object.Destroy(ArrowAchor.gameObject);
				ArrowAchor = null;
				Tutorial = null;
			}
		}

		private void isCreated()
		{
			if (dockMamager != null)
			{
				_isCreate = true;
				UiArsenalDock[] array = dockMamager;
				foreach (UiArsenalDock uiArsenalDock in array)
				{
					uiArsenalDock.isCompleteVoicePlayable = true;
				}
			}
		}

		protected override bool UnInit()
		{
			setTutorialVisible(isVisible: false);
			return true;
		}

		public void SetNeedRefreshForShipKaitaiList(bool needRefreshKaitaiList)
		{
			mNeedRefreshShipKaitaiList = needRefreshKaitaiList;
		}

		internal void SetNeedRefreshForSlotItemKaitaiList(bool needRefreshKaitaiList)
		{
			mNeedRefreshSlotItemKaitaiList = needRefreshKaitaiList;
		}

		protected override bool Run()
		{
			if (_isEnd)
			{
				if (CurrentMode == Mode.KAITAI_HAIKI_DIALOG)
				{
					ArsenalTaskManager.ReqPhase(ArsenalTaskManager.ArsenalPhase.List);
					if (StateType == State.KAITAI)
					{
						if (mNeedRefreshShipKaitaiList)
						{
							ArsenalTaskManager._clsList.StartStateKaitaiAtFirst();
						}
						else
						{
							ArsenalTaskManager._clsList.StartStateKaitai();
						}
						mNeedRefreshShipKaitaiList = false;
					}
					else if (StateType == State.HAIKI)
					{
						if (mNeedRefreshSlotItemKaitaiList)
						{
							ArsenalTaskManager._clsList.StartStateHaikiAtFirst();
						}
						else
						{
							ArsenalTaskManager._clsList.StartStateHaiki();
						}
						mNeedRefreshSlotItemKaitaiList = false;
					}
				}
				_isEnd = false;
				return false;
			}
			if (!IsControl)
			{
				return true;
			}
			return keyControllerHandler();
		}

		private bool keyControllerHandler()
		{
			NowMode = CurrentMode;
			switch (CurrentMode)
			{
			case Mode.MENU_FOCUS:
				return keyControllerMenuFocus();
			case Mode.DOCK_FOCUS:
				return keyControllerDockFocus();
			case Mode.KENZOU_DIALOG:
				return keyControllerKenzouDialog();
			case Mode.KAIHATSU_DIALOG:
				return keyControllerKaihatsuDialog();
			case Mode.KAITAI_HAIKI_DIALOG:
				return keyControllerKaitaiHaikiDialog();
			case Mode.HIGHSPEED_DIALOG:
				return keyControllerHighspeedDialog();
			case Mode.DOCKOPEN_DIALOG:
				return keyControllerDockOpenDialog();
			default:
				return false;
			}
		}

		private bool keyControllerMenuFocus()
		{
			unFocusDock();
			if (KeyController.keyState[12].down)
			{
				_hexMenu.NextButtonForcus();
			}
			else if (KeyController.keyState[8].down)
			{
				_hexMenu.BackButtonForcus();
			}
			else
			{
				if (KeyController.keyState[10].down)
				{
					unsetHexFocus();
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					return focusDock();
				}
				if (KeyController.keyState[5].down)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
				}
				else if (KeyController.keyState[1].down)
				{
					showDialogForMenu();
				}
				else if (KeyController.keyState[0].down)
				{
					return goPortTop();
				}
			}
			return true;
		}

		private bool keyControllerDockFocus()
		{
			if (KeyController.keyState[12].down)
			{
				int num = dockMamager.Length - 1;
				if (num < 3)
				{
					num++;
				}
				if (dockSelectIndex < num && dockMamager[dockSelectIndex + 1].SelectDockMode())
				{
					dockSelectIndex++;
					updateDockSelect();
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
			else if (KeyController.keyState[8].down)
			{
				if (dockSelectIndex > 0)
				{
					dockSelectIndex--;
					updateDockSelect();
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
			else
			{
				if (KeyController.keyState[14].down)
				{
					_hexMenu.UpdateButtonForcus();
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					return unFocusDock();
				}
				if (KeyController.keyState[5].down)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
				}
				else
				{
					if (KeyController.keyState[0].down)
					{
						return goPortTop();
					}
					if (KeyController.keyState[1].down)
					{
						if (dockMamager[dockSelectIndex].GetLockDockMode())
						{
							dockMamager[dockSelectIndex].DockOpenBtnEL();
						}
						else
						{
							switch (dockMamager[dockSelectIndex].GetDockState())
							{
							case KdockStates.EMPTY:
								dockMamager[dockSelectIndex].DockFrameEL();
								break;
							case KdockStates.CREATE:
								dockMamager[dockSelectIndex].HighSpeedIconEL();
								break;
							case KdockStates.COMPLETE:
								dockMamager[dockSelectIndex].GetShipBtnEL();
								break;
							}
						}
						return true;
					}
				}
			}
			return true;
		}

		private bool keyControllerKenzouDialog()
		{
			if (KeyController.keyState[0].down)
			{
				CurrentMode = (isDockSelect ? Mode.DOCK_FOCUS : Mode.MENU_FOCUS);
				for (int i = 0; i < 4; i++)
				{
					dockMamager[i].EnableParticles();
				}
			}
			return true;
		}

		private bool keyControllerKaihatsuDialog()
		{
			if (KeyController.keyState[0].down)
			{
				CurrentMode = (isDockSelect ? Mode.DOCK_FOCUS : Mode.MENU_FOCUS);
				for (int i = 0; i < 4; i++)
				{
					dockMamager[i].EnableParticles();
				}
			}
			return false;
		}

		private bool keyControllerKaitaiHaikiDialog()
		{
			if (KeyController.keyState[0].down)
			{
				CurrentMode = (isDockSelect ? Mode.DOCK_FOCUS : Mode.MENU_FOCUS);
				for (int i = 0; i < 4; i++)
				{
					dockMamager[i].EnableParticles();
				}
			}
			return true;
		}

		private bool keyControllerHighspeedDialog()
		{
			if (KeyController.keyState[0].down)
			{
				hideHighSpeedDialog();
			}
			else if (KeyController.keyState[14].down)
			{
				_speedDialogManager.updateSpeedDialogBtn(0);
			}
			else if (KeyController.keyState[10].down)
			{
				_speedDialogManager.updateSpeedDialogBtn(1);
			}
			else if (KeyController.keyState[1].down)
			{
				StartHighSpeedProcess();
			}
			return true;
		}

		private bool keyControllerDockOpenDialog()
		{
			if (KeyController.keyState[0].down)
			{
				_dockOpenDialogManager.OnNoButtonEL(null);
			}
			else if (KeyController.keyState[14].down)
			{
				_dockOpenDialogManager.updateDialogBtn(0);
			}
			else if (KeyController.keyState[10].down)
			{
				_dockOpenDialogManager.updateDialogBtn(1);
			}
			else if (KeyController.keyState[1].down)
			{
				if (_dockOpenDialogManager.Index == 0)
				{
					_dockOpenDialogManager.OnYesButtonEL(null);
				}
				else
				{
					_dockOpenDialogManager.OnNoButtonEL(null);
				}
				hideDockOpenDialog();
			}
			return true;
		}

		public void StartHighSpeedProcess()
		{
			if (_speedDialogManager.Index == 0)
			{
				highSpeedProcess();
			}
			hideHighSpeedDialog();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		private void highSpeedProcess()
		{
			for (int i = 0; i < 4; i++)
			{
				if (dockMamager[i].IsShowHigh)
				{
					arsenalManager.ChangeHighSpeed(i + 1);
					DockIndex = i;
					dockMamager[i].StartSpeedUpAnimate();
				}
				dockMamager[i].updateSpeedUpIcon();
				dockMamager[i].setSelect(DockIndex == i);
			}
			TutorialModel tutorial = ArsenalTaskManager.GetLogicManager().UserInfo.Tutorial;
			if (tutorial.GetStep() == 2 && !tutorial.GetStepTutorialFlg(3))
			{
				tutorial.SetStepTutorialFlg(3);
				CommonPopupDialog.Instance.StartPopup("「高速建造！」 達成");
				DestroyTutorial();
				SoundUtils.PlaySE(SEFIleInfos.SE_012);
			}
		}

		private void setTutorialCat()
		{
			ArrowAchor = Tutorial.transform.FindChild("TutorialGuide/ArrowAchor");
			ArrowAchor.SetActive(isActive: true);
			int num = -1;
			for (int i = 0; i < _dock.Length; i++)
			{
				if (_dock[i].State == KdockStates.CREATE)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				ArrowAchor.SetActive(isActive: false);
				return;
			}
			TweenAlpha.Begin(ArrowAchor.gameObject, 0.2f, 1f);
			UiArsenalDock uiArsenalDock = dockMamager[num];
			ArrowAchor.parent = uiArsenalDock.transform.FindChild("ButtonHight");
			ArrowAchor.localPosition = new Vector3(-126f, 0f, 0f);
			ArrowAchor.localEulerAnglesY(180f);
		}

		private bool goPortTop()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
			return true;
		}

		public void showDialogForMenu()
		{
			if (!IsControl)
			{
				return;
			}
			if (CurrentMode == Mode.DOCK_FOCUS)
			{
				unFocusDock();
			}
			_hexMenu.AllButtonEnable(enabled: false);
			switch (StateType)
			{
			case State.KENZOU:
				if (!selectedKenzou())
				{
					_isEnd = true;
				}
				break;
			case State.KENZOU_BIG:
				if (!selectedKenzou())
				{
					_isEnd = true;
				}
				break;
			case State.KAIHATSU:
				if (!selectedKaihatsu())
				{
					_isEnd = true;
				}
				break;
			case State.KAITAI:
				mLabel_ListHeaderCategory.text = "解体  艦種  艦名\u3000\u3000\u3000\u3000\u3000      Lv";
				if (!selectedKaitaiHaiki())
				{
					_isEnd = true;
				}
				break;
			case State.HAIKI:
				mLabel_ListHeaderCategory.text = "廃棄      装備名\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000 レア";
				if (!selectedKaitaiHaiki())
				{
					_isEnd = true;
				}
				break;
			case State.YUSOUSEN:
				if (!selectedKenzou())
				{
					_isEnd = true;
				}
				break;
			}
			if (!_isEnd)
			{
				_hexMenu.AllButtonEnable(enabled: true);
			}
			else
			{
				IsControl = false;
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		public void unsetHexFocus()
		{
			_hexMenu.unsetFocus();
		}

		public bool focusDock()
		{
			CurrentMode = Mode.DOCK_FOCUS;
			for (int i = 0; i < 4; i++)
			{
				dockMamager[i].EnableParticles();
			}
			isDockSelect = true;
			dockSelectIndex = 0;
			updateDockSelect();
			return true;
		}

		private bool unFocusDock()
		{
			CurrentMode = Mode.MENU_FOCUS;
			for (int i = 0; i < 4; i++)
			{
				dockMamager[i].EnableParticles();
			}
			isDockSelect = false;
			dockSelectIndex = -1;
			updateDockSelect();
			return true;
		}

		public void selectDock(int index)
		{
			dockSelectIndex = index;
			updateDockSelect();
		}

		public bool hideDialog()
		{
			CurrentMode = (isDockSelect ? Mode.DOCK_FOCUS : Mode.MENU_FOCUS);
			_hexMenu.AllButtonEnable(enabled: true);
			for (int i = 0; i < 4; i++)
			{
				dockMamager[i].EnableParticles();
			}
			return true;
		}

		public bool selectedKenzou()
		{
			bool flag = false;
			for (int i = 0; i < 4; i++)
			{
				if (dockMamager[i].CheckStateEmpty())
				{
					DockIndex = i;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				CurrentMode = Mode.KENZOU_DIALOG;
				for (int j = 0; j < 4; j++)
				{
					dockMamager[j].DisableParticles();
				}
				dockMamager[DockIndex].setConstruct();
				return false;
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.CannotArsenalByFullDeck));
			return true;
		}

		public bool selectedKaihatsu()
		{
			CurrentMode = Mode.KAIHATSU_DIALOG;
			for (int i = 0; i < 4; i++)
			{
				dockMamager[i].DisableParticles();
			}
			dockMamager[DockIndex].setConstruct();
			return false;
		}

		public bool selectedKaitaiHaiki()
		{
			CurrentMode = Mode.KAITAI_HAIKI_DIALOG;
			for (int i = 0; i < 4; i++)
			{
				dockMamager[i].DisableParticles();
			}
			return false;
		}

		public void showHighSpeedDialog(int dockNum)
		{
			CurrentMode = Mode.HIGHSPEED_DIALOG;
			for (int i = 0; i < 4; i++)
			{
				dockMamager[i].DisableParticles();
			}
			_speedDialogManager.showHighSpeedDialog(dockNum);
			ArsenalTaskManager._clsArsenal.setTutorialVisible(isVisible: false);
		}

		public void hideHighSpeedDialog()
		{
			CurrentMode = (isDockSelect ? Mode.DOCK_FOCUS : Mode.MENU_FOCUS);
			for (int i = 0; i < 4; i++)
			{
				dockMamager[i].EnableParticles();
			}
			_speedDialogManager.hideHighSpeedDialog();
			for (int j = 0; j < 4; j++)
			{
				if (dockMamager[j].IsShowHigh)
				{
					dockMamager[j].updateSpeedUpIcon();
					dockMamager[j].IsShowHigh = false;
					break;
				}
				dockMamager[j].setSelect(DockIndex == j);
			}
			ArsenalTaskManager._clsArsenal.setTutorialVisible(isVisible: true);
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
		}

		public void showTankerDialog(int CreateNum, int beforeNum, int afterNum)
		{
			_tankerDialog.setMessage(CreateNum, beforeNum, afterNum);
			commonDialog.OpenDialog(0);
			commonDialog.setCloseAction(delegate
			{
				ArsenalTaskManager._clsArsenal.setTutorialVisible(isVisible: true);
			});
		}

		public void hideTankerDialog()
		{
			commonDialog.CloseDialog();
		}

		public void showDockOpenDialog()
		{
			ArsenalTaskManager._clsArsenal.setTutorialVisible(isVisible: false);
			CurrentMode = Mode.DOCKOPEN_DIALOG;
			for (int i = 0; i < 4; i++)
			{
				dockMamager[i].DisableParticles();
			}
		}

		public void hideDockOpenDialog()
		{
			ArsenalTaskManager._clsArsenal.setTutorialVisible(isVisible: true);
			CurrentMode = (isDockSelect ? Mode.DOCK_FOCUS : Mode.MENU_FOCUS);
			for (int i = 0; i < 4; i++)
			{
				dockMamager[i].EnableParticles();
			}
		}

		public void SetDock()
		{
			dockMamager[DockIndex]._setShow();
		}

		public bool IsShipGetViewAllDock()
		{
			for (int i = 0; i < 4; i++)
			{
				if (dockMamager[i] != null && dockMamager[i].IsShipGetView())
				{
					return true;
				}
			}
			return false;
		}

		private void updateDockSelect()
		{
			for (int i = 0; i < dockMamager.Length; i++)
			{
				if (dockMamager[i].SelectDockMode())
				{
					if (dockSelectIndex == i)
					{
						dockMamager[i].setSelect(select: true);
					}
					else
					{
						dockMamager[i].setSelect(select: false);
					}
				}
			}
		}

		public void OnClickMenuKenzou()
		{
			StateType = State.KENZOU;
			showDialogForMenu();
		}

		public void OnClickMenuKenzouBig()
		{
			StateType = State.KENZOU_BIG;
			selectedKenzou();
		}

		public void OnClickMenuKaihatsu()
		{
			StateType = State.KAIHATSU;
			showDialogForMenu();
		}

		public void OnClickMenuKaitai()
		{
			StateType = State.KAITAI;
			selectedKaitaiHaiki();
		}

		public void OnClickMenuHaiki()
		{
			StateType = State.HAIKI;
			selectedKaitaiHaiki();
		}

		public bool checkDialogOpen()
		{
			bool result = CurrentMode != 0 && CurrentMode != Mode.DOCK_FOCUS;
			if (!IsControl)
			{
				result = false;
			}
			return result;
		}

		public bool isInConstructDialog()
		{
			if (checkDialogOpen())
			{
				return false;
			}
			Vector3 localPosition = _ConstructObj.transform.localPosition;
			if (!(localPosition.x < -60f))
			{
				Vector3 localPosition2 = _ConstructObj.transform.localPosition;
				if (!(localPosition2.x > 0f))
				{
					return false;
				}
			}
			return true;
		}
	}
}
