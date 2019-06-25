using Common.Enum;
using local.models;
using local.utils;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class EndingManager : ManagerBase
	{
		private Api_req_Ending _api;

		private bool _defeat_mode;

		private SortKey _selected_sort = SortKey.LEVEL_LOCK;

		private List<Mem_shipBase> _cache_takeover_ships;

		private List<Mem_slotitem> _cache_takeover_slots;

		public EndingManager()
		{
			_defeat_mode = Server_Common.Utils.IsGameOver();
			_api = new Api_req_Ending();
		}

		public EndingManager(bool is_defeat)
		{
			_defeat_mode = is_defeat;
			_api = new Api_req_Ending();
		}

		public List<User_HistoryFmt> CreateHistoryRawData()
		{
			Api_Result<List<User_HistoryFmt>> api_Result = new Api_get_Member().HistoryList();
			if (api_Result.state == Api_Result_State.Success)
			{
				return api_Result.data;
			}
			return new List<User_HistoryFmt>();
		}

		public List<HistoryModelBase> CreateHistoryData()
		{
			List<HistoryModelBase> list = new List<HistoryModelBase>();
			Api_Result<List<User_HistoryFmt>> api_Result = new Api_get_Member().HistoryList();
			if (api_Result.state == Api_Result_State.Success)
			{
				for (int i = 0; i < api_Result.data.Count; i++)
				{
					User_HistoryFmt user_HistoryFmt = api_Result.data[i];
					if (user_HistoryFmt.Type == HistoryType.MapClear1 || user_HistoryFmt.Type == HistoryType.MapClear2 || user_HistoryFmt.Type == HistoryType.MapClear3)
					{
						list.Add(new HistoryModel_AreaClear(user_HistoryFmt));
					}
					else if (user_HistoryFmt.Type == HistoryType.NewAreaOpen)
					{
						list.Add(new HistoryModel_AreaStart(user_HistoryFmt));
					}
					else if (user_HistoryFmt.Type == HistoryType.TankerLostAll || user_HistoryFmt.Type == HistoryType.TankerLostHalf)
					{
						list.Add(new HistoryModel_TransportCraft(user_HistoryFmt));
					}
					else if (user_HistoryFmt.Type == HistoryType.GameClear || user_HistoryFmt.Type == HistoryType.GameOverLost || user_HistoryFmt.Type == HistoryType.GameOverTurn)
					{
						list.Add(new HistoryModel_GameEnd(user_HistoryFmt));
					}
				}
			}
			return list;
		}

		public bool IsGoTrueEnd()
		{
			return _api.IsGoTrueEnd();
		}

		public List<ShipModel> CreateShipList()
		{
			return CreateShipList(20);
		}

		public List<ShipModel> CreateShipList(int count)
		{
			List<ShipModel> list = base.UserInfo.__GetShipList__();
			if (_cache_takeover_ships == null || _cache_takeover_ships.Count > 0)
			{
			}
			count = Math.Min(count, list.Count);
			return DeckUtil.GetSortedList(list, SortKey.LEVEL).GetRange(0, count);
		}

		public int GetTakeoverShipCountMax()
		{
			if (_defeat_mode)
			{
				return 0;
			}
			return _api.GetTakeOverShipCount();
		}

		public int GetTakeoverSlotCountMax()
		{
			if (_defeat_mode)
			{
				return 0;
			}
			return _api.GetTakeOverSlotCount();
		}

		public int GetUserShipCount()
		{
			return Comm_UserDatas.Instance.User_ship.Count;
		}

		public int GetTakeoverShipCount()
		{
			return Math.Min(GetTakeoverShipCountMax(), GetUserShipCount());
		}

		public bool CreatePlusData(bool is_level_sort)
		{
			if (_cache_takeover_ships != null)
			{
				return false;
			}
			_api.CreateNewGamePlusData(is_level_sort);
			_cache_takeover_ships = _api.GetTakeOverShips();
			_cache_takeover_slots = _api.GetTakeOverSlotItems();
			_selected_sort = ((!is_level_sort) ? SortKey.LOCK_LEVEL : SortKey.LEVEL_LOCK);
			return true;
		}

		public bool DeletePlusData()
		{
			if (_cache_takeover_ships != null)
			{
				return false;
			}
			_api.PurgeNewGamePlus();
			_cache_takeover_ships = new List<Mem_shipBase>();
			_cache_takeover_slots = new List<Mem_slotitem>();
			return true;
		}

		public uint GetLostShipCount()
		{
			return Comm_UserDatas.Instance.User_record.LostShipNum;
		}

		public void CalculateTotalRank(out OverallRank rank, out int decorationValue)
		{
			bool overallRank = _api.GetOverallRank(out rank, out decorationValue);
			decorationValue *= (overallRank ? 1 : (-1));
		}

		public DifficultKind? GetOpenedDifficulty()
		{
			if (!Server_Common.Utils.IsGameClear())
			{
				return null;
			}
			if (_cache_takeover_ships == null)
			{
				return null;
			}
			if (_cache_takeover_ships.Count == 0)
			{
				return null;
			}
			DifficultKind difficulty = base.UserInfo.Difficulty;
			DifficultKind value;
			switch (difficulty)
			{
			case DifficultKind.OTU:
				value = DifficultKind.KOU;
				break;
			case DifficultKind.KOU:
				value = DifficultKind.SHI;
				break;
			default:
				return null;
			}
			int num = Comm_UserDatas.Instance.User_plus.ClearNum(difficulty);
			if (num == 1)
			{
				return value;
			}
			return null;
		}

		public override string ToString()
		{
			string empty = string.Empty;
			empty += ((!_defeat_mode) ? "勝利" : "敗戦");
			empty += "\n";
			List<HistoryModelBase> list = CreateHistoryData();
			if (list.Count > 0)
			{
				empty += " == 年表 ==\n";
			}
			for (int i = 0; i < list.Count; i++)
			{
				empty = empty + "- " + list[i].ToString() + "\n";
			}
			if (list.Count > 0)
			{
				empty += " == 年表終わり ==\n";
			}
			DifficultKind? openedDifficulty = GetOpenedDifficulty();
			if (openedDifficulty.HasValue)
			{
				empty += $" [難易度] 新しい難易度{openedDifficulty}が開放しました。\n";
			}
			List<ShipModel> list2 = CreateShipList();
			if (list2.Count > 0)
			{
				empty += " == 上位20隻の艦 ==\n";
			}
			for (int j = 0; j < list2.Count; j++)
			{
				ShipModel shipModel = list2[j];
				empty += $"{j + 1:D3}: {shipModel.Name}(mst:{shipModel.MstId}, mem:{shipModel.MemId}) Lv{shipModel.Level} sortNo:{shipModel.SortNo}\n";
			}
			if (list2.Count > 0)
			{
				empty += " == 上位20隻の艦終わり ==\n";
			}
			if (!_defeat_mode)
			{
				empty += string.Format("{0}作戦 勝利\n", (new string[6]
				{
					string.Empty,
					"丁",
					"丙",
					"乙",
					"甲",
					"史"
				})[(int)base.UserInfo.Difficulty]);
				empty += $"作戦日数 : {base.Turn}日\n";
				empty += $"喪失艦娘 : {GetLostShipCount()}隻\n";
				CalculateTotalRank(out OverallRank rank, out int decorationValue);
				string arg = (new string[3]
				{
					string.Empty,
					"+",
					"++"
				})[decorationValue];
				empty += $"\t総合評価 : {rank}{arg}\n";
				empty += "\n";
				if (_cache_takeover_ships == null)
				{
					empty += $"= 引き継ぎ未選択 =\n";
					empty += string.Format("引き継ぎ可能な艦:{0}隻, 装備:{1}個\n", GetTakeoverShipCount(), "?");
				}
				else
				{
					empty += $"= 引き継ぎ選択済 =\n";
					empty += $"引き継ぎ可能な艦:{_cache_takeover_ships.Count}隻, 装備:{_cache_takeover_slots.Count}個\n";
				}
			}
			return empty;
		}
	}
}
