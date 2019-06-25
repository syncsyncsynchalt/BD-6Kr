using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class ShipModel_BattleAll : ShipModel_Battle
	{
		private List<ShipModel_Battle> _record;

		public int HpStart => _GetHp(_base_data.Fmt.NowHp);

		public int HpPhaseStart => ((ShipModel_BattleStart)_record.FindLast((ShipModel_Battle s) => s is ShipModel_BattleStart)).Hp;

		public int HpEnd
		{
			get
			{
				List<ShipModel_Defender> list = _record.FindAll((ShipModel_Battle s) => s is ShipModel_Defender).ConvertAll((ShipModel_Battle s) => (ShipModel_Defender)s);
				if (list.Count == 0)
				{
					return HpStart;
				}
				return list[list.Count - 1].HpAfterRecovery;
			}
		}

		public DamageState_Battle DmgStateStart => _GetDmgState(HpStart);

		public DamageState_Battle DmgStatePhaseStart => _GetDmgState(HpPhaseStart);

		public DamageState_Battle DmgStateEnd => _GetDmgState(HpEnd);

		public bool DamagedFlgStart => _GetDamagedFlg(DmgStateStart);

		public bool DamagedFlgPhaseStart => _GetDamagedFlg(DmgStatePhaseStart);

		public bool DamagedFlgEnd => _GetDamagedFlg(DmgStateEnd);

		public List<SlotitemModel_Battle> SlotitemListStart => ((ShipModel_BattleStart)_record.Find((ShipModel_Battle s) => s is ShipModel_BattleStart)).SlotitemList;

		public SlotitemModel_Battle SlotitemExStart => ((ShipModel_BattleStart)_record.Find((ShipModel_Battle s) => s is ShipModel_BattleStart)).SlotitemEx;

		public List<SlotitemModel_Battle> SlotitemListPhaseStart => ((ShipModel_BattleStart)_record.FindLast((ShipModel_Battle s) => s is ShipModel_BattleStart)).SlotitemList;

		public SlotitemModel_Battle SlotitemExPhaseStart => ((ShipModel_BattleStart)_record.FindLast((ShipModel_Battle s) => s is ShipModel_BattleStart)).SlotitemEx;

		public List<SlotitemModel_Battle> SlotitemListEnd
		{
			get
			{
				ShipModel_Battle shipModel_Battle = _record[_record.Count - 1];
				if (shipModel_Battle is ShipModel_BattleStart || shipModel_Battle is ShipModel_Attacker)
				{
					return ((__ShipModel_Battle__)shipModel_Battle).SlotitemList;
				}
				if (shipModel_Battle is ShipModel_Defender)
				{
					return ((ShipModel_Defender)shipModel_Battle).SlotitemListAfterRecovery;
				}
				if (shipModel_Battle is ShipModel_Eater)
				{
					return ((ShipModel_Eater)shipModel_Battle).SlotitemListAfterRation;
				}
				return null;
			}
		}

		public SlotitemModel_Battle SlotitemExEnd
		{
			get
			{
				ShipModel_Battle shipModel_Battle = _record[_record.Count - 1];
				if (shipModel_Battle is ShipModel_BattleStart || shipModel_Battle is ShipModel_Attacker)
				{
					return ((__ShipModel_Battle__)shipModel_Battle).SlotitemEx;
				}
				if (shipModel_Battle is ShipModel_Defender)
				{
					return ((ShipModel_Defender)shipModel_Battle).SlotitemExAfterRecovery;
				}
				if (shipModel_Battle is ShipModel_Eater)
				{
					return ((ShipModel_Eater)shipModel_Battle).SlotitemExAfterRation;
				}
				return null;
			}
		}

		public ShipModel_BattleAll(BattleShipFmt fmt, int index, bool is_friend, bool practice)
		{
			Mst_DataManager.Instance.Mst_ship.TryGetValue(fmt.ShipId, out _mst_data);
			_base_data = new __ShipModel_Battle_BaseData__();
			_base_data.IsPractice = practice;
			_base_data.IsFriend = is_friend;
			_base_data.Index = index;
			_base_data.Fmt = fmt;
			_Init();
		}

		public ShipModel_BattleAll(Mst_ship mst_ship, __ShipModel_Battle_BaseData__ baseData)
		{
			_mst_data = mst_ship;
			_base_data = baseData;
			_Init();
		}

		public bool HasRecoverYouin()
		{
			if (SlotitemExEnd != null && SlotitemExEnd.MstId == 42)
			{
				return true;
			}
			SlotitemModel_Battle slotitemModel_Battle = SlotitemListEnd.Find((SlotitemModel_Battle item) => item != null && item.MstId == 42);
			return slotitemModel_Battle != null;
		}

		public bool HasRecoverMegami()
		{
			if (SlotitemExEnd != null && SlotitemExEnd.MstId == 43)
			{
				return true;
			}
			SlotitemModel_Battle slotitemModel_Battle = SlotitemListEnd.Find((SlotitemModel_Battle item) => item != null && item.MstId == 43);
			return slotitemModel_Battle != null;
		}

		public ShipRecoveryType IsUseRecoverySlotitemAtFirstCombat()
		{
			int num = _record.FindLastIndex((ShipModel_Battle s) => s is ShipModel_BattleStart);
			if (num > 0)
			{
				List<ShipModel_Battle> range = _record.GetRange(0, num);
				return _IsUseRecoverySlotitem(range);
			}
			return _IsUseRecoverySlotitem(_record);
		}

		public ShipRecoveryType IsUseRecoverySlotitemAtSecondCombat()
		{
			int num = _record.FindLastIndex((ShipModel_Battle s) => s is ShipModel_BattleStart);
			if (num > 0)
			{
				List<ShipModel_Battle> range = _record.GetRange(num, _record.Count - num);
				return _IsUseRecoverySlotitem(range);
			}
			return ShipRecoveryType.None;
		}

		public ShipRecoveryType IsUseRecoverySlotitem()
		{
			return _IsUseRecoverySlotitem(_record);
		}

		public void __CreateStarter__()
		{
			ShipModel_BattleStart item = new ShipModel_BattleStart(_mst_data, _base_data, HpEnd, SlotitemListEnd, SlotitemExEnd);
			_record.Add(item);
		}

		public ShipModel_Attacker __CreateAttacker__()
		{
			ShipModel_Attacker shipModel_Attacker = new ShipModel_Attacker(_mst_data, _base_data, HpEnd, SlotitemListEnd, SlotitemExEnd);
			_record.Add(shipModel_Attacker);
			return shipModel_Attacker;
		}

		public ShipModel_Defender __CreateDefender__()
		{
			ShipModel_Defender shipModel_Defender = new ShipModel_Defender(_mst_data, _base_data, HpEnd, SlotitemListEnd, SlotitemExEnd);
			_record.Add(shipModel_Defender);
			return shipModel_Defender;
		}

		public ShipModel_Eater __CreateEater__()
		{
			ShipModel_Eater shipModel_Eater = new ShipModel_Eater(_mst_data, _base_data, HpEnd, SlotitemListEnd, SlotitemExEnd);
			_record.Add(shipModel_Eater);
			return shipModel_Eater;
		}

		public void __UpdateEscapeStatus__(bool value)
		{
			_base_data.Fmt.EscapeFlag = value;
		}

		private void _Init()
		{
			_record = new List<ShipModel_Battle>();
			List<SlotitemModel_Battle> list = new List<SlotitemModel_Battle>();
			for (int i = 0; i < _base_data.Fmt.Slot.Count; i++)
			{
				if (_base_data.Fmt.Slot[i] > 0)
				{
					list.Add(new SlotitemModel_Battle(_base_data.Fmt.Slot[i]));
				}
				else
				{
					list.Add(null);
				}
			}
			while (list.Count < SlotCount)
			{
				list.Add(null);
			}
			SlotitemModel_Battle slotitemex = null;
			if (HasSlotEx())
			{
				int exSlot = _base_data.Fmt.ExSlot;
				if (Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(exSlot, out Mst_slotitem value))
				{
					slotitemex = new SlotitemModel_Battle(value);
				}
			}
			ShipModel_BattleStart item = new ShipModel_BattleStart(_mst_data, _base_data, HpEnd, list, slotitemex);
			_record.Add(item);
		}

		private ShipRecoveryType _IsUseRecoverySlotitem(List<ShipModel_Battle> record)
		{
			for (int i = 0; i < record.Count; i++)
			{
				if (_record[i] is ShipModel_Defender)
				{
					ShipModel_Defender shipModel_Defender = (ShipModel_Defender)record[i];
					if (shipModel_Defender.DamageEventAfter == DamagedStates.Youin)
					{
						return ShipRecoveryType.Personnel;
					}
					if (shipModel_Defender.DamageEventAfter == DamagedStates.Megami)
					{
						return ShipRecoveryType.Goddes;
					}
				}
			}
			return ShipRecoveryType.None;
		}

		public override string ToString()
		{
			return string.Format("{0}({6})(mstId:{1})[{2}/{3} => {4}/{3}({5}/{3})]", base.Name, base.MstId, HpPhaseStart, MaxHp, HpEnd, HpStart, base.Index);
		}
	}
}
