using System;
using UnityEngine;

namespace KCV.Base
{
	[RequireComponent(typeof(UIPanel))]
	public class PageDialog : MonoBehaviour
	{
		protected UIPanel mPanel;

		private Vector3 mStartLocalPosition;

		private Vector3 mOutDisplayPosition;

		private void Awake()
		{
			mPanel = GetComponent<UIPanel>();
			mPanel.alpha = 0.01f;
			mStartLocalPosition = base.gameObject.transform.localPosition;
			mOutDisplayPosition = new Vector3(mStartLocalPosition.x + 960f, mStartLocalPosition.y, 0f);
		}

		private void Start()
		{
			base.transform.localPosition = mOutDisplayPosition;
		}

		protected void Show()
		{
			mPanel.alpha = 1f;
			TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.transform.gameObject, 0.4f);
			tweenPosition.from = mOutDisplayPosition;
			tweenPosition.to = mStartLocalPosition;
			tweenPosition.ignoreTimeScale = false;
			tweenPosition.SetOnFinished(delegate
			{
				FinishedShowAnimation();
			});
		}

		protected void Show(Vector3 to)
		{
			mPanel.alpha = 1f;
			TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.transform.gameObject, 0.4f);
			tweenPosition.from = mOutDisplayPosition;
			tweenPosition.to = to;
			tweenPosition.ignoreTimeScale = false;
			tweenPosition.SetOnFinished(delegate
			{
				FinishedShowAnimation();
			});
		}

		private void FinishedShowAnimation()
		{
			OnFinishedShowAnimation();
		}

		protected virtual void OnFinishedShowAnimation()
		{
		}

		protected void Hide(Action callBack)
		{
			AnimationClose(callBack);
		}

		private void AnimationClose(Action action)
		{
			TweenPosition moveTween = UITweener.Begin<TweenPosition>(base.gameObject, 0.4f);
			Vector3 from = mStartLocalPosition;
			Vector3 to = mOutDisplayPosition;
			moveTween.ignoreTimeScale = false;
			moveTween.from = from;
			moveTween.to = to;
			moveTween.SetOnFinished(delegate
			{
				if (action != null)
				{
					action();
				}
				UnityEngine.Object.Destroy(moveTween);
			});
		}
	}
}
