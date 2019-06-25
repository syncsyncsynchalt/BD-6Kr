using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_SupportHougeki : Exec_Hougeki
	{
		public Exec_SupportHougeki(BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, bool practice)
			: base(null, new int[2], myData, mySubInfo, enemyData, enemySubInfo, practice)
		{
			AIR_ATACK_KEISU = 15;
			valance1 = 4.0;
			valance2 = 64.0;
			valance3 = 1.0;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public override HougekiDayBattleFmt GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			return null;
		}

		public T GetResultData<T>(FormationDatas formation) where T : Support_HouRai, new()
		{
			formationData = formation;
			Support_HouRai hougekiData = getHougekiData();
			return (T)hougekiData;
		}

		protected override void setTargetingKind(FormationDatas formationDatas)
		{
			battleTargetKind = BattleTargetKind.Other;
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		private Support_HouRai getHougekiData()
		{
			BattleBaseData f_Data = F_Data;
			BattleBaseData e_Data = E_Data;
			List<Mem_ship> list = e_Data.ShipData.ToList();
			list.RemoveAll(delegate(Mem_ship x)
			{
				if (x.Nowhp <= 0)
				{
					return true;
				}
				return Mst_DataManager.Instance.Mst_stype[x.Stype].IsSubmarine() ? true : false;
			});
			if (list.Count() == 0)
			{
				return null;
			}
			Support_HouRai support_HouRai = new Support_HouRai();
			for (int i = 0; i < F_Data.ShipData.Count; i++)
			{
				Mem_ship mem_ship = f_Data.ShipData[i];
				List<Mst_slotitem> list2 = f_Data.SlotData[i];
				BattleAtackKinds_Day kind = BattleAtackKinds_Day.Normal;
				if (IsAirAttackGroup(mem_ship, list2, BattleCommand.None))
				{
					if (!CanAirAtack_DamageState(mem_ship) || !CanAirAttack(mem_ship, list2))
					{
						continue;
					}
					kind = BattleAtackKinds_Day.AirAttack;
				}
				else if (!isValidHougeki(mem_ship))
				{
					continue;
				}
				BattleDamageKinds dKind = BattleDamageKinds.Normal;
				Mem_ship atackTarget = getAtackTarget(mem_ship, list, overKill: true, subMarineFlag: false, rescueFlag: true, ref dKind);
				if (atackTarget != null)
				{
					int deckIdx = E_SubInfo[atackTarget.Rid].DeckIdx;
					int hougAttackValue = getHougAttackValue(kind, mem_ship, list2, atackTarget, 0);
					int soukou = atackTarget.Soukou;
					int hougHitProb = getHougHitProb(kind, mem_ship, list2, 0);
					int battleAvo = getBattleAvo(FormationDatas.GetFormationKinds.HOUGEKI, atackTarget);
					BattleHitStatus hitStatus = getHitStatus(hougHitProb, battleAvo, mem_ship, atackTarget, valance3, airAttack: false);
					int num = setDamageValue(hitStatus, hougAttackValue, soukou, mem_ship, atackTarget, e_Data.LostFlag);
					support_HouRai.Damage[deckIdx] += num;
					if (hitStatus != 0 && support_HouRai.Clitical[deckIdx] != BattleHitStatus.Clitical)
					{
						support_HouRai.Clitical[deckIdx] = hitStatus;
					}
					if (support_HouRai.DamageType[deckIdx] != BattleDamageKinds.Rescue)
					{
						support_HouRai.DamageType[deckIdx] = dKind;
					}
				}
			}
			return support_HouRai;
		}

		protected override int getHougAttackValue(BattleAtackKinds_Day kind, Mem_ship atk_ship, List<Mst_slotitem> atk_slot, Mem_ship def_ship, int tekkouKind)
		{
			int num = 150;
			int num2 = 0;
			int num3 = 0;
			foreach (var item in atk_slot.Select((Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				num2 += item.obj.Baku;
				num3 += item.obj.Raig;
			}
			double num4 = valance1 + (double)atk_ship.Houg;
			if (Mst_DataManager.Instance.Mst_stype[def_ship.Stype].IsLandFacillity(Mst_DataManager.Instance.Mst_ship[def_ship.Ship_id].Soku))
			{
				num4 *= getLandFacciilityKeisu(atk_slot);
				num3 = 0;
			}
			if (kind == BattleAtackKinds_Day.AirAttack)
			{
				int airAtackPow = getAirAtackPow(num2, num3);
				num4 += (double)airAtackPow;
				num4 = 25.0 + (double)(int)(num4 * 1.5);
			}
			BattleFormationKinds1 formation;
			BattleFormationKinds2 battleFormation;
			if (!atk_ship.IsEnemy())
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
			double formationParamBattle = formationData.GetFormationParamBattle(FormationDatas.GetFormationKinds.HOUGEKI, battleFormation);
			double formationParamF = formationData.GetFormationParamF1(FormationDatas.GetFormationKinds.HOUGEKI, formation);
			num4 = num4 * formationParamBattle * formationParamF;
			DamageState damageState = atk_ship.Get_DamageState();
			double num5 = 1.0;
			switch (damageState)
			{
			case DamageState.Tyuuha:
				num5 = 0.7;
				break;
			case DamageState.Taiha:
				num5 = 0.4;
				break;
			}
			num4 *= num5;
			if (num4 > (double)num)
			{
				num4 = (double)num + Math.Sqrt(num4 - (double)num);
			}
			return (int)num4;
		}

		protected override int getHougHitProb(BattleAtackKinds_Day kind, Mem_ship atk_ship, List<Mst_slotitem> atk_slot, int tekkouKind)
		{
			int num = 0;
			foreach (var item in atk_slot.Select((Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				num += item.obj.Houm;
			}
			double num2 = valance2 + Math.Sqrt((double)atk_ship.GetBattleBaseParam().Luck * 1.5) + Math.Sqrt(atk_ship.Level) * 2.0 + (double)num;
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
			double formationParamF = formationData.GetFormationParamF2(FormationDatas.GetFormationKinds.HOUGEKI, formation, formation2);
			num2 *= formationParamF;
			FatigueState fatigueState = atk_ship.Get_FatigueState();
			double num3 = 1.0;
			switch (fatigueState)
			{
			case FatigueState.Exaltation:
				num3 = 1.2;
				break;
			case FatigueState.Light:
				num3 = 0.8;
				break;
			case FatigueState.Distress:
				num3 = 0.5;
				break;
			}
			num2 *= num3;
			return (int)num2;
		}

		protected override int getAirAtackPow(int baku, int raig)
		{
			return base.getAirAtackPow(baku, raig);
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}
	}
}
