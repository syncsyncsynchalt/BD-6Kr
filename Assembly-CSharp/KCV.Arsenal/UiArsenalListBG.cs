using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalListBG : MonoBehaviour
	{
		private GameObject _uiOverlayBtn;

		private bool _BGtouched;

		private void Start()
		{
			_uiOverlayBtn = base.gameObject;
			_BGtouched = false;
		}

		public void _onClickOverlayButton()
		{
			_BGtouched = true;
		}

		public void set_touch(bool value)
		{
			_BGtouched = value;
		}

		public bool get_touch()
		{
			return _BGtouched;
		}
	}
}
