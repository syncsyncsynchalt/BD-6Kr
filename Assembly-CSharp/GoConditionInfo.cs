using Common.Enum;
using local.models;
using local.utils;
using System.Collections.Generic;
using UnityEngine;

public class GoConditionInfo : MonoBehaviour
{
	[SerializeField]
	private UILabel GoConditionText;

	[SerializeField]
	private GoSortieEnableShipTypes enableShipTypes;

	public void Initialize(MapModel model)
	{
		HashSet<SType> sortieLimit = Utils.GetSortieLimit(model.MstId, is_permitted: true);
		HashSet<SType> sortieLimit2 = Utils.GetSortieLimit(model.MstId, is_permitted: false);
		if (sortieLimit == null && sortieLimit2 == null)
		{
			GoConditionText.SetActive(isActive: true);
			enableShipTypes.SetActive(isActive: false);
		}
		else
		{
			GoConditionText.SetActive(isActive: false);
			enableShipTypes.SetActive(isActive: true);
			enableShipTypes.Initialize(model, sortieLimit, sortieLimit2);
		}
	}
}
