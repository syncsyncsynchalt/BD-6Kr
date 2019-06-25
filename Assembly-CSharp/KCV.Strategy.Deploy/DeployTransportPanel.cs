using KCV.EscortOrganize;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy.Deploy
{
	public class DeployTransportPanel : MonoBehaviour
	{
		[SerializeField]
		private UISprite upButton;

		[SerializeField]
		private UISprite downButton;

		[SerializeField]
		private UILabel mLabel_UsedTankerValue;

		[SerializeField]
		private UILabel needTankerNum;

		[SerializeField]
		private UILabel mLabel_SetTankerValue;

		[SerializeField]
		private DeployMaterials deployMaterials;

		[SerializeField]
		private TaskDeployTop top;

		private KeyControl TransportKeyController;

		private StrategyMapManager mStrategyMapManager;

		private int mMaximumSetTankerValue;

		private int mMinimumSetTankerValue;

		private int mSetTankerValue;

		private int mAlreadySetTankerValue;

		private int mOwnUsableTankerValue;

		public void Init()
		{
			TransportKeyController = new KeyControl();
			mStrategyMapManager = StrategyTopTaskManager.GetLogicManager();
			mMinimumSetTankerValue = 0;
			mMaximumSetTankerValue = 30;
			mAlreadySetTankerValue = mStrategyMapManager.Area[top.areaID].GetTankerCount().GetCount();
			mOwnUsableTankerValue = mStrategyMapManager.GetNonDeploymentTankerCount().GetCountNoMove();
			if (mOwnUsableTankerValue + mAlreadySetTankerValue < 30)
			{
				mMaximumSetTankerValue = mOwnUsableTankerValue + mAlreadySetTankerValue;
			}
			needTankerNum.textInt = mStrategyMapManager.Area[top.areaID].GetTankerCount().GetReqCount();
			mSetTankerValue = mAlreadySetTankerValue + (top.TankerCount - mAlreadySetTankerValue);
			OnUpdatedTankerValue(mSetTankerValue);
			base.gameObject.SafeGetTweenAlpha(0f, 1f, 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
			top.isChangeMode = false;
			TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
			if (SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(tutorial, TutorialGuideManager.TutorialID.TankerDeploy, null, delegate
			{
				TransportKeyController.IsRun = true;
			}))
			{
				TransportKeyController.IsRun = false;
			}
		}

		public bool Run()
		{
			TransportKeyController.Update();
			if (TransportKeyController.keyState[1].down || TransportKeyController.keyState[0].down)
			{
				Back();
			}
			else if (TransportKeyController.keyState[8].down)
			{
				UpTankerValue();
			}
			else if (TransportKeyController.keyState[12].down)
			{
				DownTankerValue();
			}
			else if (TransportKeyController.keyState[5].down)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			return true;
		}

		[Obsolete("外部UI[輸送船増ボタン]から参照して使用します")]
		public void OnClickUpTankerValue()
		{
			UpTankerValue();
		}

		[Obsolete("外部UI[輸送船減ボタン]から参照して使用します")]
		public void OnClickDownTankerValue()
		{
			DownTankerValue();
		}

		[Obsolete("外部UI[バックボタン（背景）]から参照して使用します")]
		public void OnClickBack()
		{
			Back();
		}

		private void UpTankerValue()
		{
			if (TransportKeyController.IsRun && RangeTanker(mSetTankerValue + 1, mMinimumSetTankerValue, mMaximumSetTankerValue))
			{
				mSetTankerValue++;
				OnUpdatedTankerValue(mSetTankerValue);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		private void DownTankerValue()
		{
			if (TransportKeyController.IsRun && RangeTanker(mSetTankerValue - 1, mMinimumSetTankerValue, mMaximumSetTankerValue))
			{
				mSetTankerValue--;
				OnUpdatedTankerValue(mSetTankerValue);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		private void OnUpdatedTankerValue(int tankerValue)
		{
			mLabel_SetTankerValue.text = tankerValue.ToString();
			int num = mOwnUsableTankerValue + (mAlreadySetTankerValue - tankerValue);
			mLabel_UsedTankerValue.text = num.ToString();
			deployMaterials.updateMaterials(top.areaID, tankerValue, EscortOrganizeTaskManager.GetEscortManager());
		}

		private bool RangeTanker(int checkValue, int minimumValue, int maximumValue)
		{
			if (minimumValue <= checkValue && checkValue <= maximumValue)
			{
				return true;
			}
			return false;
		}

		private void Back()
		{
			if (TransportKeyController.IsRun && top.isDeployPanel)
			{
				base.gameObject.SafeGetTweenAlpha(1f, 0f, 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
				top.TankerCount = mSetTankerValue;
				top.isDeployPanel = false;
				top.isChangeMode = true;
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			}
		}
	}
}
