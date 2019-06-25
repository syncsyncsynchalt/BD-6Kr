using Server_Common.Formats;
using System.Collections.Generic;

namespace local.models
{
	public abstract class DeckActionResultModel
	{
		protected MissionResultFmt _mission_fmt;

		protected UserInfoModel _user_info;

		protected Dictionary<int, ShipExpModel> _exps;

		public int DeckID => _mission_fmt.Deck.Rid;

		public ShipModel[] Ships => _user_info.GetDeck(DeckID).GetShips();

		public string Name => _user_info.Name;

		public int Rank => _user_info.Rank;

		public int Level => _mission_fmt.MemberLevel;

		public string FleetName => _mission_fmt.Deck.Name;

		public int Exp => _mission_fmt.GetMemberExp;

		public ShipExpModel GetShipExpInfo(int ship_mem_id)
		{
			_exps.TryGetValue(ship_mem_id, out ShipExpModel value);
			return value;
		}

		protected void _SetShipExp(Dictionary<int, int> exp_rates_before)
		{
			ShipModel[] ships = Ships;
			foreach (ShipModel shipModel in ships)
			{
				exp_rates_before.TryGetValue(shipModel.MemId, out int value);
				_mission_fmt.GetShipExp.TryGetValue(shipModel.MemId, out int value2);
				_mission_fmt.LevelUpInfo.TryGetValue(shipModel.MemId, out List<int> value3);
				_exps[shipModel.MemId] = new ShipExpModel(value, shipModel, value2, value3);
			}
		}
	}
}
