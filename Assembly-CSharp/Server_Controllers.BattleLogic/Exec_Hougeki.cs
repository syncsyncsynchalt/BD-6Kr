using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_Hougeki : BattleLogicBase<HougekiDayBattleFmt>
	{
		protected BattleBaseData _f_Data;

		protected BattleBaseData _e_Data;

		protected Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		protected Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		protected MiddleBattleCallInfo callReferenceInfo;

		protected int[] seikuValue;

		protected List<int> f_AtkIdxs;

		protected List<int> e_AtkIdxs;

		protected double valance1;

		protected double valance2;

		protected double valance3;

		protected int AIR_ATACK_KEISU = 15;

		protected HashSet<BattleAtackKinds_Day> enableSpType;

		private HashSet<int> airAttackEndRid;

		public override BattleBaseData F_Data => _f_Data;

		public override BattleBaseData E_Data => _e_Data;

		public override Dictionary<int, BattleShipSubInfo> F_SubInfo => _f_SubInfo;

		public override Dictionary<int, BattleShipSubInfo> E_SubInfo => _e_SubInfo;

		public Exec_Hougeki(MiddleBattleCallInfo callInfo, int[] seikuValue, BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, bool practice)
		{
			_f_Data = myData;
			_e_Data = enemyData;
			_f_SubInfo = mySubInfo;
			_e_SubInfo = enemySubInfo;
			practiceFlag = practice;
			callReferenceInfo = callInfo;
			this.seikuValue = ((seikuValue != null) ? seikuValue : new int[2]);
			f_AtkIdxs = new List<int>();
			e_AtkIdxs = new List<int>();
			makeAttackerData(enemyFlag: false);
			makeAttackerData(enemyFlag: true);
			valance1 = 5.0;
			valance2 = 90.0;
			valance3 = 1.3;
			enableSpType = new HashSet<BattleAtackKinds_Day>
			{
				BattleAtackKinds_Day.Renzoku,
				BattleAtackKinds_Day.Sp1,
				BattleAtackKinds_Day.Sp2,
				BattleAtackKinds_Day.Sp3,
				BattleAtackKinds_Day.Sp4
			};
		}

		public void SetAirSubstituteAttacker(HashSet<int> airAtteker)
		{
			airAttackEndRid = airAtteker;
		}

		public override void Dispose()
		{
			randInstance = null;
			f_AtkIdxs.Clear();
			e_AtkIdxs.Clear();
			enableSpType.Clear();
		}

		public override HougekiDayBattleFmt GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			commandParams = cParam;
			setTargetingKind(formation);
			formationData = formation;
			int num = f_AtkIdxs.Count();
			int num2 = e_AtkIdxs.Count();
			if (num == 0 && num2 == 0)
			{
				return null;
			}
			new HougekiDayBattleFmt();
			List<int> fAtkIdx = new List<int>();
			List<int> eAtkIdx = new List<int>();
			if (callReferenceInfo.CommandPos != -1)
			{
				fAtkIdx = takeAttacker(f_AtkIdxs, callReferenceInfo.AttackType);
			}
			else
			{
				eAtkIdx = takeAttacker(e_AtkIdxs, callReferenceInfo.AttackType);
			}
			return GetResultData(callReferenceInfo.UseCommand, callReferenceInfo.CommandPos, fAtkIdx, eAtkIdx);
		}

		private List<int> takeAttacker(List<int> srcList, int atkType)
		{
			int count = srcList.Count;
			int count2 = count / 2 + count % 2;
			switch (atkType)
			{
			case 1:
				return srcList.Take(count2).ToList();
			case 2:
				return srcList.Skip(count2).ToList();
			case 3:
				return srcList.ToList();
			default:
				return new List<int>();
			}
		}

		private HougekiDayBattleFmt GetResultData(BattleCommand command, int commandPos, List<int> fAtkIdx, List<int> eAtkIdx)
		{
			if (!F_Data.ShipData.Any((Mem_ship x) => x.IsFight()) || !E_Data.ShipData.Any((Mem_ship y) => y.IsFight()))
			{
				return null;
			}
			if (!isHougCommand(command))
			{
				fAtkIdx.Clear();
			}
			HougekiDayBattleFmt hougekiDayBattleFmt = new HougekiDayBattleFmt();
			List<Hougeki<BattleAtackKinds_Day>> list = new List<Hougeki<BattleAtackKinds_Day>>();
			int count = fAtkIdx.Count;
			int count2 = eAtkIdx.Count;
			int num = (count < count2) ? count2 : count;
			for (int i = 0; i < num; i++)
			{
				if (i >= count && i >= count2)
				{
					return null;
				}
				if (i < count)
				{
					Hougeki<BattleAtackKinds_Day> hougekiData = getHougekiData(command, fAtkIdx[i], F_Data.ShipData[fAtkIdx[i]]);
					if (hougekiData != null)
					{
						list.Add(hougekiData);
					}
				}
				if (i < count2)
				{
					Hougeki<BattleAtackKinds_Day> hougekiData2 = getHougekiData(eAtkIdx[i], E_Data.ShipData[eAtkIdx[i]]);
					if (hougekiData2 != null)
					{
						list.Add(hougekiData2);
					}
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			hougekiDayBattleFmt.AttackData = list;
			return hougekiDayBattleFmt;
		}

		public static bool isHougCommand(BattleCommand command)
		{
			HashSet<BattleCommand> hashSet = new HashSet<BattleCommand>();
			hashSet.Add(BattleCommand.Hougeki);
			hashSet.Add(BattleCommand.Taisen);
			hashSet.Add(BattleCommand.Kouku);
			hashSet.Add(BattleCommand.Tousha);
			hashSet.Add(BattleCommand.Totugeki);
			HashSet<BattleCommand> hashSet2 = hashSet;
			return hashSet2.Contains(command);
		}

		private Hougeki<BattleAtackKinds_Day> getHougekiData(BattleCommand command, int atk_idx, Mem_ship attacker)
		{
			if (!attacker.IsFight())
			{
				return null;
			}
			BattleBaseData f_Data = F_Data;
			Dictionary<int, BattleShipSubInfo> f_SubInfo = F_SubInfo;
			BattleBaseData e_Data = E_Data;
			Dictionary<int, BattleShipSubInfo> e_SubInfo = E_SubInfo;
			if (!isAttackerFromTargetKind(f_SubInfo[attacker.Rid]))
			{
				return null;
			}
			Dictionary<int, Mst_stype> mst_stypes = Mst_DataManager.Instance.Mst_stype;
			Dictionary<int, Mst_ship> mst_ships = Mst_DataManager.Instance.Mst_ship;
			List<Mem_ship> shipData = e_Data.ShipData;
			bool submarine_flag = false;
			BattleAtackKinds_Day battleAtackKinds_Day = BattleAtackKinds_Day.Normal;
			List<int> slot_List = null;
			KeyValuePair<int, int> submarine_keisu = new KeyValuePair<int, int>(0, 0);
			bool flag = IsAirAttackGroup(attacker, f_Data.SlotData[atk_idx], command);
			if (flag && !CanAirAtack_DamageState(attacker))
			{
				return null;
			}
			int num = 0;
			if (command != BattleCommand.Taisen)
			{
				shipData = (from x in shipData
					where !mst_stypes[x.Stype].IsSubmarine()
					select x).ToList();
				if (flag)
				{
					if (command != BattleCommand.Kouku)
					{
						return null;
					}
					if (!CanAirAttack(attacker, f_Data.SlotData[atk_idx]))
					{
						return null;
					}
					battleAtackKinds_Day = BattleAtackKinds_Day.AirAttack;
					List<int> list = new List<int>();
					list.Add(0);
					slot_List = list;
					if (!isValidAirAtack_To_LandFaccillity(attacker, f_Data.SlotData[atk_idx]))
					{
						shipData = (from x in shipData
							where (!mst_stypes[x.Stype].IsLandFacillity(mst_ships[x.Ship_id].Soku)) ? true : false
							select x).ToList();
					}
				}
				else if (!isValidHougeki(attacker))
				{
					return null;
				}
				if (!flag && command == BattleCommand.Kouku)
				{
					if (airAttackEndRid.Contains(attacker.Rid))
					{
						return null;
					}
					airAttackEndRid.Add(attacker.Rid);
					num = 2;
					int hougSlotData = getHougSlotData(f_Data.SlotData[atk_idx]);
					List<int> list = new List<int>();
					list.Add(hougSlotData);
					slot_List = list;
				}
			}
			else
			{
				shipData = (from x in shipData
					where mst_stypes[x.Stype].IsSubmarine()
					select x).ToList();
				submarine_keisu = getSubMarineAtackKeisu(shipData, attacker, f_Data.SlotData[atk_idx], midnight: false);
				if (submarine_keisu.Key == 0)
				{
					shipData = (from x in e_Data.ShipData
						where !mst_stypes[x.Stype].IsSubmarine()
						select x).ToList();
					submarine_flag = false;
					num = 1;
					if (flag)
					{
						if (!CanAirAttack(attacker, f_Data.SlotData[atk_idx]))
						{
							return null;
						}
						battleAtackKinds_Day = BattleAtackKinds_Day.AirAttack;
						List<int> list = new List<int>();
						list.Add(0);
						slot_List = list;
						if (!isValidAirAtack_To_LandFaccillity(attacker, f_Data.SlotData[atk_idx]))
						{
							shipData = (from x in shipData
								where (!mst_stypes[x.Stype].IsLandFacillity(mst_ships[x.Ship_id].Soku)) ? true : false
								select x).ToList();
						}
					}
					else if (!isValidHougeki(attacker))
					{
						return null;
					}
					if (!flag)
					{
						int hougSlotData2 = getHougSlotData(f_Data.SlotData[atk_idx]);
						List<int> list = new List<int>();
						list.Add(hougSlotData2);
						slot_List = list;
					}
				}
				else
				{
					battleAtackKinds_Day = ((submarine_keisu.Key == 1) ? BattleAtackKinds_Day.Bakurai : BattleAtackKinds_Day.AirAttack);
					List<int> list = new List<int>();
					list.Add(0);
					slot_List = list;
					submarine_flag = true;
				}
			}
			if (shipData.Count == 0)
			{
				return null;
			}
			if (command != BattleCommand.Taisen && num == 0)
			{
				KeyValuePair<BattleAtackKinds_Day, List<int>> spAttackKind = getSpAttackKind(attacker, f_Data.SlotData[atk_idx]);
				if (battleAtackKinds_Day != BattleAtackKinds_Day.AirAttack || spAttackKind.Key != 0)
				{
					battleAtackKinds_Day = spAttackKind.Key;
					slot_List = spAttackKind.Value;
				}
			}
			Hougeki<BattleAtackKinds_Day> attackData = getAttackData(attacker, f_Data.SlotData[atk_idx], f_Data.SlotLevel[atk_idx], battleAtackKinds_Day, submarine_flag, submarine_keisu, shipData, e_Data.LostFlag, e_SubInfo, num);
			if (attackData != null)
			{
				attackData.Slot_List = slot_List;
			}
			return attackData;
		}

		private Hougeki<BattleAtackKinds_Day> getHougekiData(int atk_idx, Mem_ship attacker)
		{
			if (!attacker.IsFight())
			{
				return null;
			}
			BattleBaseData e_Data = E_Data;
			Dictionary<int, BattleShipSubInfo> e_SubInfo = E_SubInfo;
			Dictionary<int, BattleShipSubInfo> f_SubInfo = F_SubInfo;
			BattleBaseData f_Data = F_Data;
			if (!isAttackerFromTargetKind(e_SubInfo[attacker.Rid]))
			{
				return null;
			}
			BattleAtackKinds_Day battleAtackKinds_Day = BattleAtackKinds_Day.Normal;
			List<int> slot_List = null;
			List<Mem_ship> list = f_Data.ShipData;
			KeyValuePair<int, int> subMarineAtackKeisu = getSubMarineAtackKeisu(list, attacker, e_Data.SlotData[atk_idx], midnight: false);
			bool submarine_flag = false;
			bool flag = IsAirAttackGroup(attacker, e_Data.SlotData[atk_idx], BattleCommand.None);
			if (flag && !CanAirAtack_DamageState(attacker))
			{
				return null;
			}
			if (subMarineAtackKeisu.Key != 0)
			{
				battleAtackKinds_Day = ((subMarineAtackKeisu.Key == 1) ? BattleAtackKinds_Day.Bakurai : BattleAtackKinds_Day.AirAttack);
				List<int> list2 = new List<int>();
				list2.Add(0);
				slot_List = list2;
				submarine_flag = true;
			}
			else
			{
				if (flag)
				{
					if (!CanAirAttack(attacker, e_Data.SlotData[atk_idx]))
					{
						return null;
					}
					battleAtackKinds_Day = BattleAtackKinds_Day.AirAttack;
					List<int> list2 = new List<int>();
					list2.Add(0);
					slot_List = list2;
					if (!isValidAirAtack_To_LandFaccillity(attacker, e_Data.SlotData[atk_idx]))
					{
						List<Mem_ship> list3 = (from shipobj in list
							let soku = Mst_DataManager.Instance.Mst_ship[shipobj.Ship_id].Soku
							let land_flag = Mst_DataManager.Instance.Mst_stype[shipobj.Stype].IsLandFacillity(soku)
							where !land_flag
							select shipobj).ToList();
						list = list3;
					}
				}
				else if (!isValidHougeki(attacker))
				{
					return null;
				}
				KeyValuePair<BattleAtackKinds_Day, List<int>> spAttackKind = getSpAttackKind(attacker, e_Data.SlotData[atk_idx]);
				if (battleAtackKinds_Day != BattleAtackKinds_Day.AirAttack || spAttackKind.Key != 0)
				{
					battleAtackKinds_Day = spAttackKind.Key;
					slot_List = spAttackKind.Value;
				}
			}
			Hougeki<BattleAtackKinds_Day> attackData = getAttackData(attacker, e_Data.SlotData[atk_idx], e_Data.SlotLevel[atk_idx], battleAtackKinds_Day, submarine_flag, subMarineAtackKeisu, list, f_Data.LostFlag, f_SubInfo, 0);
			if (attackData != null)
			{
				attackData.Slot_List = slot_List;
			}
			return attackData;
		}

		private Hougeki<BattleAtackKinds_Day> getAttackData(Mem_ship attacker, List<Mst_slotitem> attackerSlot, List<int> attackerSlotLevel, BattleAtackKinds_Day attackType, bool submarine_flag, KeyValuePair<int, int> submarine_keisu, List<Mem_ship> targetShips, List<bool> targetLostFlags, Dictionary<int, BattleShipSubInfo> targetSubInfo, int powerDownType)
		{
			BattleDamageKinds dKind = BattleDamageKinds.Normal;
			Mem_ship atackTarget = getAtackTarget(attacker, targetShips, overKill: false, submarine_flag, rescueFlag: true, ref dKind);
			if (atackTarget == null)
			{
				return null;
			}
			int deckIdx = targetSubInfo[atackTarget.Rid].DeckIdx;
			Hougeki<BattleAtackKinds_Day> hougeki = new Hougeki<BattleAtackKinds_Day>();
			hougeki.Attacker = attacker.Rid;
			hougeki.SpType = attackType;
			int num = (attackType != BattleAtackKinds_Day.Renzoku) ? 1 : 2;
			for (int i = 0; i < num; i++)
			{
				int soukou = atackTarget.Soukou;
				hougeki.Target.Add(atackTarget.Rid);
				int num2;
				int num3;
				FormationDatas.GetFormationKinds battleState;
				if (submarine_flag)
				{
					num2 = getSubmarineAttackValue(submarine_keisu, attacker, attackerSlot, attackerSlotLevel);
					num3 = getSubmarineHitProb(attacker, attackerSlot, attackerSlotLevel);
					battleState = FormationDatas.GetFormationKinds.SUBMARINE;
				}
				else
				{
					int tekkouKind = getTekkouKind(atackTarget.Stype, attackerSlot);
					num2 = getHougAttackValue(attackType, attacker, attackerSlot, atackTarget, tekkouKind);
					num3 = getHougHitProb(attackType, attacker, attackerSlot, tekkouKind);
					battleState = FormationDatas.GetFormationKinds.HOUGEKI;
				}
				int battleAvo = getBattleAvo(battleState, atackTarget);
				switch (powerDownType)
				{
				case 1:
					num2 = (int)((double)num2 * 0.55);
					num3 = (int)((double)num3 * 0.55);
					break;
				case 2:
					num3 = (int)((double)num3 * 0.55);
					break;
				}
				bool airAttack = (attackType == BattleAtackKinds_Day.AirAttack) ? true : false;
				BattleHitStatus battleHitStatus = getHitStatus(num3, battleAvo, attacker, atackTarget, valance3, airAttack);
				if (battleHitStatus == BattleHitStatus.Miss && enableSpType.Contains(attackType))
				{
					battleHitStatus = BattleHitStatus.Normal;
				}
				int item = setDamageValue(battleHitStatus, num2, soukou, attacker, atackTarget, targetLostFlags);
				hougeki.Damage.Add(item);
				hougeki.Clitical.Add(battleHitStatus);
				hougeki.DamageKind.Add(dKind);
			}
			if (attacker.IsEnemy())
			{
				RecoveryShip(deckIdx);
			}
			return hougeki;
		}

		protected virtual void makeAttackerData(bool enemyFlag)
		{
			List<int> list;
			Dictionary<int, BattleShipSubInfo> dictionary;
			if (!enemyFlag)
			{
				list = f_AtkIdxs;
				BattleBaseData f_Datum = F_Data;
				dictionary = F_SubInfo;
			}
			else
			{
				list = e_AtkIdxs;
				BattleBaseData e_Datum = E_Data;
				dictionary = E_SubInfo;
			}
			IOrderedEnumerable<BattleShipSubInfo> orderedEnumerable = from x in dictionary.Values
				orderby x.AttackNo
				select x;
			foreach (BattleShipSubInfo item in orderedEnumerable)
			{
				list.Add(item.DeckIdx);
			}
		}

		protected virtual int getHougAttackValue(BattleAtackKinds_Day kind, Mem_ship atk_ship, List<Mst_slotitem> atk_slot, Mem_ship def_ship, int tekkouKind)
		{
			int num = 150;
			List<int> list;
			BattleFormationKinds1 formation;
			BattleFormationKinds2 battleFormation;
			if (atk_ship.IsEnemy())
			{
				int deckIdx = E_SubInfo[atk_ship.Rid].DeckIdx;
				list = E_Data.SlotLevel[deckIdx];
				formation = E_Data.Formation;
				battleFormation = E_Data.BattleFormation;
				BattleFormationKinds1 formation2 = F_Data.Formation;
			}
			else
			{
				int deckIdx2 = F_SubInfo[atk_ship.Rid].DeckIdx;
				list = F_Data.SlotLevel[deckIdx2];
				formation = F_Data.Formation;
				battleFormation = F_Data.BattleFormation;
				BattleFormationKinds1 formation3 = E_Data.Formation;
			}
			double num2 = 0.0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			foreach (var item in atk_slot.Select((Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				Mst_slotitem obj2 = item.obj;
				if (IsAtapSlotItem(obj2.Api_mapbattle_type3))
				{
					num5++;
				}
				num3 += obj2.Baku;
				num4 += obj2.Raig;
				num2 += getHougSlotPlus_Attack(obj2, list[item.idx]);
			}
			double num6 = valance1 + (double)atk_ship.Houg + num2;
			if (Mst_DataManager.Instance.Mst_stype[def_ship.Stype].IsLandFacillity(Mst_DataManager.Instance.Mst_ship[def_ship.Ship_id].Soku))
			{
				num6 *= getLandFacciilityKeisu(atk_slot);
				num6 += (double)getAtapKeisu(num5);
				num4 = 0;
			}
			if (kind == BattleAtackKinds_Day.AirAttack)
			{
				int airAtackPow = getAirAtackPow(num3, num4);
				num6 += (double)airAtackPow;
				num6 = 25.0 + (double)(int)(num6 * 1.5);
			}
			double formationParamBattle = formationData.GetFormationParamBattle(FormationDatas.GetFormationKinds.HOUGEKI, battleFormation);
			double formationParamF = formationData.GetFormationParamF1(FormationDatas.GetFormationKinds.HOUGEKI, formation);
			num6 = num6 * formationParamBattle * formationParamF;
			DamageState damageState = atk_ship.Get_DamageState();
			double num7 = 1.0;
			switch (damageState)
			{
			case DamageState.Tyuuha:
				num7 = 0.7;
				break;
			case DamageState.Taiha:
				num7 = 0.4;
				break;
			}
			num6 *= num7;
			num6 += getHougItemAtackHosei(atk_ship, atk_slot);
			if (num6 > (double)num)
			{
				num6 = (double)num + Math.Sqrt(num6 - (double)num);
			}
			Dictionary<BattleAtackKinds_Day, double> dictionary = new Dictionary<BattleAtackKinds_Day, double>();
			dictionary.Add(BattleAtackKinds_Day.Renzoku, 1.2);
			dictionary.Add(BattleAtackKinds_Day.Sp1, 1.1);
			dictionary.Add(BattleAtackKinds_Day.Sp2, 1.2);
			dictionary.Add(BattleAtackKinds_Day.Sp3, 1.3);
			dictionary.Add(BattleAtackKinds_Day.Sp4, 1.5);
			Dictionary<BattleAtackKinds_Day, double> dictionary2 = dictionary;
			if (dictionary2.ContainsKey(kind))
			{
				num6 *= dictionary2[kind];
			}
			num6 *= getTekkouKeisu_Attack(tekkouKind);
			return (int)num6;
		}

		protected virtual double getHougSlotPlus_Attack(Mst_slotitem mstItem, int slotLevel)
		{
			double result = 0.0;
			if (slotLevel <= 0)
			{
				return result;
			}
			if (mstItem.Api_mapbattle_type3 == 5 || mstItem.Api_mapbattle_type3 == 22)
			{
				return result;
			}
			double num = 2.0;
			if (mstItem.Houg > 12)
			{
				num = 3.0;
			}
			if (mstItem.Api_mapbattle_type3 == 12 || mstItem.Api_mapbattle_type3 == 13 || mstItem.Api_mapbattle_type3 == 16 || mstItem.Api_mapbattle_type3 == 17 || mstItem.Api_mapbattle_type3 == 27 || mstItem.Api_mapbattle_type3 == 28)
			{
				num = 0.0;
			}
			else if (mstItem.Api_mapbattle_type3 == 14 || mstItem.Api_mapbattle_type3 == 15 || mstItem.Api_mapbattle_type3 == 40)
			{
				num = 1.5;
			}
			return num * Math.Sqrt(slotLevel) * 0.5;
		}

		protected virtual double getHougItemAtackHosei(Mem_ship ship, List<Mst_slotitem> mst_item)
		{
			if (mst_item.Count == 0)
			{
				return 0.0;
			}
			if (ship.Stype != 3 && ship.Stype != 4 && ship.Stype != 21)
			{
				return 0.0;
			}
			ILookup<int, Mst_slotitem> lookup = mst_item.ToLookup((Mst_slotitem x) => x.Id);
			int num = 0;
			if (lookup.Contains(4))
			{
				num += lookup[4].Count();
			}
			if (lookup.Contains(11))
			{
				num += lookup[11].Count();
			}
			int num2 = 0;
			if (lookup.Contains(119))
			{
				num2 += lookup[119].Count();
			}
			if (lookup.Contains(65))
			{
				num2 += lookup[65].Count();
			}
			if (lookup.Contains(139))
			{
				num2 += lookup[139].Count();
			}
			return 1.0 * Math.Sqrt(num) + 2.0 * Math.Sqrt(num2);
		}

		protected virtual int getHougHitProb(BattleAtackKinds_Day kind, Mem_ship atk_ship, List<Mst_slotitem> atk_slot, int tekkouKind)
		{
			double num = 0.0;
			BattleFormationKinds1 formation;
			BattleFormationKinds1 formation2;
			List<int> list;
			if (!atk_ship.IsEnemy())
			{
				formation = F_Data.Formation;
				formation2 = E_Data.Formation;
				int deckIdx = F_SubInfo[atk_ship.Rid].DeckIdx;
				list = F_Data.SlotLevel[deckIdx];
				num = (double)commandParams.Fspp / 100.0;
			}
			else
			{
				formation = E_Data.Formation;
				formation2 = F_Data.Formation;
				int deckIdx2 = E_SubInfo[atk_ship.Rid].DeckIdx;
				list = E_Data.SlotLevel[deckIdx2];
			}
			double num2 = 0.0;
			int num3 = 0;
			foreach (var item in atk_slot.Select((Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				num3 += item.obj.Houm;
				num2 += getHougSlotPlus_Hit(item.obj, list[item.idx]);
			}
			double num4 = Math.Sqrt((double)atk_ship.GetBattleBaseParam().Luck * 1.5) + Math.Sqrt(atk_ship.Level) * 2.0 + (double)num3;
			double num5 = valance2 + num4 + num2;
			double formationParamF = formationData.GetFormationParamF2(FormationDatas.GetFormationKinds.HOUGEKI, formation, formation2);
			num5 *= formationParamF;
			FatigueState fatigueState = atk_ship.Get_FatigueState();
			double num6 = 1.0;
			switch (fatigueState)
			{
			case FatigueState.Exaltation:
				num6 = 1.2;
				break;
			case FatigueState.Light:
				num6 = 0.8;
				break;
			case FatigueState.Distress:
				num6 = 0.5;
				break;
			}
			num5 *= num6;
			num5 = getHougHitProbUpValue(num5, atk_ship, atk_slot);
			Dictionary<BattleAtackKinds_Day, double> dictionary = new Dictionary<BattleAtackKinds_Day, double>();
			dictionary.Add(BattleAtackKinds_Day.Renzoku, 1.1);
			dictionary.Add(BattleAtackKinds_Day.Sp1, 1.3);
			dictionary.Add(BattleAtackKinds_Day.Sp2, 1.5);
			dictionary.Add(BattleAtackKinds_Day.Sp3, 1.3);
			dictionary.Add(BattleAtackKinds_Day.Sp4, 1.2);
			Dictionary<BattleAtackKinds_Day, double> dictionary2 = dictionary;
			if (dictionary2.ContainsKey(kind))
			{
				num5 *= dictionary2[kind];
			}
			num5 *= getTekkouKeisu_Hit(tekkouKind);
			double num7 = num5 * num;
			num5 += num7;
			return (int)num5;
		}

		protected virtual double getHougSlotPlus_Hit(Mst_slotitem mstItem, int slotLevel)
		{
			double result = 0.0;
			if (slotLevel <= 0)
			{
				return result;
			}
			if (mstItem.Api_mapbattle_type3 == 5 || mstItem.Api_mapbattle_type3 == 22)
			{
				return result;
			}
			if ((mstItem.Api_mapbattle_type3 == 12 || mstItem.Api_mapbattle_type3 == 13) && mstItem.Houm > 2)
			{
				result = Math.Sqrt(slotLevel) * 1.7;
			}
			else if (mstItem.Api_mapbattle_type3 == 21 || mstItem.Api_mapbattle_type3 == 14 || mstItem.Api_mapbattle_type3 == 40 || mstItem.Api_mapbattle_type3 == 16 || mstItem.Api_mapbattle_type3 == 27 || mstItem.Api_mapbattle_type3 == 28 || mstItem.Api_mapbattle_type3 == 17 || mstItem.Api_mapbattle_type3 == 15)
			{
				result = Math.Sqrt(slotLevel);
			}
			return result;
		}

		protected virtual double getHougHitProbUpValue(double hit_prob, Mem_ship atk_ship, List<Mst_slotitem> atk_slot)
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[atk_ship.Ship_id];
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(8);
			hashSet.Add(10);
			hashSet.Add(3);
			hashSet.Add(9);
			hashSet.Add(4);
			hashSet.Add(21);
			HashSet<int> hashSet2 = hashSet;
			if (!hashSet2.Contains(atk_ship.Stype))
			{
				return hit_prob;
			}
			if (atk_ship.Stype == 9 && mst_ship.Taik > 92)
			{
				return hit_prob;
			}
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			dictionary.Add(1, new List<int>
			{
				9
			});
			dictionary.Add(2, new List<int>
			{
				117
			});
			dictionary.Add(3, new List<int>
			{
				105,
				8
			});
			dictionary.Add(4, new List<int>
			{
				7,
				103,
				104,
				76,
				114
			});
			dictionary.Add(5, new List<int>
			{
				133,
				137
			});
			dictionary.Add(6, new List<int>
			{
				4,
				11
			});
			dictionary.Add(7, new List<int>
			{
				119,
				65,
				139
			});
			Dictionary<int, List<int>> dictionary2 = dictionary;
			Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
			ILookup<int, Mst_slotitem> lookup = atk_slot.ToLookup((Mst_slotitem x) => x.Id);
			foreach (KeyValuePair<int, List<int>> item in dictionary2)
			{
				int num = 0;
				foreach (int item2 in item.Value)
				{
					if (lookup.Contains(item2))
					{
						num += lookup[item2].Count();
					}
				}
				dictionary3.Add(item.Key, num);
			}
			double num2 = 1.0;
			double num3 = hit_prob;
			if (atk_ship.Level >= 100)
			{
				num2 = 0.6;
			}
			int num4 = dictionary3[1];
			int num5 = dictionary3[2];
			int num6 = dictionary3[3];
			int num7 = dictionary3[4];
			int num8 = dictionary3[5];
			int num9 = dictionary3[6];
			int num10 = dictionary3[7];
			if (atk_ship.Stype == 8)
			{
				num3 = num3 - 10.0 * num2 * Math.Sqrt(num4) - 5.0 * num2 * Math.Sqrt(num6) - 7.0 * num2 * Math.Sqrt(num5);
				num3 -= 2.0 * num2 * Math.Sqrt(num8);
				if (mst_ship.Yomi.Equals("ビスマルク") || mst_ship.Yomi.Equals("リットリオ・イタリア") || mst_ship.Yomi.Equals("ロ\u30fcマ"))
				{
					num3 += 3.0 * num2 * Math.Sqrt(num8);
				}
				num3 += 4.0 * Math.Sqrt(num7);
			}
			else if (atk_ship.Stype == 10)
			{
				num3 = num3 - 7.0 * num2 * Math.Sqrt(num4) - 3.0 * num2 * Math.Sqrt(num5);
				num3 += 2.0 * num2 * Math.Sqrt(num8);
				num3 = num3 + 4.0 * Math.Sqrt(num7) + 2.0 * Math.Sqrt(num6);
			}
			else if (atk_ship.Stype == 9)
			{
				num3 = num3 - 7.0 * num2 * Math.Sqrt(num4) - 3.0 * num2 * Math.Sqrt(num5);
				num3 += 2.0 * num2 * Math.Sqrt(num8);
				num3 = num3 + 2.0 * Math.Sqrt(num7) + 2.0 * Math.Sqrt(num6);
			}
			else if (atk_ship.Stype == 3 || atk_ship.Stype == 4 || atk_ship.Stype == 21)
			{
				num3 = num3 + 4.0 * Math.Sqrt(num9) + 3.0 * Math.Sqrt(num10) - 2.0;
			}
			return num3;
		}

		protected int getHougSlotData(List<Mst_slotitem> items)
		{
			int a_slot = 0;
			List<int> b_slot = new List<int>();
			List<Mst_slotitem> list = items.ToList();
			list.RemoveAll((Mst_slotitem x) => x == null);
			list.ForEach(delegate(Mst_slotitem x)
			{
				if (x.Api_mapbattle_type3 >= 1 && x.Api_mapbattle_type3 <= 3)
				{
					a_slot = x.Id;
				}
				else if (x.Api_mapbattle_type3 == 4)
				{
					b_slot.Add(x.Id);
				}
			});
			if (a_slot > 0)
			{
				return a_slot;
			}
			if (a_slot == 0 && b_slot.Count == 0)
			{
				return 0;
			}
			return b_slot[0];
		}

		protected KeyValuePair<BattleAtackKinds_Day, List<int>> getSpAttackKind(Mem_ship ship, List<Mst_slotitem> slotitems)
		{
			if (slotitems.Count == 0)
			{
				return new KeyValuePair<BattleAtackKinds_Day, List<int>>(BattleAtackKinds_Day.Normal, new List<int>
				{
					0
				});
			}
			BattleAtackKinds_Day battleAtackKinds_Day = BattleAtackKinds_Day.Normal;
			List<int> ret_slotitem = new List<int>();
			Func<List<Mst_slotitem>, KeyValuePair<BattleAtackKinds_Day, List<int>>> func = delegate(List<Mst_slotitem> x)
			{
				int hougSlotData = getHougSlotData(x);
				ret_slotitem.Add(hougSlotData);
				return new KeyValuePair<BattleAtackKinds_Day, List<int>>(BattleAtackKinds_Day.Normal, ret_slotitem);
			};
			int num;
			BattleBaseData battleBaseData;
			Dictionary<int, BattleShipSubInfo> dictionary;
			if (ship.IsEnemy())
			{
				num = seikuValue[1];
				battleBaseData = E_Data;
				dictionary = E_SubInfo;
			}
			else
			{
				num = seikuValue[0];
				battleBaseData = F_Data;
				dictionary = F_SubInfo;
			}
			if (num <= 1)
			{
				return func(slotitems);
			}
			if (ship.Get_DamageState() >= DamageState.Taiha)
			{
				return func(slotitems);
			}
			Dictionary<int, List<Mst_slotitem>> dictionary2 = new Dictionary<int, List<Mst_slotitem>>();
			dictionary2.Add(1, new List<Mst_slotitem>());
			dictionary2.Add(12, new List<Mst_slotitem>());
			dictionary2.Add(10, new List<Mst_slotitem>());
			dictionary2.Add(19, new List<Mst_slotitem>());
			dictionary2.Add(4, new List<Mst_slotitem>());
			Dictionary<int, List<Mst_slotitem>> dictionary3 = dictionary2;
			double num2 = 0.0;
			foreach (var item in slotitems.Select((Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				int api_mapbattle_type = item.obj.Api_mapbattle_type3;
				num2 += (double)item.obj.Saku;
				switch (api_mapbattle_type)
				{
				case 1:
				case 2:
				case 3:
					dictionary3[1].Add(item.obj);
					continue;
				case 12:
				case 13:
					dictionary3[12].Add(item.obj);
					continue;
				case 10:
				case 11:
					if (ship.Onslot[item.idx] > 0)
					{
						dictionary3[10].Add(item.obj);
						continue;
					}
					break;
				}
				if (api_mapbattle_type == 19 || api_mapbattle_type == 4)
				{
					dictionary3[api_mapbattle_type].Add(item.obj);
				}
			}
			if (dictionary3[10].Count == 0 || dictionary3[1].Count == 0)
			{
				return func(slotitems);
			}
			double num3 = 0.0;
			foreach (var item2 in battleBaseData.ShipData.Select((Mem_ship obj, int ship_idx) => new
			{
				obj,
				ship_idx
			}))
			{
				if (item2.obj.IsFight())
				{
					num3 += (double)item2.obj.GetBattleBaseParam().Sakuteki;
					List<Mst_slotitem> list = battleBaseData.SlotData[item2.ship_idx];
					if (list.Count != 0)
					{
						foreach (var item3 in list.Select((Mst_slotitem obj, int slot_idx) => new
						{
							obj,
							slot_idx
						}))
						{
							int num4 = item2.obj.Onslot[item3.slot_idx];
							if ((item3.obj.Api_mapbattle_type3 == 10 || item3.obj.Api_mapbattle_type3 == 11) && num4 > 0)
							{
								int num5 = item3.obj.Saku * (int)Math.Sqrt(num4);
								num3 += (double)num5;
							}
						}
					}
				}
			}
			double num6 = (int)(Math.Sqrt(num3) + num3 * 0.1);
			int num7 = (int)(Math.Sqrt(ship.GetBattleBaseParam().Luck) + 10.0);
			switch (num)
			{
			case 3:
				num7 = (int)((double)num7 + 10.0 + (num6 + num2 * 1.6) * 0.7);
				break;
			case 2:
				num7 = (int)((double)num7 + (num6 + num2 * 1.2) * 0.6);
				break;
			}
			if (dictionary[ship.Rid].DeckIdx == 0)
			{
				num7 += 15;
			}
			Dictionary<BattleAtackKinds_Day, int> dictionary4 = new Dictionary<BattleAtackKinds_Day, int>();
			dictionary4.Add(BattleAtackKinds_Day.Sp4, 150);
			dictionary4.Add(BattleAtackKinds_Day.Sp3, 140);
			dictionary4.Add(BattleAtackKinds_Day.Sp2, 130);
			dictionary4.Add(BattleAtackKinds_Day.Sp1, 120);
			dictionary4.Add(BattleAtackKinds_Day.Renzoku, 130);
			Dictionary<BattleAtackKinds_Day, int> dictionary5 = dictionary4;
			if (dictionary3[1].Count >= 2 && dictionary3[19].Count >= 1 && num7 > randInstance.Next(dictionary5[BattleAtackKinds_Day.Sp4]))
			{
				ret_slotitem.Add(dictionary3[10][0].Id);
				ret_slotitem.Add(dictionary3[1][0].Id);
				ret_slotitem.Add(dictionary3[1][1].Id);
				battleAtackKinds_Day = BattleAtackKinds_Day.Sp4;
			}
			else if (dictionary3[4].Count >= 1 && dictionary3[19].Count >= 1 && num7 > randInstance.Next(dictionary5[BattleAtackKinds_Day.Sp3]))
			{
				ret_slotitem.Add(dictionary3[10][0].Id);
				ret_slotitem.Add(dictionary3[1][0].Id);
				ret_slotitem.Add(dictionary3[19][0].Id);
				battleAtackKinds_Day = BattleAtackKinds_Day.Sp3;
			}
			else if (dictionary3[4].Count >= 1 && dictionary3[12].Count >= 1 && num7 > randInstance.Next(dictionary5[BattleAtackKinds_Day.Sp2]))
			{
				ret_slotitem.Add(dictionary3[10][0].Id);
				ret_slotitem.Add(dictionary3[12][0].Id);
				ret_slotitem.Add(dictionary3[1][0].Id);
				battleAtackKinds_Day = BattleAtackKinds_Day.Sp2;
			}
			else if (dictionary3[4].Count >= 1 && num7 > randInstance.Next(dictionary5[BattleAtackKinds_Day.Sp1]))
			{
				ret_slotitem.Add(dictionary3[10][0].Id);
				ret_slotitem.Add(dictionary3[1][0].Id);
				ret_slotitem.Add(dictionary3[4][0].Id);
				battleAtackKinds_Day = BattleAtackKinds_Day.Sp1;
			}
			else if (dictionary3[1].Count >= 2 && num7 > randInstance.Next(dictionary5[BattleAtackKinds_Day.Renzoku]))
			{
				ret_slotitem.Add(dictionary3[1][0].Id);
				ret_slotitem.Add(dictionary3[1][1].Id);
				battleAtackKinds_Day = BattleAtackKinds_Day.Renzoku;
			}
			if (battleAtackKinds_Day == BattleAtackKinds_Day.Normal)
			{
				return func(slotitems);
			}
			return new KeyValuePair<BattleAtackKinds_Day, List<int>>(battleAtackKinds_Day, ret_slotitem);
		}

		protected bool isValidHougeki(Mem_ship ship)
		{
			if (!ship.IsFight())
			{
				return false;
			}
			if (Mst_DataManager.Instance.Mst_stype[ship.Stype].IsSubmarine())
			{
				return false;
			}
			return (ship.GetBattleBaseParam().Houg > 0) ? true : false;
		}

		protected bool IsAirAttackGroup(Mem_ship ship, List<Mst_slotitem> slotData, BattleCommand command)
		{
			if (Mst_DataManager.Instance.Mst_stype[ship.Stype].IsMother() || Mst_DataManager.Instance.Mst_stype[ship.Stype].IsLandFacillity(Mst_DataManager.Instance.Mst_ship[ship.Ship_id].Soku))
			{
				return true;
			}
			if (ship.Stype == 22)
			{
				if (ship.IsEnemy())
				{
					return slotData.Any((Mst_slotitem x) => x.Api_mapbattle_type3 == 7 || x.Api_mapbattle_type3 == 8);
				}
				if (command == BattleCommand.Kouku)
				{
					return true;
				}
			}
			return false;
		}

		protected bool CanAirAttack(Mem_ship ship, List<Mst_slotitem> slotData)
		{
			if (!ship.IsFight())
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(8);
			HashSet<int> hashSet2 = hashSet;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < slotData.Count(); i++)
			{
				Mst_slotitem mst_slotitem = slotData[i];
				if (mst_slotitem != null && hashSet2.Contains(mst_slotitem.Api_mapbattle_type3))
				{
					num += ship.Onslot[i];
					num2 += mst_slotitem.Baku;
					num3 += mst_slotitem.Raig;
				}
			}
			if (num <= 0)
			{
				return false;
			}
			if (getAirAtackPow(num2, num3) <= 0)
			{
				return false;
			}
			return true;
		}

		protected bool CanAirAtack_DamageState(Mem_ship ship)
		{
			DamageState damageState = (ship.Stype != 18) ? DamageState.Tyuuha : DamageState.Taiha;
			return (ship.Get_DamageState() < damageState) ? true : false;
		}

		protected virtual int getAirAtackPow(int baku, int raig)
		{
			return (int)(Math.Floor((double)baku * 1.3) + (double)raig) + AIR_ATACK_KEISU;
		}

		protected virtual bool isValidAirAtack_To_LandFaccillity(Mem_ship attacker, List<Mst_slotitem> slotitems)
		{
			foreach (var item in slotitems.Select((Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				if (item.obj.Api_mapbattle_type3 == 7 && attacker.Onslot[item.idx] > 0)
				{
					return true;
				}
			}
			return false;
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}

		private int getTekkouKind(int targetStype, List<Mst_slotitem> attackerSlot)
		{
			int result = 0;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(5);
			hashSet.Add(6);
			hashSet.Add(8);
			hashSet.Add(9);
			hashSet.Add(10);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> hashSet2 = hashSet;
			if (!hashSet2.Contains(targetStype))
			{
				return result;
			}
			bool haveMain = false;
			bool haveSub = false;
			bool haveDentan = false;
			bool haveTekkou = false;
			attackerSlot.ForEach(delegate(Mst_slotitem mst)
			{
				if (mst.Api_mapbattle_type3 >= 1 && mst.Api_mapbattle_type3 <= 3)
				{
					haveMain = true;
				}
				else if (mst.Api_mapbattle_type3 == 4)
				{
					haveSub = true;
				}
				else if (mst.Api_mapbattle_type3 == 12 || mst.Api_mapbattle_type3 == 13)
				{
					haveDentan = true;
				}
				else if (mst.Api_mapbattle_type3 == 19)
				{
					haveTekkou = true;
				}
			});
			if (haveMain && haveSub && haveDentan && haveTekkou)
			{
				return 4;
			}
			if (haveMain && haveSub && haveTekkou)
			{
				return 3;
			}
			if (haveMain && haveDentan && haveTekkou)
			{
				return 2;
			}
			if (haveMain && haveTekkou)
			{
				return 1;
			}
			return result;
		}

		private double getTekkouKeisu_Attack(int tekkouKind)
		{
			switch (tekkouKind)
			{
			case 1:
				return 1.08;
			case 2:
				return 1.1;
			case 3:
			case 4:
				return 1.15;
			default:
				return 1.0;
			}
		}

		private double getTekkouKeisu_Hit(int tekkouKind)
		{
			switch (tekkouKind)
			{
			case 1:
				return 1.1;
			case 2:
				return 1.25;
			case 3:
				return 1.2;
			case 4:
				return 1.3;
			default:
				return 1.0;
			}
		}
	}
}
