using System;

using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class PolygonCollider2D : Collider2D
	{
		public Vector2[] points
		{
			get;
			set;
		}

		public int pathCount
		{
			get;
			set;
		}

		public Vector2[] GetPath(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetPath(int index, Vector2[] points) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public int GetTotalPointCount() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void CreatePrimitive(int sides, [DefaultValue("Vector2.one")] Vector2 scale, [DefaultValue("Vector2.zero")] Vector2 offset)
		{
			INTERNAL_CALL_CreatePrimitive(this, sides, ref scale, ref offset);
		}

		[ExcludeFromDocs]
		public void CreatePrimitive(int sides, Vector2 scale)
		{
			Vector2 offset = Vector2.zero;
			INTERNAL_CALL_CreatePrimitive(this, sides, ref scale, ref offset);
		}

		[ExcludeFromDocs]
		public void CreatePrimitive(int sides)
		{
			Vector2 offset = Vector2.zero;
			Vector2 scale = Vector2.one;
			INTERNAL_CALL_CreatePrimitive(this, sides, ref scale, ref offset);
		}

		private static void INTERNAL_CALL_CreatePrimitive(PolygonCollider2D self, int sides, ref Vector2 scale, ref Vector2 offset) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
