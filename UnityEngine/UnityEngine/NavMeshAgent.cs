using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class NavMeshAgent : Behaviour
	{
		public Vector3 destination
		{
			get
			{
				INTERNAL_get_destination(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_destination(ref value);
			}
		}

		public float stoppingDistance
		{
			get;
			set;
		}

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

		public Vector3 nextPosition
		{
			get
			{
				INTERNAL_get_nextPosition(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_nextPosition(ref value);
			}
		}

		public Vector3 steeringTarget
		{
			get
			{
				INTERNAL_get_steeringTarget(out Vector3 value);
				return value;
			}
		}

		public Vector3 desiredVelocity
		{
			get
			{
				INTERNAL_get_desiredVelocity(out Vector3 value);
				return value;
			}
		}

		public float remainingDistance
		{
			get;
		}

		public float baseOffset
		{
			get;
			set;
		}

		public bool isOnOffMeshLink
		{
			get;
		}

		public OffMeshLinkData currentOffMeshLinkData => GetCurrentOffMeshLinkDataInternal();

		public OffMeshLinkData nextOffMeshLinkData => GetNextOffMeshLinkDataInternal();

		public bool autoTraverseOffMeshLink
		{
			get;
			set;
		}

		public bool autoBraking
		{
			get;
			set;
		}

		public bool autoRepath
		{
			get;
			set;
		}

		public bool hasPath
		{
			get;
		}

		public bool pathPending
		{
			get;
		}

		public bool isPathStale
		{
			get;
		}

		public NavMeshPathStatus pathStatus
		{
			get;
		}

		public Vector3 pathEndPosition
		{
			get
			{
				INTERNAL_get_pathEndPosition(out Vector3 value);
				return value;
			}
		}

		public NavMeshPath path
		{
			get
			{
				NavMeshPath navMeshPath = new NavMeshPath();
				CopyPathTo(navMeshPath);
				return navMeshPath;
			}
			set
			{
				if (value == null)
				{
					throw new NullReferenceException();
				}
				SetPath(value);
			}
		}

		[Obsolete("Use areaMask instead.")]
		public int walkableMask
		{
			get;
			set;
		}

		public int areaMask
		{
			get;
			set;
		}

		public float speed
		{
			get;
			set;
		}

		public float angularSpeed
		{
			get;
			set;
		}

		public float acceleration
		{
			get;
			set;
		}

		public bool updatePosition
		{
			get;
			set;
		}

		public bool updateRotation
		{
			get;
			set;
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

		public ObstacleAvoidanceType obstacleAvoidanceType
		{
			get;
			set;
		}

		public int avoidancePriority
		{
			get;
			set;
		}

		public bool isOnNavMesh
		{
			get;
		}

		public bool SetDestination(Vector3 target)
		{
			return INTERNAL_CALL_SetDestination(this, ref target);
		}

		private static bool INTERNAL_CALL_SetDestination(NavMeshAgent self, ref Vector3 target) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_destination(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_destination(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_velocity(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_velocity(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_nextPosition(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_nextPosition(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_steeringTarget(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_desiredVelocity(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void ActivateCurrentOffMeshLink(bool activated) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal OffMeshLinkData GetCurrentOffMeshLinkDataInternal() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal OffMeshLinkData GetNextOffMeshLinkDataInternal() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void CompleteOffMeshLink() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_pathEndPosition(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool Warp(Vector3 newPosition)
		{
			return INTERNAL_CALL_Warp(this, ref newPosition);
		}

		private static bool INTERNAL_CALL_Warp(NavMeshAgent self, ref Vector3 newPosition) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Move(Vector3 offset)
		{
			INTERNAL_CALL_Move(this, ref offset);
		}

		private static void INTERNAL_CALL_Move(NavMeshAgent self, ref Vector3 offset) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Stop()
		{
			StopInternal();
		}

		internal void StopInternal() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Use Stop() instead")]
		public void Stop(bool stopUpdates)
		{
			StopInternal();
		}

		public void Resume() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void ResetPath() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool SetPath(NavMeshPath path) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void CopyPathTo(NavMeshPath path) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool FindClosestEdge(out NavMeshHit hit) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool Raycast(Vector3 targetPosition, out NavMeshHit hit)
		{
			return INTERNAL_CALL_Raycast(this, ref targetPosition, out hit);
		}

		private static bool INTERNAL_CALL_Raycast(NavMeshAgent self, ref Vector3 targetPosition, out NavMeshHit hit) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool CalculatePath(Vector3 targetPosition, NavMeshPath path)
		{
			path.ClearCorners();
			return CalculatePathInternal(targetPosition, path);
		}

		private bool CalculatePathInternal(Vector3 targetPosition, NavMeshPath path)
		{
			return INTERNAL_CALL_CalculatePathInternal(this, ref targetPosition, path);
		}

		private static bool INTERNAL_CALL_CalculatePathInternal(NavMeshAgent self, ref Vector3 targetPosition, NavMeshPath path) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool SamplePathPosition(int areaMask, float maxDistance, out NavMeshHit hit) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Use SetAreaCost instead.")]
		public void SetLayerCost(int layer, float cost) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Use GetAreaCost instead.")]
		public float GetLayerCost(int layer) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetAreaCost(int areaIndex, float areaCost) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetAreaCost(int areaIndex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
