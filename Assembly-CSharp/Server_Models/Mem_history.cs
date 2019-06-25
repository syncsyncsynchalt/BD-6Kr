using Server_Common;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_history", Namespace = "")]
	public class Mem_history : Model_Base
	{
		public const int MAPCLEAR_MAX_RECORD = 3;

		[DataMember]
		private int _rid;

		[DataMember]
		private int _type;

		[DataMember]
		private int _map_clear_num;

		[DataMember]
		private int _turn;

		[DataMember]
		private int _mapinfo_id;

		[DataMember]
		private int _flagship_id;

		[DataMember]
		private bool _tanker_lost_all;

		[DataMember]
		private int _game_end_type;

		private static string _tableName = "mem_history";

		public int Rid
		{
			get
			{
				return _rid;
			}
			private set
			{
				_rid = value;
			}
		}

		public int Type
		{
			get
			{
				return _type;
			}
			private set
			{
				_type = value;
			}
		}

		public int MapClearNum
		{
			get
			{
				return _map_clear_num;
			}
			private set
			{
				_map_clear_num = value;
			}
		}

		public int Turn
		{
			get
			{
				return _turn;
			}
			private set
			{
				_turn = value;
			}
		}

		public int MapinfoId
		{
			get
			{
				return _mapinfo_id;
			}
			private set
			{
				_mapinfo_id = value;
			}
		}

		public int FlagShipId
		{
			get
			{
				return _flagship_id;
			}
			private set
			{
				_flagship_id = value;
			}
		}

		public bool TankerLostAll
		{
			get
			{
				return _tanker_lost_all;
			}
			private set
			{
				_tanker_lost_all = value;
			}
		}

		public int GameEndType
		{
			get
			{
				return _game_end_type;
			}
			private set
			{
				_game_end_type = value;
			}
		}

		public static string tableName => _tableName;

		public static int GetMapClearNum(int mapinfo_id)
		{
			List<Mem_history> value = null;
			if (!Comm_UserDatas.Instance.User_history.TryGetValue(1, out value))
			{
				return 1;
			}
			int num = value.Count((Mem_history x) => x.MapinfoId == mapinfo_id);
			return num + 1;
		}

		public static bool IsFirstOpenArea(int mapinfo_id)
		{
			if (Mst_DataManager.Instance.Mst_mapinfo[mapinfo_id].No != 1)
			{
				return false;
			}
			List<Mem_history> value = null;
			if (!Comm_UserDatas.Instance.User_history.TryGetValue(3, out value))
			{
				return true;
			}
			return (!value.Any((Mem_history x) => x.MapinfoId == mapinfo_id)) ? true : false;
		}

		public void SetMapClear(int turn, int mapinfo_id, int clearNum, int flagship_id)
		{
			setNewRid();
			Type = 1;
			Turn = turn;
			MapClearNum = clearNum;
			MapinfoId = mapinfo_id;
			FlagShipId = flagship_id;
		}

		public void SetTanker(int turn, int mapinfo_id, bool tanker_destroy)
		{
			setNewRid();
			Type = 2;
			Turn = turn;
			MapinfoId = mapinfo_id;
			TankerLostAll = tanker_destroy;
		}

		public void SetAreaOpen(int turn, int mapinfo_id)
		{
			setNewRid();
			Type = 3;
			Turn = turn;
			MapinfoId = mapinfo_id;
		}

		public void SetGameClear(int turn)
		{
			setNewRid();
			Type = 4;
			Turn = turn;
			GameEndType = 1;
		}

		public void SetGameOverToLost(int turn)
		{
			setNewRid();
			Type = 4;
			Turn = turn;
			GameEndType = 2;
		}

		public void SetGameOverToTurn(int turn)
		{
			setNewRid();
			Type = 4;
			Turn = turn;
			GameEndType = 3;
		}

		public void SetAreaComplete(int mapinfo)
		{
			setNewRid();
			Type = 999;
			MapinfoId = mapinfo;
		}

		private void setNewRid()
		{
			int num = 0;
			foreach (List<Mem_history> value in Comm_UserDatas.Instance.User_history.Values)
			{
				num += value.Count;
			}
			Rid = num + 1;
		}

		protected override void setProperty(XElement element)
		{
			Rid = int.Parse(element.Element("_rid").Value);
			Type = int.Parse(element.Element("_type").Value);
			MapClearNum = int.Parse(element.Element("_map_clear_num").Value);
			Turn = int.Parse(element.Element("_turn").Value);
			MapinfoId = int.Parse(element.Element("_mapinfo_id").Value);
			MapClearNum = int.Parse(element.Element("_map_clear_num").Value);
			FlagShipId = int.Parse(element.Element("_flagship_id").Value);
			TankerLostAll = bool.Parse(element.Element("_tanker_lost_all").Value);
			GameEndType = int.Parse(element.Element("_game_end_type").Value);
		}
	}
}
