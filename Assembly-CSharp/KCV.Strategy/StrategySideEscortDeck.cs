using Common.Enum;
using local.models;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategySideEscortDeck : MonoBehaviour
	{
		private const string SENKAN = "icon_ship1";

		private const string ZYUNYOU = "icon_ship2";

		private const string KUBO = "icon_ship3";

		private const string KUTIKU = "icon_ship4";

		private const string SENSUI = "icon_ship5";

		private const string SONOTA = "icon_ship6";

		private const string SUIBO = "icon_ship7";

		private const string YOURIKU = "icon_ship8";

		[SerializeField]
		private UIWidget EscortDeckParent;

		[SerializeField]
		private CommonShipBanner Banner;

		[SerializeField]
		private Transform BannerShutter;

		[SerializeField]
		private UISprite[] shipTypeIcons;

		private string[] ShipTypeIconName;

		private static readonly Color32 TaihaColor = new Color32(byte.MaxValue, 90, 90, byte.MaxValue);

		private static readonly Color32 TyuuhaColor = new Color32(byte.MaxValue, 178, 108, byte.MaxValue);

		private static readonly Color32 ShouhaColor = new Color32(243, byte.MaxValue, 165, byte.MaxValue);

		private void Awake()
		{
			CreateShipTypeIconNameArray();
			Banner.isUseKira = false;
			Banner.isUseSmoke = false;
		}

		public void UpdateEscortDeck(EscortDeckModel deck)
		{
			bool flag = Banner.ShipModel == null || Banner.ShipModel != deck.GetFlagShip();
			Banner.SetShipData(deck.GetFlagShip());
			for (int i = 0; i < shipTypeIcons.Length; i++)
			{
				ShipModel ship = deck.GetShip(i + 1);
				if (ship != null)
				{
					shipTypeIcons[i].spriteName = ShipTypeIconName[ship.ShipType];
					ChangeColor(ship, shipTypeIcons[i]);
				}
				else
				{
					shipTypeIcons[i].spriteName = string.Empty;
				}
			}
			if (flag)
			{
				updateView(0.2f);
			}
		}

		public void updateView(float time)
		{
			if (Banner.ShipModel == null)
			{
				Banner.SetActive(isActive: false);
				BannerShutter.SetActive(isActive: true);
				return;
			}
			Banner.SetActive(isActive: true);
			BannerShutter.SetActive(isActive: false);
			EscortDeckParent.alpha = 0f;
			TweenAlpha.Begin(EscortDeckParent.gameObject, time, 1f);
		}

		private void ChangeColor(ShipModel ship, UISprite icon)
		{
			switch (ship.DamageStatus)
			{
			case DamageState.Normal:
				icon.color = Color.white;
				break;
			case DamageState.Shouha:
				icon.color = ShouhaColor;
				break;
			case DamageState.Tyuuha:
				icon.color = TyuuhaColor;
				break;
			case DamageState.Taiha:
				icon.color = TaihaColor;
				break;
			}
		}

		private void CreateShipTypeIconNameArray()
		{
			ShipTypeIconName = new string[23];
			ShipTypeIconName[0] = string.Empty;
			ShipTypeIconName[1] = "icon_ship4";
			ShipTypeIconName[2] = "icon_ship4";
			ShipTypeIconName[3] = "icon_ship2";
			ShipTypeIconName[4] = "icon_ship2";
			ShipTypeIconName[5] = "icon_ship2";
			ShipTypeIconName[6] = "icon_ship2";
			ShipTypeIconName[7] = "icon_ship3";
			ShipTypeIconName[8] = "icon_ship1";
			ShipTypeIconName[9] = "icon_ship1";
			ShipTypeIconName[10] = "icon_ship1";
			ShipTypeIconName[11] = "icon_ship3";
			ShipTypeIconName[12] = "icon_ship1";
			ShipTypeIconName[13] = "icon_ship5";
			ShipTypeIconName[14] = "icon_ship5";
			ShipTypeIconName[15] = "icon_ship6";
			ShipTypeIconName[16] = "icon_ship7";
			ShipTypeIconName[17] = "icon_ship8";
			ShipTypeIconName[18] = "icon_ship3";
			ShipTypeIconName[19] = "icon_ship6";
			ShipTypeIconName[20] = "icon_ship5";
			ShipTypeIconName[21] = "icon_ship2";
			ShipTypeIconName[22] = "icon_ship6";
		}
	}
}
