using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class PointEffector2D : Effector2D
	{
		public float forceMagnitude
		{
			get;
			set;
		}

		public float forceVariation
		{
			get;
			set;
		}

		public float distanceScale
		{
			get;
			set;
		}

		public float drag
		{
			get;
			set;
		}

		public float angularDrag
		{
			get;
			set;
		}

		public EffectorSelection2D forceSource
		{
			get;
			set;
		}

		public EffectorSelection2D forceTarget
		{
			get;
			set;
		}

		public EffectorForceMode2D forceMode
		{
			get;
			set;
		}
	}
}
