using UnityEngine;

namespace KCV.Arsenal
{
	public class Arsenal_SPoint : MonoBehaviour
	{
		public UIButtonManager _buttonManager;

		[SerializeField]
		private UISprite _selecter;

		private bool _isStarted;

		public bool SPointStarted()
		{
			return _isStarted;
		}

		public void Init()
		{
			if (_buttonManager == null)
			{
				_buttonManager = ((Component)base.transform.FindChild("Switches")).GetComponent<UIButtonManager>();
			}
			if (_selecter == null)
			{
				_selecter = ((Component)base.transform.FindChild("FrameSelect")).GetComponent<UISprite>();
			}
			_buttonManager.setFocus(0);
			_buttonManager.IsFocusButtonAlwaysHover = true;
			_buttonManager.isLoopIndex = true;
			_isStarted = true;
		}

		public void NextSwitch()
		{
			_buttonManager.moveNextButton();
		}

		public void SetSelecter(bool show)
		{
			_selecter.SetActive(show);
		}

		public int GetUseSpointNum()
		{
			switch (_buttonManager.nowForcusIndex)
			{
			case 0:
				return 50;
			case 1:
				return 100;
			case 2:
				return 250;
			case 3:
				return 400;
			default:
				Debug.LogWarning("戦略ポイントのインデックスが不正です");
				return -1;
			}
		}

		private void OnDestroy()
		{
			_buttonManager = null;
			_selecter = null;
		}
	}
}
