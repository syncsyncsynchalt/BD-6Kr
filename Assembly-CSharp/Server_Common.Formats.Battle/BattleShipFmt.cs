using Server_Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Server_Common.Formats.Battle
{
	[DataContract]
	public class BattleShipFmt
	{
		[DataMember]
		public int Id;

		[DataMember]
		public int ShipId;

		[DataMember]
		public int Level;

		[DataMember]
		public int NowHp;

		[DataMember]
		public int MaxHp;

		[DataMember]
		public bool EscapeFlag;

		[DataMember]
		public Ship_GrowValues BattleParam;

		[DataMember]
		public List<int> Slot;

		public int ExSlot;

		public BattleShipFmt()
		{
		}

		public BattleShipFmt(Mem_ship ship)
		{
			Id = ship.Rid;
			ShipId = ship.Ship_id;
			Level = ship.Level;
			NowHp = ship.Nowhp;
			MaxHp = ship.Maxhp;
			BattleParam = ship.GetBattleBaseParam().Copy();
			EscapeFlag = ship.Escape_sts;
			Slot = new List<int>();
			if (!ship.IsEnemy())
			{
				ship.Slot.ForEach(delegate(int x)
				{
					int item = -1;
					if (Comm_UserDatas.Instance.User_slot.ContainsKey(x))
					{
						item = Comm_UserDatas.Instance.User_slot[x].Slotitem_id;
					}
					Slot.Add(item);
				});
			}
			else
			{
				ship.Slot.ForEach(delegate(int x)
				{
					Slot.Add(x);
				});
			}
			Mst_slotitem mstSlotItemToExSlot = ship.GetMstSlotItemToExSlot();
			if (mstSlotItemToExSlot != null)
			{
				ExSlot = mstSlotItemToExSlot.Id;
			}
		}
	}
}
