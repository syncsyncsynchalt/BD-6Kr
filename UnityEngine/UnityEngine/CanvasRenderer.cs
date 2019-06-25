using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class CanvasRenderer : Component
	{
		[Obsolete("isMask is no longer supported. See EnableClipping for vertex clipping configuration")]
		public bool isMask
		{
			get;
			set;
		}

		public bool hasRectClipping
		{
			get;
		}

		public bool hasPopInstruction
		{
			get;
			set;
		}

		public int materialCount
		{
			get;
			set;
		}

		public int popMaterialCount
		{
			get;
			set;
		}

		public int relativeDepth
		{
			get;
		}

		public bool cull
		{
			get;
			set;
		}

		public int absoluteDepth
		{
			get;
		}

		public bool hasMoved
		{
			get;
		}

		public void SetColor(Color color)
		{
			INTERNAL_CALL_SetColor(this, ref color);
		}

		private static void INTERNAL_CALL_SetColor(CanvasRenderer self, ref Color color) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Color GetColor() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetAlpha() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetAlpha(float alpha) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		private static void INTERNAL_CALL_EnableRectClipping(CanvasRenderer self, ref Rect rect) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void DisableRectClipping() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetMaterial(Material material, int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		public Material GetMaterial(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetPopMaterial(Material material, int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Material GetPopMaterial(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetTexture(Texture texture) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetMesh(Mesh mesh) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Clear() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SplitUIVertexStreams(List<UIVertex> verts, List<Vector3> positions, List<Color32> colors, List<Vector2> uv0S, List<Vector2> uv1S, List<Vector3> normals, List<Vector4> tangents, List<int> indicies)
		{
			SplitUIVertexStreamsInternal(verts, positions, colors, uv0S, uv1S, normals, tangents);
			SplitIndiciesStreamsInternal(verts, indicies);
		}

		private static void SplitUIVertexStreamsInternal(object verts, object positions, object colors, object uv0S, object uv1S, object normals, object tangents) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void SplitIndiciesStreamsInternal(object verts, object indicies) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void CreateUIVertexStream(List<UIVertex> verts, List<Vector3> positions, List<Color32> colors, List<Vector2> uv0S, List<Vector2> uv1S, List<Vector3> normals, List<Vector4> tangents, List<int> indicies)
		{
			CreateUIVertexStreamInternal(verts, positions, colors, uv0S, uv1S, normals, tangents, indicies);
		}

		private static void CreateUIVertexStreamInternal(object verts, object positions, object colors, object uv0S, object uv1S, object normals, object tangents, object indicies) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void AddUIVertexStream(List<UIVertex> verts, List<Vector3> positions, List<Color32> colors, List<Vector2> uv0S, List<Vector2> uv1S, List<Vector3> normals, List<Vector4> tangents)
		{
			SplitUIVertexStreamsInternal(verts, positions, colors, uv0S, uv1S, normals, tangents);
		}
	}
}
