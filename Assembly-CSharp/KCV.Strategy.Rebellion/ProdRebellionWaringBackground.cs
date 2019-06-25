using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class ProdRebellionWaringBackground : BaseAnimation
	{
		public static ProdRebellionWaringBackground Instantiate(ProdRebellionWaringBackground prefab, Transform parent)
		{
			ProdRebellionWaringBackground prodRebellionWaringBackground = Object.Instantiate(prefab);
			prodRebellionWaringBackground.transform.parent = parent;
			prodRebellionWaringBackground.transform.localScaleOne();
			prodRebellionWaringBackground.transform.localScaleZero();
			return prodRebellionWaringBackground;
		}
	}
}
