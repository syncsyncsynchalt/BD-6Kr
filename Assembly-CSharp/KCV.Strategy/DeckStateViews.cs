using Common.Enum;
using DG.Tweening;
using KCV.Organize;
using KCV.PopupString;
using KCV.Strategy.Rebellion;
using KCV.Utils;
using local.managers;
using local.models;
using ModeProc;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class DeckStateViews : MonoBehaviour
	{
		private enum Mode
		{
			Top,
			SupplyConfirm,
			RepairConfirm,
			RepairKitConfirm,
			OrganizeDetail,
			OrganizeList,
			OrganizeListDetail
		}

		[SerializeField]
		private UIRebellionOrgaizeShipBanner[] ShipStates;

		private KeyControl key;

		private UIRebellionOrgaizeShipBanner FocusBanner;

		private SupplyManager SupplyMng;

		private RepairManager RepairMng;

		private OrganizeManager OrganizeMng;

		[SerializeField]
		private UILabel DeckNoLabel;

		[SerializeField]
		private UITexture DeckNoIcon;

		private ModeProcessor ModeProcessor;

		private KeyControl dialogKeyController;

		private ShipModel ListSelectShipModel;

		private DeckModel CurrentDeck;

		[SerializeField]
		private Camera DialogCamera;

		[SerializeField]
		private BoxCollider2D BackButtonCol;

		[SerializeField]
		private Transform DeckActionEnd;

		[SerializeField]
		private OrganizeScrollListParent ListParent;

		[SerializeField]
		private Animation BauxiSuccess;

		[SerializeField]
		private Animation BauxiField;

		[SerializeField]
		private GameObject Prefab_SupplyConfim;

		private StrategySupplyConfirm supplyConfirm;

		private RepairDockModel repairDockModel;

		[SerializeField]
		private GameObject Prefab_RepairConfim;

		private StrategyRepairConfirm repairConfim;

		[SerializeField]
		private GameObject Prefab_RepairKitConfim;

		private StrategyRepairKitConfirm repairKitConfim;

		[SerializeField]
		private Transform Prefab_OrganizeDetailMng;

		[SerializeField]
		private Transform Prefab_OrganizeList;

		private OrganizeDetail_Manager OrganizeDetailMng;

		private bool isInitialize;

		public void Start()
		{
			ListParent.SetActive(isActive: false);
		}

		public void Initialize(DeckModel deckModel, bool isCustomMode = false)
		{
			UpdateView(deckModel);
			CurrentDeck = deckModel;
			if (isCustomMode)
			{
				SetBannerColliderEnable(isEnable: true);
				SetCustomMode();
			}
			else
			{
				ListParent.SetActive(isActive: false);
				SetBannerColliderEnable(isEnable: false);
				((Component)BauxiSuccess).transform.parent.SetActive(isActive: false);
			}
		}

		private void SetBannerColliderEnable(bool isEnable)
		{
			UIRebellionOrgaizeShipBanner[] shipStates = ShipStates;
			foreach (UIRebellionOrgaizeShipBanner uIRebellionOrgaizeShipBanner in shipStates)
			{
				uIRebellionOrgaizeShipBanner.GetComponent<BoxCollider2D>().enabled = isEnable;
			}
		}

		private void Update()
		{
			if (key != null)
			{
				key.Update();
				ModeProcessor.ModeRun();
			}
		}

		private void UpdateView(DeckModel deckModel)
		{
			int num = 0;
			int count = deckModel.Count;
			UIRebellionOrgaizeShipBanner[] shipStates = ShipStates;
			foreach (UIRebellionOrgaizeShipBanner uIRebellionOrgaizeShipBanner in shipStates)
			{
				int nIndex = num + 1;
				uIRebellionOrgaizeShipBanner.SetShipData(deckModel.GetShip(num), nIndex);
				uIRebellionOrgaizeShipBanner.SetShipIndex(num);
				num++;
			}
			if (FocusBanner != null)
			{
				BannerFocusAnim(isEnable: false);
				FocusBanner = ShipStates[key.Index];
				BannerFocusAnim(isEnable: true);
			}
			DeckNoLabel.text = deckModel.Name;
			DeckNoLabel.supportEncoding = false;
			DeckNoIcon.mainTexture = (Resources.Load("Textures/Common/DeckFlag/icon_deck" + deckModel.Id) as Texture2D);
			if (deckModel.IsActionEnd())
			{
				DeckActionEnd.SetActive(isActive: true);
				DeckActionEnd.DOKill();
				DeckActionEnd.DOLocalRotate(new Vector3(0f, 0f, 300f), 0f, RotateMode.FastBeyond360);
				DeckActionEnd.DOLocalRotate(new Vector3(0f, 0f, 360f), 0.8f, RotateMode.FastBeyond360).SetEase(Ease.OutBounce);
			}
			else
			{
				DeckActionEnd.SetActive(isActive: false);
			}
		}

		public void SetCustomMode()
		{
			dialogKeyController = App.OnlyController;
			dialogKeyController.IsRun = false;
			((Component)BauxiSuccess).transform.localPositionX(1000f);
			((Component)BauxiField).transform.localPositionX(1000f);
			key = new KeyControl(0, 5);
			key.isLoopIndex = false;
			key.setChangeValue(-2f, 1f, 2f, -1f);
			App.OnlyController = key;
			((Component)BauxiSuccess).transform.parent.SetActive(isActive: true);
			ModeProcessor = GetComponent<ModeProcessor>();
			ModeProcessor.addMode("Top", TopModeRun, TopModeEnter, TopModeExit);
			ModeProcessor.addMode("SupplyConfirm", SupplyConfirmModeRun, SupplyConfirmModeEnter, SupplyConfirmModeExit);
			ModeProcessor.addMode("RepairConfirm", RepairConfirmModeRun, RepairConfirmModeEnter, RepairConfirmModeExit);
			ModeProcessor.addMode("RepairKitConfirm", RepairKitConfirmModeRun, RepairKitConfirmModeEnter, RepairKitConfirmModeExit);
			ModeProcessor.addMode("OrganizeDetail", OrganizeDetailModeRun, OrganizeDetailModeEnter, OrganizeDetailModeExit);
			ModeProcessor.addMode("OrganizeList", OrganizeListModeRun, OrganizeListModeEnter, OrganizeListModeExit);
			ModeProcessor.addMode("OrganizeListDetail", OrganizeListDetailModeRun, OrganizeListDetailModeEnter, OrganizeListDetailModeExit);
			FocusBanner = ShipStates[0];
			UIRebellionOrgaizeShipBanner[] shipStates = ShipStates;
			foreach (UIRebellionOrgaizeShipBanner uIRebellionOrgaizeShipBanner in shipStates)
			{
				uIRebellionOrgaizeShipBanner.SetOnClick(OnClickBanner);
			}
			BannerFocusAnim(isEnable: true);
			OrganizeMng = new OrganizeManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
			StrategyTopTaskManager.Instance.setActiveStrategy(isActive: false);
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			ListParent.SetBackButtonEnable(isEnable: false);
			StrategyTopTaskManager.Instance.UIModel.HowToStrategy.isForceShow();
			isInitialize = false;
		}

		private void BannerFocusAnim(bool isEnable)
		{
			FocusBanner.SetFocus(isEnable);
		}

		private void TopModeRun()
		{
			if (key.IsChangeIndex)
			{
				if (key.prevIndex == 4 && key.IsDownDown())
				{
					key.Index = 4;
					return;
				}
				if (key.prevIndex == 1 && key.IsUpDown())
				{
					key.Index = 1;
					return;
				}
				BannerFocusAnim(isEnable: false);
				FocusBanner = ShipStates[key.Index];
				BannerFocusAnim(isEnable: true);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove, null);
			}
			else if (key.IsMaruDown())
			{
				GotoOrganize();
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1, null);
			}
			else if (key.IsShikakuDown())
			{
				GotoSupplyConfirm();
			}
			else if (key.IsSankakuDown() && FocusBanner.ShipModel != null)
			{
				GotoRepairConfirm();
			}
			else if (key.IsBatuDown())
			{
				BackToSailSelect();
			}
			else if (key.IsRSRightDown())
			{
				ChangeDeck(isNext: true);
			}
			else if (key.IsRSLeftDown())
			{
				ChangeDeck(isNext: false);
			}
		}

		private IEnumerator TopModeEnter()
		{
			key.isStopIndex = false;
			BackButtonCol.enabled = false;
			StrategyTopTaskManager.Instance.UIModel.HowToStrategy.isForceShow();
			yield return new WaitForEndOfFrame();
			ListParent.MovePosition(1030);
		}

		private IEnumerator TopModeExit()
		{
			StrategyTopTaskManager.Instance.UIModel.HowToStrategy.isForceHide();
			key.isStopIndex = true;
			yield return null;
		}

		private void ChangeDeck(bool isNext)
		{
			int num = isNext ? 1 : (-1);
			int id = CurrentDeck.Id;
			id = (int)Util.LoopValue(id + num, 1f, StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecks().Length);
			StrategyTopTaskManager.GetSailSelect().DeckSelectController.SilentChangeIndex(id - 1);
			DeckModel deckModel = StrategyTopTaskManager.GetSailSelect().changeDeck(id);
			if (deckModel != null)
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
				SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = deckModel;
				CurrentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
				UpdateView(CurrentDeck);
				UpdateFlagShip();
				StrategyTopTaskManager.Instance.GetAreaMng().UpdateSelectArea(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID);
				OrganizeMng = new OrganizeManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
				ListParent.Initialize(OrganizeMng, DialogCamera);
			}
		}

		private void BackToSailSelect()
		{
			StrategyTopTaskManager.Instance.UIModel.HowToStrategy.isForceHide();
			StrategyTopTaskManager.Instance.GetInfoMng().updateFooterInfo(isUpdateMaterial: true);
			dialogKeyController.IsRun = true;
			StrategyTopTaskManager.GetSailSelect().CloseCommonDialog();
			StrategyTopTaskManager.Instance.setActiveStrategy(isActive: true);
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
			ListParent.SetActive(isActive: false);
			if (CurrentDeck.GetFlagShip() == null)
			{
				StrategyTopTaskManager.GetSailSelect().SearchAndChangeDeck(isNext: false, isSeachLocalArea: false);
				StrategyTopTaskManager.Instance.UIModel.Character.ChangeCharacter(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
			}
			UIRebellionOrgaizeShipBanner[] shipStates = ShipStates;
			foreach (UIRebellionOrgaizeShipBanner uIRebellionOrgaizeShipBanner in shipStates)
			{
				uIRebellionOrgaizeShipBanner.UnsetFocus();
			}
			FocusBanner = null;
		}

		private void GotoSupplyConfirm()
		{
			if (!CheckDeckState())
			{
				return;
			}
			ShipModel[] ships = CurrentDeck.GetShips();
			for (int i = 0; i < ships.Length; i++)
			{
				if (ships[i].IsTettaiBling())
				{
					CommonPopupDialog.Instance.StartPopup("撤退中の艦を含んでいます");
					return;
				}
			}
			SupplyMng = new SupplyManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
			SupplyMng.InitForDeck(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id);
			if (SupplyMng.CheckBoxALLState != CheckBoxStatus.ON)
			{
				SupplyMng.ClickCheckBoxAll();
			}
			if (SupplyMng.CheckedShipIndices.Length == 0)
			{
				CommonPopupDialog.Instance.StartPopup("補給対象の艦が居ません");
			}
			else if (!SupplyMng.IsValidSupply(SupplyType.All))
			{
				CommonPopupDialog.Instance.StartPopup("資源が不足しています");
			}
			else
			{
				ModeProcessor.ChangeMode(1);
			}
		}

		private void GotoRepairConfirm()
		{
			if (!CheckDeckState())
			{
				return;
			}
			if (FocusBanner.ShipModel.IsTettaiBling())
			{
				CommonPopupDialog.Instance.StartPopup("撤退中の艦は入渠出来ません");
			}
			else if (FocusBanner.ShipModel.IsInRepair())
			{
				if (StrategyTopTaskManager.GetLogicManager().Material.RepairKit > 0)
				{
					ModeProcessor.ChangeMode(3);
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup("高速修復材を持っていません");
				}
			}
			else if (IsValidRepair())
			{
				ModeProcessor.ChangeMode(2);
			}
		}

		private void GotoOrganize()
		{
			if (FocusBanner.ShipModel != null)
			{
				ModeProcessor.ChangeMode(4);
			}
			else
			{
				ModeProcessor.ChangeMode(5);
			}
		}

		private void OnClickBanner(int index)
		{
			BannerFocusAnim(isEnable: false);
			FocusBanner = ShipStates[index];
			BannerFocusAnim(isEnable: true);
			GotoOrganize();
			key.SilentChangeIndex(index);
		}

		private void SupplyConfirmModeRun()
		{
		}

		private IEnumerator SupplyConfirmModeEnter()
		{
			if (SupplyMng.CheckBoxALLState != CheckBoxStatus.ON)
			{
				SupplyMng.ClickCheckBoxAll();
			}
			GameObject Instance = Util.Instantiate(Prefab_SupplyConfim, base.transform.parent.gameObject);
			supplyConfirm = Instance.GetComponent<StrategySupplyConfirm>();
			supplyConfirm.SetModel(SupplyMng);
			supplyConfirm.SetOnSelectPositive(OnDesideSupply);
			supplyConfirm.SetOnSelectNeagtive(OnCancelSupply);
			yield return new WaitForEndOfFrame();
			supplyConfirm.Open();
		}

		private IEnumerator SupplyConfirmModeExit()
		{
			yield return StartCoroutine(supplyConfirm.Close());
		}

		private void OnDesideSupply()
		{
			App.OnlyController = key;
			bool use_baux;
			bool flag = SupplyMng.Supply(SupplyType.All, out use_baux);
			UpdateView(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
			if (flag && use_baux)
			{
				BauxiSuccess.Play();
			}
			else if (!flag && use_baux)
			{
				BauxiField.Play();
			}
			ShipModel model = (FocusBanner.ShipModel == null) ? CurrentDeck.GetFlagShip() : FocusBanner.ShipModel;
			ShipUtils.PlayShipVoice(model, 27);
			ModeProcessor.ChangeMode(0);
		}

		private void OnCancelSupply()
		{
			App.OnlyController = key;
			ModeProcessor.ChangeMode(0);
		}

		private void RepairConfirmModeRun()
		{
		}

		private IEnumerator RepairConfirmModeEnter()
		{
			GameObject Instance = Util.Instantiate(Prefab_RepairConfim, base.transform.parent.gameObject);
			repairConfim = Instance.GetComponent<StrategyRepairConfirm>();
			repairConfim.SetModel(FocusBanner.ShipModel);
			repairConfim.SetOnSelectPositive(OnDesideRepair);
			repairConfim.SetOnSelectNeagtive(OnCancelRepair);
			yield return new WaitForEndOfFrame();
			repairConfim.Open();
		}

		private IEnumerator RepairConfirmModeExit()
		{
			yield return StartCoroutine(repairConfim.Close());
		}

		private void OnDesideRepair()
		{
			App.OnlyController = key;
			RepairMng.StartRepair(RepairMng.GetDockIndexFromDock(repairDockModel), FocusBanner.ShipModel.MemId, use_repairkit: false);
			UpdateView(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
			StrategyTopTaskManager.Instance.TileManager.UpdateAllAreaDockIcons();
			if (FocusBanner.ShipModel.TaikyuRate > 50.0)
			{
				ShipUtils.PlayShipVoice(FocusBanner.ShipModel, 11);
			}
			else
			{
				ShipUtils.PlayShipVoice(FocusBanner.ShipModel, 12);
			}
			ModeProcessor.ChangeMode(0);
		}

		private void OnCancelRepair()
		{
			App.OnlyController = key;
			ModeProcessor.ChangeMode(0);
		}

		private bool IsValidRepair()
		{
			RepairMng = new RepairManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
			repairDockModel = null;
			RepairDockModel[] docks = RepairMng.GetDocks();
			if (FocusBanner.ShipModel.NowHp >= FocusBanner.ShipModel.MaxHp)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NoDamage));
				return false;
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckAreaModel.NDockMax == 0)
			{
				CommonPopupDialog.Instance.StartPopup("この海域には入渠ドックがありません");
				return false;
			}
			for (int i = 0; i < RepairMng.GetDocks().Length; i++)
			{
				if (docks[i].State == NdockStates.EMPTY)
				{
					repairDockModel = docks[i];
					break;
				}
			}
			if (repairDockModel == null)
			{
				CommonPopupDialog.Instance.StartPopup("入渠ドックに空きがありません");
				return false;
			}
			if (!RepairMng.IsValidStartRepair(FocusBanner.ShipModel.MemId))
			{
				CommonPopupDialog.Instance.StartPopup("資源が不足しています");
				return false;
			}
			return true;
		}

		private void RepairKitConfirmModeRun()
		{
		}

		private IEnumerator RepairKitConfirmModeEnter()
		{
			GameObject Instance = Util.Instantiate(Prefab_RepairKitConfim, base.transform.parent.gameObject);
			repairKitConfim = Instance.GetComponent<StrategyRepairKitConfirm>();
			RepairMng = new RepairManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
			RepairDockModel[] dockModels = RepairMng.GetDocks();
			for (int i = 0; i < RepairMng.GetDocks().Length; i++)
			{
				if (dockModels[i].GetShip() != null && dockModels[i].GetShip().MemId == FocusBanner.ShipModel.MemId)
				{
					repairDockModel = dockModels[i];
					break;
				}
			}
			if (repairDockModel == null)
			{
				ModeProcessor.ChangeMode(0);
				yield break;
			}
			repairKitConfim.SetModel(repairDockModel, RepairMng.Material.RepairKit);
			repairKitConfim.SetOnSelectPositive(OnDesideRepairKit);
			repairKitConfim.SetOnSelectNeagtive(OnCancelRepairKit);
			yield return new WaitForEndOfFrame();
			repairKitConfim.Open();
			yield return null;
		}

		private IEnumerator RepairKitConfirmModeExit()
		{
			yield return StartCoroutine(repairKitConfim.Close());
		}

		private void OnDesideRepairKit()
		{
			App.OnlyController = key;
			RepairMng.ChangeRepairSpeed(RepairMng.GetDockIndexFromDock(repairDockModel));
			UpdateView(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
			ShipUtils.PlayShipVoice(FocusBanner.ShipModel, 26);
			ModeProcessor.ChangeMode(0);
			UpdateFlagShip();
			StrategyTopTaskManager.Instance.TileManager.UpdateAllAreaDockIcons();
		}

		private void OnCancelRepairKit()
		{
			App.OnlyController = key;
			ModeProcessor.ChangeMode(0);
		}

		private void OrganizeDetailModeRun()
		{
			if (key.IsBatuDown())
			{
				BackToTop();
			}
			else if (key.IsRightDown())
			{
				OrganizeDetailMng.buttons.UpdateButton(isLeft: false, OrganizeMng);
			}
			else if (key.IsLeftDown())
			{
				OrganizeDetailMng.buttons.UpdateButton(isLeft: true, OrganizeMng);
			}
			else if (key.IsMaruDown())
			{
				OrganizeDetailMng.buttons.Decide();
			}
		}

		private IEnumerator OrganizeDetailModeEnter()
		{
			CreateDetailPanel(isFirstDetail: true, FocusBanner.ShipModel);
			BackButtonCol.enabled = true;
			OrganizeDetailMng.SetBackButton(base.gameObject, "BackToTop");
			yield return new WaitForEndOfFrame();
			ShipUtils.PlayShipVoice(FocusBanner.ShipModel, App.rand.Next(2, 4));
			ListParent.MovePosition(1030);
		}

		private IEnumerator OrganizeDetailModeExit()
		{
			yield return StartCoroutine(OrganizeDetailMng.CloseAndDestroy());
			OrganizeDetailMng = null;
			yield return new WaitForEndOfFrame();
		}

		public void SetBtnEL()
		{
			ModeProcessor.ChangeMode(5);
		}

		public void ResetBtnEL()
		{
			OrganizeMng.UnsetOrganize(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID, key.Index);
			ModeProcessor.ChangeMode(0);
			UpdateView(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
			UpdateFlagShip();
		}

		public void BackToTop()
		{
			ModeProcessor.ChangeMode(0);
		}

		private void CreateDetailPanel(bool isFirstDetail, ShipModel Ship)
		{
			GameObject gameObject = Util.Instantiate(Prefab_OrganizeDetailMng.gameObject, base.gameObject);
			OrganizeDetailMng = gameObject.GetComponent<OrganizeDetail_Manager>();
			OrganizeDetailMng.SetDetailPanel(Ship, isFirstDetail, SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id, OrganizeMng, FocusBanner.index, this);
		}

		private void OrganizeListModeRun()
		{
		}

		private IEnumerator OrganizeListModeEnter()
		{
			ListParent.SetActive(isActive: true);
			yield return new WaitForEndOfFrame();
			if (!isInitialize)
			{
				ListParent.Initialize(OrganizeMng, DialogCamera);
				ListParent.HeadFocus();
				isInitialize = true;
				yield return new WaitForEndOfFrame();
			}
			ListParent.SetOnSelect(OnShipSelect);
			ListParent.SetOnCancel(OnCancel);
			ListParent.SetKeyController(key);
			BackButtonCol.enabled = true;
			ListParent.SetBackButton(ListParent.gameObject, "OnCancel");
			ListParent.StartControl();
			ListParent.RefreshViews();
			yield return new WaitForEndOfFrame();
			ListParent.MovePosition(205, isOpen: true);
			key.KeyInputInterval = 0.05f;
			yield return null;
		}

		private IEnumerator OrganizeListModeExit()
		{
			key.KeyInputInterval = 0.1f;
			yield return new WaitForEndOfFrame();
			yield return null;
		}

		private void OnShipSelect(ShipModel ship)
		{
			ModeProcessor.ChangeMode(6);
			ListSelectShipModel = ship;
		}

		private void OnCancel()
		{
			ModeProcessor.ChangeMode(0);
		}

		private void OrganizeListDetailModeRun()
		{
			if (key.IsBatuDown())
			{
				BackToList();
			}
			else if (key.IsMaruDown())
			{
				OrganizeDetailMng.buttons.Decide();
			}
			else if (key.IsShikakuDown())
			{
				ChangeLockButton();
			}
			else if (key.IsRightDown() && ListParent.GetFocusModel().IsLocked())
			{
				ChangeLockButton();
			}
			else if (key.IsLeftDown() && !ListParent.GetFocusModel().IsLocked())
			{
				ChangeLockButton();
			}
		}

		private IEnumerator OrganizeListDetailModeEnter()
		{
			CreateDetailPanel(isFirstDetail: false, ListSelectShipModel);
			OrganizeDetailMng.buttons.LockSwitch.setChangeListViewIcon(ListParent.ChangeLockBtnState);
			BackButtonCol.enabled = true;
			OrganizeDetailMng.SetBackButton(base.gameObject, "BackToList");
			yield return new WaitForEndOfFrame();
			ShipUtils.PlayShipVoice(ListSelectShipModel, App.rand.Next(2, 4));
		}

		private IEnumerator OrganizeListDetailModeExit()
		{
			yield return StartCoroutine(OrganizeDetailMng.CloseAndDestroy());
			OrganizeDetailMng = null;
		}

		public void ChangeLockButton()
		{
			OrganizeDetailMng.buttons.LockSwitch.MoveLockBtn();
		}

		public void ChangeButtonEL()
		{
			if (OrganizeMng.IsValidChange(CurrentDeck.Id, key.Index, ListSelectShipModel.MemId))
			{
				OrganizeMng.ChangeOrganize(CurrentDeck.Id, key.Index, ListSelectShipModel.MemId);
				UpdateView(CurrentDeck);
				UpdateFlagShip();
				ListParent.RefreshViews();
				ListParent.MovePosition(1030);
				ShipUtils.PlayShipVoice(ListSelectShipModel, 13);
				ModeProcessor.ChangeMode(0);
				StrategyTopTaskManager.Instance.ShipIconManager.changeFocus();
			}
		}

		public void BackToList()
		{
			ModeProcessor.ChangeMode(5);
		}

		private void OnDestroy()
		{
			ShipStates = null;
			key = null;
			FocusBanner = null;
			SupplyMng = null;
			RepairMng = null;
			OrganizeMng = null;
			ModeProcessor = null;
			dialogKeyController = null;
			repairDockModel = null;
			Prefab_RepairConfim = null;
			repairConfim = null;
			Prefab_RepairKitConfim = null;
			repairKitConfim = null;
			Prefab_OrganizeDetailMng = null;
			Prefab_OrganizeList = null;
			OrganizeDetailMng = null;
			DeckNoIcon = null;
			DeckNoLabel = null;
			ListSelectShipModel = null;
			CurrentDeck = null;
		}

		private void UpdateFlagShip()
		{
			StrategyTopTaskManager.Instance.UIModel.Character.ChangeCharacter(CurrentDeck);
			StrategyTopTaskManager.Instance.UIModel.Character.ResetPosition();
			this.DelayActionFrame(1, delegate
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			});
			UpdateShipIcons();
		}

		private void UpdateShipIcons()
		{
			StrategyTopTaskManager.Instance.ShipIconManager.setShipIcons(OrganizeMng.UserInfo.GetDecks(), isScaleZero: false);
			StrategyTopTaskManager.Instance.ShipIconManager.changeFocus();
			StrategyTopTaskManager.Instance.ShipIconManager.SetVisible(isVisible: true);
		}

		private bool CheckDeckState()
		{
			if (CurrentDeck.MissionState != 0)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getCancelReason(IsGoCondition.Mission));
				return false;
			}
			return true;
		}
	}
}
