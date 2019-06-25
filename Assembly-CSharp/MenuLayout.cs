using UnityEngine;

public class MenuLayout
{
	private int width;

	private int height;

	private int ySpace;

	private int x;

	private int y;

	private GUIStyle style;

	private GUIStyle styleSelected;

	private int selectedItemIndex;

	private bool buttonPressed;

	private bool backButtonPressed;

	private int numItems;

	private int fontSize = 16;

	private int currCount;

	private IScreen owner;

	public MenuLayout(IScreen screen, int itemWidth, int itemFontSize)
	{
		owner = screen;
		numItems = 0;
		width = itemWidth;
		fontSize = itemFontSize;
	}

	public IScreen GetOwner()
	{
		return owner;
	}

	public void DoLayout()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		numItems = currCount;
		style = new GUIStyle(GUI.skin.GetStyle("Button"));
		style.fontSize = fontSize;
		style.alignment = TextAnchor.MiddleCenter;
		styleSelected = new GUIStyle(GUI.skin.GetStyle("Button"));
		styleSelected.fontSize = fontSize + 8;
		styleSelected.alignment = TextAnchor.MiddleCenter;
		height = style.fontSize + 16;
		ySpace = 8;
		x = (Screen.width - width) / 2;
		y = (Screen.height - (height + ySpace) * numItems) / 2;
		currCount = 0;
	}

	public void SetSelectedItem(int index)
	{
		if (index < 0)
		{
			selectedItemIndex = 0;
		}
		if (index > numItems - 1)
		{
			selectedItemIndex = numItems - 1;
		}
	}

	public void ItemNext()
	{
		if (numItems > 0)
		{
			selectedItemIndex++;
			if (selectedItemIndex >= numItems)
			{
				selectedItemIndex = 0;
			}
		}
	}

	public void ItemPrev()
	{
		if (numItems > 0)
		{
			selectedItemIndex--;
			if (selectedItemIndex < 0)
			{
				selectedItemIndex = numItems - 1;
			}
		}
	}

	public void Update()
	{
		DoLayout();
		HandleInput();
	}

	public void HandleInput()
	{
	}

	private bool AddButton(string text, bool enabled = true, bool selected = false)
	{
		GUI.enabled = enabled;
		bool result = GUI.Button(GetRect(), text, (!selected) ? style : styleSelected);
		y += height + ySpace;
		return result;
	}

	public bool AddItem(string name, bool enabled = true)
	{
		bool result = false;
		if (numItems > 0)
		{
			if (AddButton(name, enabled, selectedItemIndex == currCount))
			{
				selectedItemIndex = currCount;
				result = true;
			}
			else if (buttonPressed && enabled && selectedItemIndex == currCount)
			{
				result = true;
				buttonPressed = false;
			}
		}
		currCount++;
		return result;
	}

	public bool AddBackIndex(string name, bool enabled = true)
	{
		bool result = false;
		if (numItems > 0)
		{
			if (AddButton(name, enabled, selectedItemIndex == currCount))
			{
				selectedItemIndex = currCount;
				result = true;
			}
			else if (buttonPressed && enabled && selectedItemIndex == currCount)
			{
				result = true;
				buttonPressed = false;
			}
			else if (backButtonPressed && enabled)
			{
				result = true;
				backButtonPressed = false;
			}
		}
		currCount++;
		return result;
	}

	public Rect GetRect()
	{
		return new Rect(x, y, width, height);
	}
}
