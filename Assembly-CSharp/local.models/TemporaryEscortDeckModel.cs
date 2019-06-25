using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class TemporaryEscortDeckModel : __EscortDeckModel__
	{
		private int _id;

		private DeckShips _deckships;

		private string _name;

		public override int Id => _id;

		public override int AreaId => _id;

		public override string Name => (!(_name != string.Empty)) ? base.Name : _name;

		public override int Turn => 0;

		public DeckShips DeckShips => _deckships;

		public override string __Name__ => _name;

		public TemporaryEscortDeckModel(int id, DeckShips deckships, string name, Dictionary<int, ShipModel> ships)
			: base(null, null)
		{
			_id = id;
			_deckships = deckships;
			_name = name;
			if (_deckships != null)
			{
				_Update(deckships, ships);
			}
		}

		public void ChangeName(string new_name)
		{
			_name = new_name;
		}

		public void __Update__(Dictionary<int, ShipModel> ships)
		{
			_Update(_deckships, ships);
		}
	}
}
