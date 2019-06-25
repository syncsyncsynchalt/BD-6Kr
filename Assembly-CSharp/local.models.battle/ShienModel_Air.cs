using Common.Enum;
using Server_Common.Formats.Battle;
using System.Collections.Generic;

namespace local.models.battle
{
	public class ShienModel_Air : KoukuuModelBase, IShienModel
	{
		protected int _shien_deck_id;

		protected List<ShipModel_Attacker> _ships_shien;

		public int ShienDeckId => _shien_deck_id;

		public ShipModel_Attacker[] ShienShips => _ships_shien.ToArray();

		public BattleSupportKinds SupportType => BattleSupportKinds.AirAtack;

		public ShienModel_Air(DeckModel shien_deck, List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, SupportAtack data)
			: base(ships_f, ships_e, data.AirBattle)
		{
			_shien_deck_id = shien_deck.Id;
			_ships_shien = new List<ShipModel_Attacker>();
			ShipModel[] ships = shien_deck.GetShips();
			for (int i = 0; i < ships.Length; i++)
			{
				_ships_shien.Add(new __ShipModel_Attacker__(ships[i], i));
			}
			_Initialize();
		}

		protected override void _CreatePlanes()
		{
			_planes_f = __CreatePlanes(_ships_shien, _data.F_PlaneFrom);
			List<ShipModel_Attacker> list = new List<ShipModel_Attacker>();
			for (int i = 0; i < _ships_e.Count; i++)
			{
				if (_ships_e[i] != null)
				{
					list.Add(_ships_e[i].__CreateAttacker__());
				}
			}
			_planes_e = __CreatePlanes(list, _data.E_PlaneFrom);
		}

		private string ToString_Plane(ShipModel_Attacker[] ships)
		{
			string empty = string.Empty;
			PlaneModelBase[] planes = GetPlanes(is_friend: true);
			if (planes.Length == 0)
			{
				empty += $"味方側 航空機無し\n";
			}
			else
			{
				empty += $"味方側 ";
				for (int i = 0; i < planes.Length; i++)
				{
					empty += $"({planes[i]}) ";
				}
				empty += $"\n";
			}
			PlaneModelBase[] planes2 = GetPlanes(is_friend: false);
			if (planes2.Length == 0)
			{
				return empty + $"相手側 航空機無し\n";
			}
			empty += $"相手側 ";
			for (int j = 0; j < planes2.Length; j++)
			{
				empty += $"({planes2[j]}) ";
			}
			return empty + $"\n";
		}

		public override string ToString()
		{
			string empty = string.Empty;
			empty += $"--味方側航空機\n";
			empty += ToString_Plane(ShienShips);
			empty += ToString_Stage1();
			empty += ToString_Stage2();
			return empty + ToString_Stage3();
		}
	}
}
