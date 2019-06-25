using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class LineRenderer : Renderer
	{
		public bool useWorldSpace
		{
			get;
			set;
		}

		public void SetWidth(float start, float end)
		{
			INTERNAL_CALL_SetWidth(this, start, end);
		}

		private static void INTERNAL_CALL_SetWidth(LineRenderer self, float start, float end) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetColors(Color start, Color end)
		{
			INTERNAL_CALL_SetColors(this, ref start, ref end);
		}

		private static void INTERNAL_CALL_SetColors(LineRenderer self, ref Color start, ref Color end) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetVertexCount(int count)
		{
			INTERNAL_CALL_SetVertexCount(this, count);
		}

		private static void INTERNAL_CALL_SetVertexCount(LineRenderer self, int count) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetPosition(int index, Vector3 position)
		{
			INTERNAL_CALL_SetPosition(this, index, ref position);
		}

		private static void INTERNAL_CALL_SetPosition(LineRenderer self, int index, ref Vector3 position) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
