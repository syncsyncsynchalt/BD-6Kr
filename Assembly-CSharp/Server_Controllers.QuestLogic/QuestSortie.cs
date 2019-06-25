using Common.Enum;
using Server_Common;
using Server_Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Server_Controllers.QuestLogic
{
	public class QuestSortie : QuestLogicBase
	{
		private BattleWinRankKinds winKind;

		private List<Mem_ship> fShip;

		private List<Mem_ship> enemyShip;

		private int deckRid;

		private int areaId;

		private int mapNo;

		private bool boss;

		private Dictionary<int, Mst_ship> mstShip;

		private int checkType;

		public QuestSortie(int maparea_id, int mapinfo_no, int deckRid, List<Mem_ship> myShip)
		{
			areaId = maparea_id;
			mapNo = mapinfo_no;
			mstShip = Mst_DataManager.Instance.Mst_ship;
			checkData = getCheckDatas(2);
			fShip = myShip;
			winKind = BattleWinRankKinds.NONE;
			boss = false;
			checkType = 1;
			this.deckRid = deckRid;
		}

		public QuestSortie(int maparea_id, int mapinfo_no, BattleWinRankKinds winKind, int deckRid, List<Mem_ship> myShip, List<Mem_ship> enemyShip, bool boss)
			: this(maparea_id, mapinfo_no, deckRid, myShip)
		{
			this.boss = boss;
			this.winKind = winKind;
			this.enemyShip = enemyShip;
			checkType = 2;
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
			if (checkType != 2)
			{
				return false;
			}
			if (winKind == BattleWinRankKinds.S)
			{
				return true;
			}
			return false;
		}

		public bool Check_02(Mem_quest targetQuest)
		{
			if (fShip != null)
			{
				return true;
			}
			return false;
		}

		public bool Check_03(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId == 1 && mapNo == 1 && boss && Utils.IsBattleWin(winKind))
			{
				return true;
			}
			return false;
		}

		public bool Check_04(Mem_quest targetQuest)
		{
			if (checkType != 1)
			{
				return false;
			}
			if (areaId == 1 && mapNo == 2 && fShip != null)
			{
				return true;
			}
			return false;
		}

		public bool Check_05(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId == 1 && mapNo == 2 && boss && Utils.IsBattleWin(winKind))
			{
				return true;
			}
			return false;
		}

		public bool Check_06(Mem_quest targetQuest)
		{
			if (checkType != 1)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (fShip[0].Stype == 3 && stypeCount[2] >= 2)
			{
				return true;
			}
			return false;
		}

		public bool Check_07(Mem_quest targetQuest)
		{
			if (checkType != 1)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(5);
			HashSet<int> target = hashSet;
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (stypeCount[5] >= 1)
			{
				return true;
			}
			return false;
		}

		public bool Check_08(Mem_quest targetQuest)
		{
			if (checkType != 1)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(8);
			hashSet.Add(9);
			hashSet.Add(10);
			HashSet<int> target = hashSet;
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (stypeCount[8] + stypeCount[9] + stypeCount[10] >= 1)
			{
				return true;
			}
			return false;
		}

		public bool Check_09(Mem_quest targetQuest)
		{
			if (checkType != 1)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> target = hashSet;
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (fShip.Count >= 4 && stypeCount[7] + stypeCount[11] + stypeCount[18] >= 1)
			{
				return true;
			}
			return false;
		}

		public bool Check_10(Mem_quest targetQuest)
		{
			if (checkType != 1)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_11(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			List<Mem_ship> destroyShip = getDestroyShip(enemyShip);
			int num = destroyShip.Count((Mem_ship x) => Mst_DataManager.Instance.Mst_stype[x.Stype].IsMother());
			if (num == 0)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_12(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			List<Mem_ship> destroyShip = getDestroyShip(enemyShip);
			Dictionary<int, int> stypeCount = getStypeCount(destroyShip, new HashSet<int>
			{
				15
			});
			if (stypeCount[15] == 0)
			{
				return false;
			}
			for (int i = 0; i < stypeCount[15]; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_13(Mem_quest targetQuest)
		{
			return Check_12(targetQuest);
		}

		public bool Check_14(Mem_quest targetQuest)
		{
			Dictionary<int, int> dictionary = isAddCounter(targetQuest.Rid, checkData);
			if (checkType == 1)
			{
				if (dictionary.ContainsKey(2011))
				{
					dictionary[2011] = 1;
					addCounter(dictionary);
				}
				return CheckClearCounter(targetQuest.Rid);
			}
			dictionary.Remove(2011);
			int value = 0;
			int value2 = 0;
			if (boss)
			{
				value = 1;
				value2 = (Utils.IsBattleWin(winKind) ? 1 : 0);
			}
			int value3 = (winKind == BattleWinRankKinds.S) ? 1 : 0;
			if (dictionary.ContainsKey(2012))
			{
				dictionary[2012] = value;
			}
			if (dictionary.ContainsKey(2013))
			{
				dictionary[2013] = value2;
			}
			if (dictionary.ContainsKey(2014))
			{
				dictionary[2014] = value3;
			}
			addCounter(dictionary);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_15(Mem_quest targetQuest)
		{
			if (checkType != 1)
			{
				return false;
			}
			if (deckRid == 2)
			{
				return true;
			}
			return false;
		}

		public bool Check_16(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			if (!Utils.IsBattleWin(winKind) || !boss)
			{
				return false;
			}
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_17(Mem_quest targetQuest)
		{
			return Check_11(targetQuest);
		}

		public bool Check_18(Mem_quest targetQuest)
		{
			return Check_12(targetQuest);
		}

		public bool Check_19(Mem_quest targetQuest)
		{
			if (checkType != 1)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ちょうかい");
			hashSet.Add("かこ");
			hashSet.Add("あおば");
			hashSet.Add("ふるたか");
			hashSet.Add("てんりゅう");
			HashSet<string> target = hashSet;
			if (fShip.Count < 6)
			{
				return false;
			}
			if (!containsAllYomi(fShip, target))
			{
				return false;
			}
			int num = 0;
			foreach (Mem_ship item in fShip)
			{
				if (mstShip[item.Ship_id].Soku == 10)
				{
					num++;
				}
			}
			if (num == 6)
			{
				return true;
			}
			return false;
		}

		public bool Check_20(Mem_quest targetQuest)
		{
			return Check_11(targetQuest);
		}

		public bool Check_21(Mem_quest targetQuest)
		{
			return Check_12(targetQuest);
		}

		public bool Check_22(Mem_quest targetQuest)
		{
			if (checkType != 1)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あかつき");
			hashSet.Add("ひびき");
			hashSet.Add("いかづち");
			hashSet.Add("いなづま");
			HashSet<string> target = hashSet;
			if (fShip.Count < 4)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_23(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あたご");
			hashSet.Add("たかお");
			hashSet.Add("ちょうかい");
			hashSet.Add("まや");
			HashSet<string> target = hashSet;
			if (fShip.Count < 4 || areaId != 1 || mapNo != 4 || !boss || !Utils.IsBattleWin(winKind))
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_24(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふそう");
			hashSet.Add("やましろ");
			hashSet.Add("もがみ");
			hashSet.Add("しぐれ");
			HashSet<string> target = hashSet;
			if (fShip.Count < 4 || areaId != 9 || mapNo != 4 || !boss || !Utils.IsBattleWin(winKind))
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_25(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("しょうかく");
			hashSet.Add("ずいかく");
			HashSet<string> target = hashSet;
			if (fShip.Count < 2 || areaId != 11 || mapNo != 4 || !boss || !Utils.IsBattleWin(winKind))
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_26(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 7)
			{
				return false;
			}
			if (!boss || !Utils.IsBattleWin(winKind))
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_27(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ちょうかい");
			hashSet.Add("あおば");
			hashSet.Add("きぬがさ");
			hashSet.Add("かこ");
			hashSet.Add("ふるたか");
			hashSet.Add("てんりゅう");
			HashSet<string> target = hashSet;
			if (fShip.Count != 6 || areaId != 7 || mapNo != 2 || !boss || !Utils.IsBattleWin(winKind))
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_28(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			List<Mem_ship> destroyShip = getDestroyShip(enemyShip);
			int num = destroyShip.Count((Mem_ship x) => Mst_DataManager.Instance.Mst_stype[x.Stype].IsSubmarine());
			if (num == 0)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_29(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 4 || mapNo > 4)
			{
				return false;
			}
			if (!boss || !Utils.IsBattleWin(winKind))
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_30(Mem_quest targetQuest)
		{
			return Check_28(targetQuest);
		}

		public bool Check_31(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(13);
			hashSet.Add(14);
			HashSet<int> target = hashSet;
			if (areaId != 11 || mapNo != 2 || !boss || !Utils.IsBattleWin(winKind))
			{
				return false;
			}
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (stypeCount[13] + stypeCount[14] == fShip.Count && stypeCount[13] + stypeCount[14] >= 2)
			{
				return true;
			}
			return false;
		}

		public bool Check_32(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(6);
			hashSet.Add(10);
			HashSet<int> target = hashSet;
			if (fShip.Count < 4 || areaId != 8 || mapNo != 4 || !boss || !Utils.IsBattleWin(winKind))
			{
				return false;
			}
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (stypeCount[6] >= 2 && stypeCount[10] >= 2)
			{
				return true;
			}
			return false;
		}

		public bool Check_33(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふるたか");
			hashSet.Add("かこ");
			hashSet.Add("あおば");
			hashSet.Add("きぬがさ");
			HashSet<string> target = hashSet;
			if (fShip.Count < 4 || areaId != 11 || mapNo != 3 || !boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_34(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 8 || mapNo != 2)
			{
				return false;
			}
			if (!boss || !Utils.IsBattleWin(winKind))
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_35(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId == 9 && mapNo == 1 && boss && Utils.IsBattleWin(winKind))
			{
				return true;
			}
			return false;
		}

		public bool Check_36(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId == 7 && mapNo == 1 && boss && Utils.IsBattleWin(winKind))
			{
				return true;
			}
			return false;
		}

		public bool Check_37(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId == 1 && mapNo == 4 && boss && Utils.IsBattleWin(winKind))
			{
				return true;
			}
			return false;
		}

		public bool Check_38(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId == 9 && mapNo == 2 && boss && Utils.IsBattleWin(winKind))
			{
				return true;
			}
			return false;
		}

		public bool Check_39(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId == 11 && mapNo == 2 && boss && Utils.IsBattleWin(winKind))
			{
				return true;
			}
			return false;
		}

		public bool Check_40(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("かすみ");
			hashSet.Add("あられ");
			hashSet.Add("かげろう");
			hashSet.Add("しらぬい");
			HashSet<string> target = hashSet;
			if (areaId != 3 || mapNo != 1)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || !Utils.IsBattleWin(winKind))
			{
				return false;
			}
			if (winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_41(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 3 || (mapNo != 3 && mapNo != 4))
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> dictionary = isAddCounter(targetQuest.Rid, checkData);
			int value = (mapNo == 3) ? 1 : 0;
			int value2 = (mapNo == 4) ? 1 : 0;
			if (dictionary.ContainsKey(2201))
			{
				dictionary[2201] = value;
			}
			if (dictionary.ContainsKey(2202))
			{
				dictionary[2202] = value2;
			}
			addCounter(dictionary);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_42(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 4 || mapNo != 4)
			{
				return false;
			}
			if (boss && winKind >= BattleWinRankKinds.A)
			{
				return true;
			}
			return false;
		}

		public bool Check_43(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 5 || mapNo != 2)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_44(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("むつき");
			hashSet.Add("きさらぎ");
			hashSet.Add("やよい");
			hashSet.Add("もちづき");
			HashSet<string> target = hashSet;
			if (areaId != 3 || mapNo != 2)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind < BattleWinRankKinds.C)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_45(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (deckRid != 1)
			{
				return false;
			}
			if (areaId != 11 || mapNo != 2)
			{
				return false;
			}
			if (fShip[0].Level < 90 || fShip[0].Level > 99)
			{
				return false;
			}
			if (boss && winKind == BattleWinRankKinds.S)
			{
				return true;
			}
			return false;
		}

		public bool Check_46(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (deckRid != 1)
			{
				return false;
			}
			if (areaId != 4 || mapNo != 3)
			{
				return false;
			}
			if (fShip[0].Level < 100)
			{
				return false;
			}
			if (boss && winKind == BattleWinRankKinds.S)
			{
				return true;
			}
			return false;
		}

		public bool Check_47(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(10);
			HashSet<int> target = hashSet;
			if (areaId != 4 || mapNo != 1)
			{
				return false;
			}
			if (fShip.Count < 2)
			{
				return false;
			}
			if (!boss || !Utils.IsBattleWin(winKind))
			{
				return false;
			}
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (stypeCount[10] >= 2)
			{
				return true;
			}
			return false;
		}

		public bool Check_48(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("むつき");
			hashSet.Add("やよい");
			hashSet.Add("うづき");
			hashSet.Add("もちづき");
			HashSet<string> target = hashSet;
			if (areaId != 7 || mapNo != 1)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind < BattleWinRankKinds.B)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_49(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("みょうこう");
			hashSet.Add("なち");
			hashSet.Add("はぐろ");
			HashSet<string> target = hashSet;
			if (areaId != 11 || mapNo != 4)
			{
				return false;
			}
			if (fShip.Count < 3)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_50(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("そうりゅう");
			HashSet<string> target = hashSet;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(2);
			HashSet<int> target2 = hashSet2;
			if (areaId != 5 || mapNo != 2)
			{
				return false;
			}
			if (fShip[0].Ship_id != 196)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (!containsAllYomi(fShip, target))
			{
				return false;
			}
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target2);
			if (stypeCount[2] >= 2)
			{
				return true;
			}
			return false;
		}

		public bool Check_51(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(196);
			HashSet<int> target = hashSet;
			hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target2 = hashSet;
			if (areaId != 4 || mapNo != 3)
			{
				return false;
			}
			if (fShip[0].Ship_id != 197)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (!containsAllShipId(fShip, target))
			{
				return false;
			}
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target2);
			if (stypeCount[2] >= 2)
			{
				return true;
			}
			return false;
		}

		public bool Check_52(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 7 || mapNo != 3)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (mstShip[fShip[0].Ship_id].Ctype == 53)
			{
				return true;
			}
			return false;
		}

		public bool Check_53(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(3);
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			if (areaId != 11 || mapNo != 1)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (fShip.Count != stypeCount[7] + stypeCount[3] + stypeCount[2])
			{
				return false;
			}
			if (stypeCount[7] != 1 && stypeCount[7] != 2)
			{
				return false;
			}
			if (stypeCount[3] == 1)
			{
				return true;
			}
			return false;
		}

		public bool Check_54(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(3);
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			if (areaId != 11 || mapNo != 1)
			{
				return false;
			}
			if (fShip[0].Stype != 3)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (fShip.Count != stypeCount[3] + stypeCount[2])
			{
				return false;
			}
			if (stypeCount[3] == 1 || stypeCount[3] == 2)
			{
				return true;
			}
			return false;
		}

		public bool Check_55(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 6 || mapNo != 1)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_56(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(3);
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			if (areaId != 1 || mapNo != 4)
			{
				return false;
			}
			if (fShip[0].Stype != 3)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (fShip.Count != stypeCount[3] + stypeCount[2])
			{
				return false;
			}
			if (stypeCount[3] >= 1 && stypeCount[3] <= 3)
			{
				return true;
			}
			return false;
		}

		public bool Check_57(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ながと");
			hashSet.Add("むつ");
			hashSet.Add("ふそう");
			hashSet.Add("やましろ");
			HashSet<string> target = hashSet;
			if (areaId != 4 || mapNo != 1)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (!containsAllYomi(fShip, target))
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_58(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
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
			if (areaId != 5 || mapNo != 1)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (stypeCount[3] < 1)
			{
				return false;
			}
			int num = fShip.Count((Mem_ship x) => enableCtype.Contains(mstShip[x.Ship_id].Ctype));
			if (num == 3)
			{
				return true;
			}
			return false;
		}

		public bool Check_59(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(8);
			hashSet.Add(9);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> target = hashSet;
			if (areaId != 3 || mapNo != 3)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (stypeCount[8] + stypeCount[9] == 2 && stypeCount[7] == 1 && stypeCount[11] == 0 && stypeCount[18] == 0)
			{
				return true;
			}
			return false;
		}

		public bool Check_60(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 9 || mapNo != 4)
			{
				return false;
			}
			if (boss && winKind >= BattleWinRankKinds.B)
			{
				return true;
			}
			return false;
		}

		public bool Check_61(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふそう");
			hashSet.Add("やましろ");
			hashSet.Add("もがみ");
			hashSet.Add("しぐれ");
			hashSet.Add("みちしお");
			HashSet<string> target = hashSet;
			if (areaId != 5 || mapNo != 1)
			{
				return false;
			}
			if (fShip.Count < 5)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_62(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふるたか");
			hashSet.Add("かこ");
			hashSet.Add("あおば");
			hashSet.Add("きぬがさ");
			HashSet<string> target = hashSet;
			if (areaId != 11 || mapNo != 3)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_63(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			hashSet.Add(7);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> target = hashSet;
			if (areaId != 4 || mapNo != 1)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (stypeCount[2] >= 2 && stypeCount[7] + stypeCount[11] + stypeCount[18] >= 2)
			{
				return true;
			}
			return false;
		}

		public bool Check_64(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 2 || mapNo != 3)
			{
				return false;
			}
			if (boss && winKind >= BattleWinRankKinds.B)
			{
				return true;
			}
			return false;
		}

		public bool Check_65(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			hashSet.Add(3);
			hashSet.Add(5);
			HashSet<int> target = hashSet;
			if (areaId != 11 || mapNo != 4)
			{
				return false;
			}
			if (fShip.Count < 4 || fShip[0].Stype != 2)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (stypeCount[3] == 1 && stypeCount[5] == 1 && stypeCount[2] == 2)
			{
				return true;
			}
			return false;
		}

		public bool Check_66(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふぶき");
			hashSet.Add("しらゆき");
			hashSet.Add("はつゆき");
			hashSet.Add("むらくも");
			HashSet<string> target = hashSet;
			if (areaId != 11 || mapNo != 1)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || !Utils.IsBattleWin(winKind))
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_67(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 10 || mapNo != 3)
			{
				return false;
			}
			if (boss && winKind >= BattleWinRankKinds.B)
			{
				return true;
			}
			return false;
		}

		public bool Check_68(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("はつはる");
			hashSet.Add("ねのひ");
			hashSet.Add("わかば");
			hashSet.Add("はつしも");
			HashSet<string> target = hashSet;
			if (areaId != 3 || mapNo != 1)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_69(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("さつき");
			hashSet.Add("ふみづき");
			hashSet.Add("ながつき");
			HashSet<string> target = hashSet;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(2);
			HashSet<int> target2 = hashSet2;
			if (areaId != 1 || mapNo != 4)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (!containsAllYomi(fShip, target))
			{
				return false;
			}
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target2);
			if (stypeCount[2] >= 4)
			{
				return true;
			}
			return false;
		}

		public bool Check_70(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("はつしも");
			hashSet.Add("かすみ");
			hashSet.Add("うしお");
			hashSet.Add("あけぼの");
			HashSet<string> target = hashSet;
			if (areaId != 11 || mapNo != 1)
			{
				return false;
			}
			if (fShip.Count != 6)
			{
				return false;
			}
			if (!(mstShip[fShip[0].Ship_id].Yomi == "なち"))
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_71(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			HashSet<int> enableId = new HashSet<int>
			{
				271,
				428
			};
			if (areaId != 7 || mapNo != 2)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind < BattleWinRankKinds.C)
			{
				return false;
			}
			int num = fShip.Count((Mem_ship x) => enableId.Contains(x.Ship_id));
			if (num != 1)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (stypeCount[2] >= 3)
			{
				return true;
			}
			return false;
		}

		public bool Check_72(Mem_quest targetQuest)
		{
			if (checkType != 2)
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
			if (areaId != 5 || mapNo != 1)
			{
				return false;
			}
			if (deckRid != 1)
			{
				return false;
			}
			if (fShip.Count != 6 || fShip[0].Ship_id != 427)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			int num = fShip.Count(delegate(Mem_ship x)
			{
				Mst_ship value = null;
				return mstShip.TryGetValue(x.Ship_id, out value) && enableYomi.Contains(value.Yomi);
			});
			if (num == 5)
			{
				return true;
			}
			return false;
		}

		public bool Check_73(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あかつき");
			hashSet.Add("ひびき");
			hashSet.Add("いかづち");
			hashSet.Add("いなづま");
			HashSet<string> target = hashSet;
			if (areaId != 9 || mapNo != 2)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind < BattleWinRankKinds.C)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_74(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("てんりゅう");
			hashSet.Add("たつた");
			HashSet<string> target = hashSet;
			if (areaId != 9 || mapNo != 1)
			{
				return false;
			}
			if (fShip.Count < 2)
			{
				return false;
			}
			if (!boss || winKind < BattleWinRankKinds.C)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_75(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ひえい");
			hashSet.Add("きりしま");
			hashSet.Add("ながら");
			hashSet.Add("あかつき");
			hashSet.Add("いかづち");
			hashSet.Add("いなづま");
			HashSet<string> target = hashSet;
			if (areaId != 5 || mapNo != 3)
			{
				return false;
			}
			if (fShip.Count != 6)
			{
				return false;
			}
			if (!boss || winKind < BattleWinRankKinds.C)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_76(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あかつき");
			hashSet.Add("ひびき");
			hashSet.Add("いかづち");
			hashSet.Add("いなづま");
			HashSet<string> target = hashSet;
			if (areaId != 9 || mapNo != 1)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind < BattleWinRankKinds.A)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_77(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ひびき");
			hashSet.Add("はつしも");
			hashSet.Add("わかば");
			hashSet.Add("さみだれ");
			hashSet.Add("しまかぜ");
			if (areaId != 3 || mapNo != 2)
			{
				return false;
			}
			if (fShip.Count != 6 || !(mstShip[fShip[0].Ship_id].Yomi == "あぶくま"))
			{
				return false;
			}
			if (boss && winKind >= BattleWinRankKinds.B)
			{
				return true;
			}
			return false;
		}

		public bool Check_78(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ひびき");
			hashSet.Add("ゆうぐも");
			hashSet.Add("ながなみ");
			hashSet.Add("あきぐも");
			hashSet.Add("しまかぜ");
			if (areaId != 3 || mapNo != 2)
			{
				return false;
			}
			if (fShip.Count != 6 || fShip[0].Ship_id != 200)
			{
				return false;
			}
			if (boss && winKind == BattleWinRankKinds.S)
			{
				return true;
			}
			return false;
		}

		public bool Check_79(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 3 || mapNo != 1)
			{
				return false;
			}
			if (boss && winKind >= BattleWinRankKinds.B)
			{
				return true;
			}
			return false;
		}

		public bool Check_80(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 13 || (mapNo != 2 && mapNo != 3))
			{
				return false;
			}
			if (!boss || winKind < BattleWinRankKinds.B)
			{
				return false;
			}
			Dictionary<int, int> dictionary = isAddCounter(targetQuest.Rid, checkData);
			int value = (mapNo == 2) ? 1 : 0;
			int value2 = (mapNo == 3) ? 1 : 0;
			if (dictionary.ContainsKey(2206))
			{
				dictionary[2206] = value;
			}
			if (dictionary.ContainsKey(2207))
			{
				dictionary[2207] = value2;
			}
			addCounter(dictionary);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_81(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 12)
			{
				return false;
			}
			if (!boss || winKind < BattleWinRankKinds.B)
			{
				return false;
			}
			Dictionary<int, int> dictionary = isAddCounter(targetQuest.Rid, checkData);
			int value = (mapNo == 1) ? 1 : 0;
			int value2 = (mapNo == 2) ? 1 : 0;
			int value3 = (mapNo == 3) ? 1 : 0;
			int value4 = (mapNo == 4) ? 1 : 0;
			if (dictionary.ContainsKey(2208))
			{
				dictionary[2208] = value;
			}
			if (dictionary.ContainsKey(2209))
			{
				dictionary[2209] = value2;
			}
			if (dictionary.ContainsKey(2210))
			{
				dictionary[2210] = value3;
			}
			if (dictionary.ContainsKey(2211))
			{
				dictionary[2211] = value4;
			}
			addCounter(dictionary);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_82(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 4 || mapNo < 1 || mapNo > 4)
			{
				return false;
			}
			if (!boss || winKind < BattleWinRankKinds.B)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_83(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 14 || mapNo != 4)
			{
				return false;
			}
			if (boss && winKind >= BattleWinRankKinds.B)
			{
				return true;
			}
			return false;
		}

		public bool Check_84(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 2 || mapNo != 4)
			{
				return false;
			}
			if (fShip[0].Stype != 7 && fShip[0].Stype != 11 && fShip[0].Stype != 18)
			{
				return false;
			}
			if (boss && winKind >= BattleWinRankKinds.A)
			{
				return true;
			}
			return false;
		}

		public bool Check_85(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 4 || mapNo != 1)
			{
				return false;
			}
			if (boss && winKind >= BattleWinRankKinds.B)
			{
				return true;
			}
			return false;
		}

		public bool Check_86(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("しょうかく");
			hashSet.Add("ずいかく");
			hashSet.Add("おぼろ");
			hashSet.Add("あきぐも");
			HashSet<string> target = hashSet;
			if (areaId != 5 || mapNo != 2)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_87(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("たま");
			hashSet.Add("きそ");
			HashSet<string> target = hashSet;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(192);
			hashSet2.Add(193);
			HashSet<int> target2 = hashSet2;
			if (areaId != 13 || mapNo != 2)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (!containsAllShipId(fShip, target2))
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_88(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("くま");
			hashSet.Add("ながら");
			HashSet<string> target = hashSet;
			if (areaId != 8 || mapNo != 3)
			{
				return false;
			}
			if (fShip.Count < 3 || !(mstShip[fShip[0].Ship_id].Yomi == "あしがら"))
			{
				return false;
			}
			if (!boss || winKind < BattleWinRankKinds.B)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_89(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あさしお");
			hashSet.Add("みちしお");
			hashSet.Add("おおしお");
			hashSet.Add("あらしお");
			HashSet<string> target = hashSet;
			if (areaId != 11 || mapNo != 2)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_90(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 5 || mapNo != 4)
			{
				return false;
			}
			if (boss && winKind >= BattleWinRankKinds.B)
			{
				return true;
			}
			return false;
		}

		public bool Check_91(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 16 || mapNo != 3)
			{
				return false;
			}
			if (boss && winKind >= BattleWinRankKinds.B)
			{
				return true;
			}
			return false;
		}

		public bool Check_92(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
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
			if (areaId != 1 || mapNo != 4)
			{
				return false;
			}
			if (fShip.Count < 4)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (fShip[0].Ship_id != 112 && fShip[0].Ship_id != 462 && fShip[0].Ship_id != 467)
			{
				return false;
			}
			int num = fShip.Count((Mem_ship x) => enableId.Contains(x.Ship_id));
			if (num != 2)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_93(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
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
			if (areaId != 11 || mapNo != 4)
			{
				return false;
			}
			if (fShip.Count != 6)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (fShip[0].Ship_id != 112 && fShip[0].Ship_id != 462 && fShip[0].Ship_id != 467)
			{
				return false;
			}
			int num = fShip.Count((Mem_ship x) => enableId.Contains(x.Ship_id));
			if (num == 5)
			{
				return true;
			}
			return false;
		}

		public bool Check_94(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("いすず");
			hashSet.Add("きぬ");
			HashSet<string> target = hashSet;
			if (areaId != 4 || mapNo != 2)
			{
				return false;
			}
			if (fShip.Count < 3 || !(mstShip[fShip[0].Ship_id].Yomi == "なとり"))
			{
				return false;
			}
			if (!boss || winKind < BattleWinRankKinds.B)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_95(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			hashSet.Add(6);
			hashSet.Add(7);
			hashSet.Add(10);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> target = hashSet;
			if (areaId != 3 || mapNo != 4)
			{
				return false;
			}
			if (fShip.Count != 6)
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = getStypeCount(fShip, target);
			if (stypeCount[2] == 2 && stypeCount[7] + stypeCount[11] + stypeCount[18] == 2 && stypeCount[6] + stypeCount[10] == 2)
			{
				return true;
			}
			return false;
		}

		public bool Check_96(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あしがら");
			hashSet.Add("おおよど");
			hashSet.Add("あさしも");
			hashSet.Add("きよしも");
			HashSet<string> target = hashSet;
			if (areaId != 11 || mapNo != 2)
			{
				return false;
			}
			if (fShip.Count < 5 || !(mstShip[fShip[0].Ship_id].Yomi == "かすみ"))
			{
				return false;
			}
			if (!boss || winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (containsAllYomi(fShip, target))
			{
				return true;
			}
			return false;
		}

		public bool Check_97(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (areaId != 7 || mapNo != 2)
			{
				return false;
			}
			if (boss && winKind >= BattleWinRankKinds.B)
			{
				return true;
			}
			return false;
		}

		public bool Check_98(Mem_quest targetQuest)
		{
			if (checkType != 2)
			{
				return false;
			}
			if (mstShip[fShip[0].Ship_id].Yomi != "やまと")
			{
				return false;
			}
			if (areaId != 12 || mapNo != 3)
			{
				return false;
			}
			if (boss && winKind == BattleWinRankKinds.S)
			{
				return true;
			}
			return false;
		}

		private bool containsAllYomi(List<Mem_ship> ships, HashSet<string> target)
		{
			int num = ships.Count(delegate(Mem_ship x)
			{
				Mst_ship value = null;
				return mstShip.TryGetValue(x.Ship_id, out value) && target.Contains(value.Yomi);
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

		private List<Mem_ship> getDestroyShip(List<Mem_ship> ships)
		{
			if (ships == null)
			{
				return new List<Mem_ship>();
			}
			return (from x in ships
				where !x.IsFight()
				select x).ToList();
		}
	}
}
