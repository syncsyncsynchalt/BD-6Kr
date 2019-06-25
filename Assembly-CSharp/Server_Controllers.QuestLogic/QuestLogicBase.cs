using Common.Enum;
using Server_Common;
using Server_Models;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.QuestLogic
{
	public abstract class QuestLogicBase : IQuestOperator
	{
		protected List<Mem_quest> checkData;

		protected Dictionary<int, Mst_questcount> mst_count;

		protected Dictionary<int, Mem_quest> questData;

		protected Dictionary<int, Mem_questcount> mem_count;

		private Dictionary<int, int> mst_counter_limit;

		public QuestLogicBase()
		{
			questData = Comm_UserDatas.Instance.User_quest;
			mem_count = Comm_UserDatas.Instance.User_questcount;
			mst_count = Mst_DataManager.Instance.Mst_questcount;
			mst_counter_limit = ArrayMaster.GetQuestCounterLimit();
		}

		public abstract List<int> ExecuteCheck();

		public bool CheckClearCounter(int quest_id)
		{
			Dictionary<int, int> clear_num = mst_count[quest_id].Clear_num;
			if (clear_num.Count != mst_count[quest_id].Counter_id.Count)
			{
				return checkCalcCounter(mst_count[quest_id].Counter_id, clear_num.Values.First());
			}
			foreach (KeyValuePair<int, int> item in clear_num)
			{
				int key = item.Key;
				Mem_questcount value = null;
				if (!mem_count.TryGetValue(key, out value))
				{
					return false;
				}
				if (value.Value < item.Value)
				{
					return false;
				}
			}
			return true;
		}

		private bool checkCalcCounter(HashSet<int> target, int clearNum)
		{
			int num = 0;
			foreach (int item in target)
			{
				Mem_questcount value = null;
				if (mem_count.TryGetValue(item, out value))
				{
					num += value.Value;
				}
			}
			return (num >= clearNum) ? true : false;
		}

		protected List<Mem_quest> getCheckDatas(int category)
		{
			IEnumerable<Mem_quest> enumerable = from x in questData.Values
				where (x.State == QuestState.RUNNING && questData[x.Rid].Category == category) ? true : false
				select x;
			return (enumerable == null) ? new List<Mem_quest>() : (from x in enumerable
				orderby x.Rid
				select x).ToList();
		}

		protected Dictionary<int, int> isAddCounter(int questId, List<Mem_quest> nowExecQuestList)
		{
			Dictionary<int, int> dictionary = mst_count[questId].Counter_id.ToDictionary((int x) => x, (int y) => 0);
			foreach (Mem_quest nowExecQuest in nowExecQuestList)
			{
				if (nowExecQuest.Rid == questId)
				{
					return dictionary;
				}
				if (mst_count.ContainsKey(nowExecQuest.Rid))
				{
					HashSet<int> counter_id = mst_count[nowExecQuest.Rid].Counter_id;
					foreach (int item in counter_id)
					{
						if (dictionary.ContainsKey(item))
						{
							dictionary.Remove(item);
						}
						if (dictionary.Count == 0)
						{
							return dictionary;
						}
					}
				}
			}
			return dictionary;
		}

		protected void addCounterIncrementAll(Dictionary<int, int> counter)
		{
			if (counter.Count != 0)
			{
				List<int> list = counter.Keys.ToList();
				foreach (int item in list)
				{
					counter[item] = 1;
				}
				addCounter(counter);
			}
		}

		protected void addCounter(Dictionary<int, int> addValues)
		{
			foreach (KeyValuePair<int, int> addValue in addValues)
			{
				Mem_questcount value = null;
				if (!mem_count.TryGetValue(addValue.Key, out value))
				{
					value = new Mem_questcount(addValue.Key, addValue.Value);
					mem_count.Add(value.Rid, value);
				}
				else
				{
					value.AddCount(addValue.Value);
				}
				int value2 = 0;
				if (mst_counter_limit.TryGetValue(value.Rid, out value2) && value.Value > value2)
				{
					value.Reset(deleteFlag: false);
					value.AddCount(value2);
				}
			}
		}

		protected string getFuncName(Mem_quest quest)
		{
			string str = quest.Rid.ToString().Substring(quest.Category.ToString().Length);
			return "Check_" + str;
		}
	}
}
