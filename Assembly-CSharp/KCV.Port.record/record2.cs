using Common.Enum;
using KCV.Display;
using KCV.Scene.Strategy;
using KCV.Strategy;
using KCV.Utils;
using local.managers;
using local.models;
using UnityEngine;

namespace KCV.Port.record
{
	public class record2 : MonoBehaviour
	{
		private bool _DEBUG_MODE_NOW_;

		private string _ANIM_filebase = "boards_mvud";

		private UIStageCover[] uiStageCovers;

		private UILabel label;

		private UILabel label2;

		private UISprite sprite;

		private UITexture texture;

		private UIButton _Button_L;

		private UIButton _Button_R;

		private UITexture _Button_L_B;

		private UITexture _Button_R_B;

		private UIButton _DBG_Button_L;

		private UIButton _DBG_Button_R;

		private Animation _AM;

		private Animation _AM_l;

		private Animation _AM_b;

		private SoundManager _SM;

		private KeyControl ItemSelectController;

		private int _now_page;

		private AsyncObjects _AS;

		private bool _StartUp;

		private int _flag_ship;

		private bool _damaged;

		private bool _isRecordScene;

		private GameObject _board1;

		private Color alphaZero_b = new Color(0f, 0f, 0f, 0f);

		private Color _Color_dock = new Color(0.51f, 0.953f, 0.357f);

		private int _dbg_class;

		private bool _Xpressed;

		private bool _firstUpdate;

		private bool _isAnime;

		private bool _isTouch;

		private bool _onYet;

		private bool _arrow_flag;

		private int _now_area;

		private CommonShipBanner[] csb = new CommonShipBanner[6];

		private StrategyMapManager strategyLogicManager;

		public bool getRecordDone()
		{
			return _Xpressed;
		}

		private void CompleteHandler(GameObject value)
		{
			_isAnime = false;
			_isTouch = false;
		}

		private void PageAnimeDone()
		{
			_isAnime = false;
		}

		private void Start()
		{
			_now_area = -1;
			_StartUp = false;
			_Xpressed = false;
			_firstUpdate = false;
			_isAnime = false;
			_isTouch = false;
			_arrow_flag = false;
			StartUp();
		}

		private void OnDestroy()
		{
			Mem.Del(ref label);
			Mem.Del(ref label2);
			Mem.Del(ref sprite);
			Mem.Del(ref texture);
			Mem.Del(ref _Button_L);
			Mem.Del(ref _Button_R);
			Mem.Del(ref _Button_L_B);
			Mem.Del(ref _Button_R_B);
			Mem.Del(ref _DBG_Button_L);
			Mem.Del(ref _DBG_Button_R);
			Mem.Del(ref _AM);
			Mem.Del(ref _AM_l);
			Mem.Del(ref _AM_b);
			uiStageCovers = null;
		}

		private void OnEnable()
		{
			strategyLogicManager = StrategyTopTaskManager.GetLogicManager();
			map_status();
			if (_onYet)
			{
				_Xpressed = false;
				_firstUpdate = false;
				_isAnime = false;
				_isTouch = false;
				_arrow_flag = false;
				_draw_labels();
				_board1 = GameObject.Find("board1");
				Object.Destroy(_board1.GetComponent<iTween>());
				_board1.transform.localPosition = Vector3.zero;
				_now_page = 1;
				_Button_L_B.transform.localScale = Vector3.zero;
				_Button_R_B.transform.localScale = Vector3.one;
			}
		}

		private void OnDisable()
		{
			if (uiStageCovers != null)
			{
				UIStageCover[] array = uiStageCovers;
				foreach (UIStageCover uIStageCover in array)
				{
					uIStageCover.SelfRelease();
				}
			}
		}

		private void SwipeJudgeDelegate(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (_isAnime)
			{
				_isTouch = false;
			}
			else if (actionType == UIDisplaySwipeEventRegion.ActionType.Moving && movedPercentageY >= 0.05f && !_isTouch)
			{
				_isTouch = true;
				Pressed_Button_L(null);
			}
			else if (actionType == UIDisplaySwipeEventRegion.ActionType.Moving && movedPercentageY <= -0.05f && !_isTouch)
			{
				_isTouch = true;
				Pressed_Button_R(null);
			}
			else if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
			{
				_isTouch = false;
			}
		}

