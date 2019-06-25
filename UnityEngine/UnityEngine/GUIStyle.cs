using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class GUIStyle
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		[NonSerialized]
		private GUIStyleState m_Normal;

		[NonSerialized]
		private GUIStyleState m_Hover;

		[NonSerialized]
		private GUIStyleState m_Active;

		[NonSerialized]
		private GUIStyleState m_Focused;

		[NonSerialized]
		private GUIStyleState m_OnNormal;

		[NonSerialized]
		private GUIStyleState m_OnHover;

		[NonSerialized]
		private GUIStyleState m_OnActive;

		[NonSerialized]
		private GUIStyleState m_OnFocused;

		[NonSerialized]
		private RectOffset m_Border;

		[NonSerialized]
		private RectOffset m_Padding;

		[NonSerialized]
		private RectOffset m_Margin;

		[NonSerialized]
		private RectOffset m_Overflow;

		[NonSerialized]
		private Font m_FontInternal;

		internal static bool showKeyboardFocus = true;

		private static GUIStyle s_None;

		public GUIStyleState normal
		{
			get
			{
				if (m_Normal == null)
				{
					m_Normal = new GUIStyleState(this, GetStyleStatePtr(0));
				}
				return m_Normal;
			}
			set
			{
				AssignStyleState(0, value.m_Ptr);
			}
		}

		public GUIStyleState hover
		{
			get
			{
				if (m_Hover == null)
				{
					m_Hover = new GUIStyleState(this, GetStyleStatePtr(1));
				}
				return m_Hover;
			}
			set
			{
				AssignStyleState(1, value.m_Ptr);
			}
		}

		public GUIStyleState active
		{
			get
			{
				if (m_Active == null)
				{
					m_Active = new GUIStyleState(this, GetStyleStatePtr(2));
				}
				return m_Active;
			}
			set
			{
				AssignStyleState(2, value.m_Ptr);
			}
		}

		public GUIStyleState onNormal
		{
			get
			{
				if (m_OnNormal == null)
				{
					m_OnNormal = new GUIStyleState(this, GetStyleStatePtr(4));
				}
				return m_OnNormal;
			}
			set
			{
				AssignStyleState(4, value.m_Ptr);
			}
		}

		public GUIStyleState onHover
		{
			get
			{
				if (m_OnHover == null)
				{
					m_OnHover = new GUIStyleState(this, GetStyleStatePtr(5));
				}
				return m_OnHover;
			}
			set
			{
				AssignStyleState(5, value.m_Ptr);
			}
		}

		public GUIStyleState onActive
		{
			get
			{
				if (m_OnActive == null)
				{
					m_OnActive = new GUIStyleState(this, GetStyleStatePtr(6));
				}
				return m_OnActive;
			}
			set
			{
				AssignStyleState(6, value.m_Ptr);
			}
		}

		public GUIStyleState focused
		{
			get
			{
				if (m_Focused == null)
				{
					m_Focused = new GUIStyleState(this, GetStyleStatePtr(3));
				}
				return m_Focused;
			}
			set
			{
				AssignStyleState(3, value.m_Ptr);
			}
		}

		public GUIStyleState onFocused
		{
			get
			{
				if (m_OnFocused == null)
				{
					m_OnFocused = new GUIStyleState(this, GetStyleStatePtr(7));
				}
				return m_OnFocused;
			}
			set
			{
				AssignStyleState(7, value.m_Ptr);
			}
		}

		public RectOffset border
		{
			get
			{
				if (m_Border == null)
				{
					m_Border = new RectOffset(this, GetRectOffsetPtr(0));
				}
				return m_Border;
			}
			set
			{
				AssignRectOffset(0, value.m_Ptr);
			}
		}

		public RectOffset margin
		{
			get
			{
				if (m_Margin == null)
				{
					m_Margin = new RectOffset(this, GetRectOffsetPtr(1));
				}
				return m_Margin;
			}
			set
			{
				AssignRectOffset(1, value.m_Ptr);
			}
		}

		public RectOffset padding
		{
			get
			{
				if (m_Padding == null)
				{
					m_Padding = new RectOffset(this, GetRectOffsetPtr(2));
				}
				return m_Padding;
			}
			set
			{
				AssignRectOffset(2, value.m_Ptr);
			}
		}

		public RectOffset overflow
		{
			get
			{
				if (m_Overflow == null)
				{
					m_Overflow = new RectOffset(this, GetRectOffsetPtr(3));
				}
				return m_Overflow;
			}
			set
			{
				AssignRectOffset(3, value.m_Ptr);
			}
		}

		[Obsolete("warning Don't use clipOffset - put things inside BeginGroup instead. This functionality will be removed in a later version.")]
		public Vector2 clipOffset
		{
			get
			{
				return Internal_clipOffset;
			}
			set
			{
				Internal_clipOffset = value;
			}
		}

		public Font font
		{
			get
			{
				return GetFontInternal();
			}
			set
			{
				SetFontInternal(value);
				m_FontInternal = value;
			}
		}

		public float lineHeight => Mathf.Round(Internal_GetLineHeight(m_Ptr));

		public static GUIStyle none
		{
			get
			{
				if (s_None == null)
				{
					s_None = new GUIStyle();
				}
				return s_None;
			}
		}

		public bool isHeightDependantOnWidth => fixedHeight == 0f && wordWrap && imagePosition != ImagePosition.ImageOnly;

		public string name
		{
			get;
			set;
		}

		public ImagePosition imagePosition
		{
			get;
			set;
		}

		public TextAnchor alignment
		{
			get;
			set;
		}

		public bool wordWrap
		{
			get;
			set;
		}

		public TextClipping clipping
		{
			get;
			set;
		}

		public Vector2 contentOffset
		{
			get
			{
				INTERNAL_get_contentOffset(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_contentOffset(ref value);
			}
		}

		internal Vector2 Internal_clipOffset
		{
			get
			{
				INTERNAL_get_Internal_clipOffset(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_Internal_clipOffset(ref value);
			}
		}

		public float fixedWidth
		{
			get;
			set;
		}

		public float fixedHeight
		{
			get;
			set;
		}

		public bool stretchWidth
		{
			get;
			set;
		}

		public bool stretchHeight
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

		public bool richText
		{
			get;
			set;
		}

		public GUIStyle()
		{
			Init();
		}

		public GUIStyle(GUIStyle other)
		{
			InitCopy(other);
		}

		~GUIStyle()
		{
			Cleanup();
		}

		internal void InternalOnAfterDeserialize()
		{
			m_FontInternal = GetFontInternal();
			m_Normal = new GUIStyleState(this, GetStyleStatePtr(0));
			m_Hover = new GUIStyleState(this, GetStyleStatePtr(1));
			m_Active = new GUIStyleState(this, GetStyleStatePtr(2));
			m_Focused = new GUIStyleState(this, GetStyleStatePtr(3));
			m_OnNormal = new GUIStyleState(this, GetStyleStatePtr(4));
			m_OnHover = new GUIStyleState(this, GetStyleStatePtr(5));
			m_OnActive = new GUIStyleState(this, GetStyleStatePtr(6));
			m_OnFocused = new GUIStyleState(this, GetStyleStatePtr(7));
		}

		private static void Internal_Draw(IntPtr target, Rect position, GUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
		{
			Internal_DrawArguments arguments = default(Internal_DrawArguments);
			arguments.target = target;
			arguments.position = position;
			arguments.isHover = (isHover ? 1 : 0);
			arguments.isActive = (isActive ? 1 : 0);
			arguments.on = (on ? 1 : 0);
			arguments.hasKeyboardFocus = (hasKeyboardFocus ? 1 : 0);
			Internal_Draw(content, ref arguments);
		}

		public void Draw(Rect position, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
		{
			Internal_Draw(m_Ptr, position, GUIContent.none, isHover, isActive, on, hasKeyboardFocus);
		}

		public void Draw(Rect position, string text, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
		{
			Internal_Draw(m_Ptr, position, GUIContent.Temp(text), isHover, isActive, on, hasKeyboardFocus);
		}

		public void Draw(Rect position, Texture image, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
		{
			Internal_Draw(m_Ptr, position, GUIContent.Temp(image), isHover, isActive, on, hasKeyboardFocus);
		}

		public void Draw(Rect position, GUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
		{
			Internal_Draw(m_Ptr, position, content, isHover, isActive, on, hasKeyboardFocus);
		}

		public void Draw(Rect position, GUIContent content, int controlID)
		{
			Draw(position, content, controlID, on: false);
		}

		public void Draw(Rect position, GUIContent content, int controlID, bool on)
		{
			if (content != null)
			{
				Internal_Draw2(m_Ptr, position, content, controlID, on);
			}
			else
			{
				Debug.LogError("Style.Draw may not be called with GUIContent that is null.");
			}
		}

		public void DrawCursor(Rect position, GUIContent content, int controlID, int Character)
		{
			Event current = Event.current;
			if (current.type == EventType.Repaint)
			{
				Color cursorColor = new Color(0f, 0f, 0f, 0f);
				float cursorFlashSpeed = GUI.skin.settings.cursorFlashSpeed;
				float num = (Time.realtimeSinceStartup - Internal_GetCursorFlashOffset()) % cursorFlashSpeed / cursorFlashSpeed;
				if (cursorFlashSpeed == 0f || num < 0.5f)
				{
					cursorColor = GUI.skin.settings.cursorColor;
				}
				Internal_DrawCursor(m_Ptr, position, content, Character, cursorColor);
			}
		}

		internal void DrawWithTextSelection(Rect position, GUIContent content, int controlID, int firstSelectedCharacter, int lastSelectedCharacter, bool drawSelectionAsComposition)
		{
			Event current = Event.current;
			Color cursorColor = new Color(0f, 0f, 0f, 0f);
			float cursorFlashSpeed = GUI.skin.settings.cursorFlashSpeed;
			float num = (Time.realtimeSinceStartup - Internal_GetCursorFlashOffset()) % cursorFlashSpeed / cursorFlashSpeed;
			if (cursorFlashSpeed == 0f || num < 0.5f)
			{
				cursorColor = GUI.skin.settings.cursorColor;
			}
			Internal_DrawWithTextSelectionArguments arguments = default(Internal_DrawWithTextSelectionArguments);
			arguments.target = m_Ptr;
			arguments.position = position;
			arguments.firstPos = firstSelectedCharacter;
			arguments.lastPos = lastSelectedCharacter;
			arguments.cursorColor = cursorColor;
			arguments.selectionColor = GUI.skin.settings.selectionColor;
			arguments.isHover = (position.Contains(current.mousePosition) ? 1 : 0);
			arguments.isActive = ((controlID == GUIUtility.hotControl) ? 1 : 0);
			arguments.on = 0;
			arguments.hasKeyboardFocus = ((controlID == GUIUtility.keyboardControl && showKeyboardFocus) ? 1 : 0);
			arguments.drawSelectionAsComposition = (drawSelectionAsComposition ? 1 : 0);
			Internal_DrawWithTextSelection(content, ref arguments);
		}

		public void DrawWithTextSelection(Rect position, GUIContent content, int controlID, int firstSelectedCharacter, int lastSelectedCharacter)
		{
			DrawWithTextSelection(position, content, controlID, firstSelectedCharacter, lastSelectedCharacter, drawSelectionAsComposition: false);
		}

		public Vector2 GetCursorPixelPosition(Rect position, GUIContent content, int cursorStringIndex)
		{
			Internal_GetCursorPixelPosition(m_Ptr, position, content, cursorStringIndex, out Vector2 ret);
			return ret;
		}

		public int GetCursorStringIndex(Rect position, GUIContent content, Vector2 cursorPixelPosition)
		{
			return Internal_GetCursorStringIndex(m_Ptr, position, content, cursorPixelPosition);
		}

		internal int GetNumCharactersThatFitWithinWidth(string text, float width)
		{
			return Internal_GetNumCharactersThatFitWithinWidth(m_Ptr, text, width);
		}

		public Vector2 CalcSize(GUIContent content)
		{
			Internal_CalcSize(m_Ptr, content, out Vector2 ret);
			return ret;
		}

		public Vector2 CalcScreenSize(Vector2 contentSize)
		{
			return new Vector2((fixedWidth == 0f) ? Mathf.Ceil(contentSize.x + (float)padding.left + (float)padding.right) : fixedWidth, (fixedHeight == 0f) ? Mathf.Ceil(contentSize.y + (float)padding.top + (float)padding.bottom) : fixedHeight);
		}

		public float CalcHeight(GUIContent content, float width)
		{
			return Internal_CalcHeight(m_Ptr, content, width);
		}

		public void CalcMinMaxWidth(GUIContent content, out float minWidth, out float maxWidth)
		{
			Internal_CalcMinMaxWidth(m_Ptr, content, out minWidth, out maxWidth);
		}

		public override string ToString()
		{
			return UnityString.Format("GUIStyle '{0}'", name);
		}

		private void Init() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void InitCopy(GUIStyle other) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Cleanup() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private IntPtr GetStyleStatePtr(int idx) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void AssignStyleState(int idx, IntPtr srcStyleState) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private IntPtr GetRectOffsetPtr(int idx) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void AssignRectOffset(int idx, IntPtr srcRectOffset) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_contentOffset(out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_contentOffset(ref Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_Internal_clipOffset(out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_Internal_clipOffset(ref Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static float Internal_GetLineHeight(IntPtr target) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetFontInternal(Font value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private Font GetFontInternal() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_Draw(GUIContent content, ref Internal_DrawArguments arguments) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_Draw2(IntPtr style, Rect position, GUIContent content, int controlID, bool on)
		{
			INTERNAL_CALL_Internal_Draw2(style, ref position, content, controlID, on);
		}

		private static void INTERNAL_CALL_Internal_Draw2(IntPtr style, ref Rect position, GUIContent content, int controlID, bool on) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_DrawPrefixLabel(IntPtr style, Rect position, GUIContent content, int controlID, bool on)
		{
			INTERNAL_CALL_Internal_DrawPrefixLabel(style, ref position, content, controlID, on);
		}

		private static void INTERNAL_CALL_Internal_DrawPrefixLabel(IntPtr style, ref Rect position, GUIContent content, int controlID, bool on) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static float Internal_GetCursorFlashOffset() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_DrawCursor(IntPtr target, Rect position, GUIContent content, int pos, Color cursorColor)
		{
			INTERNAL_CALL_Internal_DrawCursor(target, ref position, content, pos, ref cursorColor);
		}

		private static void INTERNAL_CALL_Internal_DrawCursor(IntPtr target, ref Rect position, GUIContent content, int pos, ref Color cursorColor) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_DrawWithTextSelection(GUIContent content, ref Internal_DrawWithTextSelectionArguments arguments) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static void SetDefaultFont(Font font) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static void Internal_GetCursorPixelPosition(IntPtr target, Rect position, GUIContent content, int cursorStringIndex, out Vector2 ret)
		{
			INTERNAL_CALL_Internal_GetCursorPixelPosition(target, ref position, content, cursorStringIndex, out ret);
		}

		private static void INTERNAL_CALL_Internal_GetCursorPixelPosition(IntPtr target, ref Rect position, GUIContent content, int cursorStringIndex, out Vector2 ret) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static int Internal_GetCursorStringIndex(IntPtr target, Rect position, GUIContent content, Vector2 cursorPixelPosition)
		{
			return INTERNAL_CALL_Internal_GetCursorStringIndex(target, ref position, content, ref cursorPixelPosition);
		}

		private static int INTERNAL_CALL_Internal_GetCursorStringIndex(IntPtr target, ref Rect position, GUIContent content, ref Vector2 cursorPixelPosition) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static int Internal_GetNumCharactersThatFitWithinWidth(IntPtr target, string text, float width) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static void Internal_CalcSize(IntPtr target, GUIContent content, out Vector2 ret) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static float Internal_CalcHeight(IntPtr target, GUIContent content, float width) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_CalcMinMaxWidth(IntPtr target, GUIContent content, out float minWidth, out float maxWidth) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static implicit operator GUIStyle(string str)
		{
			if (GUISkin.current == null)
			{
				Debug.LogError("Unable to use a named GUIStyle without a current skin. Most likely you need to move your GUIStyle initialization code to OnGUI");
				return GUISkin.error;
			}
			return GUISkin.current.GetStyle(str);
		}
	}
}
