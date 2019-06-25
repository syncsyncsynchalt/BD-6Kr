using System;

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public class Physics2D
	{
		public const int IgnoreRaycastLayer = 4;

		public const int DefaultRaycastLayers = -5;

		public const int AllLayers = -1;

		private static List<Rigidbody2D> m_LastDisabledRigidbody2D = new List<Rigidbody2D>();

		public static int velocityIterations
		{
			get;
			set;
		}

		public static int positionIterations
		{
			get;
			set;
		}

		public static Vector2 gravity
		{
			get
			{
				INTERNAL_get_gravity(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_gravity(ref value);
			}
		}

		public static bool queriesHitTriggers
		{
			get;
			set;
		}

		public static bool queriesStartInColliders
		{
			get;
			set;
		}

		public static bool changeStopsCallbacks
		{
			get;
			set;
		}

		public static float velocityThreshold
		{
			get;
			set;
		}

		public static float maxLinearCorrection
		{
			get;
			set;
		}

		public static float maxAngularCorrection
		{
			get;
			set;
		}

		public static float maxTranslationSpeed
		{
			get;
			set;
		}

		public static float maxRotationSpeed
		{
			get;
			set;
		}

		public static float minPenetrationForPenalty
		{
			get;
			set;
		}

		public static float baumgarteScale
		{
			get;
			set;
		}

		public static float baumgarteTOIScale
		{
			get;
			set;
		}

		public static float timeToSleep
		{
			get;
			set;
		}

		public static float linearSleepTolerance
		{
			get;
			set;
		}

		public static float angularSleepTolerance
		{
			get;
			set;
		}

		private static void INTERNAL_get_gravity(out Vector2 value) { throw new NotImplementedException("なにこれ"); }

		private static void INTERNAL_set_gravity(ref Vector2 value) { throw new NotImplementedException("なにこれ"); }

		public static void IgnoreCollision(Collider2D collider1, Collider2D collider2, [DefaultValue("true")] bool ignore) { throw new NotImplementedException("なにこれ"); }

		[ExcludeFromDocs]
		public static void IgnoreCollision(Collider2D collider1, Collider2D collider2)
		{
			bool ignore = true;
			IgnoreCollision(collider1, collider2, ignore);
		}

		public static bool GetIgnoreCollision(Collider2D collider1, Collider2D collider2) { throw new NotImplementedException("なにこれ"); }

		public static void IgnoreLayerCollision(int layer1, int layer2, [DefaultValue("true")] bool ignore) { throw new NotImplementedException("なにこれ"); }

		[ExcludeFromDocs]
		public static void IgnoreLayerCollision(int layer1, int layer2)
		{
			bool ignore = true;
			IgnoreLayerCollision(layer1, layer2, ignore);
		}

		public static bool GetIgnoreLayerCollision(int layer1, int layer2) { throw new NotImplementedException("なにこれ"); }

		public static bool IsTouching(Collider2D collider1, Collider2D collider2) { throw new NotImplementedException("なにこれ"); }

		public static bool IsTouchingLayers(Collider2D collider, [DefaultValue("AllLayers")] int layerMask) { throw new NotImplementedException("なにこれ"); }

		[ExcludeFromDocs]
		public static bool IsTouchingLayers(Collider2D collider)
		{
			int layerMask = -1;
			return IsTouchingLayers(collider, layerMask);
		}

		internal static void SetEditorDragMovement(bool dragging, GameObject[] objs)
		{
			foreach (Rigidbody2D item in m_LastDisabledRigidbody2D)
			{
				item.isKinematic = false;
			}
			m_LastDisabledRigidbody2D.Clear();
			if (!dragging)
			{
				return;
			}
			foreach (GameObject gameObject in objs)
			{
				Rigidbody2D[] componentsInChildren = gameObject.GetComponentsInChildren<Rigidbody2D>(includeInactive: false);
				Rigidbody2D[] array = componentsInChildren;
				foreach (Rigidbody2D rigidbody2D in array)
				{
					if (!rigidbody2D.isKinematic)
					{
						rigidbody2D.isKinematic = true;
						m_LastDisabledRigidbody2D.Add(rigidbody2D);
					}
				}
			}
		}

		private static void Internal_Linecast(Vector2 start, Vector2 end, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit)
		{
			INTERNAL_CALL_Internal_Linecast(ref start, ref end, layerMask, minDepth, maxDepth, out raycastHit);
		}

		private static void INTERNAL_CALL_Internal_Linecast(ref Vector2 start, ref Vector2 end, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit) { throw new NotImplementedException("なにこれ"); }

		[ExcludeFromDocs]
		public static RaycastHit2D Linecast(Vector2 start, Vector2 end, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return Linecast(start, end, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Linecast(Vector2 start, Vector2 end, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return Linecast(start, end, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Linecast(Vector2 start, Vector2 end)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return Linecast(start, end, layerMask, minDepth, maxDepth);
		}

		public static RaycastHit2D Linecast(Vector2 start, Vector2 end, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			Internal_Linecast(start, end, layerMask, minDepth, maxDepth, out RaycastHit2D raycastHit);
			return raycastHit;
		}

		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_LinecastAll(ref start, ref end, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_LinecastAll(ref start, ref end, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_LinecastAll(ref start, ref end, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_LinecastAll(ref start, ref end, layerMask, minDepth, maxDepth);
		}

		private static RaycastHit2D[] INTERNAL_CALL_LinecastAll(ref Vector2 start, ref Vector2 end, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_LinecastNonAlloc(ref start, ref end, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_LinecastNonAlloc(ref start, ref end, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_LinecastNonAlloc(ref start, ref end, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_LinecastNonAlloc(ref start, ref end, results, layerMask, minDepth, maxDepth);
		}

		private static int INTERNAL_CALL_LinecastNonAlloc(ref Vector2 start, ref Vector2 end, RaycastHit2D[] results, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		private static void Internal_Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit)
		{
			INTERNAL_CALL_Internal_Raycast(ref origin, ref direction, distance, layerMask, minDepth, maxDepth, out raycastHit);
		}

		private static void INTERNAL_CALL_Internal_Raycast(ref Vector2 origin, ref Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit) { throw new NotImplementedException("なにこれ"); }

		[ExcludeFromDocs]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return Raycast(origin, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return Raycast(origin, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return Raycast(origin, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return Raycast(origin, direction, distance, layerMask, minDepth, maxDepth);
		}

		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			Internal_Raycast(origin, direction, distance, layerMask, minDepth, maxDepth, out RaycastHit2D raycastHit);
			return raycastHit;
		}

		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_RaycastAll(ref origin, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_RaycastAll(ref origin, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_RaycastAll(ref origin, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_RaycastAll(ref origin, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return INTERNAL_CALL_RaycastAll(ref origin, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		private static RaycastHit2D[] INTERNAL_CALL_RaycastAll(ref Vector2 origin, ref Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_RaycastNonAlloc(ref origin, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_RaycastNonAlloc(ref origin, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_RaycastNonAlloc(ref origin, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_RaycastNonAlloc(ref origin, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return INTERNAL_CALL_RaycastNonAlloc(ref origin, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		private static int INTERNAL_CALL_RaycastNonAlloc(ref Vector2 origin, ref Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		private static void Internal_CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit)
		{
			INTERNAL_CALL_Internal_CircleCast(ref origin, radius, ref direction, distance, layerMask, minDepth, maxDepth, out raycastHit);
		}

		private static void INTERNAL_CALL_Internal_CircleCast(ref Vector2 origin, float radius, ref Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit) { throw new NotImplementedException("なにこれ"); }

		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return CircleCast(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return CircleCast(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return CircleCast(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return CircleCast(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
		}

		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			Internal_CircleCast(origin, radius, direction, distance, layerMask, minDepth, maxDepth, out RaycastHit2D raycastHit);
			return raycastHit;
		}

		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_CircleCastAll(ref origin, radius, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_CircleCastAll(ref origin, radius, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_CircleCastAll(ref origin, radius, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_CircleCastAll(ref origin, radius, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return INTERNAL_CALL_CircleCastAll(ref origin, radius, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		private static RaycastHit2D[] INTERNAL_CALL_CircleCastAll(ref Vector2 origin, float radius, ref Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_CircleCastNonAlloc(ref origin, radius, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_CircleCastNonAlloc(ref origin, radius, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_CircleCastNonAlloc(ref origin, radius, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_CircleCastNonAlloc(ref origin, radius, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return INTERNAL_CALL_CircleCastNonAlloc(ref origin, radius, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		private static int INTERNAL_CALL_CircleCastNonAlloc(ref Vector2 origin, float radius, ref Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		private static void Internal_BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit)
		{
			INTERNAL_CALL_Internal_BoxCast(ref origin, ref size, angle, ref direction, distance, layerMask, minDepth, maxDepth, out raycastHit);
		}

		private static void INTERNAL_CALL_Internal_BoxCast(ref Vector2 origin, ref Vector2 size, float angle, ref Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit) { throw new NotImplementedException("なにこれ"); }

		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return BoxCast(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return BoxCast(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return BoxCast(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return BoxCast(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			Internal_BoxCast(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth, out RaycastHit2D raycastHit);
			return raycastHit;
		}

		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_BoxCastAll(ref origin, ref size, angle, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_BoxCastAll(ref origin, ref size, angle, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_BoxCastAll(ref origin, ref size, angle, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_BoxCastAll(ref origin, ref size, angle, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return INTERNAL_CALL_BoxCastAll(ref origin, ref size, angle, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		private static RaycastHit2D[] INTERNAL_CALL_BoxCastAll(ref Vector2 origin, ref Vector2 size, float angle, ref Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_BoxCastNonAlloc(ref origin, ref size, angle, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_BoxCastNonAlloc(ref origin, ref size, angle, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_BoxCastNonAlloc(ref origin, ref size, angle, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_BoxCastNonAlloc(ref origin, ref size, angle, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return INTERNAL_CALL_BoxCastNonAlloc(ref origin, ref size, angle, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		private static int INTERNAL_CALL_BoxCastNonAlloc(ref Vector2 origin, ref Vector2 size, float angle, ref Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		private static void Internal_GetRayIntersection(Ray ray, float distance, int layerMask, out RaycastHit2D raycastHit)
		{
			INTERNAL_CALL_Internal_GetRayIntersection(ref ray, distance, layerMask, out raycastHit);
		}

		private static void INTERNAL_CALL_Internal_GetRayIntersection(ref Ray ray, float distance, int layerMask, out RaycastHit2D raycastHit) { throw new NotImplementedException("なにこれ"); }

		[ExcludeFromDocs]
		public static RaycastHit2D GetRayIntersection(Ray ray, float distance)
		{
			int layerMask = -5;
			return GetRayIntersection(ray, distance, layerMask);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D GetRayIntersection(Ray ray)
		{
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return GetRayIntersection(ray, distance, layerMask);
		}

		public static RaycastHit2D GetRayIntersection(Ray ray, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
		{
			Internal_GetRayIntersection(ray, distance, layerMask, out RaycastHit2D raycastHit);
			return raycastHit;
		}

		public static RaycastHit2D[] GetRayIntersectionAll(Ray ray, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
		{
			return INTERNAL_CALL_GetRayIntersectionAll(ref ray, distance, layerMask);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] GetRayIntersectionAll(Ray ray, float distance)
		{
			int layerMask = -5;
			return INTERNAL_CALL_GetRayIntersectionAll(ref ray, distance, layerMask);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] GetRayIntersectionAll(Ray ray)
		{
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return INTERNAL_CALL_GetRayIntersectionAll(ref ray, distance, layerMask);
		}

		private static RaycastHit2D[] INTERNAL_CALL_GetRayIntersectionAll(ref Ray ray, float distance, int layerMask) { throw new NotImplementedException("なにこれ"); }

		public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
		{
			return INTERNAL_CALL_GetRayIntersectionNonAlloc(ref ray, results, distance, layerMask);
		}

		[ExcludeFromDocs]
		public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results, float distance)
		{
			int layerMask = -5;
			return INTERNAL_CALL_GetRayIntersectionNonAlloc(ref ray, results, distance, layerMask);
		}

		[ExcludeFromDocs]
		public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results)
		{
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return INTERNAL_CALL_GetRayIntersectionNonAlloc(ref ray, results, distance, layerMask);
		}

		private static int INTERNAL_CALL_GetRayIntersectionNonAlloc(ref Ray ray, RaycastHit2D[] results, float distance, int layerMask) { throw new NotImplementedException("なにこれ"); }

		public static Collider2D OverlapPoint(Vector2 point, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_OverlapPoint(ref point, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapPoint(Vector2 point, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_OverlapPoint(ref point, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapPoint(Vector2 point, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_OverlapPoint(ref point, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapPoint(Vector2 point)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_OverlapPoint(ref point, layerMask, minDepth, maxDepth);
		}

		private static Collider2D INTERNAL_CALL_OverlapPoint(ref Vector2 point, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		public static Collider2D[] OverlapPointAll(Vector2 point, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_OverlapPointAll(ref point, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapPointAll(Vector2 point, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_OverlapPointAll(ref point, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapPointAll(Vector2 point, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_OverlapPointAll(ref point, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapPointAll(Vector2 point)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_OverlapPointAll(ref point, layerMask, minDepth, maxDepth);
		}

		private static Collider2D[] INTERNAL_CALL_OverlapPointAll(ref Vector2 point, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_OverlapPointNonAlloc(ref point, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_OverlapPointNonAlloc(ref point, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_OverlapPointNonAlloc(ref point, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_OverlapPointNonAlloc(ref point, results, layerMask, minDepth, maxDepth);
		}

		private static int INTERNAL_CALL_OverlapPointNonAlloc(ref Vector2 point, Collider2D[] results, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		public static Collider2D OverlapCircle(Vector2 point, float radius, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_OverlapCircle(ref point, radius, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapCircle(Vector2 point, float radius, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_OverlapCircle(ref point, radius, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapCircle(Vector2 point, float radius, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_OverlapCircle(ref point, radius, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapCircle(Vector2 point, float radius)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_OverlapCircle(ref point, radius, layerMask, minDepth, maxDepth);
		}

		private static Collider2D INTERNAL_CALL_OverlapCircle(ref Vector2 point, float radius, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_OverlapCircleAll(ref point, radius, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_OverlapCircleAll(ref point, radius, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_OverlapCircleAll(ref point, radius, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_OverlapCircleAll(ref point, radius, layerMask, minDepth, maxDepth);
		}

		private static Collider2D[] INTERNAL_CALL_OverlapCircleAll(ref Vector2 point, float radius, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_OverlapCircleNonAlloc(ref point, radius, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_OverlapCircleNonAlloc(ref point, radius, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_OverlapCircleNonAlloc(ref point, radius, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_OverlapCircleNonAlloc(ref point, radius, results, layerMask, minDepth, maxDepth);
		}

		private static int INTERNAL_CALL_OverlapCircleNonAlloc(ref Vector2 point, float radius, Collider2D[] results, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_OverlapArea(ref pointA, ref pointB, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_OverlapArea(ref pointA, ref pointB, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_OverlapArea(ref pointA, ref pointB, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_OverlapArea(ref pointA, ref pointB, layerMask, minDepth, maxDepth);
		}

		private static Collider2D INTERNAL_CALL_OverlapArea(ref Vector2 pointA, ref Vector2 pointB, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_OverlapAreaAll(ref pointA, ref pointB, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_OverlapAreaAll(ref pointA, ref pointB, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_OverlapAreaAll(ref pointA, ref pointB, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_OverlapAreaAll(ref pointA, ref pointB, layerMask, minDepth, maxDepth);
		}

		private static Collider2D[] INTERNAL_CALL_OverlapAreaAll(ref Vector2 pointA, ref Vector2 pointB, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }

		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return INTERNAL_CALL_OverlapAreaNonAlloc(ref pointA, ref pointB, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return INTERNAL_CALL_OverlapAreaNonAlloc(ref pointA, ref pointB, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return INTERNAL_CALL_OverlapAreaNonAlloc(ref pointA, ref pointB, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return INTERNAL_CALL_OverlapAreaNonAlloc(ref pointA, ref pointB, results, layerMask, minDepth, maxDepth);
		}

		private static int INTERNAL_CALL_OverlapAreaNonAlloc(ref Vector2 pointA, ref Vector2 pointB, Collider2D[] results, int layerMask, float minDepth, float maxDepth) { throw new NotImplementedException("なにこれ"); }
	}
}
