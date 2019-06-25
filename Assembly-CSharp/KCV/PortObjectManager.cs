using KCV.PopupString;
using KCV.Tutorial.Guide;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public class PortObjectManager : SingletonMonoBehaviour<PortObjectManager>
	{
		[Button("Unload", "Unload", new object[]
		{

		})]
		public int unload;

		[SerializeField]
		private Transform SceneChanger;

		[SerializeField]
		private GameObject PortFramePrefab;

		public PortTransitionManager PortTransition;

		private IEnumerator OnSceneChangeCoroutine;

		private int SceneMoveCount;

		private string nowScene;

		private bool isLoadLevelScene;

		public static Action SceneChangeAct;

		[SerializeField]
		private GameObject[] ScenePrefabs;

		[SerializeField]
		private GameObject[] SceneObjects;

		public GameObject SceneObject;

		private Vector3 furnitureScale;

		private Vector3 furniturePosition;

		public bool isHidePortObject;

		private KeyControl portKeyControl;

		private Dictionary<string, int> SceneNo;

		private TutorialGuide NowTutorialGuide;

		private KeyControl dummyKey;

		private bool[,] ReleaseSet = new bool[4, 8]
		{
			{
				true,
				false,
				false,
				true,
				false,
				true,
				true,
				false
			},
			{
				false,
				false,
				true,
				false,
				false,
				true,
				true,
				true
			},
			{
				false,
				true,
				false,
				false,
				true,
				true,
				true,
				false
			},
			{
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true
			}
		};

		public string NowScene => nowScene;

		public bool IsLoadLevelScene => isLoadLevelScene;

		public void setOnSceneChangeCoroutine(IEnumerator cor)
		{
			OnSceneChangeCoroutine = cor;
		}

		public void UnloadFlagOn()
		{
			SceneMoveCount = 6;
		}

		private new void Awake()
		{
			base.Awake();
			SceneMoveCount = 0;
		}

		private void Start()
		{
			setNowScene(Generics.Scene.Strategy.ToString(), isLoadLevel: false);
			SceneNo = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			SceneNo.Add(Generics.Scene.Organize.ToString(), 0);
			SceneNo.Add(Generics.Scene.Remodel.ToString(), 1);
			SceneNo.Add(Generics.Scene.Arsenal.ToString(), 2);
			SceneNo.Add(Generics.Scene.Supply.ToString(), 3);
			SceneNo.Add(Generics.Scene.Duty.ToString(), 4);
			SceneNo.Add(Generics.Scene.PortTop.ToString(), 5);
			SceneNo.Add(Generics.Scene.Strategy.ToString(), 6);
			SceneNo.Add(Generics.Scene.Repair.ToString(), 7);
			SceneNo.Add(Generics.Scene.ArsenalSelector.ToString(), 8);
			dummyKey = new KeyControl();
		}

		private void Update()
		{
			if (isHidePortObject && (portKeyControl.IsAnyKey || Input.GetMouseButtonDown(0)))
			{
				HidePortObject(null);
			}
		}

		public void EnterStrategy()
		{
			if (SingletonMonoBehaviour<UIPortFrame>.Instance != null)
			{
				UnityEngine.Object.Destroy(SingletonMonoBehaviour<UIPortFrame>.Instance.gameObject);
			}
		}

		private void setLive2D(Generics.Scene NextScene)
		{
			if (NextScene == Generics.Scene.PortTop || NextScene == Generics.Scene.Strategy)
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
			}
			else
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			}
		}

		public void HidePortObject(KeyControl portKeyControl)
		{
			this.portKeyControl = portKeyControl;
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			if (isHidePortObject)
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.SetActive(isActive: true);
				isHidePortObject = false;
				base.enabled = false;
			}
			else
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.SetActive(isActive: false);
				isHidePortObject = true;
				base.enabled = true;
			}
		}

		public void InstantiateScene(Generics.Scene NextScene, bool isForceFadeOut = false)
		{
			App.OnlyController = new KeyControl();
			if (NowTutorialGuide != null)
			{
				NowTutorialGuide.Hide();
			}
			if (SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsOpen)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.CloseMenu();
			}
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = false;
			}
			if (SingletonMonoBehaviour<UIShortCutMenu>.exist())
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: true);
			}
			PortTransition.isTransitionNow = true;
			if (isUseCrossFade(NextScene) && !isForceFadeOut)
			{
				PortTransition.StartTransition(NextScene, isPortFramePos: true, delegate
				{
					InstantiateSceneChange(NextScene);
				});
			}
			else
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					InstantiateSceneChange(NextScene);
				});
			}
		}

		private bool isUseCrossFade(Generics.Scene NextScene)
		{
			return NextScene != Generics.Scene.PortTop && NextScene != Generics.Scene.Strategy && NextScene != Generics.Scene.Interior && NowScene != Generics.Scene.Strategy.ToString();
		}

		private void InstantiateSceneChange(Generics.Scene NextScene)
		{
			StartCoroutine(InstantiateSceneChange(NextScene, destroyPrevScene: true));
		}

		private IEnumerator InstantiateSceneChange(Generics.Scene NextScene, bool destroyPrevScene)
		{
			yield return null;
			if (OnSceneChangeCoroutine != null)
			{
				yield return StartCoroutine(OnSceneChangeCoroutine);
			}
			if (SceneObject != null && destroyPrevScene)
			{
				DestroyScene();
			}
			else
			{
				GameObject sceneObject = SceneObject;
			}
			SceneAdjustNow(NextScene);
			setNowScene(NextScene.ToString(), isLoadLevel: false);
			if (SceneChangeAct != null)
			{
				SceneChangeAct();
				SceneChangeAct = null;
			}
			SceneAdjustNext(NextScene);
			setLive2D(NextScene);
			yield return new WaitForEndOfFrame();
			if (SceneMoveCount > 5)
			{
				yield return Resources.UnloadUnusedAssets();
				GC.Collect();
				yield return new WaitForEndOfFrame();
				SceneMoveCount = 0;
			}
			else
			{
				SceneMoveCount++;
			}
			SceneObject = Util.Instantiate(ScenePrefabs[SceneNo[NextScene.ToString()]]);
			App.OnlyController = null;
		}

		public static bool isPrefabSecene(Generics.Scene scene)
		{
			return scene == Generics.Scene.Organize || scene == Generics.Scene.Remodel || scene == Generics.Scene.Supply || scene == Generics.Scene.Arsenal || scene == Generics.Scene.Duty || scene == Generics.Scene.PortTop || scene == Generics.Scene.Strategy || scene == Generics.Scene.Repair;
		}

		public bool isLoadSecene()
		{
			string a = nowScene.ToLower();
			return a == Generics.Scene.Record.ToString().ToLower() || a == Generics.Scene.Album.ToString().ToLower() || a == Generics.Scene.Item.ToString().ToLower() || a == Generics.Scene.Interior.ToString().ToLower() || a == Generics.Scene.SaveLoad.ToString().ToLower() || a == Generics.Scene.ImprovementArsenal.ToString().ToLower();
		}

		public void DestroyScene()
		{
			UnityEngine.Object.DestroyImmediate(SceneObject);
			SceneObject = null;
		}

		private void OnLevelWasLoaded()
		{
			setNowScene(Application.loadedLevelName, isLoadLevel: true);
			SceneMoveCount = 0;
			if (SceneNo != null && SceneNo.ContainsKey(Application.loadedLevelName))
			{
				SceneObject = GameObject.Find(ScenePrefabs[SceneNo[Application.loadedLevelName]].name);
			}
			else
			{
				SceneObject = null;
			}
		}

		private void CheckMemory()
		{
			DebugUtils.Log(Profiler.GetTotalReservedMemory().ToString());
			DebugUtils.Log(Profiler.GetTotalAllocatedMemory().ToString());
			DebugUtils.Log(Profiler.GetTotalUnusedReservedMemory().ToString());
		}

		private void SceneAdjustNow(Generics.Scene NextScene)
		{
			switch (NextScene)
			{
			case Generics.Scene.Remodel:
				if (!SingletonMonoBehaviour<UIPortFrame>.exist())
				{
					Util.Instantiate(PortFramePrefab);
				}
				break;
			case Generics.Scene.ImprovementArsenal:
				if (!SingletonMonoBehaviour<UIPortFrame>.exist())
				{
					Util.Instantiate(PortFramePrefab);
				}
				SingletonMonoBehaviour<UIPortFrame>.Instance.setVisibleHeader(isVisible: true);
				break;
			case Generics.Scene.Item:
				if (!SingletonMonoBehaviour<UIPortFrame>.exist())
				{
					Util.Instantiate(PortFramePrefab);
				}
				SingletonMonoBehaviour<UIPortFrame>.Instance.setVisibleHeader(isVisible: true);
				break;
			}
			if (NowScene.ToLower() == Generics.Scene.Remodel.ToString().ToLower())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.setVisibleHeader(isVisible: true);
			}
			else if (NowScene.ToLower() == Generics.Scene.Strategy.ToString().ToLower())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.DestroyCache();
			}
		}

		private void SceneAdjustNext(Generics.Scene NextScene)
		{
			if (NextScene == Generics.Scene.Strategy)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockOffControl();
			}
			else if (!SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				Util.Instantiate(PortFramePrefab);
			}
			if (NextScene == Generics.Scene.PortTop)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockOffControl();
			}
		}

		public void setNowScene(string NowScene, bool isLoadLevel)
		{
			nowScene = NowScene;
			isLoadLevelScene = isLoadLevel;
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.disableButtonList.Clear();
		}

		public void SceneLoad(Generics.Scene NextScene)
		{
			App.OnlyController = dummyKey;
			App.isFirstUpdate = true;
			if (SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsOpen)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.CloseMenu();
			}
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = false;
			}
			if (SingletonMonoBehaviour<UIShortCutMenu>.exist())
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: true);
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			}
			SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
			{
				SceneAdjustNow(NextScene);
				if (SceneChangeAct != null)
				{
					SceneChangeAct();
					SceneChangeAct = null;
				}
				StartCoroutine(GotoNextScene(NextScene));
			});
		}

		private IEnumerator GotoNextScene(Generics.Scene NextScene)
		{
			if (OnSceneChangeCoroutine != null)
			{
				yield return StartCoroutine(OnSceneChangeCoroutine);
				OnSceneChangeCoroutine = null;
			}
			AsyncOperation async = Application.LoadLevelAsync(NextScene.ToString());
			while (async.progress < 0.9f)
			{
				yield return new WaitForEndOfFrame();
			}
			async.allowSceneActivation = true;
			setLive2D(NextScene);
			App.OnlyController = null;
			if (NextScene == Generics.Scene.Strategy)
			{
				yield return new WaitForEndOfFrame();
				if (SingletonMonoBehaviour<UIShortCutMenu>.exist())
				{
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockOffControl();
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: false);
				}
			}
		}

		private bool hasSceneObject()
		{
			return SceneObject != null;
		}

		private GameObject GetSceneObject()
		{
			return SceneObject;
		}

		private void SetSceneObject(GameObject nextSceneObject)
		{
			SceneObject = nextSceneObject;
		}

		public void OverwriteSceneObject(GameObject sceneObject)
		{
			if (hasSceneObject())
			{
				GameObject sceneObject2 = GetSceneObject();
				UnityEngine.Object.Destroy(sceneObject2);
			}
			SetSceneObject(sceneObject);
		}

		public void SetTutorialGuide(TutorialGuide guide)
		{
			NowTutorialGuide = guide;
		}

		public TutorialGuide GetTutorialGuide()
		{
			return NowTutorialGuide;
		}

		private void OnDestroy()
		{
			Release();
		}

		public void ManualRelease()
		{
			Release();
		}

		private void Release()
		{
			int num = (SingletonMonoBehaviour<AppInformation>.Instance != null) ? SingletonMonoBehaviour<AppInformation>.Instance.ReleaseSetNo : 0;
			if (ScenePrefabs != null)
			{
				for (int i = 0; i < ReleaseSet.GetLength(1); i++)
				{
					if (ReleaseSet[num, i])
					{
						ScenePrefabs[i] = null;
					}
				}
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance != null)
			{
				SingletonMonoBehaviour<AppInformation>.Instance.ReleaseSetNo = (int)Util.LoopValue(SingletonMonoBehaviour<AppInformation>.Instance.ReleaseSetNo + 1, 0f, 3f);
			}
			SceneChanger = null;
			PortFramePrefab = null;
			NowTutorialGuide = null;
			PortTransition = null;
			SceneChangeAct = null;
			SceneObjects = null;
			SceneObject = null;
			portKeyControl = null;
			if (SceneNo != null)
			{
				SceneNo.Clear();
			}
			SceneNo = null;
			NowTutorialGuide = null;
		}

		private void Unload()
		{
			Resources.UnloadUnusedAssets();
		}

		public bool BackToActiveScene()
		{
			if (IsGoPortCurrentDeck())
			{
				return BackToPort();
			}
			return BackToStrategy();
		}

		public bool BackToPort()
		{
			if (isLoadLevelScene)
			{
				SceneLoad(Generics.Scene.PortTop);
			}
			else
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.PortTop);
			}
			return true;
		}

		public bool BackToPortOrOrganize()
		{
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState != 0)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.InMissionShip));
				return false;
			}
			if (IsGoPortCurrentDeck())
			{
				return BackToPort();
			}
			return BackToOrganize();
		}

		private bool BackToOrganize()
		{
			SceneLoad(Generics.Scene.Organize);
			return true;
		}

		public bool BackToStrategy()
		{
			if (isLoadLevelScene)
			{
				SceneLoad(Generics.Scene.Strategy);
			}
			else
			{
				InstantiateScene(Generics.Scene.Strategy);
			}
			return true;
		}

		private bool IsGoPortCurrentDeck()
		{
			return IsGoPort(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
		}

		public bool IsGoPort(DeckModel deckModel)
		{
			bool flag = deckModel.GetFlagShip() != null;
			bool flag2 = !deckModel.HasBling();
			if (flag && flag2)
			{
				return true;
			}
			return false;
		}
	}
}
