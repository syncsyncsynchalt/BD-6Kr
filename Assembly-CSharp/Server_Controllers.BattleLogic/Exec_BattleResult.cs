using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_BattleResult : BattleLogicBase<BattleResultFmt>, IRebellionPointOperator
	{
		private BattleBaseData _f_Data;

		private BattleBaseData _e_Data;

		private Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		private Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		private readonly Mst_mapenemy2 mst_enemy;

		private readonly Mst_mapinfo mst_mapinfo;

		private readonly ExecBattleKinds battleKinds;

		private List<int> clothBrokenIds;

		private List<Mem_ship> deleteTargetShip;

		private Mem_mapclear cleard;

		private Mst_mapcell2 nowCell;

		private Dictionary<int, int> mst_shiplevel;

		private bool isRebellionBattle;

		private List<MapItemGetFmt> airCellItems;

		public override BattleBaseData F_Data => _f_Data;

		public override BattleBaseData E_Data => _e_Data;

		public override Dictionary<int, BattleShipSubInfo> F_SubInfo => _f_SubInfo;

		public override Dictionary<int, BattleShipSubInfo> E_SubInfo => _e_SubInfo;

		public Exec_BattleResult(BattleResultBase execBattleData)
		{
			mst_shiplevel = Mst_DataManager.Instance.Get_MstLevel(shipTable: true);
			_f_Data = execBattleData.MyData;
			_e_Data = execBattleData.EnemyData;
			_f_SubInfo = execBattleData.F_SubInfo;
			_e_SubInfo = execBattleData.E_SubInfo;
			practiceFlag = execBattleData.PracticeFlag;
			battleKinds = execBattleData.ExecKinds;
			clothBrokenIds = new List<int>();
			if (!execBattleData.PracticeFlag)
			{
				mst_enemy = Mst_DataManager.Instance.Mst_mapenemy[_e_Data.Enemy_id];
				string str = mst_enemy.Maparea_id.ToString();
				string str2 = mst_enemy.Mapinfo_no.ToString();
				int key = int.Parse(str + str2);
				mst_mapinfo = Mst_DataManager.Instance.Mst_mapinfo[key];
				deleteTargetShip = new List<Mem_ship>();
				cleard = execBattleData.Cleard;
				nowCell = execBattleData.NowCell;
			}
			isRebellionBattle = execBattleData.RebellionBattle;
			airCellItems = execBattleData.GetAirCellItems;
		}

		void IRebellionPointOperator.AddRebellionPoint(int area_id, int addNum)
		{
			throw new NotImplementedException();
		}

		void IRebellionPointOperator.SubRebellionPoint(int area_id, int subNum)
		{
			Comm_UserDatas.Instance.User_rebellion_point[area_id].EndInvation(this);
		}

		public override BattleResultFmt GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			return getData();
		}

		private BattleResultFmt getData()
		{
			BattleResultFmt ret = new BattleResultFmt();
			if (!practiceFlag)
			{
				ret.QuestName = mst_mapinfo.Name;
			}
			ret.EnemyName = E_Data.Enemy_Name;
			E_Data.ShipData.ForEach(delegate(Mem_ship x)
			{
				ret.EnemyId.Add(x.Ship_id);
			});
			ret.WinRank = getWinRank();
			if (isMvpGet(ret.WinRank))
			{
				Dictionary<int, BattleShipSubInfo> subInfoDict = F_SubInfo.Values.ToDictionary((BattleShipSubInfo key) => key.DeckIdx, (BattleShipSubInfo value) => value);
				int mvp = getMvp(ret.WinRank, subInfoDict);
				ret.MvpShip = mvp;
			}
			Mem_record user_record = Comm_UserDatas.Instance.User_record;
			int addValue;
			if (!practiceFlag)
			{
				int num = Mst_maparea.MaxMapNum(Comm_UserDatas.Instance.User_basic.Difficult, mst_mapinfo.Maparea_id);
				ret.GetBaseExp = mst_enemy.Experience;
				ret.GetShipExp = getShipExpSortie(ret.WinRank, ret.MvpShip, ret.GetBaseExp);
				addValue = getUserExpSortie(ret.WinRank);
				SerializableDictionary<int, List<int>> lvupInfo = null;
				updateShip(ret.WinRank, ret.MvpShip, ret.GetShipExp, out lvupInfo);
				ret.LevelUpInfo = lvupInfo;
				bool flag = Utils.IsBattleWin(ret.WinRank);
				bool takeAwayBattle = false;
				if (cleard != null && (cleard.State == MapClearState.InvationNeighbor || cleard.State == MapClearState.InvationOpen))
				{
					takeAwayBattle = true;
				}
				if (flag)
				{
					List<ItemGetFmt> list = new List<ItemGetFmt>();
					ItemGetFmt getShip = null;
					bool flag2 = isLastDance(Comm_UserDatas.Instance.User_basic.Difficult);
					if (!flag2)
					{
						getRewardShip(ret.WinRank, out getShip);
						if (getShip != null)
						{
							if (Comm_UserDatas.Instance.User_turn.Total_turn <= 100 && Comm_UserDatas.Instance.User_ship.Values.Any((Mem_ship x) => x.Ship_id == getShip.Id))
							{
								getRewardShip(ret.WinRank, out getShip);
							}
							if (getShip != null && Comm_UserDatas.Instance.User_ship.Values.Any((Mem_ship x) => x.Ship_id == getShip.Id))
							{
								getRewardShip(ret.WinRank, out getShip);
							}
						}
					}
					else
					{
						getClearShip(Comm_UserDatas.Instance.User_basic.Difficult, ret.WinRank, out getShip);
					}
					if (getShip != null)
					{
						addShip(getShip.Id, flag2);
						list.Add(getShip);
					}
					if (list.Count > 0)
					{
						ret.GetItem = list;
					}
					if (!isRebellionBattle)
					{
						List<int> diffMapOpen = null;
						List<int> reOpenMap = null;
						ret.FirstClear = updateMapComp(out diffMapOpen, out reOpenMap);
						ret.NewOpenMapId = diffMapOpen;
						ret.ReOpenMapId = reOpenMap;
						if (ret.FirstClear && Utils.IsGameClear())
						{
							user_record.AddClearDifficult(Comm_UserDatas.Instance.User_basic.Difficult);
						}
						else if (ret.FirstClear && mst_mapinfo.No == num)
						{
							ItemGetFmt itemGetFmt = new ItemGetFmt();
							itemGetFmt.Id = 57;
							itemGetFmt.Count = 1;
							itemGetFmt.Category = ItemGetKinds.UseItem;
							ret.AreaClearRewardItem = itemGetFmt;
							Comm_UserDatas.Instance.Add_Useitem(itemGetFmt.Id, itemGetFmt.Count);
						}
						if (mst_enemy.Boss != 0)
						{
							ret.FirstAreaComplete = updateAreaCompHisory(num);
						}
					}
					if (mst_enemy.Boss != 0 && airCellItems != null)
					{
						ret.GetAirReconnaissanceItems = airCellItems;
					}
				}
				else if (mst_enemy.Boss != 0 && airCellItems != null)
				{
					ret.GetAirReconnaissanceItems = new List<MapItemGetFmt>();
				}
				if (isRebellionBattle)
				{
					updateRebellion(ret.WinRank);
					ret.FirstClear = false;
				}
				bool rebellionBoss = (isRebellionBattle && mst_enemy.Boss != 0) ? true : false;
				user_record.UpdateSortieCount(ret.WinRank, rebellionBoss);
				if (ret.FirstClear)
				{
					ret.GetSpoint = mst_mapinfo.Clear_spoint;
					Comm_UserDatas.Instance.User_basic.AddPoint(ret.GetSpoint);
				}
				deleteLostShip();
				if (mst_enemy.Boss != 0)
				{
					updateHistory(flag, ret.NewOpenMapId, takeAwayBattle);
				}
			}
			else
			{
				ret.GetBaseExp = getBaseExpPractice(ret.WinRank);
				ret.GetShipExp = getShipExpPractice(ret.WinRank, ret.MvpShip, ret.GetBaseExp);
				addValue = getUserExpPractice(ret.WinRank, Comm_UserDatas.Instance.User_record.Level, E_Data.ShipData[0].Level);
				SerializableDictionary<int, List<int>> lvupInfo2 = null;
				updateShip(ret.WinRank, ret.MvpShip, ret.GetShipExp, out lvupInfo2);
				updateShipPracticeEnemy(ret.GetShipExp, ref lvupInfo2);
				ret.LevelUpInfo = lvupInfo2;
				user_record.UpdatePracticeCount(ret.WinRank, practiceBattle: true);
			}
			Comm_UserDatas.Instance.UpdateShipBookBrokenClothState(clothBrokenIds);
			ret.BasicLevel = user_record.UpdateExp(addValue, Mst_DataManager.Instance.Get_MstLevel(shipTable: false));
			return ret;
		}

		private bool isLastDance(DifficultKind kind)
		{
			if (mst_mapinfo.Maparea_id != 17)
			{
				return false;
			}
			int num = Mst_maparea.MaxMapNum(kind, 17);
			if (mst_mapinfo.No == num && mst_enemy.Boss != 0)
			{
				return true;
			}
			return false;
		}

		private void updateRebellion(BattleWinRankKinds winRank)
		{
			if (mst_enemy.Boss != 0)
			{
				int maparea_id = mst_mapinfo.Maparea_id;
				if (Utils.IsBattleWin(winRank))
				{
					((IRebellionPointOperator)this).SubRebellionPoint(maparea_id, 0);
				}
			}
		}

		private bool updateAreaCompHisory(int maxMapNum)
		{
			int firstMap = Mst_mapinfo.ConvertMapInfoId(mst_mapinfo.Maparea_id, 1);
			List<Mem_history> value = null;
			if (!Comm_UserDatas.Instance.User_history.TryGetValue(999, out value))
			{
				value = new List<Mem_history>();
			}
			if (value.Any((Mem_history x) => x.MapinfoId == firstMap))
			{
				return false;
			}
			int num = Comm_UserDatas.Instance.User_mapclear.Values.Count((Mem_mapclear x) => (x.Maparea_id == mst_mapinfo.Maparea_id && x.Cleared && x.State == MapClearState.Cleard) ? true : false);
			if (num != maxMapNum)
			{
				return false;
			}
			Mem_history mem_history = new Mem_history();
			mem_history.SetAreaComplete(firstMap);
			Comm_UserDatas.Instance.Add_History(mem_history);
			return true;
		}

		private bool updateMapComp(out List<int> diffMapOpen, out List<int> reOpenMap)
		{
			diffMapOpen = new List<int>();
			reOpenMap = new List<int>();
			if (mst_enemy.Boss == 0)
			{
				return false;
			}
			bool result = false;
			Dictionary<int, Mst_mapinfo>.KeyCollection keys = Utils.GetActiveMap().Keys;
			if (cleard == null)
			{
				cleard = new Mem_mapclear(mst_mapinfo.Id, mst_mapinfo.Maparea_id, mst_mapinfo.No, MapClearState.Cleard);
				cleard.Insert();
				result = true;
			}
			else
			{
				if (cleard.State != MapClearState.InvationOpen && cleard.State != MapClearState.InvationNeighbor)
				{
					if (cleard.State == MapClearState.Cleard)
					{
						return false;
					}
					return false;
				}
				if (!cleard.Cleared)
				{
					result = true;
				}
				cleard.StateChange(MapClearState.Cleard);
			}
			List<int> reOpenMap2 = null;
			new RebellionUtils().MapReOpen(cleard, out reOpenMap2);
			Dictionary<int, Mst_mapinfo>.KeyCollection keys2 = Utils.GetActiveMap().Keys;
			diffMapOpen = keys2.Except(keys).ToList();
			reOpenMap = reOpenMap2.Intersect(diffMapOpen).ToList();
			foreach (int item in reOpenMap)
			{
				diffMapOpen.Remove(item);
			}
			return result;
		}

		private void updateHistory(bool winFlag, List<int> openMaps, bool takeAwayBattle)
		{
			int total_turn = Comm_UserDatas.Instance.User_turn.Total_turn;
			foreach (int openMap in openMaps)
			{
				if (Mem_history.IsFirstOpenArea(openMap))
				{
					Mem_history mem_history = new Mem_history();
					mem_history.SetAreaOpen(total_turn, openMap);
					Comm_UserDatas.Instance.Add_History(mem_history);
				}
			}
			Mem_history mem_history2 = null;
			if (winFlag)
			{
				int mapClearNum = Mem_history.GetMapClearNum(mst_mapinfo.Id);
				int num = Mst_maparea.MaxMapNum(Comm_UserDatas.Instance.User_basic.Difficult, mst_mapinfo.Maparea_id);
				if (mst_mapinfo.Maparea_id == 17 && mst_mapinfo.No == num)
				{
					mem_history2 = new Mem_history();
					mem_history2.SetGameClear(total_turn);
				}
				else if (mapClearNum == 1)
				{
					mem_history2 = new Mem_history();
					mem_history2.SetMapClear(total_turn, mst_mapinfo.Id, mapClearNum, F_Data.ShipData[0].Ship_id);
				}
				else if (mapClearNum <= 3 && takeAwayBattle)
				{
					mem_history2 = new Mem_history();
					mem_history2.SetMapClear(total_turn, mst_mapinfo.Id, mapClearNum, F_Data.ShipData[0].Ship_id);
				}
			}
			if (mem_history2 != null)
			{
				Comm_UserDatas.Instance.Add_History(mem_history2);
			}
		}

		private void getRewardShip(BattleWinRankKinds rank, out ItemGetFmt out_items)
		{
			out_items = null;
			if (rank < BattleWinRankKinds.B || Comm_UserDatas.Instance.User_basic.IsMaxChara() || Comm_UserDatas.Instance.User_basic.IsMaxSlotitem())
			{
				return;
			}
			int enemy_type = 0;
			if (mst_enemy.Boss == 0)
			{
				enemy_type = 1;
				Dictionary<BattleWinRankKinds, int> dictionary = new Dictionary<BattleWinRankKinds, int>();
				dictionary.Add(BattleWinRankKinds.S, 30);
				dictionary.Add(BattleWinRankKinds.A, 40);
				dictionary.Add(BattleWinRankKinds.B, 30);
				Dictionary<BattleWinRankKinds, int> dictionary2 = dictionary;
				if (randInstance.Next(100) < dictionary2[rank])
				{
					return;
				}
			}
			else
			{
				enemy_type = 2;
			}
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			Mem_turn user_turn = Comm_UserDatas.Instance.User_turn;
			Dictionary<BattleWinRankKinds, int[]> dictionary4;
			switch (difficult)
			{
			case DifficultKind.KOU:
			case DifficultKind.SHI:
			{
				Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
				dictionary3.Add(BattleWinRankKinds.S, new int[2]
				{
					0,
					50
				});
				dictionary3.Add(BattleWinRankKinds.A, new int[2]
				{
					20,
					70
				});
				dictionary3.Add(BattleWinRankKinds.B, new int[2]
				{
					50,
					100
				});
				dictionary4 = dictionary3;
				break;
			}
			case DifficultKind.OTU:
				if (user_turn.Total_turn >= 0 && user_turn.Total_turn <= 50)
				{
					Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
					dictionary3.Add(BattleWinRankKinds.S, new int[2]
					{
						0,
						45
					});
					dictionary3.Add(BattleWinRankKinds.A, new int[2]
					{
						15,
						60
					});
					dictionary3.Add(BattleWinRankKinds.B, new int[2]
					{
						50,
						95
					});
					dictionary4 = dictionary3;
				}
				else if (user_turn.Total_turn >= 51 && user_turn.Total_turn <= 100)
				{
					Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
					dictionary3.Add(BattleWinRankKinds.S, new int[2]
					{
						0,
						50
					});
					dictionary3.Add(BattleWinRankKinds.A, new int[2]
					{
						15,
						65
					});
					dictionary3.Add(BattleWinRankKinds.B, new int[2]
					{
						50,
						100
					});
					dictionary4 = dictionary3;
				}
				else
				{
					Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
					dictionary3.Add(BattleWinRankKinds.S, new int[2]
					{
						0,
						50
					});
					dictionary3.Add(BattleWinRankKinds.A, new int[2]
					{
						20,
						70
					});
					dictionary3.Add(BattleWinRankKinds.B, new int[2]
					{
						50,
						100
					});
					dictionary4 = dictionary3;
				}
				break;
			case DifficultKind.HEI:
				if (user_turn.Total_turn >= 0 && user_turn.Total_turn <= 50)
				{
					Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
					dictionary3.Add(BattleWinRankKinds.S, new int[2]
					{
						0,
						40
					});
					dictionary3.Add(BattleWinRankKinds.A, new int[2]
					{
						15,
						55
					});
					dictionary3.Add(BattleWinRankKinds.B, new int[2]
					{
						50,
						90
					});
					dictionary4 = dictionary3;
				}
				else if (user_turn.Total_turn >= 51 && user_turn.Total_turn <= 100)
				{
					Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
					dictionary3.Add(BattleWinRankKinds.S, new int[2]
					{
						0,
						45
					});
					dictionary3.Add(BattleWinRankKinds.A, new int[2]
					{
						15,
						60
					});
					dictionary3.Add(BattleWinRankKinds.B, new int[2]
					{
						50,
						95
					});
					dictionary4 = dictionary3;
				}
				else
				{
					Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
					dictionary3.Add(BattleWinRankKinds.S, new int[2]
					{
						0,
						45
					});
					dictionary3.Add(BattleWinRankKinds.A, new int[2]
					{
						20,
						65
					});
					dictionary3.Add(BattleWinRankKinds.B, new int[2]
					{
						50,
						95
					});
					dictionary4 = dictionary3;
				}
				break;
			default:
				if (user_turn.Total_turn >= 0 && user_turn.Total_turn <= 100)
				{
					Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
					dictionary3.Add(BattleWinRankKinds.S, new int[2]
					{
						0,
						35
					});
					dictionary3.Add(BattleWinRankKinds.A, new int[2]
					{
						15,
						50
					});
					dictionary3.Add(BattleWinRankKinds.B, new int[2]
					{
						50,
						85
					});
					dictionary4 = dictionary3;
				}
				else if (user_turn.Total_turn >= 101 && user_turn.Total_turn <= 200)
				{
					Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
					dictionary3.Add(BattleWinRankKinds.S, new int[2]
					{
						0,
						40
					});
					dictionary3.Add(BattleWinRankKinds.A, new int[2]
					{
						15,
						55
					});
					dictionary3.Add(BattleWinRankKinds.B, new int[2]
					{
						50,
						90
					});
					dictionary4 = dictionary3;
				}
				else
				{
					Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
					dictionary3.Add(BattleWinRankKinds.S, new int[2]
					{
						0,
						45
					});
					dictionary3.Add(BattleWinRankKinds.A, new int[2]
					{
						15,
						60
					});
					dictionary3.Add(BattleWinRankKinds.B, new int[2]
					{
						50,
						95
					});
					dictionary4 = dictionary3;
				}
				break;
			}
			int num = randInstance.Next(dictionary4[rank][0], dictionary4[rank][1]) + mst_enemy.Geth;
			if (num > dictionary4[rank][1])
			{
				num = dictionary4[rank][1] - 1;
			}
			XElement[] array = (from data in Mst_DataManager.Instance.Mst_shipget
				where int.Parse(data.Element("Type").Value) == enemy_type
				orderby int.Parse(data.Element("Id").Value)
				select data).Skip(num).Take(1).ToArray();
			if (array != null && array.Length != 0)
			{
				Mst_shipget2 instance = null;
				Model_Base.SetMaster(out instance, array[0]);
				if (instance.Ship_id > 0)
				{
					out_items = new ItemGetFmt();
					out_items.Category = ItemGetKinds.Ship;
					out_items.Count = 1;
					out_items.Id = instance.Ship_id;
				}
			}
		}

		private void getClearShip(DifficultKind kind, BattleWinRankKinds rank, out ItemGetFmt out_items)
		{
			out_items = null;
			if (Utils.IsBattleWin(rank))
			{
				List<int> clearRewardShipList = ArrayMaster.GetClearRewardShipList(kind);
				Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
				int num = 0;
				foreach (int item in clearRewardShipList)
				{
					string targetYomi = mst_ship[item].Yomi;
					if (!Comm_UserDatas.Instance.User_ship.Values.Any(delegate(Mem_ship x)
					{
						int ship_id = x.Ship_id;
						string yomi = mst_ship[ship_id].Yomi;
						return yomi.Equals(targetYomi);
					}))
					{
						num = item;
						break;
					}
				}
				if (num != 0)
				{
					out_items = new ItemGetFmt();
					out_items.Category = ItemGetKinds.Ship;
					out_items.Count = 1;
					out_items.Id = num;
				}
			}
		}

		private void addShip(int mst_id, bool lastDance)
		{
			List<int> list = Comm_UserDatas.Instance.Add_Ship(new List<int>
			{
				mst_id
			});
			if (lastDance)
			{
				Comm_UserDatas.Instance.User_plus.SetRewardShipRid(this, list[0]);
			}
		}

		private BattleWinRankKinds getWinRank()
		{
			int count = F_Data.ShipData.Count;
			List<Mem_ship> shipData = F_Data.ShipData;
			List<int> startHp = F_Data.StartHp;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < count; i++)
			{
				if (!shipData[i].Escape_sts)
				{
					if (shipData[i].Nowhp <= 0)
					{
						num3++;
					}
					num += shipData[i].Nowhp;
					num2 += startHp[i];
				}
			}
			int count2 = E_Data.ShipData.Count;
			List<Mem_ship> shipData2 = E_Data.ShipData;
			List<int> startHp2 = E_Data.StartHp;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			for (int j = 0; j < count2; j++)
			{
				if (shipData2[j].Nowhp <= 0)
				{
					num6++;
				}
				num4 += shipData2[j].Nowhp;
				num5 += startHp2[j];
			}
			if (num3 == 0 && count2 == num6)
			{
				return BattleWinRankKinds.S;
			}
			int num7 = (int)Math.Floor((float)count2 * 0.7f);
			if (num3 == 0 && num6 >= num7 && count2 > 1)
			{
				return BattleWinRankKinds.A;
			}
			if (num3 < num6 && shipData2[0].Nowhp <= 0)
			{
				return BattleWinRankKinds.B;
			}
			DamageState damageState = shipData[0].Get_DamageState();
			if (count == 1 && damageState == DamageState.Taiha)
			{
				return BattleWinRankKinds.D;
			}
			float num8 = num2 - num;
			float num9 = num5 - num4;
			int num10 = (int)(num9 / (float)num5 * 100f);
			int num11 = (int)(num8 / (float)num2 * 100f);
			if ((float)num10 > (float)num11 * 2.5f)
			{
				return BattleWinRankKinds.B;
			}
			if ((float)num10 > (float)num11 * 0.9f)
			{
				return BattleWinRankKinds.C;
			}
			if (count > 1 && count - 1 == num3)
			{
				return BattleWinRankKinds.E;
			}
			return BattleWinRankKinds.D;
		}

		private bool isMvpGet(BattleWinRankKinds kind)
		{
			return (kind != BattleWinRankKinds.E && kind != 0) ? true : false;
		}

		private int getMvp(BattleWinRankKinds rank, Dictionary<int, BattleShipSubInfo> subInfoDict)
		{
			int num = (from item in subInfoDict.Values
				select item.TotalDamage).Max();
			if (subInfoDict[0].TotalDamage == num)
			{
				return subInfoDict[0].ShipInstance.Rid;
			}
			int num2 = -1;
			int result = 0;
			foreach (BattleShipSubInfo value in subInfoDict.Values)
			{
				if (value.ShipInstance.IsFight() && value.TotalDamage > num2)
				{
					result = value.ShipInstance.Rid;
					num2 = value.TotalDamage;
				}
			}
			return result;
		}

		private int getBaseExpPractice(BattleWinRankKinds rank)
		{
			int num = 500;
			int key = (E_Data.ShipData[0].Level != 100) ? E_Data.ShipData[0].Level : 99;
			int num2 = 0;
			if (E_Data.ShipData.Count() > 1)
			{
				num2 = ((E_Data.ShipData[1].Level != 100) ? E_Data.ShipData[1].Level : 99);
			}
			float num3 = (float)mst_shiplevel[key] / 100f;
			float num4 = (num2 <= 0) ? 0f : ((float)mst_shiplevel[num2] / 300f);
			int num5 = (int)(num3 + num4);
			num5 += randInstance.Next(4);
			if (num5 > num)
			{
				num5 = (int)((double)num + Math.Sqrt(num5 - num));
			}
			float num6 = 1f;
			float num7 = 0.8f;
			Dictionary<BattleWinRankKinds, float> dictionary = new Dictionary<BattleWinRankKinds, float>();
			dictionary.Add(BattleWinRankKinds.S, 1.2f * num6);
			dictionary.Add(BattleWinRankKinds.A, 1f * num6);
			dictionary.Add(BattleWinRankKinds.B, 1f * num6);
			dictionary.Add(BattleWinRankKinds.C, 0.8f * num7);
			dictionary.Add(BattleWinRankKinds.D, 0.7f * num7);
			dictionary.Add(BattleWinRankKinds.E, 0.5f * num7);
			Dictionary<BattleWinRankKinds, float> dictionary2 = dictionary;
			return (int)((float)num5 * dictionary2[rank]);
		}

		private SerializableDictionary<int, int> getShipExpSortie(BattleWinRankKinds rank, int mvpShip, int shipBaseExp)
		{
			SerializableDictionary<int, int> serializableDictionary = new SerializableDictionary<int, int>();
			float num = 1.5f;
			int num2 = 2;
			double num3 = 4.5;
			new List<int>();
			foreach (KeyValuePair<int, BattleShipSubInfo> item in F_SubInfo)
			{
				Mem_ship shipInstance = item.Value.ShipInstance;
				double num4 = shipInstance.IsFight() ? shipBaseExp : 0;
				if (item.Value.DeckIdx == 0)
				{
					num4 *= (double)num;
				}
				num4 *= num3;
				serializableDictionary.Add(shipInstance.Rid, (int)num4);
			}
			if (mvpShip <= 0)
			{
				return serializableDictionary;
			}
			double num5 = serializableDictionary[mvpShip] * num2;
			serializableDictionary[mvpShip] = (int)num5;
			return serializableDictionary;
		}

		private SerializableDictionary<int, int> getShipExpPractice(BattleWinRankKinds rank, int mvpShip, int shipBaseExp)
		{
			SerializableDictionary<int, int> serializableDictionary = new SerializableDictionary<int, int>();
			float num = 1.5f;
			int num2 = 2;
			double num3 = 0.7;
			double difficultShipExpKeisuToPractice = getDifficultShipExpKeisuToPractice();
			double trainingShipExpKeisuToPractice = getTrainingShipExpKeisuToPractice();
			double num4 = 7.5;
			new List<int>();
			foreach (KeyValuePair<int, BattleShipSubInfo> item in F_SubInfo)
			{
				Mem_ship shipInstance = item.Value.ShipInstance;
				double num5 = shipInstance.IsFight() ? shipBaseExp : 0;
				if (item.Value.DeckIdx == 0)
				{
					num5 *= (double)num;
				}
				num5 = num5 * difficultShipExpKeisuToPractice * trainingShipExpKeisuToPractice * num4 * num3;
				serializableDictionary.Add(shipInstance.Rid, (int)num5);
			}
			double num6 = (double)shipBaseExp * 0.5;
			double num7 = 3.5;
			foreach (KeyValuePair<int, BattleShipSubInfo> item2 in E_SubInfo)
			{
				Mem_ship shipInstance2 = item2.Value.ShipInstance;
				double num8 = (!shipInstance2.IsFight()) ? 0.0 : num6;
				num8 = num8 * difficultShipExpKeisuToPractice * num7 * num3;
				serializableDictionary.Add(shipInstance2.Rid, (int)num8);
			}
			if (mvpShip <= 0)
			{
				return serializableDictionary;
			}
			double num9 = serializableDictionary[mvpShip] * num2;
			serializableDictionary[mvpShip] = (int)num9;
			return serializableDictionary;
		}

		private double getDifficultShipExpKeisuToPractice()
		{
			switch (Comm_UserDatas.Instance.User_basic.Difficult)
			{
			case DifficultKind.SHI:
				return 1.2;
			case DifficultKind.KOU:
				return 1.3;
			case DifficultKind.OTU:
				return 1.4;
			case DifficultKind.HEI:
				return 1.5;
			case DifficultKind.TEI:
				return 2.0;
			default:
				return 1.0;
			}
		}

		private double getTrainingShipExpKeisuToPractice()
		{
			int num = F_Data.ShipData.Count((Mem_ship x) => x.IsFight() && Mst_DataManager.Instance.Mst_stype[x.Stype].IsTrainingShip());
			if (num == 0)
			{
				return 1.0;
			}
			Mem_ship mem_ship = F_Data.ShipData[0];
			if (Mst_DataManager.Instance.Mst_stype[mem_ship.Stype].IsTrainingShip())
			{
				if (num - 1 == 0)
				{
					if (mem_ship.Level < 9)
					{
						return 1.05;
					}
					if (mem_ship.Level >= 10 && mem_ship.Level <= 29)
					{
						return 1.08;
					}
					if (mem_ship.Level >= 30 && mem_ship.Level <= 59)
					{
						return 1.12;
					}
					if (mem_ship.Level >= 60 && mem_ship.Level <= 99)
					{
						return 1.1;
					}
					return 1.2;
				}
				if (mem_ship.Level < 9)
				{
					return 1.1;
				}
				if (mem_ship.Level >= 10 && mem_ship.Level <= 29)
				{
					return 1.13;
				}
				if (mem_ship.Level >= 30 && mem_ship.Level <= 59)
				{
					return 1.16;
				}
				if (mem_ship.Level >= 60 && mem_ship.Level <= 99)
				{
					return 1.2;
				}
				return 1.25;
			}
			if (num >= 2)
			{
				if (mem_ship.Level < 9)
				{
					return 1.04;
				}
				if (mem_ship.Level >= 10 && mem_ship.Level <= 29)
				{
					return 1.06;
				}
				if (mem_ship.Level >= 30 && mem_ship.Level <= 59)
				{
					return 1.08;
				}
				if (mem_ship.Level >= 60 && mem_ship.Level <= 99)
				{
					return 1.12;
				}
				return 1.175;
			}
			if (mem_ship.Level < 9)
			{
				return 1.03;
			}
			if (mem_ship.Level >= 10 && mem_ship.Level <= 29)
			{
				return 1.05;
			}
			if (mem_ship.Level >= 30 && mem_ship.Level <= 59)
			{
				return 1.07;
			}
			if (mem_ship.Level >= 60 && mem_ship.Level <= 99)
			{
				return 1.1;
			}
			return 1.15;
		}

		private int getUserExpSortie(BattleWinRankKinds rank)
		{
			Dictionary<BattleWinRankKinds, float[]> dictionary = new Dictionary<BattleWinRankKinds, float[]>();
			dictionary.Add(BattleWinRankKinds.S, new float[2]
			{
				1f,
				2f
			});
			dictionary.Add(BattleWinRankKinds.A, new float[2]
			{
				0.8f,
				1.5f
			});
			dictionary.Add(BattleWinRankKinds.B, new float[2]
			{
				0.5f,
				1.2f
			});
			dictionary.Add(BattleWinRankKinds.C, new float[2]
			{
				0f,
				1f
			});
			dictionary.Add(BattleWinRankKinds.D, new float[2]
			{
				0f,
				1f
			});
			dictionary.Add(BattleWinRankKinds.E, new float[2]
			{
				0f,
				1f
			});
			Dictionary<BattleWinRankKinds, float[]> dictionary2 = dictionary;
			int boss = mst_enemy.Boss;
			int num = 0;
			if (boss == 1 && Utils.IsBattleWin(rank))
			{
				num = mst_mapinfo.Clear_exp;
			}
			int member_exp = mst_mapinfo.Member_exp;
			return (int)((float)member_exp * dictionary2[rank][boss]) + num;
		}

		private int getUserExpPractice(BattleWinRankKinds rank, int myBasicLevel, int enemyBasicLevel)
		{
			Dictionary<BattleWinRankKinds, float> dictionary = new Dictionary<BattleWinRankKinds, float>();
			dictionary.Add(BattleWinRankKinds.S, 2f);
			dictionary.Add(BattleWinRankKinds.A, 1.5f);
			dictionary.Add(BattleWinRankKinds.B, 1.2f);
			dictionary.Add(BattleWinRankKinds.C, 1f);
			dictionary.Add(BattleWinRankKinds.D, 1f);
			dictionary.Add(BattleWinRankKinds.E, 1f);
			Dictionary<BattleWinRankKinds, float> dictionary2 = dictionary;
			List<int[]> list = new List<int[]>();
			list.Add(new int[2]
			{
				5,
				80
			});
			list.Add(new int[2]
			{
				3,
				60
			});
			list.Add(new int[2]
			{
				1,
				40
			});
			list.Add(new int[2]
			{
				0,
				30
			});
			list.Add(new int[2]
			{
				-2,
				20
			});
			list.Add(new int[2]
			{
				-3,
				10
			});
			List<int[]> list2 = list;
			int lvaway = enemyBasicLevel - myBasicLevel;
			int[] array = list2.FirstOrDefault((int[] x) => lvaway >= x[0]);
			int num = (array == null) ? list2[list2.Count - 1][1] : array[1];
			return (int)((float)num * dictionary2[rank]);
		}

		private void updateShip(BattleWinRankKinds rank, int mvpShip, SerializableDictionary<int, int> getShipExp, out SerializableDictionary<int, List<int>> lvupInfo)
		{
			lvupInfo = new SerializableDictionary<int, List<int>>();
			List<Mem_ship> shipData = F_Data.ShipData;
			int count = shipData.Count;
			bool flag = true;
			double num = 1.0;
			double num2 = 0.0;
			if (!practiceFlag)
			{
				flag = E_Data.ShipData.Exists((Mem_ship x) => (!Mst_DataManager.Instance.Mst_stype[x.Stype].IsSubmarine()) ? true : false);
				if (!flag)
				{
					num = 0.25;
					num2 = 0.5;
				}
			}
			Dictionary<int, int> dictionary = mst_shiplevel;
			for (int i = 0; i < count; i++)
			{
				Mem_ship mem_ship = shipData[i];
				Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
				Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[mem_shipBase.Ship_id];
				int addExp = getShipExp[mem_ship.Rid];
				List<int> lvupInfo2 = null;
				int levelupInfo = mem_ship.getLevelupInfo(dictionary, mem_shipBase.Level, mem_shipBase.Exp, ref addExp, out lvupInfo2);
				lvupInfo.Add(mem_ship.Rid, lvupInfo2);
				if (!mem_ship.Escape_sts)
				{
					mem_shipBase.Level = levelupInfo;
					mem_shipBase.Exp += addExp;
					mem_shipBase.Fuel -= mst_ship.Use_fuel;
					if (mem_shipBase.Fuel < 0)
					{
						mem_shipBase.Fuel = 0;
					}
					int num3 = mst_ship.Use_bull;
					if (battleKinds == ExecBattleKinds.DayToNight)
					{
						num3 = (int)Math.Ceiling((double)num3 * 1.5);
					}
					num3 = (int)((double)num3 * num + num2);
					if (!flag && num3 <= 0)
					{
						num3 = 1;
					}
					mem_shipBase.Bull -= num3;
					if (mem_shipBase.Bull < 0)
					{
						mem_shipBase.Bull = 0;
					}
					setCondSubValue(ref mem_shipBase.Cond);
					bool mvp = (mvpShip == mem_ship.Rid) ? true : false;
					bool flag2 = (F_SubInfo[mem_ship.Rid].DeckIdx == 0) ? true : false;
					setCondBonus(rank, flag2, mvp, ref mem_shipBase.Cond);
					int num4 = levelupInfo - mem_ship.Level;
					Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary2 = mem_ship.Kyouka;
					for (int j = 0; j < num4; j++)
					{
						dictionary2 = mem_ship.getLevelupKyoukaValue(mem_ship.Ship_id, dictionary2);
					}
					mem_shipBase.SetKyoukaValue(dictionary2);
					if (mem_ship.Get_DamageState() >= DamageState.Tyuuha)
					{
						clothBrokenIds.Add(mem_ship.Ship_id);
					}
					if (practiceFlag)
					{
						mem_shipBase.Nowhp = F_Data.StartHp[i];
					}
					else if (mem_shipBase.Nowhp <= 0)
					{
						deleteTargetShip.Add(mem_ship);
					}
					int value = 0;
					int value2 = 0;
					dictionary.TryGetValue(mem_shipBase.Level - 1, out value);
					dictionary.TryGetValue(mem_shipBase.Level + 1, out value2);
					mem_ship.SetRequireExp(mem_shipBase.Level, dictionary);
					mem_ship.SumLovToBattle(rank, flag2, mvp, F_Data.StartHp[i], mem_ship.Nowhp);
					mem_shipBase.Lov = mem_ship.Lov;
					mem_ship.Set_ShipParam(mem_shipBase, mst_ship, enemy_flag: false);
				}
			}
		}

		private void updateShipPracticeEnemy(SerializableDictionary<int, int> getShipExp, ref SerializableDictionary<int, List<int>> lvupInfo)
		{
			List<Mem_ship> shipData = E_Data.ShipData;
			Dictionary<int, int> mst_level = mst_shiplevel;
			foreach (Mem_ship item in shipData)
			{
				Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[item.Rid];
				Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
				Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship[mem_shipBase.Ship_id];
				int addExp = getShipExp[mem_ship.Rid];
				List<int> lvupInfo2 = null;
				int levelupInfo = mem_ship.getLevelupInfo(mst_level, mem_shipBase.Level, mem_shipBase.Exp, ref addExp, out lvupInfo2);
				lvupInfo.Add(mem_ship.Rid, lvupInfo2);
				mem_shipBase.Level = levelupInfo;
				mem_shipBase.Exp += addExp;
				int num = levelupInfo - mem_ship.Level;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary = mem_ship.Kyouka;
				for (int i = 0; i < num; i++)
				{
					dictionary = mem_ship.getLevelupKyoukaValue(mem_ship.Ship_id, dictionary);
				}
				mem_shipBase.SetKyoukaValue(dictionary);
				mem_ship.SetRequireExp(mem_shipBase.Level, mst_level);
				mem_ship.Set_ShipParam(mem_shipBase, mst_data, enemy_flag: false);
			}
		}

		private void setCondSubValue(ref int cond)
		{
			if (!practiceFlag)
			{
				if (battleKinds == ExecBattleKinds.DayOnly || battleKinds == ExecBattleKinds.DayToNight || battleKinds == ExecBattleKinds.NithtToDay)
				{
					int num = 3;
					if (Mem_ship.Get_FatitgueState(cond) >= FatigueState.Distress)
					{
						num += 6;
					}
					cond -= num;
				}
				if (battleKinds == ExecBattleKinds.DayToNight || battleKinds == ExecBattleKinds.NightOnly || battleKinds == ExecBattleKinds.NithtToDay)
				{
					cond -= 2;
				}
			}
			if (0 > cond)
			{
				cond = 0;
			}
		}

		private void setCondBonus(BattleWinRankKinds rank, bool flagship, bool mvp, ref int cond)
		{
			if (rank != BattleWinRankKinds.E)
			{
				int num = 0;
				switch (rank)
				{
				case BattleWinRankKinds.S:
					num = 4;
					break;
				case BattleWinRankKinds.A:
					num = 3;
					break;
				case BattleWinRankKinds.B:
					num = 2;
					break;
				case BattleWinRankKinds.C:
					num = 1;
					break;
				}
				if (flagship)
				{
					num += 3;
				}
				if (mvp)
				{
					num += 10;
				}
				cond += num;
				if (cond > 100)
				{
					cond = 100;
				}
			}
		}

		private void deleteLostShip()
		{
			if (deleteTargetShip.Count != 0)
			{
				deleteTargetShip.ForEach(delegate(Mem_ship x)
				{
					F_Data.Deck.Ship.RemoveShip(x.Rid);
					Comm_UserDatas.Instance.User_record.AddLostShipCount();
				});
				Comm_UserDatas.Instance.Remove_Ship(deleteTargetShip);
			}
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}

		public override void Dispose()
		{
		}
	}
}
