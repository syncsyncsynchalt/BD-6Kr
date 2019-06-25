using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers
{
	public class Api_req_Quest : IQuestOperator, Mem_slotitem.IMemSlotIdOperator
	{
		private Dictionary<int, Mst_quest> mst_quest = new Dictionary<int, Mst_quest>();

		private Dictionary<int, List<int>> mst_slotitemchange = new Dictionary<int, List<int>>();

		private Dictionary<int, List<int>> mst_quest_reset = new Dictionary<int, List<int>>();

		public Api_req_Quest()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result(Mst_quest.tableName, Mst_quest.tableName, "Id");
			foreach (XElement item6 in enumerable)
			{
				Mst_quest instance = null;
				Model_Base.SetMaster(out instance, item6);
				mst_quest.Add(instance.Id, instance);
			}
			IEnumerable<XElement> enumerable2 = Utils.Xml_Result("mst_quest_slotitemchange", "mst_quest_slotitemchange", "Id");
			foreach (XElement item7 in enumerable2)
			{
				int key = int.Parse(item7.Element("Id").Value);
				int item = int.Parse(item7.Element("Old_slotitem_id").Value);
				int item2 = int.Parse(item7.Element("New_slotitem_id").Value);
				int item3 = int.Parse(item7.Element("Level_max").Value);
				int item4 = int.Parse(item7.Element("Use_crew").Value);
				mst_slotitemchange.Add(key, new List<int>
				{
					item,
					item2,
					item3,
					item4
				});
			}
			IEnumerable<XElement> enumerable3 = Utils.Xml_Result("mst_questcount_reset", "mst_questcount_reset", string.Empty);
			mst_quest_reset = new Dictionary<int, List<int>>
			{
				{
					1,
					new List<int>()
				},
				{
					2,
					new List<int>()
				},
				{
					3,
					new List<int>()
				},
				{
					4,
					new List<int>()
				}
			};
			foreach (XElement item8 in enumerable3)
			{
				int num = int.Parse(item8.Element("Type").Value);
				if (num != 0)
				{
					int item5 = int.Parse(item8.Element("Id").Value);
					mst_quest_reset[num].Add(item5);
				}
			}
		}

		public Api_req_Quest(Api_TurnOperator tInstance)
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result(Mst_quest.tableName, Mst_quest.tableName, "Id");
			foreach (XElement item2 in enumerable)
			{
				Mst_quest instance = null;
				Model_Base.SetMaster(out instance, item2);
				mst_quest.Add(instance.Id, instance);
			}
			IEnumerable<XElement> enumerable2 = Utils.Xml_Result("mst_questcount_reset", "mst_questcount_reset", string.Empty);
			mst_quest_reset = new Dictionary<int, List<int>>
			{
				{
					1,
					new List<int>()
				},
				{
					2,
					new List<int>()
				},
				{
					3,
					new List<int>()
				},
				{
					4,
					new List<int>()
				}
			};
			foreach (XElement item3 in enumerable2)
			{
				int num = int.Parse(item3.Element("Type").Value);
				if (num != 0)
				{
					int item = int.Parse(item3.Element("Id").Value);
					mst_quest_reset[num].Add(item);
				}
			}
		}

		void Mem_slotitem.IMemSlotIdOperator.ChangeSlotId(Mem_slotitem obj, int changeId)
		{
			obj.ChangeSlotId(this, changeId);
			obj.ChangeExperience(-obj.Experience);
			obj.ChangeExperience(Mst_DataManager.Instance.Mst_Slotitem[changeId].Default_exp);
		}

		public void EnforceQuestReset()
		{
			if (Comm_UserDatas.Instance.User_quest.Count != 0 && Comm_UserDatas.Instance.User_turn.ReqQuestReset)
			{
				QuestReset();
				Comm_UserDatas.Instance.User_turn.DisableQuestReset();
				mst_quest.Clear();
				mst_quest = null;
				mst_quest_reset.Clear();
				mst_quest_reset = null;
			}
		}

		public Api_Result<List<User_QuestFmt>> QuestList()
		{
			if (Comm_UserDatas.Instance.User_quest.Count == 0)
			{
				Comm_UserDatas.Instance.InitQuest(this, mst_quest.Values.ToList());
			}
			if (Comm_UserDatas.Instance.User_turn.ReqQuestReset)
			{
				QuestReset();
				Comm_UserDatas.Instance.User_turn.DisableQuestReset();
			}
			SetEnableList();
			Api_Result<List<User_QuestFmt>> api_Result = new Api_Result<List<User_QuestFmt>>();
			IEnumerable<Mem_quest> enumerable = from member in Comm_UserDatas.Instance.User_quest.Values
				orderby member.Rid
				where member.State != QuestState.END && member.State != QuestState.NOT_DISP
				select member;
			api_Result.data = new List<User_QuestFmt>();
			int key = Comm_UserDatas.Instance.User_deck[1].Ship[0];
			Mem_ship flagShip = Comm_UserDatas.Instance.User_ship[key];
			foreach (Mem_quest item in enumerable)
			{
				Mst_quest mstObj = mst_quest[item.Rid];
				User_QuestFmt user_QuestFmt = new User_QuestFmt(item, mstObj);
				slotModelChangeQuestNormalize(flagShip, item, mstObj, user_QuestFmt);
				api_Result.data.Add(user_QuestFmt);
			}
			return api_Result;
		}

		public bool Start(User_QuestFmt fmt)
		{
			Mem_quest value = null;
			if (!Comm_UserDatas.Instance.User_quest.TryGetValue(fmt.No, out value))
			{
				return false;
			}
			if (value.State != QuestState.WAITING_START)
			{
				return false;
			}
			value.StateChange(this, QuestState.RUNNING);
			fmt.State = value.State;
			if (fmt.Category == 1)
			{
				QuestHensei questHensei = new QuestHensei(fmt.No);
				List<int> list = questHensei.ExecuteCheck();
				if (list.Contains(fmt.No))
				{
					fmt.State = QuestState.COMPLETE;
				}
			}
			return true;
		}

		public bool Stop(User_QuestFmt fmt)
		{
			Mem_quest value = null;
			if (!Comm_UserDatas.Instance.User_quest.TryGetValue(fmt.No, out value))
			{
				return false;
			}
			if (value.State != QuestState.RUNNING)
			{
				return false;
			}
			value.StateChange(this, QuestState.WAITING_START);
			fmt.State = value.State;
			return true;
		}

		public List<QuestItemGetFmt> ClearItemGet(User_QuestFmt fmt)
		{
			Mem_quest value = null;
			if (!Comm_UserDatas.Instance.User_quest.TryGetValue(fmt.No, out value))
			{
				return null;
			}
			if (value.State != QuestState.COMPLETE)
			{
				return null;
			}
			value.StateChange(this, QuestState.END);
			Mst_questcount value2 = null;
			if (Mst_DataManager.Instance.Mst_questcount.TryGetValue(value.Rid, out value2))
			{
				foreach (KeyValuePair<int, bool> item in value2.Reset_flag)
				{
					if (item.Value)
					{
						Mem_questcount value3 = null;
						if (Comm_UserDatas.Instance.User_questcount.TryGetValue(item.Key, out value3))
						{
							value3.Reset(deleteFlag: true);
						}
					}
				}
			}
			Mst_quest mst_quest = this.mst_quest[fmt.No];
			List<QuestItemGetFmt> list = new List<QuestItemGetFmt>();
			foreach (KeyValuePair<enumMaterialCategory, int> item2 in fmt.GetMaterial)
			{
				if (item2.Key <= enumMaterialCategory.Bauxite)
				{
					list.Add(_ItemGet(item2.Key, item2.Value));
				}
			}
			int slotModelChangeId = mst_quest.GetSlotModelChangeId(1);
			int slotModelChangeId2 = mst_quest.GetSlotModelChangeId(2);
			bool useCrewFlag = false;
			bool maxExpFlag = false;
			if (slotModelChangeId > 0)
			{
				maxExpFlag = mst_quest.IsLevelMax(mst_slotitemchange[slotModelChangeId]);
				useCrewFlag = mst_quest.IsUseCrew(mst_slotitemchange[slotModelChangeId]);
			}
			list.Add(_ItemGet(mst_quest.Get_1_type, mst_quest.Get_1_id, mst_quest.Get_1_count, maxExpFlag, useCrewFlag));
			bool useCrewFlag2 = false;
			bool maxExpFlag2 = false;
			if (slotModelChangeId2 > 0)
			{
				maxExpFlag2 = mst_quest.IsLevelMax(mst_slotitemchange[slotModelChangeId2]);
				useCrewFlag2 = mst_quest.IsUseCrew(mst_slotitemchange[slotModelChangeId2]);
			}
			list.Add(_ItemGet(mst_quest.Get_2_type, mst_quest.Get_2_id, mst_quest.Get_2_count, maxExpFlag2, useCrewFlag2));
			return list.FindAll((QuestItemGetFmt item) => item != null);
		}

		public bool Debug_StateChange(int no)
		{
			Mem_quest value = null;
			if (!Comm_UserDatas.Instance.User_quest.TryGetValue(no, out value))
			{
				return false;
			}
			if (value.State != QuestState.RUNNING)
			{
				return false;
			}
			value.StateChange(this, QuestState.COMPLETE);
			Mst_questcount value2 = null;
			if (!Mst_DataManager.Instance.Mst_questcount.TryGetValue(value.Rid, out value2))
			{
				return true;
			}
			foreach (KeyValuePair<int, int> item in value2.Clear_num)
			{
				Mem_questcount value3 = null;
				if (!Comm_UserDatas.Instance.User_questcount.TryGetValue(item.Key, out value3))
				{
					value3 = new Mem_questcount(item.Key, item.Value);
					Comm_UserDatas.Instance.User_questcount.Add(item.Key, value3);
				}
				else
				{
					int num = item.Value - value3.Value;
					if (num > 0)
					{
						value3.AddCount(num);
					}
				}
			}
			return true;
		}

		public bool Debug_CompleteToRunningChange(int no)
		{
			Mem_quest value = null;
			if (!Comm_UserDatas.Instance.User_quest.TryGetValue(no, out value))
			{
				return false;
			}
			if (value.State != QuestState.COMPLETE)
			{
				return false;
			}
			value.StateChange(this, QuestState.RUNNING);
			return true;
		}

		private void QuestReset()
		{
			List<int> reset_type = null;
			HashSet<int> reset_counter_type = null;
			setResetType(out reset_type, out reset_counter_type);
			foreach (Mst_quest value4 in mst_quest.Values)
			{
				if (value4.Type != 1)
				{
					Mem_quest value = null;
					if (Comm_UserDatas.Instance.User_quest.TryGetValue(value4.Id, out value) && value.State != 0 && reset_type.Contains(value4.Type))
					{
						if (value4.Torigger == 0)
						{
							value.StateChange(this, QuestState.WAITING_START);
						}
						else
						{
							Mst_quest value2 = null;
							if (!mst_quest.TryGetValue(value4.Torigger, out value2))
							{
								value.StateChange(this, QuestState.NOT_DISP);
							}
							else if (value2.Type != 1)
							{
								value.StateChange(this, QuestState.NOT_DISP);
							}
							else
							{
								value.StateChange(this, QuestState.WAITING_START);
							}
						}
					}
				}
			}
			foreach (int item in reset_counter_type)
			{
				List<int> list = mst_quest_reset[item];
				foreach (int item2 in list)
				{
					Mem_questcount value3 = null;
					if (Comm_UserDatas.Instance.User_questcount.TryGetValue(item2, out value3))
					{
						value3.Reset(deleteFlag: true);
					}
				}
			}
		}

		private void setResetType(out List<int> reset_type, out HashSet<int> reset_counter_type)
		{
			reset_type = new List<int>();
			reset_counter_type = new HashSet<int>();
			DateTime dateTime = Comm_UserDatas.Instance.User_turn.GetDateTime();
			reset_type.Add(2);
			reset_counter_type.Add(1);
			if (dateTime.DayOfWeek == DayOfWeek.Sunday)
			{
				reset_type.Add(3);
				reset_counter_type.Add(2);
			}
			reset_type.Add(4);
			reset_type.Add(5);
			if (dateTime.Day == 1)
			{
				reset_type.Add(6);
				reset_counter_type.Add(3);
			}
			if (dateTime.Month == 1 && dateTime.Day == 1)
			{
				reset_type.Add(7);
				reset_counter_type.Add(4);
			}
		}

		private void SetEnableList()
		{
			if (mst_quest.Keys.Count() != Comm_UserDatas.Instance.User_quest.Keys.Count())
			{
				IEnumerable<int> enumerable = mst_quest.Keys.Except(Comm_UserDatas.Instance.User_quest.Keys);
				foreach (int item in enumerable)
				{
					int category = mst_quest[item].Category;
					Mem_quest mem_quest = new Mem_quest(item, category, QuestState.NOT_DISP);
					Comm_UserDatas.Instance.User_quest.Add(mem_quest.Rid, mem_quest);
				}
			}
			int num = Comm_UserDatas.Instance.User_turn.GetDateTime().Day % 10;
			foreach (Mem_quest value3 in Comm_UserDatas.Instance.User_quest.Values)
			{
				if (mst_quest[value3.Rid].Type == 4 && num != 0 && num != 3 && num != 7)
				{
					value3.StateChange(this, QuestState.NOT_DISP);
				}
				else if (mst_quest[value3.Rid].Type == 5 && num != 2 && num != 8)
				{
					value3.StateChange(this, QuestState.NOT_DISP);
				}
				else if (value3.State == QuestState.NOT_DISP && specialToriggerCheck(mst_quest[value3.Rid]))
				{
					if (mst_quest[value3.Rid].Sub_torigger != 0)
					{
						Mem_quest value = null;
						if (!Comm_UserDatas.Instance.User_quest.TryGetValue(mst_quest[value3.Rid].Sub_torigger, out value) || value.State != QuestState.END)
						{
							continue;
						}
					}
					Mem_quest value2 = null;
					if (!Comm_UserDatas.Instance.User_quest.TryGetValue(mst_quest[value3.Rid].Torigger, out value2))
					{
						if (mst_quest[value3.Rid].Torigger == 0)
						{
							value3.StateChange(this, QuestState.WAITING_START);
						}
					}
					else if (value2.State == QuestState.END)
					{
						value3.StateChange(this, QuestState.WAITING_START);
					}
				}
			}
		}

		private void slotModelChangeQuestNormalize(Mem_ship flagShip, Mem_quest mem_quest, Mst_quest mst_quest, User_QuestFmt changeFmt)
		{
			HashSet<int> hashSet = new HashSet<int>();
			if (mem_quest.State == QuestState.COMPLETE)
			{
				if (mst_quest.Get_1_type == 15)
				{
					hashSet.Add(mst_quest.Get_1_id);
				}
				if (mst_quest.Get_2_type == 15)
				{
					hashSet.Add(mst_quest.Get_2_id);
				}
			}
			if (hashSet.Count == 0)
			{
				return;
			}
			Mst_questcount value = null;
			Mst_DataManager.Instance.Mst_questcount.TryGetValue(mem_quest.Rid, out value);
			HashSet<int> hashSet2 = new HashSet<int>();
			bool flag = false;
			foreach (int item2 in hashSet)
			{
				int mst_id = mst_slotitemchange[item2][0];
				bool maxFlag = mst_quest.IsLevelMax(mst_slotitemchange[item2]);
				if (mst_quest.IsUseCrew(mst_slotitemchange[item2]))
				{
					flag = true;
				}
				int item = findModelChangeEnableSlotPos(flagShip.Slot, mst_id, maxFlag);
				hashSet2.Add(item);
			}
			if (flag)
			{
				int num = 0;
				Mem_useitem value2 = null;
				if (Comm_UserDatas.Instance.User_useItem.TryGetValue(70, out value2))
				{
					num = value2.Value;
				}
				if (num == 0)
				{
					hashSet2.Add(-1);
				}
			}
			List<string> requireShip = QuestKousyou.GetRequireShip(mem_quest.Rid);
			if (requireShip.Count > 0)
			{
				string yomi = Mst_DataManager.Instance.Mst_ship[flagShip.Ship_id].Yomi;
				if (!requireShip.Any((string x) => x.Equals(yomi)))
				{
					hashSet2.Add(-1);
				}
			}
			if (hashSet2.Contains(-1))
			{
				mem_quest.StateChange(this, QuestState.WAITING_START);
				changeFmt.State = mem_quest.State;
				if (value != null)
				{
					foreach (int item3 in value.Counter_id)
					{
						Mem_questcount value3 = null;
						if (Comm_UserDatas.Instance.User_questcount.TryGetValue(item3, out value3))
						{
							value3.Reset(deleteFlag: false);
						}
					}
				}
			}
			else if (hashSet2.Contains(-2))
			{
				changeFmt.InvalidFlag = true;
			}
		}

		private QuestItemGetFmt _ItemGet(enumMaterialCategory material_category, int count)
		{
			QuestItemGetFmt questItemGetFmt = new QuestItemGetFmt();
			questItemGetFmt.Category = QuestItemGetKind.Material;
			questItemGetFmt.Id = (int)material_category;
			questItemGetFmt.Count = count;
			if (questItemGetFmt.Category == QuestItemGetKind.Material)
			{
				enumMaterialCategory id = (enumMaterialCategory)questItemGetFmt.Id;
				Comm_UserDatas.Instance.User_material[id].Add_Material(questItemGetFmt.Count);
			}
			return questItemGetFmt;
		}

		private QuestItemGetFmt _ItemGet(int type, int id, int count, bool maxExpFlag, bool useCrewFlag)
		{
			QuestItemGetFmt questItemGetFmt = new QuestItemGetFmt();
			questItemGetFmt.Category = (QuestItemGetKind)type;
			questItemGetFmt.Id = id;
			questItemGetFmt.Count = count;
			questItemGetFmt.IsUseCrewItem = useCrewFlag;
			if (questItemGetFmt.Category == QuestItemGetKind.Material)
			{
				enumMaterialCategory id2 = (enumMaterialCategory)questItemGetFmt.Id;
				Comm_UserDatas.Instance.User_material[id2].Add_Material(questItemGetFmt.Count);
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.Deck)
			{
				Comm_UserDatas.Instance.Add_Deck(questItemGetFmt.Id);
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.LargeBuild)
			{
				Comm_UserDatas.Instance.User_basic.OpenLargeDock();
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.Ship)
			{
				Comm_UserDatas.Instance.Add_Ship(questItemGetFmt.createIds());
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.SlotItem)
			{
				Comm_UserDatas.Instance.Add_Slot(questItemGetFmt.createIds());
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.FurnitureBox || questItemGetFmt.Category == QuestItemGetKind.UseItem)
			{
				if (questItemGetFmt.Id == 63)
				{
					questItemGetFmt.IsQuestExtend = Comm_UserDatas.Instance.User_basic.QuestExtend(Mst_DataManager.Instance.Mst_const);
				}
				else
				{
					Comm_UserDatas.Instance.Add_Useitem(questItemGetFmt.Id, questItemGetFmt.Count);
				}
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.Furniture)
			{
				Comm_UserDatas.Instance.Add_Furniture(questItemGetFmt.Id);
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.Exchange)
			{
				questItemGetFmt.FromId = mst_slotitemchange[id][0];
				questItemGetFmt.Id = mst_slotitemchange[id][1];
				int key = Comm_UserDatas.Instance.User_deck[1].Ship[0];
				Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[key];
				int num = findModelChangeEnableSlotPos(mem_ship.Slot, questItemGetFmt.FromId, maxExpFlag);
				if (num < 0)
				{
					return null;
				}
				Mem_slotitem obj = Comm_UserDatas.Instance.User_slot[mem_ship.Slot[num]];
				((Mem_slotitem.IMemSlotIdOperator)this).ChangeSlotId(obj, questItemGetFmt.Id);
				Mem_shipBase baseData = new Mem_shipBase(mem_ship);
				mem_ship.Set_ShipParam(baseData, Mst_DataManager.Instance.Mst_ship[mem_ship.Ship_id], enemy_flag: false);
				if (questItemGetFmt.IsUseCrewItem && Comm_UserDatas.Instance.User_useItem.TryGetValue(70, out Mem_useitem value))
				{
					value.Sub_UseItem(1);
				}
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.Spoint)
			{
				Comm_UserDatas.Instance.User_basic.AddPoint(questItemGetFmt.Count);
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.DeckPractice)
			{
				Comm_UserDatas.Instance.User_deckpractice.StateChange((DeckPracticeType)questItemGetFmt.Id, state: true);
			}
			else
			{
				if (questItemGetFmt.Category != QuestItemGetKind.Tanker)
				{
					return null;
				}
				Comm_UserDatas.Instance.Add_Tanker(questItemGetFmt.Count);
			}
			return questItemGetFmt;
		}

		private int findModelChangeEnableSlotPos(List<int> slot_rids, int mst_id, bool maxFlag)
		{
			int result = -1;
			for (int i = 0; i < slot_rids.Count; i++)
			{
				int num = slot_rids[i];
				if (num <= 0)
				{
					continue;
				}
				Mem_slotitem mem_slotitem = Comm_UserDatas.Instance.User_slot[num];
				if (mem_slotitem.Slotitem_id == mst_id && (!maxFlag || mem_slotitem.IsMaxSkillLevel()))
				{
					if (!mem_slotitem.Lock)
					{
						return i;
					}
					result = -2;
				}
			}
			return result;
		}

		private bool specialToriggerCheck(Mst_quest mst_record)
		{
			if (mst_record.Id == 423)
			{
				DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
				if (difficult == DifficultKind.KOU || difficult == DifficultKind.SHI)
				{
					return isRequireShipLimit(mst_record.Get_1_id, 1);
				}
				return false;
			}
			return true;
		}

		private bool isRequireShipLimit(int search_id, int limitNum)
		{
			Dictionary<int, Mst_ship> m_ship = Mst_DataManager.Instance.Mst_ship;
			string target = m_ship[search_id].Yomi;
			int num = Comm_UserDatas.Instance.User_ship.Values.Count((Mem_ship x) => m_ship[x.Ship_id].Yomi.Equals(target));
			return (limitNum >= num) ? true : false;
		}
	}
}
