using Common.Enum;
using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_deck", Namespace = "")]
	public class Mem_deck : Model_Base
	{
		[DataContract(Namespace = "")]
		public enum SupportKinds
		{
			[EnumMember]
			NONE,
			[EnumMember]
			WAIT,
			[EnumMember]
			SUPPORTED
		}

		[DataMember]
		private int _rid;

		[DataMember]
		private int _area_id;

		[DataMember]
		private string _name;

		[DataMember]
		private int _mission_id;

		[DataMember]
		private MissionStates _missionState;

		[DataMember]
		private SupportKinds _supportKind;

		[DataMember]
		private int _startTime;

		[DataMember]
		private int _completeTime;

		[DataMember]
		private DeckShips _ship;

		[DataMember]
		private bool _isActionEnd;

		private static string _tableName = "mem_deck";

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

		public int Mission_id
		{
			get
			{
				return _mission_id;
			}
			private set
			{
				_mission_id = value;
			}
		}

		public MissionStates MissionState
		{
			get
			{
				return _missionState;
			}
			private set
			{
				_missionState = value;
			}
		}

		public SupportKinds SupportKind
		{
			get
			{
				return _supportKind;
			}
			private set
			{
				_supportKind = value;
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

		public bool IsActionEnd
		{
			get
			{
				return _isActionEnd;
			}
			private set
			{
				_isActionEnd = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_deck()
		{
			Area_id = 1;
			Mission_id = 0;
			MissionState = MissionStates.NONE;
			StartTime = 0;
			CompleteTime = 0;
			Ship = new DeckShips();
			IsActionEnd = false;
		}

		public Mem_deck(int rid)
			: this()
		{
			Rid = rid;
			Name = $"第{Rid}艦隊";
		}

		public int[] Search_ShipIdx(Dictionary<int, Mem_deck> target_decks, int ship_id)
		{
			int[] array = new int[2]
			{
				-1,
				-1
			};
			foreach (KeyValuePair<int, Mem_deck> target_deck in target_decks)
			{
				array[1] = target_deck.Value.Ship.Find(ship_id);
				if (array[1] != -1)
				{
					array[0] = target_deck.Key;
					return array;
				}
			}
			return array;
		}

		public int[] Search_ShipIdx(int ship_id)
		{
			int[] array = new int[2]
			{
				-1,
				-1
			};
			foreach (KeyValuePair<int, Mem_deck> item in Comm_UserDatas.Instance.User_deck)
			{
				array[1] = item.Value.Ship.Find(ship_id);
				if (array[1] != -1)
				{
					array[0] = item.Key;
					return array;
				}
			}
			return array;
		}

		public bool Contains_Yomi(int mst_id)
		{
			Mst_ship value = null;
			if (!Mst_DataManager.Instance.Mst_ship.TryGetValue(mst_id, out value))
			{
				return true;
			}
			string yomi = value.Yomi;
			for (int i = 0; i < Ship.Count(); i++)
			{
				Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[Ship[i]];
				string yomi2 = Mst_DataManager.Instance.Mst_ship[mem_ship.Ship_id].Yomi;
				if (yomi.Equals(yomi2))
				{
					return true;
				}
			}
			return false;
		}

		public void SetDeckName(string name)
		{
			name.TrimEnd(default(char));
			Name = name;
		}

		public bool IsMissionComplete()
		{
			if (MissionState == MissionStates.END)
			{
				return true;
			}
			if (MissionState == MissionStates.STOP && CompleteTime >= Comm_UserDatas.Instance.User_turn.Total_turn)
			{
				return true;
			}
			return false;
		}

		public bool MissionStart(Mst_mission2 mst_mission)
		{
			if (mst_mission == null || MissionState != 0)
			{
				return false;
			}
			Mission_id = mst_mission.Id;
			StartTime = Comm_UserDatas.Instance.User_turn.Total_turn;
			CompleteTime = Comm_UserDatas.Instance.User_turn.Total_turn + mst_mission.Time;
			MissionState = MissionStates.RUNNING;
			SupportKind = (mst_mission.IsSupportMission() ? SupportKinds.WAIT : SupportKinds.NONE);
			return true;
		}

		public bool MissionEnd()
		{
			if (MissionState != MissionStates.RUNNING && MissionState != MissionStates.STOP)
			{
				return false;
			}
			if (CompleteTime > Comm_UserDatas.Instance.User_turn.Total_turn)
			{
				return false;
			}
			if (SupportKind != 0 && MissionState != MissionStates.STOP)
			{
				UpdateSupportShip();
				MissionState = MissionStates.END;
				MissionInit();
			}
			else if (SupportKind == SupportKinds.NONE)
			{
				if (MissionState != MissionStates.STOP)
				{
					MissionState = MissionStates.END;
				}
				StartTime = 0;
				CompleteTime = 0;
			}
			return true;
		}

		public bool MissionStop(int newEndTime)
		{
			if (MissionState != MissionStates.RUNNING)
			{
				return false;
			}
			if (CompleteTime < newEndTime)
			{
				return false;
			}
			MissionState = MissionStates.STOP;
			CompleteTime = newEndTime;
			return true;
		}

		public bool MissionEnforceEnd()
		{
			if (MissionState == MissionStates.NONE)
			{
				return false;
			}
			if (SupportKind != 0)
			{
				CompleteTime = Comm_UserDatas.Instance.User_turn.Total_turn;
				return MissionEnd();
			}
			CompleteTime = Comm_UserDatas.Instance.User_turn.Total_turn;
			MissionState = MissionStates.END;
			return MissionInit();
		}

		public bool MissionInit()
		{
			if (MissionState != MissionStates.END && MissionState != MissionStates.STOP)
			{
				return false;
			}
			Mission_id = 0;
			MissionState = MissionStates.NONE;
			SupportKind = SupportKinds.NONE;
			return true;
		}

		public void ChangeSupported()
		{
			if (SupportKind == SupportKinds.WAIT)
			{
				SupportKind = SupportKinds.SUPPORTED;
			}
		}

		private void UpdateSupportShip()
		{
			if (SupportKind != SupportKinds.SUPPORTED)
			{
				return;
			}
			Mst_mission2 mst_mission = Mst_DataManager.Instance.Mst_mission[Mission_id];
			int maxValue = (mst_mission.Mission_type != MissionType.SupportForward) ? 10 : 5;
			Random random = new Random();
			int subCond = random.Next(maxValue) + 1;
			double subFuel = mst_mission.Use_fuel;
			double subBull = mst_mission.Use_bull;
			int num = Ship.Count();
			int num2 = 0;
			List<Mem_ship> list = new List<Mem_ship>();
			for (int i = 0; i < num; i++)
			{
				if (Ship[i] > 0)
				{
					Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[Ship[i]];
					list.Add(mem_ship);
					if (Mst_DataManager.Instance.Mst_stype[mem_ship.Stype].IsMother())
					{
						num2++;
					}
				}
			}
			if (num2 >= 3)
			{
				subBull *= 0.5;
			}
			list.ForEach(delegate(Mem_ship x)
			{
				x.SetSubBull_ToMission(subBull);
				x.SetSubFuel_ToMission(subFuel);
				Mem_shipBase mem_shipBase = new Mem_shipBase(x);
				mem_shipBase.Cond -= subCond;
				if (mem_shipBase.Cond < 0)
				{
					mem_shipBase.Cond = 0;
				}
				x.Set_ShipParam(mem_shipBase, Mst_DataManager.Instance.Mst_ship[mem_shipBase.Ship_id], enemy_flag: false);
			});
		}

		public int GetRequireMissionTime()
		{
			if (CompleteTime < Comm_UserDatas.Instance.User_turn.Total_turn)
			{
				return 0;
			}
			return CompleteTime - Comm_UserDatas.Instance.User_turn.Total_turn;
		}

		public void MoveArea(int area_id)
		{
			Area_id = area_id;
		}

		public void ActionStart()
		{
			if (IsActionEnd)
			{
				IsActionEnd = false;
			}
		}

		public void ActionEnd()
		{
			if (!IsActionEnd)
			{
				IsActionEnd = true;
			}
		}

		protected override void setProperty(XElement element)
		{
			Rid = int.Parse(element.Element("_rid").Value);
			Area_id = int.Parse(element.Element("_area_id").Value);
			Name = element.Element("_name").Value;
			Mission_id = int.Parse(element.Element("_mission_id").Value);
			MissionState = (MissionStates)(int)Enum.Parse(typeof(MissionStates), element.Element("_missionState").Value);
			SupportKind = (SupportKinds)(int)Enum.Parse(typeof(SupportKinds), element.Element("_supportKind").Value);
			StartTime = int.Parse(element.Element("_startTime").Value);
			CompleteTime = int.Parse(element.Element("_completeTime").Value);
			IsActionEnd = bool.Parse(element.Element("_isActionEnd").Value);
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
