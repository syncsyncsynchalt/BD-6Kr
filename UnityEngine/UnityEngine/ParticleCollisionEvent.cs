using System.Runtime.CompilerServices;

namespace UnityEngine;

public struct ParticleCollisionEvent
{
	private Vector3 m_Intersection;

	private Vector3 m_Normal;

	private Vector3 m_Velocity;

	private int m_ColliderInstanceID;

	public Vector3 intersection => m_Intersection;

	public Vector3 normal => m_Normal;

	public Vector3 velocity => m_Velocity;

	public Collider collider => InstanceIDToCollider(m_ColliderInstanceID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern Collider InstanceIDToCollider(int instanceID);
}
