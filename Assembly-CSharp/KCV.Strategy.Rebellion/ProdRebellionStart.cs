using KCV.Utils;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class ProdRebellionStart : MonoBehaviour
	{
		[Button("StartAnimation", "StartAnimation", new object[]
		{

		})]
		public int button;

		[SerializeField]
		private UIPanel RedMask;

		[SerializeField]
		private UITexture Obi;

		[SerializeField]
		private Transform RaisyuText;

		[SerializeField]
		private TextureFlash texFlash;

		[SerializeField]
		private UILabel AreaName;

		private Action Onfinished;

		public static ProdRebellionStart Instantiate(ProdRebellionStart prefab, Transform parent)
		{
			ProdRebellionStart prodRebellionStart = UnityEngine.Object.Instantiate(prefab);
			prodRebellionStart.transform.parent = parent;
			prodRebellionStart.transform.localPositionZero();
			prodRebellionStart.transform.localScaleOne();
			return prodRebellionStart;
		}

		public void StartAnimation()
		{
			((Component)base.transform).GetComponent<Animation>().Play();
		}

		public void maskEffect()
		{
			texFlash.MaskFadeExpanding(2f, 0.5f, isWhite: false);
		}

		public void AreaNameFadeIn()
		{
			TweenAlpha.Begin(AreaName.transform.parent.gameObject, 0.2f, 1f);
		}

		public void AreaNameFadeOut()
		{
			TweenAlpha.Begin(AreaName.transform.parent.gameObject, 0.2f, 0f);
		}

		public UniRx.IObservable<bool> Play(Action Onfinished)
		{
			this.Onfinished = Onfinished;
			return Observable.FromCoroutine((UniRx.IObserver<bool> observer) => PlayAnimation(observer));
		}

		private IEnumerator PlayAnimation(UniRx.IObserver<bool> observer)
		{
			InitAreaName();
			yield return new WaitForEndOfFrame();
			StartAnimation();
			SingletonMonoBehaviour<SoundManager>.Instance.StopBGM();
			SoundUtils.PlaySE(SEFIleInfos.Keihou);

            throw new NotImplementedException("‚È‚É‚±‚ê");
            // yield return new WaitForSeconds(GetComponent<Animation>().get_Item("ProdRebellionStart").length);
            yield return new WaitForSeconds(0);


            if (Onfinished != null)
			{
				Onfinished();
			}
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		public void PlaySE()
		{
			SoundUtils.PlaySE(SEFIleInfos.EnemyComming);
		}

		public void PlayBGM()
		{
			SoundUtils.PlayBGM((BGMFileInfos)4, isLoop: true);
		}

		private void InitAreaName()
		{
			AreaName.text = StrategyTopTaskManager.GetLogicManager().Area[StrategyRebellionTaskManager.RebellionArea].Name;
		}
	}
}
