using Common.Enum;
using System.Collections.Generic;

namespace local.models.battle
{
	public abstract class DamageModelBase
	{
		protected bool _initialized;

		protected ShipModel_Defender _defender;

		protected List<int> _calc_damages;

		protected List<int> _damages;

		protected List<BattleHitStatus> _hitstates;

		protected List<BattleDamageKinds> _dmgkind;

		public ShipModel_Defender Defender => _defender;

		public DamageModelBase(ShipModel_BattleAll defender)
		{
			_defender = defender.__CreateDefender__();
			_calc_damages = new List<int>();
			_damages = new List<int>();
			_hitstates = new List<BattleHitStatus>();
			_dmgkind = new List<BattleDamageKinds>();
		}

		public int GetDamage()
		{
			int num = 0;
			for (int i = 0; i < _damages.Count; i++)
			{
				num += _damages[i];
			}
			return num;
		}

		public BattleHitStatus GetHitState()
		{
			if (_hitstates.Count == 0)
			{
				return BattleHitStatus.Miss;
			}
			BattleHitStatus battleHitStatus = _hitstates[0];
			for (int i = 1; i < _hitstates.Count; i++)
			{
				if (_hitstates[i] == BattleHitStatus.Clitical)
				{
					return BattleHitStatus.Clitical;
				}
				if (_hitstates[i] == BattleHitStatus.Normal && battleHitStatus == BattleHitStatus.Miss)
				{
					battleHitStatus = BattleHitStatus.Normal;
				}
			}
			return battleHitStatus;
		}

		public bool GetProtectEffect()
		{
			for (int i = 0; i < _dmgkind.Count; i++)
			{
				if (_dmgkind[i] == BattleDamageKinds.Rescue)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool GetGurdEffect()
		{
			return false;
		}

		public int __GetDamage__()
		{
			int num = 0;
			for (int i = 0; i < _calc_damages.Count; i++)
			{
				num += _calc_damages[i];
			}
			return num;
		}

		public void __CalcDamage__()
		{
			_Initialize();
		}

		protected int _AddData(int damage, BattleHitStatus hitstate, BattleDamageKinds dmgkind)
		{
			_calc_damages.Add(damage);
			_damages.Add(damage);
			_hitstates.Add(hitstate);
			_dmgkind.Add(dmgkind);
			return _damages.Count;
		}

		protected void _Initialize()
		{
			if (!_initialized)
			{
				if (Defender.IsPractice())
				{
					_AdjustDamage();
				}
				Defender.SetDamage(GetDamage());
				_initialized = true;
			}
		}

		protected void _InitializeRengeki()
		{
			if (!_initialized)
			{
				if (Defender.IsPractice())
				{
					_AdjustDamage();
				}
				Defender.SetDamage(_damages[0], _damages[1]);
				_initialized = true;
			}
		}

		private void _AdjustDamage()
		{
			int damage = GetDamage();
			if (Defender.HpBefore > damage)
			{
				return;
			}
			int num = damage - (Defender.HpBefore - 1);
			for (int i = 0; i < _damages.Count; i++)
			{
				if (_damages[i] > 0)
				{
					_damages[i] -= 1;
					num--;
				}
				if (i == _damages.Count - 1)
				{
					i = -1;
				}
				if (num == 0)
				{
					break;
				}
			}
		}
	}
}
