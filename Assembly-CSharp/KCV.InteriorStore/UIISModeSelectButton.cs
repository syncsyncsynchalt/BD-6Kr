using System.Collections.Generic;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIISModeSelectButton : MonoBehaviour
	{
		private DelDecideISMode _delDecideISMode;

		private List<UIButton> _listBtns;

		private bool _isFocus;

		private bool _isDecide;

		private ISMode _iMode;

		public bool isFocus
		{
			get
			{
				return _isFocus;
			}
			set
			{
				if (value && !_isFocus)
				{
					_isFocus = true;
					setBtnsState(_isFocus);
				}
				else if (!value && isFocus)
				{
					_isFocus = false;
					setBtnsState(_isFocus);
				}
			}
		}

		public ISMode mode => _iMode;

		public bool isColliderEnabled
		{
			get
			{
				return GetComponent<Collider2D>().enabled;
			}
			set
			{
				GetComponent<Collider2D>().enabled = value;
			}
		}

		private void Awake()
		{
			_isFocus = false;
			_isDecide = false;
			_iMode = ISMode.None;
			_listBtns = new List<UIButton>();
			_listBtns.AddRange(GetComponents<UIButton>());
			setBtnsState(_isFocus);
			_listBtns[0].onClick = Util.CreateEventDelegateList(this, "decideButton", null);
			isColliderEnabled = true;
		}

		public void Reset()
		{
			_isDecide = false;
			isFocus = false;
			isColliderEnabled = true;
		}

		public bool Init(ISMode iMode, DelDecideISMode delDecide)
		{
			_iMode = iMode;
			_delDecideISMode = delDecide;
			return true;
		}

		private void Update()
		{
			if (!isFocus && _listBtns[0].state != 0)
			{
				setBtnsState(isFocus: false);
			}
		}

		private void setBtnsState(bool isFocus)
		{
			UIButtonColor.State state = isFocus ? UIButtonColor.State.Pressed : UIButtonColor.State.Normal;
			foreach (UIButton listBtn in _listBtns)
			{
				listBtn.state = state;
			}
		}

		private void decideButton()
		{
			_isDecide = true;
			if (_delDecideISMode != null)
			{
				_delDecideISMode(_iMode);
			}
		}
	}
}
