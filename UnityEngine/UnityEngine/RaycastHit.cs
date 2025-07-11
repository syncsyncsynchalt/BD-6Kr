using System;
using System.Runtime.CompilerServices;

namespace UnityEngine;

public struct RaycastHit
{
	private Vector3 m_Point;

	private Vector3 m_Normal;

	private int m_FaceID;

	private float m_Distance;

	private Vector2 m_UV;

	private Collider m_Collider;

	public Vector3 point
	{
		get
		{
			return m_Point;
		}
		set
		{
			m_Point = value;
		}
	}

	public Vector3 normal
	{
		get
		{
			return m_Normal;
		}
		set
		{
			m_Normal = value;
		}
	}

	public Vector3 barycentricCoordinate
	{
		get
		{
			return new Vector3(1f - (m_UV.y + m_UV.x), m_UV.x, m_UV.y);
		}
		set
		{
			m_UV = value;
		}
	}

	public float distance
	{
		get
		{
			return m_Distance;
		}
		set
		{
			m_Distance = value;
		}
	}

	public int triangleIndex => m_FaceID;

	public Vector2 textureCoord
	{
		get
		{
			CalculateRaycastTexCoord(out var output, collider, m_UV, m_Point, m_FaceID, 0);
			return output;
		}
	}

	public Vector2 textureCoord2
	{
		get
		{
			CalculateRaycastTexCoord(out var output, collider, m_UV, m_Point, m_FaceID, 1);
			return output;
		}
	}

	[Obsolete("Use textureCoord2 instead")]
	public Vector2 textureCoord1
	{
		get
		{
			CalculateRaycastTexCoord(out var output, collider, m_UV, m_Point, m_FaceID, 1);
			return output;
		}
	}

	public Vector2 lightmapCoord
	{
		get
		{
			CalculateRaycastTexCoord(out var output, collider, m_UV, m_Point, m_FaceID, 1);
			if (collider.GetComponent<Renderer>() != null)
			{
				Vector4 lightmapScaleOffset = collider.GetComponent<Renderer>().lightmapScaleOffset;
				output.x = output.x * lightmapScaleOffset.x + lightmapScaleOffset.z;
				output.y = output.y * lightmapScaleOffset.y + lightmapScaleOffset.w;
			}
			return output;
		}
	}

	public Collider collider => m_Collider;

	public Rigidbody rigidbody => (!(collider != null)) ? null : collider.attachedRigidbody;

	public Transform transform
	{
		get
		{
			Rigidbody rigidbody = this.rigidbody;
			if (rigidbody != null)
			{
				return rigidbody.transform;
			}
			if (collider != null)
			{
				return collider.transform;
			}
			return null;
		}
	}

	private static void CalculateRaycastTexCoord(out Vector2 output, Collider col, Vector2 uv, Vector3 point, int face, int index)
	{
		INTERNAL_CALL_CalculateRaycastTexCoord(out output, col, ref uv, ref point, face, index);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_CalculateRaycastTexCoord(out Vector2 output, Collider col, ref Vector2 uv, ref Vector3 point, int face, int index);
}
