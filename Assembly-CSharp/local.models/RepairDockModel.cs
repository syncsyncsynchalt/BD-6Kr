using Common.Enum;
using Server_Models;

namespace local.models
{
	public class RepairDockModel
	{
		private Mem_ndock _mem_data;

		public int Id => (_mem_data != null) ? _mem_data.Rid : 0;

		public NdockStates State => (_mem_data != null) ? _mem_data.State : NdockStates.NOTUSE;

		public int ShipId => (_mem_data != null) ? _mem_data.Ship_id : 0;

		public int StartTurn => (_mem_data != null) ? _mem_data.StartTime : 0;

		public int CompleteTurn => (_mem_data != null) ? _mem_data.CompleteTime : 0;

		public int RemainingTurns => (_mem_data != null) ? _mem_data.GetRequireTime() : 0;

		public int Fuel => (_mem_data != null) ? _mem_data.Item1 : 0;

		public int Ammo => (_mem_data != null) ? _mem_data.Item2 : 0;

		public int Steel => (_mem_data != null) ? _mem_data.Item3 : 0;

		public int Baux => (_mem_data != null) ? _mem_data.Item4 : 0;

		public RepairDockModel(Mem_ndock mem_ndock)
		{
			__Update__(mem_ndock);
		}

		public ShipModel GetShip()
		{
			if (State == NdockStates.RESTORE)
			{
				return new ShipModel(ShipId);
			}
			return null;
		}

		public void __Update__(Mem_ndock mem_ndock)
		{
			_mem_data = mem_ndock;
		}

		public override string ToString()
		{
			string text = $"ID:{Id} 状態:{State} ";
			if (State == NdockStates.RESTORE)
			{
				ShipModel ship = GetShip();
				text += $"艦:{ship.ShortName} 開始:{StartTurn} 終了:{CompleteTurn}";
			}
			return text;
		}
	}
}
