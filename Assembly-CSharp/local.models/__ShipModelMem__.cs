using Common.Enum;
using Common.Struct;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public abstract class __ShipModelMem__ : ShipModelMst, IShipModel
	{
		protected Mem_ship _mem_data;

		public override int ShipType => _mem_data.Stype;

		public override int KaryokuMax
		{
			get
			{
				int num = _mem_data.Houg - _mem_data.Kyouka[Mem_ship.enumKyoukaIdx.Houg];
				int num2 = _mst_data.Houg_max - _mst_data.Houg;
				return num + num2;
			}
		}

		public override int RaisouMax
		{
			get
			{
				int num = _mem_data.Raig - _mem_data.Kyouka[Mem_ship.enumKyoukaIdx.Raig];
				int num2 = _mst_data.Raig_max - _mst_data.Raig;
				return num + num2;
			}
		}

		public override int TaikuMax
		{
			get
			{
				int num = _mem_data.Taiku - _mem_data.Kyouka[Mem_ship.enumKyoukaIdx.Tyku];
				int num2 = _mst_data.Tyku_max - _mst_data.Tyku;
				return num + num2;
			}
		}

		public override int SoukouMax
		{
			get
			{
				int num = _mem_data.Soukou - _mem_data.Kyouka[Mem_ship.enumKyoukaIdx.Souk];
				int num2 = _mst_data.Souk_max - _mst_data.Souk;
				return num + num2;
			}
		}

		public override int KaihiMax
		{
			get
			{
				int num = _mem_data.Kaihi - _mem_data.Kyouka[Mem_ship.enumKyoukaIdx.Kaihi];
				int num2 = _mst_data.Kaih_max - _mst_data.Kaih;
				return num + num2;
			}
		}

		public override int TaisenMax
		{
			get
			{
				int num = _mem_data.Taisen - _mem_data.Kyouka[Mem_ship.enumKyoukaIdx.Taisen];
				int num2 = _mst_data.Tais_max - _mst_data.Tais;
				return num + num2;
			}
		}

		public override int LuckyMax
		{
			get
			{
				int num = _mem_data.Luck - _mem_data.Kyouka[Mem_ship.enumKyoukaIdx.Luck];
				int num2 = _mst_data.Luck_max - _mst_data.Luck;
				return num + num2;
			}
		}

		public override int Leng => _mem_data.Leng;

		public override int SlotCount => _mem_data.Slotnum;

		public int MemId => _mem_data.Rid;

		public int GetNo => _mem_data.GetNo;

		public int SortNo => _mem_data.Sortno;

		public int Level => _mem_data.Level;

		public int NowHp => _mem_data.Nowhp;

		public int MaxHp => _mem_data.Maxhp;

		public int Srate => _mem_data.Srate;

		public override int Karyoku => _mem_data.Houg;

		public override int Raisou => _mem_data.Raig;

		public override int Taiku => _mem_data.Taiku;

		public override int Soukou => _mem_data.Soukou;

		public override int Kaihi => _mem_data.Kaihi;

		public override int Taisen => _mem_data.Taisen;

		public int Sakuteki => _mem_data.Sakuteki;

		public override int Lucky => _mem_data.Luck;

		public int[] Tousai => _mem_data.Onslot.GetRange(0, SlotCount).ToArray();

		public int Fuel => _mem_data.Fuel;

		public int Ammo => _mem_data.Bull;

		public double FuelRate => (double)Fuel * 100.0 / (double)base.FuelMax;

		public double AmmoRate => (double)Ammo * 100.0 / (double)base.AmmoMax;

		public int Exp => _mem_data.Exp;

		public int Exp_Next => _mem_data.Exp_next;

		public int Exp_Percentage => _mem_data.Exp_rate;

		public double TaikyuRate => (MaxHp != 0) ? _mem_data.Damage_Rate : 0;

		public DamageState DamageStatus => _mem_data.Get_DamageState();

		public int Condition => _mem_data.Cond;

		public FatigueState ConditionState => _mem_data.Get_FatigueState();

		public int Lov => _mem_data.Lov;

		public int BlingStartTurn => _mem_data.Bling_start;

		public int BlingEndTurn => _mem_data.Bling_end;

		public int BlingRemainingTurns => _mem_data.GetBlingTurn();

		public int AreaIdBeforeBlingWait => IsBlingWait() ? _mem_data.BlingWaitArea : 0;

		public string ShortName => $"{base.Name}(mst:{base.MstId} mem:{MemId})";

		public __ShipModelMem__(Mem_ship mem_ship)
		{
			SetMemData(mem_ship);
		}

		public __ShipModelMem__(int memID)
		{
			Api_Result<Dictionary<int, Mem_ship>> api_Result = new Api_get_Member().Ship(new List<int>
			{
				memID
			});
			_mem_data = api_Result.data[memID];
			int ship_id = _mem_data.Ship_id;
			_mst_data = Mst_DataManager.Instance.Mst_ship[ship_id];
		}

		public bool LovAction(int type, int voiceID)
		{
			switch (type)
			{
			case 0:
			{
				TouchLovInfo touchInfo2 = new TouchLovInfo(voiceID, backTouch: false);
				return _mem_data.SumLov(ref touchInfo2);
			}
			case 1:
			{
				TouchLovInfo touchInfo = new TouchLovInfo(voiceID, backTouch: true);
				return _mem_data.SumLov(ref touchInfo);
			}
			default:
				return false;
			}
		}

		public bool IsBling()
		{
			return _mem_data.IsBlingShip();
		}

		public bool IsBlingWait()
		{
			return _mem_data.IsBlingWait();
		}

		public bool IsBlingWaitFromDeck()
		{
			return _mem_data.IsBlingWait() && _mem_data.BlingType == Mem_ship.BlingKind.WaitDeck;
		}

		public bool IsBlingWaitFromEscortDeck()
		{
			return _mem_data.IsBlingWait() && _mem_data.BlingType == Mem_ship.BlingKind.WaitEscort;
		}

		public bool IsTettaiBling()
		{
			return _mem_data.IsPortBack();
		}

		public void SetMemData(Mem_ship mem_ship)
		{
			_mem_data = mem_ship;
			int ship_id = _mem_data.Ship_id;
			_mst_data = Mst_DataManager.Instance.Mst_ship[ship_id];
		}

		public bool IsDamaged()
		{
			DamageState damageState = _mem_data.Get_DamageState();
			return damageState == DamageState.Tyuuha || damageState == DamageState.Taiha;
		}

		public bool IsTaiha()
		{
			return _mem_data.Get_DamageState() == DamageState.Taiha;
		}

		public bool IsMarriage()
		{
			return Level >= 100;
		}

		public bool IsLocked()
		{
			return _mem_data.Locked == 1;
		}

		public bool IsEscaped()
		{
			return _mem_data.Escape_sts;
		}

		public string ToString(string slot_string)
		{
			string str = ShortName + "\n";
			str += slot_string;
			string text = str;
			str = text + "Lv" + Level + ((!IsMarriage()) ? string.Empty : "(結婚済)") + "    HP:" + NowHp + "/" + MaxHp + ((!IsDamaged()) ? string.Empty : "(中破絵)") + "    Exp" + Exp + "/" + Exp_Next + "(" + Exp_Percentage + "%)    艦星数:" + Srate + "    火力:" + Karyoku + "    雷装:" + Raisou + "    装甲:" + Soukou + "    回避:" + Kaihi + "    対潜:" + Taisen + "    索的:" + Sakuteki + "    運  :" + Lucky + "    対空:" + Taiku + "    搭載:" + base.TousaiMaxAll + "    速力:" + base.Soku + "    射程:" + Leng + "    Lov値:" + Lov + "\n";
			str += "装備可能なカテゴリ:";
			foreach (SlotitemCategory item in GetEquipCategory())
			{
				str = str + " " + item;
			}
			str += "\n";
			return str + $"立ち絵のID:{GetGraphicsMstId()}";
		}

		public void AddExp(int exp)
		{
			new Debug_Mod().ShipAddExp(new List<Mem_ship>
			{
				_mem_data
			}, new List<int>
			{
				exp
			});
		}

		int IShipModel.MstId
		{
            get { return base.MstId;  }
			
		}

		string IShipModel.ShipTypeName
		{
            get { return base.ShipTypeName;  }
			
		}

		string IShipModel.Name
		{
            get { return base.Name; }
			
		}
	}
}
