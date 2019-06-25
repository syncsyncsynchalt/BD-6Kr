using System.Collections.Generic;

namespace Server_Common.Formats.Battle
{
	public class AirBattle
	{
		public bool[] StageFlag;

		public List<int> F_PlaneFrom;

		public List<int> E_PlaneFrom;

		public AirBattle1 Air1;

		public AirBattle2 Air2;

		public AirBattle3 Air3;

		public AirBattle()
		{
			StageFlag = new bool[3];
			F_PlaneFrom = new List<int>();
			E_PlaneFrom = new List<int>();
		}

		public void SetStageFlag()
		{
			StageFlag[0] = ((Air1 != null) ? true : false);
			StageFlag[1] = ((Air2 != null) ? true : false);
			StageFlag[2] = ((Air3 != null) ? true : false);
		}
	}
}