		private void StartUp()
		{
			_StartUp = true;
			if (_DEBUG_MODE_NOW_)
			{
				_dbg_class = 1;
				_DBG_Button_L = ((Component)base.transform.FindChild("Debug_ship/DBG_Button_L")).GetComponent<UIButton>();
				_DBG_Button_R = ((Component)base.transform.FindChild("Debug_ship/DBG_Button_R")).GetComponent<UIButton>();
				UIButtonMessage component = _DBG_Button_L.GetComponent<UIButtonMessage>();
				component.target = base.gameObject;
				component.functionName = "Pressed_DBG_Button_L";
				component.trigger = UIButtonMessage.Trigger.OnClick;
				UIButtonMessage component2 = _DBG_Button_R.GetComponent<UIButtonMessage>();
				component2.target = base.gameObject;
				component2.functionName = "Pressed_DBG_Button_R";
				component2.trigger = UIButtonMessage.Trigger.OnClick;
			}
			UIDisplaySwipeEventRegion component3 = GameObject.Find("TouchEventArea").GetComponent<UIDisplaySwipeEventRegion>();
			Camera component4;
			if (Application.loadedLevelName == "Record")
			{
				_isRecordScene = true;
				component4 = GameObject.Find("Camera").GetComponent<Camera>();
				SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(delegate
				{
					ShipUtils.PlayShipVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip(), 8);
				});
			}
			else
			{
				_isRecordScene = false;
				component4 = GameObject.Find("OverViewCamera").GetComponent<Camera>();
			}
			component3.SetOnSwipeActionJudgeCallBack(SwipeJudgeDelegate);
			component3.SetEventCatchCamera(component4);
			_ANIM_filebase = "boards_mvud";
			_AM = GameObject.Find("RecordScene").GetComponent<Animation>();
			if (_isRecordScene)
			{
				_AM_l = GameObject.Find("medalist").GetComponent<Animation>();
			}
			_AM_b = GameObject.Find("btn").GetComponent<Animation>();
			_SM = SingletonMonoBehaviour<SoundManager>.Instance;
			_Button_L = ((Component)base.transform.FindChild("btn/Button_L")).GetComponent<UIButton>();
			_Button_R = ((Component)base.transform.FindChild("btn/Button_R")).GetComponent<UIButton>();
			_Button_L_B = ((Component)base.transform.FindChild("btn/Button_L/Background")).GetComponent<UITexture>();
			_Button_R_B = ((Component)base.transform.FindChild("btn/Button_R/Background")).GetComponent<UITexture>();
			_Button_L_B.transform.localScale = Vector3.zero;
			_Button_R_B.transform.localScale = Vector3.one;
			UIButtonMessage component5 = _Button_L.GetComponent<UIButtonMessage>();
			component5.target = base.gameObject;
			component5.functionName = "Pressed_Button_L";
			component5.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component6 = _Button_R.GetComponent<UIButtonMessage>();
			component6.target = base.gameObject;
			component6.functionName = "Pressed_Button_R";
			component6.trigger = UIButtonMessage.Trigger.OnClick;
			_board1 = GameObject.Find("board1");
			ItemSelectController = new KeyControl(0, 2);
			ItemSelectController.setChangeValue(-1f, 0f, 1f, 0f);
			_draw_labels();
			_now_page = 1;
			ShipModel shipModel = (SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel == null) ? new ShipModel(1) : SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel;
			_flag_ship = shipModel.GetGraphicsMstId();
			DamageState damageStatus = shipModel.DamageStatus;
			if (damageStatus == DamageState.Normal || damageStatus == DamageState.Shouha)
			{
				_damaged = false;
			}
			else
			{
				_damaged = true;
			}
		}

		private void _set_arrow()
		{
			if (!_isAnime && _arrow_flag)
			{
				if (_now_page == 1)
				{
					_AM_b.Play("btn_mvud_off_on");
					iTween.MoveTo(_Button_L_B.gameObject, iTween.Hash("islocal", true, "x", -10f, "time", 0.5f));
					_Button_L_B.transform.localScale = Vector3.zero;
					_Button_R_B.transform.localScale = Vector3.one;
				}
				if (_now_page == 2)
				{
					_AM_b.Play("btn_mvud_on_on");
					_Button_L_B.transform.localScale = Vector3.one;
					_Button_R_B.transform.localScale = Vector3.one;
				}
				if (_now_page == 3)
				{
					_AM_b.Play("btn_mvud_on_off");
					iTween.MoveTo(_Button_R_B.gameObject, iTween.Hash("islocal", true, "x", 10f, "time", 0.5f));
					_Button_L_B.transform.localScale = Vector3.one;
					_Button_R_B.transform.localScale = Vector3.zero;
				}
			}
		}

