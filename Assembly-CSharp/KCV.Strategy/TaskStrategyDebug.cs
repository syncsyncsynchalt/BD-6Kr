using Common.Enum;
using Common.Struct;
using KCV.Strategy.Rebellion;
using KCV.Utils;
using local.managers;
using local.models;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PSVita;

namespace KCV.Strategy
{
	public class TaskStrategyDebug : SceneTaskMono
	{
		private enum Mode
		{
			CategorySelect,
			Material,
			Ship,
			SlotItem,
			Deck,
			Area,
			Slog,
			Heal,
			Large,
			Rebellion
		}

		private KeyControl keyController;

		[SerializeField]
		private GameObject rootPrefab;

		[SerializeField]
		private GameObject cursol;

		[SerializeField]
		private UILabel[] materialsLabel;

		[SerializeField]
		private UILabel[] materialsNum;

		[SerializeField]
		private UILabel MstID;

		[SerializeField]
		private UILabel MemID;

		[SerializeField]
		private UILabel[] ShipName;

		[SerializeField]
		private UILabel ShipLevel;

		[SerializeField]
		private UILabel ItemMstID;

		[SerializeField]
		private UILabel ItemName;

		[SerializeField]
		private UILabel DeckNum;

		[SerializeField]
		private UILabel AreaOpenNo;

		[SerializeField]
		private UILabel MapOpenNo;

		[SerializeField]
		private UILabel ClearState;

		[SerializeField]
		private UILabel[] Category;

		[SerializeField]
		private UILabel SlogOnOff;

		[SerializeField]
		private UILabel LargeOnOff;

		[SerializeField]
		private UILabel RebellionForceOnOff;

		[SerializeField]
		private Transform DebugMode1;

		[SerializeField]
		private Transform DebugMode3;

		[SerializeField]
		private Transform DebugMenuNormal;

		[SerializeField]
		private UnloadAtlas unloadAtlas;

		private int mag;

		private Vector3 cursolOffset;

		private StrategyMapManager logicMng;

		public static bool isControl;

		public static bool ForceEnding;

		private Mode nowMode;

		private Debug_Mod debugMod;

		private List<int> openAreaIDs;

		private List<int> AddMstIDs;

		private int materialPhase;

		private int nowMaterial;

		private bool AreaModeCursol = true;

		private int maxIndex;

		[SerializeField]
		private TaskStrategyDebug AnotherMode;

		private Coroutine turnend;

		protected override bool Init()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			maxIndex = Category.Length - 1;
			logicMng = new StrategyMapManager();
			rootPrefab.SetActive(true);
			keyController = new KeyControl(0, maxIndex);
			keyController.setChangeValue(-1f, 0f, 1f, 0f);
			keyController.HoldJudgeTime = 1f;
			nowMode = Mode.CategorySelect;
			cursolOffset = new Vector3(-77f, -20f, 0f);
			MstID.text = "1";
			MemID.text = "1";
			debugMod = new Debug_Mod();
			materialsNum[0].text = StrategyTopTaskManager.GetLogicManager().Material.Fuel.ToString();
			materialsNum[1].text = StrategyTopTaskManager.GetLogicManager().Material.Ammo.ToString();
			materialsNum[2].text = StrategyTopTaskManager.GetLogicManager().Material.Steel.ToString();
			materialsNum[3].text = StrategyTopTaskManager.GetLogicManager().Material.Baux.ToString();
			materialsNum[4].text = StrategyTopTaskManager.GetLogicManager().Material.Devkit.ToString();
			materialsNum[5].text = StrategyTopTaskManager.GetLogicManager().Material.RepairKit.ToString();
			materialsNum[6].text = StrategyTopTaskManager.GetLogicManager().Material.BuildKit.ToString();
			materialsNum[7].text = StrategyTopTaskManager.GetLogicManager().Material.Revkit.ToString();
			materialsNum[8].text = StrategyTopTaskManager.GetLogicManager().GetNonDeploymentTankerCount().GetCountNoMove()
				.ToString();
			materialsNum[9].text = StrategyTopTaskManager.GetLogicManager().UserInfo.FCoin.ToString();
			materialsNum[10].text = StrategyTopTaskManager.GetLogicManager().UserInfo.SPoint.ToString();
			materialsNum[11].text = "0";
			int focusAreaID = StrategyAreaManager.FocusAreaID;
			AreaOpenNo.text = focusAreaID.ToString();
			if (StrategyTopTaskManager.GetLogicManager().SelectArea(focusAreaID).Maps[0].Cleared)
			{
			}
			openAreaIDs = new List<int>();
			AddMstIDs = new List<int>();
			ShipName[0].text = new ShipModelMst(1).Name;
			ShipModel ship = StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(1);
			if (ship != null)
			{
				ShipName[1].text = ship.Name;
				ShipLevel.text = ship.Level.ToString();
			}
			ItemMstID.text = "1";
			SlogOnOff.text = SingletonMonoBehaviour<AppInformation>.Instance.SlogDraw.ToString();
			DeckNum.text = StrategyTopTaskManager.GetLogicManager().UserInfo.DeckCount.ToString();
			LargeOnOff.text = ((!new ArsenalManager().LargeEnabled) ? "OFF" : "ON");
			return true;
		}

