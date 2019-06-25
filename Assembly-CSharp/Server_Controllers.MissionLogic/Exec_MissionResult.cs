using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers.MissionLogic
{
	public class Exec_MissionResult : IRebellionPointOperator
	{
		private List<Mem_ship> mem_ships;

		private Mem_deck mem_deck;

		private Dictionary<int, int> mst_level;

		private Dictionary<int, int> mst_userlevel;

		private Mst_mission2 mst_mission;

		private MissionResultFmt resultInfo;

		private Random randInstance;

		private List<Mem_tanker> missionTanker;

		private int daihatuNum;

		private int drumNum;

		private int drumShipNum;

		public Exec_MissionResult(Mem_deck deck, Dictionary<int, int> mst_level, Dictionary<int, int> mst_userlevel)
		{
			daihatuNum = 0;
			drumNum = 0;
			drumShipNum = 0;
			mem_deck = deck;
			this.mst_level = mst_level;
			this.mst_userlevel = mst_userlevel;
			mem_ships = deck.Ship.getMemShip();
			mst_mission = Mst_DataManager.Instance.Mst_mission[deck.Mission_id];
			missionTanker = (from x in Comm_UserDatas.Instance.User_tanker.Values
				where x.Mission_deck_rid == deck.Rid
				select x).ToList();
			randInstance = new Random();
		}

		void IRebellionPointOperator.AddRebellionPoint(int area_id, int addNum)
		{
			throw new NotImplementedException();
		}

		void IRebellionPointOperator.SubRebellionPoint(int area_id, int subNum)
		{
			Mem_rebellion_point value = null;
			if (Comm_UserDatas.Instance.User_rebellion_point.TryGetValue(area_id, out value))
			{
				value.SubPoint(this, subNum);
			}
		}

		public MissionResultFmt GetResultData()
		{
			resultInfo = new MissionResultFmt();
			resultInfo.Deck = mem_deck;
			resultInfo.MissionName = mst_mission.Name;
			setResultKind();
			Dictionary<enumMaterialCategory, int> up_mat = null;
			setItems(out up_mat);
			setBasicExp();
			Dictionary<int, int> shipExp = getShipExp();
			resultInfo.GetShipExp = shipExp;
			updateShip(shipExp, out resultInfo.LevelUpInfo);
			updateMaterial(up_mat);
			updateDeck();
			updateItem();
			updateMissionComp();
			updateBasic();
			updateRebellionPoint(resultInfo.MissionResult);
			resultInfo.MemberLevel = updateRecord();
			missionTanker.ForEach(delegate(Mem_tanker x)
			{
				x.MissionTerm();
			});
			return resultInfo;
		}

		private void setResultKind()
		{
			if (mem_deck.MissionState == MissionStates.STOP)
			{
				resultInfo.MissionResult = MissionResultKinds.FAILE;
				return;
			}
			if (mst_mission.Flagship_level > 0)
			{
				FatigueState fatigueState = mem_ships[0].Get_FatigueState();
				if (fatigueState >= FatigueState.Light)
				{
					resultInfo.MissionResult = MissionResultKinds.FAILE;
					return;
				}
				if (mst_mission.Flagship_level > mem_ships[0].Level)
				{
					resultInfo.MissionResult = MissionResultKinds.FAILE;
					return;
				}
			}
			if (mst_mission.Flagship_stype1 > 0 && mst_mission.Flagship_stype1 != mem_ships[0].Stype)
			{
				resultInfo.MissionResult = MissionResultKinds.FAILE;
				return;
			}
			if (mst_mission.Tanker_num > 0 && missionTanker.Count < mst_mission.Tanker_num)
			{
				resultInfo.MissionResult = MissionResultKinds.FAILE;
				return;
			}
			Dictionary<int, int> dictionary = (from data in Utils.Xml_Result("mst_stype_group", "mst_stype_group", "Id")
				let item = new
				{
					id = int.Parse(data.Element("Id").Value),
					type = int.Parse(data.Element("Mission").Value)
				}
				select item).ToDictionary(obj => obj.id, value => value.type);
			IEnumerable<int> source = dictionary.Values.Distinct();
			Dictionary<int, int> dictionary2 = source.ToDictionary((int type) => type, (int count) => 0);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			foreach (Mem_ship mem_ship in mem_ships)
			{
				FatigueState fatigueState2 = mem_ship.Get_FatigueState();
				if (fatigueState2 == FatigueState.Distress)
				{
					resultInfo.MissionResult = MissionResultKinds.FAILE;
					return;
				}
				Dictionary<int, int> mstSlotItemNum_OrderId = mem_ship.GetMstSlotItemNum_OrderId(new HashSet<int>
				{
					68,
					75
				});
				Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[mem_ship.Ship_id];
				daihatuNum += mstSlotItemNum_OrderId[68];
				if (mstSlotItemNum_OrderId[75] > 0)
				{
					drumShipNum++;
				}
				drumNum += mstSlotItemNum_OrderId[75];
				num7 += mem_ship.Level;
				num6 += mst_ship.Bull_max;
				num5 += mst_ship.Fuel_max;
				switch (fatigueState2)
				{
				case FatigueState.Light:
					num2++;
					continue;
				case FatigueState.Exaltation:
					num++;
					break;
				}
				num4 += mem_ship.Bull;
				num3 += mem_ship.Fuel;
				int num8 = dictionary[mem_ship.Stype];
				Dictionary<int, int> dictionary3;
				Dictionary<int, int> dictionary4 = dictionary3 = dictionary2;
				int key;
				int key2 = key = num8;
				key = dictionary3[key];
				dictionary4[key2] = key + 1;
			}
			if (dictionary2[1] < mst_mission.Stype_num1 || dictionary2[2] < mst_mission.Stype_num2 || dictionary2[3] < mst_mission.Stype_num3 || dictionary2[4] < mst_mission.Stype_num4 || dictionary2[5] < mst_mission.Stype_num5 || dictionary2[6] < mst_mission.Stype_num6 || dictionary2[7] < mst_mission.Stype_num7 || dictionary2[8] < mst_mission.Stype_num8 || dictionary2[9] < mst_mission.Stype_num9)
			{
				resultInfo.MissionResult = MissionResultKinds.FAILE;
				return;
			}
			if (mst_mission.Deck_num > 0)
			{
				int num9 = mem_ships.Count - num2;
				if (num9 < mst_mission.Deck_num)
				{
					resultInfo.MissionResult = MissionResultKinds.FAILE;
					return;
				}
			}
			if (num7 < mst_mission.Level)
			{
				resultInfo.MissionResult = MissionResultKinds.FAILE;
				return;
			}
			if (drumShipNum < mst_mission.Drum_ship_num || drumNum < mst_mission.Drum_total_num1)
			{
				resultInfo.MissionResult = MissionResultKinds.FAILE;
				return;
			}
			double num10 = num3 + num4;
			double num11 = num5 + num6;
			double num12 = num10 / num11 * 100.0;
			int num13 = (int)(num12 + (double)randInstance.Next(20));
			if (num13 < 100)
			{
				resultInfo.MissionResult = MissionResultKinds.FAILE;
				return;
			}
			if (num2 > 0)
			{
				resultInfo.MissionResult = MissionResultKinds.SUCCESS;
				return;
			}
			if (!mst_mission.IsGreatSuccessCondition() && num < mem_ships.Count)
			{
				resultInfo.MissionResult = MissionResultKinds.SUCCESS;
				return;
			}
			int drumKeisu = 0;
			if (mst_mission.Drum_total_num2 > 0)
			{
				drumKeisu = ((drumNum < mst_mission.Drum_total_num2) ? (-15) : 20);
			}
			if (mst_mission.Flagship_stype2 > 0 && mst_mission.Flagship_stype2 != mem_ships[0].Stype)
			{
				resultInfo.MissionResult = MissionResultKinds.SUCCESS;
				return;
			}
			int checkRate = getCheckRate(num, drumKeisu);
			int num14 = randInstance.Next(100);
			if (checkRate >= num14)
			{
				resultInfo.MissionResult = MissionResultKinds.GREAT;
			}
			else
			{
				resultInfo.MissionResult = MissionResultKinds.SUCCESS;
			}
		}

		private int getCheckRate(int goodCondNum, int drumKeisu)
		{
			int num = 20 + goodCondNum * 15 + drumKeisu;
			if (mst_mission.Flagship_level_check_type == 2)
			{
				int flagShipLevelCheckValue = getFlagShipLevelCheckValue();
				num = num - 5 + flagShipLevelCheckValue;
			}
			return num;
		}

		private int getFlagShipLevelCheckValue()
		{
			int level = mem_ships[0].Level;
			double num = Math.Sqrt(level) + (double)level / 10.0;
			return (int)num;
		}

		private void setItems(out Dictionary<enumMaterialCategory, int> up_mat)
		{
			up_mat = null;
			if (resultInfo.MissionResult == MissionResultKinds.FAILE)
			{
				return;
			}
			double success_keisu = (resultInfo.MissionResult != MissionResultKinds.GREAT) ? 1.0 : 1.5;
			up_mat = new Dictionary<enumMaterialCategory, int>();
			Array values = Enum.GetValues(typeof(enumMaterialCategory));
			foreach (object item in values)
			{
				up_mat.Add((enumMaterialCategory)(int)item, 0);
			}
			Dictionary<enumMaterialCategory, int> getMaterials;
			Dictionary<enumMaterialCategory, int> dictionary = getMaterials = resultInfo.GetMaterials;
			enumMaterialCategory key;
			enumMaterialCategory key2 = key = enumMaterialCategory.Fuel;
			int num = getMaterials[key];
			dictionary[key2] = num + mst_mission.Win_mat1;
			Dictionary<enumMaterialCategory, int> getMaterials2;
			Dictionary<enumMaterialCategory, int> dictionary2 = getMaterials2 = resultInfo.GetMaterials;
			enumMaterialCategory key3 = key = enumMaterialCategory.Bull;
			num = getMaterials2[key];
			dictionary2[key3] = num + mst_mission.Win_mat2;
			Dictionary<enumMaterialCategory, int> getMaterials3;
			Dictionary<enumMaterialCategory, int> dictionary3 = getMaterials3 = resultInfo.GetMaterials;
			enumMaterialCategory key4 = key = enumMaterialCategory.Steel;
			num = getMaterials3[key];
			dictionary3[key4] = num + mst_mission.Win_mat3;
			Dictionary<enumMaterialCategory, int> getMaterials4;
			Dictionary<enumMaterialCategory, int> dictionary4 = getMaterials4 = resultInfo.GetMaterials;
			enumMaterialCategory key5 = key = enumMaterialCategory.Bauxite;
			num = getMaterials4[key];
			dictionary4[key5] = num + mst_mission.Win_mat4;
			foreach (enumMaterialCategory item2 in resultInfo.GetMaterials.Keys.ToList())
			{
				int materialBonusNum = getMaterialBonusNum(resultInfo.GetMaterials[item2], success_keisu);
				Dictionary<enumMaterialCategory, int> getMaterials5;
				Dictionary<enumMaterialCategory, int> dictionary5 = getMaterials5 = resultInfo.GetMaterials;
				enumMaterialCategory key6 = key = item2;
				num = getMaterials5[key];
				dictionary5[key6] = num + materialBonusNum;
				up_mat[item2] = resultInfo.GetMaterials[item2];
			}
			List<int[]> list = new List<int[]>();
			if (resultInfo.MissionResult == MissionResultKinds.SUCCESS)
			{
				resultInfo.GetSpoint += mst_mission.Win_spoint1;
				int num2 = randInstance.Next(mst_mission.Win_item1_num + 1);
				if (num2 == 0)
				{
					return;
				}
				list.Add(new int[2]
				{
					mst_mission.Win_item1,
					num2
				});
			}
			else if (resultInfo.MissionResult == MissionResultKinds.GREAT)
			{
				resultInfo.GetSpoint += mst_mission.Win_spoint2;
				int num3 = randInstance.Next(mst_mission.Win_item1_num + 1);
				if (num3 > 0)
				{
					list.Add(new int[2]
					{
						mst_mission.Win_item1,
						num3
					});
				}
				if (mst_mission.Win_item2_num > 0)
				{
					int num4 = randInstance.Next(mst_mission.Win_item2_num) + 1;
					list.Add(new int[2]
					{
						mst_mission.Win_item2,
						num4
					});
				}
			}
			resultInfo.GetItems = new List<ItemGetFmt>();
			foreach (int[] item3 in list)
			{
				Mst_useitem mst_useitem = Mst_DataManager.Instance.Mst_useitem[item3[0]];
				ItemGetFmt itemGetFmt = new ItemGetFmt();
				itemGetFmt.Id = mst_useitem.Id;
				itemGetFmt.Category = ItemGetKinds.UseItem;
				itemGetFmt.Count = item3[1];
				if (itemGetFmt.Id >= 1 && itemGetFmt.Id <= 4)
				{
					int materialBonusNum2 = getMaterialBonusNum(itemGetFmt.Count, success_keisu);
					itemGetFmt.Count += materialBonusNum2;
					enumMaterialCategory enumMaterialCategory = (itemGetFmt.Id == 1) ? enumMaterialCategory.Repair_Kit : ((itemGetFmt.Id == 2) ? enumMaterialCategory.Build_Kit : ((itemGetFmt.Id != 3) ? enumMaterialCategory.Revamp_Kit : enumMaterialCategory.Dev_Kit));
					Dictionary<enumMaterialCategory, int> dictionary6;
					Dictionary<enumMaterialCategory, int> dictionary7 = dictionary6 = up_mat;
					enumMaterialCategory key7 = key = enumMaterialCategory;
					num = dictionary6[key];
					dictionary7[key7] = num + itemGetFmt.Count;
				}
				resultInfo.GetItems.Add(itemGetFmt);
			}
			if (resultInfo.GetItems.Count == 0)
			{
				resultInfo.GetItems = null;
			}
		}

		private int getMaterialBonusNum(int nowNum, double success_keisu)
		{
			double num = 0.05;
			int num2 = (daihatuNum <= 4) ? daihatuNum : 4;
			int num3 = (int)((double)nowNum * success_keisu);
			int num4 = (int)((double)(num3 * num2) * num);
			return num3 - nowNum + num4;
		}

		private void setBasicExp()
		{
			if (mem_deck.MissionState == MissionStates.STOP)
			{
				resultInfo.GetMemberExp = 0;
				return;
			}
			int win_exp_member = mst_mission.Win_exp_member;
			Dictionary<MissionResultKinds, double> dictionary = new Dictionary<MissionResultKinds, double>();
			dictionary.Add(MissionResultKinds.FAILE, 0.3);
			dictionary.Add(MissionResultKinds.SUCCESS, 1.0);
			dictionary.Add(MissionResultKinds.GREAT, 2.0);
			Dictionary<MissionResultKinds, double> dictionary2 = dictionary;
			MissionResultKinds missionResult = resultInfo.MissionResult;
			resultInfo.GetMemberExp = (int)((double)win_exp_member * dictionary2[missionResult]);
		}

		private Dictionary<int, int> getShipExp()
		{
			if (mem_deck.MissionState == MissionStates.STOP)
			{
				return mem_ships.ToDictionary((Mem_ship key) => key.Rid, (Mem_ship value) => 0);
			}
			int base_exp = mst_mission.Win_exp_ship * (randInstance.Next(2) + 1);
			Dictionary<MissionResultKinds, double> keisu = new Dictionary<MissionResultKinds, double>
			{
				{
					MissionResultKinds.FAILE,
					1.0
				},
				{
					MissionResultKinds.SUCCESS,
					1.0
				},
				{
					MissionResultKinds.GREAT,
					2.0
				}
			};
			double num = 1.5;
			Dictionary<int, int> ret_exp = new Dictionary<int, int>(6);
			int flagLevelCheckValue = getFlagShipLevelCheckValue();
			int flagshipRid = mem_ships[0].Rid;
			mem_ships.ForEach(delegate(Mem_ship x)
			{
				int getExp = (int)Math.Floor((double)base_exp * keisu[resultInfo.MissionResult]);
				if (mst_mission.Mission_type == MissionType.Practice)
				{
					setPracticeTypeShipExp(ref getExp, x.Level, flagLevelCheckValue, (x.Rid == flagshipRid) ? true : false);
				}
				ret_exp.Add(x.Rid, getExp);
			});
			ret_exp[flagshipRid] = (int)Math.Floor((double)ret_exp[flagshipRid] * num);
			return ret_exp;
		}

		private void setPracticeTypeShipExp(ref int getExp, int nowLevel, int flagLevelCheckValue, bool flagShip)
		{
			if (!flagShip)
			{
				getExp += flagLevelCheckValue * 10;
			}
			if (nowLevel <= 9)
			{
				getExp = (int)((double)getExp * 1.5);
			}
			else if (nowLevel >= 10 && nowLevel <= 19)
			{
				getExp = (int)((double)getExp * 1.25);
			}
			else if (nowLevel >= 20 && nowLevel <= 29)
			{
				getExp = (int)((double)getExp * 1.1);
			}
			else if (nowLevel >= 70)
			{
				getExp = (int)((double)getExp * 0.9);
			}
		}

		private void updateShip(Dictionary<int, int> shipExp, out Dictionary<int, List<int>> lvupInfo)
		{
			lvupInfo = new Dictionary<int, List<int>>();
			int count = mem_ships.Count;
			for (int i = 0; i < count; i++)
			{
				mem_ships[i].SetSubFuel_ToMission(mst_mission.Use_fuel);
				mem_ships[i].SetSubBull_ToMission(mst_mission.Use_bull);
				Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ships[i]);
				Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship[mem_shipBase.Ship_id];
				List<int> lvupInfo2 = null;
				int addExp = shipExp[mem_ships[i].Rid];
				int levelupInfo = mem_ships[i].getLevelupInfo(mst_level, mem_ships[i].Level, mem_ships[i].Exp, ref addExp, out lvupInfo2);
				lvupInfo.Add(mem_ships[i].Rid, lvupInfo2);
				mem_shipBase.Level = levelupInfo;
				mem_shipBase.Exp += addExp;
				int num = levelupInfo - mem_ships[i].Level;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary = mem_ships[i].Kyouka;
				for (int j = 0; j < num; j++)
				{
					dictionary = mem_ships[i].getLevelupKyoukaValue(mem_ships[i].Ship_id, dictionary);
				}
				mem_shipBase.SetKyoukaValue(dictionary);
				setCondSubValue(ref mem_shipBase.Cond);
				int value = 0;
				int value2 = 0;
				mst_level.TryGetValue(mem_shipBase.Level - 1, out value);
				mst_level.TryGetValue(mem_shipBase.Level + 1, out value2);
				mem_ships[i].SetRequireExp(mem_shipBase.Level, mst_level);
				mem_ships[i].Set_ShipParam(mem_shipBase, mst_data, enemy_flag: false);
				mem_ships[i].SumLovToMission(resultInfo.MissionResult);
			}
		}

		private void setCondSubValue(ref int cond)
		{
			if (mem_deck.MissionState != MissionStates.STOP)
			{
				int num = 3;
				if (Mem_ship.Get_FatitgueState(cond) >= FatigueState.Distress)
				{
					num += 6;
				}
				cond -= num;
				if (0 > cond)
				{
					cond = 0;
				}
			}
		}

		private void updateMaterial(Dictionary<enumMaterialCategory, int> up_mat)
		{
			if (mem_deck.MissionState != MissionStates.STOP && up_mat != null)
			{
				foreach (KeyValuePair<enumMaterialCategory, int> item in up_mat)
				{
					Comm_UserDatas.Instance.User_material[item.Key].Add_Material(item.Value);
				}
			}
		}

		private void updateDeck()
		{
			mem_deck.MissionInit();
		}

		private void updateItem()
		{
			if (resultInfo.GetItems != null)
			{
				resultInfo.GetItems.ForEach(delegate(ItemGetFmt x)
				{
					if (x.Category == ItemGetKinds.UseItem && x.Id > 4)
					{
						Comm_UserDatas.Instance.Add_Useitem(x.Id, x.Count);
					}
				});
			}
		}

		private void updateMissionComp()
		{
			if (resultInfo.MissionResult != 0)
			{
				Mem_missioncomp mem_missioncomp = new Mem_missioncomp(mst_mission.Id, mst_mission.Maparea_id, MissionClearKinds.CLEARED);
				mem_missioncomp.Update();
			}
		}

		private int updateRecord()
		{
			int result = Comm_UserDatas.Instance.User_record.UpdateExp(resultInfo.GetMemberExp, mst_userlevel);
			Comm_UserDatas.Instance.User_record.UpdateMissionCount(resultInfo.MissionResult);
			return result;
		}

		private void updateBasic()
		{
			Comm_UserDatas.Instance.User_basic.AddPoint(resultInfo.GetSpoint);
		}

		private void updateRebellionPoint(MissionResultKinds kind)
		{
			int num = 0;
			switch (kind)
			{
			case MissionResultKinds.SUCCESS:
				num = 4;
				break;
			case MissionResultKinds.GREAT:
				num = 6;
				break;
			}
			int subNum = mst_mission.Rp_sub * num;
			((IRebellionPointOperator)this).SubRebellionPoint(mst_mission.Maparea_id, subNum);
		}
	}
}
