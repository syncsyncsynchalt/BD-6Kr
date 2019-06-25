using Common.Enum;
using local.managers;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace local.utils
{
	public class TrophyUtil
	{
		public static int __tmp_start_album_ship_num__;

		public static int __tmp_start_album_slot_num__;

		public static bool __tmp_area_reopen__;

		public static List<int> Unlock_At_Marriage()
		{
			return Unlock_At_Marriage(unlock: true);
		}

		public static List<int> Unlock_At_Marriage(bool unlock)
		{
			List<int> list = __convert__(1);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_At_AreaClear(int area_id)
		{
			return Unlock_At_AreaClear(area_id, unlock: true);
		}

		public static List<int> Unlock_At_AreaClear(int area_id, bool unlock)
		{
			int num = area_id + 8;
			if (SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(num))
			{
				return new List<int>();
			}
			List<int> list = new List<int>();
			list.Add(num);
			List<int> list2 = list;
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list2);
			}
			return list2;
		}

		public static List<int> Unlock_At_MapStart()
		{
			return Unlock_At_MapStart(unlock: true);
		}

		public static List<int> Unlock_At_MapStart(bool unlock)
		{
			int start_map_count = Comm_UserDatas.Instance.User_trophy.Start_map_count;
			List<int> list = (start_map_count < 10) ? __convert__(null) : __convert__(26);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_At_BattleResultOnlySally()
		{
			return Unlock_At_BattleResultOnlySally(unlock: true);
		}

		public static List<int> Unlock_At_BattleResultOnlySally(bool unlock)
		{
			int win_S_count = Comm_UserDatas.Instance.User_trophy.Win_S_count;
			List<int> list = new List<int>();
			if (win_S_count >= 30)
			{
				list.Add(27);
				if (win_S_count >= 100)
				{
					list.Add(28);
				}
			}
			list = __convert__(list);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_ReOpenMap()
		{
			return Unlock_ReOpenMap(unlock: true);
		}

		public static List<int> Unlock_ReOpenMap(bool unlock)
		{
			List<int> list = new List<int>();
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(29) && __tmp_area_reopen__)
			{
				list.Add(29);
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_At_Battle(int count_at_battlestart, int count_in_battle)
		{
			return Unlock_At_Battle(count_at_battlestart, count_in_battle, unlock: true);
		}

		public static List<int> Unlock_At_Battle(int count_at_battlestart, int count_in_battle, bool unlock)
		{
			int count = count_at_battlestart + count_in_battle;
			return __Unlock_For_RecoveryItem__(count, unlock);
		}

		public static List<int> Unlock_At_SCutBattle()
		{
			return Unlock_At_SCutBattle(unlock: true);
		}

		public static List<int> Unlock_At_SCutBattle(bool unlock)
		{
			int use_recovery_item_count = Comm_UserDatas.Instance.User_trophy.Use_recovery_item_count;
			return __Unlock_For_RecoveryItem__(use_recovery_item_count, unlock);
		}

		public static List<int> Unlock_At_GoNext()
		{
			return Unlock_At_GoNext(unlock: true);
		}

		public static List<int> Unlock_At_GoNext(bool unlock)
		{
			int count = Comm_UserDatas.Instance.User_trophy.Use_recovery_item_count + 1;
			return __Unlock_For_RecoveryItem__(count, unlock);
		}

		public static List<int> Unlock_At_Rading()
		{
			return Unlock_At_Rading(unlock: true);
		}

		public static List<int> Unlock_At_Rading(bool unlock)
		{
			int use_recovery_item_count = Comm_UserDatas.Instance.User_trophy.Use_recovery_item_count;
			return __Unlock_For_RecoveryItem__(use_recovery_item_count, unlock);
		}

		private static List<int> __Unlock_For_RecoveryItem__(int count, bool unlock)
		{
			List<int> list2;
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(30) && count >= 5)
			{
				List<int> list = new List<int>();
				list.Add(30);
				list2 = list;
			}
			else
			{
				list2 = new List<int>();
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list2);
			}
			return list2;
		}

		public static List<int> Unlock_At_Revamp()
		{
			return Unlock_At_Revamp(unlock: true);
		}

		public static List<int> Unlock_At_Revamp(bool unlock)
		{
			int revamp_count = Comm_UserDatas.Instance.User_trophy.Revamp_count;
			List<int> list2;
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(38) && revamp_count >= 100)
			{
				List<int> list = new List<int>();
				list.Add(38);
				list2 = list;
			}
			else
			{
				list2 = new List<int>();
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list2);
			}
			return list2;
		}

		public static List<int> Unlock_At_BuyFurniture()
		{
			return Unlock_At_BuyFurniture(unlock: true);
		}

		public static List<int> Unlock_At_BuyFurniture(bool unlock)
		{
			List<int> list = new List<int>();
			if (SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(39))
			{
				return list;
			}
			Dictionary<int, Mst_furniture> mst = Mst_DataManager.Instance.Mst_furniture;
			int num = Comm_UserDatas.Instance.User_furniture.Values.Count((Mem_furniture f) => mst[f.Rid].Title != "なし");
			if (num < 50)
			{
				return list;
			}
			list.Add(39);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_At_DockOpen()
		{
			return Unlock_At_DockOpen(unlock: true);
		}

		public static List<int> Unlock_At_DockOpen(bool unlock)
		{
			if (SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(40))
			{
				return new List<int>();
			}
			int num = 0;
			foreach (Mst_maparea value in Mst_DataManager.Instance.Mst_maparea.Values)
			{
				num += value.Ndocks_max;
			}
			List<int> list2;
			if (Comm_UserDatas.Instance.User_ndock.Count >= num)
			{
				List<int> list = new List<int>();
				list.Add(40);
				list2 = list;
			}
			else
			{
				list2 = new List<int>();
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list2);
			}
			return list2;
		}

		public static List<int> Unlock_UserLevel()
		{
			return Unlock_UserLevel(unlock: true);
		}

		public static List<int> Unlock_UserLevel(bool unlock)
		{
			List<int> list = new List<int>();
			if (Comm_UserDatas.Instance.User_basic.UserLevel() >= 70)
			{
				list.Add(41);
			}
			list = __convert__(list);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_At_GameClear()
		{
			return Unlock_At_GameClear(unlock: true);
		}

		public static List<int> Unlock_At_GameClear(bool unlock)
		{
			List<int> list = new List<int>();
			if (!Server_Common.Utils.IsGameClear())
			{
				return list;
			}
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(44))
			{
				list.Add(44);
			}
			switch (Comm_UserDatas.Instance.User_basic.Difficult)
			{
			case DifficultKind.OTU:
				if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(45))
				{
					list.Add(45);
				}
				break;
			case DifficultKind.KOU:
				if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(46))
				{
					list.Add(46);
				}
				break;
			case DifficultKind.SHI:
				if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(47))
				{
					list.Add(47);
				}
				break;
			}
			list = __convert__(list);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_Material()
		{
			return Unlock_Material(unlock: true);
		}

		public static List<int> Unlock_Material(bool unlock)
		{
			List<int> list = new List<int>();
			bool flag = SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(42);
			bool flag2 = SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(43);
			if (flag && flag2)
			{
				return list;
			}
			Dictionary<enumMaterialCategory, Mem_material> user_material = Comm_UserDatas.Instance.User_material;
			int value = user_material[enumMaterialCategory.Fuel].Value;
			int value2 = user_material[enumMaterialCategory.Bull].Value;
			int value3 = user_material[enumMaterialCategory.Steel].Value;
			int value4 = user_material[enumMaterialCategory.Bauxite].Value;
			int num = Math.Min(Math.Min(Math.Min(value, value2), value3), value4);
			if (!flag && num >= 20000)
			{
				list.Add(42);
			}
			if (!flag2 && num >= 100000)
			{
				list.Add(43);
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_DeckNum()
		{
			return Unlock_DeckNum(unlock: true);
		}

		public static List<int> Unlock_DeckNum(bool unlock)
		{
			List<int> list = new List<int>();
			if (SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(31))
			{
				return list;
			}
			if (Comm_UserDatas.Instance.User_deck.Count < 8)
			{
				return list;
			}
			list.Add(31);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_At_BuildShip(int built_ship_mst_id)
		{
			return Unlock_At_BuildShip(built_ship_mst_id, unlock: true);
		}

		public static List<int> Unlock_At_BuildShip(int built_ship_mst_id, bool unlock)
		{
			if (!Mst_DataManager.Instance.Mst_ship.TryGetValue(built_ship_mst_id, out Mst_ship value))
			{
				return new List<int>();
			}
			List<int> list2;
			if (value.Yomi == "やまと" && !SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(32))
			{
				List<int> list = new List<int>();
				list.Add(32);
				list2 = list;
			}
			else
			{
				list2 = new List<int>();
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list2);
			}
			return list2;
		}

		public static List<int> Unlock_AlbumSlotNum()
		{
			return Unlock_AlbumSlotNum(unlock: true);
		}

		public static List<int> Unlock_AlbumSlotNum(bool unlock)
		{
			List<int> list = new List<int>();
			int bookRegNum = Server_Common.Utils.GetBookRegNum(2);
			if (bookRegNum <= __tmp_start_album_slot_num__)
			{
				return list;
			}
			if (bookRegNum >= 30)
			{
				list.Add(6);
				if (bookRegNum >= 70)
				{
					list.Add(7);
					if (bookRegNum >= 120)
					{
						list.Add(8);
					}
				}
			}
			list = __convert__(list);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_AlbumShipNum()
		{
			return Unlock_AlbumShipNum(unlock: true);
		}

		public static List<int> Unlock_AlbumShipNum(bool unlock)
		{
			List<int> list = new List<int>();
			int bookRegNum = Server_Common.Utils.GetBookRegNum(1);
			if (bookRegNum <= __tmp_start_album_ship_num__)
			{
				return list;
			}
			if (bookRegNum >= 30)
			{
				list.Add(2);
				if (bookRegNum >= 100)
				{
					list.Add(3);
					if (bookRegNum >= 150)
					{
						list.Add(4);
						if (bookRegNum >= 200)
						{
							list.Add(5);
						}
					}
				}
			}
			list = __convert__(list);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_GatherShips(int new_ship_mst_id)
		{
			return Unlock_GatherShips(new_ship_mst_id, unlock: true);
		}

		public static List<int> Unlock_GatherShips(int new_ship_mst_id, bool unlock)
		{
			List<int> list = new List<int>();
			HashSet<int> hashSet = new HashSet<int>();
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(33))
			{
				hashSet.Add(33);
			}
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(35))
			{
				hashSet.Add(35);
			}
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(36))
			{
				hashSet.Add(36);
			}
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(37))
			{
				hashSet.Add(37);
			}
			if (hashSet.Count > 0)
			{
				if (!Mst_DataManager.Instance.Mst_ship.TryGetValue(new_ship_mst_id, out Mst_ship value))
				{
					return new List<int>();
				}
				string yomi = value.Yomi;
				Mst_ship value2;
				if (hashSet.Contains(33))
				{
					HashSet<string> hashSet2 = new HashSet<string>();
					hashSet2.Add("こんごう");
					hashSet2.Add("ひえい");
					hashSet2.Add("はるな");
					hashSet2.Add("きりしま");
					hashSet2.Add("ながと");
					hashSet2.Add("むつ");
					hashSet2.Add("ふそう");
					hashSet2.Add("やましろ");
					hashSet2.Add("いせ");
					hashSet2.Add("ひゅうが");
					hashSet2.Add("やまと");
					hashSet2.Add("むさし");
					HashSet<string> hashSet3 = hashSet2;
					if (hashSet3.Contains(yomi))
					{
						foreach (Mem_ship value3 in Comm_UserDatas.Instance.User_ship.Values)
						{
							if (Mst_DataManager.Instance.Mst_ship.TryGetValue(value3.Ship_id, out value2) && hashSet3.Contains(value2.Yomi))
							{
								hashSet3.Remove(value2.Yomi);
							}
						}
						if (hashSet3.Count == 0)
						{
							list.Add(33);
						}
					}
				}
				if (hashSet.Contains(35))
				{
					HashSet<string> hashSet2 = new HashSet<string>();
					hashSet2.Add("あかぎ");
					hashSet2.Add("かが");
					hashSet2.Add("ひりゅう");
					hashSet2.Add("そうりゅう");
					hashSet2.Add("しょうかく");
					hashSet2.Add("ずいかく");
					HashSet<string> hashSet4 = hashSet2;
					if (hashSet4.Contains(yomi))
					{
						foreach (Mem_ship value4 in Comm_UserDatas.Instance.User_ship.Values)
						{
							if (Mst_DataManager.Instance.Mst_ship.TryGetValue(value4.Ship_id, out value2) && hashSet4.Contains(value2.Yomi))
							{
								hashSet4.Remove(value2.Yomi);
							}
						}
						if (hashSet4.Count == 0)
						{
							list.Add(35);
						}
					}
				}
				if (hashSet.Contains(36))
				{
					HashSet<string> hashSet2 = new HashSet<string>();
					hashSet2.Add("こんごう");
					hashSet2.Add("きりしま");
					hashSet2.Add("はるな");
					hashSet2.Add("ひえい");
					HashSet<string> hashSet5 = hashSet2;
					if (hashSet5.Contains(yomi))
					{
						foreach (Mem_ship value5 in Comm_UserDatas.Instance.User_ship.Values)
						{
							if (Mst_DataManager.Instance.Mst_ship.TryGetValue(value5.Ship_id, out value2) && hashSet5.Contains(value2.Yomi))
							{
								hashSet5.Remove(value2.Yomi);
							}
						}
						if (hashSet5.Count == 0)
						{
							list.Add(36);
						}
					}
				}
				if (hashSet.Contains(37))
				{
					HashSet<string> hashSet2 = new HashSet<string>();
					hashSet2.Add("ふそう");
					hashSet2.Add("やましろ");
					hashSet2.Add("もがみ");
					hashSet2.Add("しぐれ");
					hashSet2.Add("みちしお");
					hashSet2.Add("あさぐも");
					hashSet2.Add("やまぐも");
					HashSet<string> hashSet6 = hashSet2;
					if (hashSet6.Contains(yomi))
					{
						foreach (Mem_ship value6 in Comm_UserDatas.Instance.User_ship.Values)
						{
							if (Mst_DataManager.Instance.Mst_ship.TryGetValue(value6.Ship_id, out value2) && hashSet6.Contains(value2.Yomi))
							{
								hashSet6.Remove(value2.Yomi);
							}
						}
						if (hashSet6.Count == 0)
						{
							list.Add(37);
						}
					}
				}
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_At_GetIowa(int new_ship_mst_id)
		{
			return Unlock_At_GetIowa(new_ship_mst_id, unlock: true);
		}

		public static List<int> Unlock_At_GetIowa(int new_ship_mst_id, bool unlock)
		{
			List<int> list = new List<int>();
			if (SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(34))
			{
				return list;
			}
			if (!Mst_DataManager.Instance.Mst_ship.TryGetValue(new_ship_mst_id, out Mst_ship value))
			{
				return list;
			}
			if (value.Yomi == "アイオワ")
			{
				List<int> list2 = new List<int>();
				list2.Add(34);
				list = list2;
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_GetShip(int new_ship_mst_id)
		{
			return Unlock_GetShip(new_ship_mst_id, unlock: true);
		}

		public static List<int> Unlock_GetShip(int new_ship_mst_id, bool unlock)
		{
			List<int> list = new List<int>();
			list.AddRange(Unlock_AlbumShipNum(unlock: false));
			list.AddRange(Unlock_AlbumSlotNum(unlock: false));
			list.AddRange(Unlock_GatherShips(new_ship_mst_id, unlock: false));
			list.AddRange(Unlock_At_GetIowa(new_ship_mst_id, unlock: false));
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		private static List<int> __convert__(int tmp)
		{
			List<int> list = new List<int>();
			list.Add(tmp);
			return __convert__(list);
		}

		private static List<int> __convert__(List<int> tmp)
		{
			if (tmp == null)
			{
				return new List<int>();
			}
			return tmp.FindAll((int id) => !SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(id));
		}
	}
}
