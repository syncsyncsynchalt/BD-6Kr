using System;
using UnityEngine;

namespace KCV.Title
{
	[Serializable]
	public class TitlePrefabFile : BasePrefabFile
	{
		[SerializeField]
		private Transform _prefabUIPressAnyKey;

		[SerializeField]
		private Transform _prefabCtrlTitleSelectMode;

		[SerializeField]
		private Transform _prefabCtrlDifficultySelect;

		public Transform prefabUIPressAnyKey => _prefabUIPressAnyKey;

		public Transform prefabCtrlTitleSelectMode => _prefabCtrlTitleSelectMode;

		public Transform prefabCtrlDifficultySelect => _prefabCtrlDifficultySelect;

		protected override void Dispose(bool disposing)
		{
			Mem.Del(ref _prefabUIPressAnyKey);
			Mem.Del(ref _prefabCtrlTitleSelectMode);
			Mem.Del(ref _prefabCtrlDifficultySelect);
			base.Dispose(disposing);
		}
	}
}
