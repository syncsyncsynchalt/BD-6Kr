using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OnDoubleClickEventSender : MonoBehaviour
{
	[SerializeField]
	private EventDelegate mOnDoubleClickEventListener;

	private void OnDoubleClick()
	{
		if (mOnDoubleClickEventListener != null)
		{
			mOnDoubleClickEventListener.Execute();
		}
	}
}
