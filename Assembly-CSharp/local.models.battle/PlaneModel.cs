namespace local.models.battle
{
	public class PlaneModel : PlaneModelBase
	{
		private ShipModel_Attacker _parent;

		public ShipModel_Attacker Parent => _parent;

		public PlaneModel(ShipModel_Attacker parent, int slotitem_mst_id)
			: base(slotitem_mst_id)
		{
			_parent = parent;
		}
	}
}
