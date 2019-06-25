using UnityEngine;

namespace KCV.Scene.Revamp.RepairShipAnime
{
	public class UIRepairShipAnime : MonoBehaviour
	{
		private UISprite _sprite;

		private Animation _ani;

		private bool _eye_move;

		private bool _pos_right;

		private void Start()
		{
			_sprite = GameObject.Find("Eye").GetComponent<UISprite>();
			_ani = GameObject.Find("RepairShipPanel").GetComponent<Animation>();
			_eye_move = true;
			_pos_right = false;
		}

		private void Update()
		{
			if (_eye_move)
			{
				if ((int)Random.Range(0f, 100f) == 1)
				{
					_ani.Play("akashi_eye_blink_open");
				}
				else if ((int)Random.Range(0f, 200f) == 1)
				{
					_ani.Play("akashi_eye_kyoro_open");
				}
			}
		}

		public void eye_play(int ptn)
		{
			if (ptn < 1 || 5 < ptn)
			{
				ptn = 1;
			}
			_sprite.spriteName = "a_eye" + ptn;
		}

		public void eye_motion(bool mode)
		{
			_eye_move = mode;
			if (!_eye_move)
			{
				_sprite.spriteName = "a_eye1";
			}
		}
	}
}
