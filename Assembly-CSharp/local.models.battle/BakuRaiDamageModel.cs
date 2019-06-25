using Common.Enum;

namespace local.models.battle
{
	public class BakuRaiDamageModel : RaigekiDamageModel
	{
		private bool _is_raigeki;

		private bool _is_bakugeki;

		public BakuRaiDamageModel(ShipModel_BattleAll defender, bool is_raigeki, bool is_bakugeki)
			: base(defender)
		{
			_is_raigeki = is_raigeki;
			_is_bakugeki = is_bakugeki;
		}

		public bool IsRaigeki()
		{
			return _is_raigeki;
		}

		public bool IsBakugeki()
		{
			return _is_bakugeki;
		}

		public int __AddData__(int damage, BattleHitStatus hitstate, BattleDamageKinds dmgkind)
		{
			_attackers.Add(null);
			return _AddData(damage, hitstate, dmgkind);
		}
	}
}
