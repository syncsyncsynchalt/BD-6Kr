using Common.Enum;
using Server_Models;
using System.Collections.Generic;
using System.Linq;

namespace Server_Common.Formats
{
	public class User_QuestFmt
	{
		public int No;

		public int Category;

		public QuestState State;

		public string Title;

		public string Detail;

		public Dictionary<enumMaterialCategory, int> GetMaterial;

		public int GetSpoint;

		public QuestProgressKinds Progress;

		public bool InvalidFlag;

		public int Type;

		public List<QuestItemGetKind> RewardTypes;

		public List<int> RewardCount;

		public User_QuestFmt()
		{
			State = QuestState.NOT_DISP;
			Progress = QuestProgressKinds.NOT_DISP;
			GetMaterial = new Dictionary<enumMaterialCategory, int>();
			RewardTypes = new List<QuestItemGetKind>();
			RewardCount = new List<int>();
		}

		public User_QuestFmt(Mem_quest memObj, Mst_quest mstObj)
			: this()
		{
			No = memObj.Rid;
			Category = mstObj.Category;
			State = memObj.State;
			Title = mstObj.Name;
			Detail = mstObj.Details;
			Type = mstObj.Type;
			GetMaterial = mstObj.GetMaterialValues();
			GetSpoint = mstObj.GetSpointNum();
			Progress = setProgress();
			if (mstObj.Get_1_type > 0)
			{
				RewardTypes.Add((QuestItemGetKind)mstObj.Get_1_type);
				RewardCount.Add(mstObj.Get_1_count);
			}
			if (mstObj.Get_2_type > 0)
			{
				RewardTypes.Add((QuestItemGetKind)mstObj.Get_2_type);
				RewardCount.Add(mstObj.Get_2_count);
			}
		}

		private QuestProgressKinds setProgress()
		{
			if (State == QuestState.COMPLETE)
			{
				return QuestProgressKinds.NOT_DISP;
			}
			Mst_questcount value = null;
			if (!Mst_DataManager.Instance.Mst_questcount.TryGetValue(No, out value))
			{
				return QuestProgressKinds.NOT_DISP;
			}
			double num = 0.0;
			if (value.Counter_id.Count != value.Clear_num.Count)
			{
				double num2 = 0.0;
				Mem_questcount value2 = null;
				foreach (int item in value.Counter_id)
				{
					if (Comm_UserDatas.Instance.User_questcount.TryGetValue(item, out value2))
					{
						num2 += (double)value2.Value;
					}
				}
				double num3 = value.Clear_num.Values.First();
				num = num2 / num3;
				return getprogress(num);
			}
			foreach (int item2 in value.Counter_id)
			{
				double num4 = 0.0;
				Mem_questcount value3 = null;
				if (Comm_UserDatas.Instance.User_questcount.TryGetValue(item2, out value3))
				{
					num4 = value3.Value;
				}
				double num5 = value.Clear_num[item2];
				double num6 = num4 / num5;
				if (num6 >= 1.0)
				{
					num6 = 1.0;
				}
				num += num6;
			}
			num /= (double)value.Counter_id.Count;
			return getprogress(num);
		}

		private QuestProgressKinds getprogress(double progressProb)
		{
			if (progressProb >= 0.8)
			{
				return QuestProgressKinds.MORE_THAN_80;
			}
			if (progressProb >= 0.5)
			{
				return QuestProgressKinds.MORE_THAN_50;
			}
			return QuestProgressKinds.NOT_DISP;
		}
	}
}
