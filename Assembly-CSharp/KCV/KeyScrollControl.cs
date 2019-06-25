using UnityEngine;

namespace KCV
{
	public class KeyScrollControl
	{
		public int index;

		public int topIndex;

		public int underIndex;

		public int viewRange;

		public int objCount;

		private UIScrollBar _scrollBar;

		public KeyScrollControl(int range, int count, UIScrollBar scrollBar)
		{
			index = 1;
			viewRange = range;
			topIndex = 1;
			underIndex = range;
			objCount = count;
			_scrollBar = scrollBar;
		}

		private void Start()
		{
		}

		public void SetKeyScroll(KeyControl.KeyName key)
		{
			switch (key)
			{
			case KeyControl.KeyName.DOWN:
				if (index == objCount)
				{
					break;
				}
				if (CheckScrollBar())
				{
					index = topIndex;
					break;
				}
				if (index < objCount)
				{
					index++;
				}
				if (underIndex < index)
				{
					MoveScrollBar(key);
					topIndex++;
					underIndex++;
					Debug.Log("True top" + topIndex + " under:" + underIndex + "  index:" + index);
				}
				break;
			case KeyControl.KeyName.UP:
				if (index == 1)
				{
					break;
				}
				if (CheckScrollBar())
				{
					index = topIndex;
					break;
				}
				if (index > 1)
				{
					index--;
				}
				if (topIndex > index)
				{
					MoveScrollBar(key);
					topIndex--;
					underIndex--;
					Debug.Log("True top" + topIndex + " under:" + underIndex + "  index:" + index);
				}
				break;
			}
		}

		public void MoveScrollBar(KeyControl.KeyName key)
		{
			float num = objCount - viewRange;
			if (num <= 0f)
			{
				return;
			}
			float num2 = 1f / num;
			Debug.Log(key);
			switch (key)
			{
			case KeyControl.KeyName.DOWN:
				if (_scrollBar.value < 1f)
				{
					_scrollBar.value += num2;
				}
				break;
			case KeyControl.KeyName.UP:
				if (_scrollBar.value > 0f)
				{
					_scrollBar.value -= num2;
				}
				break;
			}
		}

		public bool CheckScrollBar()
		{
			float num = objCount - (viewRange - 1);
			float num2 = 1f / num;
			int num3 = 0;
			for (int i = 0; (float)i < num && _scrollBar.value >= num2 * (float)(i + 1); i++)
			{
				num3++;
				if (1f <= num2 * (float)(i + 1))
				{
					num3 = (int)num;
				}
			}
			if (index == objCount && _scrollBar.value >= 1f)
			{
				return false;
			}
			topIndex = num3 + 1;
			underIndex = topIndex + (viewRange - 1);
			Debug.Log("CheckScrollBar  Top:" + topIndex + " Und:" + underIndex + " List:" + index);
			if (index < topIndex || index > underIndex)
			{
				return true;
			}
			return false;
		}

		public void ChangeObjAllCount(int cnt)
		{
			objCount = cnt;
		}

		private void Update()
		{
		}
	}
}
