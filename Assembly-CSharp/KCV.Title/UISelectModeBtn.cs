using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Title
{
	[Serializable]
	public class UISelectModeBtn : IDisposable
	{
		[SerializeField]
		private UISprite _uiSprite;

		private SelectMode _iMode;

		private UIToggle _uiToggle;

		private bool _isFocus;

		public Transform transform => _uiSprite.transform;

		public bool isFocus
		{
			get
			{
				return _isFocus;
			}
			set
			{
				if (value)
				{
					_isFocus = true;
					_uiSprite.spriteName = $"btn_{_iMode.ToString()}_on";
					_uiSprite.transform.LTCancel();
					_uiSprite.transform.LTValue(_uiSprite.color, Color.white, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(Color x)
					{
						_uiSprite.color = x;
					});
					toggle.value = true;
				}
				else
				{
					_isFocus = false;
					_uiSprite.spriteName = $"btn_{_iMode.ToString()}";
					_uiSprite.transform.LTCancel();
					_uiSprite.transform.LTValue(_uiSprite.color, Color.gray, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(Color x)
					{
						_uiSprite.color = x;
					});
					toggle.value = false;
				}
			}
		}

		public UIToggle toggle => _uiSprite.GetComponent<UIToggle>();

		public SelectMode mode => _iMode;

		public float alpha
		{
			get
			{
				return _uiSprite.alpha;
			}
			set
			{
				_uiSprite.alpha = value;
			}
		}

		public UISelectModeBtn(Transform obj)
		{
			_uiSprite = ((Component)obj).GetComponent<UISprite>();
			_isFocus = false;
			_iMode = SelectMode.Appointed;
		}

		public bool Init(SelectMode iMode)
		{
			_iMode = iMode;
			_uiSprite.color = Color.gray;
			return true;
		}

		public void Dispose()
		{
			Mem.Del(ref _uiSprite);
			Mem.Del(ref _iMode);
			Mem.Del(ref _uiToggle);
			Mem.Del(ref _isFocus);
		}
	}
}
