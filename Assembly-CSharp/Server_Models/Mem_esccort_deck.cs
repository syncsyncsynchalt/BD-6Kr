using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_esccort_deck", Namespace = "")]
	public class Mem_esccort_deck : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private string _name;

		[DataMember]
		private int _maparea_id;

		[DataMember]
		private DispositionStatus _disposition_status;

		[DataMember]
		private int _startTime;

		[DataMember]
		private int _completeTime;

		[DataMember]
		private DeckShips _ship;

		private static string _tableName = "mem_esccort_deck";

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

		public string Name
		{
			get
			{
				return _name;
			}
			private set
			{
				_name = value;
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

		public int StartTime
		{
			get
			{
				return _startTime;
			}
			private set
			{
				_startTime = value;
			}
		}

		public int CompleteTime
		{
			get
			{
				return _completeTime;
			}
			private set
			{
				_completeTime = value;
			}
		}

		public DeckShips Ship
		{
			get
			{
				return _ship;
			}
			private set
			{
				_ship = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_esccort_deck()
		{
			Maparea_id = 1;
			Ship = new DeckShips();
		}

		public Mem_esccort_deck(int rid, int area_id)
			: this()
		{
			Rid = rid;
			Disposition_status = DispositionStatus.NONE;
			Maparea_id = area_id;
			Name = string.Empty;
		}

		public void SetDeckName(string name)
		{
			name.TrimEnd(default(char));
			Name = name;
		}

		public void GoArea(int area_id)
		{
			Disposition_status = DispositionStatus.ARRIVAL;
			StartTime = 0;
			CompleteTime = 0;
		}

		public void EscortStop()
		{
			if (Disposition_status != DispositionStatus.NONE)
			{
				Disposition_status = DispositionStatus.NONE;
				StartTime = 0;
				CompleteTime = 0;
			}
		}

		public bool IsBlingDeck()
		{
			return (CompleteTime - StartTime > 0) ? true : false;
		}

		public int GetBlingTurn()
		{
			if (!IsBlingDeck())
			{
				return 0;
			}
			return CompleteTime - Comm_UserDatas.Instance.User_turn.Total_turn;
		}

		public bool BlingTerm()
		{
			if (!IsBlingDeck())
			{
				return false;
			}
			if (CompleteTime > Comm_UserDatas.Instance.User_turn.Total_turn)
			{
				return false;
			}
			StartTime = 0;
			CompleteTime = 0;
			return true;
		}

		protected override void setProperty(XElement element)
		{
			Rid = int.Parse(element.Element("_rid").Value);
			Name = element.Element("_name").Value;
			Maparea_id = int.Parse(element.Element("_maparea_id").Value);
			Disposition_status = (DispositionStatus)(int)Enum.Parse(typeof(DispositionStatus), element.Element("_disposition_status").Value);
			StartTime = int.Parse(element.Element("_startTime").Value);
			CompleteTime = int.Parse(element.Element("_completeTime").Value);
			IEnumerable<XElement> source = element.Elements("_ship").Elements("ships");
			foreach (var item in source.Elements().Select((XElement obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				Ship[item.idx] = int.Parse(item.obj.Value);
			}
		}
	}
}
