using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_StrategeMap
	{
		private Dictionary<int, User_StrategyMapFmt> strategyFmt;

		public Api_req_StrategeMap()
		{
			Api_get_Member api_get_Member = new Api_get_Member();
			strategyFmt = api_get_Member.StrategyInfo().data;
		}

		public bool IsLastAreaOpend()
		{
			List<Mem_history> value = null;
			if (!Comm_UserDatas.Instance.User_history.TryGetValue(3, out value))
			{
				return false;
			}
			return value.Any((Mem_history x) => x.MapinfoId == 171);
		}

		public Api_Result<Mem_deck> MoveArea(int rid, int move_id)
		{
			Api_Result<Mem_deck> api_Result = new Api_Result<Mem_deck>();
			Mem_deck value = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			value.MoveArea(move_id);
			value.ActionEnd();
			api_Result.t_state = TurnState.OWN_END;
			return api_Result;
		}

		public List<int> GetRebellionAreaOrderByEvent()
		{
			return (from item in Comm_UserDatas.Instance.User_rebellion_point.Values
				where item.State == RebellionState.Invation
				orderby item.Rid descending
				select item.Rid).ToList();
		}

		public bool ExecuteRebellionLostArea(int maparea_id)
		{
			new RebellionUtils().LostArea(maparea_id, null);
			return true;
		}
	}
}
