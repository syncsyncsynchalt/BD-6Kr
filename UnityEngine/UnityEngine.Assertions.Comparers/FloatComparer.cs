using System;
using System.Collections.Generic;

namespace UnityEngine.Assertions.Comparers;

internal class FloatComparer : IEqualityComparer<float>
{
	internal const float k_Epsilon = 1E-05f;

	private readonly float m_Error;

	private readonly bool m_Relative;

	public static readonly FloatComparer s_ComparerWithDefaultTolerance = new FloatComparer(1E-05f);

	public FloatComparer()
		: this(1E-05f, relative: false)
	{
	}

	public FloatComparer(bool relative)
		: this(1E-05f, relative)
	{
	}

	public FloatComparer(float error)
		: this(error, relative: false)
	{
	}

	public FloatComparer(float error, bool relative)
	{
		m_Error = error;
		m_Relative = relative;
	}

	public bool Equals(float a, float b)
	{
		return (!m_Relative) ? AreEqual(a, b, m_Error) : AreEqualRelative(a, b, m_Error);
	}

	public int GetHashCode(float obj)
	{
		return base.GetHashCode();
	}

	public static bool AreEqual(float expected, float actual, float error)
	{
		return Math.Abs(actual - expected) <= error;
	}

	public static bool AreEqualRelative(float expected, float actual, float error)
	{
		if (expected == actual)
		{
			return true;
		}
		float num = Math.Abs(expected);
		float num2 = Math.Abs(actual);
		float num3 = Math.Abs((actual - expected) / ((!(num > num2)) ? num2 : num));
		return num3 <= error;
	}
}
