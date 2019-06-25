using Common.Enum;
using local.models;
using local.utils;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.managers
{
	public class PortManager : TurnManager
	{
		private int _area_id;

		private int _yubiwa_num;

		public MapAreaModel MapArea => ManagerBase._area[_area_id];

		public int YubiwaNum => _yubiwa_num;

		public PortManager(int area_id)
		{
			_area_id = area_id;
			ManagerBase._userInfoModel.__UpdateDeck__(new Api_get_Member());
			_CreateMapAreaModel();
			_yubiwa_num = new UseitemUtil().GetCount(55);
		}

		public Dictionary<FurnitureKinds, FurnitureModel> GetFurnitures(int deck_id)
		{
			Api_Result<List<Mst_furniture>> api_Result = new Api_get_Member().DecorateFurniture(deck_id);
			if (api_Result.state != 0)
			{
				return null;
			}
			Dictionary<FurnitureKinds, FurnitureModel> dictionary = new Dictionary<FurnitureKinds, FurnitureModel>();
			for (int i = 0; i < api_Result.data.Count; i++)
			{
				FurnitureModel furnitureModel = new FurnitureModel(api_Result.data[i], string.Empty);
				dictionary.Add(furnitureModel.Type, furnitureModel);
			}
			return dictionary;
		}

		public List<Mst_bgm_jukebox> GetJukeboxList()
		{
			return Mst_DataManager.Instance.GetJukeBoxList();
		}

		public bool PlayJukeboxBGM(int deck_id, int bgm_id)
		{
			Api_Result<bool> api_Result = new Api_req_furniture().BuyMusic(deck_id, bgm_id);
			return api_Result.state == Api_Result_State.Success;
		}

		public bool SetPortBGM(int deck_id, int bgm_id)
		{
			Api_Result<bool> api_Result = new Api_req_furniture().SetPortBGMFromJukeBoxList(deck_id, bgm_id);
			return api_Result.state == Api_Result_State.Success;
		}

		public bool IsValidMarriage(int ship_mem_id)
		{
			return new Api_req_Kaisou().ValidMarriage(ship_mem_id);
		}

		public bool Marriage(int ship_mem_id)
		{
			Api_Result<Mem_ship> api_Result = new Api_req_Kaisou().Marriage(ship_mem_id);
			if (api_Result.state != 0 || api_Result.data == null)
			{
				return false;
			}
			return true;
		}

		public override string ToString()
		{
			string str = $"[--PortManager--]\n";
			str += $"{base.ToString()}\n";
			str += $"海域ID:{MapArea.Id}\n";
			str += $"指輪の数(ケッコンカッコカリ):{YubiwaNum}\n";
			DeckModel[] decksFromArea = base.UserInfo.GetDecksFromArea(MapArea.Id);
			foreach (DeckModel deckModel in decksFromArea)
			{
				str += $"== 艦隊:{deckModel} (母港BGMID:{base.UserInfo.GetPortBGMId(deckModel.Id)})==\n";
				Dictionary<FurnitureKinds, FurnitureModel> furnitures = GetFurnitures(deckModel.Id);
				str += $"[設定家具]\n";
				str += $"床:{furnitures[FurnitureKinds.Floor]}\t壁紙:{furnitures[FurnitureKinds.Wall]}\t窓:{furnitures[FurnitureKinds.Window]}\n";
				str += $"壁掛け:{furnitures[FurnitureKinds.Hangings]}\t棚:{furnitures[FurnitureKinds.Chest]}\t机:{furnitures[FurnitureKinds.Desk]}\n";
			}
			List<Mst_bgm_jukebox> jukeboxList = GetJukeboxList();
			str += $"[JUKE]\n";
			for (int j = 0; j < jukeboxList.Count; j++)
			{
				Mst_bgm_jukebox mst_bgm_jukebox = jukeboxList[j];
				str += string.Format("JUKE{0} {1} 家具コイン:{2} ル\u30fcプ:{3} BGM設定:{4} {5}\n", mst_bgm_jukebox.Bgm_id, mst_bgm_jukebox.Name, mst_bgm_jukebox.R_coins, mst_bgm_jukebox.Loops, (mst_bgm_jukebox.Bgm_flag != 1) ? "不可" : "可能", mst_bgm_jukebox.Remarks);
			}
			return str + $"[--PortManager--]\n";
		}
	}
}
