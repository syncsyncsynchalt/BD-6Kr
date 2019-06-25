using Common.Enum;
using Server_Models;
using System.Collections.Generic;
using System.Reflection;

namespace Server_Controllers.QuestLogic
{
	public class QuestSupply : QuestLogicBase
	{
		private int supplyShipCount;

		private int type;

		public QuestSupply()
		{
			checkData = getCheckDatas(5);
			type = 1;
		}

		public QuestSupply(int shipCount)
		{
			checkData = getCheckDatas(5);
			supplyShipCount = shipCount;
			type = 2;
		}

		public override List<int> ExecuteCheck()
		{
			List<int> list = new List<int>(checkData.Count);
			foreach (Mem_quest checkDatum in checkData)
			{
				string funcName = getFuncName(checkDatum);
				if ((bool)GetType().InvokeMember(funcName, BindingFlags.InvokeMethod, null, this, new object[1]
				{
					checkDatum
				}))
				{
					checkDatum.StateChange(this, QuestState.COMPLETE);
					list.Add(checkDatum.Rid);
				}
			}
			return list;
		}

		public bool Check_01(Mem_quest targetQuest)
		{
			if (type != 2)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_02(Mem_quest targetQuest)
		{
			if (type != 1)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_03(Mem_quest targetQuest)
		{
			if (type != 1)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_04(Mem_quest targetQuest)
		{
			if (type != 2)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}
	}
}
