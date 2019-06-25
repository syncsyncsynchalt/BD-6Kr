using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Models;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_OpeningAtack : Exec_Raigeki
	{
		private int raigType;

		public Exec_OpeningAtack(BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, bool practice)
			: base(3, myData, mySubInfo, enemyData, enemySubInfo, practice)
		{
		}

		public int getUserAttackShipNum()
		{
			return f_AtkIdxs.Count;
		}

		public int CanRaigType()
		{
			return raigType;
		}

		public override Raigeki GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			return base.GetResultData(formation, cParam);
		}

		protected override void setHitHoseiFromBattleCommand()
		{
		}

		protected override void makeAttackerData(bool enemyFlag)
		{
			List<int> list;
			BattleBaseData battleBaseData;
			List<int> list2;
			bool flag;
			if (!enemyFlag)
			{
				list = f_AtkIdxs;
				battleBaseData = F_Data;
				list2 = f_startHp;
				flag = isRaigBattleCommand();
			}
			else
			{
				list = e_AtkIdxs;
				battleBaseData = E_Data;
				list2 = e_startHp;
				flag = true;
			}
			if (flag)
			{
				foreach (var item in battleBaseData.ShipData.Select((Mem_ship obj, int idx) => new
				{
					obj,
					idx
				}))
				{
					list2.Add(item.obj.Nowhp);
					if (isValidRaigeki(item.obj, battleBaseData.SlotData[item.idx]))
					{
						list.Add(item.idx);
					}
				}
			}
		}

		protected override bool isRaigBattleCommand()
		{
			List<BattleCommand> deckBattleCommand = F_Data.GetDeckBattleCommand();
			IEnumerable<BattleCommand> source = deckBattleCommand.Take(deckBattleCommand.Count - 1);
			bool flag = source.Any((BattleCommand x) => x == BattleCommand.Raigeki);
			if (flag && deckBattleCommand[0] == BattleCommand.Raigeki)
			{
				raigType = 1;
			}
			else if (flag)
			{
				raigType = 2;
			}
			return flag;
		}

		private bool isValidRaigeki(Mem_ship ship, List<Mst_slotitem> slotitems)
		{
			if (ship.Get_DamageState() > DamageState.Shouha || !ship.IsFight())
			{
				return false;
			}
			if (ship.GetBattleBaseParam().Raig <= 0)
			{
				return false;
			}
			if (!ship.IsEnemy() || practiceFlag)
			{
				if (ship.Level >= 10 && Mst_DataManager.Instance.Mst_stype[ship.Stype].IsSubmarine())
				{
					return true;
				}
				if (slotitems == null || slotitems.Count == 0)
				{
					return false;
				}
				Mst_slotitem item = Mst_DataManager.Instance.Mst_Slotitem[41];
				if (slotitems.Contains(item))
				{
					return true;
				}
				return false;
			}
			if (Mst_DataManager.Instance.Mst_stype[ship.Stype].IsSubmarine())
			{
				Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[ship.Ship_id];
				return (mst_ship.Yomi.Equals("flagship") || mst_ship.Yomi.Equals("elite")) ? true : false;
			}
			if (slotitems == null || slotitems.Count == 0)
			{
				return false;
			}
			Mst_slotitem item2 = Mst_DataManager.Instance.Mst_Slotitem[541];
			if (slotitems.Contains(item2))
			{
				return true;
			}
			return false;
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return base.getAvoHosei(target);
		}
	}
}
