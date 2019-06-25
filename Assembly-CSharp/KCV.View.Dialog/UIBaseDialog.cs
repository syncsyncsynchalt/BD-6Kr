using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.View.Dialog
{
	[RequireComponent(typeof(UIPanel))]
	public abstract class UIBaseDialog : MonoBehaviour
	{
		public enum EventType
		{
			BeginInitialize,
			BeginShow,
			BeginHide,
			Initialized,
			Shown,
			Hidden
		}

		private const float HIDE_ANIMATION_TIME = 0.4f;

		[SerializeField]
		private Vector3 SHOW_START_SCALE = new Vector3(0.9f, 0.9f, 1f);

		[SerializeField]
		private Vector3 SHOWN_SCALE = new Vector3(1f, 1f, 1f);

		private Coroutine mInitializeCoroutine;

		private Coroutine mShowCoroutine;

		private Coroutine mHideCoroutine;

		private UIPanel mPanelThis;

		private void Awake()
		{
			DOTween.Init();
			mPanelThis = GetComponent<UIPanel>();
			mPanelThis.alpha = 0.01f;
		}

		public void Begin()
		{
			RemoveInitializeCoroutine();
			mInitializeCoroutine = StartCoroutine(InitializeCoroutine(delegate
			{
				RemoveInitializeCoroutine();
				OnCallEventCoroutine(EventType.Initialized, this);
			}));
		}

		public void Show()
		{
			RemoveShowCoroutine();
			mShowCoroutine = StartCoroutine(ShowCoroutine(delegate
			{
				RemoveShowCoroutine();
				base.transform.localScale = SHOW_START_SCALE;
				base.transform.DOScale(SHOWN_SCALE, 0.4f).SetEase(Ease.OutCirc).OnComplete(delegate
				{
					OnCallEventCoroutine(EventType.Shown, this);
				});
				DOVirtual.Float(mPanelThis.alpha, 1f, 0.4f, delegate(float alpha)
				{
					mPanelThis.alpha = alpha;
				});
			}));
		}

		public void Hide()
		{
			RemoveHideCoroutine();
			mHideCoroutine = StartCoroutine(HideCoroutine(delegate
			{
				RemoveHideCoroutine();
				base.transform.DOScale(SHOW_START_SCALE, 0.4f).SetEase(Ease.InCirc).OnComplete(delegate
				{
					OnCallEventCoroutine(EventType.Hidden, this);
				});
				DOVirtual.Float(mPanelThis.alpha, 0.01f, 0.4f, delegate(float alpha)
				{
					mPanelThis.alpha = alpha;
				});
			}));
		}

		private IEnumerator InitializeCoroutine(Action onFinished)
		{
			yield return OnCallEventCoroutine(EventType.BeginInitialize, this);
			onFinished?.Invoke();
		}

		private IEnumerator ShowCoroutine(Action onFinished)
		{
			yield return OnCallEventCoroutine(EventType.BeginShow, this);
			onFinished?.Invoke();
		}

		private IEnumerator HideCoroutine(Action onFinished)
		{
			yield return OnCallEventCoroutine(EventType.BeginHide, this);
			onFinished?.Invoke();
		}

		private void RemoveShowCoroutine()
		{
			if (mShowCoroutine != null)
			{
				StopCoroutine(mShowCoroutine);
				mShowCoroutine = null;
			}
		}

		private void RemoveInitializeCoroutine()
		{
			if (mInitializeCoroutine != null)
			{
				StopCoroutine(mInitializeCoroutine);
				mInitializeCoroutine = null;
			}
		}

		private void RemoveHideCoroutine()
		{
			if (mHideCoroutine != null)
			{
				StopCoroutine(mHideCoroutine);
				mHideCoroutine = null;
			}
		}

		protected abstract Coroutine OnCallEventCoroutine(EventType actionType, UIBaseDialog calledObject);
	}
}
