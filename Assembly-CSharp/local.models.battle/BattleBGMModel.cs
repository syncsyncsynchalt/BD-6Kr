using System.Collections.Generic;

namespace local.models.battle
{
	public class BattleBGMModel
	{
		private int _area_id;

		private int _map_no;

		private Dictionary<int, List<int>> _mst;

		public int BGM_Day => getBGMID(is_day: true, is_boss: false);

		public int BGM_Night => getBGMID(is_day: false, is_boss: false);

		public int BGM_Day_Boss => getBGMID(is_day: true, is_boss: true);

		public int BGM_Night_Boss => getBGMID(is_day: false, is_boss: true);

		public BattleBGMModel(int area_id, int map_no)
		{
			_area_id = area_id;
			_map_no = map_no;
		}

		public int getBGMID(bool is_day, bool is_boss)
		{
			int key = (!is_day) ? 1 : 0;
			int index = is_boss ? 1 : 0;
			return _mst[key][index];
		}

		public override string ToString()
		{
			string str = $"戦闘BGM {_area_id}-{_map_no}";
			str += " ";
			str += $"ザコ戦(昼:{BGM_Day} 夜:{BGM_Night})";
			str += " ";
			return str + $"ボス戦(昼:{BGM_Day_Boss} 夜:{BGM_Night_Boss})";
		}
	}
}
