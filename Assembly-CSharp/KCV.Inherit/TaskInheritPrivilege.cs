using Common.Enum;
using KCV.Utils;
using local.managers;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace KCV.Inherit
{
	public class TaskInheritPrivilege : SceneTaskMono
	{
		private enum RewardType
		{
			Difficulty,
			PortExtend,
			Syorui,
			SPoint,
			_num
		}

		private UIWidget myUIWidget;

		[SerializeField]
		private InheritRewardMessage[] rewardMessages;

		private InheritRewardMessage NowRewardMessage;

		private KeyControl key;

		[SerializeField]
		private UISprite DifficltyTex;

		protected override void Start()
		{
			key = new KeyControl();
			myUIWidget = GetComponent<UIWidget>();
			ClearCheck();
		}

		protected override bool Run()
		{
			key.Update();
			return true;
		}

		protected override bool Init()
		{
			StartCoroutine(DialogControl());
			return true;
		}

		private IEnumerator DialogControl()
		{
			yield return Util.WaitEndOfFrames(3);
			float fadeTime = 0.5f;
			if (rewardMessages.Any((InheritRewardMessage x) => x.isNeedShow))
			{
				TweenAlpha.Begin(myUIWidget.gameObject, fadeTime, 1f);
				yield return new WaitForSeconds(fadeTime);
				for (int i = 0; i < rewardMessages.Length; i++)
				{
					if (rewardMessages[i].isNeedShow)
					{
						if (NowRewardMessage != null)
						{
							TweenAlpha.Begin(NowRewardMessage.gameObject, fadeTime, 0f);
							yield return new WaitForSeconds(fadeTime);
						}
						SoundUtils.PlaySE(SEFIleInfos.SE_027);
						NowRewardMessage = rewardMessages[i];
						TweenAlpha.Begin(NowRewardMessage.gameObject, fadeTime, 1f);
						yield return new WaitForSeconds(fadeTime);
						yield return StartCoroutine(WaitForKey(KeyControl.KeyName.MARU, KeyControl.KeyName.BATU));
					}
				}
				TweenAlpha.Begin(NowRewardMessage.gameObject, fadeTime, 0f);
				TweenAlpha.Begin(myUIWidget.gameObject, fadeTime, 0f);
				yield return new WaitForSeconds(fadeTime);
			}
			yield return new WaitForSeconds(0.5f);
			InheritLoadTaskManager.ReqMode(InheritLoadTaskManager.InheritTaskManagerMode.InheritTaskManagerMode_ST);
			yield return null;
		}

		private IEnumerator WaitForKey(params KeyControl.KeyName[] keyNames)
		{
			while (true)
			{
				for (int i = 0; i < keyNames.Length; i++)
				{
					if (key.keyState[(int)keyNames[i]].down)
					{
						yield break;
					}
				}
				yield return null;
			}
		}

		private void ClearCheck()
		{
			if (App.GetTitleManager() == null)
			{
				App.SetTitleManager(new TitleManager());
			}
			DifficultKind? openedDifficulty = App.GetTitleManager().GetOpenedDifficulty();
			Debug.Log(openedDifficulty);
			if (openedDifficulty.HasValue)
			{
				int value = (int)openedDifficulty.Value;
				rewardMessages[0].isNeedShow = true;
				DifficltyTex.spriteName = "txt_diff" + value;
			}
			rewardMessages[1].isNeedShow = true;
		}

		private void OnDestroy()
		{
			myUIWidget = null;
			rewardMessages = null;
			NowRewardMessage = null;
			key = null;
			DifficltyTex = null;
		}
	}
}
