using Common.Enum;
using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class HougekiModel : DamageModelBase, IBattlePhase
	{
		private bool _is_night;

		private BattleAttackKind _attack_type;

		private ShipModel_Attacker _attacker;

		private List<SlotitemModel_Battle> _slots;

		public ShipModel_Attacker Attacker => _attacker;

		public BattleAttackKind AttackType => _attack_type;

		public HougekiModel(Hougeki<BattleAtackKinds_Day> data, Dictionary<int, ShipModel_BattleAll> ships)
			: base(ships[data.Target[0]])
		{
			_attack_type = _convertAttackTypeEnum(data.SpType);
			_initialize(data, ships);
			_SetDamageForDefender();
		}

		public HougekiModel(Hougeki<BattleAtackKinds_Night> data, Dictionary<int, ShipModel_BattleAll> ships)
			: base(ships[data.Target[0]])
		{
			_is_night = true;
			_attack_type = _convertAttackTypeEnum(data.SpType);
			_initialize(data, ships);
			_SetDamageForDefender();
		}

		public int GetDamage(int index)
		{
			return _damages[index];
		}

		public BattleHitStatus GetHitState(int index)
		{
			return _hitstates[index];
		}

		public SlotitemModel_Battle[] GetSlotitems()
		{
			return _slots.ToArray();
		}

		public SlotitemModel_Battle GetSlotitem()
		{
			return _slots[0];
		}

		public bool GetProtectEffect(int index)
		{
			return _dmgkind[index] == BattleDamageKinds.Rescue;
		}

		public override bool GetGurdEffect()
		{
			return GetHitState() != 0 && GetDamage() == 0;
		}

		public bool GetGurdEffect(int index)
		{
			return GetHitState(index) != 0 && GetDamage(index) == 0;
		}

		public bool GetRocketEffenct()
		{
			return _attack_type == BattleAttackKind.Normal && Attacker.HasRocket() && base.Defender.IsGroundFacility();
		}

		public bool GetMihariEffect()
		{
			return _is_night && (_attack_type == BattleAttackKind.Syu_Syu_Syu || _attack_type == BattleAttackKind.Syu_Syu_Fuku || _attack_type == BattleAttackKind.Syu_Rai || _attack_type == BattleAttackKind.Rai_Rai) && Attacker.HasMihari();
		}

		[Obsolete("GetProtectEffect を使用してください", false)]
		public bool IsShielded(int index)
		{
			return _dmgkind[index] == BattleDamageKinds.Rescue;
		}

		[Obsolete("GetProtectEffect を使用してください", false)]
		public bool IsShielded()
		{
			return IsShielded(0);
		}

		public bool IsNight()
		{
			return _is_night;
		}

		public List<ShipModel_Defender> GetDefenders(bool is_friend)
		{
			if (is_friend == base.Defender.IsFriend())
			{
				List<ShipModel_Defender> list = new List<ShipModel_Defender>();
				list.Add(base.Defender);
				return list;
			}
			return new List<ShipModel_Defender>();
		}

		public List<ShipModel_Defender> GetDefenders(bool is_friend, DamagedStates damage_event)
		{
			if (is_friend == base.Defender.IsFriend() && base.Defender.DamageEventAfter == damage_event)
			{
				List<ShipModel_Defender> list = new List<ShipModel_Defender>();
				list.Add(base.Defender);
				return list;
			}
			return new List<ShipModel_Defender>();
		}

		public List<ShipModel_Defender> GetGekichinShips()
		{
			if (base.Defender.DamageEventAfter == DamagedStates.Gekichin || base.Defender.DamageEventAfter == DamagedStates.Youin || base.Defender.DamageEventAfter == DamagedStates.Megami)
			{
				List<ShipModel_Defender> list = new List<ShipModel_Defender>();
				list.Add(base.Defender);
				return list;
			}
			return new List<ShipModel_Defender>();
		}

		public List<ShipModel_Defender> GetGekichinShips(bool is_friend)
		{
			if ((base.Defender.DamageEventAfter == DamagedStates.Gekichin || base.Defender.DamageEventAfter == DamagedStates.Youin || base.Defender.DamageEventAfter == DamagedStates.Megami) && base.Defender.IsFriend())
			{
				List<ShipModel_Defender> list = new List<ShipModel_Defender>();
				list.Add(base.Defender);
				return list;
			}
			return new List<ShipModel_Defender>();
		}

		public bool HasChuhaEvent()
		{
			return HasChuhaEvent(is_friend: true);
		}

		public bool HasChuhaEvent(bool is_friend)
		{
			return is_friend == base.Defender.IsFriend() && base.Defender.DamageEventAfter == DamagedStates.Tyuuha;
		}

		public bool HasTaihaEvent()
		{
			return HasTaihaEvent(is_friend: true);
		}

		public bool HasTaihaEvent(bool is_friend)
		{
			return is_friend == base.Defender.IsFriend() && base.Defender.DamageEventAfter == DamagedStates.Taiha;
		}

		public bool HasGekichinEvent()
		{
			return HasGekichinEvent(is_friend: true);
		}

		public bool HasGekichinEvent(bool is_friend)
		{
			return is_friend == base.Defender.IsFriend() && (base.Defender.DamageEventAfter == DamagedStates.Gekichin || base.Defender.DamageEventAfter == DamagedStates.Youin || base.Defender.DamageEventAfter == DamagedStates.Megami);
		}

		public bool HasRecoveryEvent()
		{
			return HasRecoveryEvent(is_friend: true);
		}

		public bool HasRecoveryEvent(bool is_friend)
		{
			return is_friend == base.Defender.IsFriend() && (base.Defender.DamageEventAfter == DamagedStates.Youin || base.Defender.DamageEventAfter == DamagedStates.Megami);
		}

		public int __GetDamage__(int index)
		{
			return _calc_damages[index];
		}

		private void _initialize<T>(Hougeki<T> data, Dictionary<int, ShipModel_BattleAll> ships) where T : IConvertible
		{
			_attacker = ships[data.Attacker].__CreateAttacker__();
			for (int i = 0; i < data.Target.Count; i++)
			{
				_AddData(data.Damage[i], data.Clitical[i], data.DamageKind[i]);
			}
			_slots = new List<SlotitemModel_Battle>();
			if (data.Slot_List != null)
			{
				for (int j = 0; j < data.Slot_List.Count; j++)
				{
					int num = data.Slot_List[j];
					if (num == 0)
					{
						_slots.Add(null);
					}
					else
					{
						_slots.Add(new SlotitemModel_Battle(num));
					}
				}
			}
			while (_slots.Count < 3)
			{
				_slots.Add(null);
			}
			if (IsNight() && Attacker.IsAircraftCarrier() && Attacker.Yomi != "グラ\u30fcフ・ツェッペリン")
			{
				_attack_type = BattleAttackKind.AirAttack;
			}
			if (AttackType == BattleAttackKind.Normal)
			{
				_setAttackSubType();
			}
			else if (AttackType == BattleAttackKind.AirAttack)
			{
				List<SlotitemModel_Battle> slotitemList = Attacker.SlotitemList;
				List<int> enable_type3;
				if (base.Defender.IsSubMarine())
				{
					enable_type3 = new List<int>
					{
						7,
						8,
						11,
						25,
						26,
						41
					};
					_slots[0] = slotitemList.Find((SlotitemModel_Battle slot) => enable_type3.IndexOf(slot.Type3) >= 0 && slot.Taisen > 0);
				}
				else
				{
					enable_type3 = new List<int>
					{
						7,
						8
					};
					_slots[0] = slotitemList.Find((SlotitemModel_Battle slot) => enable_type3.IndexOf(slot.Type3) >= 0 && (slot.Bakugeki > 0 || slot.Raigeki > 0));
				}
			}
		}

		private void _setAttackSubType()
		{
			int shipType = Attacker.ShipType;
			SlotitemModel_Battle slotitem = GetSlotitem();
			if (slotitem != null && (slotitem.Type3 == 5 || slotitem.Type3 == 32))
			{
				_attack_type = BattleAttackKind.Gyorai;
			}
			else if (slotitem != null && slotitem.Type3 != 1 && slotitem.Type3 != 2 && slotitem.Type3 != 3 && slotitem.Type3 != 4 && slotitem.Type3 != 5 && slotitem.Type3 != 32)
			{
				_slots = new List<SlotitemModel_Battle>
				{
					null,
					null,
					null
				};
			}
		}

		private BattleAttackKind _convertAttackTypeEnum(BattleAtackKinds_Day kind)
		{
			return (BattleAttackKind)kind;
		}

		private BattleAttackKind _convertAttackTypeEnum(BattleAtackKinds_Night kind)
		{
			string value = kind.ToString();
			return (BattleAttackKind)(int)Enum.Parse(typeof(BattleAttackKind), value);
		}

		private void _SetDamageForDefender()
		{
			if (AttackType == BattleAttackKind.Renzoku)
			{
				_InitializeRengeki();
			}
			else
			{
				_Initialize();
			}
		}

		public override string ToString()
		{
			string str = $"{Attacker.Name}({Attacker.Index})の攻撃 {AttackType}";
			if (AttackType == BattleAttackKind.Normal || AttackType == BattleAttackKind.AirAttack || AttackType == BattleAttackKind.Bakurai)
			{
				SlotitemModel_Battle[] slotitems = GetSlotitems();
				str += $"{base.Defender.Name}({base.Defender.Index})は";
				str += string.Format("{0}(c:{2})のダメ\u30fcジ({1})", GetDamage(0), GetHitState(0), __GetDamage__(0));
				str += string.Format("{0}", (!GetRocketEffenct()) ? string.Empty : "[対地演出]");
				str += string.Format("{0}", (!GetProtectEffect(0)) ? string.Empty : "[かばう]");
				str += string.Format("{0}", (!GetGurdEffect(0)) ? string.Empty : "[ガ\u30fcド]");
				for (int i = 0; i < slotitems.Length; i++)
				{
					str = ((slotitems[i] == null) ? (str + $"[--]") : (str + $"{slotitems[i]}"));
				}
			}
			else if (AttackType == BattleAttackKind.Renzoku)
			{
				SlotitemModel_Battle[] slotitems2 = GetSlotitems();
				str += $"\"{base.Defender.Name}({base.Defender.Index})は";
				str += $"{slotitems2[0]},{slotitems2[1]}で";
				str += string.Format("{0}({2})のダメ\u30fcジ({1})", GetDamage(0), GetHitState(0), __GetDamage__(0));
				str += string.Format("{0}", (!GetProtectEffect(0)) ? string.Empty : "[かばう]");
				str += string.Format("{0}\" ", (!GetGurdEffect(0)) ? string.Empty : "[ガ\u30fcド]");
				str += string.Format("と{0}({2})のダメ\u30fcジ({1})", GetDamage(1), GetHitState(1), __GetDamage__(1));
				str += string.Format("{0}\" ", (!GetProtectEffect(1)) ? string.Empty : "[かばう]");
				str += string.Format("{0}\" ", (!GetGurdEffect(1)) ? string.Empty : "[ガ\u30fcド]");
			}
			else if (AttackType == BattleAttackKind.Sp1 || AttackType == BattleAttackKind.Sp2 || AttackType == BattleAttackKind.Sp3 || AttackType == BattleAttackKind.Sp4)
			{
				SlotitemModel_Battle[] slotitems3 = GetSlotitems();
				str += $"\"{base.Defender.Name}({base.Defender.Index})は";
				str += $"{slotitems3[0]},{slotitems3[1]},{slotitems3[2]}で";
				str += string.Format("{0}({2})のダメ\u30fcジ({1})", GetDamage(0), GetHitState(0), __GetDamage__(0));
				str += string.Format("{0}\" ", (!GetProtectEffect(0)) ? string.Empty : "[かばう]");
				str += string.Format("{0}\" ", (!GetGurdEffect(0)) ? string.Empty : "[ガ\u30fcド]");
			}
			else if (AttackType == BattleAttackKind.Syu_Rai || AttackType == BattleAttackKind.Rai_Rai)
			{
				SlotitemModel_Battle[] slotitems4 = GetSlotitems();
				str += $"\"{base.Defender.Name}({base.Defender.Index})は";
				str += $"{slotitems4[0]},{slotitems4[1]}で";
				str += string.Format("{0}({2})のダメ\u30fcジ({1})", GetDamage(0), GetHitState(0), __GetDamage__(0));
				str += string.Format("{0}\" ", (!GetProtectEffect(0)) ? string.Empty : "[かばう]");
				str += string.Format("{0}\" ", (!GetGurdEffect(0)) ? string.Empty : "[ガ\u30fcド]");
			}
			else if (AttackType == BattleAttackKind.Syu_Syu_Syu || AttackType == BattleAttackKind.Syu_Syu_Fuku)
			{
				SlotitemModel_Battle[] slotitems5 = GetSlotitems();
				str += $"\"{base.Defender.Name}({base.Defender.Index})は";
				str += $"{slotitems5[0]},{slotitems5[1]},{slotitems5[2]}で";
				str += string.Format("{0}({2})のダメ\u30fcジ({1})", GetDamage(0), GetHitState(0), __GetDamage__(0));
				str += string.Format("{0}\" ", (!GetProtectEffect(0)) ? string.Empty : "[かばう]");
				str += string.Format("{0}\" ", (!GetGurdEffect(0)) ? string.Empty : "[ガ\u30fcド]");
			}
			str += "\n";
			str += $"Attacker:{Attacker}\n";
			return str + $"Defender:{base.Defender}";
		}
	}
}
