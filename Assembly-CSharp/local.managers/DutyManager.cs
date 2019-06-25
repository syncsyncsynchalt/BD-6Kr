using Common.Enum;
using local.models;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class DutyManager : ManagerBase
	{
		private Api_req_Quest _quest;

		private List<__DutyModel__> _duties;

		public int MaxExecuteCount => base.UserInfo.MaxDutyExecuteCount;

		public DutyManager()
		{
			_quest = new Api_req_Quest();
			_UpdateDutyData();
		}

		public DutyModel GetDuty(int duty_no)
		{
			return _duties.Find((__DutyModel__ duty) => duty.No == duty_no);
		}

		public List<DutyModel> GetDutyList(bool is_sorted)
		{
			List<__DutyModel__> range = _duties.GetRange(0, _duties.Count);
			if (is_sorted)
			{
				range.Sort((__DutyModel__ x, __DutyModel__ y) => _CompareFunc(x, y));
			}
			return range.ConvertAll((Converter<__DutyModel__, DutyModel>)((__DutyModel__ duty) => duty));
		}

		public List<DutyModel> GetDutyList()
		{
			return GetDutyList(is_sorted: false);
		}

		public List<DutyModel> GetRunningDutyList()
		{
			return GetDutyList(is_sorted: true).FindAll((DutyModel duty) => duty.State == QuestState.RUNNING);
		}

		public List<DutyModel> GetExecutedDutyList()
		{
			return GetDutyList(is_sorted: true).FindAll((DutyModel duty) => duty.State == QuestState.RUNNING || duty.State == QuestState.COMPLETE);
		}

		[Obsolete("GetDutyList(is_sorted) を使用してください", false)]
		public DutyModel[] GetDuties(bool is_sorted)
		{
			return GetDutyList(is_sorted).ToArray();
		}

		public bool StartDuty(int duty_no)
		{
			DutyModel duty = GetDuty(duty_no);
			if (duty == null || duty.State != QuestState.WAITING_START)
			{
				return false;
			}
			if (GetExecutedDutyList().Count >= MaxExecuteCount)
			{
				return false;
			}
			return _quest.Start(((__DutyModel__)duty).Fmt);
		}

		public bool Cancel(int duty_no)
		{
			DutyModel duty = GetDuty(duty_no);
			if (duty.State != QuestState.RUNNING)
			{
				return false;
			}
			return _quest.Stop(((__DutyModel__)duty).Fmt);
		}

		public List<IReward> RecieveRewards(int duty_no)
		{
			DutyModel duty = GetDuty(duty_no);
			if (duty.State != QuestState.COMPLETE)
			{
				return null;
			}
			List<QuestItemGetFmt> list = _quest.ClearItemGet(((__DutyModel__)duty).Fmt);
			if (list == null)
			{
				return null;
			}
			List<IReward> list2 = new List<IReward>();
			List<IReward_Material> list3 = null;
			Reward_Useitems reward_Useitems = null;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Category == QuestItemGetKind.Material && list[i].Count > 0)
				{
					if (list3 == null)
					{
						list3 = new List<IReward_Material>();
						Reward_Materials item = new Reward_Materials(list3);
						list2.Add(item);
					}
					list3.Add(new Reward_Material((enumMaterialCategory)list[i].Id, list[i].Count));
				}
				else if (list[i].Category == QuestItemGetKind.Deck)
				{
					list2.Add(new Reward_Deck(list[i].Id));
					base.UserInfo.__UpdateDeck__(new Api_get_Member());
				}
				else if (list[i].Category == QuestItemGetKind.FurnitureBox)
				{
					if (reward_Useitems == null)
					{
						reward_Useitems = new Reward_Useitems();
						list2.Add(reward_Useitems);
					}
					reward_Useitems.__Add__(list[i].Id, list[i].Count);
				}
				else if (list[i].Category == QuestItemGetKind.LargeBuild)
				{
					list2.Add(new Reward_LargeBuild());
				}
				else if (list[i].Category == QuestItemGetKind.Ship)
				{
					for (int j = 0; j < list[i].Count; j++)
					{
						list2.Add(new Reward_Ship(list[i].Id));
					}
					base.UserInfo.__UpdateShips__(new Api_get_Member());
				}
				else if (list[i].Category == QuestItemGetKind.SlotItem)
				{
					for (int k = 0; k < list[i].Count; k++)
					{
						list2.Add(new Reward_Slotitem(list[i].Id));
					}
				}
				else if (list[i].Category == QuestItemGetKind.UseItem)
				{
					list2.Add(new Reward_Useitem(list[i].Id, list[i].Count));
				}
				else if (list[i].Category == QuestItemGetKind.Furniture)
				{
					list2.Add(new Reward_Furniture(list[i].Id));
				}
				else if (list[i].Category == QuestItemGetKind.Exchange)
				{
					list2.Add(new Reward_Exchange_Slotitem(list[i].FromId, list[i].Id, list[i].IsUseCrewItem));
					List<SlotitemModel> slotitemList = base.UserInfo.GetDeck(1).GetFlagShip().SlotitemList;
					for (int l = 0; l < slotitemList.Count; l++)
					{
						if (slotitemList[l] != null)
						{
							slotitemList[l].__update__();
						}
					}
				}
				else if (list[i].Category == QuestItemGetKind.Spoint)
				{
					list2.Add(new Reward_SPoint(list[i].Count));
				}
				else if (list[i].Category == QuestItemGetKind.DeckPractice)
				{
					list2.Add(new Reward_DeckPracitce(list[i].Id));
				}
				else if (list[i].Category == QuestItemGetKind.Tanker)
				{
					list2.Add(new Reward_TransportCraft(list[i].Count));
					_UpdateTankerManager();
				}
			}
			List<IReward> list4 = list2.FindAll((IReward reward) => reward is Reward_Furniture);
			if (list4.Count > 0)
			{
				Api_Result<Dictionary<FurnitureKinds, List<Mst_furniture>>> api_Result = new Api_get_Member().FurnitureList();
				if (api_Result.state == Api_Result_State.Success)
				{
					for (int m = 0; m < list4.Count; m++)
					{
						Reward_Furniture reward_Furniture = (Reward_Furniture)list4[m];
						reward_Furniture.__Init__(api_Result.data);
					}
				}
			}
			_UpdateDutyData();
			return list2;
		}

		private bool _UpdateDutyData()
		{
			Api_Result<List<User_QuestFmt>> api_Result = _quest.QuestList();
			if (api_Result.state != 0 || api_Result.data == null)
			{
				return false;
			}
			List<User_QuestFmt> data = api_Result.data;
			_duties = data.ConvertAll((User_QuestFmt fmt) => new __DutyModel__(fmt));
			return true;
		}

		private int _CompareFunc(__DutyModel__ x, __DutyModel__ y)
		{
			int num = __CompareState(x, y);
			if (num != 0)
			{
				return num;
			}
			return __CompareNo(x, y);
		}

		private int __CompareState(__DutyModel__ x, __DutyModel__ y)
		{
			return y.State - x.State;
		}

		private int __CompareNo(__DutyModel__ x, __DutyModel__ y)
		{
			return x.No - y.No;
		}

		public override string ToString()
		{
			string str = base.ToString();
			str += $"  同時遂行最大数:{MaxExecuteCount}\n";
			str += "\n";
			str += $"--[任務一覧]--\n";
			str += $" - 遂行中の任務数:{GetExecutedDutyList().Count}\n";
			List<DutyModel> dutyList = GetDutyList();
			for (int i = 0; i < dutyList.Count; i++)
			{
				str += $"{dutyList[i]}\n";
			}
			return str + $"----\n";
		}
	}
}
