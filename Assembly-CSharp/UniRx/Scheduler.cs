using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using UniRx.InternalUtil;
using UnityEngine;

namespace UniRx
{
	public static class Scheduler
	{
		private class CurrentThreadScheduler : IScheduler
		{
			private static class Trampoline
			{
				public static void Run(SchedulerQueue queue)
				{
					while (queue.Count > 0)
					{
						ScheduledItem scheduledItem = queue.Dequeue();
						if (!scheduledItem.IsCanceled)
						{
							TimeSpan timeout = scheduledItem.DueTime - Time;
							if (timeout.Ticks > 0)
							{
								Thread.Sleep(timeout);
							}
							if (!scheduledItem.IsCanceled)
							{
								scheduledItem.Invoke();
							}
						}
					}
				}
			}

			[ThreadStatic]
			private static SchedulerQueue s_threadLocalQueue;

			[ThreadStatic]
			private static Stopwatch s_clock;

			private static TimeSpan Time
			{
				get
				{
					if (s_clock == null)
					{
						s_clock = Stopwatch.StartNew();
					}
					return s_clock.Elapsed;
				}
			}

			[EditorBrowsable(EditorBrowsableState.Advanced)]
			public static bool IsScheduleRequired => GetQueue() == null;

			public DateTimeOffset Now => Scheduler.Now;

			private static SchedulerQueue GetQueue()
			{
				return s_threadLocalQueue;
			}

			private static void SetQueue(SchedulerQueue newQueue)
			{
				s_threadLocalQueue = newQueue;
			}

			public IDisposable Schedule(Action action)
			{
				return Schedule(TimeSpan.Zero, action);
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				if (action == null)
				{
					throw new ArgumentNullException("action");
				}
				TimeSpan dueTime2 = Time + Normalize(dueTime);
				ScheduledItem scheduledItem = new ScheduledItem(action, dueTime2);
				SchedulerQueue queue = GetQueue();
				if (queue == null)
				{
					queue = new SchedulerQueue(4);
					queue.Enqueue(scheduledItem);
					SetQueue(queue);
					try
					{
						Trampoline.Run(queue);
					}
					finally
					{
						SetQueue(null);
					}
				}
				else
				{
					queue.Enqueue(scheduledItem);
				}
				return scheduledItem.Cancellation;
			}
		}

		private class ImmediateScheduler : IScheduler
		{
			public DateTimeOffset Now => Scheduler.Now;

			public IDisposable Schedule(Action action)
			{
				action();
				return Disposable.Empty;
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				TimeSpan timeout = Normalize(dueTime);
				if (timeout.Ticks > 0)
				{
					Thread.Sleep(timeout);
				}
				action();
				return Disposable.Empty;
			}
		}

		public static class DefaultSchedulers
		{
			private static IScheduler constantTime;

			private static IScheduler tailRecursion;

			private static IScheduler iteration;

			private static IScheduler timeBasedOperations;

			private static IScheduler asyncConversions;

			public static IScheduler ConstantTimeOperations
			{
				get
				{
					return constantTime ?? (constantTime = Immediate);
				}
				set
				{
					constantTime = value;
				}
			}

			public static IScheduler TailRecursion
			{
				get
				{
					return tailRecursion ?? (tailRecursion = Immediate);
				}
				set
				{
					tailRecursion = value;
				}
			}

			public static IScheduler Iteration
			{
				get
				{
					return iteration ?? (iteration = CurrentThread);
				}
				set
				{
					iteration = value;
				}
			}

			public static IScheduler TimeBasedOperations
			{
				get
				{
					return timeBasedOperations ?? (timeBasedOperations = MainThread);
				}
				set
				{
					timeBasedOperations = value;
				}
			}

			public static IScheduler AsyncConversions
			{
				get
				{
					return asyncConversions ?? (asyncConversions = ThreadPool);
				}
				set
				{
					asyncConversions = value;
				}
			}

			public static void SetDefaultForUnity()
			{
				ConstantTimeOperations = Immediate;
				TailRecursion = Immediate;
				Iteration = CurrentThread;
				TimeBasedOperations = MainThread;
				AsyncConversions = ThreadPool;
			}

			public static void SetDotNetCompatible()
			{
				ConstantTimeOperations = Immediate;
				TailRecursion = Immediate;
				Iteration = CurrentThread;
				TimeBasedOperations = ThreadPool;
				AsyncConversions = ThreadPool;
			}
		}

		private class ThreadPoolScheduler : IScheduler
		{
			private sealed class Timer : IDisposable
			{
				private static readonly HashSet<System.Threading.Timer> s_timers = new HashSet<System.Threading.Timer>();

