using Server_Common;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_mapclear", Namespace = "")]
	public class Mem_mapclear : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private int _maparea_id;

		[DataMember]
		private int _mapinfo_no;

		[DataMember]
		private MapClearState _state;

		[DataMember]
		private bool _cleared;

		private static string _tableName = "mem_mapclear";

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

		public int Mapinfo_no
		{
			get
			{
				return _mapinfo_no;
			}
			private set
			{
				_mapinfo_no = value;
			}
		}

		public MapClearState State
		{
			get
			{
				return _state;
			}
			private set
			{
				_state = value;
			}
		}

		public bool Cleared
		{
			get
			{
				return _cleared;
			}
			private set
			{
				_cleared = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_mapclear()
		{
			Cleared = false;
		}

		public Mem_mapclear(int mapinfo_id, int maparea_id, int mapinfo_no, MapClearState state)
			: this()
		{
			Rid = mapinfo_id;
			Maparea_id = maparea_id;
			Mapinfo_no = mapinfo_no;
			State = state;
			if (state == MapClearState.Cleard)
			{
				Cleared = true;
			}
		}

		public void Insert()
		{
			if (!Comm_UserDatas.Instance.User_mapclear.ContainsKey(Rid))
			{
				Comm_UserDatas.Instance.User_mapclear.Add(Rid, this);
			}
		}

		public void StateChange(MapClearState state)
		{
			State = state;
			if (state == MapClearState.Cleard)
			{
				Cleared = true;
			}
		}

		protected override void setProperty(XElement element)
		{
			Rid = int.Parse(element.Element("_rid").Value);
			Maparea_id = int.Parse(element.Element("_maparea_id").Value);
			Mapinfo_no = int.Parse(element.Element("_mapinfo_no").Value);
			State = (MapClearState)(int)Enum.Parse(typeof(MapClearState), element.Element("_state").Value);
			Cleared = bool.Parse(element.Element("_cleared").Value);
		}
	}
}
