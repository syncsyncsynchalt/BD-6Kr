using Common.Enum;
using local.models;
using Server_Common.Formats;
using Server_Controllers;
using System.Collections.Generic;

namespace local.managers
{
	public class RebellionMapManager : MapManager
	{
		public DeckModel Deck_Main => _mainDeck;

		public DeckModel Deck_Sub => _subDeck;

		public RebellionMapManager(MapModel map, DeckModel mainDeck, DeckModel subDeck)
			: base(mainDeck, map)
		{
			_mainDeck = mainDeck;
			_subDeck = subDeck;
			if (_subDeck == null)
			{
				_deck = _mainDeck;
			}
			else
			{
				_deck = _subDeck;
			}
			_Init();
		}

		public new RebellionBattleManager BattleStart(BattleFormationKinds1 formation_id)
		{
			Api_req_SortieBattle reqBattle = new Api_req_SortieBattle(_req_map);
			bool isBoss = base.NextCategory == enumMapEventType.War_Boss;
			string nextCellEnemyFleetName = GetNextCellEnemyFleetName();
			RebellionBattleManager rebellionBattleManager = new RebellionBattleManager(nextCellEnemyFleetName);
			rebellionBattleManager.__Init__(reqBattle, base.NextEventType, formation_id, _map, IsNextFinal(), isBoss, _deck == _subDeck);
			return rebellionBattleManager;
		}

		public new RebellionBattleManager BattleStart_Write(BattleFormationKinds1 formation_id)
		{
			DebugBattleMaker.SerializeBattleStart();
			Api_req_SortieBattle reqBattle = new Api_req_SortieBattle(_req_map);
			bool isBoss = base.NextCategory == enumMapEventType.War_Boss;
			string nextCellEnemyFleetName = GetNextCellEnemyFleetName();
			RebellionBattleManager_Write rebellionBattleManager_Write = new RebellionBattleManager_Write(nextCellEnemyFleetName);
			rebellionBattleManager_Write.__Init__(reqBattle, base.NextEventType, formation_id, _map, IsNextFinal(), isBoss, _deck == _subDeck);
			return rebellionBattleManager_Write;
		}

		public new RebellionBattleManager BattleStart_Read()
		{
			bool is_boss = base.NextCategory == enumMapEventType.War_Boss;
			return new RebellionBattleManager_Read(is_boss, IsNextFinal(), _map);
		}

		public bool RebellionEnd()
		{
			bool flag = _req_map.RebellionEnd();
			if (flag)
			{
				base.UserInfo.__UpdateEscortDeck__(new Api_get_Member());
			}
			_deck = (_mainDeck = (_subDeck = null));
			_map = null;
			_cells = null;
			_next_cell = null;
			_req_map = null;
			_passed = null;
			return flag;
		}

		protected override void _Init()
		{
			Api_Result<Map_ResultFmt> api_Result = (_subDeck != null) ? _req_map.StartResisted(base.Map.AreaId, _subDeck.Id, _mainDeck.Id) : _req_map.StartResisted(base.Map.AreaId, _mainDeck.Id, 0);
			if (api_Result.state == Api_Result_State.Success)
			{
				_next_cell = api_Result.data;
				Dictionary<int, User_MapCellInfo> user_mapcell = _req_map.User_mapcell;
				foreach (int key in user_mapcell.Keys)
				{
					CellModel item = new CellModel(user_mapcell[key]);
					_cells.Add(item);
				}
				_cells.Sort((CellModel a, CellModel b) => a.CellNo.CompareTo(b.CellNo));
			}
			_passed = new List<int>();
		}
	}
}
