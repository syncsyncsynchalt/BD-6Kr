using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class SpringJoint : Joint
	{
		public float spring
		{
			get;
			set;
		}

		public float damper
		{
			get;
			set;
		}

		public float minDistance
		{
			get;
			set;
		}

		public float maxDistance
		{
			get;
			set;
		}
	}
}
