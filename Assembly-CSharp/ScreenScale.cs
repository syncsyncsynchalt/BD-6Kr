using UnityEngine;

public class ScreenScale : MonoBehaviour
{
	public int maxHeight = 1024;

	private void Start()
	{
		float num = (float)maxHeight / (float)Screen.height;
		if (num > 1f)
		{
			num = 1f;
		}
		int width = (int)((float)Screen.width * num);
		int num2 = (int)((float)Screen.height * num);
		if (Screen.height != num2)
		{
			Screen.SetResolution(width, num2, fullscreen: true, 15);
		}
	}
}
