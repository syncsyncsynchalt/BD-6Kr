using Common.Enum;
using KCV.Utils;
using local.managers;
using local.models;
using Server_Common;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class TaskRebellionEvent : SceneTaskMono
	{
		[SerializeField]
		private Transform _prefabProdRebellionStart;

		[SerializeField]
		private GameObject _prefabProdAreaCheck;

		[SerializeField]
		private DeckSortieInfoManager deckInfoManager;

		[SerializeField]
		private CommonDialog commonDialog;

		[SerializeField]
		private CommonDialog StrategyDialog;

		[SerializeField]
		private UIGoSortieConfirm GoSortieConfirm;

		[SerializeField]
		private TweenAlpha TweenAlphaRedMask;

		private ProdRebellionStart _prodRebellionStart;

		private ProdRebellionAreaCheck _prodRebellionAreaCheck;

		private int SortieEnableDeckNum;

		private List<DeckModel> AreaDecks;

		private void OnDestroy()
		{
			_prefabProdRebellionStart = null;
			_prodRebellionAreaCheck = null;
			deckInfoManager = null;
			commonDialog = null;
			StrategyDialog = null;
			GoSortieConfirm = null;
			TweenAlphaRedMask = null;
			_prodRebellionStart = null;
			_prodRebellionAreaCheck = null;
			Mem.DelListSafe(ref AreaDecks);
		}

		protected override bool Init()
		{
			this.DelayAction(1.5f, delegate
			{
				StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenOut(null);
			});
			_prodRebellionStart = ProdRebellionStart.Instantiate(((Component)_prefabProdRebellionStart).GetComponent<ProdRebellionStart>(), StrategyTaskManager.GetOverView());
			_prodRebellionAreaCheck = Util.Instantiate(_prefabProdAreaCheck, StrategyTaskManager.GetMapRoot().gameObject).GetComponent<ProdRebellionAreaCheck>();
			_prodRebellionStart.Play(delegate
			{
				setActiveRedMask(isActive: true);
			}).Subscribe(delegate
			{
				_prodRebellionAreaCheck.Play(StrategyRebellionTaskManager.RebellionFromArea, StrategyRebellionTaskManager.RebellionArea, delegate
				{
					StartCoroutine(GoNextSceneAtDeckNum(StrategyTaskManager.GetStrategyRebellion().GetRebellionManager().Decks));
				});
			}).AddTo(base.gameObject);
			return true;
		}

		protected override bool UnInit()
		{
			if (_prodRebellionStart != null && _prodRebellionStart.gameObject != null)
			{
				_prodRebellionStart.gameObject.Discard();
			}
			_prodRebellionStart = null;
			return true;
		}

		protected override bool Run()
		{
			return StrategyUtils.ChkStateRebellionTaskIsRun(StrategyRebellionTaskManagerMode.StrategyRebellionTaskManager_ST);
		}

		private IEnumerator GoNextSceneAtDeckNum(List<DeckModel> areaDecks)
		{
			yield return StartCoroutine(PopupCantSortieDecksDialog(areaDecks));
			if (SortieEnableDeckNum < 1)
			{
				StartCoroutine(NonDeckLose());
			}
			else if (SortieEnableDeckNum == 1)
			{
				StartCoroutine(OneDeckGoSortie());
			}
			else
			{
				StrategyTaskManager.GetStrategyRebellion().ReqMode(StrategyRebellionTaskManagerMode.Organize);
			}
			yield return null;
		}

		private IEnumerator PopupCantSortieDecksDialog(List<DeckModel> areaDecks)
		{
			AreaDecks = areaDecks;
			if (areaDecks.Count == 0)
			{
				SortieEnableDeckNum = 0;
				yield return StartCoroutine(ShowTutorial(0));
				yield break;
			}
			int disableDeckNum = deckInfoManager.Init(areaDecks);
			SortieEnableDeckNum = areaDecks.Count - disableDeckNum;
			if (0 < disableDeckNum)
			{
				yield return new WaitForEndOfFrame();
				commonDialog.OpenDialog(0);
				yield return StartCoroutine(commonDialog.WaitForDialogClose());
				deckInfoManager.transform.parent.SetActive(isActive: false);
			}
			yield return StartCoroutine(ShowTutorial(SortieEnableDeckNum));
		}

		public IEnumerator NonDeckLose()
		{
			Dictionary<int, MapAreaModel> area = StrategyTopTaskManager.GetLogicManager().Area;
			RebellionWinLoseAnimation winloseAnimation = Util.Instantiate(Resources.Load("Prefabs/StrategyPrefab/Rebellion/RebellionWinLose") as GameObject, StrategyTopTaskManager.Instance.UIModel.OverView.gameObject).GetComponent<RebellionWinLoseAnimation>();
			yield return winloseAnimation.StartAnimation(isWin: false);
			int areaID = StrategyRebellionTaskManager.RebellionArea;
			Object.Destroy(winloseAnimation);
			bool isShipExist = StrategyTopTaskManager.GetLogicManager().Area[areaID].GetDecks().Length > 0;
			ShipModel fShip = null;
			if (isShipExist)
			{
				fShip = StrategyTopTaskManager.GetLogicManager().Area[areaID].GetDecks()[0].GetFlagShip();
			}
			StrategyTopTaskManager.Instance.GetAreaMng().setShipMove(isShipExist, fShip);
			Dictionary<int, MapAreaModel> beforeAreas = StrategyTopTaskManager.GetLogicManager().Area;
			int[] beforeIDs = StrategyAreaManager.DicToIntArray(beforeAreas);
			StrategyTaskManager.GetStrategyRebellion().GetRebellionManager().NotGoRebellion();
			StrategyTopTaskManager.CreateLogicManager();
			if (Server_Common.Utils.IsGameOver())
			{
				StrategyTopTaskManager.Instance.GameOver();
				yield break;
			}
			Dictionary<int, MapAreaModel> afterAreas = StrategyTopTaskManager.GetLogicManager().Area;
			int[] afterIDs = StrategyAreaManager.DicToIntArray(afterAreas);
			StrategyTopTaskManager.Instance.GetAreaMng().MakeTogetherCloseTilesList(areaID, beforeIDs, afterIDs);
			setActiveRedMask(isActive: false);
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip() != null)
			{
				StrategyTopTaskManager.Instance.UIModel.Character.ChangeCharacter(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
			}
			StrategyTopTaskManager.Instance.ShipIconManager.setShipIconsState();
			StartCoroutine(StrategyTopTaskManager.Instance.GetAreaMng().CloseArea(areaID, delegate
			{
				this.DelayActionCoroutine(this.StartCoroutine(this.ShowLoseGuide()), delegate
				{
					StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenIn(null);
					if (StrategyTopTaskManager.GetLogicManager().GetRebellionAreaList().Count > 0)
					{
						StrategyRebellionTaskManager.checkRebellionArea();
						StrategyTaskManager.ReqMode(StrategyTaskManager.StrategyTaskManagerMode.Rebellion);
					}
					else
					{
						StrategyTopTaskManager.GetTurnEnd().isRebellion = false;
						SoundUtils.PlayBGM(BGMFileInfos.Strategy, isLoop: true);
						StrategyTaskManager.ReqMode(StrategyTaskManager.StrategyTaskManagerMode.StrategyTaskManagerMode_ST);
					}
				});
			}));
			Close();
			StrategyTaskManager.GetStrategyRebellion().Termination();
			yield return null;
		}

		private IEnumerator OneDeckGoSortie()
		{
			RebellionManager mng = StrategyTaskManager.GetStrategyRebellion().GetRebellionManager();
			List<DeckModel> enableDecks = deckInfoManager.GetSortieEnableDeck(AreaDecks);
			if (enableDecks.Count != 1)
			{
			}
			DeckModel deck = enableDecks[0];
			if (mng.IsGoRebellion(-1, deck.Id, -1, -1))
			{
				StartCoroutine(OpenConfirmDialog(deck));
				GoSortieConfirm.Initialize(deck, isConfirm: true);
				GoSortieConfirm.SetPushYesButton(delegate
				{
					this.StrategyDialog.CloseDialog();
					this.StrategyDialog.setCloseAction(delegate
					{
                        RebellionMapManager rebellionMapManager = mng.GoRebellion(-1, deck.Id, -1, -1);
						MapModel map = rebellionMapManager.Map;
						RetentionData.SetData(new Hashtable
						{
							{
								"rebellionMapManager",
								rebellionMapManager
							},
							{
								"rootType",
								0
							},
							{
								"shipRecoveryType",
								ShipRecoveryType.None
							},
							{
								"escape",
								false
							}
						});
						Object.Destroy(SingletonMonoBehaviour<PortObjectManager>.Instance.gameObject);
						this.StartCoroutine(this.PlayTransition(map, deck));
					});
				});
				GoSortieConfirm.SetPushNoButton(delegate
				{
					this.StrategyDialog.CloseDialog();
					this.StrategyDialog.setCloseAction(delegate
					{
						this.StartCoroutine(this.NonDeckLose());
					});
				});
			}
			else
			{
				if (AreaDecks.Count != 1)
				{
					SingletonMonoBehaviour<AppInformation>.Instance.SlogDraw = true;
				}
				StartCoroutine(NonDeckLose());
			}
			yield return null;
		}

		private IEnumerator PlayTransition(MapModel mapModel, DeckModel deck)
		{
			Camera cam = GameObject.Find("TopCamera").GetComponent<Camera>();
			AsyncOperation asyncOpe = Application.LoadLevelAsync(Generics.Scene.SortieAreaMap.ToString());
			asyncOpe.allowSceneActivation = false;
			yield return null;
			MapTransitionCutManager mtcm = Util.Instantiate(Resources.Load("Prefabs/StrategyPrefab/MapSelect/MapTransitionCut"), base.transform.root.Find("Map Root").gameObject).GetComponent<MapTransitionCutManager>();
			mtcm.transform.localPosition = cam.transform.localPosition + new Vector3(-26.4f, -43f, 496.4f);
			mtcm.Initialize(mapModel, asyncOpe);
			ShipUtils.PlayShipVoice(deck.GetFlagShip(), 14);
		}

		private IEnumerator OpenConfirmDialog(DeckModel deck)
		{
			yield return new WaitForEndOfFrame();
			StrategyDialog.disableBackTouch();
			StrategyDialog.isUseDefaultKeyController = false;
			StrategyDialog.OpenDialog(2);
			StrategyTaskManager.GetStrategyRebellion().keycontrol.IsRun = false;
			StrategyDialog.setOpenAction(delegate
			{
				this.GoSortieConfirm.SetKeyController(new KeyControl());
			});
		}

		public void setActiveRedMask(bool isActive)
		{
			if (isActive)
			{
				TweenAlphaRedMask.SetActive(isActive: true);
				TweenAlphaRedMask.from = 0.5f;
				TweenAlphaRedMask.to = 1f;
				TweenAlphaRedMask.tweenFactor = 0.5f;
				TweenAlphaRedMask.style = UITweener.Style.PingPong;
				TweenAlphaRedMask.duration = 1f;
				TweenAlphaRedMask.PlayForward();
			}
			else
			{
				TweenAlpha.Begin(TweenAlphaRedMask.gameObject, 0.5f, 0f);
				this.DelayAction(0.5f, delegate
				{
					TweenAlphaRedMask.SetActive(isActive: false);
				});
			}
		}

		private IEnumerator ShowTutorial(int enableDeckNum)
		{
			bool TutorialFinished2 = false;
			bool isTutorialShow2 = false;
			TutorialModel model = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
			if (enableDeckNum >= 1)
			{
				isTutorialShow2 = SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(model, TutorialGuideManager.TutorialID.Rebellion_EnableIntercept, delegate
				{
					SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog().OnClosed = delegate
					{
                        TutorialFinished2 = true;
					};
				});
			}
			else if (enableDeckNum == 0)
			{
				isTutorialShow2 = SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(model, TutorialGuideManager.TutorialID.Rebellion_DisableIntercept, delegate
				{
					SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog().OnClosed = delegate
					{
                        TutorialFinished2 = true;
					};
				});
			}
			if (isTutorialShow2)
			{
				while (!TutorialFinished2)
				{
					yield return new WaitForEndOfFrame();
				}
			}
			isTutorialShow2 = false;
			TutorialFinished2 = false;
			if (enableDeckNum >= 2)
			{
				isTutorialShow2 = SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(model, TutorialGuideManager.TutorialID.Rebellion_CombinedFleet, delegate
				{
					SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog().OnClosed = delegate
					{
                        TutorialFinished2 = true;
					};
				});
			}
			if (isTutorialShow2)
			{
				while (!TutorialFinished2)
				{
					yield return new WaitForEndOfFrame();
				}
			}
		}

		private IEnumerator ShowLoseGuide()
		{
			bool TutorialFinished = false;
			TutorialModel model = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
			if (SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(model, TutorialGuideManager.TutorialID.Rebellion_Lose, delegate
			{
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog().OnClosed = delegate
				{
                    TutorialFinished = true;
				};
			}))
			{
				while (!TutorialFinished)
				{
					yield return new WaitForEndOfFrame();
				}
			}
		}
	}
}
