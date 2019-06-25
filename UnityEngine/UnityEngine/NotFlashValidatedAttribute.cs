using System;

namespace UnityEngine
{
	[Obsolete("NotFlashValidatedAttribute was used for the Flash buildtarget, which is not supported anymore starting Unity 5.0", true)]
	[NotConverted]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface)]
	public sealed class NotFlashValidatedAttribute : Attribute
	{
	}
}
