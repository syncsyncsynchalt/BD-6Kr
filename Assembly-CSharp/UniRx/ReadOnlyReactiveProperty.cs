using System;

namespace UniRx
{
	public class ReadOnlyReactiveProperty<T> : IDisposable, IReadOnlyReactiveProperty<T>, IObservable<T>
	{
		private bool canPublishValueOnSubscribe;

		private bool isDisposed;

		private T value = default(T);

		private Subject<T> publisher;

		private IDisposable sourceConnection;

		public T Value => value;

		public ReadOnlyReactiveProperty(IObservable<T> source)
		{
			publisher = new Subject<T>();
			sourceConnection = source.Subscribe(delegate(T x)
			{
				value = x;
				canPublishValueOnSubscribe = true;
				publisher.OnNext(x);
			}, publisher.OnError, publisher.OnCompleted);
		}

		public ReadOnlyReactiveProperty(IObservable<T> source, T initialValue)
		{
			value = initialValue;
			publisher = new Subject<T>();
			sourceConnection = source.Subscribe(delegate(T x)
			{
				value = x;
				canPublishValueOnSubscribe = true;
				publisher.OnNext(x);
			}, publisher.OnError, publisher.OnCompleted);
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			if (isDisposed)
			{
				observer.OnCompleted();
				return Disposable.Empty;
			}
			if (publisher == null)
			{
				publisher = new Subject<T>();
			}
			IDisposable result = publisher.Subscribe(observer);
			if (canPublishValueOnSubscribe)
			{
				observer.OnNext(value);
			}
			return result;
		}

		public void Dispose()
		{
			if (!isDisposed)
			{
				isDisposed = true;
				if (sourceConnection != null)
				{
					sourceConnection.Dispose();
					sourceConnection = null;
				}
				if (publisher != null)
				{
					try
					{
						publisher.OnCompleted();
					}
					finally
					{
						publisher.Dispose();
						publisher = null;
					}
				}
			}
		}

		public override string ToString()
		{
			return (value != null) ? value.ToString() : "null";
		}
	}
}
