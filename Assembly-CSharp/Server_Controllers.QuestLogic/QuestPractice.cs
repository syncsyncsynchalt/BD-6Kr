using Common.Enum;
using Server_Common.Formats;
using Server_Common.Formats.Battle;
using Server_Models;
using System.Collections.Generic;
using System.Reflection;

namespace Server_Controllers.QuestLogic
{
	public class QuestPractice : QuestLogicBase
	{
		private BattleResultFmt battleResult;

		private DeckPracticeType deckPrackType;

		private PracticeDeckResultFmt deckPracticeResult;

		private QuestPractice()
		{
			checkData = getCheckDatas(3);
			battleResult = null;
			deckPracticeResult = null;
		}

		public QuestPractice(BattleResultFmt battleResultFmt)
			: this()
		{
			battleResult = battleResultFmt;
		}

		public QuestPractice(DeckPracticeType pracType, PracticeDeckResultFmt pracResultFmt)
			: this()
		{
			deckPrackType = pracType;
			deckPracticeResult = pracResultFmt;
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
			if (deckPracticeResult == null)
			{
				return false;
			}
			if (deckPrackType != DeckPracticeType.Normal)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_02(Mem_quest targetQuest)
		{
			if (deckPracticeResult == null)
			{
				return false;
			}
			if (deckPrackType != DeckPracticeType.Hou)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_03(Mem_quest targetQuest)
		{
			if (deckPracticeResult == null)
			{
				return false;
			}
			if (deckPrackType != DeckPracticeType.Rai)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_04(Mem_quest targetQuest)
		{
			if (deckPracticeResult == null)
			{
				return false;
			}
			if (deckPrackType != DeckPracticeType.Hou)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_05(Mem_quest targetQuest)
		{
			if (deckPracticeResult == null)
			{
				return false;
			}
			if (deckPrackType != DeckPracticeType.Normal && deckPrackType != DeckPracticeType.Rai)
			{
				return false;
			}
			Dictionary<int, int> dictionary = isAddCounter(targetQuest.Rid, checkData);
			int value = (deckPrackType == DeckPracticeType.Normal) ? 1 : 0;
			int value2 = (deckPrackType == DeckPracticeType.Rai) ? 1 : 0;
			if (dictionary.ContainsKey(3001))
			{
				dictionary[3001] = value;
			}
			if (dictionary.ContainsKey(3003))
			{
				dictionary[3003] = value2;
			}
			addCounter(dictionary);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_06(Mem_quest targetQuest)
		{
			if (battleResult == null)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_07(Mem_quest targetQuest)
		{
			if (battleResult == null)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_08(Mem_quest targetQuest)
		{
			if (battleResult == null)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_09(Mem_quest targetQuest)
		{
			if (deckPracticeResult == null)
			{
				return false;
			}
			if (deckPrackType != DeckPracticeType.Normal)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_10(Mem_quest targetQuest)
		{
			if (deckPracticeResult == null)
			{
				return false;
			}
			if (deckPrackType != DeckPracticeType.Hou)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_11(Mem_quest targetQuest)
		{
			if (deckPracticeResult == null)
			{
				return false;
			}
			if (deckPrackType != DeckPracticeType.Normal && deckPrackType != DeckPracticeType.Rai)
			{
				return false;
			}
			Dictionary<int, int> dictionary = isAddCounter(targetQuest.Rid, checkData);
			int value = (deckPrackType == DeckPracticeType.Normal) ? 1 : 0;
			int value2 = (deckPrackType == DeckPracticeType.Rai) ? 1 : 0;
			if (dictionary.ContainsKey(3001))
			{
				dictionary[3001] = value;
			}
			if (dictionary.ContainsKey(3003))
			{
				dictionary[3003] = value2;
			}
			addCounter(dictionary);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_12(Mem_quest targetQuest)
		{
			if (deckPracticeResult == null)
			{
				return false;
			}
			if (deckPrackType != DeckPracticeType.Taisen)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_13(Mem_quest targetQuest)
		{
			if (deckPracticeResult == null)
			{
				return false;
			}
			if (deckPrackType != DeckPracticeType.Sougou)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_14(Mem_quest targetQuest)
		{
			if (deckPracticeResult == null)
			{
				return false;
			}
			if (deckPrackType != DeckPracticeType.Kouku)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_15(Mem_quest targetQuest)
		{
			if (battleResult == null)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_16(Mem_quest targetQuest)
		{
			if (deckPracticeResult == null)
			{
				return false;
			}
			if (deckPrackType != DeckPracticeType.Taisen)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_17(Mem_quest targetQuest)
		{
			if (battleResult == null)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_18(Mem_quest targetQuest)
		{
			if (deckPracticeResult == null)
			{
				return false;
			}
			if (deckPrackType != DeckPracticeType.Kouku)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_19(Mem_quest targetQuest)
		{
			if (deckPracticeResult == null)
			{
				return false;
			}
			if (deckPrackType != DeckPracticeType.Sougou)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}
	}
}
