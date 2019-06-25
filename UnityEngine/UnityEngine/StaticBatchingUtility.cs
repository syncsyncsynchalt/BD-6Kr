using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class StaticBatchingUtility
	{
		public static void Combine(GameObject staticBatchRoot)
		{
			InternalStaticBatchingUtility.CombineRoot(staticBatchRoot);
		}

		public static void Combine(GameObject[] gos, GameObject staticBatchRoot)
		{
			InternalStaticBatchingUtility.CombineGameObjects(gos, staticBatchRoot, isEditorPostprocessScene: false);
		}

		internal static Mesh InternalCombineVertices(MeshSubsetCombineUtility.MeshInstance[] meshes, string meshName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static void InternalCombineIndices(MeshSubsetCombineUtility.SubMeshInstance[] submeshes, [Writable] Mesh combinedMesh) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
