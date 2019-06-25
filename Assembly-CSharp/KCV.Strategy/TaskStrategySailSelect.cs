using Common.Enum;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace KCV.Strategy
{
	public class TaskStrategySailSelect : SceneTaskMono
	{
		private StrategyShipManager shipIconManager;

		private StrategyInfoManager infoManager;

		private StrategyAreaManager areaManager;

		private GameObject topCamera;

		private StrategyTopTaskManager sttm;

		public KeyControl DeckSelectController;

		private int touchedButtonID;

		private Action FirstPlayVoice;

		public bool isEnableCharacterEnter;

		private int prevDeckID;

		public UILabel turnLabel;

		public UILabel DateLabel;

		private GameObject selectTile;

		private AsyncOperation asyncOpe;

		public Live2DModel Live2DModel;

		private Color TileFocusColor;

		[SerializeField]
		private UIGoSortieConfirm uiGoSortieConfirm;

		[SerializeField]
		private CommonDialog commonDialog;

		[SerializeField]
		private PSVitaMovie movie;

		private StrategyMapManager LogicMng => StrategyTopTaskManager.GetLogicManager();

		public int PrevDeckID => prevDeckID;

		public void sailSelectFirstInit()
		{
			shipIconManager = StrategyTopTaskManager.Instance.ShipIconManager;
			infoManager = StrategyTopTaskManager.Instance.GetInfoMng();
			areaManager = StrategyTopTaskManager.Instance.GetAreaMng();
			TileFocusColor = new Color(25f, 227f, 143f, 1f);
			sttm = StrategyTaskManager.GetStrategyTop();
			DeckSelectController = new KeyControl(0, StrategyTopTaskManager.GetLogicManager().UserInfo.DeckCount - 1);
			DeckSelectController.setChangeValue(0f, 1f, 0f, -1f);
			DeckSelectController.KeyInputInterval = 0.2f;
			DeckSelectController.SilentChangeIndex(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
			Live2DModel = SingletonMonoBehaviour<Live2DModel>.Instance;
			int currentAreaID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
			KeyControlManager.Instance.KeyController.Index = currentAreaID;
			shipIconManager.setShipIcons(StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecks());
			SingletonMonoBehaviour<AppInformation>.Instance.prevStrategyDecks = null;
			FirstPlayVoice = delegate
			{
				StrategyTopTaskManager.Instance.UIModel.Character.PlayVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
			};
			isEnableCharacterEnter = true;
			prevDeckID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID;
		}

		protected override bool Init()
		{
			shipIconManager.changeFocus();
			KeyControlManager.Instance.KeyController = DeckSelectController;
			DeckSelectController.SilentChangeIndex(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id - 1);
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck != null && SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState != 0)
			{
				isEnableCharacterEnter = false;
			}
			if (isEnableCharacterEnter)
			{
				moveCharacterScreen(isEnter: true, FirstPlayVoice);
			}
			FirstPlayVoice = null;
			if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Show();
			}
			if (StrategyTopTaskManager.Instance.TutorialGuide6_2 != null)
			{
				StrategyTopTaskManager.Instance.TutorialGuide6_2.Hide();
			}
			StrategyTopTaskManager.Instance.UIModel.HowToStrategy.SetKeyController(DeckSelectController, StrategyAreaManager.sailKeyController);
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			return true;
		}

		protected override bool UnInit()
		{
			if (StrategyTopTaskManager.Instance != null)
			{
				StrategyTopTaskManager.Instance.UIModel.HowToStrategy.SetKeyController(null, null);
			}
			return true;
		}

		protected override bool Run()
		{
			if (LogicMng == null)
			{
				return true;
			}
			DeckSelectController.Update();
			StrategyAreaManager.sailKeyController.SilentChangeIndex(StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID);
			StrategyAreaManager.sailKeyController.RightStickUpdate();
			return KeyAction();
		}

		private bool KeyAction()
		{
			if (DeckSelectController.IsChangeIndex)
			{
				bool isNext = (DeckSelectController.prevIndexChangeValue == 1) ? true : false;
				SearchAndChangeDeck(isNext, isSeachLocalArea: false);
				if (prevDeckID != SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID)
				{
					changeDeck(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
					StrategyTopTaskManager.Instance.UIModel.Character.PlayVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
					if (StrategyTopTaskManager.Instance.UIModel.Character.shipModel != null)
					{
						StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(isEnter: true, null);
					}
				}
				return true;
			}
			if (StrategyAreaManager.sailKeyController.IsChangeIndex)
			{
				areaManager.UpdateSelectArea(StrategyAreaManager.sailKeyController.Index);
			}
			else if (DeckSelectController.keyState[1].down)
			{
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip() == null)
				{
					GotoOrganize();
				}
				else
				{
					OpenCommandMenu();
				}
			}
			else if (DeckSelectController.keyState[3].down)
			{
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetShipCount() != 0)
				{
					if (prevDeckID != SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID)
					{
						changeDeck(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
						StrategyTopTaskManager.Instance.UIModel.Character.PlayVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
					}
					if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
					{
						SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
					}
					uiGoSortieConfirm.SetKeyController(new KeyControl());
					commonDialog.OpenDialog(2, DialogAnimation.AnimType.FEAD);
					uiGoSortieConfirm.Initialize(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck, isConfirm: false);
					commonDialog.setCloseAction(delegate
					{
						KeyControlManager.Instance.KeyController = DeckSelectController;
						if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
						{
							TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
							SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Show();
						}
					});
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				}
			}
			else if (DeckSelectController.keyState[5].down)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			else if (DeckSelectController.keyState[0].down)
			{
				areaManager.UpdateSelectArea(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID);
			}
			else if (DeckSelectController.keyState[2].down)
			{
				if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
				}
				commonDialog.OpenDialog(4);
				commonDialog.keyController.IsRun = false;
				commonDialog.setOpenAction(delegate
				{
					commonDialog.keyController.IsRun = true;
				});
				commonDialog.ShikakuButtonAction = delegate
				{
					Close();
					StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.TurnEnd);
					StrategyTopTaskManager.GetTurnEnd().TurnEnd();
					if (StrategyTopTaskManager.Instance.TutorialGuide8_1 != null)
					{
						if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
						{
							SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().HideAndDestroy();
						}
						StrategyTopTaskManager.Instance.TutorialGuide8_1.HideAndDestroy();
					}
				};
				commonDialog.BatuButtonAction = delegate
				{
					if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
					{
						SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Show();
					}
				};
			}
			return true;
		}

		public IEnumerator SceneChange()
		{
			while (asyncOpe.progress < 0.9f)
			{
				yield return null;
			}
			asyncOpe.allowSceneActivation = true;
			yield return null;
		}

		public void CloseCommonDialog()
		{
			commonDialog.CloseDialog();
		}

		public void OpenCommandMenu()
		{
			if (StrategyTopTaskManager.GetCommandMenu().CommandMenu.isOpen)
			{
				return;
			}
			changeDeckAreaSelect(StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID);
			StrategyTopTaskManager.Instance.GetInfoMng().ExitInfoPanel();
			if (UnityEngine.Random.Range(0, 3) == 0)
			{
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip() != null && SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState == MissionStates.NONE)
				{
					ShipUtils.PlayShipVoice(SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel, 3);
				}
			}
			else
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.CommandMenu);
			Close();
		}

		public void moveCharacterScreen(bool isEnter, Action Onfinished)
		{
			UIShipCharacter character = StrategyTopTaskManager.Instance.UIModel.Character;
			if (isEnter)
			{
				character.Enter(Onfinished);
			}
			else
			{
				character.Exit(Onfinished);
			}
		}

		public DeckModel changeDeck(int DeckID)
		{
			if (prevDeckID == DeckID)
			{
				return null;
			}
			prevDeckID = DeckID;
			DeckModel deck = LogicMng.UserInfo.GetDeck(DeckID);
			SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckAreaModel = StrategyTopTaskManager.GetLogicManager().Area[deck.AreaId];
			if (deck.GetFlagShip() != null)
			{
				deck.GetFlagShip().IsDamaged();
			}
			StrategyTopTaskManager.Instance.GetInfoMng().changeCharacter(deck);
			StrategyTopTaskManager.Instance.UIModel.Character.setState(deck);
			StrategyTopTaskManager.Instance.GetInfoMng().updateFooterInfo(isUpdateMaterial: false);
			return deck;
		}

		private void changeDeckAreaSelect(int areaID)
		{
			int id = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id;
			DeckModel[] decks = LogicMng.Area[areaID].GetDecks();
			if (decks.Length != 0 && SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID != areaID && !decks.All((DeckModel x) => x.GetFlagShip() == null) && SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck != decks[0])
			{
				prevDeckID = id;
				SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = decks[0];
				changeDeck(decks[0].Id);
				shipIconManager.changeFocus();
			}
		}

		public void SearchAndChangeDeck(bool isNext, bool isSeachLocalArea)
		{
			if (StrategyTopTaskManager.GetLogicManager().UserInfo.DeckCount > 1)
			{
				int id;
				if (isNext)
				{
					id = StrategyTopTaskManager.Instance.UIModel.UIMapManager.ShipIconManager.getNextDeck(DeckSelectController.prevIndex + 1, isSeachLocalArea).Id;
					DeckSelectController.SilentChangeIndex(id - 1);
				}
				else
				{
					id = StrategyTopTaskManager.Instance.UIModel.UIMapManager.ShipIconManager.getPrevDeck(DeckSelectController.prevIndex + 1, isSeachLocalArea).Id;
					DeckSelectController.SilentChangeIndex(id - 1);
				}
				prevDeckID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID;
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck == LogicMng.UserInfo.GetDeck(id))
				{
					return;
				}
				SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = LogicMng.UserInfo.GetDeck(id);
				shipIconManager.changeFocus();
			}
			areaManager.UpdateSelectArea(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID);
		}

		public void GotoOrganize()
		{
			if (base.isRun)
			{
				DeckModel deckModel = StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecks().FirstOrDefault((DeckModel x) => x.Count == 0);
				if (deckModel != null)
				{
					SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = deckModel;
					SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.Organize);
				}
			}
		}

		private void OnDestroy()
		{
		}
	}
}
