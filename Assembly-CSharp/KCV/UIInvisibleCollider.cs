using System;
using UnityEngine;

namespace KCV
{
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(UIButton))]
	public class UIInvisibleCollider : MonoBehaviour
	{
		[SerializeField]
		private UIWidget _uiInvisibleObject;

		[SerializeField]
		private int _nDepth;

		private UIButton _uiButton;

		private BoxCollider2D _colBox2D;

		private Action _actOnTouch;

		public UIButton button => this.GetComponentThis(ref _uiButton);

		public new BoxCollider2D collider2D => this.GetComponentThis(ref _colBox2D);

		public int depth
		{
			get
			{
				return _nDepth;
			}
			set
			{
				if (_uiInvisibleObject != null)
				{
					_nDepth = value;
					_uiInvisibleObject.depth = value;
				}
			}
		}

		private void Awake()
		{
			button.onClick = Util.CreateEventDelegateList(this, "OnTouch", null);
			if (_uiInvisibleObject == null)
			{
				GameObject gameObject = new GameObject("InvisibleObject");
				gameObject.transform.parent = base.transform;
				gameObject.transform.AddComponent<UIWidget>();
				_uiInvisibleObject = gameObject.GetComponent<UIWidget>();
				_uiInvisibleObject.depth = depth;
			}
			_uiInvisibleObject.depth = _nDepth;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiInvisibleObject);
			Mem.Del(ref _uiButton);
			Mem.Del(ref _colBox2D);
			Mem.Del(ref _actOnTouch);
		}

		public void SetOnTouch(Action onTouch)
		{
			_actOnTouch = onTouch;
		}

		private void OnTouch()
		{
			Dlg.Call(ref _actOnTouch);
		}
	}
}
