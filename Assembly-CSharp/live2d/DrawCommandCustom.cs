using UnityEngine;

namespace live2d
{
	internal class DrawCommandCustom
	{
		public string name;

		public CombineInstance combine;

		public int opacity;

		public Color color;

		public Vector3[] vertices;

		public Color[] colors;

		public DrawCommandCustom()
		{
			combine = default(CombineInstance);
			combine.transform = Matrix4x4.identity;
			color.a = 0f;
			opacity = -1;
		}

		public void setMesh(Mesh mesh)
		{
			combine.mesh = mesh;
			vertices = mesh.vertices;
			colors = mesh.colors;
		}

		public override string ToString()
		{
			return $"DrawCommand {name} opacity:{opacity} color{color.ToString()}";
		}
	}
}
