using System;
using System.Runtime.CompilerServices;

namespace UnityEngine;

public sealed class NavMeshAgent : Behaviour
{
	public Vector3 destination
	{
		get
		{
			INTERNAL_get_destination(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_destination(ref value);
		}
	}

	public extern float stoppingDistance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public Vector3 velocity
	{
		get
		{
			INTERNAL_get_velocity(out var value);
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
			INTERNAL_get_nextPosition(out var value);
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
			INTERNAL_get_steeringTarget(out var value);
			return value;
		}
	}

	public Vector3 desiredVelocity
	{
		get
		{
			INTERNAL_get_desiredVelocity(out var value);
			return value;
		}
	}

	public extern float remainingDistance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern float baseOffset
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern bool isOnOffMeshLink
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public OffMeshLinkData currentOffMeshLinkData => GetCurrentOffMeshLinkDataInternal();

	public OffMeshLinkData nextOffMeshLinkData => GetNextOffMeshLinkDataInternal();

	public extern bool autoTraverseOffMeshLink
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern bool autoBraking
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern bool autoRepath
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern bool hasPath
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern bool pathPending
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern bool isPathStale
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern NavMeshPathStatus pathStatus
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public Vector3 pathEndPosition
	{
		get
		{
			INTERNAL_get_pathEndPosition(out var value);
			return value;
		}
	}

	public NavMeshPath path
	{
		get
		{
			NavMeshPath result = new NavMeshPath();
			CopyPathTo(result);
			return result;
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
	public extern int walkableMask
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int areaMask
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float speed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float angularSpeed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float acceleration
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern bool updatePosition
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern bool updateRotation
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float radius
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float height
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern ObstacleAvoidanceType obstacleAvoidanceType
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int avoidancePriority
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern bool isOnNavMesh
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public bool SetDestination(Vector3 target)
	{
		return INTERNAL_CALL_SetDestination(this, ref target);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern bool INTERNAL_CALL_SetDestination(NavMeshAgent self, ref Vector3 target);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_destination(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_destination(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_velocity(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_velocity(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_nextPosition(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_nextPosition(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_steeringTarget(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_desiredVelocity(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void ActivateCurrentOffMeshLink(bool activated);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern OffMeshLinkData GetCurrentOffMeshLinkDataInternal();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern OffMeshLinkData GetNextOffMeshLinkDataInternal();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void CompleteOffMeshLink();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_pathEndPosition(out Vector3 value);

	public bool Warp(Vector3 newPosition)
	{
		return INTERNAL_CALL_Warp(this, ref newPosition);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern bool INTERNAL_CALL_Warp(NavMeshAgent self, ref Vector3 newPosition);

	public void Move(Vector3 offset)
	{
		INTERNAL_CALL_Move(this, ref offset);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_Move(NavMeshAgent self, ref Vector3 offset);

	public void Stop()
	{
		StopInternal();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void StopInternal();

	[Obsolete("Use Stop() instead")]
	public void Stop(bool stopUpdates)
	{
		StopInternal();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void Resume();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void ResetPath();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern bool SetPath(NavMeshPath path);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void CopyPathTo(NavMeshPath path);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern bool FindClosestEdge(out NavMeshHit hit);

	public bool Raycast(Vector3 targetPosition, out NavMeshHit hit)
	{
		return INTERNAL_CALL_Raycast(this, ref targetPosition, out hit);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern bool INTERNAL_CALL_Raycast(NavMeshAgent self, ref Vector3 targetPosition, out NavMeshHit hit);

	public bool CalculatePath(Vector3 targetPosition, NavMeshPath path)
	{
		path.ClearCorners();
		return CalculatePathInternal(targetPosition, path);
	}

	private bool CalculatePathInternal(Vector3 targetPosition, NavMeshPath path)
	{
		return INTERNAL_CALL_CalculatePathInternal(this, ref targetPosition, path);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern bool INTERNAL_CALL_CalculatePathInternal(NavMeshAgent self, ref Vector3 targetPosition, NavMeshPath path);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern bool SamplePathPosition(int areaMask, float maxDistance, out NavMeshHit hit);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	[Obsolete("Use SetAreaCost instead.")]
	public extern void SetLayerCost(int layer, float cost);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[Obsolete("Use GetAreaCost instead.")]
	[WrapperlessIcall]
	public extern float GetLayerCost(int layer);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void SetAreaCost(int areaIndex, float areaCost);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern float GetAreaCost(int areaIndex);
}
