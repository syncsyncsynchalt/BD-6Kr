using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class OffMeshLink : Component
	{
		public bool activated
		{
			get;
			set;
		}

		public bool occupied
		{
			get;
		}

		public float costOverride
		{
			get;
			set;
		}

		public bool biDirectional
		{
			get;
			set;
		}

		[Obsolete("Use area instead.")]
		public int navMeshLayer
		{
			get;
			set;
		}

		public int area
		{
			get;
			set;
		}

		public bool autoUpdatePositions
		{
			get;
			set;
		}

		public Transform startTransform
		{
			get;
			set;
		}

		public Transform endTransform
		{
			get;
			set;
		}

		public void UpdatePositions() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
