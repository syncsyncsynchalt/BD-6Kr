using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UniRx.Triggers;
using UnityEngine;

namespace UniRx
{
	public static class Observable
	{
		private enum AmbState
		{
			Left,
			Right,
			Neither
		}

		private class AnonymousObservable<T> : IObservable<T>
		{
			private readonly Func<IObserver<T>, IDisposable> subscribe;

			public AnonymousObservable(Func<IObserver<T>, IDisposable> subscribe)
			{
				this.subscribe = subscribe;
			}

			public IDisposable Subscribe(IObserver<T> observer)
			{
				SingleAssignmentDisposable subscription = new SingleAssignmentDisposable();
				IObserver<T> safeObserver = Observer.Create<T>(observer.OnNext, observer.OnError, observer.OnCompleted, subscription);
				if (Scheduler.IsCurrentThreadSchedulerScheduleRequired)
				{
					Scheduler.CurrentThread.Schedule(delegate
					{
						subscription.Disposable = subscribe(safeObserver);
					});
				}
				else
				{
					subscription.Disposable = subscribe(safeObserver);
				}
				return subscription;
			}
		}

		private class ConnectableObservable<T> : IObservable<T>, IConnectableObservable<T>
		{
			private readonly IObservable<T> source;

			private readonly ISubject<T> subject;

			public ConnectableObservable(IObservable<T> source, ISubject<T> subject)
			{
				this.source = source;
				this.subject = subject;
			}

			public IDisposable Connect()
			{
				return source.Subscribe(subject);
			}

			public IDisposable Subscribe(IObserver<T> observer)
			{
				return subject.Subscribe(observer);
			}
		}

		private static readonly TimeSpan InfiniteTimeSpan = new TimeSpan(0, 0, 0, 0, -1);

		private static readonly HashSet<Type> YieldInstructionTypes = new HashSet<Type>
		{
			typeof(WWW),
			typeof(WaitForEndOfFrame),
			typeof(WaitForFixedUpdate),
			typeof(WaitForSeconds),
			typeof(Coroutine)
		};

		private static IObservable<T> AddRef<T>(IObservable<T> xs, RefCountDisposable r)
		{
			return Create((IObserver<T> observer) => new CompositeDisposable(r.GetDisposable(), xs.Subscribe(observer)));
		}

