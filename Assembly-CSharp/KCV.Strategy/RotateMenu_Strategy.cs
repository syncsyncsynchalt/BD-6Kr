using Common.Enum;
using KCV.Display;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy
{
	public class RotateMenu_Strategy : MonoBehaviour
	{
		public GameObject Menus;

		public GameObject[] menuButton;

		public bool[] MenuEnable;

		private string[] MenuButtonName;

		private UISprite[] MenuButtonSprite;

		public MissionStates missionState;

		public KeyControl keyContoroller;

		public List<GameObject> Buttons;

		private float[] nowRotateZ;

		private int upperNo;

		private int footerNo;

		private int myOffset;

		private int a;

		[SerializeField]
		private BoxCollider2D cancelTouch;

		[SerializeField]
		private UIDisplaySwipeEventRegion swipeEvent;

		private void Start()
		{
			swipeEvent.enabled = false;
			cancelTouch.enabled = false;
		}

		public void Init(KeyControl key)
		{
			keyContoroller = key;
			keyContoroller.setMinMaxIndex(0, menuButton.Length - 1);
			keyContoroller.Index = 0;
			keyContoroller.setChangeValue(-1f, 0f, 1f, 0f);
			MenuButtonName = new string[menuButton.Length];
			MenuButtonSprite = new UISprite[menuButton.Length];
			for (int i = 0; i < menuButton.Length; i++)
			{
				MenuButtonSprite[i] = menuButton[i].GetComponentInChildren<UISprite>();
				MenuButtonName[i] = MenuButtonSprite[i].spriteName;
				int num = MenuButtonSprite[i].spriteName.LastIndexOf("_");
				MenuButtonName[i] = MenuButtonName[i].Substring(0, num + 1);
			}
			nowRotateZ = new float[menuButton.Length];
			MenuEnable = new bool[menuButton.Length];
			upperNo = menuButton.Length - 3;
			footerNo = menuButton.Length - 4;
		}

		public void MenuEnter(int offset)
		{
			swipeEvent.enabled = true;
			cancelTouch.enabled = true;
			myOffset = offset;
			StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(isEnter: true, null);
			for (int i = 0; i < menuButton.Length; i++)
			{
				if (i == 0 && MenuEnable[offset])
				{
					menuButton[i].GetComponent<UIButton>().isEnabled = true;
					menuButton[i].GetComponentInChildren<UISprite>().spriteName = MenuButtonName[offset] + "on";
				}
				else
				{
					menuButton[i].GetComponent<UIButton>().isEnabled = false;
					menuButton[i].GetComponentInChildren<UISprite>().spriteName = MenuButtonName[(int)Util.LoopValue(i + offset, 0f, 6f)] + "off";
				}
				TweenRotation component = menuButton[i].GetComponent<TweenRotation>();
				component.from = default(Vector3);
				menuButton[i].transform.eulerAngles = component.from;
				if (i <= footerNo)
				{
					component.to = new Vector3(0f, 0f, i * -25);
				}
				if (i >= upperNo)
				{
					component.to = new Vector3(0f, 0f, 25 * footerNo + -25 * (i - upperNo));
				}
				nowRotateZ[i] = component.to.z;
				component.delay = 0.3f;
				component.duration = 0.3f;
			}
			for (int j = 0; j < Buttons.Count; j++)
			{
				Buttons[j].GetComponent<UISprite>().enabled = true;
				Buttons[j].GetComponent<TweenScale>().delay = 0.6f;
			}
			base.gameObject.GetComponent<UIPlayTween>().Play(forward: true);
			for (int k = 0; k < Buttons.Count; k++)
			{
				Buttons[k].GetComponent<TweenScale>().delay = 0f;
				Buttons[k].GetComponent<TweenScale>().from = new Vector3(0.7f, 0.7f, 0.7f);
			}
		}

		public void MenuExit()
		{
			swipeEvent.enabled = false;
			cancelTouch.enabled = false;
			for (int i = 0; i < menuButton.Length; i++)
			{
				TweenRotation component = menuButton[i].GetComponent<TweenRotation>();
				component.from = default(Vector3);
				menuButton[i].transform.eulerAngles = component.from;
				if (i <= 3)
				{
					component.to = new Vector3(0f, 0f, i * -25);
				}
				if (i >= 4)
				{
					component.to = new Vector3(0f, 0f, 75 + -25 * (i - 4));
				}
				component.delay = 0f;
				component.duration = 0.3f;
			}
			for (int j = 0; j < Buttons.Count; j++)
			{
				Buttons[j].GetComponent<TweenScale>().delay = 0f;
				Buttons[j].GetComponent<TweenScale>().from = new Vector3(0f, 0f, 0f);
			}
			base.gameObject.GetComponent<UIPlayTween>().Play(forward: false);
		}

		public void moveCursol()
		{
			int num = (int)Util.LoopValue(keyContoroller.Index - myOffset, 0f, 6f);
			bool flag = (keyContoroller.prevIndexChangeValue > 0) ? true : false;
			for (int i = 0; i < menuButton.Length; i++)
			{
				int num2 = i - num;
				if (num2 < 0)
				{
					num2 = menuButton.Length + num2;
				}
				TweenRotation component = menuButton[i].GetComponent<TweenRotation>();
				new Vector3(0f, 0f, nowRotateZ[i]);
				if (num2 == 0 && MenuEnable[(int)Util.LoopValue(myOffset + i, 0f, 6f)])
				{
					menuButton[i].GetComponent<UIButton>().isEnabled = true;
					menuButton[i].GetComponentInChildren<UISprite>().spriteName = MenuButtonName[(int)Util.LoopValue(myOffset + i, 0f, 6f)] + "on";
				}
				else
				{
					menuButton[i].GetComponent<UIButton>().isEnabled = false;
					menuButton[i].GetComponentInChildren<UISprite>().spriteName = MenuButtonName[(int)Util.LoopValue(myOffset + i, 0f, 6f)] + "off";
				}
				int num3 = 25;
				if (flag)
				{
					if (num2 == footerNo)
					{
						num3 = 210;
					}
				}
				else
				{
					num3 *= -1;
					if (num2 == upperNo)
					{
						num3 = -210;
					}
				}
				component.from = new Vector3(0f, 0f, nowRotateZ[i]);
				component.to = new Vector3(0f, 0f, nowRotateZ[i] + (float)num3);
				nowRotateZ[i] += (float)num3;
				component.ResetToBeginning();
				component.delay = 0f;
				component.PlayForward();
			}
		}

		public void changeButtonAlpha()
		{
		}

		public void SetMissionState(MissionStates state)
		{
			if (state == MissionStates.RUNNING)
			{
				MenuButtonName[3] = "menu_stop_";
			}
			else
			{
				MenuButtonName[3] = "menu_expedition_";
			}
			missionState = state;
			if (myOffset == 3)
			{
				string empty = string.Empty;
				string str = (!MenuEnable[0]) ? "off" : "on";
				MenuButtonSprite[0].spriteName = MenuButtonName[3] + str;
			}
		}

		private void OnDestroy()
		{
			Menus = null;
			menuButton = null;
			MenuEnable = null;
			MenuButtonName = null;
			MenuButtonSprite = null;
			keyContoroller = null;
			if (Buttons != null)
			{
				Buttons.Clear();
			}
			Buttons = null;
			nowRotateZ = null;
			cancelTouch = null;
			swipeEvent = null;
		}
	}
}