		private void _set_view_board(int page)
		{
			if (_isAnime)
			{
				return;
			}
			_onYet = true;
			_isAnime = true;
			switch (page)
			{
			case 1:
				if (_now_page != page)
				{
					SoundUtils.PlaySE(SEFIleInfos.MainMenuOnMouse);
					GameObject board2 = _board1;
					Vector3 localPosition3 = _board1.transform.localPosition;
					float x2 = localPosition3.x;
					Vector3 localPosition4 = _board1.transform.localPosition;
					TweenPosition tweenPosition2 = TweenPosition.Begin(board2, 0.5f, new Vector3(x2, 0f, localPosition4.z));
					tweenPosition2.animationCurve = UtilCurves.TweenEaseInOutQuad;
					tweenPosition2.SetOnFinished(PageAnimeDone);
					_AM.Play("boards_wait");
					_arrow_flag = true;
				}
				_now_page = 1;
				break;
			case 2:
				if (_now_page != page)
				{
					SoundUtils.PlaySE(SEFIleInfos.MainMenuOnMouse);
					GameObject board3 = _board1;
					Vector3 localPosition5 = _board1.transform.localPosition;
					float x3 = localPosition5.x;
					Vector3 localPosition6 = _board1.transform.localPosition;
					TweenPosition tweenPosition3 = TweenPosition.Begin(board3, 0.5f, new Vector3(x3, 544f, localPosition6.z));
					tweenPosition3.animationCurve = UtilCurves.TweenEaseInOutQuad;
					tweenPosition3.SetOnFinished(PageAnimeDone);
					_AM.Play("boards_wait");
					_arrow_flag = true;
				}
				_now_page = 2;
				break;
			default:
				if (_now_page != page)
				{
					SoundUtils.PlaySE(SEFIleInfos.MainMenuOnMouse);
					GameObject board = _board1;
					Vector3 localPosition = _board1.transform.localPosition;
					float x = localPosition.x;
					Vector3 localPosition2 = _board1.transform.localPosition;
					TweenPosition tweenPosition = TweenPosition.Begin(board, 0.5f, new Vector3(x, 1088f, localPosition2.z));
					tweenPosition.animationCurve = UtilCurves.TweenEaseInOutQuad;
					tweenPosition.SetOnFinished(PageAnimeDone);
					_AM.Play("boards_wait");
					_arrow_flag = true;
				}
				_now_page = 3;
				break;
			}
		}

