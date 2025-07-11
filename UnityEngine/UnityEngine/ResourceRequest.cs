using System;
using System.Runtime.InteropServices;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public sealed class ResourceRequest : AsyncOperation
{
	internal string m_Path;

	internal Type m_Type;

	public Object asset => Resources.Load(m_Path, m_Type);
}
