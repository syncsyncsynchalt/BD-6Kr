using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_ship : Model_Base
	{
		private int _id;

		private int _sortno;

		private int _bookno;

		private string _name;

		private string _yomi;

		private int _stype;

		private int _ctype;

		private int _cnum;

		private int _backs;

		private int _taik;

		private int _taik_max;

		private int _souk;

		private int _souk_max;

		private int _kaih;

		private int _kaih_max;

		private int _tous;

		private int _tous_max;

		private int _sokuh;

		private int _soku;

		private int _leng;

		private int _houg;

		private int _houg_max;

		private int _raig;

		private int _raig_max;

		private int _tyku;

		private int _tyku_max;

		private int _tais;

		private int _tais_max;

		private int _saku;

		private int _saku_max;

		private int _luck;

		private int _luck_max;

		private int _slot_num;

		private int _maxeq1;

		private int _maxeq2;

		private int _maxeq3;

		private int _maxeq4;

		private int _maxeq5;

		private int _defeq1;

		private int _defeq2;

		private int _defeq3;

		private int _defeq4;

		private int _defeq5;

		private int _afterlv;

		private int _afterfuel;

		private int _afterbull;

		private int _aftershipid;

		private int _buildtime;

		private int _broken1;

		private int _broken2;

		private int _broken3;

		private int _broken4;

		private int _powup1;

		private int _powup2;

		private int _powup3;

		private int _powup4;

		private int _use_fuel;

		private int _use_bull;

		private int _fuel_max;

		private int _bull_max;

		private int _raim;

		private int _raim_max;

		private int _append_ship_id;

		private int _event_limited;

		private int _btp;

		private List<int> _maxeq;

		private List<int> _defeq;

		private List<int> _broken;

		private List<int> _powup;

		private static string _tableName = "mst_ship";

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

		public int Bookno
		{
			get
			{
				return _bookno;
			}
			private set
			{
				_bookno = value;
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

		public string Yomi
		{
			get
			{
				return _yomi;
			}
			private set
			{
				_yomi = value;
			}
		}

		public int Stype
		{
			get
			{
				return _stype;
			}
			private set
			{
				_stype = value;
			}
		}

		public int Ctype
		{
			get
			{
				return _ctype;
			}
			private set
			{
				_ctype = value;
			}
		}

		public int Cnum
		{
			get
			{
				return _cnum;
			}
			private set
			{
				_cnum = value;
			}
		}

		public int Backs
		{
			get
			{
				return _backs;
			}
			private set
			{
				_backs = value;
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

		public int Taik_max
		{
			get
			{
				return _taik_max;
			}
			private set
			{
				_taik_max = value;
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

		public int Souk_max
		{
			get
			{
				return _souk_max;
			}
			private set
			{
				_souk_max = value;
			}
		}

		public int Kaih
		{
			get
			{
				return _kaih;
			}
			private set
			{
				_kaih = value;
			}
		}

		public int Kaih_max
		{
			get
			{
				return _kaih_max;
			}
			private set
			{
				_kaih_max = value;
			}
		}

		public int Tous
		{
			get
			{
				return _tous;
			}
			private set
			{
				_tous = value;
			}
		}

		public int Tous_max
		{
			get
			{
				return _tous_max;
			}
			private set
			{
				_tous_max = value;
			}
		}

		public int Sokuh
		{
			get
			{
				return _sokuh;
			}
			private set
			{
				_sokuh = value;
			}
		}

		public int Soku
		{
			get
			{
				return _soku;
			}
			private set
			{
				_soku = value;
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

		public int Houg_max
		{
			get
			{
				return _houg_max;
			}
			private set
			{
				_houg_max = value;
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

		public int Raig_max
		{
			get
			{
				return _raig_max;
			}
			private set
			{
				_raig_max = value;
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

		public int Tyku_max
		{
			get
			{
				return _tyku_max;
			}
			private set
			{
				_tyku_max = value;
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

		public int Tais_max
		{
			get
			{
				return _tais_max;
			}
			private set
			{
				_tais_max = value;
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

		public int Saku_max
		{
			get
			{
				return _saku_max;
			}
			private set
			{
				_saku_max = value;
			}
		}

		public int Luck
		{
			get
			{
				return _luck;
			}
			private set
			{
				_luck = value;
			}
		}

		public int Luck_max
		{
			get
			{
				return _luck_max;
			}
			private set
			{
				_luck_max = value;
			}
		}

		public int Slot_num
		{
			get
			{
				return _slot_num;
			}
			private set
			{
				_slot_num = value;
			}
		}

		public int Maxeq1
		{
			get
			{
				return _maxeq1;
			}
			private set
			{
				_maxeq1 = value;
			}
		}

		public int Maxeq2
		{
			get
			{
				return _maxeq2;
			}
			private set
			{
				_maxeq2 = value;
			}
		}

		public int Maxeq3
		{
			get
			{
				return _maxeq3;
			}
			private set
			{
				_maxeq3 = value;
			}
		}

		public int Maxeq4
		{
			get
			{
				return _maxeq4;
			}
			private set
			{
				_maxeq4 = value;
			}
		}

		public int Maxeq5
		{
			get
			{
				return _maxeq5;
			}
			private set
			{
				_maxeq5 = value;
			}
		}

		public int Defeq1
		{
			get
			{
				return _defeq1;
			}
			private set
			{
				_defeq1 = value;
			}
		}

		public int Defeq2
		{
			get
			{
				return _defeq2;
			}
			private set
			{
				_defeq2 = value;
			}
		}

		public int Defeq3
		{
			get
			{
				return _defeq3;
			}
			private set
			{
				_defeq3 = value;
			}
		}

		public int Defeq4
		{
			get
			{
				return _defeq4;
			}
			private set
			{
				_defeq4 = value;
			}
		}

		public int Defeq5
		{
			get
			{
				return _defeq5;
			}
			private set
			{
				_defeq5 = value;
			}
		}

		public int Afterlv
		{
			get
			{
				return _afterlv;
			}
			private set
			{
				_afterlv = value;
			}
		}

		public int Afterfuel
		{
			get
			{
				return _afterfuel;
			}
			private set
			{
				_afterfuel = value;
			}
		}

		public int Afterbull
		{
			get
			{
				return _afterbull;
			}
			private set
			{
				_afterbull = value;
			}
		}

		public int Aftershipid
		{
			get
			{
				return _aftershipid;
			}
			private set
			{
				_aftershipid = value;
			}
		}

		public int Buildtime
		{
			get
			{
				double num = Math.Ceiling((double)_buildtime / 15.0);
				return ((int)num > 1) ? ((int)num) : 2;
			}
			private set
			{
				_buildtime = value;
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

		public int Powup1
		{
			get
			{
				return _powup1;
			}
			private set
			{
				_powup1 = value;
			}
		}

		public int Powup2
		{
			get
			{
				return _powup2;
			}
			private set
			{
				_powup2 = value;
			}
		}

		public int Powup3
		{
			get
			{
				return _powup3;
			}
			private set
			{
				_powup3 = value;
			}
		}

		public int Powup4
		{
			get
			{
				return _powup4;
			}
			private set
			{
				_powup4 = value;
			}
		}

		public int Use_fuel
		{
			get
			{
				return _use_fuel;
			}
			private set
			{
				_use_fuel = value;
			}
		}

		public int Use_bull
		{
			get
			{
				return _use_bull;
			}
			private set
			{
				_use_bull = value;
			}
		}

		public int Fuel_max
		{
			get
			{
				return _fuel_max;
			}
			private set
			{
				_fuel_max = value;
			}
		}

		public int Bull_max
		{
			get
			{
				return _bull_max;
			}
			private set
			{
				_bull_max = value;
			}
		}

		public int Voicef => Mst_DataManager.Instance.Mst_ship_resources[Id].Voicef;

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

		public int Raim_max
		{
			get
			{
				return _raim_max;
			}
			private set
			{
				_raim_max = value;
			}
		}

		public int Append_ship_id
		{
			get
			{
				return _append_ship_id;
			}
			private set
			{
				_append_ship_id = value;
			}
		}

		public int Event_limited
		{
			get
			{
				return _event_limited;
			}
			private set
			{
				_event_limited = value;
			}
		}

		public List<int> Maxeq => _maxeq;

		public List<int> Defeq => _defeq;

		public List<int> Broken => _broken;

		public List<int> Powup => _powup;

		public static string tableName => _tableName;

		public List<int> GetEquipList()
		{
			Mst_equip_ship value = null;
			if (Mst_DataManager.Instance.Mst_equip_ship.TryGetValue(Id, out value))
			{
				return value.Equip.ToList();
			}
			return Mst_DataManager.Instance.Mst_stype[Stype].Equip.ToList();
		}

		public double GetLuckUpKeisu()
		{
			if (Id == 163)
			{
				return 1.2;
			}
			if (Id == 402)
			{
				return 1.6;
			}
			return 0.0;
		}

		public BtpType GetBtpType()
		{
			return (BtpType)_btp;
		}

		public int GetRemodelDevKitNum()
		{
			if (Afterfuel >= 0 && Afterfuel <= 4499)
			{
				return 0;
			}
			if (Afterfuel >= 4500 && Afterfuel <= 5499)
			{
				return 10;
			}
			if (Afterfuel >= 5500 && Afterfuel <= 6499)
			{
				return 15;
			}
			if (Afterfuel >= 6500 && Afterfuel <= 999999)
			{
				return 20;
			}
			return 0;
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			Id = int.Parse(element.Element("Id").Value);
			Sortno = int.Parse(element.Element("Sortno").Value);
			Bookno = int.Parse(element.Element("Bookno").Value);
			Name = element.Element("Name").Value;
			if (!Name.Equals("なし"))
			{
				Yomi = element.Element("Yomi").Value;
				Stype = int.Parse(element.Element("Stype").Value);
				Ctype = int.Parse(element.Element("Ctype").Value);
				Cnum = int.Parse(element.Element("Cnum").Value);
				Backs = int.Parse(element.Element("Backs").Value);
				string[] array = element.Element("Taik").Value.Split(c);
				Taik = int.Parse(array[0]);
				Taik_max = int.Parse(array[1]);
				string[] array2 = element.Element("Souk").Value.Split(c);
				Souk = int.Parse(array2[0]);
				Souk_max = int.Parse(array2[1]);
				string[] array3 = element.Element("Kaih").Value.Split(c);
				Kaih = int.Parse(array3[0]);
				Kaih_max = int.Parse(array3[1]);
				string[] array4 = element.Element("Tous").Value.Split(c);
				Tous = int.Parse(array4[0]);
				Tous_max = int.Parse(array4[1]);
				Sokuh = int.Parse(element.Element("Sokuh").Value);
				Soku = int.Parse(element.Element("Soku").Value);
				Leng = int.Parse(element.Element("Leng").Value);
				string[] array5 = element.Element("Houg").Value.Split(c);
				Houg = int.Parse(array5[0]);
				Houg_max = int.Parse(array5[1]);
				string[] array6 = element.Element("Raig").Value.Split(c);
				Raig = int.Parse(array6[0]);
				Raig_max = int.Parse(array6[1]);
				string[] array7 = element.Element("Tyku").Value.Split(c);
				Tyku = int.Parse(array7[0]);
				Tyku_max = int.Parse(array7[1]);
				string[] array8 = element.Element("Tais").Value.Split(c);
				Tais = int.Parse(array8[0]);
				Tais_max = int.Parse(array8[1]);
				string[] array9 = element.Element("Saku").Value.Split(c);
				Saku = int.Parse(array9[0]);
				Saku_max = int.Parse(array9[1]);
				string[] array10 = element.Element("Luck").Value.Split(c);
				Luck = int.Parse(array10[0]);
				Luck_max = int.Parse(array10[1]);
				Slot_num = int.Parse(element.Element("Slot_num").Value);
				string[] array11 = element.Element("Maxeq").Value.Split(c);
				Maxeq1 = int.Parse(array11[0]);
				Maxeq2 = int.Parse(array11[1]);
				Maxeq3 = int.Parse(array11[2]);
				Maxeq4 = int.Parse(array11[3]);
				Maxeq5 = int.Parse(array11[4]);
				string[] array12 = element.Element("Defeq").Value.Split(c);
				Defeq1 = int.Parse(array12[0]);
				Defeq2 = int.Parse(array12[1]);
				Defeq3 = int.Parse(array12[2]);
				Defeq4 = int.Parse(array12[3]);
				Defeq5 = int.Parse(array12[4]);
				if (element.Element("After") != null)
				{
					string[] array13 = element.Element("After").Value.Split(c);
					Afterlv = int.Parse(array13[0]);
					Afterfuel = int.Parse(array13[1]);
					Afterbull = int.Parse(array13[2]);
					Aftershipid = int.Parse(array13[3]);
				}
				Buildtime = int.Parse(element.Element("Buildtime").Value);
				if (element.Element("Broken") != null)
				{
					string[] array14 = element.Element("Broken").Value.Split(c);
					Broken1 = int.Parse(array14[0]);
					Broken2 = int.Parse(array14[1]);
					Broken3 = int.Parse(array14[2]);
					Broken4 = int.Parse(array14[3]);
				}
				if (element.Element("Powup") != null)
				{
					string[] array15 = element.Element("Powup").Value.Split(c);
					Powup1 = int.Parse(array15[0]);
					Powup2 = int.Parse(array15[1]);
					Powup3 = int.Parse(array15[2]);
					Powup4 = int.Parse(array15[3]);
				}
				Use_fuel = int.Parse(element.Element("Use_fuel").Value);
				Use_bull = int.Parse(element.Element("Use_bull").Value);
				Fuel_max = int.Parse(element.Element("Fuel_max").Value);
				Bull_max = int.Parse(element.Element("Bull_max").Value);
				string[] array16 = element.Element("Raim").Value.Split(c);
				Raim = int.Parse(array16[0]);
				Raim_max = int.Parse(array16[1]);
				Append_ship_id = int.Parse(element.Element("Append_ship_id").Value);
				Event_limited = int.Parse(element.Element("Event_limited").Value);
				_btp = int.Parse(element.Element("Btp").Value);
			}
		}

		protected override void setArrayItems()
		{
			_maxeq = new List<int>
			{
				Maxeq1,
				Maxeq2,
				Maxeq3,
				Maxeq4,
				Maxeq5
			};
			_defeq = new List<int>
			{
				Defeq1,
				Defeq2,
				Defeq3,
				Defeq4,
				Defeq5
			};
			_broken = new List<int>
			{
				Broken1,
				Broken2,
				Broken3,
				Broken4
			};
			_powup = new List<int>
			{
				Powup1,
				Powup2,
				Powup3,
				Powup4
			};
		}
	}
}
