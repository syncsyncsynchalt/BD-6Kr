using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class SliderJoint2D : AnchoredJoint2D
	{
		public float angle
		{
			get;
			set;
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

		public JointMotor2D motor
		{
			get
			{
				INTERNAL_get_motor(out JointMotor2D value);
				return value;
			}
			set
			{
				INTERNAL_set_motor(ref value);
			}
		}

		public JointTranslationLimits2D limits
		{
			get
			{
				INTERNAL_get_limits(out JointTranslationLimits2D value);
				return value;
			}
			set
			{
				INTERNAL_set_limits(ref value);
			}
		}

		public JointLimitState2D limitState
		{
			get;
		}

		public float referenceAngle
		{
			get;
		}

		public float jointTranslation
		{
			get;
		}

		public float jointSpeed
		{
			get;
		}

		private void INTERNAL_get_motor(out JointMotor2D value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_motor(ref JointMotor2D value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_limits(out JointTranslationLimits2D value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_limits(ref JointTranslationLimits2D value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetMotorForce(float timeStep)
		{
			return INTERNAL_CALL_GetMotorForce(this, timeStep);
		}

		private static float INTERNAL_CALL_GetMotorForce(SliderJoint2D self, float timeStep) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
