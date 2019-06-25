using Common.Enum;
using local.utils;
using Server_Models;
using System;

namespace local.models
{
	public class SlotitemModel_Mst
	{
		protected Mst_slotitem _mst;

		public virtual int MstId => _mst.Id;

		public string Name => _mst.Name;

		public int Type1 => _mst.Type1;

		public int Type2 => _mst.Type2;

		public int Type3 => _mst.Type3;

		public int Type4 => _mst.Type4;

		public int Soukou => _mst.Souk;

		public int Hougeki => _mst.Houg;

		public int Raigeki => _mst.Raig;

		public int Bakugeki => _mst.Baku;

		public int Taikuu => _mst.Tyku;

		public int Taisen => _mst.Tais;

		public int HouMeityu => _mst.Houm;

		public int Kaihi => _mst.Houk;

		public int Sakuteki => _mst.Saku;

		public int Syatei => _mst.Leng;

		public int Rare => _mst.Rare;

		public int BrokenFuel => _mst.Broken1;

		public int BrokenAmmo => _mst.Broken2;

		public int BrokenSteel => _mst.Broken3;

		public int BrokenBaux => _mst.Broken4;

		public virtual string ShortName => string.Format("{0}(MstId:{1}, Cost:{2}{3})", Name, MstId, GetCost(), (!IsPlane()) ? string.Empty : "[艦載機]");

		public SlotitemModel_Mst(int mst_id)
		{
			_mst = Mst_DataManager.Instance.Mst_Slotitem[mst_id];
		}

		public SlotitemModel_Mst(Mst_slotitem mst)
		{
			_mst = mst;
		}

		public int GetGraphicId()
		{
			return Utils.GetSlotitemGraphicId(MstId);
		}

		public int GetCost()
		{
			if (!Mst_DataManager.Instance.Mst_slotitem_cost.TryGetValue(MstId, out Mst_slotitem_cost value))
			{
				return 0;
			}
			return value.Cost;
		}

		public bool IsPlane()
		{
			SlotitemCategory slotitem_type = Mst_DataManager.Instance.Mst_equip_category[Type3].Slotitem_type;
			return slotitem_type == SlotitemCategory.Kanjouki || slotitem_type == SlotitemCategory.Suijouki;
		}

		[Obsolete("IsPlane()引数無しを使用してください", false)]
		public bool IsPlane(bool dummy)
		{
			return IsPlane();
		}

		public override string ToString()
		{
			return $"[{ShortName}]";
		}
	}
}
