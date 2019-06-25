using Common.Enum;
using Common.Struct;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_PracticeDeck
	{
		private Mem_deck mem_deck;

		private List<Mem_ship> mem_ship;

		private readonly Dictionary<DeckPracticeType, double[]> useMaterial;

		private readonly HashSet<int> submarineGroup;

		private readonly HashSet<int> motherBGroup;

		private readonly Dictionary<int, int> mstLevelUser;

		private readonly Dictionary<int, int> mstLevelShip;

		private Dictionary<DeckPracticeType, Action<PracticeDeckResultFmt>> execFunc;

		public Api_req_PracticeDeck()
		{
			useMaterial = new Dictionary<DeckPracticeType, double[]>
			{
				{
					DeckPracticeType.Normal,
					new double[2]
					{
						0.5,
						0.2
					}
				},
				{
					DeckPracticeType.Hou,
					new double[2]
					{
						0.5,
						0.6
					}
				},
				{
					DeckPracticeType.Rai,
					new double[2]
					{
						0.55,
						0.65
					}
				},
				{
					DeckPracticeType.Taisen,
					new double[2]
					{
						0.55,
						0.4
					}
				},
				{
					DeckPracticeType.Kouku,
					new double[2]
					{
						0.65,
						0.55
					}
				},
				{
					DeckPracticeType.Sougou,
					new double[2]
					{
						0.7,
						0.7
					}
				}
			};
			submarineGroup = new HashSet<int>
			{
				13,
				14
			};
			motherBGroup = new HashSet<int>
			{
				11,
				7,
				18,
				14,
				16
			};
			mstLevelShip = Mst_DataManager.Instance.Get_MstLevel(shipTable: true);
			mstLevelUser = Mst_DataManager.Instance.Get_MstLevel(shipTable: false);
		}

		public double[] GetUseMaterialKeisu(DeckPracticeType type)
		{
			return useMaterial[type];
		}

		public bool PrackticeDeckCheck(DeckPracticeType type, int deck_rid)
		{
			Mem_deck value = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, out value))
			{
				return false;
			}
			List<Mem_ship> memShip = value.Ship.getMemShip();
			if (memShip.Count < 0)
			{
				return false;
			}
			Dictionary<int, Mst_ship> mst_shipDict = Mst_DataManager.Instance.Mst_ship;
			switch (type)
			{
			case DeckPracticeType.Normal:
				return true;
			case DeckPracticeType.Hou:
			{
				HashSet<int> disableItem = submarineGroup;
				return memShip.Any((Mem_ship x) => !disableItem.Contains(x.Stype));
			}
			case DeckPracticeType.Rai:
				return memShip.Any((Mem_ship x) => mst_shipDict[x.Ship_id].Raig > 0);
			case DeckPracticeType.Taisen:
				return memShip.Any((Mem_ship x) => mst_shipDict[x.Ship_id].Tais > 0);
			case DeckPracticeType.Kouku:
			{
				HashSet<int> enableItem = motherBGroup;
				return memShip.Any((Mem_ship x) => enableItem.Contains(x.Stype));
			}
			case DeckPracticeType.Sougou:
				return (memShip.Count >= 6) ? true : false;
			default:
				return false;
			}
		}

		public Api_Result<PracticeDeckResultFmt> GetResultData(DeckPracticeType type, int deck_rid)
		{
			Api_Result<PracticeDeckResultFmt> api_Result = new Api_Result<PracticeDeckResultFmt>();
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, out mem_deck))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			mem_ship = mem_deck.Ship.getMemShip();
			PracticeDeckResultFmt practiceDeckResultFmt = new PracticeDeckResultFmt();
			practiceDeckResultFmt.PracticeResult.Deck = mem_deck;
			practiceDeckResultFmt.PracticeResult.GetSpoint = 0;
			if (execFunc == null)
			{
				execFunc = new Dictionary<DeckPracticeType, Action<PracticeDeckResultFmt>>
				{
					{
						DeckPracticeType.Normal,
						getPracticeUpInfo_To_Normal
					},
					{
						DeckPracticeType.Hou,
						getPracticeUpInfo_To_Houg
					},
					{
						DeckPracticeType.Rai,
						getPracticeUpInfo_To_Raig
					},
					{
						DeckPracticeType.Taisen,
						getPracticeUpInfo_To_Taisen
					},
					{
						DeckPracticeType.Kouku,
						getPracticeUpInfo_To_Kouku
					},
					{
						DeckPracticeType.Sougou,
						getPracticeUpInfo_To_Sougou
					}
				};
			}
			execFunc[type](practiceDeckResultFmt);
			practiceDeckResultFmt.PracticeResult.MemberLevel = updateRecordLevel(practiceDeckResultFmt.PracticeResult.GetMemberExp);
			practiceDeckResultFmt.PracticeResult.LevelUpInfo = updateShip(type, practiceDeckResultFmt.PracticeResult.GetShipExp, practiceDeckResultFmt.PowerUpData);
			api_Result.data = practiceDeckResultFmt;
			mem_deck.ActionEnd();
			QuestPractice questPractice = new QuestPractice(type, practiceDeckResultFmt);
			questPractice.ExecuteCheck();
			Comm_UserDatas.Instance.User_record.UpdatePracticeCount(BattleWinRankKinds.NONE, practiceBattle: false);
			return api_Result;
		}

		private void getPracticeUpInfo_To_Normal(PracticeDeckResultFmt fmt)
		{
			int level = mem_ship[0].Level;
			double num = Math.Sqrt(mem_ship[0].Level);
			bool flag = Mst_DataManager.Instance.Mst_stype[mem_ship[0].Stype].IsTrainingShip();
			double num2 = 10.0 + Utils.GetRandDouble(0.0, 10.0, 1.0, 1) + Utils.GetRandDouble(0.0, num, 1.0, 1);
			fmt.PracticeResult.GetMemberExp = (int)num2;
			int num3 = flag ? 1 : 0;
			fmt.PracticeResult.GetShipExp = new Dictionary<int, int>();
			fmt.PowerUpData = new Dictionary<int, PowUpInfo>();
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			double difficultShipExpKeisu = getDifficultShipExpKeisu();
			double shipExpCommonKeisu = getShipExpCommonKeisu();
			foreach (Mem_ship item in mem_ship)
			{
				fmt.PracticeResult.GetShipExp.Add(item.Rid, 0);
				fmt.PowerUpData.Add(item.Rid, default(PowUpInfo));
				Mst_ship mst_ship2 = mst_ship[item.Ship_id];
				double num4 = Math.Sqrt(item.Level);
				double max = 14 + num3 * 7;
				double num5 = 20.0 + Utils.GetRandDouble(0.0, max, 1.0, 1) + num + num4;
				num5 = num5 * difficultShipExpKeisu * shipExpCommonKeisu;
				fmt.PracticeResult.GetShipExp[item.Rid] = (int)num5;
				PowUpInfo value = default(PowUpInfo);
				double max2 = 1.0 + (double)num3 * 0.5 + (num + num4) / 20.0;
				value.Kaihi = (int)Utils.GetRandDouble(0.0, max2, 1.0, 1);
				Ship_GrowValues battleBaseParam = item.GetBattleBaseParam();
				if (battleBaseParam.Kaihi + value.Kaihi > mst_ship2.Kaih_max)
				{
					int num6 = mst_ship2.Kaih_max - mst_ship2.Kaih;
					value.Kaihi = num6 - item.Kyouka[Mem_ship.enumKyoukaIdx.Kaihi];
				}
				fmt.PowerUpData[item.Rid] = value;
			}
		}

		private void getPracticeUpInfo_To_Houg(PracticeDeckResultFmt fmt)
		{
			int level = mem_ship[0].Level;
			double num = Math.Sqrt(mem_ship[0].Level);
			bool flag = Mst_DataManager.Instance.Mst_stype[mem_ship[0].Stype].IsTrainingShip();
			fmt.PracticeResult.GetMemberExp = 0;
			int num2 = flag ? 1 : 0;
			fmt.PracticeResult.GetShipExp = new Dictionary<int, int>();
			fmt.PowerUpData = new Dictionary<int, PowUpInfo>();
			double difficultShipExpKeisu = getDifficultShipExpKeisu();
			double shipExpCommonKeisu = getShipExpCommonKeisu();
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			foreach (Mem_ship item in mem_ship)
			{
				fmt.PracticeResult.GetShipExp.Add(item.Rid, 0);
				fmt.PowerUpData.Add(item.Rid, default(PowUpInfo));
				if (!submarineGroup.Contains(item.Stype))
				{
					Mst_ship mst_ship2 = mst_ship[item.Ship_id];
					double num3 = Math.Sqrt(item.Level);
					double max = 14 + num2 * 7;
					double num4 = 10.0 + Utils.GetRandDouble(0.0, max, 1.0, 1) + Utils.GetRandDouble(0.0, num, 1.0, 1) + num / 2.0 + num3;
					num4 = num4 * difficultShipExpKeisu * shipExpCommonKeisu;
					fmt.PracticeResult.GetShipExp[item.Rid] = (int)num4;
					PowUpInfo value = default(PowUpInfo);
					double max2 = 1.3 + (double)num2 * 0.2 + (num + num3) / 20.0;
					value.Karyoku = (int)Utils.GetRandDouble(0.0, max2, 1.0, 1);
					Ship_GrowValues battleBaseParam = item.GetBattleBaseParam();
					if (battleBaseParam.Houg + value.Karyoku > mst_ship2.Houg_max)
					{
						int num5 = mst_ship2.Houg_max - mst_ship2.Houg;
						value.Karyoku = num5 - item.Kyouka[Mem_ship.enumKyoukaIdx.Houg];
					}
					fmt.PowerUpData[item.Rid] = value;
				}
			}
		}

		private void getPracticeUpInfo_To_Raig(PracticeDeckResultFmt fmt)
		{
			int level = mem_ship[0].Level;
			double num = Math.Sqrt(mem_ship[0].Level);
			bool flag = Mst_DataManager.Instance.Mst_stype[mem_ship[0].Stype].IsTrainingShip();
			fmt.PracticeResult.GetMemberExp = 0;
			int num2 = flag ? 1 : 0;
			fmt.PracticeResult.GetShipExp = new Dictionary<int, int>();
			fmt.PowerUpData = new Dictionary<int, PowUpInfo>();
			double difficultShipExpKeisu = getDifficultShipExpKeisu();
			double shipExpCommonKeisu = getShipExpCommonKeisu();
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			foreach (Mem_ship item in mem_ship)
			{
				fmt.PracticeResult.GetShipExp.Add(item.Rid, 0);
				fmt.PowerUpData.Add(item.Rid, default(PowUpInfo));
				if (mst_ship[item.Ship_id].Raig != 0)
				{
					Mst_ship mst_ship2 = mst_ship[item.Ship_id];
					double num3 = Math.Sqrt(item.Level);
					double max = 16 + num2 * 6;
					double num4 = 15.0 + Utils.GetRandDouble(0.0, max, 1.0, 1) + Utils.GetRandDouble(0.0, num, 1.0, 1) + num / 2.0 + num3;
					num4 = num4 * difficultShipExpKeisu * shipExpCommonKeisu;
					fmt.PracticeResult.GetShipExp[item.Rid] = (int)num4;
					PowUpInfo value = default(PowUpInfo);
					double max2 = 1.3 + (double)num2 * 0.2 + (num + num3) / 20.0;
					value.Raisou = (int)Utils.GetRandDouble(0.0, max2, 1.0, 1);
					Ship_GrowValues battleBaseParam = item.GetBattleBaseParam();
					if (battleBaseParam.Raig + value.Raisou > mst_ship2.Raig_max)
					{
						int num5 = mst_ship2.Raig_max - mst_ship2.Raig;
						value.Raisou = num5 - item.Kyouka[Mem_ship.enumKyoukaIdx.Raig];
					}
					fmt.PowerUpData[item.Rid] = value;
				}
			}
		}

		private void getPracticeUpInfo_To_Taisen(PracticeDeckResultFmt fmt)
		{
			int level = mem_ship[0].Level;
			double num = Math.Sqrt(mem_ship[0].Level);
			bool flag = Mst_DataManager.Instance.Mst_stype[mem_ship[0].Stype].IsTrainingShip();
			fmt.PracticeResult.GetMemberExp = 0;
			int num2 = flag ? 1 : 0;
			fmt.PracticeResult.GetShipExp = new Dictionary<int, int>();
			fmt.PowerUpData = new Dictionary<int, PowUpInfo>();
			double difficultShipExpKeisu = getDifficultShipExpKeisu();
			double shipExpCommonKeisu = getShipExpCommonKeisu();
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			foreach (Mem_ship item in mem_ship)
			{
				fmt.PracticeResult.GetShipExp.Add(item.Rid, 0);
				fmt.PowerUpData.Add(item.Rid, default(PowUpInfo));
				Mst_ship mst_ship2 = mst_ship[item.Ship_id];
				if (mst_ship2.Tais != 0)
				{
					double num3 = Math.Sqrt(item.Level);
					double max = 12 + num2 * 6;
					double num4 = 7.0 + Utils.GetRandDouble(0.0, max, 1.0, 1) + Utils.GetRandDouble(0.0, num, 1.0, 1) + num / 2.0 + num3;
					num4 = num4 * difficultShipExpKeisu * shipExpCommonKeisu;
					fmt.PracticeResult.GetShipExp[item.Rid] = (int)num4;
					PowUpInfo value = default(PowUpInfo);
					double max2 = 1.4 + (double)num2 * 0.3 + (num + num3) / 20.0;
					value.Taisen = (int)Utils.GetRandDouble(0.0, max2, 1.0, 1);
					Ship_GrowValues battleBaseParam = item.GetBattleBaseParam();
					if (battleBaseParam.Taisen + value.Taisen > mst_ship2.Tais_max)
					{
						int num5 = mst_ship2.Tais_max - mst_ship2.Tais;
						value.Taisen = num5 - item.Kyouka[Mem_ship.enumKyoukaIdx.Taisen];
					}
					fmt.PowerUpData[item.Rid] = value;
				}
			}
		}

		private void getPracticeUpInfo_To_Kouku(PracticeDeckResultFmt fmt)
		{
			int level = mem_ship[0].Level;
			double num = Math.Sqrt(mem_ship[0].Level);
			bool flag = Mst_DataManager.Instance.Mst_stype[mem_ship[0].Stype].IsTrainingShip();
			fmt.PracticeResult.GetMemberExp = 0;
			int num2 = flag ? 1 : 0;
			fmt.PracticeResult.GetShipExp = new Dictionary<int, int>();
			fmt.PowerUpData = new Dictionary<int, PowUpInfo>();
			double difficultShipExpKeisu = getDifficultShipExpKeisu();
			double shipExpCommonKeisu = getShipExpCommonKeisu();
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			foreach (Mem_ship item in mem_ship)
			{
				fmt.PracticeResult.GetShipExp.Add(item.Rid, 0);
				fmt.PowerUpData.Add(item.Rid, default(PowUpInfo));
				Mst_ship mst_ship2 = mst_ship[item.Ship_id];
				if (mst_ship2.Stype != 13)
				{
					if (mst_ship2.Stype == 14)
					{
						List<Mst_slotitem> mstSlotItems = item.GetMstSlotItems();
						bool flag2 = false;
						for (int i = 0; i < mstSlotItems.Count; i++)
						{
							SlotitemCategory slotitem_type = Mst_DataManager.Instance.Mst_equip_category[mstSlotItems[i].Type3].Slotitem_type;
							if (slotitem_type == SlotitemCategory.Kanjouki || slotitem_type == SlotitemCategory.Suijouki)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							continue;
						}
					}
					double num3 = Math.Sqrt(item.Level);
					double max = 14 + num2 * 7;
					double num4 = 10.0 + Utils.GetRandDouble(0.0, max, 1.0, 1) + Utils.GetRandDouble(0.0, num, 1.0, 1) + num / 2.0 + num3;
					num4 = num4 * difficultShipExpKeisu * shipExpCommonKeisu;
					fmt.PracticeResult.GetShipExp[item.Rid] = (int)num4;
					Ship_GrowValues battleBaseParam = item.GetBattleBaseParam();
					PowUpInfo value = default(PowUpInfo);
					double max2 = 1.5 + (double)num2 * 0.2 + (num + num3) / 20.0;
					value.Taiku = (int)Utils.GetRandDouble(0.0, max2, 1.0, 1);
					if (battleBaseParam.Taiku + value.Taiku > mst_ship2.Tyku_max)
					{
						int num5 = mst_ship2.Tyku_max - mst_ship2.Tyku;
						value.Taiku = num5 - item.Kyouka[Mem_ship.enumKyoukaIdx.Tyku];
					}
					if (motherBGroup.Contains(item.Stype))
					{
						double max3 = 1.2 + (num + num3) / 20.0;
						value.Karyoku = (int)Utils.GetRandDouble(0.0, max3, 1.0, 1);
						if (battleBaseParam.Houg + value.Karyoku > mst_ship2.Houg_max)
						{
							int num6 = mst_ship2.Houg_max - mst_ship2.Houg;
							value.Karyoku = num6 - item.Kyouka[Mem_ship.enumKyoukaIdx.Houg];
						}
					}
					fmt.PowerUpData[item.Rid] = value;
				}
			}
		}

		private void getPracticeUpInfo_To_Sougou(PracticeDeckResultFmt fmt)
		{
			int level = mem_ship[0].Level;
			double num = Math.Sqrt(mem_ship[0].Level);
			bool flag = Mst_DataManager.Instance.Mst_stype[mem_ship[0].Stype].IsTrainingShip();
			double num2 = 30.0 + Utils.GetRandDouble(0.0, 10.0, 1.0, 1) + num;
			fmt.PracticeResult.GetMemberExp = (int)num2;
			int num3 = flag ? 1 : 0;
			fmt.PracticeResult.GetShipExp = new Dictionary<int, int>();
			fmt.PowerUpData = new Dictionary<int, PowUpInfo>();
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			double difficultShipExpKeisu = getDifficultShipExpKeisu();
			double shipExpCommonKeisu = getShipExpCommonKeisu();
			foreach (Mem_ship item in mem_ship)
			{
				fmt.PracticeResult.GetShipExp.Add(item.Rid, 0);
				fmt.PowerUpData.Add(item.Rid, default(PowUpInfo));
				Mst_ship mst_ship2 = mst_ship[item.Ship_id];
				double num4 = Math.Sqrt(item.Level);
				double num5 = 40.0 + Utils.GetRandDouble(0.0, 10.0, 1.0, 1) + (double)(num3 * 10) + Utils.GetRandDouble(0.0, num, 1.0, 1) + num4;
				num5 = num5 * difficultShipExpKeisu * shipExpCommonKeisu;
				fmt.PracticeResult.GetShipExp[item.Rid] = (int)num5;
				Ship_GrowValues battleBaseParam = item.GetBattleBaseParam();
				PowUpInfo value = default(PowUpInfo);
				double max = 1.0 + (num + num4) / 20.0;
				value.Karyoku = (int)Utils.GetRandDouble(0.0, max, 1.0, 1);
				if (battleBaseParam.Houg + value.Karyoku > mst_ship2.Houg_max)
				{
					int num6 = mst_ship2.Houg_max - mst_ship2.Houg;
					value.Karyoku = num6 - item.Kyouka[Mem_ship.enumKyoukaIdx.Houg];
				}
				double max2 = 0.7 + (num + num4) / 20.0;
				value.Lucky = (int)Utils.GetRandDouble(0.0, max2, 1.0, 1);
				if (battleBaseParam.Luck + value.Lucky > mst_ship2.Luck_max)
				{
					int num7 = mst_ship2.Luck_max - mst_ship2.Luck;
					value.Lucky = num7 - item.Kyouka[Mem_ship.enumKyoukaIdx.Luck];
				}
				fmt.PowerUpData[item.Rid] = value;
			}
		}

		private Dictionary<int, List<int>> updateShip(DeckPracticeType type, Dictionary<int, int> getShipExp, Dictionary<int, PowUpInfo> powerUp)
		{
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			double[] array = useMaterial[type];
			foreach (Mem_ship item in mem_ship)
			{
				int rid = item.Rid;
				Mem_shipBase mem_shipBase = new Mem_shipBase(item);
				Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship[mem_shipBase.Ship_id];
				int addExp = getShipExp[rid];
				List<int> lvupInfo = null;
				int levelupInfo = item.getLevelupInfo(mstLevelShip, item.Level, item.Exp, ref addExp, out lvupInfo);
				getShipExp[rid] = addExp;
				if (getShipExp[rid] != 0)
				{
					mem_shipBase.Exp += addExp;
					mem_shipBase.Level = levelupInfo;
				}
				dictionary.Add(rid, lvupInfo);
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary2 = item.Kyouka;
				if (!powerUp[item.Rid].IsAllZero())
				{
					addKyoukaValues(powerUp[item.Rid], dictionary2);
				}
				int num = levelupInfo - item.Level;
				for (int i = 0; i < num; i++)
				{
					dictionary2 = item.getLevelupKyoukaValue(item.Ship_id, dictionary2);
				}
				mem_shipBase.SetKyoukaValue(dictionary2);
				item.SetRequireExp(mem_shipBase.Level, mstLevelShip);
				mem_shipBase.Fuel = (int)((double)mem_shipBase.Fuel - (double)mem_shipBase.Fuel * array[0]);
				mem_shipBase.Bull = (int)((double)mem_shipBase.Bull - (double)mem_shipBase.Bull * array[1]);
				if (mem_shipBase.Fuel < 0)
				{
					mem_shipBase.Fuel = 0;
				}
				if (mem_shipBase.Bull < 0)
				{
					mem_shipBase.Bull = 0;
				}
				item.Set_ShipParam(mem_shipBase, mst_data, enemy_flag: false);
			}
			return dictionary;
		}

		private double getShipExpCommonKeisu()
		{
			return 9.5;
		}

		private double getDifficultShipExpKeisu()
		{
			if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.SHI)
			{
				return 1.2;
			}
			if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.KOU)
			{
				return 1.3;
			}
			if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.OTU)
			{
				return 1.4;
			}
			if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.HEI)
			{
				return 1.5;
			}
			if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.TEI)
			{
				return 2.0;
			}
			return 1.0;
		}

		private void addKyoukaValues(PowUpInfo powerUpValues, Dictionary<Mem_ship.enumKyoukaIdx, int> baseKyouka)
		{
			if (powerUpValues.Kaihi > 0)
			{
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary2 = dictionary = baseKyouka;
				Mem_ship.enumKyoukaIdx key;
				Mem_ship.enumKyoukaIdx key2 = key = Mem_ship.enumKyoukaIdx.Kaihi;
				int num = dictionary[key];
				dictionary2[key2] = num + powerUpValues.Kaihi;
			}
			if (powerUpValues.Karyoku > 0)
			{
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary3;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary4 = dictionary3 = baseKyouka;
				Mem_ship.enumKyoukaIdx key;
				Mem_ship.enumKyoukaIdx key3 = key = Mem_ship.enumKyoukaIdx.Houg;
				int num = dictionary3[key];
				dictionary4[key3] = num + powerUpValues.Karyoku;
			}
			if (powerUpValues.Raisou > 0)
			{
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary5;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary6 = dictionary5 = baseKyouka;
				Mem_ship.enumKyoukaIdx key;
				Mem_ship.enumKyoukaIdx key4 = key = Mem_ship.enumKyoukaIdx.Raig;
				int num = dictionary5[key];
				dictionary6[key4] = num + powerUpValues.Raisou;
			}
			if (powerUpValues.Taisen > 0)
			{
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary7;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary8 = dictionary7 = baseKyouka;
				Mem_ship.enumKyoukaIdx key;
				Mem_ship.enumKyoukaIdx key5 = key = Mem_ship.enumKyoukaIdx.Taisen;
				int num = dictionary7[key];
				dictionary8[key5] = num + powerUpValues.Taisen;
			}
			if (powerUpValues.Taiku > 0)
			{
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary9;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary10 = dictionary9 = baseKyouka;
				Mem_ship.enumKyoukaIdx key;
				Mem_ship.enumKyoukaIdx key6 = key = Mem_ship.enumKyoukaIdx.Tyku;
				int num = dictionary9[key];
				dictionary10[key6] = num + powerUpValues.Taiku;
			}
			if (powerUpValues.Lucky > 0)
			{
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary11;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary12 = dictionary11 = baseKyouka;
				Mem_ship.enumKyoukaIdx key;
				Mem_ship.enumKyoukaIdx key7 = key = Mem_ship.enumKyoukaIdx.Luck;
				int num = dictionary11[key];
				dictionary12[key7] = num + powerUpValues.Lucky;
			}
		}

		private int updateRecordLevel(int addExp)
		{
			return Comm_UserDatas.Instance.User_record.UpdateExp(addExp, mstLevelUser);
		}
	}
}
