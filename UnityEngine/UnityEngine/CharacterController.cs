using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class CharacterController : Collider
	{
		public bool isGrounded
		{
			get;
		}

		public Vector3 velocity
		{
			get
			{
				INTERNAL_get_velocity(out Vector3 value);
				return value;
			}
		}

		public CollisionFlags collisionFlags
		{
			get;
		}

		public float radius
		{
			get;
			set;
		}

		public float height
		{
			get;
			set;
		}

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

		public float slopeLimit
		{
			get;
			set;
		}

		public float stepOffset
		{
			get;
			set;
		}

		public float skinWidth
		{
			get;
			set;
		}

		public bool detectCollisions
		{
			get;
			set;
		}

		public bool SimpleMove(Vector3 speed)
		{
			return INTERNAL_CALL_SimpleMove(this, ref speed);
		}

		private static bool INTERNAL_CALL_SimpleMove(CharacterController self, ref Vector3 speed) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public CollisionFlags Move(Vector3 motion)
		{
			return INTERNAL_CALL_Move(this, ref motion);
		}

		private static CollisionFlags INTERNAL_CALL_Move(CharacterController self, ref Vector3 motion) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_velocity(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_center(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_center(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
