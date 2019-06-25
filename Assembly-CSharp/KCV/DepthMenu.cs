using System.Collections;
using UnityEngine;

namespace KCV
{
	public class DepthMenu : MonoBehaviour
	{
		public struct NextChecker
		{
			public bool enable;

			public int move;

			public NextChecker(bool b, int i)
			{
				enable = b;
				move = i;
			}
		}

		public struct MenuChip
		{
			public UIWidget widget;

			public int posNo;
		}

		protected struct MenuData
		{
			public int depth;

			public Color Color;

			public Vector3 Pos;

			public Vector3 Scale;

			public bool enable;
		}

		[SerializeField]
		private GameObject[] Menus;

		public MenuChip[] MenuChips;

		protected MenuData[] MenusData;

		public int MenuNum;

		private int[,] usePosNo;

		private int index;

		private bool moving;

		public KeyControl keyCon;

		public int currentPos
		{
			get;
			private set;
		}

		protected void Start()
		{
			MenusData = new MenuData[Menus.Length];
			MenuChips = new MenuChip[Menus.Length];
			for (int i = 0; i < Menus.Length; i++)
			{
				Vector3 localPosition = Menus[i].transform.localPosition;
				Vector3 localScale = Menus[i].transform.localScale;
				MenusData[i].Pos = new Vector3(localPosition.x, localPosition.y, localPosition.z);
				MenusData[i].Scale = new Vector3(localScale.x, localScale.y, localScale.z);
				MenuChips[i].posNo = i;
				MenuChips[i].widget = Menus[i].GetComponent<UIWidget>();
				MenusData[i].Color = MenuChips[i].widget.color;
				MenusData[i].depth = MenuChips[i].widget.depth;
				MenusData[i].enable = true;
			}
		}

		protected virtual void Init(int menuNum, KeyControl key)
		{
			keyCon = key;
			MenuNum = menuNum;
			usePosNo = new int[9, 8]
			{
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				},
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				},
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				},
				{
					1,
					3,
					7,
					0,
					0,
					0,
					0,
					0
				},
				{
					1,
					3,
					5,
					7,
					0,
					0,
					0,
					0
				},
				{
					1,
					2,
					3,
					7,
					8,
					0,
					0,
					0
				},
				{
					1,
					2,
					3,
					5,
					7,
					8,
					0,
					0
				},
				{
					1,
					2,
					3,
					4,
					6,
					7,
					8,
					0
				},
				{
					1,
					2,
					3,
					4,
					5,
					6,
					7,
					8
				}
			};
			for (int i = 0; i < Menus.Length; i++)
			{
				if (i >= MenuNum)
				{
					Menus[i].SetActive(false);
				}
			}
		}

		protected void InitPosition()
		{
			currentPos = 0;
			for (int i = 0; i < MenuNum; i++)
			{
				int num = i;
				MenuData menuData = MenusData[usePosNo[MenuNum, num] - 1];
				Menus[i].transform.localPosition = menuData.Pos;
				Menus[i].transform.localScale = menuData.Scale;
				MenuChips[i].widget.depth = menuData.depth;
				MenuChips[i].widget.color = ((!MenusData[i].enable) ? new Color(0.5f, 0.5f, 0.5f, menuData.Color.a) : menuData.Color);
				MenuChips[i].posNo = num;
			}
		}

		public void nextMenu(int move)
		{
			if (!moving)
			{
				moving = true;
				processNextMenu(move);
			}
		}

		private void processNextMenu(int move)
		{
			currentPos += move;
			while (currentPos < 0)
			{
				currentPos += MenuNum;
			}
			currentPos %= MenuNum;
			for (int i = 0; i < MenuNum; i++)
			{
				int num = (i + MenuNum - currentPos) % MenuNum;
				MenuData menuData = MenusData[usePosNo[MenuNum, num] - 1];
				Hashtable hashtable = new Hashtable();
				hashtable.Add("position", menuData.Pos);
				hashtable.Add("islocal", true);
				if (num == 0)
				{
					hashtable.Add("time", 0.03f);
					hashtable.Add("oncomplete", "OnCompleteHandler");
					hashtable.Add("easetype", iTween.EaseType.linear);
					hashtable.Add("oncompletetarget", base.gameObject);
					hashtable.Add("oncompleteparams", new NextChecker(MenusData[i].enable, move));
				}
				else
				{
					hashtable.Add("time", 0.2f);
					hashtable.Add("easetype", iTween.EaseType.easeOutBack);
				}
				iTween.MoveTo(Menus[i], hashtable);
				TweenScale.Begin(Menus[i], 0.2f, menuData.Scale);
				MenuChips[i].widget.depth = menuData.depth;
				MenuChips[i].widget.color = ((!MenusData[i].enable) ? new Color(0.5f, 0.5f, 0.5f, menuData.Color.a) : menuData.Color);
				MenuChips[i].posNo = num;
			}
		}

		public void OnCompleteHandler(NextChecker nextChecker)
		{
			if (nextChecker.enable)
			{
				moving = false;
				return;
			}
			processNextMenu(nextChecker.move);
			keyCon.Index -= nextChecker.move;
			Debug.Log("Index " + keyCon.Index);
		}
	}
}
