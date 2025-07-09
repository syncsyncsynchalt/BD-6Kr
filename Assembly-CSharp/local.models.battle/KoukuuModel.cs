using Common.Enum;
using Server_Common.Formats.Battle;
using System.Collections.Generic;

namespace local.models.battle
{
	public class KoukuuModel : KoukuuModelBase
	{
		private List<ShipModel_Attacker> _attackers_f;

		private List<ShipModel_Attacker> _attackers_e;

		public BattleSeikuKinds SeikuKind => (_data.Air1 != null) ? _data.Air1.SeikuKind : BattleSeikuKinds.None;

		public KoukuuModel(List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, AirBattle data)
			: base(ships_f, ships_e, data)
		{
			_attackers_f = new List<ShipModel_Attacker>();
			int j;
			for (j = 0; j < _data.F_PlaneFrom.Count; j++)
			{
				ShipModel_BattleAll shipModel_BattleAll = _ships_f.Find((ShipModel_BattleAll ship) => ship != null && ship.TmpId == _data.F_PlaneFrom[j]);
				if (shipModel_BattleAll != null)
				{
					_attackers_f.Add(shipModel_BattleAll.__CreateAttacker__());
				}
			}
			_attackers_e = new List<ShipModel_Attacker>();
			int i;
			for (i = 0; i < _data.E_PlaneFrom.Count; i++)
			{
				ShipModel_BattleAll shipModel_BattleAll2 = _ships_e.Find((ShipModel_BattleAll ship) => ship != null && ship.TmpId == _data.E_PlaneFrom[i]);
				if (shipModel_BattleAll2 != null)
				{
					_attackers_e.Add(shipModel_BattleAll2.__CreateAttacker__());
				}
			}
			_Initialize();
		}

		public bool ExistKoukuuBattle()
		{
			return _data.F_PlaneFrom.Count > 0 || _data.E_PlaneFrom.Count > 0;
		}

		public ShipModel_Attacker GetCaptainShip(bool is_friend)
		{
			if (is_friend)
			{
				return (_attackers_f.Count != 0) ? _attackers_f[0] : null;
			}
			return (_attackers_e.Count != 0) ? _attackers_e[0] : null;
		}

		public ShipModel_Battle GetFirstActionShip()
		{
			ShipModel_Battle captainShip = GetCaptainShip(is_friend: true);
			if (captainShip == null)
			{
				captainShip = GetCaptainShip(is_friend: false);
			}
			return captainShip;
		}

		public List<ShipModel_Attacker> GetAttackers(bool is_friend)
		{
			if (is_friend)
			{
				return _attackers_f.GetRange(0, _attackers_f.Count);
			}
			return _attackers_e.GetRange(0, _attackers_e.Count);
		}

		public PlaneModelBase[] GetPlane(int ship_tmp_id)
		{
			ShipModel_Battle shipModel_Battle = _ships_f.Find((ShipModel_BattleAll ship) => ship != null && ship.TmpId == ship_tmp_id);
			if (shipModel_Battle == null)
			{
				shipModel_Battle = _ships_e.Find((ShipModel_BattleAll ship) => ship != null && ship.TmpId == ship_tmp_id);
			}
			if (shipModel_Battle == null)
			{
				return new PlaneModelBase[0];
			}
			return _GetPlane(shipModel_Battle).ToArray();
		}

		public SlotitemModel_Battle GetTouchPlane(bool is_friend)
		{
			if (_data.Air1 != null)
			{
				int num = (!is_friend) ? _data.Air1.E_TouchPlane : _data.Air1.F_TouchPlane;
				if (num > 0)
				{
					return new SlotitemModel_Battle(num);
				}
			}
			return null;
		}

		private List<PlaneModel> _GetPlane(ShipModel_Battle ship)
		{
			List<PlaneModel> list = (!ship.IsFriend()) ? _planes_e.ConvertAll((PlaneModelBase plane) => (PlaneModel)plane) : _planes_f.ConvertAll((PlaneModelBase plane) => (PlaneModel)plane);
			return list.FindAll((PlaneModel plane) => plane.Parent.TmpId == ship.TmpId);
		}

		protected override void _CreatePlanes()
		{
			_planes_f = __CreatePlanes(_attackers_f, _data.F_PlaneFrom);
			_planes_e = __CreatePlanes(_attackers_e, _data.E_PlaneFrom);
		}

		private string ToString_Plane(List<ShipModel_BattleAll> ships)
		{
			string text = string.Empty;
			for (int i = 0; i < ships.Count; i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = ships[i];
				if (shipModel_BattleAll == null)
				{
					continue;
				}
				PlaneModelBase[] plane = GetPlane(shipModel_BattleAll.TmpId);
				text += $"[{i}]{shipModel_BattleAll.Name}:";
				if (plane.Length == 0)
				{
					text += $"航空機無し";
				}
				else
				{
					for (int j = 0; j < plane.Length; j++)
					{
						text += $"({plane[j]}) ";
					}
				}
				text += "\n";
			}
			return text;
		}

		private string ToString_TouchPlane(bool is_friend)
		{
			string empty = string.Empty;
			SlotitemModel_Battle touchPlane = GetTouchPlane(is_friend);
			if (touchPlane == null)
			{
				return empty + string.Format("{0}側触接:無し ", (!is_friend) ? "相手" : "味方");
			}
			return empty + string.Format("{0}側触接:{1} ", (!is_friend) ? "相手" : "味方", touchPlane);
		}

		public override string ToString()
		{
			string str = "[航空戦データ]\n";
			str += $"--味方側航空機\n";
			str += ToString_Plane(_ships_f);
			str += $"--相手側航空機\n";
			str += ToString_Plane(_ships_e);
			str += $"--制空権:{SeikuKind} ";
			str += ToString_TouchPlane(is_friend: true);
			str += ToString_TouchPlane(is_friend: false);
			str += $"味方側カットイン対象艦:{GetCaptainShip(is_friend: true)} ";
			str += $"相手側カットイン対象艦:{GetCaptainShip(is_friend: false)}\n";
			str += ToString_Stage1();
			str += ToString_Stage2();
			return str + ToString_Stage3();
		}
	}
}
