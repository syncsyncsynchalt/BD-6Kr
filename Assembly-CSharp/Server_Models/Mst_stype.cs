using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_stype : Model_Base
	{
		private int _id;

		private int _sortno;

		private string _name;

		private int _scnt;

		private int _kcnt;

		private List<int> _equip;

		private static string _tableName = "mst_stype";

		public int Id
		{
			get
			{
				return _id;
			}
			private set
			{
				_id = value;
			}
		}

		public int Sortno
		{
			get
			{
				return _sortno;
			}
			private set
			{
				_sortno = value;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			private set
			{
				_name = value;
			}
		}

		public int Scnt
		{
			get
			{
				return _scnt;
			}
			private set
			{
				_scnt = value;
			}
		}

		public int Kcnt
		{
			get
			{
				return _kcnt;
			}
			private set
			{
				_kcnt = value;
			}
		}

		public List<int> Equip
		{
			get
			{
				return _equip;
			}
			private set
			{
				_equip = value;
			}
		}

		public static string tableName => _tableName;

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			Sortno = int.Parse(element.Element("Sortno").Value);
			Name = element.Element("Name").Value;
			Scnt = int.Parse(element.Element("Scnt").Value);
			Kcnt = int.Parse(element.Element("Kcnt").Value);
		}

		public void SetEquip(List<int> equips)
		{
			if (Equip == null)
			{
				Equip = equips;
			}
		}

		public bool IsSubmarine()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(13);
			hashSet.Add(14);
			HashSet<int> hashSet2 = hashSet;
			return hashSet2.Contains(Id);
		}

		public bool IsMother()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> hashSet2 = hashSet;
			return hashSet2.Contains(Id);
		}

		public bool IsLandFacillity(int soku)
		{
			return (soku == 0) ? true : false;
		}

		public bool IsBattleShip()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(8);
			hashSet.Add(9);
			hashSet.Add(10);
			hashSet.Add(12);
			HashSet<int> hashSet2 = hashSet;
			return hashSet2.Contains(Id);
		}

		public bool IsTrainingShip()
		{
			return (Id == 21) ? true : false;
		}

		public bool IsKouku()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(11);
			hashSet.Add(18);
			hashSet.Add(7);
			hashSet.Add(10);
			hashSet.Add(6);
			hashSet.Add(16);
			hashSet.Add(17);
			hashSet.Add(22);
			hashSet.Add(14);
			HashSet<int> hashSet2 = hashSet;
			return hashSet2.Contains(Id);
		}
	}
}
