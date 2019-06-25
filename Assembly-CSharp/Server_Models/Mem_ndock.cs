using Common.Enum;
using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_ndock", Namespace = "")]
	public class Mem_ndock : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private int _area_id;

		[DataMember]
		private int _dock_no;

		[DataMember]
		private NdockStates _state;

		[DataMember]
		private int _ship_id;

		[DataMember]
		private int _startTime;

		[DataMember]
		private int _completeTime;

		[DataMember]
		private int _item1;

		[DataMember]
		private int _item2;

		[DataMember]
		private int _item3;

		[DataMember]
		private int _item4;

		private static string _tableName = "mem_ndock";

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

		public int Area_id
		{
			get
			{
				return _area_id;
			}
			private set
			{
				_area_id = value;
			}
		}

		public int Dock_no
		{
			get
			{
				return _dock_no;
			}
			private set
			{
				_dock_no = value;
			}
		}

		public NdockStates State
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

		public int Ship_id
		{
			get
			{
				return _ship_id;
			}
			private set
			{
				_ship_id = value;
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

		public int Item1
		{
			get
			{
				return _item1;
			}
			private set
			{
				_item1 = value;
			}
		}

		public int Item2
		{
			get
			{
				return _item2;
			}
			private set
			{
				_item2 = value;
			}
		}

		public int Item3
		{
			get
			{
				return _item3;
			}
			private set
			{
				_item3 = value;
			}
		}

		public int Item4
		{
			get
			{
				return _item4;
			}
			private set
			{
				_item4 = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_ndock()
		{
		}

		public Mem_ndock(int rid, int area_id, int no)
		{
			Rid = rid;
			Area_id = area_id;
			Dock_no = no;
			State = NdockStates.EMPTY;
			StartTime = 0;
			CompleteTime = 0;
			Item1 = 0;
			Item2 = 0;
			Item3 = 0;
			Item4 = 0;
		}

		public void RecoverStart(int ship_rid, Dictionary<enumMaterialCategory, int> material, int span)
		{
			Ship_id = ship_rid;
			Item1 = material[enumMaterialCategory.Fuel];
			Item3 = material[enumMaterialCategory.Steel];
			State = NdockStates.RESTORE;
			StartTime = Comm_UserDatas.Instance.User_turn.Total_turn;
			CompleteTime = Comm_UserDatas.Instance.User_turn.Total_turn + span;
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Fuel].Sub_Material(Item1);
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Steel].Sub_Material(Item3);
		}

		public bool IsRecoverEndTime()
		{
			return (CompleteTime <= Comm_UserDatas.Instance.User_turn.Total_turn) ? true : false;
		}

		public bool RecoverEnd(bool timeChk)
		{
			if (timeChk && !IsRecoverEndTime())
			{
				return false;
			}
			if (State != NdockStates.RESTORE)
			{
				return false;
			}
			Mem_ship ship = Comm_UserDatas.Instance.User_ship[Ship_id];
			ship.NdockRecovery(this);
			Ship_id = 0;
			Item1 = 0;
			Item3 = 0;
			State = NdockStates.EMPTY;
			StartTime = 0;
			CompleteTime = 0;
			if (!Comm_UserDatas.Instance.User_deck.Values.Any((Mem_deck x) => x.Ship.Find(ship.Rid) != -1))
			{
				if (timeChk)
				{
					ship.BlingSet(Area_id);
				}
				else
				{
					ship.BlingWait(Area_id, Mem_ship.BlingKind.WaitDeck);
				}
			}
			return true;
		}

		public int GetRequireTime()
		{
			int num = CompleteTime - Comm_UserDatas.Instance.User_turn.Total_turn;
			return (num >= 0) ? num : 0;
		}

		protected override void setProperty(XElement element)
		{
			Rid = int.Parse(element.Element("_rid").Value);
			Area_id = int.Parse(element.Element("_area_id").Value);
			Dock_no = int.Parse(element.Element("_dock_no").Value);
			State = (NdockStates)(int)Enum.Parse(typeof(NdockStates), element.Element("_state").Value);
			Ship_id = int.Parse(element.Element("_ship_id").Value);
			StartTime = int.Parse(element.Element("_startTime").Value);
			CompleteTime = int.Parse(element.Element("_completeTime").Value);
			Item1 = int.Parse(element.Element("_item1").Value);
			Item2 = int.Parse(element.Element("_item2").Value);
			Item3 = int.Parse(element.Element("_item3").Value);
			Item4 = int.Parse(element.Element("_item4").Value);
		}
	}
}
