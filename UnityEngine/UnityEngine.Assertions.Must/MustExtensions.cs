#define UNITY_ASSERTIONS
using System.Diagnostics;

namespace UnityEngine.Assertions.Must;

[DebuggerStepThrough]
public static class MustExtensions
{
	[Conditional("UNITY_ASSERTIONS")]
	public static void MustBeTrue(this bool value)
	{
		Assert.IsTrue(value);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustBeTrue(this bool value, string message)
	{
		Assert.IsTrue(value, message);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustBeFalse(this bool value)
	{
		Assert.IsFalse(value);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustBeFalse(this bool value, string message)
	{
		Assert.IsFalse(value, message);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustBeApproximatelyEqual(this float actual, float expected)
	{
		Assert.AreApproximatelyEqual(actual, expected);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustBeApproximatelyEqual(this float actual, float expected, string message)
	{
		Assert.AreApproximatelyEqual(actual, expected, message);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustBeApproximatelyEqual(this float actual, float expected, float tolerance)
	{
		Assert.AreApproximatelyEqual(actual, expected, tolerance);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustBeApproximatelyEqual(this float actual, float expected, float tolerance, string message)
	{
		Assert.AreApproximatelyEqual(expected, actual, tolerance, message);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustNotBeApproximatelyEqual(this float actual, float expected)
	{
		Assert.AreNotApproximatelyEqual(expected, actual);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustNotBeApproximatelyEqual(this float actual, float expected, string message)
	{
		Assert.AreNotApproximatelyEqual(expected, actual, message);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustNotBeApproximatelyEqual(this float actual, float expected, float tolerance)
	{
		Assert.AreNotApproximatelyEqual(expected, actual, tolerance);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustNotBeApproximatelyEqual(this float actual, float expected, float tolerance, string message)
	{
		Assert.AreNotApproximatelyEqual(expected, actual, tolerance, message);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustBeEqual<T>(this T actual, T expected)
	{
		Assert.AreEqual(actual, expected);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustBeEqual<T>(this T actual, T expected, string message)
	{
		Assert.AreEqual(expected, actual, message);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustNotBeEqual<T>(this T actual, T expected)
	{
		Assert.AreNotEqual(actual, expected);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustNotBeEqual<T>(this T actual, T expected, string message)
	{
		Assert.AreNotEqual(expected, actual, message);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustBeNull<T>(this T expected) where T : class
	{
		Assert.IsNull(expected);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustBeNull<T>(this T expected, string message) where T : class
	{
		Assert.IsNull(expected, message);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustNotBeNull<T>(this T expected) where T : class
	{
		Assert.IsNotNull(expected);
	}

	[Conditional("UNITY_ASSERTIONS")]
	public static void MustNotBeNull<T>(this T expected, string message) where T : class
	{
		Assert.IsNotNull(expected, message);
	}
}
