using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class __ShipModel_Attacker__ : ShipModel_Attacker
	{
		private ShipModel _baseModel;

		public override int TmpId => _baseModel.MemId;

		public override int Level => _baseModel.Level;

		public override int MaxHp => _baseModel.MaxHp;

		public override int ParamKaryoku => 0;

		public override int ParamRaisou => 0;

		public override int ParamTaiku => 0;

		public override int ParamSoukou => 0;

		public override int Hp => _baseModel.NowHp;

		public __ShipModel_Attacker__(ShipModel baseModel, int index)
			: base(null, null, 0, null, null)
		{
			_mst_data = Mst_DataManager.Instance.Mst_ship[baseModel.MstId];
			_baseModel = baseModel;
			_base_data = new __ShipModel_Battle_BaseData__();
			_base_data.Index = base.Index;
			_base_data.IsFriend = true;
			_base_data.IsPractice = false;
			_slotitems = new List<SlotitemModel_Battle>();
			List<SlotitemModel> slotitemList = baseModel.SlotitemList;
			for (int i = 0; i < slotitemList.Count; i++)
			{
				SlotitemModel slotitemModel = slotitemList[i];
				if (slotitemModel == null)
				{
					_slotitems.Add(null);
				}
				else
				{
					_slotitems.Add(new SlotitemModel_Battle(slotitemList[i].MstId));
				}
			}
			if (_baseModel.HasExSlot() && _baseModel.SlotitemEx != null)
			{
				int mstId = _baseModel.SlotitemEx.MstId;
				_slotitemex = new SlotitemModel_Battle(mstId);
			}
		}

		public override bool IsEscape()
		{
			return false;
		}

		public override string ToString()
		{
			string str = $"{base.Name}(mstId:{base.MstId})[{Hp}/{MaxHp}]";
			return str + _ToString(base.SlotitemList, base.SlotitemEx);
		}
	}
}
