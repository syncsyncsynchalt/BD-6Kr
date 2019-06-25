using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_missioncomp", Namespace = "")]
	public class Mem_missioncomp : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private int _maparea_id;

		[DataMember]
		private MissionClearKinds _state;

		private static string _tableName = "mem_missioncomp";

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

		public MissionClearKinds State
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

		public Mem_missioncomp()
		{
		}

		public Mem_missioncomp(int rid, int maparea_id, MissionClearKinds state)
			: this()
		{
			Rid = rid;
			Maparea_id = maparea_id;
			State = state;
		}

		public bool Insert()
		{
			if (!Comm_UserDatas.Instance.User_missioncomp.ContainsKey(Rid))
			{
				Comm_UserDatas.Instance.User_missioncomp.Add(Rid, this);
			}
			return true;
		}

		public bool Update()
		{
			Mem_missioncomp value = null;
			if (Comm_UserDatas.Instance.User_missioncomp.TryGetValue(Rid, out value))
			{
				value.Maparea_id = Maparea_id;
				value.Rid = Rid;
				value.State = State;
			}
			return true;
		}

		public List<User_MissionFmt> GetActiveMission()
		{
			Dictionary<int, Mst_mission2> mst_mission = Mst_DataManager.Instance.Mst_mission;
			if (Comm_UserDatas.Instance.User_missioncomp.Count == 0)
			{
				return newUserActiveMission(mst_mission);
			}
			var source = from element in Comm_UserDatas.Instance.User_missioncomp.Values
				select new
				{
					id = element.Rid,
					state = element.State
				};
			List<User_MissionFmt> list = new List<User_MissionFmt>();
			using (Dictionary<int, Mst_mission2>.ValueCollection.Enumerator enumerator = mst_mission.Values.GetEnumerator())
			{
				Mst_mission2 mst_item;
				while (enumerator.MoveNext())
				{
					mst_item = enumerator.Current;
					if (Mst_DataManager.Instance.Mst_maparea.ContainsKey(mst_item.Maparea_id) && Mst_DataManager.Instance.Mst_maparea[mst_item.Maparea_id].Evt_flag == 0)
					{
						var anon = source.FirstOrDefault(x => x.id == mst_item.Id);
						if (anon != null)
						{
							User_MissionFmt user_MissionFmt = new User_MissionFmt();
							user_MissionFmt.MissionId = mst_item.Id;
							user_MissionFmt.State = anon.state;
							list.Add(user_MissionFmt);
						}
						else if (string.IsNullOrEmpty(mst_item.Required_ids))
						{
							User_MissionFmt user_MissionFmt2 = new User_MissionFmt();
							user_MissionFmt2.MissionId = mst_item.Id;
							user_MissionFmt2.State = MissionClearKinds.NEW;
							list.Add(user_MissionFmt2);
						}
						else
						{
							string[] array = mst_item.Required_ids.Split(',');
							bool flag = true;
							string[] array2 = array;
							foreach (string s in array2)
							{
								int id = int.Parse(s);
								var anon2 = source.FirstOrDefault(y => y.id == id);
								if (anon2 == null)
								{
									flag = false;
									break;
								}
								MissionClearKinds state = anon2.state;
								if (state != MissionClearKinds.CLEARED)
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								User_MissionFmt user_MissionFmt3 = new User_MissionFmt();
								user_MissionFmt3.MissionId = mst_item.Id;
								user_MissionFmt3.State = MissionClearKinds.NEW;
								list.Add(user_MissionFmt3);
							}
						}
					}
				}
			}
			return (from x in list
				orderby x.MissionId
				select x).ToList();
		}

		private List<User_MissionFmt> newUserActiveMission(Dictionary<int, Mst_mission2> mst_mission)
		{
			List<User_MissionFmt> list = new List<User_MissionFmt>();
			foreach (Mst_mission2 value in mst_mission.Values)
			{
				if (Mst_DataManager.Instance.Mst_maparea.ContainsKey(value.Maparea_id) && Mst_DataManager.Instance.Mst_maparea[value.Maparea_id].Evt_flag == 0 && string.IsNullOrEmpty(value.Required_ids))
				{
					User_MissionFmt user_MissionFmt = new User_MissionFmt();
					user_MissionFmt.MissionId = value.Id;
					user_MissionFmt.State = MissionClearKinds.NEW;
					list.Add(user_MissionFmt);
				}
			}
			return (from x in list
				orderby x.MissionId
				select x).ToList();
		}

		protected override void setProperty(XElement element)
		{
			Rid = int.Parse(element.Element("_rid").Value);
			Maparea_id = int.Parse(element.Element("_maparea_id").Value);
			State = (MissionClearKinds)(int)Enum.Parse(typeof(MissionClearKinds), element.Element("_state").Value);
		}
	}
}
