using KCV.Strategy.Deploy;
using KCV.Strategy.Rebellion;
using KCV.Tutorial.Guide;
using KCV.Utils;
using local.managers;
using local.models;
using local.utils;
using Server_Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyTopTaskManager : SceneTaskMono
	{
		public enum StrategyTopTaskManagerMode
		{
			StrategyTopTaskManagerMode_ST = 0,
			StrategyTopTaskManagerMode_BEF = -1,
			StrategyTopTaskManagerMode_NONE = -1,
			SailSelect = 0,
			CommandMenu = 1,
			ShipMove = 2,
			MapSelect = 3,
			Deploy = 4,
			Debug = 5,
			Info = 6,
			TurnEnd = 7,
			StrategyTopTaskManagerMode_AFT = 8,
			StrategyTopTaskManagerMode_NUM = 8,
			StrategyTopTaskManagerMode_ED = 7
		}

		public delegate void CreateTutorialGuideInstance(TutorialGuide ins);

		[SerializeField]
		private StrategyUIModel uiModel;

		private GameObject InfoLayer;

		public GameObject InfoRoot;

		public StrategyCamera strategyCamera;

		public AsyncObjects InfoAsyncObj;

		public Action initAction;

		public static StrategyTopTaskManager Instance;

		private Coroutine StartCor;

		private SceneTasksMono _clsTasks;

		private static StrategyTopTaskManagerMode _iMode;

		private static StrategyTopTaskManagerMode _iModeReq;

		private static TaskStrategySailSelect _clsSailSelect;

		private static TaskStrategyCommandMenu _clsCommandMenuTask;

		private static TaskStrategyShipMove _clsShipMove;

		private static TaskStrategyMapSelect _clsMapSelect;

		private static TaskDeployTop _clsDeploy;

		private static TaskStrategyAreaInfo _clsAreaInfo;

		private static TaskStrategyDebug _clsDebug;

		private static StrategyTurnEnd _clsTurnEnd;

		private static StrategyMapManager StrategyLogicManager;

		private bool AnimationEnd;

		private GameObject AlertToastCamera;

		public StrategyUIModel UIModel
		{
			get
			{
				return uiModel;
			}
			private set
			{
				uiModel = value;
			}
		}

		public StrategyHexTileManager TileManager => uiModel.UIMapManager.TileManager;

		public StrategyShipManager ShipIconManager => uiModel.UIMapManager.ShipIconManager;

		public TutorialGuide TutorialGuide6_1
		{
			get;
			private set;
		}

		public TutorialGuide TutorialGuide6_2
		{
			get;
			private set;
		}

		public TutorialGuide TutorialGuide8_1
		{
			get;
			private set;
		}

		public TutorialGuide TutorialGuide9_1
		{
			get;
			private set;
		}

		public static TaskStrategySailSelect GetSailSelect()
		{
			return _clsSailSelect;
		}

		public static TaskStrategyCommandMenu GetCommandMenu()
		{
			return _clsCommandMenuTask;
		}

		public static TaskStrategyShipMove GetShipMove()
		{
			return _clsShipMove;
		}

		public static TaskStrategyMapSelect GetMapSelect()
		{
			return _clsMapSelect;
		}

		public static TaskDeployTop GetDeploy()
		{
			return _clsDeploy;
		}

		public static TaskStrategyAreaInfo GetAreaInfoTask()
		{
			return _clsAreaInfo;
		}

		public static TaskStrategyDebug GetDebug()
		{
			return _clsDebug;
		}

		public static void SetDebug(TaskStrategyDebug debug)
		{
			_clsDebug = debug;
		}

		public static StrategyTurnEnd GetTurnEnd()
		{
			return _clsTurnEnd;
		}

		public static StrategyMapManager GetLogicManager()
		{
			return StrategyLogicManager;
		}

		public static void CreateLogicManager()
		{
			StrategyLogicManager = new StrategyMapManager();
		}

		public StrategyInfoManager GetInfoMng()
		{
			return uiModel.InfoManager;
		}

		public StrategyAreaManager GetAreaMng()
		{
			return uiModel.AreaManager;
		}

		protected override void Awake()
		{
			DebugUtils.SLog("+++ StrategyTopTask +++");
			if (AppInitializeManager.IsInitialize)
			{
				StrategyLogicManager = new StrategyMapManager();
				Instance = this;
				GetRef();
			}
		}

		private void OnValidate()
		{
			if (Application.isPlaying)
			{
				GetRef();
			}
		}

		private void GetRef()
		{
			_clsCommandMenuTask = ((Component)base.transform.FindChild("CommandMenu")).GetComponent<TaskStrategyCommandMenu>();
			_clsSailSelect = ((Component)base.transform.FindChild("SailSelect")).GetComponent<TaskStrategySailSelect>();
			_clsShipMove = ((Component)base.transform.FindChild("ShipMove")).GetComponent<TaskStrategyShipMove>();
			_clsMapSelect = ((Component)base.transform.FindChild("MapSelect")).GetComponent<TaskStrategyMapSelect>();
			_clsDeploy = ((Component)base.transform.FindChild("Deploy")).GetComponent<TaskDeployTop>();
			_clsAreaInfo = ((Component)base.transform.FindChild("Record")).GetComponent<TaskStrategyAreaInfo>();
			_clsDebug = ((Component)base.transform.FindChild("Debug")).GetComponent<TaskStrategyDebug>();
			_clsTurnEnd = ((Component)base.transform.FindChild("TurnEnd")).GetComponent<StrategyTurnEnd>();
		}

		private new void Start()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			SingletonMonoBehaviour<FadeCamera>.Instance.SetTransitionRule(FadeCamera.TransitionRule.Transition1);
			this.DelayActionFrame(1, delegate
			{
				int deckID = isNoneCurrentFlagShip() ? 1 : SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id;
				SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = GetSailSelect().changeDeck(deckID);
				GetSailSelect().sailSelectFirstInit();
				GetAreaMng().init();
				GetInfoMng().init();
				SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = true;
				SingletonMonoBehaviour<NowLoadingAnimation>.Instance.isNowLoadingAnimation = false;
				Debug.Log("StrategyTopTaskManager Start");
				GetAreaMng().UpdateSelectArea(StrategyAreaManager.FocusAreaID, immediate: true);
				TileManager.setVisibleFocusObject(isVisible: false);
			});
		}

		private bool isNoneCurrentFlagShip()
		{
			return SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck == null || SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip() == null;
		}

		protected override bool Init()
		{
			_clsTasks = this.SafeGetComponent<SceneTasksMono>();
			if (_clsTasks.tasks == null)
			{
				_clsTasks.Init();
				AnimationEnd = false;
				if (Server_Common.Utils.IsGameClear())
				{
					SoundUtils.StopBGM();
				}
				else
				{
					SoundUtils.SwitchBGM(BGMFileInfos.Strategy);
				}
				StartCor = StartCoroutine(StrategyStart());
			}
			else
			{
				_iMode = (_iModeReq = StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_ST);
			}
			return true;
		}

		protected override bool UnInit()
		{
			return true;
		}

		protected override bool Run()
		{
			if (!AnimationEnd)
			{
				return true;
			}
			if (_clsTasks == null)
			{
				_clsTasks = this.SafeGetComponent<SceneTasksMono>();
			}
			_clsTasks.Run();
			UpdateMode();
			if (StrategyTaskManager.GetMode() != StrategyTaskManager.StrategyTaskManagerMode.StrategyTaskManagerMode_BEF)
			{
				return (StrategyTaskManager.GetMode() == StrategyTaskManager.StrategyTaskManagerMode.StrategyTaskManagerMode_ST) ? true : false;
			}
			return true;
		}

		private void OnDestroy()
		{
			Instance = null;
			uiModel = null;
			_clsCommandMenuTask = null;
			_clsSailSelect = null;
			_clsShipMove = null;
			_clsMapSelect = null;
			_clsDeploy = null;
			_clsAreaInfo = null;
			_clsDebug = null;
			_clsTurnEnd = null;
			StrategyLogicManager = null;
			if (AlertToastCamera != null)
			{
				AlertToastCamera.SetActive(true);
			}
			AlertToastCamera = null;
			TutorialGuide6_1 = null;
			TutorialGuide6_2 = null;
			TutorialGuide8_1 = null;
			TutorialGuide9_1 = null;
			StartCor = null;
		}

		public static StrategyTopTaskManagerMode GetMode()
		{
			return _iModeReq;
		}

		public static void ReqMode(StrategyTopTaskManagerMode iMode)
		{
			_iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (_iModeReq == StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_BEF)
			{
				return;
			}
			switch (_iModeReq)
			{
			case StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_ST:
				if (_clsTasks.Open(_clsSailSelect) < 0)
				{
					return;
				}
				break;
			case StrategyTopTaskManagerMode.CommandMenu:
				if (_clsTasks.Open(_clsCommandMenuTask) < 0)
				{
					return;
				}
				break;
			case StrategyTopTaskManagerMode.ShipMove:
				if (_clsTasks.Open(_clsShipMove) < 0)
				{
					return;
				}
				break;
			case StrategyTopTaskManagerMode.MapSelect:
				if (_clsTasks.Open(_clsMapSelect) < 0)
				{
					return;
				}
				break;
			case StrategyTopTaskManagerMode.Deploy:
				if (_clsTasks.Open(_clsDeploy) < 0)
				{
					return;
				}
				break;
			case StrategyTopTaskManagerMode.Info:
				if (_clsTasks.Open(_clsAreaInfo) < 0)
				{
					return;
				}
				break;
			case StrategyTopTaskManagerMode.Debug:
				if (_clsTasks.Open(_clsDebug) < 0)
				{
					return;
				}
				break;
			case StrategyTopTaskManagerMode.TurnEnd:
				if (_clsTasks.Open(_clsTurnEnd) < 0)
				{
					return;
				}
				break;
			}
			_iMode = _iModeReq;
			_iModeReq = StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_BEF;
		}

		public void ButtonClick()
		{
			int index = KeyControlManager.Instance.KeyController.Index;
		}

		private IEnumerator StrategyStart()
		{
			yield return StartCoroutine(Util.WaitEndOfFrames(3));
			SingletonMonoBehaviour<FadeCamera>.Instance.SetWithOutNowLoading(isWithOut: false);
			if (SingletonMonoBehaviour<FadeCamera>.Instance.isFadeOut)
			{
				bool isFadeIn = false;
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, delegate
				{
                    isFadeIn = true;
				});
				while (!isFadeIn)
				{
					yield return new WaitForEndOfFrame();
				}
			}
			yield return StartCoroutine(Util.WaitEndOfFrames(1));
			Util.FindParentToChild(ref GetCommandMenu().CommandMenu, InfoRoot, "CommandMenu");
			InfoLayer = GameObject.Find("InfoLayer");
			int[] openMaps = GetAreaMng().getNewOpenArea();
			RebellionMapManager rmm = GetAreaMng().checkRebellionResult();
			if (rmm != null)
			{
				TileManager.Tiles[rmm.Map.AreaId].setRebellionTile();
			}
			bool? result = false;
			StartCoroutine(TileManager.StartTilesPopUp(openMaps, delegate(bool r)
			{
                result = r;
			}));
			while (!result.Value)
			{
				yield return new WaitForEndOfFrame();
			}
			if (rmm != null)
			{
				ShipIconManager.unsetShipIconsStateForSupportMission();
			}
			ShipIconManager.popUpShipIcon();
			yield return new WaitForSeconds(0.5f);
			if (Server_Common.Utils.IsGameClear() || TaskStrategyDebug.ForceEnding)
			{
				GameClear();
				TaskStrategyDebug.ForceEnding = false;
				yield break;
			}
			if (rmm != null)
			{
				int areaID = rmm.Map.AreaId;
				bool isShipExist = GetLogicManager().Area[areaID].GetDecks().Length > 0;
				ShipModel fShip = null;
				if (isShipExist)
				{
					fShip = GetLogicManager().Area[areaID].GetDecks()[0].GetFlagShip();
				}
				Instance.GetAreaMng().setShipMove(isShipExist, fShip);
				Dictionary<int, MapAreaModel> beforeAreas = GetLogicManager().Area;
				int[] beforeIDs = StrategyAreaManager.DicToIntArray(beforeAreas);
				rmm.RebellionEnd();
				CreateLogicManager();
				Dictionary<int, MapAreaModel> afterAreas = GetLogicManager().Area;
				int[] afterIDs = StrategyAreaManager.DicToIntArray(afterAreas);
				GetAreaMng().MakeTogetherCloseTilesList(areaID, beforeIDs, afterIDs);
				bool isWin = GetLogicManager().Area[areaID].IsOpen();
				SingletonMonoBehaviour<AppInformation>.Instance.OpenAreaNum = TileManager.OpenAreaNum;
				yield return StartCoroutine(GetAreaMng().RebellionResult(rmm, isWin, areaID));
			}
			if (openMaps != null && openMaps.Length != 0)
			{
				yield return StartCoroutine(GetAreaMng().OpenArea(openMaps));
			}
			List<int> OpenTileIDs = Instance.GetAreaMng().tileRouteManager.CreateOpenTileIDs();
			Instance.GetAreaMng().tileRouteManager.UpdateTileRouteState(OpenTileIDs);
			TileManager.setVisibleFocusObject(isVisible: true);
			yield return StartCoroutine(TutorialCheck());
			GetInfoMng().MoveScreenIn(delegate
			{
				this.AnimationEnd = true;
			});
			if (GetLogicManager().GetRebellionAreaList().Count > 0 && !StrategyRebellionTaskManager.RebellionForceDebug)
			{
				StrategyRebellionTaskManager.checkRebellionArea();
				StrategyTaskManager.ReqMode(StrategyTaskManager.StrategyTaskManagerMode.Rebellion);
				yield break;
			}
			_iMode = (_iModeReq = StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_ST);
			RetentionData.Release();
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: false);
			yield return null;
		}

		public void setActiveStrategy(bool isActive)
		{
			TileManager.setActivePositionAnimations(isActive);
			TileManager.setVisibleFocusObject(isActive);
			uiModel.UIMapManager.ShipIconManager.DeckSelectCursol.SetActive(isActive);
		}

		public IEnumerator TutorialCheck()
		{
			TutorialModel model = GetLogicManager().UserInfo.Tutorial;
			SingletonMonoBehaviour<TutorialGuideManager>.Instance.model = model;
			if (RetentionData.GetData() != null && RetentionData.GetData().ContainsKey("TutorialCancel"))
			{
				for (int j = 0; j < 20; j++)
				{
					model.SetStepTutorialFlg(j);
				}
				for (int i = 0; i < 99; i++)
				{
					model.SetKeyTutorialFlg(i);
				}
			}
			if (model.GetStep() == 4 && !model.GetStepTutorialFlg(5))
			{
				model.SetStepTutorialFlg(5);
			}
			if (model.GetStep() == 5 && !model.GetStepTutorialFlg(6) && GetLogicManager().UserInfo.StartMapCount > 0)
			{
				model.SetStepTutorialFlg(6);
				CommonPopupDialog.Instance.StartPopup("「作戦海域への出撃！」 達成");
				SoundUtils.PlaySE(SEFIleInfos.SE_012);
			}
			if (!model.GetKeyTutorialFlg(10))
			{
				bool isDialogLoaded = false;
				if (!SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(model, TutorialGuideManager.TutorialID.StrategyText, delegate
				{
                    isDialogLoaded = true;
				}))
				{
					yield break;
				}
				while (!isDialogLoaded)
				{
					yield return null;
				}
				if (Instance != null)
				{
					bool isDialogClosed = false;
					SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog().OnClosed = delegate
					{
                        isDialogClosed = true;
					};
					while (!isDialogClosed)
					{
						yield return null;
					}
					yield return StartCoroutine(LoadStepTutorial("Prefabs/TutorialGuide/TutorialGuide1", null));
					yield return Util.WaitEndOfFrames(3);
				}
			}
			else if (!model.GetStepTutorialFlg(0))
			{
				yield return StartCoroutine(LoadStepTutorial("Prefabs/TutorialGuide/TutorialGuide1", null));
			}
			else if (model.GetStep() == 5 && !model.GetStepTutorialFlg(6))
			{
				yield return StartCoroutine(LoadStepTutorial("Prefabs/TutorialGuide/TutorialGuide6_1", delegate(TutorialGuide x)
				{
					this.TutorialGuide6_1 = x;
				}));
			}
			else if (model.GetStep() == 6 && !model.GetStepTutorialFlg(7))
			{
				yield return StartCoroutine(LoadStepTutorial("Prefabs/TutorialGuide/TutorialGuide7_1", null));
			}
			else if (model.GetStep() == 7 && !model.GetStepTutorialFlg(8))
			{
				yield return StartCoroutine(LoadStepTutorial("Prefabs/TutorialGuide/TutorialGuide8_1", delegate(TutorialGuide x)
				{
					this.TutorialGuide8_1 = x;
				}, 8));
			}
			else if (model.GetStep() == 8 && !model.GetStepTutorialFlg(9) && TutorialGuide9_1 == null)
			{
				yield return StartCoroutine(LoadStepTutorial("Prefabs/TutorialGuide/TutorialGuide9_1", delegate(TutorialGuide x)
				{
					this.TutorialGuide9_1 = x;
				}, 9));
			}
		}

		private IEnumerator LoadStepTutorial(string prefabPath, CreateTutorialGuideInstance del, int tutorialID = -1)
		{
			IEnumerator wait = WaitforLoad(prefabPath);
			yield return StartCoroutine(wait);
			UnityEngine.Object obj = (UnityEngine.Object)wait.Current;
			bool isInstantiated = false;
			while (!isInstantiated)
			{
				TutorialGuide tutorialGuide = Util.Instantiate(obj, Instance.gameObject).GetComponent<TutorialGuide>();
				isInstantiated = true;
				if (del != null)
				{
					del(tutorialGuide);
				}
				else
				{
					tutorialGuide = null;
				}
				if (tutorialGuide != null)
				{
					tutorialGuide.tutorialID = tutorialID;
				}
				yield return new WaitForEndOfFrame();
			}
		}

		private IEnumerator WaitforLoad(string prefabPath)
		{
			ResourceRequest req = Resources.LoadAsync(prefabPath);
			yield return req;
			yield return req.asset;
		}

		public void GameOver()
		{
			SoundUtils.StopBGM();
			if (StartCor != null)
			{
				StopCoroutine(StartCor);
			}
			GameObject gameObject = Resources.Load("Prefabs/Ending/EndPhase") as GameObject;
			bool clear = false;
			bool isTurnOver = Server_Common.Utils.IsTurnOver() ? true : false;
			KeyControl keyController = App.OnlyController = new KeyControl();
			SoundUtils.PlaySE(SEFIleInfos.FanfareE);
			ProdEndPhase prodEndPhase = ProdEndPhase.Instantiate(gameObject.GetComponent<ProdEndPhase>(), uiModel.OverView, keyController, clear, isTurnOver);
			prodEndPhase.Play(delegate
			{
				App.OnlyController = null;
				StartCoroutine(GoToTitle());
			});
		}

		private IEnumerator GoToTitle()
		{
			SingletonMonoBehaviour<AppInformation>.Instance.ReleaseSetNo = 3;
			UnityEngine.Object.Destroy(SingletonMonoBehaviour<PortObjectManager>.Instance.gameObject);
			UnityEngine.Object.Destroy(GameObject.Find("SingletonObject").gameObject);
			yield return Resources.UnloadUnusedAssets();
			this.DelayActionFrame(3, delegate
			{
				Application.LoadLevel(Generics.Scene.Title.ToString());
			});
		}

		public void GameClear()
		{
			SoundUtils.StopBGM();
			SoundUtils.StopSEAll(0.5f);
			TrophyUtil.Unlock_At_GameClear();
			StartCoroutine(GameClearCoroutine());
		}

		private IEnumerator GameClearCoroutine()
		{
			GameObject go = Resources.Load("Prefabs/Ending/EndPhase") as GameObject;
			KeyControl key = App.OnlyController = new KeyControl();
			yield return StartCoroutine(Util.WaitEndOfFrames(3));
			yield return StartCoroutine(GetAreaMng().ClearRedSeaColor());
			ProdEndPhase EndPhase = ProdEndPhase.Instantiate(go.GetComponent<ProdEndPhase>(), uiModel.OverView, key, clear: true, isTurnOver: false);
			EndPhase.Play(delegate
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOutNotNowLoading(0.8f, delegate
				{
					this.StartCoroutine(this.GoToEnding());
				});
			});
			yield return null;
		}

		private IEnumerator GoToEnding()
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			UnityEngine.Object.Destroy(GameObject.Find("Live2DRender").gameObject);
			SingletonMonoBehaviour<AppInformation>.Instance.ReleaseSetNo = 3;
			UnityEngine.Object.Destroy(SingletonMonoBehaviour<PortObjectManager>.Instance.gameObject);
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return Resources.UnloadUnusedAssets();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			Application.LoadLevel(Generics.Scene.Ending.ToString());
		}
	}
}
