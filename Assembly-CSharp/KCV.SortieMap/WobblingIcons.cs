using local.managers;
using local.models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.SortieMap
{
	public class WobblingIcons : IDisposable
	{
		private List<UIWobblingIcon> _listIcons;

		public List<UIWobblingIcon> wobblingIcons => _listIcons;

		public WobblingIcons(MapManager manager, Transform target)
		{
			_listIcons = new List<UIWobblingIcon>();
			_listIcons.Add(null);
			manager.Cells.Skip(1).ForEach(delegate(CellModel x)
			{
				Transform transform = target.FindChild($"UIWobblingIcon{x.CellNo}");
				_listIcons.Add((!(transform == null)) ? ((Component)transform).GetComponent<UIWobblingIcon>() : null);
			});
			if (manager.Map.MstId == 127)
			{
				SPProcessMap127();
			}
		}

		private void SPProcessMap127()
		{
			wobblingIcons[6] = wobblingIcons[5];
			wobblingIcons[5] = null;
			wobblingIcons[8] = null;
		}

		public void DestroyDrawWobblingIcons()
		{
			(from x in _listIcons
				where x != null && x.isWobbling
				select x).ForEach(delegate(UIWobblingIcon x)
			{
				Mem.DelComponentSafe(ref x);
			});
		}

		public void Dispose()
		{
			Mem.DelListSafe(ref _listIcons);
		}

		public bool FixedRun()
		{
			for (int i = 0; i < _listIcons.Count; i++)
			{
				if (_listIcons[i] != null)
				{
					_listIcons[i].FixedRun();
				}
			}
			return true;
		}
	}
}
