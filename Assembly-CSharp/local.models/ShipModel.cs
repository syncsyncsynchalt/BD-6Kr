using Common.Enum;
using Common.Struct;
using local.managers;
using local.utils;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class ShipModel : __ShipModelMem__, IMemShip
	{
		private List<SlotitemModel> _slotitems;

		private SlotitemModel _slotitem_ex;

		public int RepairTime => _mem_data.GetNdockTimeSpan();

		public List<SlotitemModel> SlotitemList
		{
			get
			{
				if (!_CheckSlotitemCache())
				{
					_UpdateSlotitems();
				}
				return _slotitems.GetRange(0, _slotitems.Count);
			}
		}

		public SlotitemModel SlotitemEx
		{
			get
			{
				if (!_CheckSlotitemCache())
				{
					_UpdateSlotitems();
				}
				return _slotitem_ex;
			}
		}

		public ShipModel(int mem_id)
			: base(mem_id)
		{
		}

		public ShipModel(Mem_ship mem_ship)
			: base(mem_ship)
		{
		}

		public MaterialInfo GetResourcesForSupply()
		{
			MaterialInfo result = default(MaterialInfo);
			result.Fuel = _mem_data.GetRequireChargeFuel();
			result.Ammo = _mem_data.GetRequireChargeBull();
			return result;
		}

		public MaterialInfo GetResourcesForRepair()
		{
			Dictionary<enumMaterialCategory, int> ndockMaterialNum = _mem_data.GetNdockMaterialNum();
			return new MaterialInfo(ndockMaterialNum);
		}

		public bool IsMaxTaikyu()
		{
			int num = _mem_data.Kyouka[Mem_ship.enumKyoukaIdx.Taik_Powerup];
			if (num >= 3)
			{
				return true;
			}
			return MaxHp >= _mst_data.Taik_max;
		}

		public bool IsMaxKaryoku()
		{
			return Karyoku >= KaryokuMax;
		}

		public bool IsMaxRaisou()
		{
			return Raisou >= RaisouMax;
		}

		public bool IsMaxTaiku()
		{
			return Taiku >= TaikuMax;
		}

		public bool IsMaxSoukou()
		{
			return Soukou >= SoukouMax;
		}

		public bool IsMaxKaihi()
		{
			return Kaihi >= KaihiMax;
		}

		public bool IsMaxTaisen()
		{
			return Taisen >= TaisenMax;
		}

		public bool IsMaxLucky()
		{
			return Lucky >= LuckyMax;
		}

		public MaterialInfo GetResourcesForDestroy()
		{
			Dictionary<enumMaterialCategory, int> destroyShipMaterials = _mem_data.getDestroyShipMaterials();
			return new MaterialInfo(destroyShipMaterials);
		}

		public bool HasLocked()
		{
			foreach (SlotitemModel slotitem in SlotitemList)
			{
				if (slotitem != null && slotitem.IsLocked())
				{
					return true;
				}
			}
			if (_slotitem_ex != null && _slotitem_ex.IsLocked())
			{
				return true;
			}
			return false;
		}

		public bool __HasLocked__(Dictionary<int, Mem_slotitem> slotitems)
		{
			if (!_CheckSlotitemCache())
			{
				_slotitems = new List<SlotitemModel>();
				for (int i = 0; i < SlotCount; i++)
				{
					Mem_slotitem value;
					if (_mem_data.Slot[i] == -1)
					{
						_slotitems.Add(null);
					}
					else if (!slotitems.TryGetValue(_mem_data.Slot[i], out value))
					{
						_slotitems.Add(null);
					}
					else
					{
						_slotitems.Add(new SlotitemModel(value));
					}
				}
				if (_mem_data.Exslot > 0)
				{
					if (!slotitems.TryGetValue(_mem_data.Exslot, out Mem_slotitem value2))
					{
						_slotitem_ex = null;
					}
					else
					{
						_slotitem_ex = new SlotitemModel(value2);
					}
				}
				else
				{
					_slotitem_ex = null;
				}
			}
			return HasLocked();
		}

		public int GetSlotitemEquipCount()
		{
			return _mem_data.Slot.FindAll((int i) => i != -1).Count;
		}

		public bool HasExSlot()
		{
			return _mem_data.IsOpenExSlot();
		}

		public DeckModelBase getDeck()
		{
			return ManagerBase.getDeck(base.MemId);
		}

		public int IsInDeck()
		{
			return IsInDeck(search_flag_ship: true);
		}

		public int IsInDeck(bool search_flag_ship)
		{
			int num = DeckUtil.__IsInDeck__(base.MemId);
			if (!search_flag_ship && num == 0)
			{
				num = 1;
			}
			return num;
		}

		public bool IsInActionEndDeck()
		{
			return _mem_data.IsActiveEnd();
		}

		public int IsInEscortDeck()
		{
			return ManagerBase.IsInEscortDeck(base.MemId);
		}

		public bool IsInRepair()
		{
			return _mem_data.ExistsNdock();
		}

		public bool IsInMission()
		{
			return _mem_data.ExistsMission();
		}

		private bool _CheckSlotitemCache()
		{
			if (_slotitems == null)
			{
				return false;
			}
			if (_slotitems.Count != _mem_data.Slot.Count)
			{
				return false;
			}
			for (int i = 0; i < _slotitems.Count; i++)
			{
				if (_slotitems[i] == null && _mem_data.Slot[i] != -1)
				{
					return false;
				}
				if (_slotitems[i] != null && _mem_data.Slot[i] != _slotitems[i].MemId)
				{
					return false;
				}
			}
			if (_slotitem_ex == null && _mem_data.Exslot > 0)
			{
				return false;
			}
			if (_slotitem_ex != null && _slotitem_ex.MemId != _mem_data.Exslot)
			{
				return false;
			}
			return true;
		}

		private void _UpdateSlotitems()
		{
			_slotitems = new List<SlotitemModel>();
			Api_Result<Dictionary<int, Mem_slotitem>> api_Result = new Api_get_Member().Slotitem();
			if (api_Result.state == Api_Result_State.Success && api_Result.data != null)
			{
				for (int i = 0; i < SlotCount; i++)
				{
					if (_mem_data.Slot[i] == -1)
					{
						_slotitems.Add(null);
						continue;
					}
					Mem_slotitem data = api_Result.data[_mem_data.Slot[i]];
					_slotitems.Add(new SlotitemModel(data));
				}
				if (_mem_data.Exslot <= 0)
				{
					_slotitem_ex = null;
				}
				else
				{
					Mem_slotitem data2 = api_Result.data[_mem_data.Exslot];
					_slotitem_ex = new SlotitemModel(data2);
				}
			}
			while (_slotitems.Count < SlotCount)
			{
				_slotitems.Add(null);
			}
		}

		private int _GetMaterialCount(enumMaterialCategory material_type, Dictionary<enumMaterialCategory, int> data)
		{
			if (data == null)
			{
				return 0;
			}
			return data[material_type];
		}

		public override string ToString()
		{
			string str = string.Empty;
			if (IsBling())
			{
				str += "[回航中] ";
			}
			str += "Eq (";
			for (int i = 0; i < SlotCount; i++)
			{
				str += _ToString(SlotitemList[i], i);
				str += ((i >= SlotCount - 1) ? string.Empty : ", ");
			}
			str = ((!HasExSlot()) ? (str + ", [X]") : (str + ", " + _ToString(SlotitemEx, -1)));
			str += ")\n";
			return ToString(str);
		}

		private string _ToString(SlotitemModel slotitem, int slot_index)
		{
			string text = (slotitem != null) ? slotitem.ToString() : "[-]";
			if (slot_index >= 0 && slot_index < SlotCount)
			{
				text += $"搭載:{base.Tousai[slot_index]}/{base.TousaiMax[slot_index]} ";
			}
			return text;
		}

		int IMemShip.Lov
		{
            get {
                return base.Lov;
            }
        }
	}
}
