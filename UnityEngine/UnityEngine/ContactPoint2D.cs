namespace UnityEngine
{
	public struct ContactPoint2D
	{
		internal Vector2 m_Point;

		internal Vector2 m_Normal;

		internal Collider2D m_Collider;

		internal Collider2D m_OtherCollider;

		public Vector2 point => m_Point;

		public Vector2 normal => m_Normal;

		public Collider2D collider => m_Collider;

		public Collider2D otherCollider => m_OtherCollider;
	}
}
