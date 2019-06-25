using UnityEngine;

public class GyroCube : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.transform.rotation = Input.gyro.attitude;
	}
}
