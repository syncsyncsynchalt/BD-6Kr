using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public class Effector2D : Behaviour
	{
		public bool useColliderMask
		{
			get;
			set;
		}

		public int colliderMask
		{
			get;
			set;
		}

		internal bool requiresCollider
		{
			get;
		}

		internal bool designedForTrigger
		{
			get;
		}

		internal bool designedForNonTrigger
		{
			get;
		}
	}
}
