using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public struct Bounds
	{
		private Vector3 m_Center;

		private Vector3 m_Extents;

		public Vector3 center
		{
			get
			{
				return m_Center;
			}
			set
			{
				m_Center = value;
			}
		}

		public Vector3 size
		{
			get
			{
				return m_Extents * 2f;
			}
			set
			{
				m_Extents = value * 0.5f;
			}
		}

		public Vector3 extents
		{
			get
			{
				return m_Extents;
			}
			set
			{
				m_Extents = value;
			}
		}

		public Vector3 min
		{
			get
			{
				return center - extents;
			}
			set
			{
				SetMinMax(value, max);
			}
		}

		public Vector3 max
		{
			get
			{
				return center + extents;
			}
			set
			{
				SetMinMax(min, value);
			}
		}

		public Bounds(Vector3 center, Vector3 size)
		{
			m_Center = center;
			m_Extents = size * 0.5f;
		}

		public override int GetHashCode()
		{
			return center.GetHashCode() ^ (extents.GetHashCode() << 2);
		}

		public override bool Equals(object other)
		{
			if (!(other is Bounds))
			{
				return false;
			}
			Bounds bounds = (Bounds)other;
			return center.Equals(bounds.center) && extents.Equals(bounds.extents);
		}

		public void SetMinMax(Vector3 min, Vector3 max)
		{
			extents = (max - min) * 0.5f;
			center = min + extents;
		}

		public void Encapsulate(Vector3 point)
		{
			SetMinMax(Vector3.Min(min, point), Vector3.Max(max, point));
		}

		public void Encapsulate(Bounds bounds)
		{
			Encapsulate(bounds.center - bounds.extents);
			Encapsulate(bounds.center + bounds.extents);
		}

		public void Expand(float amount)
		{
			amount *= 0.5f;
			extents += new Vector3(amount, amount, amount);
		}

		public void Expand(Vector3 amount)
		{
			extents += amount * 0.5f;
		}

		public bool Intersects(Bounds bounds)
		{
			Vector3 min = this.min;
			float x = min.x;
			Vector3 max = bounds.max;
			int result;
			if (x <= max.x)
			{
				Vector3 max2 = this.max;
				float x2 = max2.x;
				Vector3 min2 = bounds.min;
				if (x2 >= min2.x)
				{
					Vector3 min3 = this.min;
					float y = min3.y;
					Vector3 max3 = bounds.max;
					if (y <= max3.y)
					{
						Vector3 max4 = this.max;
						float y2 = max4.y;
						Vector3 min4 = bounds.min;
						if (y2 >= min4.y)
						{
							Vector3 min5 = this.min;
							float z = min5.z;
							Vector3 max5 = bounds.max;
							if (z <= max5.z)
							{
								Vector3 max6 = this.max;
								float z2 = max6.z;
								Vector3 min6 = bounds.min;
								result = ((z2 >= min6.z) ? 1 : 0);
								goto IL_00d7;
							}
						}
					}
				}
			}
			result = 0;
			goto IL_00d7;
			IL_00d7:
			return (byte)result != 0;
		}

		private static bool Internal_Contains(Bounds m, Vector3 point)
		{
			return INTERNAL_CALL_Internal_Contains(ref m, ref point);
		}

		private static bool INTERNAL_CALL_Internal_Contains(ref Bounds m, ref Vector3 point) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool Contains(Vector3 point)
		{
			return Internal_Contains(this, point);
		}

		private static float Internal_SqrDistance(Bounds m, Vector3 point)
		{
			return INTERNAL_CALL_Internal_SqrDistance(ref m, ref point);
		}

		private static float INTERNAL_CALL_Internal_SqrDistance(ref Bounds m, ref Vector3 point) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float SqrDistance(Vector3 point)
		{
			return Internal_SqrDistance(this, point);
		}

		private static bool Internal_IntersectRay(ref Ray ray, ref Bounds bounds, out float distance)
		{
			return INTERNAL_CALL_Internal_IntersectRay(ref ray, ref bounds, out distance);
		}

		private static bool INTERNAL_CALL_Internal_IntersectRay(ref Ray ray, ref Bounds bounds, out float distance) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool IntersectRay(Ray ray)
		{
			float distance;
			return Internal_IntersectRay(ref ray, ref this, out distance);
		}

		public bool IntersectRay(Ray ray, out float distance)
		{
			return Internal_IntersectRay(ref ray, ref this, out distance);
		}

		private static Vector3 Internal_GetClosestPoint(ref Bounds bounds, ref Vector3 point)
		{
			return INTERNAL_CALL_Internal_GetClosestPoint(ref bounds, ref point);
		}

		private static Vector3 INTERNAL_CALL_Internal_GetClosestPoint(ref Bounds bounds, ref Vector3 point) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector3 ClosestPoint(Vector3 point)
		{
			return Internal_GetClosestPoint(ref this, ref point);
		}

		public override string ToString()
		{
			return UnityString.Format("Center: {0}, Extents: {1}", m_Center, m_Extents);
		}

		public string ToString(string format)
		{
			return UnityString.Format("Center: {0}, Extents: {1}", m_Center.ToString(format), m_Extents.ToString(format));
		}

		public static bool operator ==(Bounds lhs, Bounds rhs)
		{
			return lhs.center == rhs.center && lhs.extents == rhs.extents;
		}

		public static bool operator !=(Bounds lhs, Bounds rhs)
		{
			return !(lhs == rhs);
		}
	}
}
