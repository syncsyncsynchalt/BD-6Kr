using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Controllers.BattleLogic;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public abstract class BattleControllerBase : IDisposable, IMakeBattleData
	{
		protected bool practiceFlag;

		protected Api_req_Map mapInstance;

		protected BattleBaseData userData;

		protected Dictionary<int, BattleShipSubInfo> userSubInfo;

		protected BattleBaseData enemyData;

		protected Dictionary<int, BattleShipSubInfo> enemySubInfo;

		protected FormationDatas formationParams;

		protected BattleCommandParams battleCommandParams;

		protected ExecBattleKinds battleKinds;

		protected int[] seikuValue;

		protected AllBattleFmt allBattleFmt;

		protected HashSet<BattleCommand> enableBattleCommand;

		protected List<MiddleBattleCallInfo> callInfo = new List<MiddleBattleCallInfo>();

		protected HashSet<int> airSubstituteAttackRid;

		private bool enforceMotherFlag;

		protected abstract void init();

		public Dictionary<int, int> GetSlotExpBattleData()
		{
			if (userData.SlotExperience.Count == 0)
			{
				return new Dictionary<int, int>();
			}
			return userData.SlotExperience.ToDictionary((KeyValuePair<int, int[]> key) => key.Key, (KeyValuePair<int, int[]> val) => val.Value[1]);
		}

		public virtual void Dispose()
		{
			userData.Dispose();
			enemyData.Dispose();
			formationParams.Dispose();
			userSubInfo.Clear();
			enemySubInfo.Clear();
			battleCommandParams.Dispose();
			enableBattleCommand.Clear();
			callInfo.Clear();
			airSubstituteAttackRid.Clear();
		}

		public int GetBattleCommand(out List<BattleCommand> command)
		{
			command = null;
			if (userData == null)
			{
				return 0;
			}
			Mem_ship mem_ship = userData.ShipData[0];
			command = mem_ship.GetBattleCommad();
			if (!validBattleCommand(command))
			{
				mem_ship.PurgeBattleCommand();
				command = mem_ship.GetBattleCommad();
			}
			bool flag = userData.ShipData.Any((Mem_ship x) => Mst_DataManager.Instance.Mst_stype[x.Stype].IsMother() && x.IsFight());
			enforceMotherFlag = false;
			if (flag)
			{
				mem_ship.SetBattleCommand(new List<BattleCommand>
				{
					BattleCommand.Kouku
				});
				command = mem_ship.GetBattleCommad();
				enforceMotherFlag = true;
			}
			return mem_ship.GetBattleCommandEnableNum();
		}

		public HashSet<BattleCommand> GetEnableBattleCommand()
		{
			if (enableBattleCommand != null)
			{
				return enableBattleCommand;
			}
			Mem_ship mem_ship = userData.ShipData[0];
			HashSet<BattleCommand> hashSet = new HashSet<BattleCommand>();
			hashSet.Add(BattleCommand.Sekkin);
			hashSet.Add(BattleCommand.Hougeki);
			hashSet.Add(BattleCommand.Raigeki);
			hashSet.Add(BattleCommand.Ridatu);
			hashSet.Add(BattleCommand.Taisen);
			hashSet.Add(BattleCommand.Kaihi);
			HashSet<BattleCommand> hashSet2 = hashSet;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			List<bool> list = new List<bool>();
			List<Mem_ship> list2 = (from x in userData.ShipData
				where !x.Escape_sts
				select x).Take(3).ToList();
			for (int i = 0; i < userData.ShipData.Count; i++)
			{
				Mem_ship mem_ship2 = userData.ShipData[i];
				if (mem_ship2.Escape_sts)
				{
					num3++;
					continue;
				}
				if (mem_ship2.Level >= 10)
				{
					num++;
				}
				if (mem_ship2.Level >= 25)
				{
					num2++;
				}
				if (list2.Contains(mem_ship2))
				{
					List<Mst_slotitem> source = userData.SlotData[i];
					if (source.Any((Mst_slotitem x) => x.IsDentan()))
					{
						list.Add(item: true);
					}
				}
				if (Mst_DataManager.Instance.Mst_stype[mem_ship2.Stype].IsKouku())
				{
					hashSet2.Add(BattleCommand.Kouku);
				}
			}
			int num4 = userData.ShipData.Count - num3;
			if (mem_ship.Level >= 20 && num4 == num)
			{
				hashSet2.Add(BattleCommand.Totugeki);
			}
			if (mem_ship.Level >= 50 && num4 == num2 && list.Count == 3)
			{
				hashSet2.Add(BattleCommand.Tousha);
			}
			enableBattleCommand = hashSet2;
			return hashSet2;
		}

		public bool ValidBattleCommand(List<BattleCommand> command)
		{
			if (command == null || userData == null)
			{
				return false;
			}
			HashSet<BattleCommand> items = GetEnableBattleCommand();
			int battleCommandEnableNum = userData.ShipData[0].GetBattleCommandEnableNum();
			IEnumerable<BattleCommand> source = command.Skip(battleCommandEnableNum);
			if (!source.All((BattleCommand x) => x == BattleCommand.None))
			{
				return false;
			}
			List<BattleCommand> list = command.Take(battleCommandEnableNum).ToList();
			if (enforceMotherFlag)
			{
				list.RemoveAt(0);
			}
			return list.All((BattleCommand x) => items.Contains(x));
		}

		private bool validBattleCommand(List<BattleCommand> command)
		{
			HashSet<BattleCommand> e_items = GetEnableBattleCommand();
			IEnumerable<BattleCommand> source = command.TakeWhile((BattleCommand x) => x != BattleCommand.None);
			return source.All((BattleCommand x) => e_items.Contains(x));
		}

		public void SetBattleCommand(List<BattleCommand> command)
		{
			if (ValidBattleCommand(command))
			{
				userData.ShipData[0].SetBattleCommand(command);
			}
		}

		public virtual Api_Result<AllBattleFmt> GetDayPreBattleInfo(BattleFormationKinds1 formationKind)
		{
			Api_Result<AllBattleFmt> api_Result = new Api_Result<AllBattleFmt>();
			if (userData == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			airSubstituteAttackRid = new HashSet<int>();
			if (battleKinds != 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (userData.ShipData[0].Get_DamageState() == DamageState.Taiha)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			initFormation(formationKind);
			AllBattleFmt allBattleFmt = new AllBattleFmt(userData.Formation, enemyData.Formation, userData.BattleFormation);
			allBattleFmt.DayBattle = new DayBattleFmt(userData.Deck.Rid, userData.ShipData, enemyData.ShipData);
			using (Exec_Search exec_Search = new Exec_Search(userData, userSubInfo, enemyData, enemySubInfo, practiceFlag))
			{
				allBattleFmt.DayBattle.Search = exec_Search.GetResultData(formationParams, battleCommandParams);
			}
			api_Result.data = allBattleFmt;
			this.allBattleFmt = allBattleFmt;
			return api_Result;
		}

		public virtual Api_Result<AllBattleFmt> DayBattle()
		{
			Api_Result<AllBattleFmt> api_Result = new Api_Result<AllBattleFmt>();
			if (this.allBattleFmt == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (battleKinds != 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			AllBattleFmt allBattleFmt = this.allBattleFmt;
			allBattleFmt.DayBattle.OpeningProduction = battleCommandParams.GetProductionData(userData.GetDeckBattleCommand()[0]);
			if (battleCommandParams.IsEscape)
			{
				allBattleFmt.DayBattle.ValidMidnight = isGoMidnight();
				battleKinds = ExecBattleKinds.DayOnly;
				api_Result.data = allBattleFmt;
				return api_Result;
			}
			using (Exec_AirBattle exec_AirBattle = new Exec_AirBattle(userData, userSubInfo, enemyData, enemySubInfo, allBattleFmt.DayBattle.Search, practiceFlag))
			{
				allBattleFmt.DayBattle.AirBattle = exec_AirBattle.GetResultData(formationParams, battleCommandParams);
				seikuValue = exec_AirBattle.getSeikuValue();
			}
			allBattleFmt.BattleFormation = (userData.BattleFormation = (enemyData.BattleFormation = formationParams.AfterAirBattle_RewriteBattleFormation2(userData)));
			if (!practiceFlag)
			{
				mapInstance.GetNowCell();
				using (Exec_SupportAttack exec_SupportAttack = new Exec_SupportAttack(enemyData, enemySubInfo, allBattleFmt.DayBattle.Search, mapInstance.Mst_SupportData))
				{
					if (mapInstance.IsRebbelion)
					{
						exec_SupportAttack.SelectSupportDeck(mapInstance.Support_decks, isForwardDeck: false);
					}
					else
					{
						exec_SupportAttack.SelectSupportDeck(mapInstance.Support_decks);
					}
					allBattleFmt.DayBattle.SupportAtack = exec_SupportAttack.GetResultData(formationParams, new BattleCommandParams(userData));
				}
			}
			using (Exec_OpeningAtack exec_OpeningAtack = new Exec_OpeningAtack(userData, userSubInfo, enemyData, enemySubInfo, practiceFlag))
			{
				allBattleFmt.DayBattle.OpeningAtack = exec_OpeningAtack.GetResultData(formationParams, battleCommandParams);
				if (exec_OpeningAtack.CanRaigType() == 1 && exec_OpeningAtack.getUserAttackShipNum() == 0)
				{
					allBattleFmt.DayBattle.OpeningProduction = battleCommandParams.GetProductionData(0, BattleCommand.Raigeki);
				}
			}
			List<BattleCommand> deckBattleCommand = userData.GetDeckBattleCommand();
			List<MiddleBattleCallInfo> list = getMiddleBattleCallType(deckBattleCommand);
			if (deckBattleCommand[0] == BattleCommand.Kouku || deckBattleCommand[0] == BattleCommand.Raigeki)
			{
				MiddleBattleCallInfo middleBattleCallInfo = list[0];
				if (middleBattleCallInfo.BattleType == MiddleBattleCallInfo.CallType.None && middleBattleCallInfo.CommandPos != -1)
				{
					MiddleBattleCallInfo middleBattleCallInfo2 = list.FirstOrDefault((MiddleBattleCallInfo x) => x.BattleType == MiddleBattleCallInfo.CallType.LastRaig);
					IEnumerable<MiddleBattleCallInfo> source = list.TakeWhile((MiddleBattleCallInfo x) => x.BattleType != MiddleBattleCallInfo.CallType.LastRaig);
					List<MiddleBattleCallInfo> list2 = (from x in source
						where x.CommandPos != -1 && x.BattleType != MiddleBattleCallInfo.CallType.None
						select x).ToList();
					MiddleBattleCallInfo middleBattleCallInfo3 = list2[0];
					if (middleBattleCallInfo3.BattleType == MiddleBattleCallInfo.CallType.Houg && middleBattleCallInfo3.AttackType == 3)
					{
						middleBattleCallInfo3.AttackType = 1;
						MiddleBattleCallInfo item = new MiddleBattleCallInfo(middleBattleCallInfo3.CommandPos, middleBattleCallInfo3.UseCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						list2.Insert(1, item);
					}
					List<MiddleBattleCallInfo> list3 = (from x in source
						where x.CommandPos == -1
						select x).ToList();
					List<MiddleBattleCallInfo> list4 = new List<MiddleBattleCallInfo>();
					int num = (list2.Count <= list3.Count) ? list3.Count : list2.Count;
					for (int i = 0; i < num; i++)
					{
						MiddleBattleCallInfo item2 = (i >= list2.Count) ? new MiddleBattleCallInfo(100, BattleCommand.None, MiddleBattleCallInfo.CallType.None, 0) : list2[i];
						MiddleBattleCallInfo item3 = (i >= list3.Count) ? new MiddleBattleCallInfo(-1, BattleCommand.None, MiddleBattleCallInfo.CallType.None, 0) : list3[i];
						list4.Add(item2);
						list4.Add(item3);
					}
					list = list4;
					if (middleBattleCallInfo2 != null)
					{
						list.Add(middleBattleCallInfo2);
					}
				}
			}
			bool flag = false;
			allBattleFmt.DayBattle.FromMiddleDayBattle = new List<FromMiddleBattleDayData>();
			bool flag2 = false;
			bool flag3 = false;
			FromMiddleBattleDayData fromMiddleBattleDayData = null;
			foreach (MiddleBattleCallInfo item4 in list)
			{
				if (!flag2 && !flag3)
				{
					fromMiddleBattleDayData = new FromMiddleBattleDayData();
				}
				IBattleType battleType = null;
				if (item4.BattleType == MiddleBattleCallInfo.CallType.EffectOnly && isGoMidnight())
				{
					fromMiddleBattleDayData.Production = battleCommandParams.GetProductionData(item4.CommandPos, item4.UseCommand);
					if (battleCommandParams.IsEscape)
					{
						allBattleFmt.DayBattle.ValidMidnight = isGoMidnight();
						battleKinds = ExecBattleKinds.DayOnly;
						api_Result.data = allBattleFmt;
						allBattleFmt.DayBattle.FromMiddleDayBattle.Add(fromMiddleBattleDayData);
						return api_Result;
					}
				}
				else if (item4.BattleType == MiddleBattleCallInfo.CallType.Houg)
				{
					if ((item4.UseCommand == BattleCommand.Tousha || item4.UseCommand == BattleCommand.Totugeki) && isGoMidnight())
					{
						fromMiddleBattleDayData.Production = battleCommandParams.GetProductionData(item4.CommandPos, item4.UseCommand);
					}
					using (Exec_Hougeki exec_Hougeki = new Exec_Hougeki(item4, seikuValue, userData, userSubInfo, enemyData, enemySubInfo, practiceFlag))
					{
						exec_Hougeki.SetAirSubstituteAttacker(airSubstituteAttackRid);
						HougekiDayBattleFmt resultData = exec_Hougeki.GetResultData(formationParams, battleCommandParams);
						battleType = resultData;
					}
				}
				else if (item4.BattleType == MiddleBattleCallInfo.CallType.Raig)
				{
					using (Exec_Raigeki exec_Raigeki = new Exec_Raigeki(1, userData, userSubInfo, enemyData, enemySubInfo, practiceFlag))
					{
						Raigeki resultData2 = exec_Raigeki.GetResultData(formationParams, battleCommandParams);
						battleType = resultData2;
					}
				}
				else if (item4.BattleType == MiddleBattleCallInfo.CallType.LastRaig)
				{
					flag = true;
					break;
				}
				if (item4.CommandPos != -1)
				{
					fromMiddleBattleDayData.F_BattleData = battleType;
					flag2 = true;
				}
				else
				{
					fromMiddleBattleDayData.E_BattleData = battleType;
					flag3 = true;
				}
				if (flag2 && flag3)
				{
					flag2 = false;
					flag3 = false;
					allBattleFmt.DayBattle.FromMiddleDayBattle.Add(fromMiddleBattleDayData);
				}
			}
			if (allBattleFmt.DayBattle.FromMiddleDayBattle.Count == 0)
			{
				allBattleFmt.DayBattle.FromMiddleDayBattle = null;
			}
			int atkType = 2;
			if (flag)
			{
				atkType = 3;
			}
			using (Exec_Raigeki exec_Raigeki2 = new Exec_Raigeki(atkType, userData, userSubInfo, enemyData, enemySubInfo, practiceFlag))
			{
				allBattleFmt.DayBattle.Raigeki = exec_Raigeki2.GetResultData(formationParams, battleCommandParams);
			}
			allBattleFmt.DayBattle.ValidMidnight = isGoMidnight();
			battleKinds = ExecBattleKinds.DayOnly;
			api_Result.data = allBattleFmt;
			return api_Result;
		}

		protected bool isGoMidnight()
		{
			Mem_ship mem_ship = userData.ShipData.FirstOrDefault((Mem_ship x) => x.IsFight());
			Mem_ship mem_ship2 = enemyData.ShipData.FirstOrDefault((Mem_ship y) => y.IsFight());
			return (mem_ship != null && mem_ship2 != null) ? true : false;
		}

		public virtual Api_Result<AllBattleFmt> NightBattle()
		{
			Api_Result<AllBattleFmt> api_Result = new Api_Result<AllBattleFmt>();
			if (userData == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (battleKinds != ExecBattleKinds.DayOnly)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_ship mem_ship = userData.ShipData.FirstOrDefault((Mem_ship x) => x.IsFight());
			Mem_ship mem_ship2 = enemyData.ShipData.FirstOrDefault((Mem_ship y) => y.IsFight());
			if (mem_ship == null && mem_ship2 == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			AllBattleFmt allBattleFmt = new AllBattleFmt(userData.Formation, enemyData.Formation, userData.BattleFormation);
			using (Exec_Midnight exec_Midnight = new Exec_Midnight(1, seikuValue, userData, userSubInfo, enemyData, enemySubInfo, practiceFlag))
			{
				allBattleFmt.NightBattle = exec_Midnight.GetResultData(formationParams, battleCommandParams);
			}
			battleKinds = ExecBattleKinds.DayToNight;
			api_Result.data = allBattleFmt;
			this.allBattleFmt = allBattleFmt;
			return api_Result;
		}

		public virtual Api_Result<BattleResultFmt> BattleResult()
		{
			Api_Result<BattleResultFmt> api_Result = new Api_Result<BattleResultFmt>();
			if (battleKinds == ExecBattleKinds.None)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
			}
			using (Exec_BattleResult exec_BattleResult = new Exec_BattleResult(new BattleResultBase(this)))
			{
				api_Result.data = exec_BattleResult.GetResultData(formationParams, battleCommandParams);
			}
			battleKinds = ExecBattleKinds.None;
			return api_Result;
		}

		public virtual void GetBattleResultBase(BattleResultBase out_data)
		{
			out_data.MyData = userData;
			out_data.EnemyData = enemyData;
			out_data.F_SubInfo = userSubInfo;
			out_data.E_SubInfo = enemySubInfo;
			out_data.ExecKinds = battleKinds;
			out_data.PracticeFlag = practiceFlag;
		}

		protected void setBattleSubInfo(BattleBaseData baseData, out Dictionary<int, BattleShipSubInfo> subInfo)
		{
			subInfo = new Dictionary<int, BattleShipSubInfo>();
			List<Mem_ship> shipData = baseData.ShipData;
			var source = shipData.Select((Mem_ship obj, int idx) => new
			{
				obj,
				idx
			});
			var lookup = (from x in source
				orderby x.obj.Leng descending
				select x).ToLookup(gkey => gkey.obj.Leng);
			int num = 0;
			foreach (var item in lookup)
			{
				var orderedEnumerable = from x in item
					orderby x.idx
					select x;
				foreach (var item2 in orderedEnumerable)
				{
					BattleShipSubInfo value = new BattleShipSubInfo(item2.idx, item2.obj, num);
					subInfo.Add(item2.obj.Rid, value);
					num++;
				}
			}
		}

        protected void initFormation(BattleFormationKinds1 userFormation)
		{
            userData.Formation = userFormation;
            // ここでサイコロ振ってT字有利とか決まる
            formationParams = new FormationDatas(userData, enemyData, practiceFlag);
			userData.BattleFormation = formationParams.BattleFormation;
			enemyData.Formation = formationParams.E_Formation;
			enemyData.BattleFormation = formationParams.BattleFormation;
		}

		protected List<MiddleBattleCallInfo> getMiddleBattleCallType(List<BattleCommand> useCommands)
		{
			List<MiddleBattleCallInfo> list = new List<MiddleBattleCallInfo>();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (useCommands[0] == BattleCommand.Sekkin)
			{
				num = 1;
			}
			if (useCommands[0] == BattleCommand.Raigeki)
			{
				num3 = 1;
			}
			BattleCommand useCommand = BattleCommand.None;
			if (useCommands.Count == 3)
			{
				BattleCommand battleCommand = useCommands[0];
				MiddleBattleCallInfo item;
				if (Exec_Hougeki.isHougCommand(battleCommand) && battleCommand != BattleCommand.Kouku)
				{
					item = new MiddleBattleCallInfo(0, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
					num2 = 1;
				}
				else
				{
					item = new MiddleBattleCallInfo(0, battleCommand, MiddleBattleCallInfo.CallType.None, 0);
				}
				list.Add(item);
				MiddleBattleCallInfo item2 = new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 1);
				list.Add(item2);
				MiddleBattleCallInfo item3 = null;
				battleCommand = useCommands[1];
				switch (battleCommand)
				{
				case BattleCommand.Sekkin:
					num++;
					item3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
					break;
				case BattleCommand.Kouku:
					if (useCommands[2] == BattleCommand.Kouku)
					{
						item3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num4 = 1;
					}
					else if (useCommands[2] != BattleCommand.Kouku)
					{
						item3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num4 = 2;
					}
					break;
				default:
					if (Exec_Hougeki.isHougCommand(battleCommand))
					{
						if (num2 == 1)
						{
							item3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
							num2 = 2;
						}
						else
						{
							item3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
							num2 = 2;
						}
						break;
					}
					switch (battleCommand)
					{
					case BattleCommand.Raigeki:
						if (num == 1 && (useCommands[2] == BattleCommand.Ridatu || useCommands[2] == BattleCommand.Kaihi))
						{
							item3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Raig, 3);
							num3 = 2;
						}
						else
						{
							num++;
							item3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
						}
						break;
					case BattleCommand.Ridatu:
					case BattleCommand.Kaihi:
						item3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
						break;
					}
					break;
				}
				list.Add(item3);
				MiddleBattleCallInfo item4 = new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 2);
				list.Add(item4);
				MiddleBattleCallInfo item5 = null;
				battleCommand = useCommands[2];
				switch (battleCommand)
				{
				case BattleCommand.Sekkin:
					item5 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
					break;
				case BattleCommand.Kouku:
					item5 = ((num4 != 1) ? new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3) : new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2));
					break;
				default:
					if (Exec_Hougeki.isHougCommand(battleCommand))
					{
						switch (num2)
						{
						case 2:
							item5 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
							break;
						case 1:
							item5 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
							break;
						default:
							item5 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
							break;
						}
					}
					else
					{
						switch (battleCommand)
						{
						case BattleCommand.Raigeki:
							item5 = ((num3 > 1) ? new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0) : new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.LastRaig, 3));
							break;
						case BattleCommand.Ridatu:
						case BattleCommand.Kaihi:
							item5 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
							break;
						}
					}
					break;
				}
				list.Add(item5);
				list.Add(new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.None, 0));
			}
			else if (useCommands.Count == 4)
			{
				BattleCommand battleCommand = useCommands[0];
				MiddleBattleCallInfo item6;
				if (Exec_Hougeki.isHougCommand(battleCommand) && battleCommand != BattleCommand.Kouku)
				{
					item6 = new MiddleBattleCallInfo(0, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
					num2 = 1;
				}
				else
				{
					item6 = new MiddleBattleCallInfo(0, battleCommand, MiddleBattleCallInfo.CallType.None, 0);
				}
				list.Add(item6);
				MiddleBattleCallInfo item7 = new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 1);
				list.Add(item7);
				MiddleBattleCallInfo item8 = null;
				battleCommand = useCommands[1];
				switch (battleCommand)
				{
				case BattleCommand.Sekkin:
					num++;
					item8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
					break;
				case BattleCommand.Kouku:
					if (useCommands[2] == BattleCommand.Kouku)
					{
						item8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num4 = 1;
					}
					if (useCommands[2] != BattleCommand.Kouku)
					{
						item8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num4 = 2;
					}
					break;
				default:
					if (Exec_Hougeki.isHougCommand(battleCommand))
					{
						if (num2 == 1)
						{
							item8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
							num2 = 2;
						}
						else
						{
							item8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
							num2 = 2;
						}
						break;
					}
					switch (battleCommand)
					{
					case BattleCommand.Raigeki:
						if (num == 1 && (useCommands[2] == BattleCommand.Ridatu || useCommands[2] == BattleCommand.Kaihi))
						{
							item8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Raig, 3);
							num3 = 2;
						}
						else
						{
							num++;
							item8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
						}
						break;
					case BattleCommand.Ridatu:
					case BattleCommand.Kaihi:
						item8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
						break;
					}
					break;
				}
				list.Add(item8);
				MiddleBattleCallInfo item9 = new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 2);
				list.Add(item9);
				MiddleBattleCallInfo item10 = null;
				battleCommand = useCommands[2];
				switch (battleCommand)
				{
				case BattleCommand.Sekkin:
					num++;
					item10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
					break;
				case BattleCommand.Kouku:
					if (num4 == 1 && useCommands[3] == BattleCommand.Kouku)
					{
						item10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num4 = 2;
					}
					else if (num4 == 1 && useCommands[3] != BattleCommand.Kouku)
					{
						item10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num4 = 2;
					}
					else if (num4 == 0 && useCommands[3] == BattleCommand.Kouku)
					{
						item10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num4 = 1;
					}
					else if (num4 == 0 && useCommands[3] != BattleCommand.Kouku)
					{
						item10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num4 = 2;
					}
					break;
				default:
					if (Exec_Hougeki.isHougCommand(battleCommand))
					{
						switch (num2)
						{
						case 2:
							item10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
							num2 = 3;
							break;
						case 1:
							item10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
							num2 = 2;
							break;
						default:
							item10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
							num2 = 2;
							break;
						}
						break;
					}
					switch (battleCommand)
					{
					case BattleCommand.Raigeki:
						if (num >= 1 && num3 <= 1)
						{
							if (useCommands[3] == BattleCommand.Kaihi || useCommands[3] == BattleCommand.Ridatu)
							{
								item10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Raig, 3);
								num3 = 3;
							}
							else
							{
								item10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
								num++;
							}
						}
						else
						{
							item10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
							num++;
						}
						break;
					case BattleCommand.Ridatu:
					case BattleCommand.Kaihi:
						item10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
						break;
					}
					break;
				}
				list.Add(item10);
				list.Add(new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 1));
				MiddleBattleCallInfo item11 = null;
				battleCommand = useCommands[3];
				switch (battleCommand)
				{
				case BattleCommand.Sekkin:
					item11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
					break;
				case BattleCommand.Kouku:
					switch (num4)
					{
					case 2:
						item11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						break;
					case 1:
						item11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						break;
					default:
						item11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						break;
					}
					break;
				default:
					if (Exec_Hougeki.isHougCommand(battleCommand))
					{
						switch (num2)
						{
						case 3:
							item11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
							break;
						case 2:
							item11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
							break;
						case 1:
							item11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
							break;
						default:
							item11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
							break;
						}
					}
					else
					{
						switch (battleCommand)
						{
						case BattleCommand.Raigeki:
							item11 = ((num3 > 1) ? new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0) : new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.LastRaig, 3));
							break;
						case BattleCommand.Ridatu:
						case BattleCommand.Kaihi:
							item11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
							break;
						}
					}
					break;
				}
				list.Add(item11);
				list.Add(new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.None, 0));
			}
			else if (useCommands.Count == 5)
			{
				BattleCommand battleCommand = useCommands[0];
				MiddleBattleCallInfo item12;
				if (Exec_Hougeki.isHougCommand(battleCommand) && battleCommand != BattleCommand.Kouku)
				{
					item12 = new MiddleBattleCallInfo(0, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
					num2 = 1;
				}
				else
				{
					item12 = new MiddleBattleCallInfo(0, battleCommand, MiddleBattleCallInfo.CallType.None, 0);
				}
				list.Add(item12);
				MiddleBattleCallInfo item13 = new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 1);
				list.Add(item13);
				MiddleBattleCallInfo item14 = null;
				battleCommand = useCommands[1];
				switch (battleCommand)
				{
				case BattleCommand.Sekkin:
					num++;
					item14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
					break;
				case BattleCommand.Kouku:
					if (useCommands[2] == BattleCommand.Kouku)
					{
						item14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num4 = 1;
					}
					else
					{
						item14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num4 = 2;
					}
					break;
				default:
					if (Exec_Hougeki.isHougCommand(battleCommand))
					{
						if (num2 == 1)
						{
							item14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
							num2 = 2;
						}
						else
						{
							item14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
							num2 = 2;
						}
						break;
					}
					switch (battleCommand)
					{
					case BattleCommand.Raigeki:
						if (num == 1 && (useCommands[2] == BattleCommand.Ridatu || useCommands[2] == BattleCommand.Kaihi))
						{
							item14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Raig, 3);
							num3 = 2;
						}
						else
						{
							num++;
							item14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
						}
						break;
					case BattleCommand.Ridatu:
					case BattleCommand.Kaihi:
						item14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
						break;
					}
					break;
				}
				list.Add(item14);
				MiddleBattleCallInfo item15 = new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 2);
				list.Add(item15);
				MiddleBattleCallInfo item16 = null;
				battleCommand = useCommands[2];
				switch (battleCommand)
				{
				case BattleCommand.Sekkin:
					num++;
					item16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
					break;
				case BattleCommand.Kouku:
					if (num4 == 1 && useCommands[3] == BattleCommand.Kouku)
					{
						item16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num4 = 2;
					}
					else if (num4 == 1 && useCommands[3] != BattleCommand.Kouku)
					{
						item16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num4 = 2;
					}
					else if (num4 == 0 && useCommands[3] == BattleCommand.Kouku)
					{
						item16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num4 = 1;
					}
					else
					{
						item16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num4 = 2;
					}
					break;
				default:
					if (Exec_Hougeki.isHougCommand(battleCommand))
					{
						switch (num2)
						{
						case 2:
							item16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
							num2 = 3;
							break;
						case 1:
							item16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
							num2 = 2;
							break;
						default:
							item16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
							num2 = 2;
							break;
						}
						break;
					}
					switch (battleCommand)
					{
					case BattleCommand.Raigeki:
						if (num >= 1 && num3 <= 1)
						{
							if (useCommands[3] == BattleCommand.Kaihi || useCommands[3] == BattleCommand.Ridatu)
							{
								item16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Raig, 3);
								num3 = 3;
							}
							else
							{
								item16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
								num++;
							}
						}
						else
						{
							item16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
							num++;
						}
						break;
					case BattleCommand.Ridatu:
					case BattleCommand.Kaihi:
						item16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
						break;
					}
					break;
				}
				list.Add(item16);
				list.Add(new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 1));
				MiddleBattleCallInfo item17 = null;
				battleCommand = useCommands[3];
				switch (battleCommand)
				{
				case BattleCommand.Sekkin:
					num++;
					item17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
					break;
				case BattleCommand.Kouku:
					if (num4 == 0 && useCommands[4] == BattleCommand.Kouku)
					{
						item17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num4 = 1;
					}
					else if (num4 == 0 && useCommands[4] != BattleCommand.Kouku)
					{
						item17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num4 = 2;
					}
					else if (num4 == 1 && useCommands[4] == BattleCommand.Kouku)
					{
						item17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num4 = 2;
					}
					else if (num4 == 1 && useCommands[4] != BattleCommand.Kouku)
					{
						item17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num4 = 2;
					}
					else if (num4 == 2 && useCommands[4] == BattleCommand.Kouku)
					{
						item17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num4 = 3;
					}
					else if (num4 == 2 && useCommands[4] != BattleCommand.Kouku)
					{
						item17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num4 = 4;
					}
					break;
				default:
					if (Exec_Hougeki.isHougCommand(battleCommand))
					{
						switch (num2)
						{
						case 3:
							item17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
							num2 = 4;
							break;
						case 2:
							item17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
							num2 = 3;
							break;
						case 1:
							item17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
							num2 = 2;
							break;
						default:
							item17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
							num2 = 4;
							break;
						}
						break;
					}
					switch (battleCommand)
					{
					case BattleCommand.Raigeki:
						if (num >= 1 && num3 <= 1)
						{
							if (useCommands[4] == BattleCommand.Kaihi || useCommands[4] == BattleCommand.Ridatu)
							{
								item17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Raig, 3);
								num3 = 4;
							}
							else
							{
								item17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
								num++;
							}
						}
						else
						{
							item17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
							num++;
						}
						break;
					case BattleCommand.Ridatu:
					case BattleCommand.Kaihi:
						item17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
						break;
					}
					break;
				}
				list.Add(item17);
				list.Add(new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 2));
				MiddleBattleCallInfo item18 = null;
				battleCommand = useCommands[4];
				switch (battleCommand)
				{
				case BattleCommand.Sekkin:
					item18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
					break;
				case BattleCommand.Kouku:
					switch (num4)
					{
					case 0:
						item18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						break;
					case 1:
						item18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						break;
					case 2:
						item18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						break;
					case 3:
						item18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						break;
					default:
						item18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						break;
					}
					break;
				default:
					if (Exec_Hougeki.isHougCommand(battleCommand))
					{
						switch (num2)
						{
						case 2:
						case 4:
							item18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
							break;
						case 1:
						case 3:
							item18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
							break;
						default:
							item18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
							break;
						}
					}
					else
					{
						switch (battleCommand)
						{
						case BattleCommand.Raigeki:
							item18 = ((num3 > 1) ? new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0) : new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.LastRaig, 3));
							break;
						case BattleCommand.Ridatu:
						case BattleCommand.Kaihi:
							item18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
							break;
						}
					}
					break;
				}
				list.Add(item18);
				list.Add(new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.None, 0));
			}
			return list;
		}
	}
}
