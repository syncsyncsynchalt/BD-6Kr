using UnityEngine;

public class CommonDialogMessage : MonoBehaviour
{
	[SerializeField]
	private UILabel MessageTitle;

	[SerializeField]
	private UILabel MessageText;

	public void SetMessage(string Title, string Message)
	{
		MessageTitle.text = Title;
		MessageText.text = Message;
	}
}
