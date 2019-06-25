using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class TextMesh : Component
	{
		public string text
		{
			get;
			set;
		}

		public Font font
		{
			get;
			set;
		}

		public int fontSize
		{
			get;
			set;
		}

		public FontStyle fontStyle
		{
			get;
			set;
		}

		public float offsetZ
		{
			get;
			set;
		}

		public TextAlignment alignment
		{
			get;
			set;
		}

		public TextAnchor anchor
		{
			get;
			set;
		}

		public float characterSize
		{
			get;
			set;
		}

		public float lineSpacing
		{
			get;
			set;
		}

		public float tabSize
		{
			get;
			set;
		}

		public bool richText
		{
			get;
			set;
		}

		public Color color
		{
			get
			{
				INTERNAL_get_color(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_color(ref value);
			}
		}

		private void INTERNAL_get_color(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_color(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
