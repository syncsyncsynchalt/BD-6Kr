using System;
using UnityEngine;

namespace LT.Tweening
{
	[Serializable]
	public class LTSpline
	{
		public static int DISTANCE_COUNT = 30;

		public static int SUBLINE_COUNT = 50;

		public Vector3[] pts;

		public Vector3[] ptsAdj;

		public int ptsAdjLength;

		public bool orientToPath;

		public bool orientToPath2d;

		private int numSections;

		private int currPt;

		private float totalLength;

		public LTSpline(params Vector3[] pts)
		{
			this.pts = new Vector3[pts.Length];
			Array.Copy(pts, this.pts, pts.Length);
			numSections = pts.Length - 3;
			float num = float.PositiveInfinity;
			Vector3 b = this.pts[1];
			float num2 = 0f;
			for (int i = 2; i < this.pts.Length - 2; i++)
			{
				float num3 = Vector3.Distance(this.pts[i], b);
				if (num3 < num)
				{
					num = num3;
				}
				num2 += num3;
			}
			float num4 = num / (float)SUBLINE_COUNT;
			int num5 = (int)Mathf.Ceil(num2 / num4) * DISTANCE_COUNT;
			ptsAdj = new Vector3[num5];
			b = interp(0f);
			int num6 = 0;
			for (int j = 0; j < num5; j++)
			{
				float t = ((float)j + 1f) / (float)num5;
				Vector3 vector = interp(t);
				float num7 = Vector3.Distance(vector, b);
				if (num7 >= num4)
				{
					ptsAdj[num6] = vector;
					b = vector;
					num6++;
				}
			}
			ptsAdjLength = num6;
		}

		public Vector3 map(float u)
		{
			if (u >= 1f)
			{
				return pts[pts.Length - 2];
			}
			float num = u * (float)(ptsAdjLength - 1);
			int num2 = (int)Mathf.Floor(num);
			int num3 = (int)Mathf.Ceil(num);
			Vector3 vector = ptsAdj[num2];
			Vector3 a = ptsAdj[num3];
			float d = num - (float)num2;
			return vector + (a - vector) * d;
		}

		public Vector3 interp(float t)
		{
			currPt = Mathf.Min(Mathf.FloorToInt(t * (float)numSections), numSections - 1);
			float num = t * (float)numSections - (float)currPt;
			Vector3 a = pts[currPt];
			Vector3 a2 = pts[currPt + 1];
			Vector3 vector = pts[currPt + 2];
			Vector3 b = pts[currPt + 3];
			return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num * num * num) + (2f * a - 5f * a2 + 4f * vector - b) * (num * num) + (-a + vector) * num + 2f * a2);
		}

		public Vector3 point(float ratio)
		{
			float u = (!(ratio > 1f)) ? ratio : 1f;
			return map(u);
		}

		public void place2d(Transform transform, float ratio)
		{
			transform.position = point(ratio);
			ratio += 0.001f;
			if (ratio <= 1f)
			{
				Vector3 vector = point(ratio) - transform.position;
				float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				transform.eulerAngles = new Vector3(0f, 0f, z);
			}
		}

		public void placeLocal2d(Transform transform, float ratio)
		{
			transform.localPosition = point(ratio);
			ratio += 0.001f;
			if (ratio <= 1f)
			{
				Vector3 vector = transform.parent.TransformPoint(point(ratio)) - transform.localPosition;
				float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				transform.eulerAngles = new Vector3(0f, 0f, z);
			}
		}

		public void place(Transform transform, float ratio)
		{
			place(transform, ratio, Vector3.up);
		}

		public void place(Transform transform, float ratio, Vector3 worldUp)
		{
			transform.position = point(ratio);
			ratio += 0.001f;
			if (ratio <= 1f)
			{
				transform.LookAt(point(ratio), worldUp);
			}
		}

		public void placeLocal(Transform transform, float ratio)
		{
			placeLocal(transform, ratio, Vector3.up);
		}

		public void placeLocal(Transform transform, float ratio, Vector3 worldUp)
		{
			transform.localPosition = point(ratio);
			ratio += 0.001f;
			if (ratio <= 1f)
			{
				transform.LookAt(transform.parent.TransformPoint(point(ratio)), worldUp);
			}
		}

		public void gizmoDraw(float t = -1f)
		{
			Vector3 vector = point(0f);
			for (int i = 1; i <= 120; i++)
			{
				float ratio = (float)i / 120f;
				Vector3 vector2 = point(ratio);
				Gizmos.DrawLine(vector2, vector);
				vector = vector2;
			}
		}
	}
}
