using UnityEngine;

public class prefabTest : MonoBehaviour
{
	public GameObject go;

	private void Awake()
	{
	}

	private void Start()
	{
		GameObject gameObject = Object.Instantiate(go);
		gameObject.transform.positionX(123f);
	}

	private void Update()
	{
	}
}
