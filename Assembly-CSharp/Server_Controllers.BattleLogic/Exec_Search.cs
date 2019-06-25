using Common.Enum;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_Search : BattleLogicBase<SearchInfo[]>
	{
		private BattleBaseData _f_Data;

		private BattleBaseData _e_Data;

		private Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		private Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		protected SakutekiInfo f_Param;

		protected SakutekiInfo e_Param;

		protected int valance1;

		protected Dictionary<int, int> valanceShipCount;

		public override BattleBaseData F_Data => _f_Data;

		public override BattleBaseData E_Data => _e_Data;

		public override Dictionary<int, BattleShipSubInfo> F_SubInfo => _f_SubInfo;

		public override Dictionary<int, BattleShipSubInfo> E_SubInfo => _e_SubInfo;

		public Exec_Search(BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, bool practice)
		{
			_f_Data = myData;
			_e_Data = enemyData;
			_f_SubInfo = mySubInfo;
			_e_SubInfo = enemySubInfo;
			practiceFlag = practice;
			f_Param = new SakutekiInfo(practiceFlag);
			e_Param = new SakutekiInfo(practiceFlag);
			valanceShipCount = new Dictionary<int, int>
			{
				{
					1,
					0
				},
				{
					2,
					0
				},
				{
					3,
					1
				},
				{
					4,
					2
				},
				{
					5,
					3
				},
				{
					6,
					4
				}
			};
			valance1 = 20;
		}

		public override void Dispose()
		{
			randInstance = null;
			f_Param.Dispose();
			e_Param.Dispose();
		}

		public override SearchInfo[] GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			SearchInfo searchInfo = new SearchInfo();
			SearchInfo searchInfo2 = new SearchInfo();
			SearchInfo[] result = new SearchInfo[2]
			{
				searchInfo,
				searchInfo2
			};
			f_Param.SetParametar(F_Data, out searchInfo.UsePlane);
			e_Param.SetParametar(E_Data, out searchInfo2.UsePlane);
			searchInfo.SearchValue = execSearch(f_Param, e_Param);
			searchInfo2.SearchValue = ((!practiceFlag) ? BattleSearchValues.Success : execSearch(e_Param, f_Param));
			if (searchInfo.SearchValue == BattleSearchValues.Success_Lost || searchInfo.SearchValue == BattleSearchValues.Lost)
			{
				subSlotExp(f_Param, searchInfo.SearchValue);
				subOnslot(f_Param);
			}
			if (searchInfo2.SearchValue == BattleSearchValues.Success_Lost || searchInfo2.SearchValue == BattleSearchValues.Lost)
			{
				subOnslot(e_Param);
			}
			return result;
		}

		protected BattleSearchValues execSearch(SakutekiInfo atk_info, SakutekiInfo def_info)
		{
			double randDouble = Utils.GetRandDouble(1.0, 1.4, 0.1, 1);
			int key = atk_info.LostTargetOnslot.Count();
			int num = atk_info.BasePow + valanceShipCount[key] - valance1 + (int)Math.Sqrt((double)atk_info.Attack * 10.0);
			int num2 = atk_info.Attack - (int)((double)def_info.Def * randDouble);
			int num3 = randInstance.Next(20);
			if (atk_info.Attack == 0)
			{
				return (num <= num3) ? BattleSearchValues.NotFound : BattleSearchValues.Found;
			}
			if (num > num3)
			{
				return (num2 > 0) ? BattleSearchValues.Success : BattleSearchValues.Success_Lost;
			}
			return (num2 <= 0) ? BattleSearchValues.Faile : BattleSearchValues.Lost;
		}

		protected void subOnslot(SakutekiInfo targetInfo)
		{
			foreach (KeyValuePair<Mem_ship, List<int>> item in targetInfo.LostTargetOnslot)
			{
				Mem_ship key = item.Key;
				foreach (int item2 in item.Value)
				{
					int num = randInstance.Next(3);
					if (key.Onslot[item2] < num)
					{
						num = key.Onslot[item2];
					}
					List<int> onslot;
					List<int> list = onslot = key.Onslot;
					int index;
					int index2 = index = item2;
					index = onslot[index];
					list[index2] = index - num;
				}
			}
		}

		protected void subSlotExp(SakutekiInfo targetInfo, BattleSearchValues serchKind)
		{
			if (serchKind == BattleSearchValues.Lost || serchKind == BattleSearchValues.Success_Lost)
			{
				Dictionary<int, int[]> slotExperience = F_Data.SlotExperience;
				foreach (KeyValuePair<Mem_ship, List<int>> item in targetInfo.LostTargetOnslot)
				{
					Mem_ship key = item.Key;
					foreach (int item2 in item.Value)
					{
						if (key.Onslot[item2] > 0)
						{
							int key2 = key.Slot[item2];
							double num = (serchKind != BattleSearchValues.Lost) ? 6.0 : 12.0;
							double randDouble = Utils.GetRandDouble(0.0, num - 1.0, 1.0, 1);
							int num2 = slotExperience[key2][0];
							int num3 = num2 - (int)(num * 0.5 + randDouble * 0.05);
							if (num3 < 0)
							{
								num3 = 0;
							}
							int num4 = num3 - num2;
							slotExperience[key2][1] = slotExperience[key2][1] + num4;
						}
					}
				}
			}
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}
	}
}
