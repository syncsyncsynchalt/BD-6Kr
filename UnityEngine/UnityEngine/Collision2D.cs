using System.Runtime.InteropServices;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public class Collision2D
{
	internal Rigidbody2D m_Rigidbody;

	internal Collider2D m_Collider;

	internal ContactPoint2D[] m_Contacts;

	internal Vector2 m_RelativeVelocity;

	internal bool m_Enabled;

	public bool enabled => m_Enabled;

	public Rigidbody2D rigidbody => m_Rigidbody;

	public Collider2D collider => m_Collider;

	public Transform transform => (!(rigidbody != null)) ? collider.transform : rigidbody.transform;

	public GameObject gameObject => (!(m_Rigidbody != null)) ? m_Collider.gameObject : m_Rigidbody.gameObject;

	public ContactPoint2D[] contacts => m_Contacts;

	public Vector2 relativeVelocity => m_RelativeVelocity;
}
