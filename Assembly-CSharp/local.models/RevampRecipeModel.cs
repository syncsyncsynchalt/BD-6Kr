using Server_Models;

namespace local.models
{
	public class RevampRecipeModel : RevampRecipeModelBase
	{
		private SlotitemModel_Mst _slotitem;

		public SlotitemModel_Mst Slotitem => _slotitem;

		public RevampRecipeModel(Mst_slotitem_remodel mst)
			: base(mst)
		{
			_slotitem = new SlotitemModel_Mst(mst.Slotitem_id);
		}

		public override string ToString()
		{
			string str = $"{Slotitem.Name}(MstId:{Slotitem.MstId})";
			str += $" 改修必要資材 燃/弾/鋼/ボ:{base.Fuel}/{base.Ammo}/{base.Steel}/{base.Baux}";
			return str + $" 開発資材:{DevKit} 改修資材:{RevKit}";
		}
	}
}
