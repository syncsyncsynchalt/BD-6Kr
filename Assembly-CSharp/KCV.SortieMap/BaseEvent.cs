using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	public class BaseEvent : IDisposable
	{
		private bool _isDisposed;

		public BaseEvent()
		{
			_isDisposed = false;
		}

        public bool _003CisWait_003E__3 { get; internal set; }

        public void Dispose()
		{
			Dispose(disposing: true);
			Mem.Del(ref _isDisposed);
		}

		public UniRx.IObservable<bool> PlayAnimation()
		{
			return Observable.FromCoroutine((UniRx.IObserver<bool> observer) => AnimationObserver(observer));
		}

		protected virtual IEnumerator AnimationObserver(UniRx.IObserver<bool> observer)
		{
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing)
				{
				}
				_isDisposed = true;
			}
		}
	}
}
