using System;
using System.Collections;
using System.Collections.Generic;

public static class EnumerableExtensions
{
	public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
	{
		foreach (T item in enumerable)
		{
			action(item);
		}
	}

	public static void ForEach(this IEnumerable enumerable, Action<object> action)
	{
		foreach (object item in enumerable)
		{
			action(item);
		}
	}
}
