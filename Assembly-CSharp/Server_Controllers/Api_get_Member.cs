using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers
{
	public class Api_get_Member
	{
		private Dictionary<int, int> mst_level;

		private Dictionary<int, List<int>> type3Dict;

		private Dictionary<int, string> tempShipBookText;

		private Dictionary<int, string> tempSlotBookText;

		private Dictionary<int, string> tempShipClass;

		private Dictionary<int, int> tempMstShipBook;

		public Api_Result<Mem_basic> Basic()
		{
			Api_Result<Mem_basic> api_Result = new Api_Result<Mem_basic>();
			api_Result.data = Comm_UserDatas.Instance.User_basic;
			return api_Result;
		}

		public Api_Result<Dictionary<int, Mem_ship>> Ship(List<int> target_rid)
		{
			Api_Result<Dictionary<int, Mem_ship>> api_Result = new Api_Result<Dictionary<int, Mem_ship>>();
			Dictionary<int, Mem_ship> ret_ship = new Dictionary<int, Mem_ship>();
			if (target_rid != null && target_rid.Count == 0)
			{
				api_Result.data = ret_ship;
				return api_Result;
			}
			if (mst_level == null)
			{
				mst_level = Mst_DataManager.Instance.Get_MstLevel(shipTable: true);
			}
			if (target_rid == null)
			{
				foreach (Mem_ship value2 in Comm_UserDatas.Instance.User_ship.Values)
				{
					value2.SetRequireExp(value2.Level, mst_level);
					ret_ship.Add(value2.Rid, value2);
				}
				api_Result.data = ret_ship;
				return api_Result;
			}
			target_rid.ForEach(delegate(int x)
			{
				Mem_ship value = null;
				if (Comm_UserDatas.Instance.User_ship.TryGetValue(x, out value))
				{
					value.SetRequireExp(value.Level, mst_level);
					ret_ship.Add(x, value);
				}
			});
			api_Result.data = ret_ship;
			if (ret_ship.Count == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				api_Result.data = null;
			}
			return api_Result;
		}

		public Api_Result<Dictionary<int, Mem_ship>> Ship(int area_id)
		{
			List<DeckShips> list = new List<DeckShips>();
			Mem_esccort_deck mem_esccort_deck = Comm_UserDatas.Instance.User_EscortDeck.Values.FirstOrDefault((Mem_esccort_deck x) => x.Maparea_id == area_id);
			if (mem_esccort_deck != null)
			{
				list.Add(mem_esccort_deck.Ship);
			}
			IEnumerable<DeckShips> enumerable = from deck in Comm_UserDatas.Instance.User_deck.Values
				where deck.Area_id == area_id
				select deck.Ship;
			if (enumerable.Count() > 0)
			{
				list.AddRange(enumerable);
			}
			if (list.Count == 0)
			{
				Api_Result<Dictionary<int, Mem_ship>> api_Result = new Api_Result<Dictionary<int, Mem_ship>>();
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			List<int> list2 = new List<int>();
			foreach (DeckShips item in list)
			{
				List<int> out_ships = null;
				item.Clone(out out_ships);
				list2.AddRange(out_ships);
			}
			return Ship(list2);
		}

		public Api_Result<Dictionary<int, Mem_deck>> Deck()
		{
			Api_Result<Dictionary<int, Mem_deck>> api_Result = new Api_Result<Dictionary<int, Mem_deck>>();
			Dictionary<int, Mem_deck> dictionary = api_Result.data = Comm_UserDatas.Instance.User_deck.ToDictionary((KeyValuePair<int, Mem_deck> x) => x.Key, (KeyValuePair<int, Mem_deck> y) => y.Value);
			return api_Result;
		}

		public Api_Result<Dictionary<int, Mem_deck>> Deck_Port()
		{
			Api_Result<Dictionary<int, Mem_deck>> api_Result = Deck();
			if (api_Result.data == null)
			{
				return api_Result;
			}
			foreach (Mem_deck value in api_Result.data.Values)
			{
				value.MissionEnd();
			}
			return api_Result;
		}

		public Api_Result<Dictionary<enumMaterialCategory, Mem_material>> Material()
		{
			Api_Result<Dictionary<enumMaterialCategory, Mem_material>> api_Result = new Api_Result<Dictionary<enumMaterialCategory, Mem_material>>();
			Dictionary<enumMaterialCategory, Mem_material> dictionary = api_Result.data = Comm_UserDatas.Instance.User_material.ToDictionary((KeyValuePair<enumMaterialCategory, Mem_material> x) => x.Key, (KeyValuePair<enumMaterialCategory, Mem_material> y) => y.Value);
			return api_Result;
		}

		public Api_Result<Dictionary<int, Mem_slotitem>> Slotitem()
		{
			Api_Result<Dictionary<int, Mem_slotitem>> api_Result = new Api_Result<Dictionary<int, Mem_slotitem>>();
			Dictionary<int, Mem_slotitem> dictionary = api_Result.data = Comm_UserDatas.Instance.User_slot.ToDictionary((KeyValuePair<int, Mem_slotitem> x) => x.Key, (KeyValuePair<int, Mem_slotitem> y) => y.Value);
			return api_Result;
		}

		public Api_Result<Dictionary<int, User_StrategyMapFmt>> StrategyInfo()
		{
			Api_Result<Dictionary<int, User_StrategyMapFmt>> api_Result = new Api_Result<Dictionary<int, User_StrategyMapFmt>>();
			api_Result.data = new Dictionary<int, User_StrategyMapFmt>();
			Dictionary<int, Mst_mapinfo> dictionary = (from x in Mst_DataManager.Instance.Mst_mapinfo.Values
				where x.No == 1
				select x).ToDictionary((Mst_mapinfo y) => y.Maparea_id, (Mst_mapinfo z) => z);
			foreach (Mst_maparea value2 in Mst_DataManager.Instance.Mst_maparea.Values)
			{
				bool flag = (dictionary[value2.Id].GetUser_MapinfoData() != null) ? true : false;
				User_StrategyMapFmt user_StrategyMapFmt = new User_StrategyMapFmt(value2, flag);
				Mem_rebellion_point value = null;
				if (Comm_UserDatas.Instance.User_rebellion_point.TryGetValue(value2.Id, out value))
				{
					user_StrategyMapFmt.RebellionState = value.State;
				}
				if (user_StrategyMapFmt.IsActiveArea)
				{
					if (!Comm_UserDatas.Instance.User_EscortDeck.ContainsKey(value2.Id))
					{
						Comm_UserDatas.Instance.Add_EscortDeck(value2.Id, value2.Id);
					}
					if (value == null)
					{
						value = new Mem_rebellion_point(value2.Id);
						Comm_UserDatas.Instance.User_rebellion_point.Add(value.Rid, value);
						user_StrategyMapFmt.RebellionState = value.State;
					}
				}
				api_Result.data.Add(user_StrategyMapFmt.Maparea.Id, user_StrategyMapFmt);
			}
			return api_Result;
		}

		public Api_Result<Dictionary<int, List<Mem_tanker>>> Tanker()
		{
			Api_Result<Dictionary<int, List<Mem_tanker>>> api_Result = new Api_Result<Dictionary<int, List<Mem_tanker>>>();
			Dictionary<int, List<Mem_tanker>> dictionary = Mst_DataManager.Instance.Mst_maparea.Keys.ToDictionary((int n) => n, (int value) => new List<Mem_tanker>());
			dictionary.Add(0, new List<Mem_tanker>());
			IEnumerable<IGrouping<int, Mem_tanker>> enumerable = from tankers in Comm_UserDatas.Instance.User_tanker.Values
				let maparea_id = (tankers.Disposition_status != DispositionStatus.NONE) ? tankers.Maparea_id : 0
				group tankers by maparea_id;
			foreach (IGrouping<int, Mem_tanker> item in enumerable)
			{
				int key = item.Key;
				dictionary[key].AddRange(item.ToList());
			}
			api_Result.data = dictionary;
			return api_Result;
		}

		public Api_Result<Dictionary<int, Mem_esccort_deck>> EscortDeck()
		{
			Api_Result<Dictionary<int, Mem_esccort_deck>> api_Result = new Api_Result<Dictionary<int, Mem_esccort_deck>>();
			Dictionary<int, Mem_esccort_deck> dictionary = Comm_UserDatas.Instance.User_EscortDeck.Values.ToDictionary((Mem_esccort_deck value) => value.Maparea_id, (Mem_esccort_deck value) => value);
			Dictionary<int, Mem_esccort_deck> dictionary2 = new Dictionary<int, Mem_esccort_deck>();
			foreach (int key in Mst_DataManager.Instance.Mst_maparea.Keys)
			{
				dictionary2.Add(key, null);
				Mem_esccort_deck value2 = null;
				if (dictionary.TryGetValue(key, out value2))
				{
					dictionary2[key] = value2;
				}
			}
			api_Result.data = dictionary2;
			return api_Result;
		}

		public Api_Result<Dictionary<int, User_MapinfoFmt>> Mapinfo()
		{
			Api_Result<Dictionary<int, User_MapinfoFmt>> api_Result = new Api_Result<Dictionary<int, User_MapinfoFmt>>();
			api_Result.data = new Dictionary<int, User_MapinfoFmt>();
			foreach (KeyValuePair<int, Mst_mapinfo> item in Mst_DataManager.Instance.Mst_mapinfo)
			{
				User_MapinfoFmt user_MapinfoData = item.Value.GetUser_MapinfoData();
				if (user_MapinfoData != null)
				{
					api_Result.data.Add(user_MapinfoData.Id, user_MapinfoData);
				}
				else if (item.Value.IsOpenMapSys())
				{
					user_MapinfoData = new User_MapinfoFmt();
					user_MapinfoData.Id = item.Value.Id;
					api_Result.data.Add(user_MapinfoData.Id, user_MapinfoData);
				}
			}
			return api_Result;
		}

		public Api_Result<User_RecordFmt> Record()
		{
			Api_Result<User_RecordFmt> api_Result = new Api_Result<User_RecordFmt>();
			Mem_basic user_basic = Comm_UserDatas.Instance.User_basic;
			Mem_record user_record = Comm_UserDatas.Instance.User_record;
			User_RecordFmt user_RecordFmt = new User_RecordFmt();
			user_basic.SetUserRecordData(user_RecordFmt);
			user_record.SetUserRecordData(user_RecordFmt);
			user_RecordFmt.Deck_num = Comm_UserDatas.Instance.User_deck.Count();
			user_RecordFmt.Ship_num = Comm_UserDatas.Instance.User_ship.Count();
			user_RecordFmt.Slot_num = Comm_UserDatas.Instance.User_slot.Count();
			user_RecordFmt.Ndock_num = Comm_UserDatas.Instance.User_ndock.Count();
			user_RecordFmt.Kdock_num = Comm_UserDatas.Instance.User_kdock.Count();
			int num = Comm_UserDatas.Instance.User_furniture.Values.Count((Mem_furniture x) => (Mst_DataManager.Instance.Mst_furniture[x.Rid].Price == 0 && Mst_DataManager.Instance.Mst_furniture[x.Rid].Rarity == 0 && Mst_DataManager.Instance.Mst_furniture[x.Rid].Title.Equals("なし")) ? true : false);
			user_RecordFmt.Furniture_num = Comm_UserDatas.Instance.User_furniture.Count - num;
			user_RecordFmt.Material_max = user_basic.GetMaterialMaxNum();
			api_Result.data = user_RecordFmt;
			return api_Result;
		}

		public Api_Result<Dictionary<int, Mem_useitem>> UseItem()
		{
			Api_Result<Dictionary<int, Mem_useitem>> api_Result = new Api_Result<Dictionary<int, Mem_useitem>>();
			if (Comm_UserDatas.Instance.User_useItem == null || Comm_UserDatas.Instance.User_useItem.Count == 0)
			{
				return api_Result;
			}
			api_Result.data = Comm_UserDatas.Instance.User_useItem.ToDictionary((KeyValuePair<int, Mem_useitem> x) => x.Key, (KeyValuePair<int, Mem_useitem> y) => y.Value);
			return api_Result;
		}

		public void InitBookData()
		{
			IEnumerable<XElement> source = Utils.Xml_Result("mst_shiptext", "mst_shiptext", null);
			IEnumerable<XElement> source2 = Utils.Xml_Result("mst_slotitemtext", "mst_slotitemtext", null);
			IEnumerable<XElement> source3 = Utils.Xml_Result("mst_ship_class", "mst_ship_class", null);
			tempShipBookText = source.ToDictionary((XElement key) => int.Parse(key.Element("Id").Value), (XElement value) => value.Element("Sinfo").Value);
			tempSlotBookText = source2.ToDictionary((XElement key) => int.Parse(key.Element("Id").Value), (XElement value) => value.Element("Info").Value);
			tempShipClass = source3.ToDictionary((XElement key) => int.Parse(key.Element("Id").Value), (XElement value) => value.Element("Name").Value);
			tempMstShipBook = ArrayMaster.GetShipBookInfo();
		}

		public Api_Result<Dictionary<int, User_BookFmt<BookShipData>>> PictureShip()
		{
			Api_Result<Dictionary<int, User_BookFmt<BookShipData>>> api_Result = new Api_Result<Dictionary<int, User_BookFmt<BookShipData>>>();
			var dictionary = (from item in Comm_UserDatas.Instance.Ship_book.Values
				select new
				{
					id = item.Table_id,
					state = new List<int>
					{
						item.Flag1,
						item.Flag2,
						item.Flag3,
						item.Flag4,
						item.Flag5
					}
				}).ToDictionary(x => x.id, y => y);
			foreach (KeyValuePair<int, int> item in tempMstShipBook)
			{
				if (dictionary.ContainsKey(item.Key))
				{
					bool flag = dictionary.ContainsKey(item.Value);
					if (dictionary[item.Key].state[2] != 0 && flag)
					{
						dictionary[item.Value].state[2] = dictionary[item.Key].state[2];
					}
				}
			}
			int bookMax = Mst_DataManager.Instance.Mst_const[MstConstDataIndex.Book_max_ships].Int_value;
			List<Mst_ship> list = (from obj in Mst_DataManager.Instance.Mst_ship.Values
				orderby obj.Bookno
				select obj into x
				where x.Bookno > 0 && x.Bookno <= bookMax
				select x).ToList();
			Dictionary<int, User_BookFmt<BookShipData>> dictionary2 = new Dictionary<int, User_BookFmt<BookShipData>>();
			HashSet<Mst_ship> hashSet = new HashSet<Mst_ship>();
			foreach (Mst_ship item2 in list)
			{
				if (!hashSet.Contains(item2))
				{
					List<int> list2 = new List<int>();
					List<List<int>> list3 = new List<List<int>>();
					Mst_ship mst_ship = item2;
					bool flag2 = false;
					if (dictionary.ContainsKey(mst_ship.Id))
					{
						list2.Add(item2.Id);
						list3.Add(dictionary[item2.Id].state);
						flag2 = true;
					}
					hashSet.Add(mst_ship);
					for (int append_ship_id = item2.Append_ship_id; append_ship_id != 0; append_ship_id = Mst_DataManager.Instance.Mst_ship[append_ship_id].Append_ship_id)
					{
						hashSet.Add(Mst_DataManager.Instance.Mst_ship[append_ship_id]);
						if (dictionary.ContainsKey(append_ship_id))
						{
							list2.Add(append_ship_id);
							list3.Add(dictionary[append_ship_id].state);
						}
					}
					if (flag2)
					{
						string info = tempShipBookText[list2[0]];
						int ctype = Mst_DataManager.Instance.Mst_ship[list2[0]].Ctype;
						string cname = tempShipClass[ctype];
						User_BookFmt<BookShipData> user_BookFmt = new User_BookFmt<BookShipData>(list2[0], list2, list3, info, cname, null);
						if (user_BookFmt.State.Any((List<int> x) => x[2] != 0))
						{
							user_BookFmt.State.ForEach(delegate(List<int> flags)
							{
								flags[2] = 1;
							});
						}
						dictionary2.Add(user_BookFmt.IndexNo, user_BookFmt);
					}
					else
					{
						dictionary2.Add(item2.Bookno, null);
					}
				}
			}
			api_Result.data = dictionary2;
			tempShipBookText.Clear();
			tempShipBookText = null;
			tempShipClass.Clear();
			tempShipClass = null;
			return api_Result;
		}

		public Api_Result<Dictionary<int, User_BookFmt<BookSlotData>>> PictureSlot()
		{
			Api_Result<Dictionary<int, User_BookFmt<BookSlotData>>> api_Result = new Api_Result<Dictionary<int, User_BookFmt<BookSlotData>>>();
			var bookData = (from item in Comm_UserDatas.Instance.Slot_book.Values
				select new
				{
					id = item.Table_id,
					state = new List<int>
					{
						item.Flag1,
						item.Flag2,
						item.Flag3,
						item.Flag4,
						item.Flag5
					}
				}).ToDictionary(x => x.id, y => y);
			if (type3Dict == null)
			{
				IEnumerable<int> enumerable = (from x in Mst_DataManager.Instance.Mst_Slotitem
					select x.Value.Type3).Distinct();
				type3Dict = new Dictionary<int, List<int>>();
				using (IEnumerator<int> enumerator = enumerable.GetEnumerator())
				{
					int type3;
					while (enumerator.MoveNext())
					{
						type3 = enumerator.Current;
						List<int> value = (from stypes in Mst_DataManager.Instance.Mst_stype.Values
							where stypes.Equip.Contains(type3)
							select stypes.Id).ToList();
						type3Dict.Add(type3, value);
					}
				}
			}
			Dictionary<int, User_BookFmt<BookSlotData>> fmt = new Dictionary<int, User_BookFmt<BookSlotData>>();
			List<Mst_slotitem> list = (from x in Mst_DataManager.Instance.Mst_Slotitem.Values
				where x.Sortno > 0
				select x).ToList();
			list.ForEach(delegate(Mst_slotitem dispItem)
			{
				if (bookData.ContainsKey(dispItem.Id))
				{
					var anon = bookData[dispItem.Id];
					string value2 = null;
					if (!tempSlotBookText.TryGetValue(anon.id, out value2))
					{
						value2 = string.Empty;
					}
					List<int> ids = new List<int>
					{
						anon.id
					};
					List<List<int>> state = new List<List<int>>
					{
						anon.state
					};
					User_BookFmt<BookSlotData> user_BookFmt = new User_BookFmt<BookSlotData>(anon.id, ids, state, value2, null, type3Dict);
					fmt.Add(user_BookFmt.IndexNo, user_BookFmt);
				}
				else
				{
					fmt.Add(dispItem.Sortno, null);
				}
			});
			api_Result.data = fmt;
			tempSlotBookText.Clear();
			tempSlotBookText = null;
			return api_Result;
		}

		public Api_Result<List<Mem_ndock>> ndock()
		{
			Api_Result<List<Mem_ndock>> api_Result = new Api_Result<List<Mem_ndock>>();
			List<Mem_ndock> list = (from x in Comm_UserDatas.Instance.User_ndock.Values
				orderby x.Rid
				select x).ToList();
			foreach (Mem_ndock item in list)
			{
				item.RecoverEnd(timeChk: true);
			}
			api_Result.data = list;
			return api_Result;
		}

		public Api_Result<Dictionary<int, List<Mem_ndock>>> AreaNdock()
		{
			Api_Result<Dictionary<int, List<Mem_ndock>>> api_Result = new Api_Result<Dictionary<int, List<Mem_ndock>>>();
			Dictionary<int, User_StrategyMapFmt> data2 = StrategyInfo().data;
			IEnumerable<User_StrategyMapFmt> enumerable = from x in data2.Values
				where x.IsActiveArea
				select x;
			List<int> list = (from x in Comm_UserDatas.Instance.User_ndock.Values
				where x.Dock_no == 1
				select x into y
				select y.Area_id).ToList();
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			foreach (User_StrategyMapFmt item in enumerable)
			{
				dictionary.Add(item.Maparea.Id, item.Maparea.Ndocks_max);
				if (!list.Contains(item.Maparea.Id))
				{
					for (int i = 0; i < item.Maparea.Ndocks_init; i++)
					{
						Comm_UserDatas.Instance.Add_Ndock(item.Maparea.Id);
					}
				}
			}
			IEnumerable<IGrouping<int, Mem_ndock>> enumerable2 = from data in Comm_UserDatas.Instance.User_ndock.Values
				orderby data.Dock_no
				group data by data.Area_id;
			Dictionary<int, List<Mem_ndock>> dictionary2 = new Dictionary<int, List<Mem_ndock>>();
			foreach (IGrouping<int, Mem_ndock> item2 in enumerable2)
			{
				List<Mem_ndock> list2 = item2.ToList();
				if (dictionary.ContainsKey(list2[0].Area_id))
				{
					list2.Capacity = dictionary[list2[0].Area_id];
					dictionary2.Add(item2.Key, list2);
					list2.ForEach(delegate(Mem_ndock x)
					{
						x.RecoverEnd(timeChk: true);
					});
				}
			}
			api_Result.data = dictionary2;
			return api_Result;
		}

		public Api_Result<List<Mem_kdock>> kdock()
		{
			Api_Result<List<Mem_kdock>> api_Result = new Api_Result<List<Mem_kdock>>();
			List<Mem_kdock> list = (from x in Comm_UserDatas.Instance.User_kdock.Values
				orderby x.Rid
				select x).ToList();
			foreach (Mem_kdock item in list)
			{
				item.CreateEnd(timeChk: true);
			}
			api_Result.data = list;
			return api_Result;
		}

		public Api_Result<List<User_MissionFmt>> Mission()
		{
			Api_Result<List<User_MissionFmt>> api_Result = new Api_Result<List<User_MissionFmt>>();
			Mem_missioncomp mem_missioncomp = new Mem_missioncomp();
			api_Result.data = mem_missioncomp.GetActiveMission();
			return api_Result;
		}

		public Api_Result<List<Mst_furniture>> DecorateFurniture(int deck_rid)
		{
			Api_Result<List<Mst_furniture>> api_Result = new Api_Result<List<Mst_furniture>>();
			Mem_room value = null;
			if (!Comm_UserDatas.Instance.User_room.TryGetValue(deck_rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			api_Result.data = value.getFurnitureDatas();
			return api_Result;
		}

		public Api_Result<Dictionary<FurnitureKinds, List<Mst_furniture>>> FurnitureList()
		{
			Api_Result<Dictionary<FurnitureKinds, List<Mst_furniture>>> api_Result = new Api_Result<Dictionary<FurnitureKinds, List<Mst_furniture>>>();
			IEnumerable<IGrouping<int, Mst_furniture>> enumerable = from data in Comm_UserDatas.Instance.User_furniture.Values
				let master = Mst_DataManager.Instance.Mst_furniture[data.Rid]
				orderby data.Rid
				group master by master.Type;
			Dictionary<FurnitureKinds, List<Mst_furniture>> dictionary = new Dictionary<FurnitureKinds, List<Mst_furniture>>();
			foreach (IGrouping<int, Mst_furniture> item in enumerable)
			{
				dictionary.Add((FurnitureKinds)item.Key, item.ToList());
			}
			api_Result.data = dictionary;
			return api_Result;
		}

		public Api_Result<List<User_HistoryFmt>> HistoryList()
		{
			Api_Result<List<User_HistoryFmt>> api_Result = new Api_Result<List<User_HistoryFmt>>();
			List<User_HistoryFmt> list = api_Result.data = new List<User_HistoryFmt>();
			List<Mem_history> list2 = new List<Mem_history>();
			foreach (List<Mem_history> value in Comm_UserDatas.Instance.User_history.Values)
			{
				list2.AddRange(value);
			}
			if (list2.Count == 0)
			{
				return api_Result;
			}
			IOrderedEnumerable<Mem_history> orderedEnumerable = from x in list2
				orderby x.Rid
				select x;
			foreach (Mem_history item3 in orderedEnumerable)
			{
				if (item3.Type != 999)
				{
					User_HistoryFmt item = new User_HistoryFmt(item3);
					list.Add(item);
				}
			}
			if (Comm_UserDatas.Instance.User_kdock.Count == 0)
			{
				Mem_history mem_history = new Mem_history();
				mem_history.SetGameOverToLost(Comm_UserDatas.Instance.User_turn.Total_turn);
				User_HistoryFmt item2 = new User_HistoryFmt(mem_history);
				list.Add(item2);
			}
			api_Result.data = list;
			return api_Result;
		}

		public Mem_option Option()
		{
			return new Mem_option();
		}

		public Api_Result<Mem_deckpractice> DeckPractice()
		{
			Api_Result<Mem_deckpractice> api_Result = new Api_Result<Mem_deckpractice>();
			api_Result.data = Comm_UserDatas.Instance.User_deckpractice;
			return api_Result;
		}
	}
}
