using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicWindowFurnitureCounterBar : UIDynamicWindowFurniture
	{
		public enum FoodOrDring
		{
			None,
			Beer,
			Sake,
			ItalyWine,
			Wine,
			Food,
			Wisky,
			Juice
		}

		public enum Customer
		{
			A,
			B,
			C,
			D,
			E,
			F,
			G,
			H,
			I,
			J,
			Others
		}

		public enum MenuShift
		{
			A,
			B,
			C,
			D,
			E
		}

		[SerializeField]
		private UITexture mTexture_FoodOrDrink;

		[SerializeField]
		private Texture mTexture2d_Beer;

		[SerializeField]
		private Texture mTexture2d_Sake;

		[SerializeField]
		private Texture mTexture2d_ItalyWine;

		[SerializeField]
		private Texture mTexture2d_Wine;

		[SerializeField]
		private Texture mTexture2d_Food;

		[SerializeField]
		private Texture mTexture2d_Wisky;

		[SerializeField]
		private Texture mTexture2d_Juice;

		private Vector3 mVector3_Beer = new Vector3(-476f, -210f);

		private Vector3 mVector3_Sake = new Vector3(-480f, -195f);

		private Vector3 mVector3_ItalyWine = new Vector3(-485f, -204f);

		private Vector3 mVector3_Wine = new Vector3(-464f, -205f);

		private Vector3 mVector3_Food = new Vector3(-475f, -210f);

		private Vector3 mVector3_Wisky = new Vector3(-470f, -204f);

		private Vector3 mVector3_Juice = new Vector3(-445f, -225f);

		protected override void OnUpdate()
		{
			base.OnUpdate();
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_FoodOrDrink);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Beer);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Sake);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_ItalyWine);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Wine);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Food);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Wisky);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Juice);
		}

		protected override void OnInitialize(UIFurnitureModel uiFurnitureModel)
		{
			base.OnInitialize(uiFurnitureModel);
			ShipModel flagShip = uiFurnitureModel.GetDeck().GetFlagShip();
			DateTime dateTime = uiFurnitureModel.GetDateTime();
			FoodOrDring foodOrDrink = RequestFoodOrDring(flagShip, dateTime);
			InitializeFoodOrDringTexture(foodOrDrink);
		}

		private void InitializeFoodOrDringTexture(FoodOrDring foodOrDrink)
		{
			switch (foodOrDrink)
			{
			case FoodOrDring.None:
				mTexture_FoodOrDrink.mainTexture = null;
				break;
			case FoodOrDring.Wisky:
				mTexture_FoodOrDrink.mainTexture = mTexture2d_Wisky;
				mTexture_FoodOrDrink.width = 170;
				mTexture_FoodOrDrink.height = 76;
				mTexture_FoodOrDrink.transform.localPosition = mVector3_Wisky;
				break;
			case FoodOrDring.Beer:
				mTexture_FoodOrDrink.mainTexture = mTexture2d_Beer;
				mTexture_FoodOrDrink.width = 220;
				mTexture_FoodOrDrink.height = 142;
				mTexture_FoodOrDrink.transform.localPosition = mVector3_Beer;
				break;
			case FoodOrDring.Sake:
				mTexture_FoodOrDrink.mainTexture = mTexture2d_Sake;
				mTexture_FoodOrDrink.width = 190;
				mTexture_FoodOrDrink.height = 105;
				mTexture_FoodOrDrink.transform.localPosition = mVector3_Sake;
				break;
			case FoodOrDring.ItalyWine:
				mTexture_FoodOrDrink.mainTexture = mTexture2d_ItalyWine;
				mTexture_FoodOrDrink.width = 286;
				mTexture_FoodOrDrink.height = 150;
				mTexture_FoodOrDrink.transform.localPosition = mVector3_ItalyWine;
				break;
			case FoodOrDring.Wine:
				mTexture_FoodOrDrink.mainTexture = mTexture2d_Wine;
				mTexture_FoodOrDrink.width = 208;
				mTexture_FoodOrDrink.height = 105;
				mTexture_FoodOrDrink.transform.localPosition = mVector3_Wine;
				break;
			case FoodOrDring.Food:
				mTexture_FoodOrDrink.mainTexture = mTexture2d_Food;
				mTexture_FoodOrDrink.width = 264;
				mTexture_FoodOrDrink.height = 124;
				mTexture_FoodOrDrink.transform.localPosition = mVector3_Food;
				break;
			case FoodOrDring.Juice:
				mTexture_FoodOrDrink.mainTexture = mTexture2d_Juice;
				mTexture_FoodOrDrink.width = 207;
				mTexture_FoodOrDrink.height = 68;
				mTexture_FoodOrDrink.transform.localPosition = mVector3_Juice;
				break;
			}
		}

		private FoodOrDring RequestFoodOrDring(ShipModel customerShip, DateTime dateTime)
		{
			Customer customer = RequestCustomerType(customerShip);
			MenuShift menuShift = RequestMenuShift(dateTime);
			switch (customer)
			{
			case Customer.A:
				return RequestFoodOrDringFromMenuForCustomerA(menuShift);
			case Customer.B:
				return RequestFoodOrDringFromMenuForCustomerB(menuShift);
			case Customer.C:
				return RequestFoodOrDringFromMenuForCustomerC(menuShift);
			case Customer.D:
				return RequestFoodOrDringFromMenuForCustomerD(menuShift);
			case Customer.E:
				return RequestFoodOrDringFromMenuForCustomerE(menuShift);
			case Customer.F:
				return RequestFoodOrDringFromMenuForCustomerF(menuShift);
			case Customer.G:
				return RequestFoodOrDringFromMenuForCustomerG(menuShift);
			case Customer.H:
				return RequestFoodOrDringFromMenuForCustomerH(menuShift);
			case Customer.I:
				return RequestFoodOrDringFromMenuForCustomerI(menuShift);
			case Customer.J:
				return RequestFoodOrDringFromMenuForCustomerJ(menuShift);
			case Customer.Others:
				return RequestFoodOrDringFromMenuForOtherCustomer(menuShift);
			default:
				return FoodOrDring.None;
			}
		}

		private FoodOrDring RequestFoodOrDringFromMenuForOtherCustomer(MenuShift menuShift)
		{
			switch (menuShift)
			{
			case MenuShift.A:
				return FoodOrDring.None;
			case MenuShift.B:
				return FoodOrDring.Beer;
			case MenuShift.C:
				return FoodOrDring.Beer;
			case MenuShift.D:
				return FoodOrDring.None;
			case MenuShift.E:
				return FoodOrDring.None;
			default:
				return FoodOrDring.None;
			}
		}

		private FoodOrDring RequestFoodOrDringFromMenuForCustomerJ(MenuShift menuShift)
		{
			switch (menuShift)
			{
			case MenuShift.A:
				return FoodOrDring.None;
			case MenuShift.B:
				return FoodOrDring.Food;
			case MenuShift.C:
				return FoodOrDring.None;
			case MenuShift.D:
				return FoodOrDring.Food;
			case MenuShift.E:
				return FoodOrDring.None;
			default:
				return FoodOrDring.None;
			}
		}

		private FoodOrDring RequestFoodOrDringFromMenuForCustomerI(MenuShift menuShift)
		{
			switch (menuShift)
			{
			case MenuShift.A:
				return FoodOrDring.ItalyWine;
			case MenuShift.B:
				return FoodOrDring.ItalyWine;
			case MenuShift.C:
				return FoodOrDring.ItalyWine;
			case MenuShift.D:
				return FoodOrDring.Wine;
			case MenuShift.E:
				return FoodOrDring.None;
			default:
				return FoodOrDring.None;
			}
		}

		private FoodOrDring RequestFoodOrDringFromMenuForCustomerH(MenuShift menuShift)
		{
			switch (menuShift)
			{
			case MenuShift.A:
				return FoodOrDring.Juice;
			case MenuShift.B:
				return FoodOrDring.Juice;
			case MenuShift.C:
				return FoodOrDring.None;
			case MenuShift.D:
				return FoodOrDring.None;
			case MenuShift.E:
				return FoodOrDring.None;
			default:
				return FoodOrDring.None;
			}
		}

		private FoodOrDring RequestFoodOrDringFromMenuForCustomerG(MenuShift menuShift)
		{
			switch (menuShift)
			{
			case MenuShift.A:
				return FoodOrDring.None;
			case MenuShift.B:
				return FoodOrDring.Beer;
			case MenuShift.C:
				return FoodOrDring.Sake;
			case MenuShift.D:
				return FoodOrDring.Wine;
			case MenuShift.E:
				return FoodOrDring.Wisky;
			default:
				return FoodOrDring.None;
			}
		}

		private FoodOrDring RequestFoodOrDringFromMenuForCustomerE(MenuShift menuShift)
		{
			switch (menuShift)
			{
			case MenuShift.A:
				return FoodOrDring.None;
			case MenuShift.B:
				return FoodOrDring.Beer;
			case MenuShift.C:
				return FoodOrDring.Wine;
			case MenuShift.D:
				return FoodOrDring.Wine;
			case MenuShift.E:
				return FoodOrDring.Wine;
			default:
				return FoodOrDring.None;
			}
		}

		private FoodOrDring RequestFoodOrDringFromMenuForCustomerF(MenuShift menuShift)
		{
			switch (menuShift)
			{
			case MenuShift.A:
				return FoodOrDring.None;
			case MenuShift.B:
				return FoodOrDring.Beer;
			case MenuShift.C:
				return FoodOrDring.Wine;
			case MenuShift.D:
				return FoodOrDring.Wine;
			case MenuShift.E:
				return FoodOrDring.None;
			default:
				return FoodOrDring.None;
			}
		}

		private FoodOrDring RequestFoodOrDringFromMenuForCustomerD(MenuShift menuShift)
		{
			switch (menuShift)
			{
			case MenuShift.A:
				return FoodOrDring.None;
			case MenuShift.B:
				return FoodOrDring.Beer;
			case MenuShift.C:
				return FoodOrDring.Wisky;
			case MenuShift.D:
				return FoodOrDring.Wisky;
			case MenuShift.E:
				return FoodOrDring.None;
			default:
				return FoodOrDring.None;
			}
		}

		private FoodOrDring RequestFoodOrDringFromMenuForCustomerC(MenuShift menuShift)
		{
			switch (menuShift)
			{
			case MenuShift.A:
				return FoodOrDring.None;
			case MenuShift.B:
				return FoodOrDring.Beer;
			case MenuShift.C:
				return FoodOrDring.Wisky;
			case MenuShift.D:
				return FoodOrDring.Wisky;
			case MenuShift.E:
				return FoodOrDring.Wisky;
			default:
				return FoodOrDring.None;
			}
		}

		private FoodOrDring RequestFoodOrDringFromMenuForCustomerB(MenuShift menuShift)
		{
			switch (menuShift)
			{
			case MenuShift.A:
				return FoodOrDring.None;
			case MenuShift.B:
				return FoodOrDring.Beer;
			case MenuShift.C:
				return FoodOrDring.Sake;
			case MenuShift.D:
				return FoodOrDring.Sake;
			case MenuShift.E:
				return FoodOrDring.None;
			default:
				return FoodOrDring.None;
			}
		}

		private FoodOrDring RequestFoodOrDringFromMenuForCustomerA(MenuShift menuShift)
		{
			switch (menuShift)
			{
			case MenuShift.A:
				return FoodOrDring.None;
			case MenuShift.B:
				return FoodOrDring.Beer;
			case MenuShift.C:
				return FoodOrDring.Sake;
			case MenuShift.D:
				return FoodOrDring.Sake;
			case MenuShift.E:
				return FoodOrDring.Sake;
			default:
				return FoodOrDring.None;
			}
		}

		public Customer RequestCustomerType(ShipModel customerShip)
		{
			if (customerShip.Yomi == "ひびき")
			{
				switch (customerShip.MstId)
				{
				case 35:
					return Customer.D;
				case 147:
				case 235:
					return Customer.H;
				}
			}
			switch (customerShip.Yomi)
			{
			case "ちとせ":
			case "きそ":
			case "やましろ":
			case "きりしま":
				return Customer.A;
			case "ほうしょう":
			case "ずいほう":
			case "やまと":
			case "かが":
			case "うんりゅう":
			case "ひりゅう":
			case "みょうこう":
			case "はつはる":
			case "あかし":
			case "しょうほう":
			case "あきつまる":
			case "きくづき":
			case "あまぎ":
			case "いせ":
			case "ひゅうが":
			case "じんつう":
			case "たいげい・りゅうほう":
				return Customer.B;
			case "なち":
			case "あしがら":
			case "むつ":
			case "はやしも":
				return Customer.C;
			case "こんごう":
			case "ちくま":
			case "きさらぎ":
			case "おおよど":
			case "たいほう":
			case "ゆうぐも":
			case "ながなみ":
			case "もがみ":
			case "いそかぜ":
			case "やはぎ":
				return Customer.D;
			case "ビスマルク":
			case "かとり":
			case "い8":
			case "むらさめ":
			case "たつた":
				return Customer.E;
			case "はるな":
			case "ひよう":
			case "くまの":
			case "すずや":
			case "そうりゅう":
			case "ゆうばり":
			case "むらくも":
			case "あまつかぜ":
			case "プリンツ・オイゲン":
			case "ゆ\u30fc511・ろ500":
				return Customer.F;
			case "じゅんよう":
			case "むさし":
			case "かこ":
			case "あさしも":
			case "い19":
				return Customer.G;
			case "むつき":
			case "やよい":
			case "うづき":
			case "ふみづき":
			case "あかつき":
			case "いかづち":
			case "いなづま":
			case "あけぼの":
			case "おぼろ":
			case "あさしお":
			case "おおしお":
			case "てんりゅう":
			case "しらつゆ":
			case "はるさめ":
			case "あさぐも":
			case "やまぐも":
			case "まいかぜ":
			case "きよしも":
			case "まきぐも":
			case "ながと":
			case "しまかぜ":
			case "ゆきかぜ":
			case "さかわ":
			case "たかなみ":
				return Customer.H;
			case "リットリオ・イタリア":
			case "ロ\u30fcマ":
				return Customer.I;
			case "あきづき":
			case "かすみ":
			case "しぐれ":
			case "あかぎ":
			case "あぶくま":
			case "あきつしま":
			case "かつらぎ":
				return Customer.J;
			default:
				return Customer.Others;
			}
		}

		public MenuShift RequestMenuShift(DateTime dateTime)
		{
			if (5 <= dateTime.Hour && dateTime.Hour < 19)
			{
				return MenuShift.A;
			}
			if (19 <= dateTime.Hour && dateTime.Hour < 21)
			{
				return MenuShift.B;
			}
			if (21 <= dateTime.Hour && dateTime.Hour < 22)
			{
				return MenuShift.C;
			}
			if (22 <= dateTime.Hour || dateTime.Hour < 1)
			{
				return MenuShift.D;
			}
			if (1 <= dateTime.Hour || dateTime.Hour < 5)
			{
				return MenuShift.E;
			}
			Debug.Log("ERROR:Shift Error Exception RecoverMe X<");
			return MenuShift.A;
		}
	}
}
