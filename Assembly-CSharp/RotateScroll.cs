using UnityEngine;

public class RotateScroll : MonoBehaviour
{
	public GameObject obj;

	public float speed = 0.05f;

	private void Update()
	{
		if (Input.touchCount <= 0)
		{
			return;
		}
		Touch touch = Input.GetTouch(0);
		switch (touch.phase)
		{
		case TouchPhase.Stationary:
			break;
		case TouchPhase.Began:
			Debug.Log("touch  Panel start");
			break;
		case TouchPhase.Moved:
		{
			Vector2 position = touch.position;
			float x = position.x;
			float num = Screen.height;
			Vector2 position2 = touch.position;
			Vector2 v = new Vector2(x, num - position2.y);
			Vector2 vector = Camera.main.ScreenToWorldPoint(v);
			Vector2 deltaPosition = touch.deltaPosition;
			float num2 = deltaPosition.x * speed * 10f;
			Vector2 deltaPosition2 = touch.deltaPosition;
			float num3 = deltaPosition2.y * speed * 10f;
			Vector3 position3 = base.transform.position;
			float num4;
			if (position3.x > vector.x)
			{
				Vector3 position4 = base.transform.position;
				num4 = ((!(position4.y > vector.y)) ? (0f - num3 + num2) : (0f - num3 - num2));
			}
			else
			{
				Vector3 position5 = base.transform.position;
				num4 = ((!(position5.y > vector.y)) ? (num3 + num2) : (num3 - num2));
			}
			obj.transform.Rotate(0f, 0f, num4, Space.World);
			break;
		}
		case TouchPhase.Ended:
			Debug.Log("touch  Panel end");
			break;
		}
	}
}
