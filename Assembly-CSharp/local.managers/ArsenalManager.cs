using Common.Enum;
using Common.Struct;
using local.models;
using local.utils;
using Server_Common;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class ArsenalManager : ManagerBase
	{
		private Api_req_Kousyou _api;

		private Mem_useitem _dock_key_item;

		private bool _large;

		private List<BuildDockModel> _docks;

		private List<ShipModel> _breakable_ships;

		private SortKey _pre_sort_key;

		private List<SlotitemModel> _unset_slotitem;

		private List<int> _selected_items = new List<int>();

		public int NumOfKeyPossessions => (_dock_key_item != null) ? _dock_key_item.Value : 0;

		public bool LargeState
		{
			get
			{
				return _large;
			}
			set
			{
				_large = (value && LargeEnabled);
			}
		}

		public bool LargeEnabled => Comm_UserDatas.Instance.User_basic.Large_dock > 0;

		public SortKey NowSortKey => _pre_sort_key;

		public ArsenalManager()
		{
			_tanker_manager = new _TankerManager();
			_api = new Api_req_Kousyou();
			_UpdateBuildDockData();
			Api_Result<Dictionary<int, Mem_useitem>> api_Result = new Api_get_Member().UseItem();
			if (api_Result.state == Api_Result_State.Success && api_Result.data != null)
			{
				api_Result.data.TryGetValue(49, out _dock_key_item);
			}
		}

		public MaterialInfo GetMaxForCreateShip()
		{
			Dictionary<enumMaterialCategory, int> requireMaterials_Max = _api.GetRequireMaterials_Max(LargeState ? 1 : 0);
			return new MaterialInfo(requireMaterials_Max);
		}

		public MaterialInfo GetMinForCreateShip()
		{
			Dictionary<enumMaterialCategory, int> requireMaterials_Min = _api.GetRequireMaterials_Min(LargeState ? 1 : 0);
			return new MaterialInfo(requireMaterials_Min);
		}

		public BuildDockModel GetDock(int dock_id)
		{
			_CheckBuildDockState();
			return _docks.Find((BuildDockModel item) => item.Id == dock_id);
		}

		public BuildDockModel[] GetDocks()
		{
			_CheckBuildDockState();
			return _docks.ToArray();
		}

		public bool IsValidCreateShip(int dock_id, bool highSpeed, int fuel, int ammo, int steel, int baux, int dev_kit, int deck_id)
		{
			Dictionary<enumMaterialCategory, int> materials = new Dictionary<enumMaterialCategory, int>();
			materials[enumMaterialCategory.Fuel] = fuel;
			materials[enumMaterialCategory.Bull] = ammo;
			materials[enumMaterialCategory.Steel] = steel;
			materials[enumMaterialCategory.Bauxite] = baux;
			materials[enumMaterialCategory.Dev_Kit] = dev_kit;
			return _api.ValidStart(dock_id, highSpeed, _large, ref materials, deck_id);
		}

		public bool CreateShip(int dock_id, bool highSpeed, int fuel, int ammo, int steel, int baux, int dev_kit, int deck_id)
		{
			Dictionary<enumMaterialCategory, int> materials = new Dictionary<enumMaterialCategory, int>();
			materials[enumMaterialCategory.Fuel] = fuel;
			materials[enumMaterialCategory.Bull] = ammo;
			materials[enumMaterialCategory.Steel] = steel;
			materials[enumMaterialCategory.Bauxite] = baux;
			materials[enumMaterialCategory.Dev_Kit] = dev_kit;
			if (!_api.ValidStart(dock_id, highSpeed, _large, ref materials, deck_id))
			{
				return false;
			}
			Api_Result<Mem_kdock> api_Result = _api.Start(dock_id, highSpeed, _large, materials, deck_id);
			if (api_Result.state == Api_Result_State.Success)
			{
				Mem_kdock kdock = api_Result.data;
				int index = _docks.FindIndex((BuildDockModel item) => item.Id == kdock.Rid);
				_docks[index].__Update__(kdock);
				return true;
			}
			return false;
		}

		public bool IsValidGetCreatedShip(int dock_id)
		{
			return _api.ValidGetShip(dock_id);
		}

		public IReward_Ship GetCreatedShip(int dock_id)
		{
			if (!IsValidGetCreatedShip(dock_id))
			{
				return null;
			}
			int shipMstId = GetDock(dock_id).ShipMstId;
			Api_Result<Mem_kdock> ship = _api.GetShip(dock_id);
			if (ship.state == Api_Result_State.Success)
			{
				base.UserInfo.__UpdateShips__(new Api_get_Member());
				_breakable_ships = null;
				return new Reward_Ship(shipMstId);
			}
			return null;
		}

		public bool IsValidCreateShip_ChangeHighSpeed(int dock_id)
		{
			return _api.ValidSpeedChange(dock_id);
		}

		public bool ChangeHighSpeed(int dock_id)
		{
			if (!_api.ValidSpeedChange(dock_id))
			{
				return false;
			}
			Api_Result<Mem_kdock> api_Result = _api.SpeedChange(dock_id);
			if (api_Result.state == Api_Result_State.Success)
			{
				return true;
			}
			return false;
		}

		public bool IsValidOpenNewDock()
		{
			return new Api_req_Member().IsValidKdockOpen();
		}

		public BuildDockModel OpenNewDock()
		{
			Api_Result<Mem_kdock> result = new Api_req_Member().KdockOpen();
			if (result.state == Api_Result_State.Success && result.data != null)
			{
				_UpdateBuildDockData();
				return _docks.Find((BuildDockModel dock) => dock.Id == result.data.Rid);
			}
			return null;
		}

		private void _CheckBuildDockState()
		{
			bool flag = false;
			for (int i = 0; i < _docks.Count; i++)
			{
				BuildDockModel buildDockModel = _docks[i];
				if (buildDockModel.State == KdockStates.CREATE && buildDockModel.GetTurn() == 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				_UpdateBuildDockData();
			}
		}

		private void _UpdateBuildDockData()
		{
			if (_docks == null)
			{
				_docks = new List<BuildDockModel>();
			}
			Api_Result<List<Mem_kdock>> api_Result = new Api_get_Member().kdock();
			if (api_Result.state != 0)
			{
				return;
			}
			List<Mem_kdock> mem_docks = api_Result.data;
			int i;
			for (i = 0; i < mem_docks.Count; i++)
			{
				BuildDockModel buildDockModel = _docks.Find((BuildDockModel dock) => dock.Id == mem_docks[i].Rid);
				if (buildDockModel != null)
				{
					buildDockModel.__Update__(mem_docks[i]);
				}
				else
				{
					_docks.Add(new BuildDockModel(mem_docks[i]));
				}
			}
		}

		public bool ChangeSortKey(SortKey new_sort_key)
		{
			if (_pre_sort_key != new_sort_key)
			{
				_pre_sort_key = new_sort_key;
				_breakable_ships = null;
				return true;
			}
			return false;
		}

		public ShipModel[] GetShipList()
		{
			if (_breakable_ships == null)
			{
				_UpdateShipList();
			}
			return _breakable_ships.ToArray();
		}

		public ShipModel[] GetShipList(int page_no, int count_in_page)
		{
			if (_breakable_ships == null)
			{
				_UpdateShipList();
			}
			int val = (page_no - 1) * count_in_page;
			val = Math.Max(val, 0);
			val = Math.Min(val, _breakable_ships.Count);
			int val2 = _breakable_ships.Count - val;
			val2 = Math.Max(val2, 0);
			val2 = Math.Min(val2, count_in_page);
			return _breakable_ships.GetRange(val, val2).ToArray();
		}

		public bool IsValidBreakShip(ShipModel ship)
		{
			if (ship.IsLocked())
			{
				return false;
			}
			if (ship.HasLocked())
			{
				return false;
			}
			if (ship.IsInDeck() >= 0)
			{
				return false;
			}
			if (ship.IsInEscortDeck() >= 0)
			{
				return false;
			}
			if (ship.IsInRepair())
			{
				return false;
			}
			if (ship.IsBling())
			{
				return false;
			}
			return true;
		}

		public bool BreakShip(int ship_mem_id)
		{
			Api_Result<object> api_Result = _api.DestroyShip(ship_mem_id);
			if (api_Result.state == Api_Result_State.Success)
			{
				base.UserInfo.__UpdateShips__(new Api_get_Member());
				_breakable_ships = null;
				return true;
			}
			return false;
		}

		private void _UpdateShipList()
		{
			List<ShipModel> all_ships = base.UserInfo.__GetShipList__();
			_breakable_ships = GetAreaShips(1, use_deck: false, use_edeck: false, all_ships);
			_breakable_ships.AddRange(GetDepotShips(all_ships));
			_breakable_ships = DeckUtil.GetSortedList(_breakable_ships, _pre_sort_key);
		}

		public MaterialInfo GetMaxForCreateItem()
		{
			MaterialInfo result = default(MaterialInfo);
			result.Fuel = 300;
			result.Ammo = 300;
			result.Steel = 300;
			result.Baux = 300;
			result.Devkit = 1;
			return result;
		}

		public MaterialInfo GetMinForCreateItem()
		{
			MaterialInfo result = default(MaterialInfo);
			result.Fuel = 10;
			result.Ammo = 10;
			result.Steel = 10;
			result.Baux = 10;
			result.Devkit = 0;
			return result;
		}

		public bool IsValidCreateItem(int fuel, int ammo, int steel, int baux)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary[enumMaterialCategory.Fuel] = fuel;
			dictionary[enumMaterialCategory.Bull] = ammo;
			dictionary[enumMaterialCategory.Steel] = steel;
			dictionary[enumMaterialCategory.Bauxite] = baux;
			return IsValidCreateItem(dictionary);
		}

		public bool IsValidCreateItem(Dictionary<enumMaterialCategory, int> materials)
		{
			if (Comm_UserDatas.Instance.User_basic.IsMaxSlotitem())
			{
				return false;
			}
			MaterialInfo maxForCreateItem = GetMaxForCreateItem();
			MaterialInfo minForCreateItem = GetMinForCreateItem();
			materials.TryGetValue(enumMaterialCategory.Fuel, out int value);
			if (value < minForCreateItem.Fuel || maxForCreateItem.Fuel < value || base.Material.Fuel < value)
			{
				return false;
			}
			materials.TryGetValue(enumMaterialCategory.Bull, out value);
			if (value < minForCreateItem.Ammo || maxForCreateItem.Ammo < value || base.Material.Ammo < value)
			{
				return false;
			}
			materials.TryGetValue(enumMaterialCategory.Steel, out value);
			if (value < minForCreateItem.Steel || maxForCreateItem.Steel < value || base.Material.Steel < value)
			{
				return false;
			}
			materials.TryGetValue(enumMaterialCategory.Bauxite, out value);
			if (value < minForCreateItem.Baux || maxForCreateItem.Baux < value || base.Material.Baux < value)
			{
				return false;
			}
			if (base.Material.Devkit < 1)
			{
				return false;
			}
			return true;
		}

		public IReward_Slotitem CreateItem(int fuel, int ammo, int steel, int baux, int deck_id)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary[enumMaterialCategory.Fuel] = fuel;
			dictionary[enumMaterialCategory.Bull] = ammo;
			dictionary[enumMaterialCategory.Steel] = steel;
			dictionary[enumMaterialCategory.Bauxite] = baux;
			return CreateItem(dictionary, deck_id);
		}

		public IReward_Slotitem CreateItem(Dictionary<enumMaterialCategory, int> materials, int deck_id)
		{
			Api_Result<Mst_slotitem> api_Result = _api.CreateItem(materials, deck_id);
			if (api_Result.state != 0 || api_Result.data == null)
			{
				return null;
			}
			_unset_slotitem = null;
			return new Reward_Slotitem(api_Result.data);
		}

		public List<SlotitemModel> GetSelectedItemsForDetroy()
		{
			if (_unset_slotitem == null)
			{
				_UpdateUnsetSlotitems();
			}
			List<SlotitemModel> list = _unset_slotitem.FindAll((SlotitemModel item) => _selected_items.IndexOf(item.MemId) >= 0);
			if (list.Count != _selected_items.Count)
			{
				return null;
			}
			return list;
		}

		public bool IsSelected(int slotitem_mem_id)
		{
			int num = _selected_items.IndexOf(slotitem_mem_id);
			return num != -1;
		}

		public bool ToggleSelectedState(int slotitem_mem_id)
		{
			if (IsSelected(slotitem_mem_id))
			{
				_selected_items.Remove(slotitem_mem_id);
				return false;
			}
			if (_unset_slotitem == null)
			{
				_UpdateUnsetSlotitems();
			}
			if (_unset_slotitem.Find((SlotitemModel item) => item.MemId == slotitem_mem_id) != null)
			{
				_selected_items.Add(slotitem_mem_id);
				return true;
			}
			return false;
		}

		public bool SelectForDestroy(int slotitem_mem_id)
		{
			if (!IsSelected(slotitem_mem_id))
			{
				if (_unset_slotitem == null)
				{
					_UpdateUnsetSlotitems();
				}
				if (_unset_slotitem.Find((SlotitemModel item) => item.MemId == slotitem_mem_id) != null)
				{
					_selected_items.Add(slotitem_mem_id);
					return true;
				}
			}
			return false;
		}

		public bool UnselectForDestroy(int slotitem_mem_id)
		{
			if (IsSelected(slotitem_mem_id))
			{
				_selected_items.Remove(slotitem_mem_id);
				return true;
			}
			return false;
		}

		public void ClearSelectedState()
		{
			_selected_items.Clear();
		}

		public SlotitemModel[] GetUnsetSlotitems()
		{
			if (_unset_slotitem == null)
			{
				_UpdateUnsetSlotitems();
			}
			return _unset_slotitem.ToArray();
		}

		public SlotitemModel[] GetUnsetSlotitems(int page_no, int count_in_page)
		{
			if (_unset_slotitem == null)
			{
				_UpdateUnsetSlotitems();
			}
			int val = (page_no - 1) * count_in_page;
			val = Math.Max(val, 0);
			val = Math.Min(val, _unset_slotitem.Count);
			int val2 = _unset_slotitem.Count - val;
			val2 = Math.Max(val2, 0);
			val2 = Math.Min(val2, count_in_page);
			return _unset_slotitem.GetRange(val, val2).ToArray();
		}

		public MaterialInfo GetMaterialsForBreakItem()
		{
			MaterialInfo result = default(MaterialInfo);
			List<SlotitemModel> selectedItemsForDetroy = GetSelectedItemsForDetroy();
			for (int i = 0; i < selectedItemsForDetroy.Count; i++)
			{
				SlotitemModel slotitemModel = selectedItemsForDetroy[i];
				if (slotitemModel != null)
				{
					result.Fuel += slotitemModel.BrokenFuel;
					result.Ammo += slotitemModel.BrokenAmmo;
					result.Steel += slotitemModel.BrokenSteel;
					result.Baux += slotitemModel.BrokenBaux;
				}
			}
			return result;
		}

		public void GetMaterialsForBreakItem(out int fuel, out int ammo, out int steel, out int baux)
		{
			fuel = (ammo = (steel = (baux = 0)));
			if (_unset_slotitem == null)
			{
				_UpdateUnsetSlotitems();
			}
			for (int i = 0; i < _selected_items.Count; i++)
			{
				int slotitem_mem_id = _selected_items[i];
				SlotitemModel slotitemModel = _unset_slotitem.Find((SlotitemModel tmp) => tmp.MemId == slotitem_mem_id);
				if (slotitemModel != null)
				{
					fuel += slotitemModel.BrokenFuel;
					ammo += slotitemModel.BrokenAmmo;
					steel += slotitemModel.BrokenSteel;
					baux += slotitemModel.BrokenBaux;
				}
			}
		}

		public bool IsValidBreakItem()
		{
			if (_selected_items.Count == 0)
			{
				return false;
			}
			if (_unset_slotitem == null)
			{
				_UpdateUnsetSlotitems();
			}
			for (int i = 0; i < _selected_items.Count; i++)
			{
				int slotitem_mem_id = _selected_items[i];
				SlotitemModel slotitemModel = _unset_slotitem.Find((SlotitemModel tmp) => tmp.MemId == slotitem_mem_id);
				if (slotitemModel.IsLocked())
				{
					return false;
				}
			}
			return true;
		}

		public bool BreakItem()
		{
			Api_Result<object> api_Result = _api.DestroyItem(_selected_items);
			if (api_Result.state == Api_Result_State.Success)
			{
				_unset_slotitem = null;
				ClearSelectedState();
				return true;
			}
			return false;
		}

		private void _UpdateUnsetSlotitems()
		{
			_unset_slotitem = SlotitemUtil.__GetUnsetSlotitems__();
			SlotitemUtil.Sort(_unset_slotitem, SlotitemUtil.SlotitemSortKey.Type3);
		}

		public MaterialInfo GetMaxForCreateTanker()
		{
			Dictionary<enumMaterialCategory, int> requireMaterials_Max = _api.GetRequireMaterials_Max(2);
			return new MaterialInfo(requireMaterials_Max);
		}

		public MaterialInfo GetMinForCreateTanker()
		{
			Dictionary<enumMaterialCategory, int> requireMaterials_Min = _api.GetRequireMaterials_Min(2);
			return new MaterialInfo(requireMaterials_Min);
		}

		public int GetSpointMaxForCreateTanker()
		{
			return _api.Stratege_Max;
		}

		public int GetSpointMinForCreateTanker()
		{
			return _api.Stratege_Min;
		}

		public AreaTankerModel GetNonDeploymentTankerCount()
		{
			return _tanker_manager.GetCounts();
		}

		public bool IsValidCreateTanker(int dock_id, bool highSpeed, int fuel, int ammo, int steel, int baux, int spoint)
		{
			Dictionary<enumMaterialCategory, int> materials = new Dictionary<enumMaterialCategory, int>();
			materials[enumMaterialCategory.Fuel] = fuel;
			materials[enumMaterialCategory.Bull] = ammo;
			materials[enumMaterialCategory.Steel] = steel;
			materials[enumMaterialCategory.Bauxite] = baux;
			return _api.ValidStartTanker(dock_id, highSpeed, ref materials, spoint);
		}

		public BuildDockModel CreateTanker(int dock_id, bool highSpeed, int fuel, int ammo, int steel, int baux, int spoint)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary[enumMaterialCategory.Fuel] = fuel;
			dictionary[enumMaterialCategory.Bull] = ammo;
			dictionary[enumMaterialCategory.Steel] = steel;
			dictionary[enumMaterialCategory.Bauxite] = baux;
			int value = 0;
			if (highSpeed)
			{
				value = _api.GetRequireMaterials_Max(2)[enumMaterialCategory.Build_Kit];
			}
			dictionary[enumMaterialCategory.Build_Kit] = value;
			if (!IsValidCreateTanker(dock_id, highSpeed, fuel, ammo, steel, baux, spoint))
			{
				return null;
			}
			Api_Result<Mem_kdock> api_result = _api.StartTanker(dock_id, highSpeed, dictionary, spoint);
			if (api_result.state != 0 || api_result.data == null)
			{
				return null;
			}
			return _docks.Find((BuildDockModel dock) => dock.Id == api_result.data.Rid);
		}

		public bool IsValidGetCreatedTanker(int dock_id)
		{
			return _api.ValidGetTanker(dock_id);
		}

		public int GetCreatedTanker(int dock_id)
		{
			if (!IsValidGetCreatedTanker(dock_id))
			{
				return 0;
			}
			int tankerCount = GetDock(dock_id).TankerCount;
			Api_Result<Mem_kdock> tanker = _api.GetTanker(dock_id);
			if (tanker.state == Api_Result_State.Success)
			{
				_tanker_manager.Update();
				return tankerCount;
			}
			return 0;
		}

		public bool IsValid_ChangeHighSpeedTanker(int dock_id)
		{
			return _api.ValidSpeedChangeTanker(dock_id);
		}

		public bool ChangeHighSpeedTanker(int dock_id)
		{
			if (!_api.ValidSpeedChangeTanker(dock_id))
			{
				return false;
			}
			Api_Result<Mem_kdock> api_Result = _api.SpeedChangeTanker(dock_id);
			if (api_Result.state == Api_Result_State.Success)
			{
				return true;
			}
			return false;
		}

		public override string ToString()
		{
			BuildDockModel[] docks = GetDocks();
			string empty = string.Empty;
			empty += base.ToString();
			empty += "\n";
			empty += string.Format("状態:{0}\n", (!LargeState) ? "通常" : "大型");
			for (int i = 0; i < docks.Length; i++)
			{
				empty += $"Dock{i}:{docks[i]}\n";
			}
			empty += $"所持している開放キー:{NumOfKeyPossessions}\n";
			return empty + $"未配備の輸送船数:{_tanker_manager.GetCounts().GetCount()}(この海域への移動中:{_tanker_manager.GetCounts().GetCountMove()}) - 総数:{_tanker_manager.GetAllCount()}";
		}
	}
}
