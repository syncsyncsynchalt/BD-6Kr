using Common.Enum;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("BattleResultFmt")]
	public class BattleResultFmt
	{
		[XmlElement("WinRank")]
		public BattleWinRankKinds WinRank;

		[XmlElement("BasicLevel")]
		public int BasicLevel;

		[XmlElement("QuestName")]
		public string QuestName;

		[XmlElement("MvpShip")]
		public int MvpShip;

		[XmlElement("GetBaseExp")]
		public int GetBaseExp;

		[XmlIgnore]
		public SerializableDictionary<int, int> GetShipExp;

		[XmlIgnore]
		public SerializableDictionary<int, List<int>> LevelUpInfo;

		[XmlElement("EnemyId")]
		public List<int> EnemyId;

		[XmlElement("EnemyName")]
		public string EnemyName;

		[XmlElement("FirstClear")]
		public bool FirstClear;

		[XmlElement("FirstAreaComplete")]
		public bool FirstAreaComplete;

		[XmlElement("GetSpoint")]
		public int GetSpoint;

		[XmlElement("GetAirReconnaissanceItems")]
		public List<MapItemGetFmt> GetAirReconnaissanceItems;

		[XmlElement("GetItem")]
		public List<ItemGetFmt> GetItem;

		[XmlElement("AreaClearRewardItem")]
		public ItemGetFmt AreaClearRewardItem;

		[XmlElement("GetEventItem")]
		public List<ItemGetFmt> GetEventItem;

		[XmlElement("ExMapReward")]
		public ExMapRewardInfo ExMapReward;

		[XmlElement("EscapeInfo")]
		public EscapeInfo EscapeInfo;

		[XmlElement("NewOpenMapId")]
		public List<int> NewOpenMapId;

		[XmlElement("ReOpenMapId")]
		public List<int> ReOpenMapId;

		public BattleResultFmt()
		{
			EnemyId = new List<int>();
			NewOpenMapId = new List<int>();
			ReOpenMapId = new List<int>();
		}
	}
}
