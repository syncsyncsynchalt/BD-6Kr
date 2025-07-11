using UnityEngine.Internal;

namespace UnityEngine;

public sealed class TouchScreenKeyboard
{
	public string text
	{
		get
		{
			return string.Empty;
		}
		set
		{
		}
	}

	public static bool hideInput
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool active
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool done => true;

	public bool wasCanceled => false;

	private static Rect area => default(Rect);

	private static bool visible => false;

	public static bool isSupported => false;

	[ExcludeFromDocs]
	public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline, bool secure, bool alert)
	{
		string empty = string.Empty;
		return Open(text, keyboardType, autocorrection, multiline, secure, alert, empty);
	}

	[ExcludeFromDocs]
	public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline, bool secure)
	{
		string empty = string.Empty;
		bool alert = false;
		return Open(text, keyboardType, autocorrection, multiline, secure, alert, empty);
	}

	[ExcludeFromDocs]
	public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline)
	{
		string empty = string.Empty;
		bool alert = false;
		bool secure = false;
		return Open(text, keyboardType, autocorrection, multiline, secure, alert, empty);
	}

	[ExcludeFromDocs]
	public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection)
	{
		string empty = string.Empty;
		bool alert = false;
		bool secure = false;
		bool multiline = false;
		return Open(text, keyboardType, autocorrection, multiline, secure, alert, empty);
	}

	[ExcludeFromDocs]
	public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType)
	{
		string empty = string.Empty;
		bool alert = false;
		bool secure = false;
		bool multiline = false;
		bool autocorrection = true;
		return Open(text, keyboardType, autocorrection, multiline, secure, alert, empty);
	}

	[ExcludeFromDocs]
	public static TouchScreenKeyboard Open(string text)
	{
		string empty = string.Empty;
		bool alert = false;
		bool secure = false;
		bool multiline = false;
		bool autocorrection = true;
		TouchScreenKeyboardType keyboardType = TouchScreenKeyboardType.Default;
		return Open(text, keyboardType, autocorrection, multiline, secure, alert, empty);
	}

	public static TouchScreenKeyboard Open(string text, [DefaultValue("TouchScreenKeyboardType.Default")] TouchScreenKeyboardType keyboardType, [DefaultValue("true")] bool autocorrection, [DefaultValue("false")] bool multiline, [DefaultValue("false")] bool secure, [DefaultValue("false")] bool alert, [DefaultValue("\"\"")] string textPlaceholder)
	{
		return null;
	}
}
