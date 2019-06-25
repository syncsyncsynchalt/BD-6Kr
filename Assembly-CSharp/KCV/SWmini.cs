using UnityEngine;

namespace KCV
{
	public class SWmini : MonoBehaviour
	{
		private string _WhoamI;

		private Animation _Ani;

		private bool _Now_State;

		private KeyControl KeyController;

		public bool _get_nowstate()
		{
			return _Now_State;
		}

		public void _change_state(bool val)
		{
			_Now_State = val;
		}

		private void Start()
		{
			_init_repair();
		}

		public void _init_repair()
		{
			_Now_State = false;
			_Ani = base.gameObject.GetComponent<Animation>();
			_Ani.Play();
			KeyController = new KeyControl();
			KeyController.setChangeValue(0f, 0f, 0f, 0f);
		}

		public void _toggle_state()
		{
			if (_Now_State)
			{
				_Now_State = false;
			}
			else
			{
				_Now_State = true;
			}
		}

		public void OnClick()
		{
			_toggle_state();
			_sw_draw();
		}

		private void _sw_draw()
		{
			Debug.Log(base.gameObject.name + "/switch_ball");
			if (_Now_State)
			{
				_Ani.Play("mini_up");
				GameObject.Find(base.gameObject.name + "/switch_ball").GetComponent<UISprite>().spriteName = "switch_on_pin";
				GameObject.Find(base.gameObject.name + "/switch_use").GetComponent<UISprite>().spriteName = "switch_m_on";
				GameObject.Find(base.gameObject.name + "/z").ScaleTo(new Vector3(0f, 0f, 0f), 0.01f);
			}
			else
			{
				_Ani.Play("mini_down");
				GameObject.Find(base.gameObject.name + "/switch_ball").GetComponent<UISprite>().spriteName = "switch_off_pin";
				GameObject.Find(base.gameObject.name + "/switch_use").GetComponent<UISprite>().spriteName = "switch_m_off";
				GameObject.Find(base.gameObject.name + "/z").ScaleTo(new Vector3(1f, 1f, 1f), 0.01f);
			}
		}

		private void Update()
		{
			KeyController.Update();
			if (KeyController.keyState[1].down)
			{
				OnClick();
			}
			else if (KeyController.keyState[2].down)
			{
				OnClick();
			}
		}
	}
}
