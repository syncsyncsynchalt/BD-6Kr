using Common.Enum;
using KCV.Scene.Port;
using local.models;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicChestFurnitureBathhouse : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_Clothes;

		[SerializeField]
		private Texture mTexture2d_BattleShipClothes;

		[SerializeField]
		private Texture mTexture2d_AircraftCarrierClothes;

		[SerializeField]
		private Texture mTexture2d_LightAircraftCarrierClothes;

		[SerializeField]
		private Texture mTexture2d_DestroyterClothes;

		[SerializeField]
		private Texture mTexture2d_SubmarineClothes;

		[SerializeField]
		private Texture mTexture2d_DefaultClothes;

		protected override void OnInitialize(UIFurnitureModel uiFurnitureModel)
		{
			ShipModel flagShip = uiFurnitureModel.GetDeck().GetFlagShip();
			SType shipType = (SType)flagShip.ShipType;
			InitializeClothes(shipType);
		}

		private void InitializeClothes(SType shipType)
		{
			switch (shipType)
			{
			case SType.BattleCruiser:
				mTexture_Clothes.mainTexture = mTexture2d_BattleShipClothes;
				break;
			case SType.AircraftCarrier:
				mTexture_Clothes.mainTexture = mTexture2d_AircraftCarrierClothes;
				break;
			case SType.LightAircraftCarrier:
				mTexture_Clothes.mainTexture = mTexture2d_LightAircraftCarrierClothes;
				break;
			case SType.Destroyter:
				mTexture_Clothes.mainTexture = mTexture2d_DestroyterClothes;
				break;
			case SType.Submarine:
			case SType.SubmarineTender:
				mTexture_Clothes.mainTexture = mTexture2d_SubmarineClothes;
				break;
			default:
				mTexture_Clothes.mainTexture = mTexture2d_DefaultClothes;
				break;
			}
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Clothes);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_BattleShipClothes);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_AircraftCarrierClothes);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_LightAircraftCarrierClothes);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_DestroyterClothes);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_SubmarineClothes);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_DefaultClothes);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Clothes);
		}
	}
}
