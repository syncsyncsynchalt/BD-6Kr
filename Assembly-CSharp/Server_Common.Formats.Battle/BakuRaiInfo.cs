using Common.Enum;

namespace Server_Common.Formats.Battle
{
	public class BakuRaiInfo
	{
		private readonly int capacity;

		public bool[] IsRaigeki;

		public bool[] IsBakugeki;

		public BattleHitStatus[] Clitical;

		public int[] Damage;

		public BattleDamageKinds[] DamageType;

		public BakuRaiInfo()
		{
			capacity = 6;
			IsRaigeki = new bool[capacity];
			IsBakugeki = new bool[capacity];
			Clitical = new BattleHitStatus[capacity];
			Damage = new int[capacity];
			DamageType = new BattleDamageKinds[capacity];
			for (int i = 0; i < capacity; i++)
			{
				IsRaigeki[i] = false;
				IsBakugeki[i] = false;
				Clitical[i] = BattleHitStatus.Normal;
				Damage[i] = 0;
				DamageType[i] = BattleDamageKinds.Normal;
			}
		}
	}
}
