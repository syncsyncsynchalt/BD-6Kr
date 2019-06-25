using UnityEngine;

public class TargetRotation : MonoBehaviour
{
	public float angle = 30f;

	public Transform target;

	private Vector3 targetPos;

	public Vector3 startPos;

	public bool horizontal;

	public bool vertical;

	private void Start()
	{
		targetPos = target.position;
		base.transform.Rotate(new Vector3(0f, 0f, 0f), Space.World);
		Vector3 localPosition = base.transform.localPosition;
		float x = localPosition.x;
		Vector3 localPosition2 = base.transform.localPosition;
		float y = localPosition2.y;
		Vector3 localPosition3 = base.transform.localPosition;
		startPos = new Vector3(x, y, localPosition3.z);
	}

	private void Update()
	{
	}

	private void OnValidate()
	{
		targetPos = target.position;
		base.transform.localPosition = new Vector3(startPos.x, startPos.y, startPos.z);
		Vector3 direction = new Vector3(1f, 0f, 0f);
		if (horizontal)
		{
			direction = new Vector3(0f, 1f, 0f);
		}
		Vector3 vector = base.transform.TransformDirection(direction);
		base.transform.RotateAround(targetPos, vector, angle);
		base.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
	}
}
