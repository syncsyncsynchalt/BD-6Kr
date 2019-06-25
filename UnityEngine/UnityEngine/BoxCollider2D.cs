using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class BoxCollider2D : Collider2D
	{
		public Vector2 size
		{
			get
			{
				INTERNAL_get_size(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_size(ref value);
			}
		}

		private void INTERNAL_get_size(out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_size(ref Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
