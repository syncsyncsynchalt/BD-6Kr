using Common.Enum;
using DG.Tweening;
using KCV.Arsenal;
using KCV.Furniture.JukeBox;
using KCV.PopupString;
using KCV.PortTop;
using KCV.Scene.Marriage;
using KCV.Scene.Others;
using KCV.Strategy;
using KCV.Strategy.Rebellion;
using KCV.Utils;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Port
{
	public class UserInterfacePortManager : MonoBehaviour
	{
		private enum State
		{
			NONE,
			Menu,
			FirstOpenMenu,
			JukeBox,
			PortViewer,
			MarriageConfirm,
			MarriageProduction,
			Option,
			ArsenalTypeSelect
		}

		private enum ShipDepth
		{
			Default,
			HigherPortFrame
		}

		private enum EngageValidation
		{
			NoYubiwa,
			InRepair
		}

		public class StateManager<State>
		{
			private Stack<State> mStateStack;

			private State mEmptyState;

			public Action<State> OnPush
			{
				private get;
				set;
			}

			public Action<State> OnPop
			{
				private get;
				set;
			}

			public Action<State> OnResume
			{
				private get;
				set;
			}

			public Action<State> OnSwitch
			{
				private get;
				set;
			}

			public State CurrentState
			{
				get
				{
					if (0 < mStateStack.Count)
					{
						return mStateStack.Peek();
					}
					return mEmptyState;
				}
			}

			public StateManager(State emptyState)
			{
				mEmptyState = emptyState;
				mStateStack = new Stack<State>();
			}

			public void PushState(State state)
			{
				mStateStack.Push(state);
				Notify(OnPush, mStateStack.Peek());
				Notify(OnSwitch, mStateStack.Peek());
			}

			public void ReplaceState(State state)
			{
				if (0 < mStateStack.Count)
				{
					PopState();
				}
				mStateStack.Push(state);
				Notify(OnPush, mStateStack.Peek());
				Notify(OnSwitch, mStateStack.Peek());
			}

			public void PopState()
			{
				if (0 < mStateStack.Count)
				{
					State state = mStateStack.Pop();
					Notify(OnPop, state);
				}
			}

			public void ResumeState()
			{
				if (0 < mStateStack.Count)
				{
					Notify(OnResume, mStateStack.Peek());
					Notify(OnSwitch, mStateStack.Peek());
				}
			}

			public override string ToString()
			{
				mStateStack.ToArray();
				string text = string.Empty;
				foreach (State item in mStateStack)
				{
					text = item + " > " + text;
				}
				return text;
			}

			private void Notify(Action<State> target, State state)
			{
				target?.Invoke(state);
			}
		}

		public static class ReleaseUtils
		{
			public static void Release(ref UITexture uiTexture, bool unloadUnUsedAsset = false)
			{
				if (uiTexture != null)
				{
					if (uiTexture.mainTexture != null)
					{
						if (unloadUnUsedAsset)
						{
							Resources.UnloadAsset(uiTexture.mainTexture);
						}
						uiTexture.mainTexture = null;
					}
					uiTexture.RemoveFromPanel();
				}
				uiTexture = null;
			}

			public static void Release(ref UILabel uiLabel)
			{
				if (uiLabel != null)
				{
					uiLabel.text = string.Empty;
					uiLabel.RemoveFromPanel();
				}
				uiLabel = null;
			}

			public static void Release(ref Texture texture, bool unloadUnUsedAsset = false)
			{
				if (texture != null && unloadUnUsedAsset)
				{
					Resources.UnloadAsset(texture);
				}
				texture = null;
			}

			internal static void Release(ref UISprite uiSprite)
			{
				if (uiSprite != null)
				{
					uiSprite.spriteName = string.Empty;
					uiSprite.atlas = null;
					uiSprite.RemoveFromPanel();
				}
				uiSprite = null;
			}

			internal static void Release(ref UIPanel uiPanel)
			{
				uiPanel = null;
			}

			internal static void Release(ref UIWidget uiWidget)
			{
				if (uiWidget != null)
				{
					uiWidget.RemoveFromPanel();
				}
				uiWidget = null;
			}

			internal static void Releases(ref UISprite[] uiSprites)
			{
				if (uiSprites != null)
				{
					for (int i = 0; i < uiSprites.Length; i++)
					{
						Release(ref uiSprites[i]);
					}
				}
				uiSprites = null;
			}

			internal static void Releases(ref UIButton[] uiButtons)
			{
				if (uiButtons != null)
				{
					for (int i = 0; i < uiButtons.Length; i++)
					{
						Release(ref uiButtons[i]);
					}
				}
				uiButtons = null;
			}

			internal static void Release(ref UIButton uiButton)
			{
				if (uiButton != null)
				{
					uiButton.Release();
				}
				uiButton = null;
			}

			internal static void Release(ref UILabel[] uiLabels)
			{
				if (uiLabels != null)
				{
					for (int i = 0; i < uiLabels.Length; i++)
					{
						Release(ref uiLabels[i]);
					}
				}
			}

			internal static void Releases(ref Texture[] textures, bool unloadUnUsedAsset = false)
			{
				if (textures != null)
				{
					for (int i = 0; i < textures.Length; i++)
					{
						Release(ref textures[i], unloadUnUsedAsset);
					}
				}
				textures = null;
			}

			public static void Releases(ref GameObject[] gameObjects)
			{
				if (gameObjects != null)
				{
					for (int i = 0; i < gameObjects.Length; i++)
					{
						gameObjects[i] = null;
					}
				}
				gameObjects = null;
			}

			internal static void Release(ref AudioClip audioClip, bool unloadUnUsedAsset = false)
			{
				if ((UnityEngine.Object)audioClip != null && unloadUnUsedAsset)
				{
					Resources.UnloadAsset((UnityEngine.Object)audioClip);
				}
				audioClip = null;
			}

			internal static void Releases(ref ParticleSystem[] particleSystems)
			{
				if (particleSystems != null)
				{
					for (int i = 0; i < particleSystems.Length; i++)
					{
						Release(ref particleSystems[i]);
					}
				}
				particleSystems = null;
			}

			internal static void Release(ref ParticleSystem particleSystem)
			{
				if ((UnityEngine.Object)particleSystem != null)
				{
					Renderer component = ((Component)particleSystem).GetComponent<Renderer>();
					if ((UnityEngine.Object)component != null)
					{
						Material[] materials = component.materials;
						if (materials != null)
						{
							for (int i = 0; i < materials.Length; i++)
							{
								materials[i] = null;
							}
						}
					}
					UnityEngine.Object.DestroyImmediate((UnityEngine.Object)particleSystem);
				}
				particleSystem = null;
			}

			public static void Release(ref Material material, bool immidiate = false)
			{
				material = null;
			}

			internal static void OverwriteCheck()
			{
			}

			internal static void Release(ref CommonShipBanner commonShipBanner, bool unloadAsset = false)
			{
				commonShipBanner.ReleaseShipBannerTexture(unloadAsset);
			}
		}

		private const string PREFAB_PATH_JUKEBOX_MANAGER = "Prefabs/JukeBox/UserInterfaceJukeBoxManager";

		private const string PREFAB_PATH_OPTION_MANAGER = "Prefabs/Others/Option";

		private const string PREFAB_PATH_COMMON_DIALOG_PORT = "Prefabs/Others/CommonDialogPort";

		private const string PREFAB_PATH_MARRIAGE_CUT = "Prefabs/PortTop/MarriageCut";

		private StateManager<State> mStateManager;

		[SerializeField]
		private StrategyShipCharacter mUIShipCharacter;

		[SerializeField]
		private UserInterfacePortInteriorManager mUserInterfacePortInteriorManager;

		[SerializeField]
		private UserInterfacePortMenuManager mUserInterfacePortMenuManager;

		[SerializeField]
		private Blur mBlur_Camera;

		[SerializeField]
		private Transform mTransform_LayerOverlay;

		[SerializeField]
		private Transform mTransform_LayerPort;

		[SerializeField]
		private UIInteriorFurniturePreviewWaiter mUIInteriorFurniturePreviewWaiter;

		[SerializeField]
		private UIPortCameraControlMode mUIPortCameraControlMode;

		[SerializeField]
		private Camera mCamera_Overlay;

		[SerializeField]
		private Camera mCamera_MenuCamera;

		private Option mUserInterfaceOptionManager;

		private CommonDialog mCommonDialog;

		private UIMarriageConfirm mUIMarriageConfirm;

		private UserInterfaceJukeBoxManager mUserInterfaceJukeBoxManager;

		private PortManager mPortManager;

		private DeckModel mDeckModel;

		private KeyControl mKeyController;

		private ParticleSystem mParticleSystem_MarriagePetal;

		private IEnumerator TutorialInstantiate;

		private ShipDepth mShipDepth;

		private void Awake()
		{
			try
			{
				UICamera.mainCamera.GetComponent<UICamera>().allowMultiTouch = false;
			}
			catch (Exception)
			{
				UnityEngine.Debug.Log("Not Found UICamera, Need Arrow MultiTouch = false (Xp)");
			}
		}

		private IEnumerator Start()
		{
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Reset();
			stopWatch.Start();
			mStateManager = new StateManager<State>(State.NONE);
			mStateManager.OnPush = OnPushState;
			mStateManager.OnResume = OnResumeState;
			mStateManager.OnPop = OnPopState;
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			DOTween.Init();
			iTween.Init(base.gameObject);
			mPortManager = new PortManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID);
			mKeyController = new KeyControl();
			mDeckModel = mPortManager.UserInfo.GetDeck(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
			SingletonMonoBehaviour<PortObjectManager>.Instance.setNowScene(Generics.Scene.PortTop.ToString(), isLoadLevel: false);
			SingletonMonoBehaviour<PortObjectManager>.Instance.SceneObject = base.gameObject;
			mUIShipCharacter.ChangeCharacter();
			Transform transform = mUIShipCharacter.transform;
			Vector3 enterPosition = mUIShipCharacter.getEnterPosition();
			transform.localPositionX(enterPosition.x);
			Dictionary<FurnitureKinds, FurnitureModel> furnitures = mPortManager.GetFurnitures(mDeckModel.Id);
			mUserInterfacePortInteriorManager.InitializeFurnitures(mDeckModel, furnitures);
			mUserInterfacePortInteriorManager.SetOnRequestJukeBoxEvent(OnRequestJukeBoxEvent);
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.CircleUpdateInfo(mPortManager);
				SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(mPortManager);
				SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = false;
				SingletonMonoBehaviour<UIPortFrame>.Instance.SetOnClickCircleButtoListener(OnTouchHideMenu);
			}
			mUIInteriorFurniturePreviewWaiter.SetOnBackListener(OnFinishedFurniturePreview);
			mUserInterfacePortMenuManager.SetOnSelectedSceneListener(OnSelectedSceneListener);
			mUserInterfacePortMenuManager.SetOnFirstOpendListener(OnFirstOpendListener);
			mUIPortCameraControlMode.SetOnFinishedModeListener(OnFinishedOfficeModeListener);
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mKeyController.IsRun = false;
			ChangeShipDepth(ShipDepth.HigherPortFrame);
			TutorialInstantiate = TutorialCheck(OnCloseTutorialDialog);
			if (TutorialInstantiate == null)
			{
				mKeyController.IsRun = true;
			}
			else
			{
				mKeyController.IsRun = false;
			}
			stopWatch.Stop();
			for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
			{
				yield return new WaitForEndOfFrame();
			}
			mStateManager.PushState(State.FirstOpenMenu);
		}

		private void OnFirstOpendListener()
		{
			IEnumerator routine = OnFirstOpendListenerCoroutine();
			StartCoroutine(routine);
		}

		private IEnumerator OnFirstOpendListenerCoroutine()
		{
			TutorialModel model = mPortManager.UserInfo.Tutorial;
			if (!model.GetStepTutorialFlg(0))
			{
				model.SetStepTutorialFlg(0);
				CommonPopupDialog.Instance.StartPopup("「旗艦提督室への移動」 達成");
				SoundUtils.PlaySE(SEFIleInfos.SE_012);
			}
			if (TutorialInstantiate != null)
			{
				yield return new WaitForSeconds(0.6f);
				yield return StartCoroutine(TutorialInstantiate);
			}
		}

		private void OnCloseTutorialDialog()
		{
			mKeyController.IsRun = true;
			SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = true;
			mUIShipCharacter.SetEnableBackTouch(isEnable: true);
		}

		private void OnBackJukeBox()
		{
			if (mStateManager.CurrentState == State.JukeBox)
			{
				mKeyController.ClearKeyAll();
				mKeyController.firstUpdate = true;
				mStateManager.PopState();
				mStateManager.ResumeState();
			}
		}

		private void OnRequestJukeBoxEvent()
		{
			bool flag = mStateManager.CurrentState == State.Menu;
			if (flag)
			{
				bool flag2 = mUserInterfacePortMenuManager.GetCurrentState() == UserInterfacePortMenuManager.State.MainMenu;
				flag2 |= (mUserInterfacePortMenuManager.GetCurrentState() == UserInterfacePortMenuManager.State.SubMenu);
				flag = (flag && flag2);
			}
			if (flag || mStateManager.CurrentState == State.PortViewer)
			{
				if (mStateManager.CurrentState == State.Menu)
				{
					mUserInterfacePortMenuManager.StartWaitingState();
					mUserInterfacePortMenuManager.SetKeyController(null);
				}
				else if (mStateManager.CurrentState == State.PortViewer)
				{
					mUIInteriorFurniturePreviewWaiter.StopWait();
					mUIInteriorFurniturePreviewWaiter.SetKeyController(null);
					mUIPortCameraControlMode.SetKeyController(null);
				}
				mStateManager.PushState(State.JukeBox);
			}
		}

		private void OnFinishedFurniturePreview()
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mUIInteriorFurniturePreviewWaiter.StopWait();
			mUIInteriorFurniturePreviewWaiter.gameObject.SetActive(false);
			mUIInteriorFurniturePreviewWaiter.SetKeyController(null);
			mUIPortCameraControlMode.ExitMode();
			mUIPortCameraControlMode.SetKeyController(null);
		}

		private void OnFinishedOfficeModeListener()
		{
			mUIInteriorFurniturePreviewWaiter.StopWait();
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			DOVirtual.Float(0f, 1f, 0.1f, delegate(float alpha)
			{
				mUserInterfacePortMenuManager.alpha = alpha;
				SingletonMonoBehaviour<UIPortFrame>.Instance.alpha = alpha;
			}).SetId(this);
			mStateManager.PopState();
			mStateManager.ResumeState();
			if (mUserInterfacePortMenuManager.GetCurrentState() == UserInterfacePortMenuManager.State.MainMenu && SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Show();
			}
		}

		private void OnArsenalSelectedListener(UIArsenalSelector.SelectType selectedType)
		{
			if (mStateManager.CurrentState == State.ArsenalTypeSelect)
			{
				switch (selectedType)
				{
				case UIArsenalSelector.SelectType.Arsenal:
					SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.Arsenal, isForceFadeOut: true);
					break;
				case UIArsenalSelector.SelectType.Revamp:
					SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.ImprovementArsenal);
					break;
				}
				mStateManager.PushState(State.NONE);
			}
		}

		private void OnSelectedSceneListener(Generics.Scene selectedScene)
		{
			if (mStateManager.CurrentState != State.Menu)
			{
				return;
			}
			ChangeShipDepth(ShipDepth.Default);
			if (selectedScene != Generics.Scene.Option && selectedScene != Generics.Scene.Marriage && SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.ReqFrame(isScreenIn: false);
			}
			switch (selectedScene)
			{
			case Generics.Scene.Strategy:
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.3f, delegate
				{
					UnLoadUnUsedAssets(delegate
					{
						SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.Strategy);
					});
				});
				break;
			case Generics.Scene.Option:
				mStateManager.PushState(State.Option);
				break;
			case Generics.Scene.Marriage:
				mStateManager.PushState(State.MarriageConfirm);
				break;
			case Generics.Scene.SaveLoad:
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("rootType", Generics.Scene.Port);
				RetentionData.SetData(hashtable);
				SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(selectedScene);
				break;
			}
			case Generics.Scene.Arsenal:
				if (mDeckModel.GetFlagShip().ShipType == 19)
				{
					mStateManager.PushState(State.ArsenalTypeSelect);
				}
				else
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(selectedScene);
				}
				break;
			case Generics.Scene.Item:
			case Generics.Scene.Interior:
			case Generics.Scene.Album:
				SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(selectedScene);
				break;
			default:
				if (PortObjectManager.isPrefabSecene(selectedScene))
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(selectedScene);
				}
				else
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(selectedScene);
				}
				break;
			}
		}

		private void UnLoadUnUsedAssets(Action onFinished)
		{
			IEnumerator routine = UnLoadUnUsedAssetsCoroutine(onFinished);
			StartCoroutine(routine);
		}

		private IEnumerator UnLoadUnUsedAssetsCoroutine(Action onFinished)
		{
			yield return null;
			onFinished();
		}

		private void OnPushMarriageConfirmState()
		{
			StartCoroutine(OnPushMarriageConfirmStateCoroutine());
		}

		private IEnumerator OnPushMarriageConfirmStateCoroutine()
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mKeyController.IsRun = false;
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Reset();
			stopWatch.Start();
			if (mCommonDialog == null)
			{
				ResourceRequest requestPrefabCommonDialogPort = Resources.LoadAsync("Prefabs/Others/CommonDialogPort");
				App.OnlyController = new KeyControl();
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: true);
				yield return requestPrefabCommonDialogPort;
				mCommonDialog = Util.Instantiate(requestPrefabCommonDialogPort.asset, mTransform_LayerOverlay.gameObject).GetComponent<CommonDialog>();
				mCommonDialog.SetCameraBlur(mBlur_Camera);
				mUIMarriageConfirm = mCommonDialog.dialogMessages[0].GetComponent<UIMarriageConfirm>();
				App.OnlyController = null;
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: false);
			}
			mCommonDialog.isUseDefaultKeyController = false;
			mUIMarriageConfirm.Initialize(mPortManager.YubiwaNum, mPortManager.YubiwaNum - 1);
			mUIMarriageConfirm.SetOnNegativeListener(OnCancelMarriageConfirm);
			mUIMarriageConfirm.SetOnPositiveListener(OnStartMarriageConfirm);
			mUIMarriageConfirm.SetKeyController(mKeyController);
			stopWatch.Stop();
			for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
			{
				yield return new WaitForEndOfFrame();
			}
			mKeyController.IsRun = true;
			mCommonDialog.OpenDialog(0);
		}

		private void OnCancelMarriageConfirm()
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mUserInterfacePortMenuManager.ResumeState();
			mCommonDialog.CloseDialog();
			mStateManager.PopState();
		}

		private void OnStartMarriageConfirm()
		{
			mPortManager.Marriage(mDeckModel.GetFlagShip().MemId);
			mCommonDialog.CloseDialog();
			mStateManager.PopState();
			mStateManager.PushState(State.MarriageProduction);
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			mKeyController.Update();
			if (mKeyController.keyState[3].down)
			{
				if (mStateManager.CurrentState == State.Menu)
				{
					mKeyController.ClearKeyAll();
					mKeyController.firstUpdate = true;
					RequestPortViewrMode();
				}
			}
			else if (mKeyController.keyState[2].down)
			{
				if (mUserInterfacePortInteriorManager.IsConfigureJukeBox())
				{
					OnRequestJukeBoxEvent();
				}
			}
			else if (!mKeyController.keyState[2].down)
			{
			}
		}

		private void OnPlayMarriageProduction()
		{
		}

		private void OnCancelMarriage()
		{
		}

		private void OnPopState(State state)
		{
			switch (state)
			{
			case State.MarriageConfirm:
			case State.MarriageProduction:
				break;
			case State.Option:
				break;
			case State.PortViewer:
				mUIPortCameraControlMode.SetKeyController(null);
				break;
			case State.JukeBox:
				mUserInterfaceJukeBoxManager.SetKeyController(null);
				mUserInterfaceJukeBoxManager.CloseState();
				break;
			}
		}

		private void OnResumeState(State state)
		{
			switch (state)
			{
			case State.FirstOpenMenu:
			case State.JukeBox:
				break;
			case State.Menu:
				mUserInterfacePortMenuManager.SetKeyController(mKeyController);
				mUserInterfacePortMenuManager.ResumeState();
				break;
			case State.PortViewer:
				mUserInterfacePortMenuManager.alpha = 0f;
				SingletonMonoBehaviour<UIPortFrame>.Instance.alpha = 0f;
				mUIInteriorFurniturePreviewWaiter.gameObject.SetActive(true);
				mUIInteriorFurniturePreviewWaiter.SetKeyController(mKeyController);
				mUIPortCameraControlMode.SetKeyController(mKeyController);
				mUIInteriorFurniturePreviewWaiter.ResumeWait();
				break;
			}
		}

		private List<EngageValidation> EngegeCheck(PortManager portManager, ShipModel shipModel)
		{
			List<EngageValidation> list = new List<EngageValidation>();
			if (mPortManager.YubiwaNum <= 0)
			{
				list.Add(EngageValidation.NoYubiwa);
			}
			if (shipModel.IsInRepair())
			{
				list.Add(EngageValidation.InRepair);
			}
			return list;
		}

		private void OnPushState(State state)
		{
			switch (state)
			{
			case State.Menu:
				OnPushMenuState();
				break;
			case State.FirstOpenMenu:
				OnPushFirstOpenMenuState();
				break;
			case State.PortViewer:
				OnPushPortViewerState();
				break;
			case State.MarriageConfirm:
			{
				if (!mPortManager.IsValidMarriage(mDeckModel.GetFlagShip().MemId))
				{
					break;
				}
				List<EngageValidation> list = EngegeCheck(mPortManager, mDeckModel.GetFlagShip());
				if (list.Count <= 0)
				{
					OnPushMarriageConfirmState();
					break;
				}
				switch (list[0])
				{
				case EngageValidation.NoYubiwa:
					CommonPopupDialog.Instance.StartPopup("ケッコン指輪が必要です");
					break;
				case EngageValidation.InRepair:
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NowRepairing));
					break;
				}
				mKeyController.ClearKeyAll();
				mKeyController.firstUpdate = true;
				mStateManager.PopState();
				mStateManager.ResumeState();
				break;
			}
			case State.MarriageProduction:
				OnPushMarriageProductionState();
				break;
			case State.Option:
				OnPushOptionState();
				break;
			case State.ArsenalTypeSelect:
				OnPushArsenalTypeSelectState();
				break;
			case State.JukeBox:
				OnPushJukeBoxState();
				break;
			}
		}

		private void OnPushFirstOpenMenuState()
		{
			StartCoroutine(OnPushFirstOpenMenuStateCoroutine());
		}

		private IEnumerator OnPushFirstOpenMenuStateCoroutine()
		{
			yield return new WaitForEndOfFrame();
			BGMFileInfos bgmInfo = (BGMFileInfos)mPortManager.UserInfo.GetPortBGMId(mDeckModel.Id);
            var shipModel = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip();

			mUserInterfacePortMenuManager.Initialize(mPortManager, mDeckModel);
			yield return new WaitForEndOfFrame();
			SoundUtils.SwitchBGM(bgmInfo);
			SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(delegate
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: false);
				SingletonMonoBehaviour<UIPortFrame>.Instance.ReqFrame(isScreenIn: true);
				this.PlayPortInVoice(shipModel);
			}, isLockTouchOff: true, isPortFrameColliderEnable: false);
			yield return new WaitForEndOfFrame();
			mUserInterfacePortMenuManager.StartState();
			mStateManager.PopState();
			mStateManager.PushState(State.Menu);
		}

		private void OnPushJukeBoxState()
		{
			StartCoroutine(OnPushJukeBoxStateCoroutine());
		}

		private IEnumerator OnPushJukeBoxStateCoroutine()
		{
			if (mUserInterfacePortInteriorManager.IsConfigureJukeBox())
			{
				ResourceRequest requestPrefabUserInterfaceJukeBoxManger = Resources.LoadAsync("Prefabs/JukeBox/UserInterfaceJukeBoxManager");
				yield return requestPrefabUserInterfaceJukeBoxManger;
				GameObject prefabUserInterfaceJukeBoxManager = requestPrefabUserInterfaceJukeBoxManger.asset as GameObject;
				mUserInterfaceJukeBoxManager = Util.Instantiate(prefabUserInterfaceJukeBoxManager, mTransform_LayerOverlay.gameObject).GetComponent<UserInterfaceJukeBoxManager>();
				mUserInterfaceJukeBoxManager.SetOnBackListener(OnBackJukeBox);
			}
			mUserInterfaceJukeBoxManager.Initialize(mPortManager, mDeckModel.Id, mCamera_Overlay);
			mUserInterfaceJukeBoxManager.SetKeyController(mKeyController);
			mUserInterfaceJukeBoxManager.StartState();
		}

		private void OnPushMenuState()
		{
			if (mDeckModel.GetFlagShip().IsMarriage())
			{
				if ((UnityEngine.Object)mParticleSystem_MarriagePetal == null)
				{
					GameObject original = Resources.Load("Prefabs/Others/MarriagePetal") as GameObject;
					mParticleSystem_MarriagePetal = Util.Instantiate(original, mTransform_LayerPort.gameObject).GetComponent<ParticleSystem>();
				}
				mParticleSystem_MarriagePetal.Stop();
				mParticleSystem_MarriagePetal.Play();
			}
			mUserInterfacePortMenuManager.SetKeyController(mKeyController);
		}

		private void OnPushArsenalTypeSelectState()
		{
			IEnumerator routine = OnPushArsenalTypeSelectStateCoroutine();
			StartCoroutine(routine);
		}

		private IEnumerator OnPushArsenalTypeSelectStateCoroutine()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Reset();
			stopWatch.Start();
			if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
			}
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			GameObject prefabArsenalSelector = Resources.Load("Prefabs/Others/UIArsenalSelector") as GameObject;
			UIArsenalSelector arsenalSelector = Util.Instantiate(prefabArsenalSelector.gameObject, mTransform_LayerOverlay.gameObject).GetComponent<UIArsenalSelector>();
			arsenalSelector.SetOnArsenalSelectedListener(OnArsenalSelectedListener);
			arsenalSelector.Initialize(mDeckModel.GetFlagShip());
			stopWatch.Stop();
			for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
			{
				yield return new WaitForEndOfFrame();
			}
			arsenalSelector.SetKeyController(mKeyController);
			arsenalSelector.Show();
		}

		private void OnPushOptionState()
		{
			StartCoroutine(OnPushOptionStateCoroutine());
		}

		private IEnumerator OnPushOptionStateCoroutine()
		{
			if (mUserInterfaceOptionManager == null)
			{
				Stopwatch stopWatch = new Stopwatch();
				stopWatch.Reset();
				stopWatch.Start();
				ResourceRequest requestPrefabOption = Resources.LoadAsync("Prefabs/Others/Option");
				yield return requestPrefabOption;
				mUserInterfaceOptionManager = Util.Instantiate(requestPrefabOption.asset as GameObject, mTransform_LayerOverlay.gameObject).GetComponent<Option>();
				mUserInterfaceOptionManager.SetActive(isActive: false);
				mUserInterfaceOptionManager.SetOnBackListener(OnBackOptionState);
				stopWatch.Stop();
				for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
				{
					yield return new WaitForEndOfFrame();
				}
			}
			mUserInterfaceOptionManager.gameObject.GetComponent<UIPanel>().alpha = 0.001f;
			mUserInterfaceOptionManager.gameObject.SetActive(true);
			mUserInterfaceOptionManager.SetKeyController(mKeyController);
		}

		private void OnPushMarriageProductionState()
		{
			StartCoroutine(OnPushMarriageProductionStateCoroutine());
		}

		private IEnumerator OnPushMarriageProductionStateCoroutine()
		{
			SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			ResourceRequest requestMarriageCut = Resources.LoadAsync("Prefabs/PortTop/MarriageCut");
			yield return requestMarriageCut;
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Reset();
			stopWatch.Start();
			MarriageCutManager marriageManager = Util.Instantiate(requestMarriageCut.asset, mTransform_LayerOverlay.gameObject).GetComponent<MarriageCutManager>();
			stopWatch.Stop();
			for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
			{
				yield return new WaitForEndOfFrame();
			}
			marriageManager.Initialize(mDeckModel.GetFlagShip(), mKeyController, _compMarriageAnimation);
			yield return StartCoroutine(marriageManager.Play());
		}

		private void _compMarriageAnimation()
		{
			SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
			SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(1f, delegate
			{
				TrophyUtil.Unlock_At_Marriage();
				Application.LoadLevel(Generics.Scene.PortTop.ToString());
			});
		}

		[Obsolete("Inspector上で使用します。")]
		public void OnTouchHideMenu()
		{
			if (mStateManager.CurrentState == State.Menu)
			{
				RequestPortViewrMode();
			}
		}

		[Obsolete("Inspector上で使用します。")]
		public void OnTouchFrontDepthChange()
		{
			if (mUserInterfacePortMenuManager.GetCurrentState() != UserInterfacePortMenuManager.State.CallingNextScene)
			{
				switch (mShipDepth)
				{
				case ShipDepth.Default:
					ChangeShipDepth(ShipDepth.HigherPortFrame);
					break;
				case ShipDepth.HigherPortFrame:
					ChangeShipDepth(ShipDepth.Default);
					break;
				}
			}
		}

		[Obsolete("Inspector上で使用します。")]
		public void OnTouchShowMenu()
		{
		}

		private void RequestPortViewrMode()
		{
			bool flag = mUserInterfacePortMenuManager.GetCurrentState() == UserInterfacePortMenuManager.State.MainMenu;
			if (flag | (mUserInterfacePortMenuManager.GetCurrentState() == UserInterfacePortMenuManager.State.SubMenu))
			{
				mStateManager.PushState(State.PortViewer);
			}
			if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
			}
		}

		private void OnPushPortViewerState()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			mUserInterfacePortMenuManager.alpha = 0f;
			SingletonMonoBehaviour<UIPortFrame>.Instance.alpha = 0f;
			mUIInteriorFurniturePreviewWaiter.gameObject.SetActive(true);
			mUserInterfacePortMenuManager.StartWaitingState();
			mUserInterfacePortMenuManager.SetKeyController(null);
			mUIInteriorFurniturePreviewWaiter.SetKeyController(mKeyController);
			mUIInteriorFurniturePreviewWaiter.StartWait();
			mUIPortCameraControlMode.Init();
			mUIPortCameraControlMode.SetKeyController(mKeyController);
		}

		private void OnBackOptionState()
		{
			TweenAlpha.Begin(mUserInterfaceOptionManager.gameObject, 0.01f, 0f);
			mUserInterfaceOptionManager.SetKeyController(null);
			mUserInterfaceOptionManager.gameObject.SetActive(false);
			mStateManager.PopState();
			mStateManager.ResumeState();
		}

		private void PlayPortInVoice(ShipModelMst shipModel)
		{
			if (App.rand.Next(0, 2) == 0)
			{
				int voiceNum = App.rand.Next(2, 4);
				ShipUtils.PlayShipVoice(shipModel, voiceNum);
			}
		}

		private IEnumerator TutorialCheck(Action OnFinish)
		{
			TutorialModel tutorial = mPortManager.UserInfo.Tutorial;
			bool[] array = new bool[3]
			{
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckFirstTutorial(tutorial, TutorialGuideManager.TutorialID.PortTopText),
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckFirstTutorial(tutorial, TutorialGuideManager.TutorialID.RepairInfo),
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckFirstTutorial(tutorial, TutorialGuideManager.TutorialID.SupplyInfo)
			};
			if (array[0] || array[1] || array[2])
			{
				return StepTutorialInstantiateForWaitFirstTutorial(tutorial, OnFinish);
			}
			StepTutorialInstantiate(tutorial);
			OnFinish?.Invoke();
			return null;
		}

		private void ChangeShipDepth(ShipDepth shipDepth)
		{
			mShipDepth = shipDepth;
			switch (mShipDepth)
			{
			case ShipDepth.Default:
				mCamera_MenuCamera.depth = 1.2f;
				break;
			case ShipDepth.HigherPortFrame:
				mCamera_MenuCamera.depth = 2f;
				break;
			}
		}

		private IEnumerator StepTutorialInstantiateForWaitFirstTutorial(TutorialModel model, Action OnFinish)
		{
			SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(model, TutorialGuideManager.TutorialID.PortTopText, null);
			SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(model, TutorialGuideManager.TutorialID.RepairInfo, delegate
			{
				UIRebellionOrgaizeShipBanner component2 = ((Component)SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog().View.FindChild("ShipBanner")).GetComponent<UIRebellionOrgaizeShipBanner>();
				ShipModel[] ships2 = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetShips();
				int num2 = 1;
				for (int j = 0; j < ships2.Length; j++)
				{
					if ((float)(ships2[j].NowHp / ships2[j].MaxHp) < 0.7f)
					{
						num2 = j;
						break;
					}
				}
				component2.SetShipData(ships2[num2], num2 + 1);
			});
			SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(model, TutorialGuideManager.TutorialID.SupplyInfo, delegate
			{
				UIRebellionOrgaizeShipBanner component = ((Component)SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog().View.FindChild("ShipBanner")).GetComponent<UIRebellionOrgaizeShipBanner>();
				ShipModel[] ships = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetShips();
				int num = 1;
				for (int i = 0; i < ships.Length; i++)
				{
					if (ships[i].AmmoRate < 50.0 || ships[i].FuelRate < 50.0)
					{
						num = i;
						break;
					}
				}
				component.SetShipData(ships[num], num + 1);
			});
			while (!(SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog() != null))
			{
				yield return null;
			}
			SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog().OnClosed = delegate
			{
				this.StepTutorialInstantiate(model);                
				if (OnFinish != null)
				{
					OnFinish();
				}
			};
		}

		private bool StepTutorialInstantiate(TutorialModel model)
		{
			if (model.GetStep() == 0 && !model.GetStepTutorialFlg(1))
			{
				Util.Instantiate(Resources.Load("Prefabs/TutorialGuide/TutorialGuide2"), base.gameObject);
				return false;
			}
			if (model.GetStep() == 1 && !model.GetStepTutorialFlg(2))
			{
				Util.Instantiate(Resources.Load("Prefabs/TutorialGuide/TutorialGuide3"), base.gameObject);
				return false;
			}
			if (model.GetStep() == 2 && !model.GetStepTutorialFlg(3))
			{
				Util.Instantiate(Resources.Load("Prefabs/TutorialGuide/TutorialGuide3_2"), base.gameObject);
				return false;
			}
			if (model.GetStep() == 3 && !model.GetStepTutorialFlg(4))
			{
				Util.Instantiate(Resources.Load("Prefabs/TutorialGuide/TutorialGuide4"), base.gameObject);
				return false;
			}
			if (model.GetStep() == 4 && !model.GetStepTutorialFlg(5))
			{
				Util.Instantiate(Resources.Load("Prefabs/TutorialGuide/TutorialGuide5"), base.gameObject);
				return false;
			}
			if (model.GetStep() == 6 && !model.GetStepTutorialFlg(7))
			{
				Util.Instantiate(Resources.Load("Prefabs/TutorialGuide/TutorialGuide7_2"), base.gameObject);
				return false;
			}
			if (model.GetStep() == 7 && !model.GetStepTutorialFlg(8))
			{
				Util.Instantiate(Resources.Load("Prefabs/TutorialGuide/TutorialGuide8_port"), base.gameObject);
				return false;
			}
			return true;
		}

		private void OnDestroy()
		{
			mStateManager = null;
			mUIShipCharacter = null;
			mUserInterfacePortInteriorManager = null;
			mUserInterfacePortMenuManager = null;
			mBlur_Camera = null;
			if ((UnityEngine.Object)mParticleSystem_MarriagePetal != null)
			{
				mParticleSystem_MarriagePetal.Stop();
			}
			mParticleSystem_MarriagePetal = null;
			mTransform_LayerPort = null;
			mTransform_LayerOverlay = null;
			mUIInteriorFurniturePreviewWaiter = null;
			mUIPortCameraControlMode = null;
			mCamera_Overlay = null;
			mCamera_MenuCamera = null;
			mUserInterfaceOptionManager = null;
			mCommonDialog = null;
			mUIMarriageConfirm = null;
			mUserInterfaceJukeBoxManager = null;
			mPortManager = null;
			mDeckModel = null;
			mKeyController = null;
			TutorialInstantiate = null;
		}
	}
}
