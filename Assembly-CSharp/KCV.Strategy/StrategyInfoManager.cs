using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyInfoManager : MonoBehaviour
	{
		public enum Mode
		{
			AreaInfo,
			DeckInfo,
			EnumEnd
		}

		private const int DECK_MAXNUM = 8;

		private StrategyMapManager LogicMng;

		private Mode nowInfoMode;

		[SerializeField]
		private StrategySidePanel SidePanel;

		[SerializeField]
		private StrategyUpperInfo UpperInfo;

		[SerializeField]
		private StrategyBottomInfo FooterInfo;

		[SerializeField]
		private RotateMenu_Strategy2 RotateMenu;

		[SerializeField]
		private StrategyAreaName AreaName;

		public Mode NowInfoMode
		{
			get
			{
				return nowInfoMode;
			}
			set
			{
				nowInfoMode = value;
			}
		}

		public StrategyBottomInfo GetFooterInfo()
		{
			return FooterInfo;
		}

		public void init()
		{
			nowInfoMode = Mode.AreaInfo;
			FooterInfo.UpdateBottomPanel(nowInfoMode);
			FooterInfo.ChangeMode(nowInfoMode);
			UpperInfo.UpdateUpperInfo();
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck == null)
			{
				SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = LogicMng.Area[1].GetDecks()[0];
			}
		}

		public void updateInfoPanel(int areaID)
		{
			SidePanel.UpdateSidePanel(nowInfoMode);
			UpperInfo.UpdateUpperInfo();
		}

		public void nextInfoPanel()
		{
			nowInfoMode++;
			if (nowInfoMode >= Mode.EnumEnd)
			{
				nowInfoMode = Mode.AreaInfo;
			}
			SidePanel.ChangeMode(nowInfoMode);
			float delay = (nowInfoMode != 0) ? 0f : 0.3f;
			SidePanel.Enter(nowInfoMode, delay);
		}

		public void changeMode(Mode InfoMode)
		{
			nowInfoMode = InfoMode;
			SidePanel.ChangeMode(InfoMode);
		}

		public void SetSidePanelMode(Mode InfoMode)
		{
			nowInfoMode = InfoMode;
			SidePanel.SetMode(InfoMode);
		}

		public void EnterInfoPanel(float delay = 0f)
		{
			SidePanel.Enter(nowInfoMode, delay);
			SidePanel.EnterBG();
		}

		public void ExitInfoPanel()
		{
			SidePanel.Exit(nowInfoMode);
		}

		public void updateFooterInfo(bool isUpdateMaterial)
		{
			FooterInfo.UpdateBottomPanel(nowInfoMode, isUpdateMaterial);
		}

		public void nextFooterInfo()
		{
			FooterInfo.ChangeMode(nowInfoMode);
		}

		public void EnterFooterInfo()
		{
			FooterInfo.Enter(nowInfoMode);
		}

		public void ExitFooterInfo()
		{
			FooterInfo.Exit();
		}

		public void updateUpperInfo()
		{
			UpperInfo.UpdateUpperInfo();
		}

		public void changeAreaName(int areaID)
		{
			AreaName.setAreaName(areaID);
			AreaName.StartAnimation();
		}

		public void MoveScreenOut(Action Onfinished, bool isCharacterExit = true, bool isPanelOff = false)
		{
			SidePanel.Exit(nowInfoMode);
			SidePanel.ExitBG(isPanelOff);
			UpperInfo.Exit();
			FooterInfo.Exit();
			if (isCharacterExit)
			{
				StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(isEnter: false, Onfinished);
			}
		}

		public void MoveScreenIn(Action Onfinished, bool isCharacterEnter = true, bool isSidePanelEnter = true)
		{
			if (isSidePanelEnter && !RotateMenu.isOpen)
			{
				SidePanel.Enter(nowInfoMode, 0f);
			}
			UpperInfo.Enter();
			FooterInfo.Enter(nowInfoMode);
			SidePanel.EnterBG();
			if (isCharacterEnter)
			{
				StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(isEnter: true, Onfinished);
			}
			else if (Onfinished != null)
			{
				this.DelayAction(0.2f, Onfinished);
			}
		}

		public void changeCharacter(DeckModel deck)
		{
			StrategyTopTaskManager.Instance.UIModel.Character.ChangeCharacter(deck);
		}
	}
}
