using Common.Enum;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_AirBattle : BattleLogicBase<AirBattle>
	{
		protected BattleBaseData _f_Data;

		protected BattleBaseData _e_Data;

		protected Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		protected Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		protected Dictionary<Mem_ship, List<FighterInfo>> f_FighterInfo;

		protected Dictionary<Mem_ship, List<FighterInfo>> e_FighterInfo;

		protected bool[] isSearchSuccess;

		protected int[] seikuValue;

		protected double valance1;

		protected double valance2;

		protected double valance3;

		private Mst_slotitem fTouchPlane;

		private Mst_slotitem eTouchPlane;

		public override BattleBaseData F_Data => _f_Data;

		public override BattleBaseData E_Data => _e_Data;

		public override Dictionary<int, BattleShipSubInfo> F_SubInfo => _f_SubInfo;

		public override Dictionary<int, BattleShipSubInfo> E_SubInfo => _e_SubInfo;

		public Exec_AirBattle(BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, SearchInfo[] searchInfo, bool practice)
		{
			_f_Data = myData;
			_e_Data = enemyData;
			_f_SubInfo = mySubInfo;
			_e_SubInfo = enemySubInfo;
			practiceFlag = practice;
			setSearchData(searchInfo);
			seikuValue = new int[2];
			valance1 = 0.25;
			valance2 = 95.0;
			valance3 = 25.0;
		}

		public override void Dispose()
		{
			if (f_FighterInfo != null)
			{
				foreach (KeyValuePair<Mem_ship, List<FighterInfo>> item in f_FighterInfo)
				{
					item.Value.Clear();
				}
			}
			if (e_FighterInfo != null)
			{
				foreach (KeyValuePair<Mem_ship, List<FighterInfo>> item2 in e_FighterInfo)
				{
					item2.Value.Clear();
				}
			}
			randInstance = null;
			seikuValue = null;
		}

		public override AirBattle GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			commandParams = cParam;
			AirBattle airBattle = new AirBattle();
			if (!F_Data.ShipData.Any((Mem_ship x) => x.IsFight()) || !E_Data.ShipData.Any((Mem_ship y) => y.IsFight()))
			{
				airBattle.SetStageFlag();
				return airBattle;
			}
			setTargetingKind(formation);
			if (isSearchSuccess[0] && isAirBattleCommand())
			{
				setPlaneData(F_Data, F_SubInfo, airBattle.F_PlaneFrom, out f_FighterInfo);
			}
			if (isSearchSuccess[1])
			{
				setPlaneData(E_Data, E_SubInfo, airBattle.E_PlaneFrom, out e_FighterInfo);
			}
			formationData = formation;
			airBattle.Air1 = getAirBattle1();
			airBattle.Air2 = getAirBattle2();
			airBattle.Air3 = getAirBattle3(airBattle.Air2);
			airBattle.SetStageFlag();
			return airBattle;
		}

		private void setSearchData(SearchInfo[] info)
		{
			isSearchSuccess = new bool[2];
			if (info == null)
			{
				isSearchSuccess[0] = true;
				isSearchSuccess[1] = true;
				return;
			}
			for (int i = 0; i < info.Length; i++)
			{
				if (info[i].SearchValue == BattleSearchValues.Success || info[i].SearchValue == BattleSearchValues.Success_Lost || info[i].SearchValue == BattleSearchValues.Found)
				{
					isSearchSuccess[i] = true;
				}
				else
				{
					isSearchSuccess[i] = false;
				}
			}
		}

		protected virtual bool isAirBattleCommand()
		{
			return (F_Data.GetDeckBattleCommand()[0] == BattleCommand.Kouku) ? true : false;
		}

		protected void setPlaneData(BattleBaseData baseData, Dictionary<int, BattleShipSubInfo> subInfo, List<int> planeFrom, out Dictionary<Mem_ship, List<FighterInfo>> fighterData)
		{
			fighterData = null;
			int count = baseData.ShipData.Count;
			if (count > 0)
			{
				Dictionary<Mem_ship, List<FighterInfo>> dictionary = new Dictionary<Mem_ship, List<FighterInfo>>();
				foreach (var item3 in baseData.SlotData.Select((List<Mst_slotitem> items, int idx) => new
				{
					items,
					idx
				}))
				{
					Mem_ship mem_ship = baseData.ShipData[item3.idx];
					if (mem_ship.IsFight() && isAttackerFromTargetKind(subInfo[mem_ship.Rid]))
					{
						bool flag = false;
						dictionary.Add(mem_ship, new List<FighterInfo>());
						foreach (var item4 in item3.items.Select((Mst_slotitem item, int slot_pos) => new
						{
							item,
							slot_pos
						}))
						{
							if (mem_ship.Onslot[item4.slot_pos] > 0 && FighterInfo.ValidFighter(item4.item))
							{
								FighterInfo item2 = new FighterInfo(item4.slot_pos, item4.item);
								dictionary[mem_ship].Add(item2);
								flag = true;
							}
						}
						if (!flag)
						{
							dictionary.Remove(mem_ship);
						}
						else
						{
							planeFrom.Add(mem_ship.Rid);
						}
					}
				}
				if (dictionary.Count > 0)
				{
					fighterData = dictionary;
				}
			}
		}

		public int[] getSeikuValue()
		{
			return new int[2]
			{
				seikuValue[0],
				seikuValue[1]
			};
		}

		protected virtual AirBattle1 getAirBattle1()
		{
			if (f_FighterInfo == null && e_FighterInfo == null)
			{
				return null;
			}
			AirBattle1 airBattle = new AirBattle1();
			int deckSeikuPow = getDeckSeikuPow(f_FighterInfo, ref airBattle.F_LostInfo.Count);
			int deckSeikuPow2 = getDeckSeikuPow(e_FighterInfo, ref airBattle.E_LostInfo.Count);
			int merits = getMerits(deckSeikuPow, deckSeikuPow2, ref airBattle.SeikuKind);
			if (airBattle.F_LostInfo.Count > 0 && airBattle.E_LostInfo.Count == 0)
			{
				airBattle.SeikuKind = BattleSeikuKinds.Kakuho;
			}
			if (isSearchSuccess[0] && isSearchSuccess[1])
			{
				battleSeiku(merits, airBattle.E_LostInfo, enemyFlag: true);
				battleSeiku(merits, airBattle.F_LostInfo, enemyFlag: false);
			}
			if (seikuValue[0] >= 1 && seikuValue[0] <= 3)
			{
				airBattle.F_TouchPlane = getSyokusetuInfo(seikuValue[0], F_Data);
				Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(airBattle.F_TouchPlane, out fTouchPlane);
			}
			if (seikuValue[1] >= 1 && seikuValue[1] <= 3)
			{
				airBattle.E_TouchPlane = getSyokusetuInfo(seikuValue[1], E_Data);
				Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(airBattle.E_TouchPlane, out eTouchPlane);
			}
			return airBattle;
		}

		protected virtual int getSyokusetuInfo(int seiku, BattleBaseData baseData)
		{
			int num = 0;
			List<Mst_slotitem> list = new List<Mst_slotitem>();
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(8);
			hashSet.Add(9);
			hashSet.Add(10);
			hashSet.Add(41);
			HashSet<int> hashSet2 = hashSet;
			foreach (var item in baseData.ShipData.Select((Mem_ship obj, int ship_idx) => new
			{
				obj,
				ship_idx
			}))
			{
				Mem_ship obj2 = item.obj;
				for (int i = 0; i < obj2.Onslot.Count(); i++)
				{
					if (obj2.Onslot[i] > 0 && obj2.Slot[i] > 0)
					{
						Mst_slotitem mst_slotitem = baseData.SlotData[item.ship_idx][i];
						if (hashSet2.Contains(mst_slotitem.Api_mapbattle_type3))
						{
							list.Add(mst_slotitem);
						}
						if (mst_slotitem.Api_mapbattle_type3 != 8)
						{
							num += (int)((double)mst_slotitem.Saku * Math.Sqrt(obj2.Onslot[i]));
						}
					}
				}
			}
			int maxValue = 70 - seiku * 15;
			if (num < randInstance.Next(maxValue))
			{
				return 0;
			}
			int slotid = 0;
			int houm = -1;
			list.ForEach(delegate(Mst_slotitem x)
			{
				int num2 = randInstance.Next(20 - seiku * 2);
				if (houm < x.Houm && x.Saku > num2)
				{
					slotid = x.Id;
					houm = x.Houm;
				}
			});
			return slotid;
		}

		protected virtual int getDeckSeikuPow(Dictionary<Mem_ship, List<FighterInfo>> fighterData, ref int fighterCount)
		{
			int num = 150;
			int num2 = 0;
			if (fighterData == null)
			{
				return 0;
			}
			if (fighterData.Count == 0)
			{
				return 0;
			}
			Dictionary<int, int[]> dictionary = (!fighterData.First().Key.IsEnemy()) ? F_Data.SlotExperience : E_Data.SlotExperience;
			foreach (KeyValuePair<Mem_ship, List<FighterInfo>> fighterDatum in fighterData)
			{
				Mem_ship key = fighterDatum.Key;
				foreach (FighterInfo item in fighterDatum.Value)
				{
					int num3 = key.Onslot[item.SlotIdx];
					fighterCount += num3;
					int tyku = item.SlotData.Tyku;
					double num4 = (double)tyku * Math.Sqrt(num3);
					double num5 = 0.0;
					int key2 = key.Slot[item.SlotIdx];
					int[] value = null;
					if (dictionary.TryGetValue(key2, out value))
					{
						num5 = getAlvPlusToSeiku(num3, item.SlotData.Api_mapbattle_type3, value[0]);
						int slotExpUpValueToSeiku = getSlotExpUpValueToSeiku(num3, value[0], item.SlotData.Exp_rate);
						value[1] += slotExpUpValueToSeiku;
					}
					num4 += num5;
					num2 += (int)num4;
				}
			}
			if (num2 > num)
			{
				num2 = num + (num2 - num);
			}
			return num2;
		}

		protected virtual int getAlvPlusToSeiku(int onslotNum, int type3No, int slotExp)
		{
			if (onslotNum <= 0)
			{
				return 0;
			}
			double num = Math.Sqrt((double)slotExp * 0.1);
			switch (type3No)
			{
			case 6:
				if (slotExp >= 25)
				{
					num += 2.0;
				}
				if (slotExp >= 40)
				{
					num += 3.0;
				}
				if (slotExp >= 55)
				{
					num += 4.0;
				}
				if (slotExp >= 70)
				{
					num += 5.0;
				}
				if (slotExp >= 100)
				{
					num += 8.0;
				}
				break;
			case 11:
				if (slotExp >= 25)
				{
					num += 1.0;
				}
				if (slotExp >= 70)
				{
					num += 2.0;
				}
				if (slotExp >= 100)
				{
					num += 3.0;
				}
				break;
			}
			return (int)num;
		}

		protected virtual int getSlotExpUpValueToSeiku(int onslotNum, int slotExp, int expUpRate)
		{
			if (onslotNum <= 0)
			{
				return 0;
			}
			double num = expUpRate;
			double num2 = 0.88;
			double num3 = 10.0;
			if (slotExp > 100)
			{
				num3 = 6.0;
			}
			else if (slotExp > 50)
			{
				num3 = 8.0;
			}
			else if (slotExp < 20)
			{
				num3 = 12.0;
			}
			double randDouble = Utils.GetRandDouble(0.0, num3 - 1.0, 1.0, 1);
			double num4 = num * 0.5 + num * randDouble * 0.05 * num2;
			int num5 = slotExp + (int)num4;
			if (num5 > 120)
			{
				num5 = 120;
			}
			return num5 - slotExp;
		}

		protected virtual int getMerits(int f_seiku, int e_seiku, ref BattleSeikuKinds dispSeiku)
		{
			double num = (double)f_seiku * 3.0;
			double num2 = (double)f_seiku * 1.5;
			double num3 = (double)e_seiku * 3.0;
			double num4 = (double)e_seiku * 1.5;
			if ((double)f_seiku >= num3)
			{
				dispSeiku = BattleSeikuKinds.Kakuho;
				seikuValue[0] = 3;
				seikuValue[1] = 0;
				return 1;
			}
			if (num2 <= (double)e_seiku && (double)e_seiku < num)
			{
				dispSeiku = BattleSeikuKinds.Ressei;
				seikuValue[0] = 1;
				seikuValue[1] = 2;
				return 7;
			}
			if (num4 <= (double)f_seiku && (double)f_seiku < num3)
			{
				dispSeiku = BattleSeikuKinds.Yuusei;
				seikuValue[0] = 2;
				seikuValue[1] = 1;
				return 3;
			}
			if ((double)e_seiku >= num)
			{
				dispSeiku = BattleSeikuKinds.Lost;
				seikuValue[0] = 0;
				seikuValue[1] = 3;
				return 10;
			}
			dispSeiku = BattleSeikuKinds.None;
			seikuValue[0] = 0;
			seikuValue[1] = 0;
			return 5;
		}

		protected virtual void battleSeiku(int merits, LostPlaneInfo lostFmt, bool enemyFlag)
		{
			if (lostFmt.Count > 0)
			{
				Dictionary<int, int[]> slotExperience;
				Dictionary<Mem_ship, List<FighterInfo>> dictionary;
				Func<double> func;
				if (enemyFlag)
				{
					slotExperience = E_Data.SlotExperience;
					dictionary = e_FighterInfo;
					func = (() => (double)randInstance.Next(11 - merits + 1) * 0.35 + (double)randInstance.Next(11 - merits + 1) * 0.65);
				}
				else
				{
					slotExperience = F_Data.SlotExperience;
					dictionary = f_FighterInfo;
					func = delegate
					{
						double max = (double)merits / 3.0;
						return Utils.GetRandDouble(0.0, max, 0.1, 2) + (double)merits / 4.0;
					};
				}
				foreach (KeyValuePair<Mem_ship, List<FighterInfo>> item in dictionary)
				{
					Mem_ship key = item.Key;
					foreach (FighterInfo item2 in item.Value)
					{
						int slotIdx = item2.SlotIdx;
						double num = func();
						int num2 = (int)Math.Floor((double)key.Onslot[slotIdx] * num / 10.0);
						if (num2 > key.Onslot[slotIdx])
						{
							num2 = key.Onslot[slotIdx];
						}
						int[] value = null;
						if (slotExperience.TryGetValue(key.Slot[slotIdx], out value))
						{
							int slotExpSubValueToSeiku = getSlotExpSubValueToSeiku(key.Onslot[slotIdx], num2, value[0]);
							value[1] += slotExpSubValueToSeiku;
						}
						List<int> onslot;
						List<int> list = onslot = key.Onslot;
						int index;
						int index2 = index = slotIdx;
						index = onslot[index];
						list[index2] = index - num2;
						lostFmt.LostCount += num2;
					}
				}
			}
		}

		protected virtual int getSlotExpSubValueToSeiku(int onSlot, int lostNum, int slotExp)
		{
			if (onSlot <= 0 || lostNum <= 0)
			{
				return 0;
			}
			double num = onSlot;
			double num2 = lostNum;
			double num3 = slotExp;
			int num4 = (int)(num3 * (num2 / num) * 0.3);
			if (num2 / num > 0.5)
			{
				num4 = (int)(num3 * (num2 / num) * 0.5);
			}
			double num5 = (num4 - 1 > 0) ? Utils.GetRandDouble(0.0, num4, 1.0, 1) : 0.0;
			double num6 = num3 - (double)num4 * 0.5 + num5 * 0.05;
			if (num2 >= num || num6 < 0.0)
			{
				num6 = 0.0;
			}
			return (int)num6 - slotExp;
		}

		protected virtual AirBattle2 getAirBattle2()
		{
			AirBattle2 airBattle = new AirBattle2();
			List<Mem_ship> taikuHaveShip = selectTaikuPlane(f_FighterInfo, ref airBattle.F_LostInfo.Count);
			List<Mem_ship> taikuHaveShip2 = selectTaikuPlane(e_FighterInfo, ref airBattle.E_LostInfo.Count);
			bool flag = false;
			if (isSearchSuccess[1] && airBattle.E_LostInfo.Count > 0)
			{
				airBattle.F_AntiFire = getAntiAirFireAttacker(F_Data);
				int deckTaikuPow = getDeckTaikuPow(F_Data);
				airBattle.E_LostInfo.LostCount = battleTaiku(taikuHaveShip2, deckTaikuPow, airBattle.F_AntiFire);
				flag = true;
			}
			if (isSearchSuccess[0] && airBattle.F_LostInfo.Count > 0)
			{
				airBattle.E_AntiFire = getAntiAirFireAttacker(E_Data);
				int deckTaikuPow2 = getDeckTaikuPow(E_Data);
				airBattle.F_LostInfo.LostCount = battleTaiku(taikuHaveShip, deckTaikuPow2, airBattle.E_AntiFire);
				flag = true;
			}
			if (!flag)
			{
				return null;
			}
			return airBattle;
		}

		protected List<Mem_ship> selectTaikuPlane(Dictionary<Mem_ship, List<FighterInfo>> fighter, ref int planeCount)
		{
			if (fighter == null)
			{
				return null;
			}
			List<Mem_ship> list = new List<Mem_ship>();
			foreach (KeyValuePair<Mem_ship, List<FighterInfo>> item in fighter)
			{
				Mem_ship key = item.Key;
				int num = 0;
				foreach (FighterInfo item2 in item.Value)
				{
					if (item2.ValidTaiku() && key.Onslot[item2.SlotIdx] > 0)
					{
						num += key.Onslot[item2.SlotIdx];
					}
				}
				if (num > 0)
				{
					planeCount += num;
					list.Add(key);
				}
			}
			return list;
		}

		protected virtual AirFireInfo getAntiAirFireAttacker(BattleBaseData baseData)
		{
			List<Mem_ship> shipData = baseData.ShipData;
			List<List<Mst_slotitem>> slotData = baseData.SlotData;
			List<AirFireInfo> list = new List<AirFireInfo>();
			for (int i = 0; i < shipData.Count; i++)
			{
				if (shipData[i].IsFight() && slotData[i].Count != 0)
				{
					AirFireInfo antifireKind = getAntifireKind(shipData[i], slotData[i]);
					if (antifireKind != null)
					{
						list.Add(antifireKind);
					}
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			IOrderedEnumerable<AirFireInfo> source = from x in list
				orderby x.AirFireKind
				select x;
			return source.First();
		}

		private AirFireInfo getAntifireKind(Mem_ship shipData, List<Mst_slotitem> slotData)
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[shipData.Ship_id];
			ILookup<int, Mst_slotitem> lookup = slotData.ToLookup((Mst_slotitem x) => x.Type4);
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(54);
			HashSet<int> hashSet2 = hashSet;
			hashSet = new HashSet<int>();
			hashSet.Add(428);
			HashSet<int> hashSet3 = hashSet;
			AirFireInfo result = null;
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			dictionary.Add(1, 65);
			dictionary.Add(2, 58);
			dictionary.Add(3, 50);
			dictionary.Add(4, 52);
			dictionary.Add(5, 55);
			dictionary.Add(6, 40);
			dictionary.Add(7, 45);
			dictionary.Add(8, 50);
			dictionary.Add(9, 40);
			dictionary.Add(10, 60);
			dictionary.Add(11, 55);
			dictionary.Add(12, 45);
			dictionary.Add(13, 35);
			Dictionary<int, int> dictionary2 = dictionary;
			int num = (int)Utils.GetRandDouble(0.0, 100.0, 1.0, 1);
			List<int> list = new List<int>();
			if (hashSet2.Contains(mst_ship.Ctype))
			{
				if (!lookup.Contains(16))
				{
					return null;
				}
				List<Mst_slotitem> list2 = lookup[16].ToList().FindAll((Mst_slotitem x) => x.Tyku >= 8);
				List<Mst_slotitem> list3 = new List<Mst_slotitem>();
				if (lookup.Contains(11))
				{
					list3 = lookup[11].ToList().FindAll((Mst_slotitem x) => x.Tyku >= 2);
				}
				if (list2.Count >= 2 && list3.Count >= 1 && num < dictionary2[1])
				{
					list.Add(list2[0].Id);
					list.Add(list2[1].Id);
					list.Add(list3[0].Id);
					result = new AirFireInfo(shipData.Rid, 1, list);
				}
				else if (list2.Count >= 1 && list3.Count >= 1 && num < dictionary2[2])
				{
					list.Add(list2[0].Id);
					list.Add(list3[0].Id);
					result = new AirFireInfo(shipData.Rid, 2, list);
				}
				else if (list2.Count >= 2 && num < dictionary2[3])
				{
					list.Add(list2[0].Id);
					list.Add(list2[1].Id);
					result = new AirFireInfo(shipData.Rid, 3, list);
				}
				return result;
			}
			if (hashSet3.Contains(mst_ship.Id) && lookup.Contains(16) && lookup.Contains(15))
			{
				List<Mst_slotitem> list4 = lookup[16].ToList();
				List<Mst_slotitem> list5 = lookup[15].ToList().FindAll((Mst_slotitem x) => x.Tyku >= 9);
				List<Mst_slotitem> list6 = new List<Mst_slotitem>();
				if (lookup.Contains(11))
				{
					list6 = lookup[11].ToList().FindAll((Mst_slotitem x) => x.Tyku >= 2);
				}
				if (list5.Count > 0 && list6.Count > 0 && num < dictionary2[10])
				{
					list.Add(list4[0].Id);
					list.Add(list5[0].Id);
					list.Add(list6[0].Id);
					return new AirFireInfo(shipData.Rid, 10, list);
				}
				if (list5.Count > 0 && num < dictionary2[11])
				{
					list.Add(list4[0].Id);
					list.Add(list5[0].Id);
					return new AirFireInfo(shipData.Rid, 11, list);
				}
			}
			List<Mst_slotitem> list7 = new List<Mst_slotitem>();
			if (lookup.Contains(16))
			{
				list7 = lookup[16].ToList().FindAll((Mst_slotitem x) => x.Tyku >= 8);
			}
			List<Mst_slotitem> list8 = new List<Mst_slotitem>();
			if (lookup.Contains(11))
			{
				list8 = lookup[11].ToList().FindAll((Mst_slotitem x) => x.Tyku >= 2);
			}
			if (lookup.Contains(30) && lookup.Contains(3) && lookup.Contains(12) && list8.Count >= 1 && num < dictionary2[4])
			{
				list.Add(lookup[3].First().Id);
				list.Add(lookup[12].First().Id);
				list.Add(lookup[30].First().Id);
				return new AirFireInfo(shipData.Rid, 4, list);
			}
			if (list7.Count >= 2 && list8.Count >= 1 && num < dictionary2[5])
			{
				list.Add(list7[0].Id);
				list.Add(list7[1].Id);
				list.Add(list8[0].Id);
				return new AirFireInfo(shipData.Rid, 5, list);
			}
			if (lookup.Contains(30) && lookup.Contains(3) && lookup.Contains(12) && num < dictionary2[6])
			{
				list.Add(lookup[3].First().Id);
				list.Add(lookup[12].First().Id);
				list.Add(lookup[30].First().Id);
				return new AirFireInfo(shipData.Rid, 6, list);
			}
			if (lookup.Contains(30) && lookup.Contains(16) && list8.Count >= 1 && num < dictionary2[7])
			{
				Mst_slotitem mst_slotitem = (list7.Count <= 0) ? lookup[16].First() : list7[0];
				list.Add(mst_slotitem.Id);
				list.Add(lookup[30].First().Id);
				list.Add(list8[0].Id);
				return new AirFireInfo(shipData.Rid, 7, list);
			}
			if (list7.Count >= 1 && list8.Count >= 1 && num < dictionary2[8])
			{
				list.Add(list7[0].Id);
				list.Add(list8[0].Id);
				return new AirFireInfo(shipData.Rid, 8, list);
			}
			if (lookup.Contains(30) && lookup.Contains(16) && num < dictionary2[9])
			{
				Mst_slotitem mst_slotitem2 = (list7.Count <= 0) ? lookup[16].First() : list7[0];
				list.Add(mst_slotitem2.Id);
				list.Add(lookup[30].First().Id);
				return new AirFireInfo(shipData.Rid, 9, list);
			}
			List<Mst_slotitem> list9 = new List<Mst_slotitem>();
			new List<Mst_slotitem>();
			List<Mst_slotitem> list10 = new List<Mst_slotitem>();
			if (lookup.Contains(15))
			{
				list9 = lookup[15].ToList().FindAll((Mst_slotitem x) => x.Tyku >= 9);
				List<Mst_slotitem> collection = lookup[15].ToList().FindAll((Mst_slotitem x) => x.Tyku >= 3);
				list10.AddRange(list9);
				list10.AddRange(collection);
			}
			if (list10.Count >= 2 && list8.Count >= 1 && num < dictionary2[12])
			{
				list.Add(list10[0].Id);
				list.Add(list10[1].Id);
				list.Add(list8[0].Id);
				return new AirFireInfo(shipData.Rid, 12, list);
			}
			if (hashSet3.Contains(mst_ship.Id))
			{
				return result;
			}
			if (list7.Count >= 1 && list9.Count >= 1 && list8.Count >= 1 && num < dictionary2[13])
			{
				list.Add(list7[0].Id);
				list.Add(list9[0].Id);
				list.Add(list8[0].Id);
				return new AirFireInfo(shipData.Rid, 13, list);
			}
			return result;
		}

		private double[] getAntifireParam(AirFireInfo info)
		{
			if (info == null)
			{
				return new double[3]
				{
					1.0,
					0.0,
					1.0
				};
			}
			Dictionary<int, double[]> dictionary = new Dictionary<int, double[]>();
			dictionary.Add(1, new double[3]
			{
				3.0,
				5.0,
				1.75
			});
			dictionary.Add(2, new double[3]
			{
				3.0,
				4.0,
				1.7
			});
			dictionary.Add(3, new double[3]
			{
				2.0,
				3.0,
				1.6
			});
			dictionary.Add(4, new double[3]
			{
				5.0,
				2.0,
				1.5
			});
			dictionary.Add(5, new double[3]
			{
				2.0,
				3.0,
				1.55
			});
			dictionary.Add(6, new double[3]
			{
				4.0,
				1.0,
				1.5
			});
			dictionary.Add(7, new double[3]
			{
				2.0,
				2.0,
				1.35
			});
			dictionary.Add(8, new double[3]
			{
				2.0,
				3.0,
				1.45
			});
			dictionary.Add(9, new double[3]
			{
				1.0,
				2.0,
				1.3
			});
			dictionary.Add(10, new double[3]
			{
				3.0,
				6.0,
				1.65
			});
			dictionary.Add(11, new double[3]
			{
				2.0,
				5.0,
				1.5
			});
			dictionary.Add(12, new double[3]
			{
				1.0,
				3.0,
				1.25
			});
			dictionary.Add(13, new double[3]
			{
				1.0,
				4.0,
				1.35
			});
			Dictionary<int, double[]> dictionary2 = dictionary;
			return dictionary2[info.AirFireKind];
		}

		protected virtual int getDeckTaikuPow(BattleBaseData battleBase)
		{
			int num = 0;
			Dictionary<int, double> dictionary = new Dictionary<int, double>();
			dictionary.Add(16, 0.35);
			dictionary.Add(30, 0.35);
			dictionary.Add(12, 0.6);
			dictionary.Add(11, 0.4);
			Dictionary<int, double> dictionary2 = dictionary;
			List<List<Mst_slotitem>> slotData = battleBase.SlotData;
			double num2 = 0.0;
			foreach (var item in slotData.Select((List<Mst_slotitem> items, int idx) => new
			{
				items,
				idx
			}))
			{
				double num3 = 0.0;
				Mem_ship mem_ship = battleBase.ShipData[item.idx];
				if (mem_ship.IsFight())
				{
					List<int> list = battleBase.SlotLevel[item.idx];
					foreach (var item2 in item.items.Select((Mst_slotitem item, int idx) => new
					{
						item,
						idx
					}))
					{
						if (item2.item.Tyku > 0)
						{
							double value = 0.0;
							if (item2.item.Id == 9)
							{
								value = 0.25;
							}
							else if (!dictionary2.TryGetValue(item2.item.Type4, out value))
							{
								value = 0.2;
							}
							num3 += (double)item2.item.Tyku * value;
							int num4 = list[item2.idx];
							if (item2.item.Type4 == 16 || item2.item.Type4 == 30)
							{
								double a2Plus = getA2Plus(1, item2.item.Tyku, num4);
								num2 += a2Plus;
							}
							else if (num4 > 0 && (item2.item.Api_mapbattle_type3 == 12 || item2.item.Api_mapbattle_type3 == 13) && item2.item.Tyku > 1)
							{
								double a2Plus2 = getA2Plus(2, item2.item.Tyku, num4);
								num2 += a2Plus2;
							}
						}
					}
					num += (int)(num3 + num2);
				}
			}
			double formationParamF = formationData.GetFormationParamF3(FormationDatas.GetFormationKinds.AIR, battleBase.Formation);
			return (int)((double)num * formationParamF);
		}

		protected virtual int battleTaiku(List<Mem_ship> taikuHaveShip, int deckTyku, AirFireInfo antifire)
		{
			IEnumerable<KeyValuePair<Mem_ship, List<FighterInfo>>> enumerable;
			Dictionary<int, int[]> slotExperience;
			List<Mem_ship> shipData;
			List<List<Mst_slotitem>> slotData;
			List<List<int>> slotLevel;
			double num;
			if (taikuHaveShip[0].IsEnemy())
			{
				enumerable = from item in e_FighterInfo
					where taikuHaveShip.Contains(item.Key)
					select item;
				slotExperience = E_Data.SlotExperience;
				shipData = F_Data.ShipData;
				slotData = F_Data.SlotData;
				slotLevel = F_Data.SlotLevel;
				num = 0.75;
			}
			else
			{
				enumerable = from item in f_FighterInfo
					where taikuHaveShip.Contains(item.Key)
					select item;
				slotExperience = F_Data.SlotExperience;
				shipData = E_Data.ShipData;
				slotData = E_Data.SlotData;
				slotLevel = E_Data.SlotLevel;
				num = 0.8;
			}
			int num2 = 0;
			IEnumerable<Mem_ship> enumerable2 = from x in shipData
				where x.IsFight()
				select x;
			if (enumerable2 == null || enumerable2.Count() == 0)
			{
				return num2;
			}
			foreach (KeyValuePair<Mem_ship, List<FighterInfo>> item in enumerable)
			{
				Mem_ship key = item.Key;
				foreach (FighterInfo item2 in item.Value)
				{
					if (key.Onslot[item2.SlotIdx] > 0 && item2.ValidTaiku())
					{
						Mem_ship taikuShip = (from x in enumerable2
							orderby Guid.NewGuid()
							select x).First();
						int index = shipData.FindIndex((Mem_ship x) => x.Rid == taikuShip.Rid);
						double shipTaikuPow = getShipTaikuPow(taikuShip, slotData[index], slotLevel[index]);
						double num3 = valance1;
						double num4 = (shipTaikuPow + (double)deckTyku) * num3 * num;
						double[] antifireParam = getAntifireParam(antifire);
						if (antifire == null && taikuShip.IsEnemy())
						{
							antifireParam[0] = 0.0;
						}
						int num5 = (int)(num4 * (double)randInstance.Next(2) * antifireParam[2] + antifireParam[0]);
						int num6 = (int)((double)key.Onslot[item2.SlotIdx] * 0.02 * shipTaikuPow * num3 * (double)randInstance.Next(2) + antifireParam[1]);
						int num7 = num5 + num6;
						if (num7 > key.Onslot[item2.SlotIdx])
						{
							num7 = key.Onslot[item2.SlotIdx];
						}
						int[] value = null;
						if (slotExperience.TryGetValue(key.Slot[item2.SlotIdx], out value))
						{
							int slotExpSubValueToTaiku = getSlotExpSubValueToTaiku(key.Onslot[item2.SlotIdx], num7, value[0]);
							value[1] += slotExpSubValueToTaiku;
						}
						List<int> onslot;
						List<int> list = onslot = key.Onslot;
						int slotIdx;
						int index2 = slotIdx = item2.SlotIdx;
						slotIdx = onslot[slotIdx];
						list[index2] = slotIdx - num7;
						num2 += num7;
					}
				}
			}
			return num2;
		}

		private double getShipTaikuPow(Mem_ship shipData, List<Mst_slotitem> mst_slotData, List<int> slotLevels)
		{
			double num;
			if (shipData.IsEnemy())
			{
				num = Math.Sqrt(shipData.Taiku);
			}
			else
			{
				Ship_GrowValues battleBaseParam = shipData.GetBattleBaseParam();
				num = (double)battleBaseParam.Taiku * 0.5;
			}
			double num2 = 0.0;
			double num3 = 0.0;
			for (int i = 0; i < mst_slotData.Count; i++)
			{
				Mst_slotitem mst_slotitem = mst_slotData[i];
				if (mst_slotitem.Tyku > 0)
				{
					int slotLevel = slotLevels[i];
					if (mst_slotitem.Type4 == 16 || mst_slotitem.Type4 == 30)
					{
						double a1Plus = getA1Plus(1, mst_slotitem.Tyku, slotLevel);
						num3 += a1Plus;
						num2 += (double)mst_slotitem.Tyku * 2.0;
					}
					else if (mst_slotitem.Type4 == 15)
					{
						double a1Plus2 = getA1Plus(2, mst_slotitem.Tyku, slotLevel);
						num3 += a1Plus2;
						num2 += (double)mst_slotitem.Tyku * 3.0;
					}
					else if (mst_slotitem.Type4 == 11)
					{
						num2 += (double)mst_slotitem.Tyku * 1.5;
					}
				}
			}
			return num + num2 + num3;
		}

		protected virtual int getSlotExpSubValueToTaiku(int onSlot, int lostNum, int slotExp)
		{
			return getSlotExpSubValueToSeiku(onSlot, lostNum, slotExp);
		}

		protected virtual double getA1Plus(int type, int tyku, int slotLevel)
		{
			double num = (tyku <= 7) ? 2.0 : 3.0;
			switch (type)
			{
			case 1:
				return num * Math.Sqrt(slotLevel) * 0.5;
			case 2:
				return num * Math.Sqrt(slotLevel);
			default:
				return 0.0;
			}
		}

		protected virtual double getA2Plus(int type, int tyku, int slotLevel)
		{
			switch (type)
			{
			case 1:
			{
				double num = (tyku <= 7) ? 2.0 : 3.0;
				return num * Math.Sqrt(slotLevel);
			}
			case 2:
				return Math.Sqrt(slotLevel) * 1.5;
			default:
				return 0.0;
			}
		}

		protected virtual AirBattle3 getAirBattle3(AirBattle2 air2)
		{
			if (air2 == null)
			{
				return null;
			}
			int num = air2.F_LostInfo.Count - air2.F_LostInfo.LostCount;
			int num2 = air2.E_LostInfo.Count - air2.E_LostInfo.LostCount;
			if (num <= 0 && num2 <= 0)
			{
				return null;
			}
			AirBattle3 airBattle = new AirBattle3();
			if (isSearchSuccess[0] && num > 0)
			{
				setBakuraiPlane(f_FighterInfo, airBattle.F_BakugekiPlane, airBattle.F_RaigekiPlane);
				battleBakurai(F_Data, E_Data, f_FighterInfo, ref airBattle.E_Bakurai);
			}
			if (isSearchSuccess[1] && num2 > 0)
			{
				setBakuraiPlane(e_FighterInfo, airBattle.E_BakugekiPlane, airBattle.E_RaigekiPlane);
				battleBakurai(E_Data, F_Data, e_FighterInfo, ref airBattle.F_Bakurai);
				{
					foreach (var item in F_Data.ShipData.Select((Mem_ship obj, int idx) => new
					{
						obj,
						idx
					}))
					{
						if (item.obj.Nowhp <= 0)
						{
							RecoveryShip(item.idx);
						}
					}
					return airBattle;
				}
			}
			return airBattle;
		}

		protected virtual void setBakuraiPlane(Dictionary<Mem_ship, List<FighterInfo>> fighter, List<int> bakuPlane, List<int> raigPlane)
		{
			foreach (KeyValuePair<Mem_ship, List<FighterInfo>> item in fighter)
			{
				Mem_ship key = item.Key;
				foreach (FighterInfo item2 in item.Value)
				{
					if (key.Onslot[item2.SlotIdx] > 0 && item2.ValidBakurai())
					{
						if (item2.Kind == FighterInfo.FighterKinds.BAKU)
						{
							bakuPlane.Add(item2.SlotData.Id);
						}
						else if (item2.Kind == FighterInfo.FighterKinds.RAIG)
						{
							raigPlane.Add(item2.SlotData.Id);
						}
					}
				}
			}
		}

		protected virtual void battleBakurai(BattleBaseData attacker, BattleBaseData target, Dictionary<Mem_ship, List<FighterInfo>> fighter, ref BakuRaiInfo bakurai)
		{
			Mst_slotitem slotitem = (!attacker.ShipData[0].IsEnemy()) ? fTouchPlane : eTouchPlane;
			List<Mem_ship> list = target.ShipData.ToList();
			list.RemoveAll((Mem_ship x) => (x.Nowhp <= 0) ? true : false);
			foreach (KeyValuePair<Mem_ship, List<FighterInfo>> item in fighter)
			{
				Mem_ship key = item.Key;
				foreach (FighterInfo item2 in item.Value)
				{
					if (key.Onslot[item2.SlotIdx] > 0 && item2.ValidBakurai())
					{
						BattleDamageKinds dKind = BattleDamageKinds.Normal;
						Mem_ship atackTarget = getAtackTarget(key, list, overKill: true, subMarineFlag: false, rescueFlag: true, ref dKind);
						if (atackTarget != null)
						{
							int num = target.ShipData.IndexOf(atackTarget);
							int bakuraiAtkPow = getBakuraiAtkPow(item2, key.Onslot[item2.SlotIdx], atackTarget);
							bakuraiAtkPow = (int)((double)bakuraiAtkPow * getTouchPlaneKeisu(slotitem));
							int soukou = atackTarget.Soukou;
							int hitPorb = getHitPorb();
							int battleAvo = getBattleAvo(FormationDatas.GetFormationKinds.AIR, atackTarget);
							BattleHitStatus hitStatus = getHitStatus(hitPorb, battleAvo, key, atackTarget, 0.2, airAttack: true);
							int num2 = setDamageValue(hitStatus, bakuraiAtkPow, soukou, key, atackTarget, target.LostFlag);
							bakurai.Damage[num] += num2;
							BattleDamageKinds battleDamageKinds = dKind;
							if (battleDamageKinds == BattleDamageKinds.Rescue && bakurai.DamageType[num] != BattleDamageKinds.Rescue)
							{
								bakurai.DamageType[num] = BattleDamageKinds.Rescue;
							}
							if (bakurai.Clitical[num] != BattleHitStatus.Clitical && hitStatus == BattleHitStatus.Clitical)
							{
								bakurai.Clitical[num] = hitStatus;
							}
							if (item2.Kind == FighterInfo.FighterKinds.BAKU)
							{
								bakurai.IsBakugeki[num] = true;
							}
							else if (item2.Kind == FighterInfo.FighterKinds.RAIG)
							{
								bakurai.IsRaigeki[num] = true;
							}
						}
					}
				}
			}
		}

		protected virtual int getHitPorb()
		{
			return (int)valance2;
		}

		protected virtual int getBakuraiAtkPow(FighterInfo fighter, int fighterNum, Mem_ship target)
		{
			if (!fighter.ValidBakurai())
			{
				return 0;
			}
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[target.Ship_id];
			Mst_stype mst_stype = Mst_DataManager.Instance.Mst_stype[target.Stype];
			int num = fighter.AttackShipPow;
			if (fighter.Kind == FighterInfo.FighterKinds.RAIG && mst_stype.IsLandFacillity(mst_ship.Soku))
			{
				num = 0;
			}
			int num2 = 150;
			double num3 = valance3 + (double)num * Math.Sqrt(fighterNum);
			if (fighter.Kind == FighterInfo.FighterKinds.RAIG)
			{
				num3 *= 0.8 + (double)randInstance.Next(2) * 0.7;
			}
			if (num3 > (double)num2)
			{
				num3 = (double)num2 + Math.Sqrt(num3 - (double)num2);
			}
			return (int)num3;
		}

		protected virtual double getTouchPlaneKeisu(Mst_slotitem slotitem)
		{
			if (slotitem == null)
			{
				return 1.0;
			}
			if (slotitem.Houm >= 3)
			{
				return 1.2;
			}
			if (slotitem.Houm == 2)
			{
				return 1.17;
			}
			return 1.12;
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}
	}
}