		public void map_status()
		{
			uiStageCovers = GameObject.Find("board2nd/board1/page1/UIStageCovers").GetComponentsInChildren<UIStageCover>();
			MapAreaModel areaModel = StrategyTopTaskManager.Instance.TileManager.FocusTile.GetAreaModel();
			MapModel[] maps = StrategyTopTaskManager.GetLogicManager().SelectArea(areaModel.Id).Maps;
			UILabel component = GameObject.Find("board2nd/board1/page1/Labels/Label_0-1").GetComponent<UILabel>();
			component.text = Util.getDifficultyString(strategyLogicManager.UserInfo.Difficulty);
			UILabel component2 = GameObject.Find("board2nd/board1/page1/Labels/Label_1-2").GetComponent<UILabel>();
			component2.text = areaModel.Name;
			UILabel component3 = GameObject.Find("board2nd/board1/page2/Labels/Label_2-4").GetComponent<UILabel>();
			component3.supportEncoding = false;
			if (areaModel.Id < 15)
			{
				string name = areaModel.GetEscortDeck().Name;
				if (name.Replace(" ", string.Empty).Replace("\u3000", string.Empty).Length != 0)
				{
					component3.text = name;
				}
				else
				{
					component3.text = areaModel.Name.Replace("海域", string.Empty) + "航路護衛隊";
				}
			}
			else
			{
				component3.text = "---";
			}
			GameObject.Find("board2nd/board1/page2/Decks").transform.localPosition = new Vector3(-17.536f * (float)strategyLogicManager.UserInfo.DeckCount + 94.286f, 0f);
			for (int i = 0; i < 8; i++)
			{
				UISprite component4 = GameObject.Find("board2nd/board1/page2/Decks/Deck" + (i + 1).ToString()).GetComponent<UISprite>();
				component4.color = Color.black;
				if (i < strategyLogicManager.UserInfo.DeckCount)
				{
					component4.transform.localScale = Vector3.one;
				}
				else
				{
					component4.transform.localScale = Vector3.zero;
				}
			}
			for (int j = 0; j < areaModel.GetDecks().Length; j++)
			{
				UISprite component4 = GameObject.Find("board2nd/board1/page2/Decks/Deck" + areaModel.GetDecks()[j].Id).GetComponent<UISprite>();
				if (areaModel.GetDecks()[j].GetShipCount() != 0)
				{
					if (areaModel.GetDecks()[j].IsActionEnd())
					{
						component4.color = _Color_dock * 0.75f;
					}
					else if (areaModel.GetDecks()[j].MissionState != 0)
					{
						component4.color = Color.blue;
					}
					else
					{
						component4.color = _Color_dock;
					}
				}
			}
			if (maps.Length < 5)
			{
				UILabel component5 = GameObject.Find("board2nd/board1/page1/Labels/Label_1-2").GetComponent<UILabel>();
				component5.transform.localPosition = new Vector3(160f, 160f, 0f);
				component5.fontSize = 36;
				UILabel component6 = GameObject.Find("board2nd/board1/page1/Labels/Label_1-1").GetComponent<UILabel>();
				component6.transform.localPosition = new Vector3(160f, 105f, 0f);
				component6.fontSize = 32;
				UILabel component7 = GameObject.Find("board2nd/board1/page1/Labels/Label_0-0").GetComponent<UILabel>();
				component7.transform.localPosition = new Vector3(329f, 105f, 0f);
				component7.fontSize = 20;
				UILabel component8 = GameObject.Find("board2nd/board1/page1/Labels/Label_0-1").GetComponent<UILabel>();
				component8.transform.localPosition = new Vector3(413f, 105f, 0f);
				component8.fontSize = 20;
				UITexture component9 = GameObject.Find("board2nd/board1/page1/lines/line_1").GetComponent<UITexture>();
				if (component9 != null)
				{
					component9.transform.localPosition = new Vector3(160f, 103f, 0f);
					component9.width = 556;
					component9.height = 2;
				}
				for (int k = 0; k < 3; k++)
				{
					for (int l = 0; l < 2; l++)
					{
						int num = k * 2 + l + 1;
						GameObject gameObject = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover" + num.ToString());
						if (gameObject == null)
						{
							break;
						}
						gameObject.transform.localScale = Vector3.one * 0.6f;
						if (num < 5)
						{
							gameObject.transform.localPosition = new Vector3(18f + 293f * (float)l, -17f - 158f * (float)k, 0f);
						}
						else
						{
							gameObject.transform.localPosition = new Vector3(18f + 293f * (float)l, 320f, 0f);
						}
					}
				}
				if (maps.Length == 3)
				{
					GameObject gameObject2 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover4");
					gameObject2.transform.localScale = Vector3.zero;
					GameObject gameObject3 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover3");
					gameObject3.transform.localPosition = new Vector3(160f, -175f);
				}
				else if (maps.Length == 4)
				{
					GameObject gameObject4 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover4");
					gameObject4.transform.localScale = Vector3.one * 0.6f;
					GameObject gameObject5 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover3");
					gameObject5.transform.localPosition = new Vector3(18f, -175f);
				}
			}
			else
			{
				component2.transform.localPosition = new Vector3(160f, 171f, 0f);
				component2.fontSize = 28;
				UILabel component10 = GameObject.Find("board2nd/board1/page1/Labels/Label_1-1").GetComponent<UILabel>();
				component10.transform.localPosition = new Vector3(160f, 139f, 0f);
				component10.fontSize = 24;
				UILabel component11 = GameObject.Find("board2nd/board1/page1/Labels/Label_0-0").GetComponent<UILabel>();
				component11.transform.localPosition = new Vector3(329f, 149f, 0f);
				component11.fontSize = 20;
				component.transform.localPosition = new Vector3(413f, 149f, 0f);
				component.fontSize = 20;
				UITexture component12 = GameObject.Find("board2nd/board1/page1/lines/line_1").GetComponent<UITexture>();
				component12.transform.localPosition = new Vector3(160f, 143f, 0f);
				component12.width = 556;
				component12.height = 2;
				for (int m = 0; m < 3; m++)
				{
					for (int n = 0; n < 2; n++)
					{
						GameObject gameObject6 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover" + (m * 2 + n + 1).ToString());
						gameObject6.transform.localScale = Vector3.one * 0.5f;
						gameObject6.transform.localPosition = new Vector3(12f + 299f * (float)n, 40f - 122f * (float)m, 0f);
					}
				}
				if (maps.Length == 5)
				{
					GameObject gameObject7 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover5");
					gameObject7.transform.localPosition = new Vector3(162f, -209f, 0f);
					GameObject gameObject8 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover6");
					gameObject8.transform.localPosition = new Vector3(162f, 320f, 0f);
				}
			}
			for (int num2 = 0; num2 < maps.Length; num2++)
			{
				UIStageCover component13 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover" + (num2 + 1)).GetComponent<UIStageCover>();
				MapModel mapModel = maps[num2];
				component13.Initialize(mapModel);
			}
		}

