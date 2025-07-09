using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class RaigekiModel : BattlePhaseModel, ICommandAction
	{
		private Dictionary<int, DamageModelBase> _attack_to_f;

		private Dictionary<int, DamageModelBase> _attack_to_e;

		public int Count_f => _attack_to_f.Count;

		public int Count_e => _attack_to_e.Count;

		public RaigekiModel(List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, Raigeki data)
		{
			_Init(ships_f, ships_e, data);
		}

		public RaigekiModel(Raigeki data, Dictionary<int, ShipModel_BattleAll> ships)
		{
			List<ShipModel_BattleAll> list = new List<ShipModel_BattleAll>
			{
				null,
				null,
				null,
				null,
				null,
				null
			};
			List<ShipModel_BattleAll> list2 = new List<ShipModel_BattleAll>
			{
				null,
				null,
				null,
				null,
				null,
				null
			};
			foreach (ShipModel_BattleAll value in ships.Values)
			{
				if (value.IsFriend())
				{
					list[value.Index] = value;
				}
				else
				{
					list2[value.Index] = value;
				}
			}
			_Init(list, list2, data);
		}

		public ShipModel_Battle GetFirstActionShip()
		{
			List<ShipModel_Attacker> attackers = GetAttackers(is_friend: true);
			if (attackers != null && attackers.Count > 0)
			{
				return attackers[0];
			}
			attackers = GetAttackers(is_friend: false);
			if (attackers != null && attackers.Count > 0)
			{
				return attackers[0];
			}
			return null;
		}

		private void _Init(List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, Raigeki data)
		{
			_attack_to_f = _CreateRaigekiDamageModel(ships_f, ships_e, _data_e, data.F_Rai);
			_attack_to_e = _CreateRaigekiDamageModel(ships_e, ships_f, _data_f, data.E_Rai);
			for (int i = 0; i < _data_f.Count; i++)
			{
				if (_data_f[i] != null)
				{
					_data_f[i].__CalcDamage__();
				}
			}
			for (int j = 0; j < _data_e.Count; j++)
			{
				if (_data_e[j] != null)
				{
					_data_e[j].__CalcDamage__();
				}
			}
		}

		private Dictionary<int, DamageModelBase> _CreateRaigekiDamageModel(List<ShipModel_BattleAll> a_ships, List<ShipModel_BattleAll> d_ships, List<DamageModelBase> data, RaigekiInfo rInfo)
		{
			Dictionary<int, DamageModelBase> dictionary = new Dictionary<int, DamageModelBase>();
			for (int i = 0; i < d_ships.Count; i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = d_ships[i];
				if (shipModel_BattleAll != null)
				{
					data.Add(new RaigekiDamageModel(shipModel_BattleAll));
				}
				else
				{
					data.Add(null);
				}
			}
			for (int j = 0; j < rInfo.Target.Length; j++)
			{
				int num = rInfo.Target[j];
				if (num != -1)
				{
					ShipModel_BattleAll shipModel_BattleAll2 = a_ships[j];
					DamageModelBase damageModelBase = data[num];
					if (damageModelBase == null)
					{
						ShipModel_BattleAll defender = d_ships[num];
						damageModelBase = (data[num] = new RaigekiDamageModel(defender));
					}
					((RaigekiDamageModel)damageModelBase).__AddData__(shipModel_BattleAll2, rInfo.Damage[j], rInfo.Clitical[j], rInfo.DamageKind[j]);
					dictionary[shipModel_BattleAll2.TmpId] = damageModelBase;
				}
			}
			return dictionary;
		}

		public List<ShipModel_Attacker> GetAttackers(bool is_friend)
		{
			List<ShipModel_Attacker> list = new List<ShipModel_Attacker>();
			HashSet<int> hashSet = new HashSet<int>();
			List<DamageModelBase> list2 = (!is_friend) ? _data_f : _data_e;
			for (int i = 0; i < list2.Count; i++)
			{
				RaigekiDamageModel raigekiDamageModel = (RaigekiDamageModel)list2[i];
				if (raigekiDamageModel == null)
				{
					continue;
				}
				for (int j = 0; j < raigekiDamageModel.Attackers.Count; j++)
				{
					ShipModel_Attacker shipModel_Attacker = raigekiDamageModel.Attackers[j];
					if (!hashSet.Contains(shipModel_Attacker.TmpId))
					{
						hashSet.Add(shipModel_Attacker.TmpId);
						list.Add(shipModel_Attacker);
					}
				}
			}
			return list;
		}

		public override List<ShipModel_Defender> GetDefenders(bool is_friend)
		{
			return GetDefenders(is_friend, all: false);
		}

		public List<ShipModel_Defender> GetDefenders(bool is_friend, bool all)
		{
			List<DamageModelBase> list = (!is_friend) ? _data_e : _data_f;
			List<RaigekiDamageModel> list2 = list.ConvertAll((DamageModelBase item) => (RaigekiDamageModel)item);
			if (!all)
			{
				list2 = list2.FindAll((RaigekiDamageModel data) => data != null && data.Attackers.Count > 0);
			}
			return list2.ConvertAll((RaigekiDamageModel item) => item?.Defender);
		}

		public ShipModel_Defender GetAttackTo(int attacker_tmp_id)
		{
			if (_attack_to_f.ContainsKey(attacker_tmp_id))
			{
				return _attack_to_f[attacker_tmp_id].Defender;
			}
			if (_attack_to_e.ContainsKey(attacker_tmp_id))
			{
				return _attack_to_e[attacker_tmp_id].Defender;
			}
			return null;
		}

		[Obsolete("GetAttackTo(int attacker_tmp_id) を使用してください", false)]
		public ShipModel_Defender GetAttackTo(ShipModel_Battle attacker)
		{
			return GetAttackTo(attacker.TmpId);
		}

		public RaigekiDamageModel GetAttackDamage(int defender_tmp_id)
		{
			List<RaigekiDamageModel> list = _data_f.ConvertAll((DamageModelBase item) => (RaigekiDamageModel)item);
			RaigekiDamageModel raigekiDamageModel = list.Find((RaigekiDamageModel r) => r != null && r.Defender.TmpId == defender_tmp_id);
			if (raigekiDamageModel != null && raigekiDamageModel.Attackers.Count > 0)
			{
				return raigekiDamageModel;
			}
			List<RaigekiDamageModel> list2 = _data_e.ConvertAll((DamageModelBase item) => (RaigekiDamageModel)item);
			raigekiDamageModel = list2.Find((RaigekiDamageModel r) => r != null && r.Defender.TmpId == defender_tmp_id);
			if (raigekiDamageModel != null && raigekiDamageModel.Attackers.Count > 0)
			{
				return raigekiDamageModel;
			}
			return null;
		}

		[Obsolete("GetAttackTo(int attacker_tmp_id) を使用してください", false)]
		public RaigekiDamageModel GetAttackDamage(int index, bool is_friend)
		{
			List<DamageModelBase> list = (!is_friend) ? _data_e : _data_f;
			if (index < list.Count)
			{
				return (RaigekiDamageModel)list[index];
			}
			return null;
		}

		public List<RaigekiDamageModel> GetAttackDamages(bool is_friend)
		{
			List<DamageModelBase> list = (!is_friend) ? _data_e : _data_f;
			List<RaigekiDamageModel> list2 = list.ConvertAll((DamageModelBase item) => (RaigekiDamageModel)item);
			return list2.FindAll((RaigekiDamageModel dmgModel) => dmgModel != null && dmgModel.Attackers.Count > 0);
		}

		public override string ToString()
		{
			string text = string.Empty;
			List<ShipModel_Defender> defenders = GetDefenders(is_friend: true, all: true);
			for (int i = 0; i < defenders.Count; i++)
			{
				ShipModel_Battle shipModel_Battle = defenders[i];
				if (shipModel_Battle != null)
				{
					ShipModel_Defender attackTo = GetAttackTo(shipModel_Battle.TmpId);
					if (attackTo != null)
					{
						RaigekiDamageModel attackDamage = GetAttackDamage(attackTo.TmpId);
						text += string.Format("{0}({1}) から {2}({3}) へ雷撃 (ダメージ:{4}(c:{7}) {5}{6})\n", shipModel_Battle.Name, shipModel_Battle.Index, attackTo.Name, attackTo.Index, attackDamage.GetDamage(shipModel_Battle.TmpId), attackDamage.GetHitState(shipModel_Battle.TmpId), (!attackDamage.GetProtectEffect(shipModel_Battle.TmpId)) ? string.Empty : "[かばう]", attackDamage.__GetDamage__(shipModel_Battle.TmpId));
					}
				}
			}
			defenders = GetDefenders(is_friend: false, all: true);
			for (int j = 0; j < defenders.Count; j++)
			{
				ShipModel_Battle shipModel_Battle2 = defenders[j];
				if (shipModel_Battle2 != null)
				{
					ShipModel_Defender attackTo2 = GetAttackTo(shipModel_Battle2.TmpId);
					if (attackTo2 != null)
					{
						RaigekiDamageModel attackDamage2 = GetAttackDamage(attackTo2.TmpId);
						text += string.Format("{0}({1}) から {2}({3}) へ雷撃 (ダメージ:{4}(c:{7}) {5}{6})\n", shipModel_Battle2.Name, shipModel_Battle2.Index, attackTo2.Name, attackTo2.Index, attackDamage2.GetDamage(shipModel_Battle2.TmpId), attackDamage2.GetHitState(shipModel_Battle2.TmpId), (!attackDamage2.GetProtectEffect(shipModel_Battle2.TmpId)) ? string.Empty : "[かばう]", attackDamage2.__GetDamage__(shipModel_Battle2.TmpId));
					}
				}
			}
			return text;
		}
	}
}
