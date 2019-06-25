using UnityEngine;

public class MultiTapTracker
{
	public int index;

	public int count;

	public float lastTapTime = -1f;

	public Vector2 lastPos;

	public int fingerCount = 1;

	public MultiTapTracker(int ind)
	{
		index = ind;
	}
}
