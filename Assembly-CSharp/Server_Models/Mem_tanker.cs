using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_tanker", Namespace = "")]
	public class Mem_tanker : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private int _maparea_id;

		[DataMember]
		private DispositionStatus _disposition_status;

		[DataMember]
		private int _mission_deck_rid;

		[DataMember]
		private int _bling_start;

		[DataMember]
		private int _bling_end;

		private static string _tableName = "mem_tanker";

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

		public int Maparea_id
		{
			get
			{
				return _maparea_id;
			}
			private set
			{
				_maparea_id = value;
			}
		}

		public DispositionStatus Disposition_status
		{
			get
			{
				return _disposition_status;
			}
			private set
			{
				_disposition_status = value;
			}
		}

		public int Mission_deck_rid
		{
			get
			{
				return _mission_deck_rid;
			}
			private set
			{
				_mission_deck_rid = value;
			}
		}

		public int Bling_start
		{
			get
			{
				return _bling_start;
			}
			private set
			{
				_bling_start = value;
			}
		}

		public int Bling_end
		{
			get
			{
				return _bling_end;
			}
			private set
			{
				_bling_end = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_tanker()
		{
			Maparea_id = 1;
			Disposition_status = DispositionStatus.NONE;
			Bling_start = 0;
			Bling_end = 0;
		}

		public Mem_tanker(int rid)
			: this()
		{
			Rid = rid;
		}

		public void GoArea(int area_id)
		{
			Maparea_id = area_id;
			Disposition_status = DispositionStatus.ARRIVAL;
			Bling_start = 0;
			Bling_end = 0;
		}

		public void BackTanker()
		{
			if (Disposition_status != DispositionStatus.NONE || IsBlingShip())
			{
				int maparea_id = Maparea_id;
				Maparea_id = 1;
				Disposition_status = DispositionStatus.NONE;
				Bling_start = Comm_UserDatas.Instance.User_turn.Total_turn;
				Bling_end = Comm_UserDatas.Instance.User_turn.Total_turn + Mst_DataManager.Instance.Mst_maparea[maparea_id].Distance;
			}
		}

		public bool IsBlingShip()
		{
			return (Bling_end - Bling_start > 0) ? true : false;
		}

		public int GetBlingTurn()
		{
			if (!IsBlingShip())
			{
				return 0;
			}
			return Bling_end - Comm_UserDatas.Instance.User_turn.Total_turn;
		}

		public bool BlingTerm()
		{
			if (!IsBlingShip())
			{
				return false;
			}
			if (Bling_end > Comm_UserDatas.Instance.User_turn.Total_turn)
			{
				return false;
			}
			Bling_start = 0;
			Bling_end = 0;
			return true;
		}

		public bool MissionStart(int area_id, int deck_id)
		{
			if (Disposition_status != DispositionStatus.NONE)
			{
				return false;
			}
			Disposition_status = DispositionStatus.MISSION;
			Mission_deck_rid = deck_id;
			return true;
		}

		public bool MissionTerm()
		{
			if (Disposition_status != DispositionStatus.MISSION)
			{
				return false;
			}
			Disposition_status = DispositionStatus.NONE;
			Mission_deck_rid = 0;
			return true;
		}

		public static IEnumerable<IGrouping<int, Mem_tanker>> GetAreaEnableTanker(Dictionary<int, Mem_tanker> tankerItems)
		{
			return from item in tankerItems.Values
				where item.Disposition_status == DispositionStatus.ARRIVAL && !item.IsBlingShip()
				group item by item.Maparea_id;
		}

		public static List<Mem_tanker> GetAreaTanker(int mapAreaId)
		{
			Dictionary<int, Mem_tanker>.ValueCollection values = Comm_UserDatas.Instance.User_tanker.Values;
			return (from x in values
				where x.Maparea_id == mapAreaId
				select x).ToList();
		}

		public static List<Mem_tanker> GetFreeTanker(Dictionary<int, Mem_tanker> tankerItems)
		{
			return (from x in tankerItems.Values
				where x.Disposition_status == DispositionStatus.NONE && x.Maparea_id == 1 && !x.IsBlingShip()
				select x).ToList();
		}

		protected override void setProperty(XElement element)
		{
			Rid = int.Parse(element.Element("_rid").Value);
			Maparea_id = int.Parse(element.Element("_maparea_id").Value);
			Disposition_status = (DispositionStatus)(int)Enum.Parse(typeof(DispositionStatus), element.Element("_disposition_status").Value);
			Bling_start = int.Parse(element.Element("_bling_start").Value);
			Bling_end = int.Parse(element.Element("_bling_end").Value);
			Mission_deck_rid = int.Parse(element.Element("_mission_deck_rid").Value);
		}
	}
}
