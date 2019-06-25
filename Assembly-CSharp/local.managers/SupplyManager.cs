using Common.Enum;
using Common.Struct;
using local.models;
using local.utils;
using Server_Controllers;
using System.Collections.Generic;

namespace local.managers
{
	public class SupplyManager : ManagerBase
	{
		private int _area_id;

		private List<ShipModel> _other_ships;

		private DeckModel _selected_deck;

		private List<ShipModel> _target_ships;

		private List<int> _checked_ships;

		private int _fuel_for_supply;

		private int _ammo_for_supply;

		private CheckBoxStatus _checkbox_all_state;

		private CheckBoxStatus[] _checkbox_states;

		private SortKey _pre_sort_key;

		public MapAreaModel MapArea => ManagerBase._area[_area_id];

		public DeckModel SelectedDeck => _selected_deck;

		public int[] CheckedShipIndices => _checked_ships.ToArray();

		public int FuelForSupply => _fuel_for_supply;

		public int AmmoForSupply => _ammo_for_supply;

		public ShipModel[] Ships => _target_ships.ToArray();

		public CheckBoxStatus CheckBoxALLState => _checkbox_all_state;

		public CheckBoxStatus[] CheckBoxStates => _checkbox_states;

		public SortKey OtherShipSortKey => _pre_sort_key;

		public SupplyManager(int area_id)
		{
			_area_id = area_id;
			_CreateMapAreaModel();
			_target_ships = new List<ShipModel>();
			_checked_ships = new List<int>();
			_Initialize();
		}

		public void InitForDeck(int deck_id)
		{
			if (_selected_deck != null && _selected_deck.Id != deck_id)
			{
				_checked_ships.Clear();
			}
			else if (_selected_deck == null)
			{
				_checked_ships.Clear();
			}
			_selected_deck = base.UserInfo.GetDeck(deck_id);
			if (_selected_deck != null && _selected_deck.AreaId == MapArea.Id)
			{
				_target_ships = new List<ShipModel>(_selected_deck.GetShips());
				_checkbox_states = new CheckBoxStatus[_target_ships.Count];
				for (int i = 0; i < _target_ships.Count; i++)
				{
					ShipModel ship = _target_ships[i];
					_checkbox_states[i] = _GetCheckBoxStatus(ship, _selected_deck);
				}
			}
			else
			{
				_target_ships = new List<ShipModel>();
				_checkbox_states = new CheckBoxStatus[0];
			}
			_CalcMaterialForSupply();
			_SetCheckBoxAllState();
		}

		public void InitForOther()
		{
			if (_selected_deck != null)
			{
				_selected_deck = null;
				_checked_ships.Clear();
			}
			int count = _other_ships.Count;
			_target_ships = _other_ships.GetRange(0, count);
			_checkbox_states = new CheckBoxStatus[count];
			for (int i = 0; i < count; i++)
			{
				ShipModel ship = _other_ships[i];
				_checkbox_states[i] = _GetCheckBoxStatus(ship, null);
			}
			_CalcMaterialForSupply();
			_SetCheckBoxAllState();
		}

		public bool ChangeSortKey(SortKey new_sort_key)
		{
			if (_pre_sort_key != new_sort_key)
			{
				_pre_sort_key = new_sort_key;
				_other_ships = DeckUtil.GetSortedList(_other_ships, _pre_sort_key);
				if (_selected_deck == null)
				{
					InitForOther();
				}
				return true;
			}
			return false;
		}

		public bool IsShowOther()
		{
			return _selected_deck == null;
		}

		public bool IsChecked(int memID)
		{
			return _checked_ships.Contains(memID);
		}

