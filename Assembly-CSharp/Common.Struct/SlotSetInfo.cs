using Server_Models;
using System.Collections.Generic;
using System.Linq;

namespace Common.Struct
{
	public struct SlotSetInfo
	{
		public int Soukou;

		public int Karyoku;

		public int Raisou;

		public int Taiku;

		public int Taisen;

		public int Houm;

		public int Kaihi;

		public int Sakuteki;

		public int Leng;

		public void SetSlot(int now_leng, Mst_slotitem set_item)
		{
			Karyoku = set_item.Houg;
			Raisou = set_item.Raig;
			Sakuteki = set_item.Saku;
			Soukou = set_item.Souk;
			Taiku = set_item.Tyku;
			Taisen = set_item.Tais;
			Houm = set_item.Houm;
			Kaihi = set_item.Houk;
		}

		public void UnsetSlot(Mst_slotitem unset_item)
		{
			Karyoku -= unset_item.Houg;
			Raisou -= unset_item.Raig;
			Sakuteki -= unset_item.Saku;
			Soukou -= unset_item.Souk;
			Taiku -= unset_item.Tyku;
			Taisen -= unset_item.Tais;
			Houm -= unset_item.Houm;
			Kaihi -= unset_item.Houk;
		}

		public void ChangeSlot(Mst_slotitem pre_item, Mst_slotitem after_item)
		{
			Karyoku = after_item.Houg - pre_item.Houg;
			Raisou = after_item.Raig - pre_item.Raig;
			Sakuteki = after_item.Saku - pre_item.Saku;
			Soukou = after_item.Souk - pre_item.Souk;
			Taiku = after_item.Tyku - pre_item.Tyku;
			Taisen = after_item.Tais - pre_item.Tais;
			Houm = after_item.Houm - pre_item.Houm;
			Kaihi = after_item.Houk - pre_item.Houk;
		}

		public void SetLeng(int nowLeng, List<int> lengList)
		{
			int num = lengList.Max();
			Leng = num - nowLeng;
		}
	}
}
