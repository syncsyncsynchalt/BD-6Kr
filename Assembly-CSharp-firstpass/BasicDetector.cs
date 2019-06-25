using UnityEngine;

public class BasicDetector : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touches.Length; i++)
			{
				Touch touch = Input.touches[i];
				if (touch.phase == TouchPhase.Began)
				{
					IT_Gesture.OnTouchDown(touch);
				}
				else if (touch.phase == TouchPhase.Ended)
				{
					IT_Gesture.OnTouchUp(touch);
				}
				else
				{
					IT_Gesture.OnTouch(touch);
				}
			}
		}
		else if (Input.touchCount == 0)
		{
			if (Input.GetMouseButtonDown(0))
			{
				IT_Gesture.OnMouse1Down(Input.mousePosition);
			}
			else if (Input.GetMouseButtonUp(0))
			{
				IT_Gesture.OnMouse1Up(Input.mousePosition);
			}
			else if (Input.GetMouseButton(0))
			{
				IT_Gesture.OnMouse1(Input.mousePosition);
			}
			if (Input.GetMouseButtonDown(1))
			{
				IT_Gesture.OnMouse2Down(Input.mousePosition);
			}
			else if (Input.GetMouseButtonUp(1))
			{
				IT_Gesture.OnMouse2Up(Input.mousePosition);
			}
			else if (Input.GetMouseButton(1))
			{
				IT_Gesture.OnMouse2(Input.mousePosition);
			}
		}
	}
}
