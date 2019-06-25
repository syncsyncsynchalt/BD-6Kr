using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class Rigidbody : Component
	{
		public Vector3 velocity
		{
			get
			{
				INTERNAL_get_velocity(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_velocity(ref value);
			}
		}

		public Vector3 angularVelocity
		{
			get
			{
				INTERNAL_get_angularVelocity(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_angularVelocity(ref value);
			}
		}

		public float drag
		{
			get;
			set;
		}

		public float angularDrag
		{
			get;
			set;
		}

		public float mass
		{
			get;
			set;
		}

		public bool useGravity
		{
			get;
			set;
		}

		public float maxDepenetrationVelocity
		{
			get;
			set;
		}

		public bool isKinematic
		{
			get;
			set;
		}

		public bool freezeRotation
		{
			get;
			set;
		}

		public RigidbodyConstraints constraints
		{
			get;
			set;
		}

		public CollisionDetectionMode collisionDetectionMode
		{
			get;
			set;
		}

		public Vector3 centerOfMass
		{
			get
			{
				INTERNAL_get_centerOfMass(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_centerOfMass(ref value);
			}
		}

		public Vector3 worldCenterOfMass
		{
			get
			{
				INTERNAL_get_worldCenterOfMass(out Vector3 value);
				return value;
			}
		}

		public Quaternion inertiaTensorRotation
		{
			get
			{
				INTERNAL_get_inertiaTensorRotation(out Quaternion value);
				return value;
			}
			set
			{
				INTERNAL_set_inertiaTensorRotation(ref value);
			}
		}

		public Vector3 inertiaTensor
		{
			get
			{
				INTERNAL_get_inertiaTensor(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_inertiaTensor(ref value);
			}
		}

		public bool detectCollisions
		{
			get;
			set;
		}

		public bool useConeFriction
		{
			get;
			set;
		}

		public Vector3 position
		{
			get
			{
				INTERNAL_get_position(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_position(ref value);
			}
		}

		public Quaternion rotation
		{
			get
			{
				INTERNAL_get_rotation(out Quaternion value);
				return value;
			}
			set
			{
				INTERNAL_set_rotation(ref value);
			}
		}

		public RigidbodyInterpolation interpolation
		{
			get;
			set;
		}

		public int solverIterationCount
		{
			get;
			set;
		}

		[Obsolete("The sleepVelocity is no longer supported. Use sleepThreshold. Note that sleepThreshold is energy but not velocity.")]
		public float sleepVelocity
		{
			get;
			set;
		}

		[Obsolete("The sleepAngularVelocity is no longer supported. Set Use sleepThreshold to specify energy.")]
		public float sleepAngularVelocity
		{
			get;
			set;
		}

		public float sleepThreshold
		{
			get;
			set;
		}

		public float maxAngularVelocity
		{
			get;
			set;
		}

		private void INTERNAL_get_velocity(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_velocity(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_angularVelocity(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_angularVelocity(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetDensity(float density)
		{
			INTERNAL_CALL_SetDensity(this, density);
		}

		private static void INTERNAL_CALL_SetDensity(Rigidbody self, float density) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void AddForce(Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			INTERNAL_CALL_AddForce(this, ref force, mode);
		}

		[ExcludeFromDocs]
		public void AddForce(Vector3 force)
		{
			ForceMode mode = ForceMode.Force;
			INTERNAL_CALL_AddForce(this, ref force, mode);
		}

		private static void INTERNAL_CALL_AddForce(Rigidbody self, ref Vector3 force, ForceMode mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void AddForce(float x, float y, float z)
		{
			ForceMode mode = ForceMode.Force;
			AddForce(x, y, z, mode);
		}

		public void AddForce(float x, float y, float z, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			AddForce(new Vector3(x, y, z), mode);
		}

		public void AddRelativeForce(Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			INTERNAL_CALL_AddRelativeForce(this, ref force, mode);
		}

		[ExcludeFromDocs]
		public void AddRelativeForce(Vector3 force)
		{
			ForceMode mode = ForceMode.Force;
			INTERNAL_CALL_AddRelativeForce(this, ref force, mode);
		}

		private static void INTERNAL_CALL_AddRelativeForce(Rigidbody self, ref Vector3 force, ForceMode mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void AddRelativeForce(float x, float y, float z)
		{
			ForceMode mode = ForceMode.Force;
			AddRelativeForce(x, y, z, mode);
		}

		public void AddRelativeForce(float x, float y, float z, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			AddRelativeForce(new Vector3(x, y, z), mode);
		}

		public void AddTorque(Vector3 torque, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			INTERNAL_CALL_AddTorque(this, ref torque, mode);
		}

		[ExcludeFromDocs]
		public void AddTorque(Vector3 torque)
		{
			ForceMode mode = ForceMode.Force;
			INTERNAL_CALL_AddTorque(this, ref torque, mode);
		}

		private static void INTERNAL_CALL_AddTorque(Rigidbody self, ref Vector3 torque, ForceMode mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void AddTorque(float x, float y, float z)
		{
			ForceMode mode = ForceMode.Force;
			AddTorque(x, y, z, mode);
		}

		public void AddTorque(float x, float y, float z, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			AddTorque(new Vector3(x, y, z), mode);
		}

		public void AddRelativeTorque(Vector3 torque, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			INTERNAL_CALL_AddRelativeTorque(this, ref torque, mode);
		}

		[ExcludeFromDocs]
		public void AddRelativeTorque(Vector3 torque)
		{
			ForceMode mode = ForceMode.Force;
			INTERNAL_CALL_AddRelativeTorque(this, ref torque, mode);
		}

		private static void INTERNAL_CALL_AddRelativeTorque(Rigidbody self, ref Vector3 torque, ForceMode mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void AddRelativeTorque(float x, float y, float z)
		{
			ForceMode mode = ForceMode.Force;
			AddRelativeTorque(x, y, z, mode);
		}

		public void AddRelativeTorque(float x, float y, float z, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			AddRelativeTorque(new Vector3(x, y, z), mode);
		}

		public void AddForceAtPosition(Vector3 force, Vector3 position, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			INTERNAL_CALL_AddForceAtPosition(this, ref force, ref position, mode);
		}

		[ExcludeFromDocs]
		public void AddForceAtPosition(Vector3 force, Vector3 position)
		{
			ForceMode mode = ForceMode.Force;
			INTERNAL_CALL_AddForceAtPosition(this, ref force, ref position, mode);
		}

		private static void INTERNAL_CALL_AddForceAtPosition(Rigidbody self, ref Vector3 force, ref Vector3 position, ForceMode mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius, [DefaultValue("0.0F")] float upwardsModifier, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			INTERNAL_CALL_AddExplosionForce(this, explosionForce, ref explosionPosition, explosionRadius, upwardsModifier, mode);
		}

		[ExcludeFromDocs]
		public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius, float upwardsModifier)
		{
			ForceMode mode = ForceMode.Force;
			INTERNAL_CALL_AddExplosionForce(this, explosionForce, ref explosionPosition, explosionRadius, upwardsModifier, mode);
		}

		[ExcludeFromDocs]
		public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius)
		{
			ForceMode mode = ForceMode.Force;
			float upwardsModifier = 0f;
			INTERNAL_CALL_AddExplosionForce(this, explosionForce, ref explosionPosition, explosionRadius, upwardsModifier, mode);
		}

		private static void INTERNAL_CALL_AddExplosionForce(Rigidbody self, float explosionForce, ref Vector3 explosionPosition, float explosionRadius, float upwardsModifier, ForceMode mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector3 ClosestPointOnBounds(Vector3 position)
		{
			return INTERNAL_CALL_ClosestPointOnBounds(this, ref position);
		}

		private static Vector3 INTERNAL_CALL_ClosestPointOnBounds(Rigidbody self, ref Vector3 position) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector3 GetRelativePointVelocity(Vector3 relativePoint)
		{
			return INTERNAL_CALL_GetRelativePointVelocity(this, ref relativePoint);
		}

		private static Vector3 INTERNAL_CALL_GetRelativePointVelocity(Rigidbody self, ref Vector3 relativePoint) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector3 GetPointVelocity(Vector3 worldPoint)
		{
			return INTERNAL_CALL_GetPointVelocity(this, ref worldPoint);
		}

		private static Vector3 INTERNAL_CALL_GetPointVelocity(Rigidbody self, ref Vector3 worldPoint) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_centerOfMass(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_centerOfMass(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_worldCenterOfMass(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_inertiaTensorRotation(out Quaternion value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_inertiaTensorRotation(ref Quaternion value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_inertiaTensor(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_inertiaTensor(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_position(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_position(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_rotation(out Quaternion value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_rotation(ref Quaternion value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void MovePosition(Vector3 position)
		{
			INTERNAL_CALL_MovePosition(this, ref position);
		}

		private static void INTERNAL_CALL_MovePosition(Rigidbody self, ref Vector3 position) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void MoveRotation(Quaternion rot)
		{
			INTERNAL_CALL_MoveRotation(this, ref rot);
		}

		private static void INTERNAL_CALL_MoveRotation(Rigidbody self, ref Quaternion rot) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Sleep()
		{
			INTERNAL_CALL_Sleep(this);
		}

		private static void INTERNAL_CALL_Sleep(Rigidbody self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool IsSleeping()
		{
			return INTERNAL_CALL_IsSleeping(this);
		}

		private static bool INTERNAL_CALL_IsSleeping(Rigidbody self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void WakeUp()
		{
			INTERNAL_CALL_WakeUp(this);
		}

		private static void INTERNAL_CALL_WakeUp(Rigidbody self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool SweepTest(Vector3 direction, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return INTERNAL_CALL_SweepTest(this, ref direction, out hitInfo, maxDistance, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public bool SweepTest(Vector3 direction, out RaycastHit hitInfo, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return INTERNAL_CALL_SweepTest(this, ref direction, out hitInfo, maxDistance, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public bool SweepTest(Vector3 direction, out RaycastHit hitInfo)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			float maxDistance = float.PositiveInfinity;
			return INTERNAL_CALL_SweepTest(this, ref direction, out hitInfo, maxDistance, queryTriggerInteraction);
		}

		private static bool INTERNAL_CALL_SweepTest(Rigidbody self, ref Vector3 direction, out RaycastHit hitInfo, float maxDistance, QueryTriggerInteraction queryTriggerInteraction) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public RaycastHit[] SweepTestAll(Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return INTERNAL_CALL_SweepTestAll(this, ref direction, maxDistance, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public RaycastHit[] SweepTestAll(Vector3 direction, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return INTERNAL_CALL_SweepTestAll(this, ref direction, maxDistance, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public RaycastHit[] SweepTestAll(Vector3 direction)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			float maxDistance = float.PositiveInfinity;
			return INTERNAL_CALL_SweepTestAll(this, ref direction, maxDistance, queryTriggerInteraction);
		}

		private static RaycastHit[] INTERNAL_CALL_SweepTestAll(Rigidbody self, ref Vector3 direction, float maxDistance, QueryTriggerInteraction queryTriggerInteraction) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("use Rigidbody.maxAngularVelocity instead.")]
		public void SetMaxAngularVelocity(float a)
		{
			maxAngularVelocity = a;
		}
	}
}
