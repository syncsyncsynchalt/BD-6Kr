using UnityEngine;

public class Tap
{
	public Vector2 posInitial;

	public Vector2 pos;

	public int count;

	public int fingerCount = 1;

	public Vector2[] positions = new Vector2[1];

	public bool isMouse;

	public int index;

	public int[] indexes = new int[1];

	public Tap(Vector2 p)
		: this(p, p, 1, 0, im: false)
	{
	}

	public Tap(Vector2 p, int c)
		: this(p, p, c, 0, im: false)
	{
	}

	public Tap(Vector2 p, int c, int ind)
		: this(p, p, c, ind, im: false)
	{
	}

	public Tap(Vector2 p1, Vector2 p2, int c, int ind)
		: this(p1, p2, c, ind, im: false)
	{
	}

	public Tap(Vector2 p, int c, int ind, bool im)
		: this(p, p, c, ind, im)
	{
	}

	public Tap(Vector2 p1, Vector2 p2, int c, int ind, bool im)
	{
		posInitial = p1;
		pos = p2;
		count = c;
		index = ind;
		isMouse = im;
		positions[0] = pos;
		indexes[0] = index;
	}

	public Tap(int c, int fc, Vector2[] ps, int[] inds)
	{
		count = c;
		fingerCount = fc;
		positions = ps;
		indexes = inds;
		Vector2 a = Vector2.zero;
		for (int i = 0; i < positions.Length; i++)
		{
			a += positions[i];
		}
		Vector2 vector = a / positions.Length;
	}
}
