using Server_Common.Formats;
using Server_Models;
using System;

namespace local.models
{
	public class MapHPModel
	{
		private User_MapinfoFmt.enumExBossType _type;

		private int _mapID;

		private int _max_value;

		private int _now_value;

		public int MapID => _mapID;

		public int MaxValue => _max_value;

		public int NowValue => _now_value;

		[Obsolete("[デバッグ用] 表示の確認等の為に一時的にモデルを生成したい場合に使用してください。")]
		public MapHPModel(int mapID, int max, int now)
		{
			_mapID = mapID;
			_max_value = max;
			_now_value = now;
		}

		public MapHPModel(Mst_mapinfo mst, User_MapinfoFmt mem)
		{
			_mapID = mst.Id;
			if (mem != null)
			{
				_type = mem.Boss_type;
				if (_type == User_MapinfoFmt.enumExBossType.MapHp)
				{
					_max_value = mem.Eventmap.Event_maxhp;
					_now_value = mem.Eventmap.Event_hp;
				}
				else if (_type == User_MapinfoFmt.enumExBossType.Defeat)
				{
					_max_value = 0;
					_now_value = Math.Max(_max_value - mem.Defeat_count, 0);
				}
			}
			else
			{
				_type = User_MapinfoFmt.enumExBossType.Normal;
				_max_value = (_now_value = 0);
			}
		}

		public void __Update__(EventMapInfo info)
		{
			if (_type == User_MapinfoFmt.enumExBossType.MapHp)
			{
				_max_value = info.Event_maxhp;
				_now_value = info.Event_hp;
			}
		}

		public override string ToString()
		{
			if (_type == User_MapinfoFmt.enumExBossType.Normal)
			{
				return $"[MapHP{MapID}]通常ボスHP: {NowValue}/{MaxValue}";
			}
			if (_type == User_MapinfoFmt.enumExBossType.Defeat)
			{
				return $"[MapHP{MapID}]討伐系ボスHP: {NowValue}/{MaxValue}";
			}
			if (_type == User_MapinfoFmt.enumExBossType.Normal)
			{
				return $"[MapHP{MapID}]イベント系ボスHP: {NowValue}/{MaxValue}";
			}
			return $"[MapHP{MapID}]ボスHP: {NowValue}/{MaxValue}";
		}
	}
}
