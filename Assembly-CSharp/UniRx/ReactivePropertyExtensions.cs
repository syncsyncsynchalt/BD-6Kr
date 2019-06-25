using System.Collections.Generic;

namespace UniRx
{
	public static class ReactivePropertyExtensions
	{
		public static ReactiveProperty<T> ToReactiveProperty<T>(this IObservable<T> source)
		{
			return new ReactiveProperty<T>(source);
		}

		public static ReactiveProperty<T> ToReactiveProperty<T>(this IObservable<T> source, T initialValue)
		{
			return new ReactiveProperty<T>(source, initialValue);
		}

		public static ReadOnlyReactiveProperty<T> ToReadOnlyReactiveProperty<T>(this IObservable<T> source)
		{
			return new ReadOnlyReactiveProperty<T>(source);
		}

		public static ReadOnlyReactiveProperty<T> ToReadOnlyReactiveProperty<T>(this IObservable<T> source, T initialValue)
		{
			return new ReadOnlyReactiveProperty<T>(source, initialValue);
		}

		public static IObservable<bool> CombineLatestValuesAreAllTrue(this IEnumerable<IObservable<bool>> sources)
		{
			return sources.CombineLatest().Select(delegate(IList<bool> xs)
			{
				foreach (bool x in xs)
				{
					if (!x)
					{
						return false;
					}
				}
				return true;
			});
		}

		public static IObservable<bool> CombineLatestValuesAreAllFalse(this IEnumerable<IObservable<bool>> sources)
		{
			return sources.CombineLatest().Select(delegate(IList<bool> xs)
			{
				foreach (bool x in xs)
				{
					if (x)
					{
						return false;
					}
				}
				return true;
			});
		}
	}
}
