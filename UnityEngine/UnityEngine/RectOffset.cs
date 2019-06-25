using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class RectOffset
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		private readonly GUIStyle m_SourceStyle;

		public int left
		{
			get;
			set;
		}

		public int right
		{
			get;
			set;
		}

		public int top
		{
			get;
			set;
		}

		public int bottom
		{
			get;
			set;
		}

		public int horizontal
		{
			get;
		}

		public int vertical
		{
			get;
		}

		public RectOffset()
		{
			Init();
		}

		internal RectOffset(GUIStyle sourceStyle, IntPtr source)
		{
			m_SourceStyle = sourceStyle;
			m_Ptr = source;
		}

		public RectOffset(int left, int right, int top, int bottom)
		{
			Init();
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
		}

		~RectOffset()
		{
			if (m_SourceStyle == null)
			{
				Cleanup();
			}
		}

		public override string ToString()
		{
			return UnityString.Format("RectOffset (l:{0} r:{1} t:{2} b:{3})", left, right, top, bottom);
		}

		private void Init() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Cleanup() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Rect Add(Rect rect)
		{
			return INTERNAL_CALL_Add(this, ref rect);
		}

		private static Rect INTERNAL_CALL_Add(RectOffset self, ref Rect rect) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Rect Remove(Rect rect)
		{
			return INTERNAL_CALL_Remove(this, ref rect);
		}

		private static Rect INTERNAL_CALL_Remove(RectOffset self, ref Rect rect) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
