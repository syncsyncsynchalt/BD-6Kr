using Common.Enum;
using System;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("Support_HouRai")]
	public class Support_HouRai
	{
		private readonly int capacity;

		[XmlElement("Clitical")]
		public BattleHitStatus[] Clitical;

		[XmlElement("Damage")]
		public int[] Damage;

		[XmlElement("DamageType")]
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
