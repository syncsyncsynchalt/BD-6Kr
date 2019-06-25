using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public abstract class BattleLogicBase<T> : IDisposable
	{
		protected bool practiceFlag;

		protected Random randInstance;

		protected FormationDatas formationData;

		protected BattleCommandParams commandParams;

		protected BattleTargetKind battleTargetKind;

		protected readonly double valanceSubmarine1;

		protected readonly double valanceSubmarine2;

		protected readonly double valanceSubmarine3;

		public abstract BattleBaseData F_Data
		{
			get;
		}

		public abstract BattleBaseData E_Data
		{
			get;
		}

		public abstract Dictionary<int, BattleShipSubInfo> F_SubInfo
		{
			get;
		}

		public abstract Dictionary<int, BattleShipSubInfo> E_SubInfo
		{
			get;
		}

		public BattleLogicBase()
		{
			randInstance = new Random();
			practiceFlag = false;
			valanceSubmarine1 = 3.0;
			valanceSubmarine2 = 80.0;
			valanceSubmarine3 = 1.1;
			battleTargetKind = BattleTargetKind.Other;
		}

		public abstract T GetResultData(FormationDatas formation, BattleCommandParams cParam);

		public abstract void Dispose();

		public bool IsAtapSlotItem(int type3No)
		{
			return (type3No == 37) ? true : false;
		}

		protected virtual void setTargetingKind(FormationDatas formation)
		{
			if ((formation.F_Formation == BattleFormationKinds1.TanJuu || formation.F_Formation == BattleFormationKinds1.FukuJuu) && (formation.E_Formation == BattleFormationKinds1.TanJuu || formation.E_Formation == BattleFormationKinds1.FukuJuu))
			{
				if (formation.BattleFormation == BattleFormationKinds2.Doukou)
				{
					battleTargetKind = BattleTargetKind.Case1;
				}
				else if (formation.BattleFormation == BattleFormationKinds2.Hankou)
				{
					battleTargetKind = BattleTargetKind.Case2;
				}
				else if (formation.BattleFormation == BattleFormationKinds2.T_Own)
				{
					battleTargetKind = BattleTargetKind.Case3;
				}
				else if (formation.BattleFormation == BattleFormationKinds2.T_Enemy)
				{
					battleTargetKind = BattleTargetKind.Case4;
				}
			}
		}

		protected virtual bool isAttackerFromTargetKind(BattleShipSubInfo subInfo)
		{
			return true;
		}

		protected bool RecoveryShip(int idx)
		{
			if (practiceFlag)
			{
				return false;
			}
			if (!F_Data.LostFlag[idx])
			{
				return false;
			}
			if (F_Data.ShipData[idx].Nowhp > 0 || F_Data.ShipData[idx].Escape_sts)
			{
				return false;
			}
			int[] array = F_Data.ShipData[idx].FindRecoveryItem();
			if (!F_Data.ShipData[idx].UseRecoveryItem(array, (idx == 0) ? true : false))
			{
				return false;
			}
			F_Data.LostFlag[idx] = false;
			if (array[0] != int.MaxValue)
			{
				F_Data.SlotData[idx].RemoveAt(array[0]);
			}
			return true;
		}

		protected Mem_ship getAtackTarget(Mem_ship attacker, List<Mem_ship> targetShips, bool overKill, bool subMarineFlag, bool rescueFlag, ref BattleDamageKinds dKind)
		{
			Dictionary<int, Mst_stype> stypes = Mst_DataManager.Instance.Mst_stype;
			IEnumerable<Mem_ship> enumerable = overKill ? (from target in targetShips
				let submarineCheck = subMarineFlag == stypes[target.Stype].IsSubmarine()
				where !target.Escape_sts && submarineCheck
				select target) : (from target in targetShips
				let submarineCheck = subMarineFlag == stypes[target.Stype].IsSubmarine()
				where target.IsFight() && submarineCheck
				select target);
			dKind = BattleDamageKinds.Normal;
			if (enumerable == null || enumerable.Count() == 0)
			{
				return null;
			}
			if (enumerable.Count() == 1)
			{
				return enumerable.First();
			}
			List<Mem_ship> list = (!attacker.IsEnemy()) ? targetFillter(F_SubInfo[attacker.Rid].DeckIdx, enumerable, E_SubInfo) : targetFillter(E_SubInfo[attacker.Rid].DeckIdx, enumerable, F_SubInfo);
			Mem_ship mem_ship = (list.Count != 0) ? (from x in list
				orderby Guid.NewGuid()
				select x).First() : (from x in enumerable.ToArray()
				orderby Guid.NewGuid()
				select x).First();
			if (enumerable.Count() <= 1)
			{
				return mem_ship;
			}
			Mem_ship flagShip = (!attacker.IsEnemy()) ? E_Data.ShipData[0] : F_Data.ShipData[0];
			if (!rescueFlag || mem_ship.Rid != flagShip.Rid)
			{
				return mem_ship;
			}
			Dictionary<int, Mst_ship> mShipDict = Mst_DataManager.Instance.Mst_ship;
			Dictionary<int, Mst_stype> mStypeDict = Mst_DataManager.Instance.Mst_stype;
			if (mStypeDict[mem_ship.Stype].IsLandFacillity(mShipDict[mem_ship.Ship_id].Soku))
			{
				return mem_ship;
			}
			IEnumerable<Mem_ship> source = from re_target in enumerable
				let mShipObj = mShipDict[re_target.Ship_id]
				let mStypeObj = mStypeDict[re_target.Stype]
				where re_target.Get_DamageState() == DamageState.Normal
				where !mStypeObj.IsLandFacillity(mShipObj.Soku)
				where re_target.Rid != flagShip.Rid
				select re_target;
			if (!source.Any())
			{
				return mem_ship;
			}
			BattleFormationKinds1 battleFormationKinds = (!mem_ship.IsEnemy()) ? F_Data.Formation : E_Data.Formation;
			int num = 60;
			switch (battleFormationKinds)
			{
			case BattleFormationKinds1.TanJuu:
				num = 45;
				break;
			case BattleFormationKinds1.Rinkei:
				num = 75;
				break;
			}
			if (num > randInstance.Next(100))
			{
				return mem_ship;
			}
			dKind = BattleDamageKinds.Rescue;
			return (from x in source.ToArray()
				orderby Guid.NewGuid()
				select x).First();
		}

		private List<Mem_ship> targetFillter(int attackerIdx, IEnumerable<Mem_ship> targetShips, Dictionary<int, BattleShipSubInfo> targetSubInfo)
		{
			List<Mem_ship> list = targetShips.ToList();
			if (battleTargetKind == BattleTargetKind.Other)
			{
				return list;
			}
			HashSet<int> enableIdx = getTargetFillterEnableList(attackerIdx, !list[0].IsEnemy());
			list.RemoveAll((Mem_ship x) => !enableIdx.Contains(targetSubInfo[x.Rid].DeckIdx));
			return list;
		}

		private HashSet<int> getTargetFillterEnableList(int attacker_idx, bool enemyFlag)
		{
			if (battleTargetKind == BattleTargetKind.Case1)
			{
				switch (attacker_idx)
				{
				case 0:
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(0);
					hashSet.Add(1);
					return hashSet;
				}
				case 1:
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(0);
					hashSet.Add(1);
					hashSet.Add(2);
					return hashSet;
				}
				case 2:
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(1);
					hashSet.Add(2);
					hashSet.Add(3);
					return hashSet;
				}
				case 3:
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(2);
					hashSet.Add(3);
					hashSet.Add(4);
					return hashSet;
				}
				case 4:
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(3);
					hashSet.Add(4);
					hashSet.Add(5);
					return hashSet;
				}
				case 5:
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(4);
					hashSet.Add(5);
					return hashSet;
				}
				}
			}
			if (battleTargetKind == BattleTargetKind.Case2)
			{
				switch (attacker_idx)
				{
				case 0:
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(0);
					hashSet.Add(1);
					return hashSet;
				}
				case 1:
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(0);
					hashSet.Add(1);
					hashSet.Add(2);
					return hashSet;
				}
				case 2:
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(0);
					hashSet.Add(1);
					hashSet.Add(2);
					return hashSet;
				}
				default:
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(0);
					hashSet.Add(1);
					hashSet.Add(2);
					hashSet.Add(3);
					hashSet.Add(4);
					hashSet.Add(5);
					return hashSet;
				}
				}
			}
			if (battleTargetKind == BattleTargetKind.Case3 || battleTargetKind == BattleTargetKind.Case4)
			{
				if (enemyFlag && battleTargetKind == BattleTargetKind.Case3)
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(0);
					hashSet.Add(1);
					hashSet.Add(2);
					hashSet.Add(3);
					hashSet.Add(4);
					hashSet.Add(5);
					return hashSet;
				}
				if (!enemyFlag && battleTargetKind == BattleTargetKind.Case4)
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(0);
					hashSet.Add(1);
					hashSet.Add(2);
					hashSet.Add(3);
					hashSet.Add(4);
					hashSet.Add(5);
					return hashSet;
				}
				switch (attacker_idx)
				{
				case 0:
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(0);
					return hashSet;
				}
				case 1:
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(0);
					hashSet.Add(1);
					return hashSet;
				}
				case 2:
				case 3:
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(0);
					hashSet.Add(1);
					hashSet.Add(2);
					return hashSet;
				}
				case 4:
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(1);
					hashSet.Add(2);
					hashSet.Add(3);
					return hashSet;
				}
				case 5:
				{
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(2);
					hashSet.Add(3);
					hashSet.Add(4);
					return hashSet;
				}
				}
			}
			return new HashSet<int>();
		}

		protected int getBattleAvo(FormationDatas.GetFormationKinds battleState, Mem_ship targetShip)
		{
			double num = (double)targetShip.Kaihi + Math.Sqrt(targetShip.GetBattleBaseParam().Luck * 2);
			BattleFormationKinds1 formation;
			if (!targetShip.IsEnemy())
			{
				formation = F_Data.Formation;
				BattleFormationKinds2 battleFormation = F_Data.BattleFormation;
				BattleFormationKinds1 formation2 = E_Data.Formation;
			}
			else
			{
				formation = E_Data.Formation;
				BattleFormationKinds2 battleFormation2 = E_Data.BattleFormation;
				BattleFormationKinds1 formation3 = F_Data.Formation;
			}
			num *= formationData.GetFormationParamF3(battleState, formation);
			double num2 = (int)num;
			if (num2 >= 65.0)
			{
				double num3 = 55.0 + Math.Sqrt(num2 - 65.0) * 2.0;
				num2 = (int)num3;
			}
			else if (num2 >= 40.0)
			{
				double num4 = 40.0 + Math.Sqrt(num2 - 40.0) * 3.0;
				num2 = (int)num4;
			}
			num2 += getAvoHosei(targetShip);
			if (!targetShip.IsEnemy() && commandParams != null)
			{
				double num5 = (double)commandParams.Rspp / 100.0;
				double num6 = num2 * num5;
				num2 += num6;
			}
			int num7 = 100;
			double num8 = Mst_DataManager.Instance.Mst_ship[targetShip.Ship_id].Fuel_max;
			if (num8 != 0.0)
			{
				num7 = (int)((double)targetShip.Fuel / num8 * 100.0);
			}
			if (num7 < 75)
			{
				num2 -= (double)(75 - num7);
			}
			return (int)num2;
		}

		protected int getBattleAvo_Midnight(FormationDatas.GetFormationKinds battleState, Mem_ship targetShip, bool haveSearchLight)
		{
			int battleAvo = getBattleAvo(battleState, targetShip);
			double num = (targetShip.Stype != 5 && targetShip.Stype != 6) ? ((double)battleAvo) : ((double)battleAvo + 5.0);
			if (haveSearchLight)
			{
				num *= 0.2;
			}
			return (int)num;
		}

		protected abstract double getAvoHosei(Mem_ship target);

		protected virtual BattleHitStatus getHitStatus(int hitProb, int avoProb, Mem_ship attackShip, Mem_ship targetShip, double cliticalKeisu, bool airAttack)
		{
			double num = hitProb - avoProb;
			FatigueState fatigueState = targetShip.Get_FatigueState();
			if (num <= 10.0)
			{
				num = 10.0;
			}
			double num2 = 1.0;
			switch (fatigueState)
			{
			case FatigueState.Exaltation:
				num2 = 0.7;
				break;
			case FatigueState.Normal:
				num2 = 1.0;
				break;
			case FatigueState.Light:
				num2 = 1.2;
				break;
			case FatigueState.Distress:
				num2 = 1.4;
				break;
			}
			num *= num2;
			if (num >= 96.0)
			{
				num = 96.0;
			}
			double cht = 0.0;
			double cht2 = 1.0;
			if (airAttack)
			{
				setCliticalAlv(ref num, ref cht, ref cht2, attackShip);
			}
			int num3 = randInstance.Next(100);
			double num4 = Math.Sqrt(num) * cliticalKeisu;
			if ((double)num3 <= num4)
			{
				return BattleHitStatus.Clitical;
			}
			if ((double)num3 > num)
			{
				return BattleHitStatus.Miss;
			}
			return BattleHitStatus.Normal;
		}

		protected virtual double getLandFacciilityKeisu(List<Mst_slotitem> slot_item)
		{
			if (slot_item.Contains(Mst_DataManager.Instance.Mst_Slotitem[35]))
			{
				return 2.5;
			}
			return 1.0;
		}

		protected void setCliticalAlv(ref double check_value, ref double cht1, ref double cht2, Mem_ship attacker)
		{
			List<Mst_slotitem> list;
			Dictionary<int, int[]> slotExperience;
			if (attacker.IsEnemy())
			{
				list = E_Data.SlotData[E_SubInfo[attacker.Rid].DeckIdx];
				slotExperience = E_Data.SlotExperience;
			}
			else
			{
				list = F_Data.SlotData[F_SubInfo[attacker.Rid].DeckIdx];
				slotExperience = F_Data.SlotExperience;
			}
			int num = 0;
			int num2 = 0;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(8);
			hashSet.Add(11);
			hashSet.Add(41);
			HashSet<int> hashSet2 = hashSet;
			for (int i = 0; i < attacker.Slot.Count; i++)
			{
				int num3 = attacker.Slot[i];
				if (num3 <= 0 || attacker.Onslot[i] <= 0 || !hashSet2.Contains(list[i].Api_mapbattle_type3))
				{
					continue;
				}
				int[] value = null;
				if (slotExperience.TryGetValue(num3, out value))
				{
					if (value[0] >= 10)
					{
					}
					if (value[0] >= 25)
					{
					}
					if (value[0] >= 40)
					{
					}
					if (value[0] >= 55)
					{
					}
					if (value[0] >= 70)
					{
					}
					double num4 = (value[0] < 80) ? 10.0 : 7.0;
					if (i == 0)
					{
						cht1 += num4 * 0.8;
						double num5 = (Math.Sqrt(value[0]) + num4) / 100.0;
						cht2 += (int)num5;
					}
					else
					{
						cht1 += num4 * 0.8;
						double num6 = (Math.Sqrt(value[0]) + num4) / 200.0;
						cht2 += (int)num6;
					}
					num += value[0];
					num2++;
				}
			}
			cht1 = (int)cht1;
			double num7 = 0.0;
			if (num2 > 0)
			{
				num7 = (double)num / (double)num2;
			}
			if (num7 >= 10.0)
			{
				check_value += (int)Math.Sqrt(num7 * 0.1);
			}
			if (num7 >= 25.0)
			{
				check_value += 1.0;
			}
			if (num7 >= 40.0)
			{
				check_value += 1.0;
			}
			if (num7 >= 55.0)
			{
				check_value += 1.0;
			}
			if (num7 >= 70.0)
			{
				check_value += 1.0;
			}
			if (num7 >= 80.0)
			{
				check_value += 2.0;
			}
			if (num7 >= 100.0)
			{
				check_value += 3.0;
			}
		}

		protected virtual int setDamageValue(BattleHitStatus hitType, int atkPow, int defPow, Mem_ship attacker, Mem_ship target, List<bool> lostFlag)
		{
			switch (hitType)
			{
			case BattleHitStatus.Miss:
				return 0;
			case BattleHitStatus.Clitical:
				atkPow = (int)((double)atkPow * 1.5);
				break;
			}
			double num = (double)atkPow - ((double)defPow * 0.7 + (double)randInstance.Next(defPow) * 0.6);
			double num2 = 100.0;
			double num3 = Mst_DataManager.Instance.Mst_ship[attacker.Ship_id].Bull_max;
			if (num3 != 0.0)
			{
				num2 = (double)attacker.Bull / num3 * 100.0;
			}
			if (num2 < 50.0)
			{
				num = Math.Floor(num * num2 / 50.0);
			}
			int num4 = (int)num;
			BattleShipSubInfo battleShipSubInfo;
			int deckIdx;
			if (attacker.IsEnemy())
			{
				battleShipSubInfo = E_SubInfo[attacker.Rid];
				int deckIdx2 = battleShipSubInfo.DeckIdx;
				deckIdx = F_SubInfo[target.Rid].DeckIdx;
			}
			else
			{
				battleShipSubInfo = F_SubInfo[attacker.Rid];
				int deckIdx3 = battleShipSubInfo.DeckIdx;
				deckIdx = E_SubInfo[target.Rid].DeckIdx;
			}
			if (num4 < 1)
			{
				int num5 = randInstance.Next(target.Nowhp);
				num4 = (int)((double)target.Nowhp * 0.06 + (double)num5 * 0.08);
			}
			if (num4 >= target.Nowhp && !target.IsEnemy() && deckIdx == 0)
			{
				num4 = (int)((double)target.Nowhp * 0.5 + (double)randInstance.Next(target.Nowhp) * 0.3);
			}
			if (lostFlag != null && num4 >= target.Nowhp && !lostFlag[deckIdx])
			{
				num4 = target.Nowhp - 1;
			}
			double lovHoseiDamageKeisu = getLovHoseiDamageKeisu(target, num4);
			num4 = (int)((double)num4 * lovHoseiDamageKeisu);
			battleShipSubInfo.TotalDamage += num4;
			target.SubHp(num4);
			return num4;
		}

		private double getLovHoseiDamageKeisu(Mem_ship targetShip, int damage)
		{
			if (targetShip.IsEnemy())
			{
				return 1.0;
			}
			double randDouble = Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
			DamageState damageState = targetShip.Get_DamageState();
			DamageState damageState2 = Mem_ship.Get_DamageState(targetShip.Nowhp - damage, targetShip.Maxhp);
			if (targetShip.Lov >= 330)
			{
				if (randDouble <= 70.0)
				{
					if (damageState == DamageState.Normal && damageState2 == DamageState.Tyuuha)
					{
						return 0.5;
					}
					if (damageState == DamageState.Normal && damageState2 == DamageState.Taiha)
					{
						return 0.5;
					}
				}
				return 1.0;
			}
			if (targetShip.Lov >= 200)
			{
				if (randDouble <= 60.0 && damageState == DamageState.Normal && damageState2 == DamageState.Taiha)
				{
					return 0.55;
				}
				return 1.0;
			}
			if (targetShip.Lov >= 100)
			{
				if (randDouble <= 50.0 && damageState == DamageState.Normal && damageState2 == DamageState.Taiha)
				{
					return 0.6;
				}
				return 1.0;
			}
			return 1.0;
		}

		protected virtual KeyValuePair<int, int> getSubMarineAtackKeisu(List<Mem_ship> targetShips, Mem_ship attacker, List<Mst_slotitem> attacker_items, bool midnight)
		{
			if (!targetShips.Any((Mem_ship x) => x.IsFight() && Mst_DataManager.Instance.Mst_stype[x.Stype].IsSubmarine()))
			{
				return new KeyValuePair<int, int>(0, 0);
			}
			if (!practiceFlag && attacker.IsEnemy() && attacker.Stype == 7)
			{
				string yomi = Mst_DataManager.Instance.Mst_ship[attacker.Ship_id].Yomi;
				if (!yomi.Equals("flagship"))
				{
					return new KeyValuePair<int, int>(0, 0);
				}
			}
			if (!midnight && attacker.Ship_id == 352)
			{
				DamageState damageState = attacker.Get_DamageState();
				if (isHaveSubmarineAirPlane(attacker_items, attacker.Onslot) && damageState <= DamageState.Tyuuha)
				{
					return new KeyValuePair<int, int>(2, 5);
				}
			}
			if (attacker.GetBattleBaseParam().Taisen > 0)
			{
				return new KeyValuePair<int, int>(1, 10);
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(6);
			hashSet.Add(7);
			hashSet.Add(10);
			hashSet.Add(16);
			hashSet.Add(17);
			HashSet<int> hashSet2 = hashSet;
			if (!midnight && hashSet2.Contains(attacker.Stype) && attacker_items.Count > 0 && isHaveSubmarineAirPlane(attacker_items, attacker.Onslot))
			{
				return new KeyValuePair<int, int>(2, 5);
			}
			return new KeyValuePair<int, int>(0, 0);
		}

		private bool isHaveSubmarineAirPlane(List<Mst_slotitem> slotItems, List<int> onSlot)
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(8);
			hashSet.Add(11);
			hashSet.Add(25);
			hashSet.Add(26);
			hashSet.Add(41);
			HashSet<int> hashSet2 = hashSet;
			foreach (var item in slotItems.Select((Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				if (item.obj.Tais > 0 && onSlot[item.idx] > 0 && hashSet2.Contains(item.obj.Api_mapbattle_type3))
				{
					return true;
				}
			}
			return false;
		}

		protected virtual int getSubmarineAttackValue(KeyValuePair<int, int> submarineKeisu, Mem_ship attacker, List<Mst_slotitem> attackerSlot, List<int> slotLevels)
		{
			if (submarineKeisu.Key == 0)
			{
				return 0;
			}
			BattleFormationKinds1 formation;
			BattleFormationKinds2 battleFormation;
			if (!attacker.IsEnemy())
			{
				formation = F_Data.Formation;
				battleFormation = F_Data.BattleFormation;
				BattleFormationKinds1 formation2 = E_Data.Formation;
			}
			else
			{
				formation = E_Data.Formation;
				battleFormation = E_Data.BattleFormation;
				BattleFormationKinds1 formation3 = F_Data.Formation;
			}
			int value = submarineKeisu.Value;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(8);
			hashSet.Add(11);
			hashSet.Add(14);
			hashSet.Add(15);
			hashSet.Add(25);
			hashSet.Add(26);
			hashSet.Add(40);
			HashSet<int> hashSet2 = hashSet;
			HashSet<int> hashSet3 = new HashSet<int>();
			int num = 0;
			double num2 = 0.0;
			foreach (var item in attackerSlot.Select((Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				if (hashSet2.Contains(item.obj.Api_mapbattle_type3))
				{
					num += item.obj.Tais;
					hashSet3.Add(item.obj.Api_mapbattle_type3);
				}
				if ((item.obj.Api_mapbattle_type3 == 14 || item.obj.Api_mapbattle_type3 == 15 || item.obj.Api_mapbattle_type3 == 40) && slotLevels[item.idx] > 0)
				{
					num2 += Math.Sqrt(slotLevels[item.idx]);
				}
			}
			double num3 = Math.Sqrt(attacker.GetBattleBaseParam().Taisen * 2);
			double num4 = (double)num * 1.5;
			double num5 = valanceSubmarine1 + num3 + num4 + num2;
			double formationParamF = formationData.GetFormationParamF1(FormationDatas.GetFormationKinds.SUBMARINE, formation);
			double formationParamBattle = formationData.GetFormationParamBattle(FormationDatas.GetFormationKinds.SUBMARINE, battleFormation);
			double num6 = 1.0;
			if (hashSet3.Contains(15) && (hashSet3.Contains(14) || hashSet3.Contains(40)))
			{
				num6 = 1.15;
			}
			num5 *= formationParamBattle;
			num5 *= formationParamF;
			num5 *= num6;
			switch (attacker.Get_DamageState())
			{
			case DamageState.Tyuuha:
				num5 *= 0.7;
				break;
			case DamageState.Taiha:
				num5 *= 0.4;
				break;
			}
			num5 = Math.Floor(num5);
			if (num5 > 100.0)
			{
				num5 = 100.0 + Math.Sqrt(num5 - 100.0);
			}
			return (int)num5;
		}

		protected int getSubmarineHitProb(Mem_ship attackShip, List<Mst_slotitem> attackSlot, List<int> slotLevels)
		{
			BattleFormationKinds1 formation;
			BattleFormationKinds1 formation2;
			if (!attackShip.IsEnemy())
			{
				formation = F_Data.Formation;
				BattleFormationKinds2 battleFormation = F_Data.BattleFormation;
				formation2 = E_Data.Formation;
			}
			else
			{
				formation = E_Data.Formation;
				BattleFormationKinds2 battleFormation2 = E_Data.BattleFormation;
				formation2 = F_Data.Formation;
			}
			int num = 0;
			double num2 = 0.0;
			foreach (var item in attackSlot.Select((Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				if (item.obj.Api_mapbattle_type3 == 14)
				{
					num += item.obj.Tais;
				}
				if ((item.obj.Api_mapbattle_type3 == 14 || item.obj.Api_mapbattle_type3 == 15 || item.obj.Api_mapbattle_type3 == 40) && slotLevels[item.idx] > 0)
				{
					num2 += Math.Sqrt(slotLevels[item.idx]) * 1.3;
				}
			}
			double num3 = Math.Sqrt((double)attackShip.GetBattleBaseParam().Luck * 1.5) + Math.Sqrt(attackShip.Level * 2) + (double)num * 2.0;
			double num4 = valanceSubmarine2 + num3 + num2;
			double formationParamF = formationData.GetFormationParamF2(FormationDatas.GetFormationKinds.SUBMARINE, formation, formation2);
			num4 *= formationParamF;
			FatigueState fatigueState = attackShip.Get_FatigueState();
			double num5 = 1.0;
			switch (fatigueState)
			{
			case FatigueState.Exaltation:
				num5 = 1.2;
				break;
			case FatigueState.Normal:
				num5 = 1.0;
				break;
			case FatigueState.Light:
				num5 = 0.8;
				break;
			case FatigueState.Distress:
				num5 = 0.5;
				break;
			}
			double num6 = num4 * num5;
			if (practiceFlag)
			{
				num6 *= 1.5;
			}
			return (int)num6;
		}

		protected int getAtapKeisu(int atap_num)
		{
			if (atap_num == 1)
			{
				return 75;
			}
			if (atap_num == 2)
			{
				return 110;
			}
			if (atap_num == 3)
			{
				return 140;
			}
			if (atap_num >= 4)
			{
				return 160;
			}
			return 0;
		}
	}
}
