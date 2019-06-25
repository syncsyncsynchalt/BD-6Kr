using System;
using UnityEngine;

namespace KCV.Supply
{
	public class UISupplyAmmoIconManager : AbstractUISupplyIconManager
	{
		private const string iconObjName = "ammo";

		private const int ICON_OFFSET_X = 45;

		private const int MAX_ICON_OBJ_COUNT = 80;

		private const int ICON_OFFSET_Y = 205;

		private string[] textureNames = new string[5]
		{
			"icon_sizai2_gold",
			"icon_sizai2_red",
			"icon_sizai2_silver",
			"icon_sizai2_gold",
			"icon_sizai2_silver"
		};

		private Vector3 startPos = new Vector3(65f, 0f, 0f);

		protected override int getMaxIconObjCount()
		{
			return 80;
		}

		protected override Vector3 getStartPos()
		{
			return startPos;
		}

		protected override string getIconObjName()
		{
			return "ammo";
		}

		protected override int calculateIconCount()
		{
			return (int)(3.0 * Math.Ceiling(amount / 25.0));
		}

		protected override GameObject createIconObj(int currentIconObjCount, int i)
		{
			Transform transform = base.transform.FindChild("ammo" + currentIconObjCount);
			GameObject gameObject;
			if (transform == null)
			{
				gameObject = InstantiateIconObj();
				UISprite component = ((Component)gameObject.transform.FindChild("IconObject/Icon")).GetComponent<UISprite>();
				component.transform.localEulerAngles = new Vector3(0f, 0f, UnityEngine.Random.Range(0, 51) - 25);
				component.spriteName = textureNames[UnityEngine.Random.Range(0, textureNames.Length)];
				component.MakePixelPerfect();
			}
			else
			{
				gameObject = ResetSmoke(transform.gameObject);
			}
			int num = (int)Math.Floor((double)currentIconObjCount / 7.0);
			int num2 = currentIconObjCount - 7 * num;
			int num3 = ((i < 0 || i > 1) && (i < 6 || i > 7)) ? UnityEngine.Random.Range(0, 16) : UnityEngine.Random.Range(0, 6);
			Vector3 localPosition = new Vector3(45 - 15 * num2, 205 - num3 + 5 * num);
			gameObject.transform.localPosition = localPosition;
			int num4 = (int)Math.Floor((double)i / 7.0);
			SetIconAnimation(gameObject, i - 7 * num4);
			return gameObject;
		}
	}
}
