using Common.Enum;
using Server_Models;
using System;

namespace local.models
{
	public class BuildDockModel
	{
		private Mem_kdock _kdock;

		public int Id => _kdock.Rid;

		public KdockStates State => _kdock.State;

		public int ShipMstId => _kdock.Ship_id;

		public int TankerCount => _kdock.Tunker_num;

		public ShipModelMst Ship
		{
			get
			{
				if (ShipMstId != 0)
				{
					return new ShipModelMst(ShipMstId);
				}
				return null;
			}
		}

		public int Fuel => _kdock.Item1;

		public int Ammo => _kdock.Item2;

		public int Steel => _kdock.Item3;

		public int Baux => _kdock.Item4;

		public int Devkit => _kdock.Item5;

		public int StartTurn => _kdock.StartTime;

		public int CompleteTurn => _kdock.CompleteTime;

		public BuildDockModel(Mem_kdock mem_kdock)
		{
			__Update__(mem_kdock);
		}

		public bool IsLarge()
		{
			return _kdock.IsLargeDock();
		}

		public bool IsTunker()
		{
			return _kdock.IsTunkerDock();
		}

		public int GetTurn()
		{
			return Math.Max(_kdock.GetRequireCreateTime(), 0);
		}

		public void __Update__(Mem_kdock mem_kdock)
		{
			_kdock = mem_kdock;
		}

		public override string ToString()
		{
			string text = $"ID:{Id} 状態:{State} ";
			if (State == KdockStates.CREATE || State == KdockStates.COMPLETE)
			{
				if (IsTunker())
				{
					text += $"建造数:{TankerCount} 開始:{StartTurn} 終了:{CompleteTurn} [輸送船建造]";
				}
				else
				{
					text += string.Format("艦:{0} 開始:{1} 終了:{2}{3}", ShipMstId, StartTurn, CompleteTurn, (!IsLarge()) ? string.Empty : " [大型艦建造]");
					if (ShipMstId != 0)
					{
						ShipModelMst shipModelMst = new ShipModelMst(ShipMstId);
						text += $"(建造艦:{shipModelMst.Name})";
					}
				}
			}
			return text;
		}
	}
}
