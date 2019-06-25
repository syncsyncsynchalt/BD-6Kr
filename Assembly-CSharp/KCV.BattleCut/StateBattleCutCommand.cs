using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutCommand : BaseBattleCutState
	{
		private CtrlBCCommandSelect _ctrlBCCommandSelect;

		public override bool Init(object data)
		{
			_ctrlBCCommandSelect = CtrlBCCommandSelect.Instantiate(((Component)BattleCutManager.GetPrefabFile().prefabCtrlBCCommandSelect).GetComponent<CtrlBCCommandSelect>(), BattleCutManager.GetSharedPlase(), BattleCutManager.GetBattleManager().GetCommandPhaseModel());
			_ctrlBCCommandSelect.Play(delegate
			{
				BattleCutManager.ReqPhase(BattleCutPhase.DayBattle);
			});
			return base.Init(data);
		}

		public override bool Terminate(object data)
		{
			Mem.DelComponentSafe(ref _ctrlBCCommandSelect);
			return false;
		}

		public override bool Run(object data)
		{
			if (_ctrlBCCommandSelect != null)
			{
				_ctrlBCCommandSelect.Run();
			}
			return IsCheckPhase(BattleCutPhase.Command);
		}
	}
}
