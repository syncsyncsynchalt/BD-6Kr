using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class WheelJoint2D : AnchoredJoint2D
	{
		public JointSuspension2D suspension
		{
			get
			{
				INTERNAL_get_suspension(out JointSuspension2D value);
				return value;
			}
			set
			{
				INTERNAL_set_suspension(ref value);
			}
		}

		public bool useMotor
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

		public float jointTranslation
		{
			get;
		}

		public float jointSpeed
		{
			get;
		}

		private void INTERNAL_get_suspension(out JointSuspension2D value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_suspension(ref JointSuspension2D value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_motor(out JointMotor2D value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_motor(ref JointMotor2D value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetMotorTorque(float timeStep)
		{
			return INTERNAL_CALL_GetMotorTorque(this, timeStep);
		}

		private static float INTERNAL_CALL_GetMotorTorque(WheelJoint2D self, float timeStep) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
