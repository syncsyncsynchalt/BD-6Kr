using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdMapPoint : BaseAnimation
	{
		private int _sPoint;

		private void _init()
		{
			base.Awake();
			_sPoint = 0;
		}

		private new void OnDestroy()
		{
			base.OnDestroy();
		}

		public override void Play(Action callback)
		{
			UILabel component = ((Component)base.transform.FindChild("Panel/Label")).GetComponent<UILabel>();
			component.text = "×" + _sPoint;
			base.Play(callback);
			SoundUtils.PlaySE(SEFIleInfos.RewardGet);
		}

		public static ProdMapPoint Instantiate(ProdMapPoint prefab, Transform parent, int sPoint)
		{
			ProdMapPoint prodMapPoint = UnityEngine.Object.Instantiate(prefab);
			prodMapPoint.transform.parent = parent;
			prodMapPoint.transform.localScale = Vector3.one;
			prodMapPoint.transform.localPosition = Vector3.zero;
			prodMapPoint._sPoint = sPoint;
			return prodMapPoint;
		}
	}
}
