using local.managers;
using Server_Common.Formats.Battle;

namespace local.models.battle
{
	public class NightCombatModel
	{
		private NightBattleFmt _fmt;

		private ShipModel_BattleAll _flare_f;

		private ShipModel_BattleAll _flare_e;

		private ShipModel_BattleAll _searchlight_f;

		private ShipModel_BattleAll _searchlight_e;

		private RationModel _ration;

		public NightCombatModel(BattleManager bManager, NightBattleFmt fmt, RationModel ration)
		{
			_fmt = fmt;
			_flare_f = bManager.GetShip(_fmt.F_FlareId);
			_flare_e = bManager.GetShip(_fmt.E_FlareId);
			_searchlight_f = bManager.GetShip(_fmt.F_SearchId);
			_searchlight_e = bManager.GetShip(_fmt.E_SearchId);
			_ration = ration;
		}

		public ShipModel_BattleAll GetFlareShip(bool is_friend)
		{
			return (!is_friend) ? _flare_e : _flare_f;
		}

		public ShipModel_BattleAll GetSearchLightShip(bool is_friend)
		{
			return (!is_friend) ? _searchlight_e : _searchlight_f;
		}

		public SlotitemModel_Battle GetTouchPlane(bool is_friend)
		{
			int num = (!is_friend) ? _fmt.E_TouchPlane : _fmt.F_TouchPlane;
			if (num > 0)
			{
				return new SlotitemModel_Battle(num);
			}
			return null;
		}

		public RationModel GetRationData()
		{
			return _ration;
		}

		public override string ToString()
		{
			string text = $"[夜戦演出]\n";
			if (GetFlareShip(is_friend: true) != null)
			{
				text += $"味方側 照明弾使用:{GetFlareShip(is_friend: true)}\n";
			}
			if (GetFlareShip(is_friend: false) != null)
			{
				text += $"相手側 照明弾使用:{GetFlareShip(is_friend: false)}\n";
			}
			if (GetSearchLightShip(is_friend: true) != null)
			{
				text += $"味方側 探照灯使用:{GetSearchLightShip(is_friend: true)}\n";
			}
			if (GetSearchLightShip(is_friend: false) != null)
			{
				text += $"相手側 照明弾使用:{GetSearchLightShip(is_friend: false)}\n";
			}
			if (GetTouchPlane(is_friend: true) != null)
			{
				text += $"味方側 夜間触接使用:{GetTouchPlane(is_friend: true)}\n";
			}
			if (GetTouchPlane(is_friend: false) != null)
			{
				text += $"相手側 夜間触接使用:{GetTouchPlane(is_friend: false)}\n";
			}
			if (GetRationData() != null)
			{
				text += $"{GetRationData()}\n";
			}
			return text;
		}
	}
}
