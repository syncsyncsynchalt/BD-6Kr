using Common.Enum;
using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class DeckModel : DeckModelBase
	{
		private Mem_deck _mem_deck;

		public override int Id => _mem_deck.Rid;

		public override string Name => _mem_deck.Name;

		public override int AreaId => _mem_deck.Area_id;

		public int MissionId => _mem_deck.Mission_id;

		public MissionStates MissionState => _mem_deck.MissionState;

		public int MissionCompleteTurn => _mem_deck.CompleteTime;

		public int MissionRemainingTurns => _mem_deck.GetRequireMissionTime();

		public DeckModel(Mem_deck mem_deck, Dictionary<int, ShipModel> ships)
		{
			_mem_deck = mem_deck;
			__Update__(mem_deck, ships);
		}

		public override bool IsActionEnd()
		{
			return _mem_deck.IsActionEnd;
		}

		public List<IsGoCondition> IsValidMove()
		{
			List<IsGoCondition> list = new List<IsGoCondition>();
			_IsValid_NoActionEnd(ref list);
			_IsValid_NoMission(ref list);
			_IsValid_ExistFlagShip(ref list);
			_IsValid_NoBling(ref list);
			_IsValid_NoRepair(ref list);
			return list;
		}

		public List<IsGoCondition> IsValidSortie()
		{
			List<IsGoCondition> list = new List<IsGoCondition>();
			_IsValid_NoActionEnd(ref list);
			_IsValid_NoMission(ref list);
			_IsValid_ExistFlagShip(ref list);
			_IsValid_NoBling(ref list);
			_IsValid_NoRepair(ref list);
			_IsValid_NoTaihaFlagShip(ref list);
			_IsValid_NeedSupply(ref list, 0);
			return list;
		}

		public List<IsGoCondition> IsValidRebellion()
		{
			return IsValidSortie();
		}

		public List<IsGoCondition> IsValidMission()
		{
			List<IsGoCondition> list = new List<IsGoCondition>();
			_IsValid_NoActionEnd(ref list);
			_IsValid_NoMission(ref list);
			if (Id == 1)
			{
				list.Add(IsGoCondition.Deck1);
			}
			_IsValid_ExistFlagShip(ref list);
			_IsValid_NoBling(ref list);
			_IsValid_NoRepair(ref list);
			_IsValid_NoTaihaFlagShip(ref list);
			return list;
		}

		public List<IsGoCondition> IsValidPractice()
		{
			List<IsGoCondition> list = new List<IsGoCondition>();
			_IsValid_NoActionEnd(ref list);
			_IsValid_NoMission(ref list);
			_IsValid_ExistFlagShip(ref list);
			_IsValid_NoBling(ref list);
			_IsValid_NoRepair(ref list);
			_IsValid_NoTaihaFlagShip(ref list);
			_IsValid_ConditionRed(ref list);
			_IsValid_NeedSupply(ref list, 70);
			return list;
		}

		public bool IsInSupportMission()
		{
			if (Mst_DataManager.Instance.Mst_mission.TryGetValue(MissionId, out Mst_mission2 value))
			{
				return value.IsSupportMission();
			}
			return false;
		}

		public void __Update__(Mem_deck mem_deck, Dictionary<int, ShipModel> ships)
		{
			_Update(_mem_deck.Ship, ships);
		}

		private void _IsValid_NoActionEnd(ref List<IsGoCondition> list)
		{
			if (IsActionEnd())
			{
				list.Add(IsGoCondition.ActionEndDeck);
			}
		}

		private void _IsValid_NoMission(ref List<IsGoCondition> list)
		{
			if (MissionState != 0)
			{
				list.Add(IsGoCondition.Mission);
			}
		}

		private void _IsValid_NoRepair(ref List<IsGoCondition> list)
		{
			if (HasRepair())
			{
				list.Add(IsGoCondition.HasRepair);
			}
		}

		private void _IsValid_ExistFlagShip(ref List<IsGoCondition> list)
		{
			if (base.Count == 0)
			{
				list.Add(IsGoCondition.InvalidDeck);
			}
		}

		private void _IsValid_NoTaihaFlagShip(ref List<IsGoCondition> list)
		{
			ShipModel ship = GetShip(0);
			if (ship != null && ship.DamageStatus == DamageState.Taiha)
			{
				list.Add(IsGoCondition.FlagShipTaiha);
			}
		}

		private void _IsValid_NoBling(ref List<IsGoCondition> list)
		{
			if (HasBling())
			{
				list.Add(IsGoCondition.HasBling);
			}
		}

		private void _IsValid_ConditionRed(ref List<IsGoCondition> list)
		{
			if (_ships.Find((ShipModel ship) => ship != null && ship.ConditionState == FatigueState.Distress) != null)
			{
				list.Add(IsGoCondition.ConditionRed);
			}
		}

		private void _IsValid_NeedSupply(ref List<IsGoCondition> list, int threshold)
		{
			ShipModel shipModel = (threshold != 0) ? _ships.Find((ShipModel s) => s != null && (s.FuelRate < (double)threshold || s.AmmoRate < (double)threshold)) : _ships.Find((ShipModel s) => s != null && (s.FuelRate == 0.0 || s.AmmoRate == 0.0));
			if (shipModel != null)
			{
				list.Add(IsGoCondition.NeedSupply);
			}
		}

		public override string ToString()
		{
			string empty = string.Empty;
			empty += $"[{Id}]{Name}:[";
			ShipModel[] ships = GetShips();
			for (int i = 0; i < ships.Length; i++)
			{
				ShipModel shipModel = ships[i];
				empty += $"{shipModel.Name}({shipModel.MstId},{shipModel.MemId})";
				if (i + 1 < ships.Length)
				{
					empty += $", ";
				}
			}
			empty += $"]";
			return empty + string.Format((!IsActionEnd()) ? string.Empty : "[行動終了]");
		}

		public string ToDetailString()
		{
			string empty = string.Empty;
			empty += $"[{Id}]{Name}";
			empty += string.Format("{0}\n", (!IsActionEnd()) ? string.Empty : "[行動終了]");
			ShipModel[] ships = GetShips();
			foreach (ShipModel shipModel in ships)
			{
				empty += $"  {shipModel.Name}(Lv:{shipModel.Level} Mst:{shipModel.MstId} Mem:{shipModel.MemId}";
				empty += $" 速:{shipModel.Soku} ";
				empty += $" 燃/弾:{shipModel.Fuel}/{shipModel.Ammo} ";
				for (int j = 0; j < shipModel.SlotCount; j++)
				{
					SlotitemModel slotitemModel = shipModel.SlotitemList[j];
					empty = ((slotitemModel != null) ? (empty + $"[{slotitemModel.Name}({slotitemModel.MstId}:{slotitemModel.MemId})]") : (empty + $"[-]"));
				}
				empty += ")\n";
			}
			return empty;
		}
	}
}
