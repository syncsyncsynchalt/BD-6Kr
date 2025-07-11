using System.Runtime.CompilerServices;

namespace UnityEngine;

internal sealed class ParticleSystemExtensionsImpl
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern int GetSafeCollisionEventSize(ParticleSystem ps);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern int GetCollisionEvents(ParticleSystem ps, GameObject go, ParticleCollisionEvent[] collisionEvents);
}
