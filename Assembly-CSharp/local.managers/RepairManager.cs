using Common.Enum;
using Common.Struct;
using local.models;
using local.utils;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class RepairManager : ManagerBase
	{
		private int _area_id;

		private List<RepairDockModel> _docks;

		private List<ShipModel> _ships;

		private SortKey _pre_sort_key = SortKey.DAMAGE;

		private Mem_useitem _dock_key_item;

		public int NumOfKeyPossessions => (_dock_key_item != null) ? _dock_key_item.Value : 0;

		public MapAreaModel MapArea => ManagerBase._area[_area_id];

		public int ShipsCount => _ships.Count;

		public SortKey NowSortKey => _pre_sort_key;

		public RepairManager(int area_id)
		{
			_area_id = area_id;
			_CreateMapAreaModel();
			_UpdateRepairDockData();
			_UpdateRepairShipList();
			Api_Result<Dictionary<int, Mem_useitem>> api_Result = new Api_get_Member().UseItem();
			if (api_Result.state == Api_Result_State.Success && api_Result.data != null)
			{
				api_Result.data.TryGetValue(49, out _dock_key_item);
			}
		}

		public RepairDockModel[] GetDocks()
		{
			return _docks.ToArray();
		}

		public RepairDockModel GetDockData(int index)
		{
			return _docks[index];
		}

		public RepairDockModel GetDockDataFromID(int id)
		{
			return _docks.Find((RepairDockModel dock) => dock.Id == id);
		}

		public int GetDockIndexFromDock(RepairDockModel dock)
		{
			return _docks.IndexOf(dock);
		}

		public bool ChangeSortKey(SortKey new_sort_key)
		{
			if (_pre_sort_key != new_sort_key)
			{
				_pre_sort_key = new_sort_key;
				_ships = DeckUtil.GetSortedList(_ships, _pre_sort_key);
				return true;
			}
			return false;
		}

		public ShipModel[] GetShipList()
		{
			return _ships.ToArray();
		}

		public ShipModel[] GetShipList(int page_no, int count_in_page)
		{
			int val = (page_no - 1) * count_in_page;
			val = Math.Max(val, 0);
			val = Math.Min(val, _ships.Count);
			int val2 = _ships.Count - val;
			val2 = Math.Max(val2, 0);
			val2 = Math.Min(val2, count_in_page);
			return _ships.GetRange(val, val2).ToArray();
		}

		public bool IsValidStartRepair(int ship_mem_id)
		{
			return IsValidStartRepair(ship_mem_id, use_repairkit: false);
		}

		public bool IsValidStartRepair(int ship_mem_id, bool use_repairkit)
		{
			ShipModel shipModel = _ships.Find((ShipModel x) => x.MemId == ship_mem_id);
			if (shipModel == null)
			{
				return false;
			}
			if (shipModel.TaikyuRate >= 100.0)
			{
				return false;
			}
			if (shipModel.IsInMission() || shipModel.IsInRepair())
			{
				return false;
			}
			if (shipModel.IsBling())
			{
				return false;
			}
			if (shipModel.IsBlingWaitFromEscortDeck())
			{
				return false;
			}
			MaterialInfo resourcesForRepair = shipModel.GetResourcesForRepair();
			if (base.Material.Fuel < resourcesForRepair.Fuel)
			{
				return false;
			}
			if (base.Material.Steel < resourcesForRepair.Steel)
			{
				return false;
			}
			if (use_repairkit && base.Material.RepairKit < 1)
			{
				return false;
			}
			DeckModelBase deck = shipModel.getDeck();
			if (deck != null)
			{
				if (deck.IsEscortDeckMyself())
				{
					return false;
				}
				return deck.AreaId == MapArea.Id;
			}
			if (shipModel.IsBlingWaitFromDeck() && shipModel.AreaIdBeforeBlingWait == MapArea.Id)
			{
				return true;
			}
			if (MapArea.Id == 1)
			{
				return true;
			}
			return false;
		}

		public bool StartRepair(int selected_dock_index, int ship_mem_id, bool use_repairkit)
		{
			if (!IsValidStartRepair(ship_mem_id, use_repairkit))
			{
				return false;
			}
			RepairDockModel repairDockModel = _docks[selected_dock_index];
			Api_Result<Mem_ndock> api_Result = new Api_req_Nyuukyo().Start(repairDockModel.Id, ship_mem_id, use_repairkit);
			if (api_Result.state == Api_Result_State.Success)
			{
				Mem_ndock data = api_Result.data;
				repairDockModel.__Update__(data);
				if (use_repairkit && _pre_sort_key == SortKey.DAMAGE)
				{
					_UpdateRepairShipList();
				}
				return true;
			}
			return false;
		}

		public bool IsValidChangeRepairSpeed(int selected_dock_index)
		{
			RepairDockModel repairDockModel = _docks[selected_dock_index];
			if (repairDockModel.State != NdockStates.RESTORE)
			{
				return false;
			}
			if (base.Material.RepairKit < 1)
			{
				return false;
			}
			return true;
		}

		public bool ChangeRepairSpeed(int selected_dock_index)
		{
			RepairDockModel repairDockModel = _docks[selected_dock_index];
			Api_Result<Mem_ndock> api_Result = new Api_req_Nyuukyo().SpeedChange(repairDockModel.Id);
			if (api_Result.state == Api_Result_State.Success)
			{
				Mem_ndock data = api_Result.data;
				repairDockModel.__Update__(data);
				return true;
			}
			return false;
		}

		public bool IsValidOpenNewDock()
		{
			return new Api_req_Member().IsValidNdockOpen(MapArea.Id);
		}

		public RepairDockModel OpenNewDock()
		{
			Api_Result<Mem_ndock> result = new Api_req_Member().NdockOpen(MapArea.Id);
			if (result.state == Api_Result_State.Success && result.data != null)
			{
				_UpdateRepairDockData();
				return _docks.Find((RepairDockModel dock) => dock.Id == result.data.Rid);
			}
			return null;
		}

		public void UpdateRepairDockData()
		{
			_UpdateRepairDockData();
		}

		private void _UpdateRepairDockData()
		{
			_docks = new List<RepairDockModel>();
			Dictionary<int, List<Mem_ndock>> data = new Api_get_Member().AreaNdock().data;
			if (data.ContainsKey(MapArea.Id))
			{
				List<Mem_ndock> list = data[MapArea.Id];
				for (int i = 0; i < list.Count; i++)
				{
					RepairDockModel item = new RepairDockModel(list[i]);
					_docks.Add(item);
				}
				MapArea.__UpdateNdockData__(list);
			}
		}

		private void _UpdateRepairShipList()
		{
			List<ShipModel> all_ships = base.UserInfo.__GetShipList__();
			_ships = GetAreaShips(MapArea.Id, all_ships);
			if (_area_id == 1)
			{
				_ships.AddRange(GetDepotShips(all_ships));
			}
			_ships = _ships.FindAll((ShipModel ship) => ship.NowHp < ship.MaxHp);
			_ships = DeckUtil.GetSortedList(_ships, _pre_sort_key);
		}

		public override string ToString()
		{
			string empty = string.Empty;
			empty += $"{base.ToString()}\n";
			for (int i = 0; i < _docks.Count; i++)
			{
				empty += $"Dock{i}:{_docks[i]}\n";
			}
			return empty + $"所持している開放キ\u30fc:{NumOfKeyPossessions}\n";
		}
	}
}
