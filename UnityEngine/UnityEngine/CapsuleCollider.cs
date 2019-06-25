using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class CapsuleCollider : Collider
	{
		public Vector3 center
		{
			get
			{
				INTERNAL_get_center(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_center(ref value);
			}
		}

		public float radius
		{
			get;
			set;
		}

		public float height
		{
			get;
			set;
		}

		public int direction
		{
			get;
			set;
		}

		private void INTERNAL_get_center(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_center(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
