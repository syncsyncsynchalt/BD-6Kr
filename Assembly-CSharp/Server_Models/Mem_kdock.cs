using Common.Enum;
using Server_Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_kdock", Namespace = "")]
	public class Mem_kdock : Model_Base
	{
		private const int LARGE_FUEL_VALUE = 1000;

		[DataMember]
		private int _rid;

		[DataMember]
		private KdockStates _state;

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

		[DataMember]
		private int _item5;

		[DataMember]
		private int _strategy_point;

		[DataMember]
		private int _tunker_num;

		private static string _tableName = "mem_kdock";

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

		public KdockStates State
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

		public int Item5
		{
			get
			{
				return _item5;
			}
			private set
			{
				_item5 = value;
			}
		}

		public int Strategy_point
		{
			get
			{
				return _strategy_point;
			}
			private set
			{
				_strategy_point = value;
			}
		}

		public int Tunker_num
		{
			get
			{
				return _tunker_num;
			}
			private set
			{
				_tunker_num = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_kdock()
		{
		}

		public Mem_kdock(int rid)
		{
			Rid = rid;
			State = KdockStates.EMPTY;
			StartTime = 0;
			CompleteTime = 0;
			Ship_id = 0;
			Item1 = 0;
			Item2 = 0;
			Item3 = 0;
			Item4 = 0;
			Item5 = 0;
			Strategy_point = 0;
			Tunker_num = 0;
		}

		public void CreateStart(int ship_id, Dictionary<enumMaterialCategory, int> material, TimeSpan span)
		{
			Ship_id = ship_id;
			Item1 = material[enumMaterialCategory.Fuel];
			Item2 = material[enumMaterialCategory.Bull];
			Item3 = material[enumMaterialCategory.Steel];
			Item4 = material[enumMaterialCategory.Bauxite];
			Item5 = material[enumMaterialCategory.Dev_Kit];
			State = KdockStates.CREATE;
			StartTime = Comm_UserDatas.Instance.User_turn.Total_turn;
			CompleteTime = Comm_UserDatas.Instance.User_turn.Total_turn + (int)span.TotalMinutes;
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Fuel].Sub_Material(Item1);
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Bull].Sub_Material(Item2);
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Steel].Sub_Material(Item3);
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Bauxite].Sub_Material(Item4);
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Dev_Kit].Sub_Material(Item5);
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Build_Kit].Sub_Material(material[enumMaterialCategory.Build_Kit]);
		}

		public void CreateTunker(int createNum, Dictionary<enumMaterialCategory, int> material, int usePoint, int createTurn)
		{
			Item1 = material[enumMaterialCategory.Fuel];
			Item2 = material[enumMaterialCategory.Bull];
			Item3 = material[enumMaterialCategory.Steel];
			Item4 = material[enumMaterialCategory.Bauxite];
			Strategy_point = usePoint;
			Tunker_num = createNum;
			State = KdockStates.CREATE;
			StartTime = Comm_UserDatas.Instance.User_turn.Total_turn;
			CompleteTime = Comm_UserDatas.Instance.User_turn.Total_turn + createTurn;
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Fuel].Sub_Material(Item1);
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Bull].Sub_Material(Item2);
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Steel].Sub_Material(Item3);
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Bauxite].Sub_Material(Item4);
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Build_Kit].Sub_Material(material[enumMaterialCategory.Build_Kit]);
			Comm_UserDatas.Instance.User_basic.SubPoint(usePoint);
		}

		public bool CreateEnd(bool timeChk)
		{
			if (timeChk && CompleteTime > Comm_UserDatas.Instance.User_turn.Total_turn)
			{
				return false;
			}
			if (State != KdockStates.CREATE)
			{
				return false;
			}
			State = KdockStates.COMPLETE;
			StartTime = 0;
			CompleteTime = 0;
			return true;
		}

		public bool GetShip()
		{
			Comm_UserDatas.Instance.Add_Ship(new List<int>
			{
				Ship_id
			});
			Ship_id = 0;
			Item1 = 0;
			Item2 = 0;
			Item3 = 0;
			Item4 = 0;
			Item5 = 0;
			State = KdockStates.EMPTY;
			return true;
		}

		public bool GetTunker()
		{
			Comm_UserDatas.Instance.Add_Tanker(Tunker_num);
			Item1 = 0;
			Item2 = 0;
			Item3 = 0;
			Item4 = 0;
			State = KdockStates.EMPTY;
			Strategy_point = 0;
			Tunker_num = 0;
			return true;
		}

		public bool IsLargeDock()
		{
			return (Item1 >= 1000) ? true : false;
		}

		public bool IsTunkerDock()
		{
			return (Strategy_point > 0) ? true : false;
		}

		public int GetRequireCreateTime()
		{
			return CompleteTime - Comm_UserDatas.Instance.User_turn.Total_turn;
		}

		protected override void setProperty(XElement element)
		{
			Rid = int.Parse(element.Element("_rid").Value);
			State = (KdockStates)(int)Enum.Parse(typeof(KdockStates), element.Element("_state").Value);
			Ship_id = int.Parse(element.Element("_ship_id").Value);
			StartTime = int.Parse(element.Element("_startTime").Value);
			CompleteTime = int.Parse(element.Element("_completeTime").Value);
			Item1 = int.Parse(element.Element("_item1").Value);
			Item2 = int.Parse(element.Element("_item2").Value);
			Item3 = int.Parse(element.Element("_item3").Value);
			Item4 = int.Parse(element.Element("_item4").Value);
			Item5 = int.Parse(element.Element("_item5").Value);
			Strategy_point = int.Parse(element.Element("_strategy_point").Value);
			Tunker_num = int.Parse(element.Element("_tunker_num").Value);
		}
	}
}
