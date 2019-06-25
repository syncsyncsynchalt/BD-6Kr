using Common.Enum;
using local.managers;
using local.utils;
using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace local.models
{
	public class MapAreaModel
	{
		private UserInfoModel _userInfo;

		private User_StrategyMapFmt _strategy_fmt;

		private List<Mem_ndock> _ndocks;

		private _TankerManager _tship_manager;

		public int Id => _strategy_fmt.Maparea.Id;

		public string Name => _strategy_fmt.Maparea.Name;

		public int NDockMax => _strategy_fmt.Maparea.Ndocks_max;

		public int NDockCount => _ndocks.Count;

		public int NDockCountEmpty => _ndocks.FindAll((Mem_ndock state) => state.State == NdockStates.EMPTY).Count;

		public RebellionState RState => _strategy_fmt.RebellionState;

		public string Description => string.Format("{0}{0}{0}{0}{0}{0}{0}", "海域詳細情報");

		public List<int> NeighboringAreaIDs => _strategy_fmt.Maparea.Neighboring_area.GetRange(0, _strategy_fmt.Maparea.Neighboring_area.Count);

		public MapAreaModel(UserInfoModel user_info, User_StrategyMapFmt strategy_fmt, Dictionary<int, List<Mem_ndock>> ndock_dic, _TankerManager tship_manager)
		{
			__Update__(user_info, strategy_fmt, ndock_dic, tship_manager);
		}

		public bool IsOpen()
		{
			return _strategy_fmt.IsActiveArea;
		}

		public DeckModel[] GetDecks()
		{
			return _userInfo.GetDecksFromArea(Id);
		}

		public EscortDeckModel GetEscortDeck()
		{
			return _userInfo.GetEscortDeck(Id);
		}

		public List<ShipModel> GetRepairingShips()
		{
			List<ShipModel> list = new List<ShipModel>();
			for (int i = 0; i < _ndocks.Count; i++)
			{
				if (_ndocks[i].State == NdockStates.RESTORE)
				{
					list.Add(_userInfo.GetShip(_ndocks[i].Ship_id));
				}
			}
			return list;
		}

		public AreaTankerModel GetTankerCount()
		{
			return _tship_manager.GetCounts(Id);
		}

		public List<NdockStates> GetNDockStateList()
		{
			List<NdockStates> list = (from dock in _ndocks
				select dock.State).ToList();
			while (list.Count < NDockMax)
			{
				list.Add(NdockStates.NOTUSE);
			}
			return list;
		}

		[Obsolete("local.utils.Utils.GetAreaResource(int area_id, int tanker_count, EscortDeckManager eManager) を使用してください", false)]
		public Dictionary<enumMaterialCategory, int> GetResources(int tanker_count)
		{
			return local.utils.Utils.GetAreaResource(Id, tanker_count);
		}

		public void __Update__(UserInfoModel user_info, User_StrategyMapFmt strategy_fmt, Dictionary<int, List<Mem_ndock>> ndock_dic, _TankerManager tship_manager)
		{
			_userInfo = user_info;
			_strategy_fmt = strategy_fmt;
			if (ndock_dic.ContainsKey(Id))
			{
				_ndocks = ndock_dic[Id];
			}
			else
			{
				_ndocks = new List<Mem_ndock>();
			}
			_tship_manager = tship_manager;
		}

		public void __UpdateNdockData__(List<Mem_ndock> ndocks)
		{
			_ndocks = ndocks;
		}

		public HashSet<int> __GetRepairingShipMemIdsHash__()
		{
			HashSet<int> hashSet = new HashSet<int>();
			for (int i = 0; i < _ndocks.Count; i++)
			{
				if (_ndocks[i].State == NdockStates.RESTORE)
				{
					hashSet.Add(_ndocks[i].Ship_id);
				}
			}
			return hashSet;
		}

		public override string ToString()
		{
			string str = $"[海域{Id}:{Name}]";
			Comm_UserDatas.Instance.User_rebellion_point.TryGetValue(Id, out Mem_rebellion_point value);
			str += $" RP:{RState}({value?.Point ?? 0})";
			if (!IsOpen())
			{
				str += $" [未開放] ";
			}
			AreaTankerModel tankerCount = GetTankerCount();
			str += $" 輸送船:{tankerCount.GetCountNoMove()}/{tankerCount.GetMaxCount()}(移動中:{tankerCount.GetCountMove()},遠征中:{tankerCount.GetCountInMission()}) ";
			str += $"入渠ドック: {ToString(GetNDockStateList())} ";
			DeckModel[] decks = GetDecks();
			if (decks.Length == 0)
			{
				str += $"海域に艦隊無し";
			}
			else
			{
				foreach (DeckModel arg in decks)
				{
					str += $"{arg}";
				}
			}
			EscortDeckModel escortDeck = GetEscortDeck();
			if (escortDeck == null)
			{
				return str + $" 護衛艦隊無し";
			}
			return str + $" {escortDeck}";
		}

		public string ToString(List<NdockStates> stateList)
		{
			string text = "[";
			for (int i = 0; i < stateList.Count; i++)
			{
				text += stateList[i];
				if (i < stateList.Count - 1)
				{
					text += ", ";
				}
			}
			return text + "]";
		}
	}
}
