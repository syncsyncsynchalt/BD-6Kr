using Common.Enum;
using local.models;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIDutyStatus : MonoBehaviour
	{
		[SerializeField]
		private UISprite mProgress;

		[SerializeField]
		private UISprite[] mStars;

		[SerializeField]
		private UISprite mYousei;

		[SerializeField]
		private UISpriteAnimation mSpriteYouseiAnimation;

		[SerializeField]
		private UISprite mSprite_ClearLamp;

		private bool isYouseiAnimation;

		private bool isStarAnimation;

		public void Initialize(DutyModel dutyModel)
		{
			mProgress.spriteName = GetSpriteNameProgress(dutyModel.State, dutyModel.Progress);
			string spriteNameYouseiPrefix = GetSpriteNameYouseiPrefix(dutyModel.State, dutyModel.Progress);
			if (string.IsNullOrEmpty(spriteNameYouseiPrefix))
			{
				mYousei.spriteName = string.Empty;
				mSpriteYouseiAnimation.namePrefix = string.Empty;
				mSpriteYouseiAnimation.framesPerSecond = 0;
			}
			else
			{
				mYousei.spriteName = string.Format(spriteNameYouseiPrefix, 1);
				mSpriteYouseiAnimation.namePrefix = spriteNameYouseiPrefix;
				mSpriteYouseiAnimation.framesPerSecond = 3;
			}
			if (dutyModel.State == QuestState.COMPLETE)
			{
				UISprite[] array = mStars;
				foreach (UISprite component in array)
				{
					mSprite_ClearLamp.SetActive(isActive: true);
					component.SetActive(isActive: true);
				}
			}
			else
			{
				UISprite[] array2 = mStars;
				foreach (UISprite component2 in array2)
				{
					component2.SetActive(isActive: false);
				}
			}
		}

		private string GetSpriteNameYouseiPrefix(QuestState state, QuestProgressKinds progress)
		{
			switch (state)
			{
			case QuestState.COMPLETE:
				return "mini_06_b_0";
			case QuestState.RUNNING:
				return "mini_06_a_0";
			case QuestState.WAITING_START:
				if (progress != 0)
				{
					return string.Empty;
				}
				return string.Empty;
			default:
				return string.Empty;
			}
		}

		private string GetSpriteNameProgress(QuestState state, QuestProgressKinds progress)
		{
			switch (state)
			{
			case QuestState.COMPLETE:
				return "btn_progress_8";
			case QuestState.RUNNING:
				return GetSpriteNameRunningProgress(progress);
			case QuestState.WAITING_START:
				return GetSpriteNameWaitingProgress(progress);
			default:
				return string.Empty;
			}
		}

		private string GetSpriteNameRunningProgress(QuestProgressKinds progress)
		{
			switch (progress)
			{
			case QuestProgressKinds.MORE_THAN_50:
				return "btn_progress_5";
			case QuestProgressKinds.MORE_THAN_80:
				return "btn_progress_7";
			default:
				return "btn_progress_4";
			}
		}

		private string GetSpriteNameWaitingProgress(QuestProgressKinds progress)
		{
			switch (progress)
			{
			case QuestProgressKinds.MORE_THAN_50:
				return "btn_progress_2";
			case QuestProgressKinds.MORE_THAN_80:
				return "btn_progress_3";
			default:
				return "btn_progress_1";
			}
		}
	}
}
