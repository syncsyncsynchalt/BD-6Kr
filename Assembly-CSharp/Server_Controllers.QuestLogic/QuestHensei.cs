using Common.Enum;
using Server_Common;
using Server_Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Server_Controllers.QuestLogic
{
	public class QuestHensei : QuestLogicBase
	{
		private Dictionary<int, Mem_deck> userDeck;

		private Dictionary<int, List<Mem_ship>> userDeckShip;

		private Dictionary<int, Mst_ship> mstShips;

		public QuestHensei()
		{
			checkData = getCheckDatas(1);
			Init();
		}

		public QuestHensei(int questId)
		{
			checkData = new List<Mem_quest>();
			Mem_quest mem_quest = Comm_UserDatas.Instance.User_quest[questId];
			if (mem_quest.State == QuestState.RUNNING)
			{
				checkData.Add(mem_quest);
			}
			Init();
		}

		public override List<int> ExecuteCheck()
		{
			List<int> list = new List<int>(checkData.Count);
			foreach (Mem_quest checkDatum in checkData)
			{
				string funcName = getFuncName(checkDatum);
				if ((bool)GetType().InvokeMember(funcName, BindingFlags.InvokeMethod, null, this, null))
				{
					checkDatum.StateChange(this, QuestState.COMPLETE);
					list.Add(checkDatum.Rid);
				}
			}
			return list;
		}

		public bool Check_01()
		{
			return userDeckShip.Values.Any((List<Mem_ship> x) => x.Count >= 2);
		}

		public bool Check_02()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				Dictionary<int, int> stypeCount = getStypeCount(value, target);
				if (stypeCount[2] >= 4)
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_03()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				Dictionary<int, int> stypeCount = getStypeCount(value, target);
				if (stypeCount[2] >= 2 && value[0].Stype == 3)
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_04()
		{
			return userDeckShip.Values.Any((List<Mem_ship> x) => x.Count >= 6);
		}

		public bool Check_05()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(3);
			HashSet<int> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				Dictionary<int, int> stypeCount = getStypeCount(value, target);
				if (stypeCount[3] >= 2)
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_06()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(5);
			HashSet<int> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				Dictionary<int, int> stypeCount = getStypeCount(value, target);
				if (stypeCount[5] >= 2)
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_07()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(11);
			hashSet.Add(18);
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			int num = 1;
			int num2 = 2;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4)
				{
					Dictionary<int, int> stypeCount = getStypeCount(value, target);
					if (stypeCount[7] + stypeCount[11] + stypeCount[18] >= num && stypeCount[2] >= num2)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_08()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("てんりゅう");
			hashSet.Add("たつた");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_09()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("せんだい");
			hashSet.Add("じんつう");
			hashSet.Add("なか");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_10()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("みょうこう");
			hashSet.Add("なち");
			hashSet.Add("あしがら");
			hashSet.Add("はぐろ");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_11()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふそう");
			hashSet.Add("やましろ");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_12()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("いせ");
			hashSet.Add("ひゅうが");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_13()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(5);
			hashSet.Add(8);
			hashSet.Add(9);
			HashSet<int> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				Dictionary<int, int> stypeCount = getStypeCount(value, target);
				if (stypeCount[5] >= 2 && stypeCount[8] + stypeCount[9] >= 1)
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_14()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あかぎ");
			hashSet.Add("かが");
			hashSet.Add("ひりゅう");
			hashSet.Add("そうりゅう");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_15()
		{
			return (userDeckShip[2].Count >= 1) ? true : false;
		}

		public bool Check_16()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(16);
			HashSet<int> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				Dictionary<int, int> stypeCount = getStypeCount(value, target);
				if (stypeCount[16] >= 1)
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_17()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> target = hashSet;
			Dictionary<int, int> stypeCount = getStypeCount(userDeckShip[2], target);
			if (stypeCount[7] + stypeCount[11] + stypeCount[18] >= 1)
			{
				return true;
			}
			return false;
		}

		public bool Check_18()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("こんごう");
			hashSet.Add("ひえい");
			hashSet.Add("はるな");
			hashSet.Add("きりしま");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_19()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ちょうかい");
			hashSet.Add("かこ");
			hashSet.Add("あおば");
			hashSet.Add("ふるたか");
			hashSet.Add("てんりゅう");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 6 && containsAllYomi(value, target))
				{
					int num = 0;
					foreach (Mem_ship item in value)
					{
						if (mstShips[item.Ship_id].Soku == 10)
						{
							num++;
						}
					}
					if (num == 6)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_20()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あかつき");
			hashSet.Add("ひびき");
			hashSet.Add("いかづち");
			hashSet.Add("いなづま");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 4 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_21()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あたご");
			hashSet.Add("たかお");
			hashSet.Add("ちょうかい");
			hashSet.Add("まや");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_22()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふそう");
			hashSet.Add("やましろ");
			hashSet.Add("もがみ");
			hashSet.Add("しぐれ");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_23()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			HashSet<string> hashSet2 = new HashSet<string>();
			hashSet2.Add("しょうかく");
			hashSet2.Add("ずいかく");
			HashSet<string> target2 = hashSet2;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4 && containsAllYomi(value, target2))
				{
					Dictionary<int, int> stypeCount = getStypeCount(value, target);
					if (stypeCount[2] >= 2)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_24()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ちょうかい");
			hashSet.Add("あおば");
			hashSet.Add("きぬがさ");
			hashSet.Add("かこ");
			hashSet.Add("ふるたか");
			hashSet.Add("てんりゅう");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 6 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_25()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(13);
			hashSet.Add(14);
			HashSet<int> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 2)
				{
					Dictionary<int, int> stypeCount = getStypeCount(value, target);
					if (stypeCount[13] + stypeCount[14] >= 2 && stypeCount[13] + stypeCount[14] == value.Count)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_26()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(6);
			hashSet.Add(10);
			HashSet<int> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4)
				{
					Dictionary<int, int> stypeCount = getStypeCount(value, target);
					if (stypeCount[6] >= 2 && stypeCount[10] >= 2)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_27()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(13);
			hashSet.Add(14);
			HashSet<int> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 3)
				{
					Dictionary<int, int> stypeCount = getStypeCount(value, target);
					if (stypeCount[13] + stypeCount[14] >= 3 && stypeCount[13] + stypeCount[14] == value.Count)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_28()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あおば");
			hashSet.Add("きぬがさ");
			hashSet.Add("かこ");
			hashSet.Add("ふるたか");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_29()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("なち");
			hashSet.Add("あしがら");
			hashSet.Add("たま");
			hashSet.Add("きそ");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_30()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あぶくま");
			hashSet.Add("あけぼの");
			hashSet.Add("うしお");
			hashSet.Add("かすみ");
			hashSet.Add("しらぬい");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 5 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_31()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あさしお");
			hashSet.Add("みちしお");
			hashSet.Add("おおしお");
			hashSet.Add("あらしお");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 4 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_32()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("かすみ");
			hashSet.Add("あられ");
			hashSet.Add("かげろう");
			hashSet.Add("しらぬい");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 4 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_33()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("むつき");
			hashSet.Add("きさらぎ");
			hashSet.Add("やよい");
			hashSet.Add("もちづき");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 4 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_34()
		{
			if (userDeckShip[1].Count == 0)
			{
				return false;
			}
			int level = userDeckShip[1][0].Level;
			if (level >= 90 && level <= 99)
			{
				return true;
			}
			return false;
		}

		public bool Check_35()
		{
			if (userDeckShip[1].Count != 6)
			{
				return false;
			}
			int level = userDeckShip[1][0].Level;
			if (level >= 100)
			{
				return true;
			}
			return false;
		}

		public bool Check_36()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("むつき");
			hashSet.Add("やよい");
			hashSet.Add("うづき");
			hashSet.Add("もちづき");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 4 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_37()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("みょうこう");
			hashSet.Add("なち");
			hashSet.Add("はぐろ");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 3 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_38()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("そうりゅう");
			HashSet<string> target = hashSet;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(2);
			HashSet<int> target2 = hashSet2;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4 && value[0].Ship_id == 196 && containsAllYomi(value, target))
				{
					Dictionary<int, int> stypeCount = getStypeCount(value, target2);
					if (stypeCount[2] >= 2)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_39()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(13);
			hashSet.Add(14);
			HashSet<int> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 5 && value[0].Stype == 20)
				{
					Dictionary<int, int> stypeCount = getStypeCount(value, target);
					if (stypeCount[13] + stypeCount[14] >= 4)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_40()
		{
			if (userDeckShip[1].Count < 1)
			{
				return false;
			}
			return (userDeckShip[1][0].Ship_id == 319) ? true : false;
		}

		public bool Check_41()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(196);
			HashSet<int> target = hashSet;
			hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target2 = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4 && value[0].Ship_id == 197 && containsAllShipId(value, target))
				{
					Dictionary<int, int> stypeCount = getStypeCount(value, target2);
					if (stypeCount[2] >= 2)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_42()
		{
			if (userDeckShip[1].Count < 1)
			{
				return false;
			}
			return (mstShips[userDeckShip[1][0].Ship_id].Ctype == 53) ? true : false;
		}

		public bool Check_43()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ながと");
			hashSet.Add("むつ");
			hashSet.Add("ふそう");
			hashSet.Add("やましろ");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_44()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(3);
			HashSet<int> target = hashSet;
			HashSet<int> enableCtype = new HashSet<int>
			{
				37,
				19,
				2,
				26
			};
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4)
				{
					Dictionary<int, int> stypeCount = getStypeCount(value, target);
					if (stypeCount[3] >= 1)
					{
						int num = value.Count((Mem_ship x) => enableCtype.Contains(mstShips[x.Ship_id].Ctype));
						if (num == 3)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_45()
		{
			if (userDeckShip[1].Count < 1)
			{
				return false;
			}
			return (mstShips[userDeckShip[1][0].Ship_id].Yomi == "あかし") ? true : false;
		}

		public bool Check_46()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふそう");
			hashSet.Add("やましろ");
			hashSet.Add("もがみ");
			hashSet.Add("しぐれ");
			hashSet.Add("みちしお");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 5 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_47()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あしがら");
			HashSet<string> target = hashSet;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(2);
			hashSet2.Add(3);
			HashSet<int> target2 = hashSet2;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 6 && !(mstShips[value[0].Ship_id].Yomi != "かすみ") && containsAllYomi(value, target))
				{
					Dictionary<int, int> stypeCount = getStypeCount(value, target2);
					if (stypeCount[2] >= 4 && stypeCount[3] >= 1)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_48()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふぶき");
			hashSet.Add("しらゆき");
			hashSet.Add("はつゆき");
			hashSet.Add("むらくも");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 4 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_49()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("はつはる");
			hashSet.Add("ねのひ");
			hashSet.Add("わかば");
			hashSet.Add("はつしも");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 4 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_50()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("さつき");
			hashSet.Add("ふみづき");
			hashSet.Add("ながつき");
			HashSet<string> target = hashSet;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(2);
			HashSet<int> target2 = hashSet2;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 4 && containsAllYomi(value, target))
				{
					Dictionary<int, int> stypeCount = getStypeCount(value, target2);
					if (stypeCount[2] == 4)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_51()
		{
			if (userDeckShip[1].Count < 6)
			{
				return false;
			}
			if (userDeckShip[1][0].Ship_id != 427)
			{
				return false;
			}
			HashSet<string> enableYomi = new HashSet<string>
			{
				"あおば",
				"きぬがさ",
				"かこ",
				"ふるたか",
				"てんりゅう",
				"ゆうばり"
			};
			int num = userDeckShip[1].Count(delegate(Mem_ship x)
			{
				Mst_ship value = null;
				return mstShips.TryGetValue(x.Ship_id, out value) && enableYomi.Contains(value.Yomi);
			});
			return (num == 5) ? true : false;
		}

		public bool Check_52()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("てんりゅう");
			hashSet.Add("たつた");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_53()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ひえい");
			hashSet.Add("きりしま");
			hashSet.Add("ながら");
			hashSet.Add("あかつき");
			hashSet.Add("いかづち");
			hashSet.Add("いなづま");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 6 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_54()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ひびき");
			hashSet.Add("いかづち");
			hashSet.Add("いなづま");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 4 && value[0].Ship_id == 437 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_55()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ひびき");
			hashSet.Add("はつしも");
			hashSet.Add("わかば");
			hashSet.Add("さみだれ");
			hashSet.Add("しまかぜ");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 6 && !(mstShips[value[0].Ship_id].Yomi != "あぶくま") && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_56()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ひびき");
			hashSet.Add("ゆうぐも");
			hashSet.Add("ながなみ");
			hashSet.Add("あきぐも");
			hashSet.Add("しまかぜ");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 6 && value[0].Ship_id == 200 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_57()
		{
			return false;
		}

		public bool Check_58()
		{
			return false;
		}

		public bool Check_59()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ずいかく");
			hashSet.Add("ずいほう");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 6 && !(mstShips[value[0].Ship_id].Yomi != "しょうかく") && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_60()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("しょうかく");
			hashSet.Add("ずいかく");
			hashSet.Add("おぼろ");
			hashSet.Add("あきぐも");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4 && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_61()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("たま");
			hashSet.Add("きそ");
			HashSet<string> target = hashSet;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(192);
			hashSet2.Add(193);
			HashSet<int> target2 = hashSet2;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4 && containsAllShipId(value, target2) && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_62()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("くま");
			hashSet.Add("ながら");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 3 && !(mstShips[value[0].Ship_id].Yomi != "あしがら") && containsAllYomi(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_63()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ずいほう");
			HashSet<string> target = hashSet;
			HashSet<int> enableId = new HashSet<int>
			{
				108,
				291,
				296,
				109,
				292,
				297
			};
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4 && (value[0].Ship_id == 112 || value[0].Ship_id == 462 || value[0].Ship_id == 467))
				{
					int num = value.Count((Mem_ship x) => enableId.Contains(x.Ship_id));
					if (num == 2 && containsAllYomi(value, target))
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_64()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(82);
			hashSet.Add(88);
			HashSet<int> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 2 && containsAllShipId(value, target))
				{
					return true;
				}
			}
			return false;
		}

		public bool Check_65()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ずいほう");
			HashSet<int> enableId = new HashSet<int>
			{
				108,
				291,
				296,
				109,
				292,
				297,
				117,
				82,
				88
			};
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 6 && (value[0].Ship_id == 112 || value[0].Ship_id == 462 || value[0].Ship_id == 467))
				{
					int num = value.Count((Mem_ship x) => enableId.Contains(x.Ship_id));
					if (num == 5)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_66()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			HashSet<int> enableId = new HashSet<int>
			{
				461,
				466,
				462,
				467
			};
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4)
				{
					Dictionary<int, int> stypeCount = getStypeCount(value, target);
					if (stypeCount[2] >= 2)
					{
						int num = value.Count((Mem_ship x) => enableId.Contains(x.Ship_id));
						if (num == 2)
						{
							return true;
						}
						return false;
					}
				}
			}
			return false;
		}

		public bool Check_67()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("いすず");
			hashSet.Add("きぬ");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 3 && mstShips[value[0].Ship_id].Yomi == "なとり")
				{
					if (containsAllYomi(value, target))
					{
						return true;
					}
					return false;
				}
			}
			return false;
		}

		public bool Check_68()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			hashSet.Add(6);
			hashSet.Add(7);
			hashSet.Add(10);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 6)
				{
					Dictionary<int, int> stypeCount = getStypeCount(value, target);
					if (stypeCount[2] == 2 && stypeCount[7] + stypeCount[11] + stypeCount[18] == 2 && stypeCount[6] + stypeCount[10] == 2)
					{
						return true;
					}
					return false;
				}
			}
			return false;
		}

		public bool Check_69()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あしがら");
			hashSet.Add("おおよど");
			hashSet.Add("あさしも");
			hashSet.Add("きよしも");
			HashSet<string> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 5 && mstShips[value[0].Ship_id].Yomi == "かすみ")
				{
					if (containsAllYomi(value, target))
					{
						return true;
					}
					return false;
				}
			}
			return false;
		}

		public bool Check_70()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(13);
			hashSet.Add(14);
			HashSet<int> target = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count >= 4)
				{
					Dictionary<int, int> stypeCount = getStypeCount(value, target);
					if (stypeCount[13] + stypeCount[14] == value.Count && value[0].Level >= 80)
					{
						int num = 0;
						foreach (Mem_ship item in value)
						{
							if (item.Level >= 40)
							{
								num++;
							}
						}
						if (num >= 4)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_71()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(82);
			hashSet.Add(88);
			hashSet.Add(411);
			hashSet.Add(412);
			HashSet<int> target = hashSet;
			hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target2 = hashSet;
			foreach (List<Mem_ship> value in userDeckShip.Values)
			{
				if (value.Count == 6 && containsAllShipId(value, target))
				{
					Dictionary<int, int> stypeCount = getStypeCount(value, target2);
					if (stypeCount[2] == 2)
					{
						return true;
					}
				}
			}
			return false;
		}

		private void Init()
		{
			userDeck = Comm_UserDatas.Instance.User_deck;
			userDeckShip = new Dictionary<int, List<Mem_ship>>();
			foreach (KeyValuePair<int, Mem_deck> item in userDeck)
			{
				userDeckShip.Add(item.Key, item.Value.Ship.getMemShip());
			}
			mstShips = Mst_DataManager.Instance.Mst_ship;
		}

		private bool containsAllYomi(List<Mem_ship> ships, HashSet<string> target)
		{
			int num = ships.Count(delegate(Mem_ship x)
			{
				Mst_ship value = null;
				return mstShips.TryGetValue(x.Ship_id, out value) && target.Contains(value.Yomi);
			});
			return (target.Count() <= num) ? true : false;
		}

		private bool containsAllShipId(List<Mem_ship> ships, HashSet<int> target)
		{
			int num = ships.Count((Mem_ship x) => target.Contains(x.Ship_id));
			return (target.Count() <= num) ? true : false;
		}

		private Dictionary<int, int> getStypeCount(List<Mem_ship> ships, HashSet<int> target)
		{
			Dictionary<int, int> ret = target.ToDictionary((int x) => x, (int y) => 0);
			ships.ForEach(delegate(Mem_ship x)
			{
				if (target.Contains(x.Stype))
				{
					Dictionary<int, int> dictionary;
					Dictionary<int, int> dictionary2 = dictionary = ret;
					int stype;
					int key = stype = x.Stype;
					stype = dictionary[stype];
					dictionary2[key] = stype + 1;
				}
			});
			return ret;
		}
	}
}
