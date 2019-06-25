using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class HingeJoint2D : AnchoredJoint2D
	{
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

		public JointAngleLimits2D limits
		{
			get
			{
				INTERNAL_get_limits(out JointAngleLimits2D value);
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

		public float jointAngle
		{
			get;
		}

		public float jointSpeed
		{
			get;
		}

		private void INTERNAL_get_motor(out JointMotor2D value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_motor(ref JointMotor2D value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_limits(out JointAngleLimits2D value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_limits(ref JointAngleLimits2D value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector2 GetReactionForce(float timeStep)
		{
			HingeJoint2D_CUSTOM_INTERNAL_GetReactionForce(this, timeStep, out Vector2 value);
			return value;
		}

		private static void HingeJoint2D_CUSTOM_INTERNAL_GetReactionForce(HingeJoint2D joint, float timeStep, out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetReactionTorque(float timeStep)
		{
			return INTERNAL_CALL_GetReactionTorque(this, timeStep);
		}

		private static float INTERNAL_CALL_GetReactionTorque(HingeJoint2D self, float timeStep) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetMotorTorque(float timeStep)
		{
			return INTERNAL_CALL_GetMotorTorque(this, timeStep);
		}

		private static float INTERNAL_CALL_GetMotorTorque(HingeJoint2D self, float timeStep) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
