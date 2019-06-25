using KCV.Utils;
using UnityEngine;

namespace KCV.Arsenal
{
	public class ArsenalHexMenu : MonoBehaviour
	{
		[SerializeField]
		private UIButton KenzoButton;

		[SerializeField]
		private UIButtonManager MenuButtonManager;

		[SerializeField]
		private GameObject TopMoveMenuButtons;

		[SerializeField]
		private GameObject SubMoveMenuButtons;

		private TaskMainArsenalManager.State[] MenuStateString;

		private bool isSubMenuEnter;

		public int GetIndex()
		{
			return MenuButtonManager.nowForcusIndex;
		}

		private void Awake()
		{
			MenuStateString = new TaskMainArsenalManager.State[7]
			{
				TaskMainArsenalManager.State.KENZOU,
				TaskMainArsenalManager.State.KAIHATSU,
				TaskMainArsenalManager.State.KAITAI,
				TaskMainArsenalManager.State.HAIKI,
				TaskMainArsenalManager.State.KENZOU,
				TaskMainArsenalManager.State.KENZOU_BIG,
				TaskMainArsenalManager.State.YUSOUSEN
			};
			MenuButtonManager.setFocus(0);
			ChangeControlStateTopMenu(0);
		}

		private void OnDestroy()
		{
			KenzoButton = null;
			MenuButtonManager = null;
			TopMoveMenuButtons = null;
			SubMoveMenuButtons = null;
			MenuStateString = null;
		}

		private void Start()
		{
			MenuButtonManager.IsFocusButtonAlwaysHover = true;
			MenuButtonManager.IndexChangeAct = delegate
			{
				OnPushTopMenuButton();
			};
		}

		public bool Init()
		{
			MenuButtonManager.setAllButtonEnable(enable: true);
			return true;
		}

		public void UpdateButtonForcus()
		{
			MenuButtonManager.setAllButtonEnable(enable: true);
			MenuButtonManager.setFocus(MenuButtonManager.nowForcusIndex);
		}

		public void NextButtonForcus()
		{
			if (MenuButtonManager.moveNextButton())
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		public void BackButtonForcus()
		{
			if (MenuButtonManager.movePrevButton())
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		private void OnPushTopMenuButton()
		{
			int nowForcusIndex = MenuButtonManager.nowForcusIndex;
			ChangeControlStateTopMenu(nowForcusIndex);
		}

		private void ChangeControlStateTopMenu(int nowStateNo)
		{
			TaskMainArsenalManager.StateType = MenuStateString[nowStateNo];
		}

		public void AllButtonEnable(bool enabled)
		{
			MenuButtonManager.setAllButtonEnable(enabled);
		}

		public void unsetFocus()
		{
			MenuButtonManager.setFocus(0);
			ChangeControlStateTopMenu(0);
			MenuButtonManager.unsetFocus();
		}
	}
}
