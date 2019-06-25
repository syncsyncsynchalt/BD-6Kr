using LT.Tweening;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UISprite))]
	public class ProdExclamationPoint : MonoBehaviour
	{
		private UISprite _uiSprite;

		private UISprite sprite => this.GetComponentThis(ref _uiSprite);

		public static ProdExclamationPoint Instantiate(ProdExclamationPoint prefab, Transform parent)
		{
			ProdExclamationPoint prodExclamationPoint = UnityEngine.Object.Instantiate(prefab);
			prodExclamationPoint.transform.parent = parent;
			prodExclamationPoint.transform.localScaleZero();
			prodExclamationPoint.transform.localPositionZero();
			prodExclamationPoint.sprite.alpha = 0f;
			return prodExclamationPoint;
		}

		private void OnDestroy()
		{
			base.transform.LTCancel();
			Mem.Del(ref _uiSprite);
		}

		public UniRx.IObservable<bool> Play()
		{
			return Observable.FromCoroutine((UniRx.IObserver<bool> observer) => AnimationObserver(observer));
		}

		private IEnumerator AnimationObserver(UniRx.IObserver<bool> observer)
		{
			LeanTweenType iType = LeanTweenType.easeOutQuad;
			sprite.alpha = 0f;
			sprite.transform.localScaleZero();
			yield return new WaitForEndOfFrame();
			float fAnimTime6 = 0.2f;
			base.transform.LTValue(sprite.alpha, 1f, fAnimTime6).setEase(iType).setOnUpdate(delegate(float x)
			{
                throw new NotImplementedException("‚È‚É‚±‚ê");
                // this.sprite.alpha = x;
			});
			base.transform.LTMoveLocalY(16f, fAnimTime6).setEase(iType);
			base.transform.LTScale(Vector3.one, fAnimTime6);
			yield return Observable.Timer(TimeSpan.FromSeconds(fAnimTime6)).StartAsCoroutine();
			fAnimTime6 = 0.2f;
			base.transform.LTMoveLocalY(0f, fAnimTime6);
			yield return Observable.Timer(TimeSpan.FromSeconds(fAnimTime6)).StartAsCoroutine();
			fAnimTime6 = 0.2f;
			base.transform.LTMoveLocalY(-7f, fAnimTime6);
			base.transform.LTScale(new Vector3(1.3f, 0.3f, 0f), fAnimTime6);
			yield return Observable.Timer(TimeSpan.FromSeconds(fAnimTime6)).StartAsCoroutine();
			fAnimTime6 = 0.2f;
			base.transform.LTMoveLocalY(6f, fAnimTime6);
			base.transform.LTScale(Vector3.one, fAnimTime6);
			yield return Observable.Timer(TimeSpan.FromSeconds(fAnimTime6)).StartAsCoroutine();
			fAnimTime6 = 0.2f;
			base.transform.LTMoveLocalY(0f, fAnimTime6);
			base.transform.LTScale(Vector3.one, fAnimTime6);
			yield return Observable.Timer(TimeSpan.FromSeconds(fAnimTime6)).StartAsCoroutine();
			fAnimTime6 = 0.1f;
			base.transform.LTValue(sprite.alpha, 0f, fAnimTime6).setEase(iType).setOnUpdate(delegate(float x)
			{
                throw new NotImplementedException("‚È‚É‚±‚ê");
                // this.sprite.alpha = x;
			});
			base.transform.LTMoveLocalY(0f, fAnimTime6);
			base.transform.LTScale(Vector3.one, fAnimTime6);
			yield return Observable.Timer(TimeSpan.FromSeconds(fAnimTime6)).StartAsCoroutine();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}
	}
}
