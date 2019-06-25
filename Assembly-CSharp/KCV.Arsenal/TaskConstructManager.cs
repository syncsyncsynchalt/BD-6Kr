using Common.Enum;
using KCV.PopupString;
using KCV.Production;
using KCV.Strategy;
using KCV.Utils;
using local.models;
using Server_Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Arsenal
{
	public class TaskConstructManager : SceneTaskMono
	{
		private enum PanelType
		{
			Normal,
			Big,
			Tanker,
			SlotItem
		}

		private enum PanelFocus
		{
			Material,
			MaterialCountPanel,
			HighSpeed,
			Devkit,
			Deside,
			Tanker,
			BigShip
		}

		private enum BtnName
		{
			Fuel = 1,
			Steel,
			DevKit,
			Ammo,
			Baux,
			TankerButton,
			BigShipButton,
			HighSpeed,
			Deside
		}

		public BaseDialogPopup dialogPopUp;

		private ProdReceiveSlotItem _prodReceiveItem;

		private UiSpeedIconManager _speedIconManager;

		private BuildDockModel[] dock;

		private ShipModel[] ships;

		[SerializeField]
		private UIPanel _uiPanel;

		[SerializeField]
		private GameObject[] _uiMaterialsObj;

		[SerializeField]
		private GameObject[] _uiMaterialFrameObj;

		[SerializeField]
		private UITexture[] _uiMaterialOnFrame;

		[SerializeField]
		public UISprite[] _uiMaterialIcon;

		[SerializeField]
		private UiArsenalMaterialDialog _uiDialog;

		[SerializeField]
		private Arsenal_DevKit _devKitManager;

		[SerializeField]
		private Arsenal_SPoint _sPointManager;

		[SerializeField]
		private UISprite _uiBtnTanker;

		[SerializeField]
		private UISprite _uiBtnBig;

		[SerializeField]
		private GameObject _uiBtnBigObj;

		[SerializeField]
		private UISprite _uiBtnStart;

		private bool isConstructStart;

		private KeyControl KeyController;

		private bool isControl;

		private bool isCreate;

		private bool _isCreateView;

		private int _controllIndex;

		private bool _changeOnce;

		private ButtonLightTexture _uiBtnLightTexture;

		private PanelType nowPanelType;

		private PanelFocus nowPanelState;

		private int dockIndex;

		private readonly int[] changeIndexMaterial = new int[10]
		{
			-1,
			0,
			2,
			-1,
			1,
			3,
			-1,
			-1,
			-1,
			-1
		};

		private int[] materialCnt;

		public int[] materialMax;

		public int[] materialMin;

		public int[,] materialPreset;

		[SerializeField]
		private UiArsenalConstBG _BG_touch1;

		[SerializeField]
		private UiArsenalParamBG _BG_touch2;

		private bool isEnd;

		private Color _Orange = new Color(1f, 0.6f, 0f);

		private int[] _material = new int[5];

		private Vector3 KaihatsuPanelPosition = new Vector3(-70f, 0f, 0f);

		[SerializeField]
		private StrategyShipCharacter Live2DRender;

		[SerializeField]
		private GameObject _uiBtnLight;

		private bool isFirst;

		private readonly int[,] normalIndexMap = new int[10, 8]
		{
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			},
			{
				0,
				0,
				2,
				0,
				4,
				0,
				0,
				0
			},
			{
				0,
				0,
				6,
				0,
				5,
				0,
				1,
				0
			},
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			},
			{
				1,
				0,
				5,
				0,
				0,
				0,
				0,
				0
			},
			{
				2,
				0,
				7,
				0,
				0,
				0,
				4,
				0
			},
			{
				0,
				0,
				0,
				0,
				7,
				0,
				2,
				0
			},
			{
				6,
				0,
				0,
				0,
				8,
				0,
				5,
				0
			},
			{
				7,
				0,
				0,
				0,
				9,
				0,
				5,
				0
			},
			{
				8,
				0,
				0,
				0,
				0,
				0,
				5,
				0
			}
		};

		private readonly int[,] normalIndexMapDisableBig = new int[10, 8]
		{
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			},
			{
				0,
				0,
				2,
				0,
				4,
				0,
				0,
				0
			},
			{
				0,
				0,
				6,
				0,
				5,
				0,
				1,
				0
			},
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			},
			{
				1,
				0,
				5,
				0,
				0,
				0,
				0,
				0
			},
			{
				2,
				0,
				8,
				0,
				0,
				0,
				4,
				0
			},
			{
				0,
				0,
				0,
				0,
				8,
				0,
				2,
				0
			},
			{
				6,
				0,
				0,
				0,
				8,
				0,
				5,
				0
			},
			{
				6,
				0,
				0,
				0,
				9,
				0,
				5,
				0
			},
			{
				8,
				0,
				0,
				0,
				0,
				0,
				5,
				0
			}
		};

		private readonly int[,] BigIndexMap = new int[10, 8]
		{
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			},
			{
				0,
				0,
				2,
				0,
				4,
				0,
				0,
				0
			},
			{
				0,
				0,
				3,
				0,
				5,
				0,
				1,
				0
			},
			{
				0,
				0,
				0,
				0,
				8,
				0,
				2,
				0
			},
			{
				1,
				0,
				5,
				0,
				0,
				0,
				0,
				0
			},
			{
				2,
				0,
				8,
				0,
				0,
				0,
				4,
				0
			},
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			},
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			},
			{
				3,
				0,
				0,
				0,
				9,
				0,
				5,
				0
			},
			{
				8,
				0,
				0,
				0,
				0,
				0,
				5,
				0
			}
		};

		private readonly int[,] KaihatsuIndexMap = new int[10, 8]
		{
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			},
			{
				0,
				0,
				2,
				0,
				4,
				0,
				0,
				0
			},
			{
				0,
				0,
				9,
				0,
				5,
				0,
				1,
				0
			},
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			},
			{
				1,
				0,
				5,
				0,
				0,
				0,
				0,
				0
			},
			{
				2,
				0,
				9,
				0,
				0,
				0,
				4,
				0
			},
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			},
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			},
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			},
			{
				0,
				0,
				0,
				0,
				0,
				0,
				5,
				0
			}
		};

		private readonly int[,] TankerIndexMap = new int[10, 8]
		{
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			},
			{
				0,
				0,
				2,
				0,
				4,
				0,
				0,
				0
			},
			{
				0,
				0,
				3,
				0,
				5,
				0,
				1,
				0
			},
			{
				0,
				0,
				0,
				0,
				8,
				0,
				2,
				0
			},
			{
				1,
				0,
				5,
				0,
				0,
				0,
				0,
				0
			},
			{
				2,
				0,
				8,
				0,
				0,
				0,
				4,
				0
			},
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			},
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			},
			{
				3,
				0,
				0,
				0,
				9,
				0,
				5,
				0
			},
			{
				8,
				0,
				0,
				0,
				0,
				0,
				5,
				0
			}
		};

		private bool isBigConstruct => nowPanelType == PanelType.Big;

		private bool isMaterialCountMode => nowPanelState == PanelFocus.MaterialCountPanel;

		private bool isSlotItem => nowPanelType == PanelType.SlotItem;

		public int MaterialIndex => changeIndexMaterial[KeyController.Index];

		private new void Start()
		{
			nowPanelState = PanelFocus.Material;
			nowPanelType = PanelType.Normal;
		}

		protected override bool Init()
		{
			Show(TaskMainArsenalManager.StateType, ArsenalTaskManager._clsArsenal.DockIndex);
			if (TaskMainArsenalManager.StateType == TaskMainArsenalManager.State.KAIHATSU)
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
			}
			SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(ArsenalTaskManager.GetLogicManager().UserInfo.Tutorial, TutorialGuideManager.TutorialID.BuildShip, null);
			return true;
		}

		public void firstInit()
		{
			KeyController = new KeyControl(1, 9);
			KeyController.Index = 1;
			KeyController.HoldJudgeTime = 0.2f;
			isConstructStart = false;
			_controllIndex = 1;
			dialogPopUp = ArsenalTaskManager.GetDialogPopUp();
			_uiPanel = GameObject.Find("ConstructPanel").GetComponent<UIPanel>();
			_uiBtnTanker = ((Component)_uiPanel.transform.FindChild("RightPane/BtnTanker")).GetComponent<UISprite>();
			_uiBtnBig = ((Component)_uiPanel.transform.FindChild("RightPane/BtnBig")).GetComponent<UISprite>();
			_uiBtnBigObj = _uiBtnBig.transform.FindChild("FlameObj").gameObject;
			_uiBtnStart = ((Component)_uiPanel.transform.FindChild("RightPane/BtnStart")).GetComponent<UISprite>();
			_uiDialog = ((Component)_uiPanel.transform.FindChild("Dialog")).GetComponent<UiArsenalMaterialDialog>();
			_uiDialog.init(0);
			_speedIconManager = _uiPanel.transform.FindChild("RightPane/HighSpeed").SafeGetComponent<UiSpeedIconManager>();
			_speedIconManager.init();
			_speedIconManager.SetOff();
			_sPointManager = _uiPanel.transform.FindChild("RightPane/SPoint").SafeGetComponent<Arsenal_SPoint>();
			_devKitManager = _uiPanel.transform.FindChild("RightPane/Devkit").SafeGetComponent<Arsenal_DevKit>();
			_uiMaterialsObj = new GameObject[5];
			materialCnt = new int[5];
			materialMin = new int[5];
			materialMax = new int[5];
			materialPreset = new int[5, 5];
			for (int i = 0; i < 5; i++)
			{
				materialCnt[i] = 0;
			}
			for (int j = 0; j < 5; j++)
			{
				for (int k = 0; k < 5; k++)
				{
					materialPreset[k, j] = -1;
				}
			}
			_uiMaterialsObj = new GameObject[4];
			_uiMaterialFrameObj = new GameObject[4];
			_uiMaterialOnFrame = new UITexture[4];
			_uiMaterialIcon = new UISprite[4];
			for (int l = 0; l < 4; l++)
			{
				_uiMaterialsObj[l] = _uiPanel.transform.FindChild("Material" + (l + 1)).gameObject;
				_uiMaterialFrameObj[l] = _uiMaterialsObj[l].transform.FindChild("MaterialFrame1").gameObject;
				_uiMaterialOnFrame[l] = ((Component)_uiMaterialsObj[l].transform.FindChild("FrameOn")).GetComponent<UITexture>();
				_uiMaterialIcon[l] = ((Component)_uiMaterialsObj[l].transform.FindChild("Icon")).GetComponent<UISprite>();
			}
			_uiBtnLightTexture = _uiBtnLight.GetComponent<ButtonLightTexture>();
			_uiBtnLightTexture.StopAnim();
			isFirst = true;
			_BG_touch1.set_touch(value: false);
			_BG_touch2.set_touch(value: false);
			_BG_touch2.isEnable = false;
			UIButtonMessage component = _uiBtnBig.GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = "_bigButtonEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component2 = _uiBtnTanker.GetComponent<UIButtonMessage>();
			component2.target = base.gameObject;
			component2.functionName = "_tankerButtonEL";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			_speedIconManager.SetOff();
			Live2DRender.ChangeCharacter(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip());
			Transform transform = Live2DRender.transform;
			Vector3 enterPosition = Live2DRender.getEnterPosition();
			transform.localPositionX(enterPosition.x);
			if (SingletonMonoBehaviour<Live2DModel>.exist())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			}
			isCreate = true;
		}

		protected override bool UnInit()
		{
			if (SingletonMonoBehaviour<Live2DModel>.exist())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			}
			return true;
		}

		private void OnDestroy()
		{
			dialogPopUp = null;
			_prodReceiveItem = null;
			_speedIconManager = null;
			Mem.DelAry(ref dock);
			Mem.DelAry(ref ships);
			_uiPanel = null;
			Mem.DelAry(ref _uiMaterialsObj);
			Mem.DelAry(ref _uiMaterialFrameObj);
			Mem.DelAry(ref _uiMaterialOnFrame);
			Mem.DelAry(ref _uiMaterialIcon);
			_uiDialog = null;
			_devKitManager = null;
			_sPointManager = null;
			_uiBtnTanker = null;
			_uiBtnBig = null;
			_uiBtnBigObj = null;
			_uiBtnStart = null;
			KeyController = null;
			Mem.DelAry(ref materialCnt);
			Mem.DelAry(ref materialMax);
			Mem.DelAry(ref materialMin);
			Mem.DelAry(ref materialPreset);
			_BG_touch1 = null;
			_BG_touch2 = null;
			Mem.DelAry(ref _material);
		}

		protected override bool Run()
		{
			if (isEnd)
			{
				isEnd = false;
				return false;
			}
			if (!isControl)
			{
				return true;
			}
			KeyController.Update();
			if (_isCreateView)
			{
				return true;
			}
			if (isFirst)
			{
				LightButton();
				isFirst = false;
			}
			if (KeyController.IsChangeIndex)
			{
				UpdateSelectBtn();
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			else
			{
				if (_BG_touch1.get_touch())
				{
					isFirst = true;
					_closeConstructPanel();
					return false;
				}
				if (KeyController.keyState[5].down)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
				}
				else if (!isSlotItem && KeyController.IsShikakuDown())
				{
					_speedIconManager.SpeedIconEL(null);
				}
			}
			switch (nowPanelState)
			{
			case PanelFocus.Material:
				return KeyControlMaterialFocus();
			case PanelFocus.MaterialCountPanel:
				return KeyControlMaterialCount();
			case PanelFocus.HighSpeed:
				return KeyControlHighSpeed();
			case PanelFocus.Devkit:
				return KeyControlDevkit();
			case PanelFocus.Deside:
				return KeyControlDeside();
			case PanelFocus.Tanker:
				return KeyControlTanker();
			case PanelFocus.BigShip:
				return KeyControlBig();
			default:
				return true;
			}
		}

		private void LightButton()
		{
			if (nowPanelType == PanelType.Tanker)
			{
				if (!_sPointManager.SPointStarted())
				{
					return;
				}
				if (TaskMainArsenalManager.arsenalManager.IsValidCreateTanker(dockIndex + 1, _speedIconManager.IsHigh, materialCnt[0], materialCnt[1], materialCnt[2], materialCnt[3], _sPointManager.GetUseSpointNum()))
				{
					if (!_uiBtnLightTexture.NowPlay())
					{
						_uiBtnLightTexture.PlayAnim();
					}
				}
				else if (_uiBtnLightTexture.NowPlay())
				{
					_uiBtnLightTexture.StopAnim();
				}
			}
			else if (nowPanelType == PanelType.SlotItem)
			{
				if (materialCnt == null)
				{
					return;
				}
				if (TaskMainArsenalManager.arsenalManager.IsValidCreateItem(materialCnt[0], materialCnt[1], materialCnt[2], materialCnt[3]))
				{
					if (!_uiBtnLightTexture.NowPlay())
					{
						_uiBtnLightTexture.PlayAnim();
					}
				}
				else if (_uiBtnLightTexture.NowPlay())
				{
					_uiBtnLightTexture.StopAnim();
				}
			}
			else
			{
				if (materialCnt == null || false || _speedIconManager == null)
				{
					return;
				}
				if (TaskMainArsenalManager.arsenalManager.IsValidCreateShip(dockIndex + 1, _speedIconManager.IsHigh, materialCnt[0], materialCnt[1], materialCnt[2], materialCnt[3], materialCnt[4], SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID))
				{
					if (!_uiBtnLightTexture.NowPlay())
					{
						_uiBtnLightTexture.PlayAnim();
					}
				}
				else if (_uiBtnLightTexture.NowPlay())
				{
					_uiBtnLightTexture.StopAnim();
				}
			}
		}

		private bool KeyControlMaterialFocus()
		{
			if (KeyController.IsBatuDown())
			{
				_closeConstructPanel();
				return false;
			}
			if (KeyController.IsMaruDown() && isFocusMaterial())
			{
				_selectMaterialFrame();
			}
			return true;
		}

		private bool KeyControlMaterialCount()
		{
			if (KeyController.IsDownDown())
			{
				changeMaterialCnt(isDown: true);
			}
			else if (KeyController.IsUpDown())
			{
				changeMaterialCnt(isDown: false);
			}
			else if (KeyController.IsLDown())
			{
				changeMaterialCntMinMax(isMinButton: false);
			}
			else if (KeyController.IsRDown())
			{
				changeMaterialCntMinMax(isMinButton: true);
			}
			else if (KeyController.IsRightDown())
			{
				_uiDialog.SetFrameIndex(isLeft: true, isBigConstruct);
			}
			else if (KeyController.IsLeftDown())
			{
				_uiDialog.SetFrameIndex(isLeft: false, isBigConstruct);
			}
			else if (KeyController.IsBatuDown() || KeyController.IsMaruDown() || _BG_touch2.get_touch())
			{
				if (nowPanelState == PanelFocus.Material)
				{
					return true;
				}
				_BG_touch2.set_touch(value: false);
				UpdateMaterialCount(MaterialIndex);
				nowPanelState = PanelFocus.Material;
				_uiDialog._frameIndex = 0;
				_uiDialog.HidelDialog();
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				KeyController.isStopIndex = false;
				KeyController.firstUpdate = true;
				isFirst = true;
				LightButton();
			}
			return true;
		}

		private bool KeyControlHighSpeed()
		{
			if (KeyController.IsBatuDown())
			{
				_closeConstructPanel();
				return false;
			}
			if (KeyController.IsMaruDown())
			{
				_speedIconManager.SpeedIconEL(null);
			}
			return true;
		}

		private bool KeyControlDevkit()
		{
			if (KeyController.IsBatuDown())
			{
				_closeConstructPanel();
				return false;
			}
			if (KeyController.IsMaruDown())
			{
				if (nowPanelType == PanelType.Tanker)
				{
					_sPointManager.NextSwitch();
				}
				else
				{
					moveDevkitSwitch();
				}
				LightButton();
				UpdateSelectBtn();
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			LightButton();
			return true;
		}

		private bool KeyControlDeside()
		{
			if (KeyController.IsBatuDown())
			{
				_closeConstructPanel();
				return false;
			}
			if (KeyController.IsMaruDown())
			{
				startConstructEL(null);
			}
			return true;
		}

		private bool KeyControlTanker()
		{
			if (KeyController.IsBatuDown())
			{
				_closeConstructPanel();
				return false;
			}
			if (KeyController.IsMaruDown())
			{
				_tankerButtonEL(null);
			}
			return true;
		}

		private bool KeyControlBig()
		{
			if (KeyController.IsBatuDown())
			{
				_closeConstructPanel();
				return false;
			}
			if (KeyController.IsMaruDown())
			{
				_bigButtonEL(null);
			}
			return true;
		}

		private void _closeConstructPanel()
		{
			_BG_touch1.set_touch(value: false);
			isFirst = true;
			Hide();
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			ArsenalTaskManager.ReqPhase(ArsenalTaskManager.ArsenalPhase.BattlePhase_ST);
			ArsenalTaskManager._clsArsenal.hideDialog();
		}

		private void _changeConstructPanel(PanelType type)
		{
			nowPanelType = type;
			_changeHidePanel();
			LightButton();
		}

		private void _bigButtonEL(GameObject obj)
		{
			if (TaskMainArsenalManager.arsenalManager.LargeEnabled)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				_changeConstructPanel(PanelType.Big);
			}
		}

		private void _tankerButtonEL(GameObject obj)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			_changeConstructPanel(PanelType.Tanker);
		}

		public void Show(TaskMainArsenalManager.State state, int num)
		{
			_uiPanel.alpha = 1f;
			_BG_touch1.transform.localScale = Vector3.one;
			_BG_touch2.transform.localScale = Vector3.one;
			_speedIconManager.SetOff();
			_speedIconManager.SetBuildKitValue();
			_uiDialog._frameIndex = 0;
			dockIndex = num;
			isControl = true;
			_isCreateView = false;
			setUpConstructPanelbyType(state);
			setIndexMap();
			KeyController.firstUpdate = true;
			KeyController.Index = 1;
			_controllIndex = 1;
			for (int i = 0; i < 4; i++)
			{
				_uiMaterialFrameObj[i].SetActive(isBigConstruct);
			}
			UITexture component = ((Component)_uiPanel.transform.FindChild("BgBig2_1")).GetComponent<UITexture>();
			UITexture component2 = ((Component)_uiPanel.transform.FindChild("BgBig2_2")).GetComponent<UITexture>();
			component.SetActive((nowPanelType == PanelType.Big) ? true : false);
			component2.SetActive((nowPanelType == PanelType.Big) ? true : false);
			UpdateSelectBtn();
			UpdateMaterialFrame();
			UISelectedObject.SelectedOneButtonZoomUpDown(_uiBtnStart.gameObject, value: false);
			for (int j = 0; j < 4; j++)
			{
				UpdateMaterialCount(j);
			}
			UpdateSelectBtn();
			Vector3 pos;
			if (state == TaskMainArsenalManager.State.KAIHATSU)
			{
				pos = KaihatsuPanelPosition;
				_uiBtnStart.transform.localPositionX(414f);
				Live2DRender.SetActive(isActive: true);
			}
			else
			{
				pos = Vector3.zero;
				_uiBtnStart.transform.localPositionX(380f);
				Live2DRender.SetActive(isActive: false);
			}
			TweenPosition tweenPosition = TweenPosition.Begin(_uiPanel.gameObject, 0.3f, pos);
			tweenPosition.animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			LightButton();
		}

		public void Hide()
		{
			isControl = false;
			_BG_touch1.transform.localScale = Vector3.zero;
			_BG_touch2.transform.localScale = Vector3.zero;
			_speedIconManager.StopSleepParticle();
			for (int i = 0; i < 4; i++)
			{
				TaskMainArsenalManager.dockMamager[i].EnableParticles();
			}
			TweenPosition tweenPosition = TweenPosition.Begin(_uiPanel.gameObject, 0.3f, Vector3.right * 877f);
			tweenPosition.eventReceiver = base.gameObject;
			tweenPosition.callWhenFinished = "compHide";
			tweenPosition.animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			if (_speedIconManager.IsHigh)
			{
				StartCoroutine(SetOff());
			}
		}

		private void compHide()
		{
			_uiPanel.alpha = 0f;
		}

		private void setUpConstructPanelbyType(TaskMainArsenalManager.State state)
		{
			initMaterialCount(state);
			UILabel component = ((Component)_uiPanel.transform.FindChild("Header/NameLabel")).GetComponent<UILabel>();
			_devKitManager.SetActive(isActive: false);
			_sPointManager.SetActive(isActive: false);
			_speedIconManager.SetActive(isActive: false);
			switch (state)
			{
			case TaskMainArsenalManager.State.KENZOU:
				component.text = "建造";
				TaskMainArsenalManager.arsenalManager.LargeState = false;
				_uiBtnStart.spriteName = "btn_start";
				nowPanelType = PanelType.Normal;
				_speedIconManager.SetActive(isActive: true);
				break;
			case TaskMainArsenalManager.State.KAIHATSU:
				component.text = "開発";
				TaskMainArsenalManager.arsenalManager.LargeState = false;
				nowPanelType = PanelType.SlotItem;
				_uiBtnStart.spriteName = "btn_start2";
				break;
			case TaskMainArsenalManager.State.KENZOU_BIG:
				component.text = "大型建造";
				TaskMainArsenalManager.arsenalManager.LargeState = true;
				_uiBtnStart.spriteName = "btn_start";
				_speedIconManager.SetActive(isActive: true);
				_devKitManager.SetActive(isActive: true);
				_devKitManager.Init();
				nowPanelType = PanelType.Big;
				break;
			case TaskMainArsenalManager.State.YUSOUSEN:
				component.text = "輸送船建造";
				TaskMainArsenalManager.arsenalManager.LargeState = false;
				_uiBtnStart.spriteName = "btn_start";
				_speedIconManager.SetActive(isActive: true);
				_sPointManager.SetActive(isActive: true);
				_sPointManager.Init();
				nowPanelType = PanelType.Tanker;
				break;
			}
		}

		private void _changeShow()
		{
			if (!_changeOnce)
			{
				TaskMainArsenalManager.State state = TaskMainArsenalManager.State.NONE;
				switch (nowPanelType)
				{
				case PanelType.Normal:
					state = TaskMainArsenalManager.State.KENZOU;
					break;
				case PanelType.SlotItem:
					state = TaskMainArsenalManager.State.KAIHATSU;
					break;
				case PanelType.Tanker:
					state = TaskMainArsenalManager.State.YUSOUSEN;
					break;
				case PanelType.Big:
					state = TaskMainArsenalManager.State.KENZOU_BIG;
					break;
				}
				_changeOnce = true;
				Show(state, dockIndex);
			}
		}

		private IEnumerator SetOff()
		{
			yield return new WaitForSeconds(0.001f);
			_speedIconManager.SpeedIconEL(null);
		}

		private void _changeHidePanel()
		{
			isControl = false;
			_changeOnce = false;
			_speedIconManager.StopSleepParticle();
			TweenPosition tweenPosition = TweenPosition.Begin(_uiPanel.gameObject, 0.3f, Vector3.right * 877f);
			tweenPosition.animationCurve = UtilCurves.TweenEaseOutExpo;
			tweenPosition.SetOnFinished(_changeShow);
		}

		private bool isFocusMaterial()
		{
			return MaterialIndex != -1;
		}

		public void UpdateSelectBtn()
		{
			UnsetSelectBtn();
			if (isFocusMaterial())
			{
				FocusMaterialButton();
			}
			updateStartBtn();
			_controllIndex = KeyController.Index;
			if (KeyController.Index == 6)
			{
				_uiBtnTanker.spriteName = "btn_yuso_on";
				nowPanelState = PanelFocus.Tanker;
			}
			else if (KeyController.Index == 7)
			{
				_uiBtnBig.spriteName = "btn_big_on";
				nowPanelState = PanelFocus.BigShip;
			}
			else if (KeyController.Index == 3)
			{
				if (nowPanelType == PanelType.Tanker)
				{
					_sPointManager.SetSelecter(show: true);
				}
				else
				{
					_devKitManager.SetSelecter(show: true);
				}
				nowPanelState = PanelFocus.Devkit;
			}
			else if (KeyController.Index == 8)
			{
				_speedIconManager.SetSelect(isSet: true);
				nowPanelState = PanelFocus.HighSpeed;
			}
			else if (KeyController.Index == 9)
			{
				_uiBtnStart.spriteName = ((!isSlotItem) ? "btn_start_on" : "btn_start2_on");
				UISelectedObject.SelectedOneButtonZoomUpDown(_uiBtnStart.gameObject, value: true);
				nowPanelState = PanelFocus.Deside;
			}
			else
			{
				nowPanelState = PanelFocus.Material;
			}
		}

		private void FocusMaterialButton()
		{
			_uiMaterialOnFrame[MaterialIndex].alpha = 1f;
		}

		public void UnsetSelectBtn()
		{
			for (int i = 0; i < 4; i++)
			{
				_uiMaterialOnFrame[i].alpha = 0f;
			}
			_speedIconManager.SetSelect(isSet: false);
			if (nowPanelType == PanelType.Normal)
			{
				_uiBtnTanker.SetActive(isActive: true);
				_uiBtnBig.SetActive(isActive: true);
				_uiBtnBigObj.SetActive((!TaskMainArsenalManager.arsenalManager.LargeEnabled) ? true : false);
			}
			else
			{
				_uiBtnTanker.SetActive(isActive: false);
				_uiBtnBig.SetActive(isActive: false);
			}
			if (nowPanelType == PanelType.Tanker)
			{
				_sPointManager.SetSelecter(show: false);
			}
			if (nowPanelType == PanelType.Big)
			{
				_devKitManager.SetSelecter(show: false);
			}
			_uiBtnTanker.spriteName = "btn_yuso";
			_uiBtnBig.spriteName = "btn_big";
			_uiBtnStart.spriteName = ((!isSlotItem) ? "btn_start" : "btn_start2");
			UISelectedObject.SelectedOneButtonZoomUpDown(_uiBtnStart.gameObject, value: false);
		}

		public void updateStartBtn()
		{
			isConstructStart = true;
			if (isSlotItem)
			{
				if (!TaskMainArsenalManager.arsenalManager.IsValidCreateItem(materialCnt[0], materialCnt[1], materialCnt[2], materialCnt[3]))
				{
					isConstructStart = false;
				}
			}
			else if (nowPanelType == PanelType.Tanker)
			{
				if (!TaskMainArsenalManager.arsenalManager.IsValidCreateTanker(dockIndex + 1, _speedIconManager.IsHigh, materialCnt[0], materialCnt[1], materialCnt[2], materialCnt[3], _sPointManager.GetUseSpointNum()))
				{
					isConstructStart = false;
				}
			}
			else
			{
				materialCnt[4] = ((nowPanelType == PanelType.Normal) ? 1 : _devKitManager.getUseDevKitNum());
				if (!TaskMainArsenalManager.arsenalManager.IsValidCreateShip(dockIndex + 1, _speedIconManager.IsHigh, materialCnt[0], materialCnt[1], materialCnt[2], materialCnt[3], materialCnt[4], SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID))
				{
					isConstructStart = false;
				}
			}
			if (isConstructStart)
			{
				_uiBtnStart.spriteName = ((!isSlotItem) ? "btn_start" : "btn_start2");
				UISelectedObject.SelectedOneButtonZoomUpDown(_uiBtnStart.gameObject, value: false);
			}
			LightButton();
		}

		public void UpdateMaterialFrame()
		{
			_uiDialog.ActiveMaterialFrame(isBigConstruct);
			UpdateDialogMaterialCount();
		}

		public void changeMaterialCntEL(GameObject obj)
		{
			bool isDown = false;
			if (isControl)
			{
				switch (obj.name)
				{
				case "ArrowDown1":
					isDown = true;
					_uiDialog._frameIndex = 0;
					break;
				case "ArrowDown2":
					isDown = true;
					_uiDialog._frameIndex = 1;
					break;
				case "ArrowDown3":
					isDown = true;
					_uiDialog._frameIndex = 2;
					break;
				case "ArrowDown4":
					isDown = true;
					_uiDialog._frameIndex = 3;
					break;
				case "ArrowUp1":
					_uiDialog._frameIndex = 0;
					break;
				case "ArrowUp2":
					_uiDialog._frameIndex = 1;
					break;
				case "ArrowUp3":
					_uiDialog._frameIndex = 2;
					break;
				case "ArrowUp4":
					_uiDialog._frameIndex = 3;
					break;
				}
				_uiDialog.UpdateFrameSelect();
				changeMaterialCnt(isDown);
			}
		}

		public void changeMaterialCnt(bool isDown)
		{
			UIPanel component = ((Component)_uiDialog._uiMaterialFrame[_uiDialog._frameIndex].transform.FindChild("Panel")).GetComponent<UIPanel>();
			UILabel component2 = ((Component)component.transform.FindChild("LabelGrp/Label3")).GetComponent<UILabel>();
			if (isDown)
			{
				if (component2.textInt == 0)
				{
					materialCnt[MaterialIndex] += _uiDialog.SetMaterialCount() * 9;
				}
				else
				{
					materialCnt[MaterialIndex] -= _uiDialog.SetMaterialCount();
				}
			}
			else if (component2.textInt == 9)
			{
				materialCnt[MaterialIndex] -= _uiDialog.SetMaterialCount() * 9;
			}
			else
			{
				materialCnt[MaterialIndex] += _uiDialog.SetMaterialCount();
			}
			isControl = false;
			_uiDialog.MoveMaterialSlot((!isDown) ? true : false);
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			LightButton();
		}

		public void changeMaterialCntMinMax(bool isMinButton)
		{
			UIPanel component = ((Component)_uiDialog._uiMaterialFrame[_uiDialog._frameIndex].transform.FindChild("Panel")).GetComponent<UIPanel>();
			((Component)component.transform.FindChild("LabelGrp/Label3")).GetComponent<UILabel>();
			if (isMinButton)
			{
				if (materialCnt[MaterialIndex] == materialMin[MaterialIndex])
				{
					return;
				}
				materialCnt[MaterialIndex] = materialMin[MaterialIndex];
			}
			else
			{
				if (materialCnt[MaterialIndex] == materialMax[MaterialIndex])
				{
					return;
				}
				materialCnt[MaterialIndex] = ((materialMax[MaterialIndex] < _material[MaterialIndex]) ? materialMax[MaterialIndex] : ((_material[MaterialIndex] >= materialMin[MaterialIndex]) ? _material[MaterialIndex] : materialMin[MaterialIndex]));
			}
			isControl = false;
			_uiDialog.MoveMaterialSlot((!isMinButton) ? true : false, isAnime: true);
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}

		private void setIndexMap()
		{
			switch (nowPanelType)
			{
			case PanelType.Normal:
				if (ArsenalTaskManager.GetLogicManager().LargeEnabled)
				{
					KeyController.setUseIndexMap(normalIndexMap);
				}
				else
				{
					KeyController.setUseIndexMap(normalIndexMapDisableBig);
				}
				break;
			case PanelType.Big:
				KeyController.setUseIndexMap(BigIndexMap);
				break;
			case PanelType.SlotItem:
				KeyController.setUseIndexMap(KaihatsuIndexMap);
				break;
			case PanelType.Tanker:
				KeyController.setUseIndexMap(TankerIndexMap);
				break;
			}
		}

		public void selectMaterialFrameEL(GameObject obj)
		{
			if (isControl && !isMaterialCountMode)
			{
				switch (obj.name)
				{
				case "Material1":
					KeyController.Index = 1;
					break;
				case "Material2":
					KeyController.Index = 4;
					break;
				case "Material3":
					KeyController.Index = 2;
					break;
				case "Material4":
					KeyController.Index = 5;
					break;
				}
				UpdateDialogMaterialCount();
				UpdateSelectBtn();
				_selectMaterialFrame();
			}
		}

		private void _selectMaterialFrame()
		{
			if (!isMaterialCountMode)
			{
				nowPanelState = PanelFocus.MaterialCountPanel;
				if (_uiDialog._frameIndex == 0 && !isBigConstruct)
				{
					_uiDialog._frameIndex = 1;
				}
				else
				{
					_uiDialog._frameIndex = 0;
				}
				_BG_touch2.transform.localScale = Vector3.one;
				_uiDialog.ShowDialog(MaterialIndex);
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				KeyController.isStopIndex = true;
			}
		}

		public void initMaterialCount(TaskMainArsenalManager.State state)
		{
			switch (state)
			{
			case TaskMainArsenalManager.State.KENZOU:
				for (int l = 0; l < 4; l++)
				{
					materialMin[l] = 30;
					materialMax[l] = 999;
					materialCnt[l] = ((materialPreset[l, 0] != -1) ? materialPreset[l, 0] : materialMin[l]);
					materialPreset[l, 0] = materialCnt[l];
				}
				break;
			case TaskMainArsenalManager.State.KAIHATSU:
				for (int j = 0; j < 4; j++)
				{
					materialMin[j] = 10;
					materialMax[j] = 300;
					materialCnt[j] = ((materialPreset[j, 1] != -1) ? materialPreset[j, 1] : 20);
					materialPreset[j, 1] = materialCnt[j];
				}
				break;
			case TaskMainArsenalManager.State.KENZOU_BIG:
				materialMin[0] = 1500;
				materialMin[1] = 1500;
				materialMin[2] = 2000;
				materialMin[3] = 1000;
				for (int k = 0; k < 4; k++)
				{
					materialMax[k] = 7000;
					materialCnt[k] = ((materialPreset[k, 2] != -1) ? materialPreset[k, 2] : materialMin[k]);
					materialPreset[k, 2] = materialCnt[k];
				}
				break;
			case TaskMainArsenalManager.State.YUSOUSEN:
				materialMin[0] = 40;
				materialMin[1] = 10;
				materialMin[2] = 40;
				materialMin[3] = 10;
				for (int i = 0; i < 4; i++)
				{
					materialMax[i] = 999;
					materialCnt[i] = ((materialPreset[i, 3] != -1) ? materialPreset[i, 3] : materialMin[i]);
					materialPreset[i, 3] = materialCnt[i];
				}
				break;
			}
			materialMin[4] = 1;
			materialMax[4] = 1000;
			materialCnt[4] = ((materialPreset[4, 4] != -1) ? materialPreset[4, 4] : materialMin[4]);
			materialPreset[4, 4] = materialCnt[4];
		}

		public bool CheckDialogMaterialCount(int setCount)
		{
			int num = 0;
			int num2 = 0;
			if (KeyController.Index == 1)
			{
				num2 = 0;
				num = TaskMainArsenalManager.arsenalManager.Material.Fuel;
			}
			else if (KeyController.Index == 4)
			{
				num2 = 1;
				num = TaskMainArsenalManager.arsenalManager.Material.Ammo;
			}
			else if (KeyController.Index == 2)
			{
				num2 = 2;
				num = TaskMainArsenalManager.arsenalManager.Material.Steel;
			}
			else if (KeyController.Index == 5)
			{
				num2 = 3;
				num = TaskMainArsenalManager.arsenalManager.Material.Baux;
			}
			else if (KeyController.Index == 3 && isBigConstruct)
			{
				num2 = 4;
				num = TaskMainArsenalManager.arsenalManager.Material.Devkit;
			}
			if (setCount > num)
			{
				return false;
			}
			if (setCount > materialMax[num2])
			{
				return false;
			}
			if (setCount < materialMin[num2])
			{
				return false;
			}
			return true;
		}

		public void UpdateDialogMaterialCount()
		{
			int nowMaterial;
			switch (KeyController.Index)
			{
			default:
				return;
			case 1:
				nowMaterial = TaskMainArsenalManager.arsenalManager.Material.Fuel;
				break;
			case 4:
				nowMaterial = TaskMainArsenalManager.arsenalManager.Material.Ammo;
				break;
			case 2:
				nowMaterial = TaskMainArsenalManager.arsenalManager.Material.Steel;
				break;
			case 5:
				nowMaterial = TaskMainArsenalManager.arsenalManager.Material.Baux;
				break;
			case 3:
				nowMaterial = TaskMainArsenalManager.arsenalManager.Material.Devkit;
				break;
			}
			for (int i = 0; i < 4; i++)
			{
				_uiDialog.MoveMaterialCount(materialCnt[MaterialIndex], i, nowMaterial);
			}
			setMaterialLabel2(MaterialIndex, nowMaterial);
		}

		public void UpdateMaterialCount(int num)
		{
			int num2 = materialCnt[num];
			_material[0] = TaskMainArsenalManager.arsenalManager.Material.Fuel;
			_material[1] = TaskMainArsenalManager.arsenalManager.Material.Ammo;
			_material[2] = TaskMainArsenalManager.arsenalManager.Material.Steel;
			_material[3] = TaskMainArsenalManager.arsenalManager.Material.Baux;
			_material[4] = TaskMainArsenalManager.arsenalManager.Material.Devkit;
			GameObject[] array = new GameObject[4];
			UILabel[] array2 = new UILabel[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = _uiMaterialsObj[num].transform.FindChild("MaterialFrame" + (i + 1)).gameObject;
				array2[i] = ((Component)array[i].transform.FindChild("Label")).GetComponent<UILabel>();
				array2[i].color = setMaterialLabel(num, _material[num]);
			}
			array2[3].text = string.Empty + num2 % 10;
			num2 /= 10;
			array2[2].text = string.Empty + num2 % 10;
			num2 /= 10;
			array2[1].text = string.Empty + num2 % 10;
			num2 /= 10;
			array2[0].text = string.Empty + num2 % 10;
			if (nowPanelType == PanelType.Normal)
			{
				for (int j = 0; j < 4; j++)
				{
					materialPreset[j, 0] = materialCnt[j];
				}
			}
			else if (nowPanelType == PanelType.SlotItem)
			{
				for (int k = 0; k < 4; k++)
				{
					materialPreset[k, 1] = materialCnt[k];
				}
			}
			else if (nowPanelType == PanelType.Big)
			{
				for (int l = 0; l < 4; l++)
				{
					materialPreset[l, 2] = materialCnt[l];
				}
			}
			else if (nowPanelType == PanelType.Tanker)
			{
				for (int m = 0; m < 4; m++)
				{
					materialPreset[m, 3] = materialCnt[m];
				}
			}
		}

		public void CompMoveMaterialSlot()
		{
			UpdateDialogMaterialCount();
			isControl = true;
		}

		public void setMaterialCount(int num, int nowMaterial)
		{
			if (materialCnt[num] >= nowMaterial)
			{
				materialCnt[num] = ((nowMaterial >= materialMin[num]) ? nowMaterial : materialMin[num]);
				return;
			}
			if (materialCnt[num] >= materialMax[num])
			{
				materialCnt[num] = materialMax[num];
			}
			if (materialCnt[num] <= materialMin[num])
			{
				materialCnt[num] = materialMin[num];
			}
		}

		public Color setMaterialLabel(int num, int nowMaterial)
		{
			if (materialCnt[num] > nowMaterial)
			{
				return Color.red;
			}
			if (materialCnt[num] > materialMax[num] || materialCnt[num] < materialMin[num])
			{
				return _Orange;
			}
			return Color.white;
		}

		public void setMaterialLabel2(int num, int nowMaterial)
		{
			Color white = Color.white;
			Color color = (materialCnt[num] > nowMaterial) ? Color.red : ((materialCnt[num] <= materialMax[num] && materialCnt[num] >= materialMin[num]) ? Color.white : _Orange);
			for (int i = 0; i < 4; i++)
			{
				UIPanel component = ((Component)_uiDialog._uiMaterialFrame[i].transform.FindChild("Panel")).GetComponent<UIPanel>();
				for (int j = 0; j < 5; j++)
				{
					UILabel component2 = ((Component)component.transform.FindChild("LabelGrp/Label" + (j + 1))).GetComponent<UILabel>();
					component2.color = color;
				}
			}
		}

		public void moveDevkitSwitch()
		{
			_devKitManager.nextSwitch();
		}

		public bool startConstructEL(GameObject obj)
		{
			if (!isControl)
			{
				return false;
			}
			if (_isCreateView)
			{
				return false;
			}
			if (!isSlotItem)
			{
				return startKenzouEL();
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, materialCnt[0]);
			dictionary.Add(enumMaterialCategory.Bull, materialCnt[1]);
			dictionary.Add(enumMaterialCategory.Steel, materialCnt[2]);
			dictionary.Add(enumMaterialCategory.Bauxite, materialCnt[3]);
			if (!TaskMainArsenalManager.arsenalManager.IsValidCreateItem(materialCnt[0], materialCnt[1], materialCnt[2], materialCnt[3]))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
				if (materialCnt.Any((int x) => x > 300))
				{
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.MaterialUpperLimit));
				}
				else if (Comm_UserDatas.Instance.User_basic.IsMaxSlotitem())
				{
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.CannotArsenalByLimitItem));
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NotEnoughMaterial));
				}
				return true;
			}
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			IReward_Slotitem reward_Slotitem = TaskMainArsenalManager.arsenalManager.CreateItem(materialCnt[0], materialCnt[1], materialCnt[2], materialCnt[3], SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
			SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(TaskMainArsenalManager.arsenalManager);
			KeyController.ClearKeyAll();
			_isCreateView = true;
			TaskMainArsenalManager.isTouchEnable = false;
			TaskMainArsenalManager.IsControl = false;
			bool enabled = (reward_Slotitem != null) ? true : false;
			_prodReceiveItem = ProdReceiveSlotItem.Instantiate(PrefabFile.Load<ProdReceiveSlotItem>(PrefabFileInfos.CommonProdReceiveItem), GameObject.Find("ProdArea").transform, reward_Slotitem, 20, KeyController, enabled, arsenal: true);
			_prodReceiveItem.SetLayer(13);
			_prodReceiveItem.Play(_onKaihatsuFinished);
			ArsenalTaskManager._clsArsenal.SetNeedRefreshForSlotItemKaitaiList(needRefreshKaitaiList: true);
			if (SingletonMonoBehaviour<Live2DModel>.exist())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			}
			return false;
		}

		public bool startKenzouEL()
		{
			bool flag = false;
			bool flag2;
			if (nowPanelType == PanelType.Tanker)
			{
				materialCnt[4] = _sPointManager.GetUseSpointNum();
				BuildDockModel buildDockModel = TaskMainArsenalManager.arsenalManager.CreateTanker(dockIndex + 1, _speedIconManager.IsHigh, materialCnt[0], materialCnt[1], materialCnt[2], materialCnt[3], materialCnt[4]);
				flag2 = (buildDockModel != null);
			}
			else
			{
				materialCnt[4] = ((nowPanelType == PanelType.Normal) ? 1 : _devKitManager.getUseDevKitNum());
				flag2 = TaskMainArsenalManager.arsenalManager.CreateShip(dockIndex + 1, _speedIconManager.IsHigh, materialCnt[0], materialCnt[1], materialCnt[2], materialCnt[3], materialCnt[4], SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
				flag = true;
			}
			SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(TaskMainArsenalManager.arsenalManager);
			if (flag2)
			{
				Hide();
				ArsenalTaskManager.ReqPhase(ArsenalTaskManager.ArsenalPhase.BattlePhase_ST);
				if (_speedIconManager.IsHigh)
				{
					for (int i = 0; i < 4; i++)
					{
						if (dockIndex == i)
						{
							TaskMainArsenalManager.dockMamager[i].SetFirstHight();
						}
					}
					TutorialModel tutorial = ArsenalTaskManager.GetLogicManager().UserInfo.Tutorial;
					if (tutorial.GetStep() == 1 && !tutorial.GetStepTutorialFlg(2))
					{
						tutorial.SetStepTutorialFlg(2);
						tutorial.SetStepTutorialFlg(3);
						CommonPopupDialog.Instance.StartPopup("「はじめての建造！」 達成");
						SoundUtils.PlaySE(SEFIleInfos.SE_012);
					}
					else if (tutorial.GetStep() == 2 && !tutorial.GetStepTutorialFlg(3))
					{
						tutorial.SetStepTutorialFlg(3);
						CommonPopupDialog.Instance.StartPopup("「高速建造！」 達成");
						ArsenalTaskManager._clsArsenal.DestroyTutorial();
						SoundUtils.PlaySE(SEFIleInfos.SE_012);
					}
				}
				ArsenalTaskManager._clsArsenal.hideDialog();
				ArsenalTaskManager._clsArsenal.SetDock();
				SoundUtils.PlaySE(SEFIleInfos.SE_015);
				isEnd = true;
				isControl = false;
				for (int j = 0; j < 4; j++)
				{
					TaskMainArsenalManager.dockMamager[j].updateSpeedUpIcon();
					TaskMainArsenalManager.dockMamager[j].setSelect(dockIndex == j);
				}
				if (flag)
				{
					TutorialModel tutorial2 = ArsenalTaskManager.GetLogicManager().UserInfo.Tutorial;
					if (tutorial2.GetStep() == 1 && !tutorial2.GetStepTutorialFlg(2))
					{
						tutorial2.SetStepTutorialFlg(2);
						CommonPopupDialog.Instance.StartPopup("「はじめての建造！」 達成");
						SoundUtils.PlaySE(SEFIleInfos.SE_012);
					}
				}
				return true;
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			if (nowPanelType != PanelType.Tanker)
			{
				if (Comm_UserDatas.Instance.User_basic.IsMaxChara())
				{
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.CannotArsenalByLimitShip));
				}
				else if (Comm_UserDatas.Instance.User_basic.IsMaxSlotitem())
				{
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.CannotArsenalByLimitItem));
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NotEnoughMaterial));
				}
			}
			else if (ArsenalTaskManager.GetLogicManager().UserInfo.SPoint < _sPointManager.GetUseSpointNum())
			{
				CommonPopupDialog.Instance.StartPopup("戦略ポイントが不足しています");
			}
			else
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NotEnoughMaterial));
			}
			return false;
		}

		private void _onKaihatsuFinished()
		{
			if (_prodReceiveItem != null)
			{
				_prodReceiveItem.ReleaseTextures();
			}
			_prodReceiveItem = null;
			_isCreateView = false;
			TaskMainArsenalManager.isTouchEnable = true;
			TaskMainArsenalManager.IsControl = true;
			ArsenalTaskManager._clsArsenal.hideDialog();
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
		}
	}
}
