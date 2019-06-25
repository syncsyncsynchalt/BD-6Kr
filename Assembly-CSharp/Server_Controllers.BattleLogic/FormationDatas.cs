using Common.Enum;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class FormationDatas : IDisposable
	{
		public enum GetFormationKinds
		{
			AIR,
			HOUGEKI,
			RAIGEKI,
			MIDNIGHT,
			SUBMARINE
		}

		private BattleFormationKinds1 f_Formation;

		private BattleFormationKinds1 e_Formation;

		private BattleFormationKinds2 battleFormation;

		private Dictionary<GetFormationKinds, Dictionary<BattleFormationKinds1, double>> paramF1;

		private Dictionary<GetFormationKinds, Dictionary<BattleFormationKinds1, double>> paramF2;

		private Dictionary<GetFormationKinds, Dictionary<BattleFormationKinds1, double>> paramF3;

		private Dictionary<GetFormationKinds, Dictionary<BattleFormationKinds2, double>> paramBattleFormattion;

		public BattleFormationKinds1 F_Formation
		{
			get
			{
				return f_Formation;
			}
			private set
			{
				f_Formation = value;
			}
		}

		public BattleFormationKinds1 E_Formation
		{
			get
			{
				return e_Formation;
			}
			private set
			{
				e_Formation = value;
			}
		}

		public BattleFormationKinds2 BattleFormation
		{
			get
			{
				return battleFormation;
			}
			private set
			{
				battleFormation = value;
			}
		}

		public FormationDatas(BattleBaseData fBaseData, BattleBaseData eBaseData, bool practiceFlag)
		{
			F_Formation = fBaseData.Formation;
			E_Formation = ((!practiceFlag) ? eBaseData.Formation : selectEnemyFormation(eBaseData.ShipData.Count));
			BattleFormation = selectBattleFormation2();
			setParamater();
		}

		public void Dispose()
		{
		}

		public BattleFormationKinds2 AfterAirBattle_RewriteBattleFormation2(BattleBaseData fBaseData)
		{
			if (BattleFormation == BattleFormationKinds2.T_Enemy)
			{
				foreach (var item in fBaseData.SlotData.Select((List<Mst_slotitem> list, int ship_idx) => new
				{
					list,
					ship_idx
				}))
				{
					Mem_ship mem_ship = fBaseData.ShipData[item.ship_idx];
					foreach (var item2 in item.list.Select((Mst_slotitem obj, int slot_idx) => new
					{
						obj,
						slot_idx
					}))
					{
						if (mem_ship.Onslot[item2.slot_idx] > 0 && item2.obj.Id == 54)
						{
							BattleFormation = BattleFormationKinds2.Hankou;
							return BattleFormation;
						}
					}
				}
			}
			return BattleFormation;
		}

		public double GetFormationParamF1(GetFormationKinds kind, BattleFormationKinds1 attacker)
		{
			return paramF1[kind][attacker];
		}

		public double GetFormationParamF2(GetFormationKinds kind, BattleFormationKinds1 attacker, BattleFormationKinds1 defencer)
		{
			switch (kind)
			{
			case GetFormationKinds.HOUGEKI:
				if (attacker == BattleFormationKinds1.FukuJuu && defencer == BattleFormationKinds1.TanOu)
				{
					return 1.2;
				}
				if (attacker == BattleFormationKinds1.Teikei && defencer == BattleFormationKinds1.TanJuu)
				{
					return 1.2;
				}
				if (attacker == BattleFormationKinds1.TanOu && defencer == BattleFormationKinds1.Teikei)
				{
					return 1.2;
				}
				break;
			case GetFormationKinds.SUBMARINE:
				if (attacker == BattleFormationKinds1.FukuJuu && defencer == BattleFormationKinds1.TanOu)
				{
					return 1.2;
				}
				if (attacker == BattleFormationKinds1.Teikei && defencer == BattleFormationKinds1.TanJuu)
				{
					return 1.2;
				}
				if (attacker == BattleFormationKinds1.TanOu && defencer == BattleFormationKinds1.Teikei)
				{
					return 1.2;
				}
				break;
			}
			return paramF2[kind][attacker];
		}

		public double GetFormationParamF3(GetFormationKinds kind, BattleFormationKinds1 attacker)
		{
			return paramF3[kind][attacker];
		}

		public double GetFormationParamBattle(GetFormationKinds kind, BattleFormationKinds2 formation)
		{
			return paramBattleFormattion[kind][formation];
		}

		private BattleFormationKinds1 selectEnemyFormation(int enemyCount)
		{
			Dictionary<int, List<BattleFormationKinds1>> dictionary = new Dictionary<int, List<BattleFormationKinds1>>();
			dictionary.Add(1, new List<BattleFormationKinds1>
			{
				BattleFormationKinds1.TanJuu
			});
			dictionary.Add(2, new List<BattleFormationKinds1>
			{
				BattleFormationKinds1.TanJuu,
				BattleFormationKinds1.Teikei
			});
			dictionary.Add(3, new List<BattleFormationKinds1>
			{
				BattleFormationKinds1.TanJuu,
				BattleFormationKinds1.Teikei,
				BattleFormationKinds1.Teikei
			});
			dictionary.Add(4, new List<BattleFormationKinds1>
			{
				BattleFormationKinds1.TanJuu,
				BattleFormationKinds1.Teikei,
				BattleFormationKinds1.TanOu,
				BattleFormationKinds1.Rinkei
			});
			Dictionary<int, List<BattleFormationKinds1>> dictionary2 = dictionary;
			int key = (enemyCount <= 4) ? enemyCount : 4;
			if (dictionary2[key].Count == 1)
			{
				return dictionary2[key][0];
			}
			List<BattleFormationKinds1> source = dictionary2[key];
			var anon = (from value in source
				select new
				{
					value
				} into x
				orderby Guid.NewGuid()
				select x).First();
			return anon.value;
		}

		private BattleFormationKinds2 selectBattleFormation2()
		{
			List<BattleFormationKinds2> list = new List<BattleFormationKinds2>();
			Dictionary<BattleFormationKinds2, int> dictionary = new Dictionary<BattleFormationKinds2, int>();
			dictionary.Add(BattleFormationKinds2.T_Enemy, 10);
			dictionary.Add(BattleFormationKinds2.T_Own, 15);
			dictionary.Add(BattleFormationKinds2.Hankou, 30);
			dictionary.Add(BattleFormationKinds2.Doukou, 45);
			Dictionary<BattleFormationKinds2, int> dictionary2 = dictionary;
			foreach (KeyValuePair<BattleFormationKinds2, int> item in dictionary2)
			{
				BattleFormationKinds2 key = item.Key;
				for (int i = 0; i < item.Value; i++)
				{
					list.Add(key);
				}
			}
			var anon = (from value in list
				select new
				{
					value
				} into x
				orderby Guid.NewGuid()
				select x).First();
			return anon.value;
		}

		private void setParamater()
		{
			Dictionary<GetFormationKinds, Dictionary<BattleFormationKinds1, double>> dictionary = new Dictionary<GetFormationKinds, Dictionary<BattleFormationKinds1, double>>();
			Dictionary<GetFormationKinds, Dictionary<BattleFormationKinds1, double>> dictionary2 = new Dictionary<GetFormationKinds, Dictionary<BattleFormationKinds1, double>>();
			Dictionary<GetFormationKinds, Dictionary<BattleFormationKinds1, double>> dictionary3 = new Dictionary<GetFormationKinds, Dictionary<BattleFormationKinds1, double>>();
			Dictionary<GetFormationKinds, Dictionary<BattleFormationKinds2, double>> dictionary4 = new Dictionary<GetFormationKinds, Dictionary<BattleFormationKinds2, double>>();
			foreach (object value in Enum.GetValues(typeof(GetFormationKinds)))
			{
				GetFormationKinds key = (GetFormationKinds)(int)value;
				dictionary.Add(key, new Dictionary<BattleFormationKinds1, double>());
				dictionary2.Add(key, new Dictionary<BattleFormationKinds1, double>());
				dictionary3.Add(key, new Dictionary<BattleFormationKinds1, double>());
				dictionary4.Add(key, new Dictionary<BattleFormationKinds2, double>());
				foreach (object value2 in Enum.GetValues(typeof(BattleFormationKinds1)))
				{
					BattleFormationKinds1 key2 = (BattleFormationKinds1)(int)value2;
					dictionary[key].Add(key2, 1.0);
					dictionary2[key].Add(key2, 1.0);
					dictionary3[key].Add(key2, 1.0);
				}
				foreach (object value3 in Enum.GetValues(typeof(BattleFormationKinds2)))
				{
					BattleFormationKinds2 key3 = (BattleFormationKinds2)(int)value3;
					dictionary4[key].Add(key3, 1.0);
				}
			}
			dictionary[GetFormationKinds.HOUGEKI][BattleFormationKinds1.TanJuu] = 1.0;
			dictionary[GetFormationKinds.HOUGEKI][BattleFormationKinds1.FukuJuu] = 0.8;
			dictionary[GetFormationKinds.HOUGEKI][BattleFormationKinds1.Rinkei] = 0.7;
			dictionary[GetFormationKinds.HOUGEKI][BattleFormationKinds1.Teikei] = 0.6;
			dictionary[GetFormationKinds.HOUGEKI][BattleFormationKinds1.TanOu] = 0.6;
			dictionary[GetFormationKinds.RAIGEKI][BattleFormationKinds1.TanJuu] = 1.0;
			dictionary[GetFormationKinds.RAIGEKI][BattleFormationKinds1.FukuJuu] = 0.8;
			dictionary[GetFormationKinds.RAIGEKI][BattleFormationKinds1.Rinkei] = 0.7;
			dictionary[GetFormationKinds.RAIGEKI][BattleFormationKinds1.Teikei] = 0.6;
			dictionary[GetFormationKinds.RAIGEKI][BattleFormationKinds1.TanOu] = 0.6;
			dictionary[GetFormationKinds.MIDNIGHT][BattleFormationKinds1.TanJuu] = 1.0;
			dictionary[GetFormationKinds.MIDNIGHT][BattleFormationKinds1.FukuJuu] = 1.0;
			dictionary[GetFormationKinds.MIDNIGHT][BattleFormationKinds1.Rinkei] = 1.0;
			dictionary[GetFormationKinds.MIDNIGHT][BattleFormationKinds1.Teikei] = 1.0;
			dictionary[GetFormationKinds.MIDNIGHT][BattleFormationKinds1.TanOu] = 1.0;
			dictionary[GetFormationKinds.AIR][BattleFormationKinds1.TanJuu] = 1.0;
			dictionary[GetFormationKinds.AIR][BattleFormationKinds1.FukuJuu] = 1.0;
			dictionary[GetFormationKinds.AIR][BattleFormationKinds1.Rinkei] = 1.0;
			dictionary[GetFormationKinds.AIR][BattleFormationKinds1.Teikei] = 1.0;
			dictionary[GetFormationKinds.AIR][BattleFormationKinds1.TanOu] = 1.0;
			dictionary[GetFormationKinds.SUBMARINE][BattleFormationKinds1.TanJuu] = 0.6;
			dictionary[GetFormationKinds.SUBMARINE][BattleFormationKinds1.FukuJuu] = 0.8;
			dictionary[GetFormationKinds.SUBMARINE][BattleFormationKinds1.Rinkei] = 1.2;
			dictionary[GetFormationKinds.SUBMARINE][BattleFormationKinds1.Teikei] = 1.0;
			dictionary[GetFormationKinds.SUBMARINE][BattleFormationKinds1.TanOu] = 1.3;
			dictionary2[GetFormationKinds.HOUGEKI][BattleFormationKinds1.TanJuu] = 1.0;
			dictionary2[GetFormationKinds.HOUGEKI][BattleFormationKinds1.FukuJuu] = 1.2;
			dictionary2[GetFormationKinds.HOUGEKI][BattleFormationKinds1.Rinkei] = 1.0;
			dictionary2[GetFormationKinds.HOUGEKI][BattleFormationKinds1.Teikei] = 1.2;
			dictionary2[GetFormationKinds.HOUGEKI][BattleFormationKinds1.TanOu] = 1.2;
			dictionary2[GetFormationKinds.RAIGEKI][BattleFormationKinds1.TanJuu] = 1.0;
			dictionary2[GetFormationKinds.RAIGEKI][BattleFormationKinds1.FukuJuu] = 0.8;
			dictionary2[GetFormationKinds.RAIGEKI][BattleFormationKinds1.Rinkei] = 0.4;
			dictionary2[GetFormationKinds.RAIGEKI][BattleFormationKinds1.Teikei] = 0.6;
			dictionary2[GetFormationKinds.RAIGEKI][BattleFormationKinds1.TanOu] = 0.3;
			dictionary2[GetFormationKinds.MIDNIGHT][BattleFormationKinds1.TanJuu] = 1.0;
			dictionary2[GetFormationKinds.MIDNIGHT][BattleFormationKinds1.FukuJuu] = 0.9;
			dictionary2[GetFormationKinds.MIDNIGHT][BattleFormationKinds1.Rinkei] = 0.7;
			dictionary2[GetFormationKinds.MIDNIGHT][BattleFormationKinds1.Teikei] = 0.8;
			dictionary2[GetFormationKinds.MIDNIGHT][BattleFormationKinds1.TanOu] = 0.8;
			dictionary2[GetFormationKinds.AIR][BattleFormationKinds1.TanJuu] = 1.0;
			dictionary2[GetFormationKinds.AIR][BattleFormationKinds1.FukuJuu] = 0.9;
			dictionary2[GetFormationKinds.AIR][BattleFormationKinds1.Rinkei] = 0.7;
			dictionary2[GetFormationKinds.AIR][BattleFormationKinds1.Teikei] = 0.8;
			dictionary2[GetFormationKinds.AIR][BattleFormationKinds1.TanOu] = 0.8;
			dictionary2[GetFormationKinds.SUBMARINE][BattleFormationKinds1.TanJuu] = 1.0;
			dictionary2[GetFormationKinds.SUBMARINE][BattleFormationKinds1.FukuJuu] = 1.2;
			dictionary2[GetFormationKinds.SUBMARINE][BattleFormationKinds1.Rinkei] = 1.0;
			dictionary2[GetFormationKinds.SUBMARINE][BattleFormationKinds1.Teikei] = 1.2;
			dictionary2[GetFormationKinds.SUBMARINE][BattleFormationKinds1.TanOu] = 1.2;
			dictionary3[GetFormationKinds.HOUGEKI][BattleFormationKinds1.TanJuu] = 1.0;
			dictionary3[GetFormationKinds.HOUGEKI][BattleFormationKinds1.FukuJuu] = 1.0;
			dictionary3[GetFormationKinds.HOUGEKI][BattleFormationKinds1.Rinkei] = 1.1;
			dictionary3[GetFormationKinds.HOUGEKI][BattleFormationKinds1.Teikei] = 1.2;
			dictionary3[GetFormationKinds.HOUGEKI][BattleFormationKinds1.TanOu] = 1.3;
			dictionary3[GetFormationKinds.RAIGEKI][BattleFormationKinds1.TanJuu] = 1.0;
			dictionary3[GetFormationKinds.RAIGEKI][BattleFormationKinds1.FukuJuu] = 1.0;
			dictionary3[GetFormationKinds.RAIGEKI][BattleFormationKinds1.Rinkei] = 1.1;
			dictionary3[GetFormationKinds.RAIGEKI][BattleFormationKinds1.Teikei] = 1.3;
			dictionary3[GetFormationKinds.RAIGEKI][BattleFormationKinds1.TanOu] = 1.4;
			dictionary3[GetFormationKinds.MIDNIGHT][BattleFormationKinds1.TanJuu] = 1.0;
			dictionary3[GetFormationKinds.MIDNIGHT][BattleFormationKinds1.FukuJuu] = 1.0;
			dictionary3[GetFormationKinds.MIDNIGHT][BattleFormationKinds1.Rinkei] = 1.0;
			dictionary3[GetFormationKinds.MIDNIGHT][BattleFormationKinds1.Teikei] = 1.1;
			dictionary3[GetFormationKinds.MIDNIGHT][BattleFormationKinds1.TanOu] = 1.2;
			dictionary3[GetFormationKinds.AIR][BattleFormationKinds1.TanJuu] = 1.0;
			dictionary3[GetFormationKinds.AIR][BattleFormationKinds1.FukuJuu] = 1.2;
			dictionary3[GetFormationKinds.AIR][BattleFormationKinds1.Rinkei] = 1.6;
			dictionary3[GetFormationKinds.AIR][BattleFormationKinds1.Teikei] = 1.0;
			dictionary3[GetFormationKinds.AIR][BattleFormationKinds1.TanOu] = 1.0;
			dictionary3[GetFormationKinds.SUBMARINE][BattleFormationKinds1.TanJuu] = 1.0;
			dictionary3[GetFormationKinds.SUBMARINE][BattleFormationKinds1.FukuJuu] = 1.0;
			dictionary3[GetFormationKinds.SUBMARINE][BattleFormationKinds1.Rinkei] = 1.0;
			dictionary3[GetFormationKinds.SUBMARINE][BattleFormationKinds1.Teikei] = 1.3;
			dictionary3[GetFormationKinds.SUBMARINE][BattleFormationKinds1.TanOu] = 1.1;
			dictionary4[GetFormationKinds.HOUGEKI][BattleFormationKinds2.T_Enemy] = 0.6;
			dictionary4[GetFormationKinds.HOUGEKI][BattleFormationKinds2.T_Own] = 1.2;
			dictionary4[GetFormationKinds.HOUGEKI][BattleFormationKinds2.Hankou] = 0.8;
			dictionary4[GetFormationKinds.HOUGEKI][BattleFormationKinds2.Doukou] = 1.0;
			dictionary4[GetFormationKinds.RAIGEKI][BattleFormationKinds2.T_Enemy] = 0.6;
			dictionary4[GetFormationKinds.RAIGEKI][BattleFormationKinds2.T_Own] = 1.2;
			dictionary4[GetFormationKinds.RAIGEKI][BattleFormationKinds2.Hankou] = 0.8;
			dictionary4[GetFormationKinds.RAIGEKI][BattleFormationKinds2.Doukou] = 1.0;
			dictionary4[GetFormationKinds.SUBMARINE][BattleFormationKinds2.T_Enemy] = 0.6;
			dictionary4[GetFormationKinds.SUBMARINE][BattleFormationKinds2.T_Own] = 1.2;
			dictionary4[GetFormationKinds.SUBMARINE][BattleFormationKinds2.Hankou] = 0.8;
			dictionary4[GetFormationKinds.SUBMARINE][BattleFormationKinds2.Doukou] = 1.0;
			paramF1 = dictionary;
			paramF2 = dictionary2;
			paramF3 = dictionary3;
			paramBattleFormattion = dictionary4;
		}
	}
}
