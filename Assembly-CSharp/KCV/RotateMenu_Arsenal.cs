using KCV.Display;
using UnityEngine;

namespace KCV
{
	public class RotateMenu_Arsenal : MonoBehaviour
	{
		private class MenuObject
		{
			public UISprite sprite;

			public int nowPos;

			public TweenRotation tr;

			public UIButton button;
		}

		public GameObject Menus;

		public GameObject[] menuButton;

		private MenuObject[] menuObjects;

		private string[] posSpriteName;

		private int cursolPos;

		private string[] MenuButtonName;

		public KeyControl keyContoroller;

		private float[] nowRotateZ;

		private int upperNo;

		private int footerNo;

		private int posZeroNo;

		[SerializeField]
		private UIDisplaySwipeEventRegion swipeEvent;

		private void Start()
		{
			swipeEvent.enabled = false;
		}

		public void Init(KeyControl key)
		{
			keyContoroller = key;
			keyContoroller.setMinMaxIndex(0, menuButton.Length - 1);
			keyContoroller.Index = 0;
			keyContoroller.setChangeValue(-1f, 0f, 1f, 0f);
			MenuButtonName = new string[menuButton.Length];
			menuObjects = new MenuObject[menuButton.Length];
			for (int i = 0; i < menuButton.Length; i++)
			{
				menuObjects[i] = new MenuObject();
				menuObjects[i].sprite = menuButton[i].GetComponentInChildren<UISprite>();
				MenuButtonName[i] = menuObjects[i].sprite.spriteName;
				menuObjects[i].nowPos = i;
				menuObjects[i].tr = menuButton[i].GetComponent<TweenRotation>();
				menuObjects[i].button = menuButton[i].GetComponent<UIButton>();
			}
			posSpriteName = new string[5];
			posSpriteName[0] = MenuButtonName[0];
			posSpriteName[1] = MenuButtonName[1];
			posSpriteName[2] = MenuButtonName[2];
			posSpriteName[3] = MenuButtonName[5];
			posSpriteName[4] = MenuButtonName[6];
			nowRotateZ = new float[menuButton.Length];
			upperNo = menuButton.Length - 3;
			footerNo = menuButton.Length - 4;
			posZeroNo = 0;
			cursolPos = 0;
		}

		private void Update()
		{
		}

		public void MenuEnter()
		{
			swipeEvent.enabled = true;
			for (int i = 0; i < menuButton.Length; i++)
			{
				if (i == 0)
				{
					menuButton[i].GetComponent<UIButton>().isEnabled = true;
					menuButton[i].GetComponentInChildren<UISprite>().spriteName = MenuButtonName[0] + "_on";
				}
				else
				{
					menuButton[i].GetComponent<UIButton>().isEnabled = false;
					menuButton[i].GetComponentInChildren<UISprite>().spriteName = MenuButtonName[i];
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
			base.gameObject.GetComponent<UIPlayTween>().DelayAction(0.8f, delegate
			{
				base.gameObject.GetComponent<UIPlayTween>().Play(forward: true);
			});
		}

		public void MenuExit()
		{
			swipeEvent.enabled = false;
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
			base.gameObject.GetComponent<UIPlayTween>().Play(forward: false);
		}

		public void moveCursol(bool isDown)
		{
			int num = isDown ? 1 : (-1);
			posZeroNo += num;
			cursolPos += num;
			posZeroNo = (int)Util.LoopValue(posZeroNo, 0f, 6f);
			cursolPos = (int)Util.LoopValue(cursolPos, 0f, 4f);
			for (int i = 0; i < menuButton.Length; i++)
			{
				int num2 = i - posZeroNo;
				if (num2 < 0)
				{
					num2 = menuButton.Length + num2;
				}
				new Vector3(0f, 0f, nowRotateZ[i]);
				if (num2 == 0)
				{
					menuObjects[i].button.isEnabled = true;
					posZeroNo = i;
					menuObjects[i].sprite.spriteName = posSpriteName[cursolPos] + "_on";
				}
				else
				{
					menuObjects[i].button.isEnabled = false;
					menuObjects[i].sprite.spriteName = MenuButtonName[i];
					if (num2 == 1)
					{
						menuObjects[i].sprite.spriteName = posSpriteName[(int)Util.LoopValue(cursolPos + 1, 0f, 4f)];
					}
					if (num2 == 2)
					{
						menuObjects[i].sprite.spriteName = posSpriteName[(int)Util.LoopValue(cursolPos + 2, 0f, 4f)];
					}
					if (num2 == 5)
					{
						menuObjects[i].sprite.spriteName = posSpriteName[(int)Util.LoopValue(cursolPos + 3, 0f, 4f)];
					}
					if (num2 == 6)
					{
						menuObjects[i].sprite.spriteName = posSpriteName[(int)Util.LoopValue(cursolPos + 4, 0f, 4f)];
					}
				}
				int num3 = 25;
				if (isDown)
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
				menuObjects[i].tr.from = new Vector3(0f, 0f, nowRotateZ[i]);
				menuObjects[i].tr.to = new Vector3(0f, 0f, nowRotateZ[i] + (float)num3);
				nowRotateZ[i] += (float)num3;
				menuObjects[i].tr.ResetToBeginning();
				menuObjects[i].tr.delay = 0f;
				menuObjects[i].tr.PlayForward();
			}
		}

		public void changeButtonAlpha()
		{
		}

		public void updateSpriteName()
		{
			for (int i = 0; i < menuButton.Length; i++)
			{
				menuObjects[i].sprite.spriteName = posSpriteName[cursolPos] + ((!menuObjects[i].button.isEnabled) ? string.Empty : "_on");
			}
		}
	}
}
