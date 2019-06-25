using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class GeometryUtility
	{
		public static Plane[] CalculateFrustumPlanes(Camera camera)
		{
			return CalculateFrustumPlanes(camera.projectionMatrix * camera.worldToCameraMatrix);
		}

		public static Plane[] CalculateFrustumPlanes(Matrix4x4 worldToProjectionMatrix)
		{
			Plane[] array = new Plane[6];
			Internal_ExtractPlanes(array, worldToProjectionMatrix);
			return array;
		}

		private static void Internal_ExtractPlanes(Plane[] planes, Matrix4x4 worldToProjectionMatrix)
		{
			INTERNAL_CALL_Internal_ExtractPlanes(planes, ref worldToProjectionMatrix);
		}

		private static void INTERNAL_CALL_Internal_ExtractPlanes(Plane[] planes, ref Matrix4x4 worldToProjectionMatrix) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool TestPlanesAABB(Plane[] planes, Bounds bounds)
		{
			return INTERNAL_CALL_TestPlanesAABB(planes, ref bounds);
		}

		private static bool INTERNAL_CALL_TestPlanesAABB(Plane[] planes, ref Bounds bounds) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
