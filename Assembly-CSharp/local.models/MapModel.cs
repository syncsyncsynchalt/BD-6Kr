using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class MapModel
	{
		private Mst_mapinfo _mst_map_data;

		private User_MapinfoFmt _mem_map_data;

		private MapHPModel _mapHP;

		public int MstId => _mst_map_data.Id;

		public int AreaId => _mst_map_data.Maparea_id;

		public int No => _mst_map_data.No;

		public string Name => _mst_map_data.Name;

		public int Level => _mst_map_data.Level;

		public string Opetext => _mst_map_data.Opetext;

		public string Infotext => _mst_map_data.Infotext;

		public bool Cleared => _mem_map_data != null && _mem_map_data.Cleared;

		public bool ClearedOnce
		{
			get
			{
				if (_mem_map_data == null)
				{
					return false;
				}
				if (!Comm_UserDatas.Instance.User_mapclear.TryGetValue(MstId, out Mem_mapclear value))
				{
					return false;
				}
				return value.Cleared;
			}
		}

		public MapHPModel MapHP => _mapHP;

		public bool Map_Possible => _mem_map_data != null && _mem_map_data.IsGo;

		public MapModel(Mst_mapinfo mst_map, User_MapinfoFmt mem_map)
		{
			_mst_map_data = mst_map;
			_mem_map_data = mem_map;
			_mapHP = new MapHPModel(_mst_map_data, _mem_map_data);
			if (_mapHP.MaxValue == 0)
			{
				_mapHP = null;
			}
		}

		public int[] GetRewardItemIds()
		{
			List<int> list = new List<int>();
			list.Add(_mst_map_data.Item1);
			list.Add(_mst_map_data.Item2);
			list.Add(_mst_map_data.Item3);
			list.Add(_mst_map_data.Item4);
			List<int> list2 = list;
			return list2.FindAll((int itemID) => itemID > 0).ToArray();
		}

		public string ToShortString()
		{
			return string.Format("#{0}-{1}(Id:{2})  {3} {4} {5} {6}", AreaId, No, MstId, Name, (!Cleared) ? string.Empty : "[Cleared]", (!Map_Possible) ? "[出撃不可]" : string.Empty, (MapHP == null) ? string.Empty : MapHP.ToString());
		}

		public override string ToString()
		{
			string text = Name + "(ID:" + MstId.ToString() + ")\n";
			string text2 = text;
			text = text2 + "マスタID:" + MstId + "  海域ID:" + AreaId + "  マップ番号:" + No + "  地名:" + Name + "\n";
			text2 = text;
			text = text2 + "作戦内容:" + Opetext + "    エリア情報:" + Infotext + "\n";
			text += $"レベル:{Level}    取得可能アイテム:";
			int[] rewardItemIds = GetRewardItemIds();
			for (int i = 0; i < rewardItemIds.Length; i++)
			{
				text += $"{rewardItemIds[i]}";
				text = ((i != rewardItemIds.Length - 1) ? (text + "/") : (text + "\n"));
			}
			text2 = text;
			text = text2 + "クリア状態:" + Cleared + "    出撃可能なマップ(マップ表示の有無):" + Map_Possible + "\n";
			if (MapHP != null)
			{
				text = text + MapHP + "\n";
			}
			return text;
		}
	}
}
