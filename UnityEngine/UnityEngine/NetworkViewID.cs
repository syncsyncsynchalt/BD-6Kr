using System.Runtime.CompilerServices;

namespace UnityEngine;

public struct NetworkViewID
{
	private int a;

	private int b;

	private int c;

	public static NetworkViewID unassigned
	{
		get
		{
			INTERNAL_get_unassigned(out var value);
			return value;
		}
	}

	public bool isMine => Internal_IsMine(this);

	public NetworkPlayer owner
	{
		get
		{
			Internal_GetOwner(this, out var player);
			return player;
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_get_unassigned(out NetworkViewID value);

	internal static bool Internal_IsMine(NetworkViewID value)
	{
		return INTERNAL_CALL_Internal_IsMine(ref value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern bool INTERNAL_CALL_Internal_IsMine(ref NetworkViewID value);

	internal static void Internal_GetOwner(NetworkViewID value, out NetworkPlayer player)
	{
		INTERNAL_CALL_Internal_GetOwner(ref value, out player);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_Internal_GetOwner(ref NetworkViewID value, out NetworkPlayer player);

	internal static string Internal_GetString(NetworkViewID value)
	{
		return INTERNAL_CALL_Internal_GetString(ref value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern string INTERNAL_CALL_Internal_GetString(ref NetworkViewID value);

	internal static bool Internal_Compare(NetworkViewID lhs, NetworkViewID rhs)
	{
		return INTERNAL_CALL_Internal_Compare(ref lhs, ref rhs);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern bool INTERNAL_CALL_Internal_Compare(ref NetworkViewID lhs, ref NetworkViewID rhs);

	public override int GetHashCode()
	{
		return a ^ b ^ c;
	}

	public override bool Equals(object other)
	{
		if (!(other is NetworkViewID rhs))
		{
			return false;
		}
		return Internal_Compare(this, rhs);
	}

	public override string ToString()
	{
		return Internal_GetString(this);
	}

	public static bool operator ==(NetworkViewID lhs, NetworkViewID rhs)
	{
		return Internal_Compare(lhs, rhs);
	}

	public static bool operator !=(NetworkViewID lhs, NetworkViewID rhs)
	{
		return !Internal_Compare(lhs, rhs);
	}
}
