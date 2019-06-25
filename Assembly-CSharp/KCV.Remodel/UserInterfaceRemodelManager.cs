using Common.Enum;
using Common.Struct;
using KCV.Display;
using KCV.Scene.Port;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UserInterfaceRemodelManager : MonoBehaviour, CommonDeckSwitchHandler
	{
		private const BGMFileInfos SCENE_BGM = BGMFileInfos.PortTools;

		[SerializeField]
		private Texture[] mTextures_Preload;

		[SerializeField]
		private UISprite backGroundUpperSteelFrame;

		[SerializeField]
		private UISprite backGroundLowerSteelFrame;

		[SerializeField]
		private Camera mCamera_TouchEventCatch;

		[SerializeField]
		private PortUpgradesConvertShipManager mPrefab_PortUpgradesConvertShipManager_KaizouAnimation;

		[SerializeField]
		private PortUpgradesModernizeShipManager mPrefab_PortUpgradesModernizeShipManager_KindaikakaishuAnimation;

		[SerializeField]
		private UIRemodelHeader mUIRemodelHeader;

		[SerializeField]
		private UIRemodelShipSlider mUIRemodelShipSlider;

		[SerializeField]
		private UIRemodelShipStatus mUIRemodelShipStatus;

		[SerializeField]
		private UIRemodelLeftShipStatus mUIRemodelLeftShipStatus;

		[SerializeField]
		private UIRemodelStartRightInfo mUIRemodelStartRightInfo;

		[SerializeField]
		private UIRemodelEquipSlotItems mUIRemodelEquipSlotItems;

		[SerializeField]
		private UIRemodelModeSelectMenu mUIRemodelModeSelectMenu;

		[SerializeField]
		private UIRemodelDeckSwitchManager mUIRemodelDeckSwitchManager;

		[SerializeField]
		private UIRemodelModernization mUIRemodelModernization;

		[SerializeField]
		private UIRemodelKaizou mUIRemodelKaizou;

		[SerializeField]
		private UIRemodelEquipListParentNew mUIRemodelEquipListParent;

		[SerializeField]
		private UIRemodelSlotItemChangePreview mUIRemodelSlotItemChangePreview;

		[SerializeField]
		private CategoryMenu mCategoryMenu;

		[SerializeField]
		private UIRemodeModernizationShipTargetListParentNew mUIRemodeModernizationShipTargetListParentNew;

		[SerializeField]
		private UIRemodelModernizationStartConfirm mUIRemodelModernizationStartConfirm;

		[SerializeField]
		private UIRemodelOtherShipPickerParentNew mUIRemodelOtherShipPickerParent;

		[SerializeField]
		private UIRemodelDeckSwitchManager commonDeckSwitchManager;

		[SerializeField]
		private UIRemodelCurrentSlot mUIRemodelCurrentSlot;

		[SerializeField]
		private UIRemodelHowTo mUIHowTo;

		[SerializeField]
		private GameObject okBauxiteUseMessage;

		[SerializeField]
		private GameObject okBauxiteUseHighCostMessage;

		[SerializeField]
		private GameObject ngBauxiteShortMessage;

		[SerializeField]
		private GameObject ngBausiteShortHighCostMessage;

		[SerializeField]
		private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

		private KeyControl mKeyController;

		private List<UIRemodelView> allSwitchableViews = new List<UIRemodelView>();

		private Dictionary<ScreenStatus, List<UIRemodelView>> viewList = new Dictionary<ScreenStatus, List<UIRemodelView>>();

		public ScreenStatus status;

		public bool guideoff;

		public RemodelManager mRemodelManager
		{
			get;
			private set;
		}

		public RemodelGradeUpManager mRemodelGradeUpManager
		{
			get;
			private set;
		}

		public DeckModel focusedDeckModel
		{
			get;
			private set;
		}

		public ShipModel focusedShipModel
		{
			get;
			private set;
		}

		private bool otherShip => focusedDeckModel == null;

		public static UserInterfaceRemodelManager instance
		{
			get;
			private set;
		}

		private int CountBannersInTexture(CommonShipBanner[] banners, Texture countTargetTexture)
		{
			int num = 0;
			if (countTargetTexture == null)
			{
				return num;
			}
			foreach (CommonShipBanner commonShipBanner in banners)
			{
				if (!(commonShipBanner == null))
				{
					Texture mainTexture = commonShipBanner.GetUITexture().mainTexture;
					if (!(mainTexture == null) && countTargetTexture.GetNativeTexturePtr() == mainTexture.GetNativeTexturePtr())
					{
						num++;
					}
				}
			}
			return num;
		}

		public void ReleaseRequestBanner(ref Texture releaseRequestTexture)
		{
			if (releaseRequestTexture == null)
			{
				releaseRequestTexture = null;
				return;
			}
			int num = 0;
			IBannerResourceManage bannerResourceManage = mUIRemodelOtherShipPickerParent;
			CommonShipBanner[] banner = bannerResourceManage.GetBanner();
			int num2 = CountBannersInTexture(banner, releaseRequestTexture);
			num += num2;
			IBannerResourceManage bannerResourceManage2 = mUIRemodelModernization;
			CommonShipBanner[] banner2 = bannerResourceManage2.GetBanner();
			int num3 = CountBannersInTexture(banner2, releaseRequestTexture);
			num += num3;
			IBannerResourceManage bannerResourceManage3 = mUIRemodeModernizationShipTargetListParentNew;
			CommonShipBanner[] banner3 = bannerResourceManage3.GetBanner();
			int num4 = CountBannersInTexture(banner3, releaseRequestTexture);
			num += num4;
			IBannerResourceManage bannerResourceManage4 = mUIRemodelModernizationStartConfirm;
			CommonShipBanner[] banner4 = bannerResourceManage4.GetBanner();
			int num5 = CountBannersInTexture(banner4, releaseRequestTexture);
			if (num + num5 == 0)
			{
				Resources.UnloadAsset(releaseRequestTexture);
			}
			releaseRequestTexture = null;
		}

		private void SetStatus(ScreenStatus status)
		{
			this.status = status;
			if (status != ScreenStatus.MODE_KINDAIKA_KAISHU_ANIMATION && status != ScreenStatus.MODE_KAIZO_ANIMATION)
			{
				if (!SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.isTransitionNow)
				{
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				}
			}
			else
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			}
			UpdateHeaderTitle();
			SwitchViews();
		}

		private void SwitchViews()
		{
			switch (status)
			{
			case ScreenStatus.SELECT_DECK_SHIP:
				guideoff = false;
				break;
			case ScreenStatus.SELECT_SETTING_MODE:
				guideoff = false;
				mUIRemodelShipStatus.ShowMove();
				mUIRemodelLeftShipStatus.Init(focusedShipModel);
				mUIRemodelLeftShipStatus.SetExpand(expand: false);
				break;
			case ScreenStatus.MODE_SOUBI_HENKOU:
				mUIRemodelLeftShipStatus.Init(focusedShipModel);
				mUIRemodelLeftShipStatus.SetExpand(expand: false);
				break;
			case ScreenStatus.MODE_SOUBI_HENKOU_TYPE_SELECT:
				mUIRemodelCurrentSlot.SetActive(isActive: true);
				mUIRemodelCurrentSlot.Init(mUIRemodelEquipSlotItems.currentFocusItem.GetModel());
				break;
			case ScreenStatus.MODE_KINDAIKA_KAISHU:
				mUIRemodelModernization.InitFocus();
				mUIRemodelLeftShipStatus.Init(focusedShipModel);
				mUIRemodelLeftShipStatus.SetExpand(expand: true);
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				break;
			case ScreenStatus.MODE_KAIZO:
				mUIRemodelKaizou.Show();
				break;
			case ScreenStatus.MODE_KAIZO_ANIMATION:
				guideoff = true;
				break;
			}
			allSwitchableViews.ForEach(delegate(UIRemodelView e)
			{
				if (viewList.ContainsKey(status) && viewList[status].Contains(e))
				{
					((MonoBehaviour)e).SetActive(isActive: true);
					e.Show();
				}
				else if (((MonoBehaviour)e).gameObject.activeSelf)
				{
					e.Hide();
				}
			});
		}

		public void Awake()
		{
			instance = this;
			guideoff = false;
		}

		private void Update()
		{
			if (mKeyController != null)
			{
				mKeyController.Update();
				HandleKeyController();
			}
		}

		private void HandleKeyController()
		{
			ScreenStatus screenStatus = status;
			if ((screenStatus == ScreenStatus.SELECT_DECK_SHIP || screenStatus == ScreenStatus.SELECT_OTHER_SHIP) && mKeyController.IsBatuDown())
			{
				Back2PortTop();
			}
			if (status != ScreenStatus.MODE_KINDAIKA_KAISHU_ANIMATION && status != ScreenStatus.MODE_KAIZO_ANIMATION && mKeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
		}

		public void OnTouchShipStatus()
		{
			switch (status)
			{
			case ScreenStatus.SELECT_DECK_SHIP:
			case ScreenStatus.SELECT_OTHER_SHIP:
				Forward2ModeSelect();
				break;
			case ScreenStatus.SELECT_SETTING_MODE:
				Back2ShipSelect();
				break;
			}
		}

		private void Back2PortTop()
		{
			if (status == ScreenStatus.SELECT_DECK_SHIP || status == ScreenStatus.SELECT_OTHER_SHIP)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
			}
		}

		private IEnumerator CloseProcess()
		{
			yield return null;
		}

		public void Back2ShipSelect()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			mUIRemodelStartRightInfo.Init(focusedShipModel);
			SetStatus((focusedDeckModel == null) ? ScreenStatus.SELECT_OTHER_SHIP : ScreenStatus.SELECT_DECK_SHIP);
		}

		public void Forward2ModeSelect()
		{
			if (status == ScreenStatus.SELECT_DECK_SHIP || status == ScreenStatus.SELECT_OTHER_SHIP || status == ScreenStatus.MODE_KAIZO_END_ANIMATION)
			{
				DecideShip();
				SetStatus(ScreenStatus.SELECT_SETTING_MODE);
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}

		public void Back2ModeSelect()
		{
			if (status == ScreenStatus.MODE_SOUBI_HENKOU || status == ScreenStatus.MODE_KINDAIKA_KAISHU || status == ScreenStatus.MODE_KAIZO || status == ScreenStatus.MODE_KINDAIKA_KAISHU_END_ANIMATION)
			{
				SetStatus(ScreenStatus.SELECT_SETTING_MODE);
			}
		}

		public void Forward2SoubiHenkou(SlotitemModel slotItemModel = null, bool requestChangeMode = false)
		{
			if (status == ScreenStatus.SELECT_DECK_SHIP || status == ScreenStatus.SELECT_OTHER_SHIP || status == ScreenStatus.SELECT_SETTING_MODE)
			{
				if (status == ScreenStatus.SELECT_DECK_SHIP || status == ScreenStatus.SELECT_OTHER_SHIP)
				{
					DecideShip();
				}
				mUIRemodelEquipSlotItems.Initialize(mKeyController, focusedShipModel);
				SetStatus(ScreenStatus.MODE_SOUBI_HENKOU);
				if (requestChangeMode)
				{
					mUIRemodelEquipSlotItems.ChangeFocusItemFromModel(slotItemModel);
				}
			}
		}

		public void Back2SoubiHenkou()
		{
			if (status == ScreenStatus.MODE_SOUBI_HENKOU_TYPE_SELECT || status == ScreenStatus.MODE_SOUBI_HENKOU_PREVIEW)
			{
				mUIRemodelEquipSlotItems.Initialize(mKeyController, focusedShipModel);
				SetStatus(ScreenStatus.MODE_SOUBI_HENKOU);
			}
		}

		public void Forward2SoubiHenkouTypeSelect()
		{
			if (status == ScreenStatus.MODE_SOUBI_HENKOU)
			{
				mCategoryMenu.SetActive(isActive: true);
				mCategoryMenu.Init(mKeyController, focusedShipModel, mUIRemodelEquipSlotItems.currentFocusItem);
				SetStatus(ScreenStatus.MODE_SOUBI_HENKOU_TYPE_SELECT);
			}
		}

		public void Back2SoubiHenkouTypeSelect()
		{
			if (status == ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT)
			{
				SetStatus(ScreenStatus.MODE_SOUBI_HENKOU_TYPE_SELECT);
			}
		}

		public void Forward2SoubiHenkouItemSelect(ShipModel shipModel, SlotitemCategory slotitemCategory)
		{
			SetStatus(ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT);
			mUIRemodelEquipListParent.SetActive(isActive: true);
			mUIRemodelEquipListParent.Initialize(mKeyController, mUIRemodelShipStatus, mUIRemodelEquipSlotItems, shipModel, slotitemCategory);
		}

		public void Back2SoubiHenkouItemSelect()
		{
			SetStatus(ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT);
		}

		public void Wait2AnimationFromKindaikaKakunin()
		{
			status = ScreenStatus.WAIT;
		}

		public void Resume2WaitKindaikaKakunin()
		{
			status = ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN;
		}

		public void Forward2SoubiHenkouPreview(ShipModel targetShipModel, int selectedSlotIndex, UIRemodelEquipListChildNew child)
		{
			if (status == ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT)
			{
				SetStatus(ScreenStatus.MODE_SOUBI_HENKOU_PREVIEW);
				SlotitemModel srcSlotItemModel = null;
				if (selectedSlotIndex < targetShipModel.SlotitemList.Count)
				{
					srcSlotItemModel = targetShipModel.SlotitemList[selectedSlotIndex];
				}
				else if (targetShipModel.HasExSlot())
				{
					srcSlotItemModel = targetShipModel.SlotitemEx;
				}
				mUIRemodelSlotItemChangePreview.Initialize(mKeyController, targetShipModel, srcSlotItemModel, child.GetModel(), selectedSlotIndex);
			}
		}

		public void Forward2KindaikaKaishu()
		{
			if (status == ScreenStatus.SELECT_SETTING_MODE)
			{
				mUIRemodelModernizationStartConfirm.DrawShip(focusedShipModel);
				SetStatus(ScreenStatus.MODE_KINDAIKA_KAISHU);
			}
		}

		public void Back2KindaikaKaishu()
		{
			if (status == ScreenStatus.MODE_KINDAIKA_KAISHU_SOZAI_SENTAKU || status == ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN)
			{
				SetStatus(ScreenStatus.MODE_KINDAIKA_KAISHU);
			}
		}

		public void Forward2KindaikaKaishuSozaiSentaku(List<ShipModel> selectedSozaiShipModels)
		{
			if (status == ScreenStatus.MODE_KINDAIKA_KAISHU || status == ScreenStatus.SELECT_SETTING_MODE)
			{
				mUIRemodeModernizationShipTargetListParentNew.SetActive(isActive: true);
				mUIRemodeModernizationShipTargetListParentNew.Initialize(mKeyController, mUIRemodelModernization.GetFocusSlot().GetSlotInShip(), selectedSozaiShipModels);
				SetStatus(ScreenStatus.MODE_KINDAIKA_KAISHU_SOZAI_SENTAKU);
			}
		}

		public void Forward2KindaikaKaishuKakunin(ShipModel targetShipModel, List<ShipModel> sozaiShipModels)
		{
			if (status == ScreenStatus.MODE_KINDAIKA_KAISHU)
			{
				SetStatus(ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN);
				PowUpInfo powUpInfo = mRemodelManager.getPowUpInfo(sozaiShipModels);
				mUIRemodelModernizationStartConfirm.Initialize(mKeyController, targetShipModel, sozaiShipModels, powUpInfo);
			}
		}

		public void Forward2KindaikaKaishuAnimation(List<ShipModel> sozai, ShipModel baseShip)
		{
			if (status == ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN)
			{
				SetStatus(ScreenStatus.MODE_KINDAIKA_KAISHU_ANIMATION);
				HideHeader();
				mUIRemodelHeader.SetActive(isActive: false);
				commonDeckSwitchManager.SetActive(isActive: false);
				backGroundUpperSteelFrame.SetActive(isActive: false);
				backGroundLowerSteelFrame.SetActive(isActive: false);
				bool great_success;
				ShipModel shipModel = mRemodelManager.PowerUp(sozai, out great_success);
				bool isFail = shipModel == null;
				mUIRemodelShipStatus.SetActive(isActive: false);
				StartCoroutine(StartModernizeAnimation(baseShip, 1, mKeyController, isFail, great_success, sozai.Count, baseShip.IsDamaged(), delegate
				{
					if (status == ScreenStatus.MODE_KINDAIKA_KAISHU_ANIMATION)
					{
						SetStatus(ScreenStatus.MODE_KINDAIKA_KAISHU_END_ANIMATION);
						ShowHeader();
						mUIRemodelHeader.SetActive(isActive: true);
						backGroundUpperSteelFrame.SetActive(isActive: true);
						backGroundLowerSteelFrame.SetActive(isActive: true);
						mUIRemodelShipStatus.SetActive(isActive: true);
						mUIRemodelModernization.UnSetAll();
						mUIRemodelModernization.RemoveFocus();
						mUIRemodelOtherShipPickerParent.Refresh((!otherShip) ? null : focusedShipModel);
						commonDeckSwitchManager.SetActive(isActive: true);
						mUIRemodelLeftShipStatus.SetExpand(expand: false);
						mUIRemodelLeftShipStatus.Hide(animation: false);
						Back2ModeSelect();
					}
				}));
			}
		}

		public void Forward2Kaizo()
		{
			if (status == ScreenStatus.SELECT_SETTING_MODE)
			{
				SetStatus(ScreenStatus.MODE_KAIZO);
				mUIRemodelKaizou.Initialize(focusedShipModel, mRemodelGradeUpManager.DesignSpecificationsForGradeup);
				mUIRemodelKaizou.SetKeyController(mKeyController);
			}
		}

		public void Forward2KaizoAnimation(ShipModel targetShipModel)
		{
			if (status == ScreenStatus.MODE_KAIZO)
			{
				SetStatus(ScreenStatus.MODE_KAIZO_ANIMATION);
				if (instance.mRemodelGradeUpManager.GradeUp())
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					instance.mRemodelManager.ClearUnsetSlotsCache();
				}
				backGroundUpperSteelFrame.SetActive(isActive: false);
				backGroundLowerSteelFrame.SetActive(isActive: false);
				mUIRemodelShipStatus.SetActive(isActive: false);
				HideHeader();
				mUIRemodelHeader.SetActive(isActive: false);
				UpdateHeaderMaterial();
				StartCoroutine(StartGradeUpProductionCoroutine(targetShipModel, mKeyController, delegate
				{
					if (status == ScreenStatus.MODE_KAIZO_ANIMATION)
					{
						status = ScreenStatus.MODE_KAIZO_END_ANIMATION;
						if (otherShip)
						{
							mUIRemodelOtherShipPickerParent.Refresh(focusedShipModel);
						}
						mUIRemodelShipStatus.SetActive(isActive: true);
						ShowHeader();
						mUIRemodelHeader.SetActive(isActive: true);
						ChangeFocusShip(targetShipModel);
						backGroundUpperSteelFrame.SetActive(isActive: true);
						backGroundLowerSteelFrame.SetActive(isActive: true);
						mUIRemodelModeSelectMenu.Init(mKeyController, mRemodelGradeUpManager.GradeupBtnEnabled);
						Forward2ModeSelect();
					}
				}));
			}
		}

		private void ChangeFocusDeck(DeckModel deckModel)
		{
			focusedDeckModel = deckModel;
			if (focusedDeckModel != null)
			{
				mUIRemodelShipSlider.Initialize(focusedDeckModel);
				ChangeFocusShip(focusedDeckModel.GetFlagShip());
			}
		}

		public void ChangeFocusShip(ShipModel focusShipModel)
		{
			focusedShipModel = focusShipModel;
			mUIRemodelShipStatus.Init(focusedShipModel);
			mUIRemodelStartRightInfo.Init(focusedShipModel);
		}

		public void ProcessChangeSlotItem(int slotIndex, SlotitemModel slotItem, int voiceType, bool isExSlot)
		{
			if (status != ScreenStatus.MODE_SOUBI_HENKOU_PREVIEW)
			{
				return;
			}
			mRemodelManager.GetSlotitemInfoToChange(focusedShipModel.MemId, slotItem.MemId, slotIndex);
			SlotSetChkResult_Slot slotSetChkResult_Slot = isExSlot ? mRemodelManager.IsValidChangeSlotitemEx(focusedShipModel.MemId, slotItem.MemId) : mRemodelManager.IsValidChangeSlotitem(focusedShipModel.MemId, slotItem.MemId, slotIndex);
			bool flag = false;
			switch (slotSetChkResult_Slot)
			{
			case SlotSetChkResult_Slot.Ok:
			case SlotSetChkResult_Slot.OkBauxiteUse:
			case SlotSetChkResult_Slot.OkBauxiteUseHighCost:
				flag = true;
				break;
			case SlotSetChkResult_Slot.NgBauxiteShort:
				AnimateBauxite(ngBauxiteShortMessage);
				break;
			case SlotSetChkResult_Slot.NgBausiteShortHighCost:
				AnimateBauxite(ngBausiteShortHighCostMessage);
				break;
			}
			SlotSetChkResult_Slot slotSetChkResult_Slot2 = isExSlot ? mRemodelManager.ChangeSlotitemEx(focusedShipModel.MemId, slotItem.MemId) : mRemodelManager.ChangeSlotitem(focusedShipModel.MemId, slotItem.MemId, slotIndex);
			if (flag)
			{
				bool flag2 = true;
				switch (slotSetChkResult_Slot2)
				{
				case SlotSetChkResult_Slot.OkBauxiteUse:
					AnimateBauxite(okBauxiteUseMessage);
					break;
				case SlotSetChkResult_Slot.OkBauxiteUseHighCost:
					AnimateBauxite(okBauxiteUseHighCostMessage);
					break;
				default:
					flag2 = false;
					break;
				case SlotSetChkResult_Slot.Ok:
					break;
				}
				if (flag2)
				{
					ShipUtils.PlayShipVoice(focusedShipModel, voiceType);
				}
				if (SingletonMonoBehaviour<UIPortFrame>.exist())
				{
					SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(mRemodelManager);
				}
			}
			Back2SoubiHenkou();
		}

		private void AnimateBauxite(GameObject texture)
		{
			texture.GetComponent<Animation>().Play("RemodelBauxiteMessage");
		}

		public void SelectKindaikaKaishuSozai(ShipModel selectedShipModel)
		{
			if (selectedShipModel == null)
			{
				mUIRemodelModernization.GetFocusSlot().UnSet();
			}
			else
			{
				mUIRemodelModernization.SetCurrentFocusToShip(selectedShipModel);
			}
			mUIRemodelModernization.RefreshList();
			Back2KindaikaKaishu();
		}

		private void DecideShip()
		{
			mUIRemodelModernization.SetActive(isActive: true);
			mUIRemodelModernization.Initialize(mKeyController, focusedShipModel);
			mRemodelGradeUpManager = new RemodelGradeUpManager(focusedShipModel);
			mUIRemodelModeSelectMenu.Init(mKeyController, mRemodelGradeUpManager.GradeupBtnEnabled);
		}

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			mKeyController = new KeyControl();
			mKeyController.IsRun = false;
			mUIDisplaySwipeEventRegion.SetEventCatchCamera(mCamera_TouchEventCatch);
			mUIDisplaySwipeEventRegion.SetOnSwipeListener(OnSwipeDeckListener);
			mUIRemodeModernizationShipTargetListParentNew.SetCamera(mCamera_TouchEventCatch);
			mUIRemodelEquipListParent.SetSwipeEventCatchCamera(mCamera_TouchEventCatch);
			mUIRemodelOtherShipPickerParent.SetSwipeEventCamera(mCamera_TouchEventCatch);
			int areaId = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
			mRemodelManager = new RemodelManager(areaId);
			yield return new WaitForEndOfFrame();
			DeckModel[] decks = mRemodelManager.UserInfo.GetDecksFromArea(areaId);
			UpdateHeaderMaterial();
			yield return new WaitForEndOfFrame();
			yield return StartCoroutine(InitViewsCoroutine(decks));
			yield return new WaitForEndOfFrame();
			AudioClip sceneBgm = SoundFile.LoadBGM(BGMFileInfos.PortTools);
			yield return new WaitForEndOfFrame();
			if (SingletonMonoBehaviour<PortObjectManager>.exist())
			{
				SoundUtils.SwitchBGM(sceneBgm);
				SingletonMonoBehaviour<UIPortFrame>.Instance.setVisibleHeader(isVisible: false);
				SingletonMonoBehaviour<UIPortFrame>.Instance.CircleUpdateInfo(mRemodelManager);
				SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(null);
				mKeyController.IsRun = true;
			}
		}

		private void InitViews(DeckModel[] decks)
		{
			mUIRemodelShipSlider.Init(mKeyController);
			mUIRemodelOtherShipPickerParent.SetActive(isActive: true);
			mUIRemodelOtherShipPickerParent.Initialize(mKeyController);
			mUIRemodelDeckSwitchManager.Init(decks, this, mKeyController, otherEnabled: true);
			allSwitchableViews.Add(mUIRemodelShipSlider);
			allSwitchableViews.Add(mUIRemodelLeftShipStatus);
			allSwitchableViews.Add(mUIRemodelStartRightInfo);
			allSwitchableViews.Add(mUIRemodelEquipSlotItems);
			allSwitchableViews.Add(mUIRemodelModeSelectMenu);
			allSwitchableViews.Add(mUIRemodelDeckSwitchManager);
			allSwitchableViews.Add(mUIRemodelModernization);
			allSwitchableViews.Add(mUIRemodelKaizou);
			allSwitchableViews.Add(mUIRemodelEquipListParent);
			allSwitchableViews.Add(mUIRemodelSlotItemChangePreview);
			allSwitchableViews.Add(mCategoryMenu);
			allSwitchableViews.Add(mUIRemodeModernizationShipTargetListParentNew);
			allSwitchableViews.Add(mUIRemodelModernizationStartConfirm);
			allSwitchableViews.Add(mUIRemodelOtherShipPickerParent);
			allSwitchableViews.Add(commonDeckSwitchManager);
			allSwitchableViews.Add(mUIRemodelCurrentSlot);
			viewList.Add(ScreenStatus.SELECT_DECK_SHIP, new List<UIRemodelView>
			{
				mUIRemodelDeckSwitchManager,
				mUIRemodelShipSlider,
				mUIRemodelStartRightInfo
			});
			viewList.Add(ScreenStatus.SELECT_OTHER_SHIP, new List<UIRemodelView>
			{
				mUIRemodelDeckSwitchManager,
				mUIRemodelOtherShipPickerParent
			});
			viewList.Add(ScreenStatus.SELECT_SETTING_MODE, new List<UIRemodelView>
			{
				mUIRemodelModeSelectMenu,
				mUIRemodelLeftShipStatus
			});
			viewList.Add(ScreenStatus.MODE_SOUBI_HENKOU, new List<UIRemodelView>
			{
				mUIRemodelEquipSlotItems,
				mUIRemodelLeftShipStatus
			});
			viewList.Add(ScreenStatus.MODE_SOUBI_HENKOU_TYPE_SELECT, new List<UIRemodelView>
			{
				mCategoryMenu,
				mUIRemodelCurrentSlot
			});
			viewList.Add(ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT, new List<UIRemodelView>
			{
				mUIRemodelEquipListParent
			});
			viewList.Add(ScreenStatus.MODE_SOUBI_HENKOU_PREVIEW, new List<UIRemodelView>
			{
				mUIRemodelSlotItemChangePreview
			});
			viewList.Add(ScreenStatus.MODE_KINDAIKA_KAISHU, new List<UIRemodelView>
			{
				mUIRemodelLeftShipStatus,
				mUIRemodelModernization
			});
			viewList.Add(ScreenStatus.MODE_KINDAIKA_KAISHU_SOZAI_SENTAKU, new List<UIRemodelView>
			{
				mUIRemodelLeftShipStatus,
				mUIRemodeModernizationShipTargetListParentNew
			});
			viewList.Add(ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN, new List<UIRemodelView>
			{
				mUIRemodelModernizationStartConfirm
			});
			viewList.Add(ScreenStatus.MODE_KAIZO, new List<UIRemodelView>
			{
				mUIRemodelKaizou
			});
		}

		private IEnumerator InitViewsCoroutine(DeckModel[] decks)
		{
			mUIRemodelShipSlider.Init(mKeyController);
			yield return new WaitForEndOfFrame();
			mUIRemodelOtherShipPickerParent.SetActive(isActive: true);
			mUIRemodelOtherShipPickerParent.Initialize(mKeyController);
			yield return new WaitForEndOfFrame();
			mUIRemodelDeckSwitchManager.Init(decks, this, mKeyController, otherEnabled: true);
			yield return new WaitForEndOfFrame();
			allSwitchableViews.Add(mUIRemodelShipSlider);
			allSwitchableViews.Add(mUIRemodelLeftShipStatus);
			allSwitchableViews.Add(mUIRemodelStartRightInfo);
			allSwitchableViews.Add(mUIRemodelEquipSlotItems);
			allSwitchableViews.Add(mUIRemodelModeSelectMenu);
			allSwitchableViews.Add(mUIRemodelDeckSwitchManager);
			allSwitchableViews.Add(mUIRemodelModernization);
			allSwitchableViews.Add(mUIRemodelKaizou);
			allSwitchableViews.Add(mUIRemodelEquipListParent);
			allSwitchableViews.Add(mUIRemodelSlotItemChangePreview);
			allSwitchableViews.Add(mCategoryMenu);
			allSwitchableViews.Add(mUIRemodeModernizationShipTargetListParentNew);
			allSwitchableViews.Add(mUIRemodelModernizationStartConfirm);
			allSwitchableViews.Add(mUIRemodelOtherShipPickerParent);
			allSwitchableViews.Add(commonDeckSwitchManager);
			allSwitchableViews.Add(mUIRemodelCurrentSlot);
			allSwitchableViews.Add(mUIHowTo);
			yield return new WaitForEndOfFrame();
			viewList.Add(ScreenStatus.SELECT_DECK_SHIP, new List<UIRemodelView>
			{
				mUIRemodelDeckSwitchManager,
				mUIRemodelShipSlider,
				mUIRemodelStartRightInfo
			});
			viewList.Add(ScreenStatus.SELECT_OTHER_SHIP, new List<UIRemodelView>
			{
				mUIRemodelDeckSwitchManager,
				mUIRemodelOtherShipPickerParent
			});
			viewList.Add(ScreenStatus.SELECT_SETTING_MODE, new List<UIRemodelView>
			{
				mUIRemodelModeSelectMenu,
				mUIRemodelLeftShipStatus
			});
			viewList.Add(ScreenStatus.MODE_SOUBI_HENKOU, new List<UIRemodelView>
			{
				mUIRemodelEquipSlotItems,
				mUIRemodelLeftShipStatus
			});
			viewList.Add(ScreenStatus.MODE_SOUBI_HENKOU_TYPE_SELECT, new List<UIRemodelView>
			{
				mCategoryMenu,
				mUIRemodelCurrentSlot
			});
			viewList.Add(ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT, new List<UIRemodelView>
			{
				mUIRemodelEquipListParent
			});
			viewList.Add(ScreenStatus.MODE_SOUBI_HENKOU_PREVIEW, new List<UIRemodelView>
			{
				mUIRemodelSlotItemChangePreview
			});
			viewList.Add(ScreenStatus.MODE_KINDAIKA_KAISHU, new List<UIRemodelView>
			{
				mUIRemodelLeftShipStatus,
				mUIRemodelModernization
			});
			viewList.Add(ScreenStatus.MODE_KINDAIKA_KAISHU_SOZAI_SENTAKU, new List<UIRemodelView>
			{
				mUIRemodelLeftShipStatus,
				mUIRemodeModernizationShipTargetListParentNew
			});
			viewList.Add(ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN, new List<UIRemodelView>
			{
				mUIRemodelModernizationStartConfirm
			});
			viewList.Add(ScreenStatus.MODE_KAIZO, new List<UIRemodelView>
			{
				mUIRemodelKaizou
			});
		}

		private IEnumerator StartModernizeAnimation(ShipModelMst targetShipModelMst, int bgID, KeyControl keyController, bool isFail, bool SuperSuccessed, int sozai_count, bool isDamaged, Action OnFinishedAnimation)
		{
			guideoff = true;
			PortUpgradesModernizeShipManager portUpgradesModernizeShipManager = Util.Instantiate(mPrefab_PortUpgradesModernizeShipManager_KindaikakaishuAnimation.gameObject, base.gameObject).GetComponent<PortUpgradesModernizeShipManager>();
			portUpgradesModernizeShipManager.SetKeyController(keyController);
			portUpgradesModernizeShipManager.Initialize(targetShipModelMst, 1, isFail, SuperSuccessed, sozai_count, isDamaged);
			while (!portUpgradesModernizeShipManager.isFinished)
			{
				yield return null;
			}
			if (OnFinishedAnimation != null)
			{
				UnityEngine.Object.Destroy(portUpgradesModernizeShipManager.gameObject);
				Debug.Log("Start OnFinishedAnimation");
				OnFinishedAnimation();
				Debug.Log("End OnFinishedAnimation");
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, delegate
				{
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				});
			}
		}

		private IEnumerator StartGradeUpProductionCoroutine(ShipModel targetShipModel, KeyControl keyController, Action OnFinishedAnimation)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			PortUpgradesConvertShipManager kaizouAnimation = Util.Instantiate(mPrefab_PortUpgradesConvertShipManager_KaizouAnimation.gameObject, base.gameObject).GetComponent<PortUpgradesConvertShipManager>();
			kaizouAnimation.SetKeyController(keyController);
			kaizouAnimation.transform.localPosition = new Vector3(0f, 0f, 0f);
			kaizouAnimation.transform.localScale = new Vector3(1f, 1f, 1f);
			kaizouAnimation.enabled = true;
			kaizouAnimation.Initialize(targetShipModel, 2, 5, 1);
			while (!kaizouAnimation.isFinished)
			{
				yield return null;
			}
			if (OnFinishedAnimation != null)
			{
				UnityEngine.Object.Destroy(kaizouAnimation.gameObject);
				OnFinishedAnimation();
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, delegate
				{
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				});
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref mTextures_Preload);
			UserInterfacePortManager.ReleaseUtils.Release(ref backGroundUpperSteelFrame);
			UserInterfacePortManager.ReleaseUtils.Release(ref backGroundLowerSteelFrame);
			mCamera_TouchEventCatch = null;
			mPrefab_PortUpgradesConvertShipManager_KaizouAnimation = null;
			mPrefab_PortUpgradesModernizeShipManager_KindaikakaishuAnimation = null;
			mUIRemodelHeader = null;
			mUIRemodelShipSlider = null;
			mUIRemodelShipStatus = null;
			mUIRemodelLeftShipStatus = null;
			mUIRemodelStartRightInfo = null;
			mUIRemodelEquipSlotItems = null;
			mUIRemodelModeSelectMenu = null;
			mUIRemodelDeckSwitchManager = null;
			mUIRemodelModernization = null;
			mUIRemodelKaizou = null;
			mUIRemodelEquipListParent = null;
			mUIRemodelSlotItemChangePreview = null;
			mCategoryMenu = null;
			mUIRemodeModernizationShipTargetListParentNew = null;
			mUIRemodelModernizationStartConfirm = null;
			mUIRemodelOtherShipPickerParent = null;
			commonDeckSwitchManager = null;
			mUIRemodelCurrentSlot = null;
			mUIHowTo = null;
			okBauxiteUseMessage = null;
			okBauxiteUseHighCostMessage = null;
			ngBauxiteShortMessage = null;
			ngBausiteShortHighCostMessage = null;
			mUIDisplaySwipeEventRegion = null;
			instance = null;
		}

		public void UpdateHeaderMaterial()
		{
			mUIRemodelHeader.RefreshMaterial(mRemodelManager);
			SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(mRemodelManager);
		}

		private void UpdateHeaderTitle()
		{
			mUIRemodelHeader.RefreshTitle(status, focusedDeckModel);
		}

		private void ShowHeader()
		{
			SingletonMonoBehaviour<UIPortFrame>.Instance.ReqMode(UIPortFrame.FrameMode.Show);
		}

		private void HideHeader()
		{
			SingletonMonoBehaviour<UIPortFrame>.Instance.ReqMode(UIPortFrame.FrameMode.Hide);
		}

		public void OnDeckChange(DeckModel deck)
		{
			ChangeFocusDeck(deck);
			SetStatus(otherShip ? ScreenStatus.SELECT_OTHER_SHIP : ScreenStatus.SELECT_DECK_SHIP);
			UpdateHeaderTitle();
			if (!otherShip)
			{
				SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = deck;
			}
		}

		public bool IsDeckSelectable(int index, DeckModel deck)
		{
			if (deck == null)
			{
				return mRemodelManager.GetOtherShipList().Length > 0;
			}
			return deck.GetShipCount() > 0;
		}

		public bool IsValidShip()
		{
			return mRemodelManager.IsValidShip(focusedShipModel);
		}

		private void OnSwipeDeckListener(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movePercentageX, float movePercentageY, float elapsedTime)
		{
			if (actionType != UIDisplaySwipeEventRegion.ActionType.FingerUp || (status != 0 && status != ScreenStatus.SELECT_OTHER_SHIP))
			{
				return;
			}
			float num = 0.1f;
			if (num < Math.Abs(movePercentageX))
			{
				if (0f < movePercentageX)
				{
					commonDeckSwitchManager.ChangePrevDeck();
				}
				else
				{
					commonDeckSwitchManager.ChangeNextDeck();
				}
			}
		}

		internal void Forward2SoubiHenkouShortCut(SlotitemModel slotitemModel)
		{
			instance.Forward2ModeSelect();
			if (mUIRemodelModeSelectMenu.IsValidSlotItemChange())
			{
				instance.Forward2SoubiHenkou(slotitemModel, requestChangeMode: true);
				instance.Forward2SoubiHenkouTypeSelect();
			}
			else
			{
				mUIRemodelModeSelectMenu.PopUpFailOpenSummary();
			}
		}
	}
}
