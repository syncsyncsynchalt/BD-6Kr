using Common.Enum;
using Server_Models;
using System.Collections.Generic;
using System.Reflection;

namespace Server_Controllers.QuestLogic
{
	public class QuestMission : QuestLogicBase
	{
		private Mem_deck missionDeck;

		private int missionId;

		private MissionResultKinds missionResult;

		private Dictionary<int, HashSet<int>> commonCounterEnableMission;

		public QuestMission(int mstId, Mem_deck deck, MissionResultKinds resultKind)
		{
			checkData = getCheckDatas(4);
			missionId = mstId;
			missionDeck = deck;
			missionResult = resultKind;
			commonCounterEnableMission = new Dictionary<int, HashSet<int>>
			{
				{
					43,
					new HashSet<int>
					{
						10,
						11,
						12,
						51,
						52
					}
				},
				{
					45,
					new HashSet<int>
					{
						60
					}
				},
				{
					46,
					new HashSet<int>
					{
						67
					}
				},
				{
					47,
					new HashSet<int>
					{
						45
					}
				},
				{
					48,
					new HashSet<int>
					{
						46
					}
				},
				{
					49,
					new HashSet<int>
					{
						52
					}
				},
				{
					50,
					new HashSet<int>
					{
						3,
						9,
						13,
						20
					}
				}
			};
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
			if (missionDeck == null)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_02(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_03(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_04(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (!commonCounterEnableMission[43].Contains(missionId))
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_05(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (!commonCounterEnableMission[45].Contains(missionId))
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_06(Mem_quest targetQuest)
		{
			return Check_05(targetQuest);
		}

		public bool Check_07(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (!commonCounterEnableMission[50].Contains(missionId))
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_08(Mem_quest targetQuest)
		{
			return Check_05(targetQuest);
		}

		public bool Check_09(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (!commonCounterEnableMission[46].Contains(missionId))
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_10(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			Dictionary<int, int> dictionary = isAddCounter(targetQuest.Rid, checkData);
			int value = commonCounterEnableMission[47].Contains(missionId) ? 1 : 0;
			int value2 = commonCounterEnableMission[48].Contains(missionId) ? 1 : 0;
			if (dictionary.ContainsKey(4007))
			{
				dictionary[4007] = value;
			}
			if (dictionary.ContainsKey(4008))
			{
				dictionary[4008] = value2;
			}
			addCounter(dictionary);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_11(Mem_quest targetQuest)
		{
			return Check_10(targetQuest);
		}

		public bool Check_12(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (!commonCounterEnableMission[49].Contains(missionId))
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_13(Mem_quest targetQuest)
		{
			return Check_12(targetQuest);
		}

		public bool Check_14(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (missionId != 36)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_15(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (missionId != 107)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_16(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (missionId != 4)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_17(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (missionId != 14)
			{
				return false;
			}
			return true;
		}

		public bool Check_18(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (missionId != 5)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_19(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (missionId != 6)
			{
				return false;
			}
			return true;
		}

		public bool Check_20(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			Dictionary<int, int> dictionary = isAddCounter(targetQuest.Rid, checkData);
			int value = (missionId == 44) ? 1 : 0;
			int value2 = (missionId == 66) ? 1 : 0;
			if (dictionary.ContainsKey(4205))
			{
				dictionary[4205] = value;
			}
			if (dictionary.ContainsKey(4206))
			{
				dictionary[4206] = value2;
			}
			addCounter(dictionary);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_21(Mem_quest targetQuest)
		{
			return Check_05(targetQuest);
		}

		public bool Check_22(Mem_quest targetQuest)
		{
			return Check_09(targetQuest);
		}

		public bool Check_23(Mem_quest targetQuest)
		{
			if (missionDeck == null)
			{
				return false;
			}
			if (missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			Dictionary<int, int> dictionary = isAddCounter(targetQuest.Rid, checkData);
			int value = (missionId == 84) ? 1 : 0;
			int value2 = (missionId == 108) ? 1 : 0;
			if (dictionary.ContainsKey(4207))
			{
				dictionary[4207] = value;
			}
			if (dictionary.ContainsKey(4208))
			{
				dictionary[4208] = value2;
			}
			addCounter(dictionary);
			return CheckClearCounter(targetQuest.Rid);
		}
	}
}
