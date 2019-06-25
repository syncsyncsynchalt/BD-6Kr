using System;
using System.Collections.Generic;

namespace local.models
{
	public class ShipExpModel
	{
		private int _level_before;

		private int _level_after;

		private int _exp_before;

		private int _exp_after;

		private int _exp_rate_before;

		private List<int> _exp_rate_after;

		private List<int> _exp_at_levelup;

		public int LevelBefore => _level_before;

		public int LevelAfter => _level_after;

		public int ExpBefore => _exp_before;

		public int ExpAfter => _exp_after;

		public int Exp => _exp_after - _exp_before;

		public int ExpRateBefore => _exp_rate_before;

		public List<int> ExpRateAfter => _exp_rate_after;

		public List<int> ExpAtLevelup => _exp_at_levelup;

		public ShipExpModel(int exp_rate_before, ShipModel after_ship, int exp, List<int> levelup_info)
		{
			levelup_info = ((levelup_info != null) ? levelup_info : new List<int>
			{
				0
			});
			int num = 0;
			_level_after = 0;
			_exp_after = 0;
			int item = 0;
			if (after_ship != null)
			{
				num = ((after_ship.Level != 99 && after_ship.Level != 150) ? Math.Max(levelup_info.Count - 2, 0) : Math.Max(levelup_info.Count - 1, 0));
				_level_after = after_ship.Level;
				_exp_after = after_ship.Exp;
				item = after_ship.Exp_Percentage;
			}
			_level_before = _level_after - num;
			_exp_before = 0;
			if (levelup_info.Count > 0)
			{
				_exp_before = levelup_info[0];
			}
			_exp_rate_before = exp_rate_before;
			_exp_rate_after = new List<int>();
			for (int i = 0; i < num; i++)
			{
				_exp_rate_after.Add(100);
			}
			_exp_rate_after.Add(item);
			_exp_at_levelup = levelup_info.GetRange(1, levelup_info.Count - 1);
		}

		public override string ToString()
		{
			string empty = string.Empty;
			empty += $"獲得経験値:{ExpAfter - ExpBefore} 経験値:{ExpBefore}=>{ExpAfter}";
			empty += $" (Lv:{LevelBefore} 経験値:{ExpBefore}({ExpRateBefore}%))";
			empty += "=>";
			for (int i = 0; i <= LevelAfter - LevelBefore; i++)
			{
				int num = LevelBefore + i;
				int num2 = ExpRateAfter[i];
				int num3 = (num2 != 100) ? ExpAfter : ExpAtLevelup[i];
				empty += $"(Lv:{num} 経験値:{num3}({num2}%)))";
				if (num2 == 100)
				{
					empty += "=>";
				}
			}
			return empty;
		}
	}
}
