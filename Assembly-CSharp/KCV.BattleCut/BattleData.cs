namespace KCV.BattleCut
{
	public class BattleData
	{
		private HPData _clsFriendFleetHP;

		private HPData _clsEnemyFleetHP;

		public HPData friendFleetHP => _clsFriendFleetHP;

		public HPData enemyFleetHP => _clsEnemyFleetHP;

		public BattleData()
		{
			_clsFriendFleetHP = new HPData(0, 0);
			_clsEnemyFleetHP = new HPData(0, 0);
		}

		public bool Init()
		{
			return true;
		}

		public bool UnInit()
		{
			Mem.Del(ref _clsFriendFleetHP);
			Mem.Del(ref _clsEnemyFleetHP);
			return true;
		}
	}
}
