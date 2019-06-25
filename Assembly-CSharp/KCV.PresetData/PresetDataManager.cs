using UnityEngine;

namespace KCV.PresetData
{
	public class PresetDataManager
	{
		private Entity_PresetData PresetData;

		private Entity_PresetDeck PresetDeck;

		private Entity_PresetShip PresetShip;

		public PresetDataManager()
		{
			PresetData = Resources.Load<Entity_PresetData>("Data/PresetData");
			PresetDeck = Resources.Load<Entity_PresetDeck>("Data/PresetDeck");
			PresetShip = Resources.Load<Entity_PresetShip>("Data/PresetShip");
		}

		public Entity_PresetData.Param GetPresetData(int PresetDataNo)
		{
			return PresetData.sheets[0].list[PresetDataNo];
		}

		public Entity_PresetDeck.Param GetPresetDeck(int PresetDeckNo)
		{
			return PresetDeck.sheets[0].list[PresetDeckNo - 1];
		}

		public int GetPresetShipMstID(string PresetShipName)
		{
			return PresetShip.sheets[0].list.Find((Entity_PresetShip.Param ship) => ship.ShipName == PresetShipName).MstID;
		}

		public Entity_PresetShip.Param GetPresetShipParam(string PresetShipName)
		{
			return PresetShip.sheets[0].list.Find((Entity_PresetShip.Param ship) => ship.ShipName == PresetShipName);
		}
	}
}
