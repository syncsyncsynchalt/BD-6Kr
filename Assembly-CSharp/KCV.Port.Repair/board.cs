using Common.Enum;
using KCV.PopupString;
using KCV.Utils;
using local.managers;
using local.models;
using System.Collections;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class board : MonoBehaviour
	{
		private repair rep;

		private UIGrid grid;

		private UISprite bds;

		private UISprite bds2;

		private UISprite ele_d;

		private UILabel lab;

		private UIButton uibutton;

		private sw swsw;

		private GameObject[] btn_Stk = new GameObject[4];

		private GameObject[] r_now = new GameObject[4];

		private GameObject[] dock = new GameObject[4];

		private GameObject[] dock_cursor = new GameObject[4];

		private GameObject[] repair_btn = new GameObject[4];

		private GameObject[] kira_obj = new GameObject[4];

		private GameObject[] label1_obj = new GameObject[4];

		private GameObject[] label2_obj = new GameObject[4];

		private GameObject[] label3_obj = new GameObject[4];

		private GameObject[] shutter = new GameObject[4];

		private int[] dock_flag = new int[4];

		private int[] MaxHP = new int[4];

		private bool[] dock_anime = new bool[4];

		private bool[] dock_HS_anime = new bool[4];

		private RepairManager _clsRepair;

		private UITexture tex;

		private ShipModel _clsShipModel;

		private board2 bd2;

		private board3 bd3;

		private dialog dia;

		private dialog2 dia2;

		private dialog3 dia3;

		private KeyControl dockSelectController;

		private GameObject cursor;

		private int _go_kosoku;

		private int _select_dock;

		private int _go_kosoku_m;

		private int _select_dock_m;

		private int dock_count;

		private bool _dock_exist;

		private bool _HOLT;

		private bool _HOLT_DIALOG;

		private bool _isStartUpDone;

		private Animation _ANI;

		private bool _isBtnMaruUp;

		private bool _first_key;

		private int now_kit;

		private bool _already_des;

		private int now_dock;

		private bool now_high;

		[SerializeField]
		private ButtonLightTexture[] btnLight = new ButtonLightTexture[4];

		private Color TextShadowLight = new Color(0.63f, 0.91f, 1f);

		private int _imfocus;

		private void btnLights(bool Play)
		{
			btnLights(Play, force: false);
		}

		private void btnLights(bool Play, bool force)
		{
			if (now_high == Play && !force)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				if (Play)
				{
					btnLight[i].PlayAnim();
				}
				else
				{
					btnLight[i].StopAnim();
				}
			}
			now_high = Play;
		}

		public void dock_flag_init()
		{
			for (int i = 0; i < _clsRepair.GetDocks().Length; i++)
			{
				if (_clsRepair.GetDockData(i).ShipId != 0 && _clsRepair.GetDockData(i).RemainingTurns != 0)
				{
					dock_flag[i] = 2;
				}
				else
				{
					dock_flag[i] = 1;
				}
			}
			for (int j = _clsRepair.GetDocks().Length; j < _clsRepair.MapArea.NDockMax; j++)
			{
				dock_flag[j] = 0;
			}
			for (int k = _clsRepair.MapArea.NDockMax; k < 4; k++)
			{
				dock_flag[k] = -1;
			}
		}

		public void set_anime(int no, bool stat)
		{
			dock_anime[no] = stat;
			if (stat)
			{
				int num = no;
				GameObject.Find("board1_top/board/Grid/0" + num.ToString() + "/repair_now/btn_high_repair").transform.localScale = Vector3.one;
			}
			else
			{
				int num2 = no;
				GameObject.Find("board1_top/board/Grid/0" + num2.ToString() + "/repair_now/btn_high_repair").transform.localScale = Vector3.zero;
			}
		}

		public bool get_anime(int no)
		{
			return dock_anime[no];
		}

		public void set_HS_anime(int no, bool stat)
		{
			if (dock_HS_anime[no] != stat)
			{
				dock_HS_anime[no] = stat;
				if (stat)
				{
					now_kit--;
				}
			}
		}

		public bool get_HS_anime(int no)
		{
			return dock_HS_anime[no];
		}

		public GameObject get_dock_obj(int i)
		{
			return dock[i];
		}

		public void ResetMaruKey()
		{
			_isBtnMaruUp = false;
		}

		public static int SortByNameDesc(Transform a, Transform b)
		{
			return string.Compare(b.name, a.name);
		}

		public void set_cx(int cx)
		{
			_go_kosoku = cx;
		}

		public void set_rnow_enable(int dock, bool a)
		{
			r_now[dock].SetActive(a);
		}

		public void set_stk_enable(int dock, bool a)
		{
			btn_Stk[dock].SetActive(a);
		}

		public int get_dock_MaxHP(int dock)
		{
			return (dock >= 0 && dock <= 3) ? MaxHP[dock] : (-1);
		}

		public void set_dock_MaxHP(int dock, int value)
		{
			if (dock >= 0 && dock <= 3)
			{
				MaxHP[dock] = value;
			}
		}

		public int get_dock_count()
		{
			return dock_count;
		}

		public int get_dock_touchable_count()
		{
			int num = dock_count;
			if (num == _clsRepair.MapArea.NDockMax)
			{
				return num;
			}
			return num + 1;
		}

		private void Start()
		{
			_init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del(ref rep);
			Mem.Del(ref grid);
			Mem.Del(ref bds);
			Mem.Del(ref bds2);
			Mem.Del(ref ele_d);
			Mem.Del(ref lab);
			Mem.Del(ref uibutton);
			Mem.Del(ref swsw);
			Mem.Del(ref btn_Stk);
			Mem.Del(ref r_now);
			Mem.Del(ref dock);
			Mem.Del(ref dock_cursor);
			Mem.Del(ref repair_btn);
			Mem.Del(ref kira_obj);
			Mem.Del(ref label1_obj);
			Mem.Del(ref label2_obj);
			Mem.Del(ref label3_obj);
			Mem.Del(ref shutter);
			Mem.Del(ref dock_flag);
			Mem.Del(ref MaxHP);
			Mem.Del(ref dock_anime);
			Mem.Del(ref dock_HS_anime);
			Mem.Del(ref _clsRepair);
			Mem.Del(ref tex);
			Mem.Del(ref _clsShipModel);
			Mem.Del(ref bd2);
			Mem.Del(ref bd3);
			Mem.Del(ref dia);
			Mem.Del(ref dia2);
			Mem.Del(ref dia3);
			Mem.Del(ref dockSelectController);
			Mem.Del(ref cursor);
			Mem.Del(ref _ANI);
			Mem.Del(ref btnLight);
			Mem.Del(ref TextShadowLight);
		}

		public void _init_repair()
		{
			_already_des = false;
			_isBtnMaruUp = false;
			_HOLT = false;
			_HOLT_DIALOG = false;
			_isStartUpDone = false;
			_isBtnMaruUp = false;
			rep = ((Component)base.gameObject.transform.parent.parent).GetComponent<repair>();
		}

		public void StartUP()
		{
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			rep.set_mode(-1);
			if (!_isStartUpDone)
			{
				_isStartUpDone = true;
				bd2 = ((Component)rep.transform.FindChild("board2_top/board2")).GetComponent<board2>();
				bd3 = ((Component)rep.transform.FindChild("board3_top/board3")).GetComponent<board3>();
				dia = GameObject.Find("dialog").GetComponent<dialog>();
				dia2 = GameObject.Find("dialog2").GetComponent<dialog2>();
				dia3 = GameObject.Find("dialog3").GetComponent<dialog3>();
				for (int i = 0; i < 4; i++)
				{
					GameObject[] array = shutter;
					int num = i;
					int num2 = i;
					array[num] = GameObject.Find("board1_top/board/Grid/0" + num2.ToString() + "/Shutter");
				}
				_clsRepair = rep.now_clsRepair();
				now_kit = _clsRepair.Material.RepairKit;
				_ANI = ((Component)rep.transform.FindChild("info")).GetComponent<Animation>();
				int num3 = _clsRepair.MapArea.NDockCount;
				if (num3 == _clsRepair.MapArea.NDockMax)
				{
					num3--;
				}
				dockSelectController = new KeyControl(0, num3);
				dock_flag_init();
				dockSelectController.setChangeValue(-1f, 0f, 1f, 0f);
				dockSelectController.isLoopIndex = false;
				_first_key = false;
				_go_kosoku = 0;
				_go_kosoku_m = -1;
				_select_dock = 0;
				_select_dock_m = -1;
				_dock_exist = false;
				redraw();
				bd2.StartUp();
				bd3.StartUp();
			}
		}

		public void redraw()
		{
			redraw(anime: true);
		}

		public void redraw(bool anime)
		{
			redraw(anime: true, -1);
		}

		public void redraw(bool anime, int mode)
		{
			rep.set_mode(1);
			if (!_dock_exist)
			{
				board_dock_clear();
			}
			if (anime)
			{
				DockMake(rep.NowArea());
			}
			else if (!_dock_exist)
			{
				DockMake(rep.NowArea(), anime: false, mode);
			}
			else
			{
				DockMake(rep.NowArea(), anime: false);
			}
			DockStatus(rep.NowArea());
		}

		public void DockMake()
		{
			DockMake(rep.NowArea(), anime: true);
		}

		public void DockMake(int MapArea)
		{
			DockMake(MapArea, anime: true);
		}

		public void DockMake(int MapArea, bool anime)
		{
			DockMake(MapArea, anime, -1);
		}

		public void DockMake(int MapArea, bool anime, int mode)
		{
			int num = 4;
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			UIGrid component = GameObject.Find("board1_top/board/Grid").GetComponent<UIGrid>();
			if (!_dock_exist)
			{
				for (int i = 0; i < num; i++)
				{
					GameObject[] array = dock;
					int num2 = i;
					int num3 = i;
					array[num2] = GameObject.Find("board1_top/board/Grid/0" + num3.ToString());
					dock[i].name = string.Empty + $"{i:00}";
					dock[i].gameObject.transform.localScale = Vector3.one;
					shutter[i].transform.localScale = Vector3.one;
					dock_cursor[i] = GameObject.Find("board1_top/board/Grid/" + dock[i].name + "/bg/BackGround");
					GameObject[] array2 = kira_obj;
					int num4 = i;
					int num5 = i;
					array2[num4] = GameObject.Find("board1_top/board/Grid/0" + num5.ToString() + "/Anime_H/crane1/wire/item/Light").gameObject;
					GameObject[] array3 = btn_Stk;
					int num6 = i;
					int num7 = i;
					array3[num6] = GameObject.Find("board/Grid/0" + num7.ToString() + "/btn_Sentaku");
					GameObject[] array4 = r_now;
					int num8 = i;
					int num9 = i;
					array4[num8] = GameObject.Find("board/Grid/0" + num9.ToString() + "/repair_now");
					repair_btn[i] = GameObject.Find("board/Grid/0" + i + "/repair_now/btn_high_repair/Background");
					label1_obj[i] = GameObject.Find("board/Grid/0" + i + "/repair_now/text_lv");
					label2_obj[i] = GameObject.Find("board/Grid/0" + i + "/repair_now/text_day");
					label3_obj[i] = GameObject.Find("board/Grid/0" + i + "/repair_now/text_ato");
					dock[i].gameObject.transform.localPosition(new Vector3(0f, (float)i * -100f, 0f));
					GameObject.Find("board1_top/board/Grid/" + dock[i].name + "/Anime").GetComponent<UIPanel>().depth = i + 14;
					GameObject.Find("board1_top/board/Grid/" + dock[i].name + "/Anime_H").GetComponent<UIPanel>().depth = i + 14;
				}
			}
			for (int j = 0; j < num; j++)
			{
				if (!anime)
				{
					if (mode == -1)
					{
						iTween.MoveTo(dock[j].gameObject, iTween.Hash("islocal", true, "y", (float)j * -100f + 530f, "time", 0f));
					}
					else if (mode == j)
					{
						iTween.MoveTo(dock[j].gameObject, iTween.Hash("islocal", true, "y", (float)j * -100f + 530f, "time", 0f));
					}
				}
			}
			component.Reposition();
			_dock_exist = true;
		}

		public void DockStatus()
		{
			DockStatus(rep.NowArea(), -1);
		}

		public void DockStatus(int MapArea)
		{
			DockStatus(MapArea, -1);
		}

		public void DockStatus(int MapArea, int TargetDock)
		{
			_first_key = true;
			rep.GetMstManager();
			rep.GetMst_ship();
			rep.update_portframe();
			_clsRepair = rep.now_clsRepair();
			dock_count = _clsRepair.GetDocks().Length;
			for (int i = 0; i < dock_count; i++)
			{
				int num = i;
				bds2 = GameObject.Find("board1_top/board/Grid/0" + num.ToString() + "/bg/BackGround").GetComponent<UISprite>();
				bds2.spriteName = "list_bg";
				int num2 = i;
				bds2 = GameObject.Find("board1_top/board/Grid/0" + num2.ToString() + "/bg/BackGround3").GetComponent<UISprite>();
				bds2.spriteName = "cardArea_YB";
				int num3 = i;
				bds2 = GameObject.Find("board1_top/board/Grid/0" + num3.ToString() + "/bg/BackGround2").GetComponent<UISprite>();
				bds2.spriteName = AreaIdToSeaSpriteName(rep.NowArea());
				int num4 = i;
				GameObject.Find("board1_top/board/Grid/0" + num4.ToString() + "/Shutter/BGKey").transform.localScaleZero();
				int num5 = i;
				GameObject.Find("board1_top/board/Grid/0" + num5.ToString() + "/Shutter/BGMes").transform.localScaleZero();
				if (TargetDock == -1)
				{
					shutter[i].transform.localScale = Vector3.zero;
				}
				dock_cursor[i] = GameObject.Find("board1_top/board/Grid/" + dock[i].name + "/bg/BackGround");
				btn_Stk[i].SetActive(true);
			}
			int nDockMax = _clsRepair.MapArea.NDockMax;
			for (int j = dock_count; j < nDockMax; j++)
			{
				if (TargetDock != -1 && TargetDock != j)
				{
					continue;
				}
				int num6 = j;
				bds2 = GameObject.Find("board1_top/board/Grid/0" + num6.ToString() + "/bg/BackGround").GetComponent<UISprite>();
				bds2.spriteName = "list_bg_bar_closed";
				int num7 = j;
				bds2 = GameObject.Find("board1_top/board/Grid/0" + num7.ToString() + "/bg/BackGround3").GetComponent<UISprite>();
				bds2.spriteName = null;
				int num8 = j;
				bds2 = GameObject.Find("board1_top/board/Grid/0" + num8.ToString() + "/bg/BackGround2").GetComponent<UISprite>();
				bds2.spriteName = null;
				if (_first_key)
				{
					if (_clsRepair.IsValidOpenNewDock())
					{
						int num9 = j;
						GameObject.Find("board1_top/board/Grid/0" + num9.ToString() + "/Shutter/BGMes").GetComponent<UISprite>().spriteName = "huki_r_02";
						int num10 = j;
						GameObject.Find("board1_top/board/Grid/0" + num10.ToString() + "/Shutter/BGKey").GetComponent<UISprite>().spriteName = "btn_addDock";
					}
					else
					{
						int num11 = j;
						GameObject.Find("board1_top/board/Grid/0" + num11.ToString() + "/Shutter/BGMes").GetComponent<UISprite>().spriteName = "huki_r_01";
						int num12 = j;
						GameObject.Find("board1_top/board/Grid/0" + num12.ToString() + "/Shutter/BGKey").GetComponent<UISprite>().spriteName = "btn_addDock";
					}
					int num13 = j;
					GameObject.Find("board1_top/board/Grid/0" + num13.ToString() + "/Shutter/BGKey").transform.localScaleOne();
					_first_key = false;
				}
				else
				{
					int num14 = j;
					GameObject.Find("board1_top/board/Grid/0" + num14.ToString() + "/Shutter/BGKey").transform.localScaleZero();
					int num15 = j;
					GameObject.Find("board1_top/board/Grid/0" + num15.ToString() + "/Shutter/BGMes").transform.localScaleZero();
				}
				shutter[j].transform.localScale = Vector3.one;
				dock_cursor[j] = GameObject.Find("board1_top/board/Grid/" + dock[j].name + "/Shutter/ShutterALL");
				dock_cursor[j].transform.localScale = Vector3.one;
				btn_Stk[j].SetActive((j <= dock_count) ? true : false);
				r_now[j].SetActive(false);
			}
			for (int k = nDockMax; k < 4; k++)
			{
				if (TargetDock == -1 || TargetDock == k)
				{
					int num16 = k;
					bds2 = GameObject.Find("board1_top/board/Grid/0" + num16.ToString() + "/bg/BackGround").GetComponent<UISprite>();
					bds2.spriteName = null;
					int num17 = k;
					bds2 = GameObject.Find("board1_top/board/Grid/0" + num17.ToString() + "/bg/BackGround3").GetComponent<UISprite>();
					bds2.spriteName = null;
					int num18 = k;
					bds2 = GameObject.Find("board1_top/board/Grid/0" + num18.ToString() + "/bg/BackGround2").GetComponent<UISprite>();
					bds2.spriteName = null;
					int num19 = k;
					GameObject.Find("board1_top/board/Grid/0" + num19.ToString() + "/Shutter/BGKey").transform.localScaleZero();
					int num20 = k;
					GameObject.Find("board1_top/board/Grid/0" + num20.ToString() + "/Shutter/BGMes").transform.localScaleZero();
					shutter[k].transform.localScale = Vector3.zero;
					btn_Stk[k].SetActive(false);
					r_now[k].SetActive(false);
				}
			}
			for (int l = 0; l < dock_count; l++)
			{
				crane_anime component = GameObject.Find("board/Grid/0" + l.ToString() + "/Anime").GetComponent<crane_anime>();
				ShipModel ship = _clsRepair.GetDockData(l).GetShip();
				if (TargetDock != -1 && TargetDock != l)
				{
					continue;
				}
				if (_clsRepair.GetDockData(l).ShipId != 0 && _clsRepair.GetDockData(l).RemainingTurns != 0)
				{
					r_now[l].SetActive(true);
					btn_Stk[l].SetActive(false);
					int num21 = l;
					tex = GameObject.Find("board/Grid/0" + num21.ToString() + "/repair_now/ship_banner").GetComponent<UITexture>();
					if (ship.DamageStatus == DamageState.Taiha)
					{
						tex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(ship.MstId, 2);
					}
					else if (ship.DamageStatus == DamageState.Tyuuha)
					{
						tex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(ship.MstId, 2);
					}
					else
					{
						tex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(ship.MstId, 1);
					}
					int num22 = l;
					lab = GameObject.Find("board/Grid/0" + num22.ToString() + "/repair_now/text_ship_name").GetComponent<UILabel>();
					lab.text = ship.Name;
					int num23 = l;
					lab = GameObject.Find("board/Grid/0" + num23.ToString() + "/repair_now/text_level").GetComponent<UILabel>();
					lab.text = string.Empty + ship.Level;
					int num24 = l;
					lab = GameObject.Find("board/Grid/0" + num24.ToString() + "/repair_now/text_hp").GetComponent<UILabel>();
					lab.text = ship.NowHp + "/" + ship.MaxHp;
					MaxHP[l] = ship.MaxHp;
					int num25 = l;
					ele_d = GameObject.Find("board/Grid/0" + num25.ToString() + "/repair_now/HP_Gauge/panel/HP_bar_meter2").GetComponent<UISprite>();
					ele_d.width = (int)((float)ship.NowHp * 210f / (float)ship.MaxHp);
					ele_d.color = Util.HpGaugeColor2(ship.MaxHp, ship.NowHp);
					int num26 = l;
					ele_d = GameObject.Find("board/Grid/0" + num26.ToString() + "/repair_now/HP_Gauge/panel/HP_bar_meter").GetComponent<UISprite>();
					ele_d.width = (int)((float)ship.NowHp * 210f / (float)ship.MaxHp);
					ele_d.color = Util.HpGaugeColor2(ship.MaxHp, ship.NowHp);
					int num27 = l;
					lab = GameObject.Find("board/Grid/0" + num27.ToString() + "/repair_now/text_least_time").GetComponent<UILabel>();
					lab.text = string.Empty + _clsRepair.GetDockData(l).RemainingTurns;
					int num28 = l;
					uibutton = GameObject.Find("board/Grid/0" + num28.ToString() + "/repair_now/btn_high_repair").GetComponent<UIButton>();
					if (_clsRepair.Material.RepairKit > 0)
					{
						uibutton.isEnabled = true;
					}
					else
					{
						uibutton.isEnabled = false;
					}
					set_anime(l, stat: false);
					component.start_anime(l);
				}
				else
				{
					r_now[l].SetActive(false);
					btn_Stk[l].SetActive(true);
					component.stop_anime(l);
					set_anime(l, stat: false);
				}
			}
			swsw = GameObject.Find("board3_top/board3/sw01").GetComponent<sw>();
			swsw.set_sw_stat((_clsRepair.Material.RepairKit > 0) ? true : false);
			_dock_exist = true;
		}

		public void set_kira(bool value)
		{
			if (!(kira_obj[0] == null))
			{
				for (int i = 0; i < 4; i++)
				{
					kira_obj[i].SetActive(value);
				}
			}
		}

		public void board_dock_clear()
		{
			if (_dock_exist)
			{
				grid = GameObject.Find("board1_top/board/Grid").GetComponent<UIGrid>();
				GameObject[] children = grid.gameObject.GetChildren(includeInactive: true);
				for (int i = 0; i < children.Length; i++)
				{
					Object.Destroy(children[i]);
				}
				grid.transform.DetachChildren();
				_dock_exist = false;
			}
		}

		public void DockCursorBlink(int index)
		{
			UISelectedObject.SelectedObjectBlink(dock_cursor, index);
		}

		private void Update()
		{
			if (!_isStartUpDone || !rep.isFadeDone())
			{
				StartUP();
			}
			if (!_isStartUpDone || _HOLT)
			{
				return;
			}
			int index = dockSelectController.Index;
			dockSelectController.Update();
			if (dockSelectController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			if (rep.now_mode() != 1)
			{
				dockSelectController.Index = index;
				return;
			}
			if (dockSelectController.keyState[8].down)
			{
			}
			if (dockSelectController.keyState[12].down)
			{
			}
			if (bd2.get_board2_anime() || dia2.get_dialog2_anime() || dia.get_dialog_anime())
			{
				return;
			}
			if (!_isBtnMaruUp && (dockSelectController.keyState[1].up || !dockSelectController.keyState[1].down))
			{
				_isBtnMaruUp = true;
				return;
			}
			if (rep.first_change())
			{
				UISelectedObject.SelectedObjectBlink(dock_cursor, dockSelectController.Index);
				btnLights(Play: false, force: true);
				return;
			}
			if (dockSelectController.IsChangeIndex)
			{
				UISelectedObject.SelectedObjectBlink(dock_cursor, dockSelectController.Index);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			for (int i = 0; i < dock_cursor.Length; i++)
			{
				if (_clsRepair.Material.RepairKit <= 0 || now_kit <= 0)
				{
					repair_btn[i].GetComponent<UISprite>().spriteName = "btn_quick_off";
					btnLights(Play: false);
				}
				else
				{
					btnLights(Play: true);
					if (dockSelectController.Index == i)
					{
						if (!get_HS_anime(i))
						{
							repair_btn[i].GetComponent<UISprite>().spriteName = "btn_quick_on";
						}
					}
					else if (!get_HS_anime(i))
					{
						repair_btn[i].GetComponent<UISprite>().spriteName = "btn_quick";
					}
				}
				if (dockSelectController.Index == i)
				{
					label1_obj[i].GetComponent<UILabel>().effectColor = TextShadowLight;
					label2_obj[i].GetComponent<UILabel>().effectColor = TextShadowLight;
					label3_obj[i].GetComponent<UILabel>().effectColor = TextShadowLight;
				}
				else
				{
					label1_obj[i].GetComponent<UILabel>().effectColor = Color.white;
					label2_obj[i].GetComponent<UILabel>().effectColor = Color.white;
					label3_obj[i].GetComponent<UILabel>().effectColor = Color.white;
				}
			}
			if (dockSelectController.IsChangeIndex)
			{
				UISelectedObject.SelectedObjectBlink(dock_cursor, dockSelectController.Index);
				for (int j = 0; j < dock_cursor.Length; j++)
				{
					if (_clsRepair.Material.RepairKit <= 0 || now_kit <= 0)
					{
						repair_btn[j].GetComponent<UISprite>().spriteName = "btn_quick_off";
						btnLights(Play: false);
					}
					else
					{
						btnLights(Play: true);
						if (dockSelectController.Index == j)
						{
							if (!get_HS_anime(j))
							{
								repair_btn[j].GetComponent<UISprite>().spriteName = "btn_quick_on";
							}
						}
						else if (!get_HS_anime(j))
						{
							repair_btn[j].GetComponent<UISprite>().spriteName = "btn_quick";
						}
					}
					if (dockSelectController.Index == j)
					{
						label1_obj[j].GetComponent<UILabel>().effectColor = new Color(0.63f, 0.91f, 1f);
						label2_obj[j].GetComponent<UILabel>().effectColor = new Color(0.63f, 0.91f, 1f);
					}
					else
					{
						label1_obj[j].GetComponent<UILabel>().effectColor = Color.white;
						label2_obj[j].GetComponent<UILabel>().effectColor = Color.white;
					}
				}
				for (int k = 0; k < _clsRepair.MapArea.NDockMax; k++)
				{
					if (_clsRepair.MapArea.NDockCount == dockSelectController.Index)
					{
						int num = k;
						GameObject.Find("board1_top/board/Grid/0" + num.ToString() + "/Shutter/BGKey").GetComponent<UISprite>().spriteName = "btn_addDock_on";
					}
					else
					{
						int num2 = k;
						GameObject.Find("board1_top/board/Grid/0" + num2.ToString() + "/Shutter/BGKey").GetComponent<UISprite>().spriteName = "btn_addDock";
					}
				}
			}
			if (dockSelectController.Index < _clsRepair.GetDocks().Length)
			{
				if (_clsRepair.GetDockData(dockSelectController.Index).ShipId != 0)
				{
					_go_kosoku = 1;
				}
				else
				{
					_go_kosoku = 0;
				}
			}
			if (dockSelectController.keyState[0].down)
			{
				back_to_port();
			}
			else if (dockSelectController.keyState[1].down && _isBtnMaruUp)
			{
				_isBtnMaruUp = false;
				now_dock = dockSelectController.Index;
				if (_clsRepair.Material.RepairKit <= 0)
				{
					now_kit = -1;
				}
				dock_selected(dockSelectController.Index);
			}
			else if (dockSelectController.keyState[5].down)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
		}

		public void dock_selected(int dockNo)
		{
			DockCursorBlink(dockNo);
			dockSelectController.Index = dockNo;
			for (int i = dock_count; i < _clsRepair.MapArea.NDockMax; i++)
			{
				if (_clsRepair.IsValidOpenNewDock())
				{
					int num = i;
					GameObject.Find("board1_top/board/Grid/0" + num.ToString() + "/Shutter/BGKey").GetComponent<UISprite>().spriteName = "btn_addDock";
				}
				else
				{
					int num2 = i;
					GameObject.Find("board1_top/board/Grid/0" + num2.ToString() + "/Shutter/BGKey").GetComponent<UISprite>().spriteName = "btn_addDock";
				}
			}
			if (dockNo < _clsRepair.GetDocks().Length)
			{
				if (_clsRepair.GetDockData(dockSelectController.Index).ShipId != 0)
				{
					_go_kosoku = 1;
				}
				else
				{
					_go_kosoku = 0;
				}
				if (_go_kosoku == 0)
				{
					if (_clsRepair.GetDockData(dockNo).ShipId != 0 || get_HS_anime(dockNo))
					{
						SoundUtils.PlaySE(SEFIleInfos.CommonWrong);
						return;
					}
					rep.set_mode(-2);
					GameObject.Find("dialog_top/dialog").GetComponent<dialog>().SetDock(dockNo);
					bd2 = GameObject.Find("board2").GetComponent<board2>();
					bd2.board2_appear(boardStart: true);
					bd2.set_touch_mode(value: true);
					rep.setmask(1, value: true);
					rep.set_mode(2);
				}
				else if (get_HS_anime(dockNo) || now_kit <= 0)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonWrong);
				}
				else if (_clsRepair.IsValidChangeRepairSpeed(dockNo))
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
					rep.set_mode(-5);
					dia2.UpdateInfo(dockNo);
					dia2.SetDock(dockNo);
					rep.setmask(3, value: true);
					dia2.dialog2_appear(bstat: true);
					rep.set_mode(5);
				}
				else
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonWrong);
				}
			}
			else
			{
				int num3 = dockNo;
				GameObject.Find("board1_top/board/Grid/0" + num3.ToString() + "/Shutter/BGKey").GetComponent<UISprite>().spriteName = "btn_addDock_on";
				if (_clsRepair.IsValidOpenNewDock())
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
					dia3.UpdateInfo(dockNo);
					dia3.dialog3_appear(bstat: true);
					rep.set_mode(6);
				}
				else
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NoDockKey));
				}
			}
		}

		public void OpenDock(int dockNo)
		{
			_clsRepair.OpenNewDock();
			GameObject.Find("info/text_open").GetComponent<Animation>().PlayQueued("go_open");
			int num = get_dock_touchable_count();
			if (num == _clsRepair.MapArea.NDockMax)
			{
				num--;
			}
			dockSelectController.setMaxIndex(num);
			if (_clsRepair.MapArea.NDockCount != _clsRepair.MapArea.NDockMax)
			{
				DockStatus(rep.NowArea(), dockNo + 1);
			}
			DockStatus(rep.NowArea(), dockNo);
			GameObject.Find("board1_top/board/Grid/0" + (_clsRepair.MapArea.NDockCount - 1).ToString() + "/Shutter/ShutterALL").transform.localScale = Vector3.zero;
			GameObject.Find("board1_top/board/Grid/0" + (_clsRepair.MapArea.NDockCount - 1).ToString() + "/Shutter").GetComponent<Animation>().Play();
			UISelectedObject.SelectedObjectBlink(dock_cursor, dockNo);
		}

		private void Pop_Repairs()
		{
			GameObject.Find("board/Grid/00/Anime").GetComponent<crane_anime>().HPgrowStop();
			GameObject.Find("Repair Root/info/text_open").GetComponent<Animation>().Stop();
			GameObject.Find("Repair Root/info/text_open/text_open").transform.localPositionX(784f);
			GameObject.Find("Repair Root/info/text_open/text_bg").transform.localPositionX(784f);
			bd2.Cancelled(NotChangeMode: true, isSirent: true);
			bd3.Cancelled(NotChangeMode: true, isSirent: true);
			for (int i = 0; i < 4; i++)
			{
				dock[i].gameObject.transform.localScale = Vector3.zero;
				shutter[i].transform.localScale = Vector3.one;
				int num = i;
				GameObject.Find("board1_top/board/Grid/0" + num.ToString() + "/Shutter/ShutterALL").transform.localScale = Vector3.one;
				int num2 = i;
				GameObject.Find("board1_top/board/Grid/0" + num2.ToString() + "/Shutter/ShutterL").transform.localPositionX(0f);
				int num3 = i;
				GameObject.Find("board1_top/board/Grid/0" + num3.ToString() + "/Shutter/ShutterR").transform.localPositionX(-1f);
				GameObject.Find("board/Grid/0" + i.ToString() + "/Anime_H").GetComponent<Animation>().Stop();
				GameObject.Find("board/Grid/0" + i.ToString() + "/Anime_H/crane1").gameObject.transform.localPositionX(520f);
				GameObject.Find("board/Grid/0" + i.ToString() + "/Anime").GetComponent<Animation>().Stop();
				GameObject.Find("board/Grid/0" + i.ToString() + "/Anime/crane1").gameObject.transform.localPositionX(520f);
				int num4 = i;
				bds2 = GameObject.Find("board1_top/board/Grid/0" + num4.ToString() + "/bg/BackGround3").GetComponent<UISprite>();
				bds2.spriteName = "cardArea_YB";
				if (get_HS_anime(i))
				{
					rep.now_clsRepair().ChangeRepairSpeed(i);
					set_anime(i, stat: false);
					set_HS_anime(i, stat: false);
				}
			}
			for (int j = 0; j < 3; j++)
			{
				GameObject.Find("board" + (j + 1).ToString() + "_top_mask/board" + (j + 1).ToString() + "_guard").GetComponent<UISprite>().color = new Color(1f, 1f, 1f, 0.007f);
			}
			GameObject.Find("Repair_BGS/trusses").transform.localScale = Vector3.one;
			GameObject.Find("Repair_BGS/BG Panel2/bg_cr03").transform.localScale = Vector3.one;
			GameObject.Find("Repair Root/board1_top/header").transform.localScale = Vector3.one;
			GameObject.Find("info/docknotfound").transform.localPositionX(-1500f);
			GameObject gameObject = GameObject.Find("ObjectPool/Repair_BGS/Root");
			GameObject.Find("Repair Root/board1_top").transform.parent = gameObject.transform;
			GameObject.Find("Repair Root/board1_top_mask").transform.parent = gameObject.transform;
			GameObject.Find("Repair Root/board2_top").transform.parent = gameObject.transform;
			GameObject.Find("Repair Root/board2_top_mask").transform.parent = gameObject.transform;
			GameObject.Find("Repair Root/board3_top").transform.parent = gameObject.transform;
			GameObject.Find("Repair Root/board3_top_mask").transform.parent = gameObject.transform;
			GameObject.Find("Repair Root/dialog_top").transform.parent = gameObject.transform;
			GameObject.Find("Repair Root/dialog2_top").transform.parent = gameObject.transform;
			GameObject.Find("Repair Root/debug").transform.parent = gameObject.transform;
			GameObject.Find("Repair Root/info").transform.parent = gameObject.transform;
			GameObject.Find("Repair Root/Guide").transform.parent = gameObject.transform;
			rep.delete_clsRepair();
			rep.delete_MstManager();
			rep.delete_Mst_ship();
		}

		private void back_to_port()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
		}

		private void Dock_not_found()
		{
			if (!_HOLT_DIALOG)
			{
				GameObject.Find("Repair_BGS/trusses").transform.localScale = Vector3.zero;
				GameObject.Find("Repair_BGS/BG Panel2/bg_cr03").transform.localScale = Vector3.zero;
				GameObject.Find("board1_top/header").transform.localScale = Vector3.zero;
				UIButton component = GameObject.Find("info/Button").GetComponent<UIButton>();
				UIButtonMessage component2 = component.GetComponent<UIButtonMessage>();
				component2.target = base.gameObject;
				component2.functionName = "Pressed_Button_Back";
				component2.trigger = UIButtonMessage.Trigger.OnClick;
				iTween.MoveTo(GameObject.Find("info/docknotfound"), iTween.Hash("islocal", true, "x", 0f, "time", 2f, "easetype", iTween.EaseType.easeOutElastic));
				_HOLT_DIALOG = true;
			}
		}

		private void Pressed_Button_Back(GameObject obj)
		{
			back_to_port();
		}

		private string AreaIdToSeaSpriteName(int areaId)
		{
			switch (areaId)
			{
			case 1:
			case 8:
			case 9:
			case 11:
			case 12:
				return "list_sea" + 1;
			case 3:
			case 13:
				return "list_sea" + 3;
			case 2:
			case 4:
			case 5:
			case 6:
			case 7:
			case 10:
			case 14:
				return "list_sea" + 2;
			case 15:
			case 16:
			case 17:
				return "list_sea" + 4;
			default:
				return "list_sea" + 5;
			}
		}

		private IEnumerator _wait(float time)
		{
			yield return new WaitForSeconds(time);
		}
	}
}
