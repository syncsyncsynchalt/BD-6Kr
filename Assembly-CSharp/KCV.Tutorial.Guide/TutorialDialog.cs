using KCV.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Tutorial.Guide
{
	public class TutorialDialog : MonoBehaviour
	{
		private DialogAnimation dialogAnimation;

		public Transform View;

		private UIPanel panel;

		private KeyControl key;

		public Action OnClosed;

		private Action OnLoaded;

		[SerializeField]
		private UIWidget[] page;

		private int nowPage;

		[SerializeField]
		private Blur Blur;

		private bool isClosed;

		private TutorialGuideManager.TutorialID tutorialId;

		private AudioSource playingTutorialVoiceAudioClip;

		public void SetOnLoaded(Action onloaded)
		{
			OnLoaded = onloaded;
		}

		public bool getIsClosed()
		{
			return isClosed;
		}

		private void Awake()
		{
			dialogAnimation = ((Component)base.transform.FindChild("Dialog")).GetComponent<DialogAnimation>();
			panel = GetComponent<UIPanel>();
			nowPage = 0;
			if (Blur != null)
			{
				Blur.enabled = false;
			}
		}

		private void OnDestroy()
		{
			if ((UnityEngine.Object)playingTutorialVoiceAudioClip != null)
			{
				playingTutorialVoiceAudioClip.Stop();
			}
			playingTutorialVoiceAudioClip = null;
			dialogAnimation = null;
			View = null;
			panel = null;
			key = null;
			OnClosed = null;
			OnLoaded = null;
			for (int i = 0; i < page.Length; i++)
			{
				page[i] = null;
			}
			Mem.DelAry(ref page);
			Blur = null;
		}

		internal void SetTutorialId(TutorialGuideManager.TutorialID tutorialId)
		{
			this.tutorialId = tutorialId;
		}

		private IEnumerator Start()
		{
			key = new KeyControl();
			key.IsRun = false;
			App.OnlyController = key;
			key.firstUpdate = true;
			if (OnLoaded != null)
			{
				OnLoaded();
			}
			yield return StartCoroutine(Util.WaitEndOfFrames(3));
			Show(tutorialId, delegate
			{
				this.key.IsRun = true;
			});
			SoundUtils.PlaySE(SEFIleInfos.SE_027);
		}

		private void Update()
		{
			if (key != null)
			{
				key.Update();
				if (key.IsMaruDown() || key.IsBatuDown() || key.IsShikakuDown() || key.IsSankakuDown())
				{
					NextPage();
				}
			}
		}

		public void Show(TutorialGuideManager.TutorialID tutorialId, Action OnFinished)
		{
			Time.timeScale = 1f;
			dialogAnimation.OpenAction = delegate
			{
				if (OnFinished != null)
				{
					if ((UnityEngine.Object)playingTutorialVoiceAudioClip != null)
					{
						playingTutorialVoiceAudioClip.Stop();
					}
					playingTutorialVoiceAudioClip = PlayTutorialVoice(tutorialId, 0);
					OnFinished();
				}
			};
			dialogAnimation.fadeTime = 0.5f;
			dialogAnimation.StartAnim(DialogAnimation.AnimType.FEAD, isOpen: true);
		}

		private AudioSource PlayTutorialVoice(TutorialGuideManager.TutorialID tutorialId, int pageIndex)
		{
			AudioClip val = RequestTutorialVoice(tutorialId, pageIndex);
			if ((UnityEngine.Object)val != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(val);
			}
			return null;
		}

		private AudioClip RequestTutorialVoice(TutorialGuideManager.TutorialID tutorialId, int pageIndex)
		{
			int voiceNum = TutorialIdToVoiceId(tutorialId, pageIndex);
			return SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(0, voiceNum);
		}

		private int TutorialIdToVoiceId(TutorialGuideManager.TutorialID tutorialId, int pageIndex)
		{
			switch (tutorialId)
			{
			case TutorialGuideManager.TutorialID.StrategyText:
				return -1;
			case TutorialGuideManager.TutorialID.PortTopText:
				return -1;
			case TutorialGuideManager.TutorialID.BattleCommand:
				return -1;
			case TutorialGuideManager.TutorialID.RepairInfo:
				return 415;
			case TutorialGuideManager.TutorialID.SupplyInfo:
				return 416;
			case TutorialGuideManager.TutorialID.StrategyPoint:
				return 417;
			case TutorialGuideManager.TutorialID.BattleShortCutInfo:
				return 418;
			case TutorialGuideManager.TutorialID.Raider:
				return 421;
			case TutorialGuideManager.TutorialID.RebellionPreparation:
				return 422;
			case TutorialGuideManager.TutorialID.Rebellion_EnableIntercept:
				return 425;
			case TutorialGuideManager.TutorialID.Rebellion_DisableIntercept:
				return 423;
			case TutorialGuideManager.TutorialID.Rebellion_CombinedFleet:
				switch (pageIndex)
				{
				case 0:
					return 426;
				case 1:
					return 427;
				default:
					return -1;
				}
			case TutorialGuideManager.TutorialID.Rebellion_Lose:
				return 424;
			case TutorialGuideManager.TutorialID.ResourceRecovery:
				switch (pageIndex)
				{
				case 0:
					return 428;
				case 1:
					return 429;
				default:
					return -1;
				}
			case TutorialGuideManager.TutorialID.TankerDeploy:
				return 413;
			case TutorialGuideManager.TutorialID.EscortOrganize:
				return 414;
			case TutorialGuideManager.TutorialID.Bring:
				switch (pageIndex)
				{
				case 0:
					return 419;
				case 1:
					return 420;
				default:
					return -1;
				}
			case TutorialGuideManager.TutorialID.BuildShip:
				return 404;
			case TutorialGuideManager.TutorialID.SpeedBuild:
				return 406;
			case TutorialGuideManager.TutorialID.Organize:
				return -1;
			case TutorialGuideManager.TutorialID.EndGame:
				return -1;
			default:
				return -1;
			}
		}

		public void Hide(Action OnFinished)
		{
			if (key.IsRun)
			{
				if ((UnityEngine.Object)playingTutorialVoiceAudioClip != null)
				{
					playingTutorialVoiceAudioClip.Stop();
				}
				playingTutorialVoiceAudioClip = null;
				dialogAnimation.CloseAction = delegate
				{
					if (OnClosed != null)
					{
						OnClosed();
					}
					isClosed = true;
					UnityEngine.Object.Destroy(base.gameObject);
				};
				dialogAnimation.fadeTime = 0.5f;
				dialogAnimation.StartAnim(DialogAnimation.AnimType.FEAD, isOpen: false);
				App.OnlyController = null;
				App.isFirstUpdate = true;
				key.IsRun = false;
			}
		}

		public void NextPage()
		{
			if (page != null && nowPage < page.Length - 1)
			{
				key.IsRun = false;
				PageChange();
			}
			else
			{
				Hide(null);
			}
		}

		private void PageChange()
		{
			TweenAlpha.Begin(page[nowPage].gameObject, 0.5f, 0f);
			nowPage++;
			if ((UnityEngine.Object)playingTutorialVoiceAudioClip != null && playingTutorialVoiceAudioClip.isPlaying)
			{
				playingTutorialVoiceAudioClip.Stop();
			}
			playingTutorialVoiceAudioClip = PlayTutorialVoice(tutorialId, nowPage);
			TweenAlpha.Begin(page[nowPage].gameObject, 0.5f, 1f);
			this.DelayAction(0.5f, delegate
			{
				key.IsRun = true;
			});
		}

		public IEnumerator WaitForDialogClosed()
		{
			while (!isClosed)
			{
				yield return new WaitForEndOfFrame();
			}
		}
	}
}
