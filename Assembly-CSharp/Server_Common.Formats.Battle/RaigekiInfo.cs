using Common.Enum;

namespace Server_Common.Formats.Battle
{
	public class RaigekiInfo
	{
		public int[] Target;

		public BattleHitStatus[] Clitical;

		public int[] Damage;

		public BattleDamageKinds[] DamageKind;

		public RaigekiInfo()
		{
			Target = new int[6];
			Damage = new int[6];
			Clitical = new BattleHitStatus[6];
			DamageKind = new BattleDamageKinds[6];
			for (int i = 0; i < 6; i++)
			{
				Target[i] = -1;
				Damage[i] = 0;
				Clitical[i] = BattleHitStatus.Miss;
				DamageKind[i] = BattleDamageKinds.Normal;
			}
		}
	}
}
