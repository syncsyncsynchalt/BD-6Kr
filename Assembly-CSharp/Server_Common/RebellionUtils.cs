using Common.Enum;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Common
{
	public class RebellionUtils : IRebellionPointOperator
	{
		private List<Mst_rebellionpoint> mst_rpoint;

		private readonly double rpsw;

		private Dictionary<int, bool> areaOpenState;

		public RebellionUtils()
		{
			mst_rpoint = Mst_DataManager.Instance.Mst_RebellionPoint.Values.ToList();
			if (Comm_UserDatas.Instance.User_turn.Total_turn <= 99)
			{
				rpsw = 0.5;
				if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.HEI)
				{
					rpsw = 0.4;
				}
				else if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.TEI)
				{
					rpsw = 0.3;
				}
			}
			else if (Comm_UserDatas.Instance.User_turn.Total_turn <= 199)
			{
				rpsw = 0.6;
				if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.HEI)
				{
					rpsw = 0.5;
				}
				else if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.TEI)
				{
					rpsw = 0.4;
				}
			}
			else
			{
				rpsw = 0.7;
				if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.HEI)
				{
					rpsw = 0.6;
				}
				else if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.TEI)
				{
					rpsw = 0.5;
				}
			}
		}

		void IRebellionPointOperator.AddRebellionPoint(int area_id, int addNum)
		{
			Comm_UserDatas.Instance.User_rebellion_point[area_id].AddPoint(this, addNum);
		}

		void IRebellionPointOperator.SubRebellionPoint(int area_id, int subNum)
		{
			throw new NotImplementedException();
		}

		public void LostArea(int maparea_id, List<int> sortieDeckRid)
		{
			Comm_UserDatas commInstance = Comm_UserDatas.Instance;
			if (sortieDeckRid == null)
			{
				sortieDeckRid = new List<int>();
			}
			List<int> lostArea = getLostArea(maparea_id);
			lostArea.ForEach(delegate(int lostAreaId)
			{
				Dictionary<int, Mem_mapclear> dictionary = (from x in commInstance.User_mapclear.Values
					where x.Maparea_id == lostAreaId
					select x).ToDictionary((Mem_mapclear x) => x.Mapinfo_no, (Mem_mapclear y) => y);
				int num = Mst_maparea.MaxMapNum(commInstance.User_basic.Difficult, lostAreaId);
				for (int i = 1; i <= num; i++)
				{
					if (!dictionary.ContainsKey(i))
					{
						int mapinfo_id = Mst_mapinfo.ConvertMapInfoId(lostAreaId, i);
						Mem_mapclear mem_mapclear = new Mem_mapclear(mapinfo_id, lostAreaId, i, MapClearState.InvationClose);
						mem_mapclear.Insert();
						dictionary.Add(i, mem_mapclear);
					}
				}
				lostMapClear(dictionary.Values.ToList(), Mst_DataManager.Instance.Mst_maparea[lostAreaId].Neighboring_area);
				Comm_UserDatas.Instance.User_rebellion_point.Remove(lostAreaId);
				List<Mem_tanker> areaTanker = Mem_tanker.GetAreaTanker(lostAreaId);
				lostTanker(areaTanker);
				IEnumerable<Mem_deck> memDeck = from x in commInstance.User_deck.Values
					where x.Area_id == lostAreaId
					select x;
				goBackDeck(memDeck, sortieDeckRid);
				Mem_esccort_deck escort = commInstance.User_EscortDeck[lostAreaId];
				goBackEscortDeck(escort);
				List<Mem_ndock> ndock = (from x in commInstance.User_ndock.Values
					where x.Area_id == lostAreaId
					select x).ToList();
				lostNdock(ndock);
				Dictionary<enumMaterialCategory, Mem_material> user_material = commInstance.User_material;
				lostMaterial(user_material);
				if (lostAreaId == 1)
				{
					lostKdock();
				}
			});
		}

		private int debugMapSetting(out Dictionary<int, bool> dict)
		{
			dict = new Dictionary<int, bool>();
			int result = 0;
			List<bool> list = null;
			for (int i = 1; i < list.Count; i++)
			{
				dict.Add(i, list[i]);
			}
			return result;
		}

		public List<int> getLostArea(int ownerAreaId)
		{
			Dictionary<int, bool> sData = new Api_get_Member().StrategyInfo().data.ToDictionary((KeyValuePair<int, User_StrategyMapFmt> x) => x.Key, (KeyValuePair<int, User_StrategyMapFmt> y) => y.Value.IsActiveArea);
			sData[ownerAreaId] = false;
			List<int> list = new List<int>();
			list.Add(ownerAreaId);
			List<int> list2 = list;
			if (ownerAreaId == 1)
			{
				return list2;
			}
			Dictionary<int, Mst_maparea> mapareaDict = Mst_DataManager.Instance.Mst_maparea;
			List<int> other = mapareaDict[1].Neighboring_area.FindAll((int x) => sData[x]);
			bool flag = false;
			HashSet<int> hashSet = new HashSet<int>();
			HashSet<int> passItems = new HashSet<int>();
			hashSet.UnionWith(other);
			while (!flag)
			{
				List<List<int>> list3 = (from rootItem in hashSet
					let mstdata = mapareaDict[rootItem].Neighboring_area.FindAll((int x) => sData[x])
					select mstdata).ToList();
				hashSet.Clear();
				for (int i = 0; i < list3.Count(); i++)
				{
					hashSet.UnionWith(list3[i]);
					hashSet.RemoveWhere((int x) => passItems.Contains(x));
				}
				if (hashSet.Count == 0)
				{
					flag = true;
				}
				else
				{
					passItems.UnionWith(hashSet);
				}
			}
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.UnionWith(list2);
			if (passItems.Count == 0)
			{
				IEnumerable<int> other2 = from x in sData
					where x.Value && x.Key != 1
					select x into y
					select y.Key;
				hashSet2.UnionWith(other2);
				return hashSet2.ToList();
			}
			IEnumerable<int> first = from x in sData
				where x.Value
				select x into y
				select y.Key;
			List<int> list4 = first.Except(passItems).ToList();
			if (list4.Count == 0)
			{
				hashSet2.UnionWith(list2);
			}
			else
			{
				hashSet2.UnionWith(list4);
			}
			return hashSet2.ToList();
		}

		private void lostTanker(List<Mem_tanker> tanker)
		{
			foreach (Mem_tanker item in tanker)
			{
				if (!item.IsBlingShip())
				{
					Comm_UserDatas.Instance.User_tanker.Remove(item.Rid);
				}
				else
				{
					item.BackTanker();
				}
			}
		}

		private void goBackEscortDeck(Mem_esccort_deck escort)
		{
			List<Mem_ship> memShip = escort.Ship.getMemShip();
			memShip.ForEach(delegate(Mem_ship x)
			{
				if (!x.IsBlingShip())
				{
					x.SubHp(getShipDamage(x.Nowhp));
				}
				x.PortWithdraw(escort.Rid);
			});
			escort.EscortStop();
			escort.Ship.Clear();
		}

		private void lostMaterial(Dictionary<enumMaterialCategory, Mem_material> material)
		{
			double num = (double)material[enumMaterialCategory.Fuel].Value * 0.15;
			material[enumMaterialCategory.Fuel].Sub_Material((int)num);
			num = (double)material[enumMaterialCategory.Bull].Value * 0.15;
			material[enumMaterialCategory.Bull].Sub_Material((int)num);
			num = (double)material[enumMaterialCategory.Steel].Value * 0.1;
			material[enumMaterialCategory.Steel].Sub_Material((int)num);
			num = (double)material[enumMaterialCategory.Bauxite].Value * 0.1;
			material[enumMaterialCategory.Bauxite].Sub_Material((int)num);
		}

		private void lostNdock(List<Mem_ndock> ndock)
		{
			ndock.ForEach(delegate(Mem_ndock x)
			{
				int area_id = x.Area_id;
				if (x.Ship_id > 0)
				{
					Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[x.Ship_id];
					mem_ship.SubHp(getShipDamage(mem_ship.Nowhp));
					mem_ship.PortWithdraw(area_id);
				}
				Comm_UserDatas.Instance.User_ndock.Remove(x.Rid);
			});
		}

		private void lostKdock()
		{
			Comm_UserDatas.Instance.User_kdock.Clear();
		}

		private void lostMapClear(List<Mem_mapclear> ownMapClear, List<int> neighboringArea)
		{
			ownMapClear.ForEach(delegate(Mem_mapclear x)
			{
				x.StateChange(MapClearState.InvationClose);
			});
			neighboringArea.ForEach(delegate(int area)
			{
				int mapinfo_no = Mst_maparea.MaxMapNum(Comm_UserDatas.Instance.User_basic.Difficult, area);
				int key = Mst_mapinfo.ConvertMapInfoId(area, mapinfo_no);
				if (Comm_UserDatas.Instance.User_mapclear.TryGetValue(key, out Mem_mapclear value) && value.State != MapClearState.InvationClose)
				{
					value.StateChange(MapClearState.InvationNeighbor);
				}
			});
		}

		private void goBackDeck(IEnumerable<Mem_deck> memDeck, List<int> sortieDeck)
		{
			using (IEnumerator<Mem_deck> enumerator = memDeck.GetEnumerator())
			{
				Mem_deck deckItem;
				while (enumerator.MoveNext())
				{
					deckItem = enumerator.Current;
					List<Mem_ship> memShip = deckItem.Ship.getMemShip();
					memShip.ForEach(delegate(Mem_ship ship)
					{
						ship.PortWithdraw(deckItem.Area_id);
						if (!sortieDeck.Contains(deckItem.Rid))
						{
							ship.SubHp(getShipDamage(ship.Nowhp));
						}
					});
					deckItem.MissionEnforceEnd();
					deckItem.MoveArea(1);
				}
			}
		}

		private int getShipDamage(int nowHp)
		{
			double randDouble = Utils.GetRandDouble(1.0, 82.0, 1.0, 1);
			double num = (double)nowHp * (randDouble / 100.0);
			return (int)num;
		}

		public bool MapReOpen(Mem_mapclear clearData, out List<int> reOpenMap)
		{
			reOpenMap = new List<int>();
			if (clearData.State != 0)
			{
				return false;
			}
			Dictionary<int, Mem_mapclear> dictionary = (from data in Comm_UserDatas.Instance.User_mapclear.Values
				where data.State == MapClearState.InvationClose && data.Mapinfo_no == 1
				select data).ToDictionary((Mem_mapclear key) => key.Rid, (Mem_mapclear value) => value);
			if (dictionary.Count == 0)
			{
				return true;
			}
			Dictionary<int, Mst_maparea> mst_maparea = Mst_DataManager.Instance.Mst_maparea;
			int maparea_id = clearData.Maparea_id;
			int num = Mst_maparea.MaxMapNum(Comm_UserDatas.Instance.User_basic.Difficult, clearData.Maparea_id);
			bool flag = (clearData.Mapinfo_no == num) ? true : false;
			new Dictionary<int, List<int>>();
			Dictionary<int, List<int>> dictionary2 = new Dictionary<int, List<int>>();
			foreach (Mem_mapclear value3 in dictionary.Values)
			{
				Mst_mapinfo mst_mapinfo = Mst_DataManager.Instance.Mst_mapinfo[value3.Rid];
				if (mst_mapinfo.Required_ids.Contains(clearData.Rid))
				{
					dictionary2.Add(value3.Maparea_id, mst_mapinfo.Required_ids);
				}
				else if (flag && mst_maparea[value3.Maparea_id].Neighboring_area.Contains(maparea_id))
				{
					if (value3.Maparea_id == 7 || value3.Maparea_id == 8)
					{
						dictionary2.Add(value3.Maparea_id, new List<int>
						{
							Mst_mapinfo.ConvertMapInfoId(1, 4)
						});
					}
					else
					{
						List<int> list = mst_mapinfo.Required_ids.ToList();
						list.Add(clearData.Rid);
						dictionary2.Add(value3.Maparea_id, list);
					}
				}
			}
			if (dictionary2.Count == 0)
			{
				return true;
			}
			bool result = false;
			foreach (KeyValuePair<int, List<int>> item in dictionary2)
			{
				int areaId = item.Key;
				List<int> value2 = item.Value;
				if (value2.Count == 0)
				{
					throw new Exception();
				}
				if (mapReOpenCheck(value2))
				{
					result = true;
					IEnumerable<Mem_mapclear> enumerable = from x in Comm_UserDatas.Instance.User_mapclear.Values
						where x.Maparea_id == areaId
						select x;
					foreach (Mem_mapclear item2 in enumerable)
					{
						item2.StateChange(MapClearState.InvationOpen);
						reOpenMap.Add(item2.Rid);
					}
				}
			}
			return result;
		}

		private bool mapReOpenCheck(List<int> targetMapInfoIds)
		{
			foreach (int targetMapInfoId in targetMapInfoIds)
			{
				Mem_mapclear value = null;
				if (!Comm_UserDatas.Instance.User_mapclear.TryGetValue(targetMapInfoId, out value))
				{
					return false;
				}
				if (value.State != 0)
				{
					return false;
				}
			}
			return true;
		}

		public int AddPointTo_RPTable(Mst_maparea targetArea)
		{
			if (areaOpenState == null)
			{
				initAreaOpenState();
			}
			if (!areaOpenState[targetArea.Id])
			{
				return 0;
			}
			if (!Comm_UserDatas.Instance.User_rebellion_point.ContainsKey(targetArea.Id))
			{
				Mem_rebellion_point mem_rebellion_point = new Mem_rebellion_point(targetArea.Id);
				Comm_UserDatas.Instance.User_rebellion_point.Add(mem_rebellion_point.Rid, mem_rebellion_point);
			}
			Mst_rebellionpoint mstRebellionRecord = getMstRebellionRecord();
			if (mstRebellionRecord == null)
			{
				return 0;
			}
			if (mstRebellionRecord.Area_value.ContainsKey(targetArea.Id))
			{
				double num = mstRebellionRecord.Area_value[targetArea.Id];
				List<int> neighboring_area = targetArea.Neighboring_area;
				int num2 = 0;
				foreach (int item in neighboring_area)
				{
					if (!areaOpenState[item])
					{
						num2++;
					}
				}
				if (targetArea.Id == 4 && num2 == 0)
				{
					num2 = 1;
				}
				if (num2 == 0)
				{
					return 0;
				}
				double num3 = neighboring_area.Count;
				double num4 = num * (0.5 + 0.5 * ((double)num2 + 1.0) / num3);
				int num5 = (int)(num4 * rpsw);
				((IRebellionPointOperator)this).AddRebellionPoint(targetArea.Id, num5);
				return num5;
			}
			return 0;
		}

		private void initAreaOpenState()
		{
			areaOpenState = new Dictionary<int, bool>();
			foreach (Mst_maparea value in Mst_DataManager.Instance.Mst_maparea.Values)
			{
				areaOpenState.Add(value.Id, value.IsActiveArea());
			}
		}

		private Mst_rebellionpoint getMstRebellionRecord()
		{
			int total_turn = Comm_UserDatas.Instance.User_turn.Total_turn;
			foreach (Mst_rebellionpoint item in mst_rpoint)
			{
				if (item.Turn_from <= total_turn && item.Turn_to >= total_turn)
				{
					return item;
				}
				if (item.Turn_from <= total_turn && item.Turn_to == -1)
				{
					return item;
				}
			}
			return null;
		}
	}
}
