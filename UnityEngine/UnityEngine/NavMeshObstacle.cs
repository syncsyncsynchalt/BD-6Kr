using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class NavMeshObstacle : Behaviour
	{
		public float height
		{
			get;
			set;
		}

		public float radius
		{
			get;
			set;
		}

		public Vector3 velocity
		{
			get
			{
				INTERNAL_get_velocity(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_velocity(ref value);
			}
		}

		public bool carving
		{
			get;
			set;
		}

		public bool carveOnlyStationary
		{
			get;
			set;
		}

		public float carvingMoveThreshold
		{
			get;
			set;
		}

		public float carvingTimeToStationary
		{
			get;
			set;
		}

		public NavMeshObstacleShape shape
		{
			get;
			set;
		}

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

		public Vector3 size
		{
			get
			{
				INTERNAL_get_size(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_size(ref value);
			}
		}

		private void INTERNAL_get_velocity(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_velocity(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_center(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_center(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_size(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_size(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void FitExtents() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
