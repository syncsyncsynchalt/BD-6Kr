using System.Runtime.CompilerServices;

namespace UnityEngine;

public struct Hash128
{
	private uint m_u32_0;

	private uint m_u32_1;

	private uint m_u32_2;

	private uint m_u32_3;

	public bool isValid => m_u32_0 != 0 || m_u32_1 != 0 || m_u32_2 != 0 || m_u32_3 != 0;

	public Hash128(uint u32_0, uint u32_1, uint u32_2, uint u32_3)
	{
		m_u32_0 = u32_0;
		m_u32_1 = u32_1;
		m_u32_2 = u32_2;
		m_u32_3 = u32_3;
	}

	public override string ToString()
	{
		return Internal_Hash128ToString(m_u32_0, m_u32_1, m_u32_2, m_u32_3);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern Hash128 Parse(string hashString);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern string Internal_Hash128ToString(uint d0, uint d1, uint d2, uint d3);
}
