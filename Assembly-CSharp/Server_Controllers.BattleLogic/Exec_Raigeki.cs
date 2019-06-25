using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_Raigeki : BattleLogicBase<Raigeki>
	{
		protected BattleBaseData _f_Data;

		protected BattleBaseData _e_Data;

		protected Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		protected Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		protected List<int> f_AtkIdxs;

		protected List<int> e_AtkIdxs;

		protected List<int> f_startHp;

		protected List<int> e_startHp;

		protected double valance1;

		protected double valance2;

		protected double valance3;

		protected HashSet<int> enableRaigSlotPlusItems;

		public override BattleBaseData F_Data => _f_Data;

		public override BattleBaseData E_Data => _e_Data;

		public override Dictionary<int, BattleShipSubInfo> F_SubInfo => _f_SubInfo;

		public override Dictionary<int, BattleShipSubInfo> E_SubInfo => _e_SubInfo;

		public Exec_Raigeki(int atkType, BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, bool practice)
		{
			_f_Data = myData;
			_e_Data = enemyData;
			_f_SubInfo = mySubInfo;
			_e_SubInfo = enemySubInfo;
			f_AtkIdxs = new List<int>();
			e_AtkIdxs = new List<int>();
			f_startHp = new List<int>();
			e_startHp = new List<int>();
			practiceFlag = practice;
			if (atkType == 1 || atkType == 3)
			{
				makeAttackerData(enemyFlag: false);
			}
			if (atkType == 2 || atkType == 3)
			{
				makeAttackerData(enemyFlag: true);
			}
			valance1 = 5.0;
			valance2 = 85.0;
			valance3 = 1.5;
			enableRaigSlotPlusItems = new HashSet<int>
			{
				5,
				22
			};
		}

		public override Raigeki GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			commandParams = cParam;
			setTargetingKind(formation);
			int num = f_AtkIdxs.Count();
			int num2 = e_AtkIdxs.Count();
			if (num == 0 && num2 == 0)
			{
				return null;
			}
			if (num > 0)
			{
				setHitHoseiFromBattleCommand();
			}
			Raigeki raigeki = new Raigeki();
			formationData = formation;
			raigeki.F_Rai = getRaigekiData(enemyFlag: false);
			raigeki.E_Rai = getRaigekiData(enemyFlag: true);
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
			return raigeki;
		}

		public override void Dispose()
		{
			randInstance = null;
			f_AtkIdxs.Clear();
			e_AtkIdxs.Clear();
			f_startHp.Clear();
			e_startHp.Clear();
		}

		protected virtual void setHitHoseiFromBattleCommand()
		{
		}

		protected virtual RaigekiInfo getRaigekiData(bool enemyFlag)
		{
			RaigekiInfo raigekiInfo = new RaigekiInfo();
			List<int> list;
			BattleBaseData battleBaseData;
			Dictionary<int, BattleShipSubInfo> dictionary;
			BattleBaseData battleBaseData2;
			Dictionary<int, BattleShipSubInfo> dictionary2;
			if (enemyFlag)
			{
				list = e_AtkIdxs;
				battleBaseData = E_Data;
				dictionary = E_SubInfo;
				battleBaseData2 = F_Data;
				dictionary2 = F_SubInfo;
			}
			else
			{
				list = f_AtkIdxs;
				battleBaseData = F_Data;
				dictionary = F_SubInfo;
				battleBaseData2 = E_Data;
				dictionary2 = E_SubInfo;
			}
			int num = list.Count();
			if (num == 0)
			{
				return raigekiInfo;
			}
			List<Mem_ship> list2 = battleBaseData2.ShipData.ToList();
			Dictionary<int, Mst_stype> mst_stype = Mst_DataManager.Instance.Mst_stype;
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			list2.RemoveAll(delegate(Mem_ship x)
			{
				if (x.Nowhp <= 0)
				{
					return true;
				}
				return mst_stype[x.Stype].IsLandFacillity(mst_ship[x.Ship_id].Soku) ? true : false;
			});
			if (list2.Count == 0)
			{
				return raigekiInfo;
			}
			for (int i = 0; i < num; i++)
			{
				int num2 = list[i];
				Mem_ship mem_ship = battleBaseData.ShipData[num2];
				List<Mst_slotitem> atk_slot = battleBaseData.SlotData[num2];
				if (isAttackerFromTargetKind(dictionary[mem_ship.Rid]))
				{
					BattleDamageKinds dKind = BattleDamageKinds.Normal;
					Mem_ship atackTarget = getAtackTarget(mem_ship, list2, overKill: true, subMarineFlag: false, rescueFlag: true, ref dKind);
					if (atackTarget != null)
					{
						int deckIdx = dictionary2[atackTarget.Rid].DeckIdx;
						int raigAttackValue = getRaigAttackValue(mem_ship, atk_slot, atackTarget);
						int soukou = atackTarget.Soukou;
						int raigHitProb = getRaigHitProb(mem_ship, atk_slot, raigAttackValue);
						int battleAvo = getBattleAvo(FormationDatas.GetFormationKinds.RAIGEKI, atackTarget);
						BattleHitStatus hitStatus = getHitStatus(raigHitProb, battleAvo, mem_ship, atackTarget, valance3, airAttack: false);
						int num3 = setDamageValue(hitStatus, raigAttackValue, soukou, mem_ship, atackTarget, battleBaseData2.LostFlag);
						raigekiInfo.Damage[num2] = num3;
						raigekiInfo.Target[num2] = deckIdx;
						raigekiInfo.DamageKind[num2] = dKind;
						raigekiInfo.Clitical[num2] = hitStatus;
					}
				}
			}
			return raigekiInfo;
		}

		protected virtual int getRaigAttackValue(Mem_ship atk_ship, List<Mst_slotitem> atk_slot, Mem_ship def_ship)
		{
			int num = 150;
			BattleFormationKinds1 formation;
			BattleFormationKinds2 battleFormation;
			List<int> list;
			if (!atk_ship.IsEnemy())
			{
				formation = F_Data.Formation;
				battleFormation = F_Data.BattleFormation;
				list = F_Data.SlotLevel[F_SubInfo[atk_ship.Rid].DeckIdx];
				BattleFormationKinds1 formation2 = E_Data.Formation;
			}
			else
			{
				formation = E_Data.Formation;
				battleFormation = E_Data.BattleFormation;
				list = E_Data.SlotLevel[E_SubInfo[atk_ship.Rid].DeckIdx];
				BattleFormationKinds1 formation3 = F_Data.Formation;
			}
			double num2 = 0.0;
			foreach (var item in atk_slot.Select((Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				num2 += getSlotPlus_Attack(item.obj, list[item.idx]);
			}
			double num3 = valance1 + (double)atk_ship.Raig + num2;
			double formationParamBattle = formationData.GetFormationParamBattle(FormationDatas.GetFormationKinds.RAIGEKI, battleFormation);
			double formationParamF = formationData.GetFormationParamF1(FormationDatas.GetFormationKinds.RAIGEKI, formation);
			num3 = num3 * formationParamBattle * formationParamF;
			DamageState damageState = atk_ship.Get_DamageState();
			double num4 = 1.0;
			switch (damageState)
			{
			case DamageState.Tyuuha:
				num4 = 0.8;
				break;
			case DamageState.Taiha:
				num4 = 0.0;
				break;
			}
			num3 *= num4;
			if (num3 > (double)num)
			{
				num3 = (double)num + Math.Sqrt(num3 - (double)num);
			}
			return (int)num3;
		}

		protected virtual double getSlotPlus_Attack(Mst_slotitem mstItem, int slotLevel)
		{
			if (slotLevel <= 0)
			{
				return 0.0;
			}
			if (!enableRaigSlotPlusItems.Contains(mstItem.Api_mapbattle_type3))
			{
				return 0.0;
			}
			return Math.Sqrt(slotLevel) * 1.2;
		}

		protected virtual int getRaigHitProb(Mem_ship atk_ship, List<Mst_slotitem> atk_slot, int atk_pow)
		{
			double num = 0.0;
			List<int> list;
			BattleFormationKinds1 formation;
			BattleFormationKinds1 formation2;
			if (!atk_ship.IsEnemy())
			{
				list = F_Data.SlotLevel[F_SubInfo[atk_ship.Rid].DeckIdx];
				formation = F_Data.Formation;
				formation2 = E_Data.Formation;
				num = (double)commandParams.Tspp / 100.0;
			}
			else
			{
				list = E_Data.SlotLevel[E_SubInfo[atk_ship.Rid].DeckIdx];
				formation = E_Data.Formation;
				formation2 = F_Data.Formation;
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
				num2 += getSlotPlus_HitProb(item.obj, list[item.idx]);
			}
			int raim = Mst_DataManager.Instance.Mst_ship[atk_ship.Ship_id].Raim;
			double num4 = Math.Sqrt((double)atk_ship.GetBattleBaseParam().Luck * 1.5) + Math.Sqrt(atk_ship.Level) * 2.0 + (double)num3;
			num4 = num4 + (double)(int)((double)atk_pow * 0.2) + (double)raim;
			num4 = valance2 + num4 + num2;
			double formationParamF = formationData.GetFormationParamF2(FormationDatas.GetFormationKinds.RAIGEKI, formation, formation2);
			num4 *= formationParamF;
			FatigueState fatigueState = atk_ship.Get_FatigueState();
			double num5 = 1.0;
			switch (fatigueState)
			{
			case FatigueState.Exaltation:
				num5 = 1.3;
				break;
			case FatigueState.Light:
				num5 = 0.7;
				break;
			case FatigueState.Distress:
				num5 = 0.35;
				break;
			}
			num4 *= num5;
			double num6 = num4 * num;
			num4 += num6;
			return (int)num4;
		}

		protected virtual double getSlotPlus_HitProb(Mst_slotitem mstItem, int slotLevel)
		{
			if (slotLevel <= 0)
			{
				return 0.0;
			}
			if (!enableRaigSlotPlusItems.Contains(mstItem.Api_mapbattle_type3))
			{
				return 0.0;
			}
			return Math.Sqrt(slotLevel) * 2.0;
		}

		protected virtual void makeAttackerData(bool enemyFlag)
		{
			List<int> atkInstance = null;
			List<int> hpInstance = null;
			BattleBaseData battleBaseData;
			bool flag;
			if (!enemyFlag)
			{
				atkInstance = f_AtkIdxs;
				battleBaseData = F_Data;
				hpInstance = f_startHp;
				flag = isRaigBattleCommand();
			}
			else
			{
				atkInstance = e_AtkIdxs;
				battleBaseData = E_Data;
				hpInstance = e_startHp;
				flag = true;
			}
			if (flag)
			{
				int ins_idx = 0;
				battleBaseData.ShipData.ForEach(delegate(Mem_ship x)
				{
					hpInstance.Add(x.Nowhp);
					if (isValidRaigeki(x))
					{
						atkInstance.Add(ins_idx);
					}
					ins_idx++;
				});
			}
		}

		protected virtual bool isRaigBattleCommand()
		{
			return true;
		}

		protected virtual bool isValidRaigeki(Mem_ship ship)
		{
			if (ship.Get_DamageState() > DamageState.Shouha || !ship.IsFight())
			{
				return false;
			}
			if (ship.GetBattleBaseParam().Raig <= 0)
			{
				return false;
			}
			return true;
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			List<Mst_slotitem> source;
			List<int> list;
			if (target.IsEnemy())
			{
				int deckIdx = E_SubInfo[target.Rid].DeckIdx;
				source = E_Data.SlotData[deckIdx];
				list = E_Data.SlotLevel[deckIdx];
			}
			else
			{
				int deckIdx2 = F_SubInfo[target.Rid].DeckIdx;
				source = F_Data.SlotData[deckIdx2];
				list = F_Data.SlotLevel[deckIdx2];
			}
			double num = 0.0;
			foreach (var item in source.Select((Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				if (item.obj.Api_mapbattle_type3 == 14 || item.obj.Api_mapbattle_type3 == 40)
				{
					num += Math.Sqrt(list[item.idx]) * 1.5;
				}
			}
			return num;
		}
	}
}
