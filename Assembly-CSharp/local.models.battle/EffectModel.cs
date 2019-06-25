using Common.Enum;
using Server_Common.Formats.Battle;
using System.Collections.Generic;

namespace local.models.battle
{
	public class EffectModel : BattlePhaseModel
	{
		private DayBattleProductionFmt _fmt;

		protected ShipModel_Battle _next_action_ship;

		public BattleCommand Command => _fmt.productionKind;

		public bool Withdrawal => _fmt.Withdrawal;

		public int MeichuBuff => _fmt.FSPP;

		public int RaiMeichuBuff => _fmt.TSPP;

		public int KaihiBuff => _fmt.RSPP;

		public ShipModel_Battle NextActionShip => _next_action_ship;

		public EffectModel(DayBattleProductionFmt fmt)
		{
			_fmt = fmt;
			_data_f = new List<DamageModelBase>();
			_data_e = new List<DamageModelBase>();
		}

		public override List<ShipModel_Defender> GetDefenders(bool is_friend)
		{
			return new List<ShipModel_Defender>();
		}

		public override string ToString()
		{
			string empty = string.Empty;
			empty += $"[演出/効果] {Command}";
			if (Withdrawal)
			{
				empty += $"[離脱成功]";
			}
			if (MeichuBuff > 0)
			{
				empty += $" 命中バフ:{MeichuBuff}%";
			}
			if (KaihiBuff > 0)
			{
				empty += $" 回避バフ:{KaihiBuff}%";
			}
			if (RaiMeichuBuff > 0)
			{
				empty += $" 雷命中バフ:{RaiMeichuBuff}%";
			}
			if (_next_action_ship != null)
			{
				empty += $" 次行動艦:{NextActionShip}";
			}
			return empty;
		}
	}
}
