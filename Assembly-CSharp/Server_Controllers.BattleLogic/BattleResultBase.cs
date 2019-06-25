using Server_Common.Formats;
using Server_Models;
using System.Collections.Generic;

namespace Server_Controllers.BattleLogic
{
	public class BattleResultBase
	{
		private BattleBaseData _myData;

		private BattleBaseData _enemyData;

		private bool _practiceFlag;

		private ExecBattleKinds _execKinds;

		private Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		private Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		private Mem_mapclear _cleard;

		private Mst_mapcell2 _nowCell;

		private bool _rebellionBattle;

		private List<MapItemGetFmt> getAirCellItems;

		public BattleBaseData MyData
		{
			get
			{
				return _myData;
			}
			set
			{
				_myData = value;
			}
		}

		public BattleBaseData EnemyData
		{
			get
			{
				return _enemyData;
			}
			set
			{
				_enemyData = value;
			}
		}

		public bool PracticeFlag
		{
			get
			{
				return _practiceFlag;
			}
			set
			{
				_practiceFlag = value;
			}
		}

		public ExecBattleKinds ExecKinds
		{
			get
			{
				return _execKinds;
			}
			set
			{
				_execKinds = value;
			}
		}

		public Dictionary<int, BattleShipSubInfo> F_SubInfo
		{
			get
			{
				return _f_SubInfo;
			}
			set
			{
				_f_SubInfo = value;
			}
		}

		public Dictionary<int, BattleShipSubInfo> E_SubInfo
		{
			get
			{
				return _e_SubInfo;
			}
			set
			{
				_e_SubInfo = value;
			}
		}

		public Mem_mapclear Cleard
		{
			get
			{
				return _cleard;
			}
			set
			{
				_cleard = value;
			}
		}

		public Mst_mapcell2 NowCell
		{
			get
			{
				return _nowCell;
			}
			set
			{
				_nowCell = value;
			}
		}

		public bool RebellionBattle
		{
			get
			{
				return _rebellionBattle;
			}
			set
			{
				_rebellionBattle = value;
			}
		}

		public List<MapItemGetFmt> GetAirCellItems
		{
			get
			{
				return getAirCellItems;
			}
			set
			{
				getAirCellItems = value;
			}
		}

		private BattleResultBase()
		{
		}

		public BattleResultBase(IMakeBattleData battleInstance)
			: this()
		{
			battleInstance.GetBattleResultBase(this);
		}
	}
}
