using Common.Enum;
using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class TaskStrategyAreaInfo : SceneTaskMono
	{
		private KeyControl CommandKeyController;

		private KeyControl keyController;

		private bool isEnter;

		private Vector3 defaultPos;

		public GameObject AreaInfo;

		public AsyncObjects asyncObj;

		private Action ExitAction;

		protected override void Start()
		{
			AreaInfo.SetActive(false);
			AreaInfo.GetComponent<UIWidget>().alpha = 0f;
		}

		protected override bool Init()
		{
			isEnter = false;
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState == MissionStates.NONE)
			{
				UIShipCharacter character = StrategyTopTaskManager.Instance.UIModel.Character;
				character.moveCharacterX(StrategyTopTaskManager.Instance.UIModel.Character.getModelDefaultPosX() - 600f, 0.4f, delegate
				{
					afterInit();
				});
			}
			else
			{
				afterInit();
			}
			return true;
		}

		private void afterInit()
		{
			keyController = new KeyControl();
			AreaInfo.SetActive(true);
			AreaInfo.GetComponent<UIWidget>().alpha = 0f;
			this.DelayActionFrame(3, delegate
			{
				TweenAlpha.Begin(AreaInfo, 0.3f, 1f);
			});
			isEnter = true;
		}

		protected override bool Run()
		{
			if (!isEnter)
			{
				return true;
			}
			keyController.Update();
			return KeyAction();
		}

		protected override bool UnInit()
		{
			AreaInfo.GetComponent<UIWidget>().alpha = 0f;
			return true;
		}

		private bool KeyAction()
		{
			if (keyController.keyState[0].down)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				backToCommandMenu();
				return false;
			}
			if (keyController.keyState[5].down)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			return true;
		}

		private void backToCommandMenu()
		{
			AreaInfo.SetActive(false);
			ExitAction();
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.CommandMenu);
		}

		public void OnBackTouch()
		{
			backToCommandMenu();
			Close();
		}

		public void setExitAction(Action act)
		{
			ExitAction = act;
		}
	}
}
