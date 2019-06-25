using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public struct ContactPoint
	{
		internal Vector3 m_Point;

		internal Vector3 m_Normal;

		internal int m_ThisColliderInstanceID;

		internal int m_OtherColliderInstanceID;

		public Vector3 point => m_Point;

		public Vector3 normal => m_Normal;

		public Collider thisCollider => ColliderFromInstanceId(m_ThisColliderInstanceID);

		public Collider otherCollider => ColliderFromInstanceId(m_OtherColliderInstanceID);

		private static Collider ColliderFromInstanceId(int instanceID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
