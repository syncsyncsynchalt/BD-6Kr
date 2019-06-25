using System;
using UnityEngine;

namespace KCV.Startup
{
	[Serializable]
	public class StartupPrefabFile : BasePrefabFile
	{
		[SerializeField]
		private Transform _prefabUITutorialConfirmDialog;

		[SerializeField]
		private Transform _prefabCtrlPictureStoryShow;

		[SerializeField]
		private Transform _prefabProdSecretaryShipMovie;

		public Transform prefabUITutorialConfirmDialog => _prefabUITutorialConfirmDialog;

		public Transform prefabCtrlPictureStoryShow => BasePrefabFile.PassesPrefab(ref _prefabCtrlPictureStoryShow);

		public Transform prefabProdSecretaryShipMovie => BasePrefabFile.PassesPrefab(ref _prefabProdSecretaryShipMovie);

		protected override void Dispose(bool disposing)
		{
			Mem.Del(ref _prefabUITutorialConfirmDialog);
			Mem.Del(ref _prefabCtrlPictureStoryShow);
			Mem.Del(ref _prefabProdSecretaryShipMovie);
			base.Dispose(disposing);
		}
	}
}
