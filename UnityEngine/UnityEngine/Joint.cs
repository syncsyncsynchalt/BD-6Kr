using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public class Joint : Component
	{
		public Rigidbody connectedBody
		{
			get;
			set;
		}

		public Vector3 axis
		{
			get
			{
				INTERNAL_get_axis(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_axis(ref value);
			}
		}

		public Vector3 anchor
		{
			get
			{
				INTERNAL_get_anchor(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_anchor(ref value);
			}
		}

		public Vector3 connectedAnchor
		{
			get
			{
				INTERNAL_get_connectedAnchor(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_connectedAnchor(ref value);
			}
		}

		public bool autoConfigureConnectedAnchor
		{
			get;
			set;
		}

		public float breakForce
		{
			get;
			set;
		}

		public float breakTorque
		{
			get;
			set;
		}

		public bool enableCollision
		{
			get;
			set;
		}

		public bool enablePreprocessing
		{
			get;
			set;
		}

		private void INTERNAL_get_axis(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_axis(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_anchor(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_anchor(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_connectedAnchor(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_connectedAnchor(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
