using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class MeshFilter : Component
	{
		public Mesh mesh
		{
			get;
			set;
		}

		public Mesh sharedMesh
		{
			get;
			set;
		}
	}
}
