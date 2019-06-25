using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Projector : Behaviour
	{
		public float nearClipPlane
		{
			get;
			set;
		}

		public float farClipPlane
		{
			get;
			set;
		}

		public float fieldOfView
		{
			get;
			set;
		}

		public float aspectRatio
		{
			get;
			set;
		}

		public bool orthographic
		{
			get;
			set;
		}

		public float orthographicSize
		{
			get;
			set;
		}

		public int ignoreLayers
		{
			get;
			set;
		}

		public Material material
		{
			get;
			set;
		}
	}
}
