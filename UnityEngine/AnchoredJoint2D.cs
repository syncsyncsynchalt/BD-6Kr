using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public class AnchoredJoint2D : Joint2D
	{
		public Vector2 anchor
		{
			get
			{
				INTERNAL_get_anchor(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_anchor(ref value);
			}
		}

		public Vector2 connectedAnchor
		{
			get
			{
				INTERNAL_get_connectedAnchor(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_connectedAnchor(ref value);
			}
		}

		[WrapperlessIcall]
		private void INTERNAL_get_anchor(out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[WrapperlessIcall]
		private void INTERNAL_set_anchor(ref Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[WrapperlessIcall]
		private void INTERNAL_get_connectedAnchor(out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[WrapperlessIcall]
		private void INTERNAL_set_connectedAnchor(ref Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
