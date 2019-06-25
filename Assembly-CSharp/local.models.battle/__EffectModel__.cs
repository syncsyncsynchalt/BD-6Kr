using Server_Common.Formats.Battle;

namespace local.models.battle
{
	public class __EffectModel__ : EffectModel
	{
		public __EffectModel__(DayBattleProductionFmt fmt)
			: base(fmt)
		{
		}

		public void SetNextActionShip(ShipModel_Battle value)
		{
			_next_action_ship = value;
		}
	}
}
