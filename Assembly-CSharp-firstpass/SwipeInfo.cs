using UnityEngine;

public class SwipeInfo
{
	public Vector2 startPoint;

	public Vector2 endPoint;

	public Vector2 direction;

	public float angle;

	public float duration;

	public float speed;

	public int index;

	public bool isMouse;

	public SwipeInfo(Vector2 p1, Vector2 p2, Vector2 dir, float startT, int ind, bool im)
	{
		startPoint = p1;
		endPoint = p2;
		direction = dir;
		angle = IT_Gesture.VectorToAngle(dir);
		duration = Time.realtimeSinceStartup - startT;
		speed = dir.magnitude / duration;
		index = ind;
		isMouse = im;
	}

	public float GetMagnitude()
	{
		return (endPoint - startPoint).magnitude;
	}
}
