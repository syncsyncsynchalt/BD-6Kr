using System.Collections.Generic;
using System;
using System.Xml.Serialization;

namespace Server_Models
{
	[Serializable]
	[XmlRoot("Ship_GrowValues")]
	public class Ship_GrowValues
	{
		[XmlElement("Maxhp")]
		public int Maxhp;

		[XmlElement("Houg")]
		public int Houg;

		[XmlElement("Raig")]
		public int Raig;

		[XmlElement("Taiku")]
		public int Taiku;

		[XmlElement("Soukou")]
		public int Soukou;

		[XmlElement("Kaihi")]
		public int Kaihi;

		[XmlElement("Taisen")]
		public int Taisen;

		[XmlElement("Sakuteki")]
		public int Sakuteki;

		[XmlElement("Luck")]
		public int Luck;

		public Ship_GrowValues()
		{
		}

		public Ship_GrowValues(Mst_ship mst_data, int level, Dictionary<Mem_ship.enumKyoukaIdx, int> kyoukaValue)
		{
			changeLimitKyoukaValue(mst_data, kyoukaValue);
			Maxhp = mst_data.Taik + kyoukaValue[Mem_ship.enumKyoukaIdx.Taik] + kyoukaValue[Mem_ship.enumKyoukaIdx.Taik_Powerup];
			Houg = mst_data.Houg + kyoukaValue[Mem_ship.enumKyoukaIdx.Houg];
			Raig = mst_data.Raig + kyoukaValue[Mem_ship.enumKyoukaIdx.Raig];
			Taiku = mst_data.Tyku + kyoukaValue[Mem_ship.enumKyoukaIdx.Tyku];
			Soukou = mst_data.Souk + kyoukaValue[Mem_ship.enumKyoukaIdx.Souk];
			Kaihi = mst_data.Kaih + kyoukaValue[Mem_ship.enumKyoukaIdx.Kaihi];
			Taisen = mst_data.Tais + kyoukaValue[Mem_ship.enumKyoukaIdx.Taisen];
			Sakuteki = (int)((float)mst_data.Saku + (float)((mst_data.Saku_max - mst_data.Saku) * level) / 99f);
			if (Sakuteki > mst_data.Saku_max)
			{
				Sakuteki = mst_data.Saku_max;
			}
			Luck = mst_data.Luck + kyoukaValue[Mem_ship.enumKyoukaIdx.Luck];
		}

		public Ship_GrowValues Copy()
		{
			Ship_GrowValues ship_GrowValues = new Ship_GrowValues();
			ship_GrowValues.Maxhp = Maxhp;
			ship_GrowValues.Houg = Houg;
			ship_GrowValues.Raig = Raig;
			ship_GrowValues.Taiku = Taiku;
			ship_GrowValues.Soukou = Soukou;
			ship_GrowValues.Kaihi = Kaihi;
			ship_GrowValues.Taisen = Taisen;
			ship_GrowValues.Sakuteki = Sakuteki;
			ship_GrowValues.Luck = Luck;
			return ship_GrowValues;
		}

		private void changeLimitKyoukaValue(Mst_ship mst_data, Dictionary<Mem_ship.enumKyoukaIdx, int> kyoukaValue)
		{
			if (mst_data.Houg + kyoukaValue[Mem_ship.enumKyoukaIdx.Houg] > mst_data.Houg_max)
			{
				kyoukaValue[Mem_ship.enumKyoukaIdx.Houg] = mst_data.Houg_max - mst_data.Houg;
			}
			if (mst_data.Raig + kyoukaValue[Mem_ship.enumKyoukaIdx.Raig] > mst_data.Raig_max)
			{
				kyoukaValue[Mem_ship.enumKyoukaIdx.Raig] = mst_data.Raig_max - mst_data.Raig;
			}
			if (mst_data.Tyku + kyoukaValue[Mem_ship.enumKyoukaIdx.Tyku] > mst_data.Tyku_max)
			{
				kyoukaValue[Mem_ship.enumKyoukaIdx.Tyku] = mst_data.Tyku_max - mst_data.Tyku;
			}
			if (mst_data.Souk + kyoukaValue[Mem_ship.enumKyoukaIdx.Souk] > mst_data.Souk_max)
			{
				kyoukaValue[Mem_ship.enumKyoukaIdx.Souk] = mst_data.Souk_max - mst_data.Souk;
			}
			if (mst_data.Kaih + kyoukaValue[Mem_ship.enumKyoukaIdx.Kaihi] > mst_data.Kaih_max)
			{
				kyoukaValue[Mem_ship.enumKyoukaIdx.Kaihi] = mst_data.Kaih_max - mst_data.Kaih;
			}
			if (mst_data.Tais + kyoukaValue[Mem_ship.enumKyoukaIdx.Taisen] > mst_data.Tais_max)
			{
				kyoukaValue[Mem_ship.enumKyoukaIdx.Taisen] = mst_data.Tais_max - mst_data.Tais;
			}
			if (mst_data.Luck + kyoukaValue[Mem_ship.enumKyoukaIdx.Luck] > mst_data.Luck_max)
			{
				kyoukaValue[Mem_ship.enumKyoukaIdx.Luck] = mst_data.Luck_max - mst_data.Luck;
			}
		}
	}
}
