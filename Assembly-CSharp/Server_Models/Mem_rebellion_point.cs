using Common.Enum;
using Server_Common;
using Server_Controllers;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_rebellion_point", Namespace = "")]
	public class Mem_rebellion_point : Model_Base
	{
		private const int POINT_MAPOPEN = 40;

		private const int POINT_MAX = 200;

		private const int POINT_MIN = 0;

		[DataMember]
		private int _rid;

		[DataMember]
		private int _point;

		[DataMember]
		private RebellionState _state;

		private static string _tableName = "mem_rebellion_point";

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

		public int Point
		{
			get
			{
				return _point;
			}
			private set
			{
				_point = value;
			}
		}

		public RebellionState State
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

		public static string tableName => _tableName;

		public Mem_rebellion_point()
		{
		}

		public Mem_rebellion_point(int maparea_id)
		{
			Rid = maparea_id;
			Point = 40;
			ChangeState();
		}

		public void AddPoint(IRebellionPointOperator instance, int addNum)
		{
			if (instance != null && addNum >= 0 && State != RebellionState.Invation)
			{
				Point += addNum;
				if (Point >= 200)
				{
					Point = 200;
				}
				ChangeState();
			}
		}

		public void SubPoint(IRebellionPointOperator instance, int subNum)
		{
			if (instance != null && subNum >= 0 && State != RebellionState.Invation)
			{
				Point -= subNum;
				if (Point < 0)
				{
					Point = 0;
				}
				ChangeState();
			}
		}

		public bool StartRebellion(Api_TurnOperator instance)
		{
			if (instance == null)
			{
				return false;
			}
			if (State != RebellionState.Alert)
			{
				return false;
			}
			double randDouble = Utils.GetRandDouble(0.0, 100.0, 1.0, 1);
			if (randDouble <= 65.0)
			{
				State = RebellionState.Invation;
				return true;
			}
			return false;
		}

		public int EndInvation(IRebellionPointOperator instance)
		{
			if (instance == null)
			{
				return -1;
			}
			if (State != RebellionState.Invation)
			{
				return -2;
			}
			Point = 0;
			State = RebellionState.Safety;
			return 0;
		}

		private void ChangeState()
		{
			if (State != RebellionState.Invation)
			{
				if (Point >= 100)
				{
					State = RebellionState.Alert;
				}
				else if (Point > 90)
				{
					State = RebellionState.Warning;
				}
				else if (Point > 70)
				{
					State = RebellionState.Caution;
				}
				else if (Point > 50)
				{
					State = RebellionState.Attention;
				}
				else
				{
					State = RebellionState.Safety;
				}
			}
		}

		protected override void setProperty(XElement element)
		{
			Rid = int.Parse(element.Element("_rid").Value);
			Point = int.Parse(element.Element("_point").Value);
			State = (RebellionState)(int)Enum.Parse(typeof(RebellionState), element.Element("_state").Value);
		}
	}
}
