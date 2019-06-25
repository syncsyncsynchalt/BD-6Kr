using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public class Motion : Object
	{
		public float averageDuration
		{
			get;
		}

		public float averageAngularSpeed
		{
			get;
		}

		public Vector3 averageSpeed
		{
			get
			{
				INTERNAL_get_averageSpeed(out Vector3 value);
				return value;
			}
		}

		public float apparentSpeed
		{
			get;
		}

		public bool isLooping
		{
			get;
		}

		public bool legacy
		{
			get;
		}

		public bool isHumanMotion
		{
			get;
		}

		[Obsolete("isAnimatorMotion is not supported anymore. Use !legacy instead.", true)]
		public bool isAnimatorMotion
		{
			get;
		}

		private void INTERNAL_get_averageSpeed(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("ValidateIfRetargetable is not supported anymore. Use isHumanMotion instead.", true)]
		public bool ValidateIfRetargetable(bool val) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
