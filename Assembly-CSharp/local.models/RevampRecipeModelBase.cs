using Server_Models;

namespace local.models
{
	public abstract class RevampRecipeModelBase
	{
		protected Mst_slotitem_remodel _mst;

		public int RecipeId => _mst.Id;

		public int Fuel => _mst.Req_material1;

		public int Ammo => _mst.Req_material2;

		public int Steel => _mst.Req_material3;

		public int Baux => _mst.Req_material4;

		public virtual int DevKit => _mst.Req_material5;

		public virtual int RevKit => _mst.Req_material6;

		public Mst_slotitem_remodel __mst__ => _mst;

		public RevampRecipeModelBase(Mst_slotitem_remodel mst)
		{
			_mst = mst;
		}
	}
}
