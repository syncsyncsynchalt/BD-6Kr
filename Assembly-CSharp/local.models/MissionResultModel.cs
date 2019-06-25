using Common.Enum;
using Common.Struct;
using Server_Common.Formats;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class MissionResultModel : DeckActionResultModel
	{
		private List<IReward> _rewards;

		public MissionResultKinds result => _mission_fmt.MissionResult;

		public string MissionName => _mission_fmt.MissionName;

		public int Spoint => _mission_fmt.GetSpoint;

		[Obsolete("GetItems()を使用してください")]
		public int ExtraItemCount => (_mission_fmt.GetItems != null) ? _mission_fmt.GetItems.Count : 0;

		public MissionResultModel(MissionResultFmt fmt, UserInfoModel user_info, Dictionary<int, int> exp_rates_before)
		{
			_mission_fmt = fmt;
			_user_info = user_info;
			_exps = new Dictionary<int, ShipExpModel>();
			_SetShipExp(exp_rates_before);
			_rewards = _InitRewardItems();
		}

		[Obsolete("GetItems()を使用してください")]
		public int GetItemID(int index)
		{
			return (ExtraItemCount <= index) ? (-1) : _mission_fmt.GetItems[index].Id;
		}

		[Obsolete("GetItems()を使用してください")]
		public int GetItemCount(int index)
		{
			return (ExtraItemCount > index) ? _mission_fmt.GetItems[index].Count : 0;
		}

		public MaterialInfo GetMaterialInfo()
		{
			return new MaterialInfo(_mission_fmt.GetMaterials);
		}

		public List<IReward> GetItems()
		{
			return _rewards;
		}

		private List<IReward> _InitRewardItems()
		{
			List<IReward> list = new List<IReward>();
			int num = (_mission_fmt.GetItems != null) ? _mission_fmt.GetItems.Count : 0;
			for (int i = 0; i < num; i++)
			{
				ItemGetFmt itemGetFmt = _mission_fmt.GetItems[i];
				IReward item = null;
				if (itemGetFmt.Category == ItemGetKinds.UseItem)
				{
					item = new Reward_Useitem(itemGetFmt.Id, itemGetFmt.Count);
				}
				else if (itemGetFmt.Category == ItemGetKinds.Ship)
				{
					item = new Reward_Ship(itemGetFmt.Id);
				}
				else if (itemGetFmt.Category == ItemGetKinds.SlotItem)
				{
					item = new Reward_Slotitem(itemGetFmt.Id, itemGetFmt.Count);
				}
				list.Add(item);
			}
			return list;
		}

		public override string ToString()
		{
			string text = $"==[遠征結果]==\n";
			string str = $"帰還艦隊 ID:{base.DeckID}\n";
			ShipModel[] ships = base.Ships;
			foreach (ShipModel shipModel in ships)
			{
				ShipExpModel shipExpInfo = GetShipExpInfo(shipModel.MemId);
				str += $" {shipModel.Name}(ID:{shipModel.MemId}) {shipExpInfo}";
			}
			str += "\n";
			str += $"遠征結果:{result} 遠征名:{MissionName}\n";
			str += $"提督名:{base.Name} Lv{base.Level} [{base.Rank}] 艦隊名:{base.FleetName}\n";
			MaterialInfo materialInfo = GetMaterialInfo();
			str += $"獲得経験値:{base.Exp} 獲得資材:燃/弾/鋼/ボ {materialInfo.Fuel}/{materialInfo.Ammo}/{materialInfo.Steel}/{materialInfo.Baux}";
			str += $" 獲得戦略ポイント:{Spoint}\n";
			List<IReward> items = GetItems();
			for (int j = 0; j < items.Count; j++)
			{
				str += $"獲得アイテム:{items[j]}";
			}
			return str;
		}
	}
}
