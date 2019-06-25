using Server_Common;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class SakutekiInfo : IDisposable
	{
		public int BasePow;

		public int Attack;

		public int Def;

		public Dictionary<Mem_ship, List<int>> LostTargetOnslot;

		private bool practiceFlag;

		public SakutekiInfo(bool practice)
		{
			BasePow = 0;
			Attack = 0;
			Def = 0;
			LostTargetOnslot = new Dictionary<Mem_ship, List<int>>();
			practiceFlag = practice;
		}

		public void Dispose()
		{
			LostTargetOnslot.Clear();
			LostTargetOnslot = null;
		}

		public void SetParametar(BattleBaseData baseData, out List<SearchUsePlane> planes)
		{
			List<Mem_ship> shipData = baseData.ShipData;
			List<List<Mst_slotitem>> slotData = baseData.SlotData;
			Dictionary<int, int[]> slotExperience = baseData.SlotExperience;
			List<double> list = new List<double>();
			list.Add(2.0);
			list.Add(5.0);
			list.Add(8.0);
			list.Add(8.0);
			list.Add(8.0);
			list.Add(8.0);
			List<double> list2 = list;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			planes = new List<SearchUsePlane>();
			foreach (var item in shipData.Select((Mem_ship obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				Mem_ship obj2 = item.obj;
				if (obj2.IsFight())
				{
					LostTargetOnslot.Add(obj2, new List<int>());
					num += (int)((double)obj2.Sakuteki / list2[item.idx]);
					if (Mst_DataManager.Instance.Mst_stype[obj2.Stype].IsMother())
					{
						num4++;
					}
					List<int> list3 = new List<int>();
					foreach (var item2 in slotData[item.idx].Select((Mst_slotitem obj, int idx) => new
					{
						obj,
						idx
					}))
					{
						Mst_slotitem obj3 = item2.obj;
						int num5 = obj2.Onslot[item2.idx];
						if (num5 > 0)
						{
							if (obj3.Api_mapbattle_type3 == 9 || obj3.Api_mapbattle_type3 == 10 || obj3.Api_mapbattle_type3 == 11 || obj3.Api_mapbattle_type3 == 41)
							{
								int num6 = 0;
								int[] value = null;
								if (slotExperience.TryGetValue(obj2.Slot[item2.idx], out value))
								{
									int slotExpUpValue = getSlotExpUpValue(num5, value[0], obj3.Exp_rate);
									value[1] += slotExpUpValue;
									num6 = getAlvPlus(num5, value[0]);
								}
								num2 += num5 + num6;
								LostTargetOnslot[obj2].Add(item2.idx);
								list3.Add(obj3.Id);
							}
							else if (obj3.Api_mapbattle_type3 == 6)
							{
								num3 += num5;
							}
						}
					}
					if (list3.Count > 0)
					{
						planes.Add(new SearchUsePlane(obj2.Rid, list3));
					}
				}
			}
			if (num4 == 1)
			{
				num2 += 30;
			}
			else if (num4 > 1)
			{
				int num7 = (num4 - 1) * 10;
				num2 = num2 + 30 + num7;
			}
			BasePow = num;
			Attack = num2;
			if (num3 == 0)
			{
				Def = 0;
			}
			else if (num3 <= 30)
			{
				Def = (int)(1.0 + (double)num3 / 9.0);
			}
			else if (num3 <= 120)
			{
				Def = (int)(2.0 + (double)num3 / 20.0);
			}
			else if (num3 > 120)
			{
				Def = (int)(6.0 + (double)(num3 - 120) / 40.0);
			}
		}

		private int getSlotExpUpValue(int onslotNum, int slotExp, int expUpRate)
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

		private int getAlvPlus(int onslotNum, int slotExp)
		{
			if (onslotNum <= 0)
			{
				return 0;
			}
			double num = Math.Sqrt((double)slotExp * 0.2);
			if (num >= 25.0)
			{
				num += 5.0;
			}
			if (num >= 55.0)
			{
				num += 10.0;
			}
			if (num >= 100.0)
			{
				num += 15.0;
			}
			return (int)num;
		}
	}
}
