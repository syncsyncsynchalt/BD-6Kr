using Common.Enum;
using Common.Struct;
using local.utils;
using Server_Models;
using System;

namespace local.models
{
	public class Reward_Ship : IReward, IReward_Ship
	{
		private ShipModelMst _ship;

		public ShipModelMst Ship => _ship;

		[Obsolete("Ship から取得してください", false)]
		public string Name => _ship.Name;

		public string GreetingText => Utils.GetText(TextType.SHIP_GET_TEXT, _ship.MstId);

		[Obsolete("Ship から取得してください", false)]
		public Point Offset => _ship.Offsets.GetShipDisplayCenter(damaged: false);

		public Reward_Ship(Mst_ship mst)
		{
			_ship = new ShipModelMst(mst);
		}

		public Reward_Ship(int mst_id)
		{
			_ship = new ShipModelMst(mst_id);
		}

		public override string ToString()
		{
			return $"{Ship.ShipTypeName} {Ship.Name}(ID:{Ship.MstId}) レア度:{Ship.Rare}";
		}
	}
}
