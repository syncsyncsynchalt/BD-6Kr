using Common.Enum;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class ISDefine
	{
		public static readonly float OBJECT_DEFAULT_MOVE_TIME = 1f;

		public static readonly Dictionary<FurnitureKinds, int> CATEGORY_AREA_BUTTON_INDEX = new Dictionary<FurnitureKinds, int>
		{
			{
				FurnitureKinds.Hangings,
				0
			},
			{
				FurnitureKinds.Desk,
				1
			},
			{
				FurnitureKinds.Window,
				2
			},
			{
				FurnitureKinds.Floor,
				3
			},
			{
				FurnitureKinds.Wall,
				4
			},
			{
				FurnitureKinds.Chest,
				5
			}
		};

		public static readonly Dictionary<FurnitureKinds, Vector3> FURNITURE_SIZE = new Dictionary<FurnitureKinds, Vector3>
		{
			{
				FurnitureKinds.Window,
				new Vector3(684f, 400f, 0f)
			},
			{
				FurnitureKinds.Chest,
				new Vector3(495f, 544f, 0f)
			},
			{
				FurnitureKinds.Desk,
				new Vector3(630f, 544f, 0f)
			},
			{
				FurnitureKinds.Floor,
				new Vector3(963f, 236f, 0f)
			},
			{
				FurnitureKinds.Hangings,
				new Vector3(258f, 394f, 0f)
			},
			{
				FurnitureKinds.Wall,
				new Vector3(960f, 438f, 0f)
			}
		};

		public static readonly Dictionary<FurnitureKinds, float> FURNITURE_THUM_MAGNIFICATION = new Dictionary<FurnitureKinds, float>
		{
			{
				FurnitureKinds.Window,
				0.6435f
			},
			{
				FurnitureKinds.Chest,
				0.7315f
			},
			{
				FurnitureKinds.Floor,
				1.45f
			},
			{
				FurnitureKinds.Wall,
				0.645f
			},
			{
				FurnitureKinds.Hangings,
				1f
			},
			{
				FurnitureKinds.Desk,
				0.567f
			}
		};
	}
}
