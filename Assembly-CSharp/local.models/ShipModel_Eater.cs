using Common.Enum;
using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class ShipModel_Eater : ShipModel_Battle
	{
		private int _hp;

		private List<SlotitemModel_Battle> _slotitems_before;

		private SlotitemModel_Battle _slotitemex_before;

		private List<SlotitemModel_Battle> _slotitems_after_ration;

		private SlotitemModel_Battle _slotitemex_after_ration;

		private List<SlotitemModel_Battle> _slotitems_ration;

		public int Hp => _GetHp(_hp);

		public DamageState_Battle DmgState => _GetDmgState(_hp);

		public bool DamagedFlg => _GetDamagedFlg(DmgState);

		public List<SlotitemModel_Battle> SlotitemListBefore => _slotitems_before.GetRange(0, _slotitems_before.Count);

		public SlotitemModel_Battle SlotitemExBefore => _slotitemex_before;

		public List<SlotitemModel_Battle> SlotitemListAfterRation => _slotitems_after_ration;

		public SlotitemModel_Battle SlotitemExAfterRation => _slotitemex_after_ration;

		public List<SlotitemModel_Battle> SlotitemListRation => _slotitems_ration;

		public ShipModel_Eater(Mst_ship mst_data, __ShipModel_Battle_BaseData__ baseData, int hp, List<SlotitemModel_Battle> slotitems, SlotitemModel_Battle slotitemex)
		{
			_mst_data = mst_data;
			_base_data = baseData;
			_hp = hp;
			_slotitems_before = slotitems;
			_slotitemex_before = slotitemex;
			_init();
		}

		private void _init()
		{
			HashSet<int> hashSet = new HashSet<int>();
			_slotitems_after_ration = new List<SlotitemModel_Battle>();
			_slotitems_ration = new List<SlotitemModel_Battle>();
			int num = 43;
			SlotitemModel_Battle slotitemex_before = _slotitemex_before;
			if (slotitemex_before != null && slotitemex_before.Type3 == num && !hashSet.Contains(slotitemex_before.MstId))
			{
				_slotitems_ration.Add(slotitemex_before);
				hashSet.Add(slotitemex_before.MstId);
				_slotitemex_after_ration = null;
			}
			else
			{
				_slotitemex_after_ration = slotitemex_before;
			}
			for (int num2 = _slotitems_before.Count - 1; num2 >= 0; num2--)
			{
				slotitemex_before = _slotitems_before[num2];
				if (slotitemex_before != null && slotitemex_before.Type3 == num && !hashSet.Contains(slotitemex_before.MstId))
				{
					_slotitems_ration.Add(slotitemex_before);
					hashSet.Add(slotitemex_before.MstId);
				}
				else
				{
					_slotitems_after_ration.Add(slotitemex_before);
				}
			}
			_slotitems_after_ration.Reverse();
			while (_slotitems_after_ration.Count < SlotCount)
			{
				_slotitems_after_ration.Add(null);
			}
		}

		public override string ToString()
		{
			string str = $"{base.Name}(mstId:{base.MstId})[{Hp}/{MaxHp}({DmgState})";
			str += " ";
			str += _ToString(SlotitemListBefore, SlotitemExBefore);
			str += " -> ";
			str += _ToString(SlotitemListAfterRation, SlotitemExAfterRation);
			str += " ((";
			for (int i = 0; i < SlotitemListRation.Count; i++)
			{
				SlotitemModel_Battle item = SlotitemListRation[i];
				str += _ToString(item);
			}
			return str + "))";
		}
	}
}
