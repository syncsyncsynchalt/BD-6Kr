using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class NavMesh
	{
		public const int AllAreas = -1;

		public static float avoidancePredictionTime
		{
			get
			{
				return GetAvoidancePredictionTime();
			}
			set
			{
				SetAvoidancePredictionTime(value);
			}
		}

		public static int pathfindingIterationsPerFrame
		{
			get
			{
				return GetPathfindingIterationsPerFrame();
			}
			set
			{
				SetPathfindingIterationsPerFrame(value);
			}
		}

		public static bool Raycast(Vector3 sourcePosition, Vector3 targetPosition, out NavMeshHit hit, int areaMask)
		{
			return INTERNAL_CALL_Raycast(ref sourcePosition, ref targetPosition, out hit, areaMask);
		}

		private static bool INTERNAL_CALL_Raycast(ref Vector3 sourcePosition, ref Vector3 targetPosition, out NavMeshHit hit, int areaMask) { throw new NotImplementedException("�Ȃɂ���"); }

		public static bool CalculatePath(Vector3 sourcePosition, Vector3 targetPosition, int areaMask, NavMeshPath path)
		{
			path.ClearCorners();
			return CalculatePathInternal(sourcePosition, targetPosition, areaMask, path);
		}

		internal static bool CalculatePathInternal(Vector3 sourcePosition, Vector3 targetPosition, int areaMask, NavMeshPath path)
		{
			return INTERNAL_CALL_CalculatePathInternal(ref sourcePosition, ref targetPosition, areaMask, path);
		}

		private static bool INTERNAL_CALL_CalculatePathInternal(ref Vector3 sourcePosition, ref Vector3 targetPosition, int areaMask, NavMeshPath path) { throw new NotImplementedException("�Ȃɂ���"); }

		public static bool FindClosestEdge(Vector3 sourcePosition, out NavMeshHit hit, int areaMask)
		{
			return INTERNAL_CALL_FindClosestEdge(ref sourcePosition, out hit, areaMask);
		}

		private static bool INTERNAL_CALL_FindClosestEdge(ref Vector3 sourcePosition, out NavMeshHit hit, int areaMask) { throw new NotImplementedException("�Ȃɂ���"); }

		public static bool SamplePosition(Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
		{
			return INTERNAL_CALL_SamplePosition(ref sourcePosition, out hit, maxDistance, areaMask);
		}

		private static bool INTERNAL_CALL_SamplePosition(ref Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask) { throw new NotImplementedException("�Ȃɂ���"); }

		[Obsolete("Use SetAreaCost instead.")]
		public static void SetLayerCost(int layer, float cost) { throw new NotImplementedException("�Ȃɂ���"); }

		[Obsolete("Use GetAreaCost instead.")]
		public static float GetLayerCost(int layer) { throw new NotImplementedException("�Ȃɂ���"); }

		[Obsolete("Use GetAreaFromName instead.")]
		public static int GetNavMeshLayerFromName(string layerName) { throw new NotImplementedException("�Ȃɂ���"); }

		public static void SetAreaCost(int areaIndex, float cost) { throw new NotImplementedException("�Ȃɂ���"); }

		public static float GetAreaCost(int areaIndex) { throw new NotImplementedException("�Ȃɂ���"); }

		public static int GetAreaFromName(string areaName) { throw new NotImplementedException("�Ȃɂ���"); }

		public static NavMeshTriangulation CalculateTriangulation()
		{
			return (NavMeshTriangulation)TriangulateInternal();
		}

		internal static object TriangulateInternal() { throw new NotImplementedException("�Ȃɂ���"); }

		[Obsolete("use NavMesh.CalculateTriangulation() instead.")]
		public static void Triangulate(out Vector3[] vertices, out int[] indices) { throw new NotImplementedException("�Ȃɂ���"); }

		[Obsolete("AddOffMeshLinks has no effect and is deprecated.")]
		public static void AddOffMeshLinks() { throw new NotImplementedException("�Ȃɂ���"); }

		[Obsolete("RestoreNavMesh has no effect and is deprecated.")]
		public static void RestoreNavMesh() { throw new NotImplementedException("�Ȃɂ���"); }

		internal static void SetAvoidancePredictionTime(float t) { throw new NotImplementedException("�Ȃɂ���"); }

		internal static float GetAvoidancePredictionTime() { throw new NotImplementedException("�Ȃɂ���"); }

		internal static void SetPathfindingIterationsPerFrame(int iter) { throw new NotImplementedException("�Ȃɂ���"); }

		internal static int GetPathfindingIterationsPerFrame() { throw new NotImplementedException("�Ȃɂ���"); }
	}
}
