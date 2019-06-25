using Common.Enum;
using local.models;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public abstract class MapManager : ManagerBase
	{
		protected DeckModel _deck;

		protected DeckModel _mainDeck;

		protected DeckModel _subDeck;

		protected MapModel _map;

		protected List<MapModel> _maps;

		protected Api_req_Map _req_map;

		protected List<CellModel> _cells;

		protected int _now_cell_no;

		protected Map_ResultFmt _next_cell;

		protected List<int> _passed;

		protected List<MapEventItemModel> _items;

		public DeckModel Deck => _deck;

		public MapModel Map => _map;

		public CellModel[] Cells => _cells.ToArray();

		public List<int> Passed => _passed.GetRange(0, _passed.Count);

		public List<MapEventItemModel> Items => _items.GetRange(0, _items.Count);

		public CellModel NowCell => _cells[_now_cell_no];

		public CellModel NextCell => _cells[_next_cell.Cell_no];

		public CompassType CompassId => _next_cell.Rashin_id;

		public MapCommentKind Comment => _next_cell.Comment;

		public MapProductionKind Production => _next_cell.Production;

		public enumMapEventType NextCategory => _next_cell.Event_id;

		public enumMapWarType NextEventType => _next_cell.Event_kind;

		public int BgmId => Mst_DataManager.Instance.UiBattleMaster.MapBgm;

		public MapManager(DeckModel deck, MapModel map, List<MapModel> maps)
		{
			_deck = deck;
			_map = map;
			_maps = maps;
			_req_map = new Api_req_Map();
			_cells = new List<CellModel>();
			_items = new List<MapEventItemModel>();
			Comm_UserDatas.Instance.User_trophy.Start_map_count++;
		}

		public MapManager(DeckModel deck, MapModel map)
		{
			_deck = deck;
			_map = map;
			_maps = null;
			_req_map = new Api_req_Map();
			_cells = new List<CellModel>();
			_items = new List<MapEventItemModel>();
		}

		public virtual TurnState MapEnd()
		{
			TurnState result = _req_map.SortieEnd();
			Api_get_Member api_get_mem = new Api_get_Member();
			base.UserInfo.__UpdateShips__(api_get_mem);
			return result;
		}

		public bool hasCompass()
		{
			return CompassId != CompassType.None;
		}

		public bool HasAirReconnaissance()
		{
			return _req_map.AirReconnaissanceItems != null;
		}

		public List<CellModel> GetNextCellCandidate()
		{
			List<CellModel> list = new List<CellModel>();
			if (_next_cell.SelectCells == null)
			{
				return list;
			}
			int i;
			for (i = 0; i < _next_cell.SelectCells.Count; i++)
			{
				CellModel item = _cells.Find((CellModel cell) => cell.CellNo == _next_cell.SelectCells[i]);
				list.Add(item);
			}
			return list;
		}

		public bool IsNextFinal()
		{
			return !_next_cell.IsNext;
		}

		public bool GoNext(ShipRecoveryType recovery_type)
		{
			return GoNext(recovery_type, -1);
		}

		public bool GoNext(ShipRecoveryType recovery_type, int selected_next_cell_no)
		{
			Api_get_Member api_get_mem = new Api_get_Member();
			base.UserInfo.__UpdateDeck__(api_get_mem);
			Api_Result<Map_ResultFmt> api_Result = (selected_next_cell_no == -1) ? _req_map.Next(recovery_type) : _req_map.Next(recovery_type, selected_next_cell_no);
			if (api_Result.state == Api_Result_State.Success)
			{
				_now_cell_no = NextCell.CellNo;
				_next_cell = api_Result.data;
				_passed.Add(_now_cell_no);
				if (_map.MapHP != null && api_Result.data.MapHp != null)
				{
					_map.MapHP.__Update__(api_Result.data.MapHp);
				}
				if (_next_cell.Event_id == enumMapEventType.ItemGet)
				{
					_items.Add(GetItemEvent());
				}
				return true;
			}
			return false;
		}

		public int GetNextCellEnemyId()
		{
			if (NextCategory != enumMapEventType.War_Normal && NextCategory != enumMapEventType.War_Boss)
			{
				return 0;
			}
			return _req_map.Enemy_Id;
		}

		public string GetNextCellEnemyFleetName()
		{
			if (NextCategory != enumMapEventType.War_Normal && NextCategory != enumMapEventType.War_Boss)
			{
				return string.Empty;
			}
			return _req_map.GetEnemyShipNames();
		}

		public List<ShipModelMst> GetNextCellEnemys()
		{
			if (NextCategory != enumMapEventType.War_Normal && NextCategory != enumMapEventType.War_Boss)
			{
				return null;
			}
			List<int> enemyShipIds = _req_map.GetEnemyShipIds();
			if (enemyShipIds == null)
			{
				return null;
			}
			List<ShipModelMst> list = new List<ShipModelMst>();
			for (int i = 0; i < enemyShipIds.Count; i++)
			{
				int num = enemyShipIds[i];
				if (num <= 0)
				{
					list.Add(null);
				}
				else
				{
					list.Add(new ShipModelMst(num));
				}
			}
			return list;
		}

		public MapSupplyModel GetMapSupplyInfo()
		{
			if (_next_cell.MapSupply == null)
			{
				return null;
			}
			return new MapSupplyModel(Deck, _next_cell.MapSupply);
		}

		public SortieBattleManagerBase BattleStart(BattleFormationKinds1 formation_id)
		{
			return null;
		}

		public SortieBattleManagerBase BattleStart_Write(BattleFormationKinds1 formation_id)
		{
			return null;
		}

		public SortieBattleManagerBase BattleStart_Read()
		{
			return null;
		}

		public MapEventItemModel GetItemEvent()
		{
			return new MapEventItemModel(_next_cell.ItemGet);
		}

		public MapEventHappeningModel GetHappeningEvent()
		{
			return new MapEventHappeningModel(_next_cell.Happning);
		}

		public MapEventAirReconnaissanceModel GetAirReconnaissanceEvent()
		{
			return new MapEventAirReconnaissanceModel(_next_cell.ItemGet, _next_cell.AirReconnaissance);
		}

		public List<IReward> GetMapClearItems()
		{
			if (_next_cell.MapClearItem == null)
			{
				return null;
			}
			List<IReward> list = new List<IReward>();
			for (int i = 0; i < _next_cell.MapClearItem.Count; i++)
			{
				IReward reward = null;
				MapItemGetFmt mapItemGetFmt = _next_cell.MapClearItem[i];
				switch (mapItemGetFmt.Category)
				{
				case MapItemGetFmt.enumCategory.Furniture:
					reward = new Reward_Furniture(mapItemGetFmt.Id);
					break;
				case MapItemGetFmt.enumCategory.Slotitem:
					reward = new Reward_Slotitem(mapItemGetFmt.Id, mapItemGetFmt.GetCount);
					break;
				case MapItemGetFmt.enumCategory.Ship:
					reward = new Reward_Ship(mapItemGetFmt.Id);
					break;
				case MapItemGetFmt.enumCategory.Material:
					reward = new Reward_Material((enumMaterialCategory)mapItemGetFmt.Id, mapItemGetFmt.GetCount);
					break;
				case MapItemGetFmt.enumCategory.UseItem:
					reward = new Reward_Useitem(mapItemGetFmt.Id, mapItemGetFmt.GetCount);
					break;
				}
				if (reward != null)
				{
					list.Add(reward);
				}
			}
			return list;
		}

		public int[] GetNewOpenAreaIDs()
		{
			if (_next_cell.NewOpenMapId == null)
			{
				return null;
			}
			List<int> list = _next_cell.NewOpenMapId.FindAll((int id) => id % 10 == 1);
			if (list.Count == 0)
			{
				return null;
			}
			return list.ConvertAll((int val) => (int)Math.Floor((double)val / 10.0)).ToArray();
		}

		public int[] GetNewOpenMapIDs()
		{
			if (_next_cell.NewOpenMapId == null || _next_cell.NewOpenMapId.Count == 0)
			{
				return null;
			}
			return _next_cell.NewOpenMapId.ToArray();
		}

		public int GetSPoint()
		{
			return _next_cell.GetSpoint;
		}

		public void ChangeCurrentDeck()
		{
			if (_mainDeck != null && _deck == _subDeck && _req_map.ChangeLeadingDeck())
			{
				_deck = _mainDeck;
			}
		}

		protected abstract void _Init();

		public override string ToString()
		{
			string empty = string.Empty;
			empty += $"---[マップ{Map.AreaId}-{Map.No}(ID:{Map.MstId}) - 「{Map.Name}」への出撃]---\n";
			empty += $"{Map}";
			empty += $"出撃艦隊:{Deck}\n";
			return empty + $"セル総数:{_cells.Count} BGM:{BgmId}";
		}
	}
}
