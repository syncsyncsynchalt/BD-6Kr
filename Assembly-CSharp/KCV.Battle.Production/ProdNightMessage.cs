using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdNightMessage : BaseAnimation
	{
		private UIPanel _uiPanel;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static ProdNightMessage Instantiate(ProdNightMessage prefab, Transform parent)
		{
			ProdNightMessage prodNightMessage = UnityEngine.Object.Instantiate(prefab);
			prodNightMessage.transform.parent = parent;
			prodNightMessage.transform.localPosition = Vector3.zero;
			prodNightMessage.transform.localScaleZero();
			return prodNightMessage;
		}

		protected override void OnDestroy()
		{
			Mem.Del(ref _uiPanel);
			base.OnDestroy();
		}

		public override void Play(Action callback)
		{
			base.transform.localScaleOne();
			base.Play(callback);
		}

		private void PlayMessageSE()
		{
			SoundUtils.PlaySE(SEFIleInfos.BattleNightMessage);
		}
	}
}
