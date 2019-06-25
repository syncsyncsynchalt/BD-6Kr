using KCV.Tutorial.Guide;
using local.models;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace KCV
{
	public class TutorialGuideManager : SingletonMonoBehaviour<TutorialGuideManager>
	{
		public enum TutorialID
		{
			Step1,
			Step2,
			Step3,
			Step3_2,
			Step4,
			Step5,
			Step6,
			Step7,
			Step8,
			Step9,
			StrategyText,
			PortTopText,
			BattleCommand,
			RepairInfo,
			SupplyInfo,
			StrategyPoint,
			BattleShortCutInfo,
			Raider,
			RebellionPreparation,
			Rebellion_EnableIntercept,
			Rebellion_DisableIntercept,
			Rebellion_CombinedFleet,
			Rebellion_Lose,
			ResourceRecovery,
			TankerDeploy,
			EscortOrganize,
			Bring,
			BuildShip,
			SpeedBuild,
			Organize,
			EndGame
		}

		private TutorialDialog tutorialDialog;

		private ResourceRequest req;

		public TutorialModel model;

		[SerializeField]
		private bool[] Debug_TutorialFlag;

		public TutorialID Debug_TutorialID;

		public TutorialDialog GetTutorialDialog()
		{
			return tutorialDialog;
		}

		public bool isRequest()
		{
			return req != null;
		}

		public bool CheckAndShowFirstTutorial(TutorialModel model, TutorialID ID, Action OnLoaded)
		{
			if (!model.GetKeyTutorialFlg((int)ID) && CheckTutorialCondition(ID) && req == null)
			{
				StartCoroutine(LoadTutorial(model, ID, OnLoaded));
				return true;
			}
			return false;
		}

		public bool CheckAndShowFirstTutorial(TutorialModel model, TutorialID ID, Action OnLoaded, Action OnFinished)
		{
			Action onLoaded = delegate
			{
				if (OnLoaded != null)
				{
					OnLoaded();
				}
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog().OnClosed = OnFinished;
			};
			if (!model.GetKeyTutorialFlg((int)ID) && CheckTutorialCondition(ID) && req == null)
			{
				StartCoroutine(LoadTutorial(model, ID, onLoaded));
				return true;
			}
			if (OnFinished != null)
			{
				OnFinished();
			}
			return false;
		}

		public void ShowFirstTutorial(TutorialModel model, TutorialID ID, Action OnLoaded)
		{
			StartCoroutine(LoadTutorial(model, ID, OnLoaded));
		}

		public bool CheckFirstTutorial(TutorialModel model, TutorialID ID)
		{
			if (!model.GetKeyTutorialFlg((int)ID) && CheckTutorialCondition(ID) && req == null)
			{
				return true;
			}
			return false;
		}

		private IEnumerator LoadTutorial(TutorialModel model, TutorialID ID, Action OnLoaded)
		{
			KeyControl key = null;
			if (SingletonMonoBehaviour<UIShortCutMenu>.exist())
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: true);
				key = App.OnlyController;
				App.OnlyController = new KeyControl();
			}
			req = Resources.LoadAsync("Prefabs/TutorialGuide/TutorialDialog_" + ID.ToString());
			yield return req;
			tutorialDialog = null;
			tutorialDialog = Util.Instantiate(req.asset).GetComponent<TutorialDialog>();
			tutorialDialog.SetTutorialId(ID);
			model.SetKeyTutorialFlg((int)ID);
			tutorialDialog.SetOnLoaded(OnLoaded);
			req = null;
			if (SingletonMonoBehaviour<UIShortCutMenu>.exist())
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: false);
				App.OnlyController = key;
			}
		}

		private bool CheckTutorialCondition(TutorialID ID)
		{
			switch (ID)
			{
			case TutorialID.RepairInfo:
				return SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetShips().Any((ShipModel x) => (float)(x.NowHp / x.MaxHp) < 0.7f);
			case TutorialID.SupplyInfo:
				return SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetShips().Any((ShipModel x) => x.AmmoRate < 50.0 || x.FuelRate < 50.0);
			default:
				return true;
			}
		}

		private void OnDestroy()
		{
			SingletonMonoBehaviour<TutorialGuideManager>.instance = null;
			tutorialDialog = null;
			req = null;
			model = null;
		}
	}
}
