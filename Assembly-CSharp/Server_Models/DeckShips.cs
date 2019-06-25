using Server_Common;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Server_Models
{
	[DataContract(Namespace = "")]
	public class DeckShips
	{
		[DataMember]
		private List<int> ships;

		public int this[int index]
		{
			get
			{
				if (ships.Count() - 1 < index)
				{
					return -1;
				}
				return ships[index];
			}
			set
			{
				if (ships.Count() - 1 < index)
				{
					ships.Add(value);
				}
				else
				{
					ships[index] = value;
				}
				if (value == -1)
				{
					ships.RemoveAt(index);
				}
			}
		}

		public DeckShips()
		{
			ships = new List<int>(6);
		}

		public List<Mem_ship> getMemShip()
		{
			List<Mem_ship> ret = new List<Mem_ship>();
			ships.ForEach(delegate(int x)
			{
				Mem_ship value = null;
				if (Comm_UserDatas.Instance.User_ship.TryGetValue(x, out value))
				{
					ret.Add(value);
				}
			});
			return ret;
		}

		public int Count()
		{
			return ships.Count();
		}

		public void Clone(out List<int> out_ships)
		{
			out_ships = null;
			out_ships = ships.ToList();
		}

		public void Clone(out DeckShips out_ships)
		{
			out_ships = new DeckShips();
			out_ships.ships.AddRange(ships.AsEnumerable());
		}

		public int Find(int rid)
		{
			return ships.FindIndex((int x) => x == rid);
		}

		public void RemoveRange(int sta, int count)
		{
			ships.RemoveRange(sta, count);
		}

		public void Clear()
		{
			ships.Clear();
		}

		public bool Equals(DeckShips obj)
		{
			int num = Count();
			if (Count() != obj.Count())
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				if (this[i] != obj[i])
				{
					return false;
				}
			}
			return true;
		}

		public void RemoveShip(int target)
		{
			ships.Remove(target);
		}
	}
}
