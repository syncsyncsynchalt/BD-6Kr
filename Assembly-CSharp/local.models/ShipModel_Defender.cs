using Common.Enum;
using local.utils;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class ShipModel_Defender : ShipModel_Battle
	{
		private bool _set_damage;

		private bool _rengeki;

		private int _hp_before;

		private int _hp_pre_after;

		private int _hp_after;

		private int _hp_after_recovery;

		private List<SlotitemModel_Battle> _slotitems_before;

		private SlotitemModel_Battle _slotitemex_before;

		private List<SlotitemModel_Battle> _slotitems_after_recovery;

		private SlotitemModel_Battle _slotitemex_after_recovery;

		public int HpBefore => _GetHp(_hp_before);

		public int HpPreAfter => _GetHp(_hp_pre_after);

		public int HpAfter => _GetHp(_hp_after);

		public int HpAfterRecovery => _GetHp(_hp_after_recovery);

		public DamageState_Battle DmgStateBefore => _GetDmgState(_hp_before);

		public DamageState_Battle DmgStatePreAfter => _GetDmgState(_hp_pre_after);

		public DamageState_Battle DmgStateAfter => _GetDmgState(_hp_after);

		public DamageState_Battle DmgStateAfterRecovery => _GetDmgState(_hp_after_recovery);

		public DamagedStates DamageEventPreAfter => _GetDamageEvent1(DmgStateBefore, DmgStatePreAfter);

		public DamagedStates DamageEventAfter => _GetDamageEvent(DmgStateBefore, DmgStatePreAfter, DmgStateAfter);

		public bool DamagedFlgBefore => _GetDamagedFlg(DmgStateBefore);

		public bool DamagedFlgPreAfter => _GetDamagedFlg(DmgStatePreAfter);

		public bool DamagedFlgAfter => _GetDamagedFlg(DmgStateAfter);

		public bool DamagedFlgAfterRecovery => _GetDamagedFlg(DmgStateAfterRecovery);

		public List<SlotitemModel_Battle> SlotitemListBefore => _slotitems_before.GetRange(0, _slotitems_before.Count);

		public SlotitemModel_Battle SlotitemExBefore => _slotitemex_before;

		public List<SlotitemModel_Battle> SlotitemListAfterRecovery
		{
			get
			{
				if (_slotitems_after_recovery == null)
				{
					return SlotitemListBefore;
				}
				return _slotitems_after_recovery.GetRange(0, _slotitems_after_recovery.Count);
			}
		}

		public SlotitemModel_Battle SlotitemExAfterRecovery => _slotitemex_after_recovery;

		public ShipModel_Defender(Mst_ship mst_data, __ShipModel_Battle_BaseData__ baseData, int hp, List<SlotitemModel_Battle> slotitems, SlotitemModel_Battle slotitemex)
		{
			_mst_data = mst_data;
			_base_data = baseData;
			_hp_before = hp;
			_slotitems_before = slotitems;
			_slotitemex_before = slotitemex;
			_hp_after = (_hp_pre_after = (_hp_after_recovery = _hp_before));
		}

		public bool HasRecoveryEvent()
		{
			return DamageEventAfter == DamagedStates.Youin || DamageEventAfter == DamagedStates.Megami;
		}

		public bool HasRecoverYouin()
		{
			if (SlotitemExBefore != null && SlotitemExBefore.MstId == 42)
			{
				return true;
			}
			SlotitemModel_Battle slotitemModel_Battle = _slotitems_before.Find((SlotitemModel_Battle item) => item != null && item.MstId == 42);
			return slotitemModel_Battle != null;
		}

		public bool HasRecoverMegami()
		{
			if (SlotitemExBefore != null && SlotitemExBefore.MstId == 43)
			{
				return true;
			}
			SlotitemModel_Battle slotitemModel_Battle = _slotitems_before.Find((SlotitemModel_Battle item) => item != null && item.MstId == 43);
			return slotitemModel_Battle != null;
		}

		public void SetDamage(int damage)
		{
			_set_damage = true;
			_rengeki = false;
			_hp_pre_after = _hp_before;
			_hp_after = _hp_before - damage;
			_SetHpAfterRecovery();
		}

		public void SetDamage(int damage1, int damage2)
		{
			_set_damage = true;
			_rengeki = true;
			_hp_pre_after = _hp_before - damage1;
			_hp_after = _hp_pre_after - damage2;
			_SetHpAfterRecovery();
		}

		private void _SetHpAfterRecovery()
		{
			_slotitems_after_recovery = null;
			_slotitemex_after_recovery = null;
			if (DamageEventAfter == DamagedStates.Youin)
			{
				if (_slotitemex_before != null && _slotitemex_before.MstId == 42)
				{
					_slotitemex_after_recovery = null;
					if (base.Index == 0)
					{
						_hp_after_recovery = (int)Math.Floor((double)MaxHp * 0.5);
					}
					else
					{
						_hp_after_recovery = (int)Math.Floor((double)MaxHp * 0.20000000298023224);
					}
					return;
				}
				_slotitems_after_recovery = new List<SlotitemModel_Battle>();
				bool flag = false;
				for (int i = 0; i < _slotitems_before.Count; i++)
				{
					SlotitemModel_Battle slotitemModel_Battle = _slotitems_before[i];
					if (!flag && slotitemModel_Battle != null && slotitemModel_Battle.MstId == 42)
					{
						if (base.Index == 0)
						{
							_hp_after_recovery = (int)Math.Floor((double)MaxHp * 0.5);
						}
						else
						{
							_hp_after_recovery = (int)Math.Floor((double)MaxHp * 0.20000000298023224);
						}
						flag = true;
					}
					else
					{
						_slotitems_after_recovery.Add(slotitemModel_Battle);
					}
				}
				while (_slotitems_after_recovery.Count < _slotitems_before.Count)
				{
					_slotitems_after_recovery.Add(null);
				}
				_slotitemex_after_recovery = _slotitemex_before;
			}
			else if (DamageEventAfter == DamagedStates.Megami)
			{
				if (_slotitemex_before != null && _slotitemex_before.MstId == 43)
				{
					_slotitemex_after_recovery = null;
					_hp_after_recovery = MaxHp;
					return;
				}
				_slotitems_after_recovery = new List<SlotitemModel_Battle>();
				bool flag2 = false;
				for (int j = 0; j < _slotitems_before.Count; j++)
				{
					SlotitemModel_Battle slotitemModel_Battle2 = _slotitems_before[j];
					if (!flag2 && slotitemModel_Battle2 != null && slotitemModel_Battle2.MstId == 43)
					{
						_hp_after_recovery = MaxHp;
						flag2 = true;
					}
					else
					{
						_slotitems_after_recovery.Add(slotitemModel_Battle2);
					}
				}
				while (_slotitems_after_recovery.Count < _slotitems_before.Count)
				{
					_slotitems_after_recovery.Add(null);
				}
				_slotitemex_after_recovery = _slotitemex_before;
			}
			else
			{
				_hp_after_recovery = _hp_after;
				_slotitemex_after_recovery = _slotitemex_before;
			}
		}

		private DamagedStates _GetDamageEvent1(DamageState_Battle before, DamageState_Battle pre_after)
		{
			DamagedStates damagedStates = __GetDamageEvent(before, pre_after);
			if (damagedStates == DamagedStates.Shouha)
			{
				return damagedStates;
			}
			return DamagedStates.None;
		}

		private DamagedStates _GetDamageEvent(DamageState_Battle before, DamageState_Battle pre_after, DamageState_Battle after)
		{
			DamagedStates damagedStates = __GetDamageEvent(before, after);
			if (_rengeki && damagedStates == DamagedStates.Shouha && pre_after == DamageState_Battle.Shouha)
			{
				return DamagedStates.None;
			}
			return damagedStates;
		}

		private DamagedStates __GetDamageEvent(DamageState_Battle before, DamageState_Battle after)
		{
			if (before != after)
			{
				switch (after)
				{
				case DamageState_Battle.Shouha:
					return DamagedStates.Shouha;
				case DamageState_Battle.Tyuuha:
					return DamagedStates.Tyuuha;
				case DamageState_Battle.Taiha:
					return DamagedStates.Taiha;
				case DamageState_Battle.Gekichin:
					switch (Utils.__HasRecoveryItem__(SlotitemListBefore, SlotitemExBefore))
					{
					case ShipRecoveryType.None:
						return DamagedStates.Gekichin;
					case ShipRecoveryType.Personnel:
						return DamagedStates.Youin;
					case ShipRecoveryType.Goddes:
						return DamagedStates.Megami;
					}
					break;
				}
			}
			return DamagedStates.None;
		}

		public override string ToString()
		{
			string str = $"{base.Name}(mstId:{base.MstId})[{HpBefore}/{MaxHp}({DmgStateBefore})";
			if (!_set_damage)
			{
				return str + "]";
			}
			if (_rengeki)
			{
				str += $" => {HpPreAfter}/{MaxHp}({DmgStatePreAfter}";
				DamagedStates damageEventPreAfter = DamageEventPreAfter;
				str = ((damageEventPreAfter == DamagedStates.None) ? (str + ")") : (str + $"・{damageEventPreAfter})"));
				str += $" => {HpAfter}/{MaxHp}({DmgStateAfter}";
				damageEventPreAfter = DamageEventAfter;
				str = ((damageEventPreAfter == DamagedStates.None) ? (str + ")") : (str + $"・{damageEventPreAfter})"));
			}
			else
			{
				str += $" => {HpAfter}/{MaxHp}({DmgStateAfter}";
				DamagedStates damageEventAfter = DamageEventAfter;
				str = ((damageEventAfter == DamagedStates.None) ? (str + ")") : (str + $"・{damageEventAfter})"));
			}
			if (DamageEventAfter == DamagedStates.Youin)
			{
				return str + $" (Youin)=> {HpAfterRecovery}/{MaxHp}({DmgStateAfterRecovery})]";
			}
			if (DamageEventAfter == DamagedStates.Megami)
			{
				return str + $" (Megami)=> {HpAfterRecovery}/{MaxHp}({DmgStateAfterRecovery})]";
			}
			return str + "]";
		}
	}
}
