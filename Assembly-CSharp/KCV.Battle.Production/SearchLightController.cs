using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class SearchLightController : MonoBehaviour
	{
		private bool _isAnimating;

		private Animation _animation;

		private void OnDestroy()
		{
			Mem.Del(ref _isAnimating);
			Mem.Del(ref _animation);
		}

		private void Start()
		{
			_animation = GetComponent<Animation>();
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).gameObject.SetActive(false);
			}
		}

		private IEnumerator AnimationCoroutine(IObserver<int> observer)
		{
			if (_isAnimating)
			{
				observer.OnNext(0);
				observer.OnCompleted();
				yield break;
			}
			_isAnimating = true;
			for (int j = 0; j < base.transform.childCount; j++)
			{
				base.transform.GetChild(j).gameObject.SetActive(true);
			}
			string anim = "SearchLight";
			_animation.Play(anim);
			yield return new WaitForSeconds(_animation[anim].length);
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).gameObject.SetActive(false);
			}
			_isAnimating = false;
			observer.OnNext(0);
			observer.OnCompleted();
		}

		public IObservable<int> PlayAnimation()
		{
			return Observable.FromCoroutine((IObserver<int> observer) => AnimationCoroutine(observer));
		}
	}
}
