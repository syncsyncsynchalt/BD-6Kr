using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class TrailRenderer : Renderer
	{
		public float time
		{
			get;
			set;
		}

		public float startWidth
		{
			get;
			set;
		}

		public float endWidth
		{
			get;
			set;
		}

		public bool autodestruct
		{
			get;
			set;
		}
	}
}
