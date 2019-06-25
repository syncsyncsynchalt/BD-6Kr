using Common.Enum;
using DG.Tweening;
using KCV.Display;
using KCV.Scene.Strategy;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Strategy
{
	public class TaskStrategyMapSelect : SceneTaskMono, RouletteSelectorHandler
	{
		private const float ANIMATION_TIME = 0.3f;

		[SerializeField]
		private Camera mCamera;

		[SerializeField]
		private Transform mStageSelectRoot;

		[SerializeField]
		private UIStageCover mPrefab_UIStateCover;

		[SerializeField]
		private MapTransitionCutManager mPrefab_MapTransitionCutManager;

		[SerializeField]
		private UIStageConfirm mStageConfirm;

		[SerializeField]
		private Transform mTransform_StageCovers;

		[SerializeField]
		private RouletteSelector mRouletteSelector;

		[SerializeField]
		private UIDisplaySwipeEventRegion mDisplaySwipeRegion;

		[SerializeField]
		private UIGoSortieConfirm DeckInfoConfirm;

		[SerializeField]
		private CommonDialog commonDialog;

		public UnloadAtlas Unloader;

		private Transform mTransform_AnimationTile;

		[SerializeField]
		private UITexture mTexture_sallyBGsky;

		[SerializeField]
		private UITexture mTexture_sallyBGclouds;

		[SerializeField]
		private UITexture mTexture_sallyBGcloudRefl;

		[SerializeField]
		private UITexture mTexture_bgSea;

		[SerializeField]
		private UITexture mTexture_snow;

		private StrategyTopTaskManager mStrategyTopTaskManager;

		private Animation mAnimation_MapObjects;

		private AsyncOperation mAsyncOperation;

		private SortieManager mSortieManager;

		private KeyControl mKeyController;

		private MapModel[] mMapModels;

		private bool mIsFinishedAnimation;

		private int mAreaId;

		[SerializeField]
		private int _CenterIndex;

		private Tweener mTweener;

		private bool isAnimationStarted;

		private int moveEndCount;

		public int CenterIndex
		{
			get
			{
				return _CenterIndex;
			}
			set
			{
				_CenterIndex = value;
			}
		}

		bool RouletteSelectorHandler.IsSelectable(int index)
		{
			return true;
		}

		void RouletteSelectorHandler.OnUpdateIndex(int index, Transform transform)
		{
			Debug.Log("Index:" + index + " Model" + mMapModels[index].Name);
			mStageConfirm.Initialize(mMapModels[index]);
			if (!mAnimation_MapObjects.isPlaying)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
			}
		}

		void RouletteSelectorHandler.OnSelect(int index, Transform transform)
		{
			Debug.Log("RouletteSelectorHandler.OnSelect index:" + index);
		}

		protected override bool Init()
		{
			isAnimationStarted = false;
			mDisplaySwipeRegion.SetEventCatchCamera(mCamera);
			mDisplaySwipeRegion.SetOnSwipeActionJudgeCallBack(OnSwipeAction);
			Transform transform = mStageSelectRoot;
			Vector3 localPosition = mCamera.transform.localPosition;
			float x = localPosition.x;
			Vector3 localPosition2 = mCamera.transform.localPosition;
			transform.localPosition = new Vector3(x, localPosition2.y);
			mAreaId = StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID;
			mStrategyTopTaskManager = StrategyTaskManager.GetStrategyTop();
			mIsFinishedAnimation = false;
			mMapModels = StrategyTopTaskManager.GetLogicManager().SelectArea(mAreaId).Maps;
			mSortieManager = StrategyTopTaskManager.GetLogicManager().SelectArea(mAreaId);
			mKeyController = new KeyControl();
			mKeyController.isLoopIndex = true;
			mKeyController.IsRun = false;
			TweenAlpha.Begin(GameObject.Find("Information Root"), 0.3f, 0f);
			TweenAlpha.Begin(GameObject.Find("Map_BG"), 0.3f, 0f);
			GameObject gameObject = StrategyTopTaskManager.Instance.TileManager.Tiles[mAreaId].getSprite().gameObject;
			mTransform_AnimationTile = Util.Instantiate(gameObject, GameObject.Find("Map Root").gameObject, addParentPos: true).transform;
			mAnimation_MapObjects = ((Component)mTransform_StageCovers).GetComponent<Animation>();
			StartCoroutine(StartSeaAnimationCoroutine());
			IEnumerator routine = StartSeaAnimationCoroutine();
			StartCoroutine(routine);
			mTransform_StageCovers.SetActive(isActive: true);
			((Component)mTransform_StageCovers.Find("UIStageCovers")).GetComponent<UIWidget>().alpha = 0.001f;
			SelectedHexAnimation(delegate
			{
				StartCoroutine(InititalizeStageCovers(delegate
				{
					((Component)mTransform_StageCovers).GetComponent<Animation>().Play("SortieAnimation");
					ShowMaps(mMapModels);
				}));
			});
			if (mAreaId == 2 || mAreaId == 4 || mAreaId == 5 || mAreaId == 6 || mAreaId == 7 || mAreaId == 10 || mAreaId == 14)
			{
				mTexture_sallyBGsky.mainTexture = (Resources.Load("Textures/Strategy/sea2_Sunny_sky") as Texture);
				mTexture_sallyBGclouds.mainTexture = (Resources.Load("Textures/Strategy/sea2_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.mainTexture = (Resources.Load("Textures/Strategy/sea2_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.height = 91;
				mTexture_sallyBGcloudRefl.alpha = 0.25f;
				mTexture_bgSea.mainTexture = (Resources.Load("Textures/Strategy/sea2_Sunny_sea") as Texture);
				mTexture_snow.mainTexture = null;
			}
			else if (mAreaId == 3 || mAreaId == 13)
			{
				mTexture_sallyBGsky.mainTexture = (Resources.Load("Textures/Strategy/sea3_Sunny_sky") as Texture);
				mTexture_sallyBGclouds.mainTexture = (Resources.Load("Textures/Strategy/sea3_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.mainTexture = (Resources.Load("Textures/Strategy/sea3_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.height = 90;
				mTexture_sallyBGcloudRefl.alpha = 0.75f;
				mTexture_bgSea.mainTexture = (Resources.Load("Textures/Strategy/sea3_Sunny_sea") as Texture);
				mTexture_snow.mainTexture = (Resources.Load("Textures/Strategy/sea3_snow") as Texture);
			}
			else if (mAreaId == 15 || mAreaId == 16 || mAreaId == 17)
			{
				mTexture_sallyBGsky.mainTexture = (Resources.Load("Textures/Strategy/sea4_Sunny_sky2") as Texture);
				mTexture_sallyBGclouds.mainTexture = (Resources.Load("Textures/Strategy/sea4_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.mainTexture = (Resources.Load("Textures/Strategy/sea4_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.height = 120;
				mTexture_sallyBGcloudRefl.alpha = 0.25f;
				mTexture_bgSea.mainTexture = (Resources.Load("Textures/Strategy/sea4_Sunny_sea") as Texture);
				mTexture_snow.mainTexture = null;
			}
			else
			{
				mTexture_sallyBGsky.mainTexture = (Resources.Load("Textures/Strategy/sea1_Sunny_sky") as Texture);
				mTexture_sallyBGclouds.mainTexture = (Resources.Load("Textures/Strategy/sea1_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.mainTexture = (Resources.Load("Textures/Strategy/sea1_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.height = 140;
				mTexture_sallyBGcloudRefl.alpha = 0.25f;
				mTexture_bgSea.mainTexture = (Resources.Load("Textures/Strategy/sea1_Sunny_sea") as Texture);
				mTexture_snow.mainTexture = null;
			}
			return true;
		}

		private void OnSwipeAction(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp && !mStageConfirm.Shown)
			{
				if (40f < deltaX)
				{
					mRouletteSelector.MovePrev();
					movedPercentageX = 0f;
				}
				else if (deltaX < -40f)
				{
					mRouletteSelector.MoveNext();
					movedPercentageX = 0f;
				}
				else if (!(deltaY < -40f))
				{
				}
			}
		}

		private IEnumerator StartSeaAnimationCoroutine()
		{
			if (mAreaId == 2 || mAreaId == 4 || mAreaId == 5 || mAreaId == 6 || mAreaId == 7 || mAreaId == 10 || mAreaId == 14)
			{
				mTexture_sallyBGsky.mainTexture = (Resources.Load("Textures/Strategy/sea2_Sunny_sky") as Texture);
				mTexture_sallyBGclouds.mainTexture = (Resources.Load("Textures/Strategy/sea2_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.mainTexture = (Resources.Load("Textures/Strategy/sea2_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.height = 91;
				mTexture_sallyBGcloudRefl.alpha = 0.25f;
				mTexture_bgSea.mainTexture = (Resources.Load("Textures/Strategy/sea2_Sunny_sea") as Texture);
				mTexture_snow.mainTexture = null;
			}
			else if (mAreaId == 3 || mAreaId == 13)
			{
				mTexture_sallyBGsky.mainTexture = (Resources.Load("Textures/Strategy/sea3_Sunny_sky") as Texture);
				mTexture_sallyBGclouds.mainTexture = (Resources.Load("Textures/Strategy/sea3_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.mainTexture = (Resources.Load("Textures/Strategy/sea3_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.height = 90;
				mTexture_sallyBGcloudRefl.alpha = 0.75f;
				mTexture_bgSea.mainTexture = (Resources.Load("Textures/Strategy/sea3_Sunny_sea") as Texture);
				mTexture_snow.mainTexture = (Resources.Load("Textures/Strategy/sea3_snow") as Texture);
			}
			else if (mAreaId == 15 || mAreaId == 16 || mAreaId == 17)
			{
				mTexture_sallyBGsky.mainTexture = (Resources.Load("Textures/Strategy/sea4_Sunny_sky2") as Texture);
				mTexture_sallyBGclouds.mainTexture = (Resources.Load("Textures/Strategy/sea4_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.mainTexture = (Resources.Load("Textures/Strategy/sea4_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.height = 120;
				mTexture_sallyBGcloudRefl.alpha = 0.25f;
				mTexture_bgSea.mainTexture = (Resources.Load("Textures/Strategy/sea4_Sunny_sea") as Texture);
				mTexture_snow.mainTexture = null;
			}
			else
			{
				mTexture_sallyBGsky.mainTexture = (Resources.Load("Textures/Strategy/sea1_Sunny_sky") as Texture);
				mTexture_sallyBGclouds.mainTexture = (Resources.Load("Textures/Strategy/sea1_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.mainTexture = (Resources.Load("Textures/Strategy/sea1_Sunny_clouds") as Texture);
				mTexture_sallyBGcloudRefl.height = 140;
				mTexture_sallyBGcloudRefl.alpha = 0.25f;
				mTexture_bgSea.mainTexture = (Resources.Load("Textures/Strategy/sea1_Sunny_sea") as Texture);
				mTexture_snow.mainTexture = null;
			}
			mTexture_sallyBGsky = ((Component)mTransform_StageCovers.Find("BACKGROUND/sallyBGsky")).GetComponent<UITexture>();
			mTexture_sallyBGclouds = ((Component)mTransform_StageCovers.Find("BACKGROUND/sallyBGclouds")).GetComponent<UITexture>();
			mTexture_sallyBGcloudRefl = ((Component)mTransform_StageCovers.Find("BACKGROUND/sallyBGcloudRefl")).GetComponent<UITexture>();
			mTexture_bgSea = ((Component)mTransform_StageCovers.Find("BACKGROUND/sallyBGsea")).GetComponent<UITexture>();
			mTexture_snow = ((Component)mTransform_StageCovers.Find("BACKGROUND/sallyBGsnow")).GetComponent<UITexture>();
			float cloudSpeed = ((float)Convert.ToInt32(UnityEngine.Random.value > 0.5f) * 2f - 1f) * UnityEngine.Random.Range(0.0015f, 0.004f);
			float snowSpeed = UnityEngine.Random.Range(0.005f, 0.03f);
			Rect sallyBGcloudsRect = default(Rect);
			Rect sallyBGcloudReflRect = default(Rect);
			while (true)
			{
				if (mTexture_sallyBGclouds != null && mTexture_sallyBGcloudRefl != null)
				{
					sallyBGcloudsRect.Set(mTexture_sallyBGclouds.uvRect.x + cloudSpeed * Time.deltaTime, mTexture_sallyBGclouds.uvRect.y, mTexture_sallyBGclouds.uvRect.width, mTexture_sallyBGclouds.uvRect.height);
					sallyBGcloudReflRect.Set(mTexture_sallyBGcloudRefl.uvRect.x + cloudSpeed * Time.deltaTime, mTexture_sallyBGcloudRefl.uvRect.y, mTexture_sallyBGcloudRefl.uvRect.width, mTexture_sallyBGcloudRefl.uvRect.height);
					mTexture_sallyBGclouds.uvRect = sallyBGcloudsRect;
					mTexture_sallyBGcloudRefl.uvRect = sallyBGcloudReflRect;
				}
				if (mTexture_snow != null && mTexture_snow.mainTexture != null)
				{
					mTexture_snow.uvRect = new Rect(mTexture_snow.uvRect.x + 5f * snowSpeed * snowSpeed * Mathf.Cos(Time.time) * Time.deltaTime, mTexture_snow.uvRect.y + (snowSpeed + 10f * snowSpeed * snowSpeed * Mathf.Sin(Time.time)) * Time.deltaTime, mTexture_snow.uvRect.width, mTexture_snow.uvRect.height);
					mTexture_snow.alpha += Mathf.Max(Mathf.Min(0.75f + 0.25f * UnityEngine.Random.value - mTexture_snow.alpha, Time.deltaTime * 1.5f), Time.deltaTime * -1.5f);
				}
				yield return null;
			}
		}

		private void SelectedHexAnimation(Action onFinishedCallBack)
		{
			TweenPosition tweenPosition = TweenPosition.Begin(mTransform_AnimationTile.gameObject, 0.3f, mStrategyTopTaskManager.strategyCamera.transform.localPosition + new Vector3(0f, -78f, 196f));
			TweenRotation.Begin(mTransform_AnimationTile.gameObject, 0.3f, Quaternion.AngleAxis(90f, Vector3.right));
			TweenAlpha.Begin(mTransform_AnimationTile.gameObject, 0.8f, 0f);
			tweenPosition.SetOnFinished(delegate
			{
				if (onFinishedCallBack != null)
				{
					onFinishedCallBack();
				}
			});
		}

		private IEnumerator InititalizeStageCovers(Action onInitialized)
		{
			Transform stageCoversPlace = mTransform_StageCovers.Find("UIStageCovers");
			foreach (Transform child in mRouletteSelector.GetContainer().transform)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
			mStageConfirm.Initialize(mMapModels[0]);
			MapModel[] array = mMapModels;
			foreach (MapModel model in array)
			{
				UIStageCover stageCover = Util.Instantiate(mPrefab_UIStateCover.gameObject, stageCoversPlace.gameObject).GetComponent<UIStageCover>();
				stageCover.Initialize(model);
			}
			yield return new WaitForEndOfFrame();
			onInitialized?.Invoke();
		}

		private void ShowMaps(MapModel[] MapModels)
		{
			MapModel mapModel = MapModels.LastOrDefault((MapModel x) => x.Map_Possible);
			int current = (mapModel != null) ? (mapModel.No - 1) : 0;
			mRouletteSelector.Init(this);
			mRouletteSelector.SetCurrent(current);
			mRouletteSelector.Reposition();
			mRouletteSelector.SetKeyController(mKeyController);
			mRouletteSelector.ScaleForce(0.3f, 1f);
			mRouletteSelector.controllable = true;
		}

		protected override bool UnInit()
		{
			StopCoroutine(StartSeaAnimationCoroutine());
			return true;
		}

		private void UpdateDescription(MapModel mapModel)
		{
			mStageConfirm.Initialize(mapModel);
		}

		private void OnSelectStageCover()
		{
			Transform target = mTransform_StageCovers.Find("UIStageCovers");
			target.DOLocalMoveY(100f, 0.3f);
			Debug.Log("Shown:Show");
			mRouletteSelector.ScaleForce(0.3f, 0f);
			mRouletteSelector.controllable = false;
			mStageConfirm.Show();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		public void OnTouchSelectStageCover()
		{
			if (!mStageConfirm.Shown && mKeyController.IsRun)
			{
				OnSelectStageCover();
			}
		}

		private void OnBackSelectStageCover()
		{
			Transform target = mTransform_StageCovers.Find("UIStageCovers");
			target.DOLocalMoveY(0f, 0.3f).PlayForward();
			mStageConfirm.Hide();
		}

		public void OnTouchBG()
		{
			if (!OnKeyControlCross() && mKeyController.IsRun)
			{
				Close();
			}
		}

		private bool OnKeyControlCircle()
		{
			if (mStageConfirm.Shown && mStageConfirm.mMapModel != null && mStageConfirm.mMapModel.Map_Possible)
			{
				List<IsGoCondition> list = mSortieManager.IsGoSortie(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID, mStageConfirm.mMapModel.MstId);
				if (list.Count != 0)
				{
					CommonPopupDialog.Instance.StartPopup(Util.getCancelReason(list[0]));
					SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
					return true;
				}
				OpenDeckInfo();
			}
			else if (!mStageConfirm.Shown)
			{
				OnSelectStageCover();
			}
			return true;
		}

		private void OpenDeckInfo()
		{
			DeckInfoConfirm.SetKeyController(new KeyControl());
			commonDialog.setOpenAction(delegate
			{
				DeckInfoConfirm.SetKeyController(new KeyControl());
			});
			commonDialog.OpenDialog(2, DialogAnimation.AnimType.FEAD);
			DeckInfoConfirm.Initialize(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck, isConfirm: true);
			DeckInfoConfirm.SetPushYesButton(delegate
			{
				mKeyController.IsRun = false;
				commonDialog.CloseDialog();
				OnStartSortieStage();
				Close();
			});
			DeckInfoConfirm.SetPushNoButton(delegate
			{
				commonDialog.CloseDialog();
			});
		}

		private bool OnStartSortieStage()
		{
			mKeyController.IsRun = false;
			mKeyController.ClearKeyAll();
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			SingletonMonoBehaviour<Live2DModel>.Instance.forceStop();
			SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
			SingletonMonoBehaviour<NowLoadingAnimation>.Instance.isNowLoadingAnimation = true;
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter3);
			DebugUtils.SLog("OnStartSortieStage1");
			mStageConfirm.Hide();
			DebugUtils.SLog("OnStartSortieStage2");
			DOTween.Sequence().Append(mRouletteSelector.transform.DOLocalMoveY(0f, 0.4f).SetEase(Ease.OutBounce)).Join(mRouletteSelector.transform.DOScale(new Vector3(1.6f, 1.6f, 1f), 0.3f));
			ShipUtils.PlayShipVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip(), App.rand.Next(13, 15));
			DebugUtils.SLog("OnStartSortieStage3");
			this.DelayAction(0.5f, delegate
			{
				DebugUtils.SLog("OnStartSortieStage mStageConfirm.ClickAnimation");
				MapModel mMapModel = mStageConfirm.mMapModel;
				RetentionData.SetData(new Hashtable
				{
					{
						"sortieMapManager",
						mSortieManager.GoSortie(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id, mMapModel.MstId)
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
				UnityEngine.Object.Destroy(SingletonMonoBehaviour<PortObjectManager>.Instance.gameObject);
				UnityEngine.Object.Destroy(GameObject.Find("Information Root"));
				UnityEngine.Object.Destroy(GameObject.Find("OverView"));
				StartCoroutine(AsyncLoad());
				MapTransitionCutManager component = Util.Instantiate(mPrefab_MapTransitionCutManager.gameObject, base.transform.root.Find("Map Root").gameObject).GetComponent<MapTransitionCutManager>();
				component.transform.localPosition = mStrategyTopTaskManager.strategyCamera.transform.localPosition + new Vector3(-26.4f, -43f, 496.4f);
				component.Initialize(mMapModel, mAsyncOperation);
				TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
				SingletonMonoBehaviour<AppInformation>.Instance.NextLoadType = AppInformation.LoadType.Ship;
				DebugUtils.SLog("OnStartSortieStage mStageConfirm.ClickAnimation END");
			});
			DebugUtils.SLog("OnStartSortieStage4");
			return false;
		}

		private bool OnKeyControlCross()
		{
			if (!mKeyController.IsRun)
			{
				return false;
			}
			if (mStageConfirm.Shown)
			{
				Debug.Log("Shown:Hide");
				mRouletteSelector.controllable = true;
				mRouletteSelector.ScaleForce(0.3f, 1f);
				OnBackSelectStageCover();
				return true;
			}
			UnityEngine.Object.Destroy(mTransform_AnimationTile.gameObject);
			mTransform_StageCovers.SetActive(isActive: false);
			TweenAlpha.Begin(GameObject.Find("Information Root"), 0.3f, 1f);
			TweenAlpha.Begin(GameObject.Find("Map_BG"), 0.3f, 1f);
			Transform transform = mTransform_StageCovers.Find("UIStageCovers");
			foreach (Transform item in transform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(isEnter: true, null);
			StrategyTopTaskManager.Instance.ShipIconManager.SetVisible(isVisible: true);
			List<int> openTileIDs = StrategyTopTaskManager.Instance.GetAreaMng().tileRouteManager.CreateOpenTileIDs();
			StrategyTopTaskManager.Instance.GetAreaMng().tileRouteManager.UpdateTileRouteState(openTileIDs);
			StrategyTopTaskManager.Instance.TileManager.SetVisible(isVisible: true);
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.CommandMenu);
			return false;
		}

		public void OnTouchStartSortie()
		{
			if (!mRouletteSelector.controllable && !commonDialog.isOpen && !OnKeyControlCircle())
			{
				Close();
			}
		}

		protected override bool Run()
		{
			if (mAnimation_MapObjects.IsPlaying("SortieAnimation"))
			{
				isAnimationStarted = true;
				return true;
			}
			if (!mIsFinishedAnimation)
			{
				if (!isAnimationStarted)
				{
					return true;
				}
				AnimFinished();
			}
			mKeyController.Update();
			if (Input.anyKey)
			{
				if (mKeyController.keyState[1].down)
				{
					return OnKeyControlCircle();
				}
				if (mKeyController.keyState[0].down)
				{
					return OnKeyControlCross();
				}
				if (!mKeyController.keyState[3].down && mKeyController.keyState[5].down)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
				}
			}
			return true;
		}

		private void moveEnd()
		{
			moveEndCount++;
		}

		private IEnumerator AsyncLoad()
		{
			DebugUtils.SLog("AsyncLoad");
			mAsyncOperation = Application.LoadLevelAsync(Generics.Scene.SortieAreaMap.ToString());
			mAsyncOperation.allowSceneActivation = false;
			yield return new WaitForEndOfFrame();
			yield return mAsyncOperation;
		}

		public void AnimFinished()
		{
			mIsFinishedAnimation = true;
			mKeyController.IsRun = true;
		}

		private void OnDestroy()
		{
			mPrefab_MapTransitionCutManager = null;
			mPrefab_UIStateCover = null;
			mTexture_sallyBGsky.mainTexture = null;
			mTexture_sallyBGclouds.mainTexture = null;
			mTexture_sallyBGcloudRefl.mainTexture = null;
			mTexture_bgSea.mainTexture = null;
			mTexture_snow.mainTexture = null;
		}
	}
}
