using System;

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class Mesh : Object
	{
		public bool isReadable
		{
			get;
		}

		internal bool canAccess
		{
			get;
		}

		public Vector3[] vertices
		{
			get;
			set;
		}

		public Vector3[] normals
		{
			get;
			set;
		}

		public Vector4[] tangents
		{
			get;
			set;
		}

		public Vector2[] uv
		{
			get;
			set;
		}

		public Vector2[] uv2
		{
			get;
			set;
		}

		public Vector2[] uv3
		{
			get;
			set;
		}

		public Vector2[] uv4
		{
			get;
			set;
		}

		public Bounds bounds
		{
			get
			{
				INTERNAL_get_bounds(out Bounds value);
				return value;
			}
			set
			{
				INTERNAL_set_bounds(ref value);
			}
		}

		public Color[] colors
		{
			get;
			set;
		}

		public Color32[] colors32
		{
			get;
			set;
		}

		public int[] triangles
		{
			get;
			set;
		}

		public int vertexCount
		{
			get;
		}

		public int subMeshCount
		{
			get;
			set;
		}

		public BoneWeight[] boneWeights
		{
			get;
			set;
		}

		public Matrix4x4[] bindposes
		{
			get;
			set;
		}

		public int blendShapeCount
		{
			get;
		}

		public Mesh()
		{
			Internal_Create(this);
		}

		private static void Internal_Create([Writable] Mesh mono) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Clear([DefaultValue("true")] bool keepVertexLayout) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void Clear()
		{
			bool keepVertexLayout = true;
			Clear(keepVertexLayout);
		}

		public void SetVertices(List<Vector3> inVertices)
		{
			SetVerticesInternal(inVertices);
		}

		private void SetVerticesInternal(object vertices) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetNormals(List<Vector3> inNormals)
		{
			SetNormalsInternal(inNormals);
		}

		private void SetNormalsInternal(object normals) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetTangents(List<Vector4> inTangents)
		{
			SetTangentsInternal(inTangents);
		}

		private void SetTangentsInternal(object tangents) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetUVs(int channel, List<Vector2> uvs)
		{
			SetUVInternal(uvs, channel, 2);
		}

		public void SetUVs(int channel, List<Vector3> uvs)
		{
			SetUVInternal(uvs, channel, 3);
		}

		public void SetUVs(int channel, List<Vector4> uvs)
		{
			SetUVInternal(uvs, channel, 4);
		}

		private void SetUVInternal(object uvs, int channel, int dim) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_bounds(out Bounds value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_bounds(ref Bounds value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetColors(List<Color> inColors)
		{
			SetColorsInternal(inColors);
		}

		private void SetColorsInternal(object colors) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetColors(List<Color32> inColors)
		{
			SetColors32Internal(inColors);
		}

		private void SetColors32Internal(object colors) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void RecalculateBounds() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void RecalculateNormals() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Optimize() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public int[] GetTriangles(int submesh) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetTriangles(int[] triangles, int submesh) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetTriangles(List<int> inTriangles, int submesh)
		{
			SetTrianglesInternal(inTriangles, submesh);
		}

		private void SetTrianglesInternal(object triangles, int submesh) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public int[] GetIndices(int submesh) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetIndices(int[] indices, MeshTopology topology, int submesh) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public MeshTopology GetTopology(int submesh) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void CombineMeshes(CombineInstance[] combine, [DefaultValue("true")] bool mergeSubMeshes, [DefaultValue("true")] bool useMatrices) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void CombineMeshes(CombineInstance[] combine, bool mergeSubMeshes)
		{
			bool useMatrices = true;
			CombineMeshes(combine, mergeSubMeshes, useMatrices);
		}

		[ExcludeFromDocs]
		public void CombineMeshes(CombineInstance[] combine)
		{
			bool useMatrices = true;
			bool mergeSubMeshes = true;
			CombineMeshes(combine, mergeSubMeshes, useMatrices);
		}

		public void MarkDynamic() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void UploadMeshData(bool markNoLogerReadable) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public string GetBlendShapeName(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public int GetBlendShapeIndex(string blendShapeName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
