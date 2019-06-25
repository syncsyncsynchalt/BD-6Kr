using Common.Enum;
using System;
using System.Collections.Generic;

namespace Server_Common.Formats.Battle
{
	public class Hougeki<T> where T : IConvertible
	{
		public int Attacker;

		public T SpType;

		public List<int> Slot_List;

		public List<int> Target;

		public List<BattleHitStatus> Clitical;

		public List<int> Damage;

		public List<BattleDamageKinds> DamageKind;

		public Hougeki()
		{
			Slot_List = new List<int>();
			Target = new List<int>();
			Clitical = new List<BattleHitStatus>();
			Damage = new List<int>();
			DamageKind = new List<BattleDamageKinds>();
		}
	}
}
