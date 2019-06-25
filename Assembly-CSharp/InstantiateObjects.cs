using System.Collections;
using UnityEngine;

public class InstantiateObjects : MonoBehaviour
{
	[Range(0.1f, 1f)]
	public float interval = 1f;

	public GameObject prefab;

	private void Awake()
	{
		prefab.SetActive(false);
	}

	private IEnumerator Start()
	{
		while (true)
		{
			GameObject obj = (GameObject)Object.Instantiate(prefab, base.transform.position, Quaternion.identity);
			obj.GetComponent<Rigidbody>().AddTorque(Vector3.forward * 1000f);
			obj.SetActive(true);
			Object.Destroy(obj, 30f);
			Object.Destroy(obj.GetComponent<ConstantForce>(), 1f);
			yield return new WaitForSeconds(interval);
		}
	}
}
