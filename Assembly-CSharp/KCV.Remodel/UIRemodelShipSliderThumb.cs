using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodelShipSliderThumb : MonoBehaviour
	{
		public enum ActionType
		{
			Move,
			FingerUp
		}

		public enum CheckType
		{
			DraggableY
		}

		public delegate void UIRemodelShipSliderThumbAction(ActionType actionType, UIRemodelShipSliderThumb calledObject);

		public delegate bool UIRemodelShipSliderThumbCheck(CheckType checkType, UIRemodelShipSliderThumb calledObject);

		private UIRemodelShipSliderThumbAction mUIRemodelShipSliderThumbAction;

		private UIRemodelShipSliderThumbCheck mUIRemodelShipSliderThumbCheck;

		public Vector3 mNextDragWorldPosition = Vector3.zero;

		public void SetOnUIRemodelShipSliderThumbActionListener(UIRemodelShipSliderThumbAction action)
		{
			mUIRemodelShipSliderThumbAction = action;
		}

		public void SetOnUIRemodelShipSliderThumbCheckListener(UIRemodelShipSliderThumbCheck checker)
		{
			mUIRemodelShipSliderThumbCheck = checker;
		}

		private void OnAction(ActionType actionType, UIRemodelShipSliderThumb calledObject)
		{
			if (mUIRemodelShipSliderThumbAction != null)
			{
				mUIRemodelShipSliderThumbAction(actionType, calledObject);
			}
		}

		private bool OnCheck(CheckType checkType, UIRemodelShipSliderThumb calledObject)
		{
			if (mUIRemodelShipSliderThumbCheck != null)
			{
				return mUIRemodelShipSliderThumbCheck(checkType, calledObject);
			}
			return false;
		}

		private void OnDrag(Vector2 delta)
		{
			Vector2 lastTouchPosition = UICamera.lastTouchPosition;
			Vector2 vector = UICamera.currentCamera.ScreenToWorldPoint(lastTouchPosition);
			Vector3 position = base.transform.position;
			float x = position.x;
			float y = vector.y;
			Vector3 position2 = base.transform.position;
			mNextDragWorldPosition = new Vector3(x, y, position2.z);
			if (!OnCheck(CheckType.DraggableY, this))
			{
				OnAction(ActionType.Move, this);
			}
			else
			{
				mNextDragWorldPosition = base.transform.position;
			}
		}

		internal void Release()
		{
			mUIRemodelShipSliderThumbAction = null;
			mUIRemodelShipSliderThumbCheck = null;
		}
	}
}
