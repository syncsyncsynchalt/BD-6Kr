using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine;

public sealed class Mesh : Object
{
	public extern bool isReadable
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	internal extern bool canAccess
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern Vector3[] vertices
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern Vector3[] normals
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern Vector4[] tangents
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern Vector2[] uv
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern Vector2[] uv2
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern Vector2[] uv3
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern Vector2[] uv4
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public Bounds bounds
	{
		get
		{
			INTERNAL_get_bounds(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_bounds(ref value);
		}
	}

	public extern Color[] colors
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern Color32[] colors32
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int[] triangles
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int vertexCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern int subMeshCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern BoneWeight[] boneWeights
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern Matrix4x4[] bindposes
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int blendShapeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public Mesh()
	{
		Internal_Create(this);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void Internal_Create([Writable] Mesh mono);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void Clear([DefaultValue("true")] bool keepVertexLayout);

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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetVerticesInternal(object vertices);

	public void SetNormals(List<Vector3> inNormals)
	{
		SetNormalsInternal(inNormals);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetNormalsInternal(object normals);

	public void SetTangents(List<Vector4> inTangents)
	{
		SetTangentsInternal(inTangents);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetTangentsInternal(object tangents);

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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetUVInternal(object uvs, int channel, int dim);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_bounds(out Bounds value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_bounds(ref Bounds value);

	public void SetColors(List<Color> inColors)
	{
		SetColorsInternal(inColors);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetColorsInternal(object colors);

	public void SetColors(List<Color32> inColors)
	{
		SetColors32Internal(inColors);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetColors32Internal(object colors);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void RecalculateBounds();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void RecalculateNormals();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void Optimize();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern int[] GetTriangles(int submesh);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void SetTriangles(int[] triangles, int submesh);

	public void SetTriangles(List<int> inTriangles, int submesh)
	{
		SetTrianglesInternal(inTriangles, submesh);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetTrianglesInternal(object triangles, int submesh);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern int[] GetIndices(int submesh);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void SetIndices(int[] indices, MeshTopology topology, int submesh);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern MeshTopology GetTopology(int submesh);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void CombineMeshes(CombineInstance[] combine, [DefaultValue("true")] bool mergeSubMeshes, [DefaultValue("true")] bool useMatrices);

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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void MarkDynamic();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void UploadMeshData(bool markNoLogerReadable);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern string GetBlendShapeName(int index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern int GetBlendShapeIndex(string blendShapeName);
}
