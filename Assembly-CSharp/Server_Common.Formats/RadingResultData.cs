using Common.Enum;
using System.Collections.Generic;

namespace Server_Common.Formats
{
	public class RadingResultData
	{
		public int AreaId;

		public RadingKind AttackKind;

		public int DeckAttackPow;

		public int FlagShipMstId;

		public DamageState FlagShipDamageState;

		public List<RadingDamageData> RadingDamage;

		public int BeforeNum;

		public int BreakNum;

		public override string ToString()
		{
			return $"[通商破壊結果]\n海域{AreaId} 攻撃種別:{AttackKind} 輸送船{BeforeNum}隻(移動中の船を除く)から{BreakNum}隻ロスト";
		}
	}
}
