using System;

namespace UnityEngine
{
	[Obsolete("NetworkView RPC functions are deprecated. Refer to the new Multiplayer Networking system.")]
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public sealed class RPC : Attribute
	{
	}
}
