using System.Runtime.CompilerServices;

namespace UnityEngine;

internal sealed class GUIClip
{
	public static extern bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static Rect topmostRect
	{
		get
		{
			INTERNAL_get_topmostRect(out var value);
			return value;
		}
	}

	public static Rect visibleRect
	{
		get
		{
			INTERNAL_get_visibleRect(out var value);
			return value;
		}
	}

	public static Vector2 Unclip(Vector2 pos)
	{
		Unclip_Vector2(ref pos);
		return pos;
	}

	public static Rect Unclip(Rect rect)
	{
		Unclip_Rect(ref rect);
		return rect;
	}

	public static Vector2 Clip(Vector2 absolutePos)
	{
		Clip_Vector2(ref absolutePos);
		return absolutePos;
	}

	public static Rect Clip(Rect absoluteRect)
	{
		Internal_Clip_Rect(ref absoluteRect);
		return absoluteRect;
	}

	public static Vector2 GetAbsoluteMousePosition()
	{
		Internal_GetAbsoluteMousePosition(out var output);
		return output;
	}

	internal static void Push(Rect screenRect, Vector2 scrollOffset, Vector2 renderOffset, bool resetOffset)
	{
		INTERNAL_CALL_Push(ref screenRect, ref scrollOffset, ref renderOffset, resetOffset);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_Push(ref Rect screenRect, ref Vector2 scrollOffset, ref Vector2 renderOffset, bool resetOffset);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern void Pop();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern Rect GetTopRect();

	private static void Unclip_Vector2(ref Vector2 pos)
	{
		INTERNAL_CALL_Unclip_Vector2(ref pos);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_Unclip_Vector2(ref Vector2 pos);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_get_topmostRect(out Rect value);

	private static void Unclip_Rect(ref Rect rect)
	{
		INTERNAL_CALL_Unclip_Rect(ref rect);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_Unclip_Rect(ref Rect rect);

	private static void Clip_Vector2(ref Vector2 absolutePos)
	{
		INTERNAL_CALL_Clip_Vector2(ref absolutePos);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_Clip_Vector2(ref Vector2 absolutePos);

	private static void Internal_Clip_Rect(ref Rect absoluteRect)
	{
		INTERNAL_CALL_Internal_Clip_Rect(ref absoluteRect);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_Internal_Clip_Rect(ref Rect absoluteRect);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern void Reapply();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern Matrix4x4 GetMatrix();

	internal static void SetMatrix(Matrix4x4 m)
	{
		INTERNAL_CALL_SetMatrix(ref m);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_SetMatrix(ref Matrix4x4 m);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_get_visibleRect(out Rect value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void Internal_GetAbsoluteMousePosition(out Vector2 output);
}
