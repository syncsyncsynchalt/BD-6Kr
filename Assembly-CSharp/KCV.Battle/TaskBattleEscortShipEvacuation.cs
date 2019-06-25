using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.SortieBattle;
using local.managers;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleEscortShipEvacuation : BaseBattleTask
	{
		[SerializeField]
		private Transform _prefabEscortShipEvacuation;

		private ProdEscortShipEvacuation _prodEscortShipEvacuation;

		protected override bool Init()
		{
			BattleManager battleManager = BattleTaskManager.GetBattleManager();
			if (battleManager.GetEscapeCandidate() != null)
			{
				_prodEscortShipEvacuation = ProdEscortShipEvacuation.Instantiate((!(_prefabEscortShipEvacuation != null)) ? PrefabFile.Load<ProdEscortShipEvacuation>(PrefabFileInfos.BattleProdEscortShipEvacuation) : ((Component)_prefabEscortShipEvacuation).GetComponent<ProdEscortShipEvacuation>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform, BattleTaskManager.GetKeyControl(), BattleTaskManager.GetBattleManager().GetEscapeCandidate(), isBattleCut: false);
				_prodEscortShipEvacuation.Init();
				_prodEscortShipEvacuation.Play(DecideAdvancinsWithDrawalBtn);
			}
			else
			{
				if (battleManager.Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && ShipUtils.HasRepair(battleManager.Ships_f[0]))
				{
					BattleTaskManager.ReqPhase(BattlePhase.AdvancingWithdrawalDC);
				}
				else
				{
					BattleTaskManager.ReqPhase(BattlePhase.AdvancingWithdrawal);
				}
				ImmediateTermination();
			}
			return true;
		}

		protected override bool UnInit()
		{
			_prefabEscortShipEvacuation = null;
			if (_prodEscortShipEvacuation != null)
			{
				_prodEscortShipEvacuation.Discard();
			}
			_prodEscortShipEvacuation = null;
			return true;
		}

		protected override bool Update()
		{
			_prodEscortShipEvacuation.Run();
			return ChkChangePhase(BattlePhase.EscortShipEvacuation);
		}

		private void DecideAdvancinsWithDrawalBtn(UIHexButton btn)
		{
			BattleManager manager = BattleTaskManager.GetBattleManager();
			if (btn.index == 0)
			{
				Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate
				{
					BattleTaskManager.GetBattleManager().SendOffEscapes();
					RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawalDC(SortieBattleTaskManager.GetMapManager(), ShipRecoveryType.None));
					if (manager.Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && ShipUtils.HasRepair(manager.Ships_f[0]))
					{
						BattleTaskManager.ReqPhase(BattlePhase.AdvancingWithdrawalDC);
					}
					else
					{
						BattleTaskManager.ReqPhase(BattlePhase.AdvancingWithdrawal);
					}
				});
			}
			else
			{
				Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate
				{
					RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawalDC(SortieBattleTaskManager.GetMapManager(), ShipRecoveryType.None));
					if (manager.Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && ShipUtils.HasRepair(manager.Ships_f[0]))
					{
						BattleTaskManager.ReqPhase(BattlePhase.AdvancingWithdrawalDC);
					}
					else
					{
						BattleTaskManager.ReqPhase(BattlePhase.AdvancingWithdrawal);
					}
				});
			}
		}
	}
}
