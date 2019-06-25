using Common.Enum;
using local.models;
using local.utils;
using Server_Common;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.managers
{
	public class RevampManager : ManagerBase
	{
		public const int MAX_REVAMP_LEVEL = 10;

		private Api_req_Kousyou _req;

		private DeckModel _deck;

		private List<RevampRecipeModel> _recipes;

		private List<Mem_slotitem> _all_items;

		private Dictionary<int, List<SlotitemModel>> _items;

		public DeckModel Deck => _deck;

		public RevampManager(int area_id, int deck_id)
		{
			_req = new Api_req_Kousyou();
			_req.Initialize_SlotitemRemodel();
			_deck = base.UserInfo.GetDeck(deck_id);
			_InitializeRecipes(deck_id);
			_InitializeSlotitems();
		}

		public RevampRecipeModel[] GetRecipes()
		{
			return _recipes.ToArray();
		}

		public SlotitemModel[] GetSlotitemList(int recipe_id)
		{
			if (_items.ContainsKey(recipe_id))
			{
				_items[recipe_id].Sort((SlotitemModel x, SlotitemModel y) => y.Level - x.Level);
				return _items[recipe_id].ToArray();
			}
			return new SlotitemModel[0];
		}

		public RevampRecipeDetailModel GetDetail(int recipe_id, int slotitem_mem_id)
		{
			Api_Result<Mst_slotitem_remodel_detail> slotitemRemodelListDetail = _req.getSlotitemRemodelListDetail(recipe_id, slotitem_mem_id);
			if (slotitemRemodelListDetail.state != 0)
			{
				return null;
			}
			RevampRecipeModel revampRecipeModel = _recipes.Find((RevampRecipeModel r) => r.RecipeId == recipe_id);
			if (revampRecipeModel == null)
			{
				return null;
			}
			SlotitemModel slotitemModel = _items[recipe_id].Find((SlotitemModel slotiitem) => slotiitem.MemId == slotitem_mem_id);
			if (slotitemModel == null)
			{
				return null;
			}
			return new RevampRecipeDetailModel(revampRecipeModel.__mst__, slotitemRemodelListDetail.data, slotitemModel);
		}

		public RevampValidationResult IsValidRevamp(RevampRecipeDetailModel detail)
		{
			if (detail.Slotitem.Level >= 10)
			{
				if (!detail.IsChange())
				{
					return RevampValidationResult.Max_Level;
				}
				if (detail.Slotitem.IsLocked())
				{
					return RevampValidationResult.Lock;
				}
			}
			if (base.Material.Fuel < detail.Fuel)
			{
				return RevampValidationResult.Less_Fuel;
			}
			if (base.Material.Ammo < detail.Ammo)
			{
				return RevampValidationResult.Less_Ammo;
			}
			if (base.Material.Steel < detail.Steel)
			{
				return RevampValidationResult.Less_Steel;
			}
			if (base.Material.Baux < detail.Baux)
			{
				return RevampValidationResult.Less_Baux;
			}
			if (base.Material.Devkit < detail.DevKit)
			{
				return RevampValidationResult.Less_Devkit;
			}
			if (base.Material.Revkit < detail.RevKit)
			{
				return RevampValidationResult.Less_Revkit;
			}
			if (detail.RequiredSlotitemCount > 0)
			{
				List<Mem_slotitem> list = _all_items.FindAll((Mem_slotitem item) => item.Slotitem_id == detail.RequiredSlotitemId && item.Level == 0 && item.Equip_flag == Mem_slotitem.enumEquipSts.Unset && item.Rid != detail.Slotitem.MemId);
				if (list.Count < detail.RequiredSlotitemCount)
				{
					return RevampValidationResult.Less_Slotitem;
				}
				list = list.FindAll((Mem_slotitem item) => !item.Lock);
				if (list.Count < detail.RequiredSlotitemCount)
				{
					return RevampValidationResult.Less_Slotitem_No_Lock;
				}
			}
			return RevampValidationResult.OK;
		}

		public SlotitemModel Revamp(RevampRecipeDetailModel detail)
		{
			Api_Result<bool> api_Result = _req.RemodelSlot(detail.__mst_detail__, Deck.Id, detail.Slotitem.MemId, detail.Determined);
			_all_items = null;
			_InitializeSlotitems();
			if (api_Result.state != 0 || !api_Result.data)
			{
				return null;
			}
			Comm_UserDatas.Instance.User_trophy.Revamp_count++;
			detail.Slotitem.__update__();
			return detail.Slotitem;
		}

		public ShipModel GetConsortShip(RevampRecipeDetailModel detail, out int ResourceID, out int voiceID)
		{
			ResourceID = 0;
			voiceID = 0;
			if (Deck.Count < 2)
			{
				return null;
			}
			ShipModel shipModel = Deck.GetShips()[1];
			if (shipModel == null)
			{
				return null;
			}
			voiceID = detail.__mst__.Voice_id;
			if (voiceID == 0)
			{
				return null;
			}
			ResourceID = detail.__mst__.GetVoiceShipId(shipModel.MstId);
			ResourceID = local.utils.Utils.GetResourceMstId(ResourceID);
			return shipModel;
		}

		private void _InitializeRecipes(int deck_id)
		{
			_recipes = new List<RevampRecipeModel>();
			Api_Result<Dictionary<int, List<Mst_slotitem_remodel>>> slotitemRemodelList = _req.getSlotitemRemodelList(deck_id);
			if (slotitemRemodelList.state == Api_Result_State.Success && slotitemRemodelList.data != null)
			{
				Dictionary<int, List<Mst_slotitem_remodel>> data = slotitemRemodelList.data;
				List<Mst_slotitem_remodel> list = new List<Mst_slotitem_remodel>();
				foreach (int key in data.Keys)
				{
					List<Mst_slotitem_remodel> list2 = data[key];
					if (list2 != null && list2.Count > 0)
					{
						list.AddRange(list2);
					}
				}
				list.Sort((Mst_slotitem_remodel a, Mst_slotitem_remodel b) => (a.Position > b.Position) ? 1 : (-1));
				_recipes = list.ConvertAll((Mst_slotitem_remodel mst) => new RevampRecipeModel(mst));
			}
		}

		private void _InitializeSlotitems()
		{
			if (_all_items == null)
			{
				Api_Result<Dictionary<int, Mem_slotitem>> api_Result = new Api_get_Member().Slotitem();
				if (api_Result.state != 0)
				{
					_all_items = new List<Mem_slotitem>();
				}
				else
				{
					_all_items = new List<Mem_slotitem>(api_Result.data.Values);
				}
			}
			if (_items == null)
			{
				_items = new Dictionary<int, List<SlotitemModel>>();
			}
			else
			{
				_items.Clear();
			}
			for (int i = 0; i < _recipes.Count; i++)
			{
				RevampRecipeModel recipe = _recipes[i];
				List<Mem_slotitem> list = _all_items.FindAll((Mem_slotitem item) => item.Slotitem_id == recipe.Slotitem.MstId);
				list = list.FindAll((Mem_slotitem item) => item.Equip_flag == Mem_slotitem.enumEquipSts.Unset);
				list.Sort((Mem_slotitem a, Mem_slotitem b) => (a.Level < b.Level) ? 1 : (-1));
				_items[recipe.RecipeId] = list.ConvertAll((Mem_slotitem item) => new SlotitemModel(item));
			}
		}

		public override string ToString()
		{
			string str = base.ToString();
			str += $"\n";
			str += $"対象の艦隊:{Deck}";
			str += $"\n\n";
			RevampRecipeModel[] recipes = GetRecipes();
			str += $"レシピ数:{recipes.Length}\n";
			for (int i = 0; i < recipes.Length; i++)
			{
				str += $"[{i}] {recipes[i]}\n";
			}
			return str;
		}
	}
}
