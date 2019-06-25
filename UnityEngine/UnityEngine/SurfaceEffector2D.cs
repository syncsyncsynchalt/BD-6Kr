using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class SurfaceEffector2D : Effector2D
	{
		public float speed
		{
			get;
			set;
		}

		public float speedVariation
		{
			get;
			set;
		}

		public float forceScale
		{
			get;
			set;
		}

		public bool useContactForce
		{
			get;
			set;
		}

		public bool useFriction
		{
			get;
			set;
		}

		public bool useBounce
		{
			get;
			set;
		}
	}
}
