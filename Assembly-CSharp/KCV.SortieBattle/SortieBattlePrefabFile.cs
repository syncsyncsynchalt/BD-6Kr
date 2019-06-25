using KCV.SortieMap;
using System;
using UnityEngine;

namespace KCV.SortieBattle
{
	[Serializable]
	public class SortieBattlePrefabFile : BasePrefabFile
	{
		[SerializeField]
		private Transform _prefabBattleCutManager;

		[SerializeField]
		private Transform _prefabBattleTaskManager;

		[SerializeField]
		private Transform _prefabProdSortieTransitionToBattle;

		[SerializeField]
		private Transform _prefabUIBattleShutter;

		private ProdSortieTransitionToBattle _prodSortieTransitionToBattle;

		public Transform prefabBattleCutManager => _prefabBattleCutManager;

		public Transform prefabBattleTaskManager => _prefabBattleTaskManager;

		public ProdSortieTransitionToBattle prodSortieTransitionToBattle
		{
			get
			{
				if (_prodSortieTransitionToBattle == null)
				{
					_prodSortieTransitionToBattle = ProdSortieTransitionToBattle.Instantiate(((Component)_prefabProdSortieTransitionToBattle).GetComponent<ProdSortieTransitionToBattle>(), SortieBattleTaskManager.GetTransitionCamera().transform);
				}
				return _prodSortieTransitionToBattle;
			}
		}

		public Transform prefabUIBattleShutter => _prefabUIBattleShutter;

		protected override void Dispose(bool disposing)
		{
			Mem.Del(ref _prefabBattleCutManager);
			Mem.Del(ref _prefabBattleTaskManager);
			Mem.Del(ref _prefabProdSortieTransitionToBattle);
			Mem.Del(ref _prefabUIBattleShutter);
			Mem.Del(ref _prodSortieTransitionToBattle);
			base.Dispose(disposing);
		}

		public void DisposeProdSortieTransitionToBattle()
		{
			Mem.DelComponentSafe(ref _prodSortieTransitionToBattle);
		}
	}
}
