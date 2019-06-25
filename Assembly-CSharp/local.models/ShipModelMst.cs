using Common.Enum;
using local.utils;
using Server_Models;
using System.Collections.Generic;
using System.Linq;

namespace local.models
{
	public class ShipModelMst
	{
		protected Mst_ship _mst_data;

		public int MstId => _mst_data.Id;

		public string Name => _mst_data.Name;

		public string Yomi => _mst_data.Yomi;

		public virtual int ShipType => _mst_data.Stype;

		public string ShipTypeName => Mst_DataManager.Instance.Mst_stype[ShipType].Name;

		public int BuildStep => Mst_DataManager.Instance.Mst_stype[ShipType].Kcnt;

		public int ClassType => _mst_data.Ctype;

		public int ClassNum => _mst_data.Cnum;

		public int Rare => _mst_data.Backs;

		public virtual int Taikyu => _mst_data.Taik;

		public virtual int Karyoku => _mst_data.Houg;

		public virtual int KaryokuMax => _mst_data.Houg_max;

		public virtual int Raisou => _mst_data.Raig;

		public virtual int RaisouMax => _mst_data.Raig_max;

		public virtual int Soukou => _mst_data.Souk;

		public virtual int SoukouMax => _mst_data.Souk_max;

		public virtual int Taiku => _mst_data.Tyku;

		public virtual int TaikuMax => _mst_data.Tyku_max;

		public virtual int Kaihi => _mst_data.Kaih;

		public virtual int KaihiMax => _mst_data.Kaih_max;

		public virtual int Taisen => _mst_data.Tais;

		public virtual int TaisenMax => _mst_data.Tais_max;

		public virtual int Lucky => _mst_data.Luck;

		public virtual int LuckyMax => _mst_data.Luck_max;

		public int TousaiMaxAll
		{
			get
			{
				int num = 0;
				for (int i = 0; i < SlotCount; i++)
				{
					num += _mst_data.Maxeq[i];
				}
				return num;
			}
		}

		public int[] TousaiMax => _mst_data.Maxeq.GetRange(0, SlotCount).ToArray();

		public int Soku => _mst_data.Soku;

		public virtual int Leng => _mst_data.Leng;

		public virtual int SlotCount => _mst_data.Slot_num;

		public int FuelMax => _mst_data.Fuel_max;

		public int AmmoMax => _mst_data.Bull_max;

		public int AfterLevel => _mst_data.Afterlv;

		public int AfterAmmo => _mst_data.Afterbull;

		public int AfterSteel => _mst_data.Afterfuel;

		public int AfterDevkit => _mst_data.GetRemodelDevKitNum();

		public int AfterShipId => _mst_data.Aftershipid;

		public int PowUpKaryoku => _mst_data.Powup1;

		public int PowUpRaisou => _mst_data.Powup2;

		public int PowUpTaikuu => _mst_data.Powup3;

		public int PowUpSoukou => _mst_data.Powup4;

		public int PowUpLucky => (_mst_data.GetLuckUpKeisu() > 0.0) ? 1 : 0;

		public ShipOffset Offsets => new ShipOffset(GetGraphicsMstId());

		protected ShipModelMst()
		{
		}

		public ShipModelMst(int mst_id)
		{
			_mst_data = Mst_DataManager.Instance.Mst_ship[mst_id];
		}

		public ShipModelMst(Mst_ship mst)
		{
			_mst_data = mst;
		}

		public BtpType GetBTPType()
		{
			return _mst_data.GetBtpType();
		}

		public bool IsGroundFacility()
		{
			return _mst_data.Soku == 0;
		}

		public List<SlotitemCategory> GetEquipCategory()
		{
			List<SlotitemCategory> list = new List<SlotitemCategory>();
			Dictionary<int, Mst_equip_category> mst_equip_category = Mst_DataManager.Instance.Mst_equip_category;
			List<int> equipList = _mst_data.GetEquipList();
			foreach (int item in equipList)
			{
				SlotitemCategory slotitem_type = mst_equip_category[item].Slotitem_type;
				list.Add(slotitem_type);
			}
			return list.Distinct().ToList();
		}

		public int GetGraphicsMstId()
		{
			return Utils.GetResourceMstId(MstId);
		}

		public int GetVoiceMstId(int voice_id)
		{
			return Utils.GetVoiceMstId(MstId, voice_id);
		}
	}
}
