using System;
using UnityEngine;

namespace KCV.Scene.Port
{
	public class UIPortMenuCenterButton : UIPortMenuButton, UIPortMenuButton.CompositeMenu
	{
		public enum State
		{
			MainMenu,
			SubMenu
		}

		[SerializeField]
		private UIPortMenuButtonKeyMap mUIPortMenuButtonKeyMap_SubMenu;

		[SerializeField]
		private Texture mTexture_MainBase;

		[SerializeField]
		private Texture mTexture_SubBase;

		[SerializeField]
		private float intervalAction = 1f;

		private State mState;

		private float nextTime;

		private Action mOnLongPressListener;

		private bool LongPressed;

		public bool pressed
		{
			get;
			private set;
		}

		protected override void OnCallDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_MainBase);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_SubBase);
			mUIPortMenuButtonKeyMap_SubMenu = null;
			mOnLongPressListener = null;
		}

		protected override void OnAwake()
		{
			base.OnAwake();
			mTexture_Base.mainTexture = mTexture_MainBase;
		}

		public void ChangeState(State state)
		{
			mState = state;
			switch (mState)
			{
			case State.MainMenu:
			{
				Color color2 = new Color(10f / 51f, 154f / 255f, 214f / 255f, mTexture_Glow_Back.alpha);
				mTexture_Glow_Back.color = color2;
				mTexture_Glow_Front.color = color2;
				mTexture_Text.color = color2;
				mTexture_Base.mainTexture = mTexture_MainBase;
				break;
			}
			case State.SubMenu:
			{
				Color color = new Color(1f, 151f / 255f, 10f / 51f, mTexture_Glow_Back.alpha);
				mTexture_Glow_Back.color = color;
				mTexture_Glow_Front.color = color;
				mTexture_Text.color = color;
				mTexture_Base.mainTexture = mTexture_SubBase;
				break;
			}
			}
		}

		public UIPortMenuButtonKeyMap GetSubMenuKeyMap()
		{
			return mUIPortMenuButtonKeyMap_SubMenu;
		}

		private void Update()
		{
			if (!LongPressed && pressed && nextTime < Time.realtimeSinceStartup)
			{
				LongPressed = true;
				nextTime = Time.realtimeSinceStartup + intervalAction;
				UICamera.selectedObject = null;
				if (mOnLongPressListener != null)
				{
					mOnLongPressListener();
				}
			}
		}

		private void OnPress(bool pressed)
		{
			LongPressed = false;
			this.pressed = pressed;
			nextTime = Time.realtimeSinceStartup + intervalAction;
		}

		public void SetOnLongPressListener(Action onLongPressListener)
		{
			mOnLongPressListener = onLongPressListener;
		}
	}
}
