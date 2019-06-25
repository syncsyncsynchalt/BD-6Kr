using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class GUIStyleState
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		private readonly GUIStyle m_SourceStyle;

		[NonSerialized]
		private Texture2D m_Background;

		public Texture2D background
		{
			get
			{
				return GetBackgroundInternal();
			}
			set
			{
				SetBackgroundInternal(value);
				m_Background = value;
			}
		}

		public Color textColor
		{
			get
			{
				INTERNAL_get_textColor(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_textColor(ref value);
			}
		}

		public GUIStyleState()
		{
			Init();
		}

		internal GUIStyleState(GUIStyle sourceStyle, IntPtr source)
		{
			m_SourceStyle = sourceStyle;
			m_Ptr = source;
			m_Background = GetBackgroundInternal();
		}

		~GUIStyleState()
		{
			if (m_SourceStyle == null)
			{
				Cleanup();
			}
		}

		private void Init() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Cleanup() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetBackgroundInternal(Texture2D value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private Texture2D GetBackgroundInternal() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_textColor(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_textColor(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
