using Common.Enum;
using Common.Struct;
using Server_Common.Formats;
using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class MissionModel
	{
		private int _id;

		private MissionClearKinds _state;

		private Mst_mission2 _mst;

		private DeckModel _deck;

		public int Id => _id;

		public DeckModel Deck => (_deck == null || _deck.MissionState == MissionStates.NONE) ? null : _deck;

		public MissionClearKinds State => _state;

		public int AreaId => _mst.Maparea_id;

		public string Name => _mst.Name;

		public string Description => _mst.Details;

		public int Turn => _mst.Time;

		public int Difficulty => _mst.Difficulty;

		public double UseFuel => _mst.Use_fuel;

		public double UseAmmo => _mst.Use_bull;

		public int TankerMinCount => 0;

		public int TankerMaxCount => _mst.Tanker_num_max;

		public int TankerCount => _mst.Tanker_num;

		public int CompleteTurn => (_deck != null) ? _deck.MissionCompleteTurn : 0;

		public MissionModel(User_MissionFmt fmt)
		{
			_id = fmt.MissionId;
			_state = fmt.State;
			_mst = Mst_DataManager.Instance.Mst_mission[_id];
		}

		public MissionModel(User_MissionFmt fmt, DeckModel deck)
		{
			_id = fmt.MissionId;
			_state = fmt.State;
			_mst = Mst_DataManager.Instance.Mst_mission[_id];
			_deck = deck;
		}

		public MaterialInfo GetRewardMaterials()
		{
			MaterialInfo result = default(MaterialInfo);
			result.Fuel = _mst.Win_mat1;
			result.Ammo = _mst.Win_mat2;
			result.Steel = _mst.Win_mat3;
			result.Baux = _mst.Win_mat4;
			return result;
		}

		public List<Reward_Useitem> GetRewardUseitems()
		{
			List<Reward_Useitem> list = new List<Reward_Useitem>();
			if (_mst.Win_item1 > 0)
			{
				list.Add(new Reward_Useitem(_mst.Win_item1, _mst.Win_item1_num));
			}
			if (_mst.Win_item2 > 0)
			{
				list.Add(new Reward_Useitem(_mst.Win_item2, _mst.Win_item2_num));
			}
			return list;
		}

		public override string ToString()
		{
			string str = $"{Name}(ID:{Id}) 状態:{State} 海域:{AreaId} {Turn}タ\u30fcン";
			if (TankerMaxCount - TankerMinCount > 0)
			{
				str += $" 必要輸送船数:{TankerMinCount}-{TankerMaxCount}";
			}
			if (Deck != null)
			{
				string arg = "?";
				if (Deck.MissionState == MissionStates.RUNNING)
				{
					arg = "遠征中";
				}
				else if (Deck.MissionState == MissionStates.END)
				{
					arg = "遠征完了";
				}
				else if (Deck.MissionState == MissionStates.STOP)
				{
					arg = "遠征中止";
				}
				str += $" [[艦隊{Deck.Id} {arg} 終了タ\u30fcン:{Deck.MissionCompleteTurn}]]\n";
			}
			else
			{
				str += $"\n";
			}
			str += $"\t{Description}\n";
			str += $"\t難易度:{Difficulty} 消費資材:{UseFuel}/{UseAmmo}  ";
			MaterialInfo rewardMaterials = GetRewardMaterials();
			str += $"報酬４資材 {rewardMaterials.Fuel}/{rewardMaterials.Ammo}/{rewardMaterials.Steel}/{rewardMaterials.Baux}  ";
			List<Reward_Useitem> rewardUseitems = GetRewardUseitems();
			for (int i = 0; i < rewardUseitems.Count; i++)
			{
				str += $"報酬アイテム{i + 1}:{rewardUseitems[i]}  ";
			}
			return str + "\n";
		}
	}
}
