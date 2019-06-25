using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class AreaTankerModel
	{
		private int _area_id;

		private List<Mem_tanker> _tankers;

		private int _max_count;

		private int _req_count;

		public int AreaId => _area_id;

		public AreaTankerModel(int area_id, List<Mem_tanker> tankers, int max_count, int req_count)
		{
			_area_id = area_id;
			_tankers = tankers;
			_max_count = max_count;
			_req_count = req_count;
		}

		public int GetCount()
		{
			return _tankers.Count - GetCountInMission();
		}

		public int GetCountNoMove()
		{
			return GetCount() - GetCountMove();
		}

		public int GetCountMove()
		{
			return _tankers.FindAll((Mem_tanker tanker) => tanker.IsBlingShip()).Count;
		}

		public int GetCountInMission()
		{
			return _tankers.FindAll((Mem_tanker tanker) => tanker.Disposition_status == DispositionStatus.MISSION).Count;
		}

		public int GetMaxCount()
		{
			return _max_count;
		}

		public int GetReqCount()
		{
			return _req_count;
		}
	}
}
