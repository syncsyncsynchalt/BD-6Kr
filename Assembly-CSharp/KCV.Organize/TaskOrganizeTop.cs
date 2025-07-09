using Common.Enum;
using DG.Tweening;
using KCV.Display;
using KCV.Utils;
using local.managers;
using local.models;
using Sony.Vita.Dialog;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Organize
{
	public class TaskOrganizeTop : SceneTaskMono, CommonDeckSwitchHandler
	{
		public enum OrganizeState
		{
			None = -1,
			Top,
			Detail,
			DetailList,
			List,
			System,
			Tender
		}

		protected delegate bool StateController();

		[SerializeField]
		protected UIPanel _bgPanel;

		[SerializeField]
		protected UIPanel _bannerPanel;

		[SerializeField]
		protected UIButton _allUnsetBtn;

		[SerializeField]
		protected UIButton _tenderBtn;

		[SerializeField]
		protected UIButton _fleetNameBtn;

		[SerializeField]
		protected UILabel _fleetNameLabel;

		[SerializeField]
		protected Transform mTransform_TurnEndStamp;

		[SerializeField]
		private UIDisplaySwipeEventRegion _displaySwipeEventRegion;

		[SerializeField]
		private OrganizeDeckChangeArrows deckChangeArrows;

		protected bool isInit;

		protected bool isInitTender;

		protected bool isInitDetail;

		protected bool isInitChangeList;

		protected bool IsCreate;

		protected bool isEnd;

		protected string mEditName = string.Empty;

		protected UISprite deckIcon;

		protected Dictionary<string, StateController> StateControllerDic;

		protected OrganizeBannerManager[] _bannerManager;

		protected static int SystemIndex;

		protected static string prevControlState;

		protected static string changeState;

		private OrganizeState _state;

		public static int BannerIndex;

		public static string controlState;

		public UICamera uiCamera;

		public OrganizeTender TenderManager;

		[SerializeField]
		public CommonDeckSwitchManager deckSwitchManager;

		public static DeckModelBase[] decks;

		public static ShipModel[] allShip;

		public static KeyControl KeyController;

		[SerializeField]
		private UIButtonManager BtnManager;

		public OrganizeState _state2;

		protected OrganizeBannerManager _uiDragDropItem;

		private bool _isDragDrop;

		public bool isControl
		{
			get;
			set;
		}

		public DeckModelBase currentDeck
		{
			get;
			protected set;
		}

		public virtual int GetDeckID()
		{
			return ((DeckModel)currentDeck).Id;
		}

		private new void Start()
		{
			_displaySwipeEventRegion.SetOnSwipeActionJudgeCallBack(OnSwipeEvent);
			StateControllerDic = new Dictionary<string, StateController>();
			StateControllerDic.Add("banner", StateKeyControl_Banner);
			StateControllerDic.Add("system", StateKeyControl_System);
			StateControllerDic.Add("tender", StateKeyControl_Tender);
			Main.Initialise();
		}

		public bool FirstInit()
		{
			if (!IsCreate)
			{
				KeyController = OrganizeTaskManager.GetKeyControl();
				GameObject gameObject = GameObject.Find("OrganizeRoot").gameObject;
				Util.FindParentToChild(ref _bgPanel, gameObject.transform, "BackGround");
				Util.FindParentToChild(ref _bannerPanel, gameObject.transform, "Banner");
				decks = OrganizeTaskManager.Instance.GetLogicManager().MapArea.GetDecks();
				allShip = OrganizeTaskManager.Instance.GetLogicManager().GetShipList();
				BannerIndex = 1;
				SystemIndex = 0;
				OrganizeTaskManager.Instance.GetTopTask().setControlState();
				_bannerManager = new OrganizeBannerManager[6];
				for (int i = 0; i < 6; i++)
				{
					Util.FindParentToChild(ref _bannerManager[i], _bannerPanel.transform, "ShipSlot" + (i + 1));
					_bannerManager[i].init(i + 1, OnCheckDragDropTarget, OnDragDropStart, OnDragDropRelease, OnDragDropEnd);
				}
				Transform parent = _bgPanel.transform.FindChild("SideButtons");
				Util.FindParentToChild(ref _allUnsetBtn, parent, "AllUnsetBtn");
				Util.FindParentToChild(ref _tenderBtn, parent, "TenderBtn");
				Util.FindParentToChild(ref _fleetNameBtn, parent, "DeckNameBtn");
				UIPanel component = ((Component)_bgPanel.transform.FindChild("MiscContainer")).GetComponent<UIPanel>();
				Util.FindParentToChild(ref _fleetNameLabel, component.transform, "DeckNameLabel");
				_fleetNameLabel.supportEncoding = false;
				Util.FindParentToChild(ref deckIcon, component.transform, "DeckIcon");
				DeckModel[] array = new DeckModel[decks.Length];
				decks.CopyTo(array, 0);
				DeckModel deckModel = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
				if (deckModel == null)
				{
					deckModel = array[0];
				}
				deckSwitchManager.Init((OrganizeManager)OrganizeTaskManager.Instance.GetLogicManager(), array, this, KeyController, otherEnabled: false, deckModel);
				deckChangeArrows.UpdateView();
				_bannerManager.ForEach(delegate(OrganizeBannerManager e)
				{
					e.updateBannerWhenShipExist(openAnimation: true);
				});
				UpdateSystemButtons();
				UpdeteDeckIcon();
				IsCreate = true;
				CreateTender();
				_isDragDrop = false;
			}
			return true;
		}

		protected override bool UnInit()
		{
			_isDragDrop = false;
			return true;
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
			_displaySwipeEventRegion = null;
			deckChangeArrows = null;
			deckIcon = null;
			Mem.DelDictionarySafe(ref StateControllerDic);
			_bannerManager = null;
			SystemIndex = 0;
			prevControlState = string.Empty;
			changeState = string.Empty;
			_state = OrganizeState.Top;
			Mem.Del(ref BannerIndex);
			Mem.Del(ref controlState);
			uiCamera = null;
			TenderManager = null;
			currentDeck = null;
			deckSwitchManager = null;
			decks = null;
			allShip = null;
			KeyController = null;
		}

		protected override bool Run()
		{
			Main.Update();
			if (isEnd)
			{
				if (changeState == "detail")
				{
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Detail);
					_state2 = OrganizeState.Detail;
				}
				else if (changeState == "list")
				{
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.List);
					_state2 = OrganizeState.List;
				}
				isEnd = false;
				return false;
			}
			if (controlState != null)
			{
				if (isTenderAnimation())
				{
					return true;
				}
				switch (_state)
				{
				case OrganizeState.Top:
					_state2 = OrganizeState.Top;
					return StateKeyControl_Banner();
				case OrganizeState.System:
					_state2 = OrganizeState.System;
					return StateKeyControl_System();
				case OrganizeState.Tender:
					_state2 = OrganizeState.Tender;
					return StateKeyControl_Tender();
				}
			}
			return true;
		}

		protected bool StateKeyControl_Banner()
		{
			if (_isDragDrop)
			{
				return true;
			}
			deckSwitchManager.keyControlEnable = true;
			if (KeyController.IsMaruDown())
			{
				_bannerManager[BannerIndex - 1].DetailEL(null);
				return true;
			}
			if (KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			else if (KeyController.IsBatuDown())
			{
				BackToPort();
			}
			else if (KeyController.IsShikakuDown())
			{
				AllUnsetBtnEL();
			}
			else if (KeyController.IsDownDown())
			{
				BannerIndex += 2;
				if (BannerIndex >= 7)
				{
					BannerIndex -= 6;
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				UpdateChangeBanner();
			}
			else if (KeyController.IsUpDown())
			{
				BannerIndex -= 2;
				if (BannerIndex <= 0)
				{
					BannerIndex += 6;
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				UpdateChangeBanner();
			}
			else if (KeyController.IsLeftDown())
			{
				BannerIndex--;
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				if (BannerIndex == 0)
				{
					if (IsTenderBtn() || IsAllUnsetBtn())
					{
						SystemIndex = ((!IsTenderBtn()) ? 1 : 0);
					}
					else
					{
						SystemIndex = 2;
					}
					UpdateSystemButtons();
					OrganizeTaskManager.Instance.GetTopTask().setControlState();
				}
				UpdateChangeBanner();
			}
			else if (KeyController.IsRightDown())
			{
				if (BannerIndex >= 6)
				{
					return true;
				}
				BannerIndex++;
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				for (int i = 0; i < 6; i++)
				{
					bool enabled = (BannerIndex - 1 == i) ? true : false;
					_bannerManager[i].UpdateBanner(enabled);
				}
			}
			return true;
		}

		protected bool StateKeyControl_System()
		{
			if (_isDragDrop)
			{
				return true;
			}
			deckSwitchManager.keyControlEnable = true;
			if (KeyController.IsMaruDown())
			{
				if (SystemIndex == 0)
				{
					TenderManager.ShowSelectTender();
					OrganizeTaskManager.Instance.GetTopTask().setControlState();
				}
				else if (SystemIndex == 1)
				{
					AllUnsetBtnEL();
					SystemIndex = 0;
					BannerIndex = 1;
					UpdateChangeBanner();
					OrganizeTaskManager.Instance.GetTopTask().setControlState();
				}
				else
				{
					openDeckNameInput();
				}
				return true;
			}
			if (KeyController.IsBatuDown())
			{
				BackToPort();
			}
			else if (KeyController.IsDownDown())
			{
				if (SystemIndex >= 2)
				{
					return true;
				}
				int systemIndex = SystemIndex;
				SystemIndex++;
				if (SystemIndex == 1 && !IsAllUnsetBtn())
				{
					SystemIndex = 2;
				}
				UpdateSystemButtons();
				if (SystemIndex != systemIndex)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
			else if (KeyController.IsUpDown())
			{
				if (SystemIndex <= 0)
				{
					return true;
				}
				int systemIndex2 = SystemIndex;
				SystemIndex--;
				if (SystemIndex == 1 && !IsAllUnsetBtn())
				{
					SystemIndex = ((!IsTenderBtn()) ? 2 : 0);
				}
				if (SystemIndex == 0 && !IsTenderBtn())
				{
					SystemIndex = (IsAllUnsetBtn() ? 1 : 2);
				}
				UpdateSystemButtons();
				if (SystemIndex != systemIndex2)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
			else if (KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			else if (KeyController.IsRightDown())
			{
				BannerIndex++;
				UpdateSystemButtons();
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				UpdateChangeBanner();
				OrganizeTaskManager.Instance.GetTopTask().setControlState();
			}
			return true;
		}

		protected bool StateKeyControl_Tender()
		{
			if (KeyController.IsMaruDown())
			{
				if (TenderManager.State == OrganizeTender.TenderState.Select)
				{
					TenderManager.ShowUseDialog();
				}
				else if (TenderManager.setIndex2 == 1)
				{
					TenderManager.OtherBackEL(null);
				}
				else
				{
					TenderManager.BtnYesEL(null);
					SystemIndex = 0;
					SetTenderBtn();
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					UpdateChangeBanner();
				}
			}
			else if (KeyController.IsBatuDown())
			{
				if (TenderManager.State == OrganizeTender.TenderState.Select)
				{
					TenderManager.MainBackEL(null);
				}
				else
				{
					TenderManager.OtherBackEL(null);
				}
			}
			else if (KeyController.IsLeftDown())
			{
				FlickLeftSweet();
			}
			else if (KeyController.IsRightDown())
			{
				FlickRightSweet();
			}
			else if (KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			return true;
		}

		public void ShowBanner()
		{
			for (int i = 0; i < 6; i++)
			{
				_bannerManager[i].Show(i);
			}
		}

		public void UpdateAllBannerByChangeShip()
		{
			decks = OrganizeTaskManager.Instance.GetLogicManager().MapArea.GetDecks();
			ShipModel[] ships = currentDeck.GetShips();
			bool flag = false;
			for (int i = 0; i < 6; i++)
			{
				if (ships.Length > i)
				{
					bool flag2 = (!_bannerManager[i].IsSetShip()) ? true : false;
					if (flag2)
					{
						flag = true;
					}
					_bannerManager[i].setBanner(ships[i], flag2, compBannerChange);
				}
				else
				{
					_bannerManager[i].InitBanner(closeAnimation: false);
				}
			}
			if (ships.Length == 0 || !flag)
			{
				compBannerChange();
			}
			UpdateSystemButtons();
		}

		private void compBannerChange()
		{
			isControl = true;
		}

		public void UpdateBanner(int bannerNum)
		{
			decks = OrganizeTaskManager.Instance.GetLogicManager().MapArea.GetDecks();
			ShipModel[] ships = currentDeck.GetShips();
			if (ships.Length > bannerNum - 1)
			{
				_bannerManager[bannerNum - 1].setBanner(ships[bannerNum - 1], openAnimation: true, null);
			}
			else
			{
				_bannerManager[bannerNum - 1].InitBanner(closeAnimation: false);
			}
			UpdateSystemButtons();
		}

		public void UpdateChangeBanner(int bannerNum)
		{
			decks = OrganizeTaskManager.Instance.GetLogicManager().MapArea.GetDecks();
			ShipModel[] ships = currentDeck.GetShips();
			if (ships.Length > bannerNum - 1)
			{
				_bannerManager[bannerNum - 1].ChangeBanner(ships[bannerNum - 1]);
			}
			else
			{
				_bannerManager[bannerNum - 1].InitChangeBanner(closeAnimation: false);
			}
			UpdateSystemButtons();
		}

		public void UpdateAllBannerByRemoveShip(bool allReset)
		{
			decks = OrganizeTaskManager.Instance.GetLogicManager().MapArea.GetDecks();
			ShipModel[] ships = currentDeck.GetShips();
			for (int i = 0; i < 6; i++)
			{
				if (ships.Length > i)
				{
					_bannerManager[i].setBanner(ships[i], openAnimation: false, null);
				}
				else
				{
					_bannerManager[i].InitBanner(allReset || i == ships.Length);
				}
			}
			UpdateSystemButtons();
		}

		public void UpdateAllSelectBanner()
		{
			for (int i = 0; i < 6; i++)
			{
				_bannerManager[i].UpdateBanner((BannerIndex - 1 == i) ? true : false);
			}
			UpdateSystemButtons();
		}

		public void SetAllUnsetBtn()
		{
			if (IsAllUnsetBtn())
			{
				_allUnsetBtn.isEnabled = true;
				if (SystemIndex == 1 && BannerIndex == 0)
				{
					_allUnsetBtn.SetState(UIButtonColor.State.Hover, immediate: true);
					UISelectedObject.SelectedOneButtonZoomUpDown(_allUnsetBtn.gameObject, value: true);
				}
				else
				{
					_allUnsetBtn.SetState(UIButtonColor.State.Normal, immediate: true);
					UISelectedObject.SelectedOneButtonZoomUpDown(_allUnsetBtn.gameObject, value: false);
				}
			}
			else
			{
				_allUnsetBtn.isEnabled = false;
				UISelectedObject.SelectedOneButtonZoomUpDown(_allUnsetBtn.gameObject, value: false);
			}
		}

		public bool IsAllUnsetBtn()
		{
			return OrganizeTaskManager.Instance.GetLogicManager().IsValidUnsetAll(GetDeckID());
		}

		public void SetTenderBtn()
		{
			if (IsTenderBtn())
			{
				_tenderBtn.isEnabled = true;
				if (SystemIndex == 0 && BannerIndex == 0)
				{
					_tenderBtn.SetState(UIButtonColor.State.Hover, immediate: false);
					UISelectedObject.SelectedOneButtonZoomUpDown(_tenderBtn.gameObject, value: true);
				}
				else
				{
					_tenderBtn.SetState(UIButtonColor.State.Normal, immediate: false);
					UISelectedObject.SelectedOneButtonZoomUpDown(_tenderBtn.gameObject, value: false);
				}
			}
			else
			{
				_tenderBtn.isEnabled = false;
				UISelectedObject.SelectedOneButtonZoomUpDown(_tenderBtn.gameObject, value: false);
				if (SystemIndex == 0 && BannerIndex == 0)
				{
					BannerIndex = 1;
					setControlState();
				}
			}
		}

		public void SetFleetNameBtn()
		{
			if (SystemIndex == 2 && BannerIndex == 0)
			{
				_fleetNameBtn.SetState(UIButtonColor.State.Hover, immediate: true);
				UISelectedObject.SelectedOneButtonZoomUpDown(_fleetNameBtn.gameObject, value: true);
			}
			else
			{
				_fleetNameBtn.SetState(UIButtonColor.State.Normal, immediate: true);
				UISelectedObject.SelectedOneButtonZoomUpDown(_fleetNameBtn.gameObject, value: false);
			}
		}

		public bool IsTenderBtn()
		{
			return OrganizeTaskManager.Instance.GetLogicManager().IsValidUseSweets(GetDeckID());
		}

		public void UpdateSort(SortKey nowSortKey)
		{
			switch (OrganizeTaskManager.Instance.GetLogicManager().NowSortKey)
			{
			case SortKey.LEVEL:
				OrganizeTaskManager.Instance.GetLogicManager().ChangeSortKey(SortKey.LEVEL);
				break;
			case SortKey.SHIPTYPE:
				OrganizeTaskManager.Instance.GetLogicManager().ChangeSortKey(SortKey.SHIPTYPE);
				break;
			case SortKey.NEW:
				OrganizeTaskManager.Instance.GetLogicManager().ChangeSortKey(SortKey.NEW);
				break;
			case SortKey.DAMAGE:
				OrganizeTaskManager.Instance.GetLogicManager().ChangeSortKey(SortKey.DAMAGE);
				break;
			}
			allShip = OrganizeTaskManager.Instance.GetLogicManager().GetShipList();
		}

		public void UpdateShipLock(int shipNumber)
		{
			OrganizeTaskManager.Instance.GetLogicManager().Lock(shipNumber);
		}

		public void UpdateChangeBanner()
		{
			for (int i = 0; i < 6; i++)
			{
				_bannerManager[i].UpdateBanner((BannerIndex - 1 == i) ? true : false);
			}
		}

		public void UpdateChangeFatigue()
		{
			UpdateChangeBanner();
			for (int i = 0; i < 6; i++)
			{
				if (currentDeck.GetShips().Length > i)
				{
					_bannerManager[i].UpdateBannerFatigue();
				}
			}
		}

		public void AllUnsetBtnEL()
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled() && IsAllUnsetBtn())
			{
				OrganizeTaskManager.Instance.GetLogicManager().UnsetAllOrganize(GetDeckID());
				OrganizeTaskManager.Instance.GetListTask().UpdateList();
				UpdateAllBannerByRemoveShip(allReset: true);
				UpdateSystemButtons();
				SoundUtils.PlaySE(SEFIleInfos.CommonRemove);
			}
		}

		public void TenderBtnEL()
		{
			CreateTender();
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled() && IsTenderBtn())
			{
				BannerIndex = 0;
				SystemIndex = 0;
				UpdateSystemButtons();
				UpdateChangeBanner();
				TenderManager.ShowSelectTender();
				OrganizeTaskManager.Instance.GetTopTask().setControlState();
				_state = OrganizeState.Tender;
			}
		}

		public void ChangeDeckAnimate(int num)
		{
			UpdateChangeBanner(num);
			_bannerManager[num - 1].UpdateBanner((num - 1 == 0) ? true : false);
		}

		public void BannerPanlAlpha(float from, float to)
		{
			_bannerPanel.SafeGetTweenAlpha(from, to, 0.2f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _bannerPanel.gameObject, string.Empty);
		}

		public void CreateTender()
		{
			if (!isInitTender)
			{
				TenderManager = GameObject.Find("Tender").SafeGetComponent<OrganizeTender>();
				TenderManager.init();
				isInitTender = true;
			}
		}

		public void UpdeteDeckIcon()
		{
			deckIcon.spriteName = "icon_deck" + GetDeckID();
		}

		public void openDeckNameInput()
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled() && base.isRun)
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_001);
				_state = OrganizeState.System;
				controlState = "system";
				BannerIndex = 0;
				SystemIndex = 2;
				UpdateSystemButtons();
				UpdateChangeBanner();
				App.OnlyController = KeyController;
				KeyController.IsRun = false;
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: true);
				this.DelayActionFrame(1, delegate
				{
					Ime.OnGotIMEDialogResult += OnGotIMEDialogResult;
					Ime.Open(new Ime.ImeDialogParams
					{
						supportedLanguages = (Ime.FlagsSupportedLanguages.LANGUAGE_JAPANESE | Ime.FlagsSupportedLanguages.LANGUAGE_ENGLISH_GB),
						languagesForced = true,
						type = Ime.EnumImeDialogType.TYPE_DEFAULT,
						option = Ime.FlagsTextBoxOption.OPTION_DEFAULT,
						canCancel = true,
						textBoxMode = Ime.FlagsTextBoxMode.TEXTBOX_MODE_WITH_CLEAR,
						enterLabel = Ime.EnumImeDialogEnterLabel.ENTER_LABEL_DEFAULT,
						maxTextLength = 12,
						title = "艦隊名を入力してください。（12文字まで）",
						initialText = mEditName
					});
				});
			}
		}

		protected void OnGotIMEDialogResult(Messages.PluginMessage msg)
		{
			Ime.OnGotIMEDialogResult -= OnGotIMEDialogResult;
			Ime.ImeDialogResult result = Ime.GetResult();
			DebugUtils.SLog("OnGotIMEDialogResult2");
			DebugUtils.SLog("OnGotIMEDialogResult3");
			if (result.result == Ime.EnumImeDialogResult.RESULT_OK)
			{
				DebugUtils.SLog("OnGotIMEDialogResult4");
				DebugUtils.SLog("★ IME から得た名前： result.text: " + result.text);
				DebugUtils.SLog("   名前の長さ：result.text.Length: " + result.text.Length);
				DebugUtils.SLog("OnGotIMEDialogResult5");
				mEditName = result.text;
				_fleetNameLabel.text = mEditName;
				DebugUtils.SLog("OnGotIMEDialogResult6");
				OrganizeTaskManager.Instance.GetLogicManager().ChangeDeckName(GetDeckID(), result.text);
				DebugUtils.SLog("OnGotIMEDialogResult7");
			}
			DebugUtils.SLog("OnGotIMEDialogResult8");
			App.OnlyController = null;
			KeyController.IsRun = true;
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: false);
		}

		protected void UpdateDeckName()
		{
			_fleetNameLabel.text = currentDeck.Name;
			mEditName = currentDeck.Name;
		}

		public void BackToPort()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
		}

		public void setChangePhase(string state)
		{
			changeState = state;
			isEnd = true;
		}

		public void compFadeAnimation()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.SetActive(isActive: true);
			AsyncLoadScene.LoadLevelAsyncScene(this, Generics.Scene.PortTop, null);
		}

		protected string han2zen(string value)
		{
			string text = value;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add(" ", "\u3000");
			dictionary.Add("!", "！");
			dictionary.Add("\"", "”");
			dictionary.Add("#", "＃");
			dictionary.Add("$", "＄");
			dictionary.Add("%", "％");
			dictionary.Add("&", "＆");
			dictionary.Add("'", "’");
			dictionary.Add("(", "（");
			dictionary.Add(")", "）");
			dictionary.Add("*", "＊");
			dictionary.Add("+", "＋");
			dictionary.Add(",", "，");
			dictionary.Add("-", "－");
			dictionary.Add(".", "．");
			dictionary.Add("/", "／");
			dictionary.Add("0", "０");
			dictionary.Add("1", "１");
			dictionary.Add("2", "２");
			dictionary.Add("3", "３");
			dictionary.Add("4", "４");
			dictionary.Add("5", "５");
			dictionary.Add("6", "６");
			dictionary.Add("7", "７");
			dictionary.Add("8", "８");
			dictionary.Add("9", "９");
			dictionary.Add(":", "：");
			dictionary.Add(";", "；");
			dictionary.Add("<", "＜");
			dictionary.Add("=", "＝");
			dictionary.Add(">", "＞");
			dictionary.Add("?", "？");
			dictionary.Add("@", "＠");
			dictionary.Add("A", "Ａ");
			dictionary.Add("B", "Ｂ");
			dictionary.Add("C", "Ｃ");
			dictionary.Add("D", "Ｄ");
			dictionary.Add("E", "Ｅ");
			dictionary.Add("F", "Ｆ");
			dictionary.Add("G", "Ｇ");
			dictionary.Add("H", "Ｈ");
			dictionary.Add("I", "Ｉ");
			dictionary.Add("J", "Ｊ");
			dictionary.Add("K", "Ｋ");
			dictionary.Add("L", "Ｌ");
			dictionary.Add("M", "Ｍ");
			dictionary.Add("N", "Ｎ");
			dictionary.Add("O", "Ｏ");
			dictionary.Add("P", "Ｐ");
			dictionary.Add("Q", "Ｑ");
			dictionary.Add("R", "Ｒ");
			dictionary.Add("S", "Ｓ");
			dictionary.Add("T", "Ｔ");
			dictionary.Add("U", "Ｕ");
			dictionary.Add("V", "Ｖ");
			dictionary.Add("W", "Ｗ");
			dictionary.Add("X", "Ｘ");
			dictionary.Add("Y", "Ｙ");
			dictionary.Add("Z", "Ｚ");
			dictionary.Add("[", "［");
			dictionary.Add("\\", "￥");
			dictionary.Add("]", "］");
			dictionary.Add("^", "\uff3e");
			dictionary.Add("_", "\uff3f");
			dictionary.Add("`", "‘");
			dictionary.Add("a", "ａ");
			dictionary.Add("b", "ｂ");
			dictionary.Add("c", "ｃ");
			dictionary.Add("d", "ｄ");
			dictionary.Add("e", "ｅ");
			dictionary.Add("f", "ｆ");
			dictionary.Add("g", "ｇ");
			dictionary.Add("h", "ｈ");
			dictionary.Add("i", "ｉ");
			dictionary.Add("j", "ｊ");
			dictionary.Add("k", "ｋ");
			dictionary.Add("l", "ｌ");
			dictionary.Add("m", "ｍ");
			dictionary.Add("n", "ｎ");
			dictionary.Add("o", "ｏ");
			dictionary.Add("p", "ｐ");
			dictionary.Add("q", "ｑ");
			dictionary.Add("r", "ｒ");
			dictionary.Add("s", "ｓ");
			dictionary.Add("t", "ｔ");
			dictionary.Add("u", "ｕ");
			dictionary.Add("v", "ｖ");
			dictionary.Add("w", "ｗ");
			dictionary.Add("x", "ｘ");
			dictionary.Add("y", "ｙ");
			dictionary.Add("z", "ｚ");
			dictionary.Add("{", "｛");
			dictionary.Add("|", "｜");
			dictionary.Add("}", "｝");
			dictionary.Add("~", "～");
			dictionary.Add("｡", "。");
			dictionary.Add("｢", "「");
			dictionary.Add("｣", "」");
			dictionary.Add("､", "、");
			dictionary.Add("･", "・");
			dictionary.Add("ｶ\uff9e", "ガ");
			dictionary.Add("ｷ\uff9e", "ギ");
			dictionary.Add("ｸ\uff9e", "グ");
			dictionary.Add("ｹ\uff9e", "ゲ");
			dictionary.Add("ｺ\uff9e", "ゴ");
			dictionary.Add("ｻ\uff9e", "ザ");
			dictionary.Add("ｼ\uff9e", "ジ");
			dictionary.Add("ｽ\uff9e", "ズ");
			dictionary.Add("ｾ\uff9e", "ゼ");
			dictionary.Add("ｿ\uff9e", "ゾ");
			dictionary.Add("ﾀ\uff9e", "ダ");
			dictionary.Add("ﾁ\uff9e", "ヂ");
			dictionary.Add("ﾂ\uff9e", "ヅ");
			dictionary.Add("ﾃ\uff9e", "デ");
			dictionary.Add("ﾄ\uff9e", "ド");
			dictionary.Add("ﾊ\uff9e", "バ");
			dictionary.Add("ﾋ\uff9e", "ビ");
			dictionary.Add("ﾌ\uff9e", "ブ");
			dictionary.Add("ﾍ\uff9e", "ベ");
			dictionary.Add("ﾎ\uff9e", "ボ");
			dictionary.Add("ｳ\uff9e", "ヴ");
			dictionary.Add("ﾜ\uff9e", "?");
			dictionary.Add("ｲ\uff9e", "?");
			dictionary.Add("ｴ\uff9e", "?");
			dictionary.Add("ｦ\uff9e", "?");
			dictionary.Add("ﾊ\uff9f", "パ");
			dictionary.Add("ﾋ\uff9f", "ピ");
			dictionary.Add("ﾌ\uff9f", "プ");
			dictionary.Add("ﾍ\uff9f", "ペ");
			dictionary.Add("ﾎ\uff9f", "ポ");
			dictionary.Add("ｦ", "ヲ");
			dictionary.Add("ｧ", "ァ");
			dictionary.Add("ｨ", "ィ");
			dictionary.Add("ｩ", "ゥ");
			dictionary.Add("ｪ", "ェ");
			dictionary.Add("ｫ", "ォ");
			dictionary.Add("ｬ", "ャ");
			dictionary.Add("ｭ", "ュ");
			dictionary.Add("ｮ", "ョ");
			dictionary.Add("ｯ", "ッ");
			dictionary.Add("\uff70", "ー");
			dictionary.Add("ｱ", "ア");
			dictionary.Add("ｲ", "イ");
			dictionary.Add("ｳ", "ウ");
			dictionary.Add("ｴ", "エ");
			dictionary.Add("ｵ", "オ");
			dictionary.Add("ｶ", "カ");
			dictionary.Add("ｷ", "キ");
			dictionary.Add("ｸ", "ク");
			dictionary.Add("ｹ", "ケ");
			dictionary.Add("ｺ", "コ");
			dictionary.Add("ｻ", "サ");
			dictionary.Add("ｼ", "シ");
			dictionary.Add("ｽ", "ス");
			dictionary.Add("ｾ", "セ");
			dictionary.Add("ｿ", "ソ");
			dictionary.Add("ﾀ", "タ");
			dictionary.Add("ﾁ", "チ");
			dictionary.Add("ﾂ", "ツ");
			dictionary.Add("ﾃ", "テ");
			dictionary.Add("ﾄ", "ト");
			dictionary.Add("ﾅ", "ナ");
			dictionary.Add("ﾆ", "ニ");
			dictionary.Add("ﾇ", "ヌ");
			dictionary.Add("ﾈ", "ネ");
			dictionary.Add("ﾉ", "ノ");
			dictionary.Add("ﾊ", "ハ");
			dictionary.Add("ﾋ", "ヒ");
			dictionary.Add("ﾌ", "フ");
			dictionary.Add("ﾍ", "ヘ");
			dictionary.Add("ﾎ", "ホ");
			dictionary.Add("ﾏ", "マ");
			dictionary.Add("ﾐ", "ミ");
			dictionary.Add("ﾑ", "ム");
			dictionary.Add("ﾒ", "メ");
			dictionary.Add("ﾓ", "モ");
			dictionary.Add("ﾔ", "ヤ");
			dictionary.Add("ﾕ", "ユ");
			dictionary.Add("ﾖ", "ヨ");
			dictionary.Add("ﾗ", "ラ");
			dictionary.Add("ﾘ", "リ");
			dictionary.Add("ﾙ", "ル");
			dictionary.Add("ﾚ", "レ");
			dictionary.Add("ﾛ", "ロ");
			dictionary.Add("ﾜ", "ワ");
			dictionary.Add("ﾝ", "ン");
			dictionary.Add("\uff9e", "\u309b");
			dictionary.Add("\uff9f", "\u309c");
			Dictionary<string, string> dictionary2 = dictionary;
			foreach (KeyValuePair<string, string> item in dictionary2)
			{
				string text2 = text.Replace(item.Key, item.Value);
				text = text2;
			}
			return text;
		}

		public void setControlState()
		{
			if (TenderManager != null && TenderManager.State != 0)
			{
				controlState = "tender";
				_state = OrganizeState.Tender;
			}
			else if (BannerIndex == 0)
			{
				controlState = "system";
				_state = OrganizeState.System;
			}
			else
			{
				controlState = "banner";
				_state = OrganizeState.Top;
			}
			UpdateDeckSwitchManager();
		}

		public void UpdateSystemButtons()
		{
			SetAllUnsetBtn();
			SetTenderBtn();
			SetFleetNameBtn();
		}

		public void OnDeckChange(DeckModel deck)
		{
			if (currentDeck != null)
			{
				bool flag = GetDeckID() < deck.Id;
				for (int i = 0; i < 6; i++)
				{
					_bannerManager[i].UpdateChangeBanner(enabled: false);
					if (flag)
					{
						_bannerManager[i].DeckChangeAnimetion(isLeft: true);
					}
					else
					{
						_bannerManager[i].DeckChangeAnimetion(isLeft: false);
					}
				}
			}
			currentDeck = deck;
			SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = (DeckModel)currentDeck;
			if (currentDeck != null && deck.IsActionEnd())
			{
				mTransform_TurnEndStamp.SetActive(isActive: true);
				mTransform_TurnEndStamp.DOKill();
				mTransform_TurnEndStamp.DOLocalRotate(new Vector3(0f, 0f, 300f), 0f, RotateMode.FastBeyond360);
				mTransform_TurnEndStamp.DOLocalRotate(new Vector3(0f, 0f, 360f), 0.8f, RotateMode.FastBeyond360).SetEase(Ease.OutBounce);
			}
			else
			{
				mTransform_TurnEndStamp.SetActive(isActive: false);
			}
			ShipModel[] ships = deck.GetShips();
			for (int j = 0; j < 6; j++)
			{
				if (j < ships.Length)
				{
					_bannerManager[j].setShip(ships[j]);
					_bannerManager[j].IsSet = true;
				}
				else
				{
					_bannerManager[j].IsSet = false;
				}
			}
			BannerIndex = 1;
			setControlState();
			UpdateDeckName();
			UpdeteDeckIcon();
			UpdateSystemButtons();
			deckChangeArrows.UpdateView();
		}

		public bool IsDeckSelectable(int index, DeckModel deck)
		{
			return true;
		}

		public void UpdateByModeChanging()
		{
			UpdateDeckSwitchManager();
		}

		public virtual void UpdateDeckSwitchManager()
		{
			OrganizeTaskManager.OrganizePhase phase = OrganizeTaskManager.GetPhase();
			deckSwitchManager.keyControlEnable = (phase == OrganizeTaskManager.OrganizePhase.Phase_ST && controlState != "tender" && !isTenderAnimation());
		}

		private void OnSwipeEvent(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
			{
				if (0.3f < movedPercentageX)
				{
					FlickLeftSweet();
				}
				else if (movedPercentageX < -0.3f)
				{
					FlickRightSweet();
				}
			}
		}

		private void FlickRightSweet()
		{
			if (TenderManager.State == OrganizeTender.TenderState.Select)
			{
				if (OrganizeTaskManager.Instance.GetLogicManager().GetIrakoCount() > 0)
				{
					if (TenderManager.setIndex < 2)
					{
						TenderManager.setIndex++;
					}
					else if (TenderManager.tenderDic[SweetsType.Mamiya])
					{
						TenderManager.setIndex = 0;
					}
					else if (TenderManager.tenderDic[SweetsType.Both])
					{
						TenderManager.setIndex = 1;
					}
					TenderManager.SetMainDialog();
				}
			}
			else if (TenderManager.State == OrganizeTender.TenderState.Maimiya || TenderManager.State == OrganizeTender.TenderState.Irako)
			{
				if (TenderManager.setIndex2 != 1)
				{
					TenderManager.setIndex2++;
					TenderManager.updateSubBtn();
				}
			}
			else if (TenderManager.State == OrganizeTender.TenderState.Twin && TenderManager.setIndex2 != 1)
			{
				TenderManager.setIndex2++;
				TenderManager.updateTwinBtn();
			}
		}

		private void FlickLeftSweet()
		{
			if (TenderManager.State == OrganizeTender.TenderState.Select)
			{
				if (!KeyController.IsRun)
				{
					return;
				}
				if (0 < TenderManager.setIndex)
				{
					if (TenderManager.setIndex == 2)
					{
						if (OrganizeTaskManager.Instance.GetLogicManager().GetMamiyaCount() >= 1)
						{
							TenderManager.setIndex--;
							TenderManager.SetMainDialog();
						}
					}
					else if (TenderManager.setIndex == 1 && TenderManager.tenderDic[SweetsType.Mamiya])
					{
						TenderManager.setIndex--;
						TenderManager.SetMainDialog();
					}
					else if (TenderManager.tenderDic[SweetsType.Irako])
					{
						TenderManager.setIndex = 2;
						TenderManager.SetMainDialog();
					}
				}
				else if (0 < OrganizeTaskManager.Instance.GetLogicManager().GetIrakoCount())
				{
					TenderManager.setIndex = 2;
					TenderManager.SetMainDialog();
				}
			}
			else if (TenderManager.State == OrganizeTender.TenderState.Maimiya || TenderManager.State == OrganizeTender.TenderState.Irako)
			{
				if (TenderManager.setIndex2 != 0)
				{
					TenderManager.setIndex2--;
					TenderManager.updateSubBtn();
				}
			}
			else if (TenderManager.State == OrganizeTender.TenderState.Twin && TenderManager.setIndex2 != 0)
			{
				TenderManager.setIndex2--;
				TenderManager.updateTwinBtn();
			}
		}

		public bool isTenderAnimation()
		{
			return TenderManager != null && TenderManager.isAnimation;
		}

		private bool OnCheckDragDropTarget(OrganizeBannerManager target)
		{
			if (_isDragDrop)
			{
				return false;
			}
			return _uiDragDropItem == null || (target.Equals(_uiDragDropItem) ? true : false);
		}

		private void OnDragDropStart(OrganizeBannerManager target)
		{
			_isDragDrop = true;
			_uiDragDropItem = target;
			deckSwitchManager.keyControlEnable = false;
			if (BannerIndex != target.number)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			BannerIndex = target.number;
			UpdateChangeBanner();
		}

		private bool OnDragDropRelease(OrganizeBannerManager target)
		{
			if (target == null || _uiDragDropItem == null)
			{
				Debug.LogWarning(" DragDrop NULL");
				return false;
			}
			OrganizeManager logicManager = OrganizeTaskManager.logicManager;
			DeckModel currentDeck = deckSwitchManager.currentDeck;
			ShipModel ship = target.ship;
			ShipModel ship2 = _uiDragDropItem.ship;
			if (logicManager.ChangeOrganize(currentDeck.Id, target.number - 1, ship2.MemId))
			{
				target.setBanner(ship2, openAnimation: true, null);
				_uiDragDropItem.setBanner(ship, openAnimation: true, delegate
				{
					OnDragDropEnd();
				});
				OrganizeTaskManager.Instance.GetListTask().UpdateList();
			}
			return true;
		}

		private void OnDragDropEnd()
		{
			_isDragDrop = false;
			_uiDragDropItem = null;
		}
	}
}
