using Common.Enum;
using KCV.Battle.Production;
using local.managers;
using local.models.battle;
using UnityEngine;

namespace KCV.Battle
{
	public class BattleTest : BaseBattleTask
	{
		private static SortieMapManager _clsSortieMapManager;

		private static SortieBattleManager _clsSortieBattleManager;

		private static PracticeBattleManager _clsPracticeBattleManager;

		private static BattleManager _clsBattleManager;

		private KoukuuModel model;

		private ProdMapOpen mapOpnen;

		public GameObject currentDetonator;

		private int _currentExpIdx = -1;

		public GameObject[] detonatorPrefabs;

		private KeyControl keyInput;

		private void Awake()
		{
			keyInput = new KeyControl();
		}

		private void Start()
		{
			StrategyMapManager strategyMapManager = new StrategyMapManager();
			SortieManager sortieManager = strategyMapManager.SelectArea(1);
			_clsSortieMapManager = sortieManager.GoSortie(1, 11);
			sortieManager.GoSortie(1, 11);
			_clsSortieBattleManager = _clsSortieMapManager.BattleStart(BattleFormationKinds1.FukuJuu);
			GameObject gameObject = (GameObject)Object.Instantiate(position: new Vector3(0f, 0f, 0f), original: currentDetonator, rotation: Quaternion.identity);
		}

		private new void Update()
		{
			keyInput.Update();
			if (keyInput.keyState[1].down)
			{
				Debug.Log("WW");
				GameObject gameObject = (GameObject)Object.Instantiate(position: new Vector3(0f, 0f, 0f), original: currentDetonator, rotation: Quaternion.identity);
			}
		}

		private void testDet()
		{
		}
	}
}
