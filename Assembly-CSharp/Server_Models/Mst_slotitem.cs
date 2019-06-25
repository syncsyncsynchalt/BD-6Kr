using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_slotitem : Model_Base
	{
		private int _id;

		private int _sortno;

		private string _name;

		private int _type1;

		private int _type2;

		private int _type3;

		private int _api_mapbattle_type3;

		private int _type4;

		private int _taik;

		private int _souk;

		private int _houg;

		private int _raig;

		private int _baku;

		private int _tyku;

		private int _tais;

		private int _houm;

		private int _raim;

		private int _houk;

		private int _saku;

		private int _leng;

		private int _default_exp;

		private int _exp_rate;

		private int _rare;

		private int _broken1;

		private int _broken2;

		private int _broken3;

		private int _broken4;

		private int _flag_houg;

		private int _flag_raig;

		private int _flag_kraig;

		private int _flag_kbaku;

		private int _flag_tyku;

		private int _flag_ktyku;

		private int _flag_saku;

		private int _flag_sakb;

		private static string _tableName = "mst_slotitem";

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

		public int Type1
		{
			get
			{
				return _type1;
			}
			private set
			{
				_type1 = value;
			}
		}

		public int Type2
		{
			get
			{
				return _type2;
			}
			private set
			{
				_type2 = value;
			}
		}

		public int Type3
		{
			get
			{
				return _type3;
			}
			private set
			{
				_type3 = value;
			}
		}

		public int Api_mapbattle_type3
		{
			get
			{
				return _api_mapbattle_type3;
			}
			private set
			{
				_api_mapbattle_type3 = value;
			}
		}

		public int Type4
		{
			get
			{
				return _type4;
			}
			private set
			{
				_type4 = value;
			}
		}

		public int Taik
		{
			get
			{
				return _taik;
			}
			private set
			{
				_taik = value;
			}
		}

		public int Souk
		{
			get
			{
				return _souk;
			}
			private set
			{
				_souk = value;
			}
		}

		public int Houg
		{
			get
			{
				return _houg;
			}
			private set
			{
				_houg = value;
			}
		}

		public int Raig
		{
			get
			{
				return _raig;
			}
			private set
			{
				_raig = value;
			}
		}

		public int Baku
		{
			get
			{
				return _baku;
			}
			private set
			{
				_baku = value;
			}
		}

		public int Tyku
		{
			get
			{
				return _tyku;
			}
			private set
			{
				_tyku = value;
			}
		}

		public int Tais
		{
			get
			{
				return _tais;
			}
			private set
			{
				_tais = value;
			}
		}

		public int Houm
		{
			get
			{
				return _houm;
			}
			private set
			{
				_houm = value;
			}
		}

		public int Raim
		{
			get
			{
				return _raim;
			}
			private set
			{
				_raim = value;
			}
		}

		public int Houk
		{
			get
			{
				return _houk;
			}
			private set
			{
				_houk = value;
			}
		}

		public int Saku
		{
			get
			{
				return _saku;
			}
			private set
			{
				_saku = value;
			}
		}

		public int Leng
		{
			get
			{
				return _leng;
			}
			private set
			{
				_leng = value;
			}
		}

		public int Default_exp
		{
			get
			{
				return _default_exp;
			}
			private set
			{
				_default_exp = value;
			}
		}

		public int Exp_rate
		{
			get
			{
				return _exp_rate;
			}
			private set
			{
				_exp_rate = value;
			}
		}

		public int Rare
		{
			get
			{
				return _rare;
			}
			private set
			{
				_rare = value;
			}
		}

		public int Broken1
		{
			get
			{
				return _broken1;
			}
			private set
			{
				_broken1 = value;
			}
		}

		public int Broken2
		{
			get
			{
				return _broken2;
			}
			private set
			{
				_broken2 = value;
			}
		}

		public int Broken3
		{
			get
			{
				return _broken3;
			}
			private set
			{
				_broken3 = value;
			}
		}

		public int Broken4
		{
			get
			{
				return _broken4;
			}
			private set
			{
				_broken4 = value;
			}
		}

		public int Flag_houg
		{
			get
			{
				return _flag_houg;
			}
			private set
			{
				_flag_houg = value;
			}
		}

		public int Flag_raig
		{
			get
			{
				return _flag_raig;
			}
			private set
			{
				_flag_raig = value;
			}
		}

		public int Flag_kraig
		{
			get
			{
				return _flag_kraig;
			}
			private set
			{
				_flag_kraig = value;
			}
		}

		public int Flag_kbaku
		{
			get
			{
				return _flag_kbaku;
			}
			private set
			{
				_flag_kbaku = value;
			}
		}

		public int Flag_tyku
		{
			get
			{
				return _flag_tyku;
			}
			private set
			{
				_flag_tyku = value;
			}
		}

		public int Flag_ktyku
		{
			get
			{
				return _flag_ktyku;
			}
			private set
			{
				_flag_ktyku = value;
			}
		}

		public int Flag_saku
		{
			get
			{
				return _flag_saku;
			}
			private set
			{
				_flag_saku = value;
			}
		}

		public int Flag_sakb
		{
			get
			{
				return _flag_sakb;
			}
			private set
			{
				_flag_sakb = value;
			}
		}

		public static string tableName => _tableName;

		public bool IsDentan()
		{
			if (Type3 == 12 || Type3 == 13 || Type3 == 93)
			{
				return true;
			}
			return false;
		}

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			Sortno = int.Parse(element.Element("Sortno").Value);
			Name = element.Element("Name").Value;
			string[] array = element.Element("Types").Value.Split(',');
			Type1 = int.Parse(array[0]);
			Type2 = int.Parse(array[1]);
			int num = int.Parse(array[2]);
			Type3 = getRewirteType3No(Id, num);
			Api_mapbattle_type3 = num;
			Type4 = int.Parse(array[3]);
			Taik = int.Parse(element.Element("Taik").Value);
			Souk = int.Parse(element.Element("Souk").Value);
			Houg = int.Parse(element.Element("Houg").Value);
			Raig = int.Parse(element.Element("Raig").Value);
			Baku = int.Parse(element.Element("Baku").Value);
			Tyku = int.Parse(element.Element("Tyku").Value);
			Tais = int.Parse(element.Element("Tais").Value);
			Houm = int.Parse(element.Element("Houm").Value);
			Raim = int.Parse(element.Element("Raim").Value);
			Houk = int.Parse(element.Element("Houk").Value);
			Saku = int.Parse(element.Element("Saku").Value);
			Leng = int.Parse(element.Element("Leng").Value);
			Default_exp = int.Parse(element.Element("Default_exp").Value);
			Exp_rate = int.Parse(element.Element("Exp_rate").Value);
			Rare = int.Parse(element.Element("Rare").Value);
			if (element.Element("Brokens") != null)
			{
				string[] array2 = element.Element("Brokens").Value.Split(',');
				Broken1 = int.Parse(array2[0]);
				Broken2 = int.Parse(array2[1]);
				Broken3 = int.Parse(array2[2]);
				Broken4 = int.Parse(array2[3]);
			}
		}

		private int getRewirteType3No(int id, int nowType3No)
		{
			switch (id)
			{
			case 128:
				return 38;
			case 142:
				return 93;
			case 151:
				return 94;
			default:
				return nowType3No;
			}
		}
	}
}
