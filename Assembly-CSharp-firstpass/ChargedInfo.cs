using UnityEngine;

public class ChargedInfo
{
	public float percent;

	public Vector2 pos;

	public int fingerCount = 1;

	public Vector2[] positions = new Vector2[1];

	public bool isMouse;

	public int index;

	public int[] indexes = new int[1];

	public Vector2 pos1;

	public Vector2 pos2;

	public ChargedInfo(Vector2 p, float val)
		: this(p, val, 0, im: false)
	{
	}

	public ChargedInfo(Vector2 p, float val, int ind, bool im)
	{
		pos = p;
		percent = val;
		index = ind;
		isMouse = im;
		positions[0] = pos;
		indexes[0] = ind;
	}

	public ChargedInfo(Vector2 p, Vector2[] posL, float val, int[] inds)
	{
		pos = p;
		positions = posL;
		percent = val;
		indexes = inds;
		fingerCount = indexes.Length;
	}

	public ChargedInfo(Vector2 p, float val, Vector2 p1, Vector2 p2)
		: this(p, val, p1, p2, 0, im: false)
	{
	}

	public ChargedInfo(Vector2 p, float val, Vector2 p1, Vector2 p2, int ind, bool im)
	{
		pos = p;
		percent = val;
		pos1 = p1;
		pos2 = p2;
		index = ind;
		isMouse = im;
	}
}
