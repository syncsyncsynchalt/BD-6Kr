using local.models;
using local.utils;
using Server_Controllers;
using Server_Models;

namespace local.managers
{
	public class RemodelGradeUpManager : ManagerBase
	{
		private ShipModel _ship;

		private Api_req_Kaisou.RemodelingChkResult _chkResult;

		private int _required_design_specifications;

		private int _design_specifications;

		public ShipModel TargetShip => _ship;

		public int DesignSpecificationsForGradeup => _required_design_specifications;

		public int DesignSpecifications => _design_specifications;

		public bool GradeupBtnEnabled => _chkResult == Api_req_Kaisou.RemodelingChkResult.OK;

		public RemodelGradeUpManager(ShipModel ship)
		{
			_ship = ship;
			_IsValid();
		}

		public bool GradeUp()
		{
			if (GradeupBtnEnabled)
			{
				Api_Result<Mem_ship> api_Result = new Api_req_Kaisou().Remodeling(_ship.MemId, _required_design_specifications);
				if (api_Result.state == Api_Result_State.Success)
				{
					_ship.SetMemData(api_Result.data);
					ShipModel ship = base.UserInfo.GetShip(_ship.MemId);
					if (ship != _ship)
					{
						ship.SetMemData(api_Result.data);
					}
					return true;
				}
			}
			return false;
		}

		private bool _IsValid()
		{
			_chkResult = new Api_req_Kaisou().ValidRemodeling(_ship.MemId, out _required_design_specifications);
			_design_specifications = new UseitemUtil().GetCount(58);
			return GradeupBtnEnabled;
		}

		public override string ToString()
		{
			string str = "== 改造マネ\u30fcジャ ==\n";
			str += $"対象艦: {TargetShip.ShortName} Lv{TargetShip.Level}(必要レベル:{TargetShip.AfterLevel}) 所持弾薬:{ManagerBase._materialModel.Ammo}(必要弾薬:{TargetShip.AfterAmmo}) 所持鋼材:{ManagerBase._materialModel.Steel}(必要鋼材:{TargetShip.AfterSteel}) 所持開発資材:{ManagerBase._materialModel.Devkit}(必要開発資材:{TargetShip.AfterDevkit}) \n";
			str += $"所持改装設計書:{DesignSpecifications}(必要改装設計書:{DesignSpecificationsForGradeup})\n";
			str += $"改装開始可能:{GradeupBtnEnabled} 改造チェック:{_chkResult}\n";
			return str + "== ＝＝＝＝＝＝＝ ==";
		}
	}
}
