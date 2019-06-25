using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class BillboardAsset : Object
	{
		public float width
		{
			get;
			set;
		}

		public float height
		{
			get;
			set;
		}

		public float bottom
		{
			get;
			set;
		}

		public int imageCount
		{
			get;
		}

		public int vertexCount
		{
			get;
		}

		public int indexCount
		{
			get;
		}

		public Material material
		{
			get;
			set;
		}

		internal void MakeRenderMesh(Mesh mesh, float widthScale, float heightScale, float rotation) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void MakeMaterialProperties(MaterialPropertyBlock properties, Camera camera) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void MakePreviewMesh(Mesh mesh) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
