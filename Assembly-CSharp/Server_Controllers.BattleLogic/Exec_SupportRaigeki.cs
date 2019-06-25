using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_SupportRaigeki : Exec_Raigeki
	{
		public Exec_SupportRaigeki(BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, bool practice)
			: base(1, myData, mySubInfo, enemyData, enemySubInfo, practice)
		{
			valance1 = 8.0;
			valance2 = 54.0;
			valance3 = 1.2;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Raigeki GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			return null;
		}

		public T GetResultData<T>(FormationDatas formation) where T : Support_HouRai
		{
			formationData = formation;
			Support_HouRai raigekiData = getRaigekiData();
			return (T)raigekiData;
		}

		protected override void setHitHoseiFromBattleCommand()
		{
		}

		protected override void setTargetingKind(FormationDatas formationDatas)
		{
			battleTargetKind = BattleTargetKind.Other;
		}

		protected override void makeAttackerData(bool enemyFlag)
		{
			base.makeAttackerData(enemyFlag);
		}

		protected override bool isRaigBattleCommand()
		{
			return true;
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		private Support_HouRai getRaigekiData()
		{
			BattleBaseData f_Data = F_Data;
			BattleBaseData e_Data = E_Data;
			List<Mem_ship> list = E_Data.ShipData.ToList();
			list.RemoveAll(delegate(Mem_ship x)
			{
				if (x.Nowhp <= 0)
				{
					return true;
				}
				return Mst_DataManager.Instance.Mst_stype[x.Stype].IsLandFacillity(Mst_DataManager.Instance.Mst_ship[x.Ship_id].Soku) ? true : false;
			});
			if (list.Count() == 0)
			{
				return null;
			}
			Support_HouRai support_HouRai = new Support_HouRai();
			foreach (int f_AtkIdx in f_AtkIdxs)
			{
				Mem_ship mem_ship = f_Data.ShipData[f_AtkIdx];
				List<Mst_slotitem> atk_slot = f_Data.SlotData[f_AtkIdx];
				BattleDamageKinds dKind = BattleDamageKinds.Normal;
				Mem_ship atackTarget = getAtackTarget(mem_ship, list, overKill: true, subMarineFlag: false, rescueFlag: true, ref dKind);
				if (atackTarget != null)
				{
					int num = e_Data.ShipData.IndexOf(atackTarget);
					int raigAttackValue = getRaigAttackValue(mem_ship, atk_slot, atackTarget);
					int soukou = atackTarget.Soukou;
					int raigHitProb = getRaigHitProb(mem_ship, atk_slot, raigAttackValue);
					int battleAvo = getBattleAvo(FormationDatas.GetFormationKinds.RAIGEKI, atackTarget);
					BattleHitStatus hitStatus = getHitStatus(raigHitProb, battleAvo, mem_ship, atackTarget, valance3, airAttack: false);
					int num2 = setDamageValue(hitStatus, raigAttackValue, soukou, mem_ship, atackTarget, null);
					support_HouRai.Damage[num] += num2;
					if (hitStatus != 0 && support_HouRai.Clitical[num] != BattleHitStatus.Clitical)
					{
						support_HouRai.Clitical[num] = hitStatus;
					}
					if (support_HouRai.DamageType[num] != BattleDamageKinds.Rescue)
					{
						support_HouRai.DamageType[num] = dKind;
					}
				}
			}
			return support_HouRai;
		}

		protected override int getRaigAttackValue(Mem_ship atk_ship, List<Mst_slotitem> atk_slot, Mem_ship def_ship)
		{
			int num = 150;
			double num2 = valance1 + (double)atk_ship.Raig;
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
			double formationParamBattle = formationData.GetFormationParamBattle(FormationDatas.GetFormationKinds.RAIGEKI, battleFormation);
			double formationParamF = formationData.GetFormationParamF1(FormationDatas.GetFormationKinds.RAIGEKI, formation);
			num2 = num2 * formationParamBattle * formationParamF;
			DamageState damageState = atk_ship.Get_DamageState();
			double num3 = 1.0;
			switch (damageState)
			{
			case DamageState.Tyuuha:
				num3 = 0.8;
				break;
			case DamageState.Taiha:
				num3 = 0.0;
				break;
			}
			num2 *= num3;
			if (num2 > (double)num)
			{
				num2 = (double)num + Math.Sqrt(num2 - (double)num);
			}
			return (int)num2;
		}

		protected override int getRaigHitProb(Mem_ship atk_ship, List<Mst_slotitem> atk_slot, int atk_pow)
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
			double num2 = Math.Sqrt((double)atk_ship.GetBattleBaseParam().Luck * 1.5) + Math.Sqrt(atk_ship.Level) * 2.0 + (double)num;
			num2 += (double)(int)((double)atk_pow * 0.35);
			num2 = valance2 + num2;
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
			double formationParamF = formationData.GetFormationParamF2(FormationDatas.GetFormationKinds.RAIGEKI, formation, formation2);
			num2 *= formationParamF;
			FatigueState fatigueState = atk_ship.Get_FatigueState();
			double num3 = 1.0;
			switch (fatigueState)
			{
			case FatigueState.Exaltation:
				num3 = 1.3;
				break;
			case FatigueState.Light:
				num3 = 0.7;
				break;
			case FatigueState.Distress:
				num3 = 0.35;
				break;
			}
			num2 *= num3;
			return (int)num2;
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}
	}
}
