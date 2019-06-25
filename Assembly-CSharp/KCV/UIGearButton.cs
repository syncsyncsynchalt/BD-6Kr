using System;
using UniRx;
using UnityEngine;

namespace KCV
{
	[RequireComponent(typeof(UIButton))]
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(UIWidget))]
	public class UIGearButton : MonoBehaviour
	{
		private const float GEAR_BUTTON_ALPHA_TIME = 0.2f;

		[SerializeField]
		private UISprite _uiCenter;

		[SerializeField]
		private UISprite _uiGear;

		[SerializeField]
		private bool _isRotate = true;

		private UIButton _uiButton;

		private UIWidget _uiWidget;

		private Action _actCallback;

		public bool isRotate => _isRotate;

		public UIButton button
		{
			get
			{
				if (_uiButton == null)
				{
					_uiButton = GetComponent<UIButton>();
				}
				return _uiButton;
			}
		}

		public UIWidget widget
		{
			get
			{
				if (_uiWidget == null)
				{
					_uiWidget = GetComponent<UIWidget>();
				}
				return _uiWidget;
			}
		}

		public bool isColliderEnabled
		{
			get
			{
				return GetComponent<BoxCollider2D>();
			}
			set
			{
				GetComponent<BoxCollider2D>().enabled = value;
			}
		}

		private void Awake()
		{
			if (_uiCenter == null)
			{
				Util.FindParentToChild(ref _uiCenter, base.transform, "Center");
			}
			if (_uiGear == null)
			{
				Util.FindParentToChild(ref _uiGear, base.transform, "Gear");
			}
			_isRotate = true;
			_actCallback = null;
		}

		private void OnDestroy()
		{
			_uiCenter = null;
			_uiGear = null;
			_actCallback = null;
		}

		private void Update()
		{
			if (_isRotate)
			{
				_uiGear.transform.Rotate(new Vector3(0f, 0f, 30f) * (0f - Time.deltaTime));
			}
		}

		public void Show(Action callback)
		{
			base.transform.ValueTo(0f, 1f, 0.2f, iTween.EaseType.easeInSine, delegate(object x)
			{
				widget.alpha = Convert.ToSingle(x);
			}, delegate
			{
				isColliderEnabled = true;
				if (callback != null)
				{
					callback();
				}
			});
		}

		public void Hide(Action callback)
		{
			isColliderEnabled = false;
			base.transform.ValueTo(1f, 0f, 0.2f, iTween.EaseType.easeInSine, delegate(object x)
			{
				widget.alpha = Convert.ToSingle(x);
			}, delegate
			{
				isColliderEnabled = true;
				if (callback != null)
				{
					callback();
				}
			});
		}

		public void ResetDecideAction()
		{
			_actCallback = null;
		}

		public void SetDecideAction(Action callback)
		{
			_actCallback = callback;
			button.onClick = Util.CreateEventDelegateList(this, "Decide", null);
		}

		public void Decide()
		{
			button.state = UIButtonColor.State.Pressed;
			if (_actCallback != null)
			{
				_actCallback();
			}
			Observable.NextFrame().Subscribe(delegate
			{
				ResetDecideAction();
				button.state = UIButtonColor.State.Normal;
			});
		}
	}
}
