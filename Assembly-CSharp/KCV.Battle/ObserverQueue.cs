using System;
using System.Collections.Generic;

namespace KCV.Battle
{
	public class ObserverQueue<T> : IDisposable
	{
		public const int OBSERVER_DEFAULT_NUM = 32;

		private Queue<T> _queObserver;

		protected Queue<T> observerQueue
		{
			get
			{
				return _queObserver;
			}
			private set
			{
				_queObserver = value;
			}
		}

		public int Count => _queObserver.Count;

		public ObserverQueue()
		{
			Init(32);
		}

		public ObserverQueue(int nObserverDefaultNum)
		{
			Init(nObserverDefaultNum);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
			Mem.DelQueueSafe(ref _queObserver);
		}

		private bool Init(int nObserverDefaultNum)
		{
			observerQueue = new Queue<T>();
			return true;
		}

		public virtual ObserverQueue<T> Register(T item)
		{
			observerQueue.Enqueue(item);
			return this;
		}

		public virtual ObserverQueue<T> Register(params T[] items)
		{
			for (int i = 0; i < items.Length; i++)
			{
				observerQueue.Enqueue(items[i]);
			}
			return this;
		}

		public virtual ObserverQueue<T> Register(List<T> items)
		{
			for (int i = 0; i < items.Count; i++)
			{
				observerQueue.Enqueue(items[i]);
			}
			return this;
		}

		public virtual T DeRegister()
		{
			return observerQueue.Dequeue();
		}

		public T Peek()
		{
			return observerQueue.Peek();
		}

		public void Clear()
		{
			observerQueue.Clear();
		}

		public bool Contains(T item)
		{
			return observerQueue.Contains(item);
		}
	}
}
