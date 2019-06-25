using System;

namespace KCV.Furniture
{
	public abstract class UIDynamicFurniture : UIFurniture
	{
		private Action<UIDynamicFurniture> mOnActionEvent;

		public void SetOnActionEvent(Action<UIDynamicFurniture> onActionEvent)
		{
			mOnActionEvent = onActionEvent;
		}

		[Obsolete("Inspectorで設定して使用します。")]
		public void OnTouchActionEvent()
		{
			OnActionEvent();
		}

		protected void OnActionEvent()
		{
			if (mOnActionEvent != null)
			{
				mOnActionEvent(this);
			}
			OnCalledActionEvent();
		}

		protected virtual void OnCalledActionEvent()
		{
		}

		protected override void OnDestroyEvent()
		{
			mOnActionEvent = null;
		}
	}
}
