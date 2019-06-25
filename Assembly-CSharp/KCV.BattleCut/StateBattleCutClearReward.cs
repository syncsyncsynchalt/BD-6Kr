using KCV.Battle.Production;
using KCV.Production;
using local.models;
using local.models.battle;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutClearReward : BaseBattleCutState
	{
		private Transform _prefabProdRewardGet;

		private Transform _prefabProdReceiveShip;

		private BattleResultModel _clsResult;

		private ProdCutReceiveShip _prodReceiveShip;

		private StatementMachine _clsState;

		private List<Reward_Ship> _listRewardShips;

		private List<IReward> _listRewardModels;

		public override bool Init(object data)
		{
			_clsState = new StatementMachine();
			_clsResult = BattleCutManager.GetBattleManager().GetBattleResult();
			_listRewardModels = new List<IReward>(_clsResult.GetRewardItems());
			if (_listRewardModels.Count > 0)
			{
				_clsState.AddState(InitShipGet, UpdateShipGet);
			}
			else
			{
				BattleCutManager.ReqPhase(BattleCutPhase.MapOpen);
			}
			return false;
		}

		public override bool Terminate(object data)
		{
			_clsResult = null;
			_prodReceiveShip = null;
			if (_clsState != null)
			{
				_clsState.Clear();
			}
			_clsState = null;
			Mem.DelListSafe(ref _listRewardShips);
			Mem.DelListSafe(ref _listRewardModels);
			return false;
		}

		public override bool Run(object data)
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return IsCheckPhase(BattleCutPhase.ClearReward);
		}

		private bool InitShipGet(object data)
		{
			Observable.FromCoroutine(PlayShipGet).Subscribe();
			return false;
		}

		private bool UpdateShipGet(object data)
		{
			return false;
		}

		private IEnumerator PlayShipGet()
		{
			ProdRewardGet prodReward = ProdRewardGet.Instantiate(PrefabFile.Load<ProdRewardGet>(PrefabFileInfos.RewardGet), BattleCutManager.GetSharedPlase(), 110, ProdRewardGet.RewardType.Ship);
			yield return new WaitForSeconds(0.1f);
			prodReward.Play(delegate
			{
				Observable.FromCoroutine(this.PlayReceiveShip).Subscribe();
			});
		}

		private IEnumerator PlayReceiveShip()
		{
			_listRewardShips = new List<Reward_Ship>(_listRewardModels.Count);
			_listRewardShips.Add((Reward_Ship)_listRewardModels[0]);
			yield return new WaitForSeconds(0.5f);
			_prodReceiveShip = ProdCutReceiveShip.Instantiate(PrefabFile.Load<ProdCutReceiveShip>(PrefabFileInfos.CommonProdCutReceiveShip), BattleCutManager.GetSharedPlase(), _listRewardShips[0], 120, BattleCutManager.GetKeyControl());
			yield return new WaitForSeconds(0.1f);
			_prodReceiveShip.Play(delegate
			{
				BattleCutManager.ReqPhase(BattleCutPhase.MapOpen);
			});
		}

		private bool InitSlotItemGet(object data)
		{
			return false;
		}

		private bool UpdateSlotItemGet(object data)
		{
			return false;
		}

		private bool InitUseItemGet(object data)
		{
			return false;
		}

		private bool UpdateUseItemGet(object data)
		{
			return false;
		}
	}
}
