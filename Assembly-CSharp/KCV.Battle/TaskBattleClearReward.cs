using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.Production;
using local.models;
using local.models.battle;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleClearReward : BaseBattleTask
	{
		private Transform _prefabProdRewardGet;

		private Transform _prefabProdReceiveShip;

		private BattleResultModel _clsBattleResult;

		private ProdBattleReceiveShip _prodReceiveShip;

		private Reward_Ship[] _clsRewardShips;

		private List<IReward> _listRewardModels;

		protected override bool Init()
		{
			_clsState = new StatementMachine();
			_clsBattleResult = BattleTaskManager.GetBattleManager().GetBattleResult();
			_listRewardModels = _clsBattleResult.GetRewardItems();
			if (_listRewardModels.Count > 0)
			{
				for (int i = 0; i < _listRewardModels.Count; i++)
				{
					if (_listRewardModels[i] is IReward_Ship)
					{
						_clsState.AddState(_initShipGet, _updateShipGet);
					}
					if (_listRewardModels[i] is IReward_Slotitem)
					{
						_clsState.AddState(_initSlotItemGet, _updateSlotItemGet);
					}
					if (_listRewardModels[i] is IReward_Useitem)
					{
						_clsState.AddState(_initUseItemGet, _updateUseItemGet);
					}
				}
			}
			else
			{
				BattleTaskManager.ReqPhase(BattleUtils.NextPhase(BattlePhase.ClearReward));
				ImmediateTermination();
			}
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			Mem.DelListSafe(ref _listRewardModels);
			_clsBattleResult = null;
			return true;
		}

		protected override bool Update()
		{
			_clsState.OnUpdate(Time.deltaTime);
			return ChkChangePhase(BattlePhase.ClearReward);
		}

		private bool _initShipGet(object data)
		{
			Observable.FromCoroutine(PlayShipGet).Subscribe();
			return false;
		}

		private bool _updateShipGet(object data)
		{
			return true;
		}

		private IEnumerator PlayShipGet()
		{
			ProdRewardGet prodReward = ProdRewardGet.Instantiate(PrefabFile.Load<ProdRewardGet>(PrefabFileInfos.RewardGet), BattleTaskManager.GetBattleCameras().cutInCamera.transform, 110, ProdRewardGet.RewardType.Ship);
			yield return new WaitForSeconds(0.1f);
			prodReward.Play(delegate
			{
				Observable.FromCoroutine(this.PlayReceiveShip).Subscribe();
			});
		}

		private IEnumerator PlayReceiveShip()
		{
			_clsRewardShips = new Reward_Ship[_listRewardModels.Count];
			_clsRewardShips[0] = (Reward_Ship)_listRewardModels[0];
			yield return new WaitForSeconds(0.5f);
			_prodReceiveShip = ProdBattleReceiveShip.Instantiate(PrefabFile.Load<ProdBattleReceiveShip>(PrefabFileInfos.CommonProdBattleReceiveShip), BattleTaskManager.GetBattleCameras().cutInCamera.transform, _clsRewardShips[0], 120, BattleTaskManager.GetKeyControl());
			yield return new WaitForSeconds(0.1f);
			_prodReceiveShip.Play(delegate
			{
				BattleTaskManager.ReqPhase(BattleUtils.NextPhase(BattlePhase.ClearReward));
			});
		}

		private bool _initSlotItemGet(object data)
		{
			return false;
		}

		private bool _updateSlotItemGet(object data)
		{
			return false;
		}

		private bool _initUseItemGet(object data)
		{
			return false;
		}

		private bool _updateUseItemGet(object data)
		{
			return false;
		}
	}
}
