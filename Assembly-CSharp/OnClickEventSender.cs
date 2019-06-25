using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OnClickEventSender : MonoBehaviour
{
	[SerializeField]
	private EventDelegate onClick;

	private void OnClick()
	{
		if (onClick != null)
		{
			onClick.Execute();
		}
	}

	public void SetClickable(bool clickable)
	{
		GetComponent<Collider2D>().enabled = clickable;
	}

	private void OnDestroy()
	{
		onClick = null;
	}
}
