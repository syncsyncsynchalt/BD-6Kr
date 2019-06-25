using Server_Common.Formats.Battle;
using System.Collections.Generic;

namespace local.models.battle
{
	public class HougekiListModel : ICommandAction
	{
		private List<HougekiModel> _list;

		private int _phase_index;

		public int Count => _list.Count;

		public HougekiListModel(List<Hougeki<BattleAtackKinds_Day>> hougeki, Dictionary<int, ShipModel_BattleAll> ships)
		{
			_list = new List<HougekiModel>();
			for (int i = 0; i < hougeki.Count; i++)
			{
				HougekiModel item = new HougekiModel(hougeki[i], ships);
				_list.Add(item);
			}
		}

		public HougekiListModel(List<Hougeki<BattleAtackKinds_Night>> hougeki, Dictionary<int, ShipModel_BattleAll> ships)
		{
			_list = new List<HougekiModel>();
			for (int i = 0; i < hougeki.Count; i++)
			{
				HougekiModel item = new HougekiModel(hougeki[i], ships);
				_list.Add(item);
			}
		}

		public HougekiModel[] GetData()
		{
			return _list.ToArray();
		}

		public HougekiModel GetData(int index)
		{
			return _list[index];
		}

		public HougekiModel GetNextData()
		{
			if (_list.Count > _phase_index)
			{
				HougekiModel result = _list[_phase_index];
				_phase_index++;
				return result;
			}
			return null;
		}

		public override string ToString()
		{
			string text = string.Empty;
			for (int i = 0; i < _list.Count; i++)
			{
				text += $"[{i}:{_list[i]}]";
				if (i < _list.Count - 1)
				{
					text += "\n";
				}
			}
			return text;
		}
	}
}
