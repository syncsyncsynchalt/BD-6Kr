using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers
{
	public class Api_req_Member : ICreateNewUser
	{
		private bool create_flag;

		public void PurgeUserData()
		{
			Comm_UserDatas.Instance.PurgeUserData(this, plusGame: false);
		}

		public HashSet<DifficultKind> GetClearDifficult()
		{
			HashSet<DifficultKind> hashSet = new HashSet<DifficultKind>();
			if (Comm_UserDatas.Instance.User_record == null)
			{
				return hashSet;
			}
			hashSet.UnionWith(Comm_UserDatas.Instance.User_record.ClearDifficult.ToList());
			return hashSet;
		}

		public Api_Result<bool> CreateNewUser(int saveno, string nickName, int ship_id, DifficultKind difficult)
		{
			Api_Result<bool> result = new Api_Result<bool>();
			create_flag = true;
			CreateNewUser(difficult, ship_id);
			create_flag = false;
			Update_NickName(nickName);
			return result;
		}

		public bool CreateNewUser(DifficultKind difficult, int firstShip)
		{
			if (!create_flag)
			{
				return false;
			}
			return Comm_UserDatas.Instance.CreateNewUser(this, difficult, firstShip);
		}

		public Api_Result<bool> NewGamePlus(string nickName, DifficultKind difficult, int firstShipId)
		{
			Api_Result<bool> api_Result = new Api_Result<bool>();
			api_Result.state = Api_Result_State.Parameter_Error;
			if (!Utils.IsValidNewGamePlus())
			{
				return api_Result;
			}
			if (!Comm_UserDatas.Instance.NewGamePlus(this, nickName, difficult, firstShipId))
			{
				return api_Result;
			}
			api_Result.state = Api_Result_State.Success;
			return api_Result;
		}

		private Api_Result<bool> SetDifficult(DifficultKind difficult)
		{
			Api_Result<bool> api_Result = new Api_Result<bool>();
			Comm_UserDatas.Instance.User_basic.SetDifficult(difficult);
			api_Result.data = true;
			return api_Result;
		}

		public Api_Result<Hashtable> Update_DeckName(int deck_id, string name)
		{
			Api_Result<Hashtable> api_Result = new Api_Result<Hashtable>();
			api_Result.data = null;
			Mem_deck value = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_id, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
			}
			value.SetDeckName(name);
			return api_Result;
		}

		public Api_Result<Hashtable> Update_NickName(string name)
		{
			Api_Result<Hashtable> api_Result = new Api_Result<Hashtable>();
			api_Result.data = new Hashtable();
			if (Comm_UserDatas.Instance.User_basic == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Comm_UserDatas.Instance.User_basic.UpdateNickName(name);
			return api_Result;
		}

		public Api_Result<Hashtable> Update_Comment(string comment)
		{
			Api_Result<Hashtable> api_Result = new Api_Result<Hashtable>();
			api_Result.data = new Hashtable();
			if (Comm_UserDatas.Instance.User_basic == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Comm_UserDatas.Instance.User_basic.UpdateComment(comment);
			return api_Result;
		}

		public Api_Result<User_ItemUseFmt> ItemUse(int useitem_id, bool force_flag, ItemExchangeKinds exchange_type)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			Dictionary<ItemGetKinds, Dictionary<int, int>> dictionary2 = new Dictionary<ItemGetKinds, Dictionary<int, int>>();
			Dictionary<int, Dictionary<int, int>> dictionary3 = new Dictionary<int, Dictionary<int, int>>();
			foreach (object value6 in Enum.GetValues(typeof(ItemGetKinds)))
			{
				dictionary2.Add((ItemGetKinds)(int)value6, new Dictionary<int, int>());
			}
			for (int i = 1; i <= 3; i++)
			{
				dictionary3.Add(i, new Dictionary<int, int>());
			}
			Api_Result<User_ItemUseFmt> api_Result = new Api_Result<User_ItemUseFmt>();
			api_Result.data = new User_ItemUseFmt();
			Mst_useitem value = null;
			if (!Mst_DataManager.Instance.Mst_useitem.TryGetValue(useitem_id, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (value.Usetype != 4)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (!Comm_UserDatas.Instance.User_useItem.TryGetValue(useitem_id, out Mem_useitem value2))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			int num = value.GetItemExchangeNum(exchange_type);
			if (num == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (value2.Value < num)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Dictionary<enumMaterialCategory, Mem_material> user_material = Comm_UserDatas.Instance.User_material;
			IEnumerable<XElement> enumerable = get_package_item(exchange_type, value);
			if (enumerable != null)
			{
				Mst_item_limit mst_item_limit = Mst_DataManager.Instance.Mst_item_limit[1];
				var enumerable2 = from item in enumerable
					let category = (enumMaterialCategory)(int)Enum.Parse(typeof(enumMaterialCategory), item.Element("Material_id").Value)
					select new
					{
						material_id = category,
						useitem_id = int.Parse(item.Element("Useitem_id").Value),
						slotitem_id = int.Parse(item.Element("Slotitem_id").Value),
						items = int.Parse(item.Element("Items").Value),
						max_items = mst_item_limit.GetMaterialLimit(Mst_DataManager.Instance.Mst_item_limit, category)
					};
				foreach (var item in enumerable2)
				{
					if (item.material_id != 0)
					{
						int num2 = LimitGetCount(user_material[item.material_id].Value, item.items, item.max_items);
						if (!force_flag && num2 != item.items)
						{
							api_Result.data.CautionFlag = true;
							return api_Result;
						}
						dictionary.Add(item.material_id, item.items);
						dictionary3[1].Add((int)item.material_id, num2);
					}
					else if (item.useitem_id != 0)
					{
						int now_count = 0;
						if (Comm_UserDatas.Instance.User_useItem.TryGetValue(item.useitem_id, out Mem_useitem value3))
						{
							now_count = value3.Value;
						}
						int value4 = LimitGetCount(now_count, item.items, item.max_items);
						dictionary2[ItemGetKinds.UseItem].Add(item.useitem_id, item.items);
						dictionary3[2].Add(item.useitem_id, value4);
					}
					else if (item.slotitem_id != 0)
					{
						dictionary2[ItemGetKinds.SlotItem].Add(item.slotitem_id, item.items);
						dictionary3[3].Add(item.slotitem_id, item.items);
					}
				}
				foreach (KeyValuePair<int, Dictionary<int, int>> item2 in dictionary3)
				{
					int key = item2.Key;
					foreach (KeyValuePair<int, int> item3 in item2.Value)
					{
						int key2 = item3.Key;
						int value5 = item3.Value;
						switch (key)
						{
						case 1:
						{
							enumMaterialCategory key3 = (enumMaterialCategory)key2;
							user_material[key3].Add_Material(value5);
							break;
						}
						case 2:
							Comm_UserDatas.Instance.Add_Useitem(key2, value5);
							break;
						case 3:
							Comm_UserDatas.Instance.Add_Slot(new List<int>
							{
								key2
							});
							break;
						}
					}
				}
			}
			else if (value.Id == 10 || value.Id == 11 || value.Id == 12)
			{
				Mem_basic user_basic = Comm_UserDatas.Instance.User_basic;
				int get_count = int.Parse(value.Description2) * value2.Value;
				int max_count = 200000;
				int num3 = LimitGetCount(user_basic.Fcoin, get_count, max_count);
				dictionary2[ItemGetKinds.UseItem].Add(44, num3);
				user_basic.AddCoin(num3);
				num = value2.Value;
			}
			else if (value.Id == 53)
			{
				Mem_basic user_basic2 = Comm_UserDatas.Instance.User_basic;
				if (user_basic2.Max_chara >= user_basic2.GetPortMaxExtendNum())
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				dictionary2[ItemGetKinds.UseItem].Add(53, 1);
				user_basic2.PortExtend(1);
			}
			value2.Sub_UseItem(num);
			api_Result.data.GetItem = GetItemFmt(dictionary2);
			api_Result.data.Material = dictionary;
			return api_Result;
		}

		private IEnumerable<XElement> get_package_item(ItemExchangeKinds exchange_type, Mst_useitem mst_useitem)
		{
			List<XElement> result = null;
			switch (exchange_type)
			{
			case ItemExchangeKinds.NONE:
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("Package_type", "2");
				dictionary.Add("Package_id", mst_useitem.Id.ToString());
				Dictionary<string, string> where_dict = dictionary;
				return Utils.Xml_Result_Where("mst_item_package", "mst_item_package", where_dict);
			}
			case ItemExchangeKinds.PLAN:
			{
				List<XElement> list = new List<XElement>();
				list.Add(new XElement("mst_item_package", new XElement("Id", "101"), new XElement("Package_type", "2"), new XElement("Package_id", "57"), new XElement("Material_id", "0"), new XElement("Useitem_id", "58"), new XElement("Slotitem_id", "0"), new XElement("Items", "1")));
				result = list;
				break;
			}
			case ItemExchangeKinds.REMODEL:
			{
				List<XElement> list = new List<XElement>();
				list.Add(new XElement("mst_item_package", new XElement("Id", "102"), new XElement("Package_type", "2"), new XElement("Package_id", "57"), new XElement("Material_id", "8"), new XElement("Useitem_id", "0"), new XElement("Slotitem_id", "0"), new XElement("Items", "4")));
				result = list;
				break;
			}
			case ItemExchangeKinds.PRESENT_MATERIAL:
			{
				List<XElement> list = new List<XElement>();
				list.Add(new XElement("mst_item_package", new XElement("Id", "103"), new XElement("Package_type", "2"), new XElement("Package_id", "60"), new XElement("Material_id", "7"), new XElement("Useitem_id", "0"), new XElement("Slotitem_id", "0"), new XElement("Items", "3")));
				list.Add(new XElement("mst_item_package", new XElement("Id", "104"), new XElement("Package_type", "2"), new XElement("Package_id", "60"), new XElement("Material_id", "8"), new XElement("Useitem_id", "0"), new XElement("Slotitem_id", "0"), new XElement("Items", "1")));
				result = list;
				break;
			}
			case ItemExchangeKinds.PRESENT_IRAKO:
			{
				List<XElement> list = new List<XElement>();
				list.Add(new XElement("mst_item_package", new XElement("Id", "105"), new XElement("Package_type", "2"), new XElement("Package_id", "60"), new XElement("Material_id", "0"), new XElement("Useitem_id", "59"), new XElement("Slotitem_id", "0"), new XElement("Items", "1")));
				result = list;
				break;
			}
			}
			return result;
		}

		private int LimitGetCount(int now_count, int get_count, int max_count)
		{
			int result = get_count;
			int num = now_count + get_count;
			if (num > max_count)
			{
				result = max_count - now_count;
			}
			return result;
		}

		private List<ItemGetFmt> GetItemFmt(Dictionary<ItemGetKinds, Dictionary<int, int>> fmt_base)
		{
			List<ItemGetFmt> list = new List<ItemGetFmt>();
			foreach (KeyValuePair<ItemGetKinds, Dictionary<int, int>> item in fmt_base)
			{
				ItemGetKinds key = item.Key;
				foreach (KeyValuePair<int, int> item2 in item.Value)
				{
					if (item2.Key > 0)
					{
						ItemGetFmt itemGetFmt = new ItemGetFmt();
						itemGetFmt.Category = key;
						itemGetFmt.Id = item2.Key;
						itemGetFmt.Count = item2.Value;
						list.Add(itemGetFmt);
					}
				}
			}
			return list;
		}

		public bool[] itemuse_cond_check(int deck_id)
		{
			bool[] array = new bool[2];
			Dictionary<int, Mem_useitem> user_useItem = Comm_UserDatas.Instance.User_useItem;
			int[] array2 = new int[2];
			Mem_useitem value = null;
			Mem_useitem value2 = null;
			Comm_UserDatas.Instance.User_useItem.TryGetValue(54, out value);
			if (value != null)
			{
				array2[0] = user_useItem[54].Value;
			}
			Comm_UserDatas.Instance.User_useItem.TryGetValue(59, out value2);
			if (value2 != null)
			{
				array2[1] = user_useItem[59].Value;
			}
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_id, out Mem_deck value3))
			{
				return array;
			}
			List<Mem_ship> memShip = value3.Ship.getMemShip();
			if (memShip.Count == 0)
			{
				return array;
			}
			if (array2[1] >= 1)
			{
				array[1] = true;
			}
			if (array2[0] >= 1)
			{
				foreach (Mem_ship item in memShip)
				{
					if (item.Cond < 40)
					{
						array[0] = true;
					}
				}
				return array;
			}
			return array;
		}

		public Api_Result<bool> itemuse_cond(int deck_id, HashSet<int> useitem_id)
		{
			Api_Result<bool> api_Result = new Api_Result<bool>();
			api_Result.data = false;
			int num;
			if (useitem_id.Contains(54) && useitem_id.Contains(59))
			{
				num = 3;
			}
			else if (useitem_id.Contains(54))
			{
				num = 1;
			}
			else
			{
				if (!useitem_id.Contains(59))
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				num = 2;
			}
			Dictionary<int, Mem_useitem> user_useItem = Comm_UserDatas.Instance.User_useItem;
			List<Mem_useitem> list = new List<Mem_useitem>();
			foreach (int item in useitem_id)
			{
				Mem_useitem value = null;
				user_useItem.TryGetValue(item, out value);
				list.Add(value);
			}
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_id, out Mem_deck value2))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			List<Mem_ship> memShip = value2.Ship.getMemShip();
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 100;
			int num7 = 0;
			int maxValue = 100;
			List<Mem_ship> list2 = new List<Mem_ship>();
			switch (num)
			{
			case 1:
			{
				num2 = 50;
				num4 = 40;
				maxValue = 100;
				Random random2 = new Random();
				foreach (Mem_ship item2 in memShip)
				{
					int num10 = 0;
					if (item2.Cond < 50)
					{
						if (item2.Rid == memShip[0].Rid)
						{
							num10 = num2;
						}
						else
						{
							int num11 = random2.Next(maxValue);
							if (item2.Stype == 2)
							{
								if (num11 < 80)
								{
									num10 = num2;
								}
							}
							else if (item2.Stype == 3)
							{
								if (num11 < 50)
								{
									num10 = num2;
								}
							}
							else if (num11 < 30)
							{
								num10 = num2;
							}
							if (item2.Cond < 40)
							{
								num10 = num4;
							}
						}
					}
					if (num10 > 0)
					{
						Mem_shipBase mem_shipBase2 = new Mem_shipBase(item2);
						mem_shipBase2.Cond = num10;
						item2.Set_ShipParam(mem_shipBase2, Mst_DataManager.Instance.Mst_ship[mem_shipBase2.Ship_id], enemy_flag: false);
						list2.Add(item2);
					}
				}
				break;
			}
			case 2:
			case 3:
			{
				Dictionary<int, int> dictionary = new Dictionary<int, int>();
				Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
				if (num == 2)
				{
					num2 = 65;
					num3 = 25;
					num4 = 65;
					num5 = 25;
					num6 = 100;
					num7 = 81;
					maxValue = 100;
					dictionary.Add(2, 20);
					dictionary.Add(3, 10);
					dictionary.Add(13, 20);
					dictionary.Add(14, 20);
					dictionary2.Add("ながと", 30);
					dictionary2.Add("かが", 15);
					dictionary2.Add("すずや", 15);
				}
				if (num == 3)
				{
					num2 = 70;
					num3 = 30;
					num4 = 65;
					num5 = 20;
					num6 = 100;
					num7 = 0;
					maxValue = 11;
					dictionary.Add(2, 10);
					dictionary.Add(3, 5);
					dictionary.Add(13, 10);
					dictionary.Add(14, 10);
					dictionary2.Add("ながと", 10);
					dictionary2.Add("かが", 5);
					dictionary2.Add("すずや", 5);
				}
				Random random = new Random();
				foreach (Mem_ship item3 in memShip)
				{
					int num8 = 0;
					if (item3.Cond < num6)
					{
						if (item3.Rid == memShip[0].Rid)
						{
							num8 = ((item3.Cond >= 41) ? (item3.Cond + num3) : num2);
						}
						else
						{
							int num9 = random.Next(maxValue);
							int value3 = 0;
							dictionary.TryGetValue(item3.Stype, out value3);
							if (value3 != 0)
							{
								num9 += value3;
							}
							int value4 = 0;
							dictionary2.TryGetValue(Mst_DataManager.Instance.Mst_ship[item3.Ship_id].Yomi, out value4);
							if (value4 != 0)
							{
								num9 += value4;
							}
							if (num9 >= num7)
							{
								num8 = ((item3.Cond >= 41) ? ((num != 2) ? (item3.Cond + num5 + num9) : (item3.Cond + num5)) : num4);
							}
						}
						if (num8 > num6)
						{
							num8 = num6;
						}
						if (num8 > 0)
						{
							Mem_shipBase mem_shipBase = new Mem_shipBase(item3);
							mem_shipBase.Cond = num8;
							item3.Set_ShipParam(mem_shipBase, Mst_DataManager.Instance.Mst_ship[mem_shipBase.Ship_id], enemy_flag: false);
							list2.Add(item3);
						}
					}
				}
				break;
			}
			default:
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			foreach (Mem_ship item4 in list2)
			{
				item4.SumLovToUseFoodSupplyShip(num);
			}
			foreach (Mem_useitem item5 in list)
			{
				item5.Sub_UseItem(1);
			}
			api_Result.data = true;
			return api_Result;
		}

		public bool IsValidKdockOpen()
		{
			if (!Comm_UserDatas.Instance.User_useItem.TryGetValue(49, out Mem_useitem value))
			{
				return false;
			}
			if (value.Value == 0)
			{
				return false;
			}
			int num = 4;
			int count = Comm_UserDatas.Instance.User_kdock.Count;
			if (count >= num)
			{
				return false;
			}
			return true;
		}

		public Api_Result<Mem_kdock> KdockOpen()
		{
			Api_Result<Mem_kdock> api_Result = new Api_Result<Mem_kdock>();
			if (!IsValidKdockOpen())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_kdock data = Comm_UserDatas.Instance.Add_Kdock();
			Comm_UserDatas.Instance.User_useItem[49].Sub_UseItem(1);
			api_Result.data = data;
			return api_Result;
		}

		public bool IsValidNdockOpen(int area_id)
		{
			if (!Comm_UserDatas.Instance.User_useItem.TryGetValue(49, out Mem_useitem value))
			{
				return false;
			}
			if (value.Value <= 0)
			{
				return false;
			}
			if (!Mst_DataManager.Instance.Mst_maparea.TryGetValue(area_id, out Mst_maparea value2))
			{
				return false;
			}
			int num = (from data in Comm_UserDatas.Instance.User_ndock.Values
				where data.Area_id == area_id
				select data).Count();
			if (!value2.IsActiveArea())
			{
				return false;
			}
			if (num >= value2.Ndocks_max)
			{
				return false;
			}
			return true;
		}

		public Api_Result<Mem_ndock> NdockOpen(int area_id)
		{
			Api_Result<Mem_ndock> api_Result = new Api_Result<Mem_ndock>();
			if (!IsValidNdockOpen(area_id))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_ndock data = Comm_UserDatas.Instance.Add_Ndock(area_id);
			Comm_UserDatas.Instance.User_useItem[49].Sub_UseItem(1);
			api_Result.data = data;
			return api_Result;
		}
	}
}
