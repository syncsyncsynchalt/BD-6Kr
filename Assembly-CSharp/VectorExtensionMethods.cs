using UnityEngine;

public static class VectorExtensionMethods
{
	private static Vector3 _vVec;

	public static void Add(this Vector3 vector, Vector3 v0)
	{
		vector = Mathe.Add(vector, v0);
	}

	public static void Sub(this Vector3 vector, Vector3 v0)
	{
		vector = Mathe.Sub(vector, v0);
	}

	public static Vector3 ScrSide2Vector(this Vector3 vector, ExtensionUtils.Side side)
	{
		switch (side)
		{
		case ExtensionUtils.Side.BottomLeft:
			_vVec.Set(0f, Screen.height, 0f);
			break;
		case ExtensionUtils.Side.Left:
			_vVec.Set(0f, Screen.height / 2, 0f);
			break;
		case ExtensionUtils.Side.TopLeft:
			_vVec.Set(0f, 0f, 0f);
			break;
		case ExtensionUtils.Side.Top:
			_vVec.Set(Screen.width / 2, 0f, 0f);
			break;
		case ExtensionUtils.Side.TopRight:
			_vVec.Set(Screen.width, 0f, 0f);
			break;
		case ExtensionUtils.Side.Right:
			_vVec.Set(Screen.width, Screen.height / 2, 0f);
			break;
		case ExtensionUtils.Side.BottomRight:
			_vVec.Set(Screen.width, Screen.height, 0f);
			break;
		case ExtensionUtils.Side.Bottom:
			_vVec.Set(Screen.width / 2, Screen.height, 0f);
			break;
		case ExtensionUtils.Side.Center:
			_vVec.Set(Screen.width / 2, Screen.height / 2, 0f);
			break;
		default:
			_vVec = vector;
			break;
		}
		return vector = _vVec;
	}

	public static Vector3 ScrSideCenter(this Vector3 vector)
	{
		_vVec.Set(Screen.width / 2, Screen.height / 2, 0f);
		return vector = _vVec;
	}

	public static Vector3 Zero(this Vector3 vector)
	{
		return vector = Vector3.zero;
	}

	public static Vector3 Back(this Vector3 vector)
	{
		return vector = Vector3.back;
	}

	public static Vector3 Down(this Vector3 vector)
	{
		return vector = Vector3.down;
	}

	public static Vector3 Forward(this Vector3 vector)
	{
		return vector = Vector3.forward;
	}

	public static Vector3 Left(this Vector3 vector)
	{
		return vector = Vector3.left;
	}

	public static Vector3 One(this Vector3 vector)
	{
		return vector = Vector3.one;
	}

	public static Vector3 Right(this Vector3 vector)
	{
		return vector = Vector3.right;
	}

	public static Vector3 Up(this Vector3 vector)
	{
		return vector = Vector3.up;
	}

	public static Vector3 Sync(this Vector3 vector, Vector3 target)
	{
		return vector = target;
	}

	public static Vector3 Sync(this Vector3 vector, GameObject obj)
	{
		return vector = obj.gameObject.transform.position;
	}

	public static Vector3 Flash2Vector3(this Vector3 vector)
	{
		return new Vector3(vector.x, vector.y * -1f, vector.z);
	}
}
