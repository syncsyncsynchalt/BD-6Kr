using UnityEngine;

public class RotateInfo
{
	public float magnitude;

	public Vector2 pos1;

	public Vector2 pos2;

	public RotateInfo(float mag, Vector2 p1, Vector2 p2)
	{
		magnitude = mag;
		pos1 = p1;
		pos2 = p2;
	}
}
