using local.models;
using local.models.battle;
using local.utils;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Utils
{
	public class SlotItemUtils
	{
		private enum AircraftType
		{
			Type1 = -1,
			Type2,
			Type3,
			Type4,
			Type5,
			Type6
		}

		public static readonly List<int>[] ENEMY_AIRCRAFT_TYPE;

		public static List<AircraftOffsetInfo> AIRCRAFT_OFFSET_INFOS;

		private static Dictionary<AircraftType, List<int>> _dicAircraftType;

		static SlotItemUtils()
		{
			ENEMY_AIRCRAFT_TYPE = new List<int>[5]
			{
				new List<int>
				{
					517,
					520,
					524
				},
				new List<int>
				{
					518,
					521,
					525,
					526,
					544,
					546
				},
				new List<int>
				{
					547
				},
				new List<int>
				{
					548
				},
				new List<int>
				{
					549
				}
			};
			AIRCRAFT_OFFSET_INFOS = new List<AircraftOffsetInfo>
			{
				new AircraftOffsetInfo(19, flipHorizontal: false, 30f, new Vector3(0f, 13f, 0f)),
				new AircraftOffsetInfo(20, flipHorizontal: true, 33f, new Vector3(1f, 14f, 0f)),
				new AircraftOffsetInfo(21, flipHorizontal: false, 15f, Vector3.zero),
				new AircraftOffsetInfo(23, flipHorizontal: false, 31f, new Vector3(0f, 7f, 0f)),
				new AircraftOffsetInfo(24, flipHorizontal: true, 0f, Vector3.zero),
				new AircraftOffsetInfo(55, flipHorizontal: true, 0f, Vector3.zero),
				new AircraftOffsetInfo(57, flipHorizontal: true, 0f, Vector3.zero),
				new AircraftOffsetInfo(60, flipHorizontal: false, 340f, Vector3.zero),
				new AircraftOffsetInfo(60, flipHorizontal: false, 90f, Vector3.zero),
				new AircraftOffsetInfo(69, flipHorizontal: true, 0f, Vector3.zero),
				new AircraftOffsetInfo(79, flipHorizontal: false, 17f, Vector3.zero),
				new AircraftOffsetInfo(80, flipHorizontal: false, 316f, Vector3.zero),
				new AircraftOffsetInfo(81, flipHorizontal: false, 317f, Vector3.zero),
				new AircraftOffsetInfo(83, flipHorizontal: true, 0f, Vector3.zero),
				new AircraftOffsetInfo(96, flipHorizontal: true, 0f, Vector3.zero),
				new AircraftOffsetInfo(98, flipHorizontal: true, 0f, Vector3.zero),
				new AircraftOffsetInfo(99, flipHorizontal: true, 0f, Vector3.zero),
				new AircraftOffsetInfo(102, flipHorizontal: false, 24f, new Vector3(0f, 9f, 0f)),
				new AircraftOffsetInfo(109, flipHorizontal: true, 0f, Vector3.zero),
				new AircraftOffsetInfo(110, flipHorizontal: true, 21f, Vector3.zero),
				new AircraftOffsetInfo(111, flipHorizontal: true, 345f, Vector3.zero),
				new AircraftOffsetInfo(112, flipHorizontal: true, 0f, Vector3.zero),
				new AircraftOffsetInfo(113, flipHorizontal: true, 350f, Vector3.zero),
				new AircraftOffsetInfo(115, flipHorizontal: false, 9f, Vector3.zero)
			};
			_dicAircraftType = new Dictionary<AircraftType, List<int>>();
			_dicAircraftType.Add(AircraftType.Type2, ENEMY_AIRCRAFT_TYPE[0]);
			_dicAircraftType.Add(AircraftType.Type3, ENEMY_AIRCRAFT_TYPE[1]);
			_dicAircraftType.Add(AircraftType.Type4, ENEMY_AIRCRAFT_TYPE[2]);
			_dicAircraftType.Add(AircraftType.Type5, ENEMY_AIRCRAFT_TYPE[3]);
			_dicAircraftType.Add(AircraftType.Type6, ENEMY_AIRCRAFT_TYPE[4]);
		}

		public static string GetEnemyAircraftType(SlotitemModel_Battle model)
		{
			for (int i = 0; i < _dicAircraftType.Count; i++)
			{
				List<int> list = _dicAircraftType[(AircraftType)i];
				if (list.IndexOf(model.MstId) != -1)
				{
					return ((AircraftType)i).ToString();
				}
			}
			return AircraftType.Type1.ToString();
		}

		public static string GetEnemyAircraftType(int mstID)
		{
			for (int i = 0; i < _dicAircraftType.Count; i++)
			{
				List<int> list = _dicAircraftType[(AircraftType)i];
				if (list.IndexOf(mstID) != -1)
				{
					return ((AircraftType)i).ToString();
				}
			}
			return AircraftType.Type1.ToString();
		}

		public static AircraftOffsetInfo GetAircraftOffsetInfo(int mstID)
		{
			return AIRCRAFT_OFFSET_INFOS.Find((AircraftOffsetInfo order) => order.mstID == mstID);
		}

		public static AircraftOffsetInfo GetAircraftOffsetInfo(SlotitemModel_Battle model)
		{
			return GetAircraftOffsetInfo(model.MstId);
		}

		public static SlotitemModel_Battle GetDetectionScoutingPlane(List<List<SlotitemModel_Battle>> models)
		{
			foreach (List<SlotitemModel_Battle> model in models)
			{
				if (model != null)
				{
					foreach (SlotitemModel_Battle item in model)
					{
						if (item != null)
						{
							return item;
						}
					}
				}
			}
			return null;
		}

		public static Texture2D LoadTexture(SlotitemModel_Battle model)
		{
			if (model == null)
			{
				return null;
			}
			return SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(model.GetGraphicId(), 4);
		}

		public static Texture2D LoadTexture(PlaneModelBase model)
		{
			if (model == null)
			{
				return null;
			}
			return SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(local.utils.Utils.GetSlotitemGraphicId(model.MstId), 4);
		}

		public static UITexture LoadTexture(UITexture tex, SlotitemModel_Battle model)
		{
			tex.mainTexture = LoadTexture(model);
			tex.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE[4];
			return tex;
		}

		public static UITexture LoadTexture(ref UITexture tex, SlotitemModel_Battle model)
		{
			tex.mainTexture = LoadTexture(model);
			tex.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE[4];
			return tex;
		}

		public static Texture2D LoadUniDirTexture(SlotitemModel_Battle model)
		{
			return LoadTexture(model.GetGraphicId(), 6);
		}

		public static Texture2D LoadUniDirGlowTexture(SlotitemModel_Battle model)
		{
			return LoadTexture(model.GetGraphicId(), 7);
		}

		private static Texture2D LoadTexture(int nID, int nNum)
		{
			if (SingletonMonoBehaviour<ResourceManager>.Instance == null)
			{
				return null;
			}
			return SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(nID, nNum);
		}
	}
}
