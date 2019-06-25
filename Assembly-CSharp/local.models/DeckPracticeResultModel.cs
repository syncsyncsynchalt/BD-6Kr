using Common.Enum;
using Common.Struct;
using Server_Common.Formats;
using System.Collections.Generic;

namespace local.models
{
	public class DeckPracticeResultModel : DeckActionResultModel
	{
		private DeckPracticeType _type;

		private Dictionary<int, PowUpInfo> _powup;

		public DeckPracticeType PracticeType => _type;

		public DeckPracticeResultModel(DeckPracticeType type, PracticeDeckResultFmt fmt, UserInfoModel user_info, Dictionary<int, int> exp_rates_before)
		{
			_type = type;
			_mission_fmt = fmt.PracticeResult;
			_powup = fmt.PowerUpData;
			_user_info = user_info;
			_exps = new Dictionary<int, ShipExpModel>();
			_SetShipExp(exp_rates_before);
		}

		public PowUpInfo GetShipPowupInfo(int ship_mem_id)
		{
			_powup.TryGetValue(ship_mem_id, out PowUpInfo value);
			return value;
		}

		public override string ToString()
		{
			string str = $"==[艦隊演習結果]==\n";
			str += $"艦隊 ID:{base.DeckID}({base.FleetName}) 艦隊演習タイプ:{PracticeType}\n";
			str += $"提督名:{base.Name} Lv{base.Level}  獲得提督経験値:{base.Exp}\n";
			str += "\n";
			ShipModel[] ships = base.Ships;
			foreach (ShipModel shipModel in ships)
			{
				ShipExpModel shipExpInfo = GetShipExpInfo(shipModel.MemId);
				str += $" {shipModel.Name}(ID:{shipModel.MemId}) {shipExpInfo}\n";
				PowUpInfo powUpInfo = _powup[shipModel.MemId];
				str += $"   火力上昇:{powUpInfo.Karyoku} 雷装上昇:{powUpInfo.Raisou} 対空上昇:{powUpInfo.Taiku} 対潜上昇:{powUpInfo.Taisen} 装甲上昇:{powUpInfo.Soukou} 回避上昇:{powUpInfo.Kaihi} 運上昇:{powUpInfo.Lucky}";
				str += "\n";
			}
			return str;
		}
	}
}
