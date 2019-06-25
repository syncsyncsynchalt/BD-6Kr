using KCV.Utils;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdRewardGet : BaseAnimation
	{
		public enum RewardType
		{
			Ship,
			SlotItem,
			UseItem
		}

		private UIPanel _uiPanel;

		private UITexture _uiBackground;

		private UITexture _uiLabel;

		private UITexture _uiOverlay;

		protected override void Awake()
		{
			base.Awake();
			Util.FindParentToChild(ref _uiPanel, base.transform, "Panel");
			Util.FindParentToChild(ref _uiBackground, _uiPanel.transform, "Background");
			Util.FindParentToChild(ref _uiLabel, _uiPanel.transform, "Label");
			Util.FindParentToChild(ref _uiOverlay, _uiPanel.transform, "Overlay");
			_uiOverlay.mainTexture = (Resources.Load("Textures/Common/Overlay") as Texture2D);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiLabel);
			Mem.Del(ref _uiOverlay);
		}

		public static ProdRewardGet Instantiate(ProdRewardGet prefab, Transform parent, int nPanelDepth, RewardType iType)
		{
			ProdRewardGet prodRewardGet = Object.Instantiate(prefab);
			prodRewardGet.transform.parent = parent;
			prodRewardGet.transform.localScale = Vector3.one;
			prodRewardGet.transform.localPosition = Vector3.zero;
			prodRewardGet._uiPanel.depth = nPanelDepth;
			return prodRewardGet;
		}

		private void _playRewardSE()
		{
			SoundUtils.PlaySE(SEFIleInfos.RewardGet);
		}
	}
}
