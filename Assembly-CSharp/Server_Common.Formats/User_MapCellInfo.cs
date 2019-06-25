using Server_Models;

namespace Server_Common.Formats
{
	public class User_MapCellInfo
	{
		private Mst_mapcell2 _mst_mapcell;

		public bool Passed;

		public Mst_mapcell2 Mst_mapcell
		{
			get
			{
				return _mst_mapcell;
			}
			set
			{
				_mst_mapcell = value;
			}
		}

		public User_MapCellInfo(Mst_mapcell2 cell, bool passed)
		{
			Mst_mapcell = cell;
			Passed = passed;
		}
	}
}
