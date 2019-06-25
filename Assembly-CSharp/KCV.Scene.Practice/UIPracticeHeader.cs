using DG.Tweening;
using KCV.Scene.Port;
using System.Collections;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIPracticeHeader : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_Status;

		private void Awake()
		{
			mLabel_Status.alpha = 0f;
		}

		public void UpdateHeaderText(string text)
		{
			IEnumerator routine = UpdateHeaderTextCoroutine(text);
			StartCoroutine(routine);
		}

		private IEnumerator UpdateHeaderTextCoroutine(string text)
		{
			yield return new WaitForEndOfFrame();
			DOTween.Kill(this);
			Tween labelFadeOutTween = DOVirtual.Float(mLabel_Status.alpha, 0f, 0.3f, delegate(float alpha)
			{
				this.mLabel_Status.alpha = alpha;
			});
			Tween labelFadeInTween = DOVirtual.Float(mLabel_Status.alpha, 1f, 0.3f, delegate(float alpha)
			{
				this.mLabel_Status.alpha = alpha;
			});
			labelFadeInTween.OnStart(delegate
			{
				this.mLabel_Status.text = text;
			});
			Sequence sequence = DOTween.Sequence();
			sequence.Append(labelFadeOutTween);
			sequence.Append(labelFadeInTween);
			sequence.SetId(this);
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Status);
		}
	}
}