		public void _draw_labels()
		{
			MapAreaModel areaModel = StrategyTopTaskManager.Instance.TileManager.FocusTile.GetAreaModel();
			RecordManager recordManager = new RecordManager();
			GameObject.Find("VERSION").GetComponent<UILabel>().text = "Version 1.02";
			string text = Util.RankNameJ(recordManager.Rank);
			if (_isRecordScene)
			{
				label = GameObject.Find("adm_name").GetComponent<UILabel>();
				label.text = recordManager.Name;
				label = GameObject.Find("adm_level").GetComponent<UILabel>();
				label.textInt = recordManager.Level;
				label = GameObject.Find("adm_status").GetComponent<UILabel>();
				label.text = text;
				label = GameObject.Find("adm_exp").GetComponent<UILabel>();
				label.text = recordManager.Experience + "/" + recordManager.NextExperience;
			}
			string text2 = recordManager.DeckCount + "\n" + recordManager.ShipCount + " / " + recordManager.ShipCountMax + "\n" + recordManager.SlotitemCount + " / " + recordManager.SlotitemCountMax + "\n" + recordManager.MaterialMax + "\n" + recordManager.NDockCount + "\n";
			string text3;
			if (areaModel.NDockMax != 0)
			{
				text3 = text2;
				text2 = text3 + areaModel.NDockCount + " / " + areaModel.NDockMax + "\n";
			}
			else
			{
				text2 += "－ / －\n";
			}
			text3 = text2;
			text2 = text3 + recordManager.KDockCount + " / " + 4;
			GameObject.Find("Label_3-2").GetComponent<UILabel>().text = text2;
			for (int i = 0; i < areaModel.GetEscortDeck().Count; i++)
			{
				csb[i] = GameObject.Find("board2nd/board1/page2/banners/banner" + (i + 1).ToString() + "/CommonShipBanner2").GetComponent<CommonShipBanner>();
				csb[i].SetShipData(areaModel.GetEscortDeck().GetShips()[i]);
				csb[i].transform.localScale = Vector3.one * (45f / 64f);
			}
			for (int j = areaModel.GetEscortDeck().Count; j < 6; j++)
			{
				csb[j] = GameObject.Find("board2nd/board1/page2/banners/banner" + (j + 1).ToString() + "/CommonShipBanner2").GetComponent<CommonShipBanner>();
				csb[j].transform.localScale = Vector3.zero;
				UITexture component = GameObject.Find("board2nd/board1/page2/banners/banner" + (j + 1).ToString() + "/BannerBG").GetComponent<UITexture>();
				component.color = Color.gray / 2f;
			}
			UILabel component2 = GameObject.Find("board2nd/board1/page2/Labels/Label_2-2").GetComponent<UILabel>();
			int countNoMove = areaModel.GetTankerCount().GetCountNoMove();
			int maxCount = areaModel.GetTankerCount().GetMaxCount();
			if (areaModel.Id < 15)
			{
				component2.text = countNoMove.ToString() + "/" + maxCount.ToString();
			}
			else
			{
				component2.text = "---";
			}
			if (areaModel.Id < 15)
			{
				component2 = GameObject.Find("board2nd/board1/page2/material/GetMaterial1/num").GetComponent<UILabel>();
				component2.text = "× " + string.Format("{0, 3}", areaModel.GetResources(countNoMove)[enumMaterialCategory.Fuel]);
				component2 = GameObject.Find("board2nd/board1/page2/material/GetMaterial3/num").GetComponent<UILabel>();
				component2.text = "× " + string.Format("{0, 3}", areaModel.GetResources(countNoMove)[enumMaterialCategory.Steel]);
				component2 = GameObject.Find("board2nd/board1/page2/material/GetMaterial2/num").GetComponent<UILabel>();
				component2.text = "× " + string.Format("{0, 3}", areaModel.GetResources(countNoMove)[enumMaterialCategory.Bull]);
				component2 = GameObject.Find("board2nd/board1/page2/material/GetMaterial4/num").GetComponent<UILabel>();
				component2.text = "× " + string.Format("{0, 3}", areaModel.GetResources(countNoMove)[enumMaterialCategory.Bauxite]);
			}
			else
			{
				GameObject.Find("board2nd/board1/page2/material/GetMaterial1/num").GetComponent<UILabel>().text = "× ---";
				GameObject.Find("board2nd/board1/page2/material/GetMaterial3/num").GetComponent<UILabel>().text = "× ---";
				GameObject.Find("board2nd/board1/page2/material/GetMaterial2/num").GetComponent<UILabel>().text = "× ---";
				GameObject.Find("board2nd/board1/page2/material/GetMaterial4/num").GetComponent<UILabel>().text = "× ---";
			}
		}

