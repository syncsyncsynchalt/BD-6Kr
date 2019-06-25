using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel))]
	[RequireComponent(typeof(Animation))]
	public class ProdSortieEnd : BaseAnimation
	{
		private UITexture _uiLabel;

		private UITexture _uiOverlay;

		public static ProdSortieEnd Instantiate(ProdSortieEnd prefab, Transform parent)
		{
			ProdSortieEnd prodSortieEnd = UnityEngine.Object.Instantiate(prefab);
			prodSortieEnd.transform.parent = parent;
			prodSortieEnd.transform.localScaleOne();
			prodSortieEnd.transform.localPositionZero();
			return prodSortieEnd;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiLabel);
			Mem.Del(ref _uiOverlay);
		}

		public override void Play(Action callback)
		{
			base.transform.localScaleOne();
			base.Play(callback);
		}
	}
}
