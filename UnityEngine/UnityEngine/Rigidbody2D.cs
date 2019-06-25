using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class Rigidbody2D : Component
	{
		public Vector2 position
		{
			get
			{
				INTERNAL_get_position(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_position(ref value);
			}
		}

		public float rotation
		{
			get;
			set;
		}

		public Vector2 velocity
		{
			get
			{
				INTERNAL_get_velocity(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_velocity(ref value);
			}
		}

		public float angularVelocity
		{
			get;
			set;
		}

		public float mass
		{
			get;
			set;
		}

		public Vector2 centerOfMass
		{
			get
			{
				INTERNAL_get_centerOfMass(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_centerOfMass(ref value);
			}
		}

		public Vector2 worldCenterOfMass
		{
			get
			{
				INTERNAL_get_worldCenterOfMass(out Vector2 value);
				return value;
			}
		}

		public float inertia
		{
			get;
			set;
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

		public float gravityScale
		{
			get;
			set;
		}

		public bool isKinematic
		{
			get;
			set;
		}

		[Obsolete("The fixedAngle is no longer supported. Use constraints instead.")]
		public bool fixedAngle
		{
			get;
			set;
		}

		public bool freezeRotation
		{
			get;
			set;
		}

		public RigidbodyConstraints2D constraints
		{
			get;
			set;
		}

		public bool simulated
		{
			get;
			set;
		}

		public RigidbodyInterpolation2D interpolation
		{
			get;
			set;
		}

		public RigidbodySleepMode2D sleepMode
		{
			get;
			set;
		}

		public CollisionDetectionMode2D collisionDetectionMode
		{
			get;
			set;
		}

		private void INTERNAL_get_position(out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_position(ref Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void MovePosition(Vector2 position)
		{
			INTERNAL_CALL_MovePosition(this, ref position);
		}

		private static void INTERNAL_CALL_MovePosition(Rigidbody2D self, ref Vector2 position) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void MoveRotation(float angle)
		{
			INTERNAL_CALL_MoveRotation(this, angle);
		}

		private static void INTERNAL_CALL_MoveRotation(Rigidbody2D self, float angle) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_velocity(out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_velocity(ref Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_centerOfMass(out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_centerOfMass(ref Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_worldCenterOfMass(out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool IsSleeping() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool IsAwake() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Sleep() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void WakeUp() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool IsTouching(Collider2D collider) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool IsTouchingLayers([DefaultValue("Physics2D.AllLayers")] int layerMask) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public bool IsTouchingLayers()
		{
			int layerMask = -1;
			return IsTouchingLayers(layerMask);
		}

		public void AddForce(Vector2 force, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode)
		{
			INTERNAL_CALL_AddForce(this, ref force, mode);
		}

		[ExcludeFromDocs]
		public void AddForce(Vector2 force)
		{
			ForceMode2D mode = ForceMode2D.Force;
			INTERNAL_CALL_AddForce(this, ref force, mode);
		}

		private static void INTERNAL_CALL_AddForce(Rigidbody2D self, ref Vector2 force, ForceMode2D mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void AddRelativeForce(Vector2 relativeForce, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode)
		{
			INTERNAL_CALL_AddRelativeForce(this, ref relativeForce, mode);
		}

		[ExcludeFromDocs]
		public void AddRelativeForce(Vector2 relativeForce)
		{
			ForceMode2D mode = ForceMode2D.Force;
			INTERNAL_CALL_AddRelativeForce(this, ref relativeForce, mode);
		}

		private static void INTERNAL_CALL_AddRelativeForce(Rigidbody2D self, ref Vector2 relativeForce, ForceMode2D mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void AddForceAtPosition(Vector2 force, Vector2 position, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode)
		{
			INTERNAL_CALL_AddForceAtPosition(this, ref force, ref position, mode);
		}

		[ExcludeFromDocs]
		public void AddForceAtPosition(Vector2 force, Vector2 position)
		{
			ForceMode2D mode = ForceMode2D.Force;
			INTERNAL_CALL_AddForceAtPosition(this, ref force, ref position, mode);
		}

		private static void INTERNAL_CALL_AddForceAtPosition(Rigidbody2D self, ref Vector2 force, ref Vector2 position, ForceMode2D mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void AddTorque(float torque, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void AddTorque(float torque)
		{
			ForceMode2D mode = ForceMode2D.Force;
			AddTorque(torque, mode);
		}

		public Vector2 GetPoint(Vector2 point)
		{
			Rigidbody2D_CUSTOM_INTERNAL_GetPoint(this, point, out Vector2 value);
			return value;
		}

		private static void Rigidbody2D_CUSTOM_INTERNAL_GetPoint(Rigidbody2D rigidbody, Vector2 point, out Vector2 value)
		{
			INTERNAL_CALL_Rigidbody2D_CUSTOM_INTERNAL_GetPoint(rigidbody, ref point, out value);
		}

		private static void INTERNAL_CALL_Rigidbody2D_CUSTOM_INTERNAL_GetPoint(Rigidbody2D rigidbody, ref Vector2 point, out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector2 GetRelativePoint(Vector2 relativePoint)
		{
			Rigidbody2D_CUSTOM_INTERNAL_GetRelativePoint(this, relativePoint, out Vector2 value);
			return value;
		}

		private static void Rigidbody2D_CUSTOM_INTERNAL_GetRelativePoint(Rigidbody2D rigidbody, Vector2 relativePoint, out Vector2 value)
		{
			INTERNAL_CALL_Rigidbody2D_CUSTOM_INTERNAL_GetRelativePoint(rigidbody, ref relativePoint, out value);
		}

		private static void INTERNAL_CALL_Rigidbody2D_CUSTOM_INTERNAL_GetRelativePoint(Rigidbody2D rigidbody, ref Vector2 relativePoint, out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector2 GetVector(Vector2 vector)
		{
			Rigidbody2D_CUSTOM_INTERNAL_GetVector(this, vector, out Vector2 value);
			return value;
		}

		private static void Rigidbody2D_CUSTOM_INTERNAL_GetVector(Rigidbody2D rigidbody, Vector2 vector, out Vector2 value)
		{
			INTERNAL_CALL_Rigidbody2D_CUSTOM_INTERNAL_GetVector(rigidbody, ref vector, out value);
		}

		private static void INTERNAL_CALL_Rigidbody2D_CUSTOM_INTERNAL_GetVector(Rigidbody2D rigidbody, ref Vector2 vector, out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector2 GetRelativeVector(Vector2 relativeVector)
		{
			Rigidbody2D_CUSTOM_INTERNAL_GetRelativeVector(this, relativeVector, out Vector2 value);
			return value;
		}

		private static void Rigidbody2D_CUSTOM_INTERNAL_GetRelativeVector(Rigidbody2D rigidbody, Vector2 relativeVector, out Vector2 value)
		{
			INTERNAL_CALL_Rigidbody2D_CUSTOM_INTERNAL_GetRelativeVector(rigidbody, ref relativeVector, out value);
		}

		private static void INTERNAL_CALL_Rigidbody2D_CUSTOM_INTERNAL_GetRelativeVector(Rigidbody2D rigidbody, ref Vector2 relativeVector, out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector2 GetPointVelocity(Vector2 point)
		{
			Rigidbody2D_CUSTOM_INTERNAL_GetPointVelocity(this, point, out Vector2 value);
			return value;
		}

		private static void Rigidbody2D_CUSTOM_INTERNAL_GetPointVelocity(Rigidbody2D rigidbody, Vector2 point, out Vector2 value)
		{
			INTERNAL_CALL_Rigidbody2D_CUSTOM_INTERNAL_GetPointVelocity(rigidbody, ref point, out value);
		}

		private static void INTERNAL_CALL_Rigidbody2D_CUSTOM_INTERNAL_GetPointVelocity(Rigidbody2D rigidbody, ref Vector2 point, out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector2 GetRelativePointVelocity(Vector2 relativePoint)
		{
			Rigidbody2D_CUSTOM_INTERNAL_GetRelativePointVelocity(this, relativePoint, out Vector2 value);
			return value;
		}

		private static void Rigidbody2D_CUSTOM_INTERNAL_GetRelativePointVelocity(Rigidbody2D rigidbody, Vector2 relativePoint, out Vector2 value)
		{
			INTERNAL_CALL_Rigidbody2D_CUSTOM_INTERNAL_GetRelativePointVelocity(rigidbody, ref relativePoint, out value);
		}

		private static void INTERNAL_CALL_Rigidbody2D_CUSTOM_INTERNAL_GetRelativePointVelocity(Rigidbody2D rigidbody, ref Vector2 relativePoint, out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
