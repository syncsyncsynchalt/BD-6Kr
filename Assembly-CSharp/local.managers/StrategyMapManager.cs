using Common.Enum;
using local.models;
using local.utils;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class StrategyMapManager : TurnManager
	{
		public Dictionary<int, MapAreaModel> Area => new Dictionary<int, MapAreaModel>(ManagerBase._area);

		public StrategyMapManager()
		{
			_CreateMapAreaModel();
		}

		public bool IsOpenedLastAreaAtLeastOnce()
		{
			return new Api_req_StrategeMap().IsLastAreaOpend();
		}

		public AreaTankerModel GetNonDeploymentTankerCount()
		{
			return _tanker_manager.GetCounts();
		}

		public List<MapAreaModel> GetValidMoveToArea(int deck_id)
		{
			List<MapAreaModel> list = new List<MapAreaModel>();
			DeckModel deck = base.UserInfo.GetDeck(deck_id);
			if (deck.IsValidMove().Count > 0)
			{
				return list;
			}
			int areaId = deck.AreaId;
			List<int> neighboringAreaIDs = ManagerBase._area[areaId].NeighboringAreaIDs;
			for (int i = 0; i < neighboringAreaIDs.Count; i++)
			{
				int key = neighboringAreaIDs[i];
				MapAreaModel mapAreaModel = ManagerBase._area[key];
				if (mapAreaModel.IsOpen())
				{
					list.Add(mapAreaModel);
				}
			}
			return list;
		}

		public bool Move(int deck_id, int move_to_area_id)
		{
			Api_Result<Mem_deck> api_Result = new Api_req_StrategeMap().MoveArea(deck_id, move_to_area_id);
			if (api_Result.state == Api_Result_State.Success)
			{
				ManagerBase._turn_state = api_Result.t_state;
				return true;
			}
			return false;
		}

		public SortieManager SelectArea(int area_id)
		{
			return new SortieManager(area_id);
		}

		public HashSet<int> GetMissionAreaId()
		{
			HashSet<int> hashSet = new HashSet<int>();
			Api_Result<List<User_MissionFmt>> api_Result = new Api_get_Member().Mission();
			if (api_Result.state != 0)
			{
				return hashSet;
			}
			Dictionary<int, Mst_mission2> mst_mission = Mst_DataManager.Instance.Mst_mission;
			for (int i = 0; i < api_Result.data.Count; i++)
			{
				User_MissionFmt user_MissionFmt = api_Result.data[i];
				if (mst_mission.TryGetValue(user_MissionFmt.MissionId, out Mst_mission2 value) && !hashSet.Contains(value.Maparea_id))
				{
					hashSet.Add(value.Maparea_id);
				}
			}
			return hashSet;
		}

		public List<MapAreaModel> GetRebellionAreaList()
		{
			List<int> rebellionAreaOrderByEvent = new Api_req_StrategeMap().GetRebellionAreaOrderByEvent();
			List<MapAreaModel> list = new List<MapAreaModel>();
			for (int i = 0; i < rebellionAreaOrderByEvent.Count; i++)
			{
				int key = rebellionAreaOrderByEvent[i];
				MapAreaModel item = ManagerBase._area[key];
				list.Add(item);
			}
			return list;
		}

		public RebellionManager SelectAreaForRebellion(int area_id)
		{
			return new RebellionManager(area_id);
		}

		public bool IsValidChangeTankerCount(int area_id, int count)
		{
			AreaTankerModel tankerCount = ManagerBase._area[area_id].GetTankerCount();
			int count2 = tankerCount.GetCount();
			if (count2 > count)
			{
				int num = count2 - count;
				int count3 = new Api_req_Transport().GetEnableBackTanker(area_id).Count;
				if (count3 < num)
				{
					return false;
				}
				return true;
			}
			if (count2 < count)
			{
				int num2 = count - count2;
				if (!ManagerBase._area[area_id].IsOpen())
				{
					return false;
				}
				if (tankerCount.GetMaxCount() < count)
				{
					return false;
				}
				AreaTankerModel nonDeploymentTankerCount = GetNonDeploymentTankerCount();
				if (nonDeploymentTankerCount.GetCount() - nonDeploymentTankerCount.GetCountMove() < num2)
				{
					return false;
				}
				return true;
			}
			return true;
		}

		public void GetTankerCountRange(int area_id, out int out_max, out int out_min)
		{
			AreaTankerModel tankerCount = ManagerBase._area[area_id].GetTankerCount();
			AreaTankerModel nonDeploymentTankerCount = GetNonDeploymentTankerCount();
			out_max = tankerCount.GetCount() + nonDeploymentTankerCount.GetCount() - nonDeploymentTankerCount.GetCountMove();
			out_max = Math.Min(out_max, tankerCount.GetMaxCount());
			out_min = tankerCount.GetCountMove();
		}

		[Obsolete("local.utils.Utils.GetAreaResource(int area_id, int tanker_count, EscortDeckManager eManager) を使用してください", false)]
		public Dictionary<enumMaterialCategory, int> GetAreaResource(int area_id, int tanker_count)
		{
			return Utils.GetAreaResource(area_id, tanker_count);
		}

		public bool IsValidDeploy(int area_id, int tanker_count, EscortDeckManager escort)
		{
			if (!IsValidChangeTankerCount(area_id, tanker_count))
			{
				return false;
			}
			AreaTankerModel tankerCount = ManagerBase._area[area_id].GetTankerCount();
			if (!escort.HasChanged() && tankerCount.GetCount() == tanker_count)
			{
				return false;
			}
			return true;
		}

		public bool Deploy(int area_id, int tanker_count, EscortDeckManager escort)
		{
			AreaTankerModel tankerCount = ManagerBase._area[area_id].GetTankerCount();
			int count = tankerCount.GetCount();
			if (!IsValidDeploy(area_id, tanker_count, escort))
			{
				return false;
			}
			bool flag = false;
			if (escort.HasChanged())
			{
				flag = escort.__Commit__();
			}
			Api_Result<List<Mem_tanker>> api_Result = null;
			if (count != tanker_count)
			{
				if (count > tanker_count)
				{
					int num = count - tanker_count;
					api_Result = new Api_req_Transport().BackTanker(area_id, num);
				}
				else if (count < tanker_count)
				{
					int num2 = tanker_count - count;
					api_Result = new Api_req_Transport().GoTanker(area_id, num2);
				}
			}
			if (!flag && (api_Result == null || api_Result.state != 0))
			{
				return false;
			}
			if (flag)
			{
				base.UserInfo.__UpdateEscortDeck__(new Api_get_Member());
			}
			if (api_Result != null && api_Result.state == Api_Result_State.Success)
			{
				return _tanker_manager.Update();
			}
			return flag;
		}

		public override UserPreActionPhaseResultModel GetResult_UserPreActionPhase()
		{
			UserPreActionPhaseResultModel result_UserPreActionPhase = base.GetResult_UserPreActionPhase();
			_CreateMapAreaModel();
			return result_UserPreActionPhase;
		}

		public override UserActionPhaseResultModel GetResult_UserActionPhase()
		{
			return base.GetResult_UserActionPhase();
		}

		public override string ToString()
		{
			return ToString(detail: false);
		}

		public string ToString(bool detail)
		{
			string empty = string.Empty;
			empty += $"{base.ToString()}\n";
			AreaTankerModel nonDeploymentTankerCount = GetNonDeploymentTankerCount();
			empty += $"海域に未配備の輸送船数:{nonDeploymentTankerCount.GetCount()}(帰港中:{nonDeploymentTankerCount.GetCountMove()})\n";
			if (detail)
			{
				empty += $"[--海域一覧--]\n";
				Dictionary<int, MapAreaModel> area = Area;
				foreach (MapAreaModel value in area.Values)
				{
					SortieManager sortieManager = SelectArea(value.Id);
					MapModel[] maps = sortieManager.Maps;
					empty += $"{value} ";
					foreach (MapModel mapModel in maps)
					{
						empty = ((!mapModel.Map_Possible) ? (empty + string.Format("({0}-{1}{2}{3}) ", mapModel.AreaId, mapModel.No, (!mapModel.Cleared) ? string.Empty : "[Clear]", (!mapModel.ClearedOnce) ? string.Empty : "!")) : (empty + string.Format("{0}-{1}{2}{3} ", mapModel.AreaId, mapModel.No, (!mapModel.Cleared) ? string.Empty : "[Clear]", (!mapModel.ClearedOnce) ? string.Empty : "!")));
					}
					empty += "\n";
				}
				empty += $"[--海域一覧--]";
			}
			return empty;
		}
	}
}
