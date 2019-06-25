using KCV.CommandMenu;
using KCV.Display;
using UnityEngine;

namespace KCV.Arsenal
{
	public class RotateMenu_Arsenal2 : MonoBehaviour
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

		private int Index;

		public UIDisplaySwipeEventRegion swipeEvent;

		private void Awake()
		{
			Index = 0;
			menuParts = new CommandMenuParts[6];
			for (int i = 0; i < menuParts.Length; i++)
			{
				menuParts[i] = ((Component)Menus.transform.FindChild("menu0" + i)).GetComponent<CommandMenuParts>();
			}
			enterMenuMove = Menus.GetComponent<TweenPosition>();
		}

		private void Start()
		{
			swipeEvent.enabled = false;
			base.gameObject.GetComponent<UIPlayTween>().Play(forward: false);
		}

		public void Init(KeyControl key)
		{
			keyContoroller = key;
			nowRotateZ = new float[menuParts.Length];
			upperNo = menuParts.Length - 3;
			footerNo = menuParts.Length - 4;
		}

		public void MenuEnter(int offset)
		{
			swipeEvent.enabled = true;
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
					menuParts[i].tweenRot.to = new Vector3(0f, 0f, 25 * footerNo + -25 * (num - upperNo - 1));
				}
				nowRotateZ[i] = menuParts[i].tweenRot.to.z;
				menuParts[i].tweenRot.delay = 0.3f;
				menuParts[i].tweenRot.duration = 0.3f;
			}
			base.gameObject.GetComponent<UIPlayTween>().Play(forward: true);
		}

		public void MenuExit()
		{
			swipeEvent.enabled = false;
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
					menuParts[i].tweenRot.to = new Vector3(0f, 0f, 25 * footerNo + -25 * (num - upperNo));
				}
				menuParts[i].tweenRot.delay = 0f;
				menuParts[i].tweenRot.duration = 0.3f;
			}
			base.gameObject.GetComponent<UIPlayTween>().Play(forward: false);
		}

		public void moveCursol(bool isDown)
		{
			Index = ((!isDown) ? (Index - 1) : (Index + 1));
			Index = (int)Util.LoopValue(Index, 0f, 5f);
			SetMenuStates(Index);
			for (int i = 0; i < menuParts.Length; i++)
			{
				int num = (int)Util.LoopValue(i - Index, 0f, 5f);
				if (num < 0)
				{
					num = menuParts.Length + num;
				}
				menuParts[i].tweenRot = menuParts[i].GetComponent<TweenRotation>();
				new Vector3(0f, 0f, nowRotateZ[i]);
				int num2 = 25;
				if (isDown)
				{
					if (num == footerNo)
					{
						num2 = 235;
					}
				}
				else
				{
					num2 *= -1;
					if (num == upperNo)
					{
						num2 = -235;
					}
				}
				menuParts[i].tweenRot.from = new Vector3(0f, 0f, nowRotateZ[i]);
				menuParts[i].tweenRot.to = new Vector3(0f, 0f, nowRotateZ[i] + (float)num2);
				nowRotateZ[i] += (float)num2;
				menuParts[i].tweenRot.ResetToBeginning();
				menuParts[i].tweenRot.delay = 0f;
				menuParts[i].tweenRot.PlayForward();
			}
		}

		private void SetMenuStates(int offset)
		{
			for (int i = 0; i < menuParts.Length; i++)
			{
				int num = (int)Util.LoopValue(offset + i, 0f, menuParts.Length - 1);
				if (i == 0 && menuParts[num].menuState != CommandMenuParts.MenuState.Disable)
				{
					menuParts[offset].SetMenuState(CommandMenuParts.MenuState.Forcus);
					menuParts[offset].setColider(isEnable: true);
				}
				else if (menuParts[num].menuState != CommandMenuParts.MenuState.Disable)
				{
					menuParts[num].SetMenuState(CommandMenuParts.MenuState.NonForcus);
					menuParts[offset].setColider(isEnable: true);
				}
			}
		}

		public void SetColidersEnable(bool isEnable)
		{
			menuParts[Index].setColider(isEnable);
			swipeEvent.enabled = isEnable;
		}
	}
}
