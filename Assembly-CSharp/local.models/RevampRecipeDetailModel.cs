using Server_Models;

namespace local.models
{
	public class RevampRecipeDetailModel : RevampRecipeModelBase
	{
		private Mst_slotitem_remodel_detail _mst_detail;

		private SlotitemModel _slotitem;

		public bool Determined;

		public SlotitemModel Slotitem => _slotitem;

		public override int DevKit => (!Determined) ? _mst_detail.Req_material5_1 : _mst_detail.Req_material5_2;

		public override int RevKit => (!Determined) ? _mst_detail.Req_material6_1 : _mst_detail.Req_material6_2;

		public int RequiredSlotitemId => _mst_detail.Req_slotitem_id;

		public int RequiredSlotitemCount => _mst_detail.Req_slotitems;

		public Mst_slotitem_remodel_detail __mst_detail__ => _mst_detail;

		public RevampRecipeDetailModel(Mst_slotitem_remodel mst, Mst_slotitem_remodel_detail mst_detail, SlotitemModel slotitem)
			: base(mst)
		{
			_mst_detail = mst_detail;
			_slotitem = slotitem;
		}

		public bool IsChange()
		{
			return _mst_detail.Change_flag == 1;
		}

		public override string ToString()
		{
			string str = string.Format("{0}(MstId:{1}) Lv:{2}{3}", Slotitem.Name, Slotitem.MstId, Slotitem.Level, (!Determined) ? "  \u3000\u3000\u3000\u3000\u3000 " : " [改修確定化]");
			str += $" 改修必要資材 燃/弾/鋼/ボ:{base.Fuel}/{base.Ammo}/{base.Steel}/{base.Baux}";
			str += $" 開発資材:{DevKit} 改修資材:{RevKit}";
			str += string.Format(" 変化{0}", (!IsChange()) ? "ナシ" : "アリ");
			if (RequiredSlotitemCount > 0)
			{
				Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(RequiredSlotitemId, out Mst_slotitem value);
				str += $" 要求スロットアイテム:{value.Name}(MstId:{value.Id}) × {RequiredSlotitemCount}";
			}
			return str;
		}
	}
}
