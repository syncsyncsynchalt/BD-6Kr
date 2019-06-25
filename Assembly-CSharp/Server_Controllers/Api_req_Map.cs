using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers.BattleLogic;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers
{
	public class Api_req_Map : IRebellionPointOperator
	{
		public class MapRequireUserShipInfo
		{
			public Mem_deck Mem_deck;

			public List<Mem_ship> Mem_ship;

			public List<List<Mst_slotitem>> Mst_slotitems;

			public List<int> Stype;

			public MapRequireUserShipInfo(int deckRid)
			{
				Comm_UserDatas.Instance.User_deck.TryGetValue(deckRid, out Mem_deck);
			}

			public bool SetShips()
			{
				if (Mem_deck == null)
				{
					return false;
				}
				if (Mem_ship == null)
				{
					Mem_ship = new List<Mem_ship>();
					Stype = new List<int>();
					Mst_slotitems = new List<List<Mst_slotitem>>();
				}
				else
				{
					ClearCollection();
				}
				List<Mem_ship> memShip = Mem_deck.Ship.getMemShip();
				if (memShip.Count == 0)
				{
					return false;
				}
				memShip.ForEach(delegate(Mem_ship addShip)
				{
					Mem_ship.Add(addShip);
					Stype.Add(addShip.Stype);
					Mst_slotitems.Add(addShip.GetMstSlotItems());
				});
				return true;
			}

			public Mem_ship GetFlagShip()
			{
				return Mem_ship[0];
			}

			public void ClearCollection()
			{
				Mem_ship.Clear();
				Mst_slotitems.Clear();
				Stype.Clear();
			}
		}

		private Mst_mapcell2 _mst_cell;

		private Mst_mapcell2 _next_mst_cell;

		private int _now_Cell;

		private Mem_rebellion_point _mem_rebellion;

		private Mst_maparea _mst_maparea;

		private Dictionary<int, Dictionary<int, Mst_mapenemy2>> _map_enemy;

		private ILookup<int, Mst_mapenemylevel> _map_enemylevel;

		private MapRequireUserShipInfo userShipInfo1;

		private MapRequireUserShipInfo userShipInfo2;

		private List<Mem_deck> _support_decks;

		private bool isLeadingDeck;

		private Mem_mapcomp mapComp;

		private Mem_mapclear mapClear;

		private MapBranchResult mapBranchLogic;

		private Dictionary<int, List<int>> mstRoute;

		private Dictionary<int, List<Mst_mapincentive>> mstMapIncentive;

		private Dictionary<int, List<Mst_mapcellincentive>> mstMapCellIncentive;

		private Dictionary<int, int> slotExpChangeValues;

		private int _enemy_id;

		private Dictionary<int, User_MapCellInfo> _user_mapcell;

		private IEnumerable<XElement> _mst_stype_group;

		private ILookup<int, int> _mst_SupportData;

		private bool isRebbelion;

		private List<MapItemGetFmt> _airReconnaissanceItems;

		private int _mapBattleCellPassCount;

		public List<Mem_deck> Support_decks => _support_decks;

		public int Enemy_Id => _enemy_id;

		public Dictionary<int, User_MapCellInfo> User_mapcell
		{
			get
			{
				return _user_mapcell;
			}
			private set
			{
				_user_mapcell = value;
			}
		}

		public ILookup<int, int> Mst_SupportData
		{
			get
			{
				return _mst_SupportData;
			}
			private set
			{
				_mst_SupportData = value;
			}
		}

		public bool IsRebbelion
		{
			get
			{
				return isRebbelion;
			}
			private set
			{
				isRebbelion = value;
			}
		}

		public List<MapItemGetFmt> AirReconnaissanceItems
		{
			get
			{
				return _airReconnaissanceItems;
			}
			private set
			{
				_airReconnaissanceItems = value;
			}
		}

		public int MapBattleCellPassCount
		{
			get
			{
				return _mapBattleCellPassCount;
			}
			private set
			{
				_mapBattleCellPassCount = value;
			}
		}

		public Api_req_Map()
		{
			_support_decks = new List<Mem_deck>();
			isLeadingDeck = false;
			MapBattleCellPassCount = 0;
		}

		void IRebellionPointOperator.AddRebellionPoint(int area_id, int addNum)
		{
			throw new NotImplementedException();
		}

		void IRebellionPointOperator.SubRebellionPoint(int area_id, int subNum)
		{
			_mem_rebellion.SubPoint(this, subNum);
		}

		public void GetSortieDeckInfo(MapBranchResult instance, out List<Mem_ship> ships, out Dictionary<int, List<Mst_slotitem>> slotItems)
		{
			ships = null;
			slotItems = null;
			if (instance == null)
			{
				return;
			}
			MapRequireUserShipInfo activeShipInfo = getActiveShipInfo();
			ships = new List<Mem_ship>();
			slotItems = new Dictionary<int, List<Mst_slotitem>>();
			for (int i = 0; i < activeShipInfo.Mem_ship.Count; i++)
			{
				if (activeShipInfo.Mem_ship[i].IsFight())
				{
					ships.Add(activeShipInfo.Mem_ship[i]);
					slotItems.Add(activeShipInfo.Mem_ship[i].Rid, activeShipInfo.Mst_slotitems[i].ToList());
				}
			}
		}

		public Api_Result<Map_ResultFmt> Start(int maparea_id, int map_no, int deck_id)
		{
			mapBranchLogic = new MapBranchResult(this);
			IsRebbelion = false;
			Api_Result<Map_ResultFmt> api_Result = new Api_Result<Map_ResultFmt>();
			if (!initMapData(maparea_id, map_no))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			((IRebellionPointOperator)this).SubRebellionPoint(maparea_id, getRebellionPointSubNum(map_no));
			userShipInfo1 = new MapRequireUserShipInfo(deck_id);
			if (!userShipInfo1.SetShips())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			_now_Cell = 0;
			api_Result.data = getMapResult(-1);
			if (api_Result.data == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			List<Mem_deck> list = (from deck in Comm_UserDatas.Instance.User_deck.Values
				where deck.SupportKind == Mem_deck.SupportKinds.WAIT
				let mst_misson = Mst_DataManager.Instance.Mst_mission[deck.Mission_id]
				where mst_misson.Maparea_id == maparea_id
				select deck).ToList();
			if (list.Count > 0)
			{
				if (!makeSupportData())
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				_support_decks = list;
			}
			list.ForEach(delegate(Mem_deck x)
			{
				x.ChangeSupported();
			});
			new QuestSortie(_mst_cell.Maparea_id, _mst_cell.Mapinfo_no, userShipInfo1.Mem_deck.Rid, userShipInfo1.Mem_ship).ExecuteCheck();
			return api_Result;
		}

		public Api_Result<Map_ResultFmt> StartResisted(int maparea_id, int firstDeck, int secondDeck)
		{
			mapBranchLogic = new MapBranchResult(this);
			IsRebbelion = true;
			Api_Result<Map_ResultFmt> api_Result = new Api_Result<Map_ResultFmt>();
			initMapData(maparea_id, 7);
			userShipInfo1 = new MapRequireUserShipInfo(firstDeck);
			if (!userShipInfo1.SetShips())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (secondDeck > 0)
			{
				userShipInfo2 = new MapRequireUserShipInfo(secondDeck);
				if (!userShipInfo2.SetShips())
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
			}
			List<Mem_deck> list = (from deck in Comm_UserDatas.Instance.User_deck.Values
				where deck.SupportKind == Mem_deck.SupportKinds.WAIT
				where deck.Area_id == maparea_id
				select deck).ToList();
			if (list.Count > 0)
			{
				List<Mst_mission2> supportResistedData = Mst_DataManager.Instance.GetSupportResistedData(maparea_id);
				Mst_DataManager.Instance.Mst_mission.Add(supportResistedData[0].Id, supportResistedData[0]);
				Mst_DataManager.Instance.Mst_mission.Add(supportResistedData[1].Id, supportResistedData[1]);
				if (!makeSupportData())
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				_support_decks = list;
			}
			list.ForEach(delegate(Mem_deck x)
			{
				x.ChangeSupported();
			});
			_now_Cell = 0;
			api_Result.data = getMapResult(-1);
			if (api_Result.data == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			return api_Result;
		}

		public bool ChangeLeadingDeck()
		{
			if (userShipInfo2 == null)
			{
				return false;
			}
			isLeadingDeck = true;
			return true;
		}

		public Api_Result<Map_ResultFmt> Next(ShipRecoveryType recovery_type)
		{
			Api_Result<Map_ResultFmt> api_Result = initNext(recovery_type);
			if (api_Result.state == Api_Result_State.Parameter_Error)
			{
				return api_Result;
			}
			api_Result.data = getMapResult(-1);
			if (api_Result.data == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			return api_Result;
		}

		public Api_Result<Map_ResultFmt> Next(ShipRecoveryType recovery_type, int selectCellNo)
		{
			Api_Result<Map_ResultFmt> api_Result = initNext(recovery_type);
			if (api_Result.state == Api_Result_State.Parameter_Error)
			{
				return api_Result;
			}
			api_Result.data = getMapResult(selectCellNo);
			if (api_Result.data == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			return api_Result;
		}

		private Api_Result<Map_ResultFmt> initNext(ShipRecoveryType recovery_type)
		{
			Api_Result<Map_ResultFmt> api_Result = new Api_Result<Map_ResultFmt>();
			MapRequireUserShipInfo activeShipInfo = getActiveShipInfo();
			if (!activeShipInfo.SetShips())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (!useFlagShipRecover(activeShipInfo, recovery_type))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			return api_Result;
		}

		private bool useFlagShipRecover(MapRequireUserShipInfo shipInfo, ShipRecoveryType recovery_type)
		{
			if (recovery_type == ShipRecoveryType.None)
			{
				return true;
			}
			Mem_ship flagShip = shipInfo.GetFlagShip();
			if (flagShip == null)
			{
				return false;
			}
			int[] array = new int[2]
			{
				-1,
				(int)recovery_type
			};
			array[0] = flagShip.GetMstSlotItems().FindIndex((Mst_slotitem x) => (x.Id == (int)recovery_type) ? true : false);
			Mst_slotitem mstSlotItemToExSlot = flagShip.GetMstSlotItemToExSlot();
			if (mstSlotItemToExSlot != null)
			{
				int id = mstSlotItemToExSlot.Id;
				if (id == (int)recovery_type)
				{
					array[0] = int.MaxValue;
					array[1] = id;
				}
			}
			if (array[0] == -1)
			{
				return false;
			}
			if (array[0] != int.MaxValue)
			{
				shipInfo.Mst_slotitems[0].RemoveAt(array[0]);
			}
			flagShip.UseRecoveryItem(array, flagShipRecovery: true);
			return true;
		}

		public void GetSortieShipDatas(out List<Mem_ship> activeShips, out List<Mem_ship> inactiveShips)
		{
			MapRequireUserShipInfo activeShipInfo = getActiveShipInfo();
			MapRequireUserShipInfo inActiveShipInfo = getInActiveShipInfo();
			activeShips = activeShipInfo.Mem_ship.ToList();
			inactiveShips = ((inActiveShipInfo != null) ? inActiveShipInfo.Mem_ship.ToList() : new List<Mem_ship>());
		}

		public TurnState SortieEnd()
		{
			mapBranchLogic = null;
			UpdateMapcomp(_now_Cell);
			if (!isRebbelion)
			{
				foreach (Mem_deck support_deck in _support_decks)
				{
					support_deck.MissionEnforceEnd();
					support_deck.ActionEnd();
				}
				_support_decks.Clear();
			}
			User_mapcell.Clear();
			_map_enemy.Clear();
			_map_enemylevel = null;
			_mst_stype_group.Remove();
			userShipInfo1.Mem_ship.ForEach(delegate(Mem_ship x)
			{
				x.SetSortieEndCond(this);
				if (x.Escape_sts)
				{
					x.ChangeEscapeState();
				}
			});
			userShipInfo1.Mem_deck.ActionEnd();
			userShipInfo1.ClearCollection();
			Mst_SupportData = null;
			Mst_DataManager.Instance.Mst_mapcell.Clear();
			Mst_DataManager.Instance.Mst_mapenemy.Clear();
			Mst_DataManager.Instance.Mst_shipget.Remove();
			if (userShipInfo2 != null)
			{
				userShipInfo2.Mem_ship.ForEach(delegate(Mem_ship x)
				{
					x.SetSortieEndCond(this);
					if (x.Escape_sts)
					{
						x.ChangeEscapeState();
					}
				});
				userShipInfo2.Mem_deck.ActionEnd();
				userShipInfo2.ClearCollection();
			}
			mstMapIncentive.Clear();
			mstRoute.Clear();
			if (slotExpChangeValues != null)
			{
				foreach (KeyValuePair<int, int> slotExpChangeValue in slotExpChangeValues)
				{
					Mem_slotitem value = null;
					if (Comm_UserDatas.Instance.User_slot.TryGetValue(slotExpChangeValue.Key, out value))
					{
						value.ChangeExperience(slotExpChangeValue.Value);
					}
				}
				slotExpChangeValues.Clear();
				slotExpChangeValues = null;
			}
			if (AirReconnaissanceItems != null)
			{
				AirReconnaissanceItems.Clear();
			}
			return TurnState.OWN_END;
		}

		public bool RebellionEnd()
		{
			bool result = false;
			if (!IsRebbelion)
			{
				return result;
			}
			List<int> list = new List<int>();
			foreach (Mem_deck support_deck in _support_decks)
			{
				support_deck.MissionEnforceEnd();
				support_deck.ActionEnd();
				list.Add(support_deck.Rid);
			}
			_support_decks.Clear();
			if (_mem_rebellion.State == RebellionState.Invation)
			{
				list.Add(userShipInfo1.Mem_deck.Rid);
				if (userShipInfo2 != null)
				{
					list.Add(userShipInfo2.Mem_deck.Rid);
				}
				new RebellionUtils().LostArea(_mem_rebellion.Rid, list);
				result = true;
			}
			Mst_DataManager.Instance.Mst_mission.Remove(100000);
			Mst_DataManager.Instance.Mst_mission.Remove(100001);
			return result;
		}

		public List<int> GetEnemyShipIds()
		{
			Mst_mapenemy2 value = null;
			if (!Mst_DataManager.Instance.Mst_mapenemy.TryGetValue(_enemy_id, out value))
			{
				return new List<int>();
			}
			List<int> list = new List<int>();
			list.Add(value.E1_id);
			list.Add(value.E2_id);
			list.Add(value.E3_id);
			list.Add(value.E4_id);
			list.Add(value.E5_id);
			list.Add(value.E6_id);
			return list;
		}

		public string GetEnemyShipNames()
		{
			Mst_mapenemy2 value = null;
			if (!Mst_DataManager.Instance.Mst_mapenemy.TryGetValue(_enemy_id, out value))
			{
				return string.Empty;
			}
			return value.Deck_name;
		}

		public void GetBattleShipData(out BattleBaseData f_Instance, out BattleBaseData e_Instance)
		{
			f_Instance = null;
			e_Instance = null;
			if (Mst_DataManager.Instance.Mst_mapenemy.ContainsKey(_enemy_id))
			{
				e_Instance = new BattleBaseData(_enemy_id);
			}
			MapRequireUserShipInfo activeShipInfo = getActiveShipInfo();
			f_Instance = new BattleBaseData(activeShipInfo.Mem_deck, activeShipInfo.Mem_ship, activeShipInfo.Stype, activeShipInfo.Mst_slotitems);
		}

		public void SetSlotExpChangeValues(Api_req_SortieBattle battleInstance, Dictionary<int, int> changeExpDatas)
		{
			if (slotExpChangeValues == null)
			{
				slotExpChangeValues = changeExpDatas;
			}
			else
			{
				foreach (KeyValuePair<int, int> changeExpData in changeExpDatas)
				{
					if (slotExpChangeValues.ContainsKey(changeExpData.Key))
					{
						slotExpChangeValues[changeExpData.Key] = slotExpChangeValues[changeExpData.Key] + changeExpData.Value;
					}
					else
					{
						slotExpChangeValues.Add(changeExpData.Key, changeExpData.Value);
					}
				}
			}
		}

		public Mem_mapclear GetMapClearState()
		{
			return mapClear;
		}

		public Mst_mapcell2 GetPrevCell()
		{
			return _mst_cell;
		}

		public Mst_mapcell2 GetNowCell()
		{
			return _next_mst_cell;
		}

		private Map_ResultFmt getMapResult(int selectCellNo)
		{
			User_MapCellInfo value = null;
			if (!_user_mapcell.TryGetValue(_now_Cell, out value))
			{
				return null;
			}
			_mst_cell = value.Mst_mapcell;
			if (!User_mapcell[_now_Cell].Passed)
			{
				UpdateMapcomp(_now_Cell);
			}
			if (_mst_cell.Event_1 == enumMapEventType.War_Normal || _mst_cell.Event_1 == enumMapEventType.War_Boss)
			{
				MapBattleCellPassCount++;
			}
			if (!_mst_cell.IsNext() && selectCellNo == -1)
			{
				return null;
			}
			MapCommentKind comment_kind = MapCommentKind.None;
			MapProductionKind production_kind = MapProductionKind.None;
			int num = 0;
			if (selectCellNo != -1)
			{
				if (!User_mapcell.ContainsKey(selectCellNo))
				{
					return null;
				}
				List<int> value2 = null;
				if (!mstRoute.TryGetValue(_mst_cell.No, out value2))
				{
				}
				_now_Cell = selectCellNo;
			}
			else if (_mst_cell.Next_no_2 > 0)
			{
				num = (int)Utils.GetRandDouble(1.0, 4.0, 1.0, 1);
				if (!mapBranchLogic.getNextCellNo(out _now_Cell, out comment_kind, out production_kind) && !getNextCellNo(out _now_Cell))
				{
					return null;
				}
			}
			else
			{
				_now_Cell = _mst_cell.Next_no_1;
			}
			CompassType rashin_id = (CompassType)num;
			value = null;
			if (!_user_mapcell.TryGetValue(_now_Cell, out value))
			{
				return null;
			}
			_next_mst_cell = value.Mst_mapcell;
			Map_ResultFmt map_ResultFmt = new Map_ResultFmt();
			MapItemGetFmt item = null;
			MapHappningFmt happning = null;
			List<MapItemGetFmt> clearItem = null;
			int num2 = 0;
			List<int> second = null;
			AirReconnaissanceFmt airReconnaissanceFmt = null;
			int spoint = 0;
			int total_turn = Comm_UserDatas.Instance.User_turn.Total_turn;
			if (_next_mst_cell.Event_1 == enumMapEventType.ItemGet)
			{
				int itemNo = 0;
				int itemCount = 0;
				getItemGetCellReward(out itemNo, out itemCount);
				item = mapItemGet(itemNo, itemCount);
			}
			else if (_next_mst_cell.Event_1 == enumMapEventType.PortBackEo)
			{
				bool flag = false;
				if (mapClear != null)
				{
					flag = mapClear.Cleared;
				}
				second = new List<int>(Utils.GetActiveMap().Keys);
				num2 = setPortBackEoArrivalData(_next_mst_cell);
				setPortBackEoCellReward(num2, out item, out clearItem);
				if (mapClear.Cleared && !flag)
				{
					spoint = Mst_DataManager.Instance.Mst_mapinfo[_next_mst_cell.Map_no].Clear_spoint;
				}
			}
			else if (_next_mst_cell.Event_1 == enumMapEventType.Uzushio)
			{
				happning = mapHappning();
			}
			else if (_next_mst_cell.Event_1 == enumMapEventType.AirReconnaissance)
			{
				if (AirReconnaissanceItems == null)
				{
					AirReconnaissanceItems = new List<MapItemGetFmt>();
				}
				KeyValuePair<MapAirReconnaissanceKind, double> airSearchParam = getAirSearchParam();
				MissionResultKinds airSearchResult = getAirSearchResult(_next_mst_cell.Event_point_1, airSearchParam);
				airReconnaissanceFmt = new AirReconnaissanceFmt(airSearchParam.Key, airSearchResult);
				setMapCellReward(airReconnaissanceFmt, out item);
			}
			else if (_next_mst_cell.Event_1 == enumMapEventType.War_Normal || _next_mst_cell.Event_1 == enumMapEventType.War_Boss)
			{
				_enemy_id = selectEnemy();
			}
			List<int> value3 = null;
			mstRoute.TryGetValue(_next_mst_cell.No, out value3);
			List<int> list = null;
			if (num2 == 2)
			{
				List<int> reOpenMap = null;
				new RebellionUtils().MapReOpen(mapClear, out reOpenMap);
				List<int> first = new List<int>(Utils.GetActiveMap().Keys);
				list = first.Except(second).ToList();
				int mapClearNum = Mem_history.GetMapClearNum(_next_mst_cell.Map_no);
				if (mapClearNum <= 3)
				{
					Mem_history mem_history = new Mem_history();
					mem_history.SetMapClear(total_turn, _next_mst_cell.Map_no, mapClearNum, userShipInfo1.Mem_ship[0].Ship_id);
					Comm_UserDatas.Instance.Add_History(mem_history);
				}
				foreach (int item2 in list)
				{
					if (Mem_history.IsFirstOpenArea(item2))
					{
						Mem_history mem_history2 = new Mem_history();
						mem_history2.SetAreaOpen(total_turn, item2);
						Comm_UserDatas.Instance.Add_History(mem_history2);
					}
				}
			}
			MapRequireUserShipInfo activeShipInfo = getActiveShipInfo();
			if (IsOffshoreSupply(GetPrevCell(), GetNowCell(), activeShipInfo.Mem_ship))
			{
				executeOffshoreSupply(activeShipInfo, out map_ResultFmt.MapSupply);
			}
			map_ResultFmt.SetMember(rashin_id, _next_mst_cell, item, clearItem, happning, comment_kind, production_kind, airReconnaissanceFmt, null, value3, list, spoint);
			List<MapItemGetFmt> list2 = (clearItem != null) ? new List<MapItemGetFmt>(clearItem) : new List<MapItemGetFmt>();
			if (item != null)
			{
				list2.Add(item);
			}
			if (_next_mst_cell.Event_1 == enumMapEventType.AirReconnaissance)
			{
				AirReconnaissanceItems.AddRange(list2);
			}
			else
			{
				updateMapitemGetData(list2);
			}
			return map_ResultFmt;
		}

		private int setPortBackEoArrivalData(Mst_mapcell2 nowCell)
		{
			if (mapClear == null)
			{
				Mem_mapclear mem_mapclear = new Mem_mapclear(nowCell.Map_no, nowCell.Maparea_id, nowCell.Mapinfo_no, MapClearState.Cleard);
				mem_mapclear.Insert();
				mapClear = mem_mapclear;
				return 2;
			}
			if (mapClear.State != 0)
			{
				mapClear.StateChange(MapClearState.Cleard);
				return 2;
			}
			if (mapClear.State == MapClearState.Cleard)
			{
				return 1;
			}
			return 0;
		}

		private void setPortBackEoCellReward(int clearState, out MapItemGetFmt item, out List<MapItemGetFmt> clearItem)
		{
			item = null;
			clearItem = null;
			if (mstMapIncentive.ContainsKey(0))
			{
				new List<Mst_mapincentive>();
				List<Mst_mapincentive> list = mstMapIncentive[0];
				List<double> rateValues = (from x in list
					select x.Choose_rate).ToList();
				int randomRateIndex = Utils.GetRandomRateIndex(rateValues);
				Mst_mapincentive mst_mapincentive = list[randomRateIndex];
				item = new MapItemGetFmt();
				item.Id = mst_mapincentive.Get_id;
				item.Category = mst_mapincentive.GetCategory;
				item.GetCount = mst_mapincentive.Get_count;
			}
		}

		private KeyValuePair<MapAirReconnaissanceKind, double> getAirSearchParam()
		{
			MapRequireUserShipInfo activeShipInfo = getActiveShipInfo();
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(41);
			HashSet<int> hashSet2 = hashSet;
			hashSet = new HashSet<int>();
			hashSet.Add(10);
			hashSet.Add(11);
			HashSet<int> hashSet3 = hashSet;
			int num = 0;
			int num2 = 0;
			double num3 = 0.0;
			for (int i = 0; i < activeShipInfo.Mem_ship.Count; i++)
			{
				List<int> onslot = activeShipInfo.Mem_ship[i].Onslot;
				List<Mst_slotitem> list = activeShipInfo.Mst_slotitems[i];
				for (int j = 0; j < list.Count; j++)
				{
					Mst_slotitem mst_slotitem = list[j];
					if (hashSet3.Contains(mst_slotitem.Api_mapbattle_type3))
					{
						num2++;
						num3 += (double)mst_slotitem.Saku * Math.Sqrt(Math.Sqrt(onslot[j]));
					}
					else if (hashSet2.Contains(mst_slotitem.Api_mapbattle_type3))
					{
						num++;
						num3 += (double)mst_slotitem.Saku * Math.Sqrt(onslot[j]);
					}
				}
			}
			MapAirReconnaissanceKind key = MapAirReconnaissanceKind.Impossible;
			if (num > 0)
			{
				key = MapAirReconnaissanceKind.LargePlane;
			}
			else if (num2 > 0)
			{
				key = MapAirReconnaissanceKind.WarterPlane;
			}
			return new KeyValuePair<MapAirReconnaissanceKind, double>(key, num3);
		}

		private MissionResultKinds getAirSearchResult(double success_keisu, KeyValuePair<MapAirReconnaissanceKind, double> param)
		{
			if (param.Key == MapAirReconnaissanceKind.Impossible)
			{
				return MissionResultKinds.FAILE;
			}
			double randDouble = Utils.GetRandDouble(0.0, 0.6, 0.1, 1);
			double num = success_keisu * (1.6 + randDouble);
			if (param.Value >= num)
			{
				return MissionResultKinds.GREAT;
			}
			if (param.Value >= success_keisu)
			{
				return MissionResultKinds.SUCCESS;
			}
			return MissionResultKinds.FAILE;
		}

		private void setMapCellReward(AirReconnaissanceFmt air, out MapItemGetFmt itemFmt)
		{
			itemFmt = null;
			if (air.SearchResult != 0)
			{
				List<Mst_mapcellincentive> value = null;
				mstMapCellIncentive.TryGetValue(_next_mst_cell.No, out value);
				if (value != null)
				{
					int successFlag = (air.SearchResult != MissionResultKinds.SUCCESS) ? 1 : 0;
					List<Mst_mapcellincentive> list = (from x in value
						where x.Success_level == successFlag
						select x).ToList();
					List<double> rateValues = (from y in list
						select y.Choose_rate).ToList();
					int randomRateIndex = Utils.GetRandomRateIndex(rateValues);
					Mst_mapcellincentive mst_mapcellincentive = list[randomRateIndex];
					itemFmt = new MapItemGetFmt();
					itemFmt.Id = mst_mapcellincentive.Get_id;
					itemFmt.Category = mst_mapcellincentive.GetCategory;
					itemFmt.GetCount = mst_mapcellincentive.Get_count;
				}
			}
		}

		private void getItemGetCellReward(out int itemNo, out int itemCount)
		{
			itemNo = _next_mst_cell.Item_no;
			itemCount = _next_mst_cell.Item_count;
			MapRequireUserShipInfo activeShipInfo = getActiveShipInfo();
			List<Mst_mapcellincentive> value2 = null;
			mstMapCellIncentive.TryGetValue(_next_mst_cell.No, out value2);
			if (value2 == null)
			{
				if (itemNo < 5)
				{
					double[] source = new double[3]
					{
						0.0,
						0.5,
						1.0
					};
					var anon = (from value in source
						select new
						{
							value
						} into x
						orderby Guid.NewGuid()
						select x).First();
					double num = (double)itemCount * anon.value;
					itemCount += (int)num;
				}
				return;
			}
			Mst_mapcellincentive mst_mapcellincentive = value2[0];
			if (mst_mapcellincentive.Event_id == 2)
			{
				Dictionary<int, int> countDict = mst_mapcellincentive.Req_items.Keys.ToDictionary((int item_id) => item_id, (int item_count) => 0);
				activeShipInfo.Mst_slotitems.ForEach(delegate(List<Mst_slotitem> slotList)
				{
					slotList.ForEach(delegate(Mst_slotitem mst_slot)
					{
						if (countDict.ContainsKey(mst_slot.Id))
						{
							Dictionary<int, int> dictionary;
							Dictionary<int, int> dictionary2 = dictionary = countDict;
							int id;
							int key = id = mst_slot.Id;
							id = dictionary[id];
							dictionary2[key] = id + 1;
						}
					});
				});
				int num2 = 0;
				foreach (KeyValuePair<int, int> item in countDict)
				{
					num2 += mst_mapcellincentive.Req_items[item.Key] * item.Value;
				}
				int num3 = itemCount + num2;
				if (num3 > mst_mapcellincentive.Get_count)
				{
					num3 = mst_mapcellincentive.Get_count;
				}
				itemCount = num3;
			}
		}

		private MapItemGetFmt mapItemGet(int itemNo, int itemCount)
		{
			MapItemGetFmt mapItemGetFmt = new MapItemGetFmt();
			if (itemNo >= 1 && itemNo <= 8)
			{
				mapItemGetFmt.Category = MapItemGetFmt.enumCategory.Material;
				mapItemGetFmt.Id = itemNo;
				mapItemGetFmt.GetCount = itemCount;
			}
			else if (itemNo == 9 || itemNo == 10 || itemNo == 11)
			{
				mapItemGetFmt.Category = MapItemGetFmt.enumCategory.UseItem;
				if (_next_mst_cell.Item_no == 9)
				{
					mapItemGetFmt.Id = 10;
				}
				else if (_next_mst_cell.Item_no == 10)
				{
					mapItemGetFmt.Id = 11;
				}
				else if (_next_mst_cell.Item_no == 11)
				{
					mapItemGetFmt.Id = 12;
				}
				mapItemGetFmt.GetCount = itemCount;
			}
			return mapItemGetFmt;
		}

		public void updateMapitemGetData(List<MapItemGetFmt> itemFmt)
		{
			foreach (MapItemGetFmt item in itemFmt)
			{
				if (item.Category == MapItemGetFmt.enumCategory.Furniture)
				{
					Comm_UserDatas.Instance.Add_Furniture(item.Id);
				}
				else if (item.Category == MapItemGetFmt.enumCategory.Material)
				{
					enumMaterialCategory id = (enumMaterialCategory)item.Id;
					Comm_UserDatas.Instance.User_material[id].Add_Material(item.GetCount);
				}
				else if (item.Category == MapItemGetFmt.enumCategory.Ship)
				{
					List<int> ship_ids = Enumerable.Repeat(item.Id, item.GetCount).ToList();
					Comm_UserDatas.Instance.Add_Ship(ship_ids);
				}
				else if (item.Category == MapItemGetFmt.enumCategory.Slotitem)
				{
					List<int> slot_ids = Enumerable.Repeat(item.Id, item.GetCount).ToList();
					Comm_UserDatas.Instance.Add_Slot(slot_ids);
				}
				else if (item.Category == MapItemGetFmt.enumCategory.UseItem)
				{
					Comm_UserDatas.Instance.Add_Useitem(item.Id, item.GetCount);
				}
			}
		}

		private MapHappningFmt mapHappning()
		{
			int dentanShipCnt = 0;
			MapRequireUserShipInfo activeShipInfo = getActiveShipInfo();
			activeShipInfo.Mst_slotitems.ForEach(delegate(List<Mst_slotitem> x)
			{
				if (x.Exists((Mst_slotitem y) => y.Api_mapbattle_type3 == 12 || y.Api_mapbattle_type3 == 13))
				{
					dentanShipCnt++;
				}
			});
			double rate = 1.0;
			if (dentanShipCnt == 1)
			{
				rate = 0.75;
			}
			else if (dentanShipCnt == 2)
			{
				rate = 0.6;
			}
			else if (dentanShipCnt >= 3)
			{
				rate = 0.5;
			}
			List<int> subValues = new List<int>();
			activeShipInfo.Mem_ship.ForEach(delegate(Mem_ship x)
			{
				int num = 0;
				if (_next_mst_cell.Item_no == 1)
				{
					num = getHappenCellSubValue(rate, x.Fuel);
					int fuel = (x.Fuel - num >= 0) ? (x.Fuel - num) : 0;
					x.Set_ChargeData(x.Bull, fuel, x.Onslot);
				}
				else if (_next_mst_cell.Item_no == 2)
				{
					num = getHappenCellSubValue(rate, x.Bull);
					int bull = (x.Bull - num >= 0) ? (x.Bull - num) : 0;
					x.Set_ChargeData(bull, x.Fuel, x.Onslot);
				}
				subValues.Add(num);
			});
			MapHappningFmt mapHappningFmt = new MapHappningFmt();
			mapHappningFmt.Id = _next_mst_cell.Item_no;
			mapHappningFmt.Count = subValues.Max();
			mapHappningFmt.Dentan = ((dentanShipCnt != 0) ? true : false);
			return mapHappningFmt;
		}

		private int getHappenCellSubValue(double sub_rate, int now_num)
		{
			int num = (int)((double)now_num * 0.4 * sub_rate);
			if (num > _next_mst_cell.Item_count)
			{
				num = _next_mst_cell.Item_count;
			}
			return num;
		}

		private int selectEnemy()
		{
			int table_no = _next_mst_cell.Table_no1;
			enumMapEventType event_ = _next_mst_cell.Event_1;
			Mst_mapenemylevel mst_mapenemylevel = SelectEnemy(_map_enemylevel[table_no], Comm_UserDatas.Instance.User_basic.Difficult, Comm_UserDatas.Instance.User_turn.Total_turn);
			int deck_id = mst_mapenemylevel.Deck_id;
			return _map_enemy[table_no][deck_id].Id;
		}

		private Mst_mapenemylevel SelectEnemy(IEnumerable<Mst_mapenemylevel> enemyItems, DifficultKind difficulty, int now_turn)
		{
			List<Mst_mapenemylevel> list = (from e in enemyItems
				where e.Difficulty == difficulty
				select e).ToList();
			if (list.Count == 0)
			{
				return null;
			}
			bool flag = list.Any((Mst_mapenemylevel x) => x.Turns > 0);
			list.Sort((Mst_mapenemylevel a, Mst_mapenemylevel b) => (a.Turns != b.Turns) ? (b.Turns - a.Turns) : (b.Choose_rate - a.Choose_rate));
			int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
			for (int i = 0; i < list.Count; i++)
			{
				Mst_mapenemylevel mst_mapenemylevel = list[i];
				if (mst_mapenemylevel.Turns > now_turn)
				{
					continue;
				}
				if (flag)
				{
					if (mst_mapenemylevel.Choose_rate == -1)
					{
						return mst_mapenemylevel;
					}
				}
				else if (mst_mapenemylevel.Choose_rate == -1)
				{
					return mst_mapenemylevel;
				}
				num -= mst_mapenemylevel.Choose_rate;
				if (num <= 0)
				{
					return mst_mapenemylevel;
				}
			}
			return null;
		}

		private bool getNextCellNo(out int cellNo)
		{
			string[] useRateData = getUseRateData();
			List<int> list = new List<int>();
			int num = 0;
			string[] array = useRateData;
			foreach (string s in array)
			{
				list.Add(num);
				num += int.Parse(s);
			}
			list.Reverse();
			Random random = new Random();
			int num2 = random.Next(100);
			int index = 0;
			int num3 = list.Count();
			for (int j = 0; j < num3; j++)
			{
				if (list[j] <= num2)
				{
					index = num3 - (j + 1);
					break;
				}
			}
			List<int> list2 = new List<int>();
			list2.Add(_mst_cell.Next_no_1);
			list2.Add(_mst_cell.Next_no_2);
			list2.Add(_mst_cell.Next_no_3);
			list2.Add(_mst_cell.Next_no_4);
			List<int> list3 = list2;
			cellNo = list3[index];
			return true;
		}

		private string[] getUseRateData()
		{
			char[] separator = new char[1]
			{
				','
			};
			MapRequireUserShipInfo activeShipInfo = getActiveShipInfo();
			if (!string.IsNullOrEmpty(_mst_cell.Next_rate_req))
			{
				if (activeShipInfo.Mem_ship.Count() < _mst_cell.Req_ship_count)
				{
					return _mst_cell.Next_rate.Split(separator);
				}
				if (!string.IsNullOrEmpty(_mst_cell.Req_shiptype))
				{
					string[] array = _mst_cell.Req_shiptype.Split(separator);
					string[] array2 = array;
					foreach (string s in array2)
					{
						int item = int.Parse(s);
						if (!activeShipInfo.Stype.Contains(item))
						{
							return _mst_cell.Next_rate.Split(separator);
						}
					}
					return _mst_cell.Next_rate_req.Split(separator);
				}
			}
			return _mst_cell.Next_rate.Split(separator);
		}

		private void UpdateMapcomp(int target_cell)
		{
			User_MapCellInfo user_MapCellInfo = User_mapcell[target_cell];
			if (user_MapCellInfo.Passed)
			{
				return;
			}
			if (string.IsNullOrEmpty(user_MapCellInfo.Mst_mapcell.Link_no))
			{
				User_mapcell[_now_Cell].Passed = true;
				mapComp.No.Add(_now_Cell);
				return;
			}
			string[] array = user_MapCellInfo.Mst_mapcell.Link_no.Split(',');
			string[] array2 = array;
			foreach (string s in array2)
			{
				int num = int.Parse(s);
				if (!User_mapcell[num].Passed)
				{
					User_mapcell[num].Passed = true;
					mapComp.No.Add(num);
				}
			}
		}

		private MapRequireUserShipInfo getActiveShipInfo()
		{
			return (!isLeadingDeck) ? userShipInfo1 : userShipInfo2;
		}

		private MapRequireUserShipInfo getInActiveShipInfo()
		{
			return (!isLeadingDeck) ? userShipInfo2 : userShipInfo1;
		}

		private bool initMapData(int maparea_id, int map_no)
		{
			int num = int.Parse(maparea_id.ToString() + map_no.ToString());
			if (mapComp == null && !Comm_UserDatas.Instance.User_mapcomp.TryGetValue(num, out mapComp))
			{
				mapComp = new Mem_mapcomp(num, maparea_id, map_no);
				mapComp.Insert();
			}
			if (mapClear == null && Comm_UserDatas.Instance.User_mapclear.TryGetValue(num, out mapClear) && mapClear.State == MapClearState.InvationClose)
			{
				return false;
			}
			mstRoute = Mst_DataManager.Instance.GetMaproute(num);
			mstMapIncentive = Mst_DataManager.Instance.GetMapIncentive(num);
			mstMapCellIncentive = Mst_DataManager.Instance.GetMapCellIncentive(num);
			if (_mst_stype_group == null)
			{
				_mst_stype_group = Utils.Xml_Result("mst_stype_group", "mst_stype_group", "Id");
				if (_mst_stype_group == null)
				{
					return false;
				}
			}
			if (!makeMapcell(maparea_id, map_no))
			{
				return false;
			}
			if (!makeMapEnemy(maparea_id, map_no))
			{
				return false;
			}
			if (!makeMapEnemyLevel(maparea_id, map_no))
			{
				return false;
			}
			if (!makeMapShipget(maparea_id, map_no))
			{
				return false;
			}
			_mst_maparea = null;
			if (!Mst_DataManager.Instance.Mst_maparea.TryGetValue(maparea_id, out _mst_maparea))
			{
				return false;
			}
			if (!_mst_maparea.IsOpenArea())
			{
				return false;
			}
			if (!Comm_UserDatas.Instance.User_rebellion_point.TryGetValue(maparea_id, out _mem_rebellion))
			{
				return false;
			}
			return true;
		}

		private bool makeMapcell(int maparea_id, int map_no)
		{
			Mst_DataManager.Instance.Make_MapCell(maparea_id, map_no);
			if (Mst_DataManager.Instance.Mst_mapcell == null)
			{
				return false;
			}
			if (User_mapcell == null)
			{
				User_mapcell = new Dictionary<int, User_MapCellInfo>();
			}
			User_mapcell.Clear();
			foreach (KeyValuePair<int, Mst_mapcell2> item in Mst_DataManager.Instance.Mst_mapcell)
			{
				bool passed = false;
				if (mapComp.No.Contains(item.Value.No))
				{
					passed = true;
				}
				User_MapCellInfo value = new User_MapCellInfo(item.Value, passed);
				User_mapcell.Add(item.Value.No, value);
			}
			return true;
		}

		private bool makeMapEnemy(int maparea_id, int map_no)
		{
			Mst_DataManager.Instance.Make_Mapenemy(maparea_id, map_no);
			if (Mst_DataManager.Instance.Mst_mapenemy == null)
			{
				return false;
			}
			if (_map_enemy == null)
			{
				_map_enemy = new Dictionary<int, Dictionary<int, Mst_mapenemy2>>();
			}
			_map_enemy.Clear();
			foreach (Mst_mapenemy2 value2 in Mst_DataManager.Instance.Mst_mapenemy.Values)
			{
				if (_map_enemy.ContainsKey(value2.Enemy_list_id))
				{
					_map_enemy[value2.Enemy_list_id].Add(value2.Deck_id, value2);
				}
				else
				{
					Dictionary<int, Mst_mapenemy2> dictionary = new Dictionary<int, Mst_mapenemy2>();
					dictionary.Add(value2.Deck_id, value2);
					Dictionary<int, Mst_mapenemy2> value = dictionary;
					_map_enemy.Add(value2.Enemy_list_id, value);
				}
			}
			return true;
		}

		private bool makeMapEnemyLevel(int maparea_id, int mapinfo_no)
		{
			_map_enemylevel = Mst_DataManager.Instance.GetMapenemyLevel(maparea_id, mapinfo_no);
			if (_map_enemylevel == null)
			{
				return false;
			}
			return true;
		}

		private bool makeMapShipget(int maparea_id, int map_no)
		{
			if (isRebbelion)
			{
				map_no = 4;
			}
			Mst_DataManager.Instance.Make_Mapshipget(maparea_id, map_no);
			if (Mst_DataManager.Instance.Mst_shipget == null || Mst_DataManager.Instance.Mst_shipget.Count() == 0)
			{
				return false;
			}
			return true;
		}

		private bool makeSupportData()
		{
			if (Mst_SupportData != null)
			{
				return true;
			}
			var source = from item in _mst_stype_group
				select new
				{
					id = int.Parse(item.Element("Id").Value),
					type = int.Parse(item.Element("Support").Value)
				};
			Mst_SupportData = source.ToLookup(key => key.type, value => value.id);
			return true;
		}

		private int getRebellionPointSubNum(int map_no)
		{
			double num = 0.0;
			switch (map_no)
			{
			case 1:
				num = 0.07;
				break;
			case 2:
				num = 0.1;
				break;
			case 3:
				num = 0.2;
				break;
			case 4:
				num = 0.14;
				break;
			case 5:
				num = 0.1;
				break;
			case 6:
				num = 0.05;
				break;
			}
			return (int)((double)_mem_rebellion.Point * num);
		}

		private bool IsOffshoreSupply(Mst_mapcell2 nowCell, Mst_mapcell2 nextCell, List<Mem_ship> ships)
		{
			if (Comm_UserDatas.Instance.User_material[enumMaterialCategory.Fuel].Value <= 0 && Comm_UserDatas.Instance.User_material[enumMaterialCategory.Bull].Value <= 0)
			{
				return false;
			}
			if (!IsOffshoreSuppllyCell(nowCell, nextCell))
			{
				return false;
			}
			double num = 0.0;
			double num2 = 0.0;
			int num3 = 0;
			foreach (var item in ships.Select((Mem_ship obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				Mem_ship obj2 = item.obj;
				int idx = item.idx;
				if (!obj2.Escape_sts)
				{
					num += (double)obj2.Fuel / (double)Mst_DataManager.Instance.Mst_ship[obj2.Ship_id].Fuel_max;
					num2 += (double)obj2.Bull / (double)Mst_DataManager.Instance.Mst_ship[obj2.Ship_id].Bull_max;
					num3++;
				}
			}
			if (num3 == 0)
			{
				return false;
			}
			double num4 = 0.25;
			num /= (double)num3;
			num2 /= (double)num3;
			double num5 = (num + num2) / 2.0;
			if (num5 > num4)
			{
				return false;
			}
			return true;
		}

		private bool IsOffshoreSuppllyCell(Mst_mapcell2 nowCell, Mst_mapcell2 nextCell)
		{
			if (nowCell.Event_1 != enumMapEventType.War_Normal)
			{
				return false;
			}
			if (!nextCell.IsNext())
			{
				return true;
			}
			if (nextCell.Event_1 == enumMapEventType.War_Normal || nextCell.Event_1 == enumMapEventType.War_Boss)
			{
				return true;
			}
			return false;
		}

		private void executeOffshoreSupply(MapRequireUserShipInfo ship_info, out MapSupplyFmt mapSupply)
		{
			mapSupply = null;
			int[] array = haveOffshoreSupplyItem(ship_info);
			if (array[0] != -1)
			{
				int index = array[0];
				int num = array[1];
				Mem_shipBase mem_shipBase = new Mem_shipBase(ship_info.Mem_ship[index]);
				int key;
				if (num == int.MaxValue)
				{
					key = ship_info.Mem_ship[index].Exslot;
					mem_shipBase.Exslot = -1;
				}
				else
				{
					key = ship_info.Mem_ship[index].Slot[num];
					mem_shipBase.Slot[num] = -1;
					ship_info.Mst_slotitems[index].RemoveAt(num);
				}
				ship_info.Mem_ship[index].Set_ShipParam(mem_shipBase, Mst_DataManager.Instance.Mst_ship[mem_shipBase.Ship_id], enemy_flag: false);
				ship_info.Mem_ship[index].TrimSlot();
				Comm_UserDatas.Instance.User_slot.Remove(key);
				int rid = ship_info.Mem_ship[index].Rid;
				List<int> list = new List<int>();
				foreach (Mem_ship item in ship_info.Mem_ship)
				{
					if (setOffshoreSupply(item))
					{
						list.Add(item.Rid);
					}
				}
				mapSupply = new MapSupplyFmt(rid, list);
			}
		}

		private int[] haveOffshoreSupplyItem(MapRequireUserShipInfo ship_info)
		{
			int[] array = new int[2]
			{
				-1,
				-1
			};
			int num = 146;
			for (int num2 = ship_info.Mem_ship.Count - 1; num2 >= 0; num2--)
			{
				if (!ship_info.Mem_ship[num2].Escape_sts)
				{
					Mst_slotitem mstSlotItemToExSlot = ship_info.Mem_ship[num2].GetMstSlotItemToExSlot();
					if (mstSlotItemToExSlot != null && mstSlotItemToExSlot.Id == num)
					{
						array[0] = num2;
						array[1] = int.MaxValue;
						return array;
					}
					List<Mst_slotitem> list = ship_info.Mst_slotitems[num2];
					for (int num3 = list.Count - 1; num3 >= 0; num3--)
					{
						Mst_slotitem mst_slotitem = list[num3];
						if (mst_slotitem.Id == num)
						{
							array[0] = num2;
							array[1] = num3;
							return array;
						}
					}
				}
			}
			return array;
		}

		private bool setOffshoreSupply(Mem_ship mem_ship)
		{
			if (mem_ship.Escape_sts)
			{
				return false;
			}
			int value = Comm_UserDatas.Instance.User_material[enumMaterialCategory.Fuel].Value;
			int value2 = Comm_UserDatas.Instance.User_material[enumMaterialCategory.Bull].Value;
			if (value <= 0 && value2 <= 0)
			{
				return false;
			}
			int fuel_max = Mst_DataManager.Instance.Mst_ship[mem_ship.Ship_id].Fuel_max;
			int bull_max = Mst_DataManager.Instance.Mst_ship[mem_ship.Ship_id].Bull_max;
			if (mem_ship.Fuel >= fuel_max && mem_ship.Bull >= bull_max)
			{
				return false;
			}
			double num = 0.25;
			double num2 = 0.25;
			int num3 = mem_ship.Fuel;
			int num4 = mem_ship.Bull;
			int num5 = 0;
			if (mem_ship.Fuel < fuel_max && value > 0)
			{
				int num6 = (int)((double)fuel_max * num);
				if (num6 == 0)
				{
					num6 = 1;
				}
				if (num6 > value)
				{
					num6 = value;
				}
				num3 = mem_ship.Fuel + num6;
				if (num3 > fuel_max)
				{
					num3 = fuel_max;
				}
				int num7 = num3 - mem_ship.Fuel;
				Comm_UserDatas.Instance.User_material[enumMaterialCategory.Fuel].Sub_Material(num7);
				num5++;
			}
			if (mem_ship.Bull < bull_max && value2 > 0)
			{
				int num8 = (int)((double)bull_max * num2);
				if (num8 == 0)
				{
					num8 = 1;
				}
				if (num8 > value2)
				{
					num8 = value2;
				}
				num4 = mem_ship.Bull + num8;
				if (num4 > bull_max)
				{
					num4 = bull_max;
				}
				int num9 = num4 - mem_ship.Bull;
				Comm_UserDatas.Instance.User_material[enumMaterialCategory.Bull].Sub_Material(num9);
				num5++;
			}
			if (num5 == 0)
			{
				return false;
			}
			mem_ship.Set_ChargeData(num4, num3, null);
			return true;
		}
	}
}
