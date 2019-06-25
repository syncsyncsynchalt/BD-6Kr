using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class PlatformEffector2D : Effector2D
	{
		public bool useOneWay
		{
			get;
			set;
		}

		public bool useSideFriction
		{
			get;
			set;
		}

		public bool useSideBounce
		{
			get;
			set;
		}

		public float surfaceArc
		{
			get;
			set;
		}

		public float sideArc
		{
			get;
			set;
		}
	}
}
