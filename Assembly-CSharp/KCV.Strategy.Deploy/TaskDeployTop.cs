using KCV.EscortOrganize;
using KCV.Utils;
using UnityEngine;

namespace KCV.Strategy.Deploy
{
	public class TaskDeployTop : SceneTaskMono
	{
		[SerializeField]
		private DeployMainPanel MainPanel;

		[SerializeField]
		private DeployTransportPanel TransportPanel;

		private KeyControl CommandKeyController;

		public bool isDeployPanel;

		public bool isEscortPanel;

		public int TankerCount;

		public int areaID;

		public bool isChangeMode;

		private bool isInit;

		private bool isGoeiChange;

		private new void Start()
		{
			MainPanel.SetActive(isActive: false);
			TransportPanel.SetActive(isActive: false);
		}

		protected override bool Init()
		{
			MainPanel.GetComponent<TweenAlpha>().onFinished.Clear();
			MainPanel.SetActive(isActive: true);
			TransportPanel.SetActive(isActive: true);
			StrategyTopTaskManager.Instance.setActiveStrategy(isActive: false);
			if (!isInit)
			{
				isGoeiChange = true;
				areaID = StrategyAreaManager.FocusAreaID;
				CommandKeyController = KeyControlManager.Instance.KeyController;
				TankerCount = StrategyTopTaskManager.GetLogicManager().Area[areaID].GetTankerCount().GetCount();
				EscortOrganizeTaskManager.CreateLogicManager();
			}
			isChangeMode = true;
			this.DelayActionFrame(3, delegate
			{
				isInit = true;
			});
			return true;
		}

		protected override bool UnInit()
		{
			isGoeiChange = true;
			return true;
		}

		protected override bool Run()
		{
			if (!isInit)
			{
				return true;
			}
			if (isDeployPanel)
			{
				if (isChangeMode)
				{
					TransportPanel.Init();
					isGoeiChange = false;
				}
				return TransportPanel.Run();
			}
			if (isChangeMode)
			{
				MainPanel.Init(isGoeiChange);
			}
			return MainPanel.Run();
		}

		public void backToCommandMenu()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			this.DelayActionFrame(1, delegate
			{
				MainPanel.SafeGetTweenAlpha(1f, 0f, 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
				TweenAlpha component = MainPanel.GetComponent<TweenAlpha>();
				StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(isEnter: false, null);
				component.onFinished.Clear();
				component.SetOnFinished(delegate
				{
					this.DelayAction(0.15f, delegate
					{
						MainPanel.SetActive(isActive: false);
						TransportPanel.SetActive(isActive: false);
						StrategyTopTaskManager.Instance.GetInfoMng().changeCharacter(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
						MainPanel.DestroyEscortOrganize();
						this.DelayActionFrame(3, delegate
						{
							KeyControlManager.Instance.KeyController = CommandKeyController;
							StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.CommandMenu);
							StrategyTaskManager.SceneCallBack();
						});
					});
				});
				Close();
				isInit = false;
			});
		}

		private void OnDestroy()
		{
			MainPanel = null;
			TransportPanel = null;
			CommandKeyController = null;
		}
	}
}
