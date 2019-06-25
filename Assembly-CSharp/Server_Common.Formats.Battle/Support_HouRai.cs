using Common.Enum;

namespace Server_Common.Formats.Battle
{
	public class Support_HouRai
	{
		private readonly int capacity;

		public BattleHitStatus[] Clitical;

		public int[] Damage;

		public BattleDamageKinds[] DamageType;

		public Support_HouRai()
		{
			capacity = 6;
			Clitical = new BattleHitStatus[capacity];
			Damage = new int[capacity];
			DamageType = new BattleDamageKinds[capacity];
			for (int i = 0; i < capacity; i++)
			{
				Clitical[i] = BattleHitStatus.Miss;
				Damage[i] = 0;
				DamageType[i] = BattleDamageKinds.Normal;
			}
		}
	}
}
