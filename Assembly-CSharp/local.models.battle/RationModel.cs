using Common.Enum;
using Server_Models;
using System.Collections.Generic;
using System.Linq;

namespace local.models.battle
{
	public class RationModel
	{
		private List<ShipModel_Eater> _ships;

		private List<ShipModel_Eater> _shared;

		public List<ShipModel_Eater> EatingShips => _ships;

		public List<ShipModel_Eater> SharedShips => _shared;

		public RationModel(List<ShipModel_BattleAll> ships_f, Dictionary<int, List<Mst_slotitem>> data)
		{
			_ships = new List<ShipModel_Eater>();
			_shared = new List<ShipModel_Eater>();
			for (int i = 0; i < ships_f.Count; i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = ships_f[i];
				if (shipModel_BattleAll == null || !data.ContainsKey(shipModel_BattleAll.TmpId))
				{
					continue;
				}
				ShipModel_Eater item = shipModel_BattleAll.__CreateEater__();
				if (i > 0)
				{
					ShipModel_Eater shipModel_Eater = _GetSharedShip(data, ships_f[i - 1]);
					if (shipModel_Eater != null)
					{
						_shared.Add(shipModel_Eater);
					}
				}
				if (i < ships_f.Count - 1)
				{
					ShipModel_Eater shipModel_Eater2 = _GetSharedShip(data, ships_f[i + 1]);
					if (shipModel_Eater2 != null)
					{
						_shared.Add(shipModel_Eater2);
					}
				}
				_ships.Add(item);
			}
			_ships.Distinct();
		}

		private ShipModel_Eater _GetSharedShip(Dictionary<int, List<Mst_slotitem>> data, ShipModel_BattleAll candidate)
		{
			if (candidate == null)
			{
				return null;
			}
			if (candidate.DmgStateEnd == DamageState_Battle.Gekichin)
			{
				return null;
			}
			if (candidate.IsEscape())
			{
				return null;
			}
			if (data.ContainsKey(candidate.TmpId))
			{
				return null;
			}
			return candidate.__CreateEater__();
		}

		public override string ToString()
		{
			string text = "[ == 戦闘糧食フェ\u30fcズ == ]\n";
			for (int i = 0; i < EatingShips.Count; i++)
			{
				ShipModel_Eater arg = EatingShips[i];
				text += $"[戦闘糧食 使用艦]{arg}\n";
			}
			for (int j = 0; j < SharedShips.Count; j++)
			{
				ShipModel_Eater arg2 = SharedShips[j];
				text += $"[戦闘糧食 分配艦]{arg2}\n";
			}
			return text;
		}
	}
}
