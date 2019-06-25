using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class ConstantForce2D : PhysicsUpdateBehaviour2D
	{
		public Vector2 force
		{
			get
			{
				INTERNAL_get_force(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_force(ref value);
			}
		}

		public Vector2 relativeForce
		{
			get
			{
				INTERNAL_get_relativeForce(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_relativeForce(ref value);
			}
		}

		public float torque
		{
			get;
			set;
		}

		private void INTERNAL_get_force(out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_force(ref Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_relativeForce(out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_relativeForce(ref Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
