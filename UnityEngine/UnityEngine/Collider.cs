using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public class Collider : Component
	{
		public bool enabled
		{
			get;
			set;
		}

		public Rigidbody attachedRigidbody
		{
			get;
		}

		public bool isTrigger
		{
			get;
			set;
		}

		public float contactOffset
		{
			get;
			set;
		}

		public PhysicMaterial material
		{
			get;
			set;
		}

		public PhysicMaterial sharedMaterial
		{
			get;
			set;
		}

		public Bounds bounds
		{
			get
			{
				INTERNAL_get_bounds(out Bounds value);
				return value;
			}
		}

		public Vector3 ClosestPointOnBounds(Vector3 position)
		{
			return INTERNAL_CALL_ClosestPointOnBounds(this, ref position);
		}

		private static Vector3 INTERNAL_CALL_ClosestPointOnBounds(Collider self, ref Vector3 position) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_bounds(out Bounds value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static bool Internal_Raycast(Collider col, Ray ray, out RaycastHit hitInfo, float maxDistance)
		{
			return INTERNAL_CALL_Internal_Raycast(col, ref ray, out hitInfo, maxDistance);
		}

		private static bool INTERNAL_CALL_Internal_Raycast(Collider col, ref Ray ray, out RaycastHit hitInfo, float maxDistance) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
		{
			return Internal_Raycast(this, ray, out hitInfo, maxDistance);
		}
	}
}
