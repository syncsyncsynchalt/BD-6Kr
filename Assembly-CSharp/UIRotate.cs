using UnityEngine;

public class UIRotate : MonoBehaviour
{
	public float speed;

	public float rotateRange;

	private float rotateCount;

	private bool UseReverse;

	private void Start()
	{
		if (rotateRange != 0f)
		{
			UseReverse = true;
		}
		rotateCount = 0f;
	}

	private void Update()
	{
		if (UseReverse && (rotateCount < 0f - rotateRange || rotateCount > rotateRange))
		{
			speed *= -1f;
		}
		base.gameObject.transform.Rotate(0f, 0f, speed * Time.deltaTime, Space.World);
		rotateCount += speed * Time.deltaTime;
	}
}
