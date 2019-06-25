using UnityEngine;

namespace KCV
{
	public class MiscCalculateUtils
	{
		public static Vector3[] CalculateBezierPoint(Vector3 src1, Vector3 src2, float length)
		{
			DebugUtils.SLog("CalculateBezierPoint");
			Vector3 center = (src1 + src2) / 2f;
			float num = length / Vector3.Distance(src1, src2);
			Vector3[] array = new Vector3[2]
			{
				Rotate90AndScale(src1, center, num),
				Rotate90AndScale(src2, center, num)
			};
			DebugUtils.SLog("=========================================");
			DebugUtils.SLog("p1 x,y=" + src1.x + "," + src1.y);
			DebugUtils.SLog("p2 x,y=" + src2.x + "," + src2.y);
			DebugUtils.SLog("[center]x,y=" + center.x + "," + center.y + "[scale]" + num);
			DebugUtils.SLog("x,y=" + array[0].x + "," + array[0].y + ", x,y=" + array[1].x + "," + array[1].y);
			DebugUtils.SLog("=========================================");
			return array;
		}

		private static Vector3 Rotate90AndScale(Vector3 src, Vector3 center, float scale)
		{
			float x = (0f - (src.y - center.y)) * scale + center.x;
			float y = (src.x - center.x) * scale + center.y;
			return new Vector3(x, y, 0f);
		}
	}
}
