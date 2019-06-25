using Common.Enum;
using KCV.Battle;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using local.managers;
using System;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutEscortShipEvacuation : BaseBattleCutState
	{
		[SerializeField]
		private Transform prefabProdEscortShipEvacuation;

		private ProdEscortShipEvacuation _prodEscortShipEvacuation;

		private AsyncOperation _async;

		public override bool Init(object data)
		{
			BattleManager battleManager = BattleCutManager.GetBattleManager();
			if (battleManager.GetEscapeCandidate() != null)
			{
				_prodEscortShipEvacuation = ProdEscortShipEvacuation.Instantiate((!(prefabProdEscortShipEvacuation != null)) ? PrefabFile.Load<ProdEscortShipEvacuation>(PrefabFileInfos.BattleProdEscortShipEvacuation) : ((Component)prefabProdEscortShipEvacuation).GetComponent<ProdEscortShipEvacuation>(), BattleCutManager.GetSharedPlase(), BattleCutManager.GetKeyControl(), BattleCutManager.GetBattleManager().GetEscapeCandidate(), isBattleCut: true);
				_prodEscortShipEvacuation.Init();
				_prodEscortShipEvacuation.Play(DecideAdvancinsWithDrawalBtn);
			}
			else if (battleManager.Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && ShipUtils.HasRepair(battleManager.Ships_f[0]))
			{
				BattleCutManager.ReqPhase(BattleCutPhase.AdvancingWithdrawalDC);
			}
			else
			{
				BattleCutManager.ReqPhase(BattleCutPhase.AdvancingWithdrawal);
			}
			return false;
		}

		public override bool Terminate(object data)
		{
			prefabProdEscortShipEvacuation = null;
			if (_prodEscortShipEvacuation != null)
			{
				_prodEscortShipEvacuation.Discard();
			}
			_prodEscortShipEvacuation = null;
			return false;
		}

		public override bool Run(object data)
		{
			if (_prodEscortShipEvacuation != null)
			{
				_prodEscortShipEvacuation.Run();
			}
			return IsCheckPhase(BattleCutPhase.EscortShipEvacuation);
		}

		private void DecideAdvancinsWithDrawalBtn(UIHexButton btn)
		{
			BattleManager manager = BattleCutManager.GetBattleManager();
			if (btn.index == 0)
			{
				Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate
				{
					BattleCutManager.GetBattleManager().SendOffEscapes();
					RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawalDC(BattleCutManager.GetMapManager(), ShipRecoveryType.None));
					if (manager.Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && ShipUtils.HasRepair(manager.Ships_f[0]))
					{
						BattleCutManager.ReqPhase(BattleCutPhase.AdvancingWithdrawalDC);
					}
					else
					{
						BattleCutManager.ReqPhase(BattleCutPhase.AdvancingWithdrawal);
					}
				});
			}
			else
			{
				Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate
				{
					RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawalDC(BattleCutManager.GetMapManager(), ShipRecoveryType.None));
					if (manager.Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && ShipUtils.HasRepair(manager.Ships_f[0]))
					{
						BattleCutManager.ReqPhase(BattleCutPhase.AdvancingWithdrawalDC);
					}
					else
					{
						BattleCutManager.ReqPhase(BattleCutPhase.AdvancingWithdrawal);
					}
				});
			}
		}
	}
}
