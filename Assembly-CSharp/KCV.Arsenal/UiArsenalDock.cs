using Common.Enum;
using KCV.PopupString;
using KCV.Production;
using KCV.Utils;
using local.models;
using local.utils;
using Server_Common;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalDock : MonoBehaviour
	{
		private enum DockMode
		{
			Close,
			Show,
			Lock
		}

		[SerializeField]
		private UITexture _uiBg;

		[SerializeField]
		private UITexture _uiBg2;

		[SerializeField]
		private UIButton _uiStartBtn;

		[SerializeField]
		private UIButton _uiGetBtn;

		[SerializeField]
		private UIButton _lockBtn;

		[SerializeField]
		private UILabel _uiTurnLabel;

		[SerializeField]
		private GameObject _lockObj;

		[SerializeField]
		private GameObject _lockFrameObj;

		[SerializeField]
		private Animation _lockAnim;

		[SerializeField]
		private ButtonLightTexture btnLight;

		private UISprite _uiStartBtnSprite;

		private UISprite _uiHighBtnSprite;

		private UISprite _uiGetBtnSprite;

		private UISprite _lockBtnSprite;

		private int _number;

		private int _limit;

		private bool _isCreate;

		private Coroutine cor;

		private ShipModelMst _ship;

		private BuildDockModel _dock;

		private ProdReceiveShip _prodReceiveShip;

		private UiArsenalDockShipManager _dockShipManager;

		private TaskMainArsenalManager taskMainArsenalManager;

		private IReward_Ship _rewardShip;

		private DockMode dockMode;

		[SerializeField]
		public UIButton _uiHighBtn;

		public UITexture _uiLockedDockL;

		public UITexture _uiLockedDockR;

		public bool IsFirstHight;

		public bool IsShowHigh;

		public bool IsHight;

		public bool IsHightEnd;

		public bool IsLarge;

		public bool isCompleteVoicePlayable;

		public UiArsenalDockMini _dockMiniMamager;

		public UiArsenalShipManager _shipSManager;

		public bool init(TaskMainArsenalManager taskMainArsenalManager, int num)
		{
			this.taskMainArsenalManager = taskMainArsenalManager;
			_number = num;
			_limit = 0;
			IsLarge = false;
			IsFirstHight = false;
			IsShowHigh = false;
			IsHight = false;
			IsHightEnd = false;
			isCompleteVoicePlayable = false;
			dockMode = DockMode.Close;
			Util.FindParentToChild(ref _uiBg, base.transform, "Bg");
			Util.FindParentToChild(ref _uiBg2, base.transform, "Bg2");
			Util.FindParentToChild(ref _uiStartBtn, base.transform, "ButtonStart");
			Util.FindParentToChild(ref _uiStartBtnSprite, _uiStartBtn.transform, "Background");
			Util.FindParentToChild(ref _uiGetBtn, base.transform, "ButtonGet");
			Util.FindParentToChild(ref _uiGetBtnSprite, _uiGetBtn.transform, "Background");
			Util.FindParentToChild(ref _uiHighBtn, base.transform, "ButtonHight");
			Util.FindParentToChild(ref _uiHighBtnSprite, _uiHighBtn.transform, "Background");
			Util.FindParentToChild(ref _uiTurnLabel, base.transform, "LabelTurn");
			if (_lockObj == null)
			{
				_lockObj = base.transform.FindChild("LockObj").gameObject;
			}
			if (_lockFrameObj == null)
			{
				_lockFrameObj = _lockObj.transform.FindChild("FrameObject").gameObject;
			}
			Util.FindParentToChild(ref _lockBtn, _lockObj.transform, "LockButton");
			Util.FindParentToChild(ref _lockBtnSprite, _lockBtn.transform, "Background");
			Util.FindParentToChild<Animation>(ref _lockAnim, base.transform, "LockBtn");
			Util.FindParentToChild(ref _uiLockedDockL, _lockObj.transform, "FrameObject/LockFrameL");
			Util.FindParentToChild(ref _uiLockedDockR, _lockObj.transform, "FrameObject/LockFrameR");
			Util.FindParentToChild(ref _dockMiniMamager, _uiBg2.transform, "Panel");
			Util.FindParentToChild(ref _shipSManager, _uiBg2.transform, "Panel/ShipManager");
			Util.FindParentToChild(ref btnLight, _uiHighBtn.transform, "ButtonLight");
			_dockMiniMamager.init(_number);
			_shipSManager.init(_number);
			_close();
			_isCreate = true;
			return true;
		}

		private void OnDestroy()
		{
			if (cor != null)
			{
				StopCoroutine(cor);
			}
			cor = null;
			Mem.Del(ref _uiBg);
			Mem.Del(ref _uiBg2);
			Mem.Del(ref _uiStartBtn);
			Mem.Del(ref _uiGetBtn);
			Mem.Del(ref _lockBtn);
			Mem.Del(ref _uiTurnLabel);
			Mem.Del(ref _lockObj);
			Mem.Del(ref _lockFrameObj);
			Mem.Del(ref _lockAnim);
			Mem.Del(ref btnLight);
			Mem.Del(ref _uiStartBtnSprite);
			Mem.Del(ref _uiHighBtnSprite);
			Mem.Del(ref _uiGetBtnSprite);
			Mem.Del(ref _lockBtnSprite);
			Mem.Del(ref cor);
			Mem.Del(ref _ship);
			Mem.Del(ref _dock);
			Mem.Del(ref _prodReceiveShip);
			Mem.Del(ref _dockShipManager);
			Mem.Del(ref _rewardShip);
			Mem.Del(ref dockMode);
			Mem.Del(ref _uiHighBtn);
			Mem.Del(ref _uiLockedDockL);
			Mem.Del(ref _uiLockedDockR);
			Mem.Del(ref _dockMiniMamager);
			Mem.Del(ref _shipSManager);
		}

		public bool SelectDockMode()
		{
			return (dockMode != 0) ? true : false;
		}

		public bool GetLockDockMode()
		{
			return (dockMode == DockMode.Lock) ? true : false;
		}

		public KdockStates GetDockState()
		{
			return _dock.State;
		}

		public void DisableParticles()
		{
			if (_dockMiniMamager != null)
			{
				_dockMiniMamager.DisableParticles();
			}
		}

		public void EnableParticles()
		{
			if (_dockMiniMamager != null)
			{
				_dockMiniMamager.EnableParticles();
			}
		}

		public void _close()
		{
			dockMode = DockMode.Close;
			_uiBg.mainTexture = (Resources.Load("Textures/Arsenal/dock/kenzo_bg_none") as Texture2D);
			_uiBg2.alpha = 0f;
			_uiGetBtn.transform.localScale = Vector3.zero;
			_uiStartBtn.transform.localScale = Vector3.zero;
			_uiHighBtn.transform.localScale = Vector3.zero;
			_uiTurnLabel.alpha = 0f;
			_lockObj.SetActive(true);
			_lockFrameObj.SetActive(true);
			_lockBtn.transform.localScale = Vector3.zero;
			btnLight.StopAnim();
		}

		public void ShowKeyLock()
		{
			dockMode = DockMode.Lock;
			_lockObj.SetActive(true);
			_lockFrameObj.SetActive(true);
			_lockBtn.transform.localScale = Vector3.one;
			_lockBtnSprite.spriteName = "btn_addDock";
		}

		private bool isDockOpenEnable()
		{
			return (0 < TaskMainArsenalManager.arsenalManager.NumOfKeyPossessions) ? true : false;
		}

		public void HideKeyLock()
		{
			_lockObj.SetActive(false);
		}

		public void _setShow()
		{
			_setShow(DockOpen: false);
		}

		public void _setShow(bool DockOpen)
		{
			_dock = TaskMainArsenalManager.arsenalManager.GetDock(_number + 1);
			_close();
			if (dockMode == DockMode.Close)
			{
				dockMode = DockMode.Show;
			}
			_uiBg.alpha = 1f;
			_uiBg2.alpha = 1f;
			_uiBg.mainTexture = (Resources.Load("Textures/Arsenal/dock/kenzo_bg_1") as Texture2D);
			if (!DockOpen && dockMode != 0)
			{
				_lockObj.SetActive(false);
			}
			updateSpeedUpIcon();
			if (_dock.IsLarge())
			{
				_uiBg2.mainTexture = (Resources.Load("Textures/Arsenal/dock/kenzo_build2_bg") as Texture2D);
			}
			else if (_dock.IsTunker())
			{
				_uiBg2.mainTexture = (Resources.Load("Textures/Arsenal/dock/kenzo_build3_bg") as Texture2D);
			}
			else
			{
				_uiBg2.mainTexture = (Resources.Load("Textures/Arsenal/dock/kenzo_build1_bg") as Texture2D);
			}
			if (_dock.State == KdockStates.COMPLETE)
			{
				_ship = _dock.Ship;
				if (IsHight)
				{
					_shipSManager.set(_ship, _dock, isHight: true);
					_uiGetBtn.transform.localScale = Vector3.zero;
					_uiHighBtn.transform.localScale = Vector3.one;
					_uiTurnLabel.alpha = 1f;
					_limit = _dock.GetTurn();
					_uiTurnLabel.text = string.Empty + _limit.ToString();
					if (IsFirstHight)
					{
						PlayFirstHightAnimate();
					}
					else
					{
						StartSpeedUpAnimate();
					}
				}
				else
				{
					_shipSManager.set(_ship, _dock, isHight: false);
					endConstruct();
					if (IsHightEnd)
					{
						_dockMiniMamager.PlayEndHightAnimate();
					}
					else
					{
						_dockMiniMamager.PlayConstCompAnimation();
					}
				}
			}
			else if (_dock.State == KdockStates.CREATE)
			{
				_ship = _dock.Ship;
				_shipSManager.set(_ship, _dock, isHight: false);
				_uiGetBtn.transform.localScale = Vector3.zero;
				_uiHighBtn.transform.localScale = Vector3.one;
				_uiTurnLabel.alpha = 1f;
				_limit = _dock.GetTurn();
				_uiTurnLabel.text = string.Empty + _limit.ToString();
				_dockMiniMamager.PlayConstStartAnimation();
			}
			else
			{
				_dockMiniMamager.StopConstAnimation();
				_dockMiniMamager.PlayIdleAnimation();
				_uiStartBtn.transform.localScale = Vector3.one;
			}
		}

		public void endConstruct()
		{
			_limit = 0;
			_uiGetBtn.transform.localScale = Vector3.one;
			_uiBg2.alpha = 1f;
			_uiHighBtn.transform.localScale = Vector3.zero;
			btnLight.StopAnim();
			DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			ShipModel shipModel = (currentDeck == null) ? new ShipModel(1) : currentDeck.GetFlagShip();
			if (cor == null && !taskMainArsenalManager.IsShipGetViewAllDock())
			{
				cor = StartCoroutine(PlayCompleteVoiceWaitForFlagOn(shipModel));
			}
		}

		private IEnumerator PlayCompleteVoiceWaitForFlagOn(ShipModel shipModel)
		{
			while (!isCompleteVoicePlayable)
			{
				yield return new WaitForEndOfFrame();
			}
			ShipUtils.PlayShipVoice(shipModel, 5);
			cor = null;
		}

		public void SetFirstHight()
		{
			IsHight = true;
			IsFirstHight = true;
		}

		public void setSelect(bool select)
		{
			if (select)
			{
				_lockBtnSprite.spriteName = "btn_addDock_on";
				UISelectedObject.SelectedOneObjectBlink(_uiBg.gameObject, value: true);
				UISelectedObject.SelectedOneObjectBlink(_uiLockedDockL.gameObject, value: true);
				UISelectedObject.SelectedOneObjectBlink(_uiLockedDockR.gameObject, value: true);
				_uiStartBtnSprite.spriteName = "btn_build_on";
				_uiGetBtnSprite.spriteName = "btn_get_on";
				if (!IsHight)
				{
					if (TaskMainArsenalManager.arsenalManager.IsValidCreateShip_ChangeHighSpeed(_number + 1))
					{
						_uiHighBtnSprite.spriteName = "btn_item_on";
						return;
					}
					_uiHighBtnSprite.spriteName = "btn_item_off";
					btnLight.StopAnim();
				}
				else
				{
					_uiHighBtnSprite.spriteName = "btn_item_off";
				}
				return;
			}
			_lockBtnSprite.spriteName = "btn_addDock";
			UISelectedObject.SelectedOneObjectBlink(_uiBg.gameObject, value: false);
			UISelectedObject.SelectedOneObjectBlink(_uiLockedDockL.gameObject, value: false);
			UISelectedObject.SelectedOneObjectBlink(_uiLockedDockR.gameObject, value: false);
			_uiStartBtnSprite.spriteName = "btn_build";
			_uiGetBtnSprite.spriteName = "btn_get";
			if (!IsHight)
			{
				if (TaskMainArsenalManager.arsenalManager.IsValidCreateShip_ChangeHighSpeed(_number + 1))
				{
					_uiHighBtnSprite.spriteName = "btn_item";
					return;
				}
				_uiHighBtnSprite.spriteName = "btn_item_off";
				btnLight.StopAnim();
			}
			else
			{
				_uiHighBtnSprite.spriteName = "btn_item_off";
			}
		}

		public void PlayFirstHightAnimate()
		{
			IsHight = true;
			IsShowHigh = false;
			_dockMiniMamager.PlayFirstHighAnimation();
		}

		public void StartSpeedUpAnimate()
		{
			IsHight = true;
			IsShowHigh = false;
			_dockMiniMamager.PlayHalfwayHightAnimation();
		}

		public void endSpeedUpAnimate()
		{
			IsShowHigh = false;
			IsHightEnd = true;
			IsHight = false;
			_setShow();
		}

		public void updateSpeedUpIcon()
		{
			if (TaskMainArsenalManager.arsenalManager.IsValidCreateShip_ChangeHighSpeed(_number + 1))
			{
				_uiHighBtnSprite.spriteName = "btn_item_on";
				if (!IsHight && !IsShowHigh)
				{
					UISelectedObject.SelectedOneButtonZoomUpDown(_uiHighBtn.gameObject, value: false);
					btnLight.PlayAnim();
				}
			}
			else
			{
				_uiHighBtnSprite.spriteName = "btn_item_off";
				UISelectedObject.SelectedOneButtonZoomUpDown(_uiHighBtn.gameObject, value: false);
				btnLight.StopAnim();
			}
		}

		public void setConstruct()
		{
			for (int i = 0; i < 4; i++)
			{
				TaskMainArsenalManager.dockMamager[i].DisableParticles();
			}
			ArsenalTaskManager.ReqPhase(ArsenalTaskManager.ArsenalPhase.NormalConstruct);
		}

		private void setFocus()
		{
			if (taskMainArsenalManager.CurrentMode != TaskMainArsenalManager.Mode.DOCK_FOCUS)
			{
				taskMainArsenalManager.unsetHexFocus();
				taskMainArsenalManager.focusDock();
			}
			taskMainArsenalManager.selectDock(_number);
		}

		public void DockFrameEL()
		{
			if (UICamera.touchCount <= 1 && !ArsenalTaskManager._clsArsenal.checkDialogOpen() && TaskMainArsenalManager.IsControl)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				ArsenalTaskManager._clsArsenal.DockIndex = _number;
				TaskMainArsenalManager.StateType = TaskMainArsenalManager.State.KENZOU;
				setConstruct();
				ArsenalTaskManager._clsArsenal._isEnd = true;
				TaskMainArsenalManager.IsControl = false;
				setFocus();
			}
		}

		public void HighSpeedIconEL()
		{
			if (UICamera.touchCount <= 1 && !ArsenalTaskManager._clsArsenal.checkDialogOpen() && TaskMainArsenalManager.IsControl)
			{
				setFocus();
				if (TaskMainArsenalManager.arsenalManager.IsValidCreateShip_ChangeHighSpeed(_number + 1))
				{
					_uiHighBtnSprite.spriteName = "btn_item_on";
					ArsenalTaskManager._clsArsenal.showHighSpeedDialog(_number);
					IsShowHigh = true;
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}

		public void DockOpenBtnEL()
		{
			if (UICamera.touchCount <= 1 && !ArsenalTaskManager._clsArsenal.checkDialogOpen() && TaskMainArsenalManager.IsControl)
			{
				setFocus();
				if (TaskMainArsenalManager.arsenalManager.IsValidOpenNewDock())
				{
					ArsenalTaskManager._clsArsenal._dockOpenDialogManager.showDialog(_number);
					ArsenalTaskManager._clsArsenal.showDockOpenDialog();
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NoDockKey));
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}

		public void StartDockOpen()
		{
			TaskMainArsenalManager.arsenalManager.OpenNewDock();
			_lockAnim.Play();
			_setShow(DockOpen: true);
			_lockBtn.SetActive(isActive: false);
		}

		public bool CheckStateEmpty()
		{
			if (_dock != null)
			{
				_dock = TaskMainArsenalManager.arsenalManager.GetDock(_number + 1);
				if (_dock.State == KdockStates.CREATE || _dock.State == KdockStates.COMPLETE)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public void GetShipBtnEL()
		{
			if (UICamera.touchCount > 1 || ArsenalTaskManager._clsArsenal.checkDialogOpen() || !TaskMainArsenalManager.IsControl || IsHight)
			{
				return;
			}
			setFocus();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			ArsenalTaskManager._clsArsenal.setTutorialVisible(isVisible: false);
			if (_dock.IsTunker())
			{
				if (TaskMainArsenalManager.arsenalManager.IsValidGetCreatedTanker(_number + 1))
				{
					int countNoMove = TaskMainArsenalManager.arsenalManager.GetNonDeploymentTankerCount().GetCountNoMove();
					int createdTanker = TaskMainArsenalManager.arsenalManager.GetCreatedTanker(_number + 1);
					int afterNum = countNoMove + createdTanker;
					_shipSManager.init(_number);
					_setShow();
					ArsenalTaskManager._clsArsenal.showTankerDialog(createdTanker, countNoMove, afterNum);
				}
			}
			else if (TaskMainArsenalManager.arsenalManager.IsValidGetCreatedShip(_number + 1))
			{
				IsHight = false;
				_rewardShip = TaskMainArsenalManager.arsenalManager.GetCreatedShip(_number + 1);
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
				TaskMainArsenalManager.IsControl = false;
				TaskMainArsenalManager.isTouchEnable = false;
				Observable.FromCoroutine((IObserver<bool> observer) => createReciveShip(observer)).Subscribe(delegate
				{
					_prodReceiveShip.SetActive(isActive: true);
					_prodReceiveShip.Play(_onShipGetFinished);
				});
				this.DelayActionFrame(3, delegate
				{
					_shipSManager.init(_number);
					_setShow();
				});
			}
			else if (Comm_UserDatas.Instance.User_basic.IsMaxChara())
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.CannotGetArsenalByLimitShip));
			}
			else if (Comm_UserDatas.Instance.User_basic.IsMaxSlotitem())
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.CannotGetArsenalByLimitItem));
			}
		}

		private IEnumerator createReciveShip(IObserver<bool> observer)
		{
			_prodReceiveShip = ProdReceiveShip.Instantiate(PrefabFile.Load<ProdReceiveShip>(PrefabFileInfos.CommonProdReceiveShip), GameObject.Find("ProdArea").transform, _rewardShip, 20, ArsenalTaskManager.GetKeyControl(), needBGM: false);
			_prodReceiveShip.SetLayer(13);
			_prodReceiveShip.SetActive(isActive: false);
			taskMainArsenalManager.SetNeedRefreshForShipKaitaiList(needRefreshKaitaiList: true);
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private void _onShipGetFinished()
		{
			if (_prodReceiveShip != null)
			{
				_prodReceiveShip.ReleaseShipTextureAndBackgroundTexture();
				Object.Destroy(_prodReceiveShip.gameObject);
			}
			_prodReceiveShip = null;
			TrophyUtil.Unlock_At_BuildShip(_rewardShip.Ship.MstId);
			_rewardShip = null;
			TaskMainArsenalManager.IsControl = true;
			TaskMainArsenalManager.isTouchEnable = true;
			ArsenalTaskManager._clsArsenal.hideDialog();
			_dockMiniMamager.StopConstAnimation();
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			ArsenalTaskManager._clsArsenal.setTutorialVisible(isVisible: true);
		}

		public bool IsShipGetView()
		{
			return (_rewardShip != null) ? true : false;
		}
	}
}
