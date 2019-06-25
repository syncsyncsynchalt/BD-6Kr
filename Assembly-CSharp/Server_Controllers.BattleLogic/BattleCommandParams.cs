using Common.Enum;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class BattleCommandParams : IDisposable
	{
		private int fspp;

		private int tspp;

		private int rspp;

		private bool isEscape;

		private BattleBaseData userBaseData;

		private int escapeNum;

		private HashSet<BattleCommand> enableCommand;

		private HashSet<BattleCommand> enableCommandOpening;

		private Dictionary<BattleCommand, Func<int, DayBattleProductionFmt>> commandFunc;

		private bool highSpeedFlag;

		public int Fspp
		{
			get
			{
				return fspp;
			}
			private set
			{
				fspp = value;
				if (fspp > 100)
				{
					fspp = 100;
				}
				else if (fspp < -100)
				{
					fspp = -100;
				}
			}
		}

		public int Tspp
		{
			get
			{
				return tspp;
			}
			private set
			{
				tspp = value;
				if (tspp > 100)
				{
					tspp = 100;
				}
				else if (tspp < -100)
				{
					tspp = -100;
				}
			}
		}

		public int Rspp
		{
			get
			{
				return rspp;
			}
			private set
			{
				rspp = value;
				if (rspp > 100)
				{
					rspp = 100;
				}
				else if (rspp < -100)
				{
					rspp = -100;
				}
			}
		}

		public bool IsEscape
		{
			get
			{
				return isEscape;
			}
			private set
			{
				isEscape = value;
			}
		}

		public BattleCommandParams(BattleBaseData userData)
		{
			isEscape = false;
			Fspp = 0;
			Tspp = 0;
			Rspp = 0;
			escapeNum = 0;
			userBaseData = userData;
			IEnumerable<int> source = from x in userBaseData.ShipData
				select Mst_DataManager.Instance.Mst_ship[x.Ship_id].Soku;
			highSpeedFlag = ((!source.Any((int x) => x != 10)) ? true : false);
			enableCommand = new HashSet<BattleCommand>
			{
				BattleCommand.Sekkin,
				BattleCommand.Ridatu,
				BattleCommand.Kaihi,
				BattleCommand.Totugeki,
				BattleCommand.Tousha,
				BattleCommand.Raigeki
			};
			enableCommandOpening = new HashSet<BattleCommand>
			{
				BattleCommand.Sekkin,
				BattleCommand.Ridatu,
				BattleCommand.Kaihi
			};
			commandFunc = new Dictionary<BattleCommand, Func<int, DayBattleProductionFmt>>
			{
				{
					BattleCommand.Sekkin,
					Sekkin
				},
				{
					BattleCommand.Ridatu,
					Ridatsu
				},
				{
					BattleCommand.Kaihi,
					Kaihi
				},
				{
					BattleCommand.Totugeki,
					Totsugeki
				},
				{
					BattleCommand.Tousha,
					Tousha
				},
				{
					BattleCommand.Raigeki,
					RaigPosture
				}
			};
		}

		public void Dispose()
		{
			enableCommand.Clear();
			enableCommandOpening.Clear();
			commandFunc.Clear();
		}

		public bool IsOpenengProductionCommand(BattleCommand command)
		{
			return enableCommandOpening.Contains(command);
		}

		public DayBattleProductionFmt GetProductionData(BattleCommand command)
		{
			if (!IsOpenengProductionCommand(command))
			{
				return null;
			}
			return commandFunc[command](0);
		}

		public DayBattleProductionFmt GetProductionData(int boxPos, BattleCommand command)
		{
			if (!enableCommand.Contains(command))
			{
				return null;
			}
			return commandFunc[command](boxPos);
		}

		private DayBattleProductionFmt RaigPosture(int index)
		{
			DayBattleProductionFmt dayBattleProductionFmt = new DayBattleProductionFmt();
			dayBattleProductionFmt.productionKind = BattleCommand.Raigeki;
			dayBattleProductionFmt.FSPP = Fspp;
			dayBattleProductionFmt.RSPP = Rspp;
			dayBattleProductionFmt.BoxNo = index + 1;
			double max = Math.Sqrt((double)userBaseData.ShipData[0].Level / 3.0);
			int num = 10 + (int)Utils.GetRandDouble(0.0, max, 1.0, 1);
			Mem_ship mem_ship = userBaseData.ShipData[0];
			if (mem_ship.Lov >= 100)
			{
				double max2 = Math.Sqrt(mem_ship.Lov) / 3.0;
				double randDouble = Utils.GetRandDouble(0.0, max2, 0.1, 1);
				int num2 = (int)(randDouble + 0.5);
				num += num2;
			}
			if (highSpeedFlag)
			{
				num += 3;
			}
			Tspp += num;
			dayBattleProductionFmt.TSPP = Tspp;
			return dayBattleProductionFmt;
		}

		private DayBattleProductionFmt Ridatsu(int index)
		{
			escapeNum++;
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			bool flag = false;
			foreach (Mem_ship shipDatum in userBaseData.ShipData)
			{
				if (shipDatum.IsFight())
				{
					if (mst_ship[shipDatum.Ship_id].Soku != 10)
					{
						flag = false;
						break;
					}
					flag = true;
				}
			}
			DayBattleProductionFmt dayBattleProductionFmt = new DayBattleProductionFmt();
			dayBattleProductionFmt.productionKind = BattleCommand.Ridatu;
			dayBattleProductionFmt.FSPP = Fspp;
			dayBattleProductionFmt.RSPP = Rspp;
			dayBattleProductionFmt.TSPP = Tspp;
			dayBattleProductionFmt.BoxNo = index + 1;
			if (index == 0)
			{
				IsEscape = false;
				return dayBattleProductionFmt;
			}
			int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
			if (flag && escapeNum >= 2 && num <= 80)
			{
				dayBattleProductionFmt.Withdrawal = true;
				IsEscape = true;
				return dayBattleProductionFmt;
			}
			if (flag && escapeNum == 1 && num <= 50)
			{
				dayBattleProductionFmt.Withdrawal = true;
				IsEscape = true;
				return dayBattleProductionFmt;
			}
			if (!flag && escapeNum >= 2 && num <= 60)
			{
				dayBattleProductionFmt.Withdrawal = true;
				IsEscape = true;
				return dayBattleProductionFmt;
			}
			if (!flag && escapeNum == 1 && num <= 35)
			{
				dayBattleProductionFmt.Withdrawal = true;
				IsEscape = true;
				return dayBattleProductionFmt;
			}
			if (index >= 3)
			{
				dayBattleProductionFmt.Withdrawal = true;
				IsEscape = true;
				return dayBattleProductionFmt;
			}
			if (index == 2 && num <= 65)
			{
				dayBattleProductionFmt.Withdrawal = true;
				IsEscape = true;
				return dayBattleProductionFmt;
			}
			IsEscape = false;
			return dayBattleProductionFmt;
		}

		private DayBattleProductionFmt Sekkin(int index)
		{
			Mem_ship mem_ship = userBaseData.ShipData[0];
			double num = 5.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 5), 1.0, 1);
			double num2 = 7.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 3), 1.0, 1);
			double num3 = -5.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 10), 1.0, 1);
			if (highSpeedFlag)
			{
				num = num + 2.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 8), 1.0, 1);
				num2 = num2 + 1.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 8), 1.0, 1);
			}
			else
			{
				num = num + 1.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 12), 1.0, 1);
				num2 += Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 16), 1.0, 1);
			}
			Mem_ship mem_ship2 = userBaseData.ShipData[0];
			if (mem_ship2.Lov >= 100)
			{
				double randDouble = Utils.GetRandDouble(0.0, Math.Sqrt((double)mem_ship.Lov / 3.0), 0.1, 1);
				int num4 = (int)(randDouble + 0.5);
				num += (double)num4;
				double randDouble2 = Utils.GetRandDouble(0.0, Math.Sqrt((double)mem_ship.Lov / 5.0), 0.1, 1);
				int num5 = (int)(randDouble2 + 0.5);
				num2 += (double)num5;
			}
			Fspp += (int)num;
			Tspp += (int)num2;
			Rspp += (int)num3;
			DayBattleProductionFmt dayBattleProductionFmt = new DayBattleProductionFmt();
			dayBattleProductionFmt.productionKind = BattleCommand.Sekkin;
			dayBattleProductionFmt.FSPP = Fspp;
			dayBattleProductionFmt.RSPP = Rspp;
			dayBattleProductionFmt.TSPP = Tspp;
			dayBattleProductionFmt.BoxNo = index + 1;
			return dayBattleProductionFmt;
		}

		private DayBattleProductionFmt Kaihi(int index)
		{
			Mem_ship mem_ship = userBaseData.ShipData[0];
			double num = -4.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 20), 1.0, 1);
			double num2 = -6.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 15), 1.0, 1);
			double num3 = 5.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 10), 1.0, 1);
			num3 = ((!highSpeedFlag) ? (num3 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 25), 1.0, 1)) : (num3 + 1.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 20), 1.0, 1)));
			Fspp += (int)num;
			Tspp += (int)num2;
			Rspp += (int)num3;
			DayBattleProductionFmt dayBattleProductionFmt = new DayBattleProductionFmt();
			dayBattleProductionFmt.productionKind = BattleCommand.Kaihi;
			dayBattleProductionFmt.FSPP = Fspp;
			dayBattleProductionFmt.RSPP = Rspp;
			dayBattleProductionFmt.TSPP = Tspp;
			dayBattleProductionFmt.BoxNo = index + 1;
			return dayBattleProductionFmt;
		}

		private DayBattleProductionFmt Totsugeki(int index)
		{
			Mem_ship mem_ship = userBaseData.ShipData[0];
			double num = 3.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 5), 1.0, 1);
			double num2 = 5.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 3), 1.0, 1);
			double num3 = -7.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 8), 1.0, 1);
			if (highSpeedFlag)
			{
				num = num + 2.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 8), 1.0, 1);
				num2 = num2 + 1.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 7), 1.0, 1);
			}
			else
			{
				num = num + 1.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 12), 1.0, 1);
				num2 += Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 15), 1.0, 1);
			}
			Mem_ship mem_ship2 = userBaseData.ShipData[0];
			if (mem_ship2.Lov >= 100)
			{
				double randDouble = Utils.GetRandDouble(0.0, Math.Sqrt((double)mem_ship.Lov / 4.0), 0.1, 1);
				int num4 = (int)(randDouble + 0.5);
				num += (double)num4;
				double randDouble2 = Utils.GetRandDouble(0.0, Math.Sqrt((double)mem_ship.Lov / 8.0), 0.1, 1);
				int num5 = (int)(randDouble2 + 0.5);
				num2 += (double)num5;
			}
			Fspp += (int)num;
			Tspp += (int)num2;
			Rspp += (int)num3;
			DayBattleProductionFmt dayBattleProductionFmt = new DayBattleProductionFmt();
			dayBattleProductionFmt.productionKind = BattleCommand.Totugeki;
			dayBattleProductionFmt.FSPP = Fspp;
			dayBattleProductionFmt.RSPP = Rspp;
			dayBattleProductionFmt.TSPP = Tspp;
			dayBattleProductionFmt.BoxNo = index + 1;
			return dayBattleProductionFmt;
		}

		private DayBattleProductionFmt Tousha(int index)
		{
			List<Mst_slotitem> source = userBaseData.SlotData[0];
			double num = 2.0;
			double num2 = 0.0;
			double num3 = -2.0;
			if (source.Any((Mst_slotitem x) => (x.Api_mapbattle_type3 == 12 || x.Api_mapbattle_type3 == 13) && x.Tyku == 0))
			{
				num = num + 2.0 + Utils.GetRandDouble(0.0, Math.Sqrt(userBaseData.ShipData[0].Level / 10), 1.0, 1);
			}
			List<List<Mst_slotitem>> source2 = userBaseData.SlotData.Skip(1).ToList();
			foreach (var item in source2.Select((List<Mst_slotitem> obj, int ship_idx) => new
			{
				obj,
				ship_idx
			}))
			{
				Mem_ship mem_ship = userBaseData.ShipData[item.ship_idx + 1];
				if (mem_ship.IsFight())
				{
					num = num + 1.0 + Utils.GetRandDouble(0.0, Math.Sqrt(mem_ship.Level / 30), 1.0, 1);
				}
			}
			Fspp += (int)num;
			Tspp += (int)num2;
			Rspp += (int)num3;
			DayBattleProductionFmt dayBattleProductionFmt = new DayBattleProductionFmt();
			dayBattleProductionFmt.productionKind = BattleCommand.Tousha;
			dayBattleProductionFmt.FSPP = Fspp;
			dayBattleProductionFmt.RSPP = Rspp;
			dayBattleProductionFmt.TSPP = Tspp;
			dayBattleProductionFmt.BoxNo = index + 1;
			return dayBattleProductionFmt;
		}
	}
}
