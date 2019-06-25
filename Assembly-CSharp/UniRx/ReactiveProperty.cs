using System;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class ReactiveProperty<T> : IDisposable, IReactiveProperty<T>, IReadOnlyReactiveProperty<T>, IObservable<T>
	{
		[NonSerialized]
		private bool canPublishValueOnSubscribe;

		[NonSerialized]
		private bool isDisposed;

		[SerializeField]
		private T value = default(T);

		[NonSerialized]
		private Subject<T> publisher;

		[NonSerialized]
		private IDisposable sourceConnection;

		public T Value
		{
			get
			{
				return value;
			}
			set
			{
				if (value == null)
				{
					if (this.value != null)
					{
						SetValue(value);
						canPublishValueOnSubscribe = true;
						if (!isDisposed && publisher != null)
						{
							publisher.OnNext(this.value);
						}
					}
				}
				else if (this.value == null || !this.value.Equals(value))
				{
					SetValue(value);
					canPublishValueOnSubscribe = true;
					if (!isDisposed && publisher != null)
					{
						publisher.OnNext(this.value);
					}
				}
			}
		}

		public ReactiveProperty()
			: this(default(T))
		{
		}

		public ReactiveProperty(T initialValue)
		{
			value = initialValue;
			canPublishValueOnSubscribe = true;
		}

		public ReactiveProperty(IObservable<T> source)
		{
			canPublishValueOnSubscribe = false;
			publisher = new Subject<T>();
			sourceConnection = source.Subscribe(delegate(T x)
			{
				Value = x;
			}, publisher.OnError, publisher.OnCompleted);
		}

		public ReactiveProperty(IObservable<T> source, T initialValue)
		{
			canPublishValueOnSubscribe = false;
			Value = initialValue;
			publisher = new Subject<T>();
			sourceConnection = source.Subscribe(delegate(T x)
			{
				Value = x;
			}, publisher.OnError, publisher.OnCompleted);
		}

		protected virtual void SetValue(T value)
		{
			this.value = value;
		}

		public void SetValueAndForceNotify(T value)
		{
			SetValue(value);
			if (!isDisposed && publisher != null)
			{
				publisher.OnNext(this.value);
			}
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
