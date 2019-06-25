using Common.Enum;
using Server_Models;
using System.Collections.Generic;
using System.Linq;

namespace Server_Common.Formats
{
	public class Map_ResultFmt
	{
		public CompassType Rashin_id;

		public int Cell_no;

		public List<int> SelectCells;

		public int Color_no;

		public enumMapEventType Event_id;

		public enumMapWarType Event_kind;

		public bool IsNext;

		public MapCommentKind Comment;

		public MapProductionKind Production;

		public AirReconnaissanceFmt AirReconnaissance;

		public MapItemGetFmt ItemGet;

		public List<MapItemGetFmt> MapClearItem;

		public MapHappningFmt Happning;

		public EventMapInfo MapHp;

		public List<int> NewOpenMapId;

		public int GetSpoint;

		public MapSupplyFmt MapSupply;

		public Map_ResultFmt()
		{
			Rashin_id = CompassType.None;
			Cell_no = 0;
			Color_no = 0;
			Event_id = enumMapEventType.None;
			Event_kind = enumMapWarType.None;
			IsNext = false;
			Comment = MapCommentKind.None;
			Production = MapProductionKind.None;
			GetSpoint = 0;
		}

		public void SetMember(CompassType rashin_id, Mst_mapcell2 target_cell, MapItemGetFmt item, List<MapItemGetFmt> clearItems, MapHappningFmt happning, MapCommentKind comment, MapProductionKind production, AirReconnaissanceFmt airSearch, EventMapInfo eventMap, List<int> selectcell, List<int> newOpenMap, int spoint)
		{
			Rashin_id = rashin_id;
			Cell_no = target_cell.No;
			if (selectcell != null)
			{
				SelectCells = selectcell.ToList();
			}
			Color_no = target_cell.Color_no;
			Event_id = target_cell.Event_1;
			Event_kind = target_cell.Event_2;
			IsNext = target_cell.IsNext();
			Comment = comment;
			Production = production;
			AirReconnaissance = airSearch;
			ItemGet = item;
			MapClearItem = clearItems;
			Happning = happning;
			MapHp = eventMap;
			NewOpenMapId = newOpenMap;
			GetSpoint = spoint;
		}
	}
}
