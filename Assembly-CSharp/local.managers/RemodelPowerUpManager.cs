using Common.Struct;
using local.models;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public abstract class RemodelPowerUpManager : RemodelManagerBase
	{
		private ShipModel _powup_target_ship;

		private Dictionary<int, Mem_slotitem> _slotitems;

		private List<ShipModel> _material_candidate_ships;

		public ShipModel PowupTargetShip
		{
			get
			{
				return _powup_target_ship;
			}
			set
			{
				if (_powup_target_ship != value)
				{
					_powup_target_ship = value;
					_UpdateCandidateShips();
				}
			}
		}

		public RemodelPowerUpManager(int area_id)
			: base(area_id)
		{
			_UpdateCandidateShips();
		}

		public ShipModel[] GetCandidateShips(List<ShipModel> material_ships)
		{
			return _GetCandidateShip(material_ships).ToArray();
		}

		public ShipModel[] GetCandidateShips(List<ShipModel> material_ships, int page_no, int count_in_page, out int count)
		{
			List<ShipModel> list = _GetCandidateShip(material_ships);
			count = list.Count;
			int val = (page_no - 1) * count_in_page;
			val = Math.Max(val, 0);
			val = Math.Min(val, list.Count);
			int val2 = list.Count - val;
			val2 = Math.Max(val2, 0);
			val2 = Math.Min(val2, count_in_page);
			return list.GetRange(val, val2).ToArray();
		}

		public PowUpInfo getPowUpInfo(List<ShipModel> material_ships)
		{
			PowUpInfo result = default(PowUpInfo);
			if (_powup_target_ship == null)
			{
				return result;
			}
			HashSet<int> hashSet = new HashSet<int>();
			for (int i = 0; i < material_ships.Count; i++)
			{
				ShipModel shipModel = material_ships[i];
				if (shipModel != null)
				{
					result.Karyoku += shipModel.PowUpKaryoku;
					result.Raisou += shipModel.PowUpRaisou;
					result.Taiku += shipModel.PowUpTaikuu;
					result.Soukou += shipModel.PowUpSoukou;
					result.Lucky += shipModel.PowUpLucky;
					hashSet.Add(shipModel.MemId);
				}
			}
			result.Karyoku = (int)((double)result.Karyoku * 1.2 + 0.3);
			result.Raisou = (int)((double)result.Raisou * 1.2 + 0.3);
			result.Taiku = (int)((double)result.Taiku * 1.2 + 0.3);
			result.Soukou = (int)((double)result.Soukou * 1.2 + 0.3);
			result.Lucky = (int)((double)result.Lucky * 1.2 + 0.3);
			Api_req_Kaisou api_req_Kaisou = new Api_req_Kaisou();
			result.Taikyu += api_req_Kaisou.GetSameShipPowerupTaikyu(_powup_target_ship.MemId, hashSet);
			result.Lucky += api_req_Kaisou.GetSameShipPowerupLuck(_powup_target_ship.MemId, hashSet);
			int val = _powup_target_ship.MaxHp + result.Taikyu;
			int taik_max = Mst_DataManager.Instance.Mst_ship[_powup_target_ship.MstId].Taik_max;
			int num = Math.Min(val, taik_max);
			int val2 = num - _powup_target_ship.MaxHp;
			result.Taikyu = Math.Max(val2, 0);
			result.Karyoku = Math.Min(_powup_target_ship.KaryokuMax - _powup_target_ship.Karyoku, result.Karyoku);
			result.Raisou = Math.Min(_powup_target_ship.RaisouMax - _powup_target_ship.Raisou, result.Raisou);
			result.Taiku = Math.Min(_powup_target_ship.TaikuMax - _powup_target_ship.Taiku, result.Taiku);
			result.Soukou = Math.Min(_powup_target_ship.SoukouMax - _powup_target_ship.Soukou, result.Soukou);
			result.Lucky = Math.Min(_powup_target_ship.LuckyMax - _powup_target_ship.Lucky, result.Lucky);
			result.RemoveNegative();
			return result;
		}

		public bool IsValidPowerUp(List<ShipModel> material_ships)
		{
			if (_powup_target_ship == null)
			{
				return false;
			}
			if (material_ships == null)
			{
				return false;
			}
			if (material_ships.FindAll((ShipModel ship) => ship != null).Count <= 0)
			{
				return false;
			}
			if (getPowUpInfo(material_ships).IsAllZero())
			{
				return false;
			}
			DeckModelBase deck = _powup_target_ship.getDeck();
			if (deck != null && deck.IsActionEnd())
			{
				return false;
			}
			return true;
		}

		public ShipModel PowerUp(List<ShipModel> material_ships, out bool great_success)
		{
			great_success = false;
			if (_powup_target_ship == null)
			{
				return null;
			}
			if (material_ships == null)
			{
				return null;
			}
			HashSet<int> hashSet = new HashSet<int>();
			for (int i = 0; i < material_ships.Count; i++)
			{
				if (material_ships[i] != null)
				{
					hashSet.Add(material_ships[i].MemId);
				}
			}
			if (material_ships.FindAll((ShipModel ship) => ship != null).Count != hashSet.Count)
			{
				return null;
			}
			Api_Result<int> api_Result = new Api_req_Kaisou().Powerup(_powup_target_ship.MemId, hashSet);
			base.UserInfo.__UpdateShips__(new Api_get_Member());
			_UpdateOtherShips();
			_UpdateCandidateShips();
			_powup_target_ship = base.UserInfo.GetShip(_powup_target_ship.MemId);
			_slotitems = null;
			if (api_Result.state == Api_Result_State.Success)
			{
				if (api_Result.data == 2)
				{
					great_success = true;
					return _powup_target_ship;
				}
				if (api_Result.data == 1)
				{
					return _powup_target_ship;
				}
			}
			return null;
		}

		private List<ShipModel> _GetCandidateShip(List<ShipModel> MaterialShips)
		{
			return _material_candidate_ships.FindAll((ShipModel ship) => MaterialShips.IndexOf(ship) == -1);
		}

		private void _UpdateCandidateShips()
		{
			if (_powup_target_ship == null)
			{
				_material_candidate_ships = new List<ShipModel>();
				return;
			}
			if (_slotitems == null)
			{
				Api_Result<Dictionary<int, Mem_slotitem>> api_Result = new Api_get_Member().Slotitem();
				if (api_Result.state == Api_Result_State.Success && api_Result.data != null)
				{
					_slotitems = api_Result.data;
				}
			}
			_material_candidate_ships = base.UserInfo.__GetShipList__();
			List<int> ship_member_ids = base.UserInfo.__GetShipMemIdInAllDecks__();
			_material_candidate_ships = _material_candidate_ships.FindAll((ShipModel ship) => !ship.IsLocked() && !ship.__HasLocked__(_slotitems) && _powup_target_ship.MemId != ship.MemId && !ship_member_ids.Contains(ship.MemId) && !ship.IsInRepair() && !ship.IsBling() && (!ship.IsBlingWait() || ship.AreaIdBeforeBlingWait == base.AreaId));
		}

		public string ToString(List<ShipModel> MaterialShips)
		{
			string empty = string.Empty;
			empty += ToString();
			empty += "\n";
			empty = ((PowupTargetShip != null) ? (empty + $"対象艦: {PowupTargetShip.ShortName}\n") : (empty + $"対象艦: 未設定\n"));
			empty += "[--餌艦--]\n";
			for (int i = 0; i < MaterialShips.Count; i++)
			{
				ShipModel shipModel = MaterialShips[i];
				empty = ((shipModel == null) ? (empty + $"- - -\n") : (empty + $"{shipModel.ShortName} 餌艦効果(火:{shipModel.PowUpKaryoku} 雷:{shipModel.PowUpRaisou} 空:{shipModel.PowUpTaikuu} 装:{shipModel.PowUpSoukou})\n"));
			}
			if (_powup_target_ship != null)
			{
				PowUpInfo powUpInfo = getPowUpInfo(MaterialShips);
				empty += "対象艦のステ\u30fcタス\n";
				if (powUpInfo.Taikyu > 0)
				{
					empty += $"耐久:{PowupTargetShip.MaxHp}->{PowupTargetShip.MaxHp + powUpInfo.Taikyu}(+{powUpInfo.Taikyu}) ";
				}
				empty += $"火力:{PowupTargetShip.Karyoku}->{PowupTargetShip.Karyoku + powUpInfo.Karyoku}(+{powUpInfo.Karyoku}) 雷装:{PowupTargetShip.Raisou}->{PowupTargetShip.Raisou + powUpInfo.Raisou}(+{powUpInfo.Raisou}) 対空:{PowupTargetShip.Taiku}->{PowupTargetShip.Taiku + powUpInfo.Taiku}(+{powUpInfo.Taiku}) 装甲:{PowupTargetShip.Soukou}->{PowupTargetShip.Soukou + powUpInfo.Soukou}(+{powUpInfo.Soukou}) 運:{PowupTargetShip.Lucky}->{PowupTargetShip.Lucky + powUpInfo.Lucky}(+{powUpInfo.Lucky})";
			}
			return empty;
		}
	}
}
