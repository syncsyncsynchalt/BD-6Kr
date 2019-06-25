using Common.Enum;
using System.Collections;
using UnityEngine;

namespace KCV
{
	public class SortManager : MonoBehaviour
	{
		private UISprite[] _uiSortBtn;

		public bool isControl;

		private KeyControl _keyController;

		private int maxIndex = 4;

		public int sortIndex;

		private string[] stateList;

		private SortKey _nowSortKey;

		public bool isDown;

		public bool isUp;

		private void Start()
		{
		}

		public void init(SortKey SortKey, KeyControl KeyController)
		{
			isControl = false;
			sortIndex = 0;
			_nowSortKey = SortKey;
			_keyController = KeyController;
			stateList = new string[4];
			_uiSortBtn = new UISprite[4];
			for (int i = 0; i < 4; i++)
			{
				_uiSortBtn[i] = ((Component)base.transform.FindChild(stateList[i])).GetComponent<UISprite>();
			}
			SetListState();
			SetDepth();
		}

		public void ListDown()
		{
			if (!isDown)
			{
				SetListState();
				setSortBtn();
				for (int i = 1; i < 4; i++)
				{
					Hashtable hashtable = new Hashtable();
					hashtable.Add("time", 0.07f);
					hashtable.Add("delay", 0.05f * (float)(i - 1));
					hashtable.Add("y", -40f * (float)i);
					hashtable.Add("easeType", iTween.EaseType.linear);
					hashtable.Add("isLocal", true);
					_uiSortBtn[i].gameObject.MoveTo(hashtable);
				}
				isDown = true;
			}
		}

		public void ListUp()
		{
			if (!isUp)
			{
				for (int i = 0; i < 4; i++)
				{
					Hashtable hashtable = new Hashtable();
					hashtable.Add("time", 0.1f);
					hashtable.Add("y", 0f);
					hashtable.Add("easeType", iTween.EaseType.linear);
					hashtable.Add("isLocal", true);
					hashtable.Add("oncomplete", "OnAnimationComp");
					hashtable.Add("oncompletetarget", base.gameObject);
					_uiSortBtn[i].gameObject.MoveTo(hashtable);
				}
				isUp = true;
			}
		}

		public void OnAnimationComp()
		{
			unsetSortSelect();
			sortIndex = 0;
			isDown = false;
			isUp = false;
		}

		private void SetListState()
		{
			switch (_nowSortKey)
			{
			case SortKey.LEVEL:
				stateList[0] = "Level";
				stateList[1] = "Ship";
				stateList[2] = "New";
				stateList[3] = "Damage";
				break;
			case SortKey.SHIPTYPE:
				stateList[0] = "Ship";
				stateList[1] = "New";
				stateList[2] = "Damage";
				stateList[3] = "Level";
				break;
			case SortKey.NEW:
				stateList[0] = "New";
				stateList[1] = "Damage";
				stateList[2] = "Level";
				stateList[3] = "Ship";
				break;
			case SortKey.DAMAGE:
				stateList[0] = "Damage";
				stateList[1] = "Level";
				stateList[2] = "Ship";
				stateList[3] = "New";
				break;
			}
			for (int i = 0; i < 4; i++)
			{
				_uiSortBtn[i] = ((Component)base.transform.FindChild(stateList[i])).GetComponent<UISprite>();
			}
		}

		public void UpdateSortKey(SortKey SortKey)
		{
			_nowSortKey = SortKey;
		}

		public string GetSortKey()
		{
			return stateList[sortIndex];
		}

		public void SetControl(bool enabled)
		{
			isControl = enabled;
		}

		private void SetDepth()
		{
			for (int i = 0; i < 4; i++)
			{
				if (sortIndex == i)
				{
					_uiSortBtn[i].depth = 51;
				}
				else
				{
					_uiSortBtn[i].depth = 50;
				}
			}
		}

		private void setSortBtn()
		{
			for (int i = 0; i < 4; i++)
			{
				if (sortIndex == i)
				{
					Color color = new Color(0.6f, 0.6f, 1f);
					_uiSortBtn[i].color = color;
				}
				else
				{
					Color color2 = new Color(1f, 1f, 1f);
					_uiSortBtn[i].color = color2;
				}
			}
		}

		private void unsetSortSelect()
		{
			Color color = new Color(1f, 1f, 1f);
			for (int i = 0; i < 4; i++)
			{
				_uiSortBtn[i].color = color;
			}
		}

		public void MoveSelectSort(string type)
		{
			if (type == "Up")
			{
				sortIndex--;
			}
			else
			{
				sortIndex++;
			}
			sortIndex = Mathe.MinMax2Rev(sortIndex, 0, maxIndex - 1);
			SetDepth();
			setSortBtn();
		}
	}
}
