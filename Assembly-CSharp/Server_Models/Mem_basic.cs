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
	[DataContract(Name = "mem_basic", Namespace = "")]
	public class Mem_basic : Model_Base
	{
		private const int DEF_SLOTNUM = 400;

		private const int DEF_SHIPNUM = 100;

		[DataMember]
		private DifficultKind _difficult;

		[DataMember]
		private string _nickname;

		[DataMember]
		private int _starttime;

		[DataMember]
		private string _comment;

		[DataMember]
		private int _max_chara;

		[DataMember]
		private int _max_slotitem;

		[DataMember]
		private int _max_quest;

		[DataMember]
		private int _large_dock;

		[DataMember]
		private int _fcoin;

		[DataMember]
		private int _strategy_point;

		[DataMember]
		private HashSet<int> tutorialProgressStep;

		[DataMember]
		private HashSet<int> tutorialProgressText;

		private static string _tableName = "mem_basic";

		public DifficultKind Difficult
		{
			get
			{
				return _difficult;
			}
			private set
			{
				_difficult = value;
			}
		}

		public string Nickname
		{
			get
			{
				return _nickname;
			}
			private set
			{
				_nickname = value;
			}
		}

		public int Starttime
		{
			get
			{
				return _starttime;
			}
			private set
			{
				_starttime = value;
			}
		}

		public string Comment
		{
			get
			{
				return _comment;
			}
			private set
			{
				_comment = value;
			}
		}

		public int Max_chara
		{
			get
			{
				return _max_chara;
			}
			private set
			{
				_max_chara = value;
			}
		}

		public int Max_slotitem
		{
			get
			{
				return _max_slotitem;
			}
			private set
			{
				_max_slotitem = value;
			}
		}

		public int Max_quest
		{
			get
			{
				return _max_quest;
			}
			private set
			{
				_max_quest = value;
			}
		}

		public int Large_dock
		{
			get
			{
				return _large_dock;
			}
			private set
			{
				_large_dock = value;
			}
		}

		public int Fcoin
		{
			get
			{
				return _fcoin;
			}
			private set
			{
				_fcoin = value;
			}
		}

		public int Strategy_point
		{
			get
			{
				return _strategy_point;
			}
			private set
			{
				_strategy_point = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_basic()
		{
			Difficult = DifficultKind.HEI;
			Nickname = string.Empty;
			Max_chara = 100;
			Max_slotitem = 400;
			Max_quest = 7;
			Starttime = 0;
			Comment = string.Empty;
			Large_dock = 0;
			Fcoin = 0;
			Strategy_point = 3000;
			tutorialProgressStep = new HashSet<int>();
			tutorialProgressText = new HashSet<int>();
		}

		public Mem_basic(ICreateNewUser createInstance, Mem_basic newGamePlusBase)
		{
			if (createInstance != null)
			{
				Difficult = newGamePlusBase.Difficult;
				Nickname = newGamePlusBase.Nickname;
				Max_chara = newGamePlusBase.Max_chara;
				Max_slotitem = newGamePlusBase.Max_slotitem;
				Max_quest = 7;
				Starttime = 1;
				Comment = string.Empty;
				Large_dock = 0;
				Fcoin = 0;
				Strategy_point = 3000;
				tutorialProgressStep = new HashSet<int>();
				tutorialProgressText = new HashSet<int>();
			}
		}

		public int UserLevel()
		{
			return Comm_UserDatas.Instance.User_record.Level;
		}

		public int UserRank()
		{
			return Comm_UserDatas.Instance.User_record.GetRank();
		}

		public void SetUserRecordData(User_RecordFmt out_fmt)
		{
			if (out_fmt != null)
			{
				out_fmt.Comment = Comment;
				out_fmt.Large_dock = Large_dock;
				out_fmt.Nickname = Nickname;
				out_fmt.Ship_max = Max_chara;
				out_fmt.Slot_max = Max_slotitem;
			}
		}

		public void UpdateComment(string comment)
		{
			comment.TrimEnd(default(char));
			Comment = comment;
		}

		public void UpdateNickName(string name)
		{
			name.TrimEnd(default(char));
			Nickname = name;
		}

		public void SetDifficult(DifficultKind difficult)
		{
			Difficult = difficult;
			Starttime = 1;
		}

		public bool IsMaxChara()
		{
			return (Comm_UserDatas.Instance.User_ship.Count() >= Max_chara) ? true : false;
		}

		public bool IsMaxSlotitem()
		{
			return (Comm_UserDatas.Instance.User_slot.Count() >= Max_slotitem) ? true : false;
		}

		public void AddCoin(int addNum)
		{
			Fcoin += addNum;
		}

		public void SubCoin(int subNum)
		{
			Fcoin -= subNum;
		}

		public void AddPoint(int addNum)
		{
			int int_value = Mst_DataManager.Instance.Mst_const[MstConstDataIndex.Strategy_point_def].Int_value;
			Strategy_point += addNum;
			if (Strategy_point > int_value)
			{
				Strategy_point = int_value;
			}
		}

		public void SubPoint(int subNum)
		{
			Strategy_point -= subNum;
			if (Strategy_point < 0)
			{
				Strategy_point = 0;
			}
		}

		public void OpenLargeDock()
		{
			Large_dock = 1;
		}

		public void PortExtend(int type)
		{
			int num = 10;
			int num2 = 40;
			if (type == 1 && GetPortMaxExtendNum() >= Max_chara)
			{
				Max_chara += num;
				Max_slotitem += num2;
			}
		}

		public bool QuestExtend(Dictionary<MstConstDataIndex, Mst_const> mst_const)
		{
			if (Max_quest >= mst_const[MstConstDataIndex.Parallel_quest_max].Int_value)
			{
				return false;
			}
			Max_quest++;
			return true;
		}

		public int GetPortMaxExtendNum()
		{
			if (Comm_UserDatas.Instance.User_plus.GetLapNum() >= 1)
			{
				return Mst_DataManager.Instance.Mst_const[MstConstDataIndex.Boko_max_ships_sys].Int_value;
			}
			return Mst_DataManager.Instance.Mst_const[MstConstDataIndex.Boko_max_ships_def].Int_value;
		}

		public int GetPortMaxSlotItemNum()
		{
			int num = (GetPortMaxExtendNum() - 100) / 10;
			return 400 + 40 * num;
		}

		public int GetMaterialMaxNum()
		{
			int num = 10000;
			int num2 = 1000;
			int num3 = UserLevel();
			return num3 * num2 + num;
		}

		public void AddTutorialProgress(int tutorialType, int addKind)
		{
			HashSet<int> hashSet = (tutorialType != 1) ? tutorialProgressText : tutorialProgressStep;
			hashSet.Add(addKind);
		}

		public bool GetTutorialState(int tutorialType, int getKind)
		{
			HashSet<int> hashSet = (tutorialType != 1) ? tutorialProgressText : tutorialProgressStep;
			return hashSet.Contains(getKind);
		}

		public int GetTutorialStepLastNo()
		{
			if (tutorialProgressStep.Count == 0)
			{
				return 0;
			}
			return (from x in tutorialProgressStep
				select (x)).Max();
		}

		public List<int> GetTutorialProgress(int tutorialType)
		{
			HashSet<int> source = (tutorialType != 1) ? tutorialProgressText : tutorialProgressStep;
			return source.ToList();
		}

		protected override void setProperty(XElement element)
		{
			Difficult = (DifficultKind)(int)Enum.Parse(typeof(DifficultKind), element.Element("_difficult").Value);
			Nickname = element.Element("_nickname").Value;
			Starttime = int.Parse(element.Element("_starttime").Value);
			Comment = element.Element("_comment").Value;
			Max_chara = int.Parse(element.Element("_max_chara").Value);
			Max_slotitem = int.Parse(element.Element("_max_slotitem").Value);
			Max_quest = int.Parse(element.Element("_max_quest").Value);
			Large_dock = int.Parse(element.Element("_large_dock").Value);
			Fcoin = int.Parse(element.Element("_fcoin").Value);
			Strategy_point = int.Parse(element.Element("_strategy_point").Value);
			foreach (XElement item in element.Element("tutorialProgressStep").Elements())
			{
				tutorialProgressStep.Add(int.Parse(item.Value));
			}
			foreach (XElement item2 in element.Element("tutorialProgressText").Elements())
			{
				tutorialProgressText.Add(int.Parse(item2.Value));
			}
		}
	}
}
