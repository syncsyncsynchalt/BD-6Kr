using UnityEngine;

namespace KCV.Scene.Port
{
	public class UIPortMenuEngageButton : UIPortMenuButton, UIPortMenuButton.CompositeMenu
	{
		[SerializeField]
		private UIPortMenuButtonKeyMap mUIPortMenuButtonKeyMap_SubMenu;

		protected override void OnInitialize(bool isSelectable)
		{
			base.alpha = 0f;
		}

		public UIPortMenuButtonKeyMap GetSubMenuKeyMap()
		{
			return mUIPortMenuButtonKeyMap_SubMenu;
		}

		protected override void OnCallDestroy()
		{
			if (mUIPortMenuButtonKeyMap_SubMenu != null)
			{
				mUIPortMenuButtonKeyMap_SubMenu.Release();
			}
			mUIPortMenuButtonKeyMap_SubMenu = null;
		}
	}
}
