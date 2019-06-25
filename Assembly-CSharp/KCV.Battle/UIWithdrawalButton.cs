using UnityEngine;

namespace KCV.Battle
{
	public class UIWithdrawalButton : UIHexButtonEx
	{
		[SerializeField]
		private UISprite _uiLabelSprite;

		protected override void OnInit()
		{
			switch (index)
			{
			case 1:
				_uiLabelSprite.spriteName = "txt_yasen_off";
				break;
			case 0:
				_uiLabelSprite.spriteName = "txt_return_off";
				break;
			}
		}

		protected override void OnUnInit()
		{
			Mem.Del(ref _uiLabelSprite);
		}

		protected override void SetForeground()
		{
			switch (index)
			{
			case 1:
				_uiLabelSprite.spriteName = ((!toggle.value) ? "txt_yasen_off" : "txt_yasen_on");
				break;
			case 0:
				_uiLabelSprite.spriteName = ((!toggle.value) ? "txt_return_off" : "txt_return_on");
				break;
			}
		}
	}
}
