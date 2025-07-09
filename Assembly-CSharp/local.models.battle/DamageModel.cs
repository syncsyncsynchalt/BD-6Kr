using Common.Enum;

namespace local.models.battle
{
	public class DamageModel : DamageModelBase
	{
		public DamageModel(ShipModel_BattleAll defender)
			: base(defender)
		{
		}

		public void __AddData__(int damage, BattleHitStatus hitstate, BattleDamageKinds dmgkind)
		{
			_AddData(damage, hitstate, dmgkind);
		}

		public override string ToString()
		{
			return string.Format("{0}({1})へダメージ:{2}(r:{5}) {3}{4}\n", base.Defender.Name, base.Defender.Index, GetDamage(), GetHitState(), (!GetProtectEffect()) ? string.Empty : "[かばう]", __GetDamage__());
		}
	}
}
