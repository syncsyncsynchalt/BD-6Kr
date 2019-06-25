using Common.Enum;

namespace Server_Controllers
{
	public class MiddleBattleCallInfo
	{
		public enum CallType
		{
			None,
			Houg,
			Raig,
			LastRaig,
			EffectOnly
		}

		public int CommandPos;

		public BattleCommand UseCommand;

		public CallType BattleType;

		public int AttackType;

		public MiddleBattleCallInfo(int commandPos, BattleCommand useCommand, CallType callType, int attackType)
		{
			CommandPos = commandPos;
			UseCommand = useCommand;
			BattleType = callType;
			AttackType = attackType;
		}
	}
}
