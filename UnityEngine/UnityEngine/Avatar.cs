using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Avatar : Object
	{
		public bool isValid
		{
			get;
		}

		public bool isHuman
		{
			get;
		}

		private Avatar()
		{
		}

		internal void SetMuscleMinMax(int muscleId, float min, float max) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void SetParameter(int parameterId, float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal float GetAxisLength(int humanId) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal Quaternion GetPreRotation(int humanId) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal Quaternion GetPostRotation(int humanId) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal Quaternion GetZYPostQ(int humanId, Quaternion parentQ, Quaternion q)
		{
			return INTERNAL_CALL_GetZYPostQ(this, humanId, ref parentQ, ref q);
		}

		private static Quaternion INTERNAL_CALL_GetZYPostQ(Avatar self, int humanId, ref Quaternion parentQ, ref Quaternion q) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal Quaternion GetZYRoll(int humanId, Vector3 uvw)
		{
			return INTERNAL_CALL_GetZYRoll(this, humanId, ref uvw);
		}

		private static Quaternion INTERNAL_CALL_GetZYRoll(Avatar self, int humanId, ref Vector3 uvw) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal Vector3 GetLimitSign(int humanId) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
