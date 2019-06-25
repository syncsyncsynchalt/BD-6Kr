using Common.Enum;
using Common.Struct;
using local.managers;
using local.models;
using Server_Common;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace local.utils
{
	public static class Utils
	{
		public const int __DECK_NUM_MAX__ = 8;

		public static string GetText(TextType type, int target_id)
		{
			string text = string.Empty;
			string expandedName = string.Empty;
			switch (type)
			{
			case TextType.SHIP_GET_TEXT:
				text = "mst_shiptext";
				expandedName = "Getmes";
				break;
			case TextType.USEITEM_TEXT:
				text = "mst_useitemtext";
				expandedName = "Description";
				break;
			case TextType.FURNITURE_TEXT:
				text = "mst_furnituretext";
				expandedName = "Description";
				break;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("Id", target_id.ToString());
			IEnumerable<XElement> enumerable = Server_Common.Utils.Xml_Result_Where(text, text, dictionary);
			if (enumerable == null)
			{
				return string.Empty;
			}
			return enumerable.First().Element(expandedName).Value;
		}

		public static string enumMaterialCategoryToString(enumMaterialCategory material)
		{
			switch (material)
			{
			case enumMaterialCategory.Bauxite:
				return "ボ\u30fcキサイト";
			case enumMaterialCategory.Build_Kit:
				return "高速建造";
			case enumMaterialCategory.Bull:
				return "弾薬";
			case enumMaterialCategory.Dev_Kit:
				return "開発資材";
			case enumMaterialCategory.Fuel:
				return "燃料";
			case enumMaterialCategory.Repair_Kit:
				return "高速修復剤";
			case enumMaterialCategory.Revamp_Kit:
				return "改修資材";
			case enumMaterialCategory.Steel:
				return "鋼材";
			default:
				return string.Empty;
			}
		}

		public static string GetSlotitemType3Name(int type3)
		{
			Dictionary<int, KeyValuePair<int, string>> slotItemEquipTypeName = Mst_DataManager.Instance.GetSlotItemEquipTypeName();
			if (!slotItemEquipTypeName.TryGetValue(type3, out KeyValuePair<int, string> value))
			{
				return string.Empty;
			}
			return value.Value;
		}

		public static List<KeyValuePair<enumMaterialCategory, int>> SortMaterial(List<enumMaterialCategory> sortkey, Dictionary<enumMaterialCategory, int> materials)
		{
			List<KeyValuePair<enumMaterialCategory, int>> list = new List<KeyValuePair<enumMaterialCategory, int>>();
			foreach (enumMaterialCategory item2 in sortkey)
			{
				int value = materials.ContainsKey(item2) ? materials[item2] : 0;
				KeyValuePair<enumMaterialCategory, int> item = new KeyValuePair<enumMaterialCategory, int>(item2, value);
				list.Add(item);
			}
			return list;
		}

		public static MemberMaxInfo ShipCountData()
		{
			int count = Comm_UserDatas.Instance.User_ship.Count;
			int max_chara = Comm_UserDatas.Instance.User_basic.Max_chara;
			return new MemberMaxInfo(count, max_chara);
		}

		public static MemberMaxInfo SlotitemCountData()
		{
			int count = Comm_UserDatas.Instance.User_slot.Count;
			int max_slotitem = Comm_UserDatas.Instance.User_basic.Max_slotitem;
			return new MemberMaxInfo(count, max_slotitem);
		}

		public static int GetResourceMstId(int mst_id)
		{
			return Mst_DataManager.Instance.Mst_ship_resources[mst_id].Standing_id;
		}

		public static int GetVoiceMstId(int mst_id, int voice_id)
		{
			int voiceId = Mst_DataManager.Instance.Mst_ship_resources[mst_id].GetVoiceId(voice_id);
			return (voiceId != 0) ? voiceId : (-1);
		}

		public static int GetSpecialVoiceId(int mst_id)
		{
			return Mst_DataManager.Instance.Mst_ship_resources[mst_id].GetDeckPracticeVoiceNo();
		}

		public static int GetSlotitemGraphicId(int mst_id)
		{
			bool master_loaded;
			return GetSlotitemGraphicId(mst_id, out master_loaded);
		}

		public static int GetSlotitemGraphicId(int mst_id, out bool master_loaded)
		{
			Dictionary<int, int> mst_slotitem_comvert = Mst_DataManager.Instance.UiBattleMaster.Mst_slotitem_comvert;
			master_loaded = (mst_slotitem_comvert != null);
			if (master_loaded)
			{
				if (mst_slotitem_comvert.TryGetValue(mst_id, out int value))
				{
					return value;
				}
				if (mst_id > 500)
				{
					return mst_id - 500;
				}
			}
			return mst_id;
		}

		public static ShipRecoveryType __HasRecoveryItem__(List<SlotitemModel_Battle> slotitems)
		{
			return __HasRecoveryItem__(slotitems, null);
		}

		public static ShipRecoveryType __HasRecoveryItem__(List<SlotitemModel_Battle> slotitems, SlotitemModel_Battle slotitem_ex)
		{
			if (slotitem_ex != null)
			{
				if (slotitem_ex.MstId == 42)
				{
					return ShipRecoveryType.Personnel;
				}
				if (slotitem_ex.MstId == 43)
				{
					return ShipRecoveryType.Goddes;
				}
			}
			for (int i = 0; i < slotitems.Count; i++)
			{
				SlotitemModel_Battle slotitemModel_Battle = slotitems[i];
				if (slotitemModel_Battle != null)
				{
					if (slotitemModel_Battle.MstId == 42)
					{
						return ShipRecoveryType.Personnel;
					}
					if (slotitemModel_Battle.MstId == 43)
					{
						return ShipRecoveryType.Goddes;
					}
				}
			}
			return ShipRecoveryType.None;
		}

		public static Dictionary<enumMaterialCategory, int> GetAreaResource(int area_id, int tanker_count)
		{
			return GetAreaResource(area_id, tanker_count, null);
		}

		public static Dictionary<enumMaterialCategory, int> GetAreaResource(int area_id, int tanker_count, EscortDeckManager eManager)
		{
			DeckShips deckShip = null;
			if (eManager != null && eManager.EditDeck != null)
			{
				deckShip = ((TemporaryEscortDeckModel)eManager.EditDeck).DeckShips;
			}
			return new Api_req_Transport().GetMaterialNum(area_id, tanker_count, deckShip);
		}

		public static HashSet<SType> GetSortieLimit(int map_id, bool is_permitted)
		{
			if (is_permitted)
			{
				switch (map_id)
				{
				case 71:
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.Destroyter);
					hashSet.Add(SType.LightCruiser);
					hashSet.Add(SType.TrainingCruiser);
					hashSet.Add(SType.LightAircraftCarrier);
					return hashSet;
				}
				case 121:
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.Submarine);
					hashSet.Add(SType.SubmarineAircraftCarrier);
					hashSet.Add(SType.SubmarineTender);
					hashSet.Add(SType.Destroyter);
					hashSet.Add(SType.LightCruiser);
					hashSet.Add(SType.TrainingCruiser);
					return hashSet;
				}
				}
			}
			else
			{
				switch (map_id)
				{
				case 22:
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					hashSet.Add(SType.BattleCruiser);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					return hashSet;
				}
				case 42:
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					return hashSet;
				}
				case 93:
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.BattleCruiser);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					hashSet.Add(SType.TorpedCruiser);
					hashSet.Add(SType.Submarine);
					hashSet.Add(SType.SubmarineAircraftCarrier);
					return hashSet;
				}
				case 101:
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					return hashSet;
				}
				case 122:
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					return hashSet;
				}
				case 131:
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					hashSet.Add(SType.BattleCruiser);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					hashSet.Add(SType.HeavyCruiser);
					hashSet.Add(SType.AviationCruiser);
					return hashSet;
				}
				case 132:
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					hashSet.Add(SType.BattleCruiser);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					return hashSet;
				}
				case 141:
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					hashSet.Add(SType.BattleCruiser);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					hashSet.Add(SType.LightAircraftCarrier);
					return hashSet;
				}
				case 142:
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					return hashSet;
				}
				case 161:
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					hashSet.Add(SType.BattleCruiser);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					hashSet.Add(SType.LightAircraftCarrier);
					return hashSet;
				}
				case 163:
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					return hashSet;
				}
				case 171:
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					hashSet.Add(SType.BattleCruiser);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					return hashSet;
				}
				}
			}
			return null;
		}

		public static int GetRadingStartTurn()
		{
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			Mst_radingtype mst_radingtype = Mst_DataManager.Instance.Mst_RadingType[(int)difficult].Last();
			return mst_radingtype.Turn_to;
		}
	}
}
