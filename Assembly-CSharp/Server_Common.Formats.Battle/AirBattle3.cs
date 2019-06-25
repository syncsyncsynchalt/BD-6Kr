using System.Collections.Generic;

namespace Server_Common.Formats.Battle
{
	public class AirBattle3
	{
		public List<int> F_BakugekiPlane;

		public List<int> F_RaigekiPlane;

		public List<int> E_BakugekiPlane;

		public List<int> E_RaigekiPlane;

		public BakuRaiInfo F_Bakurai;

		public BakuRaiInfo E_Bakurai;

		public AirBattle3()
		{
			F_Bakurai = new BakuRaiInfo();
			E_Bakurai = new BakuRaiInfo();
			F_BakugekiPlane = new List<int>();
			F_RaigekiPlane = new List<int>();
			E_BakugekiPlane = new List<int>();
			E_RaigekiPlane = new List<int>();
		}
	}
}
