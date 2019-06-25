using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalParamBG : MonoBehaviour
	{
		private GameObject _uiOverlayBtn;

		private bool _BGtouched;

		public bool isEnable;

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

		private void OnDestroy()
		{
			_uiOverlayBtn = null;
		}
	}
}
