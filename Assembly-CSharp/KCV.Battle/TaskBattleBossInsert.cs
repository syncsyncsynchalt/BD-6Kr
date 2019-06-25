using KCV.Battle.Production;
using KCV.Battle.Utils;
using local.models.battle;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleBossInsert : BaseBattleTask
	{
		private BossInsertModel _clsBossInsert;

		protected override bool Init()
		{
			_clsBossInsert = BattleTaskManager.GetBattleManager().GetBossInsertData();
			if (_clsBossInsert == null)
			{
				ImmediateTermination();
				EndPhase(BattleUtils.NextPhase(BattlePhase.BattlePhase_ST));
			}
			else
			{
				ProdBossInsert prodBossInsert = ProdBossInsert.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdBossInsert).GetComponent<ProdBossInsert>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform, _clsBossInsert.Ship);
				prodBossInsert.Play(delegate
				{
					EndPhase(BattleUtils.NextPhase(BattlePhase.BattlePhase_ST));
				});
			}
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			Mem.Del(ref _clsBossInsert);
			return true;
		}

		protected override bool Update()
		{
			return ChkChangePhase(BattlePhase.BattlePhase_ST);
		}
	}
}
