using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public abstract class DeckModelBase
	{
		protected List<ShipModel> _ships;

		public virtual int AreaId => 0;

		public virtual string Name => string.Empty;

		public virtual int Id => 0;

		public string AreaName => "海域" + AreaId;

		public int Count => _ships.Count;

		public bool IsEscortDeckMyself()
		{
			return !(this is DeckModel);
		}

		public virtual bool IsActionEnd()
		{
			return false;
		}

		public bool HasShipMemId(int ship_mem_id)
		{
			return _ships.Find((ShipModel s) => s.MemId == ship_mem_id) != null;
		}

		public int GetShipIndex(int ship_mem_id)
		{
			return _ships.FindIndex((ShipModel s) => s.MemId == ship_mem_id);
		}

		public ShipModel GetFlagShip()
		{
			if (_ships.Count > 0)
			{
				return _ships[0];
			}
			return null;
		}

		public ShipModel GetShip(int index)
		{
			if (index < _ships.Count)
			{
				return _ships[index];
			}
			return null;
		}

		public ShipModel GetShipFromMemId(int mem_id)
		{
			return _ships.Find((ShipModel ship) => ship.MemId == mem_id);
		}

		public ShipModel[] GetShips()
		{
			return _ships.ToArray();
		}

		public ShipModel[] GetShips(int length)
		{
			ShipModel[] array = new ShipModel[length];
			for (int i = 0; i < array.Length; i++)
			{
				if (i < _ships.Count)
				{
					array[i] = _ships[i];
				}
				else
				{
					array[i] = null;
				}
			}
			return array;
		}

		public int GetShipCount()
		{
			return _ships.FindAll((ShipModel ship) => ship != null && ship.NowHp > 0 && !ship.IsEscaped()).Count;
		}

		public bool HasRepair()
		{
			return _ships.Find((ShipModel ship) => ship?.IsInRepair() ?? false) != null;
		}

		public bool HasBling()
		{
			return _ships.Find((ShipModel ship) => ship?.IsBling() ?? false) != null;
		}

		public List<int> __GetShipMemIds__()
		{
			return _ships.ConvertAll((ShipModel ship) => ship.MemId);
		}

		public void __CreateShipExpRatesDictionary__(ref Dictionary<int, int> dic)
		{
			for (int i = 0; i < _ships.Count; i++)
			{
				dic[_ships[i].MemId] = _ships[i].Exp_Percentage;
			}
		}

		protected void _Update(DeckShips deck_ships, Dictionary<int, ShipModel> ships)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < deck_ships.Count(); i++)
			{
				list.Add(deck_ships[i]);
			}
			_ships = new List<ShipModel>();
			for (int j = 0; j < list.Count; j++)
			{
				ShipModel item = ships[list[j]];
				_ships.Add(item);
			}
		}
	}
}
