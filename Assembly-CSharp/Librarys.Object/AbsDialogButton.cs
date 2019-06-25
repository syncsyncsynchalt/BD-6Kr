using System;
using System.Collections.Generic;
using UnityEngine;

namespace Librarys.Object
{
	[RequireComponent(typeof(UIToggle))]
	public abstract class AbsDialogButton<CallbackType> : MonoBehaviour
	{
		private int _nIndex;

		private bool _isFocus;

		private bool _isValid;

		private UIToggle _uiToggle;

		private BoxCollider2D _col2D;

		private CallbackType _pValue;

		public int index
		{
			get
			{
				return _nIndex;
			}
			protected set
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
				_isFocus = value;
			}
		}

		public bool isValid
		{
			get
			{
				return _isValid;
			}
			protected set
			{
				_isValid = value;
			}
		}

		public UIToggle toggle => this.GetComponentThis(ref _uiToggle);

		public CallbackType value
		{
			get
			{
				return _pValue;
			}
			protected set
			{
				_pValue = value;
			}
		}

		public virtual bool Init(int nIndex, bool isValid, bool isColliderEnabled, int nToggleGroup, List<EventDelegate> onActive, Action onDecide)
		{
			index = nIndex;
			this.isValid = isValid;
			toggle.enabled = isColliderEnabled;
			toggle.group = nToggleGroup;
			toggle.onActive = onActive;
			toggle.onDecide = onDecide;
			OnInit();
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _nIndex);
			Mem.Del(ref _isFocus);
			Mem.Del(ref _isValid);
			Mem.Del(ref _uiToggle);
			Mem.Del(ref _col2D);
			Mem.Del(ref _pValue);
			OnUnInit();
		}

		protected virtual void OnInit()
		{
		}

		protected virtual void OnUnInit()
		{
		}

		protected virtual void OnFocus(bool isFocus)
		{
		}
	}
}
