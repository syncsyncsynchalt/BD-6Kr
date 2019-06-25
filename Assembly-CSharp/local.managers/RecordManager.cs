using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers;

namespace local.managers
{
	public class RecordManager : ManagerBase
	{
		private User_RecordFmt _record_data;

		public string Name => _record_data.Nickname;

		public int Level => _record_data.Level;

		public int Rank => base.UserInfo.Rank;

		public uint Experience => _record_data.Exp;

		public uint NextExperience => _record_data.Exp_next;

		public uint BattleCount => _record_data.War_total;

		public uint SortieWin => _record_data.War_win;

		public uint SortieLose => _record_data.War_lose;

		public uint InterceptSuccess => _record_data.War_RebellionWin;

		public double SortieRate => _record_data.War_rate;

		public uint DeckPractice => _record_data.DeckPracticeNum;

		public uint PracticeWin => _record_data.Practice_win;

		public uint PracticeLose => _record_data.Practice_lose;

		public int DeckCount => _record_data.Deck_num;

		public int ShipCount => _record_data.Ship_num;

		public int SlotitemCount => _record_data.Slot_num;

		public int NDockCount => _record_data.Ndock_num;

		public int KDockCount => _record_data.Kdock_num;

		public int FurnitureCount => _record_data.Furniture_num;

		public int DeckCountMax => 8;

		public int ShipCountMax => _record_data.Ship_max;

		public int SlotitemCountMax => _record_data.Slot_max;

		public int MaterialMax => _record_data.Material_max;

		public RecordManager()
		{
			_Update();
		}

		public bool IsCleardOnce()
		{
			if (GetClearCount(DifficultKind.TEI) > 0)
			{
				return true;
			}
			if (GetClearCount(DifficultKind.HEI) > 0)
			{
				return true;
			}
			if (GetClearCount(DifficultKind.OTU) > 0)
			{
				return true;
			}
			if (GetClearCount(DifficultKind.KOU) > 0)
			{
				return true;
			}
			if (GetClearCount(DifficultKind.SHI) > 0)
			{
				return true;
			}
			return false;
		}

		public int GetClearCount(DifficultKind difficulty)
		{
			return Comm_UserDatas.Instance.User_plus.ClearNum(difficulty);
		}

		private void _Update()
		{
			Api_Result<User_RecordFmt> api_Result = new Api_get_Member().Record();
			if (api_Result.state == Api_Result_State.Success)
			{
				_record_data = api_Result.data;
			}
			else
			{
				_record_data = new User_RecordFmt();
			}
		}

		public override string ToString()
		{
			string text = "-- 戦績画面 --\n";
			string text2 = text;
			text = text2 + " | 提督名 / レベル / 階級 : " + Name + " / " + Level + " / " + Rank + "\n";
			text2 = text;
			text = text2 + " | 提督経験値 / 次レベル : " + Experience + " / " + NextExperience + "\n";
			text2 = text;
			text = text2 + " | [出撃] 勝利数 / 敗北数 / 勝率 : " + SortieWin + " / " + SortieLose + " / " + SortieRate + "\n";
			text2 = text;
			text = text2 + " | 保有艦隊数 : " + DeckCount + "    工廠ドック数 : " + KDockCount + "    入渠ドック数 : " + NDockCount + "\n";
			text2 = text;
			text = text2 + " | 艦娘保有数 : " + ShipCount + "    装備アイテム保有数 : " + SlotitemCount + "    家具保有数 : " + FurnitureCount + "\n";
			text2 = text;
			text = text2 + " | 最大所有可能艦隊数 : " + DeckCountMax + "    最大所有可能艦娘数 : " + ShipCountMax + "\n";
			text2 = text;
			text = text2 + " | 最大所有可能装備アイテム数 : " + SlotitemCountMax + "    最大備蓄可能各資材量 : " + MaterialMax + "\n";
			if (IsCleardOnce())
			{
				text += " | クリア実勢: ";
				text += $"丁-{GetClearCount(DifficultKind.TEI)}回  丙-{GetClearCount(DifficultKind.HEI)}回  乙-{GetClearCount(DifficultKind.OTU)}回  甲-{GetClearCount(DifficultKind.KOU)}回  史-{GetClearCount(DifficultKind.SHI)}回\n";
			}
			else
			{
				text += " | クリア実績: 無し\n";
			}
			return text + "-- 戦績画面 --";
		}
	}
}
