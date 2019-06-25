using LT.Tweening;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(UIWidget))]
	[RequireComponent(typeof(UIToggle))]
	public class UILabelButton : MonoBehaviour, ISelectedObject<int>
	{
		[SerializeField]
		private UIWidget _uiForeground;

		private int _nIndex;

		private bool _isFocus;

		private bool _isValid;

		private UIWidget _uiBackground;

		private BoxCollider2D _colBox2D;

		private UIToggle _uiToggle;

		private Color _cValidColor;

		private Color _cInvalidColor;

		public int index
		{
			get
			{
				return _nIndex;
			}
			private set
			{
				_nIndex = value;
			}
		}

		public bool isFocus
		{
			get
			{
				return _isFocus;
			}
			set
			{
				if (value && isValid)
				{
					_isFocus = true;
					_uiForeground.transform.LTCancel();
					_uiForeground.transform.LTValue(_uiForeground.alpha, 1f, Defines.FORMATION_FORMATIONLABEL_ALPHA_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
					{
						_uiForeground.alpha = x;
					});
					toggle.value = true;
				}
				else
				{
					_isFocus = false;
					_uiForeground.transform.LTCancel();
					_uiForeground.transform.LTValue(_uiForeground.alpha, 0f, Defines.FORMATION_FORMATIONLABEL_ALPHA_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
					{
						_uiForeground.alpha = x;
					});
					toggle.value = false;
				}
			}
		}

		public bool isValid => _isValid;

		protected UIWidget background => this.GetComponentThis(ref _uiBackground);

		protected UIWidget foreground => _uiForeground;

		public UIToggle toggle => this.GetComponentThis(ref _uiToggle);

		private void OnDestroy()
		{
			Mem.Del(ref _uiForeground);
			Mem.Del(ref _cValidColor);
			Mem.Del(ref _nIndex);
			Mem.Del(ref _isFocus);
			Mem.Del(ref _isValid);
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _colBox2D);
			Mem.Del(ref _uiToggle);
		}

		public bool Init(int nIndex, bool isValid)
		{
			return Init(nIndex, isValid, background.color);
		}

		public bool Init(int nIndex, bool isValid, Color validColor)
		{
			return Init(nIndex, isValid, validColor, new Color(1f, 1f, 1f, 0.5f));
		}

		public bool Init(int nIndex, bool isValid, Color validColor, Color invalidColor)
		{
			_nIndex = nIndex;
			_isValid = isValid;
			_cValidColor = validColor;
			_cInvalidColor = invalidColor;
			SetValidColor(this.isValid);
			return true;
		}

		private void SetValidColor(bool isValid)
		{
			background.color = ((!isValid) ? _cInvalidColor : _cValidColor);
		}

		public void SetValid(bool isValid)
		{
			_isValid = isValid;
			SetValidColor(_isValid);
		}
	}
}
