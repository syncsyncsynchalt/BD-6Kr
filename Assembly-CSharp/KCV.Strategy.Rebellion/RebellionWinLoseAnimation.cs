using KCV.Utils;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class RebellionWinLoseAnimation : MonoBehaviour
	{
		[SerializeField]
		private Transform gei;

		[SerializeField]
		private Transform geki;

		[SerializeField]
		private Transform shippai;

		[SerializeField]
		private Transform seikou;

		[SerializeField]
		private Transform obi;

		[SerializeField]
		private Transform Labels;

		[Button("StartAnimation", "StartAnimation", new object[]
		{
			true
		})]
		public int button1;

		public iTween.EaseType ease;

		public iTween.EaseType ease2;

		private Vector3 scale;

		public Coroutine StartAnimation(bool isWin)
		{
			return StartCoroutine(StartAnimationCor(isWin));
		}

		private IEnumerator StartAnimationCor(bool isWin)
		{
			Transform resultLabel = (!isWin) ? shippai : seikou;
			resultLabel.SetActive(isActive: true);
			scale = new Vector3(7f, 7f, 7f);
			gei.localScale = Vector3.zero;
			geki.localScale = Vector3.zero;
			resultLabel.localScale = Vector3.zero;
			Labels.localPosition = Vector3.zero;
			GetComponent<UIWidget>().alpha = 1f;
			Vector3 obiDefaultScale = new Vector3(1f, 0f, 1f);
			obi.localScale = obiDefaultScale;
			obi.ScaleTo(Vector3.one, 0.5f, null);
			yield return new WaitForSeconds(0.5f);
			gei.localScale = scale;
			gei.ScaleTo(Vector3.one, 0.5f, ease, null);
			yield return new WaitForSeconds(0.5f);
			SoundUtils.PlaySE(SEFIleInfos.SE_044);
			geki.localScale = scale;
			geki.ScaleTo(Vector3.one, 0.5f, ease, null);
			yield return new WaitForSeconds(0.5f);
			SoundUtils.PlaySE(SEFIleInfos.SE_044);
			yield return new WaitForSeconds(0.5f);
			resultLabel.localScale = scale;
			resultLabel.ScaleTo(Vector3.one, 1f, ease, null);
			yield return new WaitForSeconds(1f);
			SoundUtils.PlaySE(SEFIleInfos.SE_044);
			yield return new WaitForSeconds(1.5f);
			TweenAlpha.Begin(base.gameObject, 0.5f, 0f);
			yield return new WaitForSeconds(0.5f);
			yield return true;
		}
	}
}
