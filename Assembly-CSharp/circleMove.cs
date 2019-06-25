using System;
using UnityEngine;

public class circleMove : MonoBehaviour
{
	public GameObject target;

	public Vector3 offset;

	public float x;

	public float y;

	public float z;

	public float a;

	public void Start()
	{
	}

	public void Update()
	{
		if (target != null)
		{
			Vector3 vector = Quaternion.Euler(0f, y, 0f) * offset;
			base.transform.position = vector + target.transform.position;
		}
	}

	private void OnValidate()
	{
		if (target != null)
		{
			a = Mathf.Sin(y / 360f * (float)Math.PI);
			Vector3 vector = Quaternion.Euler(x, y, z) * offset * Mathf.Sin(y * (float)Math.PI);
			base.transform.position = vector + target.transform.position;
		}
	}
}
