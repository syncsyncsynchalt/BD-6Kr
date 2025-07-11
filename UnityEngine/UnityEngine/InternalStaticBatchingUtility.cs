using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine;

internal class InternalStaticBatchingUtility
{
	internal class SortGO : IComparer
	{
		int IComparer.Compare(object a, object b)
		{
			if (a == b)
			{
				return 0;
			}
			Renderer renderer = GetRenderer(a as GameObject);
			Renderer renderer2 = GetRenderer(b as GameObject);
			int num = GetMaterialId(renderer).CompareTo(GetMaterialId(renderer2));
			if (num == 0)
			{
				num = GetLightmapIndex(renderer).CompareTo(GetLightmapIndex(renderer2));
			}
			return num;
		}

		private static int GetMaterialId(Renderer renderer)
		{
			if (renderer == null || renderer.sharedMaterial == null)
			{
				return 0;
			}
			return renderer.sharedMaterial.GetInstanceID();
		}

		private static int GetLightmapIndex(Renderer renderer)
		{
			if (renderer == null)
			{
				return -1;
			}
			return renderer.lightmapIndex;
		}

		private static Renderer GetRenderer(GameObject go)
		{
			if (go == null)
			{
				return null;
			}
			MeshFilter meshFilter = go.GetComponent(typeof(MeshFilter)) as MeshFilter;
			if (meshFilter == null)
			{
				return null;
			}
			return meshFilter.GetComponent<Renderer>();
		}
	}

	private const int MaxVerticesInBatch = 64000;

	private const string CombinedMeshPrefix = "Combined Mesh";

	public static void CombineRoot(GameObject staticBatchRoot)
	{
		Combine(staticBatchRoot, combineOnlyStatic: false, isEditorPostprocessScene: false);
	}

