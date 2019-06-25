using Common.Enum;
using Common.Struct;
using Server_Common;
using Server_Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mem_ship : Model_Base, IReqNewGetNo
	{
		public enum enumKyoukaIdx
		{
			Houg,
			Raig,
			Tyku,
			Souk,
			Kaihi,
			Taisen,
			Taik,
			Taik_Powerup,
			Luck
		}

		public enum BlingKind
		{
			None = 0,
			Bling = 1,
			PortBack = 2,
			WaitDeck = 11,
			WaitEscort = 12
		}

		public const int C_TAIK_POWERUP_MAX = 3;

		public const int COND_MIN = 0;

		public const int COND_MAX = 255;

		public const int COND_DEFAULT = 40;

		public const int COND_RATION_MAX = 100;

		public const int LOV_DEFAULT = 50;

		public const int LOV_MIN = 0;

		public const int LOV_MAX = 999;

		public const int LOV_MAX_FRONT_TOUCH_PLUS = 5;

		public const int LOV_MAX_FRONT_TOUCH_MINUS = -10;

		public const int LOV_MAX_BACK_TOUCH_PLUS = 7;

		public const int LOV_MAX_BACK_TOUCH_MINUS = -10;

		public const int EXSLOT_NOT_OPEN = -2;

		private int _rid;

		private int _getNo;

		private int _sortno;

		private int _ship_id;

		private int _level;

		private int _exp;

		private int _nowhp;

		private int _maxhp;

		private int _leng;

		private List<int> _slot;

		private List<int> _onslot;

		private int _exslot;

		private Dictionary<enumKyoukaIdx, int> _kyouka;

		private int _backs;

		private int _fuel;

		private int _bull;

		private int _slotnum;

		private int _cond;

		private int _houg;

		private int _houg_max;

		private int _raig;

		private int _raig_max;

		private int _taiku;

		private int _taiku_max;

		private int _soukou;

		private int _soukou_max;

		private int _houm;

		private int _kaihi;

		private int _kaihi_max;

		private int _taisen;

		private int _taisen_max;

		private int _sakuteki;

		private int _sakuteki_max;

		private int _luck;

		private int _luck_max;

		private int _locked;

		private int _stype;

		private bool _escape_sts;

		private BlingKind _blingType;

		private int _blingWaitArea;

		private int _bling_start;

		private int _bling_end;

		private int _lov;

		private List<int> _lov_front_value;

		private Dictionary<byte, byte> _lov_front_processed;

		private List<int> _lov_back_value;

		private Dictionary<byte, byte> _lov_back_processed;

		private List<BattleCommand> battleCommand;

		private Ship_GrowValues BattleBaseParam;

		private int beforeRequireExp;

		private int nextRequireExp;

		private int requiereExp;

		private bool ememy_flag;

		private static string _tableName = "mem_ship";

		public int Rid
		{
			get
			{
				return _rid;
			}
			private set
			{
				_rid = value;
			}
		}

		public int GetNo
		{
			get
			{
				return _getNo;
			}
			private set
			{
				_getNo = value;
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

		public int Ship_id
		{
			get
			{
				return _ship_id;
			}
			private set
			{
				_ship_id = value;
			}
		}

		public int Level
		{
			get
			{
				return _level;
			}
			private set
			{
				_level = value;
			}
		}

		public int Exp
		{
			get
			{
				return _exp;
			}
			private set
			{
				_exp = value;
			}
		}

		public int Exp_next => getNextExp();

		public int Exp_rate => getNextExpRate();

		public int Nowhp
		{
			get
			{
				return _nowhp;
			}
			private set
			{
				_nowhp = value;
			}
		}

		public int Maxhp
		{
			get
			{
				return _maxhp;
			}
			private set
			{
				_maxhp = value;
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

		public List<int> Slot
		{
			get
			{
				return _slot;
			}
			private set
			{
				_slot = value;
			}
		}

		public List<int> Onslot
		{
			get
			{
				return _onslot;
			}
			private set
			{
				_onslot = value;
			}
		}

		public int Exslot
		{
			get
			{
				return _exslot;
			}
			private set
			{
				_exslot = value;
			}
		}

		public Dictionary<enumKyoukaIdx, int> Kyouka
		{
			get
			{
				return _kyouka;
			}
			private set
			{
				_kyouka = value;
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

		public int Fuel
		{
			get
			{
				return _fuel;
			}
			private set
			{
				_fuel = value;
			}
		}

		public int Bull
		{
			get
			{
				return _bull;
			}
			private set
			{
				_bull = value;
			}
		}

		public int Slotnum
		{
			get
			{
				return _slotnum;
			}
			private set
			{
				_slotnum = value;
			}
		}

		public int Cond
		{
			get
			{
				return _cond;
			}
			private set
			{
				_cond = value;
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

		public int Taiku
		{
			get
			{
				return _taiku;
			}
			private set
			{
				_taiku = value;
			}
		}

		public int Taiku_max
		{
			get
			{
				return _taiku_max;
			}
			private set
			{
				_taiku_max = value;
			}
		}

		public int Soukou
		{
			get
			{
				return _soukou;
			}
			private set
			{
				_soukou = value;
			}
		}

		public int Soukou_max
		{
			get
			{
				return _soukou_max;
			}
			private set
			{
				_soukou_max = value;
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

		public int Kaihi
		{
			get
			{
				return _kaihi;
			}
			private set
			{
				_kaihi = value;
			}
		}

		public int Kaihi_max
		{
			get
			{
				return _kaihi_max;
			}
			private set
			{
				_kaihi_max = value;
			}
		}

		public int Taisen
		{
			get
			{
				return _taisen;
			}
			private set
			{
				_taisen = value;
			}
		}

		public int Taisen_max
		{
			get
			{
				return _taisen_max;
			}
			private set
			{
				_taisen_max = value;
			}
		}

		public int Sakuteki
		{
			get
			{
				return _sakuteki;
			}
			private set
			{
				_sakuteki = value;
			}
		}

		public int Sakuteki_max
		{
			get
			{
				return _sakuteki_max;
			}
			private set
			{
				_sakuteki_max = value;
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

		public int Locked
		{
			get
			{
				return _locked;
			}
			private set
			{
				_locked = value;
			}
		}

		public int Damage_Rate => (int)((float)Nowhp / (float)Maxhp * 100f);

		public int Srate => GetStarValue();

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

		public bool Escape_sts
		{
			get
			{
				return _escape_sts;
			}
			private set
			{
				_escape_sts = value;
			}
		}

		public BlingKind BlingType
		{
			get
			{
				return _blingType;
			}
			private set
			{
				_blingType = value;
			}
		}

		public int BlingWaitArea
		{
			get
			{
				return _blingWaitArea;
			}
			private set
			{
				_blingWaitArea = value;
			}
		}

		public int Bling_start
		{
			get
			{
				return _bling_start;
			}
			private set
			{
				_bling_start = value;
			}
		}

		public int Bling_end
		{
			get
			{
				return _bling_end;
			}
			private set
			{
				_bling_end = value;
			}
		}

		public int Lov
		{
			get
			{
				return _lov;
			}
			private set
			{
				_lov = value;
			}
		}

		public List<int> Lov_front_value
		{
			get
			{
				return _lov_front_value;
			}
			private set
			{
				_lov_front_value = value;
			}
		}

		public Dictionary<byte, byte> Lov_front_processed
		{
			get
			{
				return _lov_front_processed;
			}
			private set
			{
				_lov_front_processed = value;
			}
		}

		public List<int> Lov_back_value
		{
			get
			{
				return _lov_back_value;
			}
			private set
			{
				_lov_back_value = value;
			}
		}

		public Dictionary<byte, byte> Lov_back_processed
		{
			get
			{
				return _lov_back_processed;
			}
			private set
			{
				_lov_back_processed = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_ship()
		{
			Slot = new List<int>(5);
			Onslot = new List<int>(5);
			Exslot = -2;
			Kyouka = new Dictionary<enumKyoukaIdx, int>();
			BlingWaitArea = 0;
			BlingType = BlingKind.None;
			Lov_front_processed = new Dictionary<byte, byte>();
			Lov_back_processed = new Dictionary<byte, byte>();
			Lov_back_value = new List<int>();
			Lov_front_value = new List<int>();
		}

		public int GetSortNo()
		{
			return _getNo;
		}

		public void ChangeSortNo(int no)
		{
			_getNo = no;
		}

		public void SetRequireExp(int level, Dictionary<int, int> mst_level)
		{
			beforeRequireExp = (mst_level.ContainsKey(level - 1) ? mst_level[level - 1] : 0);
			nextRequireExp = (mst_level.ContainsKey(level + 1) ? mst_level[level + 1] : 0);
			requiereExp = mst_level[level];
			if (beforeRequireExp == -1)
			{
				beforeRequireExp = 0;
			}
			if (nextRequireExp == -1)
			{
				nextRequireExp = 0;
			}
			if (requiereExp == -1)
			{
				requiereExp = 0;
			}
		}

		public bool IsOpenExSlot()
		{
			return (Exslot != -2) ? true : false;
		}

		public bool Set_New_ShipData(int rid, int getNo, int mst_id)
		{
			if (!Mst_DataManager.Instance.Mst_ship.ContainsKey(mst_id))
			{
				return false;
			}
			Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship[mst_id];
			Mem_shipBase baseData = new Mem_shipBase(rid, getNo, mst_data);
			Set_ShipParam(baseData, mst_data, enemy_flag: false);
			return true;
		}

		public void Set_ShipParam(Mem_shipBase baseData, Mst_ship mst_data, bool enemy_flag)
		{
			Rid = baseData.Rid;
			GetNo = baseData.GetNo;
			Sortno = mst_data.Sortno;
			Ship_id = mst_data.Id;
			Level = baseData.Level;
			Nowhp = baseData.Nowhp;
			Exp = baseData.Exp;
			Leng = mst_data.Leng;
			Kyouka[enumKyoukaIdx.Houg] = baseData.C_houg;
			Kyouka[enumKyoukaIdx.Luck] = baseData.C_luck;
			Kyouka[enumKyoukaIdx.Raig] = baseData.C_raig;
			Kyouka[enumKyoukaIdx.Souk] = baseData.C_souk;
			Kyouka[enumKyoukaIdx.Kaihi] = baseData.C_kaihi;
			Kyouka[enumKyoukaIdx.Taisen] = baseData.C_taisen;
			Kyouka[enumKyoukaIdx.Taik] = baseData.C_taik;
			Kyouka[enumKyoukaIdx.Taik_Powerup] = baseData.C_taik_powerup;
			Kyouka[enumKyoukaIdx.Tyku] = baseData.C_tyku;
			Backs = mst_data.Backs;
			Fuel = baseData.Fuel;
			Bull = baseData.Bull;
			Slotnum = mst_data.Slot_num;
			Cond = baseData.Cond;
			Ship_GrowValues ship_GrowValues = new Ship_GrowValues(mst_data, Level, Kyouka);
			baseData.C_houg = Kyouka[enumKyoukaIdx.Houg];
			baseData.C_luck = Kyouka[enumKyoukaIdx.Luck];
			baseData.C_raig = Kyouka[enumKyoukaIdx.Raig];
			baseData.C_souk = Kyouka[enumKyoukaIdx.Souk];
			baseData.C_kaihi = Kyouka[enumKyoukaIdx.Kaihi];
			baseData.C_taisen = Kyouka[enumKyoukaIdx.Taisen];
			baseData.C_taik = Kyouka[enumKyoukaIdx.Taik];
			baseData.C_taik_powerup = Kyouka[enumKyoukaIdx.Taik_Powerup];
			baseData.C_tyku = Kyouka[enumKyoukaIdx.Tyku];
			BattleBaseParam = ship_GrowValues;
			Maxhp = ship_GrowValues.Maxhp;
			Houg = ship_GrowValues.Houg;
			Houg_max = mst_data.Houg_max;
			Raig = ship_GrowValues.Raig;
			Raig_max = mst_data.Raig_max;
			Sakuteki = ship_GrowValues.Sakuteki;
			Sakuteki_max = mst_data.Saku_max;
			Soukou = ship_GrowValues.Soukou;
			Soukou_max = mst_data.Souk_max;
			Taiku = ship_GrowValues.Taiku;
			Taiku_max = mst_data.Tyku_max;
			Taisen = ship_GrowValues.Taisen;
			Taisen_max = mst_data.Tais_max;
			Luck = ship_GrowValues.Luck;
			Luck_max = mst_data.Luck_max;
			Kaihi = ship_GrowValues.Kaihi;
			Kaihi_max = mst_data.Kaih_max;
			Slot.Clear();
			Onslot.Clear();
			for (int i = 0; i < mst_data.Slot_num; i++)
			{
				Slot.Add(baseData.Slot[i]);
				Onslot.Add(baseData.Onslot[i]);
				if (baseData.Slot[i] > 0)
				{
					int key = enemy_flag ? baseData.Slot[i] : Comm_UserDatas.Instance.User_slot[baseData.Slot[i]].Slotitem_id;
					Mst_slotitem slotParam = Mst_DataManager.Instance.Mst_Slotitem[key];
					setSlotParam(slotParam);
				}
			}
			Exslot = baseData.Exslot;
			if (Exslot > 0)
			{
				setSlotParam(GetMstSlotItemToExSlot());
			}
			Locked = baseData.Locked;
			ememy_flag = enemy_flag;
			Stype = mst_data.Stype;
			Escape_sts = baseData.Escape_sts;
			BlingType = baseData.BlingType;
			BlingWaitArea = baseData.BlingWaitArea;
			Bling_start = baseData.Bling_start;
			Bling_end = baseData.Bling_end;
			battleCommand = baseData.BattleCommand;
			Lov = baseData.Lov;
			Lov_back_processed = baseData.Lov_back_processed;
			Lov_back_value = baseData.Lov_back_value;
			Lov_front_processed = baseData.Lov_front_processed;
			Lov_front_value = baseData.Lov_front_value;
		}

		private void setSlotParam(Mst_slotitem m_slot)
		{
			Houg += getSlotParamAddValue(Houg, m_slot.Houg);
			Raig += getSlotParamAddValue(Raig, m_slot.Raig);
			Sakuteki += getSlotParamAddValue(Sakuteki, m_slot.Saku);
			Soukou += getSlotParamAddValue(Soukou, m_slot.Souk);
			Taiku += getSlotParamAddValue(Taiku, m_slot.Tyku);
			Taisen += getSlotParamAddValue(Taisen, m_slot.Tais);
			Houm += getSlotParamAddValue(Houm, m_slot.Houm);
			Kaihi += getSlotParamAddValue(Kaihi, m_slot.Houk);
			if (Leng < m_slot.Leng)
			{
				Leng = m_slot.Leng;
			}
		}

		private int getSlotParamAddValue(int nowVal, int mstVal)
		{
			int num = nowVal + mstVal;
			return (num >= 0) ? mstVal : (-nowVal);
		}

		public void Set_ShipParamPracticeShip(Mem_shipBase baseData, Mst_ship mst_data)
		{
			Set_ShipParam(baseData, mst_data, enemy_flag: false);
			for (int i = 0; i < Slot.Count; i++)
			{
				int num = Slot[i];
				if (num > 0)
				{
					Slot[i] = Comm_UserDatas.Instance.User_slot[num].Slotitem_id;
				}
			}
			ememy_flag = true;
		}

		public void Set_ShipParamNewGamePlus(ICreateNewUser instance)
		{
			if (instance == null)
			{
				return;
			}
			Bling_start = 0;
			Bling_end = 0;
			BlingType = BlingKind.None;
			BlingWaitArea = 0;
			Nowhp = Maxhp;
			Cond = 40;
			Mst_ship value = null;
			if (Mst_DataManager.Instance.Mst_ship.TryGetValue(Ship_id, out value))
			{
				Fuel = value.Fuel_max;
				Bull = value.Bull_max;
				for (int i = 0; i < value.Slot_num; i++)
				{
					Onslot[i] = value.Maxeq[i];
				}
			}
		}

		public SlotSetInfo GetSlotSetParam(int slot_index, int slot_id)
		{
			SlotSetInfo result = default(SlotSetInfo);
			if (slot_index >= Slot.Count)
			{
				return result;
			}
			if (Slot[slot_index] <= 0 && slot_id < 0)
			{
				return result;
			}
			List<Mst_slotitem> mstSlotItems = GetMstSlotItems();
			List<int> list = (mstSlotItems.Count <= 0) ? new List<int>() : (from x in mstSlotItems
				select x.Leng).ToList();
			list.Add(Mst_DataManager.Instance.Mst_ship[Ship_id].Leng);
			Mst_slotitem mstSlotItemToExSlot = GetMstSlotItemToExSlot();
			if (mstSlotItemToExSlot != null)
			{
				list.Add(mstSlotItemToExSlot.Leng);
			}
			int slotitem_id;
			Mst_slotitem mst_slotitem;
			if (Slot[slot_index] <= 0)
			{
				slotitem_id = Comm_UserDatas.Instance.User_slot[slot_id].Slotitem_id;
				mst_slotitem = Mst_DataManager.Instance.Mst_Slotitem[slotitem_id];
				list.Add(mst_slotitem.Leng);
				result.SetSlot(Leng, mst_slotitem);
				result.SetLeng(Leng, list);
				return result;
			}
			int slotitem_id2 = Comm_UserDatas.Instance.User_slot[Slot[slot_index]].Slotitem_id;
			Mst_slotitem mst_slotitem2 = Mst_DataManager.Instance.Mst_Slotitem[slotitem_id2];
			list.RemoveAt(slot_index);
			if (slot_id < 0)
			{
				result.UnsetSlot(mst_slotitem2);
				result.SetLeng(Leng, list);
				return result;
			}
			slotitem_id = Comm_UserDatas.Instance.User_slot[slot_id].Slotitem_id;
			mst_slotitem = Mst_DataManager.Instance.Mst_Slotitem[slotitem_id];
			list.Add(mst_slotitem.Leng);
			result.ChangeSlot(mst_slotitem2, mst_slotitem);
			result.SetLeng(Leng, list);
			return result;
		}

		public SlotSetInfo GetSlotSetParam(int slot_id)
		{
			SlotSetInfo result = default(SlotSetInfo);
			if (Exslot <= 0 && slot_id == -1)
			{
				return result;
			}
			Mst_slotitem mstSlotItemToExSlot = GetMstSlotItemToExSlot();
			List<Mst_slotitem> mstSlotItems = GetMstSlotItems();
			List<int> list = (mstSlotItems.Count <= 0) ? new List<int>() : (from x in mstSlotItems
				select x.Leng).ToList();
			list.Add(Mst_DataManager.Instance.Mst_ship[Ship_id].Leng);
			int slotitem_id;
			Mst_slotitem mst_slotitem;
			if (mstSlotItemToExSlot == null && slot_id > 0)
			{
				slotitem_id = Comm_UserDatas.Instance.User_slot[slot_id].Slotitem_id;
				mst_slotitem = Mst_DataManager.Instance.Mst_Slotitem[slotitem_id];
				list.Add(mst_slotitem.Leng);
				result.SetSlot(Leng, mst_slotitem);
				result.SetLeng(Leng, list);
				return result;
			}
			if (slot_id < 0)
			{
				result.UnsetSlot(mstSlotItemToExSlot);
				result.SetLeng(Leng, list);
				return result;
			}
			slotitem_id = Comm_UserDatas.Instance.User_slot[slot_id].Slotitem_id;
			mst_slotitem = Mst_DataManager.Instance.Mst_Slotitem[slotitem_id];
			list.Add(mst_slotitem.Leng);
			result.ChangeSlot(mstSlotItemToExSlot, mst_slotitem);
			result.SetLeng(Leng, list);
			return result;
		}

		public Ship_GrowValues GetBattleBaseParam()
		{
			return BattleBaseParam;
		}

		public List<BattleCommand> GetBattleCommad()
		{
			if (IsEnemy())
			{
				return null;
			}
			int battleCommandEnableNum = GetBattleCommandEnableNum();
			if (battleCommand != null)
			{
				return battleCommand.ToList();
			}
			List<BattleCommand> list = Enumerable.Repeat(BattleCommand.None, 5).ToList();
			if (Mst_DataManager.Instance.Mst_stype[Stype].IsKouku())
			{
				switch (battleCommandEnableNum)
				{
				case 3:
					list[0] = BattleCommand.Kouku;
					list[1] = BattleCommand.Hougeki;
					list[2] = BattleCommand.Raigeki;
					break;
				case 4:
					list[0] = BattleCommand.Kouku;
					list[1] = BattleCommand.Hougeki;
					list[2] = BattleCommand.Hougeki;
					list[3] = BattleCommand.Raigeki;
					break;
				default:
					list[0] = BattleCommand.Kouku;
					list[1] = BattleCommand.Hougeki;
					list[2] = BattleCommand.Hougeki;
					list[3] = BattleCommand.Hougeki;
					list[4] = BattleCommand.Raigeki;
					break;
				}
			}
			else
			{
				switch (battleCommandEnableNum)
				{
				case 3:
					list[0] = BattleCommand.Hougeki;
					list[1] = BattleCommand.Hougeki;
					list[2] = BattleCommand.Raigeki;
					break;
				case 4:
					list[0] = BattleCommand.Hougeki;
					list[1] = BattleCommand.Hougeki;
					list[2] = BattleCommand.Hougeki;
					list[3] = BattleCommand.Raigeki;
					break;
				default:
					list[0] = BattleCommand.Hougeki;
					list[1] = BattleCommand.Hougeki;
					list[2] = BattleCommand.Hougeki;
					list[3] = BattleCommand.Hougeki;
					list[4] = BattleCommand.Raigeki;
					break;
				}
			}
			battleCommand = list;
			return battleCommand.ToList();
		}

		public void GetBattleCommand(out List<BattleCommand> command)
		{
			command = battleCommand;
		}

		public int GetBattleCommandEnableNum()
		{
			if (Level >= 1 && Level <= 34)
			{
				return 3;
			}
			if (Level >= 35 && Level <= 69)
			{
				return 4;
			}
			return 5;
		}

		public void SetBattleCommand(List<BattleCommand> command)
		{
			if (battleCommand == null)
			{
				GetBattleCommad();
			}
			for (int i = 0; i < command.Count; i++)
			{
				battleCommand[i] = command[i];
			}
		}

		public void PurgeBattleCommand()
		{
			battleCommand.Clear();
			battleCommand = null;
		}

		public static DamageState Get_DamageState(int nowhp, int maxhp)
		{
			if ((float)nowhp <= (float)maxhp * 0.25f)
			{
				return DamageState.Taiha;
			}
			if ((float)nowhp <= (float)maxhp * 0.5f)
			{
				return DamageState.Tyuuha;
			}
			if ((float)nowhp <= (float)maxhp * 0.75f)
			{
				return DamageState.Shouha;
			}
			return DamageState.Normal;
		}

		public DamageState Get_DamageState()
		{
			return Get_DamageState(Nowhp, Maxhp);
		}

		public static FatigueState Get_FatitgueState(int cond)
		{
			if (cond >= 50)
			{
				return FatigueState.Exaltation;
			}
			if (cond >= 30 && cond <= 49)
			{
				return FatigueState.Normal;
			}
			if (cond >= 20 && cond <= 29)
			{
				return FatigueState.Light;
			}
			return FatigueState.Distress;
		}

		public FatigueState Get_FatigueState()
		{
			return Get_FatitgueState(Cond);
		}

		public bool IsEnemy()
		{
			return ememy_flag;
		}

		public void Set_ChargeData(int bull, int fuel, List<int> onslot)
		{
			Bull = bull;
			Fuel = fuel;
			if (onslot != null)
			{
				for (int i = 0; i < Onslot.Count; i++)
				{
					Onslot[i] = onslot[i];
				}
			}
		}

		public int[] FindRecoveryItem()
		{
			int[] idx = new int[2];
			idx[0] = Slot.FindIndex(delegate(int x)
			{
				Mem_slotitem value = null;
				Comm_UserDatas.Instance.User_slot.TryGetValue(x, out value);
				if (value != null && (value.Slotitem_id == 42 || value.Slotitem_id == 43))
				{
					idx[1] = value.Slotitem_id;
					return true;
				}
				return false;
			});
			Mst_slotitem mstSlotItemToExSlot = GetMstSlotItemToExSlot();
			if (mstSlotItemToExSlot != null)
			{
				int id = mstSlotItemToExSlot.Id;
				if (id == 42 || id == 43)
				{
					idx[0] = int.MaxValue;
					idx[1] = mstSlotItemToExSlot.Id;
				}
			}
			return idx;
		}

		public int GetRecoveryItemUseHp(ShipRecoveryType recoveryType, bool flagShipRecovery)
		{
			if (recoveryType == ShipRecoveryType.Personnel)
			{
				double num = (!flagShipRecovery) ? Math.Floor((float)Maxhp * 0.2f) : Math.Floor((float)Maxhp * 0.5f);
				return (int)num;
			}
			return Maxhp;
		}

		public bool UseRecoveryItem(int[] recoveryItemIdx, bool flagShipRecovery)
		{
			if (recoveryItemIdx[0] == -1)
			{
				return false;
			}
			if (recoveryItemIdx[1] == 43)
			{
				Nowhp = GetRecoveryItemUseHp(ShipRecoveryType.Goddes, flagShipRecovery: false);
				Fuel = Mst_DataManager.Instance.Mst_ship[Ship_id].Fuel_max;
				Bull = Mst_DataManager.Instance.Mst_ship[Ship_id].Bull_max;
			}
			else if (recoveryItemIdx[1] == 42)
			{
				Nowhp = GetRecoveryItemUseHp(ShipRecoveryType.Personnel, flagShipRecovery);
			}
			int item;
			if (recoveryItemIdx[0] != int.MaxValue)
			{
				item = Slot[recoveryItemIdx[0]];
				Slot[recoveryItemIdx[0]] = -1;
				TrimSlot();
			}
			else
			{
				item = Exslot;
				Exslot = -1;
			}
			Comm_UserDatas.Instance.Remove_Slot(new List<int>
			{
				item
			});
			Comm_UserDatas.Instance.User_trophy.Use_recovery_item_count++;
			return true;
		}

		public bool IsFight()
		{
			if (Nowhp > 0 && !Escape_sts)
			{
				return true;
			}
			return false;
		}

		public bool IsEscortDeffender()
		{
			bool flag = Mst_DataManager.Instance.Mst_stype[Stype].IsSubmarine();
			DamageState damageState = Get_DamageState();
			bool flag2 = IsBlingShip();
			if (flag || flag2 || damageState >= DamageState.Tyuuha)
			{
				return false;
			}
			return true;
		}

		public void SubHp(int subValue)
		{
			Nowhp -= subValue;
			if (Nowhp < 0)
			{
				Nowhp = 0;
			}
		}

		public int getLevelupInfo(Dictionary<int, int> mst_level, int nowLevel, int nowExp, ref int addExp, out List<int> lvupInfo)
		{
			int num = nowLevel;
			int num2 = (nowLevel <= 99) ? mst_level[99] : mst_level.Values.Max();
			if (addExp + nowExp > num2)
			{
				addExp = num2 - nowExp;
			}
			int key = nowLevel + 1;
			int num3 = mst_level[key];
			int afterExp = addExp + nowExp;
			lvupInfo = new List<int>
			{
				nowExp
			};
			if (num3 == -1)
			{
				return num;
			}
			if (afterExp < num3)
			{
				lvupInfo.Add(num3);
				return num;
			}
			IEnumerable<int> enumerable = from data in mst_level
				where data.Key > nowLevel && data.Value <= afterExp && data.Value != -1
				select data.Value;
			lvupInfo.AddRange(enumerable);
			num += enumerable.Count();
			if (mst_level[num + 1] != -1)
			{
				lvupInfo.Add(mst_level[num + 1]);
			}
			return num;
		}

		public Dictionary<enumKyoukaIdx, int> getLevelupKyoukaValue(int mst_id, Dictionary<enumKyoukaIdx, int> nowKyouka)
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[mst_id];
			Dictionary<enumKyoukaIdx, int[]> dictionary = new Dictionary<enumKyoukaIdx, int[]>();
			dictionary.Add(enumKyoukaIdx.Houg, new int[2]
			{
				mst_ship.Houg,
				mst_ship.Houg_max
			});
			dictionary.Add(enumKyoukaIdx.Raig, new int[2]
			{
				mst_ship.Raig,
				mst_ship.Raig_max
			});
			dictionary.Add(enumKyoukaIdx.Tyku, new int[2]
			{
				mst_ship.Tyku,
				mst_ship.Tyku_max
			});
			dictionary.Add(enumKyoukaIdx.Souk, new int[2]
			{
				mst_ship.Souk,
				mst_ship.Souk_max
			});
			dictionary.Add(enumKyoukaIdx.Kaihi, new int[2]
			{
				mst_ship.Kaih,
				mst_ship.Kaih_max
			});
			dictionary.Add(enumKyoukaIdx.Taisen, new int[2]
			{
				mst_ship.Tais,
				mst_ship.Tais_max
			});
			Dictionary<enumKyoukaIdx, int[]> dictionary2 = dictionary;
			Dictionary<enumKyoukaIdx, int> dictionary3 = new Dictionary<enumKyoukaIdx, int>();
			foreach (KeyValuePair<enumKyoukaIdx, int> item in nowKyouka)
			{
				if (!dictionary2.ContainsKey(item.Key))
				{
					dictionary3.Add(item.Key, item.Value);
				}
				else
				{
					int num = dictionary2[item.Key][0];
					int num2 = dictionary2[item.Key][1];
					int num3 = num + item.Value;
					if (num3 >= num2)
					{
						dictionary3.Add(item.Key, item.Value);
					}
					else
					{
						int num4 = num2 - num - item.Value;
						int num5 = (num4 < 20) ? 2 : 3;
						int num6 = (int)Utils.GetRandDouble(0.0, num5, 1.0, 1);
						int num7 = num3 + num6;
						if (num7 > num2)
						{
							dictionary3.Add(item.Key, num2 - num);
						}
						else
						{
							dictionary3.Add(item.Key, item.Value + num6);
						}
					}
				}
			}
			return dictionary3;
		}

		public Dictionary<enumMaterialCategory, int> GetNdockMaterialNum()
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[Ship_id];
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			int num = Maxhp - Nowhp;
			if (num <= 0)
			{
				return null;
			}
			int value = (int)((float)(mst_ship.Use_fuel * num) * 0.4f * 0.4f);
			dictionary.Add(enumMaterialCategory.Fuel, value);
			int value2 = (int)((float)(mst_ship.Use_fuel * num) * 0.75f * 0.4f);
			dictionary.Add(enumMaterialCategory.Steel, value2);
			return dictionary;
		}

		public int GetNdockTimeSpan()
		{
			int num = Maxhp - Nowhp;
			if (num <= 0)
			{
				return 0;
			}
			int ndockTime = GetNdockTime();
			int num2 = (int)Math.Ceiling((double)ndockTime / 60.0 / 30.0);
			if (num2 > 10)
			{
				num2 = 10 + (int)((double)(num2 - 10) * 0.5 + 0.5);
			}
			return num2;
		}

		private int GetNdockTime()
		{
			int num = 30;
			int num2 = 5;
			double num3 = Level;
			if (num3 > 11.0)
			{
				num3 = (double)num2 + num3 / 2.0 + (double)(int)Math.Sqrt(num3 - 11.0);
			}
			int num4 = (int)(num3 * (double)(Maxhp - Nowhp) * (double)Mst_DataManager.Instance.Mst_stype[Stype].Scnt);
			return num + num4 * num2;
		}

		public void NdockRecovery(Mem_ndock dockInstance)
		{
			if (dockInstance.Ship_id == Rid)
			{
				Nowhp = Maxhp;
			}
			if (Cond < 40)
			{
				Cond = 40;
			}
		}

		public bool ExistsMission()
		{
			return Comm_UserDatas.Instance.User_deck.Values.Any((Mem_deck x) => (x.MissionState != 0 && x.Ship.Find(Rid) != -1) ? true : false);
		}

		public bool ExistsNdock()
		{
			return Comm_UserDatas.Instance.User_ndock.Values.Any((Mem_ndock x) => (x.Ship_id == Rid) ? true : false);
		}

		public void ChangeLockSts()
		{
			Locked ^= 1;
		}

		public bool IsBlingShip()
		{
			return (BlingType == BlingKind.Bling || BlingType == BlingKind.PortBack) ? true : false;
		}

		public bool IsBlingWait()
		{
			return IsBlingWait(BlingType);
		}

		private bool IsBlingWait(BlingKind kind)
		{
			return (kind >= (BlingKind)10 && kind <= (BlingKind)19) ? true : false;
		}

		public bool IsPortBack()
		{
			return BlingType == BlingKind.PortBack;
		}

		public int GetBlingTurn()
		{
			if (!IsBlingShip())
			{
				return 0;
			}
			return Bling_end - Comm_UserDatas.Instance.User_turn.Total_turn;
		}

		public bool BlingSet(int area_id)
		{
			if (area_id == 1)
			{
				return false;
			}
			BlingType = BlingKind.Bling;
			Bling_start = Comm_UserDatas.Instance.User_turn.Total_turn;
			Bling_end = Comm_UserDatas.Instance.User_turn.Total_turn + Mst_DataManager.Instance.Mst_maparea[area_id].Distance;
			return true;
		}

		public void BlingWait(int area_id, BlingKind kind)
		{
			if (IsBlingWait(kind) && BlingSet(area_id))
			{
				BlingType = kind;
				BlingWaitArea = area_id;
			}
		}

		public void BlingWaitToStart()
		{
			if (IsBlingWait(BlingType))
			{
				BlingSet(BlingWaitArea);
				BlingWaitArea = 0;
			}
		}

		public void BlingWaitToStop()
		{
			if (IsBlingWait(BlingType))
			{
				BlingType = BlingKind.None;
				BlingWaitArea = 0;
			}
		}

		public void PortWithdraw(int area_id)
		{
			if (BlingSet(area_id))
			{
				BlingType = BlingKind.PortBack;
			}
		}

		public bool BlingTerm()
		{
			if (!IsBlingShip())
			{
				return false;
			}
			if (Bling_end > Comm_UserDatas.Instance.User_turn.Total_turn)
			{
				return false;
			}
			Bling_start = 0;
			Bling_end = 0;
			BlingType = BlingKind.None;
			return true;
		}

		public void SetHp(Api_req_PracticeBattle instance, int startHp)
		{
			Nowhp = startHp;
		}

		public Dictionary<enumMaterialCategory, int> getDestroyShipMaterials()
		{
			Dictionary<enumMaterialCategory, int> ret = new Dictionary<enumMaterialCategory, int>
			{
				{
					enumMaterialCategory.Fuel,
					0
				},
				{
					enumMaterialCategory.Bull,
					0
				},
				{
					enumMaterialCategory.Steel,
					0
				},
				{
					enumMaterialCategory.Bauxite,
					0
				}
			};
			Slot.ForEach(delegate(int x)
			{
				if (x > 0)
				{
					Mem_slotitem value = null;
					if (Comm_UserDatas.Instance.User_slot.TryGetValue(x, out value))
					{
						Mst_slotitem mst_slotitem = Mst_DataManager.Instance.Mst_Slotitem[value.Slotitem_id];
						Dictionary<enumMaterialCategory, int> dictionary17;
						Dictionary<enumMaterialCategory, int> dictionary18 = dictionary17 = ret;
						enumMaterialCategory key10;
						enumMaterialCategory key11 = key10 = enumMaterialCategory.Fuel;
						int num2 = dictionary17[key10];
						dictionary18[key11] = num2 + mst_slotitem.Broken1;
						Dictionary<enumMaterialCategory, int> dictionary19;
						Dictionary<enumMaterialCategory, int> dictionary20 = dictionary19 = ret;
						enumMaterialCategory key12 = key10 = enumMaterialCategory.Bull;
						num2 = dictionary19[key10];
						dictionary20[key12] = num2 + mst_slotitem.Broken2;
						Dictionary<enumMaterialCategory, int> dictionary21;
						Dictionary<enumMaterialCategory, int> dictionary22 = dictionary21 = ret;
						enumMaterialCategory key13 = key10 = enumMaterialCategory.Steel;
						num2 = dictionary21[key10];
						dictionary22[key13] = num2 + mst_slotitem.Broken3;
						Dictionary<enumMaterialCategory, int> dictionary23;
						Dictionary<enumMaterialCategory, int> dictionary24 = dictionary23 = ret;
						enumMaterialCategory key14 = key10 = enumMaterialCategory.Bauxite;
						num2 = dictionary23[key10];
						dictionary24[key14] = num2 + mst_slotitem.Broken4;
					}
				}
			});
			Mst_slotitem mstSlotItemToExSlot = GetMstSlotItemToExSlot();
			enumMaterialCategory key;
			int num;
			if (mstSlotItemToExSlot != null)
			{
				Dictionary<enumMaterialCategory, int> dictionary;
				Dictionary<enumMaterialCategory, int> dictionary2 = dictionary = ret;
				enumMaterialCategory key2 = key = enumMaterialCategory.Fuel;
				num = dictionary[key];
				dictionary2[key2] = num + mstSlotItemToExSlot.Broken1;
				Dictionary<enumMaterialCategory, int> dictionary3;
				Dictionary<enumMaterialCategory, int> dictionary4 = dictionary3 = ret;
				enumMaterialCategory key3 = key = enumMaterialCategory.Bull;
				num = dictionary3[key];
				dictionary4[key3] = num + mstSlotItemToExSlot.Broken2;
				Dictionary<enumMaterialCategory, int> dictionary5;
				Dictionary<enumMaterialCategory, int> dictionary6 = dictionary5 = ret;
				enumMaterialCategory key4 = key = enumMaterialCategory.Steel;
				num = dictionary5[key];
				dictionary6[key4] = num + mstSlotItemToExSlot.Broken3;
				Dictionary<enumMaterialCategory, int> dictionary7;
				Dictionary<enumMaterialCategory, int> dictionary8 = dictionary7 = ret;
				enumMaterialCategory key5 = key = enumMaterialCategory.Bauxite;
				num = dictionary7[key];
				dictionary8[key5] = num + mstSlotItemToExSlot.Broken4;
			}
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[Ship_id];
			Dictionary<enumMaterialCategory, int> dictionary9;
			Dictionary<enumMaterialCategory, int> dictionary10 = dictionary9 = ret;
			enumMaterialCategory key6 = key = enumMaterialCategory.Fuel;
			num = dictionary9[key];
			dictionary10[key6] = num + mst_ship.Broken1;
			Dictionary<enumMaterialCategory, int> dictionary11;
			Dictionary<enumMaterialCategory, int> dictionary12 = dictionary11 = ret;
			enumMaterialCategory key7 = key = enumMaterialCategory.Bull;
			num = dictionary11[key];
			dictionary12[key7] = num + mst_ship.Broken2;
			Dictionary<enumMaterialCategory, int> dictionary13;
			Dictionary<enumMaterialCategory, int> dictionary14 = dictionary13 = ret;
			enumMaterialCategory key8 = key = enumMaterialCategory.Steel;
			num = dictionary13[key];
			dictionary14[key8] = num + mst_ship.Broken3;
			Dictionary<enumMaterialCategory, int> dictionary15;
			Dictionary<enumMaterialCategory, int> dictionary16 = dictionary15 = ret;
			enumMaterialCategory key9 = key = enumMaterialCategory.Bauxite;
			num = dictionary15[key];
			dictionary16[key9] = num + mst_ship.Broken4;
			return ret;
		}

		public List<Mst_slotitem> GetMstSlotItems()
		{
			List<Mst_slotitem> ret = new List<Mst_slotitem>();
			Slot.ForEach(delegate(int x)
			{
				if (x > 0)
				{
					Mem_slotitem value = null;
					if (Comm_UserDatas.Instance.User_slot.TryGetValue(x, out value))
					{
						Mst_slotitem item = Mst_DataManager.Instance.Mst_Slotitem[value.Slotitem_id];
						ret.Add(item);
					}
				}
			});
			return ret;
		}

		public Mst_slotitem GetMstSlotItemToExSlot()
		{
			Mem_slotitem value = null;
			if (!Comm_UserDatas.Instance.User_slot.TryGetValue(Exslot, out value))
			{
				return null;
			}
			return Mst_DataManager.Instance.Mst_Slotitem[value.Slotitem_id];
		}

		public Dictionary<int, int> GetMstSlotItemNum_OrderId(HashSet<int> order_ids)
		{
			Dictionary<int, int> ret = order_ids.ToDictionary((int key) => key, (int value) => 0);
			GetMstSlotItems().ForEach(delegate(Mst_slotitem x)
			{
				if (ret.ContainsKey(x.Id))
				{
					Dictionary<int, int> dictionary3;
					Dictionary<int, int> dictionary4 = dictionary3 = ret;
					int id2;
					int key3 = id2 = x.Id;
					id2 = dictionary3[id2];
					dictionary4[key3] = id2 + 1;
				}
			});
			Mst_slotitem mstSlotItemToExSlot = GetMstSlotItemToExSlot();
			if (mstSlotItemToExSlot == null)
			{
				return ret;
			}
			if (ret.ContainsKey(mstSlotItemToExSlot.Id))
			{
				Dictionary<int, int> dictionary;
				Dictionary<int, int> dictionary2 = dictionary = ret;
				int id;
				int key2 = id = mstSlotItemToExSlot.Id;
				id = dictionary[id];
				dictionary2[key2] = id + 1;
			}
			return ret;
		}

		public Dictionary<int, List<int>> GetSlotIndexFromId(HashSet<int> searchIds)
		{
			Dictionary<int, List<int>> dictionary = searchIds.ToDictionary((int id) => id, (int value) => new List<int>());
			List<Mst_slotitem> mstSlotItems = GetMstSlotItems();
			for (int i = 0; i < mstSlotItems.Count; i++)
			{
				Mst_slotitem mst_slotitem = mstSlotItems[i];
				if (searchIds.Contains(mst_slotitem.Id))
				{
					dictionary[mst_slotitem.Id].Add(i);
				}
			}
			Mst_slotitem mstSlotItemToExSlot = GetMstSlotItemToExSlot();
			if (mstSlotItemToExSlot == null)
			{
				return dictionary;
			}
			if (searchIds.Contains(mstSlotItemToExSlot.Id))
			{
				dictionary[mstSlotItemToExSlot.Id].Add(int.MaxValue);
			}
			return dictionary;
		}

		public bool IsActiveEnd()
		{
			return Comm_UserDatas.Instance.User_deck.Values.FirstOrDefault((Mem_deck x) => (x.Ship.Find(Rid) != -1) ? true : false)?.IsActionEnd ?? false;
		}

		public void TrimSlot()
		{
			Slot.RemoveAll((int x) => x == -1);
			if (Slot.Count != Slotnum)
			{
				Slot.AddRange(Enumerable.Repeat(-1, Slotnum - Slot.Count));
			}
		}

		public int GetRequireChargeFuel()
		{
			int fuel_max = Mst_DataManager.Instance.Mst_ship[Ship_id].Fuel_max;
			if (Fuel >= fuel_max)
			{
				return 0;
			}
			double num = (Level <= 99) ? 1.0 : 0.85;
			double num2 = fuel_max - Fuel;
			return Math.Max((int)(num2 * num), 1);
		}

		public int GetRequireChargeBull()
		{
			int bull_max = Mst_DataManager.Instance.Mst_ship[Ship_id].Bull_max;
			if (Bull >= bull_max)
			{
				return 0;
			}
			double num = (Level <= 99) ? 1.0 : 0.85;
			double num2 = bull_max - Bull;
			return Math.Max((int)(num2 * num), 1);
		}

		private int getNextExp()
		{
			if (nextRequireExp <= 0)
			{
				return 0;
			}
			return nextRequireExp - Exp;
		}

		private int getNextExpRate()
		{
			if (nextRequireExp == 0)
			{
				return 0;
			}
			int num = (Level == 100) ? beforeRequireExp : requiereExp;
			float num2 = Exp - num;
			float num3 = nextRequireExp - num;
			if (num3 == 0f)
			{
				return 0;
			}
			return (int)(num2 / num3 * 100f);
		}

		private int GetStarValue()
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[Ship_id];
			float num = Kyouka[enumKyoukaIdx.Houg] + Kyouka[enumKyoukaIdx.Raig] + Kyouka[enumKyoukaIdx.Souk] + Kyouka[enumKyoukaIdx.Tyku];
			float num2 = mst_ship.Houg_max - mst_ship.Houg + (mst_ship.Raig_max - mst_ship.Raig) + (mst_ship.Tyku_max - mst_ship.Tyku) + (mst_ship.Souk_max - mst_ship.Souk);
			if (num2 == 0f)
			{
				return 0;
			}
			int num3 = (int)(num / num2 * 100f);
			if (num3 >= 80)
			{
				return 4;
			}
			if (num3 >= 60)
			{
				return 3;
			}
			if (num3 >= 40)
			{
				return 2;
			}
			if (num3 >= 10)
			{
				return 1;
			}
			return 0;
		}

		public void SetSubFuel_ToMission(double use_fuel)
		{
			int num = (int)Math.Floor((double)Fuel * use_fuel);
			Fuel -= num;
		}

		public void SetSubBull_ToMission(double use_bull)
		{
			int num = (int)Math.Floor((double)Bull * use_bull);
			Bull -= num;
		}

		public void SetSortieEndCond(Api_req_Map instance)
		{
			if (instance != null)
			{
				Cond -= 15;
				if (Cond < 0)
				{
					Cond = 0;
				}
			}
		}

		public void AddTurnRecoveryCond(Api_TurnOperator instance, int upNum)
		{
			if (instance != null && Cond < 49)
			{
				Cond += upNum;
				if (Cond > 49)
				{
					Cond = 49;
				}
			}
		}

		public void AddRationItemCond(int upNum)
		{
			Cond += upNum;
			if (Cond > 100)
			{
				Cond = 100;
			}
		}

		public HashSet<Mem_slotitem> GetLockSlotItems()
		{
			HashSet<Mem_slotitem> ret = new HashSet<Mem_slotitem>();
			Slot.ForEach(delegate(int x)
			{
				Mem_slotitem value = null;
				if (Comm_UserDatas.Instance.User_slot.TryGetValue(x, out value) && value.Lock)
				{
					ret.Add(value);
				}
			});
			if (Exslot > 0)
			{
				Mem_slotitem mem_slotitem = Comm_UserDatas.Instance.User_slot[Exslot];
				if (mem_slotitem.Lock)
				{
					ret.Add(mem_slotitem);
				}
			}
			return ret;
		}

		public void ChangeEscapeState()
		{
			Escape_sts = !Escape_sts;
		}

		public bool SumLov(ref TouchLovInfo touchInfo)
		{
			byte b = (byte)touchInfo.VoiceType;
			byte value = 0;
			List<int> list;
			Dictionary<byte, byte> dictionary;
			int num;
			int num2;
			if (touchInfo.BackTouch)
			{
				list = Lov_back_value;
				dictionary = Lov_back_processed;
				num = -10;
				num2 = 7;
			}
			else
			{
				list = Lov_front_value;
				dictionary = Lov_front_processed;
				num = -10;
				num2 = 5;
			}
			bool flag = dictionary.TryGetValue(b, out value);
			int nowTouchNum = value + 1;
			int sumValue = touchInfo.GetSumValue(nowTouchNum);
			if (sumValue == 0)
			{
				return false;
			}
			if (list.Count == 0)
			{
				list.Add(0);
				list.Add(0);
			}
			bool result = false;
			if (sumValue > 0)
			{
				list[0] = ((list[0] > 0) ? 4 : 0);
				if (list[0] < num2)
				{
					int num3 = (list[0] + sumValue <= num2) ? sumValue : (num2 - list[0]);
					List<int> list2;
					List<int> list3 = list2 = list;
					int index;
					int index2 = index = 0;
					index = list2[index];
					list3[index2] = index + num3;
					if (!flag)
					{
						dictionary.Add(b, 1);
					}
					else
					{
						Dictionary<byte, byte> dictionary2;
						Dictionary<byte, byte> dictionary3 = dictionary2 = dictionary;
						byte key;
						byte key2 = key = b;
						key = dictionary2[key];
						dictionary3[key2] = (byte)(key + 1);
					}
					result = addLov(num3);
				}
			}
			else if (list[1] > num)
			{
				int num4 = (list[1] + sumValue >= num) ? sumValue : (num - list[1]);
				List<int> list4;
				List<int> list5 = list4 = list;
				int index;
				int index3 = index = 1;
				index = list4[index];
				list5[index3] = index + num4;
				if (!flag)
				{
					dictionary.Add(b, 1);
				}
				else
				{
					Dictionary<byte, byte> dictionary4;
					Dictionary<byte, byte> dictionary5 = dictionary4 = dictionary;
					byte key;
					byte key3 = key = b;
					key = dictionary4[key];
					dictionary5[key3] = (byte)(key + 1);
				}
				result = subLov(num4);
			}
			return result;
		}

		public void PurgeLovTouchData()
		{
			Lov_back_processed.Clear();
			Lov_back_value.Clear();
			Lov_front_processed.Clear();
			Lov_front_value.Clear();
		}

		public bool SumLovToTurnStart(bool isInNdock, bool flagShip)
		{
			int num = 0;
			int lov = Lov;
			switch (Get_FatigueState())
			{
			case FatigueState.Light:
				num = -2;
				break;
			case FatigueState.Distress:
				num = -5;
				break;
			}
			int num2 = lov + num;
			if (isInNdock && num2 <= 97)
			{
				num += 3;
				num2 = lov + num;
			}
			if (num2 > 50)
			{
				num = ((!flagShip) ? (num - 3) : (num - 1));
				num2 = lov + num;
				if (num2 < 50)
				{
					num = 50 - num2;
				}
			}
			return SumLov(num);
		}

		public bool SumLovToBattle(BattleWinRankKinds rank, bool flagShip, bool mvp, int staHp, int endHp)
		{
			int num = 0;
			if (flagShip)
			{
				num = 3;
			}
			switch (rank)
			{
			case BattleWinRankKinds.S:
				num += 10;
				break;
			case BattleWinRankKinds.A:
				num += 8;
				break;
			case BattleWinRankKinds.B:
				num += 4;
				break;
			default:
				num -= 5;
				break;
			}
			if (mvp)
			{
				num += 10;
			}
			DamageState damageState = Get_DamageState(staHp, Maxhp);
			if (damageState == DamageState.Normal || damageState == DamageState.Shouha)
			{
				switch (Get_DamageState(endHp, Maxhp))
				{
				case DamageState.Tyuuha:
					num -= 10;
					break;
				case DamageState.Taiha:
					num -= 20;
					break;
				}
			}
			return SumLov(num);
		}

		public bool SumLovToMission(MissionResultKinds kind)
		{
			int num = 0;
			switch (kind)
			{
			case MissionResultKinds.SUCCESS:
				num += 8;
				break;
			case MissionResultKinds.FAILE:
				num -= 10;
				break;
			case MissionResultKinds.GREAT:
				num += 10;
				break;
			}
			return SumLov(num);
		}

		public bool SumLovToKaisouPowUp(int eatNum)
		{
			return SumLov(eatNum);
		}

		public bool SumLovToMarriage()
		{
			return SumLov(100);
		}

		public bool SumLovToRemodeling()
		{
			return SumLov(30);
		}

		public bool SumLovToCharge()
		{
			return SumLov(3);
		}

		public bool SumLovToUseFoodSupplyShip(int useType)
		{
			int value = 0;
			switch (useType)
			{
			case 1:
				value = 5;
				break;
			case 2:
				value = 4;
				break;
			case 3:
				value = 10;
				break;
			}
			return SumLov(value);
		}

		private bool SumLov(int value)
		{
			if (value < 0)
			{
				return subLov(value);
			}
			return addLov(value);
		}

		private bool subLov(int value)
		{
			if (value == 0)
			{
				return false;
			}
			if (Lov <= 0)
			{
				return false;
			}
			Lov += value;
			if (Lov < 0)
			{
				Lov = 0;
			}
			return true;
		}

		private bool addLov(int value)
		{
			if (value == 0)
			{
				return false;
			}
			if (Lov >= 999)
			{
				return false;
			}
			Lov += value;
			if (Lov > 999)
			{
				Lov = 999;
			}
			return true;
		}

		protected override void setProperty(XElement element)
		{
			Mem_shipBase mem_shipBase = new Mem_shipBase();
			mem_shipBase.setProperty(element);
			Set_ShipParam(mem_shipBase, Mst_DataManager.Instance.Mst_ship[mem_shipBase.Ship_id], enemy_flag: false);
		}
	}
}
