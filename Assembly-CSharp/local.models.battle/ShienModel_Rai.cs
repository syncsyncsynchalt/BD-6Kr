using Server_Common.Formats.Battle;
using System.Collections.Generic;

namespace local.models.battle
{
	public class ShienModel_Rai : ShienModel_Hou
	{
		public ShienModel_Rai(DeckModel shien_deck, List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, SupportAtack data)
			: base(shien_deck, ships_f, ships_e, data)
		{
		}

		public override string ToString()
		{
			string text = $"[雷撃支援]\n";
			List<DamageModel> attackDamages = GetAttackDamages();
			for (int i = 0; i < attackDamages.Count; i++)
			{
				text += $"{attackDamages[i]}";
			}
			return text;
		}
	}
}
