using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class ParticleSystemRenderer : Renderer
	{
		public ParticleSystemRenderMode renderMode
		{
			get;
			set;
		}

		public float lengthScale
		{
			get;
			set;
		}

		public float velocityScale
		{
			get;
			set;
		}

		public float cameraVelocityScale
		{
			get;
			set;
		}

		public float maxParticleSize
		{
			get;
			set;
		}

		public Mesh mesh
		{
			get;
			set;
		}
	}
}
