using KCV.Utils;
using local.managers;
using local.utils;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class dialog3 : MonoBehaviour
	{
		private UILabel ele_l;

		private UITexture ele_t;

		private UIPanel panel;

		private repair rep;

		private UISprite ele_d;

		private board3_top_mask bd3m;

		private board bd1;

		private board2 bd2;

		private RepairManager _clsRepair;

		private KeyControl dockSelectController;

		private board2 b2;

		private board3 b3;

		private UIButton _Button_Yes3;

		private UIButton _Button_No3;

		private int selected_dock;

		private bool _isBtnMaruUp;

		private GameObject[] btn_obj = new GameObject[2];

		private UIButton _uiOverlayButton3;

		private Animation _ani;

		private repair_clickmask _clickmask;

		private Vector3 _bVector = Vector3.one;

		private bool _dialog3_anime;

		public bool get_dialog3_anime()
		{
			return _dialog3_anime;
		}

		public void set_dialog3_anime(bool value)
		{
			_dialog3_anime = value;
		}

		public void CompleteHandler()
		{
			base.gameObject.transform.localScale = Vector3.one;
			set_dialog3_anime(value: false);
		}

		public void CompleteHandler_onClose()
		{
			set_dialog3_anime(value: false);
			((Component)base.gameObject.transform.parent).GetComponent<UIPanel>().alpha = 1f;
			Set_Button_Sprite(value: true);
			dockSelectController.Index = 0;
		}

		public void Set_Button_Sprite(bool value)
		{
			if (value)
			{
				UIButton component = GameObject.Find("dialog3_top/dialog3/btn_yes").GetComponent<UIButton>();
				component.normalSprite = "btn_yes";
				((Component)component.transform.FindChild("Background")).GetComponent<UISprite>().spriteName = "btn_yes";
				component = GameObject.Find("dialog3_top/dialog3/btn_no").GetComponent<UIButton>();
				component.normalSprite = "btn_no_on";
				((Component)component.transform.FindChild("Background")).GetComponent<UISprite>().spriteName = "btn_no_on";
			}
			else
			{
				UIButton component2 = GameObject.Find("dialog3_top/dialog3/btn_yes").GetComponent<UIButton>();
				component2.normalSprite = "btn_yes_on";
				((Component)component2.transform.FindChild("Background")).GetComponent<UISprite>().spriteName = "btn_yes_on";
				component2 = GameObject.Find("dialog3_top/dialog3/btn_no").GetComponent<UIButton>();
				component2.normalSprite = "btn_no";
				((Component)component2.transform.FindChild("Background")).GetComponent<UISprite>().spriteName = "btn_no";
			}
		}

		private void Start()
		{
			_init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del(ref ele_l);
			Mem.Del(ref ele_t);
			Mem.Del(ref panel);
			Mem.Del(ref rep);
			Mem.Del(ref ele_d);
			Mem.Del(ref bd3m);
			Mem.Del(ref bd1);
			Mem.Del(ref bd2);
			Mem.Del(ref _clsRepair);
			Mem.Del(ref dockSelectController);
			Mem.Del(ref b2);
			Mem.Del(ref b3);
			Mem.Del(ref _Button_Yes3);
			Mem.Del(ref _Button_No3);
			Mem.Del(ref btn_obj);
			Mem.Del(ref _uiOverlayButton3);
			Mem.Del(ref _ani);
			Mem.Del(ref _clickmask);
			Mem.Del(ref _bVector);
		}

		public void _init_repair()
		{
			_isBtnMaruUp = false;
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			bd1 = GameObject.Find("board").GetComponent<board>();
			bd2 = GameObject.Find("board2").GetComponent<board2>();
			dockSelectController = new KeyControl(0, 1);
			dockSelectController.isLoopIndex = false;
			dockSelectController.setChangeValue(0f, 1f, 0f, -1f);
			iTween.ScaleTo(base.gameObject, new Vector3(0.6f, 0.6f, 0.6f), 0f);
			_Button_Yes3 = GameObject.Find("dialog3_top/dialog3/btn_yes").GetComponent<UIButton>();
			_Button_No3 = GameObject.Find("dialog3_top/dialog3/btn_no").GetComponent<UIButton>();
			btn_obj[0] = _Button_No3.gameObject;
			btn_obj[1] = _Button_Yes3.gameObject;
			_uiOverlayButton3 = GameObject.Find("dialog3_top/dialog3/OverlayBtn3").GetComponent<UIButton>();
			EventDelegate.Add(_uiOverlayButton3.onClick, _onClickOverlayButton3);
			UIButtonMessage component = _Button_Yes3.GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = "Pressed_Button_Yes3";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component2 = _Button_No3.GetComponent<UIButtonMessage>();
			component2.target = base.gameObject;
			component2.functionName = "Pressed_Button_No3";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			Set_Button_Sprite(value: true);
			_clickmask = GameObject.Find("click_mask").GetComponent<repair_clickmask>();
			_bVector = Vector3.one;
		}

		private void Update()
		{
			if (rep.now_mode() != 6)
			{
				return;
			}
			dockSelectController.Update();
			if (dockSelectController.keyState[1].up || !dockSelectController.keyState[1].down)
			{
				_isBtnMaruUp = true;
			}
			if (rep.first_change())
			{
				UISelectedObject.SelectedButtonsZoomUpDown(btn_obj, dockSelectController.Index);
				return;
			}
			if (dockSelectController.IsChangeIndex)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				UISelectedObject.SelectedButtonsZoomUpDown(btn_obj, dockSelectController.Index);
			}
			if (dockSelectController.Index == 1)
			{
				Set_Button_Sprite(value: true);
			}
			else
			{
				Set_Button_Sprite(value: false);
			}
			if (base.gameObject.transform.localScale != Vector3.one)
			{
				if (base.gameObject.transform.localScale == _bVector)
				{
					base.gameObject.transform.localScale = Vector3.one;
					set_dialog3_anime(value: false);
				}
				_bVector = base.gameObject.transform.localScale;
			}
			if (get_dialog3_anime())
			{
				return;
			}
			if (dockSelectController.keyState[0].up)
			{
				dockSelectController.Index = 0;
				UISelectedObject.SelectedButtonsZoomUpDown(btn_obj, dockSelectController.Index);
				Pressed_Button_No3(null);
			}
			else if (dockSelectController.keyState[1].down && _isBtnMaruUp)
			{
				_isBtnMaruUp = false;
				if (dockSelectController.Index == 1)
				{
					Pressed_Button_No3(null);
				}
				else
				{
					Pressed_Button_Yes3(null);
				}
			}
		}

		private void _onClickOverlayButton3()
		{
			dockSelectController.Index = 0;
			if (!get_dialog3_anime() && !bd2.get_board2_anime())
			{
				Pressed_Button_No3(null);
			}
		}

		private void OnClick()
		{
		}

		public void UpdateInfo(int dockNo)
		{
			selected_dock = dockNo;
			_isBtnMaruUp = false;
			_clsRepair = rep.now_clsRepair();
			iTween.ScaleTo(base.gameObject, new Vector3(0.6f, 0.6f, 0.6f), 0f);
			int numOfKeyPossessions = _clsRepair.NumOfKeyPossessions;
			GameObject.Find("dialog3_top/dialog3/Text_b").GetComponent<UILabel>().text = numOfKeyPossessions.ToString();
			GameObject.Find("dialog3_top/dialog3/Text_a").GetComponent<UILabel>().text = (numOfKeyPossessions - 1).ToString();
		}

		public void dialog3_appear(bool bstat)
		{
			_isBtnMaruUp = false;
			_ani = ((Component)base.gameObject.transform.parent).GetComponent<Animation>();
			if (bstat)
			{
				_clickmask.unclickable_sec(0.5f);
				((Component)base.gameObject.transform.parent).GetComponent<UIPanel>().enabled = true;
				base.gameObject.MoveTo(new Vector3(0f, 0f, -2f), 0f, local: true);
				set_dialog3_anime(value: true);
				TweenScale tweenScale = TweenScale.Begin(base.gameObject, 0.4f, Vector3.one);
				tweenScale.animationCurve = UtilCurves.TweenEaseOutBack;
				tweenScale.SetOnFinished(CompleteHandler);
				rep.setmask(3, value: true);
			}
			else
			{
				set_dialog3_anime(value: true);
				iTween.MoveTo(base.gameObject, iTween.Hash("islocal", true, "x", 0f, "y", 2200, "z", -2, "delay", 0.3f, "time", 0f));
				iTween.ScaleTo(base.gameObject, iTween.Hash("islocal", true, "x", 0.6f, "y", 0.6f, "z", 0.6f, "delay", 0.3f, "time", 0f, "easetype", iTween.EaseType.easeOutBack, "oncomplete", "CompleteHandler_onClose", "oncompletetarget", base.gameObject));
				_ani.Play("dialog3_off");
				rep.setmask(3, value: false);
			}
		}

		public void Pressed_Button_Yes3(GameObject obj)
		{
			rep.set_mode(-1);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			_clickmask.unclickable_onesec();
			_isBtnMaruUp = false;
			Set_Button_Sprite(value: false);
			dockSelectController.Index = 0;
			dialog3_appear(bstat: false);
			b3 = GameObject.Find("board3").GetComponent<board3>();
			b3.board3_appear(bstat: false, isSirent: true);
			b2 = GameObject.Find("board2").GetComponent<board2>();
			b2.board2_appear(boardStart: false, isSirent: true);
			rep.all_rid_mask();
			bd1.OpenDock(selected_dock);
			rep.set_mode(1);
			TrophyUtil.Unlock_At_DockOpen();
		}

		public void Pressed_Button_No3(GameObject obj)
		{
			rep.set_mode(-1);
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			_clickmask.unclickable_sec(0.3f);
			_isBtnMaruUp = false;
			Set_Button_Sprite(value: true);
			dialog3_appear(bstat: false);
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			rep.setmask(3, value: false);
			rep.set_mode(1);
		}
	}
}
