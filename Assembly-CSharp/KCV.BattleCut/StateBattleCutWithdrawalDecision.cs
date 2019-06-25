using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutWithdrawalDecision : BaseBattleCutState
	{
		private ProdBCWithdrawalDecision _prodBCWithdrawalDecision;

		public override bool Init(object data)
		{
			if (BattleCutManager.GetBattleManager().HasNightBattle())
			{
				_prodBCWithdrawalDecision = ProdBCWithdrawalDecision.Instantiate(((Component)BattleCutManager.GetPrefabFile().prefabProdWithdrawalDecision).GetComponent<ProdBCWithdrawalDecision>(), BattleCutManager.GetSharedPlase());
				_prodBCWithdrawalDecision.Play(delegate(int index)
				{
					if (index == 1)
					{
						BattleCutManager.GetBattleManager().StartDayToNightBattle();
						BattleCutManager.ReqPhase(BattleCutPhase.NightBattle);
					}
					else
					{
						BattleCutManager.ReqPhase(BattleCutPhase.Judge);
					}
				});
				return false;
			}
			BattleCutManager.ReqPhase(BattleCutPhase.Judge);
			return true;
		}

		public override bool Run(object data)
		{
			if (_prodBCWithdrawalDecision != null)
			{
				_prodBCWithdrawalDecision.Run();
			}
			return IsCheckPhase(BattleCutPhase.WithdrawalDecision);
		}

		public override bool Terminate(object data)
		{
			if (_prodBCWithdrawalDecision != null)
			{
				Object.Destroy(_prodBCWithdrawalDecision.gameObject);
			}
			_prodBCWithdrawalDecision = null;
			return false;
		}
	}
}
