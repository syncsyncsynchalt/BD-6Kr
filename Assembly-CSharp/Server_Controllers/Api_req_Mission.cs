using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers.MissionLogic;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_Mission
	{
		private Dictionary<int, int> mst_level;

		private Dictionary<int, int> mst_userlevel;

		public HashSet<IsGoCondition> ValidStart(int deck_rid, int mission_id, int tankerNum)
		{
			HashSet<IsGoCondition> ret = new HashSet<IsGoCondition>();
			Mst_mission2 mst_mission = null;
			if (mission_id > 0)
			{
				if (!Mst_DataManager.Instance.Mst_mission.TryGetValue(mission_id, out mst_mission))
				{
					ret.Add(IsGoCondition.Invalid);
					return ret;
				}
			}
			else
			{
				if (mission_id != -1 && mission_id != -2)
				{
					ret.Add(IsGoCondition.Invalid);
					return ret;
				}
				List<Mst_mission2> supportResistedData = Mst_DataManager.Instance.GetSupportResistedData(Comm_UserDatas.Instance.User_deck[deck_rid].Area_id);
				mst_mission = ((mission_id != -1) ? supportResistedData[1] : supportResistedData[0]);
			}
			Mem_deck value = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, out value))
			{
				ret.Add(IsGoCondition.InvalidDeck);
				return ret;
			}
			if (value.Ship.Count() == 0)
			{
				ret.Add(IsGoCondition.InvalidDeck);
				return ret;
			}
			if (value.MissionState != 0)
			{
				ret.Add(IsGoCondition.Mission);
			}
			if (value.IsActionEnd)
			{
				ret.Add(IsGoCondition.ActionEndDeck);
				return ret;
			}
			Mem_ship value2 = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(value.Ship[0], out value2))
			{
				ret.Add(IsGoCondition.Invalid);
				return ret;
			}
			if (value2.Get_DamageState() == DamageState.Taiha)
			{
				ret.Add(IsGoCondition.FlagShipTaiha);
			}
			Mem_deck mem_deck = Comm_UserDatas.Instance.User_deck.Values.FirstOrDefault((Mem_deck x) => (x.Mission_id == mission_id) ? true : false);
			if (mem_deck != null)
			{
				ret.Add(IsGoCondition.OtherDeckMissionRunning);
			}
			int destroy_ship = 0;
			value.Ship.getMemShip().ForEach(delegate(Mem_ship deck_ship)
			{
				if (deck_ship.Stype == 2)
				{
					destroy_ship++;
				}
				Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[deck_ship.Ship_id];
				if (deck_ship.Fuel <= 0 || deck_ship.Bull <= 0)
				{
					ret.Add(IsGoCondition.NeedSupply);
				}
				if (mst_mission.IsSupportMission() && (deck_ship.Fuel < mst_ship.Fuel_max || deck_ship.Bull < mst_ship.Bull_max))
				{
					ret.Add(IsGoCondition.ReqFullSupply);
				}
				if (Comm_UserDatas.Instance.User_ndock.Values.Any((Mem_ndock ndock) => ndock.Ship_id == deck_ship.Rid))
				{
					ret.Add(IsGoCondition.HasRepair);
				}
				if (deck_ship.IsBlingShip())
				{
					ret.Add(IsGoCondition.HasBling);
				}
			});
			if (mst_mission.IsSupportMission() && destroy_ship < 2)
			{
				ret.Add(IsGoCondition.NecessaryStype);
			}
			List<Mem_tanker> freeTanker = Mem_tanker.GetFreeTanker(Comm_UserDatas.Instance.User_tanker);
			if (freeTanker.Count < tankerNum)
			{
				ret.Add(IsGoCondition.Tanker);
			}
			return ret;
		}

		public Api_Result<Mem_deck> Start(int deck_rid, int mission_id, int tankerNum)
		{
			Api_Result<Mem_deck> api_Result = new Api_Result<Mem_deck>();
			Mst_mission2 value = null;
			if (mission_id > 0)
			{
				if (!Mst_DataManager.Instance.Mst_mission.TryGetValue(mission_id, out value))
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
			}
			else
			{
				List<Mst_mission2> supportResistedData = Mst_DataManager.Instance.GetSupportResistedData(Comm_UserDatas.Instance.User_deck[deck_rid].Area_id);
				value = ((mission_id != -1) ? supportResistedData[1] : supportResistedData[0]);
			}
			Mem_deck value2 = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, out value2))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mission_id > 0 && !Comm_UserDatas.Instance.User_missioncomp.ContainsKey(mission_id))
			{
				Mem_missioncomp mem_missioncomp = new Mem_missioncomp(mission_id, value.Maparea_id, MissionClearKinds.NOTCLEAR);
				mem_missioncomp.Insert();
			}
			if (!value2.MissionStart(value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (tankerNum > 0)
			{
				IEnumerable<Mem_tanker> enumerable = Mem_tanker.GetFreeTanker(Comm_UserDatas.Instance.User_tanker).Take(tankerNum);
				List<Mem_tanker> list = new List<Mem_tanker>();
				foreach (Mem_tanker item in enumerable)
				{
					if (!item.MissionStart(value.Maparea_id, deck_rid))
					{
						list.ForEach(delegate(Mem_tanker x)
						{
							x.MissionTerm();
						});
						api_Result.state = Api_Result_State.Parameter_Error;
						return api_Result;
					}
					list.Add(item);
				}
			}
			api_Result.data = value2;
			QuestMission questMission = new QuestMission(value.Id, value2, MissionResultKinds.FAILE);
			questMission.ExecuteCheck();
			return api_Result;
		}

		public bool ValidStop(int deck_rid)
		{
			Mem_deck value = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, out value))
			{
				return false;
			}
			if (value.MissionState != MissionStates.RUNNING)
			{
				return false;
			}
			if (value.SupportKind != 0)
			{
				return false;
			}
			int total_turn = Comm_UserDatas.Instance.User_turn.Total_turn;
			if (value.CompleteTime <= total_turn)
			{
				return false;
			}
			return true;
		}

		public Api_Result<int> Stop(int deck_rid)
		{
			Api_Result<int> api_Result = new Api_Result<int>();
			Mem_deck mem_deck = Comm_UserDatas.Instance.User_deck[deck_rid];
			Mst_mission2 mst_mission = Mst_DataManager.Instance.Mst_mission[mem_deck.Mission_id];
			int num = mst_mission.Time - (mem_deck.StartTime - mem_deck.CompleteTime);
			double num2 = Math.Ceiling((double)(mst_mission.Time + num) / 2.0);
			int total_turn = Comm_UserDatas.Instance.User_turn.Total_turn;
			int num3 = ((double)(total_turn - mem_deck.StartTime) < num2) ? 1 : 2;
			double num4 = (num3 != 1) ? ((double)(mem_deck.CompleteTime - total_turn)) : ((double)(total_turn - mem_deck.StartTime));
			num4 = Math.Ceiling(num4 / 3.0);
			int newEndTime = (int)((double)total_turn + num4);
			mem_deck.MissionStop(newEndTime);
			api_Result.data = mem_deck.StartTime - mem_deck.CompleteTime;
			return api_Result;
		}

		public Api_Result<MissionResultFmt> Result(int deck_rid)
		{
			Api_Result<MissionResultFmt> api_Result = new Api_Result<MissionResultFmt>();
			Mem_deck value = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (value.MissionState != MissionStates.END && value.MissionState != MissionStates.STOP)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (value.Ship.Count() == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mst_level == null)
			{
				mst_level = Mst_DataManager.Instance.Get_MstLevel(shipTable: true);
			}
			if (mst_userlevel == null)
			{
				mst_userlevel = Mst_DataManager.Instance.Get_MstLevel(shipTable: false);
			}
			int mission_id = value.Mission_id;
			bool flag = value.MissionState == MissionStates.STOP;
			Exec_MissionResult exec_MissionResult = new Exec_MissionResult(value, mst_level, mst_userlevel);
			api_Result.data = exec_MissionResult.GetResultData();
			if (!flag)
			{
				QuestMission questMission = new QuestMission(mission_id, value, api_Result.data.MissionResult);
				questMission.ExecuteCheck();
			}
			return api_Result;
		}
	}
}
