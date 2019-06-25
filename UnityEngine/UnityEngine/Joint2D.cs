using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public class Joint2D : Behaviour
	{
		public Rigidbody2D connectedBody
		{
			get;
			set;
		}

		public bool enableCollision
		{
			get;
			set;
		}
	}
}
