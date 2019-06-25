using System;
using UnityEngine;

namespace KCV.BattleCut
{
	[Serializable]
	public class SelectLabelObject
	{
		[SerializeField]
		private Transform _tra;

		[SerializeField]
		private UILabel _uiLabel;

		[SerializeField]
		private int _nIndex;

		private Color _cDefaultColor;

		private bool _isActive;

		private bool _isFocus;

		public Transform transform => _tra;

		public UILabel label => _uiLabel;

		public int index => _nIndex;

		public bool isActive
		{
			get
			{
				return _isActive;
			}
			set
			{
				_isActive = value;
				_uiLabel.alpha = ((!value) ? 0.5f : 1f);
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
				_isFocus = value;
				_uiLabel.color = (value ? Util.CursolColor : ((!isActive) ? new Color(0.5f, 0.5f, 0.5f, 0.5f) : _cDefaultColor));
			}
		}

		public SelectLabelObject(Transform transform, int nIndex)
		{
			_tra = transform;
			_uiLabel = ((Component)_tra).GetComponent<UILabel>();
			_nIndex = nIndex;
			_cDefaultColor = _uiLabel.color;
			isFocus = false;
			isActive = false;
		}

		public bool Init(int nIndex)
		{
			_nIndex = nIndex;
			return true;
		}
	}
}
