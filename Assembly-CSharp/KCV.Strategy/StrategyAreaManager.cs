using KCV.Strategy.Rebellion;
using KCV.Utils;
using local.managers;
using local.models;
using local.utils;
using Server_Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyAreaManager : MonoBehaviour
	{
		private const int TILE_NUM = 18;

		[Button("CloseAreaDebug", "CloseArea", new object[]
		{

		})]
		public int button1;

		private GameObject Prefab_AreaOpenAnimation;

		[SerializeField]
		private GameObject Red_Sea;

		public StrategyTileRoutes tileRouteManager;

		public int DebugOpenAreaID;

		private bool isShipMoveWait;

		private ShipModel flagship;

		private int[] TogetherCloseTiles;

		public MapAreaModel FocusAreaModel;

		public int DebugRebellionArea;

		public int DebugCloseAreaID;

		public static KeyControl sailKeyController
		{
			get;
			private set;
		}

		public static int FocusAreaID => sailKeyController.Index;

		private StrategyHexTileManager tileManager => StrategyTopTaskManager.Instance.TileManager;

		private StrategyCamera mapCamera => StrategyTopTaskManager.Instance.UIModel.MapCamera;

		public void init()
		{
			tileManager.Init();
			tileManager.setAreaModels(StrategyTopTaskManager.GetLogicManager());
			makeSailSelectController();
			if (StrategyTopTaskManager.GetLogicManager().IsOpenedLastAreaAtLeastOnce())
			{
				Red_Sea.SetActive(true);
				Red_Sea.GetComponent<UIPanel>().alpha = 1f;
			}
		}

		public int[] getNewOpenArea()
		{
			int[] result = null;
			if (RetentionData.GetData() != null)
			{
				result = (int[])RetentionData.GetData()["newOpenAreaIDs"];
			}
			return result;
		}

		public void ChangeFocusTile(int areaID, bool immediate = false)
		{
			if (tileManager.FocusTile == null || areaID != tileManager.FocusTile.areaID)
			{
				tileManager.changeFocus(areaID);
				mapCamera.MoveToTargetTile(areaID, immediate);
				sailKeyController.Index = tileManager.FocusTile.areaID;
				StrategyTopTaskManager.Instance.GetAreaMng().FocusAreaModel = StrategyTopTaskManager.GetLogicManager().Area[areaID];
			}
		}

		public void UpdateSelectArea(int focusAreaID, bool immediate = false)
		{
			ChangeFocusTile(focusAreaID, immediate);
			StrategyTopTaskManager.Instance.GetInfoMng().updateFooterInfo(isUpdateMaterial: false);
			StrategyTopTaskManager.Instance.GetInfoMng().updateInfoPanel(focusAreaID);
		}

		public IEnumerator OpenArea(int[] newOpenAreaID)
		{
			bool Finished = false;
			for (int i = 0; i < newOpenAreaID.Length; i++)
			{
				Debug.Log("OpenArea");
				Prefab_AreaOpenAnimation = Resources.Load<GameObject>("Prefabs/StrategyPrefab/StrategyTop/AreaEnterAnimation");
				yield return StartCoroutine(Util.WaitEndOfFrames(3));
				mapCamera.MoveToTargetTile(newOpenAreaID[i]);
				yield return new WaitForSeconds(0.5f);
				tileManager.Tiles[newOpenAreaID[i]].SetVisibleDockIcons(isVisible: false);
				tileManager.Tiles[newOpenAreaID[i]].PlayAreaOpenAnimation(Prefab_AreaOpenAnimation, delegate
				{
					Finished = true;
				});
				while (!Finished)
				{
					yield return new WaitForEndOfFrame();
				}
				tileManager.Tiles[newOpenAreaID[i]].UpdateDockIcons();
				tileManager.Tiles[newOpenAreaID[i]].SetVisibleDockIcons(isVisible: true);
				if (newOpenAreaID[i] == 17)
				{
					Red_Sea.SetActive(true);
					TweenAlpha.Begin(Red_Sea, 3f, 1f);
					SoundUtils.PlaySE(SEFIleInfos.SE_041);
					yield return new WaitForSeconds(4f);
				}
				TrophyUtil.Unlock_ReOpenMap();
				Finished = false;
			}
			yield return new WaitForEndOfFrame();
			mapCamera.MoveToTargetTile(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID);
			yield return new WaitForSeconds(0.5f);
			List<int> openAreaID = new List<int>();
			makeOpenAreaIdArray(openAreaID);
			tileRouteManager.UpdateTileRouteState(openAreaID);
			sailKeyController.setEnableIndex(openAreaID.ToArray());
			Resources.UnloadAsset(Prefab_AreaOpenAnimation.GetComponent<UISprite>().atlas.spriteMaterial.mainTexture);
			yield return new WaitForEndOfFrame();
			yield return null;
		}

		public void setShipMove(bool isWait, ShipModel flagShip)
		{
			flagship = flagShip;
			isShipMoveWait = isWait;
		}

		public IEnumerator RebellionResult(RebellionMapManager RmapManager, bool isWin, int areaID)
		{
			if (RmapManager != null)
			{
				mapCamera.MoveToTargetTile(areaID);
				yield return new WaitForSeconds(0.5f);
				yield return StartCoroutine(ShowRebellionResult(isWin, areaID));
				yield return null;
			}
		}

		private IEnumerator ShowRebellionResult(bool isWin, int areaID)
		{
			RebellionWinLoseAnimation winloseAnimation = Util.Instantiate(Resources.Load("Prefabs/StrategyPrefab/Rebellion/RebellionWinLose") as GameObject, StrategyTopTaskManager.Instance.UIModel.OverView.gameObject).GetComponent<RebellionWinLoseAnimation>();
			yield return winloseAnimation.StartAnimation(isWin);
			UnityEngine.Object.Destroy(winloseAnimation);
			if (Server_Common.Utils.IsGameOver())
			{
				StrategyTopTaskManager.Instance.GameOver();
				yield break;
			}
			yield return null;
			StrategyTopTaskManager.Instance.ShipIconManager.setShipIconsState();
			if (isWin)
			{
				yield return StartCoroutine(RefreshArea());
				yield break;
			}
			yield return StartCoroutine(CloseArea(areaID, null));
			yield return StartCoroutine(ShowLoseGuide());
		}

		private IEnumerator ShowLoseGuide()
		{
			bool TutorialFinished = false;
			TutorialModel model = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
			if (SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(model, TutorialGuideManager.TutorialID.Rebellion_Lose, delegate
			{
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog().OnClosed = delegate
				{
                    TutorialFinished = true;
				};
			}))
			{
				while (!TutorialFinished)
				{
					yield return new WaitForEndOfFrame();
				}
			}
		}

		public RebellionMapManager checkRebellionResult()
		{
			Hashtable data = RetentionData.GetData();
			RebellionMapManager result = null;
			if (data != null)
			{
				result = (data["rebellionMapManager"] as RebellionMapManager);
			}
			return result;
		}

		public IEnumerator ClearRedSeaColor()
		{
			TweenAlpha.Begin(Red_Sea, 3f, 0f);
			yield return new WaitForSeconds(4f);
		}

		public IEnumerator RefreshArea()
		{
			bool finish = false;
			tileManager.RebellionTile.StartTileLightAnimation(delegate
			{
                finish = true;
			});
			while (!finish)
			{
				yield return new WaitForEndOfFrame();
			}
			yield return null;
		}

		public IEnumerator CloseArea(int closeAreaID, Action Onfinished)
		{
			bool Finished = false;
			StrategyTopTaskManager.Instance.UIModel.UIMapManager.TileManager.setVisibleFocusObject(isVisible: false);
			Coroutine cor = StartCoroutine(StrategyTopTaskManager.Instance.UIModel.UIMapManager.ShipIconManager.moveAreaAllShip(closeAreaID, isShipMoveWait));
			StrategyTopTaskManager.Instance.TileManager.Tiles[closeAreaID].SetVisibleDockIcons(isVisible: false);
			yield return cor;
			tileManager.Tiles[closeAreaID].PlayAreaCloseAnimation(delegate
			{
				if (this.TogetherCloseTiles != null)
				{
					this.DelayActionCoroutine(this.StartCoroutine(this.CloseTogetherTile()), delegate
					{
						this.mapCamera.MoveToTargetTile(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
                        Finished = true;
					});
				}
				else
				{
					this.mapCamera.MoveToTargetTile(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
                    Finished = true;
				}
			});
			List<int> OpenTileIDs = StrategyTopTaskManager.Instance.GetAreaMng().tileRouteManager.CreateOpenTileIDs();
			StrategyTopTaskManager.Instance.GetAreaMng().tileRouteManager.UpdateTileRouteState(OpenTileIDs);
			while (!Finished)
			{
				yield return new WaitForEndOfFrame();
			}
			List<int> openAreaID = new List<int>();
			makeOpenAreaIdArray(openAreaID);
			sailKeyController.setEnableIndex(openAreaID.ToArray());
			StrategyTopTaskManager.Instance.GetAreaMng().ChangeFocusTile(1);
			yield return new WaitForSeconds(0.5f);
			StrategyTopTaskManager.Instance.UIModel.UIMapManager.TileManager.setVisibleFocusObject(isVisible: true);
			Onfinished?.Invoke();
			yield return null;
		}

		private void CloseAreaDebug()
		{
			StartCoroutine(CloseArea(DebugCloseAreaID, null));
		}

		public void MakeTogetherCloseTilesList(int closeAreaID, int[] beforeOpenAreasID, int[] afterOpenAreasID)
		{
			TogetherCloseTiles = (from x in beforeOpenAreasID
				where !afterOpenAreasID.Any((int y) => x == y)
				where x != closeAreaID
				select x).ToArray();
		}

		public static int[] DicToIntArray(Dictionary<int, MapAreaModel> AreasID)
		{
			AreasID = (from x in AreasID
				where x.Value.IsOpen()
				select x).ToDictionary((KeyValuePair<int, MapAreaModel> Pair) => Pair.Key, (KeyValuePair<int, MapAreaModel> Pair) => Pair.Value);
			return AreasID.Keys.ToArray();
		}

		private IEnumerator CloseTogetherTile()
		{
			for (int j = 0; j < TogetherCloseTiles.Length; j++)
			{
				TweenAlpha.Begin(tileManager.Tiles[TogetherCloseTiles[j]].gameObject, 0.4f, 0f);
			}
			yield return new WaitForSeconds(0.6f);
			for (int i = 0; i < TogetherCloseTiles.Length; i++)
			{
				tileManager.Tiles[TogetherCloseTiles[i]].transform.localScale = Vector3.zero;
			}
		}

		private void makeOpenAreaIdArray(List<int> openAreaID)
		{
			for (int i = 1; i < StrategyTopTaskManager.GetLogicManager().Area.Count; i++)
			{
				if (StrategyTopTaskManager.GetLogicManager().Area[i].IsOpen())
				{
					openAreaID.Add(i);
				}
			}
		}

		public void OpenAllArea()
		{
			int[,] useIndexMap = new int[18, 8]
			{
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				},
				{
					8,
					8,
					8,
					11,
					7,
					9,
					9,
					0
				},
				{
					9,
					7,
					7,
					0,
					0,
					0,
					10,
					10
				},
				{
					13,
					13,
					13,
					0,
					12,
					8,
					8,
					0
				},
				{
					10,
					10,
					10,
					0,
					0,
					0,
					0,
					0
				},
				{
					11,
					6,
					6,
					14,
					14,
					0,
					7,
					7
				},
				{
					12,
					17,
					17,
					0,
					14,
					5,
					5,
					11
				},
				{
					1,
					11,
					11,
					5,
					5,
					2,
					2,
					9
				},
				{
					3,
					3,
					3,
					12,
					11,
					1,
					1,
					0
				},
				{
					1,
					1,
					1,
					7,
					2,
					10,
					10,
					0
				},
				{
					9,
					9,
					9,
					2,
					2,
					4,
					4,
					0
				},
				{
					8,
					12,
					12,
					6,
					5,
					7,
					7,
					1
				},
				{
					3,
					0,
					17,
					17,
					6,
					11,
					11,
					8
				},
				{
					0,
					0,
					15,
					15,
					3,
					3,
					3,
					0
				},
				{
					6,
					16,
					16,
					0,
					0,
					0,
					5,
					5
				},
				{
					13,
					0,
					0,
					0,
					16,
					17,
					17,
					13
				},
				{
					15,
					0,
					0,
					0,
					14,
					14,
					17,
					17
				},
				{
					15,
					15,
					15,
					16,
					16,
					6,
					6,
					12
				}
			};
			sailKeyController.setUseIndexMap(useIndexMap);
		}

		private void makeSailSelectController()
		{
			sailKeyController = new KeyControl(1, 17);
			int[,] useIndexMap = new int[18, 8]
			{
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				},
				{
					8,
					8,
					8,
					11,
					7,
					9,
					9,
					0
				},
				{
					9,
					7,
					7,
					0,
					0,
					0,
					10,
					10
				},
				{
					13,
					13,
					13,
					0,
					12,
					8,
					8,
					0
				},
				{
					10,
					10,
					10,
					0,
					0,
					0,
					0,
					0
				},
				{
					11,
					6,
					6,
					14,
					14,
					0,
					7,
					7
				},
				{
					12,
					17,
					17,
					0,
					14,
					5,
					5,
					11
				},
				{
					1,
					11,
					11,
					5,
					5,
					2,
					2,
					9
				},
				{
					3,
					3,
					3,
					12,
					11,
					1,
					1,
					0
				},
				{
					1,
					1,
					1,
					7,
					2,
					10,
					10,
					0
				},
				{
					9,
					9,
					9,
					2,
					2,
					4,
					4,
					0
				},
				{
					8,
					12,
					12,
					6,
					5,
					7,
					7,
					1
				},
				{
					3,
					0,
					17,
					17,
					6,
					11,
					11,
					8
				},
				{
					0,
					0,
					15,
					15,
					3,
					3,
					3,
					0
				},
				{
					6,
					16,
					16,
					0,
					0,
					0,
					5,
					5
				},
				{
					13,
					0,
					0,
					0,
					16,
					17,
					17,
					13
				},
				{
					15,
					0,
					0,
					0,
					14,
					14,
					17,
					17
				},
				{
					15,
					15,
					15,
					16,
					16,
					6,
					6,
					12
				}
			};
			sailKeyController.setUseIndexMap(useIndexMap);
			StrategyMapManager logicManager = StrategyTopTaskManager.GetLogicManager();
			List<int> list = new List<int>();
			for (int i = 1; i < logicManager.Area.Count; i++)
			{
				if (logicManager.Area[i].IsOpen())
				{
					list.Add(i);
				}
			}
			sailKeyController.setEnableIndex(list.ToArray());
			sailKeyController.Index = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
		}

		private void OnDestroy()
		{
			Prefab_AreaOpenAnimation = null;
		}
	}
}