	public static void Combine(GameObject staticBatchRoot, bool combineOnlyStatic, bool isEditorPostprocessScene)
	{
		GameObject[] array = (GameObject[])Object.FindObjectsOfType(typeof(GameObject));
		List<GameObject> list = new List<GameObject>();
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if ((!(staticBatchRoot != null) || gameObject.transform.IsChildOf(staticBatchRoot.transform)) && (!combineOnlyStatic || gameObject.isStaticBatchable))
			{
				list.Add(gameObject);
			}
		}
		array = list.ToArray();
		CombineGameObjects(array, staticBatchRoot, isEditorPostprocessScene);
	}

	public static void CombineGameObjects(GameObject[] gos, GameObject staticBatchRoot, bool isEditorPostprocessScene)
	{
		Matrix4x4 matrix4x = Matrix4x4.identity;
		Transform staticBatchRootTransform = null;
		if ((bool)staticBatchRoot)
		{
			matrix4x = staticBatchRoot.transform.worldToLocalMatrix;
			staticBatchRootTransform = staticBatchRoot.transform;
		}
		int batchIndex = 0;
		int num = 0;
		List<MeshSubsetCombineUtility.MeshInstance> list = new List<MeshSubsetCombineUtility.MeshInstance>();
		List<MeshSubsetCombineUtility.SubMeshInstance> list2 = new List<MeshSubsetCombineUtility.SubMeshInstance>();
		List<GameObject> list3 = new List<GameObject>();
		Array.Sort(gos, new SortGO());
		foreach (GameObject gameObject in gos)
		{
			MeshFilter meshFilter = gameObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
			if (meshFilter == null)
			{
				continue;
			}
			Mesh sharedMesh = meshFilter.sharedMesh;
			if (sharedMesh == null || (!isEditorPostprocessScene && !sharedMesh.canAccess))
			{
				continue;
			}
			Renderer component = meshFilter.GetComponent<Renderer>();
			if (component == null || !component.enabled || component.staticBatchIndex != 0)
			{
				continue;
			}
			Material[] array = meshFilter.GetComponent<Renderer>().sharedMaterials;
			if (array.Any((Material m) => m != null && m.shader != null && m.shader.disableBatching != DisableBatchingType.False))
			{
				continue;
			}
			if (num + meshFilter.sharedMesh.vertexCount > 64000)
			{
				MakeBatch(list, list2, list3, staticBatchRootTransform, batchIndex++);
				list.Clear();
				list2.Clear();
				list3.Clear();
				num = 0;
			}
			MeshSubsetCombineUtility.MeshInstance item = new MeshSubsetCombineUtility.MeshInstance
			{
				meshInstanceID = sharedMesh.GetInstanceID(),
				rendererInstanceID = component.GetInstanceID()
			};
			MeshRenderer meshRenderer = component as MeshRenderer;
			if (meshRenderer != null && meshRenderer.additionalVertexStreams != null)
			{
				item.additionalVertexStreamsMeshInstanceID = meshRenderer.additionalVertexStreams.GetInstanceID();
			}
			item.transform = matrix4x * meshFilter.transform.localToWorldMatrix;
			item.lightmapScaleOffset = component.lightmapScaleOffset;
			item.realtimeLightmapScaleOffset = component.realtimeLightmapScaleOffset;
			list.Add(item);
			if (array.Length > sharedMesh.subMeshCount)
			{
				Debug.LogWarning("Mesh has more materials (" + array.Length + ") than subsets (" + sharedMesh.subMeshCount + ")", meshFilter.GetComponent<Renderer>());
				Material[] array2 = new Material[sharedMesh.subMeshCount];
				for (int num2 = 0; num2 < sharedMesh.subMeshCount; num2++)
				{
					array2[num2] = meshFilter.GetComponent<Renderer>().sharedMaterials[num2];
				}
				meshFilter.GetComponent<Renderer>().sharedMaterials = array2;
				array = array2;
			}
			for (int num3 = 0; num3 < Math.Min(array.Length, sharedMesh.subMeshCount); num3++)
			{
				list2.Add(new MeshSubsetCombineUtility.SubMeshInstance
				{
					meshInstanceID = meshFilter.sharedMesh.GetInstanceID(),
					vertexOffset = num,
					subMeshIndex = num3,
					gameObjectInstanceID = gameObject.GetInstanceID(),
					transform = item.transform
				});
				list3.Add(gameObject);
			}
			num += sharedMesh.vertexCount;
		}
		MakeBatch(list, list2, list3, staticBatchRootTransform, batchIndex);
	}

	private static void MakeBatch(List<MeshSubsetCombineUtility.MeshInstance> meshes, List<MeshSubsetCombineUtility.SubMeshInstance> subsets, List<GameObject> subsetGOs, Transform staticBatchRootTransform, int batchIndex)
	{
		if (meshes.Count < 2)
		{
			return;
		}
		MeshSubsetCombineUtility.MeshInstance[] meshes2 = meshes.ToArray();
		MeshSubsetCombineUtility.SubMeshInstance[] array = subsets.ToArray();
		string text = "Combined Mesh";
		text = text + " (root: " + ((!(staticBatchRootTransform != null)) ? "scene" : staticBatchRootTransform.name) + ")";
		if (batchIndex > 0)
		{
			text = text + " " + (batchIndex + 1);
		}
		Mesh mesh = StaticBatchingUtility.InternalCombineVertices(meshes2, text);
		StaticBatchingUtility.InternalCombineIndices(array, mesh);
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			MeshSubsetCombineUtility.SubMeshInstance subMeshInstance = array[i];
			GameObject gameObject = subsetGOs[i];
			Mesh sharedMesh = mesh;
			MeshFilter meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));
			meshFilter.sharedMesh = sharedMesh;
			Renderer component = gameObject.GetComponent<Renderer>();
			component.SetSubsetIndex(subMeshInstance.subMeshIndex, num);
			component.staticBatchRootTransform = staticBatchRootTransform;
			component.enabled = false;
			component.enabled = true;
			MeshRenderer meshRenderer = component as MeshRenderer;
			if (meshRenderer != null)
			{
				meshRenderer.additionalVertexStreams = null;
			}
			num++;
		}
	}
}
