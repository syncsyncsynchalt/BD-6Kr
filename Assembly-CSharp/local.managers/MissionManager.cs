using Common.Enum;
using local.models;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.managers
{
	public class MissionManager : ManagerBase
	{
		private int _area_id;

		private Api_req_Mission _req_mission;

		private Dictionary<int, List<MissionModel>> _mission_dic;

		public int AreaId => _area_id;

		public int TankerCount => _tanker_manager.GetCounts().GetCountNoMove();

		public MissionManager(int area_id)
		{
			_area_id = area_id;
			_req_mission = new Api_req_Mission();
			_tanker_manager = new _TankerManager();
		}

		public MissionModel GetMission(int mission_id)
		{
			if (_mission_dic == null)
			{
				_UpdateMission();
			}
			foreach (List<MissionModel> value in _mission_dic.Values)
			{
				MissionModel missionModel = value.Find((MissionModel mission) => mission.Id == mission_id);
				if (missionModel != null)
				{
					return missionModel;
				}
			}
			return null;
		}

		public MissionModel[] GetMissions()
		{
			if (_mission_dic == null)
			{
				_UpdateMission();
			}
			if (_mission_dic.ContainsKey(_area_id))
			{
				return _mission_dic[_area_id].ToArray();
			}
			return new MissionModel[0];
		}

		public MissionModel[] GetMissionsWithoutDeck()
		{
			if (_mission_dic == null)
			{
				_UpdateMission();
			}
			if (_mission_dic.ContainsKey(_area_id))
			{
				List<MissionModel> list = _mission_dic[_area_id];
				list = list.FindAll((MissionModel mission) => mission.Deck == null);
				return list.ToArray();
			}
			return new MissionModel[0];
		}

		public List<IsGoCondition> IsValidMissionStart(int deck_id, int mission_id, int tanker_count)
		{
			DeckModel deck = base.UserInfo.GetDeck(deck_id);
			List<IsGoCondition> list = deck.IsValidMission();
			HashSet<IsGoCondition> collection = _req_mission.ValidStart(deck_id, mission_id, tanker_count);
			List<IsGoCondition> list2 = new List<IsGoCondition>(collection);
			list2.Sort();
			for (int i = 0; i < list2.Count; i++)
			{
				if (!list.Contains(list2[i]))
				{
					list.Add(list2[i]);
				}
			}
			return list;
		}

		public bool MissionStart(int deck_id, int mission_id, int tanker_count)
		{
			if (IsValidMissionStart(deck_id, mission_id, tanker_count).Count == 0)
			{
				Api_Result<Mem_deck> api_Result = _req_mission.Start(deck_id, mission_id, tanker_count);
				if (api_Result.state == Api_Result_State.Success)
				{
					_mission_dic = null;
					return true;
				}
			}
			return false;
		}

		public bool IsValidMissionStop(int deck_id)
		{
			return _req_mission.ValidStop(deck_id);
		}

		public bool MissionStop(int deck_id)
		{
			if (_req_mission.ValidStop(deck_id))
			{
				Api_Result<int> api_Result = _req_mission.Stop(deck_id);
				if (api_Result.state == Api_Result_State.Success)
				{
					_mission_dic = null;
					return true;
				}
				return false;
			}
			return false;
		}

		public void UpdateMissionStates()
		{
			_UpdateMission();
		}

		private void _UpdateMission()
		{
			_mission_dic = new Dictionary<int, List<MissionModel>>();
			Api_Result<List<User_MissionFmt>> api_Result = new Api_get_Member().Mission();
			if (api_Result.state != 0)
			{
				return;
			}
			DeckModel[] decksFromArea = base.UserInfo.GetDecksFromArea(_area_id);
			Dictionary<int, DeckModel> dictionary = new Dictionary<int, DeckModel>();
			foreach (DeckModel deckModel in decksFromArea)
			{
				if (deckModel.MissionState != 0)
				{
					dictionary.Add(deckModel.MissionId, deckModel);
				}
			}
			for (int j = 0; j < api_Result.data.Count; j++)
			{
				User_MissionFmt user_MissionFmt = api_Result.data[j];
				MissionModel missionModel = (!dictionary.ContainsKey(user_MissionFmt.MissionId)) ? new MissionModel(user_MissionFmt) : new MissionModel(user_MissionFmt, dictionary[user_MissionFmt.MissionId]);
				if (!_mission_dic.ContainsKey(missionModel.AreaId))
				{
					_mission_dic[missionModel.AreaId] = new List<MissionModel>();
				}
				_mission_dic[missionModel.AreaId].Add(missionModel);
			}
		}
	}
}
