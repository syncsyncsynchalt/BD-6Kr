using Common.Enum;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class RaigekiDamageModel : DamageModelBase
	{
		protected List<ShipModel_Attacker> _attackers;

		public List<ShipModel_Attacker> Attackers => _attackers;

		public RaigekiDamageModel(ShipModel_BattleAll defender)
			: base(defender)
		{
			_attackers = new List<ShipModel_Attacker>();
		}

		public int GetDamage(int attacker_tmp_id)
		{
			int index = _GetAttackerIndex_(attacker_tmp_id);
			return _damages[index];
		}

		[Obsolete("GetDamage(int ship_tmp_id) を使用してください", false)]
		public int GetDamage(ShipModel_BattleAll attacker)
		{
			int index = __GetAttackerIndex(attacker.Index);
			return _damages[index];
		}

		public BattleHitStatus GetHitState(int attacker_tmp_id)
		{
			int index = _GetAttackerIndex_(attacker_tmp_id);
			return _hitstates[index];
		}

		[Obsolete("GetHitState(int attacker_tmp_id) を使用してください", false)]
		public BattleHitStatus GetHitState(ShipModel_BattleAll attacker)
		{
			int index = __GetAttackerIndex(attacker.Index);
			return _hitstates[index];
		}

		public bool GetProtectEffect(int attacker_tmp_id)
		{
			int index = _GetAttackerIndex_(attacker_tmp_id);
			return _dmgkind[index] == BattleDamageKinds.Rescue;
		}

		public int __AddData__(ShipModel_BattleAll attacker, int damage, BattleHitStatus hitstate, BattleDamageKinds dmgkind)
		{
			_attackers.Add(attacker.__CreateAttacker__());
			return _AddData(damage, hitstate, dmgkind);
		}

		public int __GetDamage__(int attacker_tmp_id)
		{
			int index = _GetAttackerIndex_(attacker_tmp_id);
			return _calc_damages[index];
		}

		private int _GetAttackerIndex_(int ship_tmp_id)
		{
			return _attackers.FindIndex((ShipModel_Attacker ship) => ship.TmpId == ship_tmp_id);
		}

		private int __GetAttackerIndex(int ship_index)
		{
			for (int i = 0; i < _attackers.Count; i++)
			{
				if (_attackers[i].Index == ship_index)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
