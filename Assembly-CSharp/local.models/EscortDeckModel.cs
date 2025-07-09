using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class EscortDeckModel : DeckModelBase
	{
		protected Mem_esccort_deck _mem_escort_deck;

		public override int Id => _mem_escort_deck.Rid;

		public override int AreaId => _mem_escort_deck.Maparea_id;

		public override string Name
		{
			get
			{
				string text = (_mem_escort_deck != null) ? _mem_escort_deck.Name : string.Empty;
				if (text == string.Empty)
				{
					string name = Mst_DataManager.Instance.Mst_maparea[Id].Name;
					text = name.Replace("海域", string.Empty) + "航路護衛隊";
				}
				return text;
			}
		}

		public virtual int Turn => _mem_escort_deck.GetBlingTurn();

		public virtual string __Name__ => _mem_escort_deck.Name;

		public EscortDeckModel(Mem_esccort_deck mem_escort_deck, Dictionary<int, ShipModel> ships)
		{
			__Update__(mem_escort_deck, ships);
		}

		public bool IsMove()
		{
			return Turn > 0;
		}

		public void __Update__(Mem_esccort_deck mem_escort_deck, Dictionary<int, ShipModel> ships)
		{
			_mem_escort_deck = mem_escort_deck;
			if (_mem_escort_deck != null)
			{
				_Update(_mem_escort_deck.Ship, ships);
			}
		}

		public override string ToString()
		{
			string empty = string.Empty;
			ShipModel[] ships = GetShips();
			if (ships.Length == 0)
			{
				return empty + $"{Name}({Id}) 未配備";
			}
			empty += $"[{Name}({Id})][";
			for (int i = 0; i < ships.Length; i++)
			{
				empty += $"{ships[i].Name}({ships[i].MstId},{ships[i].MemId})";
				if (i + 1 < ships.Length)
				{
					empty += $", ";
				}
			}
			if (Turn > 0)
			{
				empty += $"  移動中:残り{Turn}ターン";
			}
			return empty + $"]";
		}
	}
}
