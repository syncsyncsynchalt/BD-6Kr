using System;

using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public class Collider2D : Behaviour
	{
		public bool isTrigger
		{
			get;
			set;
		}

		public bool usedByEffector
		{
			get;
			set;
		}

		public Vector2 offset
		{
			get
			{
				INTERNAL_get_offset(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_offset(ref value);
			}
		}

		public Rigidbody2D attachedRigidbody
		{
			get;
		}

		public int shapeCount
		{
			get;
		}

		public Bounds bounds
		{
			get
			{
				INTERNAL_get_bounds(out Bounds value);
				return value;
			}
		}

		internal ColliderErrorState2D errorState
		{
			get;
		}

		public PhysicsMaterial2D sharedMaterial
		{
			get;
			set;
		}

		private void INTERNAL_get_offset(out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_offset(ref Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_bounds(out Bounds value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool OverlapPoint(Vector2 point)
		{
			return INTERNAL_CALL_OverlapPoint(this, ref point);
		}

		private static bool INTERNAL_CALL_OverlapPoint(Collider2D self, ref Vector2 point) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool IsTouching(Collider2D collider) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool IsTouchingLayers([DefaultValue("Physics2D.AllLayers")] int layerMask) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public bool IsTouchingLayers()
		{
			int layerMask = -1;
			return IsTouchingLayers(layerMask);
		}
	}
}
