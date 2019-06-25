using System.Collections.Generic;
using UnityEngine;

public class UIInteriorChangeFurnitureCategorySelector : MonoBehaviour
{
	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Hangings;

	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Window;

	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Wall;

	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Chest;

	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Floor;

	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Desk;

	private UIInteriorMenuButton[] mFocasableUIInteriorMenuButtons;

	private UIInteriorMenuButton mFocusUIInteriorMenuButton;

	private void Start()
	{
		List<UIInteriorMenuButton> list = new List<UIInteriorMenuButton>();
		list.Add(mUIInteriorMenuButton_Hangings);
		list.Add(mUIInteriorMenuButton_Window);
		list.Add(mUIInteriorMenuButton_Wall);
		list.Add(mUIInteriorMenuButton_Chest);
		list.Add(mUIInteriorMenuButton_Floor);
		list.Add(mUIInteriorMenuButton_Desk);
		mFocasableUIInteriorMenuButtons = list.ToArray();
		ChangeFocus(mFocasableUIInteriorMenuButtons[0]);
	}

	private void ChangeFocus(UIInteriorMenuButton uiInteriorMenuButton)
	{
		if (mFocusUIInteriorMenuButton != null)
		{
			mFocusUIInteriorMenuButton.RemoveFocus();
		}
		mFocusUIInteriorMenuButton = uiInteriorMenuButton;
		if (mFocusUIInteriorMenuButton != null)
		{
			mFocusUIInteriorMenuButton.Focus();
		}
	}

	private void Update()
	{
	}
}
