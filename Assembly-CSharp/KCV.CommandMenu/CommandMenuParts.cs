using UnityEngine;

namespace KCV.CommandMenu
{
	public class CommandMenuParts : MonoBehaviour
	{
		public enum MenuState
		{
			Forcus,
			NonForcus,
			Disable
		}

		[SerializeField]
		private UISprite Lamp;

		[SerializeField]
		private UISprite MenuNameSprite;

		[SerializeField]
		private UISprite MenuBase;

		[SerializeField]
		private UIPanel panel;

		[SerializeField]
		private BoxCollider2D col;

		public string MenuNameStr;

		private int nowRotateZ;

		public TweenRotation tweenRot;

		private TweenAlpha LampAnim;

		public bool isDontGo;

		public MenuState menuState
		{
			get;
			private set;
		}

		private void Awake()
		{
			menuState = MenuState.NonForcus;
			MenuNameStr = MenuNameSprite.spriteName;
			LampAnim = Lamp.AddComponent<TweenAlpha>();
			LampAnim.from = 0.2f;
			LampAnim.to = 1f;
			LampAnim.duration = 0.8f;
			LampAnim.style = UITweener.Style.PingPong;
			LampAnim.enabled = false;
		}

		public void SetMenuState(MenuState state)
		{
			menuState = state;
			switch (menuState)
			{
			case MenuState.Forcus:
				MenuNameSprite.spriteName = MenuNameStr + "_on";
				MenuNameSprite.MakePixelPerfect();
				MenuBase.spriteName = "menu_bar";
				Lamp.spriteName = "lamp_on";
				LampAnim.enabled = true;
				panel.depth = 3;
				col.enabled = true;
				break;
			case MenuState.NonForcus:
				MenuNameSprite.spriteName = MenuNameStr;
				MenuNameSprite.MakePixelPerfect();
				Lamp.spriteName = "lamp_off";
				LampAnim.enabled = false;
				panel.depth = 2;
				col.enabled = false;
				break;
			case MenuState.Disable:
				MenuBase.spriteName = "menu_bar_off";
				MenuNameSprite.spriteName = MenuNameStr + "_off";
				LampAnim.enabled = false;
				Lamp.spriteName = "lamp_off";
				panel.depth = -1;
				col.enabled = false;
				break;
			}
			if (isDontGo)
			{
				MenuBase.alpha = 0.8f;
			}
		}

		public void updateSprite(bool isFocus)
		{
			string str = (!isFocus) ? "_off" : "_on";
			MenuBase.spriteName = "menu_bar" + str;
			MenuNameSprite.spriteName = MenuNameStr + str;
			MenuNameSprite.MakePixelPerfect();
		}

		public void setColider(bool isEnable)
		{
			col.enabled = isEnable;
		}
	}
}
