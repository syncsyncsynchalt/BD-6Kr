using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class SpringJoint2D : AnchoredJoint2D
	{
		public float distance
		{
			get;
			set;
		}

		public float dampingRatio
		{
			get;
			set;
		}

		public float frequency
		{
			get;
			set;
		}

		public Vector2 GetReactionForce(float timeStep)
		{
			SpringJoint2D_CUSTOM_INTERNAL_GetReactionForce(this, timeStep, out Vector2 value);
			return value;
		}

		private static void SpringJoint2D_CUSTOM_INTERNAL_GetReactionForce(SpringJoint2D joint, float timeStep, out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetReactionTorque(float timeStep)
		{
			return INTERNAL_CALL_GetReactionTorque(this, timeStep);
		}

		private static float INTERNAL_CALL_GetReactionTorque(SpringJoint2D self, float timeStep) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
