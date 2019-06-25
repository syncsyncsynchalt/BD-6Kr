using System;
using UnityEngine;

namespace KCV.Base
{
	[RequireComponent(typeof(UIPanel))]
	public abstract class BaseDialog : MonoBehaviour
	{
		private UIPanel mPanel;

		private void Awake()
		{
			mPanel = GetComponent<UIPanel>();
			mPanel.alpha = 0.01f;
		}

		protected void Show()
		{
			mPanel.alpha = 1f;
			new BaseDialogPopup().Open(base.gameObject, 0f, 0f, 1f, 1f);
		}

		protected void Hide(Action callBack)
		{
			AnimationClose(callBack);
		}

		private void AnimationClose(Action action)
		{
			TweenScale scaleTween = UITweener.Begin<TweenScale>(base.gameObject, 0.4f);
			Vector3 localScale = base.gameObject.transform.localScale;
			Vector3 zero = Vector3.zero;
			scaleTween.ignoreTimeScale = false;
			scaleTween.from = localScale;
			scaleTween.to = zero;
			scaleTween.SetOnFinished(delegate
			{
				if (action != null)
				{
					action();
				}
				UnityEngine.Object.Destroy(scaleTween);
			});
		}
	}
}
