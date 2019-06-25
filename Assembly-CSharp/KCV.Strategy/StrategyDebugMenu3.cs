using Common.Enum;
using Common.Struct;
using KCV.PresetData;
using local.managers;
using local.models;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyDebugMenu3 : MonoBehaviour
	{
		private enum AreaGroup
		{
			Previous1,
			Previous2,
			Medium1,
			Medium2,
			Late,
			AllClear
		}

		[SerializeField]
		private UIButtonManager ButtonManager;

		[SerializeField]
		private UILabel Message;

		private Debug_Mod debugMod;

		private UserInfoModel UserInfo;

		[SerializeField]
		private GameObject DebugMenu1;

		private int NextMemID;

		private List<Entity_PresetShip.Param> ShipParamList;

		private PresetDataManager presetDataManager;

		private void Start()
		{
			debugMod = new Debug_Mod();
			UserInfo = StrategyTopTaskManager.GetLogicManager().UserInfo;
			ButtonManager.setButtonDelegate(Util.CreateEventDelegate(this, "OnPushPreset", null));
			TaskStrategyDebug.isControl = true;
			presetDataManager = new PresetDataManager();
			NextMemID = 2;
			ShipParamList = new List<Entity_PresetShip.Param>();
			presetDataManager.GetPresetShipParam("初期艦").MemID = 1;
		}

		public void OnDeside()
		{
			ButtonManager.setAllButtonEnable(enable: false);
			UIButton[] focusableButtons = ButtonManager.GetFocusableButtons();
			UIButton[] array = focusableButtons;
			foreach (UIButton uIButton in array)
			{
				if (uIButton != null)
				{
					uIButton.gameObject.SetActive(false);
				}
			}
			ButtonManager.nowForcusButton.SetActive(isActive: true);
			Message.text = "デ\u30fcタロ\u30fcド中です";
		}

		public void OnPushPreset()
		{
			int ButtonNo = ButtonManager.nowForcusIndex;
			OnDeside();
			this.DelayActionFrame(1, delegate
			{
				CreateDebugData(ButtonNo);
				Object.Destroy(StrategyTopTaskManager.Instance);
				Application.LoadLevel(Generics.Scene.Strategy.ToString());
			});
		}

		public void CreateDebugData(int ButtonNo)
		{
			Debug.Log(ButtonNo);
			App.CreateSaveDataNInitialize(UserInfo.Name, 54, UserInfo.Difficulty, isInherit: false);
			App.CreateSaveDataNInitialize(UserInfo.Name, 54, UserInfo.Difficulty, isInherit: false);
			SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = StrategyTopTaskManager.GetLogicManager().UserInfo.GetDeck(1);
			StrategyTopTaskManager.Instance.GetAreaMng().ChangeFocusTile(1, immediate: true);
			UserInfoModel userInfo = StrategyTopTaskManager.GetLogicManager().UserInfo;
			TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
			for (int i = 0; i < 20; i++)
			{
				tutorial.SetStepTutorialFlg(i);
			}
			for (int j = 0; j < 99; j++)
			{
				tutorial.SetKeyTutorialFlg(j);
			}
			LoadPresetData(ButtonNo);
			Debug_Mod.Add_SlotItemAll();
		}

		private void LoadPresetData(int PresetNo)
		{
			Entity_PresetData.Param presetData = presetDataManager.GetPresetData(PresetNo);
			Debug_Mod.SetFleetLevel(presetData.TeitokuLV);
			AddMaterialAndItems(presetData);
			Debug_Mod.Add_Tunker(presetData.Tanker);
			DeployTanker();
			AreaOpen(presetData);
			AddDeck(presetData);
			Debug_Mod.SetRebellionPhase(presetData.RebellionPhase);
			AddShips(presetData);
			SetDeck(presetData);
			SetAllShipsLevel(presetData.AllShipLevel);
			if (presetData.AddAllShip)
			{
				Debug_Mod.Add_ShipAll();
			}
		}

		public void ChangeDebugMode()
		{
			DebugMenu1.SetActive(true);
			base.gameObject.SetActive(false);
			TaskStrategyDebug.isControl = true;
		}

		private void AreaOpen(Entity_PresetData.Param Param)
		{
			for (int i = 0; i < 17; i++)
			{
				MapsOpen(i + 1, Param.Area[i]);
			}
		}

		private void MapsOpen(int AreaNo, int OpenNum)
		{
			for (int i = 1; i < OpenNum + 1; i++)
			{
				Debug_Mod.OpenMapArea(AreaNo, i);
			}
		}

		private void DeployTanker()
		{
			StrategyTopTaskManager.CreateLogicManager();
			for (int i = 1; i < 18; i++)
			{
				if (StrategyTopTaskManager.GetLogicManager().Area[i].IsOpen())
				{
					StrategyTopTaskManager.GetLogicManager().Deploy(i, 5, new EscortDeckManager(i));
				}
			}
		}

		private void AddDeck(Entity_PresetData.Param Param)
		{
			for (int i = 1; i <= 8; i++)
			{
				if (Param.Deck[i - 1] != 0)
				{
					debugMod.Add_Deck(i);
				}
			}
		}

		private void AddDeckShips(Entity_PresetDeck.Param DeckParam)
		{
			for (int i = 0; i < DeckParam.PresetShip.Length; i++)
			{
				if (DeckParam.PresetShip[i] != string.Empty)
				{
					Entity_PresetShip.Param ShipParam = presetDataManager.GetPresetShipParam(DeckParam.PresetShip[i]);
					if (!ShipParamList.Exists((Entity_PresetShip.Param s) => s == ShipParam))
					{
						ShipParamList.Add(ShipParam);
						ShipParam.MemID = NextMemID;
						NextMemID++;
					}
				}
			}
		}

		private void AddShips(Entity_PresetData.Param Param)
		{
			ShipParamList.Add(presetDataManager.GetPresetShipParam("初期艦"));
			for (int i = 0; i < Param.Deck.Length; i++)
			{
				if (Param.Deck[i] != 0)
				{
					AddDeckShips(presetDataManager.GetPresetDeck(Param.Deck[i]));
				}
			}
			List<int> list = new List<int>();
			for (int j = 0; j < ShipParamList.Count; j++)
			{
				list.Add(ShipParamList[j].MstID);
			}
			debugMod.Add_Ship(list);
		}

		private void AddSlotItems(AreaGroup group)
		{
			List<int> slot_ids = new List<int>();
			debugMod.Add_SlotItem(slot_ids);
		}

		private void SetDeck(Entity_PresetData.Param Param)
		{
			for (int i = 0; i < Param.Deck.Length; i++)
			{
				int num = Param.Deck[i];
				if (num != 0)
				{
					Entity_PresetDeck.Param presetDeck = presetDataManager.GetPresetDeck(num);
					List<int> list = new List<int>();
					for (int j = 0; j < presetDeck.PresetShip.Length; j++)
					{
						list.Add(presetDataManager.GetPresetShipParam(presetDeck.PresetShip[j]).MemID);
					}
					SetDeckShips(i + 1, list);
				}
			}
		}

		private void SetDeckShips(int DeckNo, List<int> memIDs)
		{
			OrganizeManager organizeManager = new OrganizeManager(1);
			for (int i = 0; i < memIDs.Count; i++)
			{
				organizeManager.ChangeOrganize(DeckNo, i, memIDs[i]);
			}
		}

		private void AddMaterialAndItems(Entity_PresetData.Param Param)
		{
			debugMod.Add_Materials(enumMaterialCategory.Fuel, Param.Fuel);
			debugMod.Add_Materials(enumMaterialCategory.Bull, Param.Bull);
			debugMod.Add_Materials(enumMaterialCategory.Steel, Param.Steel);
			debugMod.Add_Materials(enumMaterialCategory.Bauxite, Param.Baux);
			debugMod.Add_Materials(enumMaterialCategory.Dev_Kit, Param.Dev_Kit);
			debugMod.Add_Materials(enumMaterialCategory.Build_Kit, Param.BuildKit);
			debugMod.Add_Materials(enumMaterialCategory.Repair_Kit, Param.RepairKit);
			debugMod.Add_Materials(enumMaterialCategory.Revamp_Kit, Param.Revamp_Kit);
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int i = 0; i < 100; i++)
			{
				if ((1 <= i && i <= 3) || (10 <= i && i <= 12) || (49 <= i && i <= 59))
				{
					dictionary[i] = Param.Items;
				}
			}
			debugMod.Add_UseItem(dictionary);
			debugMod.Add_Spoint(Param.Spoint);
			debugMod.Add_Coin(Param.Coin);
		}

		private void SetAllShipsLevel(int LV)
		{
			if (LV == 0)
			{
				return;
			}
			UserInfo = StrategyTopTaskManager.GetLogicManager().UserInfo;
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Get_MstLevel(shipTable: true);
			int num = 1;
			while (true)
			{
				int num2 = num;
				MemberMaxInfo memberMaxInfo = UserInfo.ShipCountData();
				if (num2 < memberMaxInfo.NowCount + 1)
				{
					UserInfo.GetShip(num).AddExp(dictionary[LV] - UserInfo.GetShip(num).Exp);
					num++;
					continue;
				}
				break;
			}
		}
	}
}
