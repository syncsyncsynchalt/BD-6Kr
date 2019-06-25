using Common.Enum;
using KCV.Display;
using KCV.Utils;
using local.managers;
using local.models;
using local.utils;
using Server_Models;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Port.record
{
	public class record_cam : MonoBehaviour
	{
		private const BGMFileInfos SCENE_BGM = BGMFileInfos.PortTools;

		[SerializeField]
		private Transform mDifficulty;

		[SerializeField]
		private UIDisplaySwipeEventRegion mtouchEventArea;

		[SerializeField]
		private Transform mMedalist;

		[SerializeField]
		private Transform mboardud1;

		[SerializeField]
		private Transform mWindowParam;

		[SerializeField]
		private Transform mShipTexture;

		[SerializeField]
		private Transform[] mbgWalls;

		[SerializeField]
		private Transform[] mLabels;

		[SerializeField]
		private Transform Medals;

		private int _now_page;

		private int _dbg_class;

		private bool _Xpressed;

		private bool _firstUpdate;

		private bool needAnimation;

		private bool _isTouch;

		private bool _isScene;

		private bool needUpdate;

		private string _ANIM_filebase = "boards_mvud";

		private ShipModel mSecurityShipModel;

		private RecordManager _clsRecord;

		private UILabel label;

		private UILabel label2;

		private UISprite sprite;

		private UITexture mTexture_SecurityShip;

		private UITexture texture;

		private UIButton _Button_L;

		private UIButton _Button_R;

		private UISprite _Button_L_B;

		private UISprite _Button_R_B;

		private Animation _AM;

		private Animation _AM_l;

		private Animation _AM_b;

		private SoundManager _SM;

		private KeyControl ItemSelectController;

		private AsyncObjects _AS;

		private GameObject _board1;

		private Color alphaZero_b = Color.black * 0f;

		private int __USEITEM_DOCKKEY__ = 49;

		private UITexture _diff1;

		private UITexture _diff2;

		private bool _gotMedal;

		private int _SelectableDiff;

		private void Start()
		{
			needUpdate = false;
			_Xpressed = false;
			_firstUpdate = false;
			needAnimation = false;
			_isTouch = false;
			_gotMedal = false;
			_AS = GetComponent<AsyncObjects>();
			Util.FindParentToChild(ref _diff1, mDifficulty, "mojiW");
			Util.FindParentToChild(ref _diff2, mDifficulty, "mojiB");
			Util.FindParentToChild(ref _Button_L, base.transform, "MaskCamera/btn/Button_L");
			Util.FindParentToChild(ref _Button_R, base.transform, "MaskCamera/btn/Button_R");
			Util.FindParentToChild<Animation>(ref _AM_b, base.transform, "MaskCamera/btn");
			Util.FindParentToChild(ref _Button_L_B, _Button_L.transform, "Background");
			Util.FindParentToChild(ref _Button_R_B, _Button_R.transform, "Background");
			Util.FindParentToChild(ref _board1, mboardud1, "board1");
			_Button_L_B.transform.localScale = Vector3.zero;
			_Button_R_B.transform.localScale = Vector3.one;
			StartUp();
		}

		private void Update()
		{
			if (!needUpdate)
			{
				return;
			}
			_set_arrow();
			if (!_firstUpdate)
			{
				_firstUpdate = false;
				base.gameObject.transform.localScale = Vector3.one;
			}
			if (ItemSelectController != null)
			{
				ItemSelectController.Update();
			}
			if (ItemSelectController.keyState[8].down)
			{
				Pressed_Button_L(null);
			}
			if (ItemSelectController.keyState[12].down)
			{
				Pressed_Button_R(null);
			}
			if (ItemSelectController.IsRDown())
			{
				_Xpressed = true;
				needUpdate = false;
				if (_isScene)
				{
					GoToStrategy();
				}
			}
			else if (ItemSelectController.IsBatuDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
			}
		}

		public bool getRecordDone()
		{
			return _Xpressed;
		}

		private void CompleteHandler(GameObject value)
		{
			needAnimation = false;
		}

		private void PageAnimeDone()
		{
			needAnimation = false;
		}

		private void SwipeJudgeDelegate(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (!needAnimation)
			{
				if (actionType == UIDisplaySwipeEventRegion.ActionType.Moving && movedPercentageY >= 0.2f && !_isTouch)
				{
					_isTouch = true;
					Pressed_Button_L(null);
				}
				else if (actionType == UIDisplaySwipeEventRegion.ActionType.Moving && movedPercentageY <= -0.2f && !_isTouch)
				{
					_isTouch = true;
					Pressed_Button_R(null);
				}
				else if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
				{
					_isTouch = false;
				}
			}
		}

		private void StartUp()
		{
			needUpdate = true;
			Camera component;
			if (Application.loadedLevelName == "Record" || Application.loadedLevelName == "Record_cam")
			{
				_isScene = true;
				component = GameObject.Find("MaskCamera").GetComponent<Camera>();
				component.rect = new Rect(0.353f, 0.05f, 0.625f, 0.699f);
			}
			else
			{
				_isScene = false;
				component = GameObject.Find("OverViewCamera").GetComponent<Camera>();
				GameObject.Find("MaskCamera").GetComponent<Camera>().rect = new Rect(0.353f, 0.16f, 0.625f, 0.699f);
			}
			mtouchEventArea.SetOnSwipeActionJudgeCallBack(SwipeJudgeDelegate);
			mtouchEventArea.SetEventCatchCamera(component);
			_ANIM_filebase = "boards_mvud";
			_clsRecord = new RecordManager();
			_gotMedal = _clsRecord.IsCleardOnce();
			if (_gotMedal)
			{
				ItemSelectController = new KeyControl(0, 3);
			}
			else
			{
				ItemSelectController = new KeyControl(0, 2);
			}
			ItemSelectController.setChangeValue(-1f, 0f, 1f, 0f);
			_SelectableDiff = 3;
			if (_clsRecord.GetClearCount(DifficultKind.OTU) > 0)
			{
				_SelectableDiff = 4;
			}
			if (_clsRecord.GetClearCount(DifficultKind.KOU) > 0)
			{
				_SelectableDiff = 5;
			}
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.CircleUpdateInfo(_clsRecord);
			}
			_AM = GetComponent<Animation>();
			if (_isScene)
			{
				_AM_l = ((Component)mMedalist).GetComponent<Animation>();
			}
			_SM = SingletonMonoBehaviour<SoundManager>.Instance;
			_Button_L_B.transform.localPosition = Vector3.zero;
			_Button_R_B.transform.localPosition = Vector3.zero;
			UIButtonMessage component2 = _Button_L.GetComponent<UIButtonMessage>();
			component2.target = base.gameObject;
			component2.functionName = "Pressed_Button_L";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component3 = _Button_R.GetComponent<UIButtonMessage>();
			component3.target = base.gameObject;
			component3.functionName = "Pressed_Button_R";
			component3.trigger = UIButtonMessage.Trigger.OnClick;
			_draw_labels();
			_now_page = 1;
			mSecurityShipModel = ((SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel == null) ? new ShipModel(1) : SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel);
			DamageState damageStatus = mSecurityShipModel.DamageStatus;
			if (SingletonMonoBehaviour<PortObjectManager>.Instance != null)
			{
				SoundUtils.SwitchBGM(BGMFileInfos.PortTools);
				SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(delegate
				{
					ShipUtils.PlayShipVoice(mSecurityShipModel, 8);
				});
			}
			if (_isScene)
			{
				set_flagship_texture(mSecurityShipModel);
				iTween.MoveTo(mWindowParam.gameObject, iTween.Hash("islocal", true, "x", -307f, "y", -199f, "time", 1f, "delay", 0.5f));
				iTween.MoveTo(mMedalist.gameObject, iTween.Hash("islocal", true, "x", 426f, "y", -203f, "time", 1f, "delay", 1.5f));
				return;
			}
			SetColorBG(Color.white * 0.9f + Color.black);
			SetColorText(Color.white * 0.5f + Color.black);
			SetColorLine(Color.white * 0.5f + Color.black);
			SetColorIcon(Color.white + Color.black);
			SetTextShadow(shadow: false);
			SetCursorColor(Color.red * 0.75f + Color.black);
		}

		private void _set_arrow()
		{
			if (!needAnimation)
			{
				if (_now_page == 1)
				{
					_AM_b.Play("btn_mvud_off_on");
					iTween.MoveTo(_Button_L_B.gameObject, iTween.Hash("islocal", true, "x", 0f, "time", 0.5f));
					_Button_L_B.transform.localScale = Vector3.zero;
					_Button_R_B.transform.localScale = Vector3.one;
				}
				else if (_now_page == 2 || (_now_page == 3 && _gotMedal))
				{
					_AM_b.Play("btn_mvud_on_on");
					_Button_L_B.transform.localScale = Vector3.one;
					_Button_R_B.transform.localScale = Vector3.one;
				}
				else
				{
					_AM_b.Play("btn_mvud_on_off");
					iTween.MoveTo(_Button_R_B.gameObject, iTween.Hash("islocal", true, "x", 0f, "time", 0.5f));
					_Button_L_B.transform.localScale = Vector3.one;
					_Button_R_B.transform.localScale = Vector3.zero;
				}
			}
		}

		private void _set_view_board(int page)
		{
			if (needAnimation)
			{
				return;
			}
			needAnimation = true;
			switch (page)
			{
			case 1:
				if (_now_page != page)
				{
					SoundUtils.PlaySE(SEFIleInfos.MainMenuOnMouse);
					iTween.MoveTo(_board1, iTween.Hash("islocal", true, "y", 0f, "time", 0.5f, "easeType", "easeInOutQuad", "oncomplete", "OnComplete", "oncompletetarget", base.gameObject));
					_AM.Play("boards_wait");
				}
				_now_page = 1;
				break;
			case 2:
				if (_now_page != page)
				{
					SoundUtils.PlaySE(SEFIleInfos.MainMenuOnMouse);
					iTween.MoveTo(_board1, iTween.Hash("islocal", true, "y", 380f, "time", 0.5f, "easeType", "easeInOutQuad", "oncomplete", "OnComplete", "oncompletetarget", base.gameObject));
					_AM.Play("boards_wait");
				}
				_now_page = 2;
				break;
			case 3:
				if (_now_page != page)
				{
					SoundUtils.PlaySE(SEFIleInfos.MainMenuOnMouse);
					iTween.MoveTo(_board1, iTween.Hash("islocal", true, "y", 760f, "time", 0.5f, "easeType", "easeInOutQuad", "oncomplete", "OnComplete", "oncompletetarget", base.gameObject));
					_AM.Play("boards_wait");
				}
				_now_page = 3;
				break;
			case 4:
				if (_gotMedal)
				{
					if (_now_page != page)
					{
						SoundUtils.PlaySE(SEFIleInfos.MainMenuOnMouse);
						iTween.MoveTo(_board1, iTween.Hash("islocal", true, "y", 1140f, "time", 0.5f, "easeType", "easeInOutQuad", "oncomplete", "OnComplete", "oncompletetarget", base.gameObject));
						_AM.Play("boards_wait");
					}
					_now_page = 4;
				}
				break;
			}
		}

		public DifficultKind int2DiffKind(int i)
		{
			switch (i)
			{
			case 1:
				return DifficultKind.TEI;
			case 2:
				return DifficultKind.HEI;
			case 3:
				return DifficultKind.OTU;
			case 4:
				return DifficultKind.KOU;
			case 5:
				return DifficultKind.SHI;
			default:
				return DifficultKind.TEI;
			}
		}

		public void _draw_labels()
		{
			Dictionary<int, string> mstBgm = Mst_DataManager.Instance.GetMstBgm();
			mstBgm.TryGetValue(_clsRecord.UserInfo.GetPortBGMId(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID), out string value);
			if (value == null)
			{
				value = "母港";
			}
			for (int i = 1; i <= 3; i++)
			{
				texture = ((Component)mbgWalls[i - 1]).GetComponent<UITexture>();
				texture.mainTexture = (Resources.Load("Textures/Record/logo") as Texture);
			}
			label = ((Component)mWindowParam.FindChild("Labels/adm_name")).GetComponent<UILabel>();
			label.text = _clsRecord.Name;
			label.supportEncoding = false;
			label = ((Component)mWindowParam.FindChild("Labels/adm_level")).GetComponent<UILabel>();
			label.textInt = _clsRecord.Level;
			label = ((Component)mWindowParam.FindChild("Labels/adm_status")).GetComponent<UILabel>();
			label.text = Util.RankNameJ(_clsRecord.Rank);
			label = ((Component)mWindowParam.FindChild("Labels/adm_exp")).GetComponent<UILabel>();
			label.fontSize = 24;
			label.width = 300;
			if (_clsRecord.NextExperience == 0)
			{
				label.text = _clsRecord.Experience.ToString();
			}
			else
			{
				label.text = _clsRecord.Experience + "/" + _clsRecord.NextExperience;
			}
			((Component)mLabels[0].FindChild("Label_1-2")).GetComponent<UILabel>().text = _clsRecord.BattleCount + "  \n\n" + _clsRecord.SortieWin + "  \n\n" + _clsRecord.InterceptSuccess + "  \n\n" + $"{_clsRecord.SortieRate:f1}" + "%\n\n\n\n" + _clsRecord.DeckPractice + "  \n\n" + (_clsRecord.PracticeWin + _clsRecord.PracticeLose).ToString() + "  ";
			int count = new UseitemUtil().GetCount(__USEITEM_DOCKKEY__);
			((Component)mLabels[1].FindChild("Label_2-2")).GetComponent<UILabel>().text = _clsRecord.DeckCount + "\n" + _clsRecord.ShipCount + "\n" + _clsRecord.SlotitemCount + "\n" + _clsRecord.NDockCount + "\n" + _clsRecord.KDockCount + "\n" + count + "\n" + _clsRecord.FurnitureCount;
			((Component)mLabels[2].FindChild("Label_3-2")).GetComponent<UILabel>().text = _clsRecord.DeckCountMax + "\n" + _clsRecord.ShipCountMax + "\n" + _clsRecord.SlotitemCountMax + "\n" + _clsRecord.MaterialMax + "\n\n\n";
			((Component)mLabels[2].FindChild("Label_3-3")).GetComponent<UILabel>().text = "♪「" + value + "」";
			string path = "Textures/Record/difficulty/diff_" + (int)_clsRecord.UserInfo.Difficulty;
			_diff1.mainTexture = (Resources.Load(path) as Texture);
			_diff2.mainTexture = (Resources.Load(path) as Texture);
			for (int j = 1; j <= 5; j++)
			{
				((Component)Medals.FindChild("medal_" + j + "/Label_4-" + j)).GetComponent<UILabel>().text = ((_clsRecord.GetClearCount(int2DiffKind(j)) < 2) ? string.Empty : ("×" + _clsRecord.GetClearCount(int2DiffKind(j))));
				Transform transform = Medals.FindChild("medal_" + j);
				Transform transform2 = transform.FindChild("medal");
				Transform transform3 = transform.FindChild("bg");
				if (_clsRecord.GetClearCount(int2DiffKind(j)) > 0)
				{
					transform2.localScale = Vector3.one;
					transform3.localScale = Vector3.zero;
					((Component)transform.FindChild("Light")).GetComponent<ParticleSystem>().Play();
				}
				else
				{
					transform2.localScale = Vector3.zero;
					transform3.localScale = Vector3.one;
					((Component)transform.FindChild("Light")).GetComponent<ParticleSystem>().Stop();
				}
			}
			switch (_SelectableDiff)
			{
			case 3:
				Medals.FindChild("medal_5").localPositionX(999f);
				Medals.FindChild("medal_4").localPositionX(999f);
				Medals.FindChild("medal_3").localPositionX(-166f);
				Medals.FindChild("medal_2").localPositionX(0f);
				Medals.FindChild("medal_1").localPositionX(166f);
				break;
			case 4:
				Medals.FindChild("medal_5").localPositionX(999f);
				Medals.FindChild("medal_4").localPositionX(-200f);
				Medals.FindChild("medal_3").localPositionX(-66.6f);
				Medals.FindChild("medal_2").localPositionX(66.6f);
				Medals.FindChild("medal_1").localPositionX(200f);
				break;
			case 5:
				Medals.FindChild("medal_5").localPositionX(-223f);
				Medals.FindChild("medal_4").localPositionX(-96f);
				Medals.FindChild("medal_3").localPositionX(16f);
				Medals.FindChild("medal_2").localPositionX(128f);
				Medals.FindChild("medal_1").localPositionX(240f);
				Medals.FindChild("medal_4/medal").localPositionX(-9f);
				Medals.FindChild("medal_4/Label_4-4").localPositionX(-9f);
				Medals.FindChild("medal_2/medal").localPositionX(3f);
				Medals.FindChild("medal_2/Label_4-2").localPositionX(3f);
				break;
			}
		}

		private void GoToStrategy()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
		}

		public void Pressed_Button_L(GameObject obj)
		{
			if (_now_page == 2)
			{
				_set_view_board(1);
			}
			else if (_now_page == 3)
			{
				_set_view_board(2);
			}
			else if (_now_page == 4)
			{
				_set_view_board(3);
			}
		}

		public void Pressed_Button_R(GameObject obj)
		{
			if (_now_page == 1)
			{
				_set_view_board(2);
			}
			else if (_now_page == 2)
			{
				_set_view_board(3);
			}
			else if (_now_page == 3 && _gotMedal)
			{
				_set_view_board(4);
			}
		}

		public void set_flagship_texture(ShipModel shipModel)
		{
			mTexture_SecurityShip = ((Component)mShipTexture).GetComponent<UITexture>();
			int texNum = (!shipModel.IsDamaged()) ? 9 : 10;
			mTexture_SecurityShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(shipModel.GetGraphicsMstId(), texNum);
			mTexture_SecurityShip.MakePixelPerfect();
			mTexture_SecurityShip.transform.localPosition = Util.Poi2Vec(shipModel.Offsets.GetShipDisplayCenter(shipModel.IsDamaged()));
		}

		public void SetColorText(Color col)
		{
			GameObject gameObject = mboardud1.gameObject;
			UILabel[] componentsInChildren = gameObject.GetComponentsInChildren<UILabel>();
			UILabel[] array = componentsInChildren;
			foreach (UILabel uILabel in array)
			{
				uILabel.color = col;
			}
		}

		public void SetTextShadow(bool shadow)
		{
			GameObject gameObject = mboardud1.gameObject;
			UILabel[] componentsInChildren = gameObject.GetComponentsInChildren<UILabel>();
			UILabel[] array = componentsInChildren;
			foreach (UILabel uILabel in array)
			{
				if (shadow)
				{
					uILabel.effectStyle = UILabel.Effect.Shadow;
				}
				else
				{
					uILabel.effectStyle = UILabel.Effect.None;
				}
			}
		}

		public void SetColorBG(Color col)
		{
			GameObject gameObject = mboardud1.gameObject;
			UITexture[] componentsInChildren = gameObject.GetComponentsInChildren<UITexture>();
			UITexture[] array = componentsInChildren;
			foreach (UITexture uITexture in array)
			{
				if (!uITexture.name.Contains("line"))
				{
					uITexture.color = col;
				}
			}
		}

		public void SetColorLine(Color col)
		{
			GameObject gameObject = mboardud1.gameObject;
			UITexture[] componentsInChildren = gameObject.GetComponentsInChildren<UITexture>();
			UITexture[] array = componentsInChildren;
			foreach (UITexture uITexture in array)
			{
				if (uITexture.name.Contains("line"))
				{
					uITexture.color = col;
				}
			}
		}

		public void SetColorIcon(Color col)
		{
			GameObject gameObject = mboardud1.gameObject;
			UISprite[] componentsInChildren = gameObject.GetComponentsInChildren<UISprite>();
			UISprite[] array = componentsInChildren;
			foreach (UISprite uISprite in array)
			{
				uISprite.color = col;
			}
		}

		public void SetCursorColor(Color col)
		{
			_Button_L.defaultColor = col;
			_Button_R.defaultColor = col;
			_Button_L.hover = col;
			_Button_R.hover = col;
			_Button_L.pressed = col;
			_Button_R.pressed = col;
		}

		private void OnDestroy()
		{
			if (mTexture_SecurityShip != null && mTexture_SecurityShip.mainTexture != null)
			{
				mTexture_SecurityShip = null;
			}
			if (mDifficulty != null)
			{
				mDifficulty = null;
			}
			if (mtouchEventArea != null)
			{
			}
			if (mMedalist != null)
			{
				mMedalist = null;
			}
			if (mboardud1 != null)
			{
				mboardud1 = null;
			}
			if (mWindowParam != null)
			{
				mWindowParam = null;
			}
			if (mShipTexture != null)
			{
				mShipTexture = null;
			}
			if (mbgWalls != null)
			{
				Mem.DelAry(ref mbgWalls);
			}
			if (mLabels != null)
			{
				Mem.DelAry(ref mLabels);
			}
			if (Medals != null)
			{
				Medals = null;
			}
			_now_page = 0;
			_dbg_class = 0;
			_Xpressed = false;
			_firstUpdate = false;
			needAnimation = false;
			_isTouch = false;
			_isScene = false;
			needUpdate = false;
			if (_ANIM_filebase != null)
			{
				_ANIM_filebase = null;
			}
			if (mSecurityShipModel != null)
			{
				mSecurityShipModel = null;
			}
			if (_clsRecord != null)
			{
				_clsRecord = null;
			}
			if (label != null)
			{
				label = null;
			}
			if (label2 != null)
			{
				label2 = null;
			}
			if (sprite != null)
			{
				sprite = null;
			}
			if (mTexture_SecurityShip != null)
			{
				mTexture_SecurityShip = null;
			}
			if (texture != null)
			{
				texture = null;
			}
			if (_Button_L != null)
			{
				_Button_L = null;
			}
			if (_Button_R != null)
			{
				_Button_R = null;
			}
			if (_Button_L_B != null)
			{
				_Button_L_B = null;
			}
			if (_Button_R_B != null)
			{
				_Button_R_B = null;
			}
			if ((Object)_AM != null)
			{
				_AM = null;
			}
			if ((Object)_AM_l != null)
			{
				_AM_l = null;
			}
			if ((Object)_AM_b != null)
			{
				_AM_b = null;
			}
			if (_SM != null)
			{
				_SM = null;
			}
			if (ItemSelectController != null)
			{
				ItemSelectController = null;
			}
			if (_AS != null)
			{
				_AS = null;
			}
			if (_board1 != null)
			{
				_board1 = null;
			}
			alphaZero_b = Color.black * 0f;
			__USEITEM_DOCKKEY__ = 0;
			if (_diff1 != null)
			{
				_diff1 = null;
			}
			if (_diff2 != null)
			{
				_diff2 = null;
			}
			_gotMedal = false;
			_SelectableDiff = 0;
		}
	}
}
