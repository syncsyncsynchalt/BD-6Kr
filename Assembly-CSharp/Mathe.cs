using GenericOperator;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Mathe
{
	public const long siEXA = 1000000000000000000L;

	public const long siPETA = 1000000000000000L;

	public const long siTERA = 1000000000000L;

	public const long siGIGA = 1000000000L;

	public const long siMEGA = 1000000L;

	public const long siKILO = 1000L;

	public const long siHECTO = 100L;

	public const long siDECA = 10L;

	public const double siDECI = 0.10000000149011612;

	public const double siCENTI = 0.0099999997764825821;

	public const double siMILLI = 0.0010000000474974513;

	public const double siMICRO = 9.9999999747524271E-07;

	public const double siNANO = 9.9999997171806854E-10;

	public const double siPICO = 9.999999960041972E-13;

	public const double siFEMT = 1.0000000036274937E-15;

	public const double siATTO = 1.000000045813705E-18;

	public const double siZEPTO = 9.9999996826552254E-22;

	public const double siYPCTO = 1.0000000195414814E-24;

	public const long bnKIBI = 1024L;

	public const long bnMEBI = 1048576L;

	public const long bnGIBI = 1073741824L;

	public const long bnTEBI = 1099511627776L;

	public const long bnPEBI = 1125899906842624L;

	public const long bnEXBI = 1152921504606846976L;

	public const float GravityAcc = 9.80665f;

	public const long SpeedOfLight = 299792458L;

	public const float SpeedOfSound = 331.5f;

	public const float AbsoluteZero = -273.15f;

	public const float Pi = (float)Math.PI;

	public const float PiMul2 = (float)Math.PI * 2f;

	public const float PiMul3 = (float)Math.PI * 3f;

	public const float PiMul4 = (float)Math.PI * 4f;

	public const float PiMul8 = (float)Math.PI * 8f;

	public const float PiDiv2 = (float)Math.PI / 2f;

	public const float PiDiv3 = (float)Math.PI / 3f;

	public const float PiDiv4 = (float)Math.PI / 4f;

	public const float PiDiv8 = (float)Math.PI / 8f;

	public const float PiDiv180 = (float)Math.PI / 180f;

	public const float PiDiv360 = (float)Math.PI / 360f;

	public const float DivPiMul2 = 113f / 710f;

	public const float DivPi = 1f / (float)Math.PI;

	public const float Mul180DivPi = 57.29578f;

	public static float Infinity => float.PositiveInfinity;

	public static float NegativeInfinity => float.NegativeInfinity;

	public static uint BitTypeSizByt => 4u;

	public static uint BitTypeNumBit => BitTypeSizByt << 3;

	public static float PhyForce(float m, float a)
	{
		return m * a;
	}

	public static float PhyUniMot(float v, float t)
	{
		return v * t;
	}

	public static float PhyUniAccMotV(float vo, float a, float t)
	{
		return vo + a * t;
	}

	public static float PhyUniAccMotS(float vo, float a, float t)
	{
		return vo * t + a * (float)Pow(t, 2.0) / 2f;
	}

	public static float PhyElasticForce(float k, float x)
	{
		return 0f - k * x;
	}

	public static float PhyBuoyancy(float p, float v, float g)
	{
		return p * v * g;
	}

	public static float PhyFrictionForce(float u, float n)
	{
		return u * n;
	}

	public static Vector2 PhyCenterOfGravity(Vector2 pos1, Vector2 pos2, float m1, float m2)
	{
		Vector2 result = default(Vector2);
		result.x = (m1 * pos1.x + m2 * pos2.x) / (m1 + m2);
		result.y = (m1 * pos1.y + m2 * pos2.y) / (m1 + m2);
		return result;
	}

	public static Vector3 PhyCenterOfGravity(Vector3 pos1, Vector3 pos2, float m1, float m2)
	{
		Vector3 result = default(Vector3);
		result.x = (m1 * pos1.x + m2 * pos2.x) / (m1 + m2);
		Vector3 zero = Vector3.zero;
		result.y = zero.y;
		result.z = (m1 * pos1.z + m2 * pos2.z) / (m1 + m2);
		return result;
	}

	public static float SpeedofSound(float celsius)
	{
		return 331.5f + 0.61f * celsius;
	}

	public static float Abs(float f)
	{
		return (!(f < 0f)) ? f : (0f - f);
	}

	public static float Epsilon()
	{
		return float.Epsilon;
	}

	public static T Add<T>(T a, T b)
	{
		return Operator<T>.Add(a, b);
	}

	public static Vector3 Add(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static T Sub<T>(T a, T b)
	{
		return Operator<T>.Subtract(a, b);
	}

	public static Vector3 Sub(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static T Mul<T>(T a, T b)
	{
		return Operator<T>.Multiply(a, b);
	}

	public static T Div<T>(T a, T b)
	{
		return Operator<T>.Divide(a, b);
	}

	public static T Pul<T>(T a)
	{
		return Operator<T>.Plus(a);
	}

	public static int Sum(List<int> a)
	{
		int num = 0;
		foreach (int item in a)
		{
			num += item;
		}
		return num;
	}

	public static int Sum(params int[] a)
	{
		int num = 0;
		foreach (int num2 in a)
		{
			num += num2;
		}
		return num;
	}

	public static int Min2(int a, int b)
	{
		return (a >= b) ? b : a;
	}

	public static float Min2(float a, float b)
	{
		return (!(a < b)) ? b : a;
	}

	public static int Max2(int a, int b)
	{
		return (a <= b) ? b : a;
	}

	public static float Max2(float a, float b)
	{
		return (!(a > b)) ? b : a;
	}

	public static int MinMax2(int a, int mn, int mx)
	{
		return Min2(Max2(a, mn), mx);
	}

	public static float MinMax2(float a, float mn, float mx)
	{
		return Min2(Max2(a, mn), mx);
	}

	public static int MinMax2Rev(int a, int mn, int mx)
	{
		if (a < mn)
		{
			return mx;
		}
		if (a > mx)
		{
			return mn;
		}
		return a;
	}

	public static float MinMax2Rev(float a, float mn, float mx)
	{
		if (a < mn)
		{
			return mx;
		}
		if (a > mx)
		{
			return mn;
		}
		return a;
	}

	public static float MinMax2F01(float a)
	{
		return MinMax2(a, 0f, 1f);
	}

	public static float MinMax2F11(float a)
	{
		return MinMax2(a, -1f, 1f);
	}

	public static float Min3(int a, int b, int c)
	{
		return Min2(Min2(a, b), c);
	}

	public static float Min3(float a, float b, float c)
	{
		return Min2(Min2(a, b), c);
	}

	public static int Max3(int a, int b, int c)
	{
		return Max2(Max2(a, b), c);
	}

	public static float Max3(float a, float b, float c)
	{
		return Max2(Max2(a, b), c);
	}

	public static int ClipMin2(int a, int mn, int mx)
	{
		return (a >= mn) ? a : mx;
	}

	public static float ClipMin2(float a, float mn, float mx)
	{
		return (!(a < mn)) ? a : mx;
	}

	public static int ClipMax2(int a, int mn, int mx)
	{
		return (a <= mx) ? a : mn;
	}

	public static float ClipMax2(float a, float mn, float mx)
	{
		return (!(a > mx)) ? a : mn;
	}

	public static int Clip(int a, int mn, int mx)
	{
		int num = (a <= mx) ? a : mn;
		return (a >= mn) ? num : mx;
	}

	public static float Clip(float a, float mn, float mx)
	{
		float num = (!(a > mx)) ? a : mn;
		return (!(a < mn)) ? num : mx;
	}

	public static Vector3 Vec2ToVec3XY(Vector2 vec)
	{
		return new Vector3(vec.x, vec.y, 0f);
	}

	public static Vector3 Vec2ToVec3XZ(Vector2 vec)
	{
		return new Vector3(vec.x, 0f, vec.y);
	}

	public static Vector4 QuaToVec4(Quaternion rot)
	{
		return new Vector4(rot.x, rot.y, rot.z, rot.w);
	}

	public static Quaternion Vec4ToQua(Vector4 rot)
	{
		return new Quaternion(rot.x, rot.y, rot.z, rot.w);
	}

	public static float Euler2Deg(float eulerAngle)
	{
		float num = eulerAngle;
		if (Math.Sign(num) == -1)
		{
			num = ((num != -0f) ? (num + 360f) : 0f);
		}
		return num;
	}

	public static float Deg2Rad(float fDeg)
	{
		return fDeg * ((float)Math.PI / 180f);
	}

	public static float Rad2Deg(float fRad)
	{
		return fRad * 57.29578f;
	}

	public static float PiLimF01(float a)
	{
		return MinMax2(a, 0f, (float)Math.PI);
	}

	public static float PiLimF02(float a)
	{
		return MinMax2(a, 0f, (float)Math.PI * 2f);
	}

	public static float PiLimF11(float a)
	{
		return MinMax2(a, -(float)Math.PI, (float)Math.PI);
	}

	public static float PiClip(float r)
	{
		float num = (!(r < 0f)) ? ((float)Math.PI) : (-(float)Math.PI);
		int num2 = (int)((r + num) / ((float)Math.PI * 2f));
		return r - (float)num2 * ((float)Math.PI * 2f);
	}

	public static float PiLengthMin(float r0, float r1)
	{
		float r2 = r1 - r0;
		r2 = PiClip(r2);
		float num = Abs(r2);
		if (num >= (float)Math.PI)
		{
			num = (float)Math.PI * 2f - num;
		}
		return num;
	}

	public static float PiLengthMax(float r0, float r1)
	{
		return (float)Math.PI * 2f - PiLengthMin(r0, r1);
	}

	public static float PiLerp(float r0, float r1, float t)
	{
		float r2 = r1 - r0;
		float num = PiClip(r2);
		num += t;
		float r3 = r0 + num;
		return PiClip(r3);
	}

	public static float PiLimRng(float rsrc, float raim, float frng)
	{
		if (PiLengthMin(rsrc, raim) <= frng)
		{
			return raim;
		}
		float num = PiClip(rsrc - frng);
		float num2 = PiClip(rsrc + frng);
		if (PiLengthMin(num, num2) < PiLengthMin(num2, raim))
		{
			return num;
		}
		return num2;
	}

	public static float Rate(float mn, float mx, float nw)
	{
		return (nw - mn) / (mx - mn);
	}

	public static float Lerp(float a0, float a1, float t)
	{
		return a1 * t + a0 * (1f - t);
	}

	public static float Remap(float value, float inputMin, float inputMax, float outputMin, float outputMax)
	{
		return (value - inputMin) * ((outputMax - outputMin) / (inputMax - inputMin)) + outputMin;
	}

	public static uint BitAryNumCalc(int iArg, int nBit)
	{
		return (uint)((iArg + (nBit - 1)) / nBit);
	}

	public static uint BitAryNum(int iArg)
	{
		return BitAryNumCalc(iArg, (int)BitTypeNumBit);
	}

	public static byte Byte2KByteI(int by)
	{
		int num = by >> 10;
		return (byte)num;
	}

	public static byte Byte2MByteI(int by)
	{
		int num = Byte2KByteI(by);
		return (byte)num;
	}

	public static byte Byte2GByteI(int by)
	{
		int num = Byte2MByteI(by);
		return (byte)num;
	}

	public static byte KByte2ByteI(int kbyte)
	{
		int num = kbyte << 10;
		return (byte)num;
	}

	public static byte MByte2ByteI(int Mbyte)
	{
		int num = KByte2ByteI(Mbyte) << 10;
		return (byte)num;
	}

	public static byte GByte2ByteI(int Gbyte)
	{
		int num = MByte2ByteI(Gbyte) << 10;
		return (byte)num;
	}

	public static float KByte2ByteF(float Kbyte)
	{
		return Kbyte * 1024f;
	}

	public static float MByte2ByteF(float Mbyte)
	{
		return KByte2ByteF(Mbyte * 1024f);
	}

	public static float GByte2ByteF(float Gbyte)
	{
		return MByte2ByteF(Gbyte * 1024f);
	}

	public static double Pow(double f1, double f2)
	{
		return Math.Pow(f1, f2);
	}

	public static double Pow2(double f1)
	{
		return f1 * f1;
	}

	public static double Pow3(double f1)
	{
		return f1 * f1 * f1;
	}

	public static double Pow4(double f1)
	{
		return f1 * f1 * f1 * f1;
	}

	public static uint Factorial(uint n)
	{
		switch (n)
		{
		default:
			return n * Factorial(n - 1);
		case 1u:
			return 1u;
		case 0u:
			return 0u;
		}
	}

	public static float Frame2Sec(int frame, float fps = 60f)
	{
		return 1f / fps * (float)frame;
	}

	public static Vector3 Direction(Vector3 from, Vector3 to)
	{
		return to - from;
	}

	public static Vector2 Direction(Vector2 from, Vector2 to)
	{
		return to - from;
	}

	public static Vector3 NormalizeDirection(Vector3 from, Vector3 to)
	{
		return Direction(from, to).normalized;
	}

	public static Vector2 NormalizeDirection(Vector2 from, Vector2 to)
	{
		return Direction(from, to).normalized;
	}

	public static float Hermite(float start, float end, float t)
	{
		return Mathf.Lerp(start, end, t * t * (3f - 2f * t));
	}

	public static float Sinerp(float start, float end, float t)
	{
		return Mathf.Lerp(start, end, Mathf.Sin(t * (float)Math.PI * 0.5f));
	}

	public static float Coserp(float start, float end, float t)
	{
		return Mathf.Lerp(start, end, 1f - Mathf.Cos(t * (float)Math.PI * 0.5f));
	}

	public static float Berp(float start, float end, float t)
	{
		t = Mathf.Clamp01(t);
		t = Mathf.Sin(t * (float)Math.PI + (0.2f + 2.5f * t * t * t)) * Mathf.Pow(1f - t, 2.2f) + t;
		return start + (end - start) * t;
	}

	public static float SmoothStep(float x, float min, float max)
	{
		x = Mathf.Clamp(x, min, max);
		float num = (x - min) / (max - min);
		float num2 = (x - min) / (max - min);
		return -2f * num * num * num + 3f * num2 * num2;
	}

	public static float Lerp_(float start, float end, float value)
	{
		return (1f - value) * start + value * end;
	}

	public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		Vector3 vector = Vector3.Normalize(lineEnd - lineStart);
		float d = Vector3.Dot(point - lineStart, vector) / Vector3.Dot(vector, vector);
		return lineStart + d * vector;
	}

	public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		Vector3 vector = lineEnd - lineStart;
		Vector3 vector2 = Vector3.Normalize(vector);
		float value = Vector3.Dot(point - lineStart, vector2) / Vector3.Dot(vector2, vector2);
		return lineStart + Mathf.Clamp(value, 0f, Vector3.Magnitude(vector)) * vector2;
	}

	public static float Bounce(float x)
	{
		return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f)) * (1f - x));
	}

	public static bool Approx(float val, float about, float range)
	{
		return Mathf.Abs(val - about) < range;
	}

	public static bool Approx(Vector3 val, Vector3 about, float range)
	{
		return (val - about).sqrMagnitude < range * range;
	}

	public static float Clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		if (end - start < 0f - num3)
		{
			float num4 = (num2 - start + end) * value;
			return start + num4;
		}
		if (end - start > num3)
		{
			float num4 = (0f - (num2 - end + start)) * value;
			return start + num4;
		}
		return start + (end - start) * value;
	}

	public static int NextElement(int now, int min, int max, bool isFoward)
	{
		int num = isFoward ? 1 : (-1);
		now += num;
		now = MinMax2(now, min, max);
		return now;
	}

	public static void NextElement(ref int now, int min, int max, bool isFoward)
	{
		int num = isFoward ? 1 : (-1);
		now += num;
		now = MinMax2(now, min, max);
	}

	public static int NextElement(int now, int min, int max, bool isFoward, Predicate<int> notConditions)
	{
		int num = isFoward ? 1 : (-1);
		int a = now + num;
		a = MinMax2(a, min, max);
		while (!notConditions(a))
		{
			if (a == min || a == max)
			{
				a = (notConditions(a) ? a : now);
				break;
			}
			a += num;
			a = MinMax2(a, min, max);
		}
		return a;
	}

	public static void NextElement(ref int now, int min, int max, bool isFoward, Predicate<int> notConditions)
	{
		int num = isFoward ? 1 : (-1);
		int num2 = now;
		num2 += num;
		num2 = MinMax2(num2, min, max);
		while (!notConditions(num2))
		{
			if (num2 == min || num2 == max)
			{
				num2 = (notConditions(num2) ? num2 : now);
				break;
			}
			num2 += num;
			num2 = MinMax2(num2, min, max);
		}
		now = num2;
	}

	public static int NextElementRev(int now, int min, int max, bool isFoward)
	{
		int num = isFoward ? 1 : (-1);
		now += num;
		now = MinMax2Rev(now, min, max);
		return now;
	}

	public static void NextElementRev(ref int now, int min, int max, bool isFoward)
	{
		int num = isFoward ? 1 : (-1);
		now += num;
		now = MinMax2Rev(now, min, max);
	}

	public static int NextElementRev(int now, int min, int max, bool isFoward, Predicate<int> notConditions)
	{
		int num = isFoward ? 1 : (-1);
		int a = now + num;
		a = MinMax2Rev(a, min, max);
		while (!notConditions(a))
		{
			a += num;
			a = MinMax2Rev(a, min, max);
		}
		return a;
	}

	public static void NextElementRev(ref int now, int min, int max, bool isFoward, Predicate<int> notConditions)
	{
		int num = isFoward ? 1 : (-1);
		int num2 = now;
		num2 += num;
		num2 = MinMax2Rev(num2, min, max);
		new List<bool>(max);
		while (!notConditions(num2))
		{
			num2 += num;
			num2 = MinMax2Rev(num2, min, max);
		}
		now = num2;
	}

	public static Vector2[] RegularPolygonVertices(int verNum, float radius, float angle)
	{
		Vector2[] array = new Vector2[verNum];
		float num = (float)Math.PI * 2f / (float)verNum;
		for (int i = 0; i < verNum; i++)
		{
			float x = Mathf.Cos(num * (float)i + Deg2Rad(angle)) * radius;
			float y = Mathf.Sin(num * (float)i + Deg2Rad(angle)) * radius;
			array[i] = new Vector2(x, y);
		}
		return array;
	}

	public static int Clip(ref int value, int min, int max)
	{
		if (value < min)
		{
			value = min;
		}
		else if (value > max)
		{
			value = max;
		}
		return max;
	}

	public static float Clip(ref float value, float min, float max)
	{
		if (value < min)
		{
			value = min;
		}
		else if (value > max)
		{
			value = max;
		}
		return max;
	}

	public static DateTime Clip(ref DateTime value, DateTime min, DateTime max)
	{
		if (value < min)
		{
			value = min;
		}
		else if (value > max)
		{
			value = max;
		}
		return max;
	}

	public static int ValueRewind(int current, int max, ref int startCnt)
	{
		if (current < max)
		{
			return ++current;
		}
		return ++startCnt;
	}

	public static int CrossValue(int nMax, int nValue)
	{
		return nMax - 1 - nValue;
	}

	public static float Bar2Playtime(int bpm, int measure, int bar)
	{
		return 60 * measure * bar / bpm;
	}

	public static int Bar2Frame(int bpm, int measure, int bar)
	{
		return 60 * measure * bar / bpm * 60;
	}
}
