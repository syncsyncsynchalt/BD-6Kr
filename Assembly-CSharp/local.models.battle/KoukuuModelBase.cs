using Common.Enum;
using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public abstract class KoukuuModelBase : BattlePhaseModel
	{
		protected List<ShipModel_BattleAll> _ships_f;

		protected List<ShipModel_BattleAll> _ships_e;

		protected AirBattle _data;

		protected List<PlaneModelBase> _planes_f;

		protected List<PlaneModelBase> _planes_e;

		private List<BakuRaiDamageModel> _bakurai_f;

		private List<BakuRaiDamageModel> _bakurai_e;

		private ShipModel_Attacker _taiku_ship_f;

		private ShipModel_Attacker _taiku_ship_e;

		private List<SlotitemModel_Battle> _taiku_slotitems_f;

		private List<SlotitemModel_Battle> _taiku_slotitems_e;

		public int Stage1_StartCount_f => (_data.Air1 != null) ? _data.Air1.F_LostInfo.Count : 0;

		public int Stage1_LostCount_f => (_data.Air1 != null) ? _data.Air1.F_LostInfo.LostCount : 0;

		public int Stage1_EndCount_f => Stage1_StartCount_f - Stage1_LostCount_f;

		public int Stage1_StartCount_e => (_data.Air1 != null) ? _data.Air1.E_LostInfo.Count : 0;

		public int Stage1_LostCount_e => (_data.Air1 != null) ? _data.Air1.E_LostInfo.LostCount : 0;

		public int Stage1_EndCount_e => Stage1_StartCount_e - Stage1_LostCount_e;

		public int Stage2_StartCount_f => Stage1_EndCount_f;

		public int Stage2_LostCount_f
		{
			get
			{
				if (_data.Air2 == null)
				{
					return 0;
				}
				if (_data.Air2.F_LostInfo.Count == 0)
				{
					return 0;
				}
				return _data.Air2.F_LostInfo.LostCount;
			}
		}

		public int Stage2_EndCount_f => Stage2_StartCount_f - Stage2_LostCount_f;

		public int Stage2_StartCount_e => Stage1_EndCount_e;

		public int Stage2_LostCount_e
		{
			get
			{
				if (_data.Air2 == null)
				{
					return 0;
				}
				if (_data.Air2.E_LostInfo.Count == 0)
				{
					return 0;
				}
				return _data.Air2.E_LostInfo.LostCount;
			}
		}

		public int Stage2_EndCount_e => Stage2_StartCount_e - Stage2_LostCount_e;

		public KoukuuModelBase(List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, AirBattle data)
		{
			_ships_f = ships_f;
			_ships_e = ships_e;
			_data = data;
		}

		public bool existStage2()
		{
			return _data.StageFlag[1];
		}

		public ShipModel_Attacker GetTaikuShip(bool is_friend)
		{
			return (!is_friend) ? _taiku_ship_e : _taiku_ship_f;
		}

		public List<SlotitemModel_Battle> GetTaikuSlotitems(bool is_friend)
		{
			return (!is_friend) ? _taiku_slotitems_e : _taiku_slotitems_f;
		}

		public bool existStage3()
		{
			return _data.StageFlag[2];
		}

		public PlaneModelBase[] GetPlanes(bool is_friend)
		{
			return ((!is_friend) ? _planes_e : _planes_f).ToArray();
		}

		public SlotitemModel_Battle[] GetBakugekiPlanes(bool is_friend)
		{
			if (_data.Air3 == null)
			{
				return new SlotitemModel_Battle[0];
			}
			if (is_friend)
			{
				return _data.Air3.F_BakugekiPlane.ConvertAll((int mstId) => new SlotitemModel_Battle(mstId)).ToArray();
			}
			return _data.Air3.E_BakugekiPlane.ConvertAll((int mstId) => new SlotitemModel_Battle(mstId)).ToArray();
		}

		public SlotitemModel_Battle[] GetRaigekiPlanes(bool is_friend)
		{
			if (_data.Air3 == null)
			{
				return new SlotitemModel_Battle[0];
			}
			if (is_friend)
			{
				return _data.Air3.F_RaigekiPlane.ConvertAll((int mstId) => new SlotitemModel_Battle(mstId)).ToArray();
			}
			return _data.Air3.E_RaigekiPlane.ConvertAll((int mstId) => new SlotitemModel_Battle(mstId)).ToArray();
		}

		public PlaneModelBase[] GetNoDamagePlane_f()
		{
			return _planes_f.FindAll((PlaneModelBase plane) => plane.State_Stage2End == PlaneState.Normal).ToArray();
		}

		public PlaneModelBase[] GetNoDamagePlane_e()
		{
			return _planes_e.FindAll((PlaneModelBase plane) => plane.State_Stage2End == PlaneState.Normal).ToArray();
		}

		public bool IsBakugeki_f()
		{
			if (_data.Air3 == null)
			{
				return false;
			}
			if (_data.Air3.F_Bakurai == null)
			{
				return false;
			}
			List<bool> list = new List<bool>(_data.Air3.F_Bakurai.IsBakugeki);
			return list.IndexOf(item: true) != -1;
		}

		public bool IsBakugeki_e()
		{
			if (_data.Air3 == null)
			{
				return false;
			}
			if (_data.Air3.E_Bakurai == null)
			{
				return false;
			}
			List<bool> list = new List<bool>(_data.Air3.E_Bakurai.IsBakugeki);
			return list.IndexOf(item: true) != -1;
		}

		public bool IsRaigeki_f()
		{
			if (_data.Air3 == null)
			{
				return false;
			}
			if (_data.Air3.F_Bakurai == null)
			{
				return false;
			}
			List<bool> list = new List<bool>(_data.Air3.F_Bakurai.IsRaigeki);
			return list.IndexOf(item: true) != -1;
		}

		public bool IsRaigeki_e()
		{
			if (_data.Air3 == null)
			{
				return false;
			}
			if (_data.Air3.E_Bakurai == null)
			{
				return false;
			}
			List<bool> list = new List<bool>(_data.Air3.E_Bakurai.IsRaigeki);
			return list.IndexOf(item: true) != -1;
		}

		public BakuRaiDamageModel[] GetRaigekiData_f()
		{
			return _bakurai_f.ToArray();
		}

		public BakuRaiDamageModel[] GetRaigekiData_e()
		{
			return _bakurai_e.ToArray();
		}

		public override List<ShipModel_Defender> GetDefenders(bool is_friend)
		{
			return GetDefenders(is_friend, all: false);
		}

		public List<ShipModel_Defender> GetDefenders(bool is_friend, bool all)
		{
			List<BakuRaiDamageModel> list = (!is_friend) ? _bakurai_e : _bakurai_f;
			if (!all)
			{
				list = list.FindAll((BakuRaiDamageModel item) => item != null && (item.IsRaigeki() || item.IsBakugeki()));
			}
			return list.ConvertAll((BakuRaiDamageModel item) => item?.Defender);
		}

		public BakuRaiDamageModel GetAttackDamage(int defender_tmp_id)
		{
			BakuRaiDamageModel bakuRaiDamageModel = _bakurai_f.Find((BakuRaiDamageModel r) => r != null && r.Defender.TmpId == defender_tmp_id);
			if (bakuRaiDamageModel != null)
			{
				return bakuRaiDamageModel;
			}
			bakuRaiDamageModel = _bakurai_e.Find((BakuRaiDamageModel r) => r != null && r.Defender.TmpId == defender_tmp_id);
			if (bakuRaiDamageModel != null)
			{
				return bakuRaiDamageModel;
			}
			return null;
		}

		protected void _Initialize()
		{
			_CreatePlanes();
			_CalcStage1();
			_CalcStage2();
			_CalcStage3();
		}

		protected virtual void _CreatePlanes()
		{
		}

		protected List<PlaneModelBase> __CreatePlanes(List<ShipModel_Attacker> ships, List<int> plane_from)
		{
			List<PlaneModelBase> list = new List<PlaneModelBase>();
			for (int i = 0; i < plane_from.Count; i++)
			{
				int tmp_id = plane_from[i];
				ShipModel_Attacker shipModel_Attacker = ships.Find((ShipModel_Attacker item) => item.TmpId == tmp_id);
				if (shipModel_Attacker == null)
				{
					continue;
				}
				List<SlotitemModel_Battle> slotitemList = shipModel_Attacker.SlotitemList;
				slotitemList = slotitemList.FindAll((SlotitemModel_Battle slot) => slot?.IsPlaneAtKouku() ?? false);
				if (slotitemList.Count > 0)
				{
					for (int j = 0; j < slotitemList.Count; j++)
					{
						PlaneModel item2 = new PlaneModel(shipModel_Attacker, slotitemList[j].MstId);
						list.Add(item2);
					}
				}
			}
			return list;
		}

		protected void _CalcStage1()
		{
			if (_data.Air1 == null)
			{
				return;
			}
			Random random = new Random();
			if (_planes_f.Count > 0)
			{
				int count = _data.Air1.F_LostInfo.Count;
				int power = (int)Math.Floor((double)count / (double)_planes_f.Count);
				int extra = count % _planes_f.Count;
				for (int i = 0; i < _planes_f.Count; i++)
				{
					_planes_f[i].SetStage1Power(power, ref extra);
				}
				int lost = _data.Air1.F_LostInfo.LostCount;
				while (lost > 0)
				{
					List<PlaneModelBase> list = _planes_f.FindAll((PlaneModelBase plane) => plane.Power_Stage1End > 0);
					if (list.Count > 0)
					{
						PlaneModelBase planeModelBase = list[random.Next(list.Count)];
						planeModelBase.SetStage1Lost(ref lost);
					}
				}
			}
			if (_planes_e.Count <= 0)
			{
				return;
			}
			int count2 = _data.Air1.E_LostInfo.Count;
			int power2 = (int)Math.Floor((double)count2 / (double)_planes_e.Count);
			int extra2 = count2 % _planes_e.Count;
			for (int j = 0; j < _planes_e.Count; j++)
			{
				_planes_e[j].SetStage1Power(power2, ref extra2);
			}
			int lost2 = _data.Air1.E_LostInfo.LostCount;
			while (lost2 > 0)
			{
				List<PlaneModelBase> list2 = _planes_e.FindAll((PlaneModelBase plane) => plane.Power_Stage1End > 0);
				if (list2.Count > 0)
				{
					PlaneModelBase planeModelBase2 = list2[random.Next(list2.Count)];
					planeModelBase2.SetStage1Lost(ref lost2);
				}
			}
		}

		protected void _CalcStage2()
		{
			if (_data.Air2 == null)
			{
				return;
			}
			Random random = new Random();
			if (_planes_f.Count > 0)
			{
				int lost = Stage2_LostCount_f;
				while (lost > 0)
				{
					List<PlaneModelBase> list = _planes_f.FindAll((PlaneModelBase plane) => plane.Power_Stage2End > 0);
					if (list.Count > 0)
					{
						PlaneModelBase planeModelBase = list[random.Next(list.Count)];
						planeModelBase.SetStage2Lost(ref lost);
					}
				}
			}
			if (_planes_e.Count > 0)
			{
				int lost2 = Stage2_LostCount_e;
				while (lost2 > 0)
				{
					List<PlaneModelBase> list2 = _planes_e.FindAll((PlaneModelBase plane) => plane.Power_Stage2End > 0);
					if (list2.Count > 0)
					{
						PlaneModelBase planeModelBase2 = list2[random.Next(list2.Count)];
						planeModelBase2.SetStage2Lost(ref lost2);
					}
				}
			}
			if (_data.Air2.F_AntiFire != null)
			{
				AirFireInfo info = _data.Air2.F_AntiFire;
				_taiku_ship_f = _ships_f.Find((ShipModel_BattleAll ship) => ship.TmpId == info.AttackerId).__CreateAttacker__();
				_taiku_slotitems_f = new List<SlotitemModel_Battle>();
				for (int i = 0; i < info.UseItems.Count; i++)
				{
					_taiku_slotitems_f.Add(new SlotitemModel_Battle(info.UseItems[i]));
				}
			}
			if (_data.Air2.E_AntiFire != null)
			{
				AirFireInfo info2 = _data.Air2.E_AntiFire;
				_taiku_ship_e = _ships_e.Find((ShipModel_BattleAll ship) => ship.TmpId == info2.AttackerId).__CreateAttacker__();
				_taiku_slotitems_e = new List<SlotitemModel_Battle>();
				for (int j = 0; j < info2.UseItems.Count; j++)
				{
					_taiku_slotitems_e.Add(new SlotitemModel_Battle(info2.UseItems[j]));
				}
			}
		}

		protected void _CalcStage3()
		{
			if (_data.Air3 == null)
			{
				_data_f = _CreateRaigekiData(null, _ships_f);
				_data_e = _CreateRaigekiData(null, _ships_e);
			}
			else
			{
				_data_f = _CreateRaigekiData(_data.Air3.F_Bakurai, _ships_f);
				_data_e = _CreateRaigekiData(_data.Air3.E_Bakurai, _ships_e);
			}
			_bakurai_f = _data_f.ConvertAll((DamageModelBase item) => (BakuRaiDamageModel)item);
			_bakurai_e = _data_e.ConvertAll((DamageModelBase item) => (BakuRaiDamageModel)item);
			for (int i = 0; i < _bakurai_f.Count; i++)
			{
				if (_bakurai_f[i] != null)
				{
					_bakurai_f[i].__CalcDamage__();
				}
			}
			for (int j = 0; j < _bakurai_e.Count; j++)
			{
				if (_bakurai_e[j] != null)
				{
					_bakurai_e[j].__CalcDamage__();
				}
			}
		}

		private List<DamageModelBase> _CreateRaigekiData(BakuRaiInfo Bakurai, List<ShipModel_BattleAll> ships)
		{
			List<DamageModelBase> list = new List<DamageModelBase>();
			for (int i = 0; i < ships.Count; i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = ships[i];
				if (shipModel_BattleAll == null || Bakurai == null)
				{
					list.Add(null);
					continue;
				}
				bool is_raigeki = Bakurai.IsRaigeki[i];
				bool is_bakugeki = Bakurai.IsBakugeki[i];
				BakuRaiDamageModel bakuRaiDamageModel = new BakuRaiDamageModel(shipModel_BattleAll, is_raigeki, is_bakugeki);
				int damage = Bakurai.Damage[i];
				BattleHitStatus hitstate = Bakurai.Clitical[i];
				BattleDamageKinds dmgkind = Bakurai.DamageType[i];
				bakuRaiDamageModel.__AddData__(damage, hitstate, dmgkind);
				list.Add(bakuRaiDamageModel);
			}
			return list;
		}

		protected string ToString_Stage1()
		{
			string text = string.Empty;
			if (_data.Air1 != null)
			{
				text += $"--Stage1 ";
				text += $"[味方側] Count:{Stage1_StartCount_f}-{Stage1_EndCount_f} Lost:{Stage1_LostCount_f} ";
				text += $"[相手側] Count:{Stage1_StartCount_e}-{Stage1_EndCount_e} Lost:{Stage1_LostCount_e}\n";
			}
			return text;
		}

		protected string ToString_Stage2()
		{
			string text = string.Empty;
			if (_data.Air2 != null)
			{
				text += $"--Stage2 ";
				text += $"[味方側] Count:{Stage2_StartCount_f}-{Stage2_EndCount_f} Lost:{Stage2_LostCount_f} ";
				text += $"[相手側] Count:{Stage2_StartCount_e}-{Stage2_EndCount_e} Lost:{Stage2_LostCount_e}\n";
				ShipModel_Attacker taikuShip = GetTaikuShip(is_friend: true);
				List<SlotitemModel_Battle> taikuSlotitems = GetTaikuSlotitems(is_friend: true);
				if (taikuShip != null)
				{
					text += $"[味方側 対空カットイン] {taikuShip}\n";
					text += "\t使用した装備: ";
					for (int i = 0; i < taikuSlotitems.Count; i++)
					{
						text = ((taikuSlotitems[i] != null) ? (text + $" [{taikuSlotitems[i]}]") : (text + " [-]"));
					}
					text += "\n";
				}
				taikuShip = GetTaikuShip(is_friend: false);
				taikuSlotitems = GetTaikuSlotitems(is_friend: false);
				if (taikuShip != null)
				{
					text += $"[相手側 対空カットイン] {taikuShip}\n";
					text += "\t使用した装備: ";
					for (int j = 0; j < taikuSlotitems.Count; j++)
					{
						text = ((taikuSlotitems[j] != null) ? (text + $" [{taikuSlotitems[j]}]") : (text + " [-]"));
					}
					text += "\n";
				}
			}
			return text;
		}

		protected string ToString_Stage3()
		{
			string text = string.Empty;
			if (_data.Air3 != null)
			{
				text += $"--Stage3 ";
				text += string.Format("[味方側への爆撃] {0} ", (!IsBakugeki_f()) ? "無" : "有");
				text += string.Format("[相手側への爆撃] {0}\n", (!IsBakugeki_e()) ? "無" : "有");
				SlotitemModel_Battle[] bakugekiPlanes = GetBakugekiPlanes(is_friend: true);
				if (bakugekiPlanes.Length > 0)
				{
					text += $"--爆撃を行った艦載機(味方側)--\n";
				}
				for (int i = 0; i < bakugekiPlanes.Length; i++)
				{
					text += $" [{bakugekiPlanes[i]}]\n";
				}
				SlotitemModel_Battle[] bakugekiPlanes2 = GetBakugekiPlanes(is_friend: false);
				if (bakugekiPlanes2.Length > 0)
				{
					text += $"--爆撃を行った艦載機(相手側)--\n";
				}
				for (int j = 0; j < bakugekiPlanes2.Length; j++)
				{
					text += $" [{bakugekiPlanes2[j]}]\n";
				}
				SlotitemModel_Battle[] raigekiPlanes = GetRaigekiPlanes(is_friend: true);
				if (raigekiPlanes.Length > 0)
				{
					text += $"--雷撃を行った艦載機(味方側)--\n";
				}
				for (int k = 0; k < raigekiPlanes.Length; k++)
				{
					text += $" [{raigekiPlanes[k]}]\n";
				}
				SlotitemModel_Battle[] raigekiPlanes2 = GetRaigekiPlanes(is_friend: false);
				if (raigekiPlanes2.Length > 0)
				{
					text += $"--雷撃を行った艦載機(相手側)--\n";
				}
				for (int l = 0; l < raigekiPlanes2.Length; l++)
				{
					text += $" [{raigekiPlanes2[l]}]\n";
				}
				BakuRaiDamageModel[] raigekiData_f = GetRaigekiData_f();
				foreach (BakuRaiDamageModel bakuRaiDamageModel in raigekiData_f)
				{
					if (bakuRaiDamageModel != null && (bakuRaiDamageModel.IsRaigeki() || bakuRaiDamageModel.IsBakugeki()))
					{
						text += string.Format("{0}({1}) へ雷撃 (ダメ\u30fcジ:{2} {3}{4}) {5}{6}\n", bakuRaiDamageModel.Defender.Name, bakuRaiDamageModel.Defender.Index, bakuRaiDamageModel.GetDamage(), bakuRaiDamageModel.GetHitState(), (!bakuRaiDamageModel.GetProtectEffect()) ? string.Empty : "[かばう]", (!bakuRaiDamageModel.IsBakugeki()) ? string.Empty : "[爆]", (!bakuRaiDamageModel.IsRaigeki()) ? string.Empty : "[雷]");
					}
				}
				raigekiData_f = GetRaigekiData_e();
				foreach (BakuRaiDamageModel bakuRaiDamageModel2 in raigekiData_f)
				{
					if (bakuRaiDamageModel2 != null && (bakuRaiDamageModel2.IsRaigeki() || bakuRaiDamageModel2.IsBakugeki()))
					{
						text += string.Format("{0}({1}) へ雷撃 (ダメ\u30fcジ:{2} {3}{4}) {5}{6}\n", bakuRaiDamageModel2.Defender.Name, bakuRaiDamageModel2.Defender.Index, bakuRaiDamageModel2.GetDamage(), bakuRaiDamageModel2.GetHitState(), (!bakuRaiDamageModel2.GetProtectEffect()) ? string.Empty : "[かばう]", (!bakuRaiDamageModel2.IsBakugeki()) ? string.Empty : "[爆]", (!bakuRaiDamageModel2.IsRaigeki()) ? string.Empty : "[雷]");
					}
				}
			}
			return text;
		}
	}
}
