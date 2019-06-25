using local.managers;
using local.models;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	public class TitleTaskManager : MonoBehaviour
	{
		private static TitleTaskManager instance;

		[SerializeField]
		private UITitleBackground _uiTitleBackground;

		[SerializeField]
		private UITitleLogo _uiTitleLogo;

		[SerializeField]
		private Transform _sharedPlace;

		[SerializeField]
		private TitlePrefabFile _clsPrefabFile;

		[SerializeField]
		private PSVitaMovie _clsVitaMovie;

		[SerializeField]
		private UIPanel _uiMaskPanel;

		[SerializeField]
		private UILabel _uiMasterVersion;

		private Generics.InnerCamera _camTitle;

		private static KeyControl _clsInput;

		private static bool _isPlayOpening = true;

		private static SceneTasksMono _clsTasks;

		private static TitleTaskManagerMode _iMode;

		private static TitleTaskManagerMode _iModeReq;

		private static TaskTitleOpening _clsTaskOpening;

		private static TaskTitleSelectMode _clsTaskSelectMode;

		private static TaskTitleNewGame _clsTaskNewGame;

		private static TitleTaskManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Object.FindObjectOfType<TitleTaskManager>();
				}
				return instance;
			}
		}

		private void Awake()
		{
			if (App.GetTitleManager() == null)
			{
				App.SetTitleManager(new TitleManager());
			}
			_clsTasks = base.gameObject.SafeGetComponent<SceneTasksMono>();
			_clsInput = new KeyControl();
			_camTitle = new Generics.InnerCamera(base.transform.FindChild("TitleCamera").transform);
			_clsTaskOpening = base.transform.GetComponentInChildren<TaskTitleOpening>();
			_clsTaskSelectMode = base.transform.GetComponentInChildren<TaskTitleSelectMode>();
			_clsTaskNewGame = base.transform.GetComponentInChildren<TaskTitleNewGame>();
			Util.SetRootContentSize(GetComponent<UIRoot>(), App.SCREEN_RESOLUTION);
			_uiMasterVersion.text = "Version 1.02";
			App.SetSoundSettings(new SettingModel());
			if (_isPlayOpening)
			{
				_uiMaskPanel.alpha = 1f;
				_clsTaskOpening.PlayImmediateOpeningMovie();
				_iMode = (_iModeReq = TitleTaskManagerMode.TitleTaskManagerMode_ST);
				_isPlayOpening = false;
				return;
			}
			_uiMaskPanel.alpha = 0f;
			if (SingletonMonoBehaviour<FadeCamera>.Instance != null && SingletonMonoBehaviour<FadeCamera>.Instance.isFadeOut)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, null);
			}
			_iMode = (_iModeReq = TitleTaskManagerMode.SelectMode);
		}

		private void Start()
		{
			_clsTasks.Init();
			Observable.FromCoroutine(_uiTitleBackground.StartBackgroundAnim).Subscribe().AddTo(base.gameObject);
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiTitleBackground);
			Mem.Del(ref _uiTitleLogo);
			Mem.Del(ref _sharedPlace);
			_clsPrefabFile.Dispose();
			Mem.Del(ref _clsPrefabFile);
			Mem.Del(ref _clsVitaMovie);
			Mem.Del(ref _uiMaskPanel);
			Mem.Del(ref _uiMasterVersion);
			_camTitle.UnInit();
			Mem.Del(ref _camTitle);
			Mem.Del(ref _clsInput);
			_clsTasks.UnInit();
			Mem.Del(ref _iMode);
			Mem.Del(ref _iModeReq);
			Mem.Del(ref _clsTaskOpening);
			Mem.Del(ref _clsTaskSelectMode);
			Mem.Del(ref _clsTaskNewGame);
			Mem.Del(ref _clsTasks);
			Mem.Del(ref instance);
		}

		private void Update()
		{
			if (Input.touchCount == 0 && !Input.GetMouseButton(0) && _clsInput != null)
			{
				_clsInput.Update();
			}
			_clsTasks.Run();
			UpdateMode();
		}

		public static TitleTaskManagerMode GetMode()
		{
			return _iModeReq;
		}

		public static void ReqMode(TitleTaskManagerMode iMode)
		{
			_iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (_iModeReq == TitleTaskManagerMode.TitleTaskManagerMode_BEF)
			{
				return;
			}
			switch (_iModeReq)
			{
			case TitleTaskManagerMode.TitleTaskManagerMode_ST:
				if (_clsTasks.Open(_clsTaskOpening) < 0)
				{
					return;
				}
				break;
			case TitleTaskManagerMode.SelectMode:
				if (_clsTasks.Open(_clsTaskSelectMode) < 0)
				{
					return;
				}
				break;
			case TitleTaskManagerMode.NewGame:
				if (_clsTasks.Open(_clsTaskNewGame) < 0)
				{
					return;
				}
				break;
			}
			_iMode = _iModeReq;
			_iModeReq = TitleTaskManagerMode.TitleTaskManagerMode_BEF;
		}

		public static KeyControl GetKeyControl()
		{
			return _clsInput;
		}

		public static Transform GetSharedPlace()
		{
			return Instance._sharedPlace;
		}

		public static UIPanel GetMaskPanel()
		{
			return Instance._uiMaskPanel;
		}

		public static PSVitaMovie GetPSVitaMovie()
		{
			return Instance._clsVitaMovie;
		}

		public static UITitleLogo GetUITitleLogo()
		{
			return Instance._uiTitleLogo;
		}

		public static TitlePrefabFile GetPrefabFile()
		{
			return Instance._clsPrefabFile;
		}

		public static IEnumerator GotoLoadScene(IObserver<AsyncOperation> observer)
		{
			RetentionData.SetData(new Hashtable
			{
				{
					"rootType",
					Generics.Scene.Title
				}
			});
			AsyncOperation async = Application.LoadLevelAsync(Generics.Scene.SaveLoad.ToString());
			async.allowSceneActivation = false;
			while (!App.isMasterInit)
			{
				yield return Observable.NextFrame(FrameCountType.EndOfFrame);
			}
			observer.OnNext(async);
			observer.OnCompleted();
		}
	}
}
