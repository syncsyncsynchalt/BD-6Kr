using Server_Models;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("BattleShipFmt")]
	public class BattleShipFmt
	{
		[XmlElement("Id")]
		public int Id;

		[XmlElement("ShipId")]
		public int ShipId;

		[XmlElement("Level")]
		public int Level;

		[XmlElement("NowHp")]
		public int NowHp;

		[XmlElement("MaxHp")]
		public int MaxHp;

		[XmlElement("EscapeFlag")]
		public bool EscapeFlag;

		[XmlElement("BattleParam")]
		public Ship_GrowValues BattleParam;

		[XmlElement("Slot")]
		public List<int> Slot;

		[XmlElement("ExSlot")]
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
				ship.Slot.ForEach(delegate (int x)
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
				ship.Slot.ForEach(delegate (int x)
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
