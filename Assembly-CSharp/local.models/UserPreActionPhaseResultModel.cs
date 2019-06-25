using Common.Enum;
using Common.Struct;
using local.managers;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class UserPreActionPhaseResultModel : PhaseResultModel
	{
		private List<MissionResultModel> _mission_results;

		private List<ShipModel> _bling_end_ships;

		private List<EscortDeckModel> _bling_end_escort_decks;

		private Dictionary<int, int> _bling_end_tanker;

		private MaterialInfo _resources;

		private MaterialInfo _resources_monthly_bonus;

		private MaterialInfo _resources_weekly_bonus;

		private List<IReward> _rewards;

		public MaterialInfo Resources => _resources;

		public ShipModel[] BlingEnd_Ship => _bling_end_ships.ToArray();

		public EscortDeckModel[] BlingEnd_EscortDeck => _bling_end_escort_decks.ToArray();

		public Dictionary<int, int> BlingEnd_Tanker => _bling_end_tanker;

		public MissionResultModel[] MissionResults => _mission_results.ToArray();

		public List<IReward> Rewards => _rewards;

		public UserPreActionPhaseResultModel(TurnWorkResult data, ManagerBase manager)
			: base(data)
		{
			_bling_end_ships = new List<ShipModel>();
			_bling_end_escort_decks = new List<EscortDeckModel>();
			_bling_end_tanker = new Dictionary<int, int>();
			if (_data.BlingEndShip != null)
			{
				Api_Result<Dictionary<int, Mem_ship>> api_Result = new Api_get_Member().Ship(_data.BlingEndShip);
				if (api_Result.state == Api_Result_State.Success)
				{
					for (int i = 0; i < _data.BlingEndShip.Count; i++)
					{
						int key = _data.BlingEndShip[i];
						_bling_end_ships.Add(new ShipModel(api_Result.data[key]));
					}
				}
			}
			if (_data.BlingEndEscortDeck != null)
			{
				for (int j = 0; j < _data.BlingEndEscortDeck.Count; j++)
				{
					int num = _data.BlingEndEscortDeck[j];
					int area_id = num;
					EscortDeckModel escortDeck = manager.UserInfo.GetEscortDeck(area_id);
					_bling_end_escort_decks.Add(escortDeck);
				}
			}
			if (_data.BlingEndTanker != null)
			{
				foreach (int key2 in _data.BlingEndTanker.Keys)
				{
					_bling_end_tanker[key2] = _data.BlingEndTanker[key2].Count;
				}
			}
			_mission_results = new List<MissionResultModel>();
			if (data.MissionEndDecks != null && data.MissionEndDecks.Count > 0)
			{
				for (int k = 0; k < data.MissionEndDecks.Count; k++)
				{
					int rid = data.MissionEndDecks[k].Rid;
					DeckModel deck = manager.UserInfo.GetDeck(rid);
					ShipModel[] ships = deck.GetShips();
					Dictionary<int, int> dictionary = new Dictionary<int, int>();
					foreach (ShipModel shipModel in ships)
					{
						dictionary[shipModel.MemId] = shipModel.Exp_Percentage;
					}
					Api_Result<MissionResultFmt> api_Result2 = new Api_req_Mission().Result(rid);
					if (api_Result2.state == Api_Result_State.Success)
					{
						MissionResultFmt data2 = api_Result2.data;
						_mission_results.Add(new MissionResultModel(data2, manager.UserInfo, dictionary));
					}
				}
			}
			_resources = new MaterialInfo(_data.TransportMaterial);
			_resources_monthly_bonus = new MaterialInfo(_data.BonusMaterialMonthly);
			_resources_weekly_bonus = new MaterialInfo(_data.BonusMaterialWeekly);
			_rewards = new List<IReward>();
			if (_data.SpecialItem == null || _data.SpecialItem.Count <= 0)
			{
				return;
			}
			for (int m = 0; m < _data.SpecialItem.Count; m++)
			{
				ItemGetFmt itemGetFmt = _data.SpecialItem[m];
				if (itemGetFmt.Category == ItemGetKinds.Ship)
				{
					_rewards.Add(new Reward_Ship(itemGetFmt.Id));
				}
				else if (itemGetFmt.Category == ItemGetKinds.SlotItem)
				{
					_rewards.Add(new Reward_Slotitem(itemGetFmt.Id, itemGetFmt.Count));
				}
				else if (itemGetFmt.Category == ItemGetKinds.UseItem)
				{
					_rewards.Add(new Reward_Useitem(itemGetFmt.Id, itemGetFmt.Count));
				}
			}
		}

		public MaterialInfo GetMonthlyBonus()
		{
			return _resources_monthly_bonus;
		}

		public MaterialInfo GetWeeklyBonus()
		{
			return _resources_weekly_bonus;
		}

		public override string ToString()
		{
			string str = $"[ユ\u30fcザ\u30fc事前行動フェ\u30fcズ]:\n";
			MaterialInfo resources = Resources;
			str += $" 資源回収:燃/弾/鋼/ボ:{resources.Fuel}/{resources.Ammo}/{resources.Steel}/{resources.Baux} 高速建造/高速修復/開発資材/改修資材:{resources.BuildKit}/{resources.RepairKit}/{resources.Devkit}/{resources.Revkit}\n";
			resources = GetMonthlyBonus();
			if (resources.HasPositive())
			{
				str += $" 月頭ボ\u30fcナス:燃/弾/鋼/ボ:{resources.Fuel}/{resources.Ammo}/{resources.Steel}/{resources.Baux} 高速建造/高速修復/開発資材/改修資材:{resources.BuildKit}/{resources.RepairKit}/{resources.Devkit}/{resources.Revkit}\n";
			}
			resources = GetWeeklyBonus();
			if (resources.HasPositive())
			{
				str += $" 週頭ボ\u30fcナス:燃/弾/鋼/ボ:{resources.Fuel}/{resources.Ammo}/{resources.Steel}/{resources.Baux} 高速建造/高速修復/開発資材/改修資材:{resources.BuildKit}/{resources.RepairKit}/{resources.Devkit}/{resources.Revkit}\n";
			}
			if (Rewards != null && Rewards.Count > 0)
			{
				str += $"[特別ボ\u30fcナス] ";
				for (int i = 0; i < Rewards.Count; i++)
				{
					str += $"{Rewards[i]} ";
				}
			}
			str += $" -- 移動完了した艦[";
			for (int j = 0; j < _bling_end_ships.Count; j++)
			{
				ShipModel shipModel = _bling_end_ships[j];
				str += $"{shipModel.Name}(mst:{shipModel.MstId},mem:{shipModel.MemId})";
				if (j < _bling_end_ships.Count - 1)
				{
					str += ", ";
				}
			}
			str += "]\n";
			str += $" -- 移動完了した護衛艦隊[";
			for (int k = 0; k < _bling_end_escort_decks.Count; k++)
			{
				EscortDeckModel escortDeckModel = _bling_end_escort_decks[k];
				str += $"海域{escortDeckModel.AreaId}の護衛艦隊";
				if (k < _bling_end_escort_decks.Count - 1)
				{
					str += ", ";
				}
			}
			str += "]\n";
			str += $" -- 移動完了した輸送船[";
			foreach (int key in _bling_end_tanker.Keys)
			{
				str += $"海域{key}に{_bling_end_tanker[key]}隻";
				str += ", ";
			}
			str += "]\n";
			if (MissionResults.Length > 0)
			{
				str += "\n";
				for (int l = 0; l < MissionResults.Length; l++)
				{
					str += $"[遠征終了]:{MissionResults[l]}\n";
				}
			}
			return str;
		}
	}
}
