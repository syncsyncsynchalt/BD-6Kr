using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Gyroscope
	{
		private int m_GyroIndex;

		public Vector3 rotationRate => rotationRate_Internal(m_GyroIndex);

		public Vector3 rotationRateUnbiased => rotationRateUnbiased_Internal(m_GyroIndex);

		public Vector3 gravity => gravity_Internal(m_GyroIndex);

		public Vector3 userAcceleration => userAcceleration_Internal(m_GyroIndex);

		public Quaternion attitude => attitude_Internal(m_GyroIndex);

		public bool enabled
		{
			get
			{
				return getEnabled_Internal(m_GyroIndex);
			}
			set
			{
				setEnabled_Internal(m_GyroIndex, value);
			}
		}

		public float updateInterval
		{
			get
			{
				return getUpdateInterval_Internal(m_GyroIndex);
			}
			set
			{
				setUpdateInterval_Internal(m_GyroIndex, value);
			}
		}

		internal Gyroscope(int index)
		{
			m_GyroIndex = index;
		}

		private static Vector3 rotationRate_Internal(int idx) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static Vector3 rotationRateUnbiased_Internal(int idx) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static Vector3 gravity_Internal(int idx) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static Vector3 userAcceleration_Internal(int idx) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static Quaternion attitude_Internal(int idx) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static bool getEnabled_Internal(int idx) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void setEnabled_Internal(int idx, bool enabled) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static float getUpdateInterval_Internal(int idx) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void setUpdateInterval_Internal(int idx, float interval) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
