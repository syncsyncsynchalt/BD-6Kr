using UnityEngine;

namespace KCV.View.Scroll
{
	[RequireComponent(typeof(UIWidget))]
	public class UIScrollListChild<__Model__> : MonoBehaviour where __Model__ : class
	{
		public delegate void UIScrollListChildAction(ActionType actionType, UIScrollListChild<__Model__> targetChild);

		[SerializeField]
		protected UIButton mButton_Action;

		private UIScrollListChildAction mUIScrollListChildAction;

		private UIWidget mWidgetThis;

		private Vector3 mNextLocalPositionCache = Vector3.zero;

		public bool mIsClickable
		{
			get;
			private set;
		}

		public Vector2 Size => mWidgetThis.localSize;

		public __Model__ Model
		{
			get;
			private set;
		}

		public int SortIndex
		{
			get;
			private set;
		}

		public bool IsShown
		{
			get;
			private set;
		}

		public Transform cachedTransform
		{
			get;
			private set;
		}

		private void Awake()
		{
			cachedTransform = base.transform;
			mWidgetThis = GetComponent<UIWidget>();
			if (mButton_Action != null)
			{
				mButton_Action.hover = Util.CursolColor;
				mButton_Action.defaultColor = Color.white;
				mButton_Action.pressed = Color.white;
				mButton_Action.disabledColor = Color.white;
			}
		}

		public void Initialize(__Model__ model, bool clickable)
		{
			Model = model;
			mIsClickable = clickable;
			InitializeChildContents(Model, mIsClickable);
		}

		public void Refresh(__Model__ model, bool clickable)
		{
			Model = model;
			mIsClickable = clickable;
			RefreshChildContents(Model, mIsClickable);
		}

		public void UpdateIndex(int nextIndex)
		{
			SortIndex = nextIndex;
		}

		protected virtual void InitializeChildContents(__Model__ model, bool isClickable)
		{
		}

		protected virtual void RefreshChildContents(__Model__ model, bool isClickable)
		{
		}

		public virtual void Show()
		{
			IsShown = true;
			mWidgetThis.alpha = 1f;
			OnShowAnimation();
		}

		public virtual void Hide()
		{
			IsShown = false;
			mWidgetThis.alpha = 0.0001f;
		}

		public void ReleaseModel()
		{
			Model = (__Model__)null;
		}

		public virtual void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(mButton_Action.gameObject, value: true);
		}

		public virtual void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(mButton_Action.gameObject, value: false);
		}

		public void StopBlink()
		{
			RemoveHover();
		}

		public virtual void OnShowAnimation()
		{
		}

		public Vector3 GenerateConnectToPosition(UIScrollListChild<__Model__> target, ConnectType connectType)
		{
			Vector3 localPosition = target.transform.localPosition;
			switch (connectType)
			{
			case ConnectType.Top:
			{
				ref Vector3 reference2 = ref mNextLocalPositionCache;
				float x2 = localPosition.x;
				float y2 = localPosition.y;
				Vector2 size2 = Size;
				reference2.Set(x2, y2 + size2.y, localPosition.z);
				break;
			}
			case ConnectType.Bottom:
			{
				ref Vector3 reference = ref mNextLocalPositionCache;
				float x = localPosition.x;
				float y = localPosition.y;
				Vector2 size = target.Size;
				reference.Set(x, y - size.y, localPosition.z);
				break;
			}
			default:
				mNextLocalPositionCache.Set(0f, 0f, 0f);
				break;
			}
			return mNextLocalPositionCache;
		}

		public void SetOnActionListener(UIScrollListChildAction method)
		{
			mUIScrollListChildAction = method;
		}

		private void OnAction(ActionType actionType, UIScrollListChild<__Model__> targetChild)
		{
			if (mUIScrollListChildAction != null)
			{
				mUIScrollListChildAction(actionType, targetChild);
			}
		}

		public virtual void OnTouchScrollListChild()
		{
			if (Model != null && mIsClickable)
			{
				OnAction(ActionType.OnTouch, this);
			}
		}

		public void setEnable(bool isEnable)
		{
			mButton_Action.enabled = isEnable;
		}

		private void OnDestroy()
		{
			mButton_Action = null;
			mUIScrollListChildAction = null;
			mWidgetThis = null;
			Model = (__Model__)null;
			cachedTransform = null;
			OnCallDestroy();
		}

		protected virtual void OnCallDestroy()
		{
		}
	}
}
