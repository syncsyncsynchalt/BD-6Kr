using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public class GUIUtility
	{
		internal static int s_SkinMode;

		internal static int s_OriginalID;

		internal static Vector2 s_EditorScreenPointOffset = Vector2.zero;

		internal static bool s_HasKeyboardFocus = false;

		internal static float pixelsPerPoint => Internal_GetPixelsPerPoint();

		public static int hotControl
		{
			get
			{
				return Internal_GetHotControl();
			}
			set
			{
				Internal_SetHotControl(value);
			}
		}

		public static int keyboardControl
		{
			get;
			set;
		}

		public static string systemCopyBuffer
		{
			get;
			set;
		}

		internal static bool mouseUsed
		{
			get;
			set;
		}

		public static bool hasModalWindow
		{
			get;
		}

		internal static bool textFieldInput
		{
			get;
			set;
		}

		public static int GetControlID(FocusType focus)
		{
			return GetControlID(0, focus);
		}

		public static int GetControlID(GUIContent contents, FocusType focus)
		{
			return GetControlID(contents.hash, focus);
		}

		public static int GetControlID(FocusType focus, Rect position)
		{
			return Internal_GetNextControlID2(0, focus, position);
		}

		public static int GetControlID(int hint, FocusType focus, Rect position)
		{
			return Internal_GetNextControlID2(hint, focus, position);
		}

		public static int GetControlID(GUIContent contents, FocusType focus, Rect position)
		{
			return Internal_GetNextControlID2(contents.hash, focus, position);
		}

		public static object GetStateObject(Type t, int controlID)
		{
			return GUIStateObjects.GetStateObject(t, controlID);
		}

		public static object QueryStateObject(Type t, int controlID)
		{
			return GUIStateObjects.QueryStateObject(t, controlID);
		}

		public static void ExitGUI()
		{
			throw new ExitGUIException();
		}

		internal static GUISkin GetDefaultSkin()
		{
			return Internal_GetDefaultSkin(s_SkinMode);
		}

		internal static GUISkin GetBuiltinSkin(int skin)
		{
			return Internal_GetBuiltinSkin(skin) as GUISkin;
		}

		internal static void BeginGUI(int skinMode, int instanceID, int useGUILayout)
		{
			s_SkinMode = skinMode;
			s_OriginalID = instanceID;
			GUI.skin = null;
			if (useGUILayout != 0)
			{
				GUILayoutUtility.SelectIDList(instanceID, isWindow: false);
				GUILayoutUtility.Begin(instanceID);
			}
			GUI.changed = false;
		}

		internal static void EndGUI(int layoutType)
		{
			try
			{
				if (Event.current.type == EventType.Layout)
				{
					switch (layoutType)
					{
					case 1:
						GUILayoutUtility.Layout();
						break;
					case 2:
						GUILayoutUtility.LayoutFromEditorWindow();
						break;
					}
				}
				GUILayoutUtility.SelectIDList(s_OriginalID, isWindow: false);
				GUIContent.ClearStaticCache();
			}
			finally
			{
				Internal_ExitGUI();
			}
		}

		internal static bool EndGUIFromException(Exception exception)
		{
			if (exception == null)
			{
				return false;
			}
			if (!(exception is ExitGUIException) && !(exception.InnerException is ExitGUIException))
			{
				return false;
			}
			Internal_ExitGUI();
			return true;
		}

		internal static void CheckOnGUI()
		{
			if (Internal_GetGUIDepth() <= 0)
			{
				throw new ArgumentException("You can only call GUI functions from inside OnGUI.");
			}
		}

		public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return GUIClip.Unclip(guiPoint) + s_EditorScreenPointOffset;
		}

		internal static Rect GUIToScreenRect(Rect guiRect)
		{
			Vector2 vector = GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
			guiRect.x = vector.x;
			guiRect.y = vector.y;
			return guiRect;
		}

		public static Vector2 ScreenToGUIPoint(Vector2 screenPoint)
		{
			return GUIClip.Clip(screenPoint) - s_EditorScreenPointOffset;
		}

		public static Rect ScreenToGUIRect(Rect screenRect)
		{
			Vector2 vector = ScreenToGUIPoint(new Vector2(screenRect.x, screenRect.y));
			screenRect.x = vector.x;
			screenRect.y = vector.y;
			return screenRect;
		}

		public static void RotateAroundPivot(float angle, Vector2 pivotPoint)
		{
			Matrix4x4 matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.identity;
			Vector2 vector = GUIClip.Unclip(pivotPoint);
			Matrix4x4 lhs = Matrix4x4.TRS(vector, Quaternion.Euler(0f, 0f, angle), Vector3.one) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
			GUI.matrix = lhs * matrix;
		}

		public static void ScaleAroundPivot(Vector2 scale, Vector2 pivotPoint)
		{
			Matrix4x4 matrix = GUI.matrix;
			Vector2 vector = GUIClip.Unclip(pivotPoint);
			Matrix4x4 lhs = Matrix4x4.TRS(vector, Quaternion.identity, new Vector3(scale.x, scale.y, 1f)) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
			GUI.matrix = lhs * matrix;
		}

		private static float Internal_GetPixelsPerPoint() { throw new NotImplementedException("なにこれ"); }

		public static int GetControlID(int hint, FocusType focus) { throw new NotImplementedException("なにこれ"); }

		private static int Internal_GetNextControlID2(int hint, FocusType focusType, Rect rect)
		{
			return INTERNAL_CALL_Internal_GetNextControlID2(hint, focusType, ref rect);
		}

		private static int INTERNAL_CALL_Internal_GetNextControlID2(int hint, FocusType focusType, ref Rect rect) { throw new NotImplementedException("なにこれ"); }

		internal static int GetPermanentControlID() { throw new NotImplementedException("なにこれ"); }

		private static int Internal_GetHotControl() { throw new NotImplementedException("なにこれ"); }

		private static void Internal_SetHotControl(int value) { throw new NotImplementedException("なにこれ"); }

		internal static void UpdateUndoName() { throw new NotImplementedException("なにこれ"); }

		internal static void SetDidGUIWindowsEatLastEvent(bool value) { throw new NotImplementedException("なにこれ"); }

		private static GUISkin Internal_GetDefaultSkin(int skinMode) { throw new NotImplementedException("なにこれ"); }

		private static Object Internal_GetBuiltinSkin(int skin) { throw new NotImplementedException("なにこれ"); }

		private static void Internal_ExitGUI() { throw new NotImplementedException("なにこれ"); }

		internal static int Internal_GetGUIDepth() { throw new NotImplementedException("なにこれ"); }
	}
}
