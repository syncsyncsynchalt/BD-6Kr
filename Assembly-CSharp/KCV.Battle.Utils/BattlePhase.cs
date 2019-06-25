namespace KCV.Battle.Utils
{
	public enum BattlePhase
	{
		BattlePhase_ST = 0,
		BattlePhase_BEF = -1,
		BattlePhase_NONE = -1,
		BossInsert = 0,
		FleetAdvent = 1,
		Detection = 2,
		Command = 3,
		AerialCombat = 4,
		AerialCombatSecond = 5,
		SupportingFire = 6,
		OpeningTorpedoSalvo = 7,
		Shelling = 8,
		TorpedoSalvo = 9,
		WithdrawalDecision = 10,
		NightCombat = 11,
		Result = 12,
		FlagshipWreck = 13,
		EscortShipEvacuation = 14,
		AdvancingWithdrawal = 0xF,
		AdvancingWithdrawalDC = 0x10,
		ClearReward = 17,
		MapOpen = 18,
		BattlePhase_AFT = 19,
		BattlePhase_NUM = 19,
		BattlePhase_ED = 18
	}
}
