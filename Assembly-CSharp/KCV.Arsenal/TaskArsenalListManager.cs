using Common.Enum;
using Common.Struct;
using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Arsenal
{
	public class TaskArsenalListManager : SceneTaskMono
	{
		public enum State
		{
			NONE,
			ShipSelect,
			ShipDestroyConfirm,
			ShipDestroying,
			SlotItemSelect,
			SlotItemDestroyConfirm,
			SlotItemDestroying
		}

		[SerializeField]
		private UIPanel _bgPanel;

		[SerializeField]
		private GameObject[] _materialObj;

		[SerializeField]
		private UIButton _dismantleBtn;

		[SerializeField]
		private ButtonLightTexture _dismantleBtnLight;

		[SerializeField]
		private UITexture _shipBanner;

		[SerializeField]
		private UITexture _slotItemTex;

		[SerializeField]
		private ArsenalScrollListNew ShipScroll;

		[SerializeField]
		private ArsenalScrollItemListNew ItemScroll;

		[SerializeField]
		private Camera mCamera_TouchEventCatch;

		[SerializeField]
		private Transform mTransform_OverlayButtonForConfirm;

		[SerializeField]
		private UiBreakAnimation _breakMaterialManager;

		[SerializeField]
		private UIShipSortButton mUIShipSortButton;

		private StateManager<State> mStateManager;

		private ArsenalManager mArsenalManager;

		private SortKey mSortKey;

		private KeyControl KeyController;

		[SerializeField]
		private Transform _uiOverlay3;

		[SerializeField]
		private Transform _uiOverlay4;

		public bool _ShikakuON;

		protected override void Awake()
		{
			base.Awake();
			mStateManager = new StateManager<State>(State.NONE);
			mStateManager.OnPush = OnPushState;
			mStateManager.OnPop = OnPopState;
			mStateManager.OnResume = OnResumeState;
			ShipScroll.SetOnSelectedListener(OnSelectedShipListener);
			ItemScroll.SetOnSelectedListener(OnSelectedSlotItemListener);
			mUIShipSortButton.SetCheckClicableDelegate(CheckClicableDelegate);
			ShipScroll.SetCamera(mCamera_TouchEventCatch);
			_ShikakuON = false;
		}

		private bool CheckClicableDelegate()
		{
			if (ShipScroll.GetCurrentState() == UIScrollList<ShipModel, ArsenalScrollListChildNew>.ListState.Waiting)
			{
				return true;
			}
			return false;
		}

		private void OnPushState(State state)
		{
			switch (state)
			{
			case State.ShipSelect:
				OnPushStateShipSelect();
				break;
			case State.ShipDestroyConfirm:
				OnPushStateShipDestroyConfirm();
				break;
			case State.ShipDestroying:
				OnPushStateShipDestroying();
				break;
			case State.SlotItemSelect:
				OnPushStateSlotItemSelect();
				break;
			case State.SlotItemDestroyConfirm:
				OnPushStateSlotItemDestroyConfirm();
				break;
			case State.SlotItemDestroying:
				OnPushStateSlotItemDestroying();
				break;
			}
		}

		private void OnPushStateSlotItemDestroyConfirm()
		{
			_uiOverlay4.localPositionX(-344f);
			_dismantleBtn.normalSprite = "btn_haiki_on";
			_dismantleBtnLight.PlayAnim();
			_ShikakuON = true;
		}

		private void OnPushStateShipDestroying()
		{
			_breakMaterialManager.startAnimation();
		}

		private void OnPushStateSlotItemDestroying()
		{
			_breakMaterialManager.startItemAnimation();
		}

		private void OnPushStateShipDestroyConfirm()
		{
			_uiOverlay4.localPositionX(-344f);
			_dismantleBtn.normalSprite = "btn_kaitai_on";
			_dismantleBtnLight.PlayAnim();
			_ShikakuON = true;
		}

		private void OnResumeStateShipSelect()
		{
			_uiOverlay4.localPositionX(614f);
			_dismantleBtn.normalSprite = "btn_kaitai";
			ShipScroll.ResumeControl();
		}

		private void OnResumeStateSlotItemSelect()
		{
			_uiOverlay4.localPositionX(614f);
			_dismantleBtn.normalSprite = "btn_haiki";
			ItemScroll.ResumeControl();
		}

		private void OnPushStateShipSelect()
		{
			_uiOverlay4.localPositionX(614f);
			_uiOverlay3.transform.localScale = Vector3.one;
			ShipScroll.SetActive(isActive: true);
			ItemScroll.SetActive(isActive: false);
			_dismantleBtn.normalSprite = "btn_kaitai";
			KeyController.ClearKeyAll();
			ShipScroll.SetKeyController(KeyController);
			ShipScroll.StartControl();
			TweenPosition tweenPosition = TweenPosition.Begin(_bgPanel.gameObject, 0.3f, Vector3.zero);
			tweenPosition.animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		}

		private void OnPushStateSlotItemSelect()
		{
			_uiOverlay4.localPositionX(614f);
			_uiOverlay3.transform.localScale = Vector3.one;
			ShipScroll.SetActive(isActive: false);
			ItemScroll.SetActive(isActive: true);
			_dismantleBtn.normalSprite = "btn_haiki";
			KeyController.ClearKeyAll();
			ItemScroll.SetKeyController(KeyController);
			ItemScroll.StartControl();
			TweenPosition tweenPosition = TweenPosition.Begin(_bgPanel.gameObject, 0.3f, Vector3.zero);
			tweenPosition.animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		}

		private void OnResumeState(State state)
		{
			switch (state)
			{
			case State.ShipDestroyConfirm:
			case State.ShipDestroying:
				break;
			case State.ShipSelect:
				OnResumeStateShipSelect();
				break;
			case State.SlotItemSelect:
				OnResumeStateSlotItemSelect();
				break;
			}
		}

		private void OnPopState(State state)
		{
			switch (state)
			{
			case State.ShipDestroying:
			case State.SlotItemSelect:
				break;
			case State.ShipDestroyConfirm:
				OnPopStateShipDestroyConfirm();
				break;
			case State.SlotItemDestroyConfirm:
				OnPopStateSlotItemDestroyConfirm();
				break;
			case State.ShipSelect:
				ShipScroll.ClearSelected();
				break;
			}
		}

		private void OnPopStateShipDestroyConfirm()
		{
			_ShikakuON = false;
			_dismantleBtnLight.StopAnim();
		}

		private void OnPopStateSlotItemDestroyConfirm()
		{
			_ShikakuON = false;
			_dismantleBtnLight.StopAnim();
		}

		private void OnSelectedShipListener(ArsenalScrollListChildNew selectedView)
		{
			ShipModel model = selectedView.GetModel();
			UpdateKaitaiInfo(model);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		private void UpdateKaitaiInfo(ShipModel shipModel)
		{
			UpdateKaitaiShipInfo(shipModel);
			UpdateKaitaiMaterialsInfo(shipModel);
		}

		private void UpdateKaitaiShipInfo(ShipModel shipModel)
		{
			if (shipModel == null)
			{
				_shipBanner.alpha = 0f;
				return;
			}
			int texNum = (!shipModel.IsDamaged()) ? 1 : 2;
			_shipBanner.alpha = 1f;
			_shipBanner.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(shipModel.MstId, texNum);
			_shipBanner.MakePixelPerfect();
			float num = _shipBanner.mainTexture.width;
			float num2 = 256f / num;
			_shipBanner.transform.localScaleX(num2);
			_shipBanner.transform.localScaleY(num2);
		}

		private void UpdateKaitaiMaterialsInfo(ShipModel setShip)
		{
			if (setShip == null)
			{
				for (int i = 0; i < 4; i++)
				{
					_materialObj[i].SetActive(false);
				}
				return;
			}
			int[] array = new int[4];
			for (int j = 0; j < 4; j++)
			{
				array[j] = 0;
			}
			MaterialInfo resourcesForDestroy = setShip.GetResourcesForDestroy();
			array[0] = resourcesForDestroy.Fuel;
			array[1] = resourcesForDestroy.Ammo;
			array[2] = resourcesForDestroy.Steel;
			array[3] = resourcesForDestroy.Baux;
			for (int k = 0; k < 4; k++)
			{
				if (array[k] > 0)
				{
					_materialObj[k].SetActive(true);
					UILabel component = ((Component)_materialObj[k].transform.FindChild("LabelMaterial")).GetComponent<UILabel>();
					component.textInt = array[k];
				}
				else
				{
					_materialObj[k].SetActive(false);
				}
			}
		}

		public void firstInit()
		{
			KeyController = new KeyControl();
			_shipBanner.alpha = 0f;
			_slotItemTex.alpha = 0f;
			mArsenalManager = ArsenalTaskManager.GetLogicManager();
			_breakMaterialManager.init();
			Close();
		}

		protected override bool Run()
		{
			if (KeyController != null)
			{
				KeyController.Update();
				switch (mStateManager.CurrentState)
				{
				case State.ShipSelect:
					OnUpdateShipSelect();
					break;
				case State.ShipDestroyConfirm:
					OnUpdateShipDestroyConfirm();
					break;
				case State.SlotItemSelect:
					OnUpdateSlotItemSelect();
					break;
				case State.SlotItemDestroyConfirm:
					OnUpdateSlotItemDestroyConfirm();
					break;
				}
			}
			return true;
		}

		private void OnUpdateSlotItemDestroyConfirm()
		{
			if (KeyController.IsLeftDown())
			{
				RequestBackTransitionFromSlotItemDestroyConfirm();
			}
			else if (KeyController.IsMaruDown())
			{
				CommonPopupDialog.Instance.StartPopup("廃棄は □ボタンで行います");
			}
			else if (KeyController.IsBatuDown())
			{
				RequestBackTransitionFromShipDestroyConfirm();
			}
			else if (KeyController.IsShikakuDown() && mStateManager.CurrentState == State.SlotItemDestroyConfirm)
			{
				StartHaiki(mArsenalManager);
			}
		}

		private void OnUpdateSlotItemSelect()
		{
			if (KeyController.IsRightDown())
			{
				RequestTransitionForSlotItemDestroyConfirm();
			}
			else if (KeyController.IsBatuDown())
			{
				RequestBackTransitionFromSlotItemSelect();
			}
		}

		private void RequestTransitionForSlotItemDestroyConfirm()
		{
			if (0 < mArsenalManager.GetSelectedItemsForDetroy().Count)
			{
				ItemScroll.LockControl();
				mStateManager.PushState(State.SlotItemDestroyConfirm);
			}
		}

		private void OnUpdateShipSelect()
		{
			if (KeyController.IsRightDown())
			{
				RequestTransitionForShipDestroyConfirm();
			}
			else if (KeyController.IsBatuDown())
			{
				RequestBackTransitionFromShipSelect();
			}
		}

		private void RequestBackTransitionFromShipSelect()
		{
			if (mStateManager.CurrentState == State.ShipSelect)
			{
				UpdateKaitaiInfo(null);
				mStateManager.PopState();
				ShipScroll.LockControl();
				Hide();
				Close();
				ArsenalTaskManager._clsArsenal.hideDialog();
				ArsenalTaskManager.ReqPhase(ArsenalTaskManager.ArsenalPhase.BattlePhase_ST);
			}
		}

		private void RequestBackTransitionFromSlotItemSelect()
		{
			if (mStateManager.CurrentState == State.SlotItemSelect)
			{
				mArsenalManager.ClearSelectedState();
				UpdateHaikiInfo();
				mStateManager.PopState();
				ItemScroll.LockControl();
				Hide();
				Close();
				ArsenalTaskManager._clsArsenal.hideDialog();
				ArsenalTaskManager.ReqPhase(ArsenalTaskManager.ArsenalPhase.BattlePhase_ST);
			}
		}

		private void RequestTransitionForShipDestroyConfirm()
		{
			if (ShipScroll.SelectedShip != null)
			{
				ShipScroll.LockControl();
				mStateManager.PushState(State.ShipDestroyConfirm);
			}
		}

		private void OnUpdateShipDestroyConfirm()
		{
			if (KeyController.IsLeftDown())
			{
				mStateManager.PopState();
				mStateManager.ResumeState();
			}
			else if (!KeyController.IsRightDown())
			{
				if (KeyController.IsMaruDown())
				{
					CommonPopupDialog.Instance.StartPopup("解体は □ボタンで行います");
				}
				else if (KeyController.IsBatuDown())
				{
					RequestBackTransitionFromShipDestroyConfirm();
				}
				else if (KeyController.IsShikakuDown() && mStateManager.CurrentState == State.ShipDestroyConfirm)
				{
					ShipModel selectedShip = ShipScroll.SelectedShip;
					StartKaitai(selectedShip);
				}
			}
		}

		private void OnTouchStartKaitai()
		{
			if (mStateManager.CurrentState == State.ShipDestroyConfirm)
			{
				ShipModel selectedShip = ShipScroll.SelectedShip;
				StartKaitai(selectedShip);
			}
		}

		[Obsolete("Inspector上で設定して使用したい....")]
		public void OnTouchStartHaiki()
		{
			if (mStateManager.CurrentState == State.SlotItemDestroyConfirm && 0 < mArsenalManager.GetSelectedItemsForDetroy().Count)
			{
				StartHaiki(mArsenalManager);
			}
		}

		[Obsolete("Inspector上で設定して使用します。")]
		public void OnTouchDestroyButton()
		{
			if (mStateManager.CurrentState == State.ShipDestroyConfirm)
			{
				OnTouchStartKaitai();
			}
			else if (mStateManager.CurrentState == State.SlotItemDestroyConfirm)
			{
				OnTouchStartHaiki();
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchOverlay4()
		{
			if (mStateManager.CurrentState == State.ShipSelect)
			{
				RequestTransitionForShipDestroyConfirm();
			}
			else if (mStateManager.CurrentState == State.SlotItemSelect)
			{
				RequestTransitionForSlotItemDestroyConfirm();
			}
			else if (mStateManager.CurrentState == State.ShipDestroyConfirm)
			{
				RequestBackTransitionFromShipDestroyConfirm();
			}
			else if (mStateManager.CurrentState == State.SlotItemDestroyConfirm)
			{
				RequestBackTransitionFromSlotItemDestroyConfirm();
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchOverlay3()
		{
			if (mStateManager.CurrentState == State.ShipSelect)
			{
				RequestBackTransitionFromShipSelect();
			}
			else if (mStateManager.CurrentState == State.SlotItemSelect)
			{
				RequestBackTransitionFromSlotItemSelect();
			}
		}

		private void RequestBackTransitionFromShipDestroyConfirm()
		{
			mStateManager.PopState();
			mStateManager.ResumeState();
		}

		private void RequestBackTransitionFromSlotItemDestroyConfirm()
		{
			mStateManager.PopState();
			mStateManager.ResumeState();
		}

		private void StartKaitai(ShipModel shipModel)
		{
			if (mArsenalManager.IsValidBreakShip(shipModel))
			{
				if (mArsenalManager.BreakShip(shipModel.MemId))
				{
					ShipScroll.LockControl();
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					mStateManager.PushState(State.ShipDestroying);
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup("艦がロックされています");
				}
			}
		}

		private void StartHaiki(ArsenalManager arsenalManager)
		{
			if (mArsenalManager.IsValidBreakItem())
			{
				if (mArsenalManager.BreakItem())
				{
					ItemScroll.LockControl();
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					mStateManager.PushState(State.SlotItemDestroying);
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup("ロックされている装備があります");
				}
			}
		}

		private void OnSelectedSlotItemListener(ArsenalScrollItemListChildNew selectedView)
		{
			mArsenalManager.GetSelectedItemsForDetroy();
			int memId = selectedView.GetModel().GetSlotItemModel().MemId;
			mArsenalManager.ToggleSelectedState(memId);
			ItemScroll.UpdateChoiceModelAndView(selectedView.GetRealIndex(), selectedView.GetModel().GetSlotItemModel());
			UpdateHaikiInfo();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		private void UpdateHaikiInfo()
		{
			int[] array = new int[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = 0;
			}
			MaterialInfo materialsForBreakItem = mArsenalManager.GetMaterialsForBreakItem();
			array[0] = materialsForBreakItem.Fuel;
			array[1] = materialsForBreakItem.Ammo;
			array[2] = materialsForBreakItem.Steel;
			array[3] = materialsForBreakItem.Baux;
			for (int j = 0; j < 4; j++)
			{
				if (array[j] > 0)
				{
					_materialObj[j].SetActive(true);
					UILabel component = ((Component)_materialObj[j].transform.FindChild("LabelMaterial")).GetComponent<UILabel>();
					component.SetActive(isActive: true);
					component.textInt = array[j];
				}
				else
				{
					_materialObj[j].SetActive(false);
				}
			}
			List<SlotitemModel> selectedItemsForDetroy = mArsenalManager.GetSelectedItemsForDetroy();
			switch (selectedItemsForDetroy.Count)
			{
			case 0:
				_slotItemTex.alpha = 0f;
				break;
			case 1:
			{
				SlotitemModel slotitemModel = selectedItemsForDetroy[0];
				UnloadSlotItemTex();
				_slotItemTex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(slotitemModel.MstId, 2);
				_slotItemTex.width = 144;
				_slotItemTex.height = 215;
				_slotItemTex.alpha = 1f;
				break;
			}
			default:
				_slotItemTex.mainTexture = (Resources.Load("Textures/Arsenal/kaitai_haiki/icon_haiki") as Texture2D);
				_slotItemTex.width = 162;
				_slotItemTex.height = 102;
				_slotItemTex.alpha = 1f;
				break;
			}
		}

		private void UnloadSlotItemTex()
		{
			if (_slotItemTex != null)
			{
				if (_slotItemTex.mainTexture != null)
				{
					Resources.UnloadAsset(_slotItemTex.mainTexture);
				}
				_slotItemTex.mainTexture = null;
			}
		}

		public void CloseShipDialog()
		{
			_shipBanner.alpha = 0f;
			_dismantleBtn.normalSprite = "btn_kaitai";
		}

		private void CloseHaikiDialog()
		{
			_slotItemTex.alpha = 0f;
			_dismantleBtn.normalSprite = "btn_haiki";
		}

		private void HideMaterialsInfo()
		{
			for (int i = 0; i < 4; i++)
			{
				_materialObj[i].SetActive(false);
			}
		}

		public void StartStateKaitai()
		{
			mUIShipSortButton.SetActive(isActive: true);
			mUIShipSortButton.SetClickable(clickable: true);
			KeyController.ClearKeyAll();
			KeyController.firstUpdate = true;
			mStateManager.ClearStates();
			mArsenalManager.ClearSelectedState();
			mStateManager.PushState(State.ShipSelect);
		}

		public void StartStateKaitaiAtFirst()
		{
			ShipModel[] shipList = mArsenalManager.GetShipList();
			ShipScroll.Initialize(shipList);
			StartStateKaitai();
		}

		public void StartStateHaiki()
		{
			ItemScroll.ClearChecked();
			mUIShipSortButton.SetActive(isActive: false);
			mUIShipSortButton.SetClickable(clickable: false);
			KeyController.ClearKeyAll();
			KeyController.firstUpdate = true;
			mStateManager.ClearStates();
			mArsenalManager.ClearSelectedState();
			mStateManager.PushState(State.SlotItemSelect);
		}

		internal void StartStateHaikiAtFirst()
		{
			SlotitemModel[] unsetSlotitems = mArsenalManager.GetUnsetSlotitems();
			ItemScroll.Initialize(mArsenalManager, unsetSlotitems, mCamera_TouchEventCatch);
			mUIShipSortButton.SetActive(isActive: false);
			mUIShipSortButton.SetClickable(clickable: false);
			KeyController.ClearKeyAll();
			KeyController.firstUpdate = true;
			mStateManager.ClearStates();
			mArsenalManager.ClearSelectedState();
			mStateManager.PushState(State.SlotItemSelect);
		}

		public void Hide()
		{
			_uiOverlay3.transform.localScale = Vector3.zero;
			TweenPosition tweenPosition = TweenPosition.Begin(_bgPanel.gameObject, 0.3f, Vector3.right * 877f);
			tweenPosition.animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			_dismantleBtnLight.StopAnim();
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
		}

		public void compBreakAnimation()
		{
			if (mStateManager.CurrentState == State.ShipDestroying)
			{
				OnCompleteShipBreakAnimation();
			}
			else if (mStateManager.CurrentState == State.SlotItemDestroying)
			{
				OnCompleteSlotItemBreakAnimation();
			}
		}

		private void OnCompleteSlotItemBreakAnimation()
		{
			if (mStateManager.CurrentState == State.SlotItemDestroying)
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(mArsenalManager);
				SlotitemModel[] unsetSlotitems = mArsenalManager.GetUnsetSlotitems();
				UpdateHaikiInfo();
				ItemScroll.Refresh(unsetSlotitems);
				ItemScroll.SetKeyController(KeyController);
				ItemScroll.StartControl();
				CloseHaikiDialog();
				mStateManager.PopState();
				if (mStateManager.CurrentState == State.SlotItemDestroyConfirm)
				{
					mStateManager.PopState();
				}
				if (mStateManager.CurrentState == State.SlotItemSelect)
				{
					mStateManager.ResumeState();
				}
				TrophyUtil.Unlock_Material();
			}
		}

		private void OnCompleteShipBreakAnimation()
		{
			if (mStateManager.CurrentState == State.ShipDestroying)
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(mArsenalManager);
				ShipModel[] shipList = mArsenalManager.GetShipList();
				UpdateKaitaiInfo(null);
				ShipScroll.GetFocusModelIndex();
				ShipScroll.Refresh(shipList);
				ShipScroll.SetKeyController(KeyController);
				ShipScroll.StartControl();
				CloseShipDialog();
				mStateManager.PopState();
				if (mStateManager.CurrentState == State.ShipDestroyConfirm)
				{
					mStateManager.PopState();
				}
				if (mStateManager.CurrentState == State.ShipSelect)
				{
					mStateManager.ResumeState();
				}
				TrophyUtil.Unlock_Material();
			}
		}

		private void OnDestroy()
		{
			_bgPanel = null;
			_materialObj = null;
			_dismantleBtn = null;
			_dismantleBtnLight = null;
			_shipBanner = null;
			_slotItemTex = null;
			ShipScroll = null;
			ItemScroll = null;
			mCamera_TouchEventCatch = null;
			mTransform_OverlayButtonForConfirm = null;
			_breakMaterialManager = null;
			mUIShipSortButton = null;
			mStateManager = null;
			mArsenalManager = null;
			KeyController = null;
			_uiOverlay3 = null;
			_uiOverlay4 = null;
		}
	}
}
