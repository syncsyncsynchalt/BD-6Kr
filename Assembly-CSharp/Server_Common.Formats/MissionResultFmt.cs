using Common.Enum;
using Server_Models;
using System.Collections.Generic;

namespace Server_Common.Formats
{
	public class MissionResultFmt
	{
		public Mem_deck Deck;

		public MissionResultKinds MissionResult;

		public int GetMemberExp;

		public int MemberLevel;

		public Dictionary<int, int> GetShipExp;

		public Dictionary<int, List<int>> LevelUpInfo;

		public string MissionName;

		public Dictionary<enumMaterialCategory, int> GetMaterials;

		public List<ItemGetFmt> GetItems;

		public int GetSpoint;

		public MissionResultFmt()
		{
			GetMaterials = new Dictionary<enumMaterialCategory, int>
			{
				{
					enumMaterialCategory.Fuel,
					0
				},
				{
					enumMaterialCategory.Bull,
					0
				},
				{
					enumMaterialCategory.Steel,
					0
				},
				{
					enumMaterialCategory.Bauxite,
					0
				}
			};
			GetSpoint = 0;
		}
	}
}
