using UnityEngine;

namespace KCV.Arsenal
{
	public class Arsenal_DevKit : MonoBehaviour
	{
		public UIButtonManager _buttonManager;

		[SerializeField]
		private UIButton[] _switches;

		[SerializeField]
		private UISprite _selecter;

		public void Init()
		{
			if (_buttonManager == null)
			{
				_buttonManager = ((Component)base.transform.FindChild("Switches")).GetComponent<UIButtonManager>();
			}
			_switches = new UIButton[3];
			for (int i = 0; i < 3; i++)
			{
				if (_switches[i] == null)
				{
					_switches[i] = ((Component)_buttonManager.transform.FindChild("Switch" + (i + 1))).GetComponent<UIButton>();
				}
			}
			if (_selecter == null)
			{
				_selecter = ((Component)base.transform.FindChild("FrameSelect")).GetComponent<UISprite>();
			}
			setSwitch(0);
			_buttonManager.IsFocusButtonAlwaysHover = true;
		}

		public void setSwitch(int switchNo)
		{
			_buttonManager.nowForcusIndex = switchNo;
			_switches[switchNo].SetState(UIButtonColor.State.Hover, immediate: true);
		}

		public void nextSwitch()
		{
			_buttonManager.nowForcusIndex = (int)Util.LoopValue(_buttonManager.nowForcusIndex + 1, 0f, 2f);
			setSwitch(_buttonManager.nowForcusIndex);
		}

		public void SetSelecter(bool show)
		{
			_selecter.SetActive(show);
		}

		public int getUseDevKitNum()
		{
			switch (_buttonManager.nowForcusIndex)
			{
			case 0:
				return 1;
			case 1:
				return 20;
			case 2:
				return 100;
			default:
				Debug.LogWarning("開発資材のインデックスが不正です");
				return -1;
			}
		}
	}
}
