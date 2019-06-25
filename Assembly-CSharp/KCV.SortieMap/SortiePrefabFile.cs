using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[Serializable]
	public class SortiePrefabFile : BasePrefabFile
	{
		[SerializeField]
		private Transform _prefabUISortieShip;

		[SerializeField]
		private Transform _prefabUICompassManager;

		[SerializeField]
		private Transform _prefabBattleCutManager;

		[SerializeField]
		private Transform _prefabUIBattleFormationKindSelectManager;

		[SerializeField]
		private Transform _prefabProdSortieTransitionToBattle;

		[SerializeField]
		private Transform _prefabProdShipRipple;

		[SerializeField]
		private Transform _prefabProdMaelstrom;

		[SerializeField]
		private Transform _prefabUIAreaGauge;

		[SerializeField]
		private Transform _prefabProdSortieEnd;

		[SerializeField]
		private Transform _prefabCtrlSortieResult;

		[SerializeField]
		private Transform _prefabProdUnderwayReplenishment;

		public Transform prefabUISortieShip => BasePrefabFile.PassesPrefab(ref _prefabUISortieShip);

		public Transform prefabUICompassManager => _prefabUICompassManager;

		public Transform prefabBattleCutManager => _prefabBattleCutManager;

		public Transform prefabUIBattleFormationKindSelectManager => _prefabUIBattleFormationKindSelectManager;

		public Transform prefabProdSortieTransitionToBattle => _prefabProdSortieTransitionToBattle;

		public Transform prefabProdShipRipple => _prefabProdShipRipple;

		public Transform prefabProdMaelstrom => _prefabProdMaelstrom;

		public Transform prefabUIAreaGauge => BasePrefabFile.PassesPrefab(ref _prefabUIAreaGauge);

		public Transform prodSortieEnd => BasePrefabFile.PassesPrefab(ref _prefabProdSortieEnd);

		public Transform prefabCtrlSortieResult => BasePrefabFile.PassesPrefab(ref _prefabCtrlSortieResult);

		public Transform prefabProdUnderwayReplenishment => _prefabProdUnderwayReplenishment;

		public bool Init()
		{
			return true;
		}

		protected override void Dispose(bool disposing)
		{
			Mem.Del(ref _prefabUISortieShip);
			Mem.Del(ref _prefabUICompassManager);
			Mem.Del(ref _prefabBattleCutManager);
			Mem.Del(ref _prefabUIBattleFormationKindSelectManager);
			Mem.Del(ref _prefabProdSortieTransitionToBattle);
			Mem.Del(ref _prefabProdShipRipple);
			Mem.Del(ref _prefabProdMaelstrom);
			Mem.Del(ref _prefabUIAreaGauge);
			Mem.Del(ref _prefabCtrlSortieResult);
			Mem.Del(ref _prefabProdUnderwayReplenishment);
			base.Dispose(disposing);
		}
	}
}
