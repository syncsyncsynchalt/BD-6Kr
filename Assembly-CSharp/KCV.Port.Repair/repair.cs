using Common.Enum;
using KCV.Utils;
using local.managers;
using local.models;
using Server_Models;
using System.Collections;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class repair : MonoBehaviour
	{
		private const BGMFileInfos SCENE_BGM = BGMFileInfos.PortTools;

		private UISprite sprite;

		private UISprite cursor;

		private UISprite board;

		private UISprite board2;

		private UITexture tex;

		private UILabel lab;

		private UIButton btn;

		private RepairManager _clsRepair;

		private RepairDockModel _clsRepairDockModel;

		private crane_anime _crane;

		private board2 bd2;

		private dialog dg;

		private board1_top_mask bd1m;

		private board2_top_mask bd2m;

		private board3_top_mask bd3m;

		private sw sw;

		private bool first_change_mode;

		private int _MapArea;

		private board bd1;

		private SoundManager sm;

		private UISprite go;

		private int _nTimem;

		private bool _isStartUP;

		private bool _isAsFinished;

		private KeyControl dockSelectController;

		private int _now_texgirl;

		private bool _FadeDone;

		private int nowmode;

		private Mst_DataManager _mstManager;

		private Mst_ship _shipdata;

		private void Awake()
		{
		}

		private void _all_init()
		{
			board component = ((Component)base.transform.FindChild("board1_top/board")).GetComponent<board>();
			board2 component2 = ((Component)base.transform.FindChild("board2_top/board2")).GetComponent<board2>();
			board3 component3 = ((Component)base.transform.FindChild("board3_top/board3")).GetComponent<board3>();
			board3_btn component4 = ((Component)base.transform.FindChild("board3_top/board3/Button")).GetComponent<board3_btn>();
			board1_top_mask component5 = ((Component)base.transform.FindChild("board1_top_mask")).GetComponent<board1_top_mask>();
			board2_top_mask component6 = ((Component)base.transform.FindChild("board2_top_mask")).GetComponent<board2_top_mask>();
			board3_top_mask component7 = ((Component)base.transform.FindChild("board3_top_mask")).GetComponent<board3_top_mask>();
			crane_anime component8 = ((Component)base.transform.FindChild("board1_top/board/Grid/01/Anime")).GetComponent<crane_anime>();
			dialog component9 = ((Component)base.transform.FindChild("dialog_top/dialog")).GetComponent<dialog>();
			dialog2 component10 = ((Component)base.transform.FindChild("dialog2_top/dialog2")).GetComponent<dialog2>();
			high_repair component11 = ((Component)base.transform.FindChild("board1_top/board/Grid/00/repair_now/btn_high_repair")).GetComponent<high_repair>();
			sentaku component12 = ((Component)base.transform.FindChild("board1_top/board/Grid/00/btn_Sentaku")).GetComponent<sentaku>();
			((Component)base.transform.FindChild("board3_top/board3/sw01")).GetComponent<sw>();
			component._init_repair();
			component2._init_repair();
			component3._init_repair();
			component5._init_repair();
			component6._init_repair();
			component7._init_repair();
			component4._init_repair();
			component8._init_repair();
			component9._init_repair();
			component10._init_repair();
			component11._init_repair();
			component12._init_repair();
			component.ResetMaruKey();
			GameObject.Find("Repair Root/debug").transform.localPositionY(-120f);
		}

		public Mst_DataManager GetMstManager()
		{
			return _mstManager;
		}

		public Mst_ship GetMst_ship()
		{
			return _shipdata;
		}

		public void delete_MstManager()
		{
			_mstManager = null;
		}

		public void delete_Mst_ship()
		{
			_shipdata = null;
		}

		private void Start()
		{
			_MapArea = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
			_clsRepair = new RepairManager(_MapArea);
			_mstManager = Mst_DataManager.Instance;
			_shipdata = new Mst_ship();
			_FadeDone = false;
			if (SingletonMonoBehaviour<PortObjectManager>.exist())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(delegate
				{
					SoundUtils.SwitchBGM(BGMFileInfos.PortTools);
					OnTransitionFinished();
				});
			}
			else
			{
				OnTransitionFinished();
			}
			_isStartUP = false;
			_now_texgirl = 0;
			bd1 = GameObject.Find("board1_top/board").GetComponent<board>();
			StartUp();
		}

		private void OnDestroy()
		{
			Mem.Del(ref sprite);
			Mem.Del(ref cursor);
			Mem.Del(ref board);
			Mem.Del(ref board2);
			Mem.Del(ref tex);
			Mem.Del(ref lab);
			Mem.Del(ref btn);
			Mem.Del(ref _clsRepair);
			Mem.Del(ref _clsRepairDockModel);
			Mem.Del(ref _crane);
			Mem.Del(ref bd2);
			Mem.Del(ref dg);
			Mem.Del(ref bd1m);
			Mem.Del(ref bd2m);
			Mem.Del(ref bd3m);
			Mem.Del(ref sw);
			Mem.Del(ref bd1);
			Mem.Del(ref sm);
			Mem.Del(ref go);
			Mem.Del(ref dockSelectController);
			Mem.Del(ref _mstManager);
			Mem.Del(ref _shipdata);
		}

		private void OnTransitionFinished()
		{
			bd1 = GameObject.Find("board1_top/board").GetComponent<board>();
			GameObject.Find("board1_top").GetComponent<UIPanel>().enabled = true;
			for (int i = 0; i < 4; i++)
			{
				int num = i;
				GameObject.Find("board1_top/board/Grid/0" + num.ToString()).GetComponent<UIPanel>().enabled = true;
				int num2 = i;
				GameObject.Find("board1_top/board/Grid/0" + num2.ToString() + "/btn_Sentaku").GetComponent<UIPanel>().enabled = true;
				int num3 = i;
				GameObject.Find("board1_top/board/Grid/0" + num3.ToString() + "/bg").GetComponent<UIPanel>().enabled = true;
				int num4 = i;
				GameObject.Find("board1_top/board/Grid/0" + num4.ToString() + "/repair_now").GetComponent<UIPanel>().enabled = true;
			}
			_all_init();
			Animation component = GameObject.Find("board1_top/board/Grid").GetComponent<Animation>();
			component.Play();
			_FadeDone = true;
		}

		private void StartUp()
		{
			if (!_isStartUP)
			{
				_isStartUP = true;
				bd1m = ((Component)base.transform.FindChild("board1_top_mask")).GetComponent<board1_top_mask>();
				bd2m = ((Component)base.transform.FindChild("board2_top_mask")).GetComponent<board2_top_mask>();
				bd3m = ((Component)base.transform.FindChild("board3_top_mask")).GetComponent<board3_top_mask>();
				bd2 = ((Component)base.transform.FindChild("board2_top/board2")).GetComponent<board2>();
				sm = SingletonMonoBehaviour<SoundManager>.Instance;
			}
		}

		public bool isFadeDone()
		{
			return _FadeDone;
		}

		public RepairManager now_clsRepair()
		{
			return _clsRepair;
		}

		public void delete_clsRepair()
		{
			_clsRepair = null;
		}

		public RepairDockModel now_clsRepairDockModel()
		{
			return _clsRepairDockModel;
		}

		public int NowArea()
		{
			return _MapArea;
		}

		public int now_mode()
		{
			return nowmode;
		}

		public void set_mode(int mode)
		{
			if (_FadeDone)
			{
				nowmode = mode;
				UILabel component = GameObject.Find("debug/_damegeButton/lbl_setmode").GetComponent<UILabel>();
				component.text = string.Empty + nowmode;
				if (mode == 1)
				{
					bd1.set_kira(value: true);
				}
				else
				{
					bd1.set_kira(value: false);
				}
				switch (mode)
				{
				case 2:
					bd1m.GetComponent<Collider2D>().enabled = true;
					bd1m.GetComponent<UIPanel>().alpha = 0.3f;
					bd2m.GetComponent<Collider2D>().enabled = false;
					bd2m.GetComponent<UIPanel>().alpha = 0f;
					bd3m.GetComponent<Collider2D>().enabled = false;
					bd3m.GetComponent<UIPanel>().alpha = 0f;
					break;
				case 3:
					bd1m.GetComponent<Collider2D>().enabled = true;
					bd1m.GetComponent<UIPanel>().alpha = 0.3f;
					bd2m.GetComponent<Collider2D>().enabled = true;
					bd2m.GetComponent<UIPanel>().alpha = 0.3f;
					bd3m.GetComponent<Collider2D>().enabled = false;
					bd3m.GetComponent<UIPanel>().alpha = 0f;
					break;
				case 4:
					bd1m.GetComponent<Collider2D>().enabled = true;
					bd1m.GetComponent<UIPanel>().alpha = 0.3f;
					bd2m.GetComponent<Collider2D>().enabled = true;
					bd2m.GetComponent<UIPanel>().alpha = 0.3f;
					bd3m.GetComponent<Collider2D>().enabled = true;
					bd3m.GetComponent<UIPanel>().alpha = 0.3f;
					break;
				default:
					bd1m.GetComponent<Collider2D>().enabled = false;
					bd1m.GetComponent<UIPanel>().alpha = 0f;
					bd2m.GetComponent<Collider2D>().enabled = false;
					bd2m.GetComponent<UIPanel>().alpha = 0f;
					bd3m.GetComponent<Collider2D>().enabled = false;
					bd3m.GetComponent<UIPanel>().alpha = 0f;
					break;
				}
				first_change_mode = true;
			}
		}

		public bool first_change()
		{
			if (first_change_mode)
			{
				first_change_mode = false;
				return true;
			}
			return false;
		}

		private void set_bg_color(int timezone)
		{
			switch (timezone)
			{
			case 1:
				GameObject.Find("BG Panel/bg_landscape").GetComponent<UITexture>().color = new Color(1f, 1f, 1f);
				break;
			case 2:
				GameObject.Find("BG Panel/bg_landscape").GetComponent<UITexture>().color = new Color(0.94f, 0.75f, 0.56f);
				break;
			case 3:
				GameObject.Find("BG Panel/bg_landscape").GetComponent<UITexture>().color = new Color(0f, 0.13f, 0.38f);
				break;
			case 4:
				GameObject.Find("BG Panel/bg_landscape").GetComponent<UITexture>().color = new Color(0.09f, 0.13f, 0.19f);
				break;
			case 5:
				GameObject.Find("BG Panel/bg_landscape").GetComponent<UITexture>().color = new Color(0.63f, 0.78f, 1f);
				break;
			default:
				GameObject.Find("BG Panel/bg_landscape").GetComponent<UITexture>().color = new Color(1f, 1f, 1f);
				break;
			}
		}

		public void setmask(int no, bool value)
		{
			GameObject gameObject = GameObject.Find("Repair Root/board" + no + "_top_mask");
			if (value)
			{
				gameObject.GetComponent<Collider2D>().enabled = true;
				gameObject.GetComponent<Animation>().Play("bd" + no + "m_on");
			}
			else
			{
				gameObject.GetComponent<Collider2D>().enabled = false;
				gameObject.GetComponent<Animation>().Play("bd" + no + "m_off");
			}
		}

		public void all_rid_mask()
		{
			Animation component = bd1m.GetComponent<Animation>();
			bd1m.GetComponent<Collider2D>().enabled = false;
			component.Play("bd1m_off");
			component = bd2m.GetComponent<Animation>();
			bd2m.GetComponent<Collider2D>().enabled = false;
			component.Play("bd2m_off");
			component = bd3m.GetComponent<Animation>();
			bd3m.GetComponent<Collider2D>().enabled = false;
			component.Play("bd3m_off");
		}

		private void InitDock()
		{
			set_mode(1);
			all_rid_mask();
			dockSelectController = new KeyControl(0, _clsRepair.MapArea.NDockCount - 1);
		}

		public int now_repairkit()
		{
			return _clsRepair.Material.RepairKit;
		}

		private IEnumerator WaitAndSpeak(ShipModel ship, int VoiceNo, float WaitSec)
		{
			yield return new WaitForSeconds(WaitSec);
			ShipUtils.PlayShipVoice(ship, VoiceNo);
		}

		public void nyukyogo(int dock, ShipModel ship, bool _isRepairKit)
		{
			Debug.Log("入渠します Dock:" + dock + " MemId:" + ship.MemId + " 高速:" + _isRepairKit + " 耐久度率：" + ship.TaikyuRate);
			if (_isRepairKit)
			{
				StartCoroutine(WaitAndSpeak(ship, 26, 1.5f));
			}
			else if (ship.TaikyuRate >= 50.0)
			{
				StartCoroutine(WaitAndSpeak(ship, 11, 1.5f));
			}
			else
			{
				StartCoroutine(WaitAndSpeak(ship, 12, 1.5f));
			}
			_clsRepair.StartRepair(dock, ship.MemId, _isRepairKit);
			GameObject gameObject = GameObject.Find("board1_top/board/Grid/0" + dock.ToString());
			if (_isRepairKit)
			{
				bd1.set_HS_anime(dock, stat: true);
				GameObject.Find("board1_top/board").GetComponent<board>().set_rnow_enable(dock, a: true);
				GameObject.Find("board1_top/board").GetComponent<board>().set_stk_enable(dock, a: false);
				dg = GameObject.Find("dialog").GetComponent<dialog>();
				int num = dock;
				tex = GameObject.Find("board/Grid/0" + num.ToString() + "/repair_now/ship_banner").GetComponent<UITexture>();
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
				int num2 = dock;
				lab = GameObject.Find("board/Grid/0" + num2.ToString() + "/repair_now/text_ship_name").GetComponent<UILabel>();
				lab.text = ship.Name;
				int num3 = dock;
				lab = GameObject.Find("board/Grid/0" + num3.ToString() + "/repair_now/text_level").GetComponent<UILabel>();
				lab.text = string.Empty + ship.Level;
				int num4 = dock;
				lab = GameObject.Find("board/Grid/0" + num4.ToString() + "/repair_now/text_hp").GetComponent<UILabel>();
				lab.text = dg.GetBeforeHp() + "/" + ship.MaxHp;
				bd1.set_dock_MaxHP(dock, ship.MaxHp);
				int num5 = dock;
				sprite = GameObject.Find("board/Grid/0" + num5.ToString() + "/repair_now/HP_Gauge/panel/HP_bar_meter").GetComponent<UISprite>();
				sprite.width = (int)((float)dg.GetBeforeHp() * 210f / (float)ship.MaxHp);
				sprite.color = Util.HpGaugeColor2(ship.MaxHp, dg.GetBeforeHp());
				int num6 = dock;
				sprite = GameObject.Find("board/Grid/0" + num6.ToString() + "/repair_now/HP_Gauge/panel/HP_bar_meter2").GetComponent<UISprite>();
				sprite.width = (int)((float)dg.GetBeforeHp() * 210f / (float)ship.MaxHp);
				sprite.color = Util.HpGaugeColor2(ship.MaxHp, dg.GetBeforeHp());
				int num7 = dock;
				lab = GameObject.Find("board/Grid/0" + num7.ToString() + "/repair_now/text_least_time").GetComponent<UILabel>();
				lab.text = string.Empty + ship.RepairTime;
				int num8 = dock;
				GameObject.Find("board/Grid/0" + num8.ToString() + "/repair_now/btn_high_repair").GetComponent<UIButton>().isEnabled = false;
				crane_anime component = GameObject.Find("board/Grid/0" + dock.ToString() + "/Anime").GetComponent<crane_anime>();
				component.high_repair_anime(dock, _low_anime: false);
			}
			else
			{
				iTween.MoveTo(gameObject.gameObject, iTween.Hash("islocal", true, "x", 1000f, "time", 0.1f));
			}
			bd2.UpdateList();
			update_portframe();
			SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(_clsRepair);
		}

		public void tochu_go(int dock, ShipModel shipid)
		{
			int num = dock;
			_crane = GameObject.Find("board/Grid/0" + num.ToString() + "/Anime").GetComponent<crane_anime>();
			_crane.high_repair_anime(dock);
			SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(_clsRepair);
			StartCoroutine(WaitAndSpeak(shipid, 26, 1.5f));
		}

		public void update_portframe()
		{
			SingletonMonoBehaviour<UIPortFrame>.Instance.CircleUpdateInfo(_clsRepair);
		}
	}
}
