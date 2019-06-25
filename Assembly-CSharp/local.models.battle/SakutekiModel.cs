using Common.Enum;
using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;
using System.Linq;

namespace local.models.battle
{
	public class SakutekiModel
	{
		private SearchInfo _info_f;

		private SearchInfo _info_e;

		private List<List<SlotitemModel_Battle>> _planes_f;

		private List<List<SlotitemModel_Battle>> _planes_e;

		public BattleSearchValues value_f => _info_f.SearchValue;

		public BattleSearchValues value_e => _info_e.SearchValue;

		public List<List<SlotitemModel_Battle>> planes_f => _planes_f;

		public List<List<SlotitemModel_Battle>> planes_e => _planes_e;

		public SakutekiModel(SearchInfo[] infoSet, List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e)
		{
			_info_f = infoSet[0];
			_info_e = infoSet[1];
			_planes_f = _createPlaneList(ships_f, _info_f.UsePlane.ToDictionary((SearchUsePlane x) => x.Rid, (SearchUsePlane y) => y.MstIds));
			_planes_e = _createPlaneList(ships_e, _info_e.UsePlane.ToDictionary((SearchUsePlane x) => x.Rid, (SearchUsePlane y) => y.MstIds));
		}

		[Obsolete("value_f を使用してください", false)]
		public bool IsSuccess_f()
		{
			return _IsSuccess(value_f);
		}

		[Obsolete("value_f を使用してください", false)]
		public bool HasPlane_f()
		{
			return _planes_f.FindAll((List<SlotitemModel_Battle> x) => x.Count > 0).Count > 0;
		}

		[Obsolete("value_f を使用してください", false)]
		public bool ExistLost_f()
		{
			return _ExistLost(value_f);
		}

		private bool _IsSuccess(BattleSearchValues value)
		{
			return value == BattleSearchValues.Success || value == BattleSearchValues.Success_Lost || value == BattleSearchValues.Found;
		}

		private bool _HasPlane(BattleSearchValues value)
		{
			return value == BattleSearchValues.Success || value == BattleSearchValues.Success_Lost || value == BattleSearchValues.Lost || value == BattleSearchValues.Faile;
		}

		private bool _ExistLost(BattleSearchValues value)
		{
			return value == BattleSearchValues.Success_Lost || value == BattleSearchValues.Lost;
		}

		private List<List<SlotitemModel_Battle>> _createPlaneList(List<ShipModel_BattleAll> ships, Dictionary<int, List<int>> org)
		{
			List<List<SlotitemModel_Battle>> list = new List<List<SlotitemModel_Battle>>();
			for (int i = 0; i < ships.Count; i++)
			{
				List<SlotitemModel_Battle> list2 = new List<SlotitemModel_Battle>();
				ShipModel_BattleAll shipModel_BattleAll = ships[i];
				if (shipModel_BattleAll != null && org.TryGetValue(shipModel_BattleAll.TmpId, out List<int> value))
				{
					for (int j = 0; j < value.Count; j++)
					{
						list2.Add(new SlotitemModel_Battle(value[j]));
					}
				}
				list.Add(list2);
			}
			return list;
		}

		public override string ToString()
		{
			bool flag = _HasPlane(value_f);
			string str = string.Format("[味方側 索敵{0} 索敵機{1} 未帰投機{2}]\n", (!_IsSuccess(value_f)) ? "失敗" : "成功", (!flag) ? "無" : "有", (!_ExistLost(value_f)) ? "無" : "有");
			if (flag)
			{
				for (int i = 0; i < _planes_f.Count; i++)
				{
					for (int j = 0; j < _planes_f[i].Count; j++)
					{
						str += $"[{i}][{j}] {_planes_f[i][j]}";
					}
				}
				str += "\n";
			}
			bool flag2 = _HasPlane(value_e);
			str += string.Format("[相手側 索敵{0} 索敵機{1} 未帰投機{2}]\n", (!_IsSuccess(value_e)) ? "失敗" : "成功", (!flag2) ? "無" : "有", (!_ExistLost(value_e)) ? "無" : "有");
			if (flag2)
			{
				for (int k = 0; k < _planes_e.Count; k++)
				{
					for (int l = 0; l < _planes_e[k].Count; l++)
					{
						str += $"[{k}][{l}] {_planes_e[k][l]}";
					}
				}
				str += "\n";
			}
			return str;
		}
	}
}
