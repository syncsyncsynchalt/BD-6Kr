using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_Midnight : BattleLogicBase<NightBattleFmt>
	{
		private BattleBaseData _f_Data;

		private BattleBaseData _e_Data;

		private Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		private Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		protected double fValance1;

		protected double fValance2;

		protected double fValance3;

		protected double eValance1;

		protected double eValance2;

		protected double eValance3;

		private int exec_type;

		private List<int> f_AtkIdxs;

		private List<int> e_AtkIdxs;

		private Dictionary<Mst_slotitem, HashSet<Mem_ship>> fTouchPlane = new Dictionary<Mst_slotitem, HashSet<Mem_ship>>();

		private Dictionary<Mst_slotitem, HashSet<Mem_ship>> eTouchPlane = new Dictionary<Mst_slotitem, HashSet<Mem_ship>>();

		private List<int> fSerchLightIdxs = new List<int>();

		private List<int> eSerchLightIdxs = new List<int>();

		private List<int> fFlareIdxs = new List<int>();

		private List<int> eFlareIdxs = new List<int>();

		private int[] seikuValue;

		private Dictionary<BattleAtackKinds_Night, double> spAttackKeisu;

		private Dictionary<BattleAtackKinds_Night, double> spHitProbKeisu;

		private readonly HashSet<int> disableSlotPlusAttackItems;

		private readonly HashSet<int> disableSlotPlusHitItems;

		private readonly HashSet<int> enableSlotPlusHitDentan;

		public override BattleBaseData F_Data => _f_Data;

		public override BattleBaseData E_Data => _e_Data;

		public override Dictionary<int, BattleShipSubInfo> F_SubInfo => _f_SubInfo;

		public override Dictionary<int, BattleShipSubInfo> E_SubInfo => _e_SubInfo;

		public Exec_Midnight(int type, int[] seikuValue, BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, bool practice)
		{
			_f_Data = myData;
			_e_Data = enemyData;
			_f_SubInfo = mySubInfo;
			_e_SubInfo = enemySubInfo;
			practiceFlag = practice;
			exec_type = type;
			f_AtkIdxs = null;
			makeAttackerData(_f_Data.ShipData, out f_AtkIdxs);
			e_AtkIdxs = null;
			makeAttackerData(_e_Data.ShipData, out e_AtkIdxs);
			if (seikuValue != null)
			{
				this.seikuValue = seikuValue;
			}
			else if (exec_type == 2)
			{
				this.seikuValue = new int[2]
				{
					1,
					1
				};
			}
			else
			{
				this.seikuValue = new int[2];
			}
			makeSpSlotItem(_f_Data, this.seikuValue[0]);
			makeSpSlotItem(_e_Data, this.seikuValue[1]);
			fValance1 = (eValance1 = 0.0);
			fValance2 = (eValance2 = 69.0);
			fValance3 = (eValance3 = 1.5);
			if (exec_type == 1 || exec_type == 2)
			{
				spAttackKeisu = new Dictionary<BattleAtackKinds_Night, double>
				{
					{
						BattleAtackKinds_Night.Normal,
						1.0
					},
					{
						BattleAtackKinds_Night.Syu_Rai,
						1.2
					},
					{
						BattleAtackKinds_Night.Rai_Rai,
						1.3
					},
					{
						BattleAtackKinds_Night.Syu_Syu_Fuku,
						1.5
					},
					{
						BattleAtackKinds_Night.Syu_Syu_Syu,
						1.75
					},
					{
						BattleAtackKinds_Night.Renzoku,
						2.0
					}
				};
				spHitProbKeisu = new Dictionary<BattleAtackKinds_Night, double>
				{
					{
						BattleAtackKinds_Night.Normal,
						1.0
					},
					{
						BattleAtackKinds_Night.Syu_Rai,
						1.1
					},
					{
						BattleAtackKinds_Night.Rai_Rai,
						1.5
					},
					{
						BattleAtackKinds_Night.Syu_Syu_Fuku,
						1.65
					},
					{
						BattleAtackKinds_Night.Syu_Syu_Syu,
						1.5
					},
					{
						BattleAtackKinds_Night.Renzoku,
						2.0
					}
				};
			}
			disableSlotPlusAttackItems = new HashSet<int>
			{
				12,
				13,
				21,
				14,
				40,
				16,
				27,
				28,
				17,
				15
			};
			disableSlotPlusHitItems = new HashSet<int>
			{
				21,
				14,
				40,
				16,
				27,
				28,
				17,
				15
			};
			enableSlotPlusHitDentan = new HashSet<int>
			{
				12,
				13
			};
		}

		public override void Dispose()
		{
			randInstance = null;
			f_AtkIdxs.Clear();
			e_AtkIdxs.Clear();
			spAttackKeisu.Clear();
			spHitProbKeisu.Clear();
		}

		public override NightBattleFmt GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			int num = f_AtkIdxs.Count();
			int num2 = e_AtkIdxs.Count();
			NightBattleFmt nightBattleFmt = new NightBattleFmt(F_Data.Deck.Rid, F_Data.ShipData, E_Data.ShipData);
			setTargetingKind(formation);
			formationData = formation;
			nightBattleFmt.F_SearchId = getSerchLightFirstPos(fSerchLightIdxs, F_Data.ShipData);
			nightBattleFmt.E_SearchId = getSerchLightFirstPos(eSerchLightIdxs, E_Data.ShipData);
			nightBattleFmt.F_FlareId = getFlarePos(fFlareIdxs, F_Data.ShipData, ref fValance2);
			nightBattleFmt.E_FlareId = getFlarePos(eFlareIdxs, E_Data.ShipData, ref eValance2);
			if (seikuValue[0] >= 1 && seikuValue[0] <= 3)
			{
				nightBattleFmt.F_TouchPlane = getSyokusetuPlane(fTouchPlane);
				setTouchPlaneValanceValue(nightBattleFmt.F_TouchPlane, ref fValance1, ref fValance2, ref fValance3);
			}
			if (seikuValue[1] >= 1 && seikuValue[1] <= 3)
			{
				nightBattleFmt.E_TouchPlane = getSyokusetuPlane(eTouchPlane);
				setTouchPlaneValanceValue(nightBattleFmt.E_TouchPlane, ref eValance1, ref eValance2, ref eValance3);
			}
			int num3 = (num < num2) ? num2 : num;
			for (int i = 0; i < num3; i++)
			{
				if (i >= num && i >= num2)
				{
					return nightBattleFmt;
				}
				if (i < num)
				{
					Hougeki<BattleAtackKinds_Night> hougekiData = getHougekiData(f_AtkIdxs[i], F_Data.ShipData[f_AtkIdxs[i]]);
					if (hougekiData != null)
					{
						nightBattleFmt.Hougeki.Add(hougekiData);
					}
				}
				if (i < num2)
				{
					Hougeki<BattleAtackKinds_Night> hougekiData2 = getHougekiData(e_AtkIdxs[i], E_Data.ShipData[e_AtkIdxs[i]]);
					if (hougekiData2 != null)
					{
						nightBattleFmt.Hougeki.Add(hougekiData2);
					}
				}
			}
			return nightBattleFmt;
		}

		private void setTouchPlaneValanceValue(int mst_id, ref double vAttack, ref double vHit, ref double vClitical)
		{
			if (mst_id != 0)
			{
				int houm = Mst_DataManager.Instance.Mst_Slotitem[mst_id].Houm;
				if (houm <= 1)
				{
					vAttack = 5.0;
					vHit *= 1.1;
					vClitical = 1.57;
				}
				else if (houm == 2)
				{
					vAttack = 7.0;
					vHit *= 1.15;
					vClitical = 1.64;
				}
				else if (houm >= 3)
				{
					vAttack = 9.0;
					vHit *= 1.2;
					vClitical = 1.7;
				}
			}
		}

		private void makeAttackerData(List<Mem_ship> ships, out List<int> atk_idx)
		{
			atk_idx = new List<int>();
			foreach (var item in ships.Select((Mem_ship ship, int idx) => new
			{
				ship,
				idx
			}))
			{
				int num = Mst_DataManager.Instance.Mst_ship[item.ship.Ship_id].Houg + Mst_DataManager.Instance.Mst_ship[item.ship.Ship_id].Raig;
				if (num > 0)
				{
					atk_idx.Add(item.idx);
				}
			}
		}

		private void makeSpSlotItem(BattleBaseData baseData, int seiku)
		{
			List<int> list;
			Dictionary<Mst_slotitem, HashSet<Mem_ship>> dictionary;
			List<int> list2;
			if (baseData.ShipData[0].IsEnemy())
			{
				list = eSerchLightIdxs;
				dictionary = eTouchPlane;
				list2 = eFlareIdxs;
			}
			else
			{
				list = fSerchLightIdxs;
				dictionary = fTouchPlane;
				list2 = fFlareIdxs;
			}
			List<List<Mst_slotitem>> slotData = baseData.SlotData;
			List<Mem_ship> shipData = baseData.ShipData;
			foreach (var item in shipData.Select((Mem_ship obj, int ship_idx) => new
			{
				obj,
				ship_idx
			}))
			{
				if (item.obj.IsFight())
				{
					foreach (var item2 in slotData[item.ship_idx].Select((Mst_slotitem obj, int slot_idx) => new
					{
						obj,
						slot_idx
					}))
					{
						if (item.obj.Onslot[item2.slot_idx] > 0 && item2.obj.Id == 102)
						{
							if (!dictionary.ContainsKey(item2.obj))
							{
								dictionary.Add(item2.obj, new HashSet<Mem_ship>
								{
									item.obj
								});
							}
							else
							{
								dictionary[item2.obj].Add(item.obj);
							}
						}
						if (item2.obj.Api_mapbattle_type3 == 29)
						{
							if (!list.Contains(item.ship_idx))
							{
								list.Add(item.ship_idx);
							}
						}
						else if (item2.obj.Api_mapbattle_type3 == 33 && !list2.Contains(item.ship_idx))
						{
							list2.Add(item.ship_idx);
						}
					}
				}
			}
		}

		private int getSerchLightFirstPos(List<int> searchInstance, List<Mem_ship> shipInstance)
		{
			foreach (int item in searchInstance)
			{
				if (shipInstance[item].IsFight())
				{
					return shipInstance[item].Rid;
				}
			}
			return 0;
		}

		private int getFlarePos(List<int> flareInstance, List<Mem_ship> shipInstance, ref double valanceHit)
		{
			foreach (int item in flareInstance)
			{
				if (shipInstance[item].IsFight() && shipInstance[item].Nowhp > 4 && randInstance.Next(100) <= 70)
				{
					valanceHit += 5.0;
					return shipInstance[item].Rid;
				}
			}
			return 0;
		}

		private int getSyokusetuPlane(Dictionary<Mst_slotitem, HashSet<Mem_ship>> touchItems)
		{
			IOrderedEnumerable<Mst_slotitem> orderedEnumerable = from x in touchItems.Keys
				orderby x.Houm descending
				select x;
			foreach (Mst_slotitem item in orderedEnumerable)
			{
				Mst_slotitem mst_slotitem = item;
				foreach (Mem_ship item2 in touchItems[item])
				{
					double num = Math.Sqrt(mst_slotitem.Saku);
					double num2 = Math.Sqrt(item2.Level);
					int num3 = (int)(num * num2);
					if (num3 > randInstance.Next(25))
					{
						return mst_slotitem.Id;
					}
				}
			}
			return 0;
		}

		private Hougeki<BattleAtackKinds_Night> getHougekiData(int atk_idx, Mem_ship attacker)
		{
			if (attacker.Get_DamageState() == DamageState.Taiha || !attacker.IsFight())
			{
				return null;
			}
			Func<int, bool> func = null;
			BattleBaseData battleBaseData;
			Dictionary<int, BattleShipSubInfo> dictionary;
			BattleBaseData battleBaseData2;
			Dictionary<int, BattleShipSubInfo> dictionary2;
			List<int> list;
			double cliticalKeisu;
			if (attacker.IsEnemy())
			{
				battleBaseData = E_Data;
				dictionary = E_SubInfo;
				battleBaseData2 = F_Data;
				dictionary2 = F_SubInfo;
				func = base.RecoveryShip;
				list = fSerchLightIdxs;
				double eValance4 = eValance1;
				double eValance5 = eValance2;
				cliticalKeisu = eValance3;
			}
			else
			{
				battleBaseData = F_Data;
				dictionary = F_SubInfo;
				battleBaseData2 = E_Data;
				dictionary2 = E_SubInfo;
				list = eSerchLightIdxs;
				double fValance4 = fValance1;
				double fValance5 = fValance2;
				cliticalKeisu = fValance3;
			}
			if (!isAttackerFromTargetKind(dictionary[attacker.Rid]))
			{
				return null;
			}
			Hougeki<BattleAtackKinds_Night> hougeki = new Hougeki<BattleAtackKinds_Night>();
			KeyValuePair<int, int> subMarineAtackKeisu = getSubMarineAtackKeisu(battleBaseData2.ShipData, attacker, battleBaseData.SlotData[atk_idx], midnight: true);
			bool flag = false;
			if (subMarineAtackKeisu.Key != 0)
			{
				hougeki.SpType = ((subMarineAtackKeisu.Key != 1) ? BattleAtackKinds_Night.AirAttack : BattleAtackKinds_Night.Bakurai);
				hougeki.Slot_List.Add(0);
				flag = true;
			}
			BattleDamageKinds dKind = BattleDamageKinds.Normal;
			Mem_ship atackTarget = getAtackTarget(attacker, battleBaseData2.ShipData, overKill: false, flag, rescueFlag: true, ref dKind);
			if (atackTarget == null)
			{
				return null;
			}
			int deckIdx = dictionary2[atackTarget.Rid].DeckIdx;
			if (atackTarget.Nowhp > 1 && list.Count > 0 && list[0] == deckIdx)
			{
				dKind = BattleDamageKinds.Normal;
				atackTarget = getAtackTarget(attacker, battleBaseData2.ShipData, overKill: false, flag, rescueFlag: true, ref dKind);
				deckIdx = dictionary2[atackTarget.Rid].DeckIdx;
			}
			if (!flag)
			{
				setSlotData(atk_idx, attacker, battleBaseData.SlotData[atk_idx], atackTarget, hougeki);
			}
			hougeki.Attacker = attacker.Rid;
			int num = (hougeki.SpType != BattleAtackKinds_Night.Renzoku) ? 1 : 2;
			HashSet<BattleAtackKinds_Night> hashSet = new HashSet<BattleAtackKinds_Night>();
			hashSet.Add(BattleAtackKinds_Night.Rai_Rai);
			hashSet.Add(BattleAtackKinds_Night.Renzoku);
			hashSet.Add(BattleAtackKinds_Night.Syu_Rai);
			hashSet.Add(BattleAtackKinds_Night.Syu_Syu_Fuku);
			hashSet.Add(BattleAtackKinds_Night.Syu_Syu_Syu);
			HashSet<BattleAtackKinds_Night> hashSet2 = hashSet;
			for (int i = 0; i < num; i++)
			{
				int soukou = atackTarget.Soukou;
				List<Mst_slotitem> list2 = battleBaseData.SlotData[atk_idx];
				int atkPow;
				int hitProb;
				FormationDatas.GetFormationKinds battleState;
				if (flag)
				{
					atkPow = getSubmarineAttackValue(subMarineAtackKeisu, attacker, list2, battleBaseData.SlotLevel[atk_idx]);
					hitProb = getSubmarineHitProb(attacker, battleBaseData.SlotData[atk_idx], battleBaseData.SlotLevel[atk_idx]);
					battleState = FormationDatas.GetFormationKinds.SUBMARINE;
				}
				else
				{
					atkPow = getMidnightAttackValue(hougeki.SpType, attacker, list2, atackTarget);
					hitProb = getMidnightHitProb(hougeki.SpType, attacker, list2, list);
					battleState = FormationDatas.GetFormationKinds.MIDNIGHT;
				}
				int battleAvo_Midnight = getBattleAvo_Midnight(battleState, atackTarget, list.Contains(deckIdx));
				BattleHitStatus battleHitStatus = getHitStatus(hitProb, battleAvo_Midnight, attacker, atackTarget, cliticalKeisu, airAttack: false);
				if (battleHitStatus == BattleHitStatus.Miss && hashSet2.Contains(hougeki.SpType))
				{
					battleHitStatus = BattleHitStatus.Normal;
				}
				hougeki.Target.Add(atackTarget.Rid);
				int item = setDamageValue(battleHitStatus, atkPow, soukou, attacker, atackTarget, battleBaseData2.LostFlag);
				hougeki.Damage.Add(item);
				hougeki.Clitical.Add(battleHitStatus);
				hougeki.DamageKind.Add(dKind);
			}
			func?.Invoke(deckIdx);
			return hougeki;
		}

		private int getMidnightAttackValue(BattleAtackKinds_Night kind, Mem_ship atk_ship, List<Mst_slotitem> atk_slot, Mem_ship def_ship)
		{
			int num = 300;
			List<int> list;
			if (atk_ship.IsEnemy())
			{
				int deckIdx = E_SubInfo[atk_ship.Rid].DeckIdx;
				list = E_Data.SlotLevel[deckIdx];
			}
			else
			{
				int deckIdx2 = F_SubInfo[atk_ship.Rid].DeckIdx;
				list = F_Data.SlotLevel[deckIdx2];
			}
			double num2 = 0.0;
			int num3 = 0;
			foreach (var item in atk_slot.Select((Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				Mst_slotitem obj2 = item.obj;
				if (IsAtapSlotItem(obj2.Api_mapbattle_type3))
				{
					num3++;
				}
				num2 += getSlotPlus_Attack(obj2, list[item.idx]);
			}
			double num4 = atk_ship.IsEnemy() ? eValance1 : fValance1;
			double num5;
			if (Mst_DataManager.Instance.Mst_stype[def_ship.Stype].IsLandFacillity(Mst_DataManager.Instance.Mst_ship[def_ship.Ship_id].Soku))
			{
				num5 = num4 + (double)atk_ship.Houg + num2;
				num5 *= getLandFacciilityKeisu(atk_slot);
				num5 += (double)getAtapKeisu(num3);
			}
			else
			{
				num5 = num4 + (double)atk_ship.Houg + (double)atk_ship.Raig + num2;
			}
			num5 *= spAttackKeisu[kind];
			DamageState damageState = atk_ship.Get_DamageState();
			double num6 = 1.0;
			switch (damageState)
			{
			case DamageState.Tyuuha:
				num6 = 0.7;
				break;
			case DamageState.Taiha:
				num6 = 0.4;
				break;
			}
			num5 *= num6;
			if (num5 > (double)num)
			{
				num5 = (double)num + Math.Sqrt(num5 - (double)num);
			}
			return (int)num5;
		}

		private double getSlotPlus_Attack(Mst_slotitem mstItem, int slotLevel)
		{
			double result = 0.0;
			if (slotLevel <= 0)
			{
				return result;
			}
			if (disableSlotPlusAttackItems.Contains(mstItem.Api_mapbattle_type3))
			{
				return 0.0;
			}
			return Math.Sqrt(slotLevel);
		}

		private int getMidnightHitProb(BattleAtackKinds_Night kind, Mem_ship atk_ship, List<Mst_slotitem> atk_slot, List<int> deckSearchLight)
		{
			List<int> list;
			if (atk_ship.IsEnemy())
			{
				int deckIdx = E_SubInfo[atk_ship.Rid].DeckIdx;
				list = E_Data.SlotLevel[deckIdx];
			}
			else
			{
				int deckIdx2 = F_SubInfo[atk_ship.Rid].DeckIdx;
				list = F_Data.SlotLevel[deckIdx2];
			}
			double num = 0.0;
			int num2 = 0;
			foreach (var item in atk_slot.Select((Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				num2 += item.obj.Houm;
				num += getSlotPlus_HitProb(item.obj, list[item.idx]);
			}
			double num3 = Math.Sqrt((double)atk_ship.GetBattleBaseParam().Luck * 1.5) + Math.Sqrt(atk_ship.Level) * 2.0 + (double)num2;
			double num4 = (!atk_ship.IsEnemy()) ? fValance2 : eValance2;
			num3 = num4 + num3;
			BattleFormationKinds1 formation;
			BattleFormationKinds1 formation2;
			if (!atk_ship.IsEnemy())
			{
				formation = F_Data.Formation;
				formation2 = E_Data.Formation;
			}
			else
			{
				formation = E_Data.Formation;
				formation2 = F_Data.Formation;
			}
			double formationParamF = formationData.GetFormationParamF2(FormationDatas.GetFormationKinds.MIDNIGHT, formation, formation2);
			num3 = num3 * spHitProbKeisu[kind] * formationParamF;
			FatigueState fatigueState = atk_ship.Get_FatigueState();
			double num5 = 1.0;
			switch (fatigueState)
			{
			case FatigueState.Exaltation:
				num5 = 1.2;
				break;
			case FatigueState.Light:
				num5 = 0.8;
				break;
			case FatigueState.Distress:
				num5 = 0.5;
				break;
			}
			num3 *= num5;
			if (atk_ship.Stype == 5 || atk_ship.Stype == 6)
			{
				int num6 = 0;
				if (atk_slot.Contains(Mst_DataManager.Instance.Mst_Slotitem[6]))
				{
					num6 = 10;
				}
				else if (atk_slot.Contains(Mst_DataManager.Instance.Mst_Slotitem[50]))
				{
					num6 = 15;
				}
				num3 += (double)num6;
			}
			if (deckSearchLight.Count > 0)
			{
				num3 += 7.0;
			}
			num3 = getMidnightHitProbUpValue(num3, atk_ship, atk_slot);
			return (int)num3;
		}

		private double getSlotPlus_HitProb(Mst_slotitem mstItem, int slotLevel)
		{
			double result = 0.0;
			if (slotLevel <= 0)
			{
				return result;
			}
			if (disableSlotPlusHitItems.Contains(mstItem.Api_mapbattle_type3))
			{
				return 0.0;
			}
			double num = (!enableSlotPlusHitDentan.Contains(mstItem.Api_mapbattle_type3) || mstItem.Houm < 3) ? 1.3 : 1.6;
			return Math.Sqrt(slotLevel) * num;
		}

		private double getMidnightHitProbUpValue(double hit_prob, Mem_ship atk_ship, List<Mst_slotitem> atk_slot)
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[atk_ship.Ship_id];
			if (atk_ship.Stype != 8 && atk_ship.Stype != 10 && (atk_ship.Stype != 9 || mst_ship.Taik > 92))
			{
				return hit_prob;
			}
			ILookup<int, Mst_slotitem> lookup = atk_slot.ToLookup((Mst_slotitem x) => x.Id);
			Dictionary<int, HashSet<int>> dictionary = new Dictionary<int, HashSet<int>>();
			dictionary.Add(1, new HashSet<int>
			{
				9
			});
			dictionary.Add(2, new HashSet<int>
			{
				117
			});
			dictionary.Add(3, new HashSet<int>
			{
				105,
				8
			});
			dictionary.Add(4, new HashSet<int>
			{
				7,
				103,
				104,
				76,
				114
			});
			Dictionary<int, HashSet<int>> dictionary2 = dictionary;
			Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
			foreach (KeyValuePair<int, HashSet<int>> item in dictionary2)
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
			if (atk_ship.Stype == 8)
			{
				num3 = num3 - 10.0 * num2 * Math.Sqrt(dictionary3[1]) - 5.0 * num2 * Math.Sqrt(dictionary3[3]) - 7.0 * num2 * Math.Sqrt(dictionary3[2]);
				num3 += 4.0 * Math.Sqrt(dictionary3[4]);
			}
			else if (atk_ship.Stype == 10)
			{
				num3 = num3 - 8.0 * num2 * Math.Sqrt(dictionary3[1]) - 5.0 * num2 * Math.Sqrt(dictionary3[2]);
				num3 = num3 + 4.0 * Math.Sqrt(dictionary3[4]) + 2.0 * Math.Sqrt(dictionary3[3]);
			}
			else if (atk_ship.Stype == 9)
			{
				num3 = num3 - 7.0 * num2 * Math.Sqrt(dictionary3[1]) - 3.0 * num2 * Math.Sqrt(dictionary3[2]);
				num3 = num3 + 2.0 * Math.Sqrt(dictionary3[4]) + 2.0 * Math.Sqrt(dictionary3[3]);
			}
			return num3;
		}

		private void setSlotData(int attackerIdx, Mem_ship attacker, List<Mst_slotitem> atk_slot, Mem_ship target, Hougeki<BattleAtackKinds_Night> setData)
		{
			Mst_stype mst_stype = Mst_DataManager.Instance.Mst_stype[target.Stype];
			if (mst_stype.IsSubmarine())
			{
				setData.Slot_List.Add(0);
				setData.SpType = BattleAtackKinds_Night.Normal;
				return;
			}
			if (atk_slot == null || atk_slot.Count == 0)
			{
				setData.Slot_List.Add(0);
				setData.SpType = BattleAtackKinds_Night.Normal;
				return;
			}
			int luck = attacker.GetBattleBaseParam().Luck;
			int num = (int)((double)(luck + 15) + Math.Sqrt(attacker.Level) * 0.75);
			if (luck >= 50)
			{
				num = (int)(65.0 + (Math.Sqrt(luck) - 50.0) + Math.Sqrt(attacker.Level) * 0.8);
			}
			if (atk_slot.Any((Mst_slotitem x) => x.Id == 129))
			{
				num += 5;
			}
			List<int> list;
			List<int> list2;
			List<int> list3;
			List<int> list4;
			if (attacker.IsEnemy())
			{
				list = eSerchLightIdxs;
				list2 = fSerchLightIdxs;
				list3 = eFlareIdxs;
				list4 = fFlareIdxs;
			}
			else
			{
				list = fSerchLightIdxs;
				list2 = eSerchLightIdxs;
				list3 = fFlareIdxs;
				list4 = eFlareIdxs;
			}
			if (list3.Count > 0)
			{
				num += 4;
			}
			if (list4.Count > 0)
			{
				num -= 10;
			}
			if (list.Count > 0)
			{
				num += 7;
			}
			if (list2.Count > 0)
			{
				num -= 5;
			}
			if (attacker.Get_DamageState() == DamageState.Tyuuha)
			{
				num += 18;
			}
			if (attackerIdx == 0)
			{
				num += 15;
			}
			List<int> list5 = new List<int>();
			list5.Add(1);
			list5.Add(2);
			list5.Add(3);
			List<int> list6 = list5;
			list5 = new List<int>();
			list5.Add(4);
			List<int> list7 = list5;
			list5 = new List<int>();
			list5.Add(5);
			list5.Add(32);
			List<int> list8 = list5;
			List<Mst_slotitem> list9 = new List<Mst_slotitem>();
			List<Mst_slotitem> list10 = new List<Mst_slotitem>();
			List<Mst_slotitem> list11 = new List<Mst_slotitem>();
			List<Mst_slotitem> list12 = new List<Mst_slotitem>();
			int soku = Mst_DataManager.Instance.Mst_ship[target.Ship_id].Soku;
			foreach (Mst_slotitem item in atk_slot)
			{
				if (list6.Contains(item.Api_mapbattle_type3))
				{
					list9.Add(item);
					list12.Add(item);
				}
				else if (list7.Contains(item.Api_mapbattle_type3))
				{
					list10.Add(item);
					list12.Add(item);
				}
				else if (list8.Contains(item.Api_mapbattle_type3) && !mst_stype.IsLandFacillity(soku))
				{
					list11.Add(item);
					list12.Add(item);
				}
			}
			if (list12.Count == 0)
			{
				setData.Slot_List.Add(0);
				setData.SpType = BattleAtackKinds_Night.Normal;
				return;
			}
			List<BattleAtackKinds_Night> list13 = new List<BattleAtackKinds_Night>();
			Dictionary<BattleAtackKinds_Night, List<int>> dictionary = new Dictionary<BattleAtackKinds_Night, List<int>>();
			if (list9.Count >= 3)
			{
				list13.Add(BattleAtackKinds_Night.Syu_Syu_Syu);
				list5 = new List<int>();
				list5.Add(list9[0].Id);
				list5.Add(list9[1].Id);
				list5.Add(list9[2].Id);
				List<int> value = list5;
				dictionary.Add(BattleAtackKinds_Night.Syu_Syu_Syu, value);
			}
			if (list9.Count >= 2 && list10.Count >= 1)
			{
				list13.Add(BattleAtackKinds_Night.Syu_Syu_Fuku);
				list5 = new List<int>();
				list5.Add(list9[0].Id);
				list5.Add(list9[1].Id);
				list5.Add(list10[0].Id);
				List<int> value2 = list5;
				dictionary.Add(BattleAtackKinds_Night.Syu_Syu_Fuku, value2);
			}
			if (list11.Count >= 2)
			{
				list13.Add(BattleAtackKinds_Night.Rai_Rai);
				list5 = new List<int>();
				list5.Add(list11[0].Id);
				list5.Add(list11[1].Id);
				List<int> value3 = list5;
				dictionary.Add(BattleAtackKinds_Night.Rai_Rai, value3);
			}
			if (list11.Count >= 1 && list9.Count >= 1)
			{
				list13.Add(BattleAtackKinds_Night.Syu_Rai);
				list5 = new List<int>();
				list5.Add(list9[0].Id);
				list5.Add(list11[0].Id);
				List<int> value4 = list5;
				dictionary.Add(BattleAtackKinds_Night.Syu_Rai, value4);
			}
			if (list12.Count >= 2)
			{
				list13.Add(BattleAtackKinds_Night.Renzoku);
				list5 = new List<int>();
				list5.Add(list12[0].Id);
				list5.Add(list12[1].Id);
				List<int> value5 = list5;
				dictionary.Add(BattleAtackKinds_Night.Renzoku, value5);
			}
			setData.SpType = BattleAtackKinds_Night.Normal;
			setData.Slot_List.Add(list12[0].Id);
			Dictionary<BattleAtackKinds_Night, int> dictionary2 = new Dictionary<BattleAtackKinds_Night, int>();
			dictionary2.Add(BattleAtackKinds_Night.Syu_Syu_Syu, 140);
			dictionary2.Add(BattleAtackKinds_Night.Syu_Syu_Fuku, 130);
			dictionary2.Add(BattleAtackKinds_Night.Rai_Rai, 122);
			dictionary2.Add(BattleAtackKinds_Night.Syu_Rai, 115);
			dictionary2.Add(BattleAtackKinds_Night.Renzoku, 110);
			Dictionary<BattleAtackKinds_Night, int> dictionary3 = dictionary2;
			foreach (BattleAtackKinds_Night item2 in list13)
			{
				int num2 = randInstance.Next(dictionary3[item2]);
				if (num > num2)
				{
					setData.SpType = item2;
					setData.Slot_List = dictionary[item2];
					break;
				}
			}
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}
	}
}
