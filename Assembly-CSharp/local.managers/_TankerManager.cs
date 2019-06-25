using local.models;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.managers
{
	public class _TankerManager
	{
		private Dictionary<int, List<Mem_tanker>> _data;

		public _TankerManager()
		{
			Update();
		}

		public int GetAllCount()
		{
			int num = 0;
			foreach (List<Mem_tanker> value in _data.Values)
			{
				num += value.Count;
			}
			return num;
		}

		public int GetMoveCount()
		{
			int num = 0;
			foreach (List<Mem_tanker> value in _data.Values)
			{
				num += value.FindAll((Mem_tanker tanker) => tanker.IsBlingShip()).Count;
			}
			return num;
		}

		public AreaTankerModel GetCounts(int area_id)
		{
			List<Mem_tanker> tankers = _GetTankers(area_id);
			if (Mst_DataManager.Instance.Mst_maparea.TryGetValue(area_id, out Mst_maparea value))
			{
				return new AreaTankerModel(area_id, tankers, 30, value.GetUIMaterialLimitTankerNum());
			}
			return null;
		}

		public AreaTankerModel GetCounts()
		{
			List<Mem_tanker> tankers = _GetTankers(0);
			return new AreaTankerModel(0, tankers, GetAllCount(), 0);
		}

		public bool Update()
		{
			Api_Result<Dictionary<int, List<Mem_tanker>>> api_Result = new Api_get_Member().Tanker();
			if (api_Result.state == Api_Result_State.Success)
			{
				_data = api_Result.data;
				return true;
			}
			_data = new Dictionary<int, List<Mem_tanker>>();
			return false;
		}

		private List<Mem_tanker> _GetTankers(int area_id)
		{
			if (_data.TryGetValue(area_id, out List<Mem_tanker> value))
			{
				return value;
			}
			return new List<Mem_tanker>();
		}
	}
}
