using Common.Enum;
using local.models;
using local.utils;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.managers
{
	public class SortieManager : ManagerBase
	{
		private int _area_id;

		private List<MapModel> _maps;

		public MapAreaModel MapArea => ManagerBase._area[_area_id];

		public MapModel[] Maps => _maps.ToArray();

		public SortieManager(int area_id)
		{
			_area_id = area_id;
			Dictionary<int, Mst_mapinfo> mst_mapinfo = Mst_DataManager.Instance.Mst_mapinfo;
			Api_Result<Dictionary<int, User_MapinfoFmt>> api_Result = new Api_get_Member().Mapinfo();
			Dictionary<int, User_MapinfoFmt> dictionary = (api_Result.state != 0) ? new Dictionary<int, User_MapinfoFmt>() : api_Result.data;
			_maps = new List<MapModel>();
			foreach (User_MapinfoFmt value in dictionary.Values)
			{
				Mst_mapinfo mst_mapinfo2 = mst_mapinfo[value.Id];
				if (mst_mapinfo2.Maparea_id == area_id)
				{
					MapModel item = new MapModel(mst_mapinfo2, value);
					_maps.Add(item);
				}
			}
			_maps.Sort((MapModel x, MapModel y) => (x.MstId > y.MstId) ? 1 : (-1));
		}

		public List<IsGoCondition> IsGoSortie(int deck_id, int map_id)
		{
			DeckModel deck = base.UserInfo.GetDeck(deck_id);
			List<IsGoCondition> list = deck.IsValidSortie();
			if (deck.AreaId != MapArea.Id)
			{
				list.Add(IsGoCondition.AnotherArea);
			}
			if (_maps.Find((MapModel map) => map.MstId == map_id) == null)
			{
				list.Add(IsGoCondition.Invalid);
			}
			ShipModel[] ships = deck.GetShips();
			HashSet<SType> sortieLimit = Utils.GetSortieLimit(map_id, is_permitted: true);
			if (sortieLimit != null)
			{
				for (int i = 0; i < ships.Length; i++)
				{
					SType shipType = (SType)ships[i].ShipType;
					if (!sortieLimit.Contains(shipType))
					{
						list.Add(IsGoCondition.InvalidOrganization);
						break;
					}
				}
			}
			if (!list.Contains(IsGoCondition.InvalidOrganization))
			{
				HashSet<SType> sortieLimit2 = Utils.GetSortieLimit(map_id, is_permitted: false);
				if (sortieLimit2 != null)
				{
					for (int j = 0; j < ships.Length; j++)
					{
						SType shipType2 = (SType)ships[j].ShipType;
						if (sortieLimit2.Contains(shipType2))
						{
							list.Add(IsGoCondition.InvalidOrganization);
							break;
						}
					}
				}
			}
			return list;
		}

		public SortieMapManager GoSortie(int deck_id, int map_id)
		{
			DeckModel deck = base.UserInfo.GetDeck(deck_id);
			System.Console.WriteLine($"SortieManager.GoSortie: deck_id={deck_id}, map_id={map_id}");

			MapModel map = _maps.Find((MapModel m) => m.MstId == map_id);
			if (map == null)
			{
				System.Console.WriteLine($"Map with MstId={map_id} not found!");
				throw new System.Exception($"Map with MstId={map_id} not found in _maps list");
			}

			return new SortieMapManager(deck, map, _maps);
		}

		public override string ToString()
		{
			string empty = string.Empty;
			empty += $"{base.ToString()}\n";
			empty += $"\n--この海域のマップ一覧--\n";
			for (int i = 0; i < Maps.Length; i++)
			{
				empty += $"{Maps[i]}\n";
			}
			empty += $"\n--この海域の艦隊一覧--\n";
			for (int j = 0; j < MapArea.GetDecks().Length; j++)
			{
				empty += $"{MapArea.GetDecks()[j]}\n";
			}
			return empty;
		}
	}
}
