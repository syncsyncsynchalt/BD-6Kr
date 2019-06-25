using KCV.Utils;
using local.managers;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class dialog4 : MonoBehaviour
	{
		private UILabel ele_l;

		private UITexture ele_t;

		private UIPanel panel;

		private repair rep;

		private UISprite ele_d;

		private board3_top_mask bd3m;

		private board bd1;

		private board2 bd2;

		private board2 b2;

		private board3 b3;

		private KeyControl dockSelectController;

		private repair_clickmask _clickmask;

		private bool _isBtnMaruUp;

		private RepairManager _clsRepair;

		private UIButton _uiOverlayButton4;

		private Animation _ani;

		private bool _dialog4_anime;

		private Vector3 _bVector = Vector3.one;

		public bool get_dialog4_anime()
		{
			return _dialog4_anime;
		}

		public void set_dialog4_anime(bool value)
		{
			_dialog4_anime = value;
		}

		public void CompleteHandler()
		{
			base.gameObject.transform.localScale = Vector3.one;
			set_dialog4_anime(value: false);
		}

		public void CompleteHandler_onClose()
		{
			set_dialog4_anime(value: false);
			((Component)base.gameObject.transform.parent).GetComponent<UIPanel>().alpha = 1f;
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
			Mem.Del(ref b2);
			Mem.Del(ref b3);
			Mem.Del(ref dockSelectController);
			Mem.Del(ref _clickmask);
			Mem.Del(ref _clsRepair);
			Mem.Del(ref _uiOverlayButton4);
			Mem.Del(ref _ani);
			Mem.Del(ref _bVector);
		}

		public void _init_repair()
		{
			_isBtnMaruUp = false;
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			bd1 = GameObject.Find("board1_top/board").GetComponent<board>();
			bd2 = GameObject.Find("board2").GetComponent<board2>();
			iTween.ScaleTo(base.gameObject, new Vector3(0.6f, 0.6f, 0.6f), 0f);
			dockSelectController = new KeyControl(0, 1);
			dockSelectController.setChangeValue(0f, 1f, 0f, -1f);
			_uiOverlayButton4 = GameObject.Find("dialog4_top/dialog4/OverlayBtn4").GetComponent<UIButton>();
			EventDelegate.Add(_uiOverlayButton4.onClick, _onClickOverlayButton4);
			_clickmask = GameObject.Find("click_mask").GetComponent<repair_clickmask>();
			_bVector = Vector3.one;
		}

		private void Update()
		{
			if (rep.now_mode() != 7)
			{
				return;
			}
			dockSelectController.Update();
			if (dockSelectController.keyState[1].up || !dockSelectController.keyState[1].down)
			{
				_isBtnMaruUp = true;
			}
			if (base.gameObject.transform.localScale != Vector3.one)
			{
				if (base.gameObject.transform.localScale == _bVector)
				{
					base.gameObject.transform.localScale = Vector3.one;
					set_dialog4_anime(value: false);
				}
				_bVector = base.gameObject.transform.localScale;
			}
			if (!get_dialog4_anime() && (dockSelectController.keyState[0].up || (dockSelectController.keyState[1].down && _isBtnMaruUp)))
			{
				dockSelectController.Index = 0;
				Pressed_Button_No4(null);
			}
		}

		private void _onClickOverlayButton4()
		{
			if (!get_dialog4_anime() && !bd2.get_board2_anime())
			{
				Pressed_Button_No4(null);
			}
		}

		private void OnClick()
		{
		}

		public void UpdateInfo()
		{
			_clsRepair = rep.now_clsRepair();
			iTween.ScaleTo(base.gameObject, new Vector3(0.6f, 0.6f, 0.6f), 0f);
		}

		public void dialog4_appear(bool bstat)
		{
			_isBtnMaruUp = false;
			_ani = ((Component)base.gameObject.transform.parent).GetComponent<Animation>();
			if (bstat)
			{
				_clickmask.unclickable_sec(0.3f);
				GameObject.Find("dialog4_top").GetComponent<UIPanel>().enabled = true;
				base.gameObject.MoveTo(new Vector3(0f, -39f, -2f), 0f, local: true);
				set_dialog4_anime(value: true);
				iTween.ScaleTo(base.gameObject, iTween.Hash("islocal", true, "x", 1f, "y", 1f, "z", 1f, "time", 0.5f, "easetype", iTween.EaseType.easeOutBack, "oncomplete", "CompleteHandler", "oncompletetarget", base.gameObject));
				rep.setmask(3, value: true);
			}
			else
			{
				set_dialog4_anime(value: true);
				iTween.MoveTo(base.gameObject, iTween.Hash("islocal", true, "x", 0f, "y", -1100, "z", -2, "delay", 0.3f, "time", 0f));
				iTween.ScaleTo(base.gameObject, iTween.Hash("islocal", true, "x", 0.6f, "y", 0.6f, "z", 0.6f, "delay", 0.3f, "time", 0f, "easetype", iTween.EaseType.easeOutBack, "oncomplete", "CompleteHandler_onClose", "oncompletetarget", base.gameObject));
				_ani.Play("dialog4_off");
				rep.setmask(3, value: false);
			}
		}

		public void Pressed_Button_No4(GameObject obj)
		{
			rep.set_mode(-1);
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			_isBtnMaruUp = false;
			dialog4_appear(bstat: false);
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			rep.setmask(3, value: false);
			rep.set_mode(1);
		}
	}
}
