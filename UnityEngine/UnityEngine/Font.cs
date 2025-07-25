using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine;

public sealed class Font : Object
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public delegate void FontTextureRebuildCallback();

	public extern Material material
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern string[] fontNames
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern CharacterInfo[] characterInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	[Obsolete("Font.textureRebuildCallback has been deprecated. Use Font.textureRebuilt instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public FontTextureRebuildCallback textureRebuildCallback
	{
		get
		{
			return this.m_FontTextureRebuildCallback;
		}
		set
		{
			this.m_FontTextureRebuildCallback = value;
		}
	}

	public extern bool dynamic
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern int ascent
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern int lineHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern int fontSize
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static event Action<Font> textureRebuilt;

	private event FontTextureRebuildCallback m_FontTextureRebuildCallback;

	public Font()
	{
		Internal_CreateFont(this, null);
	}

	public Font(string name)
	{
		Internal_CreateFont(this, name);
	}

	private Font(string[] names, int size)
	{
		Internal_CreateDynamicFont(this, names, size);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern string[] GetOSInstalledFontNames();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void Internal_CreateFont([Writable] Font _font, string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void Internal_CreateDynamicFont([Writable] Font _font, string[] _names, int size);

	public static Font CreateDynamicFontFromOSFont(string fontname, int size)
	{
		return new Font(new string[1] { fontname }, size);
	}

	public static Font CreateDynamicFontFromOSFont(string[] fontnames, int size)
	{
		return new Font(fontnames, size);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern bool HasCharacter(char c);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void RequestCharactersInTexture(string characters, [UnityEngine.Internal.DefaultValue("0")] int size, [UnityEngine.Internal.DefaultValue("FontStyle.Normal")] FontStyle style);

	[ExcludeFromDocs]
	public void RequestCharactersInTexture(string characters, int size)
	{
		FontStyle style = FontStyle.Normal;
		RequestCharactersInTexture(characters, size, style);
	}

	[ExcludeFromDocs]
	public void RequestCharactersInTexture(string characters)
	{
		FontStyle style = FontStyle.Normal;
		int size = 0;
		RequestCharactersInTexture(characters, size, style);
	}

	private static void InvokeTextureRebuilt_Internal(Font font)
	{
		Font.textureRebuilt?.Invoke(font);
		if (font.m_FontTextureRebuildCallback != null)
		{
			font.m_FontTextureRebuildCallback();
		}
	}

	public static int GetMaxVertsForString(string str)
	{
		return str.Length * 4 + 4;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern bool GetCharacterInfo(char ch, out CharacterInfo info, [UnityEngine.Internal.DefaultValue("0")] int size, [UnityEngine.Internal.DefaultValue("FontStyle.Normal")] FontStyle style);

	[ExcludeFromDocs]
	public bool GetCharacterInfo(char ch, out CharacterInfo info, int size)
	{
		FontStyle style = FontStyle.Normal;
		return GetCharacterInfo(ch, out info, size, style);
	}

	[ExcludeFromDocs]
	public bool GetCharacterInfo(char ch, out CharacterInfo info)
	{
		FontStyle style = FontStyle.Normal;
		int size = 0;
		return GetCharacterInfo(ch, out info, size, style);
	}
}
