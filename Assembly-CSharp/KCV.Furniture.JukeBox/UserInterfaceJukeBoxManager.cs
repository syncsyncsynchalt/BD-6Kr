using DG.Tweening;
using KCV.Utils;
using local.managers;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Furniture.JukeBox
{
	[RequireComponent(typeof(UIPanel))]
	public class UserInterfaceJukeBoxManager : MonoBehaviour
	{
		public enum State
		{
			NONE,
			MusicSelect,
			BuyMusicConfirm,
			Playing
		}

		public class StateManager<State>
		{
			private Stack<State> mStateStack;

			private State mEmptyState;

			public Action<State> OnPush
			{
				private get;
				set;
			}

			public Action<State> OnPop
			{
				private get;
				set;
			}

			public Action<State> OnResume
			{
				private get;
				set;
			}

			public Action<State> OnSwitch
			{
				private get;
				set;
			}

			public State CurrentState
			{
				get
				{
					if (0 < mStateStack.Count)
					{
						return mStateStack.Peek();
					}
					return mEmptyState;
				}
			}

			public StateManager(State emptyState)
			{
				mEmptyState = emptyState;
				mStateStack = new Stack<State>();
			}

			public void PushState(State state)
			{
				mStateStack.Push(state);
				Notify(OnPush, mStateStack.Peek());
				Notify(OnSwitch, mStateStack.Peek());
			}

			public void ReplaceState(State state)
			{
				if (0 < mStateStack.Count)
				{
					PopState();
				}
				mStateStack.Push(state);
				Notify(OnPush, mStateStack.Peek());
				Notify(OnSwitch, mStateStack.Peek());
			}

			public void PopState()
			{
				if (0 < mStateStack.Count)
				{
					State state = mStateStack.Pop();
					Notify(OnPop, state);
				}
			}

			public void ResumeState()
			{
				if (0 < mStateStack.Count)
				{
					Notify(OnResume, mStateStack.Peek());
					Notify(OnSwitch, mStateStack.Peek());
				}
			}

			public override string ToString()
			{
				mStateStack.ToArray();
				string text = string.Empty;
				foreach (State item in mStateStack)
				{
					text = item + " > " + text;
				}
				return text;
			}

			private void Notify(Action<State> target, State state)
			{
				target?.Invoke(state);
			}
		}

		public class Context
		{
			private Mst_bgm_jukebox mBgmJukeBox;

			public void SetJukeBoxBGM(Mst_bgm_jukebox jukeBoxBGM)
			{
				mBgmJukeBox = jukeBoxBGM;
			}

			public Mst_bgm_jukebox GetJukeBoxBGM()
			{
				return mBgmJukeBox;
			}
		}

		private BGMFileInfos mConfiguredBGM;

		[SerializeField]
		private UIJukeBoxMusicBuyConfirm mUIJukeBoxMusicBuyConfirm;

		[SerializeField]
		private UIJukeBoxPlayListParent mUIJukeBoxPlayListParent;

		[SerializeField]
		private UIJukeBoxMusicPlayingDialog mUIJukeBoxMusicPlayingDialog;

		private Context mContext;

		private UIPanel mPanelThis;

		private StateManager<State> mStateManager;

		private PortManager mPortManager;

		private KeyControl mKeyController;

		private IEnumerator mPlayBGMWithCrossFadeCoroutine;

		private float mDefaultVolume;

		private int mDeckId;

		private Camera mOverlayCamera;

		private Action mOnBackListener;

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			mPanelThis.alpha = 0f;
			mUIJukeBoxMusicBuyConfirm.SetOnSelectNegativeListener(OnSelectNegativeJukeBoxMusicBuy);
			mUIJukeBoxMusicBuyConfirm.SetOnSelectPositiveListener(OnSelectPositiveJukeBoxMusicBuy);
			mUIJukeBoxMusicBuyConfirm.SetOnRequestChangeScene(OnRequestChangeScene);
			mUIJukeBoxMusicPlayingDialog.SetOnSelectNegativeListener(OnSelectStopPlay);
			mUIJukeBoxMusicPlayingDialog.SetOnSelectPositiveListener(OnSelectSettingBgmThis);
			mUIJukeBoxMusicPlayingDialog.SetOnRequestChangeScene(OnRequestChangeScene);
			mUIJukeBoxPlayListParent.SetOnSelectedMusicListener(OnSelectedMusicListener);
			mUIJukeBoxPlayListParent.SetOnBackListener(OnBack);
			mUIJukeBoxPlayListParent.SetOnRequestChangeScene(OnRequestChangeScene);
			mUIJukeBoxMusicBuyConfirm.SetOnRequestBackToRoot(OnRequestBackToRoot);
			mUIJukeBoxMusicPlayingDialog.SetOnRequestBackToRoot(OnRequestBackToRoot);
			mUIJukeBoxPlayListParent.SetOnRequestBackToRoot(OnRequestBackToRoot);
		}

		private void OnRequestBackToRoot()
		{
			while (mStateManager.CurrentState != 0)
			{
				switch (mStateManager.CurrentState)
				{
				case State.Playing:
					mUIJukeBoxMusicPlayingDialog.CloseState();
					CrossFadeToPortBGM();
					break;
				case State.MusicSelect:
					mUIJukeBoxPlayListParent.CloseState();
					break;
				case State.BuyMusicConfirm:
					mUIJukeBoxMusicBuyConfirm.CloseState();
					break;
				}
				mStateManager.PopState();
			}
			OnBack();
		}

		private void OnSelectedMusicListener(Mst_bgm_jukebox jukeBoxBGM)
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			if (jukeBoxBGM.R_coins <= mPortManager.UserInfo.FCoin)
			{
				mUIJukeBoxPlayListParent.LockState();
				mUIJukeBoxPlayListParent.SetKeyController(null);
				mContext.SetJukeBoxBGM(jukeBoxBGM);
				mStateManager.PushState(State.BuyMusicConfirm);
			}
			else
			{
				CommonPopupDialog.Instance.StartPopup("家具コインが不足しています");
			}
		}

		private void OnSelectSettingBgmThis()
		{
			if (mStateManager.CurrentState == State.Playing)
			{
				int bgm_id = mContext.GetJukeBoxBGM().Bgm_id;
				mPortManager.SetPortBGM(mDeckId, bgm_id);
				BGMFileInfos bGMFileInfos = mConfiguredBGM = (BGMFileInfos)bgm_id;
				SingletonMonoBehaviour<SoundManager>.Instance.bgmSource.loop = true;
				mUIJukeBoxMusicPlayingDialog.CloseState();
				OnBack();
			}
		}

		private void OnSelectStopPlay()
		{
			if (mStateManager.CurrentState == State.Playing)
			{
				mKeyController.ClearKeyAll();
				mKeyController.firstUpdate = true;
				if (mPlayBGMWithCrossFadeCoroutine != null)
				{
					StopCoroutine(mPlayBGMWithCrossFadeCoroutine);
					mPlayBGMWithCrossFadeCoroutine = null;
				}
				CrossFadeToPortBGM();
				mUIJukeBoxMusicPlayingDialog.CloseState();
				mStateManager.PopState();
				mStateManager.ResumeState();
			}
		}

		private void PlayBGMWithCrossFade(Mst_bgm_jukebox jukeBoxBGM, Action onFinished)
		{
			if (mPlayBGMWithCrossFadeCoroutine != null)
			{
				StopCoroutine(mPlayBGMWithCrossFadeCoroutine);
				mPlayBGMWithCrossFadeCoroutine = null;
			}
			mPlayBGMWithCrossFadeCoroutine = CrossFadeToBGMCoroutine(jukeBoxBGM, onFinished);
			StartCoroutine(mPlayBGMWithCrossFadeCoroutine);
		}

		private IEnumerator CrossFadeToBGMCoroutine(Mst_bgm_jukebox jukeBoxBGM, Action onFinishedPlayBGM)
		{
			if (DOTween.IsTweening(SingletonMonoBehaviour<SoundManager>.Instance))
			{
				DOTween.Kill(SingletonMonoBehaviour<SoundManager>.Instance, complete: true);
			}
			BGMFileInfos bgmFileInfos = (BGMFileInfos)jukeBoxBGM.Bgm_id;
			yield return new WaitForEndOfFrame();
			Tween fadeOutVolume = DOVirtual.Float(mDefaultVolume, 0f, 0.3f, delegate(float volume)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.bgmSource.volume = volume;
			});
			yield return fadeOutVolume.WaitForCompletion();
			AudioSource jukeAudioSource = SingletonMonoBehaviour<SoundManager>.Instance.GeneratePlayJukeAudioSource(bgmFileInfos);
			jukeAudioSource.volume = mDefaultVolume;
			jukeAudioSource.Stop();
			jukeAudioSource.Play();
			int loopCounter = 0;
			jukeAudioSource.loop = false;
			for (; loopCounter < jukeBoxBGM.Loops; loopCounter++)
			{
				jukeAudioSource.Stop();
				jukeAudioSource.Play();
				while (jukeAudioSource.isPlaying)
				{
					yield return null;
				}
			}
			onFinishedPlayBGM?.Invoke();
		}

		private void CrossFadeToPortBGM()
		{
            float volume2 = SingletonMonoBehaviour<SoundManager>.Instance.bgmSource.volume;
			if (DOTween.IsTweening(SingletonMonoBehaviour<SoundManager>.Instance))
			{
				DOTween.Kill(SingletonMonoBehaviour<SoundManager>.Instance, complete: true);
			}
			Sequence s = DOTween.Sequence().SetId(SingletonMonoBehaviour<SoundManager>.Instance);
			Tween t = DOVirtual.Float(mDefaultVolume, 0f, 0.3f, delegate(float volume)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.bgmSource.volume = volume;
			}).SetId(SingletonMonoBehaviour<SoundManager>.Instance);
			TweenCallback action = delegate
			{
				SingletonMonoBehaviour<SoundManager>.Instance.bgmSource.Stop();
				SoundUtils.PlaySceneBGM(mConfiguredBGM);
			};
			Tween t2 = DOVirtual.Float(SingletonMonoBehaviour<SoundManager>.Instance.bgmSource.volume, mDefaultVolume, 0.3f, delegate(float volume)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.bgmSource.volume = volume;
			}).OnPlay(action).SetId(SingletonMonoBehaviour<SoundManager>.Instance);
			s.Append(t);
			s.Append(t2);
		}

		public override string ToString()
		{
			if (mStateManager != null)
			{
				Debug.Log(mStateManager.ToString());
			}
			return base.ToString();
		}

		private void OnSelectNegativeJukeBoxMusicBuy()
		{
			if (mStateManager.CurrentState == State.BuyMusicConfirm)
			{
				mKeyController.ClearKeyAll();
				mKeyController.firstUpdate = true;
				mStateManager.PopState();
				mStateManager.ResumeState();
			}
		}

		private void OnSelectPositiveJukeBoxMusicBuy()
		{
			if (mStateManager.CurrentState == State.BuyMusicConfirm)
			{
				Mst_bgm_jukebox jukeBoxBGM = mContext.GetJukeBoxBGM();
				int fCoin = mPortManager.UserInfo.FCoin;
				if (jukeBoxBGM.R_coins <= fCoin)
				{
					mPortManager.PlayJukeboxBGM(mDeckId, jukeBoxBGM.Bgm_id);
					mKeyController.ClearKeyAll();
					mKeyController.firstUpdate = true;
					mStateManager.PopState();
					mStateManager.PushState(State.Playing);
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup("家具コインが不足しています");
				}
			}
		}

		public void Initialize(PortManager portManager, int deckId, Camera overlayCamera)
		{
			mPortManager = portManager;
			mOverlayCamera = overlayCamera;
			mDeckId = deckId;
			mConfiguredBGM = (BGMFileInfos)mPortManager.UserInfo.GetPortBGMId(deckId);
			mContext = new Context();
			mDefaultVolume = SingletonMonoBehaviour<SoundManager>.Instance.bgmSource.volume;
			mStateManager = new StateManager<State>(State.NONE);
			mStateManager.OnPush = OnPushState;
			mStateManager.OnPop = OnPopState;
			mStateManager.OnResume = OnResumeState;
			mUIJukeBoxPlayListParent.Initialize(mPortManager, mPortManager.GetJukeboxList().ToArray(), mOverlayCamera);
			mUIJukeBoxPlayListParent.StartState();
		}

		public void Release()
		{
			mUIJukeBoxMusicBuyConfirm.Release();
			mUIJukeBoxMusicPlayingDialog.Release();
			mPanelThis = null;
			mStateManager = null;
			mPortManager = null;
			mUIJukeBoxPlayListParent = null;
			mUIJukeBoxMusicPlayingDialog = null;
			mKeyController = null;
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchBack()
		{
			bool flag = mStateManager.CurrentState == State.BuyMusicConfirm;
			if (flag | (mStateManager.CurrentState == State.MusicSelect))
			{
				mStateManager.PopState();
				mStateManager.ResumeState();
			}
		}

		public void SetOnBackListener(Action onBackListener)
		{
			mOnBackListener = onBackListener;
		}

		private void OnBack()
		{
			if (mOnBackListener != null)
			{
				mOnBackListener();
			}
			mStateManager.PopState();
		}

		private void OnRequestChangeScene()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public void StartState()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			DOVirtual.Float(mPanelThis.alpha, 1f, 0.3f, delegate(float alpha)
			{
				mPanelThis.alpha = alpha;
			}).SetId(this);
			mStateManager.PushState(State.MusicSelect);
		}

		public void CloseState()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			DOVirtual.Float(mPanelThis.alpha, 0f, 0.3f, delegate(float alpha)
			{
				mPanelThis.alpha = alpha;
			}).SetId(this);
			mUIJukeBoxPlayListParent.LockState();
			mUIJukeBoxPlayListParent.SetKeyController(null);
		}

		private void OnPopState(State state)
		{
			if (state == State.BuyMusicConfirm)
			{
				mUIJukeBoxMusicBuyConfirm.CloseState();
			}
		}

		private void OnPushState(State state)
		{
			switch (state)
			{
			case State.MusicSelect:
				OnPushMusicSelectState();
				break;
			case State.BuyMusicConfirm:
				OnPushBuyMusicConfirmState();
				break;
			case State.Playing:
				OnPushPlayingState();
				break;
			}
		}

		private void OnResumeState(State state)
		{
			if (state == State.MusicSelect)
			{
				mUIJukeBoxPlayListParent.SetKeyController(mKeyController);
				mUIJukeBoxPlayListParent.ResumeState();
			}
		}

		private void OnPushPlayingState()
		{
			Mst_bgm_jukebox jukeBoxBGM = mContext.GetJukeBoxBGM();
			PlayBGMWithCrossFade(jukeBoxBGM, OnSelectStopPlay);
			mUIJukeBoxMusicPlayingDialog.Initialize(mContext.GetJukeBoxBGM());
			mUIJukeBoxMusicPlayingDialog.SetKeyController(mKeyController);
			mUIJukeBoxMusicPlayingDialog.StartState();
		}

		private void OnPushBuyMusicConfirmState()
		{
			mUIJukeBoxMusicBuyConfirm.Initialize(mContext.GetJukeBoxBGM(), mPortManager.UserInfo.FCoin, isValidBuy: true);
			mUIJukeBoxMusicBuyConfirm.SetKeyController(mKeyController);
			mUIJukeBoxMusicBuyConfirm.StartState();
		}

		private void OnPushMusicSelectState()
		{
			mUIJukeBoxPlayListParent.Refresh(mPortManager, mPortManager.GetJukeboxList().ToArray(), mOverlayCamera);
			mUIJukeBoxPlayListParent.SetKeyController(mKeyController);
			mUIJukeBoxPlayListParent.StartState();
		}
	}
}
