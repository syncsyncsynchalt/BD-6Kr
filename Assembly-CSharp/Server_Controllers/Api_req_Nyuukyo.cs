using Common.Enum;
using Server_Common;
using Server_Controllers.QuestLogic;
using Server_Models;
using System.Collections.Generic;

namespace Server_Controllers
{
	public class Api_req_Nyuukyo
	{
		public Api_Result<Mem_ndock> Start(int rid, int ship_rid, bool highspeed)
		{
			Api_Result<Mem_ndock> api_Result = new Api_Result<Mem_ndock>();
			Mem_ndock value = null;
			if (!Comm_UserDatas.Instance.User_ndock.TryGetValue(rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (value.State != NdockStates.EMPTY)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_ship value2 = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, out value2))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (value2.IsBlingShip())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Dictionary<enumMaterialCategory, int> ndockMaterialNum = value2.GetNdockMaterialNum();
			if (ndockMaterialNum == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			int num = highspeed ? 1 : 0;
			if (ndockMaterialNum[enumMaterialCategory.Fuel] > Comm_UserDatas.Instance.User_material[enumMaterialCategory.Fuel].Value || ndockMaterialNum[enumMaterialCategory.Steel] > Comm_UserDatas.Instance.User_material[enumMaterialCategory.Steel].Value || num > Comm_UserDatas.Instance.User_material[enumMaterialCategory.Repair_Kit].Value)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			value2.BlingWaitToStop();
			if (!highspeed)
			{
				int ndockTimeSpan = value2.GetNdockTimeSpan();
				value.RecoverStart(ship_rid, ndockMaterialNum, ndockTimeSpan);
			}
			else
			{
				value.RecoverStart(ship_rid, ndockMaterialNum, 0);
				value.RecoverEnd(timeChk: false);
				Comm_UserDatas.Instance.User_material[enumMaterialCategory.Repair_Kit].Sub_Material(1);
			}
			api_Result.data = value;
			QuestSupply questSupply = new QuestSupply();
			questSupply.ExecuteCheck();
			return api_Result;
		}

		public Api_Result<Mem_ndock> SpeedChange(int rid)
		{
			Api_Result<Mem_ndock> api_Result = new Api_Result<Mem_ndock>();
			Mem_ndock value = null;
			if (!Comm_UserDatas.Instance.User_ndock.TryGetValue(rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (value.State != NdockStates.RESTORE)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (Comm_UserDatas.Instance.User_material[enumMaterialCategory.Repair_Kit].Value < 1)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (value.RecoverEnd(timeChk: false))
			{
				Comm_UserDatas.Instance.User_material[enumMaterialCategory.Repair_Kit].Sub_Material(1);
				api_Result.data = value;
				return api_Result;
			}
			api_Result.state = Api_Result_State.Parameter_Error;
			return api_Result;
		}
	}
}
