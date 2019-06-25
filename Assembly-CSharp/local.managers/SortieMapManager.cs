using Common.Enum;
using local.models;
using Server_Common.Formats;
using Server_Controllers;
using System.Collections.Generic;

namespace local.managers
{
	public class SortieMapManager : MapManager
	{
		public SortieMapManager(DeckModel deck, MapModel map, List<MapModel> maps)
			: base(deck, map, maps)
		{
			_Init();
		}

		public new virtual SortieBattleManager BattleStart(BattleFormationKinds1 formationId)
		{
			Api_req_SortieBattle reqBattle = new Api_req_SortieBattle(_req_map);
			bool isBoss = base.NextCategory == enumMapEventType.War_Boss;
			string nextCellEnemyFleetName = GetNextCellEnemyFleetName();
			SortieBattleManager sortieBattleManager = new SortieBattleManager(nextCellEnemyFleetName);
			sortieBattleManager.__Init__(reqBattle, base.NextEventType, formationId, _map, _maps, IsNextFinal(), isBoss);
			return sortieBattleManager;
		}

		public new SortieBattleManager BattleStart_Write(BattleFormationKinds1 formationId)
		{
			DebugBattleMaker.SerializeBattleStart();
			Api_req_SortieBattle reqBattle = new Api_req_SortieBattle(_req_map);
			bool isBoss = base.NextCategory == enumMapEventType.War_Boss;
			string nextCellEnemyFleetName = GetNextCellEnemyFleetName();
			SortieBattleManager_Write sortieBattleManager_Write = new SortieBattleManager_Write(nextCellEnemyFleetName);
			sortieBattleManager_Write.__Init__(reqBattle, base.NextEventType, formationId, _map, _maps, IsNextFinal(), isBoss);
			return sortieBattleManager_Write;
		}

		public new SortieBattleManager BattleStart_Read()
		{
			bool is_boss = base.NextCategory == enumMapEventType.War_Boss;
			return new SortieBattleManager_Read(is_boss, IsNextFinal(), _map);
		}

		public override TurnState MapEnd()
		{
			TurnState result = base.MapEnd();
			_deck = (_mainDeck = (_subDeck = null));
			_map = null;
			_cells = null;
			_next_cell = null;
			_req_map = null;
			_passed = null;
			return result;
		}

		protected override void _Init()
		{
			Api_Result<Map_ResultFmt> api_Result = _req_map.Start(base.Map.AreaId, base.Map.No, base.Deck.Id);
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
