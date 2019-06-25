using Common.Enum;
using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class __ShipModel_Battle__ : ShipModel_Battle
	{
		protected int _hp;

		protected List<SlotitemModel_Battle> _slotitems;

		protected SlotitemModel_Battle _slotitemex;

		public virtual int Hp => _GetHp(_hp);

		public DamageState_Battle DmgState => _GetDmgState(Hp);

		public bool DamagedFlg => _GetDamagedFlg(DmgState);

		public List<SlotitemModel_Battle> SlotitemList => _slotitems.GetRange(0, _slotitems.Count);

		public SlotitemModel_Battle SlotitemEx => _slotitemex;

		public __ShipModel_Battle__(Mst_ship mst_data, __ShipModel_Battle_BaseData__ baseData, int hp, List<SlotitemModel_Battle> slotitems, SlotitemModel_Battle slotitemex)
		{
			_mst_data = mst_data;
			_base_data = baseData;
			_hp = hp;
			_slotitems = slotitems;
			_slotitemex = slotitemex;
		}

		public override string ToString()
		{
			string str = $"{base.Name}(mstId:{base.MstId})[{Hp}/{MaxHp}]";
			return str + _ToString(SlotitemList, SlotitemEx);
		}
	}
}