		protected override bool UnInit()
		{
			if (StrategyTopTaskManager.Instance != null)
			{
				StrategyTopTaskManager.Instance.GetInfoMng().updateFooterInfo(isUpdateMaterial: true);
				StrategyTopTaskManager.Instance.GetInfoMng().updateUpperInfo();
			}
			return true;
		}

		protected override bool Run()
		{
			keyController.Update();
			if (!isControl)
			{
				if (keyController.keyState[0].down)
				{
					rootPrefab.SetActive(false);
					StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_ST);
					return false;
				}
				return true;
			}
			if (keyController.keyState[1].down)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
			setMag();
			if (keyController.keyState[6].down && keyController.keyState[5].press)
			{
				Application.LoadLevel(Generics.Scene.Ending.ToString());
			}
			else if (keyController.keyState[5].press && keyController.keyState[2].down)
			{
				App.isInvincible = !App.isInvincible;
				CommonPopupDialog.Instance.StartPopup("無敵モード" + App.isInvincible);
			}
			else if (keyController.keyState[4].press && keyController.keyState[2].down)
			{
				for (int i = 1; i < 15; i++)
				{
					EscortDeckManager escortDeckManager = new EscortDeckManager(i);
					MemberMaxInfo memberMaxInfo = StrategyTopTaskManager.GetLogicManager().UserInfo.ShipCountData();
					if (memberMaxInfo.NowCount > 300)
					{
						for (int j = 0; j < 300 && (StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(1 + j).IsInEscortDeck() != -1 || !escortDeckManager.ChangeOrganize(6, 1 + j)); j++)
						{
						}
					}
					StrategyTopTaskManager.GetLogicManager().Deploy(i, 10, escortDeckManager);
				}
				CommonPopupDialog.Instance.StartPopup("自動配備しました");
			}
			else if (keyController.keyState[6].down && keyController.keyState[4].press)
			{
				CommonPopupDialog.Instance.StartPopup("ゲームクリア！！");
				ForceEnding = true;
			}
			else if (keyController.keyState[6].down && keyController.keyState[3].press)
			{
				if (turnend == null)
				{
					turnend = StartCoroutine(TurnEndSpeed(3495));
				}
			}
			else if (keyController.keyState[6].press && keyController.keyState[2].press)
			{
				StrategyTopTaskManager.GetTurnEnd().DebugTurnEnd();
				CommonPopupDialog.Instance.StartPopup(StrategyTopTaskManager.GetLogicManager().Turn.ToString());
			}
			else if (keyController.keyState[6].down)
			{
				StrategyTopTaskManager.Instance.GameOver();
				keyController.firstUpdate = true;
			}
			else if (keyController.keyState[3].down)
			{
				TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
				for (int k = 0; k < 20; k++)
				{
					tutorial.SetStepTutorialFlg(k);
				}
				for (int l = 0; l < 99; l++)
				{
					tutorial.SetKeyTutorialFlg(l);
				}
				if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().HideAndDestroy();
				}
			}
			if (keyController.keyState[7].down && keyController.keyState[4].press)
			{
				GameObject.Find("SingletonObject").AddComponent<TEST_Voyage>().StartVoyage();
			}
			if (keyController.keyState[7].down && keyController.keyState[5].press)
			{
				UnityEngine.Object.Destroy(GameObject.Find("Live2DRender").gameObject);
				UnityEngine.Object.Destroy(SingletonMonoBehaviour<PortObjectManager>.Instance.gameObject);
				UnityEngine.Object.Destroy(GameObject.Find("SingletonObject").gameObject);
				this.DelayActionFrame(3, delegate
				{
					Application.LoadLevel("TestEmptyScene");
					this.DelayAction(5f, delegate
					{
						Resources.UnloadUnusedAssets();
					});
				});
			}
			if (keyController.keyState[4].hold && keyController.keyState[5].hold)
			{
				if (base.gameObject.name == "DebugMenuNormal")
				{
					DebugMenuNormal.SetActive(isActive: false);
					DebugMode1.SetActive(isActive: true);
					DebugMode3.SetActive(isActive: true);
				}
				else
				{
					DebugMenuNormal.SetActive(isActive: true);
					DebugMode1.SetActive(isActive: false);
					DebugMode3.SetActive(isActive: false);
				}
				StrategyTopTaskManager.SetDebug(AnotherMode);
				StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.Debug);
				return false;
			}
			if (!Diagnostics.enableHUD && keyController.keyState[4].press && keyController.keyState[5].press && keyController.keyState[2].down)
			{
				Diagnostics.enableHUD = true;
			}
			return ModeRun();
		}

		private IEnumerator TurnEndSpeed(int turn)
		{
			for (int i = 0; i < turn; i++)
			{
				StrategyTopTaskManager.GetTurnEnd().DebugTurnEnd();
				if (i % 100 == 0)
				{
					yield return new WaitForEndOfFrame();
					CommonPopupDialog.Instance.StartPopup(StrategyTopTaskManager.GetLogicManager().Turn.ToString());
				}
			}
			yield return null;
			CommonPopupDialog.Instance.StartPopup(StrategyTopTaskManager.GetLogicManager().Turn.ToString());
		}

		private void setMag()
		{
			if (keyController.keyState[3].press)
			{
				mag = 10;
			}
			else if (keyController.keyState[2].press)
			{
				mag = 100;
			}
			else if (keyController.keyState[5].press)
			{
				mag = 1000;
			}
			else if (keyController.keyState[4].press)
			{
				mag = 10000;
			}
			else
			{
				mag = 1;
			}
		}

		private bool ModeRun()
		{
			switch (nowMode)
			{
				case Mode.CategorySelect:
					return CategorySelectMode();
				case Mode.Material:
					MaterialMode();
					break;
				case Mode.Ship:
					ShipMode();
					break;
				case Mode.SlotItem:
					SlotItemMode();
					break;
				case Mode.Deck:
					DeckMode();
					break;
				case Mode.Area:
					AreaMode();
					break;
				case Mode.Slog:
					SlogMode();
					break;
				case Mode.Heal:
					HealMode();
					break;
				case Mode.Large:
					LargeMode();
					break;
				case Mode.Rebellion:
					RebellionMode();
					break;
			}
			return true;
		}

		private void ChangeMode(int nextMode)
		{
			nowMode = (Mode)nextMode;
			keyController.Index = 0;
			switch (nowMode)
			{
				case Mode.CategorySelect:
					keyController.Index = (int)nowMode;
					keyController.maxIndex = maxIndex;
					keyController.setChangeValue(-1f, 0f, 1f, 0f);
					break;
				case Mode.Material:
					keyController.maxIndex = 11;
					break;
				case Mode.Ship:
					keyController.maxIndex = 1;
					break;
				case Mode.SlotItem:
					keyController.maxIndex = 999;
					keyController.Index = 1;
					keyController.setChangeValue(0f, 1f, 0f, -1f);
					break;
				case Mode.Deck:
					keyController.maxIndex = 0;
					break;
				case Mode.Area:
					keyController.maxIndex = 0;
					break;
			}
		}

		private bool CategorySelectMode()
		{
			keyController.SilentChangeIndex(SeachActiveIndex(keyController.Index, Category, keyController.prevIndexChangeValue == 1));
			cursol.transform.position = Category[keyController.Index].transform.position;
			cursol.transform.localPosition += cursolOffset;
			if (keyController.keyState[1].down)
			{
				ChangeMode(keyController.Index + 1);
			}
			if (keyController.keyState[0].down)
			{
				rootPrefab.SetActive(false);
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_ST);
				return false;
			}
			if (keyController.keyState[3].down)
			{
				Live2DModel.__DEBUG_MotionNAME_Draw = !Live2DModel.__DEBUG_MotionNAME_Draw;
				CommonPopupDialog.Instance.StartPopup("モーション名表示:" + Live2DModel.__DEBUG_MotionNAME_Draw);
			}
			return true;
		}

		private int SeachActiveIndex(int index, MonoBehaviour[] Array, bool isPositive)
		{
			int num = isPositive ? 1 : (-1);
			for (int i = 0; i < Array.Length; i++)
			{
				if (Array[index].isActiveAndEnabled)
				{
					break;
				}
				index = (int)Util.LoopValue(index + num, 0f, Array.Length - 1);
			}
			return index;
		}

		private void MaterialMode()
		{
			if (materialPhase == 0)
			{
				keyController.SilentChangeIndex(SeachActiveIndex(keyController.Index, materialsLabel, keyController.prevIndexChangeValue == 1));
				cursol.transform.position = materialsLabel[keyController.Index].transform.position;
				cursol.transform.localPosition += cursolOffset;
			}
			else
			{
				cursol.transform.position = materialsNum[nowMaterial].transform.position;
				cursol.transform.localPosition += new Vector3(-150f, -20f, 0f);
				if (nowMaterial != 8)
				{
					if (keyController.keyState[8].down)
					{
						materialsNum[nowMaterial].text = (Convert.ToInt32(materialsNum[nowMaterial].text) + mag).ToString();
					}
					if (keyController.keyState[12].down)
					{
						materialsNum[nowMaterial].text = (Convert.ToInt32(materialsNum[nowMaterial].text) - mag).ToString();
					}
				}
				else if (keyController.keyState[1].down)
				{
					if (mag > 10)
					{
						mag = 10;
					}
					Debug_Mod.Add_Tunker(1 * mag);
					StrategyTopTaskManager.CreateLogicManager();
					materialsNum[8].text = StrategyTopTaskManager.GetLogicManager().GetNonDeploymentTankerCount().GetCountNoMove()
						.ToString();
				}
			}
			if (keyController.keyState[1].down && materialPhase == 0)
			{
				materialPhase = 1;
				nowMaterial = keyController.Index;
			}
			if (!keyController.keyState[0].down)
			{
				return;
			}
			if (materialPhase == 0)
			{
				ChangeMode(0);
				StrategyMapManager logicManager = StrategyTopTaskManager.GetLogicManager();
				debugMod.Add_Materials(enumMaterialCategory.Fuel, Convert.ToInt32(materialsNum[0].text) - logicManager.Material.Fuel);
				debugMod.Add_Materials(enumMaterialCategory.Bull, Convert.ToInt32(materialsNum[1].text) - logicManager.Material.Ammo);
				debugMod.Add_Materials(enumMaterialCategory.Steel, Convert.ToInt32(materialsNum[2].text) - logicManager.Material.Steel);
				debugMod.Add_Materials(enumMaterialCategory.Bauxite, Convert.ToInt32(materialsNum[3].text) - logicManager.Material.Baux);
				debugMod.Add_Materials(enumMaterialCategory.Dev_Kit, Convert.ToInt32(materialsNum[4].text) - logicManager.Material.Devkit);
				debugMod.Add_Materials(enumMaterialCategory.Repair_Kit, Convert.ToInt32(materialsNum[5].text) - logicManager.Material.RepairKit);
				debugMod.Add_Materials(enumMaterialCategory.Build_Kit, Convert.ToInt32(materialsNum[6].text) - logicManager.Material.BuildKit);
				debugMod.Add_Materials(enumMaterialCategory.Revamp_Kit, Convert.ToInt32(materialsNum[7].text) - logicManager.Material.Revkit);
				debugMod.Add_Coin(Convert.ToInt32(materialsNum[9].text) - logicManager.UserInfo.FCoin);
				debugMod.Add_Spoint(Convert.ToInt32(materialsNum[10].text) - logicManager.UserInfo.SPoint);
				Dictionary<int, int> dictionary = new Dictionary<int, int>();
				for (int i = 0; i < 100; i++)
				{
					if ((1 <= i && i <= 3) || (10 <= i && i <= 12) || (49 <= i && i <= 59))
					{
						dictionary[i] = Convert.ToInt32(materialsNum[11].text);
					}
				}
				debugMod.Add_UseItem(dictionary);
			}
			else
			{
				materialPhase = 0;
				keyController.Index = nowMaterial;
			}
		}

		private void ShipMode()
		{
			int num = Convert.ToInt32(MstID.text);
			if (keyController.keyState[0].down)
			{
				ChangeMode(0);
				debugMod.Add_Ship(AddMstIDs);
				AddMstIDs.Clear();
			}
			if (keyController.Index == 0)
			{
				cursol.transform.position = MstID.transform.position;
				cursol.transform.localPosition += cursolOffset;
				if (keyController.keyState[1].down && ShipName[0].text != string.Empty && ShipName[0].text != "なし")
				{
					AddMstIDs.Add(num);
					logicMng = new StrategyMapManager();
				}
				if (keyController.keyState[10].down || keyController.keyState[14].down)
				{
					int num2 = mag;
					if (keyController.keyState[14].down)
					{
						num2 = -mag;
					}
					num += num2;
					MstID.text = num.ToString();
					if (Mst_DataManager.Instance.Mst_ship.ContainsKey(num))
					{
						ShipModelMst shipModelMst = new ShipModelMst(Convert.ToInt32(MstID.text));
						ShipName[0].text = shipModelMst.Name;
					}
					else
					{
						ShipName[0].text = string.Empty;
					}
				}
			}
			else
			{
				if (keyController.Index != 1)
				{
					return;
				}
				cursol.transform.position = MemID.transform.position;
				cursol.transform.localPosition += cursolOffset;
				if (keyController.keyState[1].down)
				{
					ShipLevelUp(1);
				}
				else if (keyController.keyState[2].down)
				{
					ShipLevelUp(10);
				}
				else if (keyController.keyState[3].down)
				{
					ShipLevelUp(100);
				}
				else if (keyController.keyState[5].down)
				{
					ShipLevelUp(100);
					int ship_mem_id = Convert.ToInt32(MemID.text);
					ShipModel ship = StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(ship_mem_id);
					PortManager portManager = new PortManager(1);
					Dictionary<int, int> dictionary = new Dictionary<int, int>();
					dictionary[55] = 1;
					debugMod.Add_UseItem(dictionary);
					portManager.Marriage(ship.MemId);
					ShipLevel.text = ship.Level.ToString();
				}
				else if (keyController.keyState[10].down || keyController.keyState[14].down)
				{
					int num3 = mag;
					if (keyController.keyState[14].down)
					{
						num3 = -mag;
					}
					int ship_mem_id2 = Convert.ToInt32(MemID.text) + num3;
					MemID.text = ship_mem_id2.ToString();
					if (StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(ship_mem_id2) != null)
					{
						ShipModel ship2 = StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(ship_mem_id2);
						ShipName[1].text = ship2.Name;
						ShipLevel.text = StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(ship_mem_id2).Level.ToString();
					}
					else
					{
						ShipName[1].text = "NONE";
					}
				}
			}
		}

		private void ShipLevelUp(int AddLevel)
		{
			if (ShipName[1].text != "NONE")
			{
				int ship_mem_id = Convert.ToInt32(MemID.text);
				ShipModel ship = StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(ship_mem_id);
				for (int i = 0; i < AddLevel; i++)
				{
					ship.AddExp(ship.Exp_Next);
				}
				ShipLevel.text = ship.Level.ToString();
			}
		}

		private void SlotItemMode()
		{
			cursol.transform.position = ItemMstID.transform.position;
			cursol.transform.localPosition += cursolOffset;
			if (keyController.IsChangeIndex)
			{
				int num = 0;
				if (keyController.keyState[10].down)
				{
					num = 1;
				}
				if (keyController.keyState[14].down)
				{
					num = -1;
				}
				keyController.Index = (int)Util.RangeValue(keyController.Index + num * mag - num, 1f, 150f);
				ItemMstID.textInt = keyController.Index;
				SlotitemModel_Mst slotitemModel_Mst = new SlotitemModel_Mst(keyController.Index);
				if (slotitemModel_Mst != null)
				{
					ItemName.text = slotitemModel_Mst.Name;
				}
				else
				{
					ItemName.text = "NONE";
				}
			}
			if (keyController.keyState[1].down)
			{
				AddMstIDs.Add(keyController.Index);
			}
			if (keyController.keyState[0].down)
			{
				ChangeMode(0);
				debugMod.Add_SlotItem(AddMstIDs);
				AddMstIDs.Clear();
			}
		}

		private void DeckMode()
		{
			cursol.transform.position = DeckNum.transform.position;
			cursol.transform.localPosition += cursolOffset;
			if (keyController.keyState[1].down)
			{
				int deckCount = StrategyTopTaskManager.GetLogicManager().UserInfo.DeckCount;
				if (deckCount < 8)
				{
					int rid = StrategyTopTaskManager.GetLogicManager().UserInfo.DeckCount + 1;
					debugMod.Add_Deck(rid);
				}
				DeckNum.text = StrategyTopTaskManager.GetLogicManager().UserInfo.DeckCount.ToString();
			}
			if (keyController.keyState[0].down)
			{
				ChangeMode(0);
			}
		}

		private void AreaMode()
		{
			int num = Convert.ToInt32(AreaOpenNo.text);
			int num2 = Convert.ToInt32(MapOpenNo.text);
			if (AreaModeCursol)
			{
				cursol.transform.position = AreaOpenNo.transform.position;
			}
			else
			{
				cursol.transform.position = MapOpenNo.transform.position;
			}
			cursol.transform.localPosition += cursolOffset;
			if (keyController.keyState[10].down || keyController.keyState[14].down)
			{
				AreaModeCursol = !AreaModeCursol;
			}
			if (keyController.keyState[12].down)
			{
				if (AreaModeCursol)
				{
					num--;
				}
				else
				{
					num2--;
				}
			}
			if (keyController.keyState[8].down)
			{
				if (AreaModeCursol)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
			if (keyController.keyState[1].down)
			{
				if (AreaModeCursol)
				{
					for (int i = 1; i < 7; i++)
					{
						Debug_Mod.OpenMapArea(num, i);
					}
				}
				else
				{
					Debug_Mod.OpenMapArea(num, num2);
				}
			}
			num = Util.FixRangeValue(num, 1, 17, 1);
			num2 = Util.FixRangeValue(num2, 1, 5, 1);
			AreaOpenNo.text = num.ToString();
			MapOpenNo.text = num2.ToString();
			if (StrategyTopTaskManager.GetLogicManager().SelectArea(num).Maps.Length > num2 - 1)
			{
				if (StrategyTopTaskManager.GetLogicManager().SelectArea(num).Maps[num2 - 1].Cleared)
				{
					ClearState.text = "状態：クリア済み";
				}
				else
				{
					ClearState.text = "状態：未クリア";
				}
			}
			else
			{
				ClearState.text = "マップが存在しません";
			}
			if (keyController.keyState[0].down)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("newOpenAreaIDs", openAreaIDs.ToArray());
				RetentionData.SetData(hashtable);
				ChangeMode(0);
			}
		}

		private void SlogMode()
		{
			cursol.transform.position = SlogOnOff.transform.position;
			cursol.transform.localPosition += cursolOffset;
			if (keyController.keyState[1].down)
			{
				SingletonMonoBehaviour<AppInformation>.Instance.SlogDraw = !SingletonMonoBehaviour<AppInformation>.Instance.SlogDraw;
				DebugUtils.ClearSLog();
				SlogOnOff.text = SingletonMonoBehaviour<AppInformation>.Instance.SlogDraw.ToString();
			}
			if (keyController.keyState[0].down)
			{
				ChangeMode(0);
			}
		}

		private void LargeMode()
		{
			cursol.transform.position = LargeOnOff.transform.position;
			cursol.transform.localPosition += cursolOffset;
			if (keyController.keyState[1].down)
			{
				Debug_Mod.OpenLargeDock();
				LargeOnOff.text = "ON";
			}
			if (keyController.keyState[0].down)
			{
				ChangeMode(0);
			}
		}

		private void HealMode()
		{
			Debug_Mod.DeckRefresh(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter3);
			ChangeMode(0);
		}

		private void RebellionMode()
		{
			cursol.transform.position = RebellionForceOnOff.transform.position;
			cursol.transform.localPosition += cursolOffset;
			if (keyController.keyState[1].down)
			{
				if (StrategyRebellionTaskManager.RebellionForceDebug)
				{
					RebellionForceOnOff.text = "OFF";
					StrategyRebellionTaskManager.RebellionForceDebug = false;
				}
				else
				{
					RebellionForceOnOff.text = "ON";
					StrategyRebellionTaskManager.RebellionForceDebug = true;
				}
			}
			if (keyController.keyState[0].down)
			{
				ChangeMode(0);
			}
		}

		public void ChangeDebugMode()
		{
			DebugMode3.SetActive(isActive: true);
			DebugMode1.SetActive(isActive: false);
			isControl = false;
		}

		private void OnDestroy()
		{
			keyController = null;
			rootPrefab = null;
			cursol = null;
			materialsLabel = null;
			materialsNum = null;
			MstID = null;
			MemID = null;
			ShipName = null;
			ShipLevel = null;
			ItemMstID = null;
			ItemName = null;
			DeckNum = null;
			AreaOpenNo = null;
			MapOpenNo = null;
			ClearState = null;
			Category = null;
			SlogOnOff = null;
			LargeOnOff = null;
			RebellionForceOnOff = null;
			DebugMode1 = null;
			DebugMode3 = null;
			AnotherMode = null;
		}
	}
}
