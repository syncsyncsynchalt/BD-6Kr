using Common.Enum;
using KCV.Furniture;
using KCV.Scene.Port;
using local.models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Others
{
	public class UserInterfacePortInteriorManager : MonoBehaviour
	{
		private class UIPortInteriorFactory
		{
			private static string FURNITURES_PREFAB_PATH = "Prefabs/Furnitures";

			private static string STATIC_FURNITURE = "UIStaticFurniture";

			public static readonly int[] DYNAMIC_FURNITURE_WINDOW = new int[4]
			{
				33,
				34,
				36,
				37
			};

			public static readonly int[] DYNAMIC_FURNITURE_WALL = new int[0];

			public static readonly int[] DYNAMIC_FURNITURE_FLOOR = new int[2]
			{
				6,
				23
			};

			public static readonly int[] DYNAMIC_FURNITURE_HANGINGS = new int[4]
			{
				29,
				35,
				42,
				45
			};

			public static readonly int[] DYNAMIC_FURNITURE_DESK = new int[5]
			{
				42,
				44,
				52,
				54,
				55
			};

			public static readonly int[] DYNAMIC_FURNITURE_CHEST = new int[4]
			{
				29,
				31,
				35,
				38
			};

			public static UIFurniture GenerateFurniturePrefab(FurnitureModel furnitureModel)
			{
				if (IsDynamicFurniture(furnitureModel))
				{
					switch (furnitureModel.Type)
					{
					case FurnitureKinds.Chest:
						return GenerateFurnitureChestPrefab(furnitureModel);
					case FurnitureKinds.Desk:
						return GenerateFurnitureDeskPrefab(furnitureModel);
					case FurnitureKinds.Floor:
						return GenerateFurnitureFloorPrefab(furnitureModel);
					case FurnitureKinds.Hangings:
						return GenerateFurnitureHangingsPrefab(furnitureModel);
					case FurnitureKinds.Wall:
						return GenerateFurnitureWallPrefab(furnitureModel);
					case FurnitureKinds.Window:
						return GenerateFurnitureWindowPrefab(furnitureModel);
					default:
						return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + STATIC_FURNITURE);
					}
				}
				return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + STATIC_FURNITURE);
			}

			private static UIFurniture GenerateFurnitureWindowPrefab(FurnitureModel windowFurnitureModel)
			{
				switch (windowFurnitureModel.NoInType)
				{
				case 32:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_211);
				case 33:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_215);
				case 35:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_230);
				case 36:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_244);
				default:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_WINDOW_FURNITURE);
				}
			}

			private static UIFurniture GenerateFurnitureWallPrefab(FurnitureModel wallFurnitureModel)
			{
				return null;
			}

			private static UIFurniture GenerateFurnitureFloorPrefab(FurnitureModel floorFurnitureModel)
			{
				switch (floorFurnitureModel.NoInType + 1)
				{
				case 6:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_6);
				case 23:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_23);
				default:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + STATIC_FURNITURE);
				}
			}

			private static UIFurniture GenerateFurnitureHangingsPrefab(FurnitureModel hangingsFurnitureModel)
			{
				switch (hangingsFurnitureModel.NoInType + 1)
				{
				case 35:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_206);
				case 42:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_235);
				case 45:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_247);
				default:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + STATIC_FURNITURE);
				}
			}

			private static UIFurniture GenerateFurnitureDeskPrefab(FurnitureModel deskFurnitureModel)
			{
				switch (deskFurnitureModel.NoInType + 1)
				{
				case 52:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_14);
				case 54:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_15);
				case 55:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_16);
				case 42:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_216);
				case 44:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_218);
				default:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + STATIC_FURNITURE);
				}
			}

			private static UIFurniture GenerateFurnitureChestPrefab(FurnitureModel chestFurnitureModel)
			{
				switch (chestFurnitureModel.NoInType + 1)
				{
				case 29:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_17);
				case 31:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_163);
				case 35:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_222);
				case 38:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + DynamicFurniture.DYNAMIC_FURNITURE_239);
				default:
					return Resources.Load<UIFurniture>(FURNITURES_PREFAB_PATH + "/" + STATIC_FURNITURE);
				}
			}

			private static bool IsDynamicFurniture(FurnitureModel furnitureModel)
			{
				int value = furnitureModel.NoInType + 1;
				switch (furnitureModel.Type)
				{
				case FurnitureKinds.Chest:
					return DYNAMIC_FURNITURE_CHEST.Contains(value);
				case FurnitureKinds.Desk:
					return DYNAMIC_FURNITURE_DESK.Contains(value);
				case FurnitureKinds.Floor:
					return DYNAMIC_FURNITURE_FLOOR.Contains(value);
				case FurnitureKinds.Hangings:
					return DYNAMIC_FURNITURE_HANGINGS.Contains(value);
				case FurnitureKinds.Wall:
					return DYNAMIC_FURNITURE_WALL.Contains(value);
				case FurnitureKinds.Window:
					return true;
				default:
					return false;
				}
			}
		}

		private class DynamicFurniture
		{
			public static string DYNAMIC_FURNITURE_206 = "UIDynamicHangingsFurnitureBigClock";

			public static string DYNAMIC_FURNITURE_230 = "UIDynamicWindowFurnitureCounterBar";

			public static string DYNAMIC_FURNITURE_239 = "UIDynamicChestFurnitureBathhouse";

			public static string DYNAMIC_FURNITURE_244 = "UIDynamicWindowFurnitureTeruteru";

			public static string DYNAMIC_FURNITURE_163 = "UIDynamicChestFurnitureDaruma";

			public static string DYNAMIC_FURNITURE_247 = "UIDynamicHangingsFurniture3MillionPeopleMemorial";

			public static string DYNAMIC_FURNITURE_6 = "UIDynamicFloorFurnitureSandBeach";

			public static string DYNAMIC_FURNITURE_216 = "UIDynamicDeskFurnitureInflatablePool";

			public static string DYNAMIC_FURNITURE_235 = "UIDynamicHangingsFurnitureMusashiMemorial";

			public static string DYNAMIC_FURNITURE_23 = "UIDynamicFloorFurnitureSnowField";

			public static string DYNAMIC_FURNITURE_211 = "UIDynamicWindowFurnitureFuurin";

			public static string DYNAMIC_FURNITURE_215 = "UIDynamicWindowFurnitureHanabi";

			public static string DYNAMIC_FURNITURE_218 = "UIDynamicDeskFurnitureJukeBox";

			public static string DYNAMIC_FURNITURE_222 = "UIDynamicChestFurnitureJukeBoxKai";

			public static string DYNAMIC_FURNITURE_14 = "UIDynamicDeskFurnitureColdWaterBath";

			public static string DYNAMIC_FURNITURE_16 = "UIDynamicDeskFurnitureShootingGallery";

			public static string DYNAMIC_FURNITURE_15 = "UIDynamicDeskFurnitureYakisoba";

			public static string DYNAMIC_FURNITURE_17 = "UIDynamicChestFurnitureKagatan";

			public static string DYNAMIC_WINDOW_FURNITURE = "UIDynamicWindowFurniture";
		}

		[SerializeField]
		private Transform mFloor;

		[SerializeField]
		private Transform mWall;

		[SerializeField]
		private Transform mWindow;

		[SerializeField]
		private Transform mChest;

		[SerializeField]
		private Transform mDesk;

		[SerializeField]
		private Transform mHangings;

		private DeckModel mDeckModel;

		private Action mOnRequestJukeBoxEvent;

		private Dictionary<FurnitureKinds, FurnitureModel> mFurnituresSet;

		private void Awake()
		{
			mFloor.localPosition = new Vector3(0f, -272f);
			mWall.localPosition = new Vector3(0f, 272f);
			mWindow.localPosition = new Vector3(480f, 272f);
			mChest.localPosition = new Vector3(480f, 0f);
			mDesk.localPosition = new Vector3(-480f, 0f);
			mHangings.localPosition = new Vector3(-480f, 272f);
		}

		private void OnDestroy()
		{
			ClearFurnitures();
			Mem.Del(ref mFloor);
			Mem.Del(ref mWall);
			Mem.Del(ref mWindow);
			Mem.Del(ref mChest);
			Mem.Del(ref mDesk);
			Mem.Del(ref mHangings);
			Mem.Del(ref mDeckModel);
			Mem.Del(ref mOnRequestJukeBoxEvent);
			Mem.DelDictionarySafe(ref mFurnituresSet);
		}

		public void InitializeFurnitures(DeckModel deckModel, Dictionary<FurnitureKinds, FurnitureModel> furnitureSet)
		{
			mDeckModel = deckModel;
			mFurnituresSet = furnitureSet;
			foreach (KeyValuePair<FurnitureKinds, FurnitureModel> item in furnitureSet)
			{
				UpdateFurniture(deckModel, item.Key, item.Value);
			}
		}

		public void SetOnRequestJukeBoxEvent(Action onRequestJukeBoxEvent)
		{
			mOnRequestJukeBoxEvent = onRequestJukeBoxEvent;
		}

		private void OnRequestJukeBoxEvent()
		{
			if (mOnRequestJukeBoxEvent != null)
			{
				mOnRequestJukeBoxEvent();
			}
		}

		public void InitializeFurnituresForConfirmation(DeckModel deckModel, Dictionary<FurnitureKinds, FurnitureModel> furnitureSet)
		{
			mDeckModel = deckModel;
			mFurnituresSet = furnitureSet;
			foreach (KeyValuePair<FurnitureKinds, FurnitureModel> item in furnitureSet)
			{
				UpdateFurniture(deckModel, item.Key, item.Value);
			}
		}

		public void UpdateFurnitures(DeckModel deckModel, Dictionary<FurnitureKinds, FurnitureModel> furnitureSet)
		{
			mDeckModel = deckModel;
			foreach (KeyValuePair<FurnitureKinds, FurnitureModel> item in furnitureSet)
			{
				if (IsNeedUpdateFurniture(item.Key, item.Value))
				{
					UpdateFurniture(deckModel, item.Key, item.Value);
					mFurnituresSet[item.Key] = item.Value;
				}
			}
		}

		public void UpdateFurniture(DeckModel deckModel, FurnitureKinds furnitureKind, FurnitureModel changeToFurniture)
		{
			UIFurniture uIFurniture = UIPortInteriorFactory.GenerateFurniturePrefab(changeToFurniture);
			UIFurniture.UIFurnitureModel uiFurnitureModel = new UIFurniture.UIFurnitureModel(changeToFurniture, deckModel);
			UIFurniture uIFurniture2 = null;
			switch (furnitureKind)
			{
			case FurnitureKinds.Window:
				ClearFurniture(mWindow);
				uIFurniture2 = NGUITools.AddChild(mWindow.gameObject, uIFurniture.gameObject).GetComponent<UIFurniture>();
				uIFurniture2.Initialize(uiFurnitureModel);
				break;
			case FurnitureKinds.Floor:
				ClearFurniture(mFloor);
				uIFurniture2 = NGUITools.AddChild(mFloor.gameObject, uIFurniture.gameObject).GetComponent<UIFurniture>();
				uIFurniture2.Initialize(uiFurnitureModel);
				break;
			case FurnitureKinds.Wall:
				ClearFurniture(mWall);
				uIFurniture2 = NGUITools.AddChild(mWall.gameObject, uIFurniture.gameObject).GetComponent<UIFurniture>();
				uIFurniture2.Initialize(uiFurnitureModel);
				break;
			case FurnitureKinds.Hangings:
				ClearFurniture(mHangings);
				uIFurniture2 = NGUITools.AddChild(mHangings.gameObject, uIFurniture.gameObject).GetComponent<UIFurniture>();
				uIFurniture2.Initialize(uiFurnitureModel);
				break;
			case FurnitureKinds.Chest:
				ClearFurniture(mChest);
				uIFurniture2 = NGUITools.AddChild(mChest.gameObject, uIFurniture.gameObject).GetComponent<UIFurniture>();
				uIFurniture2.Initialize(uiFurnitureModel);
				break;
			case FurnitureKinds.Desk:
				ClearFurniture(mDesk);
				uIFurniture2 = NGUITools.AddChild(mDesk.gameObject, uIFurniture.gameObject).GetComponent<UIFurniture>();
				uIFurniture2.Initialize(uiFurnitureModel);
				break;
			}
			if (uIFurniture2.GetComponent<UIDynamicFurniture>() != null)
			{
				uIFurniture2.GetComponent<UIDynamicFurniture>().SetOnActionEvent(OnFurnitureActionEvent);
			}
		}

		public void OnFurnitureActionEvent(UIFurniture uiFurniture)
		{
			if (IsConfigureJukeBox(uiFurniture))
			{
				OnRequestJukeBoxEvent();
			}
		}

		private bool IsConfigureJukeBox(UIFurniture furniture)
		{
			if (furniture == null)
			{
				return false;
			}
			bool flag = furniture is UIDynamicDeskFurnitureJukeBox;
			return flag | (furniture is UIDynamicChestFurnitureJukeBoxKai);
		}

		private bool IsConfigureJukeBox(FurnitureModel furniture)
		{
			if (furniture == null)
			{
				return false;
			}
			bool flag = furniture.MstId == 218;
			return flag | (furniture.MstId == 222);
		}

		private void ClearFurniture(Transform target)
		{
			if (!(target == null))
			{
				foreach (Transform item in target)
				{
					UITexture uiTexture = ((Component)item).GetComponent<UITexture>();
					if (uiTexture != null)
					{
						UserInterfacePortManager.ReleaseUtils.Release(ref uiTexture);
					}
					UISprite uiSprite = ((Component)item).GetComponent<UISprite>();
					if (uiSprite != null)
					{
						UserInterfacePortManager.ReleaseUtils.Release(ref uiSprite);
					}
					UnityEngine.Object.Destroy(item.gameObject);
				}
			}
		}

		private void ClearFurnitures()
		{
			ClearFurniture(mWindow);
			ClearFurniture(mFloor);
			ClearFurniture(mWall);
			ClearFurniture(mDesk);
			ClearFurniture(mChest);
			ClearFurniture(mHangings);
		}

		private bool IsNeedUpdateFurniture(FurnitureKinds furnitureKind, FurnitureModel furnitureModel)
		{
			if (mFurnituresSet.TryGetValue(furnitureKind, out FurnitureModel _))
			{
				return false;
			}
			return true;
		}

		public bool IsConfigureJukeBox()
		{
			foreach (FurnitureModel value in mFurnituresSet.Values)
			{
				if (IsConfigureJukeBox(value))
				{
					return true;
				}
			}
			return false;
		}
	}
}
