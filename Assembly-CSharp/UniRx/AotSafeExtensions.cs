using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
	public static class AotSafeExtensions
	{
		public static IEnumerable<T> AsSafeEnumerable<T>(this IEnumerable<T> source)
		{
			IEnumerator e = ((IEnumerable)source).GetEnumerator();
			using (e as IDisposable)
			{
				while (e.MoveNext())
				{
					yield return (T)e.Current;
				}
			}
		}

		public static IObservable<Tuple<T>> WrapValueToClass<T>(this IObservable<T> source) where T : struct
		{
			int dummy = 0;
			return Observable.Create(delegate(IObserver<Tuple<T>> observer)
			{
				IObservable<T> observable = source;
				Action<T> onNext = delegate(T x)
				{
					dummy.GetHashCode();
					Tuple<T> value = new Tuple<T>(x);
					observer.OnNext(value);
				};
				IObserver<Tuple<T>> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<Tuple<T>> observer3 = observer;
				return observable.Subscribe(Observer.Create(onNext, onError, observer3.OnCompleted));
			});
		}

		public static IEnumerable<Tuple<T>> WrapValueToClass<T>(this IEnumerable<T> source) where T : struct
		{
			IEnumerator e = ((IEnumerable)source).GetEnumerator();
			using (e as IDisposable)
			{
				while (e.MoveNext())
				{
					yield return new Tuple<T>((T)e.Current);
				}
			}
		}
	}
}
