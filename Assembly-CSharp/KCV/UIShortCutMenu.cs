using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV
{
	public class UIShortCutMenu : SingletonMonoBehaviour<UIShortCutMenu>
	{
		private class ButtonInfos
		{
			private UIButton _uiBtn;

			private Generics.Scene _iScene;

			public UIButton Button => _uiBtn;

			public Generics.Scene Scene => _iScene;

			public ButtonInfos(UIButton btn, Generics.Scene iScene)
			{
				_uiBtn = btn;
				_iScene = iScene;
			}

			public void SetDisable()
			{
				_uiBtn.SetState(UIButtonColor.State.Disabled, immediate: false);
			}
		}

		[Serializable]
		private class PECamera : Generics.InnerCamera
		{
			private UIPanel _uiPanel;

			private UIButton _uiOverlay;

			private Blur _peBlur;

			public bool IsEffects
			{
				get;
				private set;
			}

			public PECamera(Transform parent, string objName)
				: base(parent, objName)
			{
				IsEffects = false;
				Util.FindParentToChild(ref _uiPanel, _camCamera.transform, "Panel");
				Util.FindParentToChild(ref _uiOverlay, _uiPanel.transform, "Overlay");
				_uiOverlay.isEnabled = false;
				_camCamera.cullingMask = -1;
				_peBlur = _camCamera.GetComponent<Blur>();
				_peBlur.downsample = 1;
				_peBlur.blurSize = 0f;
				_peBlur.blurIterations = 1;
				_peBlur.blurType = Blur.BlurType.StandardGauss;
				_peBlur.enabled = false;
			}

			public void EnabledEffects(bool isEnabled)
			{
				IsEffects = isEnabled;
				_peBlur.enabled = isEnabled;
			}

			public void EnabledOverlay(bool isEnabled)
			{
				_uiOverlay.isEnabled = isEnabled;
			}

			public void SetBlurSize(float blurSize)
			{
				_peBlur.blurSize = blurSize;
			}

			public void LockTouchControl(bool isEnable)
			{
				if (!SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsFocus || isEnable)
				{
					_camCamera.SetActive(isEnable);
					_uiPanel.SetActive(isEnable);
					_uiOverlay.SetActive(isEnable);
					_uiOverlay.GetComponent<BoxCollider2D>().enabled = isEnable;
				}
			}
		}

		private Generics.InnerCamera _camERCamera;

		private PECamera _camPECamera;

		private UIPanel _uiBtnsPanel;

		private Dictionary<int, ButtonInfos> _dicBtns;

		[SerializeField]
		private UIShortCutGears gears;

		[SerializeField]
		private UIShortCutTruss truss;

		private bool _isFocus;

		private bool _isInputEnable;

		private bool _isTweenPos;

		private float _fPanelPosXfmCenter;

		private Action _actOperationEnabled;

		private Action _actoperationDisabled;

		private KeyControl _clsInput;

		public bool IsOpen;

		[SerializeField]
		private UIShortCutButtonManager ShortCutBtnManager;

		public bool isCloseAnimNow;

		public bool IsInputEnable
		{
			get
			{
				return _isInputEnable;
			}
			set
			{
				_isInputEnable = value;
			}
		}

		public bool IsFocus => _isFocus;

		public List<int> disableButtonList
		{
			get;
			private set;
		}

		protected override void Awake()
		{
			_isFocus = false;
			_isInputEnable = true;
			_isTweenPos = false;
			_camERCamera = new Generics.InnerCamera(base.transform, "ERCamera");
			_camPECamera = new PECamera(base.transform, "PECamera");
			_uiBtnsPanel = ((Component)base.transform.FindChild("ERCamera/SidePanel")).GetComponent<UIPanel>();
			Transform transform = base.transform.FindChild("ERCamera/SidePanel/Btns").transform;
			_dicBtns = new Dictionary<int, ButtonInfos>();
			_dicBtns.Add(0, new ButtonInfos(((Component)transform.FindChild("StrategyBtn")).GetComponent<UIButton>(), Generics.Scene.Strategy));
			_dicBtns.Add(1, new ButtonInfos(((Component)transform.FindChild("PortTopBtn")).GetComponent<UIButton>(), Generics.Scene.PortTop));
			_dicBtns.Add(2, new ButtonInfos(((Component)transform.FindChild("OrganizeBtn")).GetComponent<UIButton>(), Generics.Scene.Organize));
			_dicBtns.Add(3, new ButtonInfos(((Component)transform.FindChild("SupplyBtn")).GetComponent<UIButton>(), Generics.Scene.Supply));
			_dicBtns.Add(4, new ButtonInfos(((Component)transform.FindChild("RepairBtn")).GetComponent<UIButton>(), Generics.Scene.Repair));
			_dicBtns.Add(5, new ButtonInfos(((Component)transform.FindChild("RemodelBtn")).GetComponent<UIButton>(), Generics.Scene.Remodel));
			_dicBtns.Add(6, new ButtonInfos(((Component)transform.FindChild("ArsenalBtn")).GetComponent<UIButton>(), Generics.Scene.Arsenal));
			_dicBtns.Add(7, new ButtonInfos(((Component)transform.FindChild("RevampBtn")).GetComponent<UIButton>(), Generics.Scene.ImprovementArsenal));
			_dicBtns.Add(8, new ButtonInfos(((Component)transform.FindChild("MissionBtn")).GetComponent<UIButton>(), Generics.Scene.Duty));
			_dicBtns.Add(9, new ButtonInfos(((Component)transform.FindChild("ItemBtn")).GetComponent<UIButton>(), Generics.Scene.Item));
			_dicBtns.Add(10, new ButtonInfos(((Component)transform.FindChild("SaveBtn")).GetComponent<UIButton>(), Generics.Scene.SaveLoad));
			int num = 0;
			foreach (KeyValuePair<int, ButtonInfos> dicBtn in _dicBtns)
			{
				dicBtn.Value.Button.onClick = Util.CreateEventDelegateList(this, "_decideButton", num);
				num++;
			}
			Vector3 localPosition = _uiBtnsPanel.transform.localPosition;
			_fPanelPosXfmCenter = localPosition.x;
			_clsInput = new KeyControl();
			_clsInput.setChangeValue(0f, 0f, 0f, 0f);
			disableButtonList = new List<int>();
		}

		private void Start()
		{
			this.SetActiveChildren(isActive: false);
		}

		private void Update()
		{
			if (!_isInputEnable || _clsInput == null)
			{
				return;
			}
			_clsInput.Update();
			if (_clsInput.keyState[4].down)
			{
				if (IsFocus)
				{
					CloseMenu();
					LockOffControl();
				}
				else if (!isCloseAnimNow)
				{
					OpenMenu();
				}
			}
			else if (_isFocus && _camPECamera.IsEffects)
			{
				if (_clsInput.IsDownDown())
				{
					ShortCutBtnManager.setSelectedBtn(isDownKey: true);
				}
				else if (_clsInput.IsUpDown())
				{
					ShortCutBtnManager.setSelectedBtn(isDownKey: false);
				}
				else if (_clsInput.keyState[0].down)
				{
					OnCancel();
				}
				else if (_clsInput.keyState[1].down)
				{
					_decideButton(ShortCutBtnManager.ButtonManager.nowForcusIndex);
					_clsInput.ClearKeyAll();
					_clsInput.firstUpdate = true;
				}
				else if (_clsInput.keyState[5].down)
				{
					LockOffControl();
					CloseMenu();
				}
			}
		}

		public void OpenMenu()
		{
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState != 0)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
				CommonPopupDialog.Instance.StartPopup("艦隊は遠征中です");
			}
			else if (IsInputEnable)
			{
				this.SetActiveChildren(isActive: true);
				if (_actOperationEnabled != null)
				{
					_actOperationEnabled();
				}
				_clsInput.Index = 0;
				_camPECamera.EnabledEffects(isEnabled: true);
				_camPECamera.EnabledOverlay(isEnabled: true);
				_setTweenPos(isOpen: true);
				_isFocus = true;
				App.OnlyController = _clsInput;
				App.OnlyController.firstUpdate = true;
				ShortCutBtnManager.ButtonManager.setAllButtonActive();
				setDisable();
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.getButton(Generics.Scene.PortTop.ToString()).SetActive(isActive: false);
				ShortCutBtnManager.HideNowScene();
				ShortCutBtnManager.ButtonManager.isDisableFocus = false;
				ShortCutBtnManager.ButtonManager.setFocus(0);
				ShortCutBtnManager.ButtonManager.isDisableFocus = true;
				ShortCutBtnManager.ChangeCursolPos();
				IsOpen = true;
				if (SingletonMonoBehaviour<PortObjectManager>.exist() && SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
				}
				SoundUtils.PlaySE(SEFIleInfos.SE_037);
			}
		}

		public void CloseMenu()
		{
			if (IsInputEnable)
			{
				if (_actoperationDisabled != null)
				{
					_actoperationDisabled();
				}
				_isFocus = false;
				_camPECamera.EnabledEffects(isEnabled: false);
				_setTweenPos(isOpen: false);
				if (SingletonMonoBehaviour<PortObjectManager>.exist() && SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Show();
				}
				IsOpen = false;
				SoundUtils.PlaySE(SEFIleInfos.SE_037);
			}
		}

		public void OnCancel()
		{
			LockOffControl();
			CloseMenu();
		}

		public void LockOffControl()
		{
			if (!IsFocus)
			{
				_camPECamera.EnabledOverlay(isEnabled: false);
			}
			App.OnlyController = null;
			App.isFirstUpdate = true;
		}

		public void LockTouchControl(bool isEnable)
		{
			_camPECamera.LockTouchControl(isEnable);
		}

		private void _setTweenPos(bool isOpen)
		{
			float x;
			if (isOpen)
			{
				x = _fPanelPosXfmCenter;
				TweenAlpha.Begin(_uiBtnsPanel.gameObject, 0.4f, 1f);
				gears.Enter();
				truss.Enter();
			}
			else
			{
				Vector3 localPosition = _uiBtnsPanel.transform.localPosition;
				x = localPosition.x;
				float fPanelPosXfmCenter = _fPanelPosXfmCenter;
				TweenAlpha.Begin(_uiBtnsPanel.gameObject, 0.4f, 0f);
				gears.Exit();
				truss.Exit();
			}
			_uiBtnsPanel.transform.localPositionX(x);
			if (isOpen)
			{
				_uiBtnsPanel.GetComponent<TweenPosition>().PlayForward();
			}
			else
			{
				_uiBtnsPanel.GetComponent<TweenPosition>().PlayReverse();
			}
			Hashtable hashtable = new Hashtable();
			float num = (!isOpen) ? 2f : 0f;
			float num2 = (!isOpen) ? 0f : 2f;
			hashtable.Clear();
			hashtable.Add("from", num);
			hashtable.Add("to", num2);
			hashtable.Add("time", 0.4f);
			hashtable.Add("easeType", iTween.EaseType.easeOutExpo);
			hashtable.Add("onupdate", "_setTweenBlurSize");
			iTween.ValueTo(base.gameObject, hashtable);
			hashtable.Clear();
			_isTweenPos = true;
		}

		private void setNowSceneButtonDisable()
		{
			for (int i = 0; i < _dicBtns.Values.Count; i++)
			{
				if (_dicBtns[i].Scene.ToString() == SingletonMonoBehaviour<PortObjectManager>.instance.NowScene && !disableButtonList.Contains(i))
				{
					disableButtonList.Add(i);
				}
			}
		}

		public void setDisable()
		{
			disableButtonList.Clear();
			int currentAreaID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
			DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			if (currentDeck == null)
			{
				return;
			}
			bool flag = (currentDeck.Count <= 0) ? true : false;
			if (!SingletonMonoBehaviour<AppInformation>.Instance.IsValidMoveToScene(Generics.Scene.Arsenal))
			{
				addDisableButton(6);
				addDisableButton(7);
			}
			if (flag || currentDeck.HasBling())
			{
				setDisableExceptStrategy();
			}
			setDisableWithDock();
			if (SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel != null && SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel.ShipType != 19)
			{
				getButton(Generics.Scene.ImprovementArsenal.ToString()).SetActive(isActive: false);
			}
			else
			{
				getButton(Generics.Scene.Arsenal.ToString()).SetActive(isActive: false);
				if (SingletonMonoBehaviour<PortObjectManager>.Instance.NowScene.ToLower() == Generics.Scene.Arsenal.ToString().ToLower())
				{
					getButton(Generics.Scene.ImprovementArsenal.ToString()).SetActive(isActive: false);
				}
			}
			setNowSceneButtonDisable();
			ShortCutBtnManager.setDisableButton(disableButtonList);
		}

		private void setDisableExceptStrategy()
		{
			for (int i = 1; i < _dicBtns.Count; i++)
			{
				if (i != 2 && i != 10)
				{
					addDisableButton(i);
				}
			}
		}

		private void setDisableWithDock()
		{
			if (!SingletonMonoBehaviour<AppInformation>.Instance.IsValidMoveToScene(Generics.Scene.Repair))
			{
				addDisableButton(4);
			}
		}

		private void addDisableButton(int index)
		{
			if (!disableButtonList.Contains(index))
			{
				disableButtonList.Add(index);
			}
		}

		public UIButton getButton(string sceneName)
		{
			KeyValuePair<int, ButtonInfos> keyValuePair = _dicBtns.FirstOrDefault((KeyValuePair<int, ButtonInfos> x) => x.Value.Scene.ToString().ToLower() == sceneName.ToLower());
			if (keyValuePair.Value != null)
			{
				return keyValuePair.Value.Button;
			}
			return null;
		}

		private void _decideButton(int nIndex)
		{
			if (ShortCutBtnManager.ButtonManager.nowForcusButton.disabledSprite == ShortCutBtnManager.ButtonManager.nowForcusButton.hoverSprite)
			{
				string mes = string.Empty;
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Count == 0)
				{
					CommonPopupDialog.Instance.StartPopup("艦隊を編成する必要があります");
					return;
				}
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.HasBling())
				{
					CommonPopupDialog.Instance.StartPopup("撤退中の艦が含まれています");
					return;
				}
				switch (_dicBtns[nIndex].Scene)
				{
				case Generics.Scene.Arsenal:
					mes = "鎮守府海域でのみ選択可能です";
					break;
				case Generics.Scene.ImprovementArsenal:
					mes = ((SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID == 1) ? "旗艦が工作艦である必要があります" : "鎮守府海域でのみ選択可能です");
					break;
				case Generics.Scene.Repair:
					mes = "この海域には入渠ドックがありません";
					break;
				}
				CommonPopupDialog.Instance.StartPopup(mes);
				return;
			}
			isCloseAnimNow = true;
			if (PortObjectManager.isPrefabSecene(_dicBtns[nIndex].Scene) && !SingletonMonoBehaviour<PortObjectManager>.Instance.isLoadSecene())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(_dicBtns[nIndex].Scene);
				return;
			}
			if (_dicBtns[nIndex].Scene == Generics.Scene.SaveLoad)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("rootType", Generics.Scene.Strategy);
				RetentionData.SetData(hashtable);
			}
			SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(_dicBtns[nIndex].Scene);
			CloseMenu();
		}

		public void SetJoyStickOperation(bool isEnabled, Action operationEnabled, Action operationDisabled)
		{
			_isInputEnable = isEnabled;
			if (isEnabled)
			{
				if (_camPECamera.IsEffects)
				{
					_isFocus = false;
					_camPECamera.EnabledEffects(isEnabled: false);
					_camPECamera.EnabledOverlay(isEnabled: false);
				}
				_actOperationEnabled = operationEnabled;
				_actoperationDisabled = operationDisabled;
			}
			else
			{
				_isFocus = false;
				_camPECamera.EnabledEffects(isEnabled: false);
				_camPECamera.EnabledOverlay(isEnabled: false);
				_actOperationEnabled = null;
				_actoperationDisabled = null;
			}
		}

		public void OperationDelegateUpdate(Action operationEnabled, Action operationDisabled)
		{
			if (_actOperationEnabled != operationEnabled)
			{
				_actOperationEnabled = operationEnabled;
			}
			if (_actoperationDisabled != operationDisabled)
			{
				_actoperationDisabled = operationDisabled;
			}
		}

		public void SetDepth(int nDepth)
		{
			_camPECamera.depth = nDepth;
			_camERCamera.depth = nDepth + 1;
		}

		private void _onTweenPosFinished()
		{
			_isTweenPos = false;
			_isFocus = true;
		}

		private void _setTweenBlurSize(float fVal)
		{
			_camPECamera.SetBlurSize(fVal);
		}

		private void OnDestroy()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance = null;
			_dicBtns.Clear();
			_dicBtns = null;
		}
	}
}
