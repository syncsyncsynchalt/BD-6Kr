using System;

namespace UnityEngine
{
	[Flags]
	public enum ComputeBufferType
	{
		Default = 0x0,
		Raw = 0x1,
		Append = 0x2,
		Counter = 0x4,
		DrawIndirect = 0x100
	}
}
