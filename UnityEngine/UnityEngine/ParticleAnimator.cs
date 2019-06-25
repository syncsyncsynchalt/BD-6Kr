using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class ParticleAnimator : Component
	{
		public bool doesAnimateColor
		{
			get;
			set;
		}

		public Vector3 worldRotationAxis
		{
			get
			{
				INTERNAL_get_worldRotationAxis(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_worldRotationAxis(ref value);
			}
		}

		public Vector3 localRotationAxis
		{
			get
			{
				INTERNAL_get_localRotationAxis(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_localRotationAxis(ref value);
			}
		}

		public float sizeGrow
		{
			get;
			set;
		}

		public Vector3 rndForce
		{
			get
			{
				INTERNAL_get_rndForce(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_rndForce(ref value);
			}
		}

		public Vector3 force
		{
			get
			{
				INTERNAL_get_force(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_force(ref value);
			}
		}

		public float damping
		{
			get;
			set;
		}

		public bool autodestruct
		{
			get;
			set;
		}

		public Color[] colorAnimation
		{
			get;
			set;
		}

		private void INTERNAL_get_worldRotationAxis(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_worldRotationAxis(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_localRotationAxis(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_localRotationAxis(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_rndForce(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_rndForce(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_force(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_force(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
