using Common.Enum;
using KCV.Battle.Formation;
using System.Collections.Generic;
using UnityEngine;

public class tester : MonoBehaviour
{
	[SerializeField]
	private UIBattleFormationKindSelector mPrefab_UIBattleFormationKindSelector;

	private void Start()
	{
		UIBattleFormationKindSelector component = Util.Instantiate(mPrefab_UIBattleFormationKindSelector.gameObject, base.gameObject).GetComponent<UIBattleFormationKindSelector>();
		List<BattleFormationKinds1> list = new List<BattleFormationKinds1>();
		list.Add(BattleFormationKinds1.FukuJuu);
		list.Add(BattleFormationKinds1.Rinkei);
		list.Add(BattleFormationKinds1.Teikei);
		list.Add(BattleFormationKinds1.TanOu);
		list.Add(BattleFormationKinds1.TanJuu);
		component.Initialize(list.ToArray());
		component.Show();
	}

	private void Update()
	{
	}
}
