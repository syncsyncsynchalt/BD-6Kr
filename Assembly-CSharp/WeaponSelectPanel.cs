using UnityEngine;

public class WeaponSelectPanel : MonoBehaviour
{
	private GameObject ListItem;

	private void Awake()
	{
		ListItem = base.transform.FindChild("Item").gameObject;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
