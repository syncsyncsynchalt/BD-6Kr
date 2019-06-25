using Server_Models;

namespace local.models
{
	public class SlotitemModel : SlotitemModel_Mst, ISlotitemModel
	{
		private Mem_slotitem _data;

		public int MemId => _data.Rid;

		public int GetNo => _data.GetNo;

		public override int MstId => _data.Slotitem_id;

		public int Level => _data.Level;

		public int SkillLevel => _data.GetSkillLevel();

		public override string ShortName => string.Format("{0}(MstId:{1} MemId:{2} Lv:{3} c:{4}{5})", base.Name, MstId, MemId, Level, GetCost(), (!IsPlane()) ? string.Empty : "[艦載機]");

		public SlotitemModel(Mem_slotitem data)
			: base(data.Slotitem_id)
		{
			_data = data;
		}

		public bool IsLocked()
		{
			return _data.Lock;
		}

		public bool IsEauiped()
		{
			return _data.Equip_flag == Mem_slotitem.enumEquipSts.Equip;
		}

		public void __update__()
		{
			if (_data.Slotitem_id != _mst.Id)
			{
				_mst = Mst_DataManager.Instance.Mst_Slotitem[_data.Slotitem_id];
			}
		}

		public override string ToString()
		{
			return string.Format("[{0}{1}]", ShortName, (!IsLocked()) ? string.Empty : "(Lock)");
		}
	}
}
