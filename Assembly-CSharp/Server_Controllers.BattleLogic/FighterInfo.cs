using Server_Models;

namespace Server_Controllers.BattleLogic
{
	public class FighterInfo
	{
		public enum FighterKinds
		{
			FIGHTER = 1,
			RAIG,
			BAKU
		}

		public int SlotIdx;

		public Mst_slotitem SlotData;

		public FighterKinds Kind;

		public int AttackShipPow;

		public FighterInfo(int slotidx, Mst_slotitem mst_slot)
		{
			SlotIdx = slotidx;
			SlotData = mst_slot;
			setKindParam(mst_slot);
		}

		public static bool ValidFighter(Mst_slotitem mst_slot)
		{
			if (mst_slot.Api_mapbattle_type3 == 6 || mst_slot.Api_mapbattle_type3 == 7 || mst_slot.Api_mapbattle_type3 == 8 || mst_slot.Api_mapbattle_type3 == 11)
			{
				return true;
			}
			return false;
		}

		public static FighterKinds GetKind(Mst_slotitem mst_slot)
		{
			if (mst_slot.Api_mapbattle_type3 == 6)
			{
				return FighterKinds.FIGHTER;
			}
			if (mst_slot.Api_mapbattle_type3 == 7 || mst_slot.Api_mapbattle_type3 == 11)
			{
				return FighterKinds.BAKU;
			}
			if (mst_slot.Api_mapbattle_type3 == 8)
			{
				return FighterKinds.RAIG;
			}
			return (FighterKinds)0;
		}

		private void setKindParam(Mst_slotitem mst_slot)
		{
			FighterKinds kind = GetKind(mst_slot);
			switch (kind)
			{
			case FighterKinds.FIGHTER:
				Kind = kind;
				AttackShipPow = 0;
				break;
			case FighterKinds.BAKU:
				Kind = kind;
				AttackShipPow = mst_slot.Baku;
				break;
			case FighterKinds.RAIG:
				Kind = kind;
				AttackShipPow = mst_slot.Raig;
				break;
			}
		}

		public bool ValidTaiku()
		{
			if (Kind != FighterKinds.BAKU && Kind != FighterKinds.RAIG)
			{
				return false;
			}
			return true;
		}

		public bool ValidBakurai()
		{
			if (Kind != FighterKinds.BAKU && Kind != FighterKinds.RAIG)
			{
				return false;
			}
			return true;
		}
	}
}
