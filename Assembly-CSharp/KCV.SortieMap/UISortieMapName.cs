using local.managers;
using UnityEngine;

namespace KCV.SortieMap
{
	public class UISortieMapName : MonoBehaviour
	{
		[SerializeField]
		private UILabel _uiAreaName;

		[SerializeField]
		private UITexture _uiBackground;

		private void OnDestroy()
		{
			Mem.Del(ref _uiAreaName);
			Mem.Del(ref _uiBackground);
		}

		public void SetMapInformation(MapManager manager)
		{
			_uiAreaName.text = manager.Map.Name;
		}
	}
}