		public static IObservable<TSource> Scan<TSource>(this IObservable<TSource> source, Func<TSource, TSource, TSource> func)
		{
			return Create(delegate(IObserver<TSource> observer)
			{
				bool isFirst = true;
				TSource prev = (TSource)default(TSource);
				IObservable<TSource> source2 = source;
				Action<TSource> onNext = delegate(TSource x)
				{
					if (isFirst)
					{
						isFirst = false;
						prev = (TSource)x;
						observer.OnNext(x);
					}
					else
					{
						try
						{
							prev = (TSource)func((TSource)prev, x);
						}
						catch (Exception error)
						{
							observer.OnError(error);
							return;
						}
						observer.OnNext((TSource)prev);
					}
				};
				IObserver<TSource> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<TSource> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IObservable<TAccumulate> Scan<TSource, TAccumulate>(this IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		{
			return Create(delegate(IObserver<TAccumulate> observer)
			{
				TAccumulate prev = (TAccumulate)seed;
				observer.OnNext(seed);
				IObservable<TSource> source2 = source;
				Action<TSource> onNext = delegate(TSource x)
				{
					try
					{
						prev = (TAccumulate)func((TAccumulate)prev, x);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					observer.OnNext((TAccumulate)prev);
				};
				IObserver<TAccumulate> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<TAccumulate> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IConnectableObservable<T> Multicast<T>(this IObservable<T> source, ISubject<T> subject)
		{
			return new ConnectableObservable<T>(source, subject);
		}

		public static IConnectableObservable<T> Publish<T>(this IObservable<T> source)
		{
			return source.Multicast(new Subject<T>());
		}

		public static IConnectableObservable<T> Publish<T>(this IObservable<T> source, T initialValue)
		{
			return source.Multicast(new BehaviorSubject<T>(initialValue));
		}

		public static IConnectableObservable<T> PublishLast<T>(this IObservable<T> source)
		{
			return source.Multicast(new AsyncSubject<T>());
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source)
		{
			return source.Multicast(new ReplaySubject<T>());
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, IScheduler scheduler)
		{
			return source.Multicast(new ReplaySubject<T>(scheduler));
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, int bufferSize)
		{
			return source.Multicast(new ReplaySubject<T>(bufferSize));
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, int bufferSize, IScheduler scheduler)
		{
			return source.Multicast(new ReplaySubject<T>(bufferSize, scheduler));
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, TimeSpan window)
		{
			return source.Multicast(new ReplaySubject<T>(window));
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, TimeSpan window, IScheduler scheduler)
		{
			return source.Multicast(new ReplaySubject<T>(window, scheduler));
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, int bufferSize, TimeSpan window, IScheduler scheduler)
		{
			return source.Multicast(new ReplaySubject<T>(bufferSize, window, scheduler));
		}

		public static IObservable<T> RefCount<T>(this IConnectableObservable<T> source)
		{
			IDisposable connection = null;
			object gate = new object();
			int refCount = 0;
			return Create(delegate(IObserver<T> observer)
			{
				IDisposable subscription = source.Subscribe(observer);
				lock (gate)
				{
					if (++refCount == 1)
					{
						connection = source.Connect();
					}
				}
				return Disposable.Create(delegate
				{
					subscription.Dispose();
					lock (gate)
					{
						if (--refCount == 0)
						{
							connection.Dispose();
						}
					}
				});
			});
		}

		public static T Wait<T>(this IObservable<T> source)
		{
			return WaitCore(source, throwOnEmpty: true, InfiniteTimeSpan);
		}

		public static T Wait<T>(this IObservable<T> source, TimeSpan timeout)
		{
			return WaitCore(source, throwOnEmpty: true, timeout);
		}

		private static T WaitCore<T>(IObservable<T> source, bool throwOnEmpty, TimeSpan timeout)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			ManualResetEvent semaphore = new ManualResetEvent(initialState: false);
			bool seenValue = false;
			T value = (T)default(T);
			Exception ex = null;
			using (source.Subscribe(delegate(T x)
			{
				seenValue = true;
				value = (T)x;
			}, delegate(Exception x)
			{
				ex = x;
				semaphore.Set();
			}, delegate
			{
				semaphore.Set();
			}))
			{
				if (!((!(timeout == InfiniteTimeSpan)) ? semaphore.WaitOne(timeout) : semaphore.WaitOne()))
				{
					throw new TimeoutException("OnCompleted not fired.");
				}
			}
			if (ex != null)
			{
				throw ex;
			}
			if (throwOnEmpty && !seenValue)
			{
				throw new InvalidOperationException("No Elements.");
			}
			return (T)value;
		}

		public static IObservable<TSource> Concat<TSource>(params IObservable<TSource>[] sources)
		{
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			return ConcatCore(sources);
		}

		public static IObservable<TSource> Concat<TSource>(this IEnumerable<IObservable<TSource>> sources)
		{
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			return ConcatCore(sources);
		}

		public static IObservable<TSource> Concat<TSource>(this IObservable<IObservable<TSource>> sources)
		{
			return sources.Merge(1);
		}

		public static IObservable<TSource> Concat<TSource>(this IObservable<TSource> first, IObservable<TSource> second)
		{
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}
			if (second == null)
			{
				throw new ArgumentNullException("second");
			}
			return ConcatCore(new IObservable<TSource>[2]
			{
				first,
				second
			});
		}

		private static IObservable<T> ConcatCore<T>(IEnumerable<IObservable<T>> sources)
		{
			return Create(delegate(IObserver<T> observer)
			{
				bool isDisposed = false;
				IEnumerator<IObservable<T>> e = (IEnumerator<IObservable<T>>)sources.AsSafeEnumerable().GetEnumerator();
				SerialDisposable subscription = new SerialDisposable();
				object gate = new object();
				IDisposable disposable = Scheduler.DefaultSchedulers.TailRecursion.Schedule(delegate(Action self)
				{
					lock (gate)
					{
						if (!isDisposed)
						{
							bool flag = false;
							Exception ex = null;
							try
							{
								flag = e.MoveNext();
								if (flag)
								{
									IObservable<T> current = ((IEnumerator<IObservable<T>>)e).Current;
									if (current == null)
									{
										throw new InvalidOperationException("sequence is null.");
									}
								}
								else
								{
									e.Dispose();
								}
							}
							catch (Exception ex2)
							{
								ex = ex2;
								e.Dispose();
							}
							if (ex != null)
							{
								observer.OnError(ex);
							}
							else if (!flag)
							{
								observer.OnCompleted();
							}
							else
							{
								IObservable<T> current2 = ((IEnumerator<IObservable<T>>)e).Current;
								SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
								subscription.Disposable = singleAssignmentDisposable;
								SingleAssignmentDisposable singleAssignmentDisposable2 = singleAssignmentDisposable;
								IObservable<T> source = current2;
								IObserver<T> observer2 = observer;
								Action<T> onNext = observer2.OnNext;
								IObserver<T> observer3 = observer;
								singleAssignmentDisposable2.Disposable = source.Subscribe(onNext, observer3.OnError, self);
							}
						}
					}
				});
				return new CompositeDisposable(disposable, subscription, Disposable.Create(delegate
				{
					lock (gate)
					{
						isDisposed = true;
						e.Dispose();
					}
				}));
			});
		}

		public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources)
		{
			return sources.Merge(Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, IScheduler scheduler)
		{
			return sources.ToObservable(scheduler).Merge();
		}

		public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, int maxConcurrent)
		{
			return sources.Merge(maxConcurrent, Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, int maxConcurrent, IScheduler scheduler)
		{
			return sources.ToObservable(scheduler).Merge(maxConcurrent);
		}

		public static IObservable<TSource> Merge<TSource>(params IObservable<TSource>[] sources)
		{
			return Merge(Scheduler.DefaultSchedulers.ConstantTimeOperations, sources);
		}

		public static IObservable<TSource> Merge<TSource>(IScheduler scheduler, params IObservable<TSource>[] sources)
		{
			return sources.ToObservable(scheduler).Merge();
		}

		public static IObservable<T> Merge<T>(this IObservable<T> first, IObservable<T> second)
		{
			return Merge(new IObservable<T>[2]
			{
				first,
				second
			});
		}

		public static IObservable<T> Merge<T>(this IObservable<T> first, IObservable<T> second, IScheduler scheduler)
		{
			return Merge<T>(scheduler, first, second);
		}

		public static IObservable<T> Merge<T>(this IObservable<IObservable<T>> sources)
		{
			return Create(delegate(IObserver<T> observer)
			{
				object gate = new object();
				bool isStopped = false;
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				CompositeDisposable group = new CompositeDisposable
				{
					singleAssignmentDisposable
				};
				singleAssignmentDisposable.Disposable = sources.Subscribe(delegate(IObservable<T> innerSource)
				{
					SingleAssignmentDisposable innerSubscription = new SingleAssignmentDisposable();
					group.Add(innerSubscription);
					innerSubscription.Disposable = innerSource.Subscribe(delegate(T x)
					{
						lock (gate)
						{
							observer.OnNext(x);
						}
					}, delegate(Exception exception)
					{
						lock (gate)
						{
							observer.OnError(exception);
						}
					}, delegate
					{
						group.Remove(innerSubscription);
						if (isStopped && group.Count == 1)
						{
							lock (gate)
							{
								observer.OnCompleted();
							}
						}
					});
				}, delegate(Exception exception)
				{
					lock (gate)
					{
						observer.OnError(exception);
					}
				}, delegate
				{
					isStopped = true;
					if (group.Count == 1)
					{
						lock (gate)
						{
							observer.OnCompleted();
						}
					}
				});
				return group;
			});
		}

		public static IObservable<T> Merge<T>(this IObservable<IObservable<T>> sources, int maxConcurrent)
		{
			return Create(delegate(IObserver<T> observer)
			{
				object gate = new object();
				Queue<IObservable<T>> q = (Queue<IObservable<T>>)new Queue<IObservable<T>>();
				bool isStopped = false;
				CompositeDisposable group = new CompositeDisposable();
				int activeCount = 0;
				Action<IObservable<T>> subscribe = null;
				subscribe = (Action<IObservable<T>>)(Action<IObservable<T>>)delegate(IObservable<T> xs)
				{
					SingleAssignmentDisposable subscription = new SingleAssignmentDisposable();
					group.Add(subscription);
					subscription.Disposable = xs.Subscribe(delegate(T x)
					{
						lock (gate)
						{
							observer.OnNext(x);
						}
					}, delegate(Exception exception)
					{
						lock (gate)
						{
							observer.OnError(exception);
						}
					}, delegate
					{
						group.Remove(subscription);
						lock (gate)
						{
							if (((Queue<IObservable<T>>)q).Count > 0)
							{
								IObservable<T> obj = ((Queue<IObservable<T>>)q).Dequeue();
								subscribe(obj);
							}
							else
							{
								activeCount--;
								if (isStopped && activeCount == 0)
								{
									observer.OnCompleted();
								}
							}
						}
					});
				};
				group.Add(sources.Subscribe(delegate(IObservable<T> innerSource)
				{
					lock (gate)
					{
						if (activeCount < maxConcurrent)
						{
							activeCount++;
							subscribe(innerSource);
						}
						else
						{
							((Queue<IObservable<T>>)q).Enqueue(innerSource);
						}
					}
				}, delegate(Exception exception)
				{
					lock (gate)
					{
						observer.OnError(exception);
					}
				}, delegate
				{
					lock (gate)
					{
						isStopped = true;
						if (activeCount == 0)
						{
							observer.OnCompleted();
						}
					}
				}));
				return group;
			});
		}

		public static IObservable<TResult> Zip<TLeft, TRight, TResult>(this IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector)
		{
			return Create(delegate(IObserver<TResult> observer)
			{
				object gate = new object();
				Queue<TLeft> leftQ = (Queue<TLeft>)new Queue<TLeft>();
				bool leftCompleted = false;
				Queue<TRight> rightQ = (Queue<TRight>)new Queue<TRight>();
				bool rightCompleted = false;
				Action dequeue = delegate
				{
					if (((Queue<TLeft>)leftQ).Count != 0 && ((Queue<TRight>)rightQ).Count != 0)
					{
						TLeft arg = ((Queue<TLeft>)leftQ).Dequeue();
						TRight arg2 = ((Queue<TRight>)rightQ).Dequeue();
						TResult value;
						try
						{
							value = selector(arg, arg2);
						}
						catch (Exception error)
						{
							observer.OnError(error);
							return;
						}
						observer.OnNext(value);
					}
					else if (leftCompleted || rightCompleted)
					{
						observer.OnCompleted();
					}
				};
				IObservable<TLeft> source = left.Synchronize(gate);
				Action<TLeft> onNext = delegate(TLeft x)
				{
					((Queue<TLeft>)leftQ).Enqueue(x);
					dequeue();
				};
				IObserver<TResult> observer2 = observer;
				IDisposable item = source.Subscribe(onNext, observer2.OnError, delegate
				{
					leftCompleted = true;
					if (rightCompleted)
					{
						observer.OnCompleted();
					}
				});
				IObservable<TRight> source2 = right.Synchronize(gate);
				Action<TRight> onNext2 = delegate(TRight x)
				{
					((Queue<TRight>)rightQ).Enqueue(x);
					dequeue();
				};
				IObserver<TResult> observer3 = observer;
				IDisposable item2 = source2.Subscribe(onNext2, observer3.OnError, delegate
				{
					rightCompleted = true;
					if (leftCompleted)
					{
						observer.OnCompleted();
					}
				});
				return new CompositeDisposable
				{
					item,
					item2,
					Disposable.Create(delegate
					{
						lock (gate)
						{
							((Queue<TLeft>)leftQ).Clear();
							((Queue<TRight>)rightQ).Clear();
						}
					})
				};
			});
		}

		public static IObservable<IList<T>> Zip<T>(this IEnumerable<IObservable<T>> sources)
		{
			return Zip(sources.ToArray());
		}

		public static IObservable<IList<T>> Zip<T>(params IObservable<T>[] sources)
		{
			return Create(delegate(IObserver<IList<T>> observer)
			{
				object gate = new object();
				int num = sources.Length;
				Queue<T>[] queues = (Queue<T>[])new Queue<T>[num];
				for (int j = 0; j < num; j++)
				{
					queues[j] = (Queue<T>)new Queue<T>();
				}
				bool[] isDone = new bool[num];
				Action<int> dequeue = delegate(int index)
				{
					lock (gate)
					{
						if (((IEnumerable<Queue<T>>)queues).All((Queue<T> x) => x.Count > 0))
						{
							List<T> value = (from x in (IEnumerable<Queue<T>>)queues
								select x.Dequeue()).ToList();
							observer.OnNext(value);
						}
						else if (isDone.Where((bool x, int i) => i != index).All((bool x) => x))
						{
							observer.OnCompleted();
						}
					}
				};
				SingleAssignmentDisposable[] disposables = sources.Select(delegate(IObservable<T> source, int index)
				{
					SingleAssignmentDisposable d = new SingleAssignmentDisposable();
					d.Disposable = source.Subscribe(delegate(T x)
					{
						lock (gate)
						{
							((Queue<T>)queues[index]).Enqueue(x);
							dequeue(index);
						}
					}, delegate(Exception ex)
					{
						lock (gate)
						{
							observer.OnError(ex);
						}
					}, delegate
					{
						lock (gate)
						{
							isDone[index] = true;
							if (isDone.All((bool x) => x))
							{
								observer.OnCompleted();
							}
							else
							{
								d.Dispose();
							}
						}
					});
					return d;
				}).ToArray();
				return new CompositeDisposable(disposables)
				{
					Disposable.Create(delegate
					{
						lock (gate)
						{
							Queue<T>[] array = (Queue<T>[])queues;
							foreach (Queue<T> queue in array)
							{
								queue.Clear();
							}
						}
					})
				};
			});
		}

		public static IObservable<TResult> CombineLatest<TLeft, TRight, TResult>(this IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector)
		{
			return Create(delegate(IObserver<TResult> observer)
			{
				object gate = new object();
				TLeft leftValue = (TLeft)default(TLeft);
				bool leftStarted = false;
				bool leftCompleted = false;
				TRight rightValue = (TRight)default(TRight);
				bool rightStarted = false;
				bool rightCompleted = false;
				Action run = delegate
				{
					if ((leftCompleted && !leftStarted) || (rightCompleted && !rightStarted))
					{
						observer.OnCompleted();
					}
					else if (leftStarted && rightStarted)
					{
						TResult value;
						try
						{
							value = selector((TLeft)leftValue, (TRight)rightValue);
						}
						catch (Exception error)
						{
							observer.OnError(error);
							return;
						}
						observer.OnNext(value);
					}
				};
				IObservable<TLeft> source = left.Synchronize(gate);
				Action<TLeft> onNext = delegate(TLeft x)
				{
					leftStarted = true;
					leftValue = (TLeft)x;
					run();
				};
				IObserver<TResult> observer2 = observer;
				IDisposable item = source.Subscribe(onNext, observer2.OnError, delegate
				{
					leftCompleted = true;
					if (rightCompleted)
					{
						observer.OnCompleted();
					}
				});
				IObservable<TRight> source2 = right.Synchronize(gate);
				Action<TRight> onNext2 = delegate(TRight x)
				{
					rightStarted = true;
					rightValue = (TRight)x;
					run();
				};
				IObserver<TResult> observer3 = observer;
				IDisposable item2 = source2.Subscribe(onNext2, observer3.OnError, delegate
				{
					rightCompleted = true;
					if (leftCompleted)
					{
						observer.OnCompleted();
					}
				});
				return new CompositeDisposable
				{
					item,
					item2
				};
			});
		}

		public static IObservable<IList<T>> CombineLatest<T>(this IEnumerable<IObservable<T>> sources)
		{
			return CombineLatest(sources.ToArray());
		}

		public static IObservable<IList<TSource>> CombineLatest<TSource>(params IObservable<TSource>[] sources)
		{
			return Create(delegate(IObserver<IList<TSource>> observer)
			{
				IObservable<TSource>[] array = sources.ToArray();
				int num = array.Length;
				bool[] hasValue = new bool[num];
				bool hasValueAll = false;
				List<TSource> values = (List<TSource>)new List<TSource>(num);
				for (int l = 0; l < num; l++)
				{
					((List<TSource>)values).Add(default(TSource));
				}
				bool[] isDone = new bool[num];
				Action<int> next = delegate(int i)
				{
					hasValue[i] = true;
					if (hasValueAll || (hasValueAll = hasValue.All((bool x) => x)))
					{
						List<TSource> value = ((IEnumerable<TSource>)values).ToList();
						observer.OnNext(value);
					}
					else if (isDone.Where((bool x, int j) => j != i).All((bool x) => x))
					{
						observer.OnCompleted();
					}
				};
				Action<int> done = delegate(int i)
				{
					isDone[i] = true;
					if (isDone.All((bool x) => x))
					{
						observer.OnCompleted();
					}
				};
				SingleAssignmentDisposable[] array2 = new SingleAssignmentDisposable[num];
				object gate = new object();
				for (int m = 0; m < num; m++)
				{
					int k = m;
					SingleAssignmentDisposable[] array3 = array2;
					int num2 = k;
					SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
					SingleAssignmentDisposable singleAssignmentDisposable2 = singleAssignmentDisposable;
					IObservable<TSource> source = array[k].Synchronize(gate);
					Action<TSource> onNext = delegate(TSource x)
					{
						((List<TSource>)values)[k] = x;
						next(k);
					};
					IObserver<IList<TSource>> observer2 = observer;
					singleAssignmentDisposable2.Disposable = source.Subscribe(onNext, observer2.OnError, delegate
					{
						done(k);
					});
					array3[num2] = singleAssignmentDisposable;
				}
				return new CompositeDisposable(array2);
			});
		}

		public static IObservable<T> Switch<T>(this IObservable<IObservable<T>> sources)
		{
			return Create(delegate(IObserver<T> observer)
			{
				object gate = new object();
				SerialDisposable innerSubscription = new SerialDisposable();
				bool isStopped = false;
				ulong latest = 0uL;
				bool hasLatest = false;
				IDisposable disposable = sources.Subscribe(delegate(IObservable<T> innerSource)
				{
					ulong id = 0uL;
					lock (gate)
					{
						id = ++latest;
						hasLatest = true;
					}
					SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
					innerSubscription.Disposable = singleAssignmentDisposable;
					singleAssignmentDisposable.Disposable = innerSource.Subscribe(delegate(T x)
					{
						lock (gate)
						{
							if (latest == id)
							{
								observer.OnNext(x);
							}
						}
					}, delegate(Exception exception)
					{
						lock (gate)
						{
							if (latest == id)
							{
								observer.OnError(exception);
							}
						}
					}, delegate
					{
						lock (gate)
						{
							if (latest == id)
							{
								hasLatest = false;
								if (isStopped)
								{
									observer.OnCompleted();
								}
							}
						}
					});
				}, delegate(Exception exception)
				{
					lock (gate)
					{
						observer.OnError(exception);
					}
				}, delegate
				{
					lock (gate)
					{
						isStopped = true;
						if (!hasLatest)
						{
							observer.OnCompleted();
						}
					}
				});
				return new CompositeDisposable(disposable, innerSubscription);
			});
		}

		public static IObservable<T[]> WhenAll<T>(params IObservable<T>[] sources)
		{
			if (sources.Length == 0)
			{
				return Return(new T[0]);
			}
			return Create(delegate(IObserver<T[]> observer)
			{
				object gate = new object();
				int length = sources.Length;
				int completedCount = 0;
				T[] values = (T[])new T[length];
				IDisposable[] array = new IDisposable[length];
				for (int i = 0; i < length; i++)
				{
					IObservable<T> source = sources[i];
					int capturedIndex = i;
					array[i] = new SingleAssignmentDisposable
					{
						Disposable = source.Subscribe(delegate(T x)
						{
							lock (gate)
							{
								((T[])values)[capturedIndex] = x;
							}
						}, delegate(Exception ex)
						{
							lock (gate)
							{
								observer.OnError(ex);
							}
						}, delegate
						{
							lock (gate)
							{
								completedCount++;
								if (completedCount == length)
								{
									observer.OnNext((T[])values);
									observer.OnCompleted();
								}
							}
						})
					};
				}
				return new CompositeDisposable(array);
			});
		}

		public static IObservable<T[]> WhenAll<T>(this IEnumerable<IObservable<T>> sources)
		{
			IObservable<T>[] array = sources as IObservable<T>[];
			if (array != null)
			{
				return WhenAll(array);
			}
			return Create(delegate(IObserver<T[]> observer)
			{
				IList<IObservable<T>> list = sources as IList<IObservable<T>>;
				if (list == null)
				{
					list = new List<IObservable<T>>();
					foreach (IObservable<T> source2 in sources)
					{
						list.Add(source2);
					}
				}
				object gate = new object();
				int length = list.Count;
				int completedCount = 0;
				T[] values = (T[])new T[length];
				if (length == 0)
				{
					observer.OnNext((T[])values);
					observer.OnCompleted();
					return Disposable.Empty;
				}
				IDisposable[] array2 = new IDisposable[length];
				for (int i = 0; i < length; i++)
				{
					IObservable<T> source = list[i];
					int capturedIndex = i;
					array2[i] = new SingleAssignmentDisposable
					{
						Disposable = source.Subscribe(delegate(T x)
						{
							lock (gate)
							{
								((T[])values)[capturedIndex] = x;
							}
						}, delegate(Exception ex)
						{
							lock (gate)
							{
								observer.OnError(ex);
							}
						}, delegate
						{
							lock (gate)
							{
								completedCount++;
								if (completedCount == length)
								{
									observer.OnNext((T[])values);
									observer.OnCompleted();
								}
							}
						})
					};
				}
				return new CompositeDisposable(array2);
			});
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, T value)
		{
			return Create(delegate(IObserver<T> observer)
			{
				observer.OnNext(value);
				return source.Subscribe(observer);
			});
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, Func<T> valueFactory)
		{
			return Create(delegate(IObserver<T> observer)
			{
				T value;
				try
				{
					value = valueFactory();
				}
				catch (Exception error)
				{
					observer.OnError(error);
					return Disposable.Empty;
				}
				observer.OnNext(value);
				return source.Subscribe(observer);
			});
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, params T[] values)
		{
			return source.StartWith(Scheduler.DefaultSchedulers.ConstantTimeOperations, values);
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, IEnumerable<T> values)
		{
			return source.StartWith(Scheduler.DefaultSchedulers.ConstantTimeOperations, values);
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, IScheduler scheduler, T value)
		{
			return Return(value, scheduler).Concat(source);
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, IScheduler scheduler, IEnumerable<T> values)
		{
			T[] array = values as T[];
			if (array == null)
			{
				array = values.ToArray();
			}
			return source.StartWith(scheduler, array);
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, IScheduler scheduler, params T[] values)
		{
			return values.ToObservable(scheduler).Concat(source);
		}

		public static IObservable<T> Synchronize<T>(this IObservable<T> source)
		{
			return source.Synchronize(new object());
		}

		public static IObservable<T> Synchronize<T>(this IObservable<T> source, object gate)
		{
			return Create((IObserver<T> observer) => source.Subscribe(delegate(T x)
			{
				lock (gate)
				{
					observer.OnNext(x);
				}
			}, delegate(Exception x)
			{
				lock (gate)
				{
					observer.OnError(x);
				}
			}, delegate
			{
				lock (gate)
				{
					observer.OnCompleted();
				}
			}));
		}

		public static IObservable<T> ObserveOn<T>(this IObservable<T> source, IScheduler scheduler)
		{
			return Create(delegate(IObserver<T> observer)
			{
				CompositeDisposable group = new CompositeDisposable();
				IDisposable item = source.Subscribe(delegate(T x)
				{
					IDisposable item4 = scheduler.Schedule(delegate
					{
						observer.OnNext(x);
					});
					group.Add(item4);
				}, delegate(Exception ex)
				{
					IDisposable item3 = scheduler.Schedule(delegate
					{
						observer.OnError(ex);
					});
					group.Add(item3);
				}, delegate
				{
					IDisposable item2 = scheduler.Schedule(delegate
					{
						observer.OnCompleted();
					});
					group.Add(item2);
				});
				group.Add(item);
				return group;
			});
		}

		public static IObservable<T> SubscribeOn<T>(this IObservable<T> source, IScheduler scheduler)
		{
			return Create(delegate(IObserver<T> observer)
			{
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				SerialDisposable d = new SerialDisposable();
				d.Disposable = singleAssignmentDisposable;
				singleAssignmentDisposable.Disposable = scheduler.Schedule(delegate
				{
					d.Disposable = new ScheduledDisposable(scheduler, source.Subscribe(observer));
				});
				return d;
			});
		}

		public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, TimeSpan dueTime)
		{
			return source.DelaySubscription(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, TimeSpan dueTime, IScheduler scheduler)
		{
			return Create(delegate(IObserver<T> observer)
			{
				MultipleAssignmentDisposable d = new MultipleAssignmentDisposable();
				TimeSpan dueTime2 = Scheduler.Normalize(dueTime);
				d.Disposable = scheduler.Schedule(dueTime2, delegate
				{
					d.Disposable = source.Subscribe(observer);
				});
				return d;
			});
		}

		public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, DateTimeOffset dueTime)
		{
			return source.DelaySubscription(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, DateTimeOffset dueTime, IScheduler scheduler)
		{
			return Create(delegate(IObserver<T> observer)
			{
				MultipleAssignmentDisposable d = new MultipleAssignmentDisposable();
				d.Disposable = scheduler.Schedule(dueTime, (Action)delegate
				{
					d.Disposable = source.Subscribe(observer);
				});
				return d;
			});
		}

		public static IObservable<T> Amb<T>(params IObservable<T>[] sources)
		{
			return Amb((IEnumerable<IObservable<T>>)sources);
		}

		public static IObservable<T> Amb<T>(IEnumerable<IObservable<T>> sources)
		{
			IObservable<T> observable = Never<T>();
			foreach (IObservable<T> source in sources)
			{
				IObservable<T> second = source;
				observable = observable.Amb(second);
			}
			return observable;
		}

		public static IObservable<T> Amb<T>(this IObservable<T> source, IObservable<T> second)
		{
			return Create(delegate(IObserver<T> observer)
			{
				AmbState choice = AmbState.Neither;
				object gate = new object();
				SingleAssignmentDisposable leftSubscription = new SingleAssignmentDisposable();
				SingleAssignmentDisposable rightSubscription = new SingleAssignmentDisposable();
				leftSubscription.Disposable = source.Subscribe(delegate(T x)
				{
					lock (gate)
					{
						if (choice == AmbState.Neither)
						{
							choice = AmbState.Left;
							rightSubscription.Dispose();
						}
					}
					if (choice == AmbState.Left)
					{
						observer.OnNext(x);
					}
				}, delegate(Exception ex)
				{
					lock (gate)
					{
						if (choice == AmbState.Neither)
						{
							choice = AmbState.Left;
							rightSubscription.Dispose();
						}
					}
					if (choice == AmbState.Left)
					{
						observer.OnError(ex);
					}
				}, delegate
				{
					lock (gate)
					{
						if (choice == AmbState.Neither)
						{
							choice = AmbState.Left;
							rightSubscription.Dispose();
						}
					}
					if (choice == AmbState.Left)
					{
						observer.OnCompleted();
					}
				});
				rightSubscription.Disposable = second.Subscribe(delegate(T x)
				{
					lock (gate)
					{
						if (choice == AmbState.Neither)
						{
							choice = AmbState.Right;
							leftSubscription.Dispose();
						}
					}
					if (choice == AmbState.Right)
					{
						observer.OnNext(x);
					}
				}, delegate(Exception ex)
				{
					lock (gate)
					{
						if (choice == AmbState.Neither)
						{
							choice = AmbState.Right;
							leftSubscription.Dispose();
						}
					}
					if (choice == AmbState.Right)
					{
						observer.OnError(ex);
					}
				}, delegate
				{
					lock (gate)
					{
						if (choice == AmbState.Neither)
						{
							choice = AmbState.Right;
							leftSubscription.Dispose();
						}
					}
					if (choice == AmbState.Right)
					{
						observer.OnCompleted();
					}
				});
				return new CompositeDisposable
				{
					leftSubscription,
					rightSubscription
				};
			});
		}

		public static IObservable<T> AsObservable<T>(this IObservable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return Create((IObserver<T> observer) => source.Subscribe(observer));
		}

		public static IObservable<T> ToObservable<T>(this IEnumerable<T> source)
		{
			return source.ToObservable(Scheduler.DefaultSchedulers.Iteration);
		}

		public static IObservable<T> ToObservable<T>(this IEnumerable<T> source, IScheduler scheduler)
		{
			return Create(delegate(IObserver<T> observer)
			{
				IEnumerator<T> e;
				try
				{
					e = (IEnumerator<T>)source.AsSafeEnumerable().GetEnumerator();
				}
				catch (Exception error)
				{
					observer.OnError(error);
					return Disposable.Empty;
				}
				SingleAssignmentDisposable flag = new SingleAssignmentDisposable();
				flag.Disposable = scheduler.Schedule(delegate(Action self)
				{
					if (flag.IsDisposed)
					{
						e.Dispose();
					}
					else
					{
						T value = default(T);
						bool flag2;
						try
						{
							flag2 = e.MoveNext();
							if (flag2)
							{
								value = ((IEnumerator<T>)e).Current;
							}
						}
						catch (Exception error2)
						{
							e.Dispose();
							observer.OnError(error2);
							return;
						}
						if (flag2)
						{
							observer.OnNext(value);
							self();
						}
						else
						{
							e.Dispose();
							observer.OnCompleted();
						}
					}
				});
				return flag;
			});
		}

		public static IObservable<TResult> Cast<TSource, TResult>(this IObservable<TSource> source)
		{
			return from x in source
				select (TResult)(object)x;
		}

		public static IObservable<TResult> Cast<TSource, TResult>(this IObservable<TSource> source, TResult witness)
		{
			return from x in source
				select (TResult)(object)x;
		}

		public static IObservable<TResult> OfType<TSource, TResult>(this IObservable<TSource> source)
		{
			return from x in source
				where x is TResult
				select (TResult)(object)x;
		}

		public static IObservable<TResult> OfType<TSource, TResult>(this IObservable<TSource> source, TResult witness)
		{
			return from x in source
				where x is TResult
				select (TResult)(object)x;
		}

		public static IObservable<Unit> AsUnitObservable<T>(this IObservable<T> source)
		{
			return Create(delegate(IObserver<Unit> observer)
			{
				IObservable<T> observable = source;
				Action<T> onNext = delegate
				{
					observer.OnNext(Unit.Default);
				};
				IObserver<Unit> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<Unit> observer3 = observer;
				return observable.Subscribe(Observer.Create(onNext, onError, observer3.OnCompleted));
			});
		}

		public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe)
		{
			if (subscribe == null)
			{
				throw new ArgumentNullException("subscribe");
			}
			return new AnonymousObservable<T>(subscribe);
		}

		public static IObservable<T> Empty<T>()
		{
			return Empty<T>(Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<T> Empty<T>(IScheduler scheduler)
		{
			return Create((IObserver<T> observer) => scheduler.Schedule(observer.OnCompleted));
		}

		public static IObservable<T> Empty<T>(T witness)
		{
			return Empty<T>(Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<T> Empty<T>(IScheduler scheduler, T witness)
		{
			return Empty<T>(scheduler);
		}

		public static IObservable<T> Never<T>()
		{
			return Create((IObserver<T> observer) => Disposable.Empty);
		}

		public static IObservable<T> Never<T>(T witness)
		{
			return Never<T>();
		}

		public static IObservable<T> Return<T>(T value)
		{
			return Return(value, Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<T> Return<T>(T value, IScheduler scheduler)
		{
			return Create((IObserver<T> observer) => scheduler.Schedule(delegate
			{
				observer.OnNext(value);
				observer.OnCompleted();
			}));
		}

		public static IObservable<T> Throw<T>(Exception error)
		{
			return Throw<T>(error, Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<T> Throw<T>(Exception error, T witness)
		{
			return Throw<T>(error, Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<T> Throw<T>(Exception error, IScheduler scheduler)
		{
			return Create((IObserver<T> observer) => scheduler.Schedule(delegate
			{
				observer.OnError(error);
			}));
		}

		public static IObservable<T> Throw<T>(Exception error, IScheduler scheduler, T witness)
		{
			return Throw<T>(error, scheduler);
		}

		public static IObservable<int> Range(int start, int count)
		{
			return Range(start, count, Scheduler.DefaultSchedulers.Iteration);
		}

		public static IObservable<int> Range(int start, int count, IScheduler scheduler)
		{
			return Create(delegate(IObserver<int> observer)
			{
				int i = 0;
				return scheduler.Schedule(delegate(Action self)
				{
					if (i < count)
					{
						int value = start + i;
						observer.OnNext(value);
						Interlocked.Increment(ref i);
						self();
					}
					else
					{
						observer.OnCompleted();
					}
				});
			});
		}

		public static IObservable<T> Repeat<T>(T value)
		{
			return Repeat(value, Scheduler.DefaultSchedulers.Iteration);
		}

		public static IObservable<T> Repeat<T>(T value, IScheduler scheduler)
		{
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			return Create((IObserver<T> observer) => scheduler.Schedule(delegate(Action self)
			{
				observer.OnNext(value);
				self();
			}));
		}

		public static IObservable<T> Repeat<T>(T value, int repeatCount)
		{
			return Repeat(value, repeatCount, Scheduler.DefaultSchedulers.Iteration);
		}

		public static IObservable<T> Repeat<T>(T value, int repeatCount, IScheduler scheduler)
		{
			if (repeatCount < 0)
			{
				throw new ArgumentOutOfRangeException("repeatCount");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			return Create(delegate(IObserver<T> observer)
			{
				int currentCount = repeatCount;
				return scheduler.Schedule(delegate(Action self)
				{
					if (currentCount > 0)
					{
						observer.OnNext(value);
						currentCount--;
					}
					if (currentCount == 0)
					{
						observer.OnCompleted();
					}
					else
					{
						self();
					}
				});
			});
		}

		public static IObservable<T> Repeat<T>(this IObservable<T> source)
		{
			return RepeatInfinite(source).Concat();
		}

		private static IEnumerable<IObservable<T>> RepeatInfinite<T>(IObservable<T> source)
		{
			while (true)
			{
				yield return source;
			}
		}

		public static IObservable<T> RepeatSafe<T>(this IObservable<T> source)
		{
			return RepeatSafeCore(RepeatInfinite(source));
		}

		private static IObservable<T> RepeatSafeCore<T>(IEnumerable<IObservable<T>> sources)
		{
			return Create(delegate(IObserver<T> observer)
			{
				bool isDisposed = false;
				bool isRunNext = false;
				IEnumerator<IObservable<T>> e = (IEnumerator<IObservable<T>>)sources.AsSafeEnumerable().GetEnumerator();
				SerialDisposable subscription = new SerialDisposable();
				object gate = new object();
				IDisposable disposable = Scheduler.DefaultSchedulers.TailRecursion.Schedule(delegate(Action self)
				{
					lock (gate)
					{
						if (!isDisposed)
						{
							bool flag = false;
							Exception ex = null;
							try
							{
								flag = e.MoveNext();
								if (flag)
								{
									IObservable<T> current = ((IEnumerator<IObservable<T>>)e).Current;
									if (current == null)
									{
										throw new InvalidOperationException("sequence is null.");
									}
								}
								else
								{
									e.Dispose();
								}
							}
							catch (Exception ex2)
							{
								ex = ex2;
								e.Dispose();
							}
							if (ex != null)
							{
								observer.OnError(ex);
							}
							else if (!flag)
							{
								observer.OnCompleted();
							}
							else
							{
								IObservable<T> current2 = ((IEnumerator<IObservable<T>>)e).Current;
								SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
								subscription.Disposable = singleAssignmentDisposable;
								SingleAssignmentDisposable singleAssignmentDisposable2 = singleAssignmentDisposable;
								IObservable<T> source = current2;
								Action<T> onNext = delegate(T x)
								{
									isRunNext = true;
									observer.OnNext(x);
								};
								IObserver<T> observer2 = observer;
								singleAssignmentDisposable2.Disposable = source.Subscribe(onNext, observer2.OnError, delegate
								{
									if (isRunNext && !isDisposed)
									{
										isRunNext = false;
										self();
									}
									else
									{
										e.Dispose();
										if (!isDisposed)
										{
											observer.OnCompleted();
										}
									}
								});
							}
						}
					}
				});
				return new CompositeDisposable(disposable, subscription, Disposable.Create(delegate
				{
					lock (gate)
					{
						isDisposed = true;
						e.Dispose();
					}
				}));
			});
		}

		public static IObservable<T> Defer<T>(Func<IObservable<T>> observableFactory)
		{
			return Create(delegate(IObserver<T> observer)
			{
				IObservable<T> observable;
				try
				{
					observable = observableFactory();
				}
				catch (Exception error)
				{
					observable = Throw<T>(error);
				}
				return observable.Subscribe(observer);
			});
		}

		public static IObservable<T> Start<T>(Func<T> function)
		{
			return Start(function, Scheduler.DefaultSchedulers.AsyncConversions);
		}

		public static IObservable<T> Start<T>(Func<T> function, IScheduler scheduler)
		{
			return ToAsync(function, scheduler)();
		}

		public static IObservable<Unit> Start(Action action)
		{
			return Start(action, Scheduler.DefaultSchedulers.AsyncConversions);
		}

		public static IObservable<Unit> Start(Action action, IScheduler scheduler)
		{
			return ToAsync(action, scheduler)();
		}

		public static Func<IObservable<T>> ToAsync<T>(Func<T> function)
		{
			return ToAsync(function, Scheduler.DefaultSchedulers.AsyncConversions);
		}

		public static Func<IObservable<T>> ToAsync<T>(Func<T> function, IScheduler scheduler)
		{
			return delegate
			{
				AsyncSubject<T> subject = (AsyncSubject<T>)new AsyncSubject<T>();
				scheduler.Schedule(delegate
				{
					T value;
					try
					{
						value = function();
					}
					catch (Exception error)
					{
						((AsyncSubject<T>)subject).OnError(error);
						return;
					}
					((AsyncSubject<T>)subject).OnNext(value);
					((AsyncSubject<T>)subject).OnCompleted();
				});
				return ((IObservable<T>)subject).AsObservable();
			};
		}

		public static Func<IObservable<Unit>> ToAsync(Action action)
		{
			return ToAsync(action, Scheduler.DefaultSchedulers.AsyncConversions);
		}

		public static Func<IObservable<Unit>> ToAsync(Action action, IScheduler scheduler)
		{
			return delegate
			{
				AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
				scheduler.Schedule(delegate
				{
					try
					{
						action();
					}
					catch (Exception error)
					{
						subject.OnError(error);
						return;
					}
					subject.OnNext(Unit.Default);
					subject.OnCompleted();
				});
				return subject.AsObservable();
			};
		}

		public static IObservable<T> Finally<T>(this IObservable<T> source, Action finallyAction)
		{
			return Create(delegate(IObserver<T> observer)
			{
				IDisposable subscription;
				try
				{
					subscription = source.Subscribe(observer);
				}
				catch
				{
					finallyAction();
					throw;
				}
				return Disposable.Create(delegate
				{
					try
					{
						subscription.Dispose();
					}
					finally
					{
						finallyAction();
					}
				});
			});
		}

		public static IObservable<T> Catch<T, TException>(this IObservable<T> source, Func<TException, IObservable<T>> errorHandler) where TException : Exception
		{
			return Create(delegate(IObserver<T> observer)
			{
				SerialDisposable serialDisposable = new SerialDisposable();
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				serialDisposable.Disposable = singleAssignmentDisposable;
				SingleAssignmentDisposable singleAssignmentDisposable2 = singleAssignmentDisposable;
				IObservable<T> source2 = source;
				IObserver<T> observer2 = observer;
				Action<T> onNext = observer2.OnNext;
				Action<Exception> onError = delegate(Exception exception)
				{
					TException val = exception as TException;
					if (val != null)
					{
						IObservable<T> observable;
						try
						{
							observable = ((!(errorHandler == new Func<TException, IObservable<T>>(Stubs.CatchIgnore<T>))) ? errorHandler(val) : Empty<T>());
						}
						catch (Exception error)
						{
							observer.OnError(error);
							return;
						}
						SingleAssignmentDisposable singleAssignmentDisposable3 = new SingleAssignmentDisposable();
						serialDisposable.Disposable = singleAssignmentDisposable3;
						singleAssignmentDisposable3.Disposable = observable.Subscribe(observer);
					}
					else
					{
						observer.OnError(exception);
					}
				};
				IObserver<T> observer3 = observer;
				singleAssignmentDisposable2.Disposable = source2.Subscribe(onNext, onError, observer3.OnCompleted);
				return serialDisposable;
			});
		}

		public static IObservable<TSource> Catch<TSource>(this IEnumerable<IObservable<TSource>> sources)
		{
			return Create(delegate(IObserver<TSource> observer)
			{
				object gate = new object();
				bool isDisposed = false;
				IEnumerator<IObservable<TSource>> e = (IEnumerator<IObservable<TSource>>)sources.AsSafeEnumerable().GetEnumerator();
				SerialDisposable subscription = new SerialDisposable();
				Exception lastException = null;
				IDisposable disposable = Scheduler.DefaultSchedulers.TailRecursion.Schedule(delegate(Action self)
				{
					lock (gate)
					{
						IObservable<TSource> observable = null;
						bool flag = false;
						Exception ex = null;
						if (!isDisposed)
						{
							try
							{
								flag = e.MoveNext();
								if (flag)
								{
									observable = ((IEnumerator<IObservable<TSource>>)e).Current;
								}
								else
								{
									e.Dispose();
								}
							}
							catch (Exception ex2)
							{
								ex = ex2;
								e.Dispose();
							}
							if (ex != null)
							{
								observer.OnError(ex);
							}
							else if (!flag)
							{
								if (lastException != null)
								{
									observer.OnError(lastException);
								}
								else
								{
									observer.OnCompleted();
								}
							}
							else
							{
								SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
								subscription.Disposable = singleAssignmentDisposable;
								SingleAssignmentDisposable singleAssignmentDisposable2 = singleAssignmentDisposable;
								IObservable<TSource> source = observable;
								IObserver<TSource> observer2 = observer;
								Action<TSource> onNext = observer2.OnNext;
								Action<Exception> onError = delegate(Exception exception)
								{
									lastException = exception;
									self();
								};
								IObserver<TSource> observer3 = observer;
								singleAssignmentDisposable2.Disposable = source.Subscribe(onNext, onError, observer3.OnCompleted);
							}
						}
					}
				});
				return new CompositeDisposable(subscription, disposable, Disposable.Create(delegate
				{
					lock (gate)
					{
						e.Dispose();
						isDisposed = true;
					}
				}));
			});
		}

		public static IObservable<TSource> CatchIgnore<TSource>(this IObservable<TSource> source)
		{
			return source.Catch<TSource, Exception>(Stubs.CatchIgnore<TSource>);
		}

		public static IObservable<TSource> CatchIgnore<TSource, TException>(this IObservable<TSource> source, Action<TException> errorAction) where TException : Exception
		{
			return source.Catch(delegate(TException ex)
			{
				errorAction(ex);
				return Empty<TSource>();
			});
		}

		public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source)
		{
			return RepeatInfinite(source).Catch();
		}

		public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source, int retryCount)
		{
			return Enumerable.Repeat(source, retryCount).Catch();
		}

		public static IObservable<TSource> OnErrorRetry<TSource>(this IObservable<TSource> source)
		{
			return source.Retry();
		}

		public static IObservable<TSource> OnErrorRetry<TSource, TException>(this IObservable<TSource> source, Action<TException> onError) where TException : Exception
		{
			return source.OnErrorRetry(onError, TimeSpan.Zero);
		}

		public static IObservable<TSource> OnErrorRetry<TSource, TException>(this IObservable<TSource> source, Action<TException> onError, TimeSpan delay) where TException : Exception
		{
			return source.OnErrorRetry(onError, int.MaxValue, delay);
		}

		public static IObservable<TSource> OnErrorRetry<TSource, TException>(this IObservable<TSource> source, Action<TException> onError, int retryCount) where TException : Exception
		{
			return source.OnErrorRetry(onError, retryCount, TimeSpan.Zero);
		}

		public static IObservable<TSource> OnErrorRetry<TSource, TException>(this IObservable<TSource> source, Action<TException> onError, int retryCount, TimeSpan delay) where TException : Exception
		{
			return source.OnErrorRetry(onError, retryCount, delay, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<TSource> OnErrorRetry<TSource, TException>(this IObservable<TSource> source, Action<TException> onError, int retryCount, TimeSpan delay, IScheduler delayScheduler) where TException : Exception
		{
			return Defer(delegate
			{
				TimeSpan dueTime = (delay.Ticks >= 0) ? delay : TimeSpan.Zero;
				int count = 0;
				IObservable<TSource> self = null;
				self = (IObservable<TSource>)source.Catch(delegate(TException ex)
				{
					onError(ex);
					object result;
					if (++count < retryCount)
					{
						object observable2;
						IObservable<TSource> observable;
						if (dueTime == TimeSpan.Zero)
						{
							observable = ((IObservable<TSource>)self).SubscribeOn(Scheduler.CurrentThread);
							observable2 = observable;
						}
						else
						{
							observable2 = ((IObservable<TSource>)self).DelaySubscription(dueTime, delayScheduler).SubscribeOn(Scheduler.CurrentThread);
						}
						observable = (IObservable<TSource>)observable2;
						result = observable;
					}
					else
					{
						result = Throw<TSource>(ex);
					}
					return (IObservable<TSource>)result;
				});
				return (IObservable<TSource>)self;
			});
		}

		public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler) where TEventArgs : EventArgs
		{
			return Create(delegate(IObserver<EventPattern<TEventArgs>> observer)
			{
				TDelegate handler = (TDelegate)conversion(delegate(object sender, TEventArgs eventArgs)
				{
					observer.OnNext(new EventPattern<TEventArgs>(sender, eventArgs));
				});
				addHandler((TDelegate)handler);
				return Disposable.Create(delegate
				{
					removeHandler((TDelegate)handler);
				});
			});
		}

		public static IObservable<Unit> FromEvent<TDelegate>(Func<Action, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
		{
			return Create(delegate(IObserver<Unit> observer)
			{
				TDelegate handler = (TDelegate)conversion(delegate
				{
					observer.OnNext(Unit.Default);
				});
				addHandler((TDelegate)handler);
				return Disposable.Create(delegate
				{
					removeHandler((TDelegate)handler);
				});
			});
		}

		public static IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
		{
			return Create(delegate(IObserver<TEventArgs> observer)
			{
				TDelegate handler = (TDelegate)conversion(observer.OnNext);
				addHandler((TDelegate)handler);
				return Disposable.Create(delegate
				{
					removeHandler((TDelegate)handler);
				});
			});
		}

		public static IObservable<Unit> FromEvent(Action<Action> addHandler, Action<Action> removeHandler)
		{
			return Create(delegate(IObserver<Unit> observer)
			{
				Action handler = delegate
				{
					observer.OnNext(Unit.Default);
				};
				addHandler(handler);
				return Disposable.Create(delegate
				{
					removeHandler(handler);
				});
			});
		}

		public static IObservable<T> FromEvent<T>(Action<Action<T>> addHandler, Action<Action<T>> removeHandler)
		{
			return Create(delegate(IObserver<T> observer)
			{
				Action<T> handler = (Action<T>)(Action<T>)delegate(T x)
				{
					observer.OnNext(x);
				};
				addHandler((Action<T>)handler);
				return Disposable.Create(delegate
				{
					removeHandler((Action<T>)handler);
				});
			});
		}

		public static Func<IObservable<TResult>> FromAsyncPattern<TResult>(Func<AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
		{
			return delegate
			{
				AsyncSubject<TResult> subject = (AsyncSubject<TResult>)new AsyncSubject<TResult>();
				try
				{
					begin(delegate(IAsyncResult iar)
					{
						TResult value;
						try
						{
							value = end(iar);
						}
						catch (Exception error2)
						{
							((AsyncSubject<TResult>)subject).OnError(error2);
							return;
						}
						((AsyncSubject<TResult>)subject).OnNext(value);
						((AsyncSubject<TResult>)subject).OnCompleted();
					}, null);
				}
				catch (Exception error)
				{
					return Throw<TResult>(error, Scheduler.DefaultSchedulers.AsyncConversions);
				}
				return ((IObservable<TResult>)subject).AsObservable();
			};
		}

		public static Func<T1, IObservable<TResult>> FromAsyncPattern<T1, TResult>(Func<T1, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
		{
			return delegate(T1 x)
			{
				AsyncSubject<TResult> subject = (AsyncSubject<TResult>)new AsyncSubject<TResult>();
				try
				{
					begin(x, delegate(IAsyncResult iar)
					{
						TResult value;
						try
						{
							value = end(iar);
						}
						catch (Exception error2)
						{
							((AsyncSubject<TResult>)subject).OnError(error2);
							return;
						}
						((AsyncSubject<TResult>)subject).OnNext(value);
						((AsyncSubject<TResult>)subject).OnCompleted();
					}, null);
				}
				catch (Exception error)
				{
					return Throw<TResult>(error, Scheduler.DefaultSchedulers.AsyncConversions);
				}
				return ((IObservable<TResult>)subject).AsObservable();
			};
		}

		public static Func<T1, T2, IObservable<TResult>> FromAsyncPattern<T1, T2, TResult>(Func<T1, T2, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
		{
			return delegate(T1 x, T2 y)
			{
				AsyncSubject<TResult> subject = (AsyncSubject<TResult>)new AsyncSubject<TResult>();
				try
				{
					begin(x, y, delegate(IAsyncResult iar)
					{
						TResult value;
						try
						{
							value = end(iar);
						}
						catch (Exception error2)
						{
							((AsyncSubject<TResult>)subject).OnError(error2);
							return;
						}
						((AsyncSubject<TResult>)subject).OnNext(value);
						((AsyncSubject<TResult>)subject).OnCompleted();
					}, null);
				}
				catch (Exception error)
				{
					return Throw<TResult>(error, Scheduler.DefaultSchedulers.AsyncConversions);
				}
				return ((IObservable<TResult>)subject).AsObservable();
			};
		}

		public static Func<IObservable<Unit>> FromAsyncPattern(Func<AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
		{
			return FromAsyncPattern(begin, delegate(IAsyncResult iar)
			{
				end(iar);
				return Unit.Default;
			});
		}

		public static Func<T1, IObservable<Unit>> FromAsyncPattern<T1>(Func<T1, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
		{
			return FromAsyncPattern(begin, delegate(IAsyncResult iar)
			{
				end(iar);
				return Unit.Default;
			});
		}

		public static Func<T1, T2, IObservable<Unit>> FromAsyncPattern<T1, T2>(Func<T1, T2, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
		{
			return FromAsyncPattern(begin, delegate(IAsyncResult iar)
			{
				end(iar);
				return Unit.Default;
			});
		}

		public static IObservable<T> Take<T>(this IObservable<T> source, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count == 0)
			{
				return Empty<T>();
			}
			return Create(delegate(IObserver<T> observer)
			{
				int rest = count;
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					if (rest > 0)
					{
						rest--;
						observer.OnNext(x);
						if (rest == 0)
						{
							observer.OnCompleted();
						}
					}
				};
				IObserver<T> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<T> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IObservable<T> TakeWhile<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.TakeWhile((T x, int i) => predicate(x));
		}

		public static IObservable<T> TakeWhile<T>(this IObservable<T> source, Func<T, int, bool> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			return Create(delegate(IObserver<T> observer)
			{
				int i = 0;
				bool running = true;
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					try
					{
						running = predicate(x, ++i);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					if (running)
					{
						observer.OnNext(x);
					}
					else
					{
						observer.OnCompleted();
					}
				};
				IObserver<T> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<T> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IObservable<T> TakeUntil<T, TOther>(this IObservable<T> source, IObservable<TOther> other)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			return Create(delegate(IObserver<T> observer)
			{
				object gate = new object();
				IObservable<TOther> source2 = other.Synchronize(gate);
				Action<TOther> onNext = delegate
				{
					observer.OnCompleted();
				};
				IObserver<T> observer2 = observer;
				IDisposable disposable = source2.Subscribe(onNext, observer2.OnError);
				IObservable<T> source3 = source.Synchronize(gate);
				IDisposable disposable2 = disposable;
				IDisposable item = source3.Finally(disposable2.Dispose).Subscribe(observer);
				return new CompositeDisposable
				{
					disposable,
					item
				};
			});
		}

		public static IObservable<T> Skip<T>(this IObservable<T> source, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			return Create(delegate(IObserver<T> observer)
			{
				int index = 0;
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					if (++index >= count)
					{
						observer.OnNext(x);
					}
				};
				IObserver<T> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<T> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IObservable<T> SkipWhile<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.SkipWhile((T x, int i) => predicate(x));
		}

		public static IObservable<T> SkipWhile<T>(this IObservable<T> source, Func<T, int, bool> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			return Create(delegate(IObserver<T> observer)
			{
				int i = 0;
				bool skipEnd = false;
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					if (!skipEnd)
					{
						try
						{
							if (predicate(x, ++i))
							{
								return;
							}
							skipEnd = true;
						}
						catch (Exception error)
						{
							observer.OnError(error);
							return;
						}
					}
					observer.OnNext(x);
				};
				IObserver<T> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<T> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IObservable<T> SkipUntil<T, TOther>(this IObservable<T> source, IObservable<TOther> other)
		{
			return Create(delegate(IObserver<T> observer)
			{
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				SingleAssignmentDisposable otherSubscription = new SingleAssignmentDisposable();
				bool open = false;
				object gate = new object();
				SingleAssignmentDisposable singleAssignmentDisposable2 = singleAssignmentDisposable;
				IObservable<T> source2 = source.Synchronize(gate);
				Action<T> onNext = delegate(T x)
				{
					if (open)
					{
						observer.OnNext(x);
					}
				};
				IObserver<T> observer2 = observer;
				singleAssignmentDisposable2.Disposable = source2.Subscribe(onNext, observer2.OnError, delegate
				{
					if (open)
					{
						observer.OnCompleted();
					}
				});
				SingleAssignmentDisposable singleAssignmentDisposable3 = otherSubscription;
				IObservable<TOther> source3 = other.Synchronize(gate);
				Action<TOther> onNext2 = delegate
				{
					open = true;
					otherSubscription.Dispose();
				};
				IObserver<T> observer3 = observer;
				singleAssignmentDisposable3.Disposable = source3.Subscribe(onNext2, observer3.OnError);
				return new CompositeDisposable(singleAssignmentDisposable, otherSubscription);
			});
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count <= 0");
			}
			return Create(delegate(IObserver<IList<T>> observer)
			{
				List<T> list = (List<T>)new List<T>();
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					((List<T>)list).Add(x);
					if (((List<T>)list).Count == count)
					{
						observer.OnNext((IList<T>)list);
						list = (List<T>)new List<T>();
					}
				};
				IObserver<IList<T>> observer2 = observer;
				return source2.Subscribe(onNext, observer2.OnError, delegate
				{
					if (((List<T>)list).Count > 0)
					{
						observer.OnNext((IList<T>)list);
					}
					observer.OnCompleted();
				});
			});
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, int count, int skip)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count <= 0");
			}
			if (skip <= 0)
			{
				throw new ArgumentOutOfRangeException("skip <= 0");
			}
			return Create(delegate(IObserver<IList<T>> observer)
			{
				Queue<List<T>> q = (Queue<List<T>>)new Queue<List<T>>();
				int index = -1;
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					index++;
					if (index % skip == 0)
					{
						((Queue<List<T>>)q).Enqueue(new List<T>(count));
					}
					int count2 = ((Queue<List<T>>)q).Count;
					for (int i = 0; i < count2; i++)
					{
						List<T> list = ((Queue<List<T>>)q).Dequeue();
						list.Add(x);
						if (list.Count == count)
						{
							observer.OnNext(list);
						}
						else
						{
							((Queue<List<T>>)q).Enqueue(list);
						}
					}
				};
				IObserver<IList<T>> observer2 = observer;
				return source2.Subscribe(onNext, observer2.OnError, delegate
				{
					foreach (List<T> item in (Queue<List<T>>)q)
					{
						observer.OnNext(item);
					}
					observer.OnCompleted();
				});
			});
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan)
		{
			return source.Buffer(timeSpan, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, IScheduler scheduler)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return Create(delegate(IObserver<IList<T>> observer)
			{
				List<T> list = (List<T>)new List<T>();
				object gate = new object();
				CompositeDisposable compositeDisposable = new CompositeDisposable(2)
				{
					scheduler.Schedule(timeSpan, delegate(Action<TimeSpan> self)
					{
						List<T> value2;
						lock (gate)
						{
							value2 = (List<T>)list;
							list = (List<T>)new List<T>();
						}
						observer.OnNext(value2);
						self(timeSpan);
					})
				};
				CompositeDisposable compositeDisposable2 = compositeDisposable;
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					lock (gate)
					{
						((List<T>)list).Add(x);
					}
				};
				IObserver<IList<T>> observer2 = observer;
				compositeDisposable2.Add(source2.Subscribe(onNext, observer2.OnError, delegate
				{
					List<T> value = (List<T>)list;
					observer.OnNext(value);
					observer.OnCompleted();
				}));
				return compositeDisposable;
			});
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, int count)
		{
			return source.Buffer(timeSpan, count, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, int count, IScheduler scheduler)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count <= 0");
			}
			return Create(delegate(IObserver<IList<T>> observer)
			{
				List<T> list = (List<T>)new List<T>();
				object gate = new object();
				long timerId = 0L;
				CompositeDisposable compositeDisposable = new CompositeDisposable(2);
				SerialDisposable timerD = new SerialDisposable();
				compositeDisposable.Add(timerD);
				Action createTimer = delegate
				{
					long currentTimerId = timerId;
					SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
					timerD.Disposable = singleAssignmentDisposable;
					singleAssignmentDisposable.Disposable = scheduler.Schedule(timeSpan, delegate(Action<TimeSpan> self)
					{
						List<T> list3;
						lock (gate)
						{
							if (currentTimerId != timerId)
							{
								return;
							}
							list3 = (List<T>)list;
							if (list3.Count != 0)
							{
								list = (List<T>)new List<T>();
							}
						}
						if (list3.Count != 0)
						{
							observer.OnNext(list3);
						}
						self(timeSpan);
					});
				};
				createTimer();
				CompositeDisposable compositeDisposable2 = compositeDisposable;
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					List<T> list2 = null;
					lock (gate)
					{
						((List<T>)list).Add(x);
						if (((List<T>)list).Count == count)
						{
							list2 = (List<T>)list;
							list = (List<T>)new List<T>();
							timerId++;
							createTimer();
						}
					}
					if (list2 != null)
					{
						observer.OnNext(list2);
					}
				};
				IObserver<IList<T>> observer2 = observer;
				compositeDisposable2.Add(source2.Subscribe(onNext, observer2.OnError, delegate
				{
					lock (gate)
					{
						timerId++;
					}
					List<T> value = (List<T>)list;
					observer.OnNext(value);
					observer.OnCompleted();
				}));
				return compositeDisposable;
			});
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, TimeSpan timeShift)
		{
			return source.Buffer(timeSpan, timeShift, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return Create(delegate(IObserver<IList<T>> observer)
			{
				TimeSpan totalTime = TimeSpan.Zero;
				TimeSpan nextShift = timeShift;
				TimeSpan nextSpan = timeSpan;
				object gate = new object();
				Queue<IList<T>> q = (Queue<IList<T>>)new Queue<IList<T>>();
				SerialDisposable timerD = new SerialDisposable();
				Action createTimer = null;
				createTimer = delegate
				{
					SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
					timerD.Disposable = singleAssignmentDisposable;
					bool isSpan = false;
					bool isShift = false;
					if (nextSpan == nextShift)
					{
						isSpan = true;
						isShift = true;
					}
					else if (nextSpan < nextShift)
					{
						isSpan = true;
					}
					else
					{
						isShift = true;
					}
					TimeSpan timeSpan2 = (!isSpan) ? nextShift : nextSpan;
					TimeSpan dueTime = timeSpan2 - totalTime;
					totalTime = timeSpan2;
					if (isSpan)
					{
						nextSpan += timeShift;
					}
					if (isShift)
					{
						nextShift += timeShift;
					}
					singleAssignmentDisposable.Disposable = scheduler.Schedule(dueTime, delegate
					{
						lock (gate)
						{
							if (isShift)
							{
								List<T> item = new List<T>();
								((Queue<IList<T>>)q).Enqueue((IList<T>)item);
							}
							if (isSpan)
							{
								IList<T> value = ((Queue<IList<T>>)q).Dequeue();
								observer.OnNext(value);
							}
						}
						createTimer();
					});
				};
				((Queue<IList<T>>)q).Enqueue((IList<T>)new List<T>());
				createTimer();
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					lock (gate)
					{
						foreach (IList<T> item2 in (Queue<IList<T>>)q)
						{
							item2.Add(x);
						}
					}
				};
				IObserver<IList<T>> observer2 = observer;
				return source2.Subscribe(onNext, observer2.OnError, delegate
				{
					lock (gate)
					{
						foreach (IList<T> item3 in (Queue<IList<T>>)q)
						{
							observer.OnNext(item3);
						}
						observer.OnCompleted();
					}
				});
			});
		}

		public static IObservable<IList<TSource>> Buffer<TSource, TWindowBoundary>(this IObservable<TSource> source, IObservable<TWindowBoundary> windowBoundaries)
		{
			return Create(delegate(IObserver<IList<TSource>> observer)
			{
				List<TSource> list = (List<TSource>)new List<TSource>();
				object gate = new object();
				return new CompositeDisposable(2)
				{
					source.Subscribe(Observer.Create(delegate(TSource x)
					{
						lock (gate)
						{
							((List<TSource>)list).Add(x);
						}
					}, delegate(Exception ex)
					{
						lock (gate)
						{
							observer.OnError(ex);
						}
					}, delegate
					{
						lock (gate)
						{
							List<TSource> value2 = (List<TSource>)list;
							list = (List<TSource>)new List<TSource>();
							observer.OnNext(value2);
							observer.OnCompleted();
						}
					})),
					windowBoundaries.Subscribe(Observer.Create<TWindowBoundary>(delegate
					{
						List<TSource> list2;
						lock (gate)
						{
							list2 = (List<TSource>)list;
							if (list2.Count != 0)
							{
								list = (List<TSource>)new List<TSource>();
							}
						}
						if (list2.Count != 0)
						{
							observer.OnNext(list2);
						}
					}, delegate(Exception ex)
					{
						lock (gate)
						{
							observer.OnError(ex);
						}
					}, delegate
					{
						lock (gate)
						{
							List<TSource> value = (List<TSource>)list;
							list = (List<TSource>)new List<TSource>();
							observer.OnNext(value);
							observer.OnCompleted();
						}
					}))
				};
			});
		}

		public static IObservable<TR> Pairwise<T, TR>(this IObservable<T> source, Func<T, T, TR> selector)
		{
			return Create(delegate(IObserver<TR> observer)
			{
				T prev = (T)default(T);
				bool isFirst = true;
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					if (isFirst)
					{
						isFirst = false;
						prev = (T)x;
					}
					else
					{
						TR value;
						try
						{
							value = selector((T)prev, x);
							prev = (T)x;
						}
						catch (Exception error)
						{
							observer.OnError(error);
							return;
						}
						observer.OnNext(value);
					}
				};
				IObserver<TR> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<TR> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IObservable<T> Last<T>(this IObservable<T> source)
		{
			return source.LastCore(useDefault: false);
		}

		public static IObservable<T> Last<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.Where(predicate).LastCore(useDefault: false);
		}

		public static IObservable<T> LastOrDefault<T>(this IObservable<T> source)
		{
			return source.LastCore(useDefault: true);
		}

		public static IObservable<T> LastOrDefault<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.Where(predicate).LastCore(useDefault: true);
		}

		private static IObservable<T> LastCore<T>(this IObservable<T> source, bool useDefault)
		{
			return Create(delegate(IObserver<T> observer)
			{
				T value = (T)default(T);
				bool hasValue = false;
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					value = (T)x;
					hasValue = true;
				};
				IObserver<T> observer2 = observer;
				return source2.Subscribe(onNext, observer2.OnError, delegate
				{
					if (hasValue)
					{
						observer.OnNext((T)value);
						observer.OnCompleted();
					}
					else if (useDefault)
					{
						observer.OnNext(default(T));
						observer.OnCompleted();
					}
					else
					{
						observer.OnError(new InvalidOperationException("sequence is empty"));
					}
				});
			});
		}

		public static IObservable<T> First<T>(this IObservable<T> source)
		{
			return source.FirstCore(useDefault: false);
		}

		public static IObservable<T> First<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.Where(predicate).FirstCore(useDefault: false);
		}

		public static IObservable<T> FirstOrDefault<T>(this IObservable<T> source)
		{
			return source.FirstCore(useDefault: true);
		}

		public static IObservable<T> FirstOrDefault<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.Where(predicate).FirstCore(useDefault: true);
		}

		private static IObservable<T> FirstCore<T>(this IObservable<T> source, bool useDefault)
		{
			return Create(delegate(IObserver<T> observer)
			{
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					observer.OnNext(x);
					observer.OnCompleted();
				};
				IObserver<T> observer2 = observer;
				return source2.Subscribe(onNext, observer2.OnError, delegate
				{
					if (useDefault)
					{
						observer.OnNext(default(T));
						observer.OnCompleted();
					}
					else
					{
						observer.OnError(new InvalidOperationException("sequence is empty"));
					}
				});
			});
		}

		public static IObservable<T> Single<T>(this IObservable<T> source)
		{
			return source.SingleCore(useDefault: false);
		}

		public static IObservable<T> Single<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.Where(predicate).SingleCore(useDefault: false);
		}

		public static IObservable<T> SingleOrDefault<T>(this IObservable<T> source)
		{
			return source.SingleCore(useDefault: true);
		}

		public static IObservable<T> SingleOrDefault<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.Where(predicate).SingleCore(useDefault: true);
		}

		private static IObservable<T> SingleCore<T>(this IObservable<T> source, bool useDefault)
		{
			return Create(delegate(IObserver<T> observer)
			{
				T value = (T)default(T);
				bool seenValue = false;
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					if (seenValue)
					{
						observer.OnError(new InvalidOperationException("sequence is not single"));
					}
					value = (T)x;
					seenValue = true;
				};
				IObserver<T> observer2 = observer;
				return source2.Subscribe(onNext, observer2.OnError, delegate
				{
					if (seenValue)
					{
						observer.OnNext((T)value);
						observer.OnCompleted();
					}
					else if (useDefault)
					{
						observer.OnNext(default(T));
						observer.OnCompleted();
					}
					else
					{
						observer.OnError(new InvalidOperationException("sequence is empty"));
					}
				});
			});
		}

		public static IObservable<long> Interval(TimeSpan period)
		{
			return TimerCore(period, period, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<long> Interval(TimeSpan period, IScheduler scheduler)
		{
			return TimerCore(period, period, scheduler);
		}

		public static IObservable<long> Timer(TimeSpan dueTime)
		{
			return TimerCore(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<long> Timer(DateTimeOffset dueTime)
		{
			return TimerCore(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period)
		{
			return TimerCore(dueTime, period, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period)
		{
			return TimerCore(dueTime, period, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<long> Timer(TimeSpan dueTime, IScheduler scheduler)
		{
			return TimerCore(dueTime, scheduler);
		}

		public static IObservable<long> Timer(DateTimeOffset dueTime, IScheduler scheduler)
		{
			return TimerCore(dueTime, scheduler);
		}

		public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
		{
			return TimerCore(dueTime, period, scheduler);
		}

		public static IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period, IScheduler scheduler)
		{
			return TimerCore(dueTime, period, scheduler);
		}

		private static IObservable<long> TimerCore(TimeSpan dueTime, IScheduler scheduler)
		{
			TimeSpan time = Scheduler.Normalize(dueTime);
			return Create((IObserver<long> observer) => Scheduler.Schedule(scheduler, time, delegate
			{
				observer.OnNext(0L);
				observer.OnCompleted();
			}));
		}

		private static IObservable<long> TimerCore(DateTimeOffset dueTime, IScheduler scheduler)
		{
			return Create((IObserver<long> observer) => scheduler.Schedule(dueTime, (Action<Action<DateTimeOffset>>)delegate
			{
				observer.OnNext(0L);
				observer.OnCompleted();
			}));
		}

		private static IObservable<long> TimerCore(TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
		{
			TimeSpan timeD = Scheduler.Normalize(dueTime);
			TimeSpan timeP = Scheduler.Normalize(period);
			return Create(delegate(IObserver<long> observer)
			{
				int count = 0;
				return scheduler.Schedule(timeD, delegate(Action<TimeSpan> self)
				{
					observer.OnNext(count);
					count++;
					self(timeP);
				});
			});
		}

		private static IObservable<long> TimerCore(DateTimeOffset dueTime, TimeSpan period, IScheduler scheduler)
		{
			TimeSpan timeP = Scheduler.Normalize(period);
			return Create(delegate(IObserver<long> observer)
			{
				DateTimeOffset nextTime = dueTime;
				long count = 0L;
				return scheduler.Schedule(nextTime, delegate(Action<DateTimeOffset> self)
				{
					if (timeP > TimeSpan.Zero)
					{
						nextTime += period;
						DateTimeOffset now = scheduler.Now;
						if (nextTime <= now)
						{
							nextTime = now + period;
						}
					}
					observer.OnNext(count);
					count++;
					self(nextTime);
				});
			});
		}

		public static IObservable<Timestamped<TSource>> Timestamp<TSource>(this IObservable<TSource> source)
		{
			return source.Timestamp(Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<Timestamped<TSource>> Timestamp<TSource>(this IObservable<TSource> source, IScheduler scheduler)
		{
			return from x in source
				select new Timestamped<TSource>(x, scheduler.Now);
		}

		public static IObservable<TimeInterval<TSource>> TimeInterval<TSource>(this IObservable<TSource> source)
		{
			return source.TimeInterval(Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<TimeInterval<TSource>> TimeInterval<TSource>(this IObservable<TSource> source, IScheduler scheduler)
		{
			return Defer(delegate
			{
				DateTimeOffset last = scheduler.Now;
				return source.Select(delegate(TSource x)
				{
					DateTimeOffset now = scheduler.Now;
					TimeSpan interval = now.Subtract(last);
					last = now;
					return new TimeInterval<TSource>(x, interval);
				});
			});
		}

		public static IObservable<T> Delay<T>(this IObservable<T> source, TimeSpan dueTime)
		{
			return source.Delay(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<TSource> Delay<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
		{
			return Create(delegate(IObserver<TSource> observer)
			{
				object gate = new object();
				Queue<Timestamped<Notification<TSource>>> q = (Queue<Timestamped<Notification<TSource>>>)new Queue<Timestamped<Notification<TSource>>>();
				bool active = false;
				bool running = false;
				SerialDisposable cancelable = new SerialDisposable();
				Exception exception = null;
				IDisposable disposable = source.Materialize().Timestamp(scheduler).Subscribe(delegate(Timestamped<Notification<TSource>> notification)
				{
					bool flag = false;
					lock (gate)
					{
						if (notification.Value.Kind == NotificationKind.OnError)
						{
							((Queue<Timestamped<Notification<TSource>>>)q).Clear();
							((Queue<Timestamped<Notification<TSource>>>)q).Enqueue(notification);
							exception = notification.Value.Exception;
							flag = !running;
						}
						else
						{
							((Queue<Timestamped<Notification<TSource>>>)q).Enqueue(new Timestamped<Notification<TSource>>(notification.Value, notification.Timestamp.Add(dueTime)));
							flag = !active;
							active = true;
						}
					}
					if (flag)
					{
						if (exception != null)
						{
							observer.OnError(exception);
						}
						else
						{
							SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
							cancelable.Disposable = singleAssignmentDisposable;
							singleAssignmentDisposable.Disposable = scheduler.Schedule(dueTime, delegate(Action<TimeSpan> self)
							{
								lock (gate)
								{
									if (exception != null)
									{
										return;
									}
									running = true;
								}
								Notification<TSource> notification2;
								do
								{
									notification2 = null;
									lock (gate)
									{
										if (((Queue<Timestamped<Notification<TSource>>>)q).Count > 0 && ((Queue<Timestamped<Notification<TSource>>>)q).Peek().Timestamp.CompareTo(scheduler.Now) <= 0)
										{
											notification2 = ((Queue<Timestamped<Notification<TSource>>>)q).Dequeue().Value;
										}
									}
									if (notification2 != null)
									{
										notification2.Accept(observer);
									}
								}
								while (notification2 != null);
								bool flag2 = false;
								TimeSpan obj = TimeSpan.Zero;
								Exception ex = null;
								lock (gate)
								{
									if (((Queue<Timestamped<Notification<TSource>>>)q).Count > 0)
									{
										flag2 = true;
										obj = TimeSpan.FromTicks(Math.Max(0L, ((Queue<Timestamped<Notification<TSource>>>)q).Peek().Timestamp.Subtract(scheduler.Now).Ticks));
									}
									else
									{
										active = false;
									}
									ex = exception;
									running = false;
								}
								if (ex != null)
								{
									observer.OnError(ex);
								}
								else if (flag2)
								{
									self(obj);
								}
							});
						}
					}
				});
				return new CompositeDisposable(disposable, cancelable);
			});
		}

		public static IObservable<T> Sample<T>(this IObservable<T> source, TimeSpan interval)
		{
			return source.Sample(interval, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> Sample<T>(this IObservable<T> source, TimeSpan interval, IScheduler scheduler)
		{
			return Create(delegate(IObserver<T> observer)
			{
				T latestValue = (T)default(T);
				bool isUpdated = false;
				bool isCompleted = false;
				object gate = new object();
				IDisposable item = scheduler.Schedule(interval, delegate(Action<TimeSpan> self)
				{
					lock (gate)
					{
						if (isUpdated)
						{
							T value = (T)latestValue;
							isUpdated = false;
							observer.OnNext(value);
						}
						if (isCompleted)
						{
							observer.OnCompleted();
						}
					}
					self(interval);
				});
				SingleAssignmentDisposable sourceSubscription = new SingleAssignmentDisposable();
				sourceSubscription.Disposable = source.Subscribe(delegate(T x)
				{
					lock (gate)
					{
						latestValue = (T)x;
						isUpdated = true;
					}
				}, delegate(Exception e)
				{
					lock (gate)
					{
						observer.OnError(e);
					}
				}, delegate
				{
					lock (gate)
					{
						isCompleted = true;
						sourceSubscription.Dispose();
					}
				});
				return new CompositeDisposable
				{
					item,
					sourceSubscription
				};
			});
		}

		public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, TimeSpan dueTime)
		{
			return source.Throttle(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
		{
			return new AnonymousObservable<TSource>(delegate(IObserver<TSource> observer)
			{
				object gate = new object();
				TSource value = (TSource)default(TSource);
				bool hasValue = false;
				SerialDisposable cancelable = new SerialDisposable();
				ulong id = 0uL;
				IDisposable disposable = source.Subscribe(delegate(TSource x)
				{
					ulong currentid;
					lock (gate)
					{
						hasValue = true;
						value = (TSource)x;
						id++;
						currentid = id;
					}
					SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
					cancelable.Disposable = singleAssignmentDisposable;
					singleAssignmentDisposable.Disposable = scheduler.Schedule(dueTime, delegate
					{
						lock (gate)
						{
							if (hasValue && id == currentid)
							{
								observer.OnNext((TSource)value);
							}
							hasValue = false;
						}
					});
				}, delegate(Exception exception)
				{
					cancelable.Dispose();
					lock (gate)
					{
						observer.OnError(exception);
						hasValue = false;
						id++;
					}
				}, delegate
				{
					cancelable.Dispose();
					lock (gate)
					{
						if (hasValue)
						{
							observer.OnNext((TSource)value);
						}
						observer.OnCompleted();
						hasValue = false;
						id++;
					}
				});
				return new CompositeDisposable(disposable, cancelable);
			});
		}

		public static IObservable<TSource> ThrottleFirst<TSource>(this IObservable<TSource> source, TimeSpan dueTime)
		{
			return source.ThrottleFirst(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<TSource> ThrottleFirst<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
		{
			return new AnonymousObservable<TSource>(delegate(IObserver<TSource> observer)
			{
				object gate = new object();
				bool open = true;
				SerialDisposable cancelable = new SerialDisposable();
				IDisposable disposable = source.Subscribe(delegate(TSource x)
				{
					lock (gate)
					{
						if (!open)
						{
							return;
						}
						observer.OnNext(x);
						open = false;
					}
					SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
					cancelable.Disposable = singleAssignmentDisposable;
					singleAssignmentDisposable.Disposable = scheduler.Schedule(dueTime, delegate
					{
						lock (gate)
						{
							open = true;
						}
					});
				}, delegate(Exception exception)
				{
					cancelable.Dispose();
					lock (gate)
					{
						observer.OnError(exception);
					}
				}, delegate
				{
					cancelable.Dispose();
					lock (gate)
					{
						observer.OnCompleted();
					}
				});
				return new CompositeDisposable(disposable, cancelable);
			});
		}

		public static IObservable<T> Timeout<T>(this IObservable<T> source, TimeSpan dueTime)
		{
			return source.Timeout(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> Timeout<T>(this IObservable<T> source, TimeSpan dueTime, IScheduler scheduler)
		{
			return Create(delegate(IObserver<T> observer)
			{
				object gate = new object();
				ulong objectId = 0uL;
				bool isTimeout = false;
				Func<ulong, IDisposable> runTimer = (ulong timerId) => scheduler.Schedule(dueTime, delegate
				{
					lock (gate)
					{
						if (objectId == timerId)
						{
							isTimeout = true;
						}
					}
					if (isTimeout)
					{
						observer.OnError(new TimeoutException());
					}
				});
				SerialDisposable timerDisposable = new SerialDisposable();
				timerDisposable.Disposable = runTimer(objectId);
				SingleAssignmentDisposable item = new SingleAssignmentDisposable
				{
					Disposable = source.Subscribe(delegate(T x)
					{
						bool flag3;
						lock (gate)
						{
							flag3 = isTimeout;
							objectId++;
						}
						if (!flag3)
						{
							timerDisposable.Disposable = Disposable.Empty;
							observer.OnNext(x);
							timerDisposable.Disposable = runTimer(objectId);
						}
					}, delegate(Exception ex)
					{
						bool flag2;
						lock (gate)
						{
							flag2 = isTimeout;
							objectId++;
						}
						if (!flag2)
						{
							timerDisposable.Dispose();
							observer.OnError(ex);
						}
					}, delegate
					{
						bool flag;
						lock (gate)
						{
							flag = isTimeout;
							objectId++;
						}
						if (!flag)
						{
							timerDisposable.Dispose();
							observer.OnCompleted();
						}
					})
				};
				return new CompositeDisposable
				{
					timerDisposable,
					item
				};
			});
		}

		public static IObservable<T> Timeout<T>(this IObservable<T> source, DateTimeOffset dueTime)
		{
			return source.Timeout(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> Timeout<T>(this IObservable<T> source, DateTimeOffset dueTime, IScheduler scheduler)
		{
			return Create(delegate(IObserver<T> observer)
			{
				object gate = new object();
				bool isFinished = false;
				SingleAssignmentDisposable sourceSubscription = new SingleAssignmentDisposable();
				IDisposable timerD = scheduler.Schedule(dueTime, (Action)delegate
				{
					lock (gate)
					{
						if (isFinished)
						{
							return;
						}
						isFinished = true;
					}
					sourceSubscription.Dispose();
					observer.OnError(new TimeoutException());
				});
				sourceSubscription.Disposable = source.Subscribe(delegate(T x)
				{
					lock (gate)
					{
						if (!isFinished)
						{
							observer.OnNext(x);
						}
					}
				}, delegate(Exception ex)
				{
					lock (gate)
					{
						if (isFinished)
						{
							return;
						}
						isFinished = true;
					}
					observer.OnError(ex);
				}, delegate
				{
					lock (gate)
					{
						if (!isFinished)
						{
							isFinished = true;
							timerD.Dispose();
						}
						observer.OnCompleted();
					}
				});
				return new CompositeDisposable
				{
					timerD,
					sourceSubscription
				};
			});
		}

		public static IObservable<TR> Select<T, TR>(this IObservable<T> source, Func<T, TR> selector)
		{
			return source.Select((T x, int i) => selector(x));
		}

		public static IObservable<TR> Select<T, TR>(this IObservable<T> source, Func<T, int, TR> selector)
		{
			return Create(delegate(IObserver<TR> observer)
			{
				int index = 0;
				IObservable<T> observable = source;
				Action<T> onNext = delegate(T x)
				{
					TR value;
					try
					{
						value = selector(x, ++index);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					observer.OnNext(value);
				};
				IObserver<TR> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<TR> observer3 = observer;
				return observable.Subscribe(Observer.Create(onNext, onError, observer3.OnCompleted));
			});
		}

		public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.Where((T x, int i) => predicate(x));
		}

		public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, int, bool> predicate)
		{
			return Create(delegate(IObserver<T> observer)
			{
				int index = 0;
				IObservable<T> observable = source;
				Action<T> onNext = delegate(T x)
				{
					bool flag;
					try
					{
						flag = predicate(x, ++index);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					if (flag)
					{
						observer.OnNext(x);
					}
				};
				IObserver<T> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<T> observer3 = observer;
				return observable.Subscribe(Observer.Create(onNext, onError, observer3.OnCompleted));
			});
		}

		public static IObservable<TR> SelectMany<T, TR>(this IObservable<T> source, IObservable<TR> other)
		{
			return source.SelectMany((T _) => other);
		}

		public static IObservable<TR> SelectMany<T, TR>(this IObservable<T> source, Func<T, IObservable<TR>> selector)
		{
			return source.Select(selector).Merge();
		}

		public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector)
		{
			return source.Select(selector).Merge();
		}

		public static IObservable<TR> SelectMany<T, TC, TR>(this IObservable<T> source, Func<T, IObservable<TC>> collectionSelector, Func<T, TC, TR> resultSelector)
		{
			return source.SelectMany((T x) => from y in collectionSelector(x)
				select resultSelector(x, y));
		}

		public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, int, IObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
		{
			return source.SelectMany((TSource x, int i) => collectionSelector(x, i).Select((TCollection y, int i2) => resultSelector(x, i, y, i2)));
		}

		public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
		{
			return new AnonymousObservable<TResult>(delegate(IObserver<TResult> observer)
			{
				IObservable<TSource> source2 = source;
				Action<TSource> onNext = delegate(TSource x)
				{
					IEnumerable<TResult> source3;
					try
					{
						source3 = selector(x);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					using (IEnumerator<TResult> enumerator = source3.AsSafeEnumerable().GetEnumerator())
					{
						bool flag = true;
						while (flag)
						{
							TResult value = default(TResult);
							try
							{
								flag = enumerator.MoveNext();
								if (flag)
								{
									value = enumerator.Current;
								}
							}
							catch (Exception error2)
							{
								observer.OnError(error2);
								return;
							}
							if (flag)
							{
								observer.OnNext(value);
							}
						}
					}
				};
				IObserver<TResult> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<TResult> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
		{
			return Create(delegate(IObserver<TResult> observer)
			{
				int index = 0;
				IObservable<TSource> source2 = source;
				Action<TSource> onNext = delegate(TSource x)
				{
					IEnumerable<TResult> source3;
					try
					{
						source3 = selector(x, index = checked(index + 1));
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					using (IEnumerator<TResult> enumerator = source3.AsSafeEnumerable().GetEnumerator())
					{
						bool flag = true;
						while (flag)
						{
							TResult value = default(TResult);
							try
							{
								flag = enumerator.MoveNext();
								if (flag)
								{
									value = enumerator.Current;
								}
							}
							catch (Exception error2)
							{
								observer.OnError(error2);
								return;
							}
							if (flag)
							{
								observer.OnNext(value);
							}
						}
					}
				};
				IObserver<TResult> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<TResult> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			return new AnonymousObservable<TResult>(delegate(IObserver<TResult> observer)
			{
				IObservable<TSource> source2 = source;
				Action<TSource> onNext = delegate(TSource x)
				{
					IEnumerable<TCollection> source3;
					try
					{
						source3 = collectionSelector(x);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					using (IEnumerator<TCollection> enumerator = source3.AsSafeEnumerable().GetEnumerator())
					{
						bool flag = true;
						while (flag)
						{
							TResult value = default(TResult);
							try
							{
								flag = enumerator.MoveNext();
								if (flag)
								{
									value = resultSelector(x, enumerator.Current);
								}
							}
							catch (Exception error2)
							{
								observer.OnError(error2);
								return;
							}
							if (flag)
							{
								observer.OnNext(value);
							}
						}
					}
				};
				IObserver<TResult> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<TResult> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
		{
			return Create(delegate(IObserver<TResult> observer)
			{
				int index = 0;
				IObservable<TSource> source2 = source;
				Action<TSource> onNext = delegate(TSource x)
				{
					IEnumerable<TCollection> source3;
					try
					{
						source3 = collectionSelector(x, index = checked(index + 1));
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					using (IEnumerator<TCollection> enumerator = source3.AsSafeEnumerable().GetEnumerator())
					{
						int num = 0;
						bool flag = true;
						while (flag)
						{
							TResult value = default(TResult);
							try
							{
								flag = enumerator.MoveNext();
								if (flag)
								{
									value = resultSelector(x, index, enumerator.Current, num++);
								}
							}
							catch (Exception error2)
							{
								observer.OnError(error2);
								return;
							}
							if (flag)
							{
								observer.OnNext(value);
							}
						}
					}
				};
				IObserver<TResult> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<TResult> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IObservable<T[]> ToArray<T>(this IObservable<T> source)
		{
			return Create(delegate(IObserver<T[]> observer)
			{
				List<T> list = (List<T>)new List<T>();
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					((List<T>)list).Add(x);
				};
				IObserver<T[]> observer2 = observer;
				return source2.Subscribe(onNext, observer2.OnError, delegate
				{
					observer.OnNext(((List<T>)list).ToArray());
					observer.OnCompleted();
				});
			});
		}

		public static IObservable<T> Do<T>(this IObservable<T> source, IObserver<T> observer)
		{
			return source.Do(observer.OnNext, observer.OnError, observer.OnCompleted);
		}

		public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext)
		{
			return source.Do(onNext, Stubs.Throw, Stubs.Nop);
		}

		public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError)
		{
			return source.Do(onNext, onError, Stubs.Nop);
		}

		public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext, Action onCompleted)
		{
			return source.Do(onNext, Stubs.Throw, onCompleted);
		}

		public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted)
		{
			return Create((IObserver<T> observer) => source.Subscribe(delegate(T x)
			{
				try
				{
					if (onNext != new Action<T>(Stubs.Ignore<T>))
					{
						onNext(x);
					}
				}
				catch (Exception error3)
				{
					observer.OnError(error3);
					return;
				}
				observer.OnNext(x);
			}, delegate(Exception ex)
			{
				try
				{
					onError(ex);
				}
				catch (Exception error2)
				{
					observer.OnError(error2);
					return;
				}
				observer.OnError(ex);
			}, delegate
			{
				try
				{
					onCompleted();
				}
				catch (Exception error)
				{
					observer.OnError(error);
					return;
				}
				observer.OnCompleted();
			}));
		}

		public static IObservable<Notification<T>> Materialize<T>(this IObservable<T> source)
		{
			return Create((IObserver<Notification<T>> observer) => source.Subscribe(delegate(T x)
			{
				observer.OnNext(Notification.CreateOnNext(x));
			}, delegate(Exception x)
			{
				observer.OnNext(Notification.CreateOnError<T>(x));
				observer.OnCompleted();
			}, delegate
			{
				observer.OnNext(Notification.CreateOnCompleted<T>());
				observer.OnCompleted();
			}));
		}

		public static IObservable<T> Dematerialize<T>(this IObservable<Notification<T>> source)
		{
			return Create(delegate(IObserver<T> observer)
			{
				IObservable<Notification<T>> source2 = source;
				Action<Notification<T>> onNext = delegate(Notification<T> x)
				{
					if (x.Kind == NotificationKind.OnNext)
					{
						observer.OnNext(x.Value);
					}
					else if (x.Kind == NotificationKind.OnError)
					{
						observer.OnError(x.Exception);
					}
					else if (x.Kind == NotificationKind.OnCompleted)
					{
						observer.OnCompleted();
					}
				};
				IObserver<T> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<T> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IObservable<T> DefaultIfEmpty<T>(this IObservable<T> source)
		{
			return source.DefaultIfEmpty(default(T));
		}

		public static IObservable<T> DefaultIfEmpty<T>(this IObservable<T> source, T defaultValue)
		{
			return Create(delegate(IObserver<T> observer)
			{
				bool hasValue = false;
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					hasValue = true;
					observer.OnNext(x);
				};
				IObserver<T> observer2 = observer;
				return source2.Subscribe(onNext, observer2.OnError, delegate
				{
					if (!hasValue)
					{
						observer.OnNext(defaultValue);
					}
					observer.OnCompleted();
				});
			});
		}

		public static IObservable<TSource> Distinct<TSource>(this IObservable<TSource> source)
		{
			return source.Distinct(null);
		}

		public static IObservable<TSource> Distinct<TSource>(this IObservable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			return Create(delegate(IObserver<TSource> observer)
			{
				HashSet<TSource> hashSet = (HashSet<TSource>)((comparer != null) ? new HashSet<TSource>(comparer) : new HashSet<TSource>());
				IObservable<TSource> source2 = source;
				Action<TSource> onNext = delegate(TSource x)
				{
					bool flag;
					try
					{
						flag = ((HashSet<TSource>)hashSet).Add(x);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					if (flag)
					{
						observer.OnNext(x);
					}
				};
				IObserver<TSource> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<TSource> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IObservable<TSource> Distinct<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.Distinct(keySelector, null);
		}

		public static IObservable<TSource> Distinct<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			return Create(delegate(IObserver<TSource> observer)
			{
				HashSet<TKey> hashSet = (HashSet<TKey>)((comparer != null) ? new HashSet<TKey>(comparer) : new HashSet<TKey>());
				IObservable<TSource> source2 = source;
				Action<TSource> onNext = delegate(TSource x)
				{
					bool flag;
					try
					{
						TKey item = keySelector(x);
						flag = ((HashSet<TKey>)hashSet).Add(item);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					if (flag)
					{
						observer.OnNext(x);
					}
				};
				IObserver<TSource> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<TSource> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> source)
		{
			return source.DistinctUntilChanged(null);
		}

		public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> source, IEqualityComparer<T> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return Create(delegate(IObserver<T> observer)
			{
				bool isFirst = true;
				T prevKey = (T)default(T);
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					T val;
					try
					{
						val = x;
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					bool flag = false;
					if (isFirst)
					{
						isFirst = false;
					}
					else
					{
						try
						{
							flag = ((comparer != null) ? comparer.Equals(val, (T)prevKey) : (val?.Equals((T)prevKey) ?? (prevKey == null)));
						}
						catch (Exception error2)
						{
							observer.OnError(error2);
							return;
						}
					}
					if (!flag)
					{
						prevKey = (T)val;
						observer.OnNext(x);
					}
				};
				IObserver<T> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<T> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IObservable<T> DistinctUntilChanged<T, TKey>(this IObservable<T> source, Func<T, TKey> keySelector)
		{
			return source.DistinctUntilChanged(keySelector, null);
		}

		public static IObservable<T> DistinctUntilChanged<T, TKey>(this IObservable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return Create(delegate(IObserver<T> observer)
			{
				bool isFirst = true;
				TKey prevKey = (TKey)default(TKey);
				IObservable<T> source2 = source;
				Action<T> onNext = delegate(T x)
				{
					TKey val;
					try
					{
						val = keySelector(x);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					bool flag = false;
					if (isFirst)
					{
						isFirst = false;
					}
					else
					{
						try
						{
							flag = ((comparer != null) ? comparer.Equals(val, (TKey)prevKey) : val.Equals((TKey)prevKey));
						}
						catch (Exception error2)
						{
							observer.OnError(error2);
							return;
						}
					}
					if (!flag)
					{
						prevKey = (TKey)val;
						observer.OnNext(x);
					}
				};
				IObserver<T> observer2 = observer;
				Action<Exception> onError = observer2.OnError;
				IObserver<T> observer3 = observer;
				return source2.Subscribe(onNext, onError, observer3.OnCompleted);
			});
		}

		public static IObservable<T> IgnoreElements<T>(this IObservable<T> source)
		{
			return Create((IObserver<T> observer) => source.Subscribe(Stubs.Ignore<T>, observer.OnError, observer.OnCompleted));
		}

		public static IObservable<Unit> FromCoroutine(Func<IEnumerator> coroutine, bool publishEveryYield = false)
		{
			return FromCoroutine((IObserver<Unit> observer, CancellationToken cancellationToken) => WrapEnumerator(coroutine(), observer, cancellationToken, publishEveryYield));
		}

		private static IEnumerator WrapEnumerator(IEnumerator enumerator, IObserver<Unit> observer, CancellationToken cancellationToken, bool publishEveryYield)
		{
			bool raisedError = false;
			bool hasNext;
			do
			{
				try
				{
					hasNext = enumerator.MoveNext();
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					try
					{
						observer.OnError(ex);
					}
					finally
					{
                        throw new NotImplementedException("‚È‚É‚±‚ê");
                        // base._003C_003E__Finally0();
					}
					yield break;
				}
				if (hasNext && publishEveryYield)
				{
					try
					{
						observer.OnNext(Unit.Default);
					}
					catch
					{
						(enumerator as IDisposable)?.Dispose();
						throw;
					}
				}
				if (hasNext)
				{
					yield return enumerator.Current;
				}
			}
			while (hasNext && !cancellationToken.IsCancellationRequested);
			try
			{
				if (!raisedError && !cancellationToken.IsCancellationRequested)
				{
					observer.OnNext(Unit.Default);
					observer.OnCompleted();
				}
			}
			finally
			{
                throw new NotImplementedException("‚È‚É‚±‚ê");
                // base._003C_003E__Finally1();
			}
		}

		public static IObservable<T> FromCoroutineValue<T>(Func<IEnumerator> coroutine, bool nullAsNextUpdate = true)
		{
			return FromCoroutine((IObserver<T> observer, CancellationToken cancellationToken) => WrapEnumeratorYieldValue(coroutine(), observer, cancellationToken, nullAsNextUpdate));
		}

		private static IEnumerator WrapEnumeratorYieldValue<T>(IEnumerator enumerator, IObserver<T> observer, CancellationToken cancellationToken, bool nullAsNextUpdate)
		{
			object current = null;
			bool raisedError = false;
			bool hasNext;
			do
			{
				try
				{
					hasNext = enumerator.MoveNext();
					if (hasNext)
					{
						current = enumerator.Current;
					}
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					try
					{
						observer.OnError(ex);
					}
					finally
					{
                        throw new NotImplementedException("‚È‚É‚±‚ê");
                        // base._003C_003E__Finally0();
					}
					yield break;
				}
				if (hasNext)
				{
					if (current != null && YieldInstructionTypes.Contains(current.GetType()))
					{
						yield return current;
					}
					else if (current == null && nullAsNextUpdate)
					{
						yield return null;
					}
					else
					{
						try
						{
							observer.OnNext((T)current);
						}
						catch
						{
							(enumerator as IDisposable)?.Dispose();
							throw;
						}
					}
				}
			}
			while (hasNext && !cancellationToken.IsCancellationRequested);
			try
			{
				if (!raisedError && !cancellationToken.IsCancellationRequested)
				{
					observer.OnCompleted();
				}
			}
			finally
			{
                throw new NotImplementedException("‚È‚É‚±‚ê");
                // base._003C_003E__Finally1();
			}
		}

		public static IObservable<T> FromCoroutine<T>(Func<IObserver<T>, IEnumerator> coroutine)
		{
			return FromCoroutine((IObserver<T> observer, CancellationToken _) => coroutine(observer));
		}

		public static IObservable<T> FromCoroutine<T>(Func<IObserver<T>, CancellationToken, IEnumerator> coroutine)
		{
			return Create(delegate(IObserver<T> observer)
			{
				BooleanDisposable booleanDisposable = new BooleanDisposable();
				MainThreadDispatcher.SendStartCoroutine(coroutine(observer, new CancellationToken(booleanDisposable)));
				return booleanDisposable;
			});
		}

		public static IObservable<Unit> SelectMany<T>(this IObservable<T> source, IEnumerator coroutine, bool publishEveryYield = false)
		{
			return source.SelectMany(FromCoroutine(() => coroutine, publishEveryYield));
		}

		public static IObservable<Unit> SelectMany<T>(this IObservable<T> source, Func<IEnumerator> selector, bool publishEveryYield = false)
		{
			return source.SelectMany(FromCoroutine(() => selector(), publishEveryYield));
		}

		public static IObservable<Unit> SelectMany<T>(this IObservable<T> source, Func<T, IEnumerator> selector)
		{
			return source.SelectMany((T x) => FromCoroutine(() => selector(x)));
		}

		public static IObservable<Unit> ToObservable(this IEnumerator coroutine, bool publishEveryYield = false)
		{
			return FromCoroutine((IObserver<Unit> observer, CancellationToken cancellationToken) => WrapEnumerator(coroutine, observer, cancellationToken, publishEveryYield));
		}

		public static IObservable<long> EveryUpdate()
		{
			return FromCoroutine((IObserver<long> observer, CancellationToken cancellationToken) => EveryUpdateCore(observer, cancellationToken));
		}

		private static IEnumerator EveryUpdateCore(IObserver<long> observer, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				yield break;
			}
			long count = 0L;
			while (true)
			{
				yield return null;
				if (cancellationToken.IsCancellationRequested)
				{
					break;
				}
				long value;
				count = (value = count) + 1;
				observer.OnNext(value);
			}
		}

		public static IObservable<long> EveryFixedUpdate()
		{
			return FromCoroutine((IObserver<long> observer, CancellationToken cancellationToken) => EveryFixedUpdateCore(observer, cancellationToken));
		}

		private static IEnumerator EveryFixedUpdateCore(IObserver<long> observer, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				yield break;
			}
			long count = 0L;
			while (true)
			{
				yield return new WaitForFixedUpdate();
				if (cancellationToken.IsCancellationRequested)
				{
					break;
				}
				long value;
				count = (value = count) + 1;
				observer.OnNext(value);
			}
		}

		public static IObservable<long> EveryEndOfFrame()
		{
			return FromCoroutine((IObserver<long> observer, CancellationToken cancellationToken) => EveryEndOfFrameCore(observer, cancellationToken));
		}

		private static IEnumerator EveryEndOfFrameCore(IObserver<long> observer, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				yield break;
			}
			long count = 0L;
			while (true)
			{
				yield return new WaitForFixedUpdate();
				if (cancellationToken.IsCancellationRequested)
				{
					break;
				}
				long value;
				count = (value = count) + 1;
				observer.OnNext(value);
			}
		}

		public static IObservable<Unit> NextFrame(FrameCountType frameCountType = FrameCountType.Update)
		{
			return FromCoroutine((IObserver<Unit> observer, CancellationToken cancellation) => NextFrameCore(observer, frameCountType, cancellation));
		}

		private static IEnumerator NextFrameCore(IObserver<Unit> observer, FrameCountType frameCountType, CancellationToken cancellation)
		{
			yield return frameCountType.GetYieldInstruction();
			if (!cancellation.IsCancellationRequested)
			{
				observer.OnNext(Unit.Default);
				observer.OnCompleted();
			}
		}

		public static IObservable<long> IntervalFrame(int intervalFrameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return TimerFrame(intervalFrameCount, intervalFrameCount, frameCountType);
		}

		public static IObservable<long> TimerFrame(int dueTimeFrameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return FromCoroutine((IObserver<long> observer, CancellationToken cancellation) => TimerFrameCore(observer, dueTimeFrameCount, frameCountType, cancellation));
		}

		public static IObservable<long> TimerFrame(int dueTimeFrameCount, int periodFrameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return FromCoroutine((IObserver<long> observer, CancellationToken cancellation) => TimerFrameCore(observer, dueTimeFrameCount, periodFrameCount, frameCountType, cancellation));
		}

		private static IEnumerator TimerFrameCore(IObserver<long> observer, int dueTimeFrameCount, FrameCountType frameCountType, CancellationToken cancel)
		{
			if (dueTimeFrameCount <= 0)
			{
				dueTimeFrameCount = 0;
			}
			int currentFrame = 0;
			while (true)
			{
				if (!cancel.IsCancellationRequested)
				{
					int num;
					currentFrame = (num = currentFrame) + 1;
					if (num == dueTimeFrameCount)
					{
						break;
					}
					yield return frameCountType.GetYieldInstruction();
					continue;
				}
				yield break;
			}
			observer.OnNext(0L);
			observer.OnCompleted();
		}

		private static IEnumerator TimerFrameCore(IObserver<long> observer, int dueTimeFrameCount, int periodFrameCount, FrameCountType frameCountType, CancellationToken cancel)
		{
			if (dueTimeFrameCount <= 0)
			{
				dueTimeFrameCount = 0;
			}
			if (periodFrameCount <= 0)
			{
				periodFrameCount = 1;
			}
			long sendCount = 0L;
			int currentFrame = 0;
			while (!cancel.IsCancellationRequested)
			{
				int num;
				currentFrame = (num = currentFrame) + 1;
				if (num == dueTimeFrameCount)
				{
					long value;
					sendCount = (value = sendCount) + 1;
					observer.OnNext(value);
					currentFrame = -1;
					break;
				}
				yield return frameCountType.GetYieldInstruction();
			}
			while (!cancel.IsCancellationRequested)
			{
				int num;
				currentFrame = (num = currentFrame + 1);
				if (num == periodFrameCount)
				{
					long value;
					sendCount = (value = sendCount) + 1;
					observer.OnNext(value);
					currentFrame = 0;
				}
				yield return frameCountType.GetYieldInstruction();
			}
		}

		public static IObservable<T> DelayFrame<T>(this IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			if (frameCount < 0)
			{
				throw new ArgumentOutOfRangeException("frameCount");
			}
			return Create(delegate(IObserver<T> observer)
			{
				BooleanDisposable cancel = new BooleanDisposable();
				source.Materialize().Subscribe(delegate(Notification<T> x)
				{
					if (x.Kind == NotificationKind.OnError)
					{
						observer.OnError(x.Exception);
						cancel.Dispose();
					}
					else
					{
						MainThreadDispatcher.StartCoroutine(DelayFrameCore(delegate
						{
							x.Accept(observer);
						}, frameCount, frameCountType, cancel));
					}
				});
				return cancel;
			});
		}

		private static IEnumerator DelayFrameCore(Action onNext, int frameCount, FrameCountType frameCountType, ICancelable cancel)
		{
			while (!cancel.IsDisposed)
			{
				int num;
				frameCount = (num = frameCount) - 1;
				if (num == 0)
				{
					break;
				}
				yield return frameCountType.GetYieldInstruction();
			}
			if (!cancel.IsDisposed)
			{
				onNext();
			}
		}

		public static IObservable<T> SampleFrame<T>(this IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return Create(delegate(IObserver<T> observer)
			{
				T latestValue = (T)default(T);
				bool isUpdated = false;
				bool isCompleted = false;
				object gate = new object();
				SingleAssignmentDisposable scheduling = new SingleAssignmentDisposable();
				scheduling.Disposable = IntervalFrame(frameCount, frameCountType).Subscribe(delegate
				{
					lock (gate)
					{
						if (isUpdated)
						{
							T value = (T)latestValue;
							isUpdated = false;
							try
							{
								observer.OnNext(value);
							}
							catch
							{
								scheduling.Dispose();
							}
						}
						if (isCompleted)
						{
							observer.OnCompleted();
							scheduling.Dispose();
						}
					}
				});
				SingleAssignmentDisposable sourceSubscription = new SingleAssignmentDisposable();
				sourceSubscription.Disposable = source.Subscribe(delegate(T x)
				{
					lock (gate)
					{
						latestValue = (T)x;
						isUpdated = true;
					}
				}, delegate(Exception e)
				{
					lock (gate)
					{
						observer.OnError(e);
						scheduling.Dispose();
					}
				}, delegate
				{
					lock (gate)
					{
						isCompleted = true;
						sourceSubscription.Dispose();
					}
				});
				return new CompositeDisposable
				{
					scheduling,
					sourceSubscription
				};
			});
		}

		public static IObservable<TSource> ThrottleFrame<TSource>(this IObservable<TSource> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return new AnonymousObservable<TSource>(delegate(IObserver<TSource> observer)
			{
				object gate = new object();
				TSource value = (TSource)default(TSource);
				bool hasValue = false;
				SerialDisposable cancelable = new SerialDisposable();
				ulong id = 0uL;
				IDisposable disposable = source.Subscribe(delegate(TSource x)
				{
					ulong currentid;
					lock (gate)
					{
						hasValue = true;
						value = (TSource)x;
						id++;
						currentid = id;
					}
					SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
					cancelable.Disposable = singleAssignmentDisposable;
					singleAssignmentDisposable.Disposable = TimerFrame(frameCount, frameCountType).Subscribe(delegate
					{
						lock (gate)
						{
							if (hasValue && id == currentid)
							{
								observer.OnNext((TSource)value);
							}
							hasValue = false;
						}
					});
				}, delegate(Exception exception)
				{
					cancelable.Dispose();
					lock (gate)
					{
						observer.OnError(exception);
						hasValue = false;
						id++;
					}
				}, delegate
				{
					cancelable.Dispose();
					lock (gate)
					{
						if (hasValue)
						{
							observer.OnNext((TSource)value);
						}
						observer.OnCompleted();
						hasValue = false;
						id++;
					}
				});
				return new CompositeDisposable(disposable, cancelable);
			});
		}

		public static IObservable<TSource> ThrottleFirstFrame<TSource>(this IObservable<TSource> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return new AnonymousObservable<TSource>(delegate(IObserver<TSource> observer)
			{
				object gate = new object();
				bool open = true;
				SerialDisposable cancelable = new SerialDisposable();
				IDisposable disposable = source.Subscribe(delegate(TSource x)
				{
					lock (gate)
					{
						if (!open)
						{
							return;
						}
						observer.OnNext(x);
						open = false;
					}
					SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
					cancelable.Disposable = singleAssignmentDisposable;
					singleAssignmentDisposable.Disposable = TimerFrame(frameCount, frameCountType).Subscribe(delegate
					{
						lock (gate)
						{
							open = true;
						}
					});
				}, delegate(Exception exception)
				{
					cancelable.Dispose();
					lock (gate)
					{
						observer.OnError(exception);
					}
				}, delegate
				{
					cancelable.Dispose();
					lock (gate)
					{
						observer.OnCompleted();
					}
				});
				return new CompositeDisposable(disposable, cancelable);
			});
		}

		public static IObservable<T> TimeoutFrame<T>(this IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return Create(delegate(IObserver<T> observer)
			{
				object gate = new object();
				ulong objectId = 0uL;
				bool isTimeout = false;
				Func<ulong, IDisposable> runTimer = (ulong timerId) => TimerFrame(frameCount, frameCountType).Subscribe(delegate
				{
					lock (gate)
					{
						if (objectId == timerId)
						{
							isTimeout = true;
						}
					}
					if (isTimeout)
					{
						observer.OnError(new TimeoutException());
					}
				});
				SerialDisposable timerDisposable = new SerialDisposable();
				timerDisposable.Disposable = runTimer(objectId);
				SingleAssignmentDisposable item = new SingleAssignmentDisposable
				{
					Disposable = source.Subscribe(delegate(T x)
					{
						bool flag3;
						lock (gate)
						{
							flag3 = isTimeout;
							objectId++;
						}
						if (!flag3)
						{
							timerDisposable.Disposable = Disposable.Empty;
							observer.OnNext(x);
							timerDisposable.Disposable = runTimer(objectId);
						}
					}, delegate(Exception ex)
					{
						bool flag2;
						lock (gate)
						{
							flag2 = isTimeout;
							objectId++;
						}
						if (!flag2)
						{
							timerDisposable.Dispose();
							observer.OnError(ex);
						}
					}, delegate
					{
						bool flag;
						lock (gate)
						{
							flag = isTimeout;
							objectId++;
						}
						if (!flag)
						{
							timerDisposable.Dispose();
							observer.OnCompleted();
						}
					})
				};
				return new CompositeDisposable
				{
					timerDisposable,
					item
				};
			});
		}

		public static IObservable<T> DelayFrameSubscription<T>(this IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return Create(delegate(IObserver<T> observer)
			{
				MultipleAssignmentDisposable d = new MultipleAssignmentDisposable();
				d.Disposable = TimerFrame(frameCount, frameCountType).Subscribe(delegate
				{
					d.Disposable = source.Subscribe(observer);
				});
				return d;
			});
		}

		public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source, CancellationToken cancel = null)
		{
			return source.ToAwaitableEnumerator(Stubs.Ignore<T>, Stubs.Throw, cancel);
		}

		public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source, Action<T> onResult, CancellationToken cancel = null)
		{
			return source.ToAwaitableEnumerator(onResult, Stubs.Throw, cancel);
		}

		public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source, Action<Exception> onError, CancellationToken cancel = null)
		{
			return source.ToAwaitableEnumerator(Stubs.Ignore<T>, onError, cancel);
		}

		public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source, Action<T> onResult, Action<Exception> onError, CancellationToken cancel = null)
		{
			if (cancel == null)
			{
				cancel = CancellationToken.Empty;
			}
			bool running = true;
			IDisposable subscription = source.LastOrDefault().ObserveOnMainThread().SubscribeOnMainThread()
				.Finally(delegate
				{
					running = false;
				})
				.Subscribe(onResult, onError, Stubs.Nop);
			while (running && !cancel.IsCancellationRequested)
			{
				yield return null;
			}
			if (cancel.IsCancellationRequested)
			{
				subscription.Dispose();
			}
		}

		public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, CancellationToken cancel = null)
		{
			return source.StartAsCoroutine(Stubs.Ignore<T>, Stubs.Throw, cancel);
		}

		public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, Action<T> onResult, CancellationToken cancel = null)
		{
			return source.StartAsCoroutine(onResult, Stubs.Throw, cancel);
		}

		public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, Action<Exception> onError, CancellationToken cancel = null)
		{
			return source.StartAsCoroutine(Stubs.Ignore<T>, onError, cancel);
		}

		public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, Action<T> onResult, Action<Exception> onError, CancellationToken cancel = null)
		{
			return MainThreadDispatcher.StartCoroutine(source.ToAwaitableEnumerator(onResult, onError, cancel));
		}

		public static IObservable<T> ObserveOnMainThread<T>(this IObservable<T> source)
		{
			return source.ObserveOn(Scheduler.MainThread);
		}

		public static IObservable<T> SubscribeOnMainThread<T>(this IObservable<T> source)
		{
			return source.SubscribeOn(Scheduler.MainThread);
		}

		public static IObservable<bool> EveryApplicationPause()
		{
			return MainThreadDispatcher.OnApplicationPauseAsObservable().AsObservable();
		}

		public static IObservable<bool> EveryApplicationFocus()
		{
			return MainThreadDispatcher.OnApplicationFocusAsObservable().AsObservable();
		}

		public static IObservable<Unit> OnceApplicationQuit()
		{
			return MainThreadDispatcher.OnApplicationQuitAsObservable().Take(1);
		}

		public static IObservable<T> TakeUntilDestroy<T>(this IObservable<T> source, Component target)
		{
			return source.TakeUntil(target.OnDestroyAsObservable());
		}

		public static IObservable<T> TakeUntilDestroy<T>(this IObservable<T> source, GameObject target)
		{
			return source.TakeUntil(target.OnDestroyAsObservable());
		}

		public static IObservable<T> TakeUntilDisable<T>(this IObservable<T> source, Component target)
		{
			return source.TakeUntil(target.OnDisableAsObservable());
		}

		public static IObservable<T> TakeUntilDisable<T>(this IObservable<T> source, GameObject target)
		{
			return source.TakeUntil(target.OnDisableAsObservable());
		}

		public static IObservable<T> RepeatUntilDestroy<T>(this IObservable<T> source, GameObject target)
		{
			return RepeatInfinite(source).RepeatUntilCore(target.OnDestroyAsObservable(), target);
		}

		public static IObservable<T> RepeatUntilDestroy<T>(this IObservable<T> source, Component target)
		{
			return RepeatInfinite(source).RepeatUntilCore(target.OnDestroyAsObservable(), (!(target != null)) ? null : target.gameObject);
		}

		public static IObservable<T> RepeatUntilDisable<T>(this IObservable<T> source, GameObject target)
		{
			return RepeatInfinite(source).RepeatUntilCore(target.OnDisableAsObservable(), target);
		}

		public static IObservable<T> RepeatUntilDisable<T>(this IObservable<T> source, Component target)
		{
			return RepeatInfinite(source).RepeatUntilCore(target.OnDisableAsObservable(), (!(target != null)) ? null : target.gameObject);
		}

		private static IObservable<T> RepeatUntilCore<T>(this IEnumerable<IObservable<T>> sources, IObservable<Unit> trigger, GameObject lifeTimeChecker)
		{
			return Create(delegate(IObserver<T> observer)
			{
				bool isFirstSubscribe = true;
				bool isDisposed = false;
				bool isStopped = false;
				IEnumerator<IObservable<T>> e = (IEnumerator<IObservable<T>>)sources.AsSafeEnumerable().GetEnumerator();
				SerialDisposable subscription = new SerialDisposable();
				SingleAssignmentDisposable schedule = new SingleAssignmentDisposable();
				object gate = new object();
				IObservable<Unit> source = trigger;
				Action<Unit> onNext = delegate
				{
					lock (gate)
					{
						isStopped = true;
						e.Dispose();
						subscription.Dispose();
						schedule.Dispose();
						observer.OnCompleted();
					}
				};
				IObserver<T> observer2 = observer;
				IDisposable stopper = source.Subscribe(onNext, observer2.OnError);
				schedule.Disposable = Scheduler.CurrentThread.Schedule(delegate(Action self)
				{
					lock (gate)
					{
						if (!isDisposed && !isStopped)
						{
							bool flag = false;
							Exception ex = null;
							try
							{
								flag = e.MoveNext();
								if (flag)
								{
									IObservable<T> current = ((IEnumerator<IObservable<T>>)e).Current;
									if (current == null)
									{
										throw new InvalidOperationException("sequence is null.");
									}
								}
								else
								{
									e.Dispose();
								}
							}
							catch (Exception ex2)
							{
								ex = ex2;
								e.Dispose();
							}
							if (ex != null)
							{
								stopper.Dispose();
								observer.OnError(ex);
							}
							else if (!flag)
							{
								stopper.Dispose();
								observer.OnCompleted();
							}
							else
							{
								IObservable<T> current2 = ((IEnumerator<IObservable<T>>)e).Current;
								SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
								subscription.Disposable = singleAssignmentDisposable;
								IObserver<T> observer3 = observer;
								Action<T> onNext2 = observer3.OnNext;
								IObserver<T> observer4 = observer;
								IObserver<T> observer5 = Observer.Create(onNext2, observer4.OnError, self);
								if (isFirstSubscribe)
								{
									isFirstSubscribe = false;
									singleAssignmentDisposable.Disposable = current2.Subscribe(observer5);
								}
								else
								{
									MainThreadDispatcher.SendStartCoroutine(SubscribeAfterEndOfFrame(singleAssignmentDisposable, current2, observer5, lifeTimeChecker));
								}
							}
						}
					}
				});
				return new CompositeDisposable(schedule, subscription, stopper, Disposable.Create(delegate
				{
					lock (gate)
					{
						isDisposed = true;
						e.Dispose();
					}
				}));
			});
		}

		private static IEnumerator SubscribeAfterEndOfFrame<T>(SingleAssignmentDisposable d, IObservable<T> source, IObserver<T> observer, GameObject lifeTimeChecker)
		{
			yield return new WaitForEndOfFrame();
			if (!d.IsDisposed && lifeTimeChecker != null)
			{
				d.Disposable = source.Subscribe(observer);
			}
		}
	}
}
