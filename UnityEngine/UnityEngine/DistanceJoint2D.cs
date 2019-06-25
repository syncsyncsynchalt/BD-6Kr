using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class DistanceJoint2D : AnchoredJoint2D
	{
		public float distance
		{
			get;
			set;
		}

		public bool maxDistanceOnly
		{
			get;
			set;
		}

		public Vector2 GetReactionForce(float timeStep)
		{
			DistanceJoint2D_CUSTOM_INTERNAL_GetReactionForce(this, timeStep, out Vector2 value);
			return value;
		}

		private static void DistanceJoint2D_CUSTOM_INTERNAL_GetReactionForce(DistanceJoint2D joint, float timeStep, out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetReactionTorque(float timeStep)
		{
			return INTERNAL_CALL_GetReactionTorque(this, timeStep);
		}

		private static float INTERNAL_CALL_GetReactionTorque(DistanceJoint2D self, float timeStep) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
