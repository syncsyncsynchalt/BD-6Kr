using Common.Enum;
using System.Collections.Generic;
using UnityEngine;

public class InheritDifficultySelecter : MonoBehaviour
{
	public enum Difficulty
	{
		SHI,
		KOU,
		OTU,
		HEI,
		TYOU
	}

	public UIButtonManager btnMng;

	public UIWidget uiWidget;

	private UIGrid grid;

	[SerializeField]
	private Transform DifficultyCursol;

	private readonly int[] exchange = new int[5]
	{
		5,
		4,
		3,
		2,
		1
	};

	public void Init(int EnableNum, MonoBehaviour LoadSelect)
	{
		btnMng.setFocus(3);
		UIButton[] focusableButtons = btnMng.GetFocusableButtons();
		int num = focusableButtons.Length - EnableNum;
		List<int> list = new List<int>();
		for (int num2 = focusableButtons.Length - 1; num2 >= 0; num2--)
		{
			bool flag = (num2 >= num) ? true : false;
			focusableButtons[num2].SetActive(flag);
			if (!flag)
			{
				list.Add(num2);
			}
		}
		btnMng.setDisableButtons(list);
		grid = btnMng.GetComponent<UIGrid>();
		grid.Reposition();
		MoveCursol();
		btnMng.setButtonDelegate(Util.CreateEventDelegate(LoadSelect, "OnDesideDifficulty", null));
		btnMng.IndexChangeAct = MoveCursol;
	}

	public void MoveNextButton()
	{
		btnMng.moveNextButton();
	}

	public void MovePrevButton()
	{
		btnMng.movePrevButton();
	}

	private void MoveCursol()
	{
		DifficultyCursol.MoveTo(btnMng.nowForcusButton.transform.position, 0.2f, iTween.EaseType.easeOutQuint, null);
	}

	public DifficultKind getDifficultKind()
	{
		return (DifficultKind)exchange[btnMng.nowForcusIndex];
	}
}
