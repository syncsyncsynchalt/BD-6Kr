using System;
using System.Collections.Generic;
using UniRx.InternalUtil;

namespace UniRx
{
	public sealed class ReplaySubject<T> : IObserver<T>, ISubject<T>, ISubject<T, T>, IObservable<T>
	{
		private class Subscription : IDisposable
		{
			private readonly object gate = new object();

			private ReplaySubject<T> parent;

			private IObserver<T> unsubscribeTarget;

			public Subscription(ReplaySubject<T> parent, IObserver<T> unsubscribeTarget)
			{
				this.parent = parent;
				this.unsubscribeTarget = unsubscribeTarget;
			}

			public void Dispose()
			{
				lock (gate)
				{
					if (parent != null)
					{
						lock (parent.observerLock)
						{
							ListObserver<T> listObserver = parent.outObserver as ListObserver<T>;
							if (listObserver != null)
							{
								parent.outObserver = listObserver.Remove(unsubscribeTarget);
							}
							else
							{
								parent.outObserver = new EmptyObserver<T>();
							}
							unsubscribeTarget = null;
							parent = null;
						}
					}
				}
			}
		}

		private object observerLock = new object();

		private bool isStopped;

		private bool isDisposed;

		private Exception lastError;

		private IObserver<T> outObserver = new EmptyObserver<T>();

		private readonly int bufferSize;

		private readonly TimeSpan window;

		private readonly DateTimeOffset startTime;

		private readonly IScheduler scheduler;

		private Queue<TimeInterval<T>> queue = new Queue<TimeInterval<T>>();

		public ReplaySubject()
			: this(int.MaxValue, TimeSpan.MaxValue, Scheduler.DefaultSchedulers.Iteration)
		{
		}

		public ReplaySubject(IScheduler scheduler)
			: this(int.MaxValue, TimeSpan.MaxValue, scheduler)
		{
		}

		public ReplaySubject(int bufferSize)
			: this(bufferSize, TimeSpan.MaxValue, Scheduler.DefaultSchedulers.Iteration)
		{
		}

		public ReplaySubject(int bufferSize, IScheduler scheduler)
			: this(bufferSize, TimeSpan.MaxValue, scheduler)
		{
		}

		public ReplaySubject(TimeSpan window)
			: this(int.MaxValue, window, Scheduler.DefaultSchedulers.Iteration)
		{
		}

		public ReplaySubject(TimeSpan window, IScheduler scheduler)
			: this(int.MaxValue, window, scheduler)
		{
		}

		public ReplaySubject(int bufferSize, TimeSpan window, IScheduler scheduler)
		{
			if (bufferSize < 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize");
			}
			if (window < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("window");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			this.bufferSize = bufferSize;
			this.window = window;
			this.scheduler = scheduler;
			startTime = scheduler.Now;
		}

		private void Trim()
		{
			TimeSpan timeSpan = Scheduler.Normalize(scheduler.Now - startTime);
			while (queue.Count > bufferSize)
			{
				queue.Dequeue();
			}
			while (queue.Count > 0 && timeSpan.Subtract(queue.Peek().Interval).CompareTo(window) > 0)
			{
				queue.Dequeue();
			}
		}

		public void OnCompleted()
		{
			IObserver<T> observer;
			lock (observerLock)
			{
				ThrowIfDisposed();
				if (isStopped)
				{
					return;
				}
				observer = outObserver;
				outObserver = new EmptyObserver<T>();
				isStopped = true;
				Trim();
			}
			observer.OnCompleted();
		}

		public void OnError(Exception error)
		{
			if (error == null)
			{
				throw new ArgumentNullException("error");
			}
			IObserver<T> observer;
			lock (observerLock)
			{
				ThrowIfDisposed();
				if (isStopped)
				{
					return;
				}
				observer = outObserver;
				outObserver = new EmptyObserver<T>();
				isStopped = true;
				lastError = error;
				Trim();
			}
			observer.OnError(error);
		}

		public void OnNext(T value)
		{
			IObserver<T> observer;
			lock (observerLock)
			{
				ThrowIfDisposed();
				if (isStopped)
				{
					return;
				}
				queue.Enqueue(new TimeInterval<T>(value, Scheduler.Now - startTime));
				Trim();
				observer = outObserver;
			}
			observer.OnNext(value);
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			if (observer == null)
			{
				throw new ArgumentNullException("observer");
			}
			Exception ex = null;
			Subscription subscription = null;
			lock (observerLock)
			{
				ThrowIfDisposed();
				if (!isStopped)
				{
					ListObserver<T> listObserver = outObserver as ListObserver<T>;
					if (listObserver != null)
					{
						outObserver = listObserver.Add(observer);
					}
					else
					{
						IObserver<T> observer2 = outObserver;
						if (observer2 is EmptyObserver<T>)
						{
							outObserver = observer;
						}
						else
						{
							outObserver = new ListObserver<T>(new ImmutableList<IObserver<T>>(new IObserver<T>[2]
							{
								observer2,
								observer
							}));
						}
					}
					subscription = new Subscription(this, observer);
				}
				ex = lastError;
				Trim();
				foreach (TimeInterval<T> item in queue)
				{
					observer.OnNext(item.Value);
				}
			}
			if (subscription != null)
			{
				return subscription;
			}
			if (ex != null)
			{
				observer.OnError(ex);
			}
			else
			{
				observer.OnCompleted();
			}
			return Disposable.Empty;
		}

		public void Dispose()
		{
			lock (observerLock)
			{
				isDisposed = true;
				outObserver = new DisposedObserver<T>();
				lastError = null;
				queue = null;
			}
		}

		private void ThrowIfDisposed()
		{
			if (isDisposed)
			{
				throw new ObjectDisposedException(string.Empty);
			}
		}
	}
}
