using Common.Enum;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public abstract class ShipModel_Battle : ShipModelMst, IShipModel
	{
		protected __ShipModel_Battle_BaseData__ _base_data;

		public virtual int TmpId => _base_data.Fmt.Id;

		public int Index => _base_data.Index;

		public virtual int Level => _base_data.Fmt.Level;

		public virtual int MaxHp => _base_data.Fmt.MaxHp;

		public virtual int ParamKaryoku => _base_data.Fmt.BattleParam.Houg;

		public virtual int ParamRaisou => _base_data.Fmt.BattleParam.Raig;

		public virtual int ParamTaiku => _base_data.Fmt.BattleParam.Taiku;

		public virtual int ParamSoukou => _base_data.Fmt.BattleParam.Soukou;

		public ShipModel_Battle()
		{
		}

		public bool IsFriend()
		{
			return _base_data.IsFriend;
		}

		public bool IsPractice()
		{
			return _base_data.IsPractice;
		}

		public bool IsSubMarine()
		{
			return Mst_DataManager.Instance.Mst_stype[ShipType].IsSubmarine();
		}

		public bool IsAircraftCarrier()
		{
			return Mst_DataManager.Instance.Mst_stype[ShipType].IsMother();
		}

		public virtual bool IsEscape()
		{
			return _base_data.Fmt.EscapeFlag;
		}

		public bool HasSlotEx()
		{
			return _base_data.Fmt.ExSlot >= 0;
		}

		protected int _GetHp(int hp)
		{
			if (_base_data.IsPractice)
			{
				return Math.Max(hp, 1);
			}
			return Math.Max(hp, 0);
		}

		protected DamageState_Battle _GetDmgState(int hp)
		{
			if (hp <= 0)
			{
				return DamageState_Battle.Gekichin;
			}
			switch (Mem_ship.Get_DamageState(hp, MaxHp))
			{
			case DamageState.Taiha:
				return DamageState_Battle.Taiha;
			case DamageState.Tyuuha:
				return DamageState_Battle.Tyuuha;
			case DamageState.Shouha:
				return DamageState_Battle.Shouha;
			default:
				return DamageState_Battle.Normal;
			}
		}

		protected bool _GetDamagedFlg(DamageState_Battle state)
		{
			return state == DamageState_Battle.Tyuuha || state == DamageState_Battle.Taiha || state == DamageState_Battle.Gekichin;
		}

		protected string _ToString(List<SlotitemModel_Battle> items, SlotitemModel_Battle item_ex)
		{
			string str = "(";
			for (int i = 0; i < items.Count; i++)
			{
				SlotitemModel_Battle item = items[i];
				str += _ToString(item);
			}
			str = ((!HasSlotEx()) ? (str + " [X]") : (str + " ex:" + _ToString(item_ex)));
			return str + ")";
		}

		protected string _ToString(SlotitemModel_Battle item)
		{
			if (item == null)
			{
				return "[-]";
			}
			return $"[{item.Name}({item.MstId})]";
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
