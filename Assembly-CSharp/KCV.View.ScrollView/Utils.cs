using System;
using UnityEngine;

namespace KCV.View.ScrollView
{
	internal class Utils
	{
		public static void RangeTest(int start, int counter, int from, int to)
		{
			counter = Math.Abs(counter);
			for (int i = start; i < counter; i++)
			{
				Debug.Log("From:" + from + " To:" + to + " Value:" + i + " Range" + ((!RangeEqualsIn(i, from, to)) ? "Over" : "In"));
			}
		}

		public static bool RangeEqualsIn(float currentPosition, float from, float to)
		{
			float num;
			float num2;
			if (from < to)
			{
				num = from;
				num2 = to;
			}
			else
			{
				num = to;
				num2 = from;
			}
			num = (int)num;
			num2 = (int)num2;
			if (num <= currentPosition && currentPosition <= num2)
			{
				return true;
			}
			return false;
		}

		public static int LoopValue(int value, int min, int max)
		{
			max--;
			if (value < min)
			{
				value = max - (min - value) + 1;
			}
			if (value > max)
			{
				value = min + (value - max) - 1;
			}
			return value;
		}
	}
}
