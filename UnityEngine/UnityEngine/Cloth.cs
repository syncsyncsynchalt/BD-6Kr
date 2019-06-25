using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class Cloth : Component
	{
		public float sleepThreshold
		{
			get;
			set;
		}

		public float bendingStiffness
		{
			get;
			set;
		}

		public float stretchingStiffness
		{
			get;
			set;
		}

		public float damping
		{
			get;
			set;
		}

		public Vector3 externalAcceleration
		{
			get
			{
				INTERNAL_get_externalAcceleration(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_externalAcceleration(ref value);
			}
		}

		public Vector3 randomAcceleration
		{
			get
			{
				INTERNAL_get_randomAcceleration(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_randomAcceleration(ref value);
			}
		}

		public bool useGravity
		{
			get;
			set;
		}

		[Obsolete("Deprecated. Cloth.selfCollisions is no longer supported since Unity 5.0.", true)]
		public bool selfCollision
		{
			get;
			set;
		}

		public bool enabled
		{
			get;
			set;
		}

		public Vector3[] vertices
		{
			get;
		}

		public Vector3[] normals
		{
			get;
		}

		public float friction
		{
			get;
			set;
		}

		public float collisionMassScale
		{
			get;
			set;
		}

		public float useContinuousCollision
		{
			get;
			set;
		}

		public float useVirtualParticles
		{
			get;
			set;
		}

		public ClothSkinningCoefficient[] coefficients
		{
			get;
			set;
		}

		public float worldVelocityScale
		{
			get;
			set;
		}

		public float worldAccelerationScale
		{
			get;
			set;
		}

		public bool solverFrequency
		{
			get;
			set;
		}

		public CapsuleCollider[] capsuleColliders
		{
			get;
			set;
		}

		public ClothSphereColliderPair[] sphereColliders
		{
			get;
			set;
		}

		private void INTERNAL_get_externalAcceleration(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_externalAcceleration(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_randomAcceleration(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_randomAcceleration(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void ClearTransformMotion()
		{
			INTERNAL_CALL_ClearTransformMotion(this);
		}

		private static void INTERNAL_CALL_ClearTransformMotion(Cloth self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetEnabledFading(bool enabled, [DefaultValue("0.5f")] float interpolationTime) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void SetEnabledFading(bool enabled)
		{
			float interpolationTime = 0.5f;
			SetEnabledFading(enabled, interpolationTime);
		}
	}
}
