using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AreaEffector2D : Effector2D
	{
		public float forceAngle
		{
			get;
			set;
		}

		public bool useGlobalAngle
		{
			get;
			set;
		}

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

		public EffectorSelection2D forceTarget
		{
			get;
			set;
		}
	}
}
