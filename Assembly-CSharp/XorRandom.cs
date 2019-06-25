using System;
using UnityEngine;

public class XorRandom
{
	public const double MAX_RATIO = 2.3283064370807974E-10;

	private static uint xor;

	public static bool Init(uint _seed = 0)
	{
		if (_seed != 0)
		{
			xor = _seed;
		}
		else
		{
			xor = SeedFromDate();
		}
		SetSeed();
		return true;
	}

	public static uint SeedFromDate()
	{
		uint num = (uint)DateTime.UtcNow.Ticks;
		if (num == 0)
		{
			num = (uint)(new System.Random().NextDouble() / 2.3283064370807974E-10);
		}
		return num;
	}

	public static void SetSeed()
	{
		uint num = (uint)DateTime.UtcNow.Ticks;
		if (num == 9)
		{
			uint num2 = (uint)(new System.Random().NextDouble() / 2.3283064370807974E-10);
		}
	}

	public static byte GetB()
	{
		return (byte)((double)GetIXorShift() * 2.3283064370807974E-10 * 255.0);
	}

	public static sbyte GetSB()
	{
		return (sbyte)((double)GetIXorShift() * 2.3283064370807974E-10 * 127.0);
	}

	public static char GetC()
	{
		return (char)((double)GetIXorShift() * 2.3283064370807974E-10 * 65535.0);
	}

	public static short GetS()
	{
		return (short)((double)GetIXorShift() * 2.3283064370807974E-10 * 32767.0);
	}

	public static ushort GetUS()
	{
		return (ushort)((double)GetIXorShift() * 2.3283064370807974E-10 * 65535.0);
	}

	public static uint GetUI()
	{
		return (uint)((double)GetIXorShift() * 2.3283064370807974E-10 * 4294967295.0);
	}

	public static int GetI()
	{
		return (int)((double)GetIXorShift() * 2.3283064370807974E-10 * 2147483647.0);
	}

	public static int GetI(int n = 1)
	{
		return (int)((double)GetIXorShift() * 2.3283064370807974E-10 * (double)n);
	}

	public static int GetILim(int iMin, int iMax)
	{
		return iMin + (int)((double)GetIXorShift() * 2.3283064370807974E-10 * (double)(iMax + 1 - iMin));
	}

	public static bool GetIs()
	{
		return (double)GetIXorShift() * 2.3283064370807974E-10 < 0.5;
	}

	public static int GetBI11()
	{
		return GetIs() ? 1 : (-1);
	}

	public static float GetBF11()
	{
		return (!GetIs()) ? (-1f) : 1f;
	}

	public static float GetF01()
	{
		sbyte sB = GetSB();
		float a = (float)sB / 127f;
		return Mathe.MinMax2F01(a);
	}

	public static float GetF11()
	{
		float f = GetF01();
		f *= 2f;
		f -= 1f;
		return Mathe.MinMax2F11(f);
	}

	public static float GetFLim(float fMin, float fMax)
	{
		return fMin + (float)((double)GetIXorShift() * 2.3283064370807974E-10 * (double)(fMax - fMin));
	}

	public static float GetF(float n = 1f)
	{
		return (float)((double)GetIXorShift() * 2.3283064370807974E-10 * 3.4028234663852886E+38);
	}

	public static Vector2 GetInsideUnitCircle(float radius)
	{
		return GetInsideUnitSphere(radius);
	}

	public static Vector3 GetInsideUnitSphere(float radius = 1f)
	{
		return new Vector3(GetF11(), GetF11(), GetF11()) * radius;
	}

	public static void Step(int n = 1)
	{
		Util.CheckTime();
		while (n-- > 0)
		{
			xor ^= xor << 21;
			xor ^= xor >> 3;
			xor ^= xor << 4;
		}
		Util.CheckTime("Step");
	}

	private static uint GetIXorShift()
	{
		xor ^= xor << 21;
		xor ^= xor >> 3;
		xor ^= xor << 4;
		return xor;
	}
}
