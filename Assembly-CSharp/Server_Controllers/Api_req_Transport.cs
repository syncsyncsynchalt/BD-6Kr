using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers
{
	public class Api_req_Transport
	{
		private Dictionary<int, int> mst_escort_group;

		public Dictionary<int, int> Mst_escort_group
		{
			get
			{
				return mst_escort_group;
			}
			private set
			{
				mst_escort_group = value;
			}
		}

		public void initEscortGroup()
		{
			if (mst_escort_group == null)
			{
				IEnumerable<XElement> source = Utils.Xml_Result("mst_stype_group", "mst_stype_group", "Id");
				mst_escort_group = source.ToDictionary((XElement key) => int.Parse(key.Element("Id").Value), (XElement value) => int.Parse(value.Element("Escort").Value));
			}
		}

		public Dictionary<enumMaterialCategory, int> GetMaterialNum(int area_id, int tankerNum, DeckShips deckShip)
		{
			Dictionary<enumMaterialCategory, int> addValues = new Dictionary<enumMaterialCategory, int>();
			foreach (object value2 in Enum.GetValues(typeof(enumMaterialCategory)))
			{
				addValues.Add((enumMaterialCategory)(int)value2, 0);
			}
			Mst_maparea value = null;
			if (!Mst_DataManager.Instance.Mst_maparea.TryGetValue(area_id, out value))
			{
				return addValues;
			}
			DeckShips deckShip2 = (deckShip != null) ? deckShip : Comm_UserDatas.Instance.User_EscortDeck[area_id].Ship;
			value.TakeMaterialNum(Comm_UserDatas.Instance.User_mapclear, tankerNum, ref addValues, randMaxFlag: true, deckShip2);
			return addValues;
		}

		public Api_Result<List<Mem_tanker>> GoTanker(int area_id, int num)
		{
			Api_Result<List<Mem_tanker>> api_Result = new Api_Result<List<Mem_tanker>>();
			List<Mem_tanker> freeTanker = Mem_tanker.GetFreeTanker(Comm_UserDatas.Instance.User_tanker);
			if (freeTanker.Count() < num)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			IEnumerable<Mem_tanker> enumerable = freeTanker.Take(num);
			foreach (Mem_tanker item in enumerable)
			{
				item.GoArea(area_id);
			}
			api_Result.data = enumerable.ToList();
			return api_Result;
		}

		public List<Mem_tanker> GetEnableBackTanker(int area_id)
		{
			IEnumerable<Mem_tanker> source = from x in Comm_UserDatas.Instance.User_tanker.Values
				where x.Maparea_id == area_id && !x.IsBlingShip() && x.Disposition_status == DispositionStatus.ARRIVAL
				select x;
			return source.ToList();
		}

		public Api_Result<List<Mem_tanker>> BackTanker(int area_id, int num)
		{
			Api_Result<List<Mem_tanker>> api_Result = new Api_Result<List<Mem_tanker>>();
			List<Mem_tanker> enableBackTanker = GetEnableBackTanker(area_id);
			if (enableBackTanker.Count() < num)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			IEnumerable<Mem_tanker> enumerable = enableBackTanker.Take(num);
			foreach (Mem_tanker item in enumerable)
			{
				item.BackTanker();
			}
			api_Result.data = enumerable.ToList();
			return api_Result;
		}

		public Api_Result<Mem_esccort_deck> Update_DeckName(int deck_rid, string name)
		{
			Api_Result<Mem_esccort_deck> api_Result = new Api_Result<Mem_esccort_deck>();
			api_Result.data = null;
			Mem_esccort_deck value = null;
			if (!Comm_UserDatas.Instance.User_EscortDeck.TryGetValue(deck_rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
			}
			value.SetDeckName(name);
			api_Result.data = value;
			return api_Result;
		}

		public bool IsValidChange(int deck_rid, int deck_targetIdx, int ship_rid, DeckShips deckShip)
		{
			initEscortGroup();
			Mem_esccort_deck value = null;
			if (!Comm_UserDatas.Instance.User_EscortDeck.TryGetValue(deck_rid, out value))
			{
				return false;
			}
			List<int> unsetShips = null;
			if (ship_rid == -2)
			{
				if (deckShip.Count() <= 1)
				{
					return false;
				}
				deckShip.Clone(out unsetShips);
				unsetShips.RemoveAt(0);
			}
			else if (ship_rid == -1)
			{
				unsetShips = new List<int>
				{
					deckShip[deck_targetIdx]
				};
			}
			else
			{
				if (ship_rid == deckShip[deck_targetIdx])
				{
					return false;
				}
				int stype = Comm_UserDatas.Instance.User_ship[ship_rid].Stype;
				if (Mst_DataManager.Instance.Mst_stype[stype].IsSubmarine())
				{
					return false;
				}
				unsetShips = new List<int>
				{
					ship_rid
				};
			}
			DeckShips out_ships = null;
			deckShip.Clone(out out_ships);
			Change_TempDeck(deck_targetIdx, ship_rid, ref out_ships);
			if (!validEscortStypeCheck(out_ships, -1))
			{
				return false;
			}
			Mem_ndock mem_ndock = Comm_UserDatas.Instance.User_ndock.Values.FirstOrDefault((Mem_ndock x) => unsetShips.Contains(x.Ship_id) ? true : false);
			if (mem_ndock != null)
			{
				return false;
			}
			if (unsetShips.Any(delegate(int bling_rid)
			{
				Mem_ship value3 = null;
				return Comm_UserDatas.Instance.User_ship.TryGetValue(bling_rid, out value3) && value3.IsBlingShip();
			}))
			{
				return false;
			}
			if (ship_rid < 0)
			{
				return true;
			}
			if (Comm_UserDatas.Instance.User_deck.Values.Any((Mem_deck x) => x.Ship.Find(ship_rid) != -1))
			{
				return false;
			}
			if (Comm_UserDatas.Instance.User_EscortDeck.Values.Any((Mem_esccort_deck x) => x.Rid != deck_rid && x.Ship.Find(ship_rid) != -1))
			{
				return false;
			}
			Func<DeckShips, bool> func = delegate(DeckShips x)
			{
				int num = x.Count();
				List<string> list = new List<string>();
				for (int i = 0; i < num; i++)
				{
					int ship_id = Comm_UserDatas.Instance.User_ship[x[i]].Ship_id;
					list.Add(Mst_DataManager.Instance.Mst_ship[ship_id].Yomi);
				}
				IEnumerable<IGrouping<string, string>> source = from y in list
					group y by y;
				return (source.Count() == list.Count()) ? true : false;
			};
			Mem_ship value2 = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, out value2))
			{
				return false;
			}
			if (value2.IsBlingShip())
			{
				return false;
			}
			if (value2.BlingType == Mem_ship.BlingKind.WaitDeck)
			{
				return false;
			}
			if (value2.BlingType == Mem_ship.BlingKind.WaitEscort && value2.BlingWaitArea != value.Maparea_id)
			{
				return false;
			}
			if (deckShip.Equals(out_ships))
			{
				return false;
			}
			if (!func(out_ships))
			{
				return false;
			}
			return true;
		}

		private bool validEscortStypeCheck(DeckShips deckship, int deck_targetIdx)
		{
			if (deckship.Count() == 0)
			{
				return true;
			}
			return (DeckUtils.IsValidChangeEscort(deckship.getMemShip(), mst_escort_group, deck_targetIdx) == EscortCheckKinds.OK) ? true : false;
		}

		public bool Change_TempDeck(int deck_targetIdx, int ship_rid, ref DeckShips deckShip)
		{
			DeckShips deckShips = deckShip;
			if (ship_rid == -2)
			{
				deckShips.RemoveRange(1, deckShips.Count() - 1);
				return true;
			}
			int num = deckShips.Find(ship_rid);
			int num2 = deckShips[deck_targetIdx];
			deckShips[deck_targetIdx] = ship_rid;
			if (ship_rid == -1 || num == -1)
			{
				return true;
			}
			if (num2 != ship_rid)
			{
				deckShips[num] = num2;
			}
			else
			{
				deckShips[num] = -1;
			}
			return true;
		}

		public Api_Result<Mem_esccort_deck> Change(int deck_rid, DeckShips deckShip)
		{
			Api_Result<Mem_esccort_deck> api_Result = new Api_Result<Mem_esccort_deck>();
			Mem_esccort_deck deck = null;
			List<int> lastTempResult = null;
			deckShip.Clone(out lastTempResult);
			if (!Comm_UserDatas.Instance.User_EscortDeck.TryGetValue(deck_rid, out deck))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			List<int> out_ships = null;
			deck.Ship.Clone(out out_ships);
			Dictionary<int, Mem_ship> cloneShips = deck.Ship.getMemShip().ToDictionary((Mem_ship rid) => rid.Rid, (Mem_ship obj) => obj);
			deck.Ship.Clear();
			int count = lastTempResult.Count;
			for (int i = 0; i < count; i++)
			{
				deck.Ship[i] = lastTempResult[i];
				Comm_UserDatas.Instance.User_ship[deck.Ship[i]].BlingWaitToStop();
			}
			out_ships.ForEach(delegate(int ship_rid)
			{
				if (!lastTempResult.Contains(ship_rid) && Comm_UserDatas.Instance.Temp_escortship.Contains(ship_rid))
				{
					cloneShips[ship_rid].BlingWait(deck.Maparea_id, Mem_ship.BlingKind.WaitEscort);
				}
			});
			if (deck.Ship.Count() > 0 && deck.Disposition_status == DispositionStatus.NONE)
			{
				deck.GoArea(deck.Maparea_id);
			}
			else if (deck.Ship.Count() == 0)
			{
				deck.EscortStop();
			}
			else if (deck.Disposition_status == DispositionStatus.ARRIVAL && deck.GetBlingTurn() == 0 && deck.Ship.getMemShip().All((Mem_ship x) => x.GetBlingTurn() > 0))
			{
				deck.EscortStop();
				deck.GoArea(deck.Maparea_id);
			}
			api_Result.data = deck;
			return api_Result;
		}
	}
}
