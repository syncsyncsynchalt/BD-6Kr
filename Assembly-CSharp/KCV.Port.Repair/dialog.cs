using Common.Enum;
using Common.Struct;
using KCV.Utils;
using local.managers;
using local.models;
using System.Collections;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class dialog : MonoBehaviour
	{
		private UILabel ele_l;

		private UILabel ele_l2;

		private UIPanel panel;

		private repair rep;

		private UISprite ele_d;

		private int number;

		private bool _sw;

		private board3_top_mask bd3m;

		private RepairManager _clsRepair;

		private UITexture ele_t;

		private CommonShipBanner csb;

		private board3 bd3;

		private KeyControl dockSelectController;

		private int cx;

		private UIButton _Button_Yes;

		private UIButton _Button_No;

		private ShipModel ship;

		private bool _isSmoked;

		private bool _isKira;

		private GameObject csb_smokes;

		private GameObject csb_kira;

		private int selected_dock;

		private bool hsp;

		private int before_hp;

		private GameObject[] btn_obj = new GameObject[2];

		private UIButton _uiOverlayButton;

		private bool _dialog_anime;

		private Animation _ani;

		private sw sw;

		private repair_clickmask _clickmask;

		private Vector3 _bVector = Vector3.one;

		public void SetShip(ShipModel shi)
		{
			ship = shi;
			sw = GameObject.Find("sw01").GetComponent<sw>();
			sw.setSW(stat: false);
		}

		public void SetDock(int dock)
		{
			selected_dock = dock;
		}

		public void SetSpeed(bool use)
		{
			hsp = use;
		}

		private void Start()
		{
			_dialog_anime = false;
			_init_repair();
			set_dialog_anime(value: false);
			_clickmask = GameObject.Find("click_mask").GetComponent<repair_clickmask>();
		}

		private void OnDestroy()
		{
			Mem.Del(ref ele_l);
			Mem.Del(ref ele_l2);
			Mem.Del(ref panel);
			Mem.Del(ref rep);
			Mem.Del(ref ele_d);
			Mem.Del(ref bd3m);
			Mem.Del(ref _clsRepair);
			Mem.Del(ref ele_t);
			Mem.Del(ref csb);
			Mem.Del(ref bd3);
			Mem.Del(ref dockSelectController);
			Mem.Del(ref _Button_Yes);
			Mem.Del(ref _Button_No);
			Mem.Del(ref ship);
			Mem.Del(ref csb_smokes);
			Mem.Del(ref csb_kira);
			Mem.Del(ref btn_obj);
			Mem.Del(ref _uiOverlayButton);
			Mem.Del(ref _ani);
			Mem.Del(ref sw);
			Mem.Del(ref _clickmask);
			Mem.Del(ref _bVector);
		}

		public bool get_dialog_anime()
		{
			return _dialog_anime;
		}

		public void set_dialog_anime(bool value)
		{
			_dialog_anime = value;
		}

		public void CompleteHandler()
		{
			base.gameObject.transform.localScale = Vector3.one;
			set_dialog_anime(value: false);
		}

		public void CompleteHandler_onClose()
		{
			Set_Button_Sprite(value: true);
			((Component)base.gameObject.transform.parent).GetComponent<UIPanel>().alpha = 1f;
			set_dialog_anime(value: false);
			Set_Button_Sprite(value: true);
			dockSelectController.Index = 0;
		}

		public void Set_Button_Sprite(bool value)
		{
			if (value)
			{
				UIButton component = GameObject.Find("dialog_top/dialog/btn_yes").GetComponent<UIButton>();
				component.normalSprite = "btn_yes";
				((Component)component.transform.FindChild("Background")).GetComponent<UISprite>().spriteName = "btn_yes";
				component = GameObject.Find("dialog_top/dialog/btn_no").GetComponent<UIButton>();
				component.normalSprite = "btn_no_on";
				((Component)component.transform.FindChild("Background")).GetComponent<UISprite>().spriteName = "btn_no_on";
			}
			else
			{
				UIButton component2 = GameObject.Find("dialog_top/dialog/btn_yes").GetComponent<UIButton>();
				component2.normalSprite = "btn_yes_on";
				((Component)component2.transform.FindChild("Background")).GetComponent<UISprite>().spriteName = "btn_yes_on";
				component2 = GameObject.Find("dialog_top/dialog/btn_no").GetComponent<UIButton>();
				component2.normalSprite = "btn_no";
				((Component)component2.transform.FindChild("Background")).GetComponent<UISprite>().spriteName = "btn_no";
			}
		}

		public void _init_repair()
		{
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			csb = GameObject.Find("dialog_top/dialog/Banner/CommonShipBanner2").GetComponent<CommonShipBanner>();
			bd3 = GameObject.Find("board3_top/board3").GetComponent<board3>();
			_Button_Yes = GameObject.Find("dialog_top/dialog/btn_yes").GetComponent<UIButton>();
			_Button_No = GameObject.Find("dialog_top/dialog/btn_no").GetComponent<UIButton>();
			btn_obj[0] = _Button_No.gameObject;
			btn_obj[1] = _Button_Yes.gameObject;
			_uiOverlayButton = GameObject.Find("dialog_top/dialog/OverlayBtn").GetComponent<UIButton>();
			EventDelegate.Add(_uiOverlayButton.onClick, _onClickOverlayButton);
			UIButtonMessage component = _Button_Yes.GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = "Pressed_Button_Yes";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component2 = _Button_No.GetComponent<UIButtonMessage>();
			component2.target = base.gameObject;
			component2.functionName = "Pressed_Button_No";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			_clsRepair = rep.now_clsRepair();
			dockSelectController = new KeyControl(0, 1);
			dockSelectController.isLoopIndex = false;
			dockSelectController.setChangeValue(0f, 1f, 0f, -1f);
			sw = GameObject.Find("sw01").GetComponent<sw>();
			sw.setSW(stat: false);
			_bVector = Vector3.one;
		}

		private void Update()
		{
			if (rep.now_mode() != 4)
			{
				return;
			}
			dockSelectController.Update();
			if (rep.first_change())
			{
				UISelectedObject.SelectedButtonsZoomUpDown(btn_obj, dockSelectController.Index);
				return;
			}
			if (dockSelectController.Index == 1)
			{
				Set_Button_Sprite(value: true);
			}
			else
			{
				Set_Button_Sprite(value: false);
			}
			if (dockSelectController.IsChangeIndex)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				UISelectedObject.SelectedButtonsZoomUpDown(btn_obj, dockSelectController.Index);
			}
			if (base.gameObject.transform.localScale != Vector3.one)
			{
				if (base.gameObject.transform.localScale == _bVector)
				{
					base.gameObject.transform.localScale = Vector3.one;
					set_dialog_anime(value: false);
				}
				_bVector = base.gameObject.transform.localScale;
			}
			if (get_dialog_anime() || bd3.get_board3_anime())
			{
				return;
			}
			if (dockSelectController.keyState[0].down)
			{
				dockSelectController.Index = 0;
				Pressed_Button_No(null);
			}
			else if (dockSelectController.keyState[1].down)
			{
				if (dockSelectController.Index == 1)
				{
					Pressed_Button_No(null);
					return;
				}
				dockSelectController.Index = 0;
				rep.set_mode(1);
				Pressed_Button_Yes(null);
			}
		}

		private void _onClickOverlayButton()
		{
			if (!get_dialog_anime() && !bd3.get_board3_anime())
			{
				dockSelectController.Index = 0;
				Pressed_Button_No(null);
			}
		}

		public void UpdateInfo(ShipModel value_ship)
		{
			ship = value_ship;
			csb.SetShipData(value_ship);
			Vector3 localPosition = base.gameObject.transform.localPosition;
			if (!(localPosition.y < 100f))
			{
				ele_l = GameObject.Find("dialog/label_shipname").GetComponent<UILabel>();
				ele_l.text = ship.Name;
				ele_l = GameObject.Find("dialog/label_lv").GetComponent<UILabel>();
				ele_l.text = ship.Level + string.Empty;
				if (ship.DamageStatus != 0)
				{
					_isSmoked = true;
				}
				else
				{
					_isSmoked = false;
					csb_smokes = null;
				}
				if (ship.ConditionState == FatigueState.Exaltation)
				{
					_isKira = true;
				}
				else
				{
					_isKira = false;
				}
				MaterialInfo resourcesForRepair = ship.GetResourcesForRepair();
				ele_l = GameObject.Find("dialog/label_param").GetComponent<UILabel>();
				ele_l.text = resourcesForRepair.Steel + "\n" + resourcesForRepair.Fuel + "\n";
				ele_l.text += ((!_sw) ? (ship.RepairTime + string.Empty) : "0");
				ele_l.text += "\u3000\n使用";
				if (_sw)
				{
					ele_l.text += "する";
					before_hp = ship.NowHp;
				}
				else
				{
					ele_l.text += "しない";
				}
			}
		}

		public int GetBeforeHp()
		{
			return before_hp;
		}

		public void UpdateSW(bool sw)
		{
			_sw = sw;
			UpdateInfo(ship);
		}

		private void OnClick()
		{
		}

		public void dialog_appear(bool bstat)
		{
			rep.set_mode(-4);
			_ani = ((Component)base.gameObject.transform.parent).GetComponent<Animation>();
			if (bstat)
			{
				_clickmask.unclickable_sec(0.45f);
				((Component)base.gameObject.transform.parent).GetComponent<UIPanel>().enabled = true;
				set_dialog_anime(value: true);
				csb.StopParticle();
				base.gameObject.transform.localPosition = new Vector3(0f, -27f, 0f);
				base.gameObject.transform.localScale = Vector3.one * 0.6f;
				TweenScale tweenScale = TweenScale.Begin(base.gameObject, 0.4f, Vector3.one);
				tweenScale.animationCurve = UtilCurves.TweenEaseOutBack;
				tweenScale.SetOnFinished(CompleteHandler);
				rep.setmask(3, value: true);
				rep.set_mode(4);
			}
			else
			{
				set_dialog_anime(value: true);
				csb.StartParticle();
				base.gameObject.transform.localScale = Vector3.one;
				iTween.MoveTo(base.gameObject, iTween.Hash("islocal", true, "x", 0f, "y", 800, "z", -2, "delay", 0.3f, "time", 0f));
				iTween.ScaleTo(base.gameObject, iTween.Hash("islocal", true, "x", 0.6f, "y", 0.6f, "z", 0.6f, "delay", 0.3f, "time", 0f, "easetype", iTween.EaseType.easeOutBack, "oncomplete", "CompleteHandler_onClose", "oncompletetarget", base.gameObject));
				_ani.Play("dialog_off");
				rep.setmask(3, value: false);
				rep.set_mode(3);
			}
		}

		public void Pressed_Button_Yes(GameObject obj)
		{
			rep.set_mode(-1);
			_clickmask.unclickable_onesec();
			SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter2);
			dialog_appear(bstat: false);
			GameObject.Find("board3").GetComponent<board3>().board3_appear(bstat: false, isSirent: true);
			GameObject.Find("board2").GetComponent<board2>().board2_appear(boardStart: false, isSirent: true);
			rep.all_rid_mask();
			if (!hsp)
			{
				rep.nyukyogo(selected_dock, ship, _isRepairKit: false);
				GameObject.Find("board1_top/board").GetComponent<board>().redraw(anime: false, selected_dock);
			}
			else
			{
				rep.nyukyogo(selected_dock, ship, _isRepairKit: true);
			}
			GameObject.Find("board2").GetComponent<board2>().UpdateList();
			GameObject.Find("board1_top/board").GetComponent<board>().set_cx(0);
			rep.set_mode(1);
		}

		public void Pressed_Button_No(GameObject obj)
		{
			Debug.Log("Pressed_Button_No");
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			_clickmask.unclickable_sec(0.3f);
			rep.set_mode(-1);
			rep.setmask(3, value: false);
			dialog_appear(bstat: false);
			rep.set_mode(3);
		}

		private IEnumerator _wait(float time)
		{
			yield return new WaitForSeconds(time);
		}
	}
}
