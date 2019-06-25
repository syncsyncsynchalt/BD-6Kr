using Common.Enum;
using local.models;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace local.managers
{
	public class ItemlistManager : ManagerBase
	{
		public class Result
		{
			private List<IReward> _rewards;

			private bool _limit_over;

			public IReward[] Rewards => _rewards.ToArray();

			public Result(List<IReward> rewards, bool limit_over)
			{
				_rewards = rewards;
				_limit_over = limit_over;
			}

			public bool IsLimitOver()
			{
				return _limit_over;
			}
		}

		public const int CABINET_NO = 3;

		private Dictionary<int, List<Mst_item_shop>> _mst_cabinet;

		private List<ItemlistModel> _have_items;

		private Dictionary<ItemlistModel, Mst_item_shop> _cabinet_relations;

		private Dictionary<int, string> _descriptions;

		public List<ItemlistModel> HaveItems => _have_items;

		public ItemlistManager()
		{
		}

		public ItemlistManager(Dictionary<int, List<Mst_item_shop>> mst_cabinet)
		{
			_mst_cabinet = mst_cabinet;
		}

		public virtual void Init()
		{
			_Init(all_item: false);
		}

		public ItemlistModel GetListItem(int useitem_mst_id)
		{
			return _have_items.Find((ItemlistModel item) => item.MstId == useitem_mst_id);
		}

		public Result UseItem(int useitem_mst_id, bool is_force, ItemExchangeKinds kinds)
		{
			Api_Result<User_ItemUseFmt> api_Result = new Api_req_Member().ItemUse(useitem_mst_id, is_force, kinds);
			if (api_Result.state != 0 || api_Result.data == null)
			{
				return null;
			}
			bool has_use_item_reward;
			List<IReward> rewards = _CreateRewardModels(useitem_mst_id, api_Result.data, out has_use_item_reward);
			Init();
			return new Result(rewards, api_Result.data.CautionFlag);
		}

		public ItemStoreManager CreateStoreManager()
		{
			return new ItemStoreManager(_mst_cabinet);
		}

		protected void _Init(bool all_item)
		{
			if (_descriptions == null)
			{
				_descriptions = Mst_DataManager.Instance.GetUseitemText();
			}
			_have_items = new List<ItemlistModel>();
			if (_mst_cabinet == null)
			{
				_mst_cabinet = Mst_DataManager.Instance.GetMstCabinet();
			}
			List<Mst_item_shop> list = _mst_cabinet[3];
			_cabinet_relations = new Dictionary<ItemlistModel, Mst_item_shop>();
			Api_Result<Dictionary<int, Mem_useitem>> api_Result = new Api_get_Member().UseItem();
			if (api_Result.state == Api_Result_State.Success)
			{
				Dictionary<int, Mst_useitem> mst_useitem = Mst_DataManager.Instance.Mst_useitem;
				Dictionary<int, Mem_useitem> dictionary = (api_Result.data != null) ? api_Result.data : new Dictionary<int, Mem_useitem>();
				if (all_item)
				{
					foreach (Mst_useitem value7 in mst_useitem.Values)
					{
						if (!(value7.Name == string.Empty))
						{
							Mem_useitem value = null;
							dictionary.TryGetValue(value7.Id, out value);
							_descriptions.TryGetValue(value7.Id, out string value2);
							ItemlistModel tmp = new ItemlistModel(value7, value, value2);
							_have_items.Add(tmp);
							Mst_item_shop value3 = list.Find((Mst_item_shop item) => item.Item1_id == tmp.MstId);
							_cabinet_relations.Add(tmp, value3);
						}
					}
					_have_items.Sort((ItemlistModel a, ItemlistModel b) => (a.MstId > b.MstId) ? 1 : (-1));
				}
				else
				{
					foreach (Mst_item_shop item in list)
					{
						int key = (item.Item1_type == 1) ? item.Item1_id : 0;
						Mst_useitem value4 = null;
						mst_useitem.TryGetValue(key, out value4);
						Mem_useitem value5 = null;
						dictionary.TryGetValue(key, out value5);
						string value6 = string.Empty;
						if (value4 != null)
						{
							_descriptions.TryGetValue(value4.Id, out value6);
						}
						ItemlistModel itemlistModel = new ItemlistModel(value4, value5, value6);
						_have_items.Add(itemlistModel);
						_cabinet_relations.Add(itemlistModel, item);
					}
				}
				__UpdateCount__();
				return;
			}
			throw new Exception("Logic Error");
		}

		private List<IReward> _CreateRewardModels(int used_useitem_mst_id, User_ItemUseFmt fmt, out bool has_use_item_reward)
		{
			has_use_item_reward = false;
			List<IReward> list = new List<IReward>();
			if (fmt.Material != null && fmt.Material.Count > 0)
			{
				List<IReward_Material> list2 = new List<IReward_Material>();
				Reward_Materials item = new Reward_Materials(list2);
				list.Add(item);
				_AddMaterialReward(list2, enumMaterialCategory.Fuel, fmt.Material);
				_AddMaterialReward(list2, enumMaterialCategory.Bull, fmt.Material);
				_AddMaterialReward(list2, enumMaterialCategory.Steel, fmt.Material);
				_AddMaterialReward(list2, enumMaterialCategory.Bauxite, fmt.Material);
				_AddMaterialReward(list2, enumMaterialCategory.Build_Kit, fmt.Material);
				_AddMaterialReward(list2, enumMaterialCategory.Repair_Kit, fmt.Material);
				_AddMaterialReward(list2, enumMaterialCategory.Dev_Kit, fmt.Material);
				_AddMaterialReward(list2, enumMaterialCategory.Revamp_Kit, fmt.Material);
			}
			if (fmt.GetItem != null && fmt.GetItem.Count > 0)
			{
				foreach (ItemGetFmt item4 in fmt.GetItem)
				{
					has_use_item_reward = true;
					Reward_Useitem reward_Useitem = new Reward_Useitem(item4.Id, item4.Count);
					if (reward_Useitem.Id == 53 && used_useitem_mst_id == 53)
					{
						Reward_PortExtend item2 = new Reward_PortExtend();
						list.Add(item2);
					}
					else if (reward_Useitem.Id == 63 && used_useitem_mst_id == 63)
					{
						Reward_DutyExtend item3 = new Reward_DutyExtend(base.UserInfo.MaxDutyExecuteCount);
						list.Add(item3);
					}
					else
					{
						list.Add(reward_Useitem);
					}
				}
				return list;
			}
			return list;
		}

		private void _AddMaterialReward(List<IReward_Material> container, enumMaterialCategory mat_type, Dictionary<enumMaterialCategory, int> reward)
		{
			if (reward.TryGetValue(mat_type, out int value))
			{
				Reward_Material item = new Reward_Material(mat_type, value);
				container.Add(item);
			}
		}

		private void __UpdateCount__()
		{
			Dictionary<int, Mem_slotitem> dictionary = null;
			for (int i = 0; i < _have_items.Count; i++)
			{
				ItemlistModel itemlistModel = _have_items[i];
				Mst_item_shop mst_cabinet = _cabinet_relations[itemlistModel];
				if (mst_cabinet == null || !mst_cabinet.IsChildReference() || mst_cabinet.Item2_type == 1)
				{
					itemlistModel.__SetOverrideCount__(0);
				}
				else if (mst_cabinet.Item2_type == 2)
				{
					if (dictionary == null)
					{
						dictionary = new Api_get_Member().Slotitem().data;
					}
					int value = dictionary.Count((KeyValuePair<int, Mem_slotitem> item) => item.Value.Slotitem_id == mst_cabinet.Item2_id);
					itemlistModel.__SetOverrideCount__(value);
				}
				else if (mst_cabinet.Item2_type == 3)
				{
					enumMaterialCategory item2_id = (enumMaterialCategory)mst_cabinet.Item2_id;
					int count = base.Material.GetCount(item2_id);
					itemlistModel.__SetOverrideCount__(count);
				}
				else
				{
					itemlistModel.__SetOverrideCount__(0);
				}
			}
		}

		public override string ToString()
		{
			string str = base.ToString();
			str += "\n";
			str += "-- 保有アイテム棚 --\n";
			for (int i = 0; i < HaveItems.Count; i++)
			{
				str += string.Format("棚{0} - {1}{2}\n", i, HaveItems[i], (!HaveItems[i].IsUsable()) ? string.Empty : "[使用可能]");
			}
			return str + "\n";
		}
	}
}
