using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class HingeJoint : Joint
	{
		public JointMotor motor
		{
			get
			{
				INTERNAL_get_motor(out JointMotor value);
				return value;
			}
			set
			{
				INTERNAL_set_motor(ref value);
			}
		}

		public JointLimits limits
		{
			get
			{
				INTERNAL_get_limits(out JointLimits value);
				return value;
			}
			set
			{
				INTERNAL_set_limits(ref value);
			}
		}

		public JointSpring spring
		{
			get
			{
				INTERNAL_get_spring(out JointSpring value);
				return value;
			}
			set
			{
				INTERNAL_set_spring(ref value);
			}
		}

		public bool useMotor
		{
			get;
			set;
		}

		public bool useLimits
		{
			get;
			set;
		}

		public bool useSpring
		{
			get;
			set;
		}

		public float velocity
		{
			get;
		}

		public float angle
		{
			get;
		}

		private void INTERNAL_get_motor(out JointMotor value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_motor(ref JointMotor value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_limits(out JointLimits value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_limits(ref JointLimits value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_spring(out JointSpring value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_spring(ref JointSpring value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
