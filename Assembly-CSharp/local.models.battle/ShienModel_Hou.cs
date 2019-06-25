using Common.Enum;
using Server_Common.Formats.Battle;
using System.Collections.Generic;

namespace local.models.battle
{
	public class ShienModel_Hou : IBattlePhase, IShienModel
	{
		protected List<ShipModel_BattleAll> _ships_f;

		protected List<ShipModel_BattleAll> _ships_e;

		protected SupportAtack _data;

		protected int _shien_deck_id;

		protected List<ShipModel_Attacker> _ships_shien;

		protected List<DamageModel> _dmg_data;

		public int ShienDeckId => _shien_deck_id;

		public ShipModel_Attacker[] ShienShips => _ships_shien.ToArray();

		public BattleSupportKinds SupportType => _data.SupportType;

		public ShienModel_Hou(DeckModel shien_deck, List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, SupportAtack data)
		{
			_shien_deck_id = shien_deck.Id;
			_ships_shien = new List<ShipModel_Attacker>();
			ShipModel[] ships = shien_deck.GetShips();
			for (int i = 0; i < ships.Length; i++)
			{
				_ships_shien.Add(new __ShipModel_Attacker__(ships[i], i));
			}
			_ships_f = ships_f;
			_ships_e = ships_e;
			_data = data;
			_dmg_data = new List<DamageModel>();
			for (int j = 0; j < ships_e.Count; j++)
			{
				ShipModel_BattleAll shipModel_BattleAll = ships_e[j];
				if (shipModel_BattleAll == null)
				{
					_dmg_data.Add(null);
					continue;
				}
				DamageModel damageModel = new DamageModel(shipModel_BattleAll);
				int damage = data.Hourai.Damage[j];
				BattleHitStatus hitstate = data.Hourai.Clitical[j];
				BattleDamageKinds dmgkind = data.Hourai.DamageType[j];
				damageModel.__AddData__(damage, hitstate, dmgkind);
				damageModel.__CalcDamage__();
				_dmg_data.Add(damageModel);
			}
		}

		public List<ShipModel_Defender> GetDefenders(bool is_friend)
		{
			List<ShipModel_Defender> list = new List<ShipModel_Defender>();
			if (is_friend)
			{
				return new List<ShipModel_Defender>();
			}
			for (int i = 0; i < _dmg_data.Count; i++)
			{
				if (_dmg_data[i] != null)
				{
					ShipModel_Defender defender = _dmg_data[i].Defender;
					if (defender.DmgStateBefore != DamageState_Battle.Gekichin && !defender.IsEscape())
					{
						list.Add(defender);
					}
				}
			}
			return list;
		}

		public List<ShipModel_Defender> GetDefenders(bool is_friend, DamagedStates damage_event)
		{
			List<ShipModel_Defender> defenders = GetDefenders(is_friend);
			return defenders.FindAll((ShipModel_Defender ship) => ship.DamageEventAfter == damage_event);
		}

		public List<ShipModel_Defender> GetGekichinShips()
		{
			return GetGekichinShips(is_friend: true);
		}

		public List<ShipModel_Defender> GetGekichinShips(bool is_friend)
		{
			List<ShipModel_Defender> defenders = GetDefenders(is_friend: true);
			return defenders.FindAll((ShipModel_Defender ship) => ship.DamageEventAfter == DamagedStates.Gekichin || ship.DamageEventAfter == DamagedStates.Youin || ship.DamageEventAfter == DamagedStates.Megami);
		}

		public DamageModel GetAttackDamage(int defender_tmp_id)
		{
			return _dmg_data.Find((DamageModel d) => d != null && d.Defender.TmpId == defender_tmp_id);
		}

		public List<DamageModel> GetAttackDamages()
		{
			return _dmg_data.GetRange(0, _dmg_data.Count);
		}

		public bool HasChuhaEvent()
		{
			return HasChuhaEvent(is_friend: true);
		}

		public bool HasChuhaEvent(bool is_friend)
		{
			if (is_friend)
			{
				return false;
			}
			DamageModel damageModel = _dmg_data.Find((DamageModel model) => model != null && model.Defender != null && model.Defender.DamageEventAfter == DamagedStates.Tyuuha);
			return damageModel != null;
		}

		public bool HasTaihaEvent()
		{
			return HasTaihaEvent(is_friend: true);
		}

		public bool HasTaihaEvent(bool is_friend)
		{
			if (is_friend)
			{
				return false;
			}
			DamageModel damageModel = _dmg_data.Find((DamageModel model) => model != null && model.Defender != null && model.Defender.DamageEventAfter == DamagedStates.Taiha);
			return damageModel != null;
		}

		public bool HasGekichinEvent()
		{
			return HasGekichinEvent(is_friend: true);
		}

		public bool HasGekichinEvent(bool is_friend)
		{
			if (is_friend)
			{
				return false;
			}
			DamageModel damageModel = _dmg_data.Find((DamageModel model) => model != null && model.Defender != null && (model.Defender.DamageEventAfter == DamagedStates.Gekichin || model.Defender.DamageEventAfter == DamagedStates.Youin || model.Defender.DamageEventAfter == DamagedStates.Megami));
			return damageModel != null;
		}

		public bool HasRecoveryEvent()
		{
			return HasRecoveryEvent(is_friend: true);
		}

		public bool HasRecoveryEvent(bool is_friend)
		{
			if (is_friend)
			{
				return false;
			}
			DamageModel damageModel = _dmg_data.Find((DamageModel model) => model != null && model.Defender != null && (model.Defender.DamageEventAfter == DamagedStates.Youin || model.Defender.DamageEventAfter == DamagedStates.Megami));
			return damageModel != null;
		}

		public override string ToString()
		{
			string text = $"[砲撃支援]\n";
			List<DamageModel> attackDamages = GetAttackDamages();
			for (int i = 0; i < attackDamages.Count; i++)
			{
				text += $"{attackDamages[i]}";
			}
			return text;
		}
	}
}
