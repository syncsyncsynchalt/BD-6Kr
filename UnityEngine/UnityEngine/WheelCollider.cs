using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class WheelCollider : Collider
	{
		public Vector3 center
		{
			get
			{
				INTERNAL_get_center(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_center(ref value);
			}
		}

		public float radius
		{
			get;
			set;
		}

		public float suspensionDistance
		{
			get;
			set;
		}

		public JointSpring suspensionSpring
		{
			get
			{
				INTERNAL_get_suspensionSpring(out JointSpring value);
				return value;
			}
			set
			{
				INTERNAL_set_suspensionSpring(ref value);
			}
		}

		public float forceAppPointDistance
		{
			get;
			set;
		}

		public float mass
		{
			get;
			set;
		}

		public float wheelDampingRate
		{
			get;
			set;
		}

		public WheelFrictionCurve forwardFriction
		{
			get
			{
				INTERNAL_get_forwardFriction(out WheelFrictionCurve value);
				return value;
			}
			set
			{
				INTERNAL_set_forwardFriction(ref value);
			}
		}

		public WheelFrictionCurve sidewaysFriction
		{
			get
			{
				INTERNAL_get_sidewaysFriction(out WheelFrictionCurve value);
				return value;
			}
			set
			{
				INTERNAL_set_sidewaysFriction(ref value);
			}
		}

		public float motorTorque
		{
			get;
			set;
		}

		public float brakeTorque
		{
			get;
			set;
		}

		public float steerAngle
		{
			get;
			set;
		}

		public bool isGrounded
		{
			get;
		}

		public float sprungMass
		{
			get;
		}

		public float rpm
		{
			get;
		}

		private void INTERNAL_get_center(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_center(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_suspensionSpring(out JointSpring value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_suspensionSpring(ref JointSpring value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_forwardFriction(out WheelFrictionCurve value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_forwardFriction(ref WheelFrictionCurve value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_sidewaysFriction(out WheelFrictionCurve value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_sidewaysFriction(ref WheelFrictionCurve value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void ConfigureVehicleSubsteps(float speedThreshold, int stepsBelowThreshold, int stepsAboveThreshold) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool GetGroundHit(out WheelHit hit) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void GetWorldPose(out Vector3 pos, out Quaternion quat) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
