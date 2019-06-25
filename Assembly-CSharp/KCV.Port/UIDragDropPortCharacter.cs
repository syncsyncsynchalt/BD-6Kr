using UnityEngine;

namespace KCV.Port
{
	public class UIDragDropPortCharacter : UIDragDropItem
	{
		protected override void OnDragDropMove(Vector2 delta)
		{
			delta.y = 0f;
			base.OnDragDropMove(delta);
			Transform transform = base.transform;
			Vector3 localPosition = base.transform.localPosition;
			transform.localPositionX(Util.RangeValue(localPosition.x, -350f, 400f));
		}
	}
}
