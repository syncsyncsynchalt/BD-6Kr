using System;
using UnityEngine;

namespace KCV.BattleCut
{
	[Serializable]
	public class BattleCutPrefabFile : BasePrefabFile
	{
		[SerializeField]
		private Transform _prefabProdFormation;

		[SerializeField]
		private Transform _prefabCtrlBCCommandSelect;

		[SerializeField]
		private Transform _prefabProdBCBattle;

		[SerializeField]
		private Transform _prefabProdWithdrawalDecision;

		[SerializeField]
		private Transform _prefabProdBattleEnd;

		[SerializeField]
		private Transform _prefabProdWinRunkJudge;

		[SerializeField]
		private Transform _prefabProdResult;

		[SerializeField]
		private Transform _prefabProdBCAdvancingWithdrawal;

		[SerializeField]
		private Transform _prefabProdBCAdvancingWithdrawalDC;

		[SerializeField]
		private Transform _prefabProdFlagshipWreck;

		public Transform prefabProdFormation => BasePrefabFile.PassesPrefab(ref _prefabProdFormation);

		public Transform prefabCtrlBCCommandSelect => BasePrefabFile.PassesPrefab(ref _prefabCtrlBCCommandSelect);

		public Transform prefabProdBCBattle => BasePrefabFile.PassesPrefab(ref _prefabProdBCBattle);

		public Transform prefabProdWithdrawalDecision => BasePrefabFile.PassesPrefab(ref _prefabProdWithdrawalDecision);

		public Transform prefabProdBattleEnd => BasePrefabFile.PassesPrefab(ref _prefabProdBattleEnd);

		public Transform prefabProdWinRunkJudge => BasePrefabFile.PassesPrefab(ref _prefabProdWinRunkJudge);

		public Transform prefabProdResult => BasePrefabFile.PassesPrefab(ref _prefabProdResult);

		public Transform prefabProdBCAdvancingWithdrawal => BasePrefabFile.PassesPrefab(ref _prefabProdBCAdvancingWithdrawal);

		public Transform prefabProdBCAdvancingWithdrawalDC => BasePrefabFile.PassesPrefab(ref _prefabProdBCAdvancingWithdrawalDC);

		public Transform prefabProdFlagshipWreck => BasePrefabFile.PassesPrefab(ref _prefabProdFlagshipWreck);

		public bool Init()
		{
			return true;
		}

		public bool UnInit()
		{
			return true;
		}

		protected override void Dispose(bool disposing)
		{
			Mem.Del(ref _prefabProdFormation);
			Mem.Del(ref _prefabCtrlBCCommandSelect);
			Mem.Del(ref _prefabProdBCBattle);
			Mem.Del(ref _prefabProdWithdrawalDecision);
			Mem.Del(ref _prefabProdBattleEnd);
			Mem.Del(ref _prefabProdWinRunkJudge);
			Mem.Del(ref _prefabProdResult);
			Mem.Del(ref _prefabProdBCAdvancingWithdrawal);
			Mem.Del(ref _prefabProdBCAdvancingWithdrawalDC);
			Mem.Del(ref _prefabProdFlagshipWreck);
			base.Dispose(disposing);
		}
	}
}
