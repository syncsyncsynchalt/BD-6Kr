using KCV.EscortOrganize;
using KCV.Utils;
using local.models;
using Server_Controllers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy.Deploy
{
	public class DeployMainPanel : MonoBehaviour
	{
		private enum MAIN_BTN
		{
			HAIBI,
			GOEI,
			OK
		}

		[SerializeField]
		private UIButtonManager btnManager;

		[SerializeField]
		private DeployShip[] DeployShips;

		[SerializeField]
		private DeployMaterials deployMaterials;

		[SerializeField]
		private UILabel TankerNum;

		[SerializeField]
		private TaskDeployTop top;

		private GameObject EscortOrganize;

		[SerializeField]
		private YesNoButton BackConfirm;

		private string[] ButtonsName;

		private KeyControl keyController;

		private Debug_Mod debugMod;

		private bool isPlayVoice;

		[SerializeField]
		private CommonDialog ConfirmDialog;

		private ShipModel EscortFlagShipModel;

		[SerializeField]
		private UITexture Landscape;

		private Coroutine PanelShowCor;

		[SerializeField]
		private ButtonLightTexture btnLight;

		private bool mIsEndPhase;

		private List<int> NormalArea = new List<int>
		{
			1,
			8,
			9,
			11,
			12
		};

		private List<int> NorthArea = new List<int>
		{
			3,
			13
		};

		private List<int> SouthArea = new List<int>
		{
			2,
			4,
			5,
			6,
			7,
			10,
			14
		};

		public void Init(bool isGoeiChange)
		{
			mIsEndPhase = false;
			if (keyController == null)
			{
				keyController = new KeyControl(0, 2);
			}
			SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
			SetLandscape();
			TankerNum.text = top.TankerCount.ToString();
			deployMaterials.updateMaterials(top.areaID, top.TankerCount, EscortOrganizeTaskManager.GetEscortManager());
			KeyControlManager.Instance.KeyController = keyController;
			EscortDeckModel editDeck = EscortOrganizeTaskManager.GetEscortManager().EditDeck;
			InitializeEscortDeckIcons(editDeck);
			StrategyTopTaskManager.Instance.UIModel.OverCamera.SetActive(isActive: true);
			top.isChangeMode = false;
			btnManager.setFocus(0);
			if (isGoeiChange)
			{
				keyController.IsRun = false;
				ChangeCharactertoEscortFlagShip();
			}
			this.DelayAction(0.3f, delegate
			{
				if (EscortOrganize == null)
				{
					GameObject gameObject = Util.Instantiate(StrategyTopTaskManager.GetCommandMenu().EscortOrganize, base.transform.parent.gameObject);
					gameObject.SetActive(false);
					gameObject.transform.localPositionX(9999f);
					EscortOrganize = gameObject;
				}
				if (StrategyTopTaskManager.GetLogicManager().IsValidDeploy(StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID, top.TankerCount, EscortOrganizeTaskManager.GetEscortManager()))
				{
					btnLight.PlayAnim();
				}
				else
				{
					btnLight.StopAnim();
				}
			});
		}

		private void InitializeEscortDeckIcons(EscortDeckModel escortDeckModel)
		{
			for (int i = 0; i < DeployShips.Length; i++)
			{
				if (i < escortDeckModel.Count)
				{
					DeployShips[i].Initialize(escortDeckModel.GetShip(i));
				}
				else
				{
					DeployShips[i].InitializeDefailt();
				}
			}
		}

		private void ChangeStateHaibi()
		{
			top.isChangeMode = true;
			top.isDeployPanel = true;
			if (PanelShowCor != null)
			{
				StopCoroutine(PanelShowCor);
			}
		}

		private void ChangeStateGoei()
		{
			base.gameObject.SafeGetTweenAlpha(1f, 0f, 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
			base.gameObject.GetComponent<TweenAlpha>().onFinished.Clear();
			mIsEndPhase = true;
			StrategyTopTaskManager.Instance.UIModel.Character.isEnter = true;
			StrategyTopTaskManager.Instance.UIModel.Character.Exit(delegate
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			});
			this.DelayAction(0.3f, delegate
			{
				EscortOrganize.SetActive(true);
				EscortOrganizeTaskManager.Init();
				this.DelayActionFrame(3, delegate
				{
					TweenAlpha.Begin(EscortOrganize, 0.2f, 1f);
					App.isFirstUpdate = true;
				});
			});
			if (PanelShowCor != null)
			{
				StopCoroutine(PanelShowCor);
			}
		}

		private void ChangeStateCommandMenu()
		{
			if (mIsEndPhase)
			{
				return;
			}
			if (StrategyTopTaskManager.GetLogicManager().IsValidDeploy(StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID, top.TankerCount, EscortOrganizeTaskManager.GetEscortManager()))
			{
				StrategyTopTaskManager.GetLogicManager().Deploy(top.areaID, top.TankerCount, EscortOrganizeTaskManager.GetEscortManager());
				StrategyTopTaskManager.CreateLogicManager();
				isPlayVoice = false;
				EscortFlagShipModel = null;
				top.backToCommandMenu();
				mIsEndPhase = true;
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				if (PanelShowCor != null)
				{
					StopCoroutine(PanelShowCor);
				}
				TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
				if (tutorial.GetStep() == 8 && !tutorial.GetStepTutorialFlg(9))
				{
					tutorial.SetStepTutorialFlg(9);
					CommonPopupDialog.Instance.StartPopup("「はじめての配備！」 達成");
					SoundUtils.PlaySE(SEFIleInfos.SE_012);
				}
				StrategyTopTaskManager.Instance.setActiveStrategy(isActive: true);
			}
			else
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
				CommonPopupDialog.Instance.StartPopup("変更がありません", 0, CommonPopupDialogMessage.PlayType.Long);
			}
		}

		public bool Run()
		{
			if (mIsEndPhase)
			{
				return false;
			}
			keyController.Update();
			int index = keyController.Index;
			if (keyController.IsUpDown())
			{
				btnManager.movePrevButton();
			}
			else if (keyController.IsDownDown())
			{
				btnManager.moveNextButton();
			}
			else if (keyController.keyState[1].down)
			{
				btnManager.Decide();
			}
			else if (keyController.keyState[0].down)
			{
				OnTouchBackArea();
			}
			else if (keyController.keyState[5].down)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			return true;
		}

		private void OpenConfirmDialog()
		{
			keyController.IsRun = false;
			ConfirmDialog.isUseDefaultKeyController = false;
			ConfirmDialog.OpenDialog(1);
			ConfirmDialog.setCloseAction(delegate
			{
				keyController.IsRun = true;
				KeyControlManager.Instance.KeyController = keyController;
			});
			BackConfirm.SetKeyController(new KeyControl());
			BackConfirm.SetOnSelectPositiveListener(delegate
			{
				ChangeStateBack();
				ConfirmDialog.CloseDialog();
			});
			BackConfirm.SetOnSelectNegativeListener(delegate
			{
				ConfirmDialog.CloseDialog();
			});
		}

		public void OnTouchBackArea()
		{
			if (!mIsEndPhase)
			{
				if (StrategyTopTaskManager.GetLogicManager().IsValidDeploy(StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID, top.TankerCount, EscortOrganizeTaskManager.GetEscortManager()))
				{
					OpenConfirmDialog();
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				}
				else
				{
					ChangeStateBack();
					mIsEndPhase = true;
				}
			}
		}

		private void ChangeStateBack()
		{
			if (PanelShowCor == null && !top.isDeployPanel)
			{
				PanelShowCor = null;
				isPlayVoice = false;
				EscortFlagShipModel = null;
				top.backToCommandMenu();
				StrategyTopTaskManager.Instance.setActiveStrategy(isActive: true);
			}
		}

		private void ChangeCharactertoEscortFlagShip()
		{
			StrategyShipCharacter Character = StrategyTopTaskManager.Instance.UIModel.Character;
			ShipModel FlagShip = EscortOrganizeTaskManager.GetEscortManager().EditDeck.GetFlagShip();
			bool flag = EscortFlagShipModel == null || EscortFlagShipModel != FlagShip;
			float fromAlpha = 0f;
			UIPanel component = base.gameObject.GetComponent<UIPanel>();
			if (component != null)
			{
				fromAlpha = component.alpha;
			}
			base.gameObject.GetComponent<TweenAlpha>().onFinished.Clear();
			if (Character.isEnter && flag)
			{
				Character.Exit(delegate
				{
					EnterEscortFlagShip(Character, FlagShip);
				}, isActive: true);
				PanelShowCor = this.DelayAction(0.3f, delegate
				{
					base.gameObject.SafeGetTweenAlpha(fromAlpha, 1f, 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
					base.gameObject.GetComponent<TweenAlpha>().onFinished.Clear();
					this.DelayAction(0.1f, delegate
					{
						keyController.IsRun = true;
					});
					PanelShowCor = null;
				});
				return;
			}
			EnterEscortFlagShip(Character, FlagShip);
			base.gameObject.SafeGetTweenAlpha(fromAlpha, 1f, 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
			base.gameObject.GetComponent<TweenAlpha>().onFinished.Clear();
			this.DelayAction(0.1f, delegate
			{
				keyController.IsRun = true;
			});
		}

		private void EnterEscortFlagShip(StrategyShipCharacter Character, ShipModel FlagShip)
		{
			if (FlagShip != null)
			{
				Character.ChangeCharacter(FlagShip);
				this.DelayActionFrame(2, delegate
				{
					if (EscortFlagShipModel != FlagShip)
					{
						isPlayVoice = false;
					}
					Character.Enter(delegate
					{
						if (!isPlayVoice)
						{
							Character.PlayVoice(EscortOrganizeTaskManager.GetEscortManager().EditDeck);
							EscortFlagShipModel = FlagShip;
							isPlayVoice = true;
						}
					});
				});
			}
		}

		public void DestroyEscortOrganize()
		{
			if (EscortOrganize != null)
			{
				UnityEngine.Object.Destroy(EscortOrganize);
			}
		}

		[Obsolete("外部UI[輸送船団ボタン]から参照して使用します")]
		public void OnClickYusousendan()
		{
			if (!top.isDeployPanel && !mIsEndPhase && keyController.IsRun)
			{
				keyController.Index = 0;
				ChangeStateHaibi();
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		[Obsolete("外部UI[海上護衛部隊ボタン]から参照して使用します")]
		public void OnClickKaijougoeiButai()
		{
			if (!top.isDeployPanel && !mIsEndPhase && keyController.IsRun)
			{
				keyController.Index = 1;
				ChangeStateGoei();
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		[Obsolete("外部UI[決定ボタン]から参照して使用します")]
		public void OnClickKettei()
		{
			if (!top.isDeployPanel && !mIsEndPhase && keyController.IsRun)
			{
				keyController.Index = 2;
				ChangeStateCommandMenu();
			}
		}

		private void SetLandscape()
		{
			int areaID = StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID;
			if (NormalArea.Exists((int x) => x == areaID))
			{
				Landscape.mainTexture = (Resources.Load("Textures/Strategy/Deploy/popup_bg1") as Texture);
				Landscape.transform.localPositionY(101f);
			}
			else if (NorthArea.Exists((int x) => x == areaID))
			{
				Landscape.mainTexture = (Resources.Load("Textures/Strategy/Deploy/popup_bg3") as Texture);
				Landscape.transform.localPositionY(101f);
			}
			else if (SouthArea.Exists((int x) => x == areaID))
			{
				Landscape.mainTexture = (Resources.Load("Textures/Strategy/Deploy/popup_bg2") as Texture);
				Landscape.transform.localPositionY(110f);
			}
		}
	}
}
