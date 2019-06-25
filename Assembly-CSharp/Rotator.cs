using UnityEngine;

public class Rotator : MonoBehaviour
{
	private void Update()
	{
		base.transform.Rotate(0f, 30f * Time.deltaTime, 0f);
	}
}