				private readonly SingleAssignmentDisposable _disposable;

				private Action _action;

				private System.Threading.Timer _timer;

				private bool _hasAdded;

				private bool _hasRemoved;

				public Timer(TimeSpan dueTime, Action action)
				{
					_disposable = new SingleAssignmentDisposable();
					_disposable.Disposable = Disposable.Create(Unroot);
					_action = action;
					_timer = new System.Threading.Timer(Tick, null, dueTime, TimeSpan.FromMilliseconds(-1.0));
					lock (s_timers)
					{
						if (!_hasRemoved)
						{
							s_timers.Add(_timer);
							_hasAdded = true;
						}
					}
				}

				private void Tick(object state)
				{
					try
					{
						if (!_disposable.IsDisposed)
						{
							_action();
						}
					}
					finally
					{
						Unroot();
					}
				}

				private void Unroot()
				{
					_action = delegate
					{
					};
					System.Threading.Timer timer = null;
					lock (s_timers)
					{
						if (!_hasRemoved)
						{
							timer = _timer;
							_timer = null;
							if (_hasAdded && timer != null)
							{
								s_timers.Remove(timer);
							}
							_hasRemoved = true;
						}
					}
					timer?.Dispose();
				}

				public void Dispose()
				{
					_disposable.Dispose();
				}
			}

			public DateTimeOffset Now => Scheduler.Now;

			public IDisposable Schedule(Action action)
			{
				BooleanDisposable d = new BooleanDisposable();
				System.Threading.ThreadPool.QueueUserWorkItem(delegate
				{
					if (!d.IsDisposed)
					{
						action();
					}
				});
				return d;
			}

			public IDisposable Schedule(DateTimeOffset dueTime, Action action)
			{
				return Schedule(dueTime - Now, action);
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				return new Timer(dueTime, action);
			}
		}

		private class MainThreadScheduler : IScheduler
		{
			public DateTimeOffset Now => Scheduler.Now;

			public MainThreadScheduler()
			{
				MainThreadDispatcher.Initialize();
			}

			private IEnumerator DelayAction(TimeSpan dueTime, Action action, ICancelable cancellation)
			{
				if (dueTime == TimeSpan.Zero)
				{
					yield return null;
					if (!cancellation.IsDisposed)
					{
						MainThreadDispatcher.UnsafeSend(action);
					}
					yield break;
				}
				if (dueTime.TotalMilliseconds % 1000.0 == 0.0)
				{
					yield return new WaitForSeconds((float)dueTime.TotalSeconds);
					if (!cancellation.IsDisposed)
					{
						MainThreadDispatcher.UnsafeSend(action);
					}
					yield break;
				}
				float startTime = Time.time;
				float dt = (float)dueTime.TotalSeconds;
				float elapsed;
				do
				{
					yield return null;
					if (cancellation.IsDisposed)
					{
						yield break;
					}
					elapsed = Time.time - startTime;
				}
				while (!(elapsed >= dt));
				MainThreadDispatcher.UnsafeSend(action);
			}

			public IDisposable Schedule(Action action)
			{
				BooleanDisposable d = new BooleanDisposable();
				MainThreadDispatcher.Post(delegate
				{
					if (!d.IsDisposed)
					{
						action();
					}
				});
				return d;
			}

			public IDisposable Schedule(DateTimeOffset dueTime, Action action)
			{
				return Schedule(dueTime - Now, action);
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				BooleanDisposable d = new BooleanDisposable();
				TimeSpan dueTime2 = Normalize(dueTime);
				MainThreadDispatcher.SendStartCoroutine(DelayAction(dueTime2, delegate
				{
					if (!d.IsDisposed)
					{
						action();
					}
				}, d));
				return d;
			}
		}

		private class IgnoreTimeScaleMainThreadScheduler : IScheduler
		{
			public DateTimeOffset Now => Scheduler.Now;

			public IgnoreTimeScaleMainThreadScheduler()
			{
				MainThreadDispatcher.Initialize();
			}

			private IEnumerator DelayAction(TimeSpan dueTime, Action action, ICancelable cancellation)
			{
				if (dueTime == TimeSpan.Zero)
				{
					yield return null;
					if (!cancellation.IsDisposed)
					{
						MainThreadDispatcher.UnsafeSend(action);
					}
					yield break;
				}
				float startTime = Time.realtimeSinceStartup;
				float dt = (float)dueTime.TotalSeconds;
				float elapsed;
				do
				{
					yield return null;
					if (cancellation.IsDisposed)
					{
						yield break;
					}
					elapsed = Time.realtimeSinceStartup - startTime;
				}
				while (!(elapsed >= dt));
				MainThreadDispatcher.UnsafeSend(action);
			}

