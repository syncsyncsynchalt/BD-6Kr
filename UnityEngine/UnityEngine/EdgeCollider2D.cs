using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class EdgeCollider2D : Collider2D
	{
		public int edgeCount
		{
			get;
		}

		public int pointCount
		{
			get;
		}

		public Vector2[] points
		{
			get;
			set;
		}

		public void Reset() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
