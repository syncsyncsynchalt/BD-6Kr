using UnityEngine;

namespace KCV.Battle
{
	public class UIAdvancingWithDrawalButton : UIHexButtonEx
	{
		[SerializeField]
		private UISprite _uiLabelSprite;

		[SerializeField]
		private UISprite _uiLabelSubSprite;

		protected override void OnInit()
		{
			switch (index)
			{
			case 1:
				_uiLabelSprite.spriteName = "txt_go_off";
				break;
			case 2:
				_uiLabelSprite.spriteName = "txt_go_off";
				_uiLabelSubSprite.spriteName = "txt_kessen_off";
				break;
			case 0:
				_uiLabelSprite.spriteName = "txt_escape_off";
				break;
			}
		}

		protected override void OnUnInit()
		{
			Mem.Del(ref _uiLabelSprite);
			Mem.Del(ref _uiLabelSubSprite);
		}

		protected override void SetForeground()
		{
			switch (index)
			{
			case 1:
				_uiLabelSprite.spriteName = string.Format("txt_go_{0}", (!toggle.value) ? "off" : "on");
				break;
			case 2:
				_uiLabelSprite.spriteName = string.Format("txt_go_{0}", (!toggle.value) ? "off" : "on");
				_uiLabelSubSprite.spriteName = string.Format("txt_kessen_{0}", (!toggle.value) ? "off" : "on");
				break;
			case 0:
				_uiLabelSprite.spriteName = string.Format("txt_escape_{0}", (!toggle.value) ? "off" : "on");
				break;
			}
		}
	}
}
