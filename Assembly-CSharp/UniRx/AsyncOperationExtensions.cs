using System;
using System.Collections;
using UnityEngine;

namespace UniRx
{
	public static class AsyncOperationExtensions
	{
		public static IObservable<AsyncOperation> AsObservable(this AsyncOperation asyncOperation, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<AsyncOperation> observer, CancellationToken cancellation) => AsObservableCore(asyncOperation, observer, progress, cancellation));
		}

		public static IObservable<T> AsAsyncOperationObservable<T>(this T asyncOperation, IProgress<float> progress = null) where T : AsyncOperation
		{
			return Observable.FromCoroutine((IObserver<T> observer, CancellationToken cancellation) => AsObservableCore(asyncOperation, observer, progress, cancellation));
		}

		private static IEnumerator AsObservableCore<T>(T asyncOperation, IObserver<T> observer, IProgress<float> reportProgress, CancellationToken cancel) where T : AsyncOperation
		{
			while (!asyncOperation.isDone && !cancel.IsCancellationRequested)
			{
				if (reportProgress != null)
				{
					try
					{
						reportProgress.Report(asyncOperation.progress);
					}
					catch (Exception ex3)
					{
						Exception ex2 = ex3;
						observer.OnError(ex2);
						yield break;
					}
				}
				yield return null;
			}
			if (!cancel.IsCancellationRequested)
			{
				if (reportProgress != null)
				{
					try
					{
						reportProgress.Report(asyncOperation.progress);
					}
					catch (Exception ex4)
					{
						Exception ex = ex4;
						observer.OnError(ex);
						yield break;
					}
				}
				observer.OnNext(asyncOperation);
				observer.OnCompleted();
			}
		}
	}
}
