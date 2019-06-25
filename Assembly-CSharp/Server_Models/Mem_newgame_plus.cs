using Common.Enum;
using Server_Common;
using Server_Controllers.BattleLogic;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_newgame_plus", Namespace = "")]
	public class Mem_newgame_plus : Model_Base
	{
		[DataMember]
		private List<Mem_shipBase> _ship;

		[DataMember]
		private List<Mem_slotitem> _slotitem;

		[DataMember]
		private List<Mem_furniture> _furniture;

		[DataMember]
		private List<Mem_book> _ship_book;

		[DataMember]
		private List<Mem_book> _slot_book;

		[DataMember]
		private int _fleetLevel;

		[DataMember]
		private uint _fleetExp;

		[DataMember]
		private List<int> _clear;

		private int tempRewardShipRid;

		private static string _tableName = "mem_newgame_plus";

		public List<Mem_shipBase> Ship
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

		public List<Mem_slotitem> Slotitem
		{
			get
			{
				return _slotitem;
			}
			private set
			{
				_slotitem = value;
			}
		}

		public List<Mem_furniture> Furniture
		{
			get
			{
				return _furniture;
			}
			private set
			{
				_furniture = value;
			}
		}

		public List<Mem_book> Ship_book
		{
			get
			{
				return _ship_book;
			}
			private set
			{
				_ship_book = value;
			}
		}

		public List<Mem_book> Slot_book
		{
			get
			{
				return _slot_book;
			}
			private set
			{
				_slot_book = value;
			}
		}

		public int FleetLevel
		{
			get
			{
				return _fleetLevel;
			}
			private set
			{
				_fleetLevel = value;
			}
		}

		public uint FleetExp
		{
			get
			{
				return _fleetExp;
			}
			private set
			{
				_fleetExp = value;
			}
		}

		private int this[DifficultKind kind]
		{
			get
			{
				return _clear[(int)(kind - 1)];
			}
			set
			{
				_clear[(int)(kind - 1)] = value;
			}
		}

		public int TempRewardShipRid
		{
			get
			{
				return tempRewardShipRid;
			}
			private set
			{
				tempRewardShipRid = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_newgame_plus()
		{
			TempRewardShipRid = 0;
			PurgeData();
		}

		public int ClearNum(DifficultKind kind)
		{
			return this[kind];
		}

		public void SetRewardShipRid(Exec_BattleResult instance, int shipRid)
		{
			if (instance != null && TempRewardShipRid == 0)
			{
				TempRewardShipRid = shipRid;
			}
		}

		public void SetData(List<Mem_shipBase> shipBase, List<Mem_slotitem> slotItems)
		{
			FleetLevel = Comm_UserDatas.Instance.User_record.Level;
			FleetExp = Comm_UserDatas.Instance.User_record.Exp;
			shipBase.Sort((Mem_shipBase x, Mem_shipBase y) => x.GetNo.CompareTo(y.GetNo));
			slotItems.Sort((Mem_slotitem x, Mem_slotitem y) => x.GetNo.CompareTo(y.GetNo));
			for (int i = 0; i < shipBase.Count; i++)
			{
				shipBase[i].GetNo = i + 1;
			}
			for (int j = 0; j < slotItems.Count; j++)
			{
				slotItems[j].ChangeSortNo(j + 1);
			}
			Ship.Clear();
			Ship = shipBase;
			Slotitem.Clear();
			Slotitem = slotItems;
			Furniture.Clear();
			Furniture.AddRange(Comm_UserDatas.Instance.User_furniture.Values);
			Ship_book.Clear();
			Ship_book.AddRange(Comm_UserDatas.Instance.Ship_book.Values);
			Slot_book.Clear();
			Slot_book.AddRange(Comm_UserDatas.Instance.Slot_book.Values);
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			Mem_newgame_plus mem_newgame_plus;
			Mem_newgame_plus mem_newgame_plus2 = mem_newgame_plus = this;
			DifficultKind kind;
			DifficultKind kind2 = kind = difficult;
			int num = mem_newgame_plus[kind];
			mem_newgame_plus2[kind2] = num + 1;
			if (this[difficult] > 999)
			{
				this[difficult] = 999;
			}
		}

		public int GetLapNum()
		{
			return _clear.Sum();
		}

		public void PurgeData()
		{
			Ship = new List<Mem_shipBase>();
			Slotitem = new List<Mem_slotitem>();
			Furniture = new List<Mem_furniture>();
			Ship_book = new List<Mem_book>();
			Slot_book = new List<Mem_book>();
			_clear = new List<int>
			{
				0,
				0,
				0,
				0,
				0
			};
			FleetLevel = 0;
			FleetExp = 0u;
		}

		protected override void setProperty(XElement element)
		{
			foreach (XElement item in element.Element("_ship").Elements())
			{
				Mem_shipBase mem_shipBase = new Mem_shipBase();
				mem_shipBase.setProperty(item);
				Ship.Add(mem_shipBase);
			}
			foreach (XElement item2 in element.Element("_slotitem").Elements())
			{
				Slotitem.Add(Model_Base.SetUserData<Mem_slotitem>(item2));
			}
			foreach (XElement item3 in element.Element("_furniture").Elements())
			{
				Furniture.Add(Model_Base.SetUserData<Mem_furniture>(item3));
			}
			foreach (XElement item4 in element.Element("_ship_book").Elements())
			{
				Ship_book.Add(Model_Base.SetUserData<Mem_book>(item4));
			}
			foreach (XElement item5 in element.Element("_slot_book").Elements())
			{
				Slot_book.Add(Model_Base.SetUserData<Mem_book>(item5));
			}
			foreach (var item6 in element.Element("_clear").Elements().Select((XElement obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				_clear[item6.idx] = int.Parse(item6.obj.Value);
			}
			FleetLevel = int.Parse(element.Element("_fleetLevel").Value);
			FleetExp = uint.Parse(element.Element("_fleetExp").Value);
		}
	}
}
