using Server_Common;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_Hensei
	{
		public bool IsValidChange(int deck_rid, int deck_targetIdx, int ship_rid)
		{
			Mem_deck deck = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, out deck))
			{
				return false;
			}
			if (deck.MissionState != 0 || deck.IsActionEnd)
			{
				return false;
			}
			List<int> out_ships = new List<int>();
			if (ship_rid == -2)
			{
				if (deck.Ship.Count() == 0)
				{
					return false;
				}
				deck.Ship.Clone(out out_ships);
				out_ships.RemoveAt(0);
			}
			else if (ship_rid == -1)
			{
				out_ships.Add(deck.Ship[deck_targetIdx]);
			}
			else
			{
				out_ships.Add(ship_rid);
				out_ships.Add(deck.Ship[deck_targetIdx]);
				if (Comm_UserDatas.Instance.User_ndock.Values.Any((Mem_ndock x) => x.Ship_id == ship_rid && x.Area_id != deck.Area_id))
				{
					return false;
				}
			}
			if (Comm_UserDatas.Instance.User_EscortDeck.Values.Any((Mem_esccort_deck escort) => escort.Ship.Find(ship_rid) != -1))
			{
				return false;
			}
			if (ship_rid == -2 || (ship_rid == -1 && deck_targetIdx == 0 && deck.Rid == 1))
			{
				return (deck.Ship.Count() > 1) ? true : false;
			}
			if (ship_rid == -1)
			{
				if (deck.Area_id != 1 && deck.Ship.Count() == 1)
				{
					return false;
				}
				return true;
			}
			Mem_ship value = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, out value))
			{
				return false;
			}
			if (value.IsBlingShip())
			{
				return false;
			}
			if (value.BlingType == Mem_ship.BlingKind.WaitEscort)
			{
				return false;
			}
			if (value.BlingType == Mem_ship.BlingKind.WaitDeck && value.BlingWaitArea != deck.Area_id)
			{
				return false;
			}
			DeckShips out_ships2 = null;
			deck.Ship.Clone(out out_ships2);
			int[] array = deck.Search_ShipIdx(ship_rid);
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
			if (array[0] == deck_rid)
			{
				out_ships2[array[1]] = out_ships2[deck_targetIdx];
			}
			else if (array[0] != -1)
			{
				Mem_deck mem_deck = Comm_UserDatas.Instance.User_deck[array[0]];
				if (mem_deck.MissionState != 0 || mem_deck.IsActionEnd)
				{
					return false;
				}
				if (mem_deck.Area_id != deck.Area_id)
				{
					return false;
				}
				DeckShips out_ships3 = null;
				mem_deck.Ship.Clone(out out_ships3);
				out_ships3[array[1]] = out_ships2[deck_targetIdx];
				if (mem_deck.Rid == 1 && out_ships3.Count() == 0)
				{
					return false;
				}
				if (out_ships3.Count() == 0 && mem_deck.Area_id != 1)
				{
					return false;
				}
				if (!func(out_ships3))
				{
					return false;
				}
			}
			out_ships2[deck_targetIdx] = ship_rid;
			if (!func(out_ships2))
			{
				return false;
			}
			return (!out_ships2.Equals(deck.Ship)) ? true : false;
		}

		public Api_Result<Hashtable> Change(int deck_rid, int deck_targetIdx, int ship_rid)
		{
			Api_Result<Hashtable> api_Result = new Api_Result<Hashtable>();
			if (!IsValidChange(deck_rid, deck_targetIdx, ship_rid))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_deck value = null;
			List<int> out_ships = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			value.Ship.Clone(out out_ships);
			HashSet<int> inNdockShips = new HashSet<int>(from x in Comm_UserDatas.Instance.User_ndock.Values
				select x.Ship_id);
			Func<int, int, bool> func = delegate(int area_id, int target_ship)
			{
				if (area_id == 1)
				{
					return false;
				}
				if (!Comm_UserDatas.Instance.Temp_deckship.Contains(target_ship))
				{
					return false;
				}
				return (!inNdockShips.Contains(target_ship)) ? true : false;
			};
			if (ship_rid == -1)
			{
				value.Ship[deck_targetIdx] = ship_rid;
				Comm_UserDatas.Instance.Temp_deckship.Contains(ship_rid);
				if (func(value.Area_id, out_ships[deck_targetIdx]))
				{
					setBlingShip(out_ships[deck_targetIdx], value.Area_id, value.Rid);
				}
				new QuestHensei().ExecuteCheck();
				return api_Result;
			}
			api_Result.state = Api_Result_State.Success;
			if (ship_rid == -2)
			{
				if (value.Ship.Count() < 2)
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				if (value.Area_id != 1)
				{
					int num = out_ships.Count() - 1;
					for (int i = 1; i <= num; i++)
					{
						if (func(value.Area_id, out_ships[i]))
						{
							setBlingShip(out_ships[i], value.Area_id, value.Rid);
						}
					}
				}
				int num2 = value.Ship.Count();
				value.Ship.RemoveRange(1, value.Ship.Count() - 1);
				api_Result.data = new Hashtable();
				api_Result.data["change_count"] = num2 - value.Ship.Count();
				return api_Result;
			}
			int[] array = value.Search_ShipIdx(Comm_UserDatas.Instance.User_deck, ship_rid);
			Mem_deck value2 = null;
			List<int> out_ships2 = null;
			if (array[0] > 0)
			{
				if (!Comm_UserDatas.Instance.User_deck.TryGetValue(array[0], out value2))
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				value2.Ship.Clone(out out_ships2);
				value2.Ship[array[1]] = value.Ship[deck_targetIdx];
			}
			else if (!Comm_UserDatas.Instance.Temp_deckship.Contains(ship_rid) && value.Ship[deck_targetIdx] != -1)
			{
				if (func(value.Area_id, value.Ship[deck_targetIdx]))
				{
					setBlingShip(out_ships[deck_targetIdx], value.Area_id, value.Rid);
				}
			}
			else if (value.Ship[deck_targetIdx] != -1 && Comm_UserDatas.Instance.User_ship[ship_rid].BlingWaitArea == value.Area_id && func(value.Area_id, value.Ship[deck_targetIdx]))
			{
				setBlingShip(out_ships[deck_targetIdx], value.Area_id, value.Rid);
			}
			Comm_UserDatas.Instance.User_ship[ship_rid].BlingWaitToStop();
			value.Ship[deck_targetIdx] = ship_rid;
			new QuestHensei().ExecuteCheck();
			return api_Result;
		}

		public Api_Result<Mem_ship> Lock(int ship_rid)
		{
			Api_Result<Mem_ship> api_Result = new Api_Result<Mem_ship>();
			Mem_ship value = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			value.ChangeLockSts();
			api_Result.data = value;
			return api_Result;
		}

		private bool setBlingShip(int ship_rid, int area_id, int deck_rid)
		{
			Mem_ship value = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, out value))
			{
				return false;
			}
			value.BlingWait(area_id, Mem_ship.BlingKind.WaitDeck);
			return true;
		}
	}
}
