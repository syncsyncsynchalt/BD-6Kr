using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_record", Namespace = "")]
	public class Mem_record : Model_Base
	{
		[DataMember]
		private int _level;

		[DataMember]
		private uint _exp;

		[DataMember]
		private uint _st_win;

		[DataMember]
		private uint _st_lose;

		[DataMember]
		private uint _rebellion_win;

		[DataMember]
		private uint _pt_win;

		[DataMember]
		private uint _pt_lose;

		[DataMember]
		private uint _deck_practice_num;

		[DataMember]
		private double _rate;

		[DataMember]
		private List<DifficultKind> _clearDifficult;

		[DataMember]
		private uint _lostShipNum;

		private static string _tableName = "mem_record";

		public int Level
		{
			get
			{
				return _level;
			}
			private set
			{
				_level = value;
			}
		}

		public uint Exp
		{
			get
			{
				return _exp;
			}
			private set
			{
				_exp = value;
			}
		}

		public uint St_win
		{
			get
			{
				return _st_win;
			}
			private set
			{
				_st_win = value;
			}
		}

		public uint St_lose
		{
			get
			{
				return _st_lose;
			}
			private set
			{
				_st_lose = value;
			}
		}

		public uint Rebellion_win
		{
			get
			{
				return _rebellion_win;
			}
			private set
			{
				_rebellion_win = value;
			}
		}

		public uint Pt_win
		{
			get
			{
				return _pt_win;
			}
			private set
			{
				_pt_win = value;
			}
		}

		public uint Pt_lose
		{
			get
			{
				return _pt_lose;
			}
			private set
			{
				_pt_lose = value;
			}
		}

		public uint Deck_practice_num
		{
			get
			{
				return _deck_practice_num;
			}
			private set
			{
				_deck_practice_num = value;
			}
		}

		public double Rate
		{
			get
			{
				return _rate;
			}
			private set
			{
				_rate = value;
			}
		}

		public List<DifficultKind> ClearDifficult
		{
			get
			{
				return _clearDifficult;
			}
			private set
			{
				_clearDifficult = value;
			}
		}

		public uint LostShipNum
		{
			get
			{
				return _lostShipNum;
			}
			private set
			{
				_lostShipNum = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_record()
		{
			Level = 1;
			Exp = 0u;
			St_win = 0u;
			Pt_win = 0u;
			ClearDifficult = new List<DifficultKind>();
			Rebellion_win = 0u;
			Deck_practice_num = 0u;
			LostShipNum = 0u;
		}

		public Mem_record(ICreateNewUser createInstance, Mem_newgame_plus newGamePlusBase, List<DifficultKind> kind)
		{
			if (createInstance != null)
			{
				Level = newGamePlusBase.FleetLevel;
				Exp = newGamePlusBase.FleetExp;
				St_win = 0u;
				Pt_win = 0u;
				ClearDifficult = kind;
				Rebellion_win = 0u;
				Deck_practice_num = 0u;
			}
		}

		public void SetUserRecordData(User_RecordFmt out_fmt)
		{
			if (out_fmt != null)
			{
				out_fmt.Exp = Exp;
				Dictionary<int, int> dictionary = Mst_DataManager.Instance.Get_MstLevel(shipTable: false);
				int value = 0;
				dictionary.TryGetValue(Level + 1, out value);
				if (value == -1)
				{
					value = 0;
				}
				out_fmt.Exp_next = (uint)value;
				out_fmt.Level = Level;
				out_fmt.War_total = St_win + St_lose + Rebellion_win;
				out_fmt.War_win = St_win;
				out_fmt.War_lose = St_lose;
				out_fmt.War_RebellionWin = Rebellion_win;
				out_fmt.War_rate = GetSortieRate();
				out_fmt.Practice_win = Pt_win;
				out_fmt.Practice_lose = Pt_lose;
				out_fmt.DeckPracticeNum = Deck_practice_num;
				out_fmt.Rank = GetRank();
			}
		}

		public double GetSortieRate()
		{
			double num = St_win + St_lose + Rebellion_win;
			if (num == 0.0)
			{
				return 0.0;
			}
			return (double)St_win / num * 100.0;
		}

		public int UpdateExp(int addValue, Dictionary<int, int> mst_level)
		{
			int num = mst_level.Values.Max();
			Exp += (uint)addValue;
			if (Exp > num)
			{
				Exp = (uint)num;
			}
			int num2 = mst_level[Level + 1];
			if (num2 == -1 || Exp < num2)
			{
				return Level;
			}
			Comm_UserDatas.Instance.User_trophy.IsFleetLevelUp = true;
			int num3 = Level + 1;
			while (Exp >= num2)
			{
				if (num2 == -1)
				{
					return Level;
				}
				Level = num3;
				num3++;
				num2 = mst_level[num3];
			}
			return Level;
		}

		public void UpdateSortieCount(BattleWinRankKinds kinds, bool rebellionBoss)
		{
			if (Utils.IsBattleWin(kinds))
			{
				if (rebellionBoss)
				{
					if (Rebellion_win != uint.MaxValue)
					{
						Rebellion_win++;
					}
				}
				else if (St_win != uint.MaxValue)
				{
					St_win++;
				}
			}
			else if (St_lose != uint.MaxValue)
			{
				St_lose++;
			}
		}

		public void UpdatePracticeCount(BattleWinRankKinds kinds, bool practiceBattle)
		{
			if (practiceBattle)
			{
				if (Utils.IsBattleWin(kinds))
				{
					if (Pt_win != uint.MaxValue)
					{
						Pt_win++;
					}
				}
				else if (Pt_lose != uint.MaxValue)
				{
					Pt_lose++;
				}
			}
			else if (Deck_practice_num != uint.MaxValue)
			{
				Deck_practice_num++;
			}
		}

		public void UpdateMissionCount(MissionResultKinds ms_kind)
		{
		}

		public void AddClearDifficult(DifficultKind difficult)
		{
			if (!ClearDifficult.Contains(difficult))
			{
				ClearDifficult.Add(difficult);
			}
		}

		public int GetRank()
		{
			if (Level < 10)
			{
				return 10;
			}
			if (Level >= 10 && Level < 20)
			{
				return 9;
			}
			if (Level >= 20 && Level < 30)
			{
				return 8;
			}
			if (Level >= 30 && Level < 40)
			{
				return 7;
			}
			if (Level >= 40 && Level < 50)
			{
				return 6;
			}
			if (Level >= 50 && Level < 60)
			{
				return 5;
			}
			if (Level >= 60 && Level < 70)
			{
				return 4;
			}
			if (Level >= 70 && Level < 80)
			{
				return 3;
			}
			if (Level >= 80 && Level < 90)
			{
				return 2;
			}
			return 1;
		}

		public void AddLostShipCount()
		{
			if (LostShipNum != uint.MaxValue)
			{
				LostShipNum++;
			}
		}

		protected override void setProperty(XElement element)
		{
			Level = int.Parse(element.Element("_level").Value);
			Exp = uint.Parse(element.Element("_exp").Value);
			St_win = uint.Parse(element.Element("_st_win").Value);
			St_lose = uint.Parse(element.Element("_st_lose").Value);
			Pt_win = uint.Parse(element.Element("_pt_win").Value);
			Pt_lose = uint.Parse(element.Element("_pt_lose").Value);
			Rate = double.Parse(element.Element("_rate").Value);
			foreach (XElement item2 in element.Element("_clearDifficult").Elements())
			{
				DifficultKind item = (DifficultKind)(int)Enum.Parse(typeof(DifficultKind), item2.Value);
				ClearDifficult.Add(item);
			}
			if (element.Element("_rebellion_win") != null)
			{
				Rebellion_win = uint.Parse(element.Element("_rebellion_win").Value);
			}
			if (element.Element("_deck_practice_num") != null)
			{
				Deck_practice_num = uint.Parse(element.Element("_deck_practice_num").Value);
			}
			if (element.Element("_lostShipNum") != null)
			{
				LostShipNum = uint.Parse(element.Element("_lostShipNum").Value);
			}
		}
	}
}