		public void ClickCheckBoxAll()
		{
			switch (_checkbox_all_state)
			{
			case CheckBoxStatus.OFF:
				for (int j = 0; j < _target_ships.Count; j++)
				{
					ShipModel shipModel2 = _target_ships[j];
					if (shipModel2 != null && CheckBoxStates[j] != CheckBoxStatus.DISABLE)
					{
						_ForceOnCheckStatus(shipModel2.MemId);
					}
				}
				break;
			case CheckBoxStatus.ON:
				for (int i = 0; i < _target_ships.Count; i++)
				{
					ShipModel shipModel = _target_ships[i];
					if (shipModel != null)
					{
						_ForceOffCheckStatus(shipModel.MemId);
					}
				}
				break;
			}
			for (int k = 0; k < _target_ships.Count; k++)
			{
				_checkbox_states[k] = _GetCheckBoxStatus(_target_ships[k], _selected_deck);
			}
			_CalcMaterialForSupply();
			_SetCheckBoxAllState();
		}

		public void ClickCheckBox(int memId)
		{
			_ToggleCheckStatus(memId);
			_CalcMaterialForSupply();
			if (IsShowOther())
			{
				InitForOther();
			}
			else
			{
				InitForDeck(_selected_deck.Id);
			}
		}

		public bool IsValidSupply(SupplyType type)
		{
			switch (type)
			{
			case SupplyType.Fuel:
				if (FuelForSupply <= 0)
				{
					return false;
				}
				if (FuelForSupply > base.Material.Fuel)
				{
					return false;
				}
				return true;
			case SupplyType.Ammo:
				if (AmmoForSupply <= 0)
				{
					return false;
				}
				if (AmmoForSupply > base.Material.Ammo)
				{
					return false;
				}
				return true;
			default:
				return IsValidSupply(SupplyType.Fuel) || IsValidSupply(SupplyType.Ammo);
			}
		}

		public bool Supply(SupplyType type, out bool use_baux)
		{
			int baux = base.Material.Baux;
			bool result;
			switch (type)
			{
			case SupplyType.Fuel:
				result = _Supply(Api_req_Hokyu.enumHokyuType.Fuel);
				break;
			case SupplyType.Ammo:
				result = _Supply(Api_req_Hokyu.enumHokyuType.Bull);
				break;
			default:
				result = _Supply(Api_req_Hokyu.enumHokyuType.All);
				break;
			}
			use_baux = (baux - base.Material.Baux > 0);
			return result;
		}

		private bool _Supply(Api_req_Hokyu.enumHokyuType type)
		{
			Api_Result<bool> api_Result = new Api_req_Hokyu().Charge(_checked_ships, type);
			if (api_Result.state == Api_Result_State.Success)
			{
				_checked_ships = new List<int>();
				for (int i = 0; i < _target_ships.Count; i++)
				{
					_checkbox_states[i] = _GetCheckBoxStatus(_target_ships[i], _selected_deck);
				}
				_CalcMaterialForSupply();
				_SetCheckBoxAllState();
				return api_Result.data;
			}
			return false;
		}

		private void _CalcMaterialForSupply()
		{
			_fuel_for_supply = (_ammo_for_supply = 0);
			for (int i = 0; i < _checked_ships.Count; i++)
			{
				int mem_id = _checked_ships[i];
				ShipModel shipModel = _GetShipModel(mem_id);
				if (shipModel != null)
				{
					MaterialInfo resourcesForSupply = shipModel.GetResourcesForSupply();
					_fuel_for_supply += resourcesForSupply.Fuel;
					_ammo_for_supply += resourcesForSupply.Ammo;
				}
			}
		}

