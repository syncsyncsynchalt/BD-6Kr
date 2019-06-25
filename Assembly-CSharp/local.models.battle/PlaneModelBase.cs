using Server_Models;
using System;

namespace local.models.battle
{
	public abstract class PlaneModelBase
	{
		private Mst_slotitem _mst;

		private int _power_stage1_start;

		private int _power_stage1_end;

		private int _power_stage2_end = -1;

		public int MstId => _mst.Id;

		public string Name => _mst.Name;

		public int Power_Stage1Start => _power_stage1_start;

		public int Power_Stage1End => _power_stage1_end;

		public PlaneState State_Stage1End => _GetState(_power_stage1_end);

		public int Power_Stage2End => (_power_stage2_end != -1) ? _power_stage2_end : _power_stage1_end;

		public PlaneState State_Stage2End => _GetState(Power_Stage2End);

		public PlaneModelBase(int slotitem_mst_id)
		{
			Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(slotitem_mst_id, out _mst);
		}

		public void SetStage1Power(int power, ref int extra)
		{
			_power_stage1_start = power;
			if (extra > 0)
			{
				_power_stage1_start++;
				extra--;
			}
			_power_stage1_end = _power_stage1_start;
		}

		public void SetStage1Lost(ref int lost)
		{
			int num = Math.Min(lost, _power_stage1_end);
			_power_stage1_end = _power_stage1_start - num;
			lost -= num;
			_power_stage2_end = _power_stage1_end;
		}

		public void SetStage2Lost(ref int lost)
		{
			int num = Math.Min(lost, Power_Stage2End);
			_power_stage2_end = Power_Stage2End - num;
			lost -= num;
		}

		public bool IsAttackPlane()
		{
			return _mst.Type3 == 6;
		}

		private PlaneState _GetState(int power)
		{
			if (power <= 0)
			{
				return PlaneState.Crush;
			}
			if (power < _power_stage1_start)
			{
				return PlaneState.Damage;
			}
			return PlaneState.Normal;
		}

		protected string ToString_PlaneState()
		{
			return $"stg1:{Power_Stage1Start}->{Power_Stage1End}({State_Stage1End}) stg2:{Power_Stage1End}->{Power_Stage2End}({State_Stage2End})";
		}

		public override string ToString()
		{
			return $"[{Name}(mst:{MstId}) {ToString_PlaneState()}]";
		}
	}
}