		private void Update()
		{
			if (!_StartUp)
			{
				return;
			}
			_set_arrow();
			if (!_firstUpdate)
			{
				_firstUpdate = false;
				base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			ItemSelectController.Update();
			if (ItemSelectController.keyState[8].down)
			{
				Pressed_Button_L(null);
			}
			if (ItemSelectController.keyState[12].down)
			{
				Pressed_Button_R(null);
			}
			if (ItemSelectController.keyState[0].down)
			{
				_Xpressed = true;
				if (_isRecordScene)
				{
					_StartUp = false;
					back_to_port();
				}
			}
			if (_DEBUG_MODE_NOW_)
			{
				if (ItemSelectController.keyState[4].down)
				{
					Pressed_DBG_Button_L(null);
				}
				if (ItemSelectController.keyState[5].down)
				{
					Pressed_DBG_Button_R(null);
				}
				if (ItemSelectController.keyState[8].down)
				{
					Pressed_DBG_Button_UP(null);
				}
				if (ItemSelectController.keyState[12].down)
				{
					Pressed_DBG_Button_DOWN(null);
				}
			}
		}

		private void pop_record()
		{
			for (int i = 1; i <= 3; i++)
			{
				texture = GameObject.Find("board1/page" + i + "/bg_class").GetComponent<UITexture>();
				texture.mainTexture = null;
				Resources.UnloadAsset(texture.mainTexture);
			}
			texture = GameObject.Find("Secretary/shipgirl").GetComponent<UITexture>();
			texture.mainTexture = null;
			Resources.UnloadAsset(texture.mainTexture);
		}

		private void back_to_port()
		{
			_StartUp = false;
			SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
			{
				pop_record();
				AsyncLoadScene.LoadLevelAsyncScene(this, Generics.Scene.PortTop.ToString(), null);
			});
		}

		private void Medalist_Anime()
		{
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
		}

		public void Pressed_DBG_Button_L(GameObject obj)
		{
			if (_flag_ship > 1)
			{
				_flag_ship--;
				set_flagship_texture(_flag_ship);
				if (GameObject.Find("Secretary/shipgirl").GetComponent<UITexture>().mainTexture == null)
				{
					GameObject.Find("Debug_ship/Label_ship").GetComponent<UILabel>().text = "[ffddaa]" + _flag_ship;
				}
				else
				{
					GameObject.Find("Debug_ship/Label_ship").GetComponent<UILabel>().text = "[ffffff]" + _flag_ship;
				}
			}
		}

		public void Pressed_DBG_Button_R(GameObject obj)
		{
			if (_flag_ship <= 600)
			{
				_flag_ship++;
				set_flagship_texture(_flag_ship);
				if (GameObject.Find("Secretary/shipgirl").GetComponent<UITexture>().mainTexture == null)
				{
					GameObject.Find("Debug_ship/Label_ship").GetComponent<UILabel>().text = "[ffddaa]" + _flag_ship;
				}
				else
				{
					GameObject.Find("Debug_ship/Label_ship").GetComponent<UILabel>().text = "[ffffff]" + _flag_ship;
				}
			}
		}

