using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class TextGenerator : IDisposable
	{
		internal IntPtr m_Ptr;

		private string m_LastString;

		private TextGenerationSettings m_LastSettings;

		private bool m_HasGenerated;

		private bool m_LastValid;

		private readonly List<UIVertex> m_Verts;

		private readonly List<UICharInfo> m_Characters;

		private readonly List<UILineInfo> m_Lines;

		private bool m_CachedVerts;

		private bool m_CachedCharacters;

		private bool m_CachedLines;

		public Rect rectExtents
		{
			get
			{
				INTERNAL_get_rectExtents(out Rect value);
				return value;
			}
		}

		public int vertexCount
		{
			get;
		}

		public int characterCount
		{
			get;
		}

		public int characterCountVisible => (!string.IsNullOrEmpty(m_LastString)) ? Mathf.Min(m_LastString.Length, Mathf.Max(0, (vertexCount - 4) / 4)) : 0;

		public int lineCount
		{
			get;
		}

		public int fontSizeUsedForBestFit
		{
			get;
		}

		public IList<UIVertex> verts
		{
			get
			{
				if (!m_CachedVerts)
				{
					GetVertices(m_Verts);
					m_CachedVerts = true;
				}
				return m_Verts;
			}
		}

		public IList<UICharInfo> characters
		{
			get
			{
				if (!m_CachedCharacters)
				{
					GetCharacters(m_Characters);
					m_CachedCharacters = true;
				}
				return m_Characters;
			}
		}

		public IList<UILineInfo> lines
		{
			get
			{
				if (!m_CachedLines)
				{
					GetLines(m_Lines);
					m_CachedLines = true;
				}
				return m_Lines;
			}
		}

		public TextGenerator()
			: this(50)
		{
		}

		public TextGenerator(int initialCapacity)
		{
			m_Verts = new List<UIVertex>((initialCapacity + 1) * 4);
			m_Characters = new List<UICharInfo>(initialCapacity + 1);
			m_Lines = new List<UILineInfo>(20);
			Init();
		}

		void IDisposable.Dispose()
		{
			Dispose_cpp();
		}

		private void Init() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Dispose_cpp() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal bool Populate_Internal(string str, Font font, Color color, int fontSize, float scaleFactor, float lineSpacing, FontStyle style, bool richText, bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize, VerticalWrapMode verticalOverFlow, HorizontalWrapMode horizontalOverflow, bool updateBounds, TextAnchor anchor, Vector2 extents, Vector2 pivot, bool generateOutOfBounds)
		{
			return Populate_Internal_cpp(str, font, color, fontSize, scaleFactor, lineSpacing, style, richText, resizeTextForBestFit, resizeTextMinSize, resizeTextMaxSize, (int)verticalOverFlow, (int)horizontalOverflow, updateBounds, anchor, extents.x, extents.y, pivot.x, pivot.y, generateOutOfBounds);
		}

		internal bool Populate_Internal_cpp(string str, Font font, Color color, int fontSize, float scaleFactor, float lineSpacing, FontStyle style, bool richText, bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize, int verticalOverFlow, int horizontalOverflow, bool updateBounds, TextAnchor anchor, float extentsX, float extentsY, float pivotX, float pivotY, bool generateOutOfBounds)
		{
			return INTERNAL_CALL_Populate_Internal_cpp(this, str, font, ref color, fontSize, scaleFactor, lineSpacing, style, richText, resizeTextForBestFit, resizeTextMinSize, resizeTextMaxSize, verticalOverFlow, horizontalOverflow, updateBounds, anchor, extentsX, extentsY, pivotX, pivotY, generateOutOfBounds);
		}

		private static bool INTERNAL_CALL_Populate_Internal_cpp(TextGenerator self, string str, Font font, ref Color color, int fontSize, float scaleFactor, float lineSpacing, FontStyle style, bool richText, bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize, int verticalOverFlow, int horizontalOverflow, bool updateBounds, TextAnchor anchor, float extentsX, float extentsY, float pivotX, float pivotY, bool generateOutOfBounds) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_rectExtents(out Rect value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void GetVerticesInternal(object vertices) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public UIVertex[] GetVerticesArray() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void GetCharactersInternal(object characters) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public UICharInfo[] GetCharactersArray() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void GetLinesInternal(object lines) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public UILineInfo[] GetLinesArray() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		~TextGenerator()
		{
			((IDisposable)this).Dispose();
		}

		private TextGenerationSettings ValidatedSettings(TextGenerationSettings settings)
		{
			if (settings.font != null && settings.font.dynamic)
			{
				return settings;
			}
			if (settings.fontSize != 0 || settings.fontStyle != 0)
			{
				Debug.LogWarning("Font size and style overrides are only supported for dynamic fonts.");
				settings.fontSize = 0;
				settings.fontStyle = FontStyle.Normal;
			}
			if (settings.resizeTextForBestFit)
			{
				Debug.LogWarning("BestFit is only suppoerted for dynamic fonts.");
				settings.resizeTextForBestFit = false;
			}
			return settings;
		}

		public void Invalidate()
		{
			m_HasGenerated = false;
		}

		public void GetCharacters(List<UICharInfo> characters)
		{
			GetCharactersInternal(characters);
		}

		public void GetLines(List<UILineInfo> lines)
		{
			GetLinesInternal(lines);
		}

		public void GetVertices(List<UIVertex> vertices)
		{
			GetVerticesInternal(vertices);
		}

		public float GetPreferredWidth(string str, TextGenerationSettings settings)
		{
			settings.horizontalOverflow = HorizontalWrapMode.Overflow;
			settings.verticalOverflow = VerticalWrapMode.Overflow;
			settings.updateBounds = true;
			Populate(str, settings);
			return rectExtents.width;
		}

		public float GetPreferredHeight(string str, TextGenerationSettings settings)
		{
			settings.verticalOverflow = VerticalWrapMode.Overflow;
			settings.updateBounds = true;
			Populate(str, settings);
			return rectExtents.height;
		}

		public bool Populate(string str, TextGenerationSettings settings)
		{
			if (m_HasGenerated && str == m_LastString && settings.Equals(m_LastSettings))
			{
				return m_LastValid;
			}
			return PopulateAlways(str, settings);
		}

		private bool PopulateAlways(string str, TextGenerationSettings settings)
		{
			m_LastString = str;
			m_HasGenerated = true;
			m_CachedVerts = false;
			m_CachedCharacters = false;
			m_CachedLines = false;
			m_LastSettings = settings;
			TextGenerationSettings textGenerationSettings = ValidatedSettings(settings);
			m_LastValid = Populate_Internal(str, textGenerationSettings.font, textGenerationSettings.color, textGenerationSettings.fontSize, textGenerationSettings.scaleFactor, textGenerationSettings.lineSpacing, textGenerationSettings.fontStyle, textGenerationSettings.richText, textGenerationSettings.resizeTextForBestFit, textGenerationSettings.resizeTextMinSize, textGenerationSettings.resizeTextMaxSize, textGenerationSettings.verticalOverflow, textGenerationSettings.horizontalOverflow, textGenerationSettings.updateBounds, textGenerationSettings.textAnchor, textGenerationSettings.generationExtents, textGenerationSettings.pivot, textGenerationSettings.generateOutOfBounds);
			return m_LastValid;
		}
	}
}
