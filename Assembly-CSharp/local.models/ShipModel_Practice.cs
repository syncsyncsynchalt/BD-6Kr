using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class ShipModel_Practice : __ShipModelMem__
	{
		private List<ISlotitemModel> _slotitems;

		public ISlotitemModel[] Slotitems => _slotitems.ToArray();

		public ShipModel_Practice(Mem_ship mem_ship, List<Mst_slotitem> slotitems)
			: base(mem_ship)
		{
			_slotitems = new List<ISlotitemModel>();
			for (int i = 0; i < slotitems.Count; i++)
			{
				Mst_slotitem mst = slotitems[i];
				ISlotitemModel item = new SlotitemModel_Battle(mst);
				_slotitems.Add(item);
			}
			while (_slotitems.Count < SlotCount)
			{
				_slotitems.Add(null);
			}
		}

		public override string ToString()
		{
			string text = "Eq (";
			for (int i = 0; i < SlotCount; i++)
			{
				text = ((Slotitems[i] != null) ? (text + Slotitems[i]) : (text + "[--(MstId:- MemId:-)]"));
				text += ((i >= SlotCount - 1) ? string.Empty : ", ");
			}
			text += ")\n";
			return ToString(text);
		}
	}
}
