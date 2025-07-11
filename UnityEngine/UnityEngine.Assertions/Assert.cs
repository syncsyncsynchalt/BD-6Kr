#define UNITY_ASSERTIONS
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Assertions.Comparers;

namespace UnityEngine.Assertions;

[DebuggerStepThrough]
public static class Assert
{
	internal const string UNITY_ASSERTIONS = "UNITY_ASSERTIONS";

	public static bool raiseExceptions;

	private static readonly Dictionary<Type, object> m_ComparersCache = new Dictionary<Type, object>();

	private static void Fail(string message, string userMessage)
	{
		if (Debugger.IsAttached)
		{
			throw new AssertionException(message, userMessage);
		}
		if (raiseExceptions)
		{
			throw new AssertionException(message, userMessage);
		}
		if (message == null)
		{
			message = "Assertion has failed\n";
		}
		if (userMessage != null)
		{
			message = userMessage + '\n' + message;
		}
		Debug.LogAssertion(message);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void IsTrue(bool condition)
	{
		IsTrue(condition, null);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void IsTrue(bool condition, string message)
	{
		if (!condition)
		{
			Fail(AssertionMessageUtil.BooleanFailureMessage(expected: true), message);
		}
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void IsFalse(bool condition)
	{
		IsFalse(condition, null);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void IsFalse(bool condition, string message)
	{
		if (condition)
		{
			Fail(AssertionMessageUtil.BooleanFailureMessage(expected: false), message);
		}
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void AreApproximatelyEqual(float expected, float actual)
	{
		AreEqual(expected, actual, null, FloatComparer.s_ComparerWithDefaultTolerance);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void AreApproximatelyEqual(float expected, float actual, string message)
	{
		AreEqual(expected, actual, message, FloatComparer.s_ComparerWithDefaultTolerance);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void AreApproximatelyEqual(float expected, float actual, float tolerance)
	{
		AreApproximatelyEqual(expected, actual, tolerance, null);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void AreApproximatelyEqual(float expected, float actual, float tolerance, string message)
	{
		AreEqual(expected, actual, message, new FloatComparer(tolerance));
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void AreNotApproximatelyEqual(float expected, float actual)
	{
		AreNotEqual(expected, actual, null, FloatComparer.s_ComparerWithDefaultTolerance);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void AreNotApproximatelyEqual(float expected, float actual, string message)
	{
		AreNotEqual(expected, actual, message, FloatComparer.s_ComparerWithDefaultTolerance);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void AreNotApproximatelyEqual(float expected, float actual, float tolerance)
	{
		AreNotApproximatelyEqual(expected, actual, tolerance, null);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void AreNotApproximatelyEqual(float expected, float actual, float tolerance, string message)
	{
		AreNotEqual(expected, actual, message, new FloatComparer(tolerance));
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void AreEqual<T>(T expected, T actual)
	{
		AreEqual(expected, actual, null);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void AreEqual<T>(T expected, T actual, string message)
	{
		AreEqual(expected, actual, message, GetEqualityComparer<T>(null));
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void AreEqual<T>(T expected, T actual, string message, IEqualityComparer<T> comparer)
	{
		if (!comparer.Equals(actual, expected))
		{
			Fail(AssertionMessageUtil.GetEqualityMessage(actual, expected, expectEqual: true), message);
		}
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void AreNotEqual<T>(T expected, T actual)
	{
		AreNotEqual(expected, actual, null);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void AreNotEqual<T>(T expected, T actual, string message)
	{
		AreNotEqual(expected, actual, message, GetEqualityComparer<T>(null));
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void AreNotEqual<T>(T expected, T actual, string message, IEqualityComparer<T> comparer)
	{
		if (comparer.Equals(actual, expected))
		{
			Fail(AssertionMessageUtil.GetEqualityMessage(actual, expected, expectEqual: false), message);
		}
	}

	private static IEqualityComparer<T> GetEqualityComparer<T>(params object[] args)
	{
		Type typeFromHandle = typeof(T);
		m_ComparersCache.TryGetValue(typeFromHandle, out var value);
		if (value != null)
		{
			return (IEqualityComparer<T>)value;
		}
		value = EqualityComparer<T>.Default;
		m_ComparersCache.Add(typeFromHandle, value);
		return (IEqualityComparer<T>)value;
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void IsNull<T>(T value) where T : class
	{
		IsNull(value, null);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void IsNull<T>(T value, string message) where T : class
	{
		if (value != null)
		{
			Fail(AssertionMessageUtil.NullFailureMessage(value, expectNull: true), message);
		}
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void IsNotNull<T>(T value) where T : class
	{
		IsNotNull(value, null);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void IsNotNull<T>(T value, string message) where T : class
	{
		if (value == null)
		{
			Fail(AssertionMessageUtil.NullFailureMessage(value, expectNull: false), message);
		}
	}
}
