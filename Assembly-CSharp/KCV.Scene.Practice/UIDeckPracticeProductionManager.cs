using Common.Struct;
using DG.Tweening;
using KCV.Scene.Practice.Deck;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIDeckPracticeProductionManager : MonoBehaviour
	{
		public enum State
		{
			None,
			Production,
			EndOfPractice,
			WaitNext
		}

		[SerializeField]
		private UIDeckPracticeBanner[] mUIDeckPracticeBanner_Banners;

		[SerializeField]
		private Transform mTransform_DeckPracticeProductionArea;

		[SerializeField]
		private UIButton mButton_Next;

		[SerializeField]
		private UITexture mTexture_Frame;

		[SerializeField]
		private UIDeckPracticeProductionShipParameterResult mUIDeckPracticeProductionShipParameterResult;

		[SerializeField]
		private UIDeckPracticeShutter mUIDeckPracticeShutter;

		[SerializeField]
		private UIDeckPracticeProductionMovieClip mPrefab_UIDeckPracticeProductionMovieClip;

		private UIDeckPracticeProductionMovieClip mUIDeckPracticeProductionMovieClip;

		[SerializeField]
		private UITexture mTexture_EndMessage;

		[SerializeField]
		private TweenAlpha mTween_EndMessage;

		private UIDeckPracticeBanner[] mUIDeckPracticeBanners;

		private Action mOnFinishedProduction;

		private DeckPracticeResultModel mDeckPracticeResultModel;

		private StateManager<State> mStateManager;

		private KeyControl mKeyController;

		private Action<State> mOnChangeStateListener;

		public void SetOnChangeStateListener(Action<State> onChangeStateListener)
		{
			mOnChangeStateListener = onChangeStateListener;
		}

		private void OnChangeStateListener(State state)
		{
			if (mOnChangeStateListener != null)
			{
				mOnChangeStateListener(state);
			}
		}

		private void Awake()
		{
			mButton_Next.SetActive(isActive: false);
			mTexture_Frame.alpha = 0f;
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public IEnumerator InitializeCoroutine(DeckModel deckModel, DeckPracticeResultModel deckPracticeResultModel)
		{
			mDeckPracticeResultModel = deckPracticeResultModel;
			mUIDeckPracticeProductionShipParameterResult.Initialize(mDeckPracticeResultModel);
			yield return new WaitForEndOfFrame();
			mUIDeckPracticeProductionMovieClip = NGUITools.AddChild(mTransform_DeckPracticeProductionArea.gameObject, mPrefab_UIDeckPracticeProductionMovieClip.gameObject).GetComponent<UIDeckPracticeProductionMovieClip>();
			mUIDeckPracticeProductionMovieClip.Initialize(deckModel, deckPracticeResultModel);
			mUIDeckPracticeProductionMovieClip.transform.localPosition = Vector3.zero;
			mUIDeckPracticeProductionMovieClip.SetOnShipParameterUpEventListener(OnShipParameterUpEventListener);
			mUIDeckPracticeProductionMovieClip.SetOnFinishedProductionListener(OnFinishedProduction);
			yield return new WaitForEndOfFrame();
			mStateManager = new StateManager<State>(State.None);
			mStateManager.OnPush = OnPushState;
			mStateManager.OnSwitch = OnSwitchState;
			List<UIDeckPracticeBanner> banners = new List<UIDeckPracticeBanner>();
			for (int index = 0; index < mUIDeckPracticeBanner_Banners.Length; index++)
			{
				UIDeckPracticeBanner banner = mUIDeckPracticeBanner_Banners[index];
				banner.alpha = 0.0001f;
				if (index < deckModel.Count)
				{
					banner.Initialize(deckModel.GetShip(index));
					banners.Add(banner);
				}
			}
			mUIDeckPracticeBanners = banners.ToArray();
			yield return new WaitForEndOfFrame();
		}

		private void OnPushState(State state)
		{
			if (state == State.EndOfPractice)
			{
				OnPushEndOfPracticeState();
			}
		}

		private void OnSwitchState(State state)
		{
			OnChangeStateListener(state);
		}

		private void OnShipParameterUpEventListener(ShipModel shipModel, ShipExpModel shipExpModel, PowUpInfo powUpInfo)
		{
			UIDeckPracticeBanner uIDeckPracticeBanner = mUIDeckPracticeBanners.First((UIDeckPracticeBanner shipBanner) => shipBanner.Model.MemId == shipModel.MemId);
			if (!powUpInfo.IsAllZero())
			{
				uIDeckPracticeBanner.PlayPracticeWithLevelUp();
			}
			else
			{
				uIDeckPracticeBanner.PlayPractice();
			}
		}

		public void PlayShipBannerIn()
		{
			DOVirtual.DelayedCall(1.3f, delegate
			{
				Sequence s = DOTween.Sequence();
				for (int i = 0; i < mUIDeckPracticeBanners.Length; i++)
				{
					UIDeckPracticeBanner banner = mUIDeckPracticeBanners[i];
					Vector3 localPosition = banner.transform.localPosition;
					float x = localPosition.x;
					Transform transform = banner.transform;
					Vector3 localPosition2 = banner.transform.localPosition;
					transform.localPositionX(localPosition2.x - 80f);
					Tween t = banner.transform.DOLocalMoveX(x, 0.5f).SetEase(Ease.OutCirc);
					Tween t2 = DOVirtual.Float(banner.alpha, 1f, 0.3f, delegate(float alpha)
					{
						banner.alpha = alpha;
					});
					Sequence sequence = DOTween.Sequence();
					sequence.Join(t);
					sequence.Join(t2);
					sequence.SetDelay(0.05f);
					s.Join(sequence);
				}
			});
		}

		public void PlayProduction()
		{
			mStateManager.PushState(State.Production);
			DOVirtual.Float(mTexture_Frame.alpha, 1f, 0.5f, delegate(float alpha)
			{
				mTexture_Frame.alpha = alpha;
			}).SetDelay(0.3f);
			mUIDeckPracticeProductionMovieClip.Play();
		}

		public void SetOnFinishedProduction(Action onFinishedProduction)
		{
			mOnFinishedProduction = onFinishedProduction;
		}

		private void OnFinishedProduction()
		{
			mStateManager.PushState(State.EndOfPractice);
		}

		private void OnPushEndOfPracticeState()
		{
			if (SingletonMonoBehaviour<Live2DModel>.exist())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			}
			mUIDeckPracticeShutter.SetOnFinishedCloseShutterAnimationListener(OnClosedShutterEventForEndOfPractice);
			mUIDeckPracticeShutter.CloseShutter();
		}

		private void OnClosedShutterEventForEndOfPractice()
		{
			mUIDeckPracticeProductionMovieClip.Stop();
			mUIDeckPracticeShutter.SetOnFinishedCloseShutterAnimationListener(null);
			DOVirtual.DelayedCall(0.8f, delegate
			{
				mTween_EndMessage.SetOnFinished(delegate
				{
					ChangeNextMovableState();
				});
				mTween_EndMessage.gameObject.SetActive(true);
				mTween_EndMessage.PlayForward();
			});
			mUIDeckPracticeProductionMovieClip.SetActive(isActive: false);
		}

		protected void ChangeNextMovableState()
		{
			if (mStateManager.CurrentState == State.EndOfPractice)
			{
				mUIDeckPracticeShutter.SetOnFinishedOpenShutterAnimationListener(OnOpendShutterEventForResult);
				mUIDeckPracticeProductionShipParameterResult.SetBackGroundAlpha(1f);
				mUIDeckPracticeShutter.OpenShutter();
			}
		}

		private void OnOpendShutterEventForResult()
		{
			mUIDeckPracticeShutter.SetOnFinishedOpenShutterAnimationListener(null);
			mUIDeckPracticeProductionShipParameterResult.SetOnProductionFinishedListener(delegate
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.Strategy);
			});
			mUIDeckPracticeProductionShipParameterResult.SetKeyController(mKeyController);
			mUIDeckPracticeProductionShipParameterResult.StartProduction();
		}

		private IEnumerator WaitKeyOrTouch()
		{
			mButton_Next.SetActive(isActive: true);
			while (mKeyController != null && !mKeyController.keyState[1].down)
			{
				yield return null;
			}
			OnClickNext();
		}

		private void OnClickNext()
		{
			if (mStateManager.CurrentState == State.WaitNext)
			{
				mStateManager.PopState();
				OnMoveNext();
			}
		}

		[Obsolete("UI側から呼ぶためのメソッドです")]
		public void OnTouchNext()
		{
			if (mStateManager.CurrentState == State.WaitNext)
			{
				mStateManager.PopState();
				OnMoveNext();
			}
		}

		private void OnMoveNext()
		{
			if (mOnFinishedProduction != null)
			{
				mOnFinishedProduction();
			}
		}

		private void OnDestroy()
		{
			mPrefab_UIDeckPracticeProductionMovieClip = null;
			mUIDeckPracticeProductionMovieClip = null;
			mUIDeckPracticeBanner_Banners = null;
			mTransform_DeckPracticeProductionArea = null;
			mButton_Next = null;
			mTexture_Frame = null;
			mUIDeckPracticeProductionShipParameterResult = null;
			mUIDeckPracticeShutter = null;
			mTexture_EndMessage = null;
			mTween_EndMessage = null;
			mUIDeckPracticeBanners = null;
			mOnFinishedProduction = null;
			mDeckPracticeResultModel = null;
			mStateManager = null;
			mKeyController = null;
			mOnChangeStateListener = null;
		}
	}
}
