using Common.Enum;
using local.models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoSortieEnableShipTypes : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab_ShipTypeIcon;

	private List<GameObject> ShipTypeIcons;

	[SerializeField]
	private UILabel Message;

	public int Space = 10;

	public void Initialize(MapModel model, HashSet<SType> LimitTypeEnable, HashSet<SType> LimitTypeDisable)
	{
		if (ShipTypeIcons != null)
		{
			Release();
		}
		else
		{
			ShipTypeIcons = new List<GameObject>();
		}
		HashSet<SType> hashSet = (LimitTypeEnable == null) ? LimitTypeDisable : LimitTypeEnable;
		int num = 0;
		List<SType> list = hashSet.ToList();
		list.Sort((SType a, SType b) => a - b);
		foreach (SType item in list)
		{
			UISprite component = Util.Instantiate(prefab_ShipTypeIcon, base.gameObject).GetComponent<UISprite>();
			component.enabled = true;
			component.spriteName = "ship" + (int)item;
			component.MakePixelPerfect();
			component.transform.localPositionX(num);
			num += component.width + Space;
			ShipTypeIcons.Add(component.gameObject);
		}
		Message.text = ((hashSet != LimitTypeEnable) ? "は[FF0000]出撃不可[-]です。" : "のみ[66ccff]出撃可能[-]です。");
		Message.transform.localPositionX(num);
	}

	public void Release()
	{
		foreach (GameObject shipTypeIcon in ShipTypeIcons)
		{
			Object.Destroy(shipTypeIcon);
		}
	}

	private void OnDestroy()
	{
		prefab_ShipTypeIcon = null;
		if (ShipTypeIcons != null)
		{
			ShipTypeIcons.Clear();
		}
		ShipTypeIcons = null;
		Message = null;
	}
}
