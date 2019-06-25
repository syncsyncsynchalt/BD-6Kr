using Common.Struct;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV
{
	public class UIPortFrame : SingletonMonoBehaviour<UIPortFrame>
	{
		public enum FrameMode
		{
			None,
			Hide,
			Show
		}

		[Serializable]
		private class AdmiralInfos
		{
			private GameObject _uiInfoObj;

			private UILabel _uiName;

			private UILabel _uiRank;

			private UILabel _uiSenryakuVal;

			private UILabel _uiUserLevel;

			public AdmiralInfos(Transform parent)
			{
				Util.FindParentToChild(ref _uiInfoObj, parent, "AdmiralInfos");
				Util.FindParentToChild(ref _uiName, _uiInfoObj, "Name");
				Util.FindParentToChild(ref _uiSenryakuVal, _uiInfoObj, "SenryakuVal");
				Util.FindParentToChild(ref _uiUserLevel, _uiInfoObj, "UserLevel");
			}

			public bool UnInit()
			{
				_uiName = null;
				_uiRank = null;
				_uiSenryakuVal = null;
				_uiUserLevel = null;
				return true;
			}

			public void UpdateInfo(UserInfoModel userInfo)
			{
				if (userInfo != null)
				{
					_uiName.text = userInfo.Name;
					_uiName.supportEncoding = false;
					_uiUserLevel.text = $"Lv. {userInfo.Level}";
					_uiSenryakuVal.text = userInfo.SPoint.ToString();
				}
			}
		}

		[Serializable]
		private class MaterialInfos
		{
			private GameObject _uiInfoObj;

			private UILabel _uiFuel;

			private UILabel _uiAmmo;

			private UILabel _uiSteel;

			private UILabel _uiBaux;

			private UILabel _uiRepairKit;

			private UILabel _uiDevKit;

			public MaterialInfos(Transform parent)
			{
				Util.FindParentToChild(ref _uiInfoObj, parent, "MaterialInfos");
				Util.FindParentToChild(ref _uiFuel, _uiInfoObj, "FuelVal");
				Util.FindParentToChild(ref _uiAmmo, _uiInfoObj, "AmmoVal");
				Util.FindParentToChild(ref _uiSteel, _uiInfoObj, "SteelVal");
				Util.FindParentToChild(ref _uiBaux, _uiInfoObj, "BauxVal");
				Util.FindParentToChild(ref _uiRepairKit, _uiInfoObj, "RepairKitVal");
				Util.FindParentToChild(ref _uiDevKit, _uiInfoObj, "DebKitVal");
			}

			public bool UnInit()
			{
				_uiAmmo = null;
				_uiBaux = null;
				_uiDevKit = null;
				_uiFuel = null;
				_uiInfoObj = null;
				_uiRepairKit = null;
				_uiSteel = null;
				return true;
			}

			public void UpdateInfo(int naturalRecoverMaterialMax, MaterialModel info)
			{
				if (info != null)
				{
					if (naturalRecoverMaterialMax <= info.Fuel)
					{
						_uiFuel.color = Color.yellow;
					}
					else
					{
						_uiFuel.color = Color.white;
					}
					_uiFuel.text = info.Fuel.ToString();
					if (naturalRecoverMaterialMax <= info.Ammo)
					{
						_uiAmmo.color = Color.yellow;
					}
					else
					{
						_uiAmmo.color = Color.white;
					}
					_uiAmmo.text = info.Ammo.ToString();
					if (naturalRecoverMaterialMax <= info.Steel)
					{
						_uiSteel.color = Color.yellow;
					}
					else
					{
						_uiSteel.color = Color.white;
					}
					_uiSteel.text = info.Steel.ToString();
					if (naturalRecoverMaterialMax <= info.Baux)
					{
						_uiBaux.color = Color.yellow;
					}
					else
					{
						_uiBaux.color = Color.white;
					}
					_uiBaux.text = info.Baux.ToString();
					_uiRepairKit.text = info.RepairKit.ToString();
					_uiDevKit.text = info.Devkit.ToString();
				}
			}
		}

		[Serializable]
		private class Circles
		{
			private UIButton _uiCircleBtn;

			private UITexture _uiModeSprite;

			private UITexture _uiModeSpriteBlur;

			public Transform transform => _uiCircleBtn.transform;

			public UIButton circleButton => _uiCircleBtn;

			public UITexture circleLabel => _uiModeSprite;

			public UITexture circleLabelBlur => _uiModeSpriteBlur;

			public Circles(Transform parent)
			{
				Util.FindParentToChild(ref _uiCircleBtn, parent, "CirclesBtn");
				Util.FindParentToChild(ref _uiModeSprite, _uiCircleBtn.transform, "Circle/Texture_Name");
				Util.FindParentToChild(ref _uiModeSpriteBlur, _uiCircleBtn.transform, "Circle/Texture_Name_Blur");
			}

			public bool Init()
			{
				return true;
			}

			public bool UnInit()
			{
				_uiCircleBtn = null;
				_uiModeSprite = null;
				_uiModeSpriteBlur = null;
				return true;
			}

			public void Update()
			{
				_uiModeSprite.transform.parent.Rotate(new Vector3(0f, 0f, 10f) * (0f - Time.deltaTime));
			}

			public void UpdateInfo(ManagerBase manager)
			{
				if (_uiModeSprite.mainTexture != null)
				{
					Resources.UnloadAsset(_uiModeSprite.mainTexture);
					_uiModeSprite.mainTexture = null;
				}
				if (_uiModeSpriteBlur.mainTexture != null)
				{
					Resources.UnloadAsset(_uiModeSpriteBlur.mainTexture);
					_uiModeSpriteBlur.mainTexture = null;
				}
				_uiModeSprite.mainTexture = RequestHeaderTitle(manager);
				_uiModeSpriteBlur.mainTexture = RequestHeaderTitleBlur(manager);
			}

			private Texture RequestHeaderTitle(ManagerBase manager)
			{
				if (manager is OrganizeManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_organize");
				}
				if (manager is RepairManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_repair");
				}
				if (manager is SupplyManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_supply");
				}
				if (manager is ArsenalManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_arsenal");
				}
				if (manager is RevampManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_arsenal2");
				}
				if (manager is AlbumManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_album");
				}
				if (manager is DutyManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_duty");
				}
				if (manager is PracticeManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_ensyu");
				}
				if (manager is MissionManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_ensei");
				}
				if (manager is ItemlistManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_item");
				}
				if (manager is ItemStoreManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_item");
				}
				if (manager is RecordManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_record");
				}
				if (manager is InteriorManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_reform");
				}
				if (manager is RemodelManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_remodal");
				}
				return Resources.Load<Texture>("Textures/PortHeader/header_icon_bokou");
			}

			private Texture RequestHeaderTitleBlur(ManagerBase manager)
			{
				if (manager is OrganizeManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_organize_b");
				}
				if (manager is RepairManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_repair_b");
				}
				if (manager is SupplyManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_supply_b");
				}
				if (manager is ArsenalManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_arsenal_b");
				}
				if (manager is RevampManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_arsenal2_b");
				}
				if (manager is AlbumManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_album_b");
				}
				if (manager is DutyManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_duty_b");
				}
				if (manager is PracticeManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_ensyu_b");
				}
				if (manager is MissionManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_ensei_b");
				}
				if (manager is ItemlistManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_item_b");
				}
				if (manager is ItemStoreManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_item_b");
				}
				if (manager is RecordManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_record_b");
				}
				if (manager is InteriorManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_reform_b");
				}
				if (manager is RemodelManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_remodal_b");
				}
				return Resources.Load<Texture>("Textures/PortHeader/header_icon_bokou_b");
			}
		}

		[Serializable]
		private class DateTime
		{
			[SerializeField]
			private UILabel _uiDateTime;

			public DateTime(Transform parent)
			{
				Util.FindParentToChild(ref _uiDateTime, parent, "Time");
			}

			public void UpdateInfo(TurnString time)
			{
				_uiDateTime.fontSize = 20;
				_uiDateTime.text = $"[323232]{time.Year}の年 {time.Month}[b]{time.Day}[/b]日 {getDayOfWeekJapanese(time.DayOfWeek)}[-]";
			}
		}

		private const float CIRCLEMODE_ROT_SPD = 10f;

		private const float CIRCLE2_ROT_SPD = 5f;

		private const float CIRCLE2_ROT_INTERVAL = 2f;

		private const float TRANSITION_BTN_OFFSET_X = 165f;

		private const float TOPFRAME_MOVE_TIME = 0.8f;

		private UIPanel _uiPanel;

		private GameObject _uiHeader;

		private GameObject _uiFrame;

		private Circles _clsCircles;

		private AdmiralInfos _clsAdmiralInfo;

		private MaterialInfos _clsMaterialInfo;

		private DateTime _clsDateTime;

		private Generics.InnerCamera _clsCam;

		private Vector3[] _vHeaderPos = new Vector3[2]
		{
			new Vector3(-960f, 232f, 0f),
			new Vector3(-0f, 232f, 0f)
		};

		private Vector3[] _vFramePos = new Vector3[2]
		{
			new Vector3(-960f, -273f, 0f),
			new Vector3(-480f, -273f, 0f)
		};

		private bool _isForcus;

		private bool isInitialized;

		private Action mOnClickCircleButtonListener;

		public bool IsForcus
		{
			get
			{
				return _isForcus;
			}
			set
			{
				if (value)
				{
					if (!_isForcus)
					{
						_isForcus = true;
					}
				}
				else if (_isForcus)
				{
					_isForcus = false;
				}
			}
		}

		public float alpha
		{
			get
			{
				if (_uiPanel != null)
				{
					return _uiPanel.alpha;
				}
				return -1f;
			}
			set
			{
				if (_uiPanel != null)
				{
					_uiPanel.alpha = value;
				}
				else
				{
					Debug.LogWarning("Not Found _uiPanel");
				}
			}
		}

		public bool isColliderEnabled
		{
			get
			{
				return _clsCircles.circleButton.isEnabled;
			}
			set
			{
				_clsCircles.circleButton.isEnabled = value;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_isForcus = false;
			Util.SetRootContentSize(GetComponent<UIRoot>(), App.SCREEN_RESOLUTION);
			Util.FindParentToChild(ref _uiPanel, base.transform, "Panel");
			Util.FindParentToChild(ref _uiHeader, _uiPanel.transform, "Header");
			_uiHeader.transform.localPosition = _vHeaderPos[0];
			Util.FindParentToChild(ref _uiFrame, _uiPanel.transform, "Frame");
			_uiFrame.transform.localPosition = _vFramePos[0];
			_clsCircles = new Circles(_uiPanel.transform);
			_clsCircles.circleButton.onClick = Util.CreateEventDelegateList(this, "onCircleBtnClick", null);
			_clsAdmiralInfo = new AdmiralInfos(_uiHeader.transform);
			_clsMaterialInfo = new MaterialInfos(_uiHeader.transform);
			_clsCam = new Generics.InnerCamera(base.transform.FindChild("FrameCamera"));
			_clsDateTime = new DateTime(_uiPanel.transform.FindChild("Frame"));
		}

		private void Start()
		{
			Init();
			_portframeScreenIn();
			UpdateHeaderInfo();
		}

		private void OnDestroy()
		{
			UnInit();
		}

		private void Update()
		{
			_clsCircles.Update();
		}

		public bool Init()
		{
			_clsCircles.Init();
			return true;
		}

		public bool UnInit()
		{
			_isForcus = false;
			_clsCircles.UnInit();
			_clsAdmiralInfo.UnInit();
			_clsMaterialInfo.UnInit();
			_uiPanel = null;
			_uiHeader = null;
			return true;
		}

		public void Discard()
		{
			if (SingletonMonoBehaviour<UIPortFrame>.instance != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		public void UpdateHeaderInfo(ManagerBase manager)
		{
			_clsAdmiralInfo.UpdateInfo(manager.UserInfo);
			_clsMaterialInfo.UpdateInfo(manager.UserInfo.GetMaterialMaxNum(), manager.Material);
			_clsDateTime.UpdateInfo(manager.DatetimeString);
		}

		public void UpdateHeaderInfo()
		{
			UpdateHeaderInfo(new PortManager(SingletonMonoBehaviour<AppInformation>.instance.CurrentAreaID));
		}

		public void CircleUpdateInfo(ManagerBase manager)
		{
			_clsCircles.UpdateInfo(manager);
			fadeInCircleButtonLabel();
		}

		private void onCircleBtnClick()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			if (SingletonMonoBehaviour<PortObjectManager>.Instance.NowScene == Generics.Scene.PortTop.ToString())
			{
				OnClickCircleButton();
				return;
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Count == 0)
			{
				CommonPopupDialog.Instance.StartPopup("艦隊を編成する必要があります");
				return;
			}
			isColliderEnabled = false;
			if (PortObjectManager.SceneChangeAct != null)
			{
				PortObjectManager.SceneChangeAct();
				PortObjectManager.SceneChangeAct = null;
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.HasBling())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.Strategy);
			}
			else if (SingletonMonoBehaviour<PortObjectManager>.Instance.IsLoadLevelScene)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.PortTop);
			}
			else
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.PortTop);
			}
		}

		[Obsolete("UserInterfacePortManagerで使用します。")]
		public void SetOnClickCircleButtoListener(Action onClickCircleButtonListener)
		{
			mOnClickCircleButtonListener = onClickCircleButtonListener;
		}

		[Obsolete("UserInterfacePortManagerで使用します。")]
		private void OnClickCircleButton()
		{
			if (mOnClickCircleButtonListener != null)
			{
				mOnClickCircleButtonListener();
			}
		}

		public void ReqMode(FrameMode iMode)
		{
			if (iMode != 0)
			{
				switch (iMode)
				{
				case FrameMode.Hide:
					_clsCam.sameMask = Generics.Layers.Nothing;
					break;
				case FrameMode.Show:
					_clsCam.sameMask = Generics.Layers.UI2D;
					break;
				}
			}
		}

		public static string getDayOfWeekJapanese(string dow)
		{
			switch (dow)
			{
			case "Sunday":
				return "(日)";
			case "Monday":
				return "(月)";
			case "Tuesday":
				return "(火)";
			case "Wednesday":
				return "(水)";
			case "Thursday":
				return "(木)";
			case "Friday":
				return "(金)";
			case "Saturday":
				return "(土)";
			default:
				return string.Empty;
			}
		}

		public void setVisibleHeader(bool isVisible)
		{
			float alpha = isVisible ? 1 : 0;
			TweenAlpha.Begin(_uiHeader, 0.2f, alpha);
		}

		public void fadeOutCircleButtonLabel()
		{
			TweenAlpha.Begin(_clsCircles.circleLabel.gameObject, 0.4f, 0f);
			TweenAlpha.Begin(_clsCircles.circleLabelBlur.gameObject, 0.4f, 0f);
		}

		public void fadeInCircleButtonLabel()
		{
			TweenAlpha.Begin(_clsCircles.circleLabel.gameObject, 0.4f, 1f);
			TweenAlpha tweenAlpha = TweenAlpha.Begin(_clsCircles.circleLabelBlur.gameObject, 2f, 0f);
			tweenAlpha.from = 0f;
			tweenAlpha.to = 1f;
			tweenAlpha.style = UITweener.Style.PingPong;
		}

		private void _portframeScreenIn()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", _vHeaderPos[1]);
			hashtable.Add("time", 0.8f);
			hashtable.Add("isLocal", true);
			hashtable.Add("delay", 0f);
			hashtable.Add("easeType", iTween.EaseType.easeOutExpo);
			iTween.MoveTo(_uiHeader, hashtable);
		}

		private void _portframeScreenOut()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", _vHeaderPos[0]);
			hashtable.Add("time", 0.8f);
			hashtable.Add("isLocal", true);
			hashtable.Add("delay", 0f);
			hashtable.Add("easeType", iTween.EaseType.easeOutExpo);
			iTween.MoveTo(_uiHeader, hashtable);
			hashtable.Remove("position");
			hashtable.Remove("delay");
			hashtable.Remove("time");
			hashtable.Add("position", _vFramePos[0]);
			hashtable.Add("delay", 0f);
			hashtable.Add("time", 0.8f);
			iTween.MoveTo(_uiFrame, hashtable);
		}

		public void ReqFrame(bool isScreenIn)
		{
			if (isScreenIn)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("position", _vFramePos[1]);
				hashtable.Add("delay", 0.2f);
				hashtable.Add("time", 0.8f);
				hashtable.Add("isLocal", true);
				hashtable.Add("easeType", iTween.EaseType.easeOutExpo);
				iTween.MoveTo(_uiFrame, hashtable);
			}
			else
			{
				Hashtable hashtable2 = new Hashtable();
				hashtable2.Add("isLocal", true);
				hashtable2.Add("easeType", iTween.EaseType.easeOutExpo);
				hashtable2.Add("position", _vFramePos[0]);
				hashtable2.Add("delay", 0f);
				hashtable2.Add("time", 0.8f);
				iTween.MoveTo(_uiFrame, hashtable2);
			}
		}
	}
}
