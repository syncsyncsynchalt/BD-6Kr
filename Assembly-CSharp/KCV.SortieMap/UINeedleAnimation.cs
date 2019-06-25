using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UISprite))]
	public class UINeedleAnimation : MonoBehaviour
	{
		private UISprite _uiSprite;

		private IDisposable _disAnimation;

		private UISprite sprite => this.GetComponentThis(ref _uiSprite);

		private void Start()
		{
			sprite.alpha = 1f;
			Play();
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiSprite);
			if (_disAnimation != null)
			{
				_disAnimation.Dispose();
			}
			Mem.Del(ref _disAnimation);
		}

		public void Play()
		{
			_disAnimation = Observable.Interval(TimeSpan.FromSeconds(0.10000000149011612)).Subscribe(delegate
			{
				base.transform.LTRotateLocal(Vector3.forward * XorRandom.GetFLim(0.45f, 3.3f), 0.1f);
			});
		}

		public void Stop()
		{
			if (_disAnimation != null)
			{
				_disAnimation.Dispose();
			}
		}
	}
}
