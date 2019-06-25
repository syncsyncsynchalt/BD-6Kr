using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public class Physics
	{
		public const int IgnoreRaycastLayer = 4;

		[Obsolete("Please use Physics.IgnoreRaycastLayer instead. (UnityUpgradable) -> IgnoreRaycastLayer", true)]
		public const int kIgnoreRaycastLayer = 4;

		public const int DefaultRaycastLayers = -5;

		[Obsolete("Please use Physics.DefaultRaycastLayers instead. (UnityUpgradable) -> DefaultRaycastLayers", true)]
		public const int kDefaultRaycastLayers = -5;

		public const int AllLayers = -1;

		[Obsolete("Please use Physics.AllLayers instead. (UnityUpgradable) -> AllLayers", true)]
		public const int kAllLayers = -1;

		public static Vector3 gravity
		{
			get
			{
				INTERNAL_get_gravity(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_gravity(ref value);
			}
		}

		[Obsolete("use Physics.defaultContactOffset or Collider.contactOffset instead.", true)]
		public static float minPenetrationForPenalty
		{
			get;
			set;
		}

		public static float defaultContactOffset
		{
			get;
			set;
		}

		public static float bounceThreshold
		{
			get;
			set;
		}

		[Obsolete("Please use bounceThreshold instead.")]
		public static float bounceTreshold
		{
			get
			{
				return bounceThreshold;
			}
			set
			{
				bounceThreshold = value;
			}
		}

		[Obsolete("The sleepVelocity is no longer supported. Use sleepThreshold. Note that sleepThreshold is energy but not velocity.")]
		public static float sleepVelocity
		{
			get;
			set;
		}

		[Obsolete("The sleepAngularVelocity is no longer supported. Use sleepThreshold. Note that sleepThreshold is energy but not velocity.")]
		public static float sleepAngularVelocity
		{
			get;
			set;
		}

		[Obsolete("use Rigidbody.maxAngularVelocity instead.", true)]
		public static float maxAngularVelocity
		{
			get;
			set;
		}

		public static int solverIterationCount
		{
			get;
			set;
		}

		public static float sleepThreshold
		{
			get;
			set;
		}

		public static bool queriesHitTriggers
		{
			get;
			set;
		}

		[Obsolete("penetrationPenaltyForce has no effect.")]
		public static float penetrationPenaltyForce
		{
			get;
			set;
		}

		private static void INTERNAL_get_gravity(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_set_gravity(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return Raycast(origin, direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			return Raycast(origin, direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool Raycast(Vector3 origin, Vector3 direction)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			float maxDistance = float.PositiveInfinity;
			return Raycast(origin, direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		public static bool Raycast(Vector3 origin, Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Internal_RaycastTest(origin, direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return Raycast(origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			return Raycast(origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			float maxDistance = float.PositiveInfinity;
			return Raycast(origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Internal_Raycast(origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool Raycast(Ray ray, float maxDistance, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return Raycast(ray, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool Raycast(Ray ray, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			return Raycast(ray, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool Raycast(Ray ray)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			float maxDistance = float.PositiveInfinity;
			return Raycast(ray, maxDistance, layerMask, queryTriggerInteraction);
		}

		public static bool Raycast(Ray ray, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Raycast(ray.origin, ray.direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return Raycast(ray, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			return Raycast(ray, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool Raycast(Ray ray, out RaycastHit hitInfo)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			float maxDistance = float.PositiveInfinity;
			return Raycast(ray, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		public static bool Raycast(Ray ray, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static RaycastHit[] RaycastAll(Ray ray, float maxDistance, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return RaycastAll(ray, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static RaycastHit[] RaycastAll(Ray ray, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			return RaycastAll(ray, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static RaycastHit[] RaycastAll(Ray ray)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			float maxDistance = float.PositiveInfinity;
			return RaycastAll(ray, maxDistance, layerMask, queryTriggerInteraction);
		}

		public static RaycastHit[] RaycastAll(Ray ray, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return RaycastAll(ray.origin, ray.direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layermask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return INTERNAL_CALL_RaycastAll(ref origin, ref direction, maxDistance, layermask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction, float maxDistance, int layermask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return INTERNAL_CALL_RaycastAll(ref origin, ref direction, maxDistance, layermask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layermask = -5;
			return INTERNAL_CALL_RaycastAll(ref origin, ref direction, maxDistance, layermask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layermask = -5;
			float maxDistance = float.PositiveInfinity;
			return INTERNAL_CALL_RaycastAll(ref origin, ref direction, maxDistance, layermask, queryTriggerInteraction);
		}

		private static RaycastHit[] INTERNAL_CALL_RaycastAll(ref Vector3 origin, ref Vector3 direction, float maxDistance, int layermask, QueryTriggerInteraction queryTriggerInteraction) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static bool Linecast(Vector3 start, Vector3 end, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return Linecast(start, end, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool Linecast(Vector3 start, Vector3 end)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			return Linecast(start, end, layerMask, queryTriggerInteraction);
		}

		public static bool Linecast(Vector3 start, Vector3 end, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			Vector3 direction = end - start;
			return Raycast(start, direction, direction.magnitude, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hitInfo, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return Linecast(start, end, out hitInfo, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hitInfo)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			return Linecast(start, end, out hitInfo, layerMask, queryTriggerInteraction);
		}

		public static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hitInfo, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			Vector3 direction = end - start;
			return Raycast(start, direction, out hitInfo, direction.magnitude, layerMask, queryTriggerInteraction);
		}

		public static Collider[] OverlapSphere(Vector3 position, float radius, [DefaultValue("AllLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return INTERNAL_CALL_OverlapSphere(ref position, radius, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static Collider[] OverlapSphere(Vector3 position, float radius, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return INTERNAL_CALL_OverlapSphere(ref position, radius, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static Collider[] OverlapSphere(Vector3 position, float radius)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -1;
			return INTERNAL_CALL_OverlapSphere(ref position, radius, layerMask, queryTriggerInteraction);
		}

		private static Collider[] INTERNAL_CALL_OverlapSphere(ref Vector3 position, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return CapsuleCast(point1, point2, radius, direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			return CapsuleCast(point1, point2, radius, direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			float maxDistance = float.PositiveInfinity;
			return CapsuleCast(point1, point2, radius, direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			RaycastHit hitInfo;
			return Internal_CapsuleCast(point1, point2, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return CapsuleCast(point1, point2, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			return CapsuleCast(point1, point2, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			float maxDistance = float.PositiveInfinity;
			return CapsuleCast(point1, point2, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Internal_CapsuleCast(point1, point2, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return SphereCast(origin, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			return SphereCast(origin, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			float maxDistance = float.PositiveInfinity;
			return SphereCast(origin, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Internal_CapsuleCast(origin, origin, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool SphereCast(Ray ray, float radius, float maxDistance, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return SphereCast(ray, radius, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool SphereCast(Ray ray, float radius, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			return SphereCast(ray, radius, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool SphereCast(Ray ray, float radius)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			float maxDistance = float.PositiveInfinity;
			return SphereCast(ray, radius, maxDistance, layerMask, queryTriggerInteraction);
		}

		public static bool SphereCast(Ray ray, float radius, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			RaycastHit hitInfo;
			return Internal_CapsuleCast(ray.origin, ray.origin, radius, ray.direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo, float maxDistance, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return SphereCast(ray, radius, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			return SphereCast(ray, radius, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			float maxDistance = float.PositiveInfinity;
			return SphereCast(ray, radius, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Internal_CapsuleCast(ray.origin, ray.origin, radius, ray.direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		public static RaycastHit[] CapsuleCastAll(Vector3 point1, Vector3 point2, float radius, Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layermask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return INTERNAL_CALL_CapsuleCastAll(ref point1, ref point2, radius, ref direction, maxDistance, layermask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static RaycastHit[] CapsuleCastAll(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance, int layermask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return INTERNAL_CALL_CapsuleCastAll(ref point1, ref point2, radius, ref direction, maxDistance, layermask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static RaycastHit[] CapsuleCastAll(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layermask = -5;
			return INTERNAL_CALL_CapsuleCastAll(ref point1, ref point2, radius, ref direction, maxDistance, layermask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static RaycastHit[] CapsuleCastAll(Vector3 point1, Vector3 point2, float radius, Vector3 direction)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layermask = -5;
			float maxDistance = float.PositiveInfinity;
			return INTERNAL_CALL_CapsuleCastAll(ref point1, ref point2, radius, ref direction, maxDistance, layermask, queryTriggerInteraction);
		}

		private static RaycastHit[] INTERNAL_CALL_CapsuleCastAll(ref Vector3 point1, ref Vector3 point2, float radius, ref Vector3 direction, float maxDistance, int layermask, QueryTriggerInteraction queryTriggerInteraction) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static RaycastHit[] SphereCastAll(Vector3 origin, float radius, Vector3 direction, float maxDistance, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return SphereCastAll(origin, radius, direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static RaycastHit[] SphereCastAll(Vector3 origin, float radius, Vector3 direction, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			return SphereCastAll(origin, radius, direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static RaycastHit[] SphereCastAll(Vector3 origin, float radius, Vector3 direction)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			float maxDistance = float.PositiveInfinity;
			return SphereCastAll(origin, radius, direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		public static RaycastHit[] SphereCastAll(Vector3 origin, float radius, Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return CapsuleCastAll(origin, origin, radius, direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static RaycastHit[] SphereCastAll(Ray ray, float radius, float maxDistance, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return SphereCastAll(ray, radius, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static RaycastHit[] SphereCastAll(Ray ray, float radius, float maxDistance)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			return SphereCastAll(ray, radius, maxDistance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static RaycastHit[] SphereCastAll(Ray ray, float radius)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			float maxDistance = float.PositiveInfinity;
			return SphereCastAll(ray, radius, maxDistance, layerMask, queryTriggerInteraction);
		}

		public static RaycastHit[] SphereCastAll(Ray ray, float radius, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return CapsuleCastAll(ray.origin, ray.origin, radius, ray.direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		public static bool CheckSphere(Vector3 position, float radius, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return INTERNAL_CALL_CheckSphere(ref position, radius, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool CheckSphere(Vector3 position, float radius, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return INTERNAL_CALL_CheckSphere(ref position, radius, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool CheckSphere(Vector3 position, float radius)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layerMask = -5;
			return INTERNAL_CALL_CheckSphere(ref position, radius, layerMask, queryTriggerInteraction);
		}

		private static bool INTERNAL_CALL_CheckSphere(ref Vector3 position, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool CheckCapsule(Vector3 start, Vector3 end, float radius, [DefaultValue("DefaultRaycastLayers")] int layermask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return INTERNAL_CALL_CheckCapsule(ref start, ref end, radius, layermask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool CheckCapsule(Vector3 start, Vector3 end, float radius, int layermask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return INTERNAL_CALL_CheckCapsule(ref start, ref end, radius, layermask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		public static bool CheckCapsule(Vector3 start, Vector3 end, float radius)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			int layermask = -5;
			return INTERNAL_CALL_CheckCapsule(ref start, ref end, radius, layermask, queryTriggerInteraction);
		}

		private static bool INTERNAL_CALL_CheckCapsule(ref Vector3 start, ref Vector3 end, float radius, int layermask, QueryTriggerInteraction queryTriggerInteraction) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void IgnoreCollision(Collider collider1, Collider collider2, [DefaultValue("true")] bool ignore) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void IgnoreCollision(Collider collider1, Collider collider2)
		{
			bool ignore = true;
			IgnoreCollision(collider1, collider2, ignore);
		}

		public static void IgnoreLayerCollision(int layer1, int layer2, [DefaultValue("true")] bool ignore) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void IgnoreLayerCollision(int layer1, int layer2)
		{
			bool ignore = true;
			IgnoreLayerCollision(layer1, layer2, ignore);
		}

		public static bool GetIgnoreLayerCollision(int layer1, int layer2) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static bool Internal_Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layermask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return INTERNAL_CALL_Internal_Raycast(ref origin, ref direction, out hitInfo, maxDistance, layermask, queryTriggerInteraction);
		}

		private static bool INTERNAL_CALL_Internal_Raycast(ref Vector3 origin, ref Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layermask, QueryTriggerInteraction queryTriggerInteraction) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static bool Internal_CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layermask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return INTERNAL_CALL_Internal_CapsuleCast(ref point1, ref point2, radius, ref direction, out hitInfo, maxDistance, layermask, queryTriggerInteraction);
		}

		private static bool INTERNAL_CALL_Internal_CapsuleCast(ref Vector3 point1, ref Vector3 point2, float radius, ref Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layermask, QueryTriggerInteraction queryTriggerInteraction) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static bool Internal_RaycastTest(Vector3 origin, Vector3 direction, float maxDistance, int layermask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return INTERNAL_CALL_Internal_RaycastTest(ref origin, ref direction, maxDistance, layermask, queryTriggerInteraction);
		}

		private static bool INTERNAL_CALL_Internal_RaycastTest(ref Vector3 origin, ref Vector3 direction, float maxDistance, int layermask, QueryTriggerInteraction queryTriggerInteraction) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
