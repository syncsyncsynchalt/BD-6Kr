using KCV;
using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonManager : MonoBehaviour
{
	public interface UIButtonManagement
	{
		UIButton GetButton();
	}

	[SerializeField]
	private UIButton[] Buttons;

	private Collider2D[] ButtonColliders;

	public UIButton nowForcusButton;

	public int nowForcusIndex;

	public Action IndexChangeAct;

	public bool IsFocusButtonAlwaysHover;

	public bool isLoopIndex;

	public bool isPlaySE;

	public bool isDisableFocus;

	private void Awake()
	{
		ButtonColliders = new Collider2D[Buttons.Length];
		for (int i = 0; i < Buttons.Length; i++)
		{
			if (Buttons[i] != null)
			{
				Buttons[i].callBack = ChangeAllButtonState;
				ButtonColliders[i] = Buttons[i].GetComponent<Collider2D>();
			}
		}
	}

	public void UpdateButtons(UIButtonManagement[] managetargets)
	{
		List<UIButton> list = new List<UIButton>();
		foreach (UIButtonManagement uIButtonManagement in managetargets)
		{
			list.Add(uIButtonManagement.GetButton());
		}
		UpdateButtons(list.ToArray());
	}

	public void UpdateButtons(UIButton[] buttons)
	{
		Buttons = buttons;
		ButtonColliders = new Collider2D[Buttons.Length];
		for (int i = 0; i < Buttons.Length; i++)
		{
			Buttons[i].callBack = ChangeAllButtonState;
			ButtonColliders[i] = Buttons[i].GetComponent<Collider2D>();
		}
	}

	private void Update()
	{
		if (IsFocusButtonAlwaysHover && nowForcusButton != null && nowForcusButton.state != UIButtonColor.State.Hover)
		{
			nowForcusButton.SetStateNonCallBack(UIButtonColor.State.Hover);
		}
	}

	public void Decide()
	{
		if (!(nowForcusButton == null))
		{
			for (int i = 0; i < nowForcusButton.onClick.Count; i++)
			{
				nowForcusButton.onClick[i].Execute();
			}
		}
	}

	public void setAllButtonEnable(bool enable)
	{
		Collider2D[] buttonColliders = ButtonColliders;
		foreach (Collider2D collider2D in buttonColliders)
		{
			if (collider2D != null)
			{
				collider2D.enabled = enable;
			}
		}
		if (!enable)
		{
			return;
		}
		UIButton[] buttons = Buttons;
		foreach (UIButton uIButton in buttons)
		{
			if (uIButton != null)
			{
				uIButton.state = UIButtonColor.State.Normal;
			}
		}
	}

	public void setAllButtonActive()
	{
		for (int i = 0; i < Buttons.Length; i++)
		{
			Buttons[i].SetActive(isActive: true);
		}
	}

	public void setDisableButtons(List<int> ButtonIndex)
	{
		setAllButtonEnable(enable: true);
		for (int i = 0; i < ButtonIndex.Count; i++)
		{
			Buttons[ButtonIndex[i]].state = UIButtonColor.State.Disabled;
			if (!isDisableFocus)
			{
				ButtonColliders[ButtonIndex[i]].enabled = false;
			}
		}
	}

	public bool moveNextButton()
	{
		return movebutton(1);
	}

	public bool movePrevButton()
	{
		return movebutton(-1);
	}

	public void setFocus(int index)
	{
		if (index == -1)
		{
			index = 0;
		}
		unsetFocus();
		if (isButtonFocusAble(Buttons[index]))
		{
			if (Buttons[index].state == UIButtonColor.State.Disabled)
			{
				setDisableFocus(Buttons[index], index);
				return;
			}
			nowForcusButton = Buttons[index];
			nowForcusIndex = index;
			Buttons[index].SetState(UIButtonColor.State.Hover, immediate: false);
		}
		else
		{
			nowForcusIndex = index;
			moveNextButton();
		}
	}

	public void unsetFocus()
	{
		nowForcusButton = null;
		nowForcusIndex = -1;
		UIButton[] buttons = Buttons;
		foreach (UIButton uIButton in buttons)
		{
			if (uIButton.state != UIButtonColor.State.Disabled)
			{
				uIButton.state = UIButtonColor.State.Normal;
			}
		}
	}

	public void setButtonDelegate(EventDelegate eventDel)
	{
		UIButton[] buttons = Buttons;
		foreach (UIButton uIButton in buttons)
		{
			if (uIButton != null)
			{
				uIButton.onClick.Add(eventDel);
			}
		}
	}

	public UIButton[] GetFocusableButtons()
	{
		return Buttons;
	}

	private void ChangeAllButtonState(UIButton button)
	{
		if (nowForcusButton == button)
		{
			return;
		}
		nowForcusButton = button;
		for (int i = 0; i < Buttons.Length; i++)
		{
			if (Buttons[i] == null)
			{
				continue;
			}
			if (Buttons[i].state == UIButtonColor.State.Disabled)
			{
				if (isDisableFocus && Buttons[i] == button)
				{
					Buttons[i].SetState(UIButtonColor.State.Disabled, immediate: false);
					nowForcusIndex = i;
					nowForcusButton = Buttons[i];
					if (isPlaySE)
					{
						SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					}
				}
			}
			else if (Buttons[i] == button)
			{
				nowForcusIndex = i;
				if (isPlaySE)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
			else
			{
				Buttons[i].SetStateNonCallBack(UIButtonColor.State.Normal);
			}
		}
		if (IndexChangeAct != null)
		{
			IndexChangeAct();
		}
	}

	private bool movebutton(int SarchDirection)
	{
		int num = nowForcusIndex;
		int nowForcusIndex2 = nowForcusIndex;
		if (isLoopIndex)
		{
		}
		for (int i = 0; i < Buttons.Length; i++)
		{
			int num2 = (!isLoopIndex) ? ((int)Util.RangeValue(nowForcusIndex + SarchDirection * (i + 1), 0f, Buttons.Length - 1)) : ((int)Util.LoopValue(nowForcusIndex + SarchDirection * (i + 1), 0f, Buttons.Length - 1));
			if (isButtonFocusAble(Buttons[num2]))
			{
				if (Buttons[num2].state != UIButtonColor.State.Disabled)
				{
					Buttons[num2].SetState(UIButtonColor.State.Hover, immediate: false);
				}
				else
				{
					setDisableFocus(Buttons[num2], num2);
				}
				return num != nowForcusIndex;
			}
		}
		return false;
	}

	private void setDisableFocus(UIButton btn, int Index)
	{
		if (nowForcusButton != null && nowForcusButton.state != UIButtonColor.State.Disabled)
		{
			nowForcusButton.SetState(UIButtonColor.State.Normal, immediate: false);
		}
		nowForcusButton = btn;
		nowForcusIndex = Index;
		if (IndexChangeAct != null)
		{
			IndexChangeAct();
		}
	}

	private bool isButtonFocusAble(UIButton btn)
	{
		return (btn.state != UIButtonColor.State.Disabled || isDisableFocus) && btn.gameObject.activeInHierarchy;
	}

	private void OnDestroy()
	{
		Buttons = null;
		ButtonColliders = null;
		nowForcusButton = null;
		IndexChangeAct = null;
	}
}