			public IDisposable Schedule(Action action)
			{
				BooleanDisposable d = new BooleanDisposable();
				MainThreadDispatcher.Post(delegate
				{
					if (!d.IsDisposed)
					{
						action();
					}
				});
				return d;
			}

			public IDisposable Schedule(DateTimeOffset dueTime, Action action)
			{
				return Schedule(dueTime - Now, action);
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				BooleanDisposable d = new BooleanDisposable();
				TimeSpan dueTime2 = Normalize(dueTime);
				MainThreadDispatcher.SendStartCoroutine(DelayAction(dueTime2, delegate
				{
					if (!d.IsDisposed)
					{
						action();
					}
				}, d));
				return d;
			}
		}

		public static readonly IScheduler CurrentThread = new CurrentThreadScheduler();

		public static readonly IScheduler Immediate = new ImmediateScheduler();

		public static readonly IScheduler ThreadPool = new ThreadPoolScheduler();

		private static IScheduler mainThread;

		private static IScheduler mainThreadIgnoreTimeScale;

		public static bool IsCurrentThreadSchedulerScheduleRequired => CurrentThreadScheduler.IsScheduleRequired;

		public static DateTimeOffset Now => DateTimeOffset.UtcNow;

		public static IScheduler MainThread => mainThread ?? (mainThread = new MainThreadScheduler());

		public static IScheduler MainThreadIgnoreTimeScale => mainThreadIgnoreTimeScale ?? (mainThreadIgnoreTimeScale = new IgnoreTimeScaleMainThreadScheduler());

		public static TimeSpan Normalize(TimeSpan timeSpan)
		{
			return (!(timeSpan >= TimeSpan.Zero)) ? TimeSpan.Zero : timeSpan;
		}

		public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action action)
		{
			return scheduler.Schedule(dueTime - scheduler.Now, action);
		}

		public static IDisposable Schedule(this IScheduler scheduler, Action<Action> action)
		{
			CompositeDisposable group = new CompositeDisposable(1);
			object gate = new object();
			Action recursiveAction = null;
			recursiveAction = delegate
			{
				action(delegate
				{
					bool isAdded = false;
					bool isDone = false;
					IDisposable d = null;
					d = scheduler.Schedule(delegate
					{
						lock (gate)
						{
							if (isAdded)
							{
								group.Remove(d);
							}
							else
							{
								isDone = true;
							}
						}
						recursiveAction();
					});
					lock (gate)
					{
						if (!isDone)
						{
							group.Add(d);
							isAdded = true;
						}
					}
				});
			};
			group.Add(scheduler.Schedule(recursiveAction));
			return group;
		}

		public static IDisposable Schedule(this IScheduler scheduler, TimeSpan dueTime, Action<Action<TimeSpan>> action)
		{
			CompositeDisposable group = new CompositeDisposable(1);
			object gate = new object();
			Action recursiveAction = null;
			recursiveAction = delegate
			{
				action(delegate(TimeSpan dt)
				{
					bool isAdded = false;
					bool isDone = false;
					IDisposable d = null;
					d = scheduler.Schedule(dt, delegate
					{
						lock (gate)
						{
							if (isAdded)
							{
								group.Remove(d);
							}
							else
							{
								isDone = true;
							}
						}
						recursiveAction();
					});
					lock (gate)
					{
						if (!isDone)
						{
							group.Add(d);
							isAdded = true;
						}
					}
				});
			};
			group.Add(scheduler.Schedule(dueTime, recursiveAction));
			return group;
		}

		public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action<Action<DateTimeOffset>> action)
		{
			CompositeDisposable group = new CompositeDisposable(1);
			object gate = new object();
			Action recursiveAction = null;
			recursiveAction = delegate
			{
				action(delegate(DateTimeOffset dt)
				{
					bool isAdded = false;
					bool isDone = false;
					IDisposable d = null;
					d = scheduler.Schedule(dt, (Action)delegate
					{
						lock (gate)
						{
							if (isAdded)
							{
								group.Remove(d);
							}
							else
							{
								isDone = true;
							}
						}
						recursiveAction();
					});
					lock (gate)
					{
						if (!isDone)
						{
							group.Add(d);
							isAdded = true;
						}
					}
				});
			};
			group.Add(scheduler.Schedule(dueTime, recursiveAction));
			return group;
		}
	}
}
