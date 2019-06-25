using System;
using System.Collections.Generic;
using UnityEngine;

namespace Librarys.Object
{
	[RequireComponent(typeof(UIPanel))]
	public abstract class AbsDialog<CallbackType, ButtonType> : MonoBehaviour where CallbackType : struct where ButtonType : AbsDialogButton<CallbackType>
	{
		[SerializeField]
		protected List<ButtonType> _listButtons;

		private int _nIndex;

		private bool _isOpen;

		private UIPanel _uiPanel;

		protected Action _actOnCancel;

		protected Action<CallbackType> _actOnDecide;

		public int currentIndex
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

		public bool isOpen
		{
			get
			{
				return _isOpen;
			}
			protected set
			{
				_isOpen = value;
			}
		}

		public ButtonType currentButton => _listButtons[currentIndex];

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		protected virtual void OnDestroy()
		{
			OnUnInit();
			Mem.DelListSafe(ref _listButtons);
			Mem.Del(ref _nIndex);
			Mem.Del(ref _actOnCancel);
			Mem.Del(ref _actOnDecide);
		}

		public virtual bool Init(Action onCancel, Action<CallbackType> onDecide)
		{
			_actOnCancel = onCancel;
			_actOnDecide = onDecide;
			int cnt = 0;
			_listButtons.ForEach(delegate(ButtonType x)
			{
				x.Init(cnt, isValid: true, isColliderEnabled: false, 10, Util.CreateEventDelegateList(this, "OnActive", x.index), delegate
				{
					OnDecide();
				});
				cnt++;
			});
			OnInit();
			return true;
		}

		protected abstract void PreparaNext(bool isFoward);

		public void Next()
		{
			PreparaNext(isFoward: true);
		}

		public void Prev()
		{
			PreparaNext(isFoward: false);
		}

		public virtual void Open(Action onFinished)
		{
			if (!isOpen)
			{
				isOpen = true;
				OpenAnimation(delegate
				{
					_listButtons.ForEach(delegate(ButtonType x)
					{
						x.toggle.enabled = true;
					});
				});
			}
		}

		public virtual void Close(Action onFinished)
		{
			if (isOpen)
			{
				CloseAimation(delegate
				{
					isOpen = false;
					Dlg.Call(ref onFinished);
				});
			}
		}

		protected abstract void OpenAnimation(Action onFinished);

		protected abstract void CloseAimation(Action onFinished);

		protected virtual void ChangeFocus(int nIndex)
		{
			OnChangeFocus();
			_listButtons.ForEach(delegate(ButtonType x)
			{
				x.isFocus = ((x.index == nIndex) ? true : false);
			});
		}

		protected virtual void OnInit()
		{
		}

		protected virtual void OnUnInit()
		{
		}

		protected virtual void OnChangeFocus()
		{
		}

		protected abstract void OnActive(int nIndex);

		public virtual void OnCancel()
		{
			Dlg.Call(ref _actOnCancel);
		}

		public virtual void OnDecide()
		{
			_listButtons.ForEach(delegate(ButtonType x)
			{
				x.toggle.enabled = false;
			});
			if (_actOnDecide != null && (UnityEngine.Object)this.currentButton != (UnityEngine.Object)null)
			{
				ref Action<CallbackType> actOnDecide = ref _actOnDecide;
				ButtonType currentButton = this.currentButton;
				Dlg.Call(ref actOnDecide, currentButton.value);
			}
		}
	}
}
