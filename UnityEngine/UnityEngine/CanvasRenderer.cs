using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine;

public sealed class CanvasRenderer : Component
{
	[Obsolete("isMask is no longer supported. See EnableClipping for vertex clipping configuration")]
	public extern bool isMask
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern bool hasRectClipping
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern bool hasPopInstruction
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int materialCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int popMaterialCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int relativeDepth
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern bool cull
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int absoluteDepth
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern bool hasMoved
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public void SetColor(Color color)
	{
		INTERNAL_CALL_SetColor(this, ref color);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_SetColor(CanvasRenderer self, ref Color color);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern Color GetColor();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern float GetAlpha();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void SetAlpha(float alpha);

	[Obsolete("UI System now uses meshes. Generate a mesh and use 'SetMesh' instead")]
	public void SetVertices(List<UIVertex> vertices)
	{
		SetVertices(vertices.ToArray(), vertices.Count);
	}

	[Obsolete("UI System now uses meshes. Generate a mesh and use 'SetMesh' instead")]
	public void SetVertices(UIVertex[] vertices, int size)
	{
		Mesh mesh = new Mesh();
		List<Vector3> list = new List<Vector3>();
		List<Color32> list2 = new List<Color32>();
		List<Vector2> list3 = new List<Vector2>();
		List<Vector2> list4 = new List<Vector2>();
		List<Vector3> list5 = new List<Vector3>();
		List<Vector4> list6 = new List<Vector4>();
		List<int> list7 = new List<int>();
		for (int i = 0; i < size; i += 4)
		{
			for (int j = 0; j < 4; j++)
			{
				list.Add(vertices[i + j].position);
				list2.Add(vertices[i + j].color);
				list3.Add(vertices[i + j].uv0);
				list4.Add(vertices[i + j].uv1);
				list5.Add(vertices[i + j].normal);
				list6.Add(vertices[i + j].tangent);
			}
			list7.Add(i);
			list7.Add(i + 1);
			list7.Add(i + 2);
			list7.Add(i + 2);
			list7.Add(i + 3);
			list7.Add(i);
		}
		mesh.SetVertices(list);
		mesh.SetColors(list2);
		mesh.SetNormals(list5);
		mesh.SetTangents(list6);
		mesh.SetUVs(0, list3);
		mesh.SetUVs(1, list4);
		mesh.SetIndices(list7.ToArray(), MeshTopology.Triangles, 0);
		SetMesh(mesh);
		Object.DestroyImmediate(mesh);
	}

	public void EnableRectClipping(Rect rect)
	{
		INTERNAL_CALL_EnableRectClipping(this, ref rect);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_EnableRectClipping(CanvasRenderer self, ref Rect rect);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void DisableRectClipping();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void SetMaterial(Material material, int index);

	public void SetMaterial(Material material, Texture texture)
	{
		materialCount = Math.Max(1, materialCount);
		SetMaterial(material, 0);
		SetTexture(texture);
	}

	public Material GetMaterial()
	{
		return GetMaterial(0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern Material GetMaterial(int index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void SetPopMaterial(Material material, int index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern Material GetPopMaterial(int index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void SetTexture(Texture texture);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void SetMesh(Mesh mesh);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void Clear();

	public static void SplitUIVertexStreams(List<UIVertex> verts, List<Vector3> positions, List<Color32> colors, List<Vector2> uv0S, List<Vector2> uv1S, List<Vector3> normals, List<Vector4> tangents, List<int> indicies)
	{
		SplitUIVertexStreamsInternal(verts, positions, colors, uv0S, uv1S, normals, tangents);
		SplitIndiciesStreamsInternal(verts, indicies);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void SplitUIVertexStreamsInternal(object verts, object positions, object colors, object uv0S, object uv1S, object normals, object tangents);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void SplitIndiciesStreamsInternal(object verts, object indicies);

	public static void CreateUIVertexStream(List<UIVertex> verts, List<Vector3> positions, List<Color32> colors, List<Vector2> uv0S, List<Vector2> uv1S, List<Vector3> normals, List<Vector4> tangents, List<int> indicies)
	{
		CreateUIVertexStreamInternal(verts, positions, colors, uv0S, uv1S, normals, tangents, indicies);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void CreateUIVertexStreamInternal(object verts, object positions, object colors, object uv0S, object uv1S, object normals, object tangents, object indicies);

	public static void AddUIVertexStream(List<UIVertex> verts, List<Vector3> positions, List<Color32> colors, List<Vector2> uv0S, List<Vector2> uv1S, List<Vector3> normals, List<Vector4> tangents)
	{
		SplitUIVertexStreamsInternal(verts, positions, colors, uv0S, uv1S, normals, tangents);
	}
}
