using Common.Struct;
using KCV.PopupString;
using KCV.Utils;
using local.managers;
using local.models;
using System.Collections;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class board3 : MonoBehaviour
	{
		private UISprite ele_d;

		private UILabel ele_l;

		private UILabel ele_l2;

		private UITexture ele_t;

		private UITexture ele_t2;

		private UIScrollBar sb;

		private repair rep;

		private dialog dia;

		private sw sw_h;

		private board2_top_mask bd2m;

		private RepairManager _clsRepair;

		private UIButton uibutton;

		private dialog dig;

		private KeyControl dockSelectController;

		private board2 bd2;

		private board3 bd3;

		private sw sw;

		private int cy;

		private int mcy;

		private bool nyukyo;

		private GameObject cursor;

		private board3_btn bd3b;

		private CommonShipBanner csb;

		private bool bd3firstUpdate;

		private ShipModel _ShipModel;

		private int _sw;

		[SerializeField]
		private UILabel _uiShipName;

		[SerializeField]
		private ButtonLightTexture _btnLight;

		private GameObject[] btn_obj = new GameObject[2];

		private bool _board3_anime;

		public void CompleteHandler()
		{
			set_board3_anime(value: false);
		}

		public void CompleteHandlerStaticOn()
		{
			base.gameObject.isStatic = true;
			base.gameObject.GetComponent<UIPanel>().enabled = false;
			set_board3_anime(value: false);
			sw.setSW(stat: false);
			cy = 1;
			dockSelectController.Index = 1;
		}

		public bool get_board3_anime()
		{
			return _board3_anime;
		}

		public void set_board3_anime(bool value)
		{
			_board3_anime = value;
		}

		public void Set_Button_Sprite(bool value)
		{
			if (nyukyo)
			{
				_btnLight.PlayAnim();
				if (value)
				{
					GameObject.Find("sw01/switch_cursor").GetComponent<UISprite>().color = Color.white;
					GameObject.Find("Repair Root/board3_top/board3/Button").GetComponent<UIButton>().normalSprite = "btn_start_off";
					GameObject.Find("Repair Root/board3_top/board3/Button/Background").GetComponent<UISprite>().spriteName = "btn_start_off";
				}
				else
				{
					GameObject.Find("sw01/switch_cursor").GetComponent<UISprite>().color = new Color(1f, 1f, 1f, 0.001f);
					GameObject.Find("Repair Root/board3_top/board3/Button").GetComponent<UIButton>().normalSprite = "btn_start_on";
					GameObject.Find("Repair Root/board3_top/board3/Button/Background").GetComponent<UISprite>().spriteName = "btn_start_on";
				}
			}
			else
			{
				_btnLight.StopAnim();
				GameObject.Find("sw01/switch_cursor").GetComponent<UISprite>().color = new Color(1f, 1f, 1f, 0.001f);
				GameObject.Find("Repair Root/board3_top/board3/Button").GetComponent<UIButton>().normalSprite = "btn_start_disable";
				GameObject.Find("Repair Root/board3_top/board3/Button/Background").GetComponent<UISprite>().spriteName = "btn_start_disable";
				UISelectedObject.SelectedOneButtonZoomUpDown(btn_obj[0], value: false);
				UISelectedObject.SelectedOneButtonZoomUpDown(btn_obj[1], value: false);
			}
		}

		private void Start()
		{
			_init_repair();
			set_board3_anime(value: false);
		}

		private void OnDestroy()
		{
			Mem.Del(ref ele_d);
			Mem.Del(ref ele_l);
			Mem.Del(ref ele_l2);
			Mem.Del(ref ele_t);
			Mem.Del(ref ele_t2);
			Mem.Del(ref sb);
			Mem.Del(ref rep);
			Mem.Del(ref dia);
			Mem.Del(ref sw_h);
			Mem.Del(ref bd2m);
			Mem.Del(ref _clsRepair);
			Mem.Del(ref uibutton);
			Mem.Del(ref dig);
			Mem.Del(ref dockSelectController);
			Mem.Del(ref bd2);
			Mem.Del(ref bd3);
			Mem.Del(ref sw);
			Mem.Del(ref cursor);
			Mem.Del(ref bd3b);
			Mem.Del(ref csb);
			Mem.Del(ref _ShipModel);
			Mem.Del(ref _uiShipName);
			Mem.Del(ref _btnLight);
			Mem.Del(ref btn_obj);
		}

		public void _init_repair()
		{
			rep = ((Component)base.gameObject.transform.parent.parent).GetComponent<repair>();
			bd3b = ((Component)rep.gameObject.transform.FindChild("board3_top/board3/Button")).GetComponent<board3_btn>();
			bd3 = ((Component)rep.gameObject.transform.FindChild("board3_top/board3")).GetComponent<board3>();
			bd2 = GameObject.Find("board2").GetComponent<board2>();
			btn_obj[0] = GameObject.Find("switch_ball");
			btn_obj[1] = bd3b.gameObject;
			sw = GameObject.Find("sw01").GetComponent<sw>();
			dockSelectController = new KeyControl(0, 1);
			dockSelectController.setChangeValue(-1f, 0f, 1f, 0f);
			nyukyo = false;
			bd3firstUpdate = true;
			cy = 1;
			_clsRepair = rep.now_clsRepair();
		}

		public void StartUp()
		{
			rep = ((Component)base.gameObject.transform.parent.parent).GetComponent<repair>();
			bd3b = ((Component)rep.gameObject.transform.FindChild("board3_top/board3/Button")).GetComponent<board3_btn>();
			bd3 = ((Component)rep.gameObject.transform.FindChild("board3_top/board3")).GetComponent<board3>();
			dia = GameObject.Find("dialog_top/dialog").GetComponent<dialog>();
			btn_obj[0] = GameObject.Find("switch_ball");
			btn_obj[1] = bd3b.gameObject;
			dockSelectController = new KeyControl(0, 1);
			dockSelectController.setChangeValue(-1f, 0f, 1f, 0f);
			nyukyo = false;
			_clsRepair = rep.now_clsRepair();
		}

		public void UpdateInfo(ShipModel shipz)
		{
			_ShipModel = shipz;
			bool material = false;
			mcy = 0;
			dig = GameObject.Find("dialog_top/dialog/").GetComponent<dialog>();
			sw = GameObject.Find("board3/sw01").GetComponent<sw>();
			uibutton = GameObject.Find("Repair Root/board3_top/board3/Button").GetComponent<UIButton>();
			bool flag;
			if (flag = rep.now_clsRepair().IsValidStartRepair(_ShipModel.MemId))
			{
				uibutton.isEnabled = true;
				nyukyo = true;
				sw.set_sw_stat(val: true);
			}
			else
			{
				uibutton.isEnabled = false;
				nyukyo = false;
				sw.setSW(stat: false);
				sw.set_sw_stat(val: false);
			}
			ele_l = GameObject.Find("board3/param/shipname").GetComponent<UILabel>();
			ele_l.text = _ShipModel.Name;
			csb = GameObject.Find("board3/Banner/CommonShipBanner2").GetComponent<CommonShipBanner>();
			csb.SetShipData(_ShipModel);
			ele_l = GameObject.Find("board3/param/Label_hp").GetComponent<UILabel>();
			ele_l.text = _ShipModel.NowHp + "/" + _ShipModel.MaxHp;
			ele_d = GameObject.Find("board3/param/HP_bar_grn").GetComponent<UISprite>();
			ele_d.width = (int)((float)_ShipModel.NowHp * 210f / (float)_ShipModel.MaxHp);
			ele_d.color = Util.HpGaugeColor2(_ShipModel.MaxHp, _ShipModel.NowHp);
			GameObject.Find("board3/param/icon_stars").GetComponent<UISprite>().SetDimensions((_ShipModel.Srate + 1) * 25 - 2, 20);
			ele_l = GameObject.Find("board3/param/Label_lv").GetComponent<UILabel>();
			ele_l.text = string.Empty + _ShipModel.Level;
			ele_l = GameObject.Find("board3/param/Label_param").GetComponent<UILabel>();
			sw = GameObject.Find("board3/sw01").GetComponent<sw>();
			string empty = string.Empty;
			int steel = _clsRepair.Material.Steel;
			MaterialInfo resourcesForRepair = _ShipModel.GetResourcesForRepair();
			if (steel < resourcesForRepair.Steel)
			{
				empty += "[e32c2c]";
				material = true;
			}
			else
			{
				empty += "[404040]";
			}
			string arg = empty;
			MaterialInfo resourcesForRepair2 = _ShipModel.GetResourcesForRepair();
			empty = arg + resourcesForRepair2.Steel + "[-]\n";
			int fuel = _clsRepair.Material.Fuel;
			MaterialInfo resourcesForRepair3 = _ShipModel.GetResourcesForRepair();
			if (fuel < resourcesForRepair3.Fuel)
			{
				empty += "[e32c2c]";
				material = true;
			}
			else
			{
				empty += "[404040]";
			}
			if (flag)
			{
				dia.UpdateInfo(_ShipModel);
				dia.SetShip(_ShipModel);
			}
			else
			{
				StartCoroutine(ReasonMessage(_ShipModel, material));
			}
			dia = GameObject.Find("dialog_top/dialog").GetComponent<dialog>();
			string arg2 = empty;
			MaterialInfo resourcesForRepair4 = _ShipModel.GetResourcesForRepair();
			empty = arg2 + resourcesForRepair4.Fuel + "[-]\n";
			string text = empty;
			empty = text + "[404040]" + _ShipModel.RepairTime + "[-]  ";
			ele_l.text = empty;
		}

		private IEnumerator ReasonMessage(ShipModel _ShipModel, bool material)
		{
			yield return new WaitForSeconds(0.75f);
			if (_ShipModel.IsInRepair())
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.AlreadyRepair));
			}
			else if (_ShipModel.IsBling())
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NowBlinging));
			}
			else if (_ShipModel.IsInEscortDeck() != -1)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.InEscortShip));
			}
			else if (_ShipModel.IsInMission())
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.InMissionShip));
			}
			else if (material)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NotEnoughMaterial));
			}
			else if (_ShipModel.MaxHp == _ShipModel.NowHp)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NoDamage));
			}
			else if (_ShipModel.getDeck() == null && rep.NowArea() != 1)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NowBlinging));
			}
		}

		private void Update()
		{
			if (rep.now_mode() != 3 || dia.get_dialog_anime() || bd2.get_board2_anime())
			{
				return;
			}
			dockSelectController.Update();
			if (rep.first_change())
			{
				UISelectedObject.SelectedButtonsZoomUpDown(btn_obj, 1);
				return;
			}
			if (mcy != cy || bd3firstUpdate)
			{
				if (mcy != cy && !bd3firstUpdate)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				bd3firstUpdate = false;
				if (nyukyo)
				{
					if (cy == 0)
					{
						Set_Button_Sprite(value: true);
					}
					else
					{
						Set_Button_Sprite(value: false);
					}
					UISelectedObject.SelectedButtonsZoomUpDown(btn_obj, cy);
				}
				else
				{
					Set_Button_Sprite(value: true);
				}
			}
			mcy = cy;
			if (dockSelectController.keyState[0].down)
			{
				bd3firstUpdate = true;
				dockSelectController.ClearKeyAll();
				dockSelectController.firstUpdate = true;
				Cancelled();
			}
			else if (dockSelectController.keyState[2].down)
			{
				sw.OnClick();
			}
			else if (dockSelectController.IsLeftDown())
			{
				sw.setSW(stat: false);
			}
			else if (dockSelectController.IsRightDown())
			{
				sw.setSW(stat: true);
			}
			else if (dockSelectController.keyState[1].down)
			{
				bd3firstUpdate = true;
				if (cy != 0)
				{
					uibutton = GameObject.Find("Repair Root/board3_top/board3/Button").GetComponent<UIButton>();
					if (!uibutton.isEnabled)
					{
						SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
					}
					if (nyukyo)
					{
						bd3b.OnClick();
					}
				}
				else if (nyukyo)
				{
					sw.OnClick();
				}
				else
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonWrong);
				}
			}
			if (nyukyo)
			{
				if (dockSelectController.keyState[8].down)
				{
					cy = 0;
				}
				else if (dockSelectController.keyState[12].down)
				{
					cy = 1;
				}
			}
		}

		public void Cancelled()
		{
			Cancelled(NotChangeMode: false);
		}

		public void Cancelled(bool NotChangeMode)
		{
			Cancelled(NotChangeMode, isSirent: false);
		}

		public void Cancelled(bool NotChangeMode, bool isSirent)
		{
			if (!NotChangeMode)
			{
				rep.set_mode(2);
			}
			_board3_anime = true;
			board3_appear(bstat: false, isSirent);
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			rep.setmask(2, value: false);
			bd2.Resume();
		}

		private void OnClick()
		{
		}

		public void board3_appear(bool bstat)
		{
			board3_appear(bstat, isSirent: false);
		}

		public void board3_appear(bool bstat, bool isSirent)
		{
			if (bstat)
			{
				base.gameObject.isStatic = false;
				base.gameObject.GetComponent<UIPanel>().enabled = true;
				bd3firstUpdate = true;
				mcy = 1;
				cy = 1;
				set_board3_anime(value: true);
				TweenPosition tweenPosition = TweenPosition.Begin(base.gameObject, 0.35f, new Vector3(261f, -51f, -2f));
				tweenPosition.animationCurve = UtilCurves.TweenEaseOutExpo;
				tweenPosition.SetOnFinished(CompleteHandler);
				rep.setmask(2, value: true);
				rep.set_mode(3);
			}
			else
			{
				set_board3_anime(value: true);
				TweenPosition tweenPosition2 = TweenPosition.Begin(base.gameObject, 0.3f, new Vector3(800f, -51f, -2f));
				tweenPosition2.animationCurve = UtilCurves.TweenEaseOutExpo;
				tweenPosition2.SetOnFinished(CompleteHandlerStaticOn);
				rep.setmask(2, value: false);
				rep.set_mode(2);
			}
			if (!isSirent)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}
	}
}
