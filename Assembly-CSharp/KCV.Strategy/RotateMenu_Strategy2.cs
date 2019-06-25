using Common.Enum;
using KCV.CommandMenu;
using KCV.Display;
using KCV.Utils;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class RotateMenu_Strategy2 : MonoBehaviour
	{
		private const int ONEROTATE = 25;

		public GameObject Menus;

		private TweenPosition enterMenuMove;

		public CommandMenuParts[] menuParts;

		public KeyControl keyContoroller;

		private float[] nowRotateZ;

		private int upperNo;

		private int footerNo;

		public int myOffset;

		[SerializeField]
		private BoxCollider2D cancelTouch;

		[SerializeField]
		private UIDisplaySwipeEventRegion swipeEvent;

		[SerializeField]
		private UIPlayTween PlayTween;

		public bool isOpen;

		[SerializeField]
		private UISprite MenuBaseSprite;

		private void Awake()
		{
			menuParts = new CommandMenuParts[8];
			for (int i = 0; i < menuParts.Length; i++)
			{
				menuParts[i] = ((Component)Menus.transform.FindChild("menu0" + i)).GetComponent<CommandMenuParts>();
			}
			enterMenuMove = Menus.GetComponent<TweenPosition>();
		}

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			swipeEvent.enabled = false;
			cancelTouch.enabled = false;
			PlayTween.Play(forward: false);
			Menus.SetActive(false);
		}

		public void Init(KeyControl key)
		{
			keyContoroller = key;
			keyContoroller.setMinMaxIndex(0, menuParts.Length - 1);
			keyContoroller.Index = 0;
			nowRotateZ = new float[menuParts.Length];
			upperNo = menuParts.Length - 3;
			footerNo = menuParts.Length - 4;
		}

		public void MenuEnter(int offset)
		{
			isOpen = true;
			Menus.SetActive(true);
			swipeEvent.enabled = true;
			cancelTouch.enabled = true;
			myOffset = offset;
			keyContoroller.Index = myOffset;
			SetMenuStates(myOffset);
			for (int i = 0; i < menuParts.Length; i++)
			{
				int num = (int)Util.LoopValue(i - myOffset, 0f, menuParts.Length - 1);
				menuParts[i].tweenRot.from = Vector3.zero;
				menuParts[i].transform.eulerAngles = menuParts[i].tweenRot.from;
				if (num <= footerNo)
				{
					menuParts[i].tweenRot.to = new Vector3(0f, 0f, num * -25);
				}
				else if (num >= upperNo)
				{
					menuParts[i].tweenRot.to = new Vector3(0f, 0f, 25 * footerNo + -25 * (num - (upperNo - 1)));
				}
				nowRotateZ[i] = menuParts[i].tweenRot.to.z;
				menuParts[i].tweenRot.delay = 0.3f;
				menuParts[i].tweenRot.duration = 0.3f;
			}
			PlayTween.Play(forward: true);
		}

		public void MenuExit()
		{
			isOpen = false;
			swipeEvent.enabled = false;
			cancelTouch.enabled = false;
			for (int i = 0; i < menuParts.Length; i++)
			{
				int num = (int)Util.LoopValue(i - keyContoroller.Index, 0f, menuParts.Length - 1);
				menuParts[i].tweenRot.from = menuParts[i].transform.eulerAngles;
				menuParts[i].tweenRot.from = Vector3.zero;
				menuParts[i].transform.eulerAngles = menuParts[i].tweenRot.from;
				if (num <= footerNo)
				{
					menuParts[i].tweenRot.to = new Vector3(0f, 0f, num * -25);
				}
				if (num >= upperNo)
				{
					menuParts[i].tweenRot.to = new Vector3(0f, 0f, 25 * footerNo + -25 * (num - (upperNo - 1)));
				}
				menuParts[i].tweenRot.delay = 0f;
				menuParts[i].tweenRot.duration = 0.3f;
			}
			PlayTween.Play(forward: false);
		}

		public void moveCursol()
		{
			int num = (int)Util.LoopValue(keyContoroller.Index, 0f, menuParts.Length - 1);
			bool flag = (keyContoroller.prevIndexChangeValue > 0) ? true : false;
			SetMenuStates(num);
			for (int i = 0; i < menuParts.Length; i++)
			{
				int num2 = i - num;
				if (num2 < 0)
				{
					num2 = menuParts.Length + num2;
				}
				menuParts[i].tweenRot = menuParts[i].GetComponent<TweenRotation>();
				new Vector3(0f, 0f, nowRotateZ[i]);
				int num3 = 25;
				if (flag)
				{
					if (num2 == footerNo)
					{
						num3 = 185;
					}
				}
				else
				{
					num3 *= -1;
					if (num2 == upperNo)
					{
						num3 = -185;
					}
				}
				menuParts[i].tweenRot.from = new Vector3(0f, 0f, nowRotateZ[i]);
				menuParts[i].tweenRot.to = new Vector3(0f, 0f, nowRotateZ[i] + (float)num3);
				nowRotateZ[i] += (float)num3;
				menuParts[i].tweenRot.ResetToBeginning();
				menuParts[i].tweenRot.delay = 0f;
				menuParts[i].tweenRot.PlayForward();
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
		}

		private void SetMenuStates(int offset)
		{
			for (int i = 0; i < menuParts.Length; i++)
			{
				int num = (int)Util.LoopValue(offset + i, 0f, menuParts.Length - 1);
				if (i == 0 && menuParts[num].menuState != CommandMenuParts.MenuState.Disable)
				{
					menuParts[offset].SetMenuState(CommandMenuParts.MenuState.Forcus);
				}
				else if (menuParts[num].menuState == CommandMenuParts.MenuState.Disable)
				{
					menuParts[num].updateSprite(isFocus: false);
				}
				else
				{
					menuParts[num].SetMenuState(CommandMenuParts.MenuState.NonForcus);
				}
			}
		}

		public void SetMissionState(MissionStates state)
		{
			CommandMenuParts commandMenuParts = menuParts[3];
			if (state == MissionStates.RUNNING)
			{
				commandMenuParts.MenuNameStr = "menu_stop";
			}
			else
			{
				commandMenuParts.MenuNameStr = "menu_expedition";
			}
			SetMenuStates(myOffset);
		}

		public void setFocus()
		{
			for (int i = 0; i < menuParts.Length; i++)
			{
				if (menuParts[i].menuState != CommandMenuParts.MenuState.Disable)
				{
					if (i == keyContoroller.Index)
					{
						menuParts[i].SetMenuState(CommandMenuParts.MenuState.Forcus);
					}
					else
					{
						menuParts[i].SetMenuState(CommandMenuParts.MenuState.NonForcus);
					}
				}
			}
		}

		private void SetMenuBaseParent(CommandMenuParts menu)
		{
			MenuBaseSprite.transform.SetParent(menu.transform.GetChild(0));
			MenuBaseSprite.spriteName = menu.MenuNameStr + "off";
		}

		private void OnDestroy()
		{
			Menus = null;
			enterMenuMove = null;
			menuParts = null;
			keyContoroller = null;
			nowRotateZ = null;
			cancelTouch = null;
			swipeEvent = null;
			PlayTween = null;
			MenuBaseSprite = null;
		}
	}
}