		private void _SetCheckBoxAllState()
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < _checkbox_states.Length; i++)
			{
				if (_checkbox_states[i] == CheckBoxStatus.DISABLE)
				{
					num++;
				}
				else if (_checkbox_states[i] == CheckBoxStatus.ON)
				{
					num2++;
				}
			}
			if (_checkbox_states.Length == num)
			{
				_checkbox_all_state = CheckBoxStatus.DISABLE;
			}
			else if (_checkbox_states.Length == num2 + num)
			{
				_checkbox_all_state = CheckBoxStatus.ON;
			}
			else
			{
				_checkbox_all_state = CheckBoxStatus.OFF;
			}
		}

		private void _ToggleCheckStatus(int memId)
		{
			if (_checked_ships.Contains(memId))
			{
				_checked_ships.Remove(memId);
				return;
			}
			ShipModel shipModel = _GetShipModel(memId);
			if (shipModel != null)
			{
				MaterialInfo resourcesForSupply = shipModel.GetResourcesForSupply();
				if (resourcesForSupply.Fuel > 0 || resourcesForSupply.Ammo > 0)
				{
					_checked_ships.Add(memId);
				}
			}
		}

		private void _ForceOnCheckStatus(int memId)
		{
			if (!_checked_ships.Contains(memId))
			{
				_checked_ships.Add(memId);
			}
		}

		private void _ForceOffCheckStatus(int memId)
		{
			if (_checked_ships.Contains(memId))
			{
				_checked_ships.Remove(memId);
			}
		}

		private void _Initialize()
		{
			List<ShipModel> all_ships = base.UserInfo.__GetShipList__();
			_other_ships = GetAreaShips(_area_id, use_deck: false, use_edeck: true, all_ships);
			if (_area_id == 1)
			{
				_other_ships.AddRange(GetDepotShips(all_ships));
			}
			_other_ships = _other_ships.FindAll((ShipModel ship) => ship.GetResourcesForSupply().HasPositive());
			_other_ships = DeckUtil.GetSortedList(_other_ships, _pre_sort_key);
		}

		private ShipModel _GetShipModel(int mem_id)
		{
			return _target_ships.Find((ShipModel ship) => ship.MemId == mem_id);
		}

		private CheckBoxStatus _GetCheckBoxStatus(ShipModel ship, DeckModel deck)
		{
			if (ship == null)
			{
				return CheckBoxStatus.DISABLE;
			}
			if (ship.IsInMission() || ship.IsBling())
			{
				return CheckBoxStatus.DISABLE;
			}
			if (ship.GetResourcesForSupply().HasPositive())
			{
				if (_checked_ships.IndexOf(ship.MemId) == -1)
				{
					return CheckBoxStatus.OFF;
				}
				return CheckBoxStatus.ON;
			}
			return CheckBoxStatus.DISABLE;
		}

		private string _GetString_CheckedShip()
		{
			string text = $"[選択艦 総数:{_checked_ships.Count}] ";
			for (int i = 0; i < _checked_ships.Count; i++)
			{
				ShipModel shipModel = _GetShipModel(_checked_ships[i]);
				text = text + shipModel.ShortName + " ";
			}
			return text;
		}

		public override string ToString()
		{
			string str = "--SupplyManager--\n";
			str += base.ToString();
			string text = str;
			str = text + "\n[補給に必要な資材数]燃料/弾薬:" + FuelForSupply + "/" + AmmoForSupply + "\n";
			str += string.Format("燃料補給:{0} 弾薬補給:{1} まとめて補給:{2}\n", (!IsValidSupply(SupplyType.Fuel)) ? "不可" : "可", (!IsValidSupply(SupplyType.Ammo)) ? "不可" : "可", (!IsValidSupply(SupplyType.All)) ? "不可" : "可");
			str = (IsShowOther() ? (str + $"他艦選択中\n") : (str + $"選択中の艦隊ID:{SelectedDeck.Id}\n"));
			str += _GetString_CheckedShip();
			str += "\n";
			str += $"\n{ToString(CheckBoxALLState)}\n";
			for (int i = 0; i < Ships.Length; i++)
			{
				ShipModel shipModel = Ships[i];
				if (shipModel != null)
				{
					str += $"{ToString(CheckBoxStates[i])}{shipModel.ShortName} Lv{shipModel.Level}";
					str += $" Fuel:{shipModel.Fuel}/{shipModel.FuelMax} Ammo:{shipModel.Ammo}/{shipModel.AmmoMax}\n";
				}
			}
			return str + "\n-----------------";
		}

		private string ToString(CheckBoxStatus cb)
		{
			switch (cb)
			{
			case CheckBoxStatus.DISABLE:
				return " - ";
			case CheckBoxStatus.OFF:
				return "[ ]";
			default:
				return "[o]";
			}
		}
	}
}
