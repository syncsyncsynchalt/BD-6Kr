using System;
using System.Collections.Generic;

namespace UniRx
{
	public class MessageBroker : IDisposable, IMessageBroker
	{
		public static readonly IMessageBroker Default = new MessageBroker();

		private bool isDisposed;

		private readonly Dictionary<Type, object> notifiers = new Dictionary<Type, object>();

		public void Publish<T>(T message)
		{
			object value;
			lock (notifiers)
			{
				if (isDisposed || !notifiers.TryGetValue(typeof(T), out value))
				{
					return;
				}
			}
			((Subject<T>)value).OnNext(message);
		}

		public IObservable<T> Receive<T>()
		{
			object value;
			lock (notifiers)
			{
				if (isDisposed)
				{
					throw new ObjectDisposedException("MessageBroker");
				}
				if (!notifiers.TryGetValue(typeof(T), out value))
				{
					value = new Subject<T>();
					notifiers.Add(typeof(T), value);
				}
			}
			return ((IObservable<T>)value).AsObservable();
		}

		public void Dispose()
		{
			lock (notifiers)
			{
				if (!isDisposed)
				{
					isDisposed = true;
					notifiers.Clear();
				}
			}
		}
	}
}
