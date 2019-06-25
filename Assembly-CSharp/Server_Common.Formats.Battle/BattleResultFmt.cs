using Common.Enum;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	public class BattleResultFmt
	{
		private BattleWinRankKinds _winRank;

		private int _basicLevel;

		private string _questName;

		private int _mvpShip;

		private int _getBaseExp;

		[XmlIgnore]
		private SerializableDictionary<int, int> _getShipExp;

		[XmlIgnore]
		private SerializableDictionary<int, List<int>> _levelUpInfo;

		private List<int> _enemyId;

		private string _enemyName;

		private bool _firstClear;

		private bool _firstAreaComplete;

		private int _getSpoint;

		private List<MapItemGetFmt> _getAirReconnaissanceItems;

		private List<ItemGetFmt> _getItem;

		public ItemGetFmt AreaClearRewardItem;

		private List<ItemGetFmt> _getEventItem;

		private ExMapRewardInfo _exMapReward;

		private EscapeInfo _escapeInfo;

		private List<int> _newOpenMapId;

		private List<int> _reOpenMapId;

		public BattleWinRankKinds WinRank
		{
			get
			{
				return _winRank;
			}
			set
			{
				_winRank = value;
			}
		}

		public int BasicLevel
		{
			get
			{
				return _basicLevel;
			}
			set
			{
				_basicLevel = value;
			}
		}

		public string QuestName
		{
			get
			{
				return _questName;
			}
			set
			{
				_questName = value;
			}
		}

		public int MvpShip
		{
			get
			{
				return _mvpShip;
			}
			set
			{
				_mvpShip = value;
			}
		}

		public int GetBaseExp
		{
			get
			{
				return _getBaseExp;
			}
			set
			{
				_getBaseExp = value;
			}
		}

		[XmlIgnore]
		public SerializableDictionary<int, int> GetShipExp
		{
			get
			{
				return _getShipExp;
			}
			set
			{
				_getShipExp = value;
			}
		}

		[XmlIgnore]
		public SerializableDictionary<int, List<int>> LevelUpInfo
		{
			get
			{
				return _levelUpInfo;
			}
			set
			{
				_levelUpInfo = value;
			}
		}

		public List<int> EnemyId
		{
			get
			{
				return _enemyId;
			}
			set
			{
				_enemyId = value;
			}
		}

		public string EnemyName
		{
			get
			{
				return _enemyName;
			}
			set
			{
				_enemyName = value;
			}
		}

		public bool FirstClear
		{
			get
			{
				return _firstClear;
			}
			set
			{
				_firstClear = value;
			}
		}

		public bool FirstAreaComplete
		{
			get
			{
				return _firstAreaComplete;
			}
			set
			{
				_firstAreaComplete = value;
			}
		}

		public int GetSpoint
		{
			get
			{
				return _getSpoint;
			}
			set
			{
				_getSpoint = value;
			}
		}

		public List<MapItemGetFmt> GetAirReconnaissanceItems
		{
			get
			{
				return _getAirReconnaissanceItems;
			}
			set
			{
				_getAirReconnaissanceItems = value;
			}
		}

		public List<ItemGetFmt> GetItem
		{
			get
			{
				return _getItem;
			}
			set
			{
				_getItem = value;
			}
		}

		public List<ItemGetFmt> GetEventItem
		{
			get
			{
				return _getEventItem;
			}
			set
			{
				_getEventItem = value;
			}
		}

		public ExMapRewardInfo ExMapReward
		{
			get
			{
				return _exMapReward;
			}
			set
			{
				_exMapReward = value;
			}
		}

		public EscapeInfo EscapeInfo
		{
			get
			{
				return _escapeInfo;
			}
			set
			{
				_escapeInfo = value;
			}
		}

		public List<int> NewOpenMapId
		{
			get
			{
				return _newOpenMapId;
			}
			set
			{
				_newOpenMapId = value;
			}
		}

		public List<int> ReOpenMapId
		{
			get
			{
				return _reOpenMapId;
			}
			set
			{
				_reOpenMapId = value;
			}
		}

		public BattleResultFmt()
		{
			EnemyId = new List<int>();
			NewOpenMapId = new List<int>();
			ReOpenMapId = new List<int>();
		}
	}
}
