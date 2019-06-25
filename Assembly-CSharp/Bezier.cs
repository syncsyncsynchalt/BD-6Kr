using UnityEngine;

public class Bezier
{
	public enum BezierType
	{
		Quadratic,
		Cubic
	}

	private Vector3 _vPosStart;

	private Vector3 _vPosEnd;

	private Vector3 _vPosMid1;

	private Vector3 _vPosMid2;

	private BezierType _iType;

	public Bezier(BezierType type, Vector3 start, Vector3 end, Vector3 mid1, Vector3 mid2)
	{
		_iType = type;
		_vPosStart = start;
		_vPosEnd = end;
		_vPosMid1 = mid1;
		_vPosMid2 = mid2;
	}

	public Vector3 Interpolate(float t)
	{
		float num = 1f - t;
		float num2 = t * t;
		float num3 = num * num;
		Vector3 zero = default(Vector3);
		switch (_iType)
		{
		case BezierType.Quadratic:
			zero.x = num2 * _vPosEnd.x + 2f * t * num * _vPosMid1.x + num3 * _vPosStart.x;
			zero.y = num2 * _vPosEnd.y + 2f * t * num * _vPosMid1.y + num3 * _vPosStart.y;
			zero.z = num2 * _vPosEnd.z + 2f * t * num * _vPosMid1.z + num3 * _vPosStart.z;
			break;
		case BezierType.Cubic:
		{
			float num4 = num * num3;
			float num5 = 3f * t * num3;
			float num6 = 3f * num2 * num;
			float num7 = num2 * t;
			zero.x = num4 * _vPosStart.x + num5 * _vPosMid1.x + num6 * _vPosMid2.x + num7 * _vPosEnd.x;
			zero.y = num4 * _vPosStart.y + num5 * _vPosMid1.y + num6 * _vPosMid2.y + num7 * _vPosEnd.y;
			zero.z = num4 * _vPosStart.z + num5 * _vPosMid1.z + num6 * _vPosMid2.z + num7 * _vPosEnd.z;
			break;
		}
		default:
			zero = Vector3.zero;
			return zero;
		}
		return zero;
	}

	public float DistanceLinear()
	{
		return Vector3.Distance(_vPosStart, _vPosEnd);
	}

	public static Vector3 Interpolate(ref Vector3 pvr, Vector3 startPos, Vector3 endPos, float ft, Vector3 midPos1, Vector3 midPos2)
	{
		float num = 1f - ft;
		float num2 = ft * ft;
		float num3 = num * num;
		float num4 = num * num3;
		float num5 = 3f * ft * num3;
		float num6 = 3f * num2 * num;
		float num7 = num2 * ft;
		pvr.x = num4 * startPos.x + num5 * midPos1.x + num6 * midPos2.x + num7 * endPos.x;
		pvr.y = num4 * startPos.y + num5 * midPos1.y + num6 * midPos2.y + num7 * endPos.y;
		pvr.z = num4 * startPos.z + num5 * midPos1.z + num6 * midPos2.z + num7 * endPos.z;
		return pvr;
	}
}
