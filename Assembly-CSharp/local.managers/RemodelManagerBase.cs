using Common.Enum;
using Common.Struct;
using local.models;
using local.utils;
using Server_Common;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace local.managers
{
	public abstract class RemodelManagerBase : ManagerBase
	{
		private Api_req_Kaisou _api;

		private int _area_id;

		private List<ShipModel> _other_ships;

		private Dictionary<int, List<SlotitemModel>> _unset_slotitems;

		private int _hokyo_zousetsu_num;

		public int AreaId => _area_id;

		public int HokyoZousetsuNum => _hokyo_zousetsu_num;

		public RemodelManagerBase(int area_id)
		{
			_area_id = area_id;
			_api = new Api_req_Kaisou();
			_UpdateOtherShips();
			_hokyo_zousetsu_num = new UseitemUtil().GetCount(64);
		}

		public DeckModel[] GetDecks()
		{
			return base.UserInfo.GetDecksFromArea(_area_id);
		}

		public ShipModel[] GetOtherShipList()
		{
			return _other_ships.ToArray();
		}

		public ShipModel[] GetOtherShipList(int page_no, int count_in_page)
		{
			int val = (page_no - 1) * count_in_page;
			val = Math.Max(val, 0);
			val = Math.Min(val, _other_ships.Count);
			int val2 = _other_ships.Count - val;
			val2 = Math.Max(val2, 0);
			val2 = Math.Min(val2, count_in_page);
			return _other_ships.GetRange(val, val2).ToArray();
		}

		public ShipModel[] GetOtherShipList(int page_no, int count_in_page, out int count)
		{
			count = GetOtherShipCount();
			return GetOtherShipList(page_no, count_in_page);
		}

		public int GetOtherShipCount()
		{
			return _other_ships.Count;
		}

		public bool IsValidShip(ShipModel ship)
		{
			if (ship == null)
			{
				return false;
			}
			if (ship.IsInRepair())
			{
				return false;
			}
			if (ship.IsInMission())
			{
				return false;
			}
			if (ship.IsBling())
			{
				return false;
			}
			if (ship.IsInActionEndDeck())
			{
				return false;
			}
			return true;
		}

		public bool IsValidGradeUp(ShipModel ship)
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[ship.MstId];
			if (mst_ship.Aftershipid <= 0)
			{
				return false;
			}
			if (ship.Level < mst_ship.Afterlv)
			{
				return false;
			}
			return true;
		}

		public SlotitemModel[] GetSlotitemList(int ship_mem_id, SlotitemCategory category)
		{
			return _GetSlotitemList(ship_mem_id, category).ToArray();
		}

		public SlotitemModel[] GetSlotitemList(int ship_mem_id, SlotitemCategory category, int page_no, int count_in_page, out int count)
		{
			List<SlotitemModel> list = _GetSlotitemList(ship_mem_id, category);
			count = list.Count;
			int val = (page_no - 1) * count_in_page;
			val = Math.Max(val, 0);
			val = Math.Min(val, list.Count);
			int val2 = list.Count - val;
			val2 = Math.Max(val2, 0);
			val2 = Math.Min(val2, count_in_page);
			return list.GetRange(val, val2).ToArray();
		}

		public SlotSetChkResult_Slot IsValidChangeSlotitem(int ship_mem_id, int slotitem_id, int slot_index)
		{
			return _api.IsValidSlotSet(ship_mem_id, slotitem_id, slot_index);
		}

		public SlotSetChkResult_Slot ChangeSlotitem(int ship_mem_id, int slotitem_id, int slot_index)
		{
			Api_Result<SlotSetChkResult_Slot> api_Result = _api.SlotSet(ship_mem_id, slotitem_id, slot_index);
			_unset_slotitems = null;
			return api_Result.data;
		}

		public bool IsValidUnsetSlotitem(int ship_mem_id, int slot_index)
		{
			SlotSetChkResult_Slot slotSetChkResult_Slot = _api.IsValidSlotSet(ship_mem_id, -1, slot_index);
			return slotSetChkResult_Slot == SlotSetChkResult_Slot.Ok || slotSetChkResult_Slot == SlotSetChkResult_Slot.OkBauxiteUse || slotSetChkResult_Slot == SlotSetChkResult_Slot.OkBauxiteUseHighCost;
		}

		public bool UnsetSlotitem(int ship_mem_id, int slot_index)
		{
			Api_Result<SlotSetChkResult_Slot> api_Result = _api.SlotSet(ship_mem_id, -1, slot_index);
			if (api_Result.state != 0)
			{
				return false;
			}
			_unset_slotitems = null;
			return true;
		}

		public bool IsValidUnsetAll(int ship_mem_id)
		{
			ShipModel ship = base.UserInfo.GetShip(ship_mem_id);
			if (ship == null)
			{
				return false;
			}
			if (!IsValidShip(ship))
			{
				return false;
			}
			return ship.GetSlotitemEquipCount() > 0;
		}

		public bool UnsetAll(int ship_mem_id)
		{
			Api_Result<Hashtable> api_Result = _api.Unslot_all(ship_mem_id);
			if (api_Result.state != 0)
			{
				return false;
			}
			_unset_slotitems = null;
			return true;
		}

		public SlotSetInfo GetSlotitemInfoToChange(int ship_mem_id, int slotitem_id, int slot_index)
		{
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[ship_mem_id];
			return mem_ship.GetSlotSetParam(slot_index, slotitem_id);
		}

		public SlotSetInfo GetSlotitemInfoToUnset(int ship_mem_id, int slot_index)
		{
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[ship_mem_id];
			return mem_ship.GetSlotSetParam(slot_index, -1);
		}

		public bool SlotLock(int slot_mem_id)
		{
			Api_Result<bool> api_Result = _api.SlotLockChange(slot_mem_id);
			if (api_Result.state != 0)
			{
				return false;
			}
			return true;
		}

		public SlotitemModel[] GetSlotitemExList(int ship_mem_id)
		{
			return _GetSlotitemExList(ship_mem_id).ToArray();
		}

		public SlotitemModel[] GetSlotitemExList(int ship_mem_id, int page_no, int count_in_page, out int count)
		{
			List<SlotitemModel> list = _GetSlotitemExList(ship_mem_id);
			count = list.Count;
			int val = (page_no - 1) * count_in_page;
			val = Math.Max(val, 0);
			val = Math.Min(val, list.Count);
			int val2 = list.Count - val;
			val2 = Math.Max(val2, 0);
			val2 = Math.Min(val2, count_in_page);
			return list.GetRange(val, val2).ToArray();
		}

		public SlotSetChkResult_Slot IsValidChangeSlotitemEx(int ship_mem_id, int slotitem_id)
		{
			return _api.IsValidSlotSet(ship_mem_id, slotitem_id);
		}

		public SlotSetChkResult_Slot ChangeSlotitemEx(int ship_mem_id, int slotitem_id)
		{
			Api_Result<SlotSetChkResult_Slot> api_Result = _api.SlotSet(ship_mem_id, slotitem_id);
			_unset_slotitems = null;
			return api_Result.data;
		}

		public bool IsValidUnsetSlotitemEx(int ship_mem_id)
		{
			SlotSetChkResult_Slot slotSetChkResult_Slot = _api.IsValidSlotSet(ship_mem_id, -1);
			return slotSetChkResult_Slot == SlotSetChkResult_Slot.Ok || slotSetChkResult_Slot == SlotSetChkResult_Slot.OkBauxiteUse || slotSetChkResult_Slot == SlotSetChkResult_Slot.OkBauxiteUseHighCost;
		}

		public bool UnsetSlotitemEx(int ship_mem_id)
		{
			Api_Result<SlotSetChkResult_Slot> api_Result = _api.SlotSet(ship_mem_id, -1);
			if (api_Result.state != 0)
			{
				return false;
			}
			_unset_slotitems = null;
			return true;
		}

		public bool IsValidOpenSlotEx(int ship_mem_id)
		{
			if (HokyoZousetsuNum == 0)
			{
				return false;
			}
			return _api.IsExpandSlotShip(ship_mem_id);
		}

		public bool OpenSlotEx(int ship_mem_id)
		{
			if (!IsValidOpenSlotEx(ship_mem_id))
			{
				return false;
			}
			Api_Result<Mem_ship> api_Result = _api.ExpandSlot(ship_mem_id);
			if (api_Result.state == Api_Result_State.Success)
			{
				_hokyo_zousetsu_num--;
				return true;
			}
			return false;
		}

		public void ClearUnsetSlotsCache()
		{
			_unset_slotitems = null;
		}

		protected void _UpdateOtherShips()
		{
			List<ShipModel> all_ships = base.UserInfo.__GetShipList__();
			_other_ships = GetAreaShips(AreaId, use_deck: false, use_edeck: true, all_ships);
			if (_area_id == 1)
			{
				_other_ships.AddRange(GetDepotShips(all_ships));
			}
			_other_ships = DeckUtil.GetSortedList(_other_ships, SortKey.LEVEL);
		}

		private void _UpdateUnsetSlotitems()
		{
			_unset_slotitems = new Dictionary<int, List<SlotitemModel>>();
			Api_Result<Dictionary<int, Mem_slotitem>> api_Result = new Api_get_Member().Slotitem();
			if (api_Result.state == Api_Result_State.Success)
			{
				foreach (int key in api_Result.data.Keys)
				{
					SlotitemModel slotitemModel = new SlotitemModel(api_Result.data[key]);
					if (!slotitemModel.IsEauiped())
					{
						if (!_unset_slotitems.ContainsKey(slotitemModel.Type3))
						{
							_unset_slotitems[slotitemModel.Type3] = new List<SlotitemModel>();
						}
						_unset_slotitems[slotitemModel.Type3].Add(slotitemModel);
					}
				}
			}
		}

		private ShipModel _GetShip(int ship_mem_id)
		{
			int num = DeckUtil.__IsInDeck__(ship_mem_id, checkPartnerShip: false);
			if (num == -1)
			{
				return _other_ships.Find((ShipModel item) => item.MemId == ship_mem_id);
			}
			return base.UserInfo.GetDeck(num).GetShipFromMemId(ship_mem_id);
		}

		private List<SlotitemModel> _GetSlotitemList(int ship_id, SlotitemCategory category)
		{
			List<SlotitemModel> list = new List<SlotitemModel>();
			ShipModel shipModel = _GetShip(ship_id);
			if (shipModel == null)
			{
				return list;
			}
			if (_unset_slotitems == null)
			{
				_UpdateUnsetSlotitems();
			}
			List<int> equipList = Mst_DataManager.Instance.Mst_ship[shipModel.MstId].GetEquipList();
			List<int> list2;
			if (category == SlotitemCategory.None)
			{
				list2 = equipList;
			}
			else
			{
				list2 = (from pair in Mst_DataManager.Instance.Mst_equip_category
					where pair.Value.Slotitem_type == category
					select pair.Key).ToList();
				list2 = list2.Intersect(equipList).ToList();
			}
			for (int i = 0; i < list2.Count; i++)
			{
				int key = list2[i];
				if (_unset_slotitems.ContainsKey(key))
				{
					list.AddRange(_unset_slotitems[key]);
				}
			}
			return list;
		}

		private List<SlotitemModel> _GetSlotitemExList(int ship_mem_id)
		{
			List<SlotitemModel> list = new List<SlotitemModel>();
			ShipModel shipModel = _GetShip(ship_mem_id);
			if (shipModel == null)
			{
				return list;
			}
			if (_unset_slotitems == null)
			{
				_UpdateUnsetSlotitems();
			}
			List<int> list2 = new List<int>();
			list2.Add(23);
			list2.Add(43);
			list2.Add(44);
			List<int> first = list2;
			List<int> equipList = Mst_DataManager.Instance.Mst_ship[shipModel.MstId].GetEquipList();
			first = first.Intersect(equipList).ToList();
			for (int i = 0; i < first.Count; i++)
			{
				int key = first[i];
				if (_unset_slotitems.ContainsKey(key))
				{
					list.AddRange(_unset_slotitems[key]);
				}
			}
			return list;
		}
	}
}