		public void Pressed_DBG_Button_UP(GameObject obj)
		{
			if (_dbg_class != 10)
			{
				_dbg_class++;
				for (int i = 1; i <= 3; i++)
				{
					texture = GameObject.Find("board1/page" + i + "/bg_class").GetComponent<UITexture>();
					texture.mainTexture = (Resources.Load("Textures/Record/RecordTextures/NewUI/class_bg/class_" + $"{_dbg_class:00}") as Texture);
					texture.MakePixelPerfect();
				}
			}
		}

		public void Pressed_DBG_Button_DOWN(GameObject obj)
		{
			if (_dbg_class != 1)
			{
				_dbg_class--;
				for (int i = 1; i <= 3; i++)
				{
					texture = GameObject.Find("board1/page" + i + "/bg_class").GetComponent<UITexture>();
					texture.mainTexture = (Resources.Load("Textures/Record/RecordTextures/NewUI/class_bg/class_" + $"{_dbg_class:00}") as Texture);
					texture.MakePixelPerfect();
				}
			}
		}

		public void set_flagship_texture(int _flag_ship)
		{
			new ShipOffset(_flag_ship);
			RecordShipLocation recordShipLocation = Resources.Load<RecordShipLocation>("Data/RecordShipLocation");
			float num;
			float num2;
			int texNum;
			if (!_damaged)
			{
				num = (float)recordShipLocation.param[_flag_ship].Rec_X2 - 370f;
				num2 = (float)recordShipLocation.param[_flag_ship].Rec_Y2 - 180f;
				texNum = 9;
			}
			else
			{
				num = (float)recordShipLocation.param[_flag_ship].Rec_dam_X2 - 370f;
				num2 = (float)recordShipLocation.param[_flag_ship].Rec_dam_Y2 - 180f;
				texNum = 10;
			}
			GameObject.Find("Secretary/shipgirl").transform.localPositionX(num);
			GameObject.Find("Secretary/shipgirl").transform.localPositionY(num2);
			GameObject.Find("Secretary/shipgirl").GetComponent<UITexture>().mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(_flag_ship, texNum);
			GameObject.Find("Secretary/shipgirl").GetComponent<UITexture>().MakePixelPerfect();
			Vector2 localSize = GameObject.Find("Secretary/shipgirl").GetComponent<UITexture>().localSize;
			GameObject.Find("Secretary/shipgirl").GetComponent<UITexture>().SetDimensions((int)localSize.x, (int)localSize.y);
			if (_DEBUG_MODE_NOW_)
			{
				mst_shipgraphfaceanchor mst_shipgraphfaceanchor = Resources.Load<mst_shipgraphfaceanchor>("Data/Mst_ShipGraphFaceAnchor");
				float x = (float)mst_shipgraphfaceanchor.param[_flag_ship].facea9_x + num + 108f;
				float y = (float)mst_shipgraphfaceanchor.param[_flag_ship].facea9_y + num2 - 152f;
				GameObject.Find("Secretary/facea").transform.localPositionX(x);
				GameObject.Find("Secretary/facea").transform.localPositionY(y);
				x = (float)mst_shipgraphfaceanchor.param[_flag_ship].faceb9_x + num + 108f;
				y = (float)mst_shipgraphfaceanchor.param[_flag_ship].faceb9_y + num2 - 152f;
				GameObject.Find("Secretary/faceb").transform.localPositionX(x);
				GameObject.Find("Secretary/faceb").transform.localPositionY(y);
			}
		}

		public void SetCursorColor(Color col)
		{
			GameObject.Find("RecordScene/btn/Button_L").GetComponent<UIButton>().defaultColor = col;
			GameObject.Find("RecordScene/btn/Button_R").GetComponent<UIButton>().defaultColor = col;
			GameObject.Find("RecordScene/btn/Button_L").GetComponent<UIButton>().hover = col;
			GameObject.Find("RecordScene/btn/Button_R").GetComponent<UIButton>().hover = col;
			GameObject.Find("RecordScene/btn/Button_L").GetComponent<UIButton>().pressed = col;
			GameObject.Find("RecordScene/btn/Button_R").GetComponent<UIButton>().pressed = col;
		}
	}
}
