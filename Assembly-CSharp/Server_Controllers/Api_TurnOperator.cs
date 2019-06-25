using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_TurnOperator
	{
		private Mem_turn turnInstance;

		private Mem_basic basicInstance;

		private Api_get_Member getInstance;

		private Random randInstance;

		public Api_TurnOperator()
		{
			turnInstance = Comm_UserDatas.Instance.User_turn;
			basicInstance = Comm_UserDatas.Instance.User_basic;
			getInstance = new Api_get_Member();
			randInstance = new Random();
		}

		public TurnWorkResult ExecTurnStateChange(ITurnOperator instance, bool force, TurnState state)
		{
			if (instance == null)
			{
				return null;
			}
			if (force)
			{
				return ExecOwnEndWork();
			}
			switch (state)
			{
			case TurnState.CONTINOUS:
				return null;
			case TurnState.TURN_START:
				return ExecTurnStartWork();
			case TurnState.OWN_END:
				return ExecOwnEndWork();
			case TurnState.ENEMY_START:
				return ExecEnemyStartWork();
			case TurnState.ENEMY_END:
				return ExecEnemyEndWork();
			case TurnState.TURN_END:
				return ExecTurnEndWork();
			default:
				return null;
			}
		}

		private TurnWorkResult ExecTurnStartWork()
		{
			IEnumerable<int> collection = from ndock in Comm_UserDatas.Instance.User_ndock.Values
				where !ndock.IsRecoverEndTime() && ndock.State == NdockStates.RESTORE
				select ndock.Ship_id;
			HashSet<int> hashSet = new HashSet<int>(collection);
			IEnumerable<int> collection2 = from x in Comm_UserDatas.Instance.User_deck.Values
				select x.Ship[0];
			HashSet<int> hashSet2 = new HashSet<int>(collection2);
			foreach (Mem_ship value in Comm_UserDatas.Instance.User_ship.Values)
			{
				value.AddTurnRecoveryCond(this, 10);
				value.SumLovToTurnStart(hashSet.Contains(value.Rid), hashSet2.Contains(value.Rid));
			}
			TurnWorkResult turnWorkResult = new TurnWorkResult();
			turnWorkResult.ChangeState = TurnState.CONTINOUS;
			turnWorkResult.TransportMaterial = new Dictionary<enumMaterialCategory, int>();
			Dictionary<enumMaterialCategory, int> summaryBase = new Dictionary<enumMaterialCategory, int>();
			foreach (object value2 in Enum.GetValues(typeof(enumMaterialCategory)))
			{
				turnWorkResult.TransportMaterial.Add((enumMaterialCategory)(int)value2, 0);
				summaryBase.Add((enumMaterialCategory)(int)value2, 0);
			}
			TakeMaterial(ref turnWorkResult.TransportMaterial, ref summaryBase);
			setSummaryMaterialInfo(summaryBase, out turnWorkResult.BonusMaterialMonthly, out turnWorkResult.BonusMaterialWeekly);
			foreach (Mem_deck value3 in Comm_UserDatas.Instance.User_deck.Values)
			{
				if (value3.MissionState == MissionStates.NONE)
				{
					repairShipAutoRecovery(value3.Ship);
				}
			}
			getInstance.Deck_Port();
			turnWorkResult.MissionEndDecks = (from x in Comm_UserDatas.Instance.User_deck.Values
				where x.MissionState == MissionStates.END || (x.MissionState == MissionStates.STOP && x.CompleteTime == 0)
				select x).ToList();
			ExecBling_Ship(out turnWorkResult.BlingEndShip);
			ExecBling_EscortDeck(out turnWorkResult.BlingEndEscortDeck);
			ExecBling_Tanker(out turnWorkResult.BlingEndTanker);
			setTurnRewardItem(out turnWorkResult.SpecialItem);
			updateRewardItem(turnWorkResult.SpecialItem);
			return turnWorkResult;
		}

		private void TakeMaterial(ref Dictionary<enumMaterialCategory, int> add_mat, ref Dictionary<enumMaterialCategory, int> summaryBase)
		{
			IEnumerable<IGrouping<int, Mem_tanker>> areaEnableTanker = Mem_tanker.GetAreaEnableTanker(Comm_UserDatas.Instance.User_tanker);
			if (areaEnableTanker.Count() != 0)
			{
				foreach (IGrouping<int, Mem_tanker> item in areaEnableTanker)
				{
					Mst_maparea mst_maparea = Mst_DataManager.Instance.Mst_maparea[item.Key];
					DeckShips ship = Comm_UserDatas.Instance.User_EscortDeck[item.Key].Ship;
					mst_maparea.TakeMaterialNum(Comm_UserDatas.Instance.User_mapclear, item.Count(), ref add_mat, randMaxFlag: false, ship);
					mst_maparea.TakeMaterialNum(Comm_UserDatas.Instance.User_mapclear, item.Count(), ref summaryBase, randMaxFlag: true, ship);
				}
				int materialMaxNum = Comm_UserDatas.Instance.User_basic.GetMaterialMaxNum();
				foreach (KeyValuePair<enumMaterialCategory, int> item2 in add_mat)
				{
					int num = 0;
					int num2 = Comm_UserDatas.Instance.User_material[item2.Key].Value + item2.Value;
					int num3 = materialMaxNum - num2;
					if (num3 >= 0)
					{
						num = item2.Value;
					}
					else if (materialMaxNum > Comm_UserDatas.Instance.User_material[item2.Key].Value)
					{
						num = materialMaxNum - Comm_UserDatas.Instance.User_material[item2.Key].Value;
					}
					Comm_UserDatas.Instance.User_material[item2.Key].Add_Material(num);
				}
			}
		}

		private void setSummaryMaterialInfo(Dictionary<enumMaterialCategory, int> baseInfo, out Dictionary<enumMaterialCategory, int> monthlyInfo, out Dictionary<enumMaterialCategory, int> weeklyInfo)
		{
			monthlyInfo = null;
			weeklyInfo = null;
			DateTime dateTime = Comm_UserDatas.Instance.User_turn.GetDateTime();
			if (dateTime.Day == 1)
			{
				double keisu = 5.0;
				double tani = 10.0;
				HashSet<enumMaterialCategory> hashSet = new HashSet<enumMaterialCategory>();
				hashSet.Add(enumMaterialCategory.Fuel);
				hashSet.Add(enumMaterialCategory.Bull);
				hashSet.Add(enumMaterialCategory.Steel);
				hashSet.Add(enumMaterialCategory.Bauxite);
				HashSet<enumMaterialCategory> takeTarget = hashSet;
				monthlyInfo = takeBonusMaterial(baseInfo, takeTarget, keisu, tani);
				int value = takeBonusDevKit();
				monthlyInfo.Add(enumMaterialCategory.Dev_Kit, value);
			}
			if (dateTime.DayOfWeek == DayOfWeek.Sunday)
			{
				double keisu = 2.5;
				double tani = 5.0;
				double randDouble = Utils.GetRandDouble(0.0, 2.0, 1.0, 1);
				enumMaterialCategory item = (randDouble == 0.0) ? enumMaterialCategory.Fuel : ((randDouble != 1.0) ? enumMaterialCategory.Steel : enumMaterialCategory.Bull);
				HashSet<enumMaterialCategory> hashSet = new HashSet<enumMaterialCategory>();
				hashSet.Add(item);
				HashSet<enumMaterialCategory> takeTarget2 = hashSet;
				weeklyInfo = takeBonusMaterial(baseInfo, takeTarget2, keisu, tani);
			}
		}

		private Dictionary<enumMaterialCategory, int> takeBonusMaterial(Dictionary<enumMaterialCategory, int> baseData, HashSet<enumMaterialCategory> takeTarget, double keisu, double tani)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			Mst_item_limit mst_item_limit = Mst_DataManager.Instance.Mst_item_limit[1];
			foreach (enumMaterialCategory item in takeTarget)
			{
				int materialLimit = mst_item_limit.GetMaterialLimit(Mst_DataManager.Instance.Mst_item_limit, item);
				double a = (double)baseData[item] * keisu;
				int num = (int)(Math.Ceiling(a) / tani);
				int num2 = (int)(tani * (double)num);
				int num3 = num2 + Comm_UserDatas.Instance.User_material[item].Value;
				int num4 = materialLimit - num3;
				if (num4 < 0)
				{
					num2 = materialLimit - Comm_UserDatas.Instance.User_material[item].Value;
					if (num2 < 0)
					{
						num2 = 0;
					}
				}
				dictionary.Add(item, num2);
				Comm_UserDatas.Instance.User_material[item].Add_Material(num2);
			}
			return dictionary;
		}

		private int takeBonusDevKit()
		{
			Dictionary<int, Mst_maparea>.ValueCollection values = Mst_DataManager.Instance.Mst_maparea.Values;
			Comm_UserDatas instance = Comm_UserDatas.Instance;
			int num = 0;
			foreach (Mst_maparea item in values)
			{
				int mapinfo_no = Mst_maparea.MaxMapNum(Comm_UserDatas.Instance.User_basic.Difficult, item.Id);
				int key = Mst_mapinfo.ConvertMapInfoId(item.Id, mapinfo_no);
				Mem_mapclear value = null;
				if (instance.User_mapclear.TryGetValue(key, out value) && value.State == MapClearState.Cleard)
				{
					num++;
				}
			}
			int num2 = num + 4;
			int value2 = Comm_UserDatas.Instance.User_material[enumMaterialCategory.Dev_Kit].Value;
			int num3 = value2 + num2;
			int materialLimit = Mst_DataManager.Instance.Mst_item_limit[1].GetMaterialLimit(Mst_DataManager.Instance.Mst_item_limit, enumMaterialCategory.Dev_Kit);
			if (num3 > materialLimit)
			{
				num2 = materialLimit - value2;
				if (num2 < 0)
				{
					num2 = 0;
				}
			}
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Dev_Kit].Add_Material(num2);
			return num2;
		}

		private void ExecBling_Ship(out List<int> out_fmt)
		{
			out_fmt = new List<int>();
			foreach (KeyValuePair<int, Mem_ship> item in Comm_UserDatas.Instance.User_ship)
			{
				if (item.Value.IsBlingShip() && item.Value.BlingTerm())
				{
					out_fmt.Add(item.Key);
				}
			}
		}

		private void ExecBling_EscortDeck(out List<int> out_fmt)
		{
			out_fmt = new List<int>();
			foreach (KeyValuePair<int, Mem_esccort_deck> item in Comm_UserDatas.Instance.User_EscortDeck)
			{
				if (item.Value.BlingTerm())
				{
					out_fmt.Add(item.Key);
				}
			}
		}

		private void ExecBling_Tanker(out Dictionary<int, List<Mem_tanker>> out_fmt)
		{
			out_fmt = new Dictionary<int, List<Mem_tanker>>();
			IEnumerable<IGrouping<int, Mem_tanker>> enumerable = from tanker in Comm_UserDatas.Instance.User_tanker.Values
				where tanker.BlingTerm()
				let area_id = (tanker.Disposition_status != DispositionStatus.NONE) ? tanker.Maparea_id : 0
				group tanker by area_id;
			foreach (IGrouping<int, Mem_tanker> item in enumerable)
			{
				out_fmt.Add(item.Key, item.ToList());
			}
		}

		private void repairShipAutoRecovery(DeckShips deck)
		{
			int num = 86;
			int num2 = 19;
			List<Mem_ship> memShip = deck.getMemShip();
			if (memShip.Count() == 0 || memShip[0].Stype != num2)
			{
				return;
			}
			DamageState damageState = memShip[0].Get_DamageState();
			if (damageState != DamageState.Tyuuha && damageState != DamageState.Taiha && !memShip[0].ExistsNdock() && !memShip[0].IsBlingShip())
			{
				Mem_material mem_material = Comm_UserDatas.Instance.User_material[enumMaterialCategory.Fuel];
				Mem_material mem_material2 = Comm_UserDatas.Instance.User_material[enumMaterialCategory.Steel];
				if (mem_material.Value != 0 || mem_material2.Value != 0)
				{
					Dictionary<int, int> mstSlotItemNum_OrderId = memShip[0].GetMstSlotItemNum_OrderId(new HashSet<int>
					{
						num
					});
					int num3 = mstSlotItemNum_OrderId[num];
					memShip = memShip.Take(num3 + 2).ToList();
					foreach (Mem_ship item in memShip)
					{
						if (item.Nowhp < item.Maxhp)
						{
							DamageState damageState2 = item.Get_DamageState();
							if (damageState2 != DamageState.Tyuuha && damageState2 != DamageState.Taiha && !item.ExistsNdock() && !item.IsBlingShip())
							{
								int ndockTimeSpan = item.GetNdockTimeSpan();
								int num4 = ndockTimeSpan * 30;
								int num5 = 30;
								double num6 = (double)num5 / (double)num4;
								Dictionary<enumMaterialCategory, int> ndockMaterialNum = item.GetNdockMaterialNum();
								int num7 = (int)Math.Ceiling((double)ndockMaterialNum[enumMaterialCategory.Fuel] * num6);
								int num8 = (int)Math.Ceiling((double)ndockMaterialNum[enumMaterialCategory.Steel] * num6);
								if (mem_material.Value >= num7 && mem_material2.Value >= num8)
								{
									double num9 = (double)(item.Maxhp - item.Nowhp) * num6;
									int num10 = (!(num9 < 1.0)) ? ((int)num9) : ((int)Math.Ceiling(num9));
									item.SubHp(-num10);
									mem_material.Sub_Material(num7);
									mem_material2.Sub_Material(num8);
								}
							}
						}
					}
				}
			}
		}

		private void setTurnRewardItem(out List<ItemGetFmt> turnReward)
		{
			turnReward = null;
			List<ItemGetFmt> list = new List<ItemGetFmt>();
			DateTime dateTime = turnInstance.GetDateTime();
			if (dateTime.Month == 2 && dateTime.Day == 14)
			{
				ItemGetFmt itemGetFmt = new ItemGetFmt();
				itemGetFmt.Category = ItemGetKinds.UseItem;
				itemGetFmt.Id = 56;
				itemGetFmt.Count = 1;
				list.Add(itemGetFmt);
			}
			if (list.Count > 0)
			{
				turnReward = list;
			}
		}

		private void updateRewardItem(List<ItemGetFmt> turnReward)
		{
			turnReward?.ForEach(delegate(ItemGetFmt x)
			{
				if (x.Category == ItemGetKinds.UseItem)
				{
					Comm_UserDatas.Instance.Add_Useitem(x.Id, x.Count);
				}
			});
		}

		private TurnWorkResult ExecOwnEndWork()
		{
			TurnWorkResult turnWorkResult = new TurnWorkResult();
			turnWorkResult.ChangeState = TurnState.ENEMY_START;
			return turnWorkResult;
		}

		private TurnWorkResult ExecEnemyStartWork()
		{
			TurnWorkResult turnWorkResult = new TurnWorkResult();
			turnWorkResult.ChangeState = TurnState.ENEMY_END;
			return turnWorkResult;
		}

		private TurnWorkResult ExecEnemyEndWork()
		{
			TurnWorkResult turnWorkResult = new TurnWorkResult();
			turnWorkResult.ChangeState = TurnState.TURN_END;
			return turnWorkResult;
		}

		private TurnWorkResult ExecTurnEndWork()
		{
			TurnWorkResult turnWorkResult = new TurnWorkResult();
			turnWorkResult.ChangeState = TurnState.TURN_START;
			turnInstance.AddTurn(this);
			if (turnInstance.GetDateTime().Day == 1)
			{
				new Api_req_Quest(this).EnforceQuestReset();
			}
			if (Utils.IsTurnOver())
			{
				Mem_history mem_history = new Mem_history();
				mem_history.SetGameOverToTurn(turnInstance.Total_turn);
				Comm_UserDatas.Instance.Add_History(mem_history);
			}
			foreach (Mem_ship value in Comm_UserDatas.Instance.User_ship.Values)
			{
				value.BlingWaitToStart();
				value.PurgeLovTouchData();
			}
			Comm_UserDatas.Instance.UpdateEscortShipLocale();
			Comm_UserDatas.Instance.UpdateDeckShipLocale();
			List<Mem_deck> list = Comm_UserDatas.Instance.User_deck.Values.ToList();
			list.ForEach(delegate(Mem_deck x)
			{
				x.ActionStart();
			});
			List<Mst_radingtype> types = Mst_DataManager.Instance.Mst_RadingType[(int)basicInstance.Difficult];
			Mst_radingtype radingRecord = Mst_radingtype.GetRadingRecord(types, turnInstance.Total_turn);
			new HashSet<int>();
			if (radingRecord != null)
			{
				double randDouble = Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if ((double)radingRecord.Rading_rate >= randDouble)
				{
					IEnumerable<IGrouping<int, Mem_tanker>> areaEnableTanker = Mem_tanker.GetAreaEnableTanker(Comm_UserDatas.Instance.User_tanker);
					Dictionary<int, RadingKind> radingArea = getRadingArea(areaEnableTanker, radingRecord.Rading_type);
					if (radingArea.Count > 0)
					{
						Dictionary<int, List<Mem_tanker>> tankers = areaEnableTanker.ToDictionary((IGrouping<int, Mem_tanker> x) => x.Key, (IGrouping<int, Mem_tanker> y) => y.ToList());
						turnWorkResult.RadingResult = getRadingResult(radingRecord.Rading_type, radingArea, tankers);
					}
				}
			}
			updateRadingEscortShipExp(turnWorkResult.RadingResult);
			if (basicInstance.Difficult == DifficultKind.KOU || basicInstance.Difficult == DifficultKind.SHI)
			{
				addRebellionPoint();
				selectRegistanceArea();
			}
			else
			{
				selectRegistanceArea();
				addRebellionPoint();
			}
			return turnWorkResult;
		}

		private void updateRadingEscortShipExp(List<RadingResultData> radingData)
		{
			HashSet<int> radingArea = new HashSet<int>();
			if (radingData != null && radingData.Count > 0)
			{
				radingData.ForEach(delegate(RadingResultData x)
				{
					radingArea.Add(x.AreaId);
				});
			}
			Dictionary<int, int> mstLevel = ArrayMaster.GetMstLevel();
			foreach (Mem_esccort_deck value3 in Comm_UserDatas.Instance.User_EscortDeck.Values)
			{
				if (value3.Ship.Count() > 0)
				{
					int num = radingArea.Contains(value3.Maparea_id) ? 1 : 2;
					List<Mem_ship> memShip = value3.Ship.getMemShip();
					foreach (Mem_ship item in memShip)
					{
						if (item.IsFight() && !item.IsBlingShip())
						{
							double num2 = Math.Sqrt(item.Level);
							int addExp = (num != 1) ? ((int)(num2 * (1.0 + Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 0.5))) : ((int)(num2 * (20.0 + Utils.GetRandDouble(0.0, 80.0, 1.0, 1) + 0.5)));
							Mem_shipBase mem_shipBase = new Mem_shipBase(item);
							List<int> lvupInfo = new List<int>();
							int num3 = mem_shipBase.Level = item.getLevelupInfo(mstLevel, mem_shipBase.Level, mem_shipBase.Exp, ref addExp, out lvupInfo);
							mem_shipBase.Exp += addExp;
							int num4 = num3 - item.Level;
							Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary = item.Kyouka;
							for (int i = 0; i < num4; i++)
							{
								dictionary = item.getLevelupKyoukaValue(item.Ship_id, dictionary);
							}
							mem_shipBase.SetKyoukaValue(dictionary);
							int value = 0;
							int value2 = 0;
							mstLevel.TryGetValue(mem_shipBase.Level - 1, out value);
							mstLevel.TryGetValue(mem_shipBase.Level + 1, out value2);
							item.SetRequireExp(mem_shipBase.Level, mstLevel);
							Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship[mem_shipBase.Ship_id];
							item.Set_ShipParam(mem_shipBase, mst_data, enemy_flag: false);
						}
					}
				}
			}
		}

		private List<RadingResultData> getRadingResult(int radingPhase, Dictionary<int, RadingKind> targetArea, Dictionary<int, List<Mem_tanker>> tankers)
		{
			DifficultKind difficult = basicInstance.Difficult;
			List<RadingResultData> list = new List<RadingResultData>();
			foreach (KeyValuePair<int, RadingKind> item in targetArea)
			{
				int key = item.Key;
				List<Mem_tanker> list2 = tankers[key];
				Mst_radingrate mst_radingrate = Mst_DataManager.Instance.Mst_RadingRate[key][radingPhase];
				int count = list2.Count;
				List<Mem_ship> memShip = Comm_UserDatas.Instance.User_EscortDeck[key].Ship.getMemShip();
				RadingResultData radingResultData = new RadingResultData();
				radingResultData.AreaId = key;
				radingResultData.AttackKind = item.Value;
				radingResultData.BeforeNum = list2.Count;
				List<Mem_ship> deleteShips = null;
				if (memShip.Count > 0)
				{
					radingResultData.FlagShipMstId = memShip[0].Ship_id;
					radingResultData.FlagShipDamageState = memShip[0].Get_DamageState();
				}
				radingResultData.RadingDamage = getRadingDamage(key, item.Value, mst_radingrate, memShip, out radingResultData.DeckAttackPow, out deleteShips);
				radingResultData.BreakNum = getRadingTankerLostNum(item.Value, memShip, count, mst_radingrate);
				IEnumerable<Mem_tanker> enumerable = list2.Take(radingResultData.BreakNum);
				if (radingResultData.RadingDamage.Count > 0)
				{
					List<int> list3 = new List<int>();
					using (List<Mem_ship>.Enumerator enumerator2 = memShip.GetEnumerator())
					{
						Mem_ship checkShip;
						while (enumerator2.MoveNext())
						{
							checkShip = enumerator2.Current;
							if (radingResultData.RadingDamage.Any((RadingDamageData x) => x.Rid == checkShip.Rid && x.Damage) && checkShip.Get_DamageState() >= DamageState.Tyuuha)
							{
								list3.Add(checkShip.Ship_id);
							}
						}
					}
					Comm_UserDatas.Instance.UpdateShipBookBrokenClothState(list3);
				}
				foreach (Mem_ship item2 in deleteShips)
				{
					Comm_UserDatas.Instance.User_EscortDeck[key].Ship.RemoveShip(item2.Rid);
					Comm_UserDatas.Instance.User_record.AddLostShipCount();
				}
				Comm_UserDatas.Instance.Remove_Ship(deleteShips);
				foreach (Mem_tanker item3 in enumerable)
				{
					Comm_UserDatas.Instance.Remove_Tanker(item3.Rid);
				}
				if (radingResultData.BeforeNum > 0)
				{
					double num = (double)radingResultData.BreakNum / (double)radingResultData.BeforeNum;
					if (num >= 0.5)
					{
						int mapinfo_id = Mst_mapinfo.ConvertMapInfoId(key, 1);
						Mem_history mem_history = new Mem_history();
						mem_history.SetTanker(turnInstance.Total_turn, mapinfo_id, (num >= 1.0) ? true : false);
						Comm_UserDatas.Instance.Add_History(mem_history);
					}
				}
				list.Add(radingResultData);
			}
			return list;
		}

		private Dictionary<int, RadingKind> getRadingArea(IEnumerable<IGrouping<int, Mem_tanker>> tankerInfo, int radingType)
		{
			Mst_DataManager instance = Mst_DataManager.Instance;
			Dictionary<int, RadingKind> dictionary = new Dictionary<int, RadingKind>();
			foreach (IGrouping<int, Mem_tanker> item in tankerInfo)
			{
				int key = item.Key;
				int sc = item.Count();
				int req_tanker_num = instance.Mst_maparea[key].Req_tanker_num;
				List<Mem_ship> memShip = Comm_UserDatas.Instance.User_EscortDeck[key].Ship.getMemShip();
				int ec = 0;
				int ad = 0;
				int ad2 = 0;
				memShip.ForEach(delegate(Mem_ship ship)
				{
					if (ship.IsEscortDeffender())
					{
						ec++;
					}
					ad += ship.Taiku;
					ad2 += ship.Taisen;
				});
				Mst_radingrate radingRecord = instance.Mst_RadingRate[key][radingType];
				if (isRadingSubmarine(radingRecord, ec, ad2))
				{
					dictionary.Add(key, RadingKind.SUBMARINE_ATTACK);
				}
				else if (isRadingAir(radingRecord, sc, ec, ad))
				{
					dictionary.Add(key, RadingKind.AIR_ATTACK);
				}
			}
			return dictionary;
		}

		private bool isRadingSubmarine(Mst_radingrate radingRecord, int ec, int ad2)
		{
			int submarine_rate = radingRecord.Submarine_rate;
			int submarine_karyoku = radingRecord.Submarine_karyoku;
			int num = submarine_rate - ec * 2;
			double randDouble = Utils.GetRandDouble(1.0, 100.0 + Math.Sqrt(ad2), 1.0, 1);
			if ((double)num > randDouble && (double)ec < Utils.GetRandDouble(1.0, 10.0, 1.0, 1))
			{
				return true;
			}
			return false;
		}

		private bool isRadingAir(Mst_radingrate radingRecord, int sc, int ec, int ad1)
		{
			int air_rate = radingRecord.Air_rate;
			int air_karyoku = radingRecord.Air_karyoku;
			int num = air_rate + sc - ec * 3;
			double randDouble = Utils.GetRandDouble(1.0, 100.0 + Math.Sqrt(ad1), 1.0, 1);
			if ((double)num > randDouble && (double)ec < Utils.GetRandDouble(1.0, 10.0, 1.0, 1))
			{
				return true;
			}
			return false;
		}

		private List<RadingDamageData> getRadingDamage(int area, RadingKind kind, Mst_radingrate rateRecord, List<Mem_ship> targetShips, out int deckPow, out List<Mem_ship> deleteShips)
		{
			List<Mem_ship> list = targetShips.ToList();
			int ec = 0;
			double ad3 = 0.0;
			double ad2 = 0.0;
			list.ForEach(delegate(Mem_ship ship)
			{
				if (ship.IsEscortDeffender())
				{
					ec++;
				}
				Ship_GrowValues battleBaseParam = ship.GetBattleBaseParam();
				int num9 = ship.Taiku - battleBaseParam.Taiku;
				ad3 = ad3 + Math.Sqrt(battleBaseParam.Taiku) + (double)num9;
				int num10 = ship.Taisen - battleBaseParam.Taisen;
				ad2 = ad2 + Math.Sqrt(battleBaseParam.Taisen) + (double)num10;
			});
			int[] radingValues = rateRecord.GetRadingValues(kind);
			double num = (kind != RadingKind.AIR_ATTACK) ? ad2 : ad3;
			int num11 = radingValues[0];
			int num2 = radingValues[1];
			deckPow = (int)num;
			deleteShips = new List<Mem_ship>();
			if (list.Count == 0)
			{
				return new List<RadingDamageData>();
			}
			RadingResultData radingResultData = new RadingResultData();
			radingResultData.DeckAttackPow = (int)num;
			double num3 = (double)num2 - Math.Sqrt(ec);
			int num4 = (!(num3 < 1.0)) ? ((int)Utils.GetRandDouble(0.0, num3, 0.1, 1)) : 0;
			List<RadingDamageData> list2 = new List<RadingDamageData>();
			Dictionary<int, DamageState> dictionary = (from x in list
				select new
				{
					rid = x.Rid,
					state = x.Get_DamageState()
				}).ToDictionary(key => key.rid, val => val.state);
			for (int i = 0; i < num4; i++)
			{
				if (list.Count == 0)
				{
					return list2;
				}
				RadingDamageData radingDamageData = new RadingDamageData();
				double num5 = (double)(num2 * 5) - num / 5.0 - Math.Sqrt(num);
				int index = (int)Utils.GetRandDouble(0.0, list.Count - 1, 1.0, 1);
				Mem_ship mem_ship = list[index];
				radingDamageData.Rid = mem_ship.Rid;
				if (num5 <= 0.0)
				{
					radingDamageData.Damage = false;
					radingDamageData.DamageState = DamagedStates.None;
				}
				else
				{
					int taik = Mst_DataManager.Instance.Mst_ship[mem_ship.Ship_id].Taik;
					int num6 = (int)((double)taik * Utils.GetRandDouble(1.0, num5, 1.0, 1) / 100.0) + 1;
					int num7 = mem_ship.Nowhp - num6;
					if (num7 <= 0)
					{
						num7 = ((basicInstance.Difficult != DifficultKind.SHI) ? 1 : ((dictionary[mem_ship.Rid] != DamageState.Taiha) ? 1 : 0));
					}
					int num8 = mem_ship.Nowhp - num7;
					if (num8 > 0)
					{
						DamageState damageState = mem_ship.Get_DamageState();
						radingDamageData.Damage = true;
						if (num7 == 0)
						{
							int[] array = mem_ship.FindRecoveryItem();
							if (array[0] == -1)
							{
								radingDamageData.DamageState = DamagedStates.Gekichin;
								list.Remove(mem_ship);
								deleteShips.Add(mem_ship);
							}
							else
							{
								mem_ship.SubHp(num8);
								mem_ship.UseRecoveryItem(array, flagShipRecovery: false);
								radingDamageData.DamageState = ((array[1] != 43) ? DamagedStates.Youin : DamagedStates.Megami);
								dictionary[mem_ship.Rid] = DamageState.Normal;
							}
						}
						else
						{
							mem_ship.SubHp(num8);
							DamageState damageState2 = mem_ship.Get_DamageState();
							if (damageState != damageState2)
							{
								switch (damageState2)
								{
								case DamageState.Taiha:
									radingDamageData.DamageState = DamagedStates.Taiha;
									break;
								case DamageState.Shouha:
									radingDamageData.DamageState = DamagedStates.Shouha;
									break;
								case DamageState.Tyuuha:
									radingDamageData.DamageState = DamagedStates.Tyuuha;
									break;
								}
							}
							else
							{
								radingDamageData.DamageState = DamagedStates.None;
							}
						}
					}
					else
					{
						radingDamageData.Damage = false;
						radingDamageData.DamageState = DamagedStates.None;
					}
				}
				list2.Add(radingDamageData);
			}
			return list2;
		}

		private int getRadingTankerLostNum(RadingKind kind, List<Mem_ship> ships, int nowTankerNum, Mst_radingrate radingRecord)
		{
			int radingPow = radingRecord.GetRadingPow(kind);
			int num = ships.Count((Mem_ship x) => x.IsEscortDeffender());
			int num2 = 0;
			if (num == 0 || Utils.GetRandDouble(1.0, 12 - num, 1.0, 1) >= 6.0)
			{
				switch (num)
				{
				case 6:
				{
					double randDouble = Utils.GetRandDouble(0.0, Math.Sqrt(radingPow), 0.1, 1);
					num2 = (int)(randDouble + 0.5);
					break;
				}
				case 4:
				case 5:
				{
					double randDouble3 = Utils.GetRandDouble(0.0, Math.Sqrt(radingPow / 2), 0.1, 1);
					num2 = (int)(randDouble3 + 0.5);
					break;
				}
				case 1:
				case 2:
				case 3:
				{
					double randDouble2 = Utils.GetRandDouble(0.0, Math.Sqrt(radingPow), 0.1, 1);
					num2 = (int)(randDouble2 + 1.5);
					break;
				}
				default:
					if (num == 0)
					{
						return nowTankerNum;
					}
					break;
				}
			}
			if (num2 > nowTankerNum)
			{
				return nowTankerNum;
			}
			return num2;
		}

		private void selectRegistanceArea()
		{
			foreach (Mem_rebellion_point value in Comm_UserDatas.Instance.User_rebellion_point.Values)
			{
				value.StartRebellion(this);
			}
		}

		private void addRebellionPoint()
		{
			RebellionUtils rebellionUtils = new RebellionUtils();
			List<Mst_maparea> list = new List<Mst_maparea>();
			double rpHitProbKeisu = getRpHitProbKeisu();
			if (turnInstance.GetDateTime().Day == 1)
			{
				list = Mst_DataManager.Instance.Mst_maparea.Values.ToList();
			}
			else
			{
				foreach (Mst_maparea value in Mst_DataManager.Instance.Mst_maparea.Values)
				{
					double randDouble = Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
					if (randDouble <= rpHitProbKeisu)
					{
						list.Add(value);
					}
				}
			}
			foreach (Mst_maparea item in list)
			{
				rebellionUtils.AddPointTo_RPTable(item);
			}
		}

		private double getRpHitProbKeisu()
		{
			switch (Comm_UserDatas.Instance.User_basic.Difficult)
			{
			case DifficultKind.TEI:
				return 5.0;
			case DifficultKind.HEI:
				return 10.0;
			case DifficultKind.OTU:
				return 12.0;
			case DifficultKind.KOU:
				return 16.0;
			default:
				return 20.0;
			}
		}
	}
}
