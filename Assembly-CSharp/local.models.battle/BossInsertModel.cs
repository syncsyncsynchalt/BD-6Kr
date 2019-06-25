namespace local.models.battle
{
	public class BossInsertModel
	{
		private ShipModel_BattleAll _ship;

		public ShipModel_BattleAll Ship => _ship;

		public BossInsertModel(ShipModel_BattleAll boss_ship)
		{
			_ship = boss_ship;
		}

		public override string ToString()
		{
			return $"{_ship.Name}(MstId:{_ship.MstId}) 艦種:{_ship.ShipTypeName}\n";
		}
	}
}
