using KCV.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public class UIShortCutButtonManager : MonoBehaviour
	{
		public UIButtonManager ButtonManager;

		[SerializeField]
		private UIGrid grid;

		[SerializeField]
		private Transform ButtonFocus;

		private int[] SceneButtonIndex;

		private void Awake()
		{
			ButtonManager.IndexChangeAct = delegate
			{
				ChangeCursolPos();
			};
		}

		public void ChangeCursolPos()
		{
			ButtonFocus.localPosition = ButtonManager.nowForcusButton.transform.localPosition;
		}

		public void HideNowScene()
		{
			string nowScene = SingletonMonoBehaviour<PortObjectManager>.Instance.NowScene;
			UIButton button = SingletonMonoBehaviour<UIShortCutMenu>.Instance.getButton(nowScene);
			if (button != null)
			{
				button.SetActive(isActive: false);
			}
			else
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.getButton(Generics.Scene.SaveLoad.ToString()).SetActive(isActive: false);
			}
			grid.Reposition();
		}

		public void setDisableButton(List<int> DisableList)
		{
			ButtonManager.setDisableButtons(DisableList);
			UIButton[] focusableButtons = ButtonManager.GetFocusableButtons();
			int i;
			for (i = 0; i < focusableButtons.Length; i++)
			{
				if (DisableList.Exists((int x) => x == i))
				{
					focusableButtons[i].hoverSprite = focusableButtons[i].disabledSprite;
					focusableButtons[i].pressedSprite = focusableButtons[i].disabledSprite;
					focusableButtons[i].defaultColor = Color.gray;
					focusableButtons[i].disabledColor = Color.gray;
					focusableButtons[i].hover = Color.gray;
					focusableButtons[i].pressed = Color.gray;
					focusableButtons[i].UpdateColor(instant: true);
				}
				else
				{
					focusableButtons[i].hoverSprite = focusableButtons[i].disabledSprite + "_on";
					focusableButtons[i].pressedSprite = focusableButtons[i].disabledSprite + "_on";
					focusableButtons[i].defaultColor = Color.white;
					focusableButtons[i].disabledColor = Color.white;
					focusableButtons[i].hover = Color.white;
					focusableButtons[i].pressed = Color.white;
					focusableButtons[i].UpdateColor(instant: true);
				}
			}
		}

		public void setSelectedBtn(bool isDownKey)
		{
			if (isDownKey)
			{
				ButtonManager.moveNextButton();
			}
			else
			{
				ButtonManager.movePrevButton();
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}
	}
}
