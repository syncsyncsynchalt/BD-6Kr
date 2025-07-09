using Common.Enum;
using System;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("AllBattleFmt")]
	public class AllBattleFmt
	{
		[XmlElement("Formation")]
		public BattleFormationKinds1[] Formation;

		[XmlElement("BattleFormation")]
		public BattleFormationKinds2 BattleFormation;

		[XmlElement("DayBattle")]
		public DayBattleFmt DayBattle;

		[XmlElement("NightBattle")]
		public NightBattleFmt NightBattle;

		public AllBattleFmt()
		{
			Formation = new BattleFormationKinds1[2];
		}

		public AllBattleFmt(BattleFormationKinds1 fFormation, BattleFormationKinds1 eFormation, BattleFormationKinds2 battleFormation)
			: this()
		{
			Formation[0] = fFormation;
			Formation[1] = eFormation;
			BattleFormation = battleFormation;
		}
	}
}
